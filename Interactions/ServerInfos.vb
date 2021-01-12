Imports System.Net
Imports System.Text
Imports Newtonsoft.Json.Linq

Namespace Bot.Interactions
    Public Class ServerInfos
        Implements InteractionsExecutor

        Public Function Run(json As JObject) As JObject Implements InteractionsExecutor.Run
            Dim client As WebClient = New WebClient()
            Dim Response As New JObject From {
                {"type", 4}
            }
            Dim data As New JObject
            Dim lang_ As String = "en"
            Try
                Dim ip As String = ""
                Dim port As Integer = 27015

                Dim member As JObject = json("member")
                Dim user As JObject = member("user")
                Try
                    Dim data_options As JArray = json("data")("options")
                    For Each [option] In data_options
                        Select Case [option]("name")
                            Case "ip"
                                ip = [option]("value")
                            Case "port"
                                port = [option]("value")
                        End Select
                    Next
                Catch
                End Try

                If Not ip = "" Then
                    Try
                        Dim server_raw = client.DownloadString($"https://csgo.discord.wf/server/infos.php?address={ip}&port={port}")
                        Dim server As JObject = JObject.Parse(server_raw)
                        If Not server("error").ToObject(Of Boolean) Then
                            data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.error.unknown"))
                            Dim embed As New JObject
                            embed.Add("title", server("modname"))
                            Dim embed_fields As New JArray
                            Dim field
                            embed_fields.Add()
                            data.Add("embed", embed)
                            Response.Add("data", data)
                        End If
                    Catch ex As Exception
                        data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.error.unknown"))
                        Response.Add("data", data)
                        Console.WriteLine(ex.ToString)
                    End Try

                End If
            Catch ex As Exception
                data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.error.unknown"))
                Response.Add("data", data)
                Console.WriteLine(ex.ToString)
                Return Response
            End Try
        End Function
    End Class
End Namespace

