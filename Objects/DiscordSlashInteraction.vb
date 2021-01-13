Public Class DiscordSlashInteraction
    Public type As Integer
    Public token As String
    Public member As DiscordSlashInteractionMember
    Public id As String
    Public guild_id As String
    Public data As DiscordSlashInteractionData
    Public channel_id As String
End Class

Public Class DiscordSlashInteractionMember
    Public user As DiscordUser
    Public roles As New List(Of String)
    Public premium_since As Date?
    Public permissions As String
    Public pending As Boolean
    Public nick As String
    Public mute As Boolean
    Public joined_at As Date?
    Public is_pending As Boolean
    Public deaf As Boolean
End Class

Public Class DiscordSlashInteractionData
    Public options As New List(Of DiscordSlashInteractionOption)
    Public name As String
    Public id As String
End Class

Public Class DiscordSlashInteractionOption
    Public name As String
    Public value As Object
    Public options As New List(Of DiscordSlashInteractionOption)
End Class