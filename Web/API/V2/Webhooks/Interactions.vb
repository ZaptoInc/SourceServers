Imports Ceen.Mvc
Imports Ceen
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Text
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Crypto.Signers

Namespace Web.API.V2.Webhooks
    <Route("api/v2/webhook/interactions")>
    Public Class Interactions
        Inherits Controller
        Public Function IndexPost() As IResult
            Try
                If Context.Request.IsJsonRequest Then
                    Dim Response As JObject = Nothing
                    Dim public_key = Config.CurrentConfig.public_key
                    Dim signature = Context.Request.Headers("X-Signature-Ed25519")
                    Dim timestamp = Context.Request.Headers("X-Signature-Timestamp")
                    Dim BodyReader As New StreamReader(Context.Request.Body)
                    Dim BodyJson = BodyReader.ReadToEnd

                    Dim message = Encoding.UTF8.GetBytes($"{timestamp}{BodyJson}")
                    Dim ED_PublicKey = New Ed25519PublicKeyParameters(StringToByteArray(public_key), 0)

                    Dim validator = New Ed25519Signer
                    validator.Init(False, ED_PublicKey)
                    validator.BlockUpdate(message, 0, message.Length)

                    Dim verified As Boolean = validator.VerifySignature(StringToByteArray(signature))

                    If verified Then
                        Dim Request As JObject = JObject.Parse(BodyJson)
                        If Request("type").ToObject(Of Integer) = 1 Then
                            Response = New JObject From {
                                {"type", 1}
                            }
                            Return Json(Response)
                        ElseIf Request("type").ToObject(Of Integer) = 2 Then
                            If Request.ContainsKey("data") Then
                                Dim data As JObject = Request("data")
                                If data.ContainsKey("name") Then
                                    If Bot.Integrations.GetInteractions.ContainsKey(data("name")) Then
                                        Response = Bot.Integrations.GetInteractions()(data("name")).Run(Request)
                                        If Response IsNot Nothing Then
                                            Return Json(Response)
                                        Else
                                            Return Status(501)
                                        End If
                                    Else
                                        Return Status(501)
                                    End If
                                Else
                                    Return Status(422)
                                End If
                            Else
                                Return Status(422)
                            End If
                        Else
                            Return Status(501)
                        End If
                    Else
                        Return Status(401)
                    End If

                Else
                    Return Status(422)
                End If
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
                Return Status(500)
            End Try
        End Function
    End Class
End Namespace


