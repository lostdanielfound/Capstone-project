Imports System.IO
Imports System.IO.Compression

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

    Private Function BackupTypeToEnum(Str As String) As BackupPlan
        If Str.ToLower() = "Full Backup".ToLower() Then
            Return BackupPlan.FullBackup
        ElseIf Str.ToLower() = "Incremental".ToLower() Then
            Return BackupPlan.IncrementalBackup
        ElseIf Str.ToLower() = "Differential".ToLower() Then
            Return BackupPlan.DifferentialBackup
        End If
    End Function

    Public Sub SchedulePlanTask(Src As String, Dst As String, BackUpName As String, BackUpType As BackupPlan, ST As ScheduleType)
        Dim TaskName = "BoxIT_" & BackUpName
        Dim ScheduleT As String
        Dim cmdline As String
        Dim ExecutibleName = "Zip_Process.exe"
        Select Case BackUpType 'Append BackupType to the Task name
            Case BackupPlan.FullBackup
                TaskName &= "FullBackup"
            Case BackupPlan.IncrementalBackup
                TaskName &= "IncrementalBackup"
            Case BackupPlan.DifferentialBackup
                TaskName &= "DifferentialBackup"
            Case Else
        End Select

        Select Case ST
            Case ScheduleType.Daily
                ScheduleT = "daily"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & TaskName & " /tr " & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & " " & Src & " " & Dst & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Weekly
                ScheduleT = "weekly"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & TaskName & " /tr " & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & " " & Src & " " & Dst & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Monthly
                ScheduleT = "monthly"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & TaskName & " /tr " & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & " " & Src & " " & Dst & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
            Case ScheduleType.Yearly
                ScheduleT = "monthly"
                cmdline = "schtasks /create /sc " & ScheduleT & " /tn " & TaskName & " /tr " & Chr(34) & Environment.CurrentDirectory & "\" & ExecutibleName & " " & Src & " " & Dst & Chr(34)
                Log("Executing CMD scheduler command:" & cmdline)
                Shell(cmdline)
        End Select

    End Sub

    Private Sub DeletePlanTask(BackupName As String, BackupType As BackupPlan)
        Dim TaskName = "BoxIT_" & BackupName
        Dim cmdline As String
        Select Case BackupType 'Append BackupType to the Task name
            Case BackupPlan.FullBackup
                TaskName &= "FullBackup"
            Case BackupPlan.IncrementalBackup
                TaskName &= "IncrementalBackup"
            Case BackupPlan.DifferentialBackup
                TaskName &= "DifferentialBackup"
            Case Else
        End Select

        cmdline = "schtasks /delete /tn " & Chr(34) & TaskName & Chr(34) & " /f"
        Log("Executing CMD scheduler command:" & cmdline)
        Shell(cmdline)
    End Sub

    Private Sub PopulatePlansTable()
        Dim jsonManipObj As JsonManip = New JsonManip()
        jsonManipObj.SetJsonFile("Plans.json")

        For Each plan As Plan In jsonManipObj.ReadFromJsonFile()
            Dim newRow As New DataGridViewRow() 'Creating new row

            For index = 1 To 5 'Adding cells to row
                newRow.Cells.Add(New DataGridViewTextBoxCell())
            Next

            'populate cells
            newRow.Cells(0).Value = plan.Name
            newRow.Cells(1).Value = plan.Nxt_backup
            newRow.Cells(2).Value = plan.Src
            newRow.Cells(3).Value = plan.Dst
            Select Case plan.backupPlan
                Case BackupPlan.FullBackup
                    newRow.Cells(4).Value = "Full Backup"
                Case BackupPlan.IncrementalBackup
                    newRow.Cells(4).Value = "Incremental Backup"
                Case BackupPlan.DifferentialBackup
                    newRow.Cells(4).Value = "Differential Backup"
                Case Else
                    newRow.Cells(4).Value = "None"
            End Select

            CurrentPlanList.Rows.Add(newRow) 'Add the row to the current list
        Next
    End Sub

    '===================
    ' Form variables
    '===================
    Dim Src As String = String.Empty
    Dim Dest As String = "%userprofile\Documents\BoxIT_Backups" 'Default Backup destination

    'Flags for user progression
    Dim SrcChosen = False
    Dim DestChosen = False
    Dim ScheduleTypeChosen = False
    Dim BackupNameChosen = False

    '===================
    ' Event Handlers
    '===================
    Private Sub AddSourceBtnClick(sender As Object, e As EventArgs) Handles AddSourceBtn.Click

        'Bring up Dialog to select source
        Dim folderSelectionDig As New FolderBrowserDialog With {
            .ShowNewFolderButton = False
        }

        Log("Showing folder selection Dialog to select backup src")
        Dim ReturnResult As DialogResult = folderSelectionDig.ShowDialog()

        Select Case ReturnResult
            Case DialogResult.OK 'Set the backup source
                Src = folderSelectionDig.SelectedPath
                Label1.Text = folderSelectionDig.SelectedPath
                SrcChosen = True 'Set flag
                Log("Set Src as " & Src)

                AddDestBtn.Enabled = True
            Case DialogResult.Cancel 'Dialog failed, reset flag
                SrcChosen = False
                Label1.Text = "Please select a Source"
                Src = String.Empty
        End Select

        Backup_Plan_RadioSelection.Enabled = (SrcChosen And DestChosen)

        BackupBtn.Enabled = (SrcChosen And DestChosen And ScheduleTypeChosen And BackupNameChosen)
    End Sub

    Private Sub AddDestBtnClick(sender As Object, e As EventArgs) Handles AddDestBtn.Click

        'Bring up Dialog to select source
        Dim folderSelectionDig As New FolderBrowserDialog With {
            .ShowNewFolderButton = False
        }

        Dim ReturnResult = folderSelectionDig.ShowDialog()

        Select Case ReturnResult
            Case DialogResult.OK
                Dest = folderSelectionDig.SelectedPath
                Label2.Text = Dest
                DestChosen = True
                Log("Set Dest as " & Dest)
            Case DialogResult.Cancel 'Reset flag and Dest 
                DestChosen = False
                Label2.Text = "Please select a Destination"
                Dest = String.Empty
        End Select

        Backup_Plan_RadioSelection.Enabled = (SrcChosen And DestChosen)

        BackupBtn.Enabled = (SrcChosen And DestChosen And ScheduleTypeChosen And BackupNameChosen)
    End Sub

    Private Sub QuickBackupBtn_Click(sender As Object, e As EventArgs) Handles QuickBackupBtn.Click
        If Src = "" Or Dest = "" Then 'Check if Src and Dest have been selected 
            MsgBox("Please select a Source and Destination!", MsgBoxStyle.Information, "No Source or Destination")
            Return
        End If

        '2nd Obj: Create Zip file and store it within Dest directory
        Log("Compressing Src directory and storing it in " & Dest)
        Dim ZipUuid As String = Guid.NewGuid().ToString() 'Generating new uuid
        Log("Start Compression")
        ZipFile.CreateFromDirectory(Src, Dest & "\Backup_" & ZipUuid & ".zip", CompressionLevel.SmallestSize, False)
        Log("Finished Compression")
        MsgBox("Backup has been created!", MsgBoxStyle.OkOnly, "Backup Complete")
    End Sub

    Private Sub BackupBtn_Click(sender As Object, e As EventArgs) Handles BackupBtn.Click
        'Check to ensure that any folder / file is named the same as output Zip file
        'Ensure that User has selected a valid Src and Dest
        If Src = "" Or Dest = "" Then
            MsgBox("Please select a Source and Destination!", MsgBoxStyle.Information, "No Source or Destination")
            Return
        End If

        Dim jsonManipObj As JsonManip = New JsonManip()
        jsonManipObj.SetJsonFile("Plans.json")

        'Gets the current selected Radiobutton
        Dim selectedRadioButton As RadioButton = Backup_Plan_RadioSelection.Controls.OfType(Of RadioButton).FirstOrDefault(Function(r) r.Checked = True)

        If selectedRadioButton.Text = "Full Backup" Then
            SchedulePlanTask(Src, Dest, BackupNameTextBox.Text, BackupPlan.FullBackup, ScheduleTypeComboBox.SelectedIndex)
            jsonManipObj.WriteToJsonFile(BackupNameTextBox.Text, Today.AddDays(1).ToString(), Src, Dest, BackupPlan.FullBackup)
        ElseIf selectedRadioButton.Text = "Incremental" Then 'TODO: Add Incremental backup features
            SchedulePlanTask(Src, Dest, BackupNameTextBox.Text, BackupPlan.IncrementalBackup, ScheduleTypeComboBox.SelectedIndex)
            jsonManipObj.WriteToJsonFile(BackupNameTextBox.Text, "1/1/1900", Src, Dest, BackupPlan.IncrementalBackup)
        ElseIf selectedRadioButton.Text = "Differntial" Then 'TODO: Add Incremental backup features
            SchedulePlanTask(Src, Dest, BackupNameTextBox.Text, BackupPlan.DifferentialBackup, ScheduleTypeComboBox.SelectedIndex)
            jsonManipObj.WriteToJsonFile(BackupNameTextBox.Text, "1/1/1900", Src, Dest, BackupPlan.DifferentialBackup)
        End If

        'Conclude Plan addition
        MsgBox(selectedRadioButton.Text & " plan has been created!", vbOKOnly, "Backup Plan")
        PopulatePlansTable() 'RePopulating Plans Data table
    End Sub

    Private Sub BackupNameTextBox_TextChanged(sender As Object, e As EventArgs) Handles BackupNameTextBox.TextChanged
        BackupNameChosen = (BackupNameTextBox.Text <> String.Empty)
        BackupBtn.Enabled = (SrcChosen And DestChosen And ScheduleTypeChosen And BackupNameChosen)
        Log(SrcChosen.ToString() & DestChosen.ToString() & ScheduleTypeChosen.ToString() & BackupNameChosen.ToString())
    End Sub

    Private Sub ScheduleTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ScheduleTypeComboBox.SelectedIndexChanged
        ScheduleTypeChosen = (ScheduleTypeComboBox.SelectedItem.ToString() <> String.Empty)
        BackupBtn.Enabled = (SrcChosen And DestChosen And ScheduleTypeChosen And BackupNameChosen)
        Log(SrcChosen.ToString() & DestChosen.ToString() & ScheduleTypeChosen.ToString() & BackupNameChosen.ToString())
    End Sub

    Private Sub CurrentPlanList_MouseDoubleClickEvent(sender As Object, e As DataGridViewCellMouseEventArgs) Handles CurrentPlanList.CellMouseDoubleClick
        'Prompt the user on whether or not they would want to modify the backup plan of their choosing 
        Dim PlanRowIndex = e.RowIndex
        Dim PlanName = CurrentPlanList.Item(0, e.RowIndex).Value
        Dim PlanBackupType = CurrentPlanList.Item(4, e.RowIndex).Value
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
    End Sub

End Class
