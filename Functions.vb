Imports System.Text.RegularExpressions

Module Functions
    Public Function StringToByteArray(s As String) As Byte()
        ' remove any spaces from, e.g. "A0 20 34 34"
        s = s.Replace(" "c, "")
        ' make sure we have an even number of digits
        If (s.Length And 1) = 1 Then
            Throw New FormatException("Odd string length when even string length is required.")
        End If

        ' calculate the length of the byte array and dim an array to that
        Dim nBytes = s.Length \ 2
        Dim a(nBytes - 1) As Byte

        ' pick out every two bytes and convert them from hex representation
        For i = 0 To nBytes - 1
            a(i) = Convert.ToByte(s.Substring(i * 2, 2), 16)
        Next

        Return a

    End Function
    Public URL_Regex As New Regex("(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])", RegexOptions.Compiled)
    Function GetInvitesFromText(entry As String) As List(Of String)
        Dim reply As New List(Of String)
        Dim modified_entry As String = entry
        modified_entry = modified_entry.Replace("discord.com/invite/", "discord.gg/")
        modified_entry = modified_entry.Replace("discord.gg/", "https://discord.gg/")
        modified_entry = modified_entry.Replace("@everyone", "")
        modified_entry = modified_entry.Replace("@here", "")
        For Each inv In URL_Regex.Matches(modified_entry)
            If inv.ToString.StartsWith("https://discord.gg/") Then
                reply.Add(inv.ToString.Replace("https://discord.gg/", ""))
            End If
        Next
        Return reply
    End Function

End Module
