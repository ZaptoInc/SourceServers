Imports System.Net
Imports DSharpPlus.Entities
Imports Flurl
Imports Flurl.Http
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Module InteractionCreateExecuter
    Sub ExecuteInteractionCreate(j As String)
        Try
            Dim Request As JObject = JObject.Parse(j)
            Dim data As JObject = Request("data")
            Dim client As New WebClient
            Dim Response_data As New JObject From {
                {"content", "Please wait..."}
                }
            Dim Response As New JObject From {
                {"type", 4},
                {"data", Response_data}
            }
            If Bot.Integrations.GetInteractions.ContainsKey(data("name")) Then

                Dim r As Url = DiscordAPI.AppendPathSegments("interactions", Request("id"), Request("token"), "callback")
                Dim req_resp As IFlurlResponse = r.SendJsonAsync(System.Net.Http.HttpMethod.Post, Response).Result

                Response = Bot.Integrations.GetInteractions()(data("name")).Run(Request)

                r = "https://discord.com/api/webhooks".AppendPathSegments(Bot.Client.CurrentApplication.Id, Request("token"), "messages", "@original")
                req_resp = r.SendJsonAsync(System.Net.Http.HttpMethod.Patch, Response("data")).Result
            Else
            End If


        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

    End Sub
End Module
