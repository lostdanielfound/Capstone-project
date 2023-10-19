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
    Public Enum JsonFileType
        Plans
        Log
    End Enum

    Private _JsonFilePath As String = String.Empty
    Private _JsonType As JsonFileType

    Public Sub SetJsonFile(sampleJsonFilePath As String, Optional type As JsonFileType = JsonFileType.Plans)
        _JsonFilePath = sampleJsonFilePath
        _JsonType = type
        If Not (File.Exists(_JsonFilePath)) Then
            CreateJsonFile()
        End If
    End Sub


    ' =============================
    ' Methods Involving .BoxITLog.json
    ' =============================

    Public Function DepthFirstTransversal(Path As String, UpdateTime As String) As JArray
        Dim JsonTree = New JArray() 'Start of the JsonTree

        For Each subDirectoryPath As String In Directory.GetDirectories(Path)
            'Things before transversing in other subdirectories
            Dim DirectoryObject As New DirectoryInfo(subDirectoryPath)
            Dim DirectoryName = DirectoryObject.Name
            Dim JsonDirectoryObject = New TreeObject() With {
                .Name = DirectoryName,
                .Files = New List(Of TreeObject),
                .Type = "Directory",
                .UpdateTime = UpdateTime
            }

            'Continue making the subdirectory portion of the JsonTree
            JsonDirectoryObject.Files = DepthFirstTransversalVisit(subDirectoryPath, UpdateTime) 'Add the Files to the Directory object
            JsonTree.Add(JObject.Parse(JsonConvert.SerializeObject(JsonDirectoryObject))) 'Add the new Json serialized object to the JsonTree
        Next

        For Each filePath As String In Directory.GetFiles(Path)
            Dim FileObject As New FileInfo(filePath)
            Dim FileName = FileObject.Name
            Dim JsonFileObject = New TreeObject() With {
                .Name = FileName,
                .Files = New List(Of TreeObject),
                .Type = "File",
                .UpdateTime = UpdateTime
            }

            JsonTree.Add(JObject.Parse(JsonConvert.SerializeObject(JsonFileObject)))
        Next

        Return JsonTree
    End Function

    Public Function DepthFirstTransversalVisit(Path As String, UpdateTime As String) As List(Of TreeObject)
        Dim JsonList = New List(Of TreeObject)

        'Continue to transferse through all the subdirectories until leaf is reached
        For Each subDirectoryPath As String In Directory.GetDirectories(Path)
            Dim DirectoryObject As New DirectoryInfo(subDirectoryPath)
            Dim DirectoryName = DirectoryObject.Name
            Dim JsonDirectoryObject = New TreeObject() With {
                .Name = DirectoryName,
                .Files = New List(Of TreeObject),
                .Type = "Directory",
                .UpdateTime = UpdateTime
            }

            JsonDirectoryObject.Files = DepthFirstTransversalVisit(subDirectoryPath, UpdateTime)
            JsonList.Add(JsonDirectoryObject)
        Next

        'Now we can finally get the list of files
        For Each filePath As String In Directory.GetFiles(Path)
            Dim FileObject As New FileInfo(filePath)
            Dim FileName = FileObject.Name
            Dim JsonFileObject = New TreeObject() With {
                .Name = FileName,
                .Files = New List(Of TreeObject),
                .Type = "File",
                .UpdateTime = UpdateTime
            }

            JsonList.Add(JsonFileObject)
        Next

        Return JsonList 'Finally return the JsonList of files
    End Function

    ' Uses a form of DFS to create a JSON list that represents the File structure 
    ' of the Src directory 
    Public Sub JsonTreeCreation(UpdateDate As String)
        ' Perform a DFS on all directories within the root
        ' Once at a "leaf", collect all files within that leaf directory and backtrack
        Dim JsonTree = DepthFirstTransversal(Directory.GetParent(_JsonFilePath).FullName, UpdateDate)

        Using sw As StreamWriter = File.CreateText(_JsonFilePath)
            Using writer As New JsonTextWriter(sw)
                JsonTree.WriteTo(writer)
            End Using
        End Using

    End Sub

    ' =============================
    ' Methods Involving Plans.json
    ' =============================
    Public Function ModifyPlanObject_Nxt_backup(PlanName As String, ScheduleDate As String) As Boolean
        If _JsonType = JsonFileType.Log Then
            Throw New JsonException()
        End If

        Dim sourceJsonArray As JArray

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream = reader.ReadToEnd() 'Read json file into stream
            sourceJsonArray = JArray.Parse(JsonStream) 'Parse Array json object
        End Using

        Dim Flag = False
        'Updates the Nxt_backup time for PlanName in Jarray
        For Each plan In sourceJsonArray
            If plan("Name") = PlanName Then
                plan("Nxt_backup") = ScheduleDate
                Flag = True
                Exit For
            End If
        Next

        'Writes content of Jarray back into the json file
        Using sw As StreamWriter = File.CreateText(_JsonFilePath)
            Using writer As New JsonTextWriter(sw)
                sourceJsonArray.WriteTo(writer)
            End Using
        End Using

        Return Flag
    End Function

    Public Function ReadPlanObject_Previous_backup(PlanName As String) As String
        If _JsonType = JsonFileType.Log Then
            Throw New JsonException()
        End If

        Dim sourceJsonArray As JArray

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream = reader.ReadToEnd() 'Read json file into stream
            sourceJsonArray = JArray.Parse(JsonStream) 'Parse Array json object
        End Using

        'Transverse until we find the jtoken with the Plan name
        For Each Plan In sourceJsonArray
            If Plan("Name") = PlanName Then
                Return Plan("Previous_backup")
            End If
        Next

        Return String.Empty
    End Function

    ' ReadPlanObjectsFromJsonFile() 
    ' @desc Reads from the Plans.json and returns back a list of current
    ' active backup plans 
    ' @returns List(Of Plan), List of Backup Plans that are currently
    ' active and stored with Plans.json
    Public Function ReadPlanObjectsFromJsonFile() As List(Of Plan)
        If _JsonType = JsonFileType.Log Then
            Throw New JsonException()
        End If

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
    Public Function WritePlanObjectToJsonFile(Name As String, Nxt_backup As String, Previous_backup As String, Src As String, Dst As String, BackupPlan As BackupPlan) As Boolean
        If _JsonType = JsonFileType.Log Then
            Throw New JsonException()
        End If

        Dim CurrentBackupPlans = ReadPlanObjectsFromJsonFile()
        For Each plan In CurrentBackupPlans 'Can't be another duplicated backup plan name
            If plan.Name = Name Then
                Return False
            End If
        Next

        Dim backup_plan = New Plan() With { 'Create new Plan object
            .Name = Name,
            .Nxt_backup = Nxt_backup,
            .Previous_backup = Previous_backup,
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
        If _JsonType = JsonFileType.Log Then
            Throw New JsonException()
        End If

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


