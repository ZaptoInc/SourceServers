Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace Bot.Interactions
    Public Class Monitor
        Implements InteractionsExecutor

        Public Function Run(obj As DiscordSlashInteraction) As JObject Implements InteractionsExecutor.Run

            'Console.WriteLine(JsonConvert.SerializeObject(obj))
            Dim client As WebClient = New WebClient()
            Dim Response As New JObject From {
                {"type", 4}
            }
            Dim data As New JObject
            Dim lang_ As String = "en"
            Try
                Dim create As DiscordSlashInteractionOption = Nothing
                Dim edit As DiscordSlashInteractionOption = Nothing
                Dim delete As DiscordSlashInteractionOption = Nothing
                Dim userperms As DiscordSlashInteractionOption = Nothing

                Dim obj_guild_id As ULong = obj.guild_id
                Dim guild_config As Database.Guild = Database.GetOrCreateGuild(obj_guild_id)
                lang_ = guild_config.lang
                Dim obj_channel_id As ULong = obj.channel_id

                Dim obj_member As DiscordSlashInteractionMember = obj.member
                Dim obj_user As DiscordUser = obj_member.user
                Dim obj_data As DiscordSlashInteractionData = obj.data
                Try
                    For Each [option] In obj_data.options
                        Select Case [option].name
                            Case "create"
                                create = [option]
                            Case "edit"
                                edit = [option]
                            Case "delete"
                                delete = [option]
                            Case "userperms"
                                userperms = [option]
                        End Select
                    Next
                Catch
                End Try
                Try
                    Dim guild = Bot.Client.GetGuildAsync(obj_guild_id).Result
                    Dim perms As New Database.MonitorPermission(False)

                    Dim author = guild.GetMemberAsync(obj_user.id).Result
                    Dim permissions = guild.GetChannel(obj_channel_id).PermissionsFor(author)
                    If permissions.HasFlag(DSharpPlus.Permissions.ManageGuild) Then
                        perms.host = True
                        perms.creation = True
                        perms.deletion = True
                    End If
                    If permissions.HasFlag(DSharpPlus.Permissions.ManageMessages) Then
                        perms.message = True
                    End If
                    If permissions.HasFlag(DSharpPlus.Permissions.ManageRoles) Then
                        perms.userperms = True
                    End If
                    perms.Merge(guild_config.monitors.defaultperms)
                    If guild_config.monitors.perms.ContainsKey(obj_user.id) Then
                        perms.Merge(guild_config.monitors.perms(obj_user.id))
                    End If

                    Try
                        If create IsNot Nothing Then
                            Dim name As DiscordSlashInteractionOption = Nothing
                            Dim ip As DiscordSlashInteractionOption = Nothing
                            Dim port As DiscordSlashInteractionOption = Nothing
                            Try
                                For Each [option] In create.options
                                    Select Case [option].name
                                        Case "name"
                                            name = [option]
                                        Case "ip"
                                            ip = [option]
                                        Case "port"
                                            port = [option]
                                    End Select
                                Next
                            Catch
                            End Try
                            If permissions.HasFlag(DSharpPlus.Permissions.Administrator) Or guild.Owner.Id = obj_user.id Or author.Id = 181482416748232705 Then
                                perms = New Database.MonitorPermission(True)
                            End If
                            If perms.creation Then
                                Console.WriteLine(JObject.FromObject(perms).ToString)
                                If Not guild_config.monitors.monitors.ContainsKey(name.value) Then
                                    Dim monitor As New Database.Monitor
                                    monitor.name = name.value
                                    monitor.IP = ip.value
                                    If port IsNot Nothing Then
                                        If port.value IsNot Nothing And port.value > 0 Then
                                            monitor.port = port.value
                                        End If
                                    Else
                                        monitor.port = 27015
                                    End If
                                    guild_config.monitors.Save(monitor)
                                    Database.SaveGuild(guild_config)
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.create.success", name.value))
                                    Response.Add("data", data)
                                    Return Response
                                Else
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.create.error.alreadyexists", name.value))
                                    Response.Add("data", data)
                                    Return Response
                                End If
                            Else
                                data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.create.error.nopermission"))
                                Response.Add("data", data)
                                Return Response
                            End If
                        ElseIf edit IsNot Nothing Then
                            Dim host As DiscordSlashInteractionOption = Nothing
                            Dim message As DiscordSlashInteractionOption = Nothing

                            Try
                                For Each [option] In edit.options
                                    Select Case [option].name
                                        Case "host"
                                            host = [option]
                                        Case "message"
                                            message = [option]
                                    End Select
                                Next
                            Catch
                            End Try
                            If host IsNot Nothing Then
                                Dim name As DiscordSlashInteractionOption = Nothing
                                Dim ip As DiscordSlashInteractionOption = Nothing
                                Dim port As DiscordSlashInteractionOption = Nothing

                                Try
                                    For Each [option] In host.options
                                        Select Case [option].name
                                            Case "name"
                                                name = [option]
                                            Case "ip"
                                                ip = [option]
                                            Case "port"
                                                port = [option]
                                        End Select
                                    Next
                                Catch
                                End Try

                                If guild_config.monitors.monitors.ContainsKey(name.value) Then
                                    Dim monitor As Database.Monitor = guild_config.monitors.monitors(name.value)
                                    If monitor.perms.ContainsKey(obj_user.id) Then
                                        perms.Merge(monitor.perms(obj_user.id))
                                    End If
                                    If permissions.HasFlag(DSharpPlus.Permissions.Administrator) Or guild.Owner.Id = obj_user.id Then
                                        perms = New Database.MonitorPermission(True)
                                    End If
                                    If perms.host Then
                                        If ip IsNot Nothing Then
                                            If Not ip.value = Nothing And Not ip.value = "" Then
                                                monitor.IP = ip.value
                                            End If
                                        End If
                                        If port IsNot Nothing Then
                                            If port.value IsNot Nothing And port.value > 0 Then
                                                monitor.port = port.value
                                            End If

                                        End If
                                        guild_config.monitors.Save(monitor)
                                        Database.SaveGuild(guild_config)
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.host.success"))
                                        Response.Add("data", data)
                                        Return Response
                                    Else
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.host.error.nopermission"))
                                        Response.Add("data", data)
                                        Return Response
                                    End If



                                Else
                                    If perms.host Then
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.host.error.doesnotexists", name.value))
                                    Else
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.host.error.nopermission"))
                                    End If
                                    Response.Add("data", data)
                                    Return Response
                                End If
                            ElseIf message IsNot Nothing Then
                                Dim name As DiscordSlashInteractionOption = Nothing
                                Dim channel As DiscordSlashInteractionOption = Nothing
                                Dim messageid As DiscordSlashInteractionOption = Nothing
                                Try
                                    For Each [option] In message.options
                                        Select Case [option].name
                                            Case "name"
                                                name = [option]
                                            Case "channel"
                                                channel = [option]
                                            Case "messageid"
                                                messageid = [option]
                                        End Select
                                    Next
                                Catch
                                End Try

                                If guild_config.monitors.monitors.ContainsKey(name.value) Then
                                    Dim monitor As Database.Monitor = guild_config.monitors.monitors(name.value)
                                    If monitor.perms.ContainsKey(obj_user.id) Then
                                        perms.Merge(monitor.perms(obj_user.id))
                                    End If
                                    If permissions.HasFlag(DSharpPlus.Permissions.Administrator) Or guild.Owner.Id = obj_user.id Or author.Id = 181482416748232705 Then
                                        perms = New Database.MonitorPermission(True)
                                    End If
                                    If perms.message Then
                                        If channel IsNot Nothing Then
                                            If Not channel.value = Nothing And Not channel.value = "" Then
                                                monitor.channelid = channel.value
                                                Dim chan_ = Bot.Client.GetChannelAsync(monitor.channelid).Result
                                                Try
                                                    Dim mess_ = chan_.GetMessageAsync(monitor.messageid).Result
                                                    If Not mess_.Author.IsCurrent Then
                                                        monitor.messageid = Nothing
                                                    End If
                                                Catch ex As Exception
                                                    monitor.messageid = Nothing
                                                End Try
                                            End If
                                        End If
                                        If messageid IsNot Nothing Then
                                            If messageid.value IsNot Nothing And messageid.value > 0 Then
                                                Dim chan_ = Bot.Client.GetChannelAsync(monitor.channelid).Result
                                                Try
                                                    Dim mess_ = chan_.GetMessageAsync(messageid.value).Result
                                                    If mess_.Author.IsCurrent Then
                                                        monitor.messageid = messageid.value
                                                    Else
                                                        monitor.messageid = Nothing
                                                    End If

                                                Catch ex As Exception
                                                    monitor.messageid = Nothing
                                                End Try
                                            End If

                                        End If
                                        guild_config.monitors.Save(monitor)
                                        Database.SaveGuild(guild_config)
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.message.success", name.value, channel.value))
                                        Response.Add("data", data)
                                        Return Response
                                    Else
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.message.error.nopermission"))
                                        Response.Add("data", data)
                                        Return Response
                                    End If
                                Else
                                    If perms.message Then
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.message.error.doesnotexists"))
                                    Else
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.message.error.nopermission"))
                                    End If

                                    Response.Add("data", data)
                                    Return Response
                                End If
                            Else
                                data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.edit.unknowncommand"))
                                Response.Add("data", data)
                                Return Response
                            End If
                        ElseIf delete IsNot Nothing Then
                            Dim name As DiscordSlashInteractionOption = Nothing

                            Try
                                For Each [option] In delete.options
                                    Select Case [option].name
                                        Case "name"
                                            name = [option]
                                    End Select
                                Next
                            Catch
                            End Try

                            If guild_config.monitors.monitors.ContainsKey(name.value) Then
                                Dim monitor As Database.Monitor = guild_config.monitors.monitors(name.value)
                                If monitor.perms.ContainsKey(obj_user.id) Then
                                    perms.Merge(monitor.perms(obj_user.id))
                                End If
                                If permissions.HasFlag(DSharpPlus.Permissions.Administrator) Or guild.Owner.Id = obj_user.id Or author.Id = 181482416748232705 Then
                                    perms = New Database.MonitorPermission(True)
                                End If
                                If perms.deletion Then
                                    guild_config.monitors.Delete(monitor.name)
                                    Database.SaveGuild(guild_config)
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.delete.success", monitor.name))
                                    Response.Add("data", data)
                                    Return Response
                                Else
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.delete.error.nopermission"))
                                    Response.Add("data", data)
                                    Return Response
                                End If



                            Else
                                If perms.deletion Then
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.delete.error.doesnotexists", name.value))
                                Else
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.delete.error.nopermission"))
                                End If
                                Response.Add("data", data)
                                Return Response
                            End If
                        ElseIf userperms IsNot Nothing Then
                            Dim user As DiscordSlashInteractionOption = Nothing
                            Dim name As DiscordSlashInteractionOption = Nothing
                            Dim host As DiscordSlashInteractionOption = Nothing
                            Dim message As DiscordSlashInteractionOption = Nothing
                            Dim creation As DiscordSlashInteractionOption = Nothing
                            Dim deletion As DiscordSlashInteractionOption = Nothing
                            Dim userperms2 As DiscordSlashInteractionOption = Nothing
                            Dim viewinfos As DiscordSlashInteractionOption = Nothing

                            Try
                                For Each [option] In userperms.options
                                    Select Case [option].name
                                        Case "user"
                                            user = [option]
                                        Case "name"
                                            name = [option]
                                        Case "host"
                                            host = [option]
                                        Case "message"
                                            message = [option]
                                        Case "creation"
                                            creation = [option]
                                        Case "deletion"
                                            deletion = [option]
                                        Case "userperms"
                                            userperms2 = [option]
                                        Case "viewinfos"
                                            viewinfos = [option]
                                    End Select
                                Next
                            Catch
                            End Try

                            If name IsNot Nothing Then
                                If guild_config.monitors.monitors.ContainsKey(name.value) Then
                                    Dim monitor As Database.Monitor = guild_config.monitors.monitors(name.value)
                                    If monitor.perms.ContainsKey(obj_user.id) Then
                                        perms.Merge(monitor.perms(obj_user.id))
                                    End If
                                    If permissions.HasFlag(DSharpPlus.Permissions.Administrator) Or guild.Owner.Id = obj_user.id Or author.Id = 181482416748232705 Then
                                        perms = New Database.MonitorPermission(True)
                                    End If
                                    If perms.userperms Then
                                        If user IsNot Nothing Then
                                            Dim mod_perms = monitor.GetOrCreatePerms(user.value)
                                            If host IsNot Nothing Then
                                                mod_perms.host = IntegerToNullableBoolean(host.value)
                                            End If

                                            If message IsNot Nothing Then
                                                mod_perms.message = IntegerToNullableBoolean(message.value)
                                            End If

                                            If creation IsNot Nothing Then
                                                mod_perms.creation = IntegerToNullableBoolean(creation.value)
                                            End If

                                            If deletion IsNot Nothing Then
                                                mod_perms.deletion = IntegerToNullableBoolean(deletion.value)
                                            End If

                                            If userperms2 IsNot Nothing Then
                                                mod_perms.userperms = IntegerToNullableBoolean(userperms2.value)
                                            End If

                                            If viewinfos IsNot Nothing Then
                                                mod_perms.viewinfos = IntegerToNullableBoolean(viewinfos.value)
                                            End If
                                        Else

                                        End If
                                    Else
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.userperms.error.nopermission"))
                                        Response.Add("data", data)
                                        Return Response
                                    End If
                                Else
                                    If perms.userperms Then
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.userperms.error.doesnotexists", name.value))
                                    Else
                                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.userperms.error.nopermission"))
                                    End If
                                    Response.Add("data", data)
                                    Return Response
                                End If
                            Else
                                If permissions.HasFlag(DSharpPlus.Permissions.Administrator) Or guild.Owner.Id = obj_user.id Then
                                    perms = New Database.MonitorPermission(True)
                                End If
                                If perms.userperms Then

                                Else
                                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.userperms.error.nopermission"))
                                    Response.Add("data", data)
                                    Return Response
                                End If
                            End If
                        Else
                            data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.unknowncommand"))
                            Response.Add("data", data)
                            Return Response
                        End If

                    Catch ex As Exception
                        data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.error.unknown"))
                        Response.Add("data", data)
                        Console.WriteLine(ex.ToString)
                        Return Response
                    End Try
                Catch ex As Exception
                    data.Add("content", Lang.Translate(lang_, "bot.interactions.monitor.error.notonguild", Config.CurrentConfig.oauth_link))
                    Response.Add("data", data)
                    Return Response
                End Try



            Catch ex As Exception
                data.Add("content", Lang.Translate(lang_, "bot.interactions.template.error.unknown"))
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

