Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

' Class JsonManip 
' @desc The design is to have a created object modify the JSON within the Plans.json file or the hidden.
' .BoxITLog.json file for every source directory of a incremental / differential backup plan.
' The following is the overall design structure of the json array:
'
'[
'   {},
'   {},
'   {},
'   {},
']
'
' @members:
'   SetJsonFile()
'   ReadPlanObjectsFromJsonFile()
'   WritePlanObjectToJsonFile()
'   
'   GetTimeStamp 
'   _JsonFileType *Private
'   _JsonFilePath *Private


Public Class JsonManip
    Enum JsonFileType
        Plans
        Log
    End Enum

    Private _JsonFilePath As String = String.Empty

    Public Sub SetJsonFile(sampleJsonFilePath As String)
        _JsonFilePath = sampleJsonFilePath
        If Not (File.Exists(_JsonFilePath)) Then
            CreateJsonFile()
        End If
    End Sub

    Public Function ModifyPlanObject_Nxt_backup(PlanName As String, ScheduleDate As String) As Boolean
        Dim sourceJsonArray As JArray

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream = reader.ReadToEnd() 'Read json file into stream
            sourceJsonArray = JArray.Parse(JsonStream) 'Parse Array json object
        End Using

        For Each plan In sourceJsonArray
            If plan("Name") = PlanName Then
                plan("Nxt_backup") = ScheduleDate
            End If
        Next

        'Writes content of Jarray back into the json file
        Using sw As StreamWriter = File.CreateText(_JsonFilePath)
            Using writer As New JsonTextWriter(sw)
                sourceJsonArray.WriteTo(writer)
            End Using
        End Using
    End Function

    ' ReadPlanObjectsFromJsonFile() 
    ' @desc Reads from the Plans.json and returns back a list of current
    ' active backup plans 
    ' @returns List(Of Plan), List of Backup Plans that are currently
    ' active and stored with Plans.json
    Public Function ReadPlanObjectsFromJsonFile() As List(Of Plan)
        Dim Plans As New List(Of Plan)() 'Creates an empty Plan List

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream As String = reader.ReadToEnd() 'Reads in the JSON stream
            Plans = JsonConvert.DeserializeObject(Of List(Of Plan))(JsonStream) 'Dumps all the JSON objects within the array into Plans
        End Using

        Return Plans 'Recall this is a List of Plan Objects
    End Function

    ' WritePlanObjectToJsonFile()
    ' @desc Writes a new Plan Json object into the Plans.json file.
    ' @parameters Name As String, Nxt_backup As String, Src As String, Dst As String, BackupPlan As BackupPlan
    Public Function WritePlanObjectToJsonFile(Name As String, Nxt_backup As String, Src As String, Dst As String, BackupPlan As BackupPlan) As Boolean
        Dim CurrentBackupPlans = ReadPlanObjectsFromJsonFile()
        For Each plan In CurrentBackupPlans 'Can't be another duplicated backup plan name
            If plan.Name = Name Then
                Return False
            End If
        Next

        Dim backup_plan = New Plan() With { 'Create new Plan object
            .Name = Name,
            .Nxt_backup = Nxt_backup,
            .Src = Src,
            .Dst = Dst,
            .backupPlan = BackupPlan
        }

        Dim json = JsonConvert.SerializeObject(backup_plan) 'Serialize object to add to json database 
        Dim sourceJsonArray As JArray

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream = reader.ReadToEnd() 'Read json file into stream
            sourceJsonArray = JArray.Parse(JsonStream) 'Parse Array json object
            Dim planObject = JObject.Parse(json) 'Create jObject to add to JArray 
            sourceJsonArray.Add(planObject) 'Add the object into the JArray
        End Using

        'Writes content of Jarray back into the json file
        Using sw As StreamWriter = File.CreateText(_JsonFilePath)
            Using writer As New JsonTextWriter(sw)
                sourceJsonArray.WriteTo(writer)
            End Using
        End Using

        Return True
    End Function

    ' DeletePlanObject()
    ' @desc Selects the object stored within the json array at in index and removes it from the array 
    ' @parameters arrayIndex as Integer
    Public Sub DeletePlanObject(arrayIndex As Integer)
        Dim sourceJsonArray As JArray

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream = reader.ReadToEnd() 'Read json file into stream
            sourceJsonArray = JArray.Parse(JsonStream) 'Parse Array json object
        End Using

        sourceJsonArray(arrayIndex).Remove() 'Removed the PlanObject

        'Writes content of Jarray back into the json file
        Using sw As StreamWriter = File.CreateText(_JsonFilePath)
            Using writer As New JsonTextWriter(sw)
                sourceJsonArray.WriteTo(writer)
            End Using
        End Using
    End Sub

    Public Sub CreateJsonFile()
        Dim CleanJson As String = "[ 

        ]"

        Using writer As New StreamWriter(_JsonFilePath)
            writer.Write(CleanJson)
        End Using
    End Sub

End Class


