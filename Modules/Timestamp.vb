Module Timestamp
    Function GetMSTimestamp(date_ As Date) As Long
        Return CLng(date_.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
    End Function
    Function GetTimestamp(date_ As Date) As Long
        Return CLng(date_.Subtract(New DateTime(1970, 1, 1)).TotalSeconds)
    End Function
End Module
