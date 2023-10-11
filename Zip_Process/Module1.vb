Imports System.IO.Compression

Module Module1
    Sub Main()
        Dim args = Environment.GetCommandLineArgs() 'Get command line args
        Dim ZipUuid As String = Guid.NewGuid().ToString() 'Generating new uuid 
        ZipFile.CreateFromDirectory(args(1), args(2) & "\Backup_" & ZipUuid & ".zip", CompressionLevel.Fastest, False) 'Create Fullback up
    End Sub
End Module