Imports System
Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Timers
Imports Ceen
Imports Ceen.Httpd
Imports Ceen.Httpd.Logging
Imports Ceen.Mvc
Imports MongoDB.Driver

Module Program
    Public mongo_client As MongoClient
    Public db As MongoDatabase



    Public ReadOnly DiscordAPI As String = "https://discord.com/api/v8/"
    Sub Main(args As String())
        If Config.LoadConfig() = False Then
            Config.NewConfig()
            Console.WriteLine("No config found... Press any key to close")
            Console.Read()
            End
        End If
        If Not Directory.Exists("data") Then
            Directory.CreateDirectory("data")
        End If
        If Not Directory.Exists("data/guilds") Then
            Directory.CreateDirectory("data/guilds")
        End If
        If Not Directory.Exists("data/tags") Then
            Directory.CreateDirectory("data/tags")
        End If
        If Not Directory.Exists("data/langs") Then
            Directory.CreateDirectory("data/langs")
        End If

        mongo_client = New MongoClient(Config.CurrentConfig.mongodb)
        db = mongo_client.GetServer().GetDatabase("source_servers")

        Lang.Load()
        Bot.PrepareBot()
        Bot.Start().GetAwaiter.GetResult()

        Dim tcs = New CancellationTokenSource()
        Dim ceen_config = New ServerConfig().AddLogger(New CLFStdOut()).
            AddRoute(GetType(Web.API.V2.Webhooks.Interactions).Assembly.ToRoute(New ControllerRouterConfig(GetType(Web.API.V2.Webhooks.Interactions)))).
            AddRoute(GetType(Web.Index).Assembly.ToRoute(New ControllerRouterConfig(GetType(Web.Index))))
        Dim ceen_task = HttpServer.ListenAsync(New IPEndPoint(IPAddress.Any, Config.CurrentConfig.ApplicationPort), False, ceen_config, tcs.Token)

        While True
            Console.ReadLine()
        End While
    End Sub
End Module
