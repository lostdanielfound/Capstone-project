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

        Dim Previous_backup_date = PlansManipObj.ReadPlanObject_Previous_backup(PlanName) 'Kind of terrible since it's O(n) but get the job done
        Dim timestampDateTime As Date
        Dim enUS As New CultureInfo("en-US")

        'Convert Previous_back_date string into an actually timestamp to compare
        If Not Date.TryParseExact(Previous_backup_date, "MM/dd/yyyy hh:mm:ss tt", enUS, DateTimeStyles.None, timestampDateTime) Then
            Console.WriteLine("Incremental Backup failed, Previous_backup_date couldn't be parse correctly, exit...")
            Return
        End If

        Dim LogManipObj As New JsonManip()
        Dim LogPath = args(1) & "\.BoxITLog.json"
        LogManipObj.SetJsonFile(LogPath)

        Dim CurrentDirectoryTree = LogManipObj.DepthFirstTransversal(LogPath)
        Dim LogTree = LogManipObj.ReadJarray()

        ' Time to transverse the directory and files of the source and find files that have 
        ' a modified time logical greater than the Previous_backup_date timestamp.
        ' We first check the directories of the backup source first and then the files
        '
        ' Conditions: 
        ' If a directory's creation date is greater than the timestamp, add it to the backup tree.
        ' Else we ignore that directory and continue our search
        '
        ' IF we find a file that has it's modified time greater than the timestamp, we take that file and
        ' replace the copy of that file within the backup tree with the new file.
        ' ELSE we ignore that file and continue our search
        '
        ' IF 

        'REMEMBER TO SET PREVIOUS BACKUP TIME TO NOW

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