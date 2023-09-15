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

    '===================
    ' Form variables
    '===================
    Dim Src As String
    Dim Dest As String = "%userprofile\Documents\BoxIT_Backups" 'Default Backup destination
    Dim SrcChosen As Boolean = False
    Dim DestChosen As Boolean = False
    Dim DefaultSelection As Boolean = True

    '===================
    ' Event Handlers
    '===================
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles AddBackupPlanBtn.Click
        Dim folderSelectionDig As New FolderBrowserDialog
        folderSelectionDig.ShowNewFolderButton = False

        Log("Showing folder selection Dialog to select backup src")
        Dim ReturnResult As DialogResult = folderSelectionDig.ShowDialog()

        Select Case ReturnResult
            Case DialogResult.OK 'Set the backup source
                Src = folderSelectionDig.SelectedPath
                Label1.Text = folderSelectionDig.SelectedPath
                SrcChosen = True
                Log("Set Src as " & Src)

                AddDestBtn.Enabled = True
            Case DialogResult.Cancel 'Dialog failed, disable AddDestBtn if Src is empty
                AddDestBtn.Enabled = (Src <> "")
        End Select
    End Sub

    Private Sub AddDestBtnClick(sender As Object, e As EventArgs) Handles AddDestBtn.Click
        Dim folderSelectionDig As New FolderBrowserDialog
        folderSelectionDig.ShowNewFolderButton = False

        If folderSelectionDig.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dest = folderSelectionDig.SelectedPath
            Label2.Text = Dest
            DestChosen = True

            Log("Set Dest as " & Dest)

            'Unlock Start Backup Button after user has selected Src and Dest
            If SrcChosen And DestChosen Then
                BackupBtn.Enabled = True
            End If
        End If
    End Sub

    Private Sub BackupBtn_Click(sender As Object, e As EventArgs) Handles BackupBtn.Click
        '1st Obj: Perform token reading,
        'Check to ensure that any folder / file is named the same as output Zip file
        ReadDirectory(Dest)

        '2nd Obj: Create Zip file and store it within Dest directory
        Log("Compressing Src directory and storing it in " & Dest)
        ZipFile.CreateFromDirectory(Src, Dest & "\Backup.zip", CompressionLevel.SmallestSize, False)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = ""
    End Sub
End Class
