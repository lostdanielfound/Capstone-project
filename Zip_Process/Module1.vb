Imports System.IO.Compression
Imports System.IO

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

    End Sub

    Sub DifferentialBackUp(args As String())

    End Sub

    Sub Main()
        ' Program will be recieving the following arguments, 
        ' Arguments are avilable through the args array as 0-index
        ' Src
        ' Dst
        ' ScheduleT
        ' BackUpType
        ' BackUpName
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