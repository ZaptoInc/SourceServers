'Imports System.Net
'Imports System.Text
'Imports Newtonsoft.Json.Linq

'Namespace Bot.Interactions
'    Public Class ServerURL
'        Implements InteractionsExecutor

'        Public Function Run(json As JObject) As JObject Implements InteractionsExecutor.Run
'            Dim client As WebClient = New WebClient()
'            Dim Response As New JObject From {
'                {"type", 4}
'            }
'            Dim data As New JObject
'            Dim lang_ As String = "en"
'            Try
'                Dim tag As String = ""
'                Dim guild_id As ULong = json("guild_id").ToObject(Of ULong)
'                Dim member As JObject = json("member")
'                Dim user As JObject = member("user")
'                Try
'                    Dim data_options As JArray = json("data")("options")
'                    For Each [option] In data_options
'                        Select Case [option]("name")
'                            Case "tag"
'                                tag = [option]("value")
'                            Case "guildid"
'                                guild_id = [option]("value")
'                        End Select
'                    Next
'                Catch
'                End Try



'                Try
'                    Dim col As New Specialized.NameValueCollection From {
'                        {"username", user("username")},
'                        {"avatar", user("avatar")},
'                        {"discriminator", user("discriminator")}
'                    }
'                    Dim dm_user As JObject = JObject.Parse(Encoding.UTF8.GetString(client.UploadValues($"{Config.CurrentConfig.APIDomain}updateUser/{user("id")}?token={Config.CurrentConfig.APItoken}", col)))
'                Catch ex As Exception
'                    Console.WriteLine(ex.ToString)
'                End Try

'                If Not tag = "" Then
'                    If IO.File.Exists($"data/tags/{tag}.json") Then
'                        Dim tag_json As JObject = JObject.Parse(IO.File.ReadAllText($"data/tags/{tag}.json"))
'                        guild_id = tag_json("id").ToObject(Of ULong)
'                    Else
'                        data.Add("content", Lang.Translate(lang_, "bot.interactions.serverurl.error.unknowntag"))
'                        Response.Add("data", data)
'                        Return Response
'                    End If
'                End If

'                Dim dm_guild_raw = client.DownloadString($"{Config.CurrentConfig.APIDomain}guild/{guild_id}?token={Config.CurrentConfig.APItoken}")
'                Dim dm_guild As JObject = JObject.Parse(dm_guild_raw)
'                If Not dm_guild("error").ToObject(Of Boolean) Then
'                    Dim shorturl As String = guild_id
'                    If Not tag = "" Then
'                        shorturl = tag
'                    End If
'                    data.Add("content", Lang.Translate(lang_, "bot.interactions.serverurl.success", $"{Config.CurrentConfig.MainDomain}{shorturl}"))
'                    Response.Add("data", data)
'                    Return Response
'                Else
'                    Select Case dm_guild("data").ToString
'                        Case "Invalid guild ID"
'                            data.Add("content", Lang.Translate(lang_, "bot.interactions.serverurl.error.unknownguild"))
'                            Response.Add("data", data)
'                            Return Response
'                        Case Else
'                            data.Add("content", Lang.Translate(lang_, "bot.interactions.serverurl.error.unknown"))
'                            Response.Add("data", data)
'                            Return Response
'                    End Select
'                End If
'            Catch ex As Exception
'                data.Add("content", Lang.Translate(lang_, "bot.interactions.serverurl.error.unknown"))
'                Response.Add("data", data)
'                Console.WriteLine(ex.ToString)
'                Return Response
'            End Try
'        End Function
'    End Class
'End Namespace

