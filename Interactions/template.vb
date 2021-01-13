Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace Bot.Interactions
    Public Class Template
        Implements InteractionsExecutor

        Public Function Run(obj As DiscordSlashInteraction) As JObject Implements InteractionsExecutor.Run

            Console.WriteLine(JsonConvert.SerializeObject(obj))
            Dim client As WebClient = New WebClient()
            Dim Response As New JObject From {
                {"type", 4}
            }
            Dim data As New JObject
            Dim lang_ As String = "en"
            Try
                Dim param As DiscordSlashInteractionOption

                Dim obj_guild_id As ULong = obj.guild_id
                Dim obj_channel_id As ULong = obj.channel_id

                Dim obj_member As DiscordSlashInteractionMember = obj.member
                Dim obj_user As DiscordUser = obj_member.user
                Dim obj_data As DiscordSlashInteractionData = obj.data
                Try
                    For Each [option] In obj_data.options
                        Select Case [option].name
                            Case "param"
                                param = [option]
                        End Select
                    Next
                Catch
                End Try
                Try
                    Dim guild = Bot.Client.GetGuildAsync(obj_guild_id)
                    data.Add("content", Lang.Translate(lang_, "bot.interactions.template.success"))
                    Response.Add("data", data)
                    Return Response
                Catch ex As Exception
                    data.Add("content", Lang.Translate(lang_, "bot.interactions.template.error.notonguild"))
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

