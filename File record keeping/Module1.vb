Module Module1
    'Say that there is a src directory and a backup directory. 
    'When that src directory has been modified or new files / subfolders have been created
    'We need to keep track of those files and place them in the backup directory under an incremental schedule. 
    Sub Main()
        'The best way to probably store the records is through a .json file since it is
        'some what organized
        Dim json As String = "
            {
                ""name"":""Rap God"",  
                ""statistics"": {
                    ""likeCount"":""122255"",
                    ""dislikeCount"":""4472""
                    }
            }
        "
        Dim read = Newtonsoft.Json.Linq.JObject.Parse(json)
        Console.WriteLine(read.Item("name"))
        Console.ReadLine()

        'Adding entry to json file
        Dim newFile As String = "fortnite.txt"
        read.Property("statistics").Add(newFile) 'adding new value to key property "statistics" 

        For Each file As String In read.Property("statistics")
            Console.WriteLine(file)
        Next
        Console.ReadLine()

    End Sub

End Module
