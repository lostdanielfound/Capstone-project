Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

' Class JsonManip 
' @desc The design is to have a created object modify the JSON within the Plans.json file.
' This file is ment to be used as a way to keep track of the current active scheduled backup plans
' that have been created. The following is the overall design structure of the json array:
'
'[
'   {},
'   {},
'   {},
'   {},
']
'
' Each object contains a Plan object (Please refer to the Plan class for more details). 
' @members:
'   ReadAndParseJsonFileWithNewTonsoftJson(), This needs to be changed to a much shorter name like SetJsonFile() 
'   ReadFromJsonFile()
'   WriteToJsonFile()
'   _JsonFilePath *Private


Public Class JsonManip
    Private _JsonFilePath As String = String.Empty

    Public Sub SetJsonFile(sampleJsonFilePath As String)
        _JsonFilePath = sampleJsonFilePath
        If Not (File.Exists(_JsonFilePath)) Then
            CreateJsonFile()
        End If
    End Sub

    ' ReadFromJsonFile() 
    ' @desc Reads from the Plans.json and returns back a list of current
    ' active backup plans 
    ' @returns List(Of Plan), List of Backup Plans that are currently
    ' active and stored with Plans.json
    Public Function ReadFromJsonFile() As List(Of Plan)
        Dim Plans As New List(Of Plan)() 'Creates an empty Plan List

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream As String = reader.ReadToEnd() 'Reads in the JSON stream
            Plans = JsonConvert.DeserializeObject(Of List(Of Plan))(JsonStream) 'Dumps all the JSON objects within the array into Plans
        End Using

        Return Plans 'Recall this is a List of Plan Objects
    End Function

    ' WriteToJsonFile()
    ' @desc Writes a new Plan Json object into the Plans.json file.
    ' @parameters Name As String, Nxt_backup As String, Src As String, Dst As String, BackupPlan As BackupPlan
    ' 
    ' TODO: Ensure that the user can't add a Plan of the same name 
    Public Function WriteToJsonFile(Name As String, Nxt_backup As String, Src As String, Dst As String, BackupPlan As BackupPlan) As Boolean
        Dim CurrentBackupPlans = ReadFromJsonFile()
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


