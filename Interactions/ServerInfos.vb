Imports System.Net
Imports System.Text
Imports Newtonsoft.Json.Linq

Namespace Bot.Interactions
    Public Class ServerInfos
        Implements InteractionsExecutor

        Public Function Run(obj As DiscordSlashInteraction) As JObject Implements InteractionsExecutor.Run
            Dim client As WebClient = New WebClient()
            Dim Response As New JObject From {
                {"type", 4}
            }
            Dim data As New JObject
            Dim lang_ As String = "en"
            Try
                Dim ip As DiscordSlashInteractionOption
                Dim port As DiscordSlashInteractionOption

                Dim obj_guild_id As ULong = obj.guild_id
                Dim obj_channel_id As ULong = obj.channel_id

                Dim obj_member As DiscordSlashInteractionMember = obj.member
                Dim obj_user As DiscordUser = obj_member.user
                Dim obj_data As DiscordSlashInteractionData = obj.data
                Try
                    For Each [option] In obj_data.options
                        Select Case [option].name
                            Case "ip"
                                ip = [option]
                            Case "port"
                                port = [option]
                        End Select
                    Next
                Catch
                End Try

                If Not ip.value = "" Then
                    Dim port_def = 27015
                    If port IsNot Nothing Then
                        If port.value > 0 Then
                            port_def = port.value
                        End If
                    End If
                    Try
                        Dim server_raw = client.DownloadString($"https://csgo.discord.wf/server/infos.php?address={ip.value}&port={port_def}")
                        Dim server As JObject = JObject.Parse(server_raw)
                        If Not server.ContainsKey("error") Then
                            server.Add("error", False)
                        End If
                        If Not server("error").ToObject(Of Boolean) Then
                            data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.success.text"))
                            Dim embed As New DiscordEmbed
                            Select Case server("mod")
                                Case "garrysmod"
                                    embed.WithAuthor($"Garry's Mod ({server("modname")})", , $"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}.png")
                                Case "tf"
                                    embed.WithAuthor("Team Fortress 2", , $"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}.png")
                                Case "cstrike"
                                    Select Case server("protocol").ToObject(Of Integer)
                                        Case 73
                                            embed.WithAuthor("Counter-Strike Source", , $"https://github.com/ZaptoInc/SourceServers/raw/main/games/css.png")
                                        Case Else
                                            embed.WithAuthor("Counter-Strike", , $"https://github.com/ZaptoInc/SourceServers/raw/main/games/cs.png")
                                    End Select


                                Case Else
                                    embed.WithAuthor(server("modname"), , $"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}.png")
                            End Select

                            embed.WithCurrentTimestamp()
                            embed.WithDescription($"__{server("name")}__")
                            embed.AddField(Lang.Translate(lang_, "bot.interactions.serverinfos.ip"), $"`{server("ip")}:{server("port")}`", True)
                            embed.AddField(Lang.Translate(lang_, "bot.interactions.serverinfos.map"), server("map"), True)
                            embed.AddField(Lang.Translate(lang_, "bot.interactions.serverinfos.players"), $"{server("players")}/{server("places")}", True)
                            Select Case server("mod").ToString
                                Case "csgo"
                                    embed.WithThumbnail($"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}/{server("map")}.jpg")
                            End Select

                            Dim embeds As New JArray
                            embeds.Add(JObject.FromObject(embed))
                            data.Add("embeds", embeds)
                            Response.Add("data", data)
                            Return Response
                        Else
                            If server("message") = "failed to connect to server" Then
                                data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.error.failedtoconnect.text"))
                                Dim embed As New DiscordEmbed
                                embed.WithAuthor(Lang.Translate(lang_, "bot.interactions.serverinfos.error.failedtoconnect.game"),, "https://github.com/ZaptoInc/SourceServers/raw/main/games/unknown.png")
                                embed.WithCurrentTimestamp()
                                embed.AddField(Lang.Translate(lang_, "`bot.interactions.serverinfos.ip"), $"{ip.value}:{port_def}`", True)
                                embed.AddField(Lang.Translate(lang_, "bot.interactions.serverinfos.map"), "?", True)
                                embed.AddField(Lang.Translate(lang_, "bot.interactions.serverinfos.players"), "?/?", True)
                                Dim embeds As New JArray
                                embeds.Add(JObject.FromObject(embed))
                                data.Add("embeds", embeds)
                                Response.Add("data", data)
                                Return Response
                            Else
                                data.Add("content", Lang.Translate(lang_, "bot.interactions.serverinfos.error.unknown"))
                                Response.Add("data", data)
                                Return Response
                            End If
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
            data.Add("content", "End")
            Response.Add("data", data)
            Return Response
        End Function
    End Class
End Namespace

