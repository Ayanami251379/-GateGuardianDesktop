Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet

Public MustInherit Class ExportFileClass

    Public ReadOnly Property FileName As String
        Get
            Return GsFilename
        End Get
    End Property
    Protected GsFilename As String = ""

    Public ReadOnly Property Path As String
        Get
            Return GsPath
        End Get
    End Property
    Protected GsPath As String = ""

    Public ReadOnly Property FullPath As String
        Get
            Return GsFullPath
        End Get
    End Property
    Protected GsFullPath As String = ""

    Public MustOverride Sub CreateFile(ByVal FileName As String)
    Public MustOverride Sub AppendScanEntry(ByVal Entry As ScanEntry)
    Public MustOverride Sub FinalizeFile()

End Class

Public Class CSVFileClass
    Inherits ExportFileClass

    Public Overrides Sub CreateFile(FileName As String)
        GsFullPath = FileName
        Dim FileNameParts As String() = FileName.Split("\")
        GsFilename = FileNameParts(FileNameParts.Length - 1)
        FileNameParts.RemoveAt(FileNameParts.Length - 1)
        GsPath = Join(FileNameParts, "\")

        If IO.File.Exists(GsFullPath) Then IO.File.Delete(GsFullPath)

        appendLine("SEP=,")

        Dim sb As New System.Text.StringBuilder()
        sb.Append("ID,")
        sb.Append("Code,")
        sb.Append("CodeType,")
        sb.Append("IsBarcode,")
        sb.Append("IsIDPassport,")
        sb.Append("Latitude,")
        sb.Append("Longitude,")
        sb.Append("Accuracy,")
        sb.Append("Date,")
        sb.Append("Time")
        appendLine(sb.ToString)
    End Sub

    Public Overrides Sub AppendScanEntry(Entry As ScanEntry)
        Dim sb As New System.Text.StringBuilder()
        sb.Append(Entry.ID).Append(",")
        sb.Append(Entry.Code).Append(",")
        sb.Append(Entry.CodeType).Append(",")
        sb.Append(Entry.IsBarcode).Append(",")
        sb.Append(Entry.IsIDPassport).Append(",")
        sb.Append(Entry.Latitude).Append(",")
        sb.Append(Entry.Longitude).Append(",")
        sb.Append(Entry.Accuracy).Append(",")
        sb.Append(String.Format("{0:yyyy-MM-dd}", Entry.ScannedDate)).Append(",")
        sb.Append(String.Format("{0:HH:mm:ss}", Entry.ScannedDate))
        appendLine(sb.ToString)
    End Sub

    Public Overrides Sub FinalizeFile()

    End Sub

    Private Sub appendLine(ByVal Line As String)
        Try
            IO.File.AppendAllText(GsFullPath, Line.Trim & vbNewLine)
        Catch ex As Exception

        End Try
    End Sub
End Class

Public Class ExcelFileClass
    Inherits ExportFileClass

    Private GcDocument As SpreadsheetDocument
    Private GcWorkBookPart As WorkbookPart
    Private GcWorkSheetPart As WorksheetPart
    Private GcSheets As Sheets

    Private GcSheet As Sheet

    Public Overrides Sub CreateFile(FileName As String)
        GsFullPath = FileName
        Dim FileNameParts As String() = FileName.Split("\")
        GsFilename = FileNameParts(FileNameParts.Length - 1)
        FileNameParts.RemoveAt(FileNameParts.Length - 1)
        GsPath = Join(FileNameParts, "\")

        If IO.File.Exists(GsFullPath) Then IO.File.Delete(GsFullPath)

        GcDocument = SpreadsheetDocument.Create(GsFullPath, SpreadsheetDocumentType.Workbook)
        GcWorkBookPart = GcDocument.AddWorkbookPart()
        GcWorkBookPart.Workbook = New Workbook()
        GcWorkSheetPart = GcWorkBookPart.AddNewPart(Of WorksheetPart)()
        GcWorkSheetPart.Worksheet = New Worksheet(New SheetData())

        GcSheets = GcDocument.WorkbookPart.Workbook.AppendChild(Of Sheets)(New Sheets())

        GcSheet = New Sheet
        GcSheet.Id = GcDocument.WorkbookPart.GetIdOfPart(GcWorkSheetPart)
        GcSheet.SheetId = 1
        GcSheet.Name = "Scan Entries"


    End Sub

    Public Overrides Sub AppendScanEntry(Entry As ScanEntry)

    End Sub

    Public Overrides Sub FinalizeFile()
        GcSheets.Append(GcSheet)

        GcWorkBookPart.Workbook.Save()

        GcDocument.Close()
    End Sub
End Class