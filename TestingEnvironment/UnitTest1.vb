Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports BoxIT_Project
Imports Ionic
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports NUnit.Framework

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
            Dim LogPath = "C:\Users\Guzman\Desktop\Incremental_Testcases\Simple\.BoxITLog.json"
            Dim SrcPath = "C:\Users\Guzman\Desktop\Incremental_Testcases\Simple"
            Dim ZipDestinationPath = "C:\Users\Guzman\Desktop\Destination\Project.zip"
            JsonManipLogObj.SetJsonFile(LogPath, JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()
            Dim CurrentDirectoryTree = JsonManipLogObj.DepthFirstTransversal(SrcPath)

            Dim timestampDateTime As Date
            Dim datestring = "11/8/2023 10:40:00 PM"
            Dim formatString = "M/d/yyyy h:m:s tt"

            timestampDateTime = Date.ParseExact(datestring, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None)

            UpdateDstEntries(LogTree, CurrentDirectoryTree, timestampDateTime, ZipDestinationPath, SrcPath, "")

            Using sw As StreamWriter = File.CreateText(LogPath)
                Using writer As New JsonTextWriter(sw)
                    LogTree.WriteTo(writer)
                End Using
            End Using
        End Sub

        ' ==========================================================
        ' Adds / Updates file entry at a relative path within a zip file
        ' ==========================================================

        Sub AddArchiveEntry(PathName As String, ZipDestintationPath As String, SrcPath As String)
            Using Ziptoopen As Zip.ZipFile = Zip.ZipFile.Read(ZipDestintationPath)
                Ziptoopen.UpdateFile(SrcPath, PathName)
                Ziptoopen.Save()
            End Using
        End Sub

        ' ==========================================================
        ' Adds a new directory entry at a relative path within a zip file
        ' ==========================================================
        Sub AddArchiveDirectory(PathName As String, ZipDestintationPath As String, DirectoryName As String)
            Using zipToOpen As New FileStream(ZipDestintationPath, FileMode.Open)
                Using archive As New ZipArchive(zipToOpen, ZipArchiveMode.Update)
                    If PathName = "" Then
                        archive.CreateEntry(DirectoryName & "\")
                    Else
                        archive.CreateEntry(PathName & "\" & DirectoryName & "\")
                    End If
                End Using
            End Using
        End Sub

        Sub UpdateDstEntries(ByRef LogTree As JArray, CurrectDirectoryTree As JArray, TimestampDateTime As Date, ZipDestinationPath As String, SrcPath As String, ZipDirectoryPath As String)
            For Each Treeobject As JToken In CurrectDirectoryTree.Children().ToList()
                Dim TreeObjectTimeStamp As Date
                Dim formatString = "M/d/yyyy h:m:s tt"

                'Convert the treeObjectTimestamp string into an actually Date to compare
                If Not Date.TryParseExact(Treeobject("UpdateTime").ToString(), formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, TreeObjectTimeStamp) Then
                    Console.WriteLine("Failed to convert string to Date, failure occurred in UpdateDstEntries")
                    Return
                End If

                If TreeObjectTimeStamp > TimestampDateTime Then
                    If Treeobject("Type").ToString() = "Directory" Then 'Add new directory into DstBackup and update log
                        AddArchiveDirectory(ZipDirectoryPath, ZipDestinationPath, Treeobject("Name").ToString())

                        Dim JsonDirectoryObject = New TreeObject() With {
                        .Name = Treeobject("Name"),
                        .Files = New List(Of TreeObject),
                        .Type = "Directory",
                        .UpdateTime = Treeobject("UpdateTime")
                    }
                        LogTree.Add(JObject.Parse(JsonConvert.SerializeObject(JsonDirectoryObject)))
                    Else 'Adds / updates file entry within DstBackup and updates treelog
                        AddArchiveEntry(ZipDirectoryPath, ZipDestinationPath, SrcPath & "\" & Treeobject("Name").ToString())

                        If File.GetCreationTime(SrcPath & "\" & Treeobject("Name").ToString()) > TimestampDateTime Then 'Add the new file to the logtree if it was newly created
                            Dim JsonFileObject = New TreeObject() With {
                            .Name = Treeobject("Name"),
                            .Files = New List(Of TreeObject),
                            .Type = "File",
                            .UpdateTime = Treeobject("UpdateTime")
                        }

                            LogTree.Add(JObject.Parse(JsonConvert.SerializeObject(JsonFileObject)))
                        Else 'Update logtree entry
                            For Each Obj As JToken In LogTree.Children().ToList()
                                If Obj("Name") = Treeobject("Name").ToString() Then
                                    Obj("UpdateTime") = Treeobject("UpdateTime")
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            Next

            'Branch into other directories to add / update remaining treeobjects
            For Each TreeObject As JToken In CurrectDirectoryTree.Children().ToList()
                If TreeObject("Type").ToString() = "Directory" Then
                    Dim Branch As JToken = Nothing
                    For Each t As JToken In LogTree.Children()
                        If t("Name") = TreeObject("Name").ToString() Then
                            Branch = t
                            Exit For
                        End If
                    Next
                    UpdateDstEntriesVisit(Branch, TreeObject, TimestampDateTime, ZipDestinationPath, SrcPath & "\" & TreeObject("Name").ToString(), TreeObject("Name").ToString())
                End If
            Next
        End Sub

        Sub UpdateDstEntriesVisit(LogTree As JToken, CurrentDirectoryTree As JToken, TimestampDateTime As Date, ZipDestinationPath As String, SrcPath As String, ZipDirectoryPath As String)
            For Each TreeObject As JToken In CurrentDirectoryTree("Files").Children().ToList()
                Dim TreeObjectTimeStamp As Date
                Dim formatString = "M/d/yyyy h:m:s tt"

                'Convert the treeObjectTimestamp string into an actually Date to compare
                If Not Date.TryParseExact(TreeObject("UpdateTime").ToString(), formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, TreeObjectTimeStamp) Then
                    Console.WriteLine("Failed to convert string to Date, failure occurred in UpdateDstEntries")
                    Return
                End If

                If TreeObjectTimeStamp > TimestampDateTime Then
                    If TreeObject("Type").ToString() = "Directory" Then 'Add new directory into DstBackup and update log
                        AddArchiveDirectory(ZipDirectoryPath, ZipDestinationPath, TreeObject("Name").ToString())

                        Dim JsonDirectoryObject = New TreeObject() With {
                        .Name = TreeObject("Name"),
                        .Files = New List(Of TreeObject),
                        .Type = "Directory",
                        .UpdateTime = TreeObject("UpdateTime")
                    }

                        Dim CurrentJarray = LogTree("Files").ToString()
                        Dim NewJarray = JArray.Parse(CurrentJarray)
                        NewJarray.Add(JObject.Parse(JsonConvert.SerializeObject(JsonDirectoryObject)))

                        LogTree("Files") = NewJarray 'Add the directory to logtree

                    Else 'Update file in DstBackup and update log timestamp
                        AddArchiveEntry(ZipDirectoryPath, ZipDestinationPath, SrcPath & "\" & TreeObject("Name").ToString())

                        If File.GetCreationTime(SrcPath & "\" & TreeObject("Name").ToString()) > TimestampDateTime Then 'Add the new file to the logtree
                            Dim JsonFileObject = New TreeObject() With {
                            .Name = TreeObject("Name"),
                            .Files = New List(Of TreeObject),
                            .Type = "File",
                            .UpdateTime = TreeObject("UpdateTime")
                        }

                            Dim CurrentJarray = LogTree("Files").ToString()
                            Dim NewJarray = JArray.Parse(CurrentJarray)
                            NewJarray.Add(JObject.Parse(JsonConvert.SerializeObject(JsonFileObject)))

                            LogTree("Files") = NewJarray
                        Else 'Update logtree entry
                            For Each Obj As JToken In LogTree("Files").Children().ToList()
                                If Obj("Name") = TreeObject("Name").ToString() Then
                                    Obj("UpdateTime") = TreeObject("UpdateTime")
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            Next

            'Branch into other directories to add / update remaining treeobjects
            For Each TreeObject As JToken In CurrentDirectoryTree("Files").Children().ToList()
                If TreeObject("Type").ToString() = "Directory" Then
                    Dim Branch As Object = Nothing
                    For Each t As JToken In LogTree("Files").Children() 'LogTree should have been updated with the new directory
                        If t("Name") = TreeObject("Name").ToString() Then
                            Branch = t
                            Exit For
                        End If
                    Next

                    UpdateDstEntriesVisit(Branch, TreeObject, TimestampDateTime, ZipDestinationPath, SrcPath & "\" & TreeObject("Name").ToString(), ZipDirectoryPath & "\" & TreeObject("Name").ToString())
                End If
            Next
        End Sub

        ' ================================================================
        ' Removes File Entry from a relative path within a Zip file
        ' ================================================================
        Sub RemoveArchiveEntry(PathName As String, FullDestinationPath As String)
            Using Ziptoopen As Zip.ZipFile = Zip.ZipFile.Read(FullDestinationPath)
                Ziptoopen.RemoveEntry(PathName)
                Ziptoopen.Save()
            End Using
        End Sub

        ' ================================================================
        ' Removes Directory Entry from a relative path within a Zip file
        ' ================================================================
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