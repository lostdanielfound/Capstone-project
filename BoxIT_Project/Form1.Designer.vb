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
        AddBackupPlanBtn = New Button()
        Label1 = New Label()
        Panel1 = New Panel()
        Label2 = New Label()
        AddDestBtn = New Button()
        BackupBtn = New Button()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' AddBackupPlanBtn
        ' 
        AddBackupPlanBtn.Location = New Point(3, 3)
        AddBackupPlanBtn.Name = "AddBackupPlanBtn"
        AddBackupPlanBtn.Size = New Size(139, 23)
        AddBackupPlanBtn.TabIndex = 0
        AddBackupPlanBtn.Text = "+ Add Backup Plan"
        AddBackupPlanBtn.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(148, 7)
        Label1.Name = "Label1"
        Label1.Size = New Size(172, 15)
        Label1.TabIndex = 1
        Label1.Text = "Please add plan to show source"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(BackupBtn)
        Panel1.Controls.Add(Label2)
        Panel1.Controls.Add(AddDestBtn)
        Panel1.Controls.Add(AddBackupPlanBtn)
        Panel1.Controls.Add(Label1)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(776, 209)
        Panel1.TabIndex = 2
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(148, 36)
        Label2.Name = "Label2"
        Label2.Size = New Size(114, 15)
        Label2.TabIndex = 3
        Label2.Text = "Default Destination: "
        ' 
        ' AddDestBtn
        ' 
        AddDestBtn.Enabled = False
        AddDestBtn.Location = New Point(3, 32)
        AddDestBtn.Name = "AddDestBtn"
        AddDestBtn.Size = New Size(139, 23)
        AddDestBtn.TabIndex = 2
        AddDestBtn.Text = "+ Add Destination"
        AddDestBtn.UseVisualStyleBackColor = True
        ' 
        ' BackupBtn
        ' 
        BackupBtn.Enabled = False
        BackupBtn.Location = New Point(3, 183)
        BackupBtn.Name = "BackupBtn"
        BackupBtn.Size = New Size(139, 23)
        BackupBtn.TabIndex = 4
        BackupBtn.Text = "Start Backup"
        BackupBtn.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(Panel1)
        Name = "Form1"
        Text = "Form1"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents AddBackupPlanBtn As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents AddDestBtn As Button
    Friend WithEvents BackupBtn As Button
End Class
