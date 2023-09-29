Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

'[
'   {},
'   {},
'   {},
'   {},
']


Public Class JsonManip
    Private _JsonFilePath As String = String.Empty

    Public Sub ReadAndParseJsonFileWithNewTonsoftJson(sampleJsonFilePath As String)
        _JsonFilePath = sampleJsonFilePath
        If Not (File.Exists(_JsonFilePath)) Then
            CreateJsonFile()
        End If
    End Sub

    Public Function ReadFromJsonFile() As List(Of Plan)
        Dim Plans As New List(Of Plan)()

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream As String = reader.ReadToEnd()
            Plans = JsonConvert.DeserializeObject(Of List(Of Plan))(JsonStream)
        End Using

        Return Plans
    End Function

    Public Sub WriteToJsonFile(Name As String, Nxt_backup As String, Src As String, Dst As String, BackupPlan As BackupPlan)
        Dim backup_plan = New Plan() With { 'Create new Plan object
            .Name = Name,
            .Nxt_backup = Nxt_backup,
            .Src = Src,
            .Dst = Dst,
            .backupPlan = BackupPlan
        }

        Dim json As String = JsonConvert.SerializeObject(backup_plan, Formatting.Indented) 'Serialize object to add to json database 
        Dim sourceJsonArray As JArray

        Using reader As New StreamReader(_JsonFilePath)
            Dim JsonStream As String = reader.ReadToEnd() 'Read json file into stream
            sourceJsonArray = JArray.Parse(JsonStream) 'Parse Array json object
            sourceJsonArray.Add(json) 'Add serialize object to json array
        End Using

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


