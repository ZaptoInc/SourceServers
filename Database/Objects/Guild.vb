Namespace Database
    Public Class Guild
        Public _id As ULong
        Public main_tag As String = Nothing
        Public reviewlogs As ULong = Nothing
        Public reviewcategory As ULong = Nothing
        Public reviewnote As ULong = Nothing
        Public reviewnumber As ULong = Nothing
        Public lang As String = "en"
        Public welcomedm As Boolean = True
        Public autoinvite As Boolean = False
    End Class

    Public Class WelcomeMessage
        Public content As String
    End Class
End Namespace

