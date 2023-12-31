﻿Imports System.IO
Imports System.IO.Compression
Imports Ionic
Imports System.Globalization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Module1
    Dim BackupPlansPath = ""

    Function GetNextCustomBackUpDate(CustomDaysString As String) As Date
        ' comma-separated string
        Dim DaysList As New List(Of String)(CustomDaysString.Split(","c))

        ' Create an boolean day array
        Dim DaysToStrings = New String() {"SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"}
        Dim DaysToBoolean = New Boolean() {0, 0, 0, 0, 0, 0, 0}

        For Each day As String In DaysList
            For index = 0 To 6
                If day = DaysToStrings(index) Then
                    DaysToBoolean(index) = True
                End If
            Next
        Next

        Dim DaysCounter = 1
        Dim CurrentDay = Now.DayOfWeek
        Dim DaysIndex = CurrentDay + 1

        While DaysIndex <> CurrentDay Mod 7
            If DaysToBoolean(DaysIndex) Then
                Exit While
            End If
            DaysCounter += 1
            DaysIndex = (DaysIndex + 1) Mod 7
        End While

        Return Now.AddDays(DaysCounter)
    End Function

    Sub UpdateBackUpPlans(PlanName As String, ST As Integer, Optional CustomDaysString As String = "")
        Dim jsonManipObj As JsonManip = New JsonManip()
        jsonManipObj.SetJsonFile(BackupPlansPath)

        Select Case ST
            Case ScheduleType.Daily
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddDays(1).ToString())
            Case ScheduleType.Weekly
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddDays(7).ToString())
            Case ScheduleType.Monthly
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddMonths(1).ToString())
            Case ScheduleType.Yearly
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddYears(1).ToString())
            Case ScheduleType.Custom
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, GetNextCustomBackUpDate(CustomDaysString))
        End Select

        jsonManipObj.ModifyPlanObject_Previous_backup(PlanName, Now.ToString())
    End Sub

    Sub FullBackUp(args As String())
        Dim DestinationPath = args(2) & "\" & args(5) & "_Backup.zip"

        Dim Counter = 1
        While File.Exists(DestinationPath)
            DestinationPath = args(2) & "\" & args(5) & "_Backup" & Counter & ".zip"
            Counter += 1
        End While

        ZipFile.CreateFromDirectory(args(1), DestinationPath, CompressionLevel.Fastest, False)
    End Sub

    Sub IncrementalBackUp(args As String())
        Dim PlansManipObj As New JsonManip()
        PlansManipObj.SetJsonFile(BackupPlansPath)

        Dim PlanName = args(5)
        Dim ZipDestinationPath = args(2) & "\" & args(5) & "_Backup.zip"
        Dim SrcPath = args(1)

        Dim Previous_backup_date = PlansManipObj.ReadPlanObject_Previous_backup(PlanName) 'Kind of terrible since it's O(n) but get the job done
        Dim timestampDateTime As Date
        Dim formatString = "M/d/yyyy h:m:s tt"

        'Convert Previous_back_date string into an actually timestamp to compare
        If Not Date.TryParseExact(Previous_backup_date, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, timestampDateTime) Then
            Console.WriteLine("Incremental Backup failed, Previous_backup_date couldn't be parse correctly, exit...")
            Return
        End If

        Dim LogManipObj As New JsonManip()
        Dim LogPath = SrcPath & "\.BoxITLog.json"
        LogManipObj.SetJsonFile(LogPath)

        Dim CurrentDirectoryTree = LogManipObj.DepthFirstTransversal(SrcPath)
        Dim LogTree = LogManipObj.ReadJarray()

        DeleteSrcEntires(LogTree, SrcPath, ZipDestinationPath) 'Delete Src Files / Directories that aren't within the CurrentDirectoryTree anymore
        UpdateDstEntries(LogTree, CurrentDirectoryTree, Previous_backup_date, ZipDestinationPath, SrcPath, "") 'Update Files / Directorties within the Zip Dst File

        'Rewrite .BoxITLog.json with LogTree
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

    Sub DifferentialBackUp(args As String())

    End Sub

    Sub Main()
        Dim Debugmode = False
        ' Program will be recieving the following arguments, 
        ' Arguments are avilable through the args array as 0-index
        ' Src = 1
        ' Dst = 2
        ' ScheduleT = 3
        ' BackUpType = 4
        ' BackUpName = 5
        ' [DaysList = 6] optional
        ' 
        ' schtasks /create /sc weekly /d WED,THU,SAT /tn "BoxIT_kljk_IncrementalBackup" /tr "C:\Users\Guzman\Desktop\Capstone-project\BoxIT_Project\bin\Debug\net6.0-windows\Zip_Process_Release\Zip_Process.exe C:\Users\Guzman\Desktop\http-codes C:\Users\Guzman\Desktop\Logs - Copy 4 1 kljk WED,THU,SAT"
        ' The file that is output should be the following format: 
        ' [Name of backup]_Backup.zip
        BackupPlansPath = System.AppDomain.CurrentDomain.BaseDirectory() & "\..\" & "Plans.json"

        Dim args = Environment.GetCommandLineArgs() 'Get command line args

        If Debugmode Then
            Console.WriteLine(args.Length)
            Dim text = "Quote"
            Console.WriteLine("Test in " & Chr(34) & text & Chr(34))
            For Each arg In args
                Console.WriteLine(arg)
            Next

            Return
        End If
        If args.Length <= 1 Then 'Zip process was passed no arguments, can't continue
            Return
        End If

        Dim BackUpName = args(5)
        Dim BackUpType = args(4)
        Dim Schedule = args(3)
        Dim CustomDaysString = ""
        If Schedule = ScheduleType.Custom Then
            CustomDaysString = args(6)
        End If

        Select Case BackUpType
            Case BackupPlan.FullBackup
                FullBackUp(args)
                UpdateBackUpPlans(BackUpName, Schedule, CustomDaysString)
            Case BackupPlan.IncrementalBackup
                IncrementalBackUp(args)
                UpdateBackUpPlans(BackUpName, Schedule, CustomDaysString)
        End Select
    End Sub
End Module