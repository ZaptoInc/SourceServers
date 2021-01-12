Imports Ceen.Mvc
Imports Ceen
Imports Newtonsoft.Json.Linq
Imports System.Net

Namespace Web
    Public Class Index
        Inherits Controller
        Public Function Index() As IResult
            'Context.Response.Redirect(Config.CurrentConfig.WebsiteDomain)
            'Return Status(307)
        End Function
        '<Route("{tag}")>
        'Public Function IndexTag(tag As String) As IResult
        '    Dim client As WebClient = New WebClient()
        '    Dim guild_id As String
        '    If IO.File.Exists($"data/tags/{tag}.json") Then
        '        Dim tag_json As JObject = JObject.Parse(IO.File.ReadAllText($"data/tags/{tag}.json"))
        '        guild_id = tag_json("id").ToString
        '    Else
        '        guild_id = tag
        '    End If
        '    Dim dm_guild As JObject = JObject.Parse(client.DownloadString($"{Config.CurrentConfig.APIDomain}guild/{guild_id}?token={Config.CurrentConfig.APItoken}"))
        '    If Not dm_guild("error").ToObject(Of Boolean) Then
        '        Context.Response.Redirect($"{Config.CurrentConfig.WebsiteDomain}server/info/{guild_id}")
        '        Return Status(301)
        '    Else
        '        Context.Response.Redirect($"{Config.CurrentConfig.WebsiteDomain}/404")
        '        Return Status(307)
        '    End If
        'End Function
        '<Route("{tag}/{reviewid}")>
        'Public Function IndexTag(tag As String, reviewid As String) As IResult
        '    Dim client As WebClient = New WebClient()
        '    Dim guild_id As String
        '    If IO.File.Exists($"data/tags/{tag}.json") Then
        '        Dim tag_json As JObject = JObject.Parse(IO.File.ReadAllText($"data/tags/{tag}.json"))
        '        guild_id = tag_json("id").ToString
        '    Else
        '        guild_id = tag
        '    End If
        '    Dim dm_guild As JObject = JObject.Parse(client.DownloadString($"{Config.CurrentConfig.APIDomain}guild/{guild_id}?token={Config.CurrentConfig.APItoken}"))
        '    If Not dm_guild("error").ToObject(Of Boolean) Then
        '        Context.Response.Redirect($"{Config.CurrentConfig.WebsiteDomain}server/info/{guild_id}#review-{reviewid}")
        '        Return Status(301)
        '    Else
        '        Context.Response.Redirect($"{Config.CurrentConfig.WebsiteDomain}404")
        '        Return Status(307)
        '    End If
        'End Function
    End Class
End Namespace

