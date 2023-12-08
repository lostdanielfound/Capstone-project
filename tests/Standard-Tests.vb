Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
Imports BoxIT_Project
Imports Newtonsoft.Json
Imports NUnit.Framework

Namespace TestingEnvironment

    Public Class StandardTests
        Private JsonManipObj As JsonManip
        Private JsonManipLogObj As JsonManip
        Private JsonFileName = "Plans_UnitTest.json"
        Private JsonLogName = ".BoxITLog.json"

        Private ReadOnly CurrentDirectory = System.Environment.CurrentDirectory()
        Private ReadOnly SourceDirectory = CurrentDirectory & "\" & "TestRoot"
        Private ReadOnly DestinationDirectory = CurrentDirectory & "\" & "Dest"
        Private ReadOnly DestinationZipName = DestinationDirectory & "\" & "Backup.zip"

        Private ReadOnly filename1 = "filename1.txt"
        Private ReadOnly filename2 = "filename2.txt"
        Private ReadOnly filename3 = "filename3.txt"

        Private ReadOnly SubdirectoryName1 = "Sub1"
        Private ReadOnly SubdirectoryName2 = "Sub2"
        Private ReadOnly SubdirectoryName3 = "Sub3"


        <SetUp>
        Public Sub Setup()
            'Setting up JsonObject
            JsonManipObj = New JsonManip()
            JsonManipLogObj = New JsonManip()
        End Sub

        <Test>
        Public Sub EmptyFileCreation_EqualTest()
            JsonManipObj.SetJsonFile(JsonFileName, JsonManip.JsonFileType.Plans)

            Dim Plans = JsonManipObj.ReadPlanObjectsFromJsonFile()
            Assert.AreEqual(Plans.Capacity, 0)
        End Sub

        Function GetInformationValue(information As String, key As String) As String
            ' Use regular expression to extract the value for a given key
            Dim match As Match = Regex.Match(information, $"{key}\s*([^\r\n]*)")
            If match.Success Then
                Return match.Groups(1).Value.Trim()
            Else
                Return "N/A"
            End If
        End Function

        <Test>
        Public Sub parseIncommingQuery_EqualTest()
            Dim Taskname = "BoxIT_dfg_IncrementalBackup"

            Dim QueryList = vbCrLf &
                "Folder: \" & vbCrLf &
                "HostName: PC" & vbCrLf &
                "TaskName: \" & Taskname & vbCrLf &
                "Next Run Time: 12/13/2023 7:24:00PM" & vbCrLf &
                "Status: Ready" & vbCrLf &
                "Logon Mode: Interactive only" & vbCrLf

            Assert.AreEqual("12/13/2023 7:24:00PM", GetInformationValue(QueryList, "Next Run Time:"))

        End Sub

        Function GetNextCustomBackUpDate(CustomDaysString As String) As Date
            ' comma-separated string
            Dim DaysList As New List(Of String)(CustomDaysString.Split(","c))

            ' Create an boolean day array
            Dim DaysToStrings = New String() {"SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"}
            Dim DaysToBoolean = New Boolean() {0, 0, 0, 0, 0, 0, 0}

            For Each day As String In DaysList
                For index = 0 To 6
                    If day = DaysToStrings(index) Then
                        DaysToBoolean(index) = True
                    End If
                Next
            Next

            Dim DaysCounter = 1
            Dim CurrentDay = Now.DayOfWeek
            Dim DaysIndex = CurrentDay + 1

            While DaysIndex <> CurrentDay Mod 7
                If DaysToBoolean(DaysIndex) Then
                    Exit While
                End If
                DaysCounter += 1
                DaysIndex = (DaysIndex + 1) Mod 7
            End While

            Return Now.AddDays(DaysCounter)
        End Function

        <Test>
        Public Sub NextCustomBackupDate_Test1()
            Dim CustomDaysTest = "SUN,MON,WED"
            Console.WriteLine(GetNextCustomBackUpDate(CustomDaysTest))
            Assert.True(True)
        End Sub

        <Test>
        Public Sub NextCustomBackupDate_Test2()
            Dim CustomDaysTest = "WED,THU,MON"
            Console.WriteLine(GetNextCustomBackUpDate(CustomDaysTest))
            Assert.True(True)
        End Sub

        <Test>
        Public Sub SimpleFileLog_EqualTest()
            Directory.CreateDirectory(SourceDirectory)
            Directory.CreateDirectory(DestinationDirectory)

            Dim fs = File.Create(SourceDirectory & "\" & filename1)
            fs.Close()

            ZipFile.CreateFromDirectory(SourceDirectory, DestinationZipName, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(DestinationZipName)

            ' Perform Tree Creation of SourceDirectory Structure
            JsonManipLogObj.SetJsonFile(SourceDirectory & "\" & JsonLogName, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()

            Dim tree = JsonManipLogObj.ReadJarray()
            Dim treefilename = tree(0)("Name").ToString()

            Assert.AreEqual(filename1, treefilename)

            'TearDown Remove Directories and Files
            Directory.Delete(SourceDirectory, True)
            Directory.Delete(DestinationDirectory, True)
        End Sub

        <Test>
        Public Sub MultipleFileLog_EqualTest()
            Directory.CreateDirectory(SourceDirectory)
            Directory.CreateDirectory(DestinationDirectory)

            Dim fs = File.Create(SourceDirectory & "\" & filename1)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & filename2)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & filename3)
            fs.Close()

            ZipFile.CreateFromDirectory(SourceDirectory, DestinationZipName, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(DestinationZipName)

            ' Perform Tree Creation of SourceDirectory Structure
            JsonManipLogObj.SetJsonFile(SourceDirectory & "\" & JsonLogName, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()

            Dim tree = JsonManipLogObj.ReadJarray()
            Assert.AreEqual(filename1, tree(0)("Name").ToString())
            Assert.AreEqual(filename2, tree(1)("Name").ToString())
            Assert.AreEqual(filename3, tree(2)("Name").ToString())

            'TearDown Remove Directories and Files
            Directory.Delete(SourceDirectory, True)
            Directory.Delete(DestinationDirectory, True)
        End Sub

        <Test>
        Public Sub SimpleSubDirectoryLog_EqualTest()
            Directory.CreateDirectory(SourceDirectory)
            Directory.CreateDirectory(SourceDirectory & "\" & SubdirectoryName1)
            Directory.CreateDirectory(DestinationDirectory)

            Dim fs = File.Create(SourceDirectory & "\" & SubdirectoryName1 & "\" & filename1)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & SubdirectoryName1 & "\" & filename2)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & SubdirectoryName1 & "\" & filename3)
            fs.Close()

            ZipFile.CreateFromDirectory(SourceDirectory, DestinationZipName, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(DestinationZipName)

            ' Perform Tree Creation of SourceDirectory Structure
            JsonManipLogObj.SetJsonFile(SourceDirectory & "\" & JsonLogName, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()

            Dim tree = JsonManipLogObj.ReadJarray()
            Console.WriteLine(tree(0)("Files")(0)("Name").ToString())
            Assert.AreEqual(filename1, tree(0)("Files")(0)("Name").ToString())

            'TearDown Remove Directories and Files
            Directory.Delete(SourceDirectory, True)
            Directory.Delete(DestinationDirectory, True)
        End Sub

        <Test>
        Public Sub MultipleSubDirectoryLog_EqualTest()
            'Creating Deep DirectoryTree
            Directory.CreateDirectory(SourceDirectory & "\" & SubdirectoryName1 & "\" & SubdirectoryName2 & "\" & SubdirectoryName3)
            Directory.CreateDirectory(DestinationDirectory)

            Dim fs = File.Create(SourceDirectory & "\" & SubdirectoryName1 & "\" & filename1)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & SubdirectoryName1 & "\" & SubdirectoryName2 & "\" & filename2)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & SubdirectoryName1 & "\" & SubdirectoryName2 & "\" & SubdirectoryName3 & "\" & filename3)
            fs.Close()

            ZipFile.CreateFromDirectory(SourceDirectory, DestinationZipName, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(DestinationZipName)

            ' Perform Tree Creation of SourceDirectory Structure
            JsonManipLogObj.SetJsonFile(SourceDirectory & "\" & JsonLogName, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()

            Dim tree = JsonManipLogObj.ReadJarray()
            Assert.AreEqual(filename1, tree(0)("Files")(1)("Name").ToString())
            Assert.AreEqual(filename2, tree(0)("Files")(0)("Files")(1)("Name").ToString())
            Assert.AreEqual(filename3, tree(0)("Files")(0)("Files")(0)("Files")(0)("Name").ToString())

            'TearDown Remove Directories and Files
            Directory.Delete(SourceDirectory, True)
            Directory.Delete(DestinationDirectory, True)
        End Sub

        <Test>
        Public Sub SimpleFileCreationUpdate_EqualTest()
            Directory.CreateDirectory(SourceDirectory)
            Directory.CreateDirectory(DestinationDirectory)

            Dim fs = File.Create(SourceDirectory & "\" & filename1)
            fs.Close()

            ZipFile.CreateFromDirectory(SourceDirectory, DestinationZipName, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(DestinationZipName)

            ' Perform Tree Creation of SourceDirectory Structure
            JsonManipLogObj.SetJsonFile(SourceDirectory & "\" & JsonLogName, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()
            Dim PreviousUpdateTime = Now
            Dim LogTree = JsonManipLogObj.ReadJarray()

            ' Modification of Source Tree ======== 
            Threading.Thread.Sleep(3000) 'Wait 3 seconds before update
            fs = File.Create(SourceDirectory & "\" & filename2)
            fs.Close()
            ' Modification of Source Tree ======== 

            Dim CurrentDirectoryTree = JsonManipLogObj.DepthFirstTransversal(SourceDirectory) 'Get the Current Directory Tree

            ' Incremental Update Process =========
            DeleteSrcEntires(LogTree, SourceDirectory, DestinationZipName)
            UpdateDstEntries(LogTree, CurrentDirectoryTree, PreviousUpdateTime, DestinationZipName, SourceDirectory, "")
            ' Incremental Update Process =========

            Assert.AreEqual(filename2, LogTree(1)("Name").ToString())

            Directory.Delete(SourceDirectory, True)
            Directory.Delete(DestinationDirectory, True)
        End Sub

        <Test>
        Public Sub SimpleFileDeletionUpdate_EqualTest()
            Directory.CreateDirectory(SourceDirectory)
            Directory.CreateDirectory(DestinationDirectory)

            Dim fs = File.Create(SourceDirectory & "\" & filename2)
            fs.Close()
            fs = File.Create(SourceDirectory & "\" & filename1)
            fs.Close()

            ZipFile.CreateFromDirectory(SourceDirectory, DestinationZipName, CompressionLevel.Optimal, False) 'Create Zip Archive
            FileAssert.Exists(DestinationZipName)

            ' Perform Tree Creation of SourceDirectory Structure
            JsonManipLogObj.SetJsonFile(SourceDirectory & "\" & JsonLogName, JsonManip.JsonFileType.Log)
            JsonManipLogObj.JsonTreeCreation()
            Dim PreviousUpdateTime = Now
            Dim LogTree = JsonManipLogObj.ReadJarray()

            ' Modification of Source Tree ======== 
            Threading.Thread.Sleep(3000) 'Wait 3 seconds before update
            File.Delete(SourceDirectory & "\" & filename2)
            ' Modification of Source Tree ======== 

            Dim CurrentDirectoryTree = JsonManipLogObj.DepthFirstTransversal(SourceDirectory) 'Get the Current Directory Tree

            ' Incremental Update Process =========
            DeleteSrcEntires(LogTree, SourceDirectory, DestinationZipName)
            UpdateDstEntries(LogTree, CurrentDirectoryTree, PreviousUpdateTime, DestinationZipName, SourceDirectory, "")
            ' Incremental Update Process =========

            Console.WriteLine(LogTree.ToString())
            Assert.AreNotEqual(filename2, LogTree(0)("Name").ToString())

            Directory.Delete(SourceDirectory, True)
            Directory.Delete(DestinationDirectory, True)
        End Sub

        <TearDown>
        Public Sub Teardown()
            If File.Exists(JsonFileName) Then
                File.Delete(JsonFileName)
            End If

            If Directory.Exists(SourceDirectory) Then
                Directory.Delete(SourceDirectory, True)
            End If

            If Directory.Exists(DestinationDirectory) Then
                Directory.Delete(DestinationDirectory, True)
            End If
        End Sub

        ' The Delete Entry process of the Incremental backup
        '<Test>
        Public Sub IncrementalDeleteProcess_StageTest()
            JsonManipLogObj.SetJsonFile("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json", JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()

            DeleteSrcEntires(LogTree, "C:\Users\Guzman\Desktop\411-Project2", "C:\Users\Guzman\Desktop\Dest_backup\Project2_DP_Spring2022.zip")

            'Writes content of Jarray back into the json file
            Using sw As StreamWriter = File.CreateText("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json")
                Using writer As New JsonTextWriter(sw)
                    LogTree.WriteTo(writer)
                End Using
            End Using
            Assert.IsTrue(True)
        End Sub

        ' Update Entry process for the incremental backup 
        '<Test>
        Public Sub IncrementalUpdateProcess_StateTest()
            Dim LogPath = "C:\Users\Guzman\Desktop\Incremental_Testcases\Simple\.BoxITLog.json"
            Dim SrcPath = "C:\Users\Guzman\Desktop\Incremental_Testcases\Simple"
            Dim ZipDestinationPath = "C:\Users\Guzman\Desktop\Destination\Project.zip"
            JsonManipLogObj.SetJsonFile(LogPath, JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()
            Dim CurrentDirectoryTree = JsonManipLogObj.DepthFirstTransversal(SrcPath)

            Dim timestampDateTime As Date
            Dim datestring = "11/8/2023 10:40:00 PM"
            Dim formatString = "M/d/yyyy h:m:s tt"

            timestampDateTime = Date.ParseExact(datestring, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None)

            UpdateDstEntries(LogTree, CurrentDirectoryTree, timestampDateTime, ZipDestinationPath, SrcPath, "")

            Using sw As StreamWriter = File.CreateText(LogPath)
                Using writer As New JsonTextWriter(sw)
                    LogTree.WriteTo(writer)
                End Using
            End Using
        End Sub

    End Class

End Namespace