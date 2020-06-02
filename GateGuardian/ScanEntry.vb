Imports Newtonsoft.Json.Linq

Public Class ScanEntry
    Public ReadOnly Property ID As String
        Get
            Return GsID
        End Get
    End Property
    Private GsID As String = ""

    Public ReadOnly Property Code As String
        Get
            Return GsCode
        End Get
    End Property
    Private GsCode As String = ""

    Public ReadOnly Property CodeType As String
        Get
            Return GsCodeType
        End Get
    End Property
    Private GsCodeType As String = ""

    Public ReadOnly Property IsBarcode As Boolean
        Get
            Return GbIsBarcode
        End Get
    End Property
    Private GbIsBarcode As Boolean = False

    Public ReadOnly Property IsIDPassport As Boolean
        Get
            Return GbIsIDPassport
        End Get
    End Property
    Private GbIsIDPassport As Boolean = False

    Public ReadOnly Property ScannedDate As Date
        Get
            Return GdtScannedDate
        End Get
    End Property
    Private GdtScannedDate As Date = Now

    Public ReadOnly Property Latitude As Double
        Get
            Return GdLatitude
        End Get
    End Property
    Private GdLatitude As Double = 0

    Public ReadOnly Property Longitude As Double
        Get
            Return GdLongitude
        End Get
    End Property
    Private GdLongitude As Double = 0

    Public ReadOnly Property Accuracy As Double
        Get
            Return GdAccuracy
        End Get
    End Property
    Private GdAccuracy As Double = 0

    Public Sub New()

    End Sub

    Private Function getString(ByVal token As JToken, ByVal Key As String) As String
        Try
            Return token.Item(Key).Value(Of String).Trim
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function getDouble(ByVal token As JToken, ByVal Key As String) As Double
        Try
            Return token.Item(Key).Value(Of Double)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function getBool(ByVal token As JToken, ByVal Key As String) As String
        Try
            'If token.Item(Key).ToString.Trim = "true" Then
            '    Return True
            'Else
            '    Return False
            'End If
            Return token.Item(Key).Value(Of Boolean)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function getDate(ByVal token As JToken, ByVal Key As String) As String
        Try
            Dim TokenDate As JToken = token.Item(Key)
            Dim Year As Integer = TokenDate.Item("year").Value(Of Integer) + 1900
            Dim Month As Integer = TokenDate.Item("month").Value(Of Integer) + 1
            Dim Day As Integer = TokenDate.Item("date").Value(Of Integer)
            Dim Hour As Integer = TokenDate.Item("hours").Value(Of Integer)
            Dim Minute As Integer = TokenDate.Item("minutes").Value(Of Integer)
            Dim Second As Integer = TokenDate.Item("seconds").Value(Of Integer)

            Return New Date(Year, Month, Day, Hour, Minute, Second)
        Catch ex As Exception
            Return Now
        End Try
    End Function

    Public Sub New(ByVal token As JToken)
        GsID = getString(token, "ID")
        GsCode = getString(token, "Code")
        GsCodeType = getString(token, "CodeType")
        GbIsBarcode = getBool(token, "IsBarcode")
        GbIsIDPassport = getBool(token, "IsIDPassport")
        GdtScannedDate = getDate(token, "ScannedDate")
        GdLatitude = getDouble(token, "Latitude")
        GdLongitude = getDouble(token, "Longitude")
        GdAccuracy = getDouble(token, "Accuracy")
    End Sub
End Class
