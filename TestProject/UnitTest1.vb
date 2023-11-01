Imports NUnit.Framework

Namespace TestProject

    Public Class Tests

        <SetUp>
        Public Sub Setup()
            JsonObject = New JsonManip()
        End Sub

        <Test>
        Public Sub Test1()
            Assert.Pass()
        End Sub

    End Class

End Namespace