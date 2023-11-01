Imports NUnit.Framework
Imports System.IO
Imports BoxIT_Project

Namespace TestingEnvironment

    Public Class Tests
        Private JsonManipObj As JsonManip
        Private JsonManipLogObj As JsonManip
        Private JsonFileName = "Plans_UnitTest.json"

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

        '<Test> Used to testout array access in JToken
        <Test>
        Public Sub LogTreeManip_StageTest()
            JsonManipLogObj.SetJsonFile("C:\Users\Guzman\Desktop\411-Project2\.BoxITLog.json", JsonManip.JsonFileType.Log)
            Dim LogTree = JsonManipLogObj.ReadJarray()

            'Test to see if we can recursivly transverse through token array
            Dim nameOFDirecotyr = LogTree(0)("Files")(0)("Name").ToString()
        End Sub


        <TearDown>
        Public Sub Teardown()
            If File.Exists(JsonFileName) Then
                File.Delete(JsonFileName)
            End If
        End Sub

    End Class

End Namespace