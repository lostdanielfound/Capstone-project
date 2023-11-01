Imports System.IO.Compression
Imports System.IO
Imports System.Globalization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Module1
    Sub UpdateBackUpPlans(PlanName As String, ST As Integer)
        Dim jsonManipObj As JsonManip = New JsonManip()
        jsonManipObj.SetJsonFile("Plans.json")

        Select Case ST
            Case ScheduleType.Daily
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddDays(1).ToString())
            Case ScheduleType.Weekly
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddDays(7).ToString())
            Case ScheduleType.Monthly
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddMonths(1).ToString())
            Case ScheduleType.Yearly
                jsonManipObj.ModifyPlanObject_Nxt_backup(PlanName, Now.AddYears(1).ToString())
        End Select
    End Sub

    Sub FullBackUp(args As String())
        Dim DestinationPath = args(2) & "\" & args(5) & "_Backup"
        Dim counter = 0

        While File.Exists(DestinationPath)
            counter += 1
        End While

        If counter <> 0 Then
            DestinationPath &= "(" & counter & ")" & ".zip"
        Else
            DestinationPath &= ".zip"
        End If

        ZipFile.CreateFromDirectory(args(1), DestinationPath, CompressionLevel.Fastest, False)
    End Sub

    Sub IncrementalBackUp(args As String())
        Dim PlansManipObj As New JsonManip()
        PlansManipObj.SetJsonFile("Plans.json")
        Dim PlanName = args(5)
        Dim FullPath = args(1)

        Dim Previous_backup_date = PlansManipObj.ReadPlanObject_Previous_backup(PlanName) 'Kind of terrible since it's O(n) but get the job done
        Dim timestampDateTime As Date
        Dim enUS As New CultureInfo("en-US")

        'Convert Previous_back_date string into an actually timestamp to compare
        If Not Date.TryParseExact(Previous_backup_date, "MM/dd/yyyy hh:mm:ss tt", enUS, DateTimeStyles.None, timestampDateTime) Then
            Console.WriteLine("Incremental Backup failed, Previous_backup_date couldn't be parse correctly, exit...")
            Return
        End If

        Dim LogManipObj As New JsonManip()
        Dim LogPath = FullPath & "\.BoxITLog.json"
        LogManipObj.SetJsonFile(LogPath)

        Dim CurrentDirectoryTree = LogManipObj.DepthFirstTransversal(LogPath)
        Dim LogTree = LogManipObj.ReadJarray()

        'Remove Source Entries

        'This routine is meant to remove all treeobject entries from the JSON Log tree that don't exist within the Source backup directory (While also removing these entries from the actual Destination backup directory). From this, we wouldn't need to worry about Files or folders that aren't being tracked anymore, since there are deleted, this also relives the issues that would arise if we attempt to enter within a directory that doesn't exist. j
        'Update Destination Entries

        'This one is a little tricky, this routine is meant to transverse through the Source backup directory and compare the timestamps of each treeobject to determine if they have been updated / created. 

        'IF they have been updated, update the timestamp of that treeobject within the Logtree And update that file within the Destination backup directory. 
        'IF a treeobject that we found doesn't exist within the Logtree, then we add the treeobject to the Logtree (At the correct location of course) and add it's contents within the Destination backup directory. 

        'Remove all entries that don't exist within the source directory tree
        DeleteSrcEntires(LogTree, FullPath)

        'Goes through all the files within the tree and and file / folder that is greater
        'timestamp than the previous backup timestamp, it will be updated within the backup destination folder
        UpdateDstEntries(LogTree, CurrentDirectoryTree, timestampDateTime)

    End Sub

    Sub DeleteSrcEntires(ByRef LogTree As JArray, SrcPath As String)
        'Find which entries are a subdirectory and transverse through it first

        'Look through files and directories at the root within the logtree and remove
        'the ones that don't exist within the source.
        For Each TreeObject As JToken In LogTree.Children()
            If Not File.Exists(SrcPath & "\" & TreeObject("Name").ToString()) Then
                'Remove the treeObject from the destination backup and LogTree
                RemoveArchiveEntry(TreeObject("Name").ToString())
                TreeObject.Remove()
            End If
        Next

        'Now transverse through the directories that still exist within the source. 
        For Each TreeObject As JToken In LogTree.Children()
            If TreeObject("Type").ToString() = "Directory" Then
                DeleteSrcEntriesVisit(TreeObject, SrcPath & "\" & TreeObject("Name").ToString(), TreeObject("Name").ToString())
            End If
        Next
    End Sub

    Sub DeleteSrcEntriesVisit(ByRef Subdirectory As JToken, SrcPath As String, DstPath As String)
        'Remove the files and directories within this subdirectory that don't exist within the source
        For Each TreeObject As JToken In Subdirectory.Children()("Files")("Value").Children()
            If Not File.Exists(SrcPath & "\" & TreeObject("Name").ToString()) Then
                'Remove TreeObject from destination backup and LogTree
                RemoveArchiveEntry(DstPath & "\" & TreeObject("Name").ToString())
                TreeObject.Remove()
            End If
        Next

        For Each TreeObject As JToken In Subdirectory.Children()("Files")("Value").Children()
            If TreeObject("Type").ToString() = "Directory" Then
                DeleteSrcEntriesVisit(TreeObject, SrcPath & "\" & TreeObject("Name").ToString(), TreeObject("Name").ToString())
            End If
        Next
    End Sub

    Sub UpdateDstEntries(LogTree As JArray, CurrectDirectoryTree As JArray, TimestampDateTime As Date)
        'Go through the treeobjects within sourcetree and update the ones where 
        'Their UpdateTime is > TimestampDateTime
        For Each TreeObject As JToken In CurrectDirectoryTree.Children()
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
        For Each TreeObject As JToken In CurrectDirectoryTree.Children()
            If TreeObject("Type").ToString() = "Directory" Then
                UpdateDstEntriesVisit(LogTree(idx), TreeObject, TimestampDateTime)
            End If
            idx = idx + 1
        Next
    End Sub

    Sub UpdateDstEntriesVisit(LogTree As JToken, CurrentDirectoryTree As JToken, TimestampDateTime As Date)
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
                UpdateDstEntriesVisit(LogTree.Children()("Files")("Value").Children()(idx), TreeObject, TimestampDateTime)
            End If
            idx = idx + 1
        Next
    End Sub

    Sub UpdateLogEntry()

    End Sub

    Sub RemoveArchiveEntry(PathName As String)

    End Sub

    Sub DifferentialBackUp(args As String())

    End Sub

    Sub Main()
        ' Program will be recieving the following arguments, 
        ' Arguments are avilable through the args array as 0-index
        ' Src = 1
        ' Dst = 2
        ' ScheduleT = 3
        ' BackUpType = 4
        ' BackUpName = 5
        '
        ' The file that is output should be the following format: 
        ' [Name of backup]_Backup(counter)

        Dim args = Environment.GetCommandLineArgs() 'Get command line args
        Dim BackUpName = args(5)
        Dim BackUpType = args(4)
        Dim ScheduleType = args(3)

        Select Case BackUpType
            Case BackupPlan.FullBackup
                FullBackUp(args)
                UpdateBackUpPlans(BackUpName, ScheduleType)
            Case BackupPlan.IncrementalBackup
                IncrementalBackUp(args)
                UpdateBackUpPlans(BackUpName, ScheduleType)
            Case BackupPlan.DifferentialBackup
                DifferentialBackUp(args)
                UpdateBackUpPlans(BackUpName, ScheduleType)
        End Select
    End Sub
End Module