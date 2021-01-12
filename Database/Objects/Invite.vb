Namespace Database
    Public Class Invite
        Public _id As String = Guid.NewGuid.ToString
        Public guildid As ULong = 0
        Public checkedat As Date = New Date(1970, 1, 1)
        Public timestamp As Long = GetTimestamp(checkedat)
        Public working As Boolean = False

        Sub SetDate(d As Date)
            checkedat = d
            timestamp = GetTimestamp(checkedat)
        End Sub
    End Class
End Namespace

