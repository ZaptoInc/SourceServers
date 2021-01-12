Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports Newtonsoft.Json

Module Bson
    Function ObjectToBson(obj As Object) As BsonDocument
        Dim json = JsonConvert.SerializeObject(obj)
        Dim result As BsonDocument = BsonDocument.Parse(json)
        Return result
    End Function

    Function BsonToObject(Of T)(bson As BsonDocument) As T
        Dim result As T = BsonSerializer.Deserialize(Of T)(bson)
        Return result
    End Function
End Module
