Public Class DiscordEmbed
    Public title As String
    Public description As String
    Public url As String
    Public timestamp As Date
    Public color As Integer
    Public footer As DiscordEmbedFooter
    Public image As DiscordEmbedImage
    Public thumbnail As DiscordEmbedImage
    Public author As DiscordEmbedAuthor
    Public fields As New List(Of DiscordEmbedFields)

    Public Function WithTitle(title As String) As DiscordEmbed
        Me.title = title
        Return Me
    End Function

    Public Function WithDescription(description As String) As DiscordEmbed
        Me.description = description
        Return Me
    End Function

    Public Function WithUrl(url As String) As DiscordEmbed
        Me.url = url
        Return Me
    End Function

    Public Function WithTimestamp(timestamp As Date) As DiscordEmbed
        Me.timestamp = timestamp
        Return Me
    End Function

    Public Function WithCurrentTimestamp() As DiscordEmbed
        timestamp = Date.UtcNow
        Return Me
    End Function

    Public Function WithColor(color As Integer) As DiscordEmbed
        Me.color = color
        Return Me
    End Function

    Public Function WithColor(color As Drawing.Color) As DiscordEmbed
        Me.color = color.ToArgb
        Return Me
    End Function

    Public Function WithFooter(text As String, Optional icon_url As String = Nothing) As DiscordEmbed
        footer = New DiscordEmbedFooter(text, icon_url)
        Return Me
    End Function

    Public Function WithImage(url As String) As DiscordEmbed
        Me.image = New DiscordEmbedImage(url)
        Return Me
    End Function

    Public Function WithThumbnail(url As String) As DiscordEmbed
        Me.thumbnail = New DiscordEmbedImage(url)
        Return Me
    End Function

    Public Function WithAuthor(Optional name As String = Nothing, Optional url As String = Nothing, Optional icon_url As String = Nothing)
        Me.author = New DiscordEmbedAuthor(name, url, icon_url)
    End Function

    Public Function AddField(name As String, value As String, Optional inline As Boolean = False)
        Me.fields.Add(New DiscordEmbedFields(name, value, inline))
    End Function
End Class

Public Class DiscordEmbedFooter
    Public text As String
    Public icon_url As String

    Sub New(text As String, icon_url As String)
        Me.text = text
        Me.icon_url = icon_url
    End Sub
End Class

Public Class DiscordEmbedImage
    Public url As String
    Sub New(url As String)
        Me.url = url

    End Sub
End Class

Public Class DiscordEmbedAuthor
    Public name As String
    Public url As String
    Public icon_url As String
    Sub New(name As String, url As String, icon_url As String)
        Me.name = name
        Me.url = url
        Me.icon_url = icon_url
    End Sub
End Class

Public Class DiscordEmbedFields
    Public name As String
    Public value As String
    Public inline As Boolean = False
    Sub New(name As String, value As String, Optional inline As Boolean = False)
        Me.name = name
        Me.value = value
        Me.inline = inline
    End Sub
End Class
