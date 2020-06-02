Imports System.ComponentModel

Module MainModule

    Public Enum FileType
        Unknown = -1
        PlainText = 0
        CSV = 1
        Excel = 2
    End Enum



    Public Function PadR(ByVal Line As String, ByVal Padding As Integer)
        Return Left(Line.Trim & Space(Padding), Padding)
    End Function

    Public Function PadL(ByVal Line As String, ByVal Padding As Integer)
        Return Right(Space(Padding) & Line.Trim, Padding)
    End Function

    Public Sub SafeCloseWorker(ByRef BW_Worker As BackgroundWorker)
        Try
            Try
                BW_Worker.CancelAsync()
            Catch ex As Exception

            End Try
            BW_Worker = Nothing
        Catch ex As Exception

        End Try
    End Sub

    Public Function GetFileType(ByVal FileName As String) As FileType
        Dim fileparts As String() = FileName.Split(".")
        Select Case fileparts(fileparts.Length - 1).Trim.ToLower
            Case "txt" : Return FileType.PlainText
            Case "csv" : Return FileType.CSV
            Case "xlsx" : Return FileType.Excel
            Case Else : Return FileType.Unknown
        End Select
    End Function






    <System.Runtime.CompilerServices.Extension()>
    Public Sub RemoveAt(Of T)(ByRef arr As T(), ByVal index As Integer)
        Dim uBound = arr.GetUpperBound(0)
        Dim lBound = arr.GetLowerBound(0)
        Dim arrLen = uBound - lBound

        If index < lBound OrElse index > uBound Then
            Throw New ArgumentOutOfRangeException(
            String.Format("Index must be from {0} to {1}.", lBound, uBound))

        Else
            'create an array 1 element less than the input array
            Dim outArr(arrLen - 1) As T
            'copy the first part of the input array
            Array.Copy(arr, 0, outArr, 0, index)
            'then copy the second part of the input array
            Array.Copy(arr, index + 1, outArr, index, uBound - index)

            arr = outArr
        End If
    End Sub
End Module
