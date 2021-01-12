Public Class ApplicationInteractionsRegistry
    Implements InteractionsRegistry
    Private ReadOnly interactions_ As Dictionary(Of String, InteractionsExecutor)

    Public Sub New()
        interactions_ = New Dictionary(Of String, InteractionsExecutor)
    End Sub

    Public Sub Register(name As String, executor As InteractionsExecutor)
        If interactions_.ContainsKey(name) Then interactions_.Remove(name)
        interactions_.Add(name, executor)
    End Sub

    Public Function GetInteractions() As Dictionary(Of String, InteractionsExecutor) Implements InteractionsRegistry.GetInteractions
        Return interactions_
    End Function
End Class
