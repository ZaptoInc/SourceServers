Imports System.Threading
Imports DSharpPlus
Imports DSharpPlus.EventArgs

Namespace Bot
    Module Index
        Public WithEvents Client As DiscordClient

        Public Integrations As New ApplicationInteractionsRegistry

        Sub PrepareBot()
            Dim cfg = New DiscordConfiguration With {
                .Token = Config.CurrentConfig.token,
                .TokenType = TokenType.Bot,
                .MessageCacheSize = 0
            }
            Client = New DiscordClient(cfg)
            Integrations.Register("serverinfos", New Bot.Interactions.ServerInfos)
            'Integrations.Register("serverurl", New Bot.Interactions.ServerURL)
        End Sub

        Async Function [Start]() As Task
            Await Client.ConnectAsync()
            Return
        End Function
        Async Function [Stop]() As Task
            Await Client.DisconnectAsync()
            Return
        End Function

        Async Function [Restart](Optional prepare As Boolean = False) As Task
            Await [Stop]()
            If prepare Then PrepareBot()
            Await [Start]()
            Return
        End Function

        Private Async Function Client_UnknownEvent(sender As DiscordClient, e As UnknownEventArgs) As Task Handles Client.UnknownEvent
            Select Case e.EventName
                Case "INTERACTION_CREATE"
                    e.Handled = True
                    Dim thr As New Thread(Sub() ExecuteInteractionCreate(e.Json))
                    thr.Start()
                Case Else
                    Console.WriteLine(e.EventName)
                    Console.WriteLine(e.Json)
            End Select
            Return
        End Function
    End Module
End Namespace

