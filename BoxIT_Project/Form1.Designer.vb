﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        AddSourceBtn = New Button()
        Label1 = New Label()
        Panel1 = New Panel()
        Label3 = New Label()
        ScheduleTypeComboBox = New ComboBox()
        QuickBackupBtn = New Button()
        Backup_Plan_RadioSelection = New GroupBox()
        RadioButton1 = New RadioButton()
        RadioButton2 = New RadioButton()
        RadioButton3 = New RadioButton()
        BackupNameTextBox = New TextBox()
        Label4 = New Label()
        CurrentPlanList = New DataGridView()
        CurrentPlanList_Name = New DataGridViewTextBoxColumn()
        CurrentPlanList_NxtBackup = New DataGridViewTextBoxColumn()
        CurrentPlanList_Source = New DataGridViewTextBoxColumn()
        CurrentPlanList_Dest = New DataGridViewTextBoxColumn()
        CurrentPlanList_Plan = New DataGridViewTextBoxColumn()
        BackupBtn = New Button()
        Label2 = New Label()
        AddDestBtn = New Button()
        Panel1.SuspendLayout()
        Backup_Plan_RadioSelection.SuspendLayout()
        CType(CurrentPlanList, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' AddSourceBtn
        ' 
        AddSourceBtn.Location = New Point(3, 3)
        AddSourceBtn.Name = "AddSourceBtn"
        AddSourceBtn.Size = New Size(124, 23)
        AddSourceBtn.TabIndex = 0
        AddSourceBtn.Text = "+ Add Source"
        AddSourceBtn.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Quicksand", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Label1.Location = New Point(133, 8)
        Label1.Name = "Label1"
        Label1.Size = New Size(133, 18)
        Label1.TabIndex = 1
        Label1.Text = "Please select a Source"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(ScheduleTypeComboBox)
        Panel1.Controls.Add(QuickBackupBtn)
        Panel1.Controls.Add(Backup_Plan_RadioSelection)
        Panel1.Controls.Add(BackupNameTextBox)
        Panel1.Controls.Add(Label4)
        Panel1.Controls.Add(CurrentPlanList)
        Panel1.Controls.Add(BackupBtn)
        Panel1.Controls.Add(Label2)
        Panel1.Controls.Add(AddDestBtn)
        Panel1.Controls.Add(AddSourceBtn)
        Panel1.Controls.Add(Label1)
        Panel1.Font = New Font("Quicksand", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(803, 383)
        Panel1.TabIndex = 2
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(190, 61)
        Label3.Name = "Label3"
        Label3.Size = New Size(123, 18)
        Label3.TabIndex = 15
        Label3.Text = "Select schedule type"
        ' 
        ' ScheduleTypeComboBox
        ' 
        ScheduleTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        ScheduleTypeComboBox.FormattingEnabled = True
        ScheduleTypeComboBox.Items.AddRange(New Object() {"Daily", "Weekly", "Monthly", "Yearly"})
        ScheduleTypeComboBox.Location = New Point(190, 82)
        ScheduleTypeComboBox.Name = "ScheduleTypeComboBox"
        ScheduleTypeComboBox.Size = New Size(123, 26)
        ScheduleTypeComboBox.TabIndex = 14
        ' 
        ' QuickBackupBtn
        ' 
        QuickBackupBtn.Enabled = False
        QuickBackupBtn.Location = New Point(658, 110)
        QuickBackupBtn.Name = "QuickBackupBtn"
        QuickBackupBtn.Size = New Size(139, 31)
        QuickBackupBtn.TabIndex = 13
        QuickBackupBtn.Text = "Quick Backup"
        QuickBackupBtn.UseVisualStyleBackColor = True
        ' 
        ' Backup_Plan_RadioSelection
        ' 
        Backup_Plan_RadioSelection.Controls.Add(RadioButton1)
        Backup_Plan_RadioSelection.Controls.Add(RadioButton2)
        Backup_Plan_RadioSelection.Controls.Add(RadioButton3)
        Backup_Plan_RadioSelection.Enabled = False
        Backup_Plan_RadioSelection.Location = New Point(4, 61)
        Backup_Plan_RadioSelection.Name = "Backup_Plan_RadioSelection"
        Backup_Plan_RadioSelection.Size = New Size(180, 117)
        Backup_Plan_RadioSelection.TabIndex = 12
        Backup_Plan_RadioSelection.TabStop = False
        Backup_Plan_RadioSelection.Text = "Select a backup Plan"
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(15, 21)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(90, 22)
        RadioButton1.TabIndex = 5
        RadioButton1.TabStop = True
        RadioButton1.Text = "Full Backup"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.BackgroundImageLayout = ImageLayout.Center
        RadioButton2.Location = New Point(15, 49)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(93, 22)
        RadioButton2.TabIndex = 6
        RadioButton2.Text = "Incremental"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' RadioButton3
        ' 
        RadioButton3.AutoSize = True
        RadioButton3.Location = New Point(15, 77)
        RadioButton3.Name = "RadioButton3"
        RadioButton3.Size = New Size(82, 22)
        RadioButton3.TabIndex = 7
        RadioButton3.Text = "Differntial"
        RadioButton3.UseVisualStyleBackColor = True
        ' 
        ' BackupNameTextBox
        ' 
        BackupNameTextBox.Location = New Point(190, 144)
        BackupNameTextBox.Name = "BackupNameTextBox"
        BackupNameTextBox.Size = New Size(167, 22)
        BackupNameTextBox.TabIndex = 11
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(190, 123)
        Label4.Name = "Label4"
        Label4.Size = New Size(102, 18)
        Label4.TabIndex = 10
        Label4.Text = "Name of Backup:"
        ' 
        ' CurrentPlanList
        ' 
        CurrentPlanList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        CurrentPlanList.Columns.AddRange(New DataGridViewColumn() {CurrentPlanList_Name, CurrentPlanList_NxtBackup, CurrentPlanList_Source, CurrentPlanList_Dest, CurrentPlanList_Plan})
        CurrentPlanList.Location = New Point(3, 184)
        CurrentPlanList.Name = "CurrentPlanList"
        CurrentPlanList.RowTemplate.Height = 25
        CurrentPlanList.Size = New Size(794, 190)
        CurrentPlanList.TabIndex = 9
        ' 
        ' CurrentPlanList_Name
        ' 
        CurrentPlanList_Name.HeaderText = "Name"
        CurrentPlanList_Name.Name = "CurrentPlanList_Name"
        CurrentPlanList_Name.ReadOnly = True
        CurrentPlanList_Name.Width = 150
        ' 
        ' CurrentPlanList_NxtBackup
        ' 
        CurrentPlanList_NxtBackup.HeaderText = "Next Backup"
        CurrentPlanList_NxtBackup.Name = "CurrentPlanList_NxtBackup"
        CurrentPlanList_NxtBackup.ReadOnly = True
        ' 
        ' CurrentPlanList_Source
        ' 
        CurrentPlanList_Source.HeaderText = "Source"
        CurrentPlanList_Source.Name = "CurrentPlanList_Source"
        CurrentPlanList_Source.ReadOnly = True
        CurrentPlanList_Source.Width = 200
        ' 
        ' CurrentPlanList_Dest
        ' 
        CurrentPlanList_Dest.HeaderText = "Destination"
        CurrentPlanList_Dest.Name = "CurrentPlanList_Dest"
        CurrentPlanList_Dest.ReadOnly = True
        CurrentPlanList_Dest.Width = 200
        ' 
        ' CurrentPlanList_Plan
        ' 
        CurrentPlanList_Plan.HeaderText = "Plan"
        CurrentPlanList_Plan.Name = "CurrentPlanList_Plan"
        CurrentPlanList_Plan.ReadOnly = True
        ' 
        ' BackupBtn
        ' 
        BackupBtn.Enabled = False
        BackupBtn.Location = New Point(658, 147)
        BackupBtn.Name = "BackupBtn"
        BackupBtn.Size = New Size(139, 31)
        BackupBtn.TabIndex = 4
        BackupBtn.Text = "Start Backup Plan"
        BackupBtn.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Quicksand", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Label2.Location = New Point(133, 37)
        Label2.Name = "Label2"
        Label2.Size = New Size(158, 18)
        Label2.TabIndex = 3
        Label2.Text = "Please select a Destination"
        ' 
        ' AddDestBtn
        ' 
        AddDestBtn.Enabled = False
        AddDestBtn.Location = New Point(3, 32)
        AddDestBtn.Name = "AddDestBtn"
        AddDestBtn.Size = New Size(124, 23)
        AddDestBtn.TabIndex = 2
        AddDestBtn.Text = "+ Add Destination"
        AddDestBtn.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSize = True
        ClientSize = New Size(829, 402)
        Controls.Add(Panel1)
        MaximizeBox = False
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "BoxIt"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Backup_Plan_RadioSelection.ResumeLayout(False)
        Backup_Plan_RadioSelection.PerformLayout()
        CType(CurrentPlanList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents AddSourceBtn As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents AddDestBtn As Button
    Friend WithEvents BackupBtn As Button
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents CurrentPlanList As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents BackupNameTextBox As TextBox
    Friend WithEvents Backup_Plan_RadioSelection As GroupBox
    Friend WithEvents QuickBackupBtn As Button
    Friend WithEvents CurrentPlanList_Name As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_NxtBackup As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_Source As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_Dest As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_Plan As DataGridViewTextBoxColumn
    Friend WithEvents ScheduleTypeComboBox As ComboBox
    Friend WithEvents Label3 As Label
End Class
