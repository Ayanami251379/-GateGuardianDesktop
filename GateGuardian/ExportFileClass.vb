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
    Public MustOverride Sub MakeScanEntryExport()
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

    End Sub

    Public Overrides Sub MakeScanEntryExport()
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
    Private GcWorkSheet As Worksheet
    'Private GcWorkBookPart As WorkbookPart
    'Private GcWorkSheetPart As WorksheetPart
    'Private GcSheets As Sheets
    'Private GcSheetData As SheetData
    'Private GcSheet As Sheet
    '
    'Private GcSharedStringTablePart As SharedStringTablePart
    'Private GcWorkbookStylesPart As WorkbookStylesPart

#Region "Other Guy's Code"
    ''' <summary>
    ''' Creates the workbook
    ''' </summary>
    ''' <returns>Spreadsheet created</returns>
    Private Function CreateWorkbook(fileName As String) As SpreadsheetDocument
        Dim spreadSheet As SpreadsheetDocument = Nothing
        Dim sharedStringTablePart As SharedStringTablePart
        Dim workbookStylesPart As WorkbookStylesPart

        Try
            ' Create the Excel workbook
            spreadSheet = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook, False)

            ' Create the parts and the corresponding objects
            ' Workbook
            spreadSheet.AddWorkbookPart()
            spreadSheet.WorkbookPart.Workbook = New Workbook()
            spreadSheet.WorkbookPart.Workbook.Save()

            ' Shared string table
            sharedStringTablePart = spreadSheet.WorkbookPart.AddNewPart(Of SharedStringTablePart)()
            sharedStringTablePart.SharedStringTable = New SharedStringTable()
            sharedStringTablePart.SharedStringTable.Save()

            ' Sheets collection
            spreadSheet.WorkbookPart.Workbook.Sheets = New DocumentFormat.OpenXml.Spreadsheet.Sheets()
            spreadSheet.WorkbookPart.Workbook.Save()

            ' Stylesheet
            workbookStylesPart = spreadSheet.WorkbookPart.AddNewPart(Of WorkbookStylesPart)()
            workbookStylesPart.Stylesheet = New Stylesheet()
            workbookStylesPart.Stylesheet.Save()
        Catch exception As System.Exception
            'System.Windows.MessageBox.Show(exception.Message, "Excel OpenXML basics", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand)
            spreadSheet = Nothing
        End Try

        Return spreadSheet
    End Function

    ''' <summary>
    ''' Adds a new worksheet to the workbook
    ''' </summary>
    ''' <param name="spreadsheet">Spreadsheet to use</param>
    ''' <param name="name">Name of the worksheet</param>
    ''' <returns>True if succesful</returns>
    Private Function AddWorksheet(spreadsheet As SpreadsheetDocument, name As String) As Boolean
        Dim sheets As Sheets = spreadsheet.WorkbookPart.Workbook.GetFirstChild(Of Sheets)()
        Dim sheet As Sheet
        Dim worksheetPart As WorksheetPart

        ' Add the worksheetpart
        worksheetPart = spreadsheet.WorkbookPart.AddNewPart(Of WorksheetPart)()
        worksheetPart.Worksheet = New Worksheet(New SheetData())
        worksheetPart.Worksheet.Save()

        ' Add the sheet and make relation to workbook
        sheet = New Sheet With {
       .Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
       .SheetId = (spreadsheet.WorkbookPart.Workbook.Sheets.Count() + 1),
       .Name = name}
        sheets.Append(sheet)
        spreadsheet.WorkbookPart.Workbook.Save()

        Return True
    End Function

    ''' <summary>
    ''' Add a single string to shared strings table.
    ''' Shared string table is created if it doesn't exist.
    ''' </summary>
    ''' <param name="spreadsheet">Spreadsheet to use</param>
    ''' <param name="stringItem">string to add</param>
    ''' <param name="save">Save the shared string table</param>
    ''' <returns></returns>
    Private Function AddSharedString(spreadsheet As SpreadsheetDocument, stringItem As String, Optional save As Boolean = True) As Boolean
        Dim sharedStringTable As SharedStringTable = spreadsheet.WorkbookPart.SharedStringTablePart.SharedStringTable

        Dim stringQuery = (From item In sharedStringTable
                           Where item.InnerText = stringItem
                           Select item).Count()

        If 0 = stringQuery Then
            sharedStringTable.AppendChild(
           New DocumentFormat.OpenXml.Spreadsheet.SharedStringItem(
              New DocumentFormat.OpenXml.Spreadsheet.Text(stringItem)))

            ' Save the changes
            If save Then
                sharedStringTable.Save()
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' Converts a column number to column name (i.e. A, B, C..., AA, AB...)
    ''' </summary>
    ''' <param name="columnIndex">Index of the column</param>
    ''' <returns>Column name</returns>
    Private Function ColumnNameFromIndex(columnIndex As UInt32) As String
        Dim remainder As UInt32
        Dim columnName As String = ""

        While (columnIndex > 0)
            remainder = (columnIndex - 1) Mod 26
            columnName = System.Convert.ToChar(65 + remainder).ToString() + columnName
            columnIndex = ((columnIndex - remainder) / 26)
        End While

        Return columnName
    End Function
#End Region
    Public Overrides Sub CreateFile(FileName As String)
        GsFullPath = FileName
        Dim FileNameParts As String() = FileName.Split("\")
        GsFilename = FileNameParts(FileNameParts.Length - 1)
        FileNameParts.RemoveAt(FileNameParts.Length - 1)
        GsPath = Join(FileNameParts, "\")

        If IO.File.Exists(GsFullPath) Then IO.File.Delete(GsFullPath)

        GcDocument = CreateWorkbook(GsFullPath)

        ''Create Document
        'GcDocument = SpreadsheetDocument.Create(GsFullPath, SpreadsheetDocumentType.Workbook)
        '
        ''Create Workbook    
        'GcWorkBookPart = GcDocument.AddWorkbookPart()
        'GcWorkBookPart.Workbook = New Workbook()
        'GcWorkBookPart.Workbook.Save()
        '
        ''Create Shared Strings Table
        'GcSharedStringTablePart = GcWorkBookPart.AddNewPart(Of SharedStringTablePart)()
        'GcSharedStringTablePart.SharedStringTable = New SharedStringTable()
        'GcSharedStringTablePart.SharedStringTable.Save()
        '
        ''Create Sheets
        'GcSheets = GcWorkBookPart.AddNewPart(Of Sheets)()
        'GcWorkSheetPart.Worksheet = New Worksheet(New SheetData())
        '
        'GcSheets = New Sheets()
        'GcSheets = GcDocument.WorkbookPart.Workbook.AppendChild(Of Sheets)(New Sheets())
        '
        'GcSheet = New Sheet
        'GcSheet.Id = GcDocument.WorkbookPart.GetIdOfPart(GcWorkSheetPart)
        ''GcSheet.SheetId = 1
        ''GcSheet.Name = "Scan Entries"
        '
        'GcSheetData = New SheetData()
    End Sub



    Private Function createCell(ByVal Value As String, ByVal ValueType As CellValues) As Cell
        Dim cCell As New Cell
        cCell.DataType = ValueType
        cCell.CellValue = New CellValue(Value)
        Return cCell
    End Function

    Private Function createCell(ByVal Value As String) As Cell
        Return createCell(Value, CellValues.String)
    End Function

    Private Function createCell(ByVal Value As Double) As Cell
        Return createCell(String.Format("{0}", Value), CellValues.Number)
    End Function

    Private Function createCell(ByVal Value As Boolean) As Cell
        Return createCell(Value, CellValues.Boolean)
    End Function

    Private Function createCell(ByVal Value As Date, Optional ByVal IsTime As Boolean = False) As Cell
        If IsTime Then
            Return createCell(String.Format("{0:HH:mm:ss}", Value), CellValues.Date)
        Else
            Return createCell(String.Format("{0:yyyy-MM-dd}", Value), CellValues.Date)
        End If
    End Function

    Public Overrides Sub MakeScanEntryExport()
        'GcSheet.SheetId = 1
        'GcSheet.Name = "Scan Entries"

        Dim cRow As New Row
        cRow.Append(createCell("ID"))
        cRow.Append(createCell("Code"))
        cRow.Append(createCell("CodeType"))
        cRow.Append(createCell("IsBarcode"))
        cRow.Append(createCell("IsIDPassport"))
        cRow.Append(createCell("Latitude"))
        cRow.Append(createCell("Longitude"))
        cRow.Append(createCell("Accuracy"))
        cRow.Append(createCell("Date"))
        cRow.Append(createCell("Time"))

        'GcSheetData.Append(cRow)
    End Sub

    Public Overrides Sub AppendScanEntry(Entry As ScanEntry)
        Dim cRow As New Row
        cRow.Append(createCell(Entry.ID))
        cRow.Append(createCell(Entry.Code))
        cRow.Append(createCell(Entry.CodeType))
        cRow.Append(createCell(Entry.IsBarcode))
        cRow.Append(createCell(Entry.IsIDPassport))
        cRow.Append(createCell(Entry.Latitude))
        cRow.Append(createCell(Entry.Longitude))
        cRow.Append(createCell(Entry.Accuracy))
        cRow.Append(createCell(Entry.ScannedDate))
        cRow.Append(createCell(Entry.ScannedDate, True))
        'GcSheetData.Append(cRow)
    End Sub

    Public Overrides Sub FinalizeFile()

        'GcSheet.Append(GcSheetData)
        '
        'GcSheets.Append(GcSheet)
        '
        'GcSheets.Append(GcSheet)
        '
        'GcWorkBookPart.Workbook.Save()
        '
        GcDocument.Close()
    End Sub
End Class



