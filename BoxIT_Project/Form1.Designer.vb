<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(Form1))
        Panel1 = New Panel()
        GroupBox2 = New GroupBox()
        Clear1Btn = New Button()
        DestinationTextBox = New TextBox()
        SourceTextBox = New TextBox()
        AddSourceBtn = New Button()
        AddDestBtn = New Button()
        Clear2Btn = New Button()
        Label8 = New Label()
        CustomizeScheduleGroupBox = New GroupBox()
        Label7 = New Label()
        RepeatGroupBox = New GroupBox()
        SunCheckBox = New CheckBox()
        SatCheckBox = New CheckBox()
        FCheckBox = New CheckBox()
        ThCheckBox = New CheckBox()
        WCheckbox = New CheckBox()
        TCheckBox = New CheckBox()
        MCheckbox = New CheckBox()
        OccurancesComboBox = New ComboBox()
        EndingBackupDateAndTimePicker = New DateTimePicker()
        BackupBtn = New Button()
        ScheduleTypeComboBox = New ComboBox()
        BackupNameTextBox = New TextBox()
        Label3 = New Label()
        Label4 = New Label()
        Backup_Plan_RadioSelection = New GroupBox()
        RadioButton1 = New RadioButton()
        RadioButton2 = New RadioButton()
        CurrentPlanList = New DataGridView()
        CurrentPlanList_Name = New DataGridViewTextBoxColumn()
        CurrentPlanList_NxtBackup = New DataGridViewTextBoxColumn()
        CurrentPlanList_PreviousBackup = New DataGridViewTextBoxColumn()
        CurrentPlanList_Source = New DataGridViewTextBoxColumn()
        CurrentPlanList_Dest = New DataGridViewTextBoxColumn()
        CurrentPlanList_Plan = New DataGridViewTextBoxColumn()
        Panel5 = New Panel()
        EnableEndBackupDate = New CheckBox()
        Panel1.SuspendLayout()
        GroupBox2.SuspendLayout()
        CustomizeScheduleGroupBox.SuspendLayout()
        RepeatGroupBox.SuspendLayout()
        Backup_Plan_RadioSelection.SuspendLayout()
        CType(CurrentPlanList, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Panel1.BackColor = Color.White
        Panel1.Controls.Add(GroupBox2)
        Panel1.Controls.Add(Label8)
        Panel1.Controls.Add(CustomizeScheduleGroupBox)
        Panel1.Controls.Add(BackupBtn)
        Panel1.Controls.Add(ScheduleTypeComboBox)
        Panel1.Controls.Add(BackupNameTextBox)
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(Label4)
        Panel1.Controls.Add(Backup_Plan_RadioSelection)
        Panel1.Controls.Add(CurrentPlanList)
        Panel1.Font = New Font("Quicksand", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(549, 630)
        Panel1.TabIndex = 2
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(Clear1Btn)
        GroupBox2.Controls.Add(DestinationTextBox)
        GroupBox2.Controls.Add(SourceTextBox)
        GroupBox2.Controls.Add(AddSourceBtn)
        GroupBox2.Controls.Add(AddDestBtn)
        GroupBox2.Controls.Add(Clear2Btn)
        GroupBox2.Location = New Point(6, 3)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(537, 176)
        GroupBox2.TabIndex = 18
        GroupBox2.TabStop = False
        GroupBox2.Text = "Backup"
        ' 
        ' Clear1Btn
        ' 
        Clear1Btn.BackColor = Color.FromArgb(CByte(208), CByte(209), CByte(201))
        Clear1Btn.FlatStyle = FlatStyle.Flat
        Clear1Btn.Font = New Font("Quicksand", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        Clear1Btn.Location = New Point(459, 50)
        Clear1Btn.Name = "Clear1Btn"
        Clear1Btn.Size = New Size(75, 32)
        Clear1Btn.TabIndex = 11
        Clear1Btn.Text = "Clear"
        Clear1Btn.UseVisualStyleBackColor = False
        ' 
        ' DestinationTextBox
        ' 
        DestinationTextBox.BackColor = Color.White
        DestinationTextBox.Enabled = False
        DestinationTextBox.Location = New Point(9, 136)
        DestinationTextBox.Name = "DestinationTextBox"
        DestinationTextBox.Size = New Size(444, 24)
        DestinationTextBox.TabIndex = 9
        DestinationTextBox.Text = "Please select a Destination"
        DestinationTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' SourceTextBox
        ' 
        SourceTextBox.BackColor = Color.White
        SourceTextBox.Enabled = False
        SourceTextBox.Location = New Point(8, 58)
        SourceTextBox.Name = "SourceTextBox"
        SourceTextBox.Size = New Size(445, 24)
        SourceTextBox.TabIndex = 8
        SourceTextBox.Text = "Please select a Source"
        SourceTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' AddSourceBtn
        ' 
        AddSourceBtn.BackColor = Color.FromArgb(CByte(208), CByte(209), CByte(201))
        AddSourceBtn.FlatStyle = FlatStyle.Flat
        AddSourceBtn.Location = New Point(8, 23)
        AddSourceBtn.Name = "AddSourceBtn"
        AddSourceBtn.Size = New Size(165, 29)
        AddSourceBtn.TabIndex = 6
        AddSourceBtn.Text = "+ Add Source"
        AddSourceBtn.UseVisualStyleBackColor = False
        ' 
        ' AddDestBtn
        ' 
        AddDestBtn.BackColor = Color.FromArgb(CByte(208), CByte(209), CByte(201))
        AddDestBtn.FlatStyle = FlatStyle.Flat
        AddDestBtn.Location = New Point(9, 100)
        AddDestBtn.Name = "AddDestBtn"
        AddDestBtn.Size = New Size(164, 29)
        AddDestBtn.TabIndex = 7
        AddDestBtn.Text = "+ Add Destination"
        AddDestBtn.UseVisualStyleBackColor = False
        ' 
        ' Clear2Btn
        ' 
        Clear2Btn.BackColor = Color.FromArgb(CByte(208), CByte(209), CByte(201))
        Clear2Btn.FlatStyle = FlatStyle.Flat
        Clear2Btn.Font = New Font("Quicksand", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        Clear2Btn.Location = New Point(459, 128)
        Clear2Btn.Name = "Clear2Btn"
        Clear2Btn.Size = New Size(75, 32)
        Clear2Btn.TabIndex = 10
        Clear2Btn.Text = "Clear"
        Clear2Btn.UseVisualStyleBackColor = False
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(3, 17)
        Label8.Name = "Label8"
        Label8.Size = New Size(0, 19)
        Label8.TabIndex = 17
        ' 
        ' CustomizeScheduleGroupBox
        ' 
        CustomizeScheduleGroupBox.Controls.Add(EnableEndBackupDate)
        CustomizeScheduleGroupBox.Controls.Add(Label7)
        CustomizeScheduleGroupBox.Controls.Add(RepeatGroupBox)
        CustomizeScheduleGroupBox.Controls.Add(OccurancesComboBox)
        CustomizeScheduleGroupBox.Controls.Add(EndingBackupDateAndTimePicker)
        CustomizeScheduleGroupBox.Enabled = False
        CustomizeScheduleGroupBox.Location = New Point(247, 185)
        CustomizeScheduleGroupBox.Name = "CustomizeScheduleGroupBox"
        CustomizeScheduleGroupBox.Size = New Size(296, 245)
        CustomizeScheduleGroupBox.TabIndex = 16
        CustomizeScheduleGroupBox.TabStop = False
        CustomizeScheduleGroupBox.Text = "Schedule Customization"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(19, 25)
        Label7.Name = "Label7"
        Label7.Size = New Size(81, 19)
        Label7.TabIndex = 6
        Label7.Text = "Occurances"
        ' 
        ' RepeatGroupBox
        ' 
        RepeatGroupBox.Controls.Add(SunCheckBox)
        RepeatGroupBox.Controls.Add(SatCheckBox)
        RepeatGroupBox.Controls.Add(FCheckBox)
        RepeatGroupBox.Controls.Add(ThCheckBox)
        RepeatGroupBox.Controls.Add(WCheckbox)
        RepeatGroupBox.Controls.Add(TCheckBox)
        RepeatGroupBox.Controls.Add(MCheckbox)
        RepeatGroupBox.Location = New Point(20, 80)
        RepeatGroupBox.Name = "RepeatGroupBox"
        RepeatGroupBox.Size = New Size(240, 80)
        RepeatGroupBox.TabIndex = 4
        RepeatGroupBox.TabStop = False
        RepeatGroupBox.Text = "Repeats"
        ' 
        ' SunCheckBox
        ' 
        SunCheckBox.AutoSize = True
        SunCheckBox.Location = New Point(17, 23)
        SunCheckBox.Name = "SunCheckBox"
        SunCheckBox.Size = New Size(35, 23)
        SunCheckBox.TabIndex = 6
        SunCheckBox.Text = "S"
        SunCheckBox.UseVisualStyleBackColor = True
        ' 
        ' SatCheckBox
        ' 
        SatCheckBox.AutoSize = True
        SatCheckBox.Location = New Point(196, 22)
        SatCheckBox.Name = "SatCheckBox"
        SatCheckBox.Size = New Size(35, 23)
        SatCheckBox.TabIndex = 5
        SatCheckBox.Text = "S"
        SatCheckBox.UseVisualStyleBackColor = True
        ' 
        ' FCheckBox
        ' 
        FCheckBox.AutoSize = True
        FCheckBox.Location = New Point(165, 51)
        FCheckBox.Name = "FCheckBox"
        FCheckBox.Size = New Size(35, 23)
        FCheckBox.TabIndex = 4
        FCheckBox.Text = "F"
        FCheckBox.UseVisualStyleBackColor = True
        ' 
        ' ThCheckBox
        ' 
        ThCheckBox.AutoSize = True
        ThCheckBox.Location = New Point(137, 23)
        ThCheckBox.Name = "ThCheckBox"
        ThCheckBox.Size = New Size(43, 23)
        ThCheckBox.TabIndex = 3
        ThCheckBox.Text = "Th"
        ThCheckBox.UseVisualStyleBackColor = True
        ' 
        ' WCheckbox
        ' 
        WCheckbox.AutoSize = True
        WCheckbox.Location = New Point(102, 51)
        WCheckbox.Name = "WCheckbox"
        WCheckbox.Size = New Size(41, 23)
        WCheckbox.TabIndex = 2
        WCheckbox.Text = "W"
        WCheckbox.UseVisualStyleBackColor = True
        ' 
        ' TCheckBox
        ' 
        TCheckBox.AutoSize = True
        TCheckBox.Location = New Point(75, 23)
        TCheckBox.Name = "TCheckBox"
        TCheckBox.Size = New Size(36, 23)
        TCheckBox.TabIndex = 1
        TCheckBox.Text = "T"
        TCheckBox.UseVisualStyleBackColor = True
        ' 
        ' MCheckbox
        ' 
        MCheckbox.AutoSize = True
        MCheckbox.Location = New Point(42, 51)
        MCheckbox.Name = "MCheckbox"
        MCheckbox.Size = New Size(39, 23)
        MCheckbox.TabIndex = 0
        MCheckbox.Text = "M"
        MCheckbox.UseVisualStyleBackColor = True
        ' 
        ' OccurancesComboBox
        ' 
        OccurancesComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        OccurancesComboBox.FormattingEnabled = True
        OccurancesComboBox.Items.AddRange(New Object() {"Daily", "Weekly"})
        OccurancesComboBox.Location = New Point(20, 47)
        OccurancesComboBox.Name = "OccurancesComboBox"
        OccurancesComboBox.Size = New Size(121, 27)
        OccurancesComboBox.TabIndex = 5
        ' 
        ' EndingBackupDateAndTimePicker
        ' 
        EndingBackupDateAndTimePicker.Location = New Point(19, 206)
        EndingBackupDateAndTimePicker.Margin = New Padding(3, 4, 3, 4)
        EndingBackupDateAndTimePicker.MinDate = New Date(2023, 11, 28, 20, 56, 31, 0)
        EndingBackupDateAndTimePicker.Name = "EndingBackupDateAndTimePicker"
        EndingBackupDateAndTimePicker.Size = New Size(226, 24)
        EndingBackupDateAndTimePicker.TabIndex = 2
        EndingBackupDateAndTimePicker.Value = New Date(2023, 11, 30, 0, 0, 0, 0)
        ' 
        ' BackupBtn
        ' 
        BackupBtn.BackColor = Color.FromArgb(CByte(208), CByte(209), CByte(201))
        BackupBtn.Enabled = False
        BackupBtn.FlatStyle = FlatStyle.Flat
        BackupBtn.ForeColor = Color.Black
        BackupBtn.Location = New Point(13, 384)
        BackupBtn.Name = "BackupBtn"
        BackupBtn.Size = New Size(220, 31)
        BackupBtn.TabIndex = 4
        BackupBtn.Text = "Start Backup Plan"
        BackupBtn.UseVisualStyleBackColor = False
        ' 
        ' ScheduleTypeComboBox
        ' 
        ScheduleTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        ScheduleTypeComboBox.FormattingEnabled = True
        ScheduleTypeComboBox.Items.AddRange(New Object() {"Daily", "Weekly", "Monthly", "Yearly", "Custom"})
        ScheduleTypeComboBox.Location = New Point(13, 288)
        ScheduleTypeComboBox.Name = "ScheduleTypeComboBox"
        ScheduleTypeComboBox.Size = New Size(141, 27)
        ScheduleTypeComboBox.TabIndex = 14
        ' 
        ' BackupNameTextBox
        ' 
        BackupNameTextBox.Location = New Point(13, 352)
        BackupNameTextBox.Name = "BackupNameTextBox"
        BackupNameTextBox.Size = New Size(220, 24)
        BackupNameTextBox.TabIndex = 11
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(15, 266)
        Label3.Name = "Label3"
        Label3.Size = New Size(131, 19)
        Label3.TabIndex = 15
        Label3.Text = "Select Schedule type"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(14, 330)
        Label4.Name = "Label4"
        Label4.Size = New Size(113, 19)
        Label4.TabIndex = 10
        Label4.Text = "Name of Backup:"
        ' 
        ' Backup_Plan_RadioSelection
        ' 
        Backup_Plan_RadioSelection.Controls.Add(RadioButton1)
        Backup_Plan_RadioSelection.Controls.Add(RadioButton2)
        Backup_Plan_RadioSelection.Enabled = False
        Backup_Plan_RadioSelection.Location = New Point(6, 185)
        Backup_Plan_RadioSelection.Name = "Backup_Plan_RadioSelection"
        Backup_Plan_RadioSelection.Padding = New Padding(5, 0, 0, 0)
        Backup_Plan_RadioSelection.Size = New Size(235, 64)
        Backup_Plan_RadioSelection.TabIndex = 12
        Backup_Plan_RadioSelection.TabStop = False
        Backup_Plan_RadioSelection.Text = "Select a Backup Type"
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(8, 26)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(96, 23)
        RadioButton1.TabIndex = 5
        RadioButton1.TabStop = True
        RadioButton1.Text = "Full Backup"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.BackgroundImageLayout = ImageLayout.Center
        RadioButton2.Location = New Point(121, 26)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(99, 23)
        RadioButton2.TabIndex = 6
        RadioButton2.Text = "Incremental"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' CurrentPlanList
        ' 
        CurrentPlanList.BackgroundColor = SystemColors.ControlDarkDark
        CurrentPlanList.BorderStyle = BorderStyle.Fixed3D
        CurrentPlanList.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = Color.FromArgb(CByte(20), CByte(25), CByte(70))
        DataGridViewCellStyle1.Font = New Font("Quicksand", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle1.ForeColor = Color.WhiteSmoke
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        CurrentPlanList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        CurrentPlanList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        CurrentPlanList.Columns.AddRange(New DataGridViewColumn() {CurrentPlanList_Name, CurrentPlanList_NxtBackup, CurrentPlanList_PreviousBackup, CurrentPlanList_Source, CurrentPlanList_Dest, CurrentPlanList_Plan})
        CurrentPlanList.Cursor = Cursors.Hand
        CurrentPlanList.Location = New Point(3, 436)
        CurrentPlanList.Name = "CurrentPlanList"
        CurrentPlanList.RowTemplate.Height = 25
        CurrentPlanList.Size = New Size(543, 190)
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
        ' CurrentPlanList_PreviousBackup
        ' 
        CurrentPlanList_PreviousBackup.HeaderText = "Previous Backup"
        CurrentPlanList_PreviousBackup.Name = "CurrentPlanList_PreviousBackup"
        CurrentPlanList_PreviousBackup.ReadOnly = True
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
        ' Panel5
        ' 
        Panel5.BackColor = Color.FromArgb(CByte(157), CByte(34), CByte(53))
        Panel5.Dock = DockStyle.Fill
        Panel5.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Panel5.Location = New Point(0, 0)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(573, 654)
        Panel5.TabIndex = 3
        ' 
        ' EnableEndBackupDate
        ' 
        EnableEndBackupDate.AutoSize = True
        EnableEndBackupDate.Location = New Point(19, 176)
        EnableEndBackupDate.Name = "EnableEndBackupDate"
        EnableEndBackupDate.Size = New Size(173, 23)
        EnableEndBackupDate.TabIndex = 7
        EnableEndBackupDate.Text = "Set Ending Backup Date"
        EnableEndBackupDate.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSize = True
        ClientSize = New Size(573, 654)
        Controls.Add(Panel1)
        Controls.Add(Panel5)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "BoxIt"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        CustomizeScheduleGroupBox.ResumeLayout(False)
        CustomizeScheduleGroupBox.PerformLayout()
        RepeatGroupBox.ResumeLayout(False)
        RepeatGroupBox.PerformLayout()
        Backup_Plan_RadioSelection.ResumeLayout(False)
        Backup_Plan_RadioSelection.PerformLayout()
        CType(CurrentPlanList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents BackupBtn As Button
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents CurrentPlanList As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents BackupNameTextBox As TextBox
    Friend WithEvents Backup_Plan_RadioSelection As GroupBox
    Friend WithEvents ScheduleTypeComboBox As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents CurrentPlanList_Name As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_NxtBackup As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_PreviousBackup As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_Source As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_Dest As DataGridViewTextBoxColumn
    Friend WithEvents CurrentPlanList_Plan As DataGridViewTextBoxColumn
    Friend WithEvents Panel5 As Panel
    Friend WithEvents CustomizeScheduleGroupBox As GroupBox
    Friend WithEvents RepeatGroupBox As GroupBox
    Friend WithEvents SunCheckBox As CheckBox
    Friend WithEvents SatCheckBox As CheckBox
    Friend WithEvents FCheckBox As CheckBox
    Friend WithEvents ThCheckBox As CheckBox
    Friend WithEvents WCheckbox As CheckBox
    Friend WithEvents TCheckBox As CheckBox
    Friend WithEvents MCheckbox As CheckBox
    Friend WithEvents EndingBackupDateAndTimePicker As DateTimePicker
    Friend WithEvents Label7 As Label
    Friend WithEvents OccurancesComboBox As ComboBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Clear1Btn As Button
    Friend WithEvents DestinationTextBox As TextBox
    Friend WithEvents SourceTextBox As TextBox
    Friend WithEvents AddSourceBtn As Button
    Friend WithEvents AddDestBtn As Button
    Friend WithEvents Clear2Btn As Button
    Friend WithEvents EnableEndBackupDate As CheckBox
End Class
