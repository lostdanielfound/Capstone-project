Imports System
Imports System.IO
Imports System.IO.Compression
Module Module1

    Sub Compression_test() 'Compresses a large folder
        Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\stuff"
        Dim zipUUID As String = Guid.NewGuid().ToString()
        Dim zipPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Desktop\stuff_" & zipUUID & ".zip"

        ZipFile.CreateFromDirectory(path, zipPath, CompressionLevel.Optimal, False)
    End Sub

    Sub Add_to_Compression() 'Adds to the large compressed .zip 
        Dim userdir As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)

        Using fs As New FileStream(userdir & "\Desktop\stuff_5cfbaa1a-40e0-448a-91ff-76048b27f742.zip", FileMode.Open) 'Opens zip folder stream
            Using archive As New ZipArchive(fs, ZipArchiveMode.Create) 'Using ZipArchive to model zip folder
                archive.CreateEntry("NewFile.txt") 'Create a new entry within the zip folder
            End Using
        End Using

    End Sub

    'NOTE - ZipArchive needed to be added manually through adding the System.IO.Compression .dll file through "Add references" 
    'More can be found through here: https://stackoverflow.com/questions/24308934/compression-ziparchive-wont-compile
    Sub Main()
        'Compression_test() 'This took roughly 4 minutes to compression a 4.95gb folder into a 4.89gb folder with optimal compression

        'If we want to add to a compressed folder, we don't want to extract the folder and add and then compress, we need to do better 
        ' Add_to_Compression() This failed if the .zip file is over 4gb
    End Sub

End Module
