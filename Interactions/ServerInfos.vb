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
                        If Not server.ContainsKey("error") Then
                            server.Add("error", False)
                        End If
                        If Not server("error").ToObject(Of Boolean) Then
                            data.Add("content", "")
                            Dim embed As New DiscordEmbed
                            embed.WithAuthor(server("modname"), , $"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}.png")
                            embed.WithCurrentTimestamp()
                            embed.AddField("IP", $"[{server("ip")}:{server("port")}](steam://connect/{server("ip")}:{server("port")})", True)
                            embed.AddField("Map", server("map"), True)
                            embed.AddField("Players", $"{server("players")}/{server("places")}", True)
                            Select Case server("mod").ToString
                                Case "csgo", "gmod"
                                    embed.WithThumbnail($"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}/{server("map")}.png")
                            End Select

                            Dim embeds As New JArray
                            embeds.Add(JObject.FromObject(embed))
                            data.Add("embeds", embeds)
                            Response.Add("data", data)
                            Return Response
                        End If
                    Catch ex As Exception
                        data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.error.unknown"))
                        Response.Add("data", data)
                        Console.WriteLine(ex.ToString)
                        Return Response
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

