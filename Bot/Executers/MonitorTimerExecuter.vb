Imports System.Net
Imports System.Timers
Imports Newtonsoft.Json.Linq

Namespace Bot
    Module MonitorTimerExecuter
        Public WithEvents monitor_timer As New Timers.Timer

        Private Sub monitor_timer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles monitor_timer.Elapsed
            Dim monitors As New Dictionary(Of String, JObject)
            Dim client As WebClient = New WebClient()
            For Each guild In Bot.Client.Guilds
                Try
                    Dim guild_config As Database.Guild = Database.GetOrCreateGuild(guild.Key)
                    Dim lang_ = guild_config.lang
                    Dim mon_save = guild_config.monitors.monitors
                    For Each monitor In mon_save
                        Try
                            If Not monitor.Value.channelid = Nothing Then
                                Dim channel = guild.Value.GetChannel(monitor.Value.channelid)
                                Try
                                    If Not monitors.ContainsKey($"{monitor.Value.IP}:{monitor.Value.port}") Then
                                        Dim server_raw = client.DownloadString($"https://csgo.discord.wf/server/infos.php?address={monitor.Value.IP}&port={monitor.Value.port}")
                                        Dim server As JObject = JObject.Parse(server_raw)
                                        monitors.Add($"{monitor.Value.IP}:{monitor.Value.port}", server)
                                    End If
                                Catch ex As Exception
                                    Console.WriteLine(ex.ToString)
                                End Try
                                Try
                                    If monitors.ContainsKey($"{monitor.Value.IP}:{monitor.Value.port}") Then
                                        Dim server = monitors($"{monitor.Value.IP}:{monitor.Value.port}")
                                        Dim embed As New DSharpPlus.Entities.DiscordEmbedBuilder
                                        If Not server.ContainsKey("error") Then
                                            server.Add("error", False)
                                        End If
                                        If Not server("error").ToObject(Of Boolean) Then
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

                                            embed.WithTimestamp(Date.UtcNow)
                                            embed.WithDescription($"__{server("name")}__")
                                            embed.AddField(Lang.Translate(lang_, "monitor.ip"), $"{server("ip")}:{server("port")}", True)
                                            embed.AddField(Lang.Translate(lang_, "monitor.map"), server("map"), True)
                                            embed.AddField(Lang.Translate(lang_, "monitor.players"), $"{server("players")}/{server("places")}", True)
                                            Select Case server("mod").ToString
                                                Case "csgo"
                                                    embed.WithThumbnail($"https://github.com/ZaptoInc/SourceServers/raw/main/games/{server("mod")}/{server("map")}.jpg")
                                            End Select
                                        Else
                                            If server("message") = "failed to connect to server" Then
                                                embed.WithAuthor(Lang.Translate(lang_, "monitor.error.failedtoconnect"),, "https://github.com/ZaptoInc/SourceServers/raw/main/games/unknown.png")
                                            Else
                                                embed.WithAuthor(Lang.Translate(lang_, "monitor.error.unknown"),, "https://github.com/ZaptoInc/SourceServers/raw/main/games/unknown.png")
                                            End If
                                            embed.WithTimestamp(Date.UtcNow)
                                            embed.AddField(Lang.Translate(lang_, "monitor.ip"), $"{monitor.Value.IP}:{monitor.Value.port}", True)
                                            embed.AddField(Lang.Translate(lang_, "monitor.map"), "?", True)
                                            embed.AddField(Lang.Translate(lang_, "monitor.players"), "?/?", True)
                                        End If
                                        If monitor.Value.messageid = Nothing Or monitor.Value.messageid < 100 Then
                                            Dim message = channel.SendMessageAsync(,, embed.Build).Result
                                            monitor.Value.messageid = message.Id
                                            guild_config.monitors.Save(monitor.Value)
                                            Database.SaveGuild(guild_config)
                                        Else
                                            Dim message = channel.GetMessageAsync(monitor.Value.messageid).Result
                                            message.ModifyAsync(, embed.Build)
                                        End If
                                    End If
                                Catch ex As Exception
                                    Console.WriteLine(ex.ToString)
                                End Try


                            End If


                        Catch ex As Exception
                            Console.WriteLine(ex.ToString)
                        End Try



                    Next
                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                End Try

            Next
        End Sub
    End Module
End Namespace
