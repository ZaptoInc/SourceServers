Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Builders

Namespace Database
    Module InviteModule
        Private ReadOnly database As MongoCollection = db.GetCollection(Of BsonDocument)("invites")

        Function GetInvite(code As String) As Invite
            Dim query = New QueryDocument("_id", code)
            Dim items = database.FindAs(Of BsonDocument)(query).SetLimit(1)
            If items.Count = 1 Then
                Return BsonToObject(Of Invite)(items.First)
            Else
                Return Nothing
            End If
        End Function

        Function GetInvitesFromGuildID(id As ULong) As List(Of Invite)
            Dim query = New QueryDocument("guildid", id)
            Dim items = database.FindAs(Of BsonDocument)(query)
            Dim results As New List(Of Invite)
            If items.Count >= 1 Then
                For Each item In items
                    results.Add(BsonToObject(Of Invite)(item))
                Next
                Return results
            Else
                Return Nothing
            End If
        End Function

        Function GetWorkingInvitesFromGuildID(id As ULong) As List(Of Invite)
            Dim query = New QueryDocument("guildid", id) From {
                {"working", True}
            }
            Dim items = database.FindAs(Of BsonDocument)(query)
            Dim results As New List(Of Invite)
            For Each item In items
                    results.Add(BsonToObject(Of Invite)(item))
                Next
                Return results
        End Function

        Function GetOldestWorkingInvite(Optional limit As Integer = 1) As List(Of Invite)
            Dim query = New QueryDocument("working", True)
            Dim items = database.FindAs(Of BsonDocument)(query).SetSortOrder(SortBy.Ascending("timestamp")).SetLimit(limit)
            Dim results As New List(Of Invite)
            For Each item In items
                results.Add(BsonToObject(Of Invite)(item))
            Next
            Return results
        End Function

        Sub SaveInvite(inv As Invite)
            If inv IsNot Nothing Then
                Dim item As Invite = GetInvite(inv._id)
                Dim bson = ObjectToBson(inv)
                If item Is Nothing Then
                    database.Insert(bson)
                Else
                    Dim query = New QueryDocument("_id", inv._id)
                    Dim update = New UpdateDocument(bson)
                    database.Update(query, update)
                End If
            End If
        End Sub

        Sub DeleteInvite(code As String)
            Dim item As Invite = GetInvite(code)
            If item Is Nothing Then
                Dim query = New QueryDocument("_id", code)
                database.Remove(query)
            End If
        End Sub

    End Module
End Namespace

