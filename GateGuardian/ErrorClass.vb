Public Class ErrorClass
    Public Shared Function ExtractErrors(ByVal exception As Exception) As List(Of String)
        Dim returnList As New List(Of String)
        Dim inner As Exception = Nothing

        Try
            If exception.Message IsNot Nothing Then
                returnList.Add(exception.Message.Trim)
            End If

            inner = exception.InnerException

            While inner IsNot Nothing
                If inner.Message IsNot Nothing Then
                    returnList.Add(inner.Message.Trim)
                End If

                inner = inner.InnerException
            End While

        Catch ex As Exception
            returnList.Add(ex.Message)
        End Try

        Return returnList
    End Function

    Public Shared Function ExtractErrors(ByVal exceptionList As List(Of String)) As String
        Try
            Dim sb As New Text.StringBuilder
            For iFor As Integer = 1 To exceptionList.Count
                Dim Line As String = exceptionList(iFor - 1)
                If Line IsNot Nothing Then
                    If iFor = exceptionList.Count Then
                        sb.Append(Line)
                    Else
                        sb.Append(Line)
                        sb.Append(vbNewLine)
                    End If
                End If
            Next
            Return sb.ToString
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class
