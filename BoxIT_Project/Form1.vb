Imports System.IO
Imports System.IO.Compression
Imports Ionic

Public Class Form1

    '===================
    ' Auxiliary Methods
    '===================
    Private Sub Log(LogMessage As String)
        My.Application.Log.WriteEntry("=== " & LogMessage & " ===")
    End Sub

    Private Sub ReadDirectory(Directory As String)
        Log("Files read from directory: " & Directory)

        Dim Idx As Integer = 0
        For Each file As String In My.Computer.FileSystem.GetFiles(Directory)
            Log("[" & Idx & "]" & " " & file)
            Idx += 1
        Next

        For Each folder As String In My.Computer.FileSystem.GetDirectories(Directory)
            Log("[" & Idx & "]" & " " & folder)
            Idx += 1
        Next

    End Sub

    Private Function GetNextCustomBackUpDate() As Date
        Dim DaysCounter = 1

        Select Case Today.DayOfWeek
            Case DayOfWeek.Sunday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    MCheckbox.Checked,
                    TCheckBox.Checked,
                    WCheckbox.Checked,
                    ThCheckBox.Checked,
                    FCheckBox.Checked,
                    SatCheckBox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
            Case DayOfWeek.Monday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    TCheckBox.Checked,
                    WCheckbox.Checked,
                    ThCheckBox.Checked,
                    FCheckBox.Checked,
                    SatCheckBox.Checked,
                    SunCheckBox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
            Case DayOfWeek.Tuesday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    WCheckbox.Checked,
                    ThCheckBox.Checked,
                    FCheckBox.Checked,
                    SatCheckBox.Checked,
                    SunCheckBox.Checked,
                    MCheckbox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
            Case DayOfWeek.Wednesday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    ThCheckBox.Checked,
                    FCheckBox.Checked,
                    SatCheckBox.Checked,
                    SunCheckBox.Checked,
                    MCheckbox.Checked,
                    TCheckBox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
            Case DayOfWeek.Thursday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    FCheckBox.Checked,
                    SatCheckBox.Checked,
                    SunCheckBox.Checked,
                    MCheckbox.Checked,
                    TCheckBox.Checked,
                    WCheckbox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
            Case DayOfWeek.Friday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    SatCheckBox.Checked,
                    SunCheckBox.Checked,
                    MCheckbox.Checked,
                    TCheckBox.Checked,
                    WCheckbox.Checked,
                    ThCheckBox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
            Case DayOfWeek.Saturday
                Dim CheckBoxList As New List(Of Boolean)() From {
                    SunCheckBox.Checked,
                    MCheckbox.Checked,
                    TCheckBox.Checked,
                    WCheckbox.Checked,
                    ThCheckBox.Checked,
                    FCheckBox.Checked
                }
                For Each box As Boolean In CheckBoxList
                    If box Then
                        Return Now.AddDays(DaysCounter)
                    End If
                    DaysCounter += 1
                Next
                Exit Select
        End Select
        Return Now.AddDays(7)
    End Function

    Private Function BackupTypeToEnum(Str As String) As BackupPlan
        If Str.ToLower() = "Full Backup".ToLower() Then
            Return BackupPlan.FullBackup
        ElseIf Str.ToLower() = "Incremental Backup".ToLower() Then
            Return BackupPlan.IncrementalBackup
        ElseIf Str.ToLower() = "Differential Backup".ToLower() Then
            Return BackupPlan.DifferentialBackup
        End If

        Return BackupPlan.None
    End Function

    Private Sub SchedulePlanTask(Src As String, Dst As String, BackUpName As String, BackUpType As BackupPlan, ST As ScheduleType)
        Dim TaskName = "BoxIT_" & BackUpName
        Dim ScheduleT As String
        Dim cmdline As String
        Dim ExecutibleName = "Zip_Process_Release\Zip_Process.exe"
        Select Case BackUpType 'Append BackupType to the Task name
            Case BackupPlan.FullBackup
                TaskName &= "_FullBackup"
            Case BackupPlan.IncrementalBackup
                TaskName &= "_IncrementalBackup"
            Case BackupPlan.DifferentialBackup
                TaskName &= "_DifferentialBackup"
            Case Else
        End Select

        Select Case ST
            Case ScheduleType.Daily
                ScheduleT = "daily"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & Chr(34) & TaskName & Chr(34) & " /tr " & Chr(34) & "\" & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & "\" & Chr(34) & " " & "\" & Chr(34) & Src & "\" & Chr(34) & " " & "\" & Chr(34) & Dst & "\" & Chr(34) & " " & ST & " " & BackUpType & " " & BackUpName & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Weekly
                ScheduleT = "weekly"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & Chr(34) & TaskName & Chr(34) & " /tr " & Chr(34) & "\" & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & "\" & Chr(34) & " " & "\" & Chr(34) & Src & "\" & Chr(34) & " " & "\" & Chr(34) & Dst & "\" & Chr(34) & " " & ST & " " & BackUpType & " " & BackUpName & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Monthly
                ScheduleT = "monthly"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & Chr(34) & TaskName & Chr(34) & " /tr " & Chr(34) & "\" & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & "\" & Chr(34) & " " & "\" & Chr(34) & Src & "\" & Chr(34) & " " & "\" & Chr(34) & Dst & "\" & Chr(34) & " " & ST & " " & BackUpType & " " & BackUpName & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Yearly
                ScheduleT = "monthly"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & Chr(34) & TaskName & Chr(34) & " /tr " & Chr(34) & "\" & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & "\" & Chr(34) & " " & "\" & Chr(34) & Src & "\" & Chr(34) & " " & "\" & Chr(34) & Dst & "\" & Chr(34) & " " & ST & " " & BackUpType & " " & BackUpName & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Custom
                Dim CheckBoxList As New List(Of Boolean)() From {
                    SunCheckBox.Checked,
                    MCheckbox.Checked,
                    TCheckBox.Checked,
                    WCheckbox.Checked,
                    ThCheckBox.Checked,
                    FCheckBox.Checked,
                    SatCheckBox.Checked
                }
                Dim DaysToStrings = New String() {"SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"}
                Dim CustomString As String = String.Empty
                Dim ArrCounter = 0
                For Each box As Boolean In CheckBoxList
                    If CustomString = String.Empty And box Then
                        CustomString = DaysToStrings(ArrCounter)
                    ElseIf box Then
                        CustomString = CustomString & "," & DaysToStrings(ArrCounter)
                    End If
                    ArrCounter += 1
                Next

                ScheduleT = "weekly"

                If EnableEndBackupDate.Checked Then
                    cmdline = "schtasks /create /sc " & ScheduleT & " /d " & CustomString & " /ed " & EndingBackupDateAndTimePicker.Value.ToString("MM/dd/yyy") & " /tn " & Chr(34) & TaskName & Chr(34) & " /tr " & Chr(34) & "\" & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & "\" & Chr(34) & " " & "\" & Chr(34) & Src & "\" & Chr(34) & " " & "\" & Chr(34) & Dst & "\" & Chr(34) & " " & ST & " " & BackUpType & " " & BackUpName & " " & CustomString & Chr(34)
                Else
                    cmdline = "schtasks /create /sc " & ScheduleT & " /d " & CustomString & " /tn " & Chr(34) & TaskName & Chr(34) & " /tr " & Chr(34) & "\" & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & "\" & Chr(34) & " " & "\" & Chr(34) & Src & "\" & Chr(34) & " " & "\" & Chr(34) & Dst & "\" & Chr(34) & " " & ST & " " & BackUpType & " " & BackUpName & " " & CustomString & Chr(34)
                End If

                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
        End Select

    End Sub

    Private Sub DeletePlanTask(BackupName As String, BackupType As BackupPlan)
        Dim TaskName = "BoxIT_" & BackupName
        Dim cmdline As String
        Select Case BackupType 'Append BackupType to the Task name
            Case BackupPlan.FullBackup
                TaskName &= "_FullBackup"
            Case BackupPlan.IncrementalBackup
                TaskName &= "_IncrementalBackup"
            Case BackupPlan.DifferentialBackup
                TaskName &= "_DifferentialBackup"
            Case Else
        End Select

        cmdline = "schtasks /delete /tn " & Chr(34) & TaskName & Chr(34) & " /f"
        Log("Executing CMD scheduler command:" & cmdline)
        Shell(cmdline)
    End Sub

    Private Sub PopulatePlansTable()
        CurrentPlanList.Rows.Clear() 'Clear table rows to readd
        Dim jsonManipObj As JsonManip = New JsonManip()
        jsonManipObj.SetJsonFile("Plans.json")

        For Each plan As Plan In jsonManipObj.ReadPlanObjectsFromJsonFile()
            Dim newRow As New DataGridViewRow() 'Creating new row

            For index = 1 To 6 'Adding cells to row
                newRow.Cells.Add(New DataGridViewTextBoxCell())
            Next

            'populate cells
            newRow.Cells(0).Value = plan.Name
            newRow.Cells(1).Value = plan.Nxt_backup
            newRow.Cells(2).Value = plan.Previous_backup
            newRow.Cells(3).Value = plan.Src
            newRow.Cells(4).Value = plan.Dst
            Select Case plan.backupPlan
                Case BackupPlan.FullBackup
                    newRow.Cells(5).Value = "Full Backup"
                Case BackupPlan.IncrementalBackup
                    newRow.Cells(5).Value = "Incremental Backup"
                Case BackupPlan.DifferentialBackup
                    newRow.Cells(5).Value = "Differential Backup"
                Case Else
                    newRow.Cells(5).Value = "None"
            End Select

            CurrentPlanList.Rows.Add(newRow) 'Add the row to the current list
        Next
    End Sub

    Private Sub ActivateBackupBtn()
        Dim EnableCondition = (SrcChosen And DestChosen And ScheduleTypeChosen And BackupNameChosen)

        DaysSelected = SunCheckBox.Checked Or MCheckbox.Checked Or TCheckBox.Checked Or WCheckbox.Checked Or ThCheckBox.Checked Or FCheckBox.Checked Or SatCheckBox.Checked

        If EnableCondition And (ScheduleTypeComboBox.SelectedIndex <> ScheduleType.Custom) Then
            BackupBtn.Enabled = True
            BackupBtn.BackColor = Color.FromArgb(192, 255, 192)
        ElseIf EnableCondition And (ScheduleTypeComboBox.SelectedIndex = ScheduleType.Custom) Then
            If (OccurancesComboBox.SelectedIndex <> -1) And DaysSelected Then
                BackupBtn.Enabled = True
                BackupBtn.BackColor = Color.FromArgb(192, 255, 192)
            End If
        Else
            BackupBtn.Enabled = False
            BackupBtn.BackColor = Color.FromArgb(208, 209, 201)
        End If
    End Sub

    '===================
    ' Form variables
    '===================
    Dim Src As String = String.Empty
    Dim Dest As String = String.Empty

    Dim StartBackupDate As Date
    Dim EndBackupDate As Date

    'Flags for user progression
    Dim SrcChosen = False
    Dim DestChosen = False
    Dim ScheduleTypeChosen = False
    Dim BackupNameChosen = False
    Dim DaysSelected = False

    '===================
    ' Event Handlers
    '===================
    Private Sub AddSourceBtn_Click(sender As Object, e As EventArgs) Handles AddSourceBtn.Click

        'Bring up Dialog to select source
        Dim folderSelectionDig As New FolderBrowserDialog With {
            .ShowNewFolderButton = False
        }

        Log("Showing folder selection Dialog to select backup src")
        Dim ReturnResult As DialogResult = folderSelectionDig.ShowDialog()

        Select Case ReturnResult
            Case DialogResult.OK 'Set the backup source
                Src = folderSelectionDig.SelectedPath
                SourceTextBox.Text = Src
                SourceTextBox.BackColor = Color.FromArgb(192, 255, 192)
                SrcChosen = True 'Set flag
                Log("Set Src as " & Src)
                AddDestBtn.Enabled = True
            Case DialogResult.Cancel 'Dialog failed, reset flag
                SrcChosen = False
                SourceTextBox.Text = "Please select a Source"
                Src = String.Empty
        End Select

        Backup_Plan_RadioSelection.Enabled = (SrcChosen And DestChosen)

        ActivateBackupBtn()
    End Sub

    Private Sub Clear1Btn_Click(sender As Object, e As EventArgs) Handles Clear1Btn.Click
        If Src = String.Empty Then
            Return
        End If
        SourceTextBox.Text = "Please select a Source"
        SourceTextBox.BackColor = Color.White
        SrcChosen = False 'Set flag
        Log("Clearing Src")
        Src = String.Empty

        Backup_Plan_RadioSelection.Enabled = (SrcChosen And DestChosen)
        ActivateBackupBtn()
    End Sub

    Private Sub AddDestBtn_Click(sender As Object, e As EventArgs) Handles AddDestBtn.Click

        'Bring up Dialog to select source
        Dim folderSelectionDig As New FolderBrowserDialog With {
            .ShowNewFolderButton = False
        }

        Dim ReturnResult = folderSelectionDig.ShowDialog()

        Select Case ReturnResult
            Case DialogResult.OK
                Dest = folderSelectionDig.SelectedPath
                DestinationTextBox.Text = Dest
                DestinationTextBox.BackColor = Color.FromArgb(192, 255, 192)
                DestChosen = True 'Set flag
                Log("Set Dest as " & Dest)
            Case DialogResult.Cancel 'Reset flag and Dest 
                DestChosen = False
                DestinationTextBox.Text = "Please select a Destination"
                Dest = String.Empty
        End Select

        Backup_Plan_RadioSelection.Enabled = (SrcChosen And DestChosen)

        ActivateBackupBtn()
    End Sub

    Private Sub Clear2Btn_Click(sender As Object, e As EventArgs) Handles Clear2Btn.Click
        If Dest = String.Empty Then
            Return
        End If
        DestinationTextBox.Text = "Please select a Destination"
        DestinationTextBox.BackColor = Color.White
        DestChosen = False
        Log("Clearing Dest")
        Dest = String.Empty

        Backup_Plan_RadioSelection.Enabled = (SrcChosen And DestChosen)
        ActivateBackupBtn()
    End Sub

    Private Sub BackupBtn_Click(sender As Object, e As EventArgs) Handles BackupBtn.Click
        'Check to ensure that any folder / file is named the same as output Zip file
        'Ensure that User has selected a valid Src and Dest
        If Src = "" Or Dest = "" Then
            MsgBox("Please select a Source and Destination!", MsgBoxStyle.Information, "No Source or Destination")
            BackupBtn.Enabled = False
            BackupBtn.BackColor = Color.FromArgb(208, 209, 201)
            Return
        ElseIf CustomizeScheduleGroupBox.Enabled = True And DaysSelected = False Then
            MsgBox("Please selected a day(s) to repeat backup task!", MsgBoxStyle.Information, "No day(s) selected")
            BackupBtn.Enabled = False
            BackupBtn.BackColor = Color.FromArgb(208, 209, 201)
            Return
        End If

        'Creating new manipuation object to write to json array
        Dim jsonManipObj As JsonManip = New JsonManip()
        jsonManipObj.SetJsonFile("Plans.json")

        Dim NextBackUpDate = ""
        'Switch statment for to get the next scheduled backup
        Select Case ScheduleTypeComboBox.SelectedIndex
            Case ScheduleType.Daily
                NextBackUpDate = Now.AddDays(1).ToString()
            Case ScheduleType.Weekly
                NextBackUpDate = Now.AddDays(7).ToString()
            Case ScheduleType.Monthly
                NextBackUpDate = Now.AddMonths(1).ToString()
            Case ScheduleType.Yearly
                NextBackUpDate = Now.AddYears(1).ToString()
            Case ScheduleType.Custom
                If OccurancesComboBox.SelectedText = "Daily" Then
                    NextBackUpDate = Now.AddDays(1).ToString()
                Else
                    NextBackUpDate = GetNextCustomBackUpDate()
                End If

        End Select

        Dim selectedRadioButton As RadioButton = Backup_Plan_RadioSelection.Controls.OfType(Of RadioButton).FirstOrDefault(Function(r) r.Checked = True) 'Gets the current selected Radiobutton
        Dim ArchiveDestinationPath = Dest & "\" & BackupNameTextBox.Text & "_Backup.zip"
        Dim wroteToFile = False

        'Writes to the Plans.json on new plan and schedules the backup plan
        If selectedRadioButton.Text = "Full Backup" Then
            'Performing fullbackup
            If File.Exists(ArchiveDestinationPath) Then
                File.Delete(ArchiveDestinationPath)
            End If

            ZipFile.CreateFromDirectory(Src, ArchiveDestinationPath, CompressionLevel.Optimal, False)

            'Schedule Backup task
            wroteToFile = jsonManipObj.WritePlanObjectToJsonFile(BackupNameTextBox.Text, NextBackUpDate, Now, Src, Dest, BackupPlan.FullBackup)
            If wroteToFile Then
                SchedulePlanTask(Src, Dest, BackupNameTextBox.Text, BackupPlan.FullBackup, ScheduleTypeComboBox.SelectedIndex)
            End If
        ElseIf selectedRadioButton.Text = "Incremental" Then
            'Performing fullbackup
            If File.Exists(ArchiveDestinationPath) Then
                File.Delete(ArchiveDestinationPath)
            End If
            ZipFile.CreateFromDirectory(Src, ArchiveDestinationPath, CompressionLevel.Optimal, False)

            'Creating Logfile
            Dim logCreation As New JsonManip()
            logCreation.SetJsonFile(Src & "\.BoxITLog.json")
            logCreation.JsonTreeCreation()

            'Scheduling backup task
            wroteToFile = jsonManipObj.WritePlanObjectToJsonFile(BackupNameTextBox.Text, NextBackUpDate, Now, Src, Dest, BackupPlan.IncrementalBackup)
            If wroteToFile Then
                SchedulePlanTask(Src, Dest, BackupNameTextBox.Text, BackupPlan.IncrementalBackup, ScheduleTypeComboBox.SelectedIndex)
            End If
        End If

        'Conclude Plan addition
        If wroteToFile Then
            MsgBox(selectedRadioButton.Text & " plan has been created!", vbOKOnly, "Backup Plan")
            Log("Wrote to Plans.json file succesfully")
        Else
            MsgBox("Failed to create Backup Plan: " & selectedRadioButton.Text, vbOKOnly, "Failed to Write")
            Log("Failed to write Plan object to Plans.json")
        End If

        PopulatePlansTable() 'RePopulating Plans Data table
    End Sub

    Private Sub BackupNameTextBox_TextChanged(sender As Object, e As EventArgs) Handles BackupNameTextBox.TextChanged
        BackupNameChosen = (BackupNameTextBox.Text <> String.Empty)
        ActivateBackupBtn()
    End Sub

    Private Sub ScheduleTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ScheduleTypeComboBox.SelectedIndexChanged
        ScheduleTypeChosen = (ScheduleTypeComboBox.SelectedItem.ToString() <> String.Empty)
        If ScheduleTypeComboBox.SelectedIndex = ScheduleType.Custom Then
            CustomizeScheduleGroupBox.Enabled = True
            ActivateBackupBtn()
            Return
        End If

        CustomizeScheduleGroupBox.Enabled = False 'Ensure that the groupbox is disabled if user didn't selected Custom
        ActivateBackupBtn()
    End Sub

    Private Sub OccurancesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles OccurancesComboBox.SelectedIndexChanged
        Dim SelectedItem = OccurancesComboBox.SelectedItem.ToString()

        If SelectedItem = "Daily" Then 'Select all checkboxes
            SunCheckBox.Checked = True
            MCheckbox.Checked = True
            TCheckBox.Checked = True
            WCheckbox.Checked = True
            ThCheckBox.Checked = True
            FCheckBox.Checked = True
            SatCheckBox.Checked = True

            SunCheckBox.Enabled = False
            MCheckbox.Enabled = False
            TCheckBox.Enabled = False
            WCheckbox.Enabled = False
            ThCheckBox.Enabled = False
            FCheckBox.Enabled = False
            SatCheckBox.Enabled = False
        ElseIf SelectedItem = "Weekly" Then 'Clear out the checkboxes and have the user choose
            SunCheckBox.Checked = False
            MCheckbox.Checked = False
            TCheckBox.Checked = False
            WCheckbox.Checked = False
            ThCheckBox.Checked = False
            FCheckBox.Checked = False
            SatCheckBox.Checked = False

            SunCheckBox.Enabled = True
            MCheckbox.Enabled = True
            TCheckBox.Enabled = True
            WCheckbox.Enabled = True
            ThCheckBox.Enabled = True
            FCheckBox.Enabled = True
            SatCheckBox.Enabled = True
            Select Case Today.DayOfWeek
                Case DayOfWeek.Sunday
                    SunCheckBox.Checked = True
                Case DayOfWeek.Monday
                    MCheckbox.Checked = True
                Case DayOfWeek.Tuesday
                    TCheckBox.Checked = True
                Case DayOfWeek.Wednesday
                    WCheckbox.Checked = True
                Case DayOfWeek.Thursday
                    ThCheckBox.Checked = True
                Case DayOfWeek.Friday
                    FCheckBox.Checked = True
                Case DayOfWeek.Saturday
                    SatCheckBox.Checked = True
            End Select
        End If

        ActivateBackupBtn()
    End Sub

    Private Sub CurrentPlanList_MouseDoubleClickEvent(sender As Object, e As DataGridViewCellMouseEventArgs) Handles CurrentPlanList.CellMouseDoubleClick
        'Prompt the user on whether or not they would want to modify the backup plan of their choosing 
        Dim PlanRowIndex = e.RowIndex
        If PlanRowIndex < 0 Then
            Return
        End If
        Dim PlanName = CurrentPlanList.Item(0, e.RowIndex).Value
        Dim PlanBackupType = CurrentPlanList.Item(5, e.RowIndex).Value
        Dim Response = MsgBox("Do you want to delete the following backup plan?" & vbCrLf & CurrentPlanList.Item(0, e.RowIndex).Value, vbYesNo, "Delete Backup Plan")

        If Response = vbYes Then
            'First, delete the Plan object from the Plans.json file
            Dim jsonManipObj As JsonManip = New JsonManip()
            jsonManipObj.SetJsonFile("Plans.json")
            jsonManipObj.DeletePlanObject(PlanRowIndex)

            'Second, discontinues backup plan task from the task scheduler
            DeletePlanTask(PlanName, BackupTypeToEnum(PlanBackupType))
            PopulatePlansTable() 'Populating Plans Data table'
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulatePlansTable() 'Populating Plans Data table'
        EndingBackupDateAndTimePicker.MinDate = Today.AddDays(1)
    End Sub

    Private Sub EndingBackupDateAndTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles EndingBackupDateAndTimePicker.ValueChanged
        EndBackupDate = EndingBackupDateAndTimePicker.Value
    End Sub
End Class
