Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Builders

Namespace Database
    Module GuildModule
        Private ReadOnly database As MongoCollection = db.GetCollection(Of BsonDocument)("guilds")

        Function GetGuild(id As ULong) As Guild
            Dim query = New QueryDocument("_id", id)
            Dim items = database.FindAs(Of BsonDocument)(query).SetLimit(1)
            If items.Count = 1 Then
                Return BsonToObject(Of Guild)(items.First)
            Else
                Return Nothing
            End If
        End Function

        Function GetOrCreateGuild(id As ULong) As Guild
            Dim query = New QueryDocument("_id", id)
            Dim items = database.FindAs(Of BsonDocument)(query).SetLimit(1)
            If items.Count = 1 Then
                Return BsonToObject(Of Guild)(items.First)
            Else
                Return New Guild(id)
            End If
        End Function

        Sub SaveGuild(guild As Guild)
            If guild IsNot Nothing Then
                Dim item As Guild = GetGuild(guild._id)
                Dim bson = ObjectToBson(guild)
                If item Is Nothing Then
                    database.Insert(bson)
                Else
                    Dim query = New QueryDocument("_id", guild._id)
                    Dim update = New UpdateDocument(bson)
                    database.Update(query, update)
                End If
            End If
        End Sub

        Sub DeleteGuild(id As ULong)
            Dim item As Guild = GetGuild(id)
            If item Is Nothing Then
                Dim query = New QueryDocument("_id", id)
                database.Remove(query)
            End If
        End Sub

    End Module
End Namespace

