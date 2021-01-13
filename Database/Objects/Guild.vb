Namespace Database
    Public Class Guild
        Public _id As ULong
        Public lang As String = "en"
        Public monitors As New MonitorList

        Sub New(id As ULong)
            _id = id
        End Sub
    End Class

    Public Class MonitorList
        Public perms As New Dictionary(Of ULong, MonitorPermission)
        Public defaultperms As New MonitorPermission
        Public monitors As New Dictionary(Of String, Monitor)

        Sub Save(monitor_ As Monitor)
            If monitors.ContainsKey(monitor_.name) Then
                monitors.Remove(monitor_.name)
            End If
            monitors.Add(monitor_.name, monitor_)
        End Sub

        Sub Delete(monitor_name As String)
            If monitors.ContainsKey(monitor_name) Then
                monitors.Remove(monitor_name)
            End If
        End Sub

        Function GetOrCreatePerms(userid As ULong) As MonitorPermission
            If perms.ContainsKey(userid) Then
                Return perms(userid)
            Else
                Return New MonitorPermission
            End If
        End Function

        Sub SavePerms(userid As ULong, perm_ As MonitorPermission)
            If perms.ContainsKey(userid) Then
                perms.Remove(userid)
            End If
            perms.Add(userid, perm_)
        End Sub

        Sub DeletePerms(userid As ULong)
            If perms.ContainsKey(userid) Then
                perms.Remove(userid)
            End If
        End Sub
    End Class
    Public Class MonitorPermission
        Public host As Boolean? = Nothing
        Public message As Boolean? = Nothing
        Public creation As Boolean? = Nothing
        Public deletion As Boolean? = Nothing
        Public userperms As Boolean? = Nothing
        Public viewinfos As Boolean? = Nothing
        Sub New()

        End Sub

        Sub New(withdefault As Boolean)
            host = withdefault
            message = withdefault
            creation = withdefault
            deletion = withdefault
            userperms = withdefault
            viewinfos = withdefault
        End Sub

        Function Merge(from As MonitorPermission) As MonitorPermission
            If from.host.HasValue Then Me.host = from.host
            If from.message.HasValue Then Me.message = from.message
            If from.creation.HasValue Then Me.creation = from.creation
            If from.deletion.HasValue Then Me.deletion = from.deletion
            If from.userperms.HasValue Then Me.userperms = from.userperms
            If from.viewinfos.HasValue Then Me.viewinfos = from.viewinfos
            Return Me
        End Function
    End Class
    Public Class Monitor
        Public name As String
        Public IP As String
        Public port As Integer
        Public perms As New Dictionary(Of ULong, MonitorPermission)
        Public defaultperms As New MonitorPermission
        Public channelid As ULong = Nothing
        Public messageid As ULong = Nothing

        Function GetOrCreatePerms(userid As ULong) As MonitorPermission
            If perms.ContainsKey(userid) Then
                Return perms(userid)
            Else
                Return New MonitorPermission
            End If
        End Function

        Sub SavePerms(userid As ULong, perm_ As MonitorPermission)
            If perms.ContainsKey(userid) Then
                perms.Remove(userid)
            End If
            perms.Add(userid, perm_)
        End Sub



        Sub DeletePerms(userid As ULong)
            If perms.ContainsKey(userid) Then
                perms.Remove(userid)
            End If
        End Sub
    End Class
End Namespace

