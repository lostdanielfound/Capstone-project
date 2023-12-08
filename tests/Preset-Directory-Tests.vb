Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports BoxIT_Project
Imports Newtonsoft.Json
Imports NUnit.Framework

Namespace TestingEnvironment
    Public Class PresetTests
        Private JsonManipObj As JsonManip
        Private JsonManipLogObj As JsonManip
        Private ReadOnly JsonFileName = "Plans_UnitTest.json"
        Private ReadOnly JsonLogName = ".BoxITLog.json"

        Private EnvironmentDirectory = System.Environment.CurrentDirectory()
        Private CurrentDirectory = EnvironmentDirectory & "\..\..\..\test directories"

        Private ReadOnly FileName1 = "File1.txt"

        <SetUp>
        Public Sub Init()
            'Setting up JsonObject
            JsonManipObj = New JsonManip()
            JsonManipLogObj = New JsonManip()
        End Sub

        <Test>
        Public Sub FileCreate_Dir1_EqualTest()

            'Setup =========
            Dim SourceDirectory = CurrentDirectory & "\" & "Dir1"
            Dim DestinationDirectory = CurrentDirectory & "\" & "Dest"
            Dim ZipDirectoryPath = DestinationDirectory & "\" & "Archive.zip"
            Dim JsonLogPath = SourceDirectory & "\" & JsonLogName

            Directory.CreateDirectory(DestinationDirectory)
            ZipFile.CreateFromDirectory(SourceDirectory, ZipDirectoryPath, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(ZipDirectoryPath)
            'Setup =========

            JsonManipLogObj.SetJsonFile(JsonLogPath, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()
            Dim PreviousUpdateTime = Now
            Dim LogTree = JsonManipLogObj.ReadJarray()

            ' Modification of Source Tree ========
            Threading.Thread.Sleep(3000)
            Dim fs = File.Create(SourceDirectory & "\" & FileName1)
            fs.Close()
            ' Modification of Source Tree ========

            Dim CurrentDirectoryTree = JsonManipLogObj.DepthFirstTransversal(SourceDirectory) 'Get the current Directory tree

            ' Incremental Update Process ========
            DeleteSrcEntires(LogTree, SourceDirectory, ZipDirectoryPath)
            UpdateDstEntries(LogTree, CurrentDirectoryTree, PreviousUpdateTime, ZipDirectoryPath, SourceDirectory, "")
            ' Incremental Update Process ========

            Console.WriteLine(LogTree.ToString())

            ' TearDown ========
            File.Delete(SourceDirectory & "\" & FileName1)
            File.Delete(JsonLogPath)
            Directory.Delete(DestinationDirectory, True)
            ' TearDown ========
        End Sub

        <Test>
        Public Sub DirectoryCreate_Dir1_EqualTest()

        End Sub

        <Test>
        Public Sub FileModified_Dir1_EqualTest()

        End Sub

        <Test>
        Public Sub DirectoryModified_Dir1_EqualTest()

        End Sub

        <TearDown>
        Public Sub Teardown()

        End Sub
    End Class
End Namespace
