Imports NUnit.Framework
Imports System.IO
Imports BoxIT_Project
Imports Newtonsoft.Json.Linq
Imports System.IO.Compression
Imports Ionic
Imports Newtonsoft.Json
Imports System.Globalization

Namespace TestingEnvironment

    Public Class Tests
        Private JsonManipObj As JsonManip
        Private JsonManipLogObj As JsonManip
        Private JsonFileName = "Plans_UnitTest.json"

        <SetUp>
        Public Sub Setup()
            'Setting up JsonObject
            JsonManipObj = New JsonManip()
            JsonManipLogObj = New JsonManip()
        End Sub

        <Test>
        Public Sub EmptyFileCreation_EqualTest()
            JsonManipObj.SetJsonFile(JsonFileName, JsonManip.JsonFileType.Plans)

            Dim Plans = JsonManipObj.ReadPlanObjectsFromJsonFile()
            Assert.AreEqual(Plans.Capacity, 0)
        End Sub

        ' Used to testout array access in JToken
        '<Test>
        Public Sub LogTreeManip_StageTest()
            JsonManipLogObj.SetJsonFile("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json", JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()

            'Test to see if we can recursivly transverse through token array
            Dim nameOFDirecotyr = LogTree(0)("Files")(0)("Name").ToString()
        End Sub

        ' The Delete Entry process of the Incremental back
        '<Test>
        Public Sub IncrementalDeleteProcess_StageTest()
            JsonManipLogObj.SetJsonFile("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json", JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()

            DeleteSrcEntires(LogTree, "C:\Users\Guzman\Desktop\411-Project2", "C:\Users\Guzman\Desktop\Dest_backup\Project2_DP_Spring2022.zip")

            'Writes content of Jarray back into the json file
            Using sw As StreamWriter = File.CreateText("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json")
                Using writer As New JsonTextWriter(sw)
                    LogTree.WriteTo(writer)
                End Using
            End Using
            Assert.IsTrue(True)
        End Sub

        <Test>
        Public Sub IncrementalUpdateProcess_StateTest()
            JsonManipLogObj.SetJsonFile("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json", JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()
            Dim CurrentDirectoryTree = JsonManipLogObj.DepthFirstTransversal("C:\Users\Guzman\Desktop\411-Project2\")

            Dim timestampDateTime As Date
            Dim datestring = "11/2/2023 11:42:21 PM"
            Dim formatString = "M/d/yyyy h:m:s tt"

            timestampDateTime = Date.ParseExact(datestring, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None)

            UpdateDstEntries(LogTree, CurrentDirectoryTree, timestampDateTime, "C:\Users\Guzman\Desktop\Destination\report.zip")
        End Sub

        Sub AddArchiveEntry(PathName As String, FullDestinationPath As String)

        End Sub

        Sub AddArchiveDirectory(PathName As String, FullDestinationPath As String)

        End Sub

        Sub UpdateDstEntries(LogTree As JArray, CurrectDirectoryTree As JArray, TimestampDateTime As Date, FullDestinationPath As String)
            'Go through the treeobjects within sourcetree and update the ones where 
            'Their UpdateTime is > TimestampDateTime
            For Each Treeobject As JToken In CurrectDirectoryTree.Children().ToList()
                Dim TreeObjectTimeStamp As Date
                Dim formatString = "M/d/yyyy h:m:s tt"

                'Convert the treeObjectTimestamp string into an actually Date to compare
                If Not Date.TryParseExact(Treeobject("UpdateTime").ToString(), formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, TreeObjectTimeStamp) Then
                    Console.WriteLine("Failed to convert string to Date, failure occurred in UpdateDstEntries")
                    Return
                End If

                If TreeObjectTimeStamp > TimestampDateTime Then
                    'IF the treeobject already exist within the log, update it (will only be files)
                    'IF not then add it (can be files and directories) 
                    If Treeobject("Type").ToString() = "Directory" Then 'Add new directory into DstBackup and update log
                        'Adding Directory into Dstbackup

                        'AddArchiveEntry(Treeobject("Name").ToString(), FullDestinationPath)

                        'Add directory and it's treeobject in log

                        'Dim JsonDirectoryObject = New TreeObject() With {
                        '.Name = Treeobject("Name"),
                        '.Files = New List(Of TreeObject),
                        '.Type = "Directory",
                        '.UpdateTime = Treeobject("UpdateTime")
                        '}

                        'LogTree.Add(JObject.Parse(JsonConvert.SerializeObject(JsonDirectoryObject))) 'Add the directory
                    Else 'Update file in DstBackup
                        'AddArchiveEntry(Treeobject("Name").ToString(), FullDestinationPath)

                    End If
                End If
            Next

            Return

            'Go through directory treeobjects and update it's treeobject
            'LOGTREE MUST BE UPDATED WITH NEW ANY NEW DIRECTORY BEFORE CONTINUING
            Dim idx = 0
            For Each TreeObject As JToken In CurrectDirectoryTree.Children()
                If TreeObject("Type").ToString() = "Directory" Then
                    UpdateDstEntriesVisit(LogTree(idx), TreeObject, TimestampDateTime, FullDestinationPath)
                End If
                idx = idx + 1
            Next
        End Sub

        Sub UpdateDstEntriesVisit(LogTree As JToken, CurrentDirectoryTree As JToken, TimestampDateTime As Date, FullDestinationPath As String)
            For Each TreeObject As JToken In CurrentDirectoryTree.Children()("Files")("Value").Children()
                Dim TreeObjectTimeStamp As Date
                Dim enUS As New CultureInfo("en-US")

                'Convert the treeObjectTimestamp string into an actually Date to compare
                If Not Date.TryParseExact(TreeObject("UpdateTime").ToString(), "MM/dd/yyyy hh:mm:ss tt", enUS, DateTimeStyles.None, TreeObjectTimeStamp) Then
                    Console.WriteLine("Failed to convert string to Date, failure occurred in UpdateDstEntries")
                    Return
                End If

                If TreeObjectTimeStamp > TimestampDateTime Then
                    'IF the treeobject already exist within the log, update it (will only be files)
                    'IF not then add it (can be files and directories) 
                    If TreeObject("Type").ToString() = "Directory" Then 'Add new directory into DstBackup and update log
                        'Adding Directory into Dstbackup
                        '---

                        'Add directory and it's treeobject in log
                        '---

                    Else 'Update file in DstBackup and update log timestamp

                    End If
                End If
            Next

            'Go through directory treeobjects and update it's treeobject
            'LOGTREE MUST BE UPDATED WITH NEW ANY NEW DIRECTORY BEFORE CONTINUING
            Dim idx = 0
            For Each TreeObject As JToken In CurrentDirectoryTree.Children()("Files")("Value").Children()
                If TreeObject("Type").ToString() = "Directory" Then
                    UpdateDstEntriesVisit(LogTree.Children()("Files")("Value").Children()(idx), TreeObject, TimestampDateTime, FullDestinationPath)
                End If
                idx = idx + 1
            Next
        End Sub

        Sub RemoveArchiveEntry(PathName As String, FullDestinationPath As String)
            Using Ziptoopen As Zip.ZipFile = Zip.ZipFile.Read(FullDestinationPath)
                Ziptoopen.RemoveEntry(PathName)
                Ziptoopen.Save()
            End Using
        End Sub

        Sub RemoveArchiveDirectory(PathName As String, FullDestinationPath As String)
            Using Ziptoopen As Zip.ZipFile = Zip.ZipFile.Read(FullDestinationPath)
                Ziptoopen.RemoveSelectedEntries(PathName & "\*")
                Ziptoopen.Save()
            End Using
        End Sub

        Sub DeleteSrcEntires(ByRef LogTree As JArray, SrcPath As String, FullDestinationPath As String)
            'Find which entries are a subdirectory and transverse through it first
            Dim TreeObjectName As String
            'Look through files and directories at the root within the logtree and remove
            'the ones that don't exist within the source.
            For Each TreeObject As JToken In LogTree.Children().ToList()
                TreeObjectName = SrcPath & "\" & TreeObject("Name").ToString()

                'Remove the treeObject from the destination backup and LogTree
                If Not File.Exists(TreeObjectName) And TreeObject("Type") = "File" Then
                    RemoveArchiveEntry(TreeObject("Name").ToString(), FullDestinationPath)
                    TreeObject.Remove()
                End If

                If Not Directory.Exists(TreeObjectName) And TreeObject("Type") = "Directory" Then
                    RemoveArchiveDirectory(TreeObject("Name").ToString(), FullDestinationPath)
                    TreeObject.Remove()
                End If
            Next

            'Now transverse through the directories that still exist within the source. 
            For Each TreeObject As JToken In LogTree.Children().ToList()
                If TreeObject("Type").ToString() = "Directory" Then
                    DeleteSrcEntriesVisit(TreeObject, SrcPath & "\" & TreeObject("Name").ToString(), TreeObject("Name").ToString(), FullDestinationPath)
                End If
            Next
        End Sub

        Sub DeleteSrcEntriesVisit(ByRef Subdirectory As JToken, SrcPath As String, DstPath As String, FullDestinationPath As String)
            'Remove the files and directories within this subdirectory that don't exist within the source
            Dim TreeObjectName As String
            For Each TreeObject As JToken In Subdirectory("Files").Children().ToList()
                TreeObjectName = SrcPath & "\" & TreeObject("Name").ToString()

                'Remove the treeObject from the destination backup and LogTree
                If Not File.Exists(TreeObjectName) And TreeObject("Type") = "File" Then
                    RemoveArchiveEntry(DstPath & "\" & TreeObject("Name").ToString(), FullDestinationPath)
                    TreeObject.Remove()
                End If


                If Not Directory.Exists(TreeObjectName) And TreeObject("Type") = "Directory" Then
                    RemoveArchiveDirectory(DstPath & "\" & TreeObject("Name").ToString(), FullDestinationPath)
                    TreeObject.Remove()
                End If
            Next

            For Each TreeObject As JToken In Subdirectory("Files").Children().ToList()
                If TreeObject("Type").ToString() = "Directory" Then
                    DeleteSrcEntriesVisit(TreeObject, SrcPath & "\" & TreeObject("Name").ToString(), DstPath & "\" & TreeObject("Name").ToString(), FullDestinationPath)
                End If
            Next
        End Sub

        <TearDown>
        Public Sub Teardown()
            If File.Exists(JsonFileName) Then
                File.Delete(JsonFileName)
            End If
        End Sub

    End Class

End Namespace