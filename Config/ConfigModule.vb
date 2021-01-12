Imports System.IO
Imports Newtonsoft.Json

Namespace Config
    Module ConfigModule

        Public CurrentConfig As Config
        Public Function LoadConfig(Optional path As String = "data/config.json") As Boolean
            Try
                If Not Directory.Exists("data") Then
                    Directory.CreateDirectory("data")
                End If
                If File.Exists(path) Then
                    CurrentConfig = JsonConvert.DeserializeObject(Of Config)(IO.File.ReadAllText(path))
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub NewConfig(Optional path As String = "data/config.example.json")
            File.WriteAllText(path,
                              JsonConvert.SerializeObject(New Config, Formatting.Indented))
        End Sub
    End Module
End Namespace


