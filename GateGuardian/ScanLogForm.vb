Imports System.ComponentModel
Imports System.Text
Imports Firebase.Database
Imports Firebase.Database.Query
Imports Newtonsoft.Json.Linq

Public Class ScanLogForm
    Private GbStartedWorker As Boolean = False
    Private WithEvents bw_logs As BackgroundWorker

    Private GbFirebaseDB As FirebaseClient

    Private GlScanEntries As List(Of ScanEntry)

    Public Sub runForm(ByVal FirebaseDB As FirebaseClient)
        GbFirebaseDB = FirebaseDB
        Me.ShowDialog()
    End Sub

    Private Sub ScanLogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not GbStartedWorker Then
            GbStartedWorker = True
            setControlsEnabled(False)
            bw_logs = New BackgroundWorker
            bw_logs.WorkerReportsProgress = True
            bw_logs.WorkerSupportsCancellation = True
            bw_logs.RunWorkerAsync()
        End If
    End Sub

    Private Sub setControlsEnabled(ByVal enabled As Boolean)
        btnClearServer.Enabled = enabled
        btnExport.Enabled = enabled
        btnRefresh.Enabled = enabled
        lbEntries.Enabled = enabled
    End Sub

    Private Async Function getLogs() As Task(Of List(Of ScanEntry))
        Try
            Dim returnList As New List(Of ScanEntry)
            Dim result = Await FirebaseHelper.GetScans(GbFirebaseDB).OnceSingleAsync(Of Object)

            Dim mainlist As JObject = JObject.Parse(result.ToString)

            For Each listItem As JToken In mainlist.Children
                returnList.Add(New ScanEntry(listItem.First))
            Next

            If returnList.Count = 0 Then
                returnList = Nothing
            End If

            Return returnList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub bw_logs_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw_logs.DoWork
        Try
            Dim doGetLogsTask As Task(Of List(Of ScanEntry)) = getLogs()
            doGetLogsTask.Wait()
            GlScanEntries = doGetLogsTask.Result

        Catch ex As Exception

        End Try
    End Sub

    Private Sub bw_logs_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw_logs.RunWorkerCompleted
        lbEntries.Items.Clear()

        If Not IsNothing(GlScanEntries) Then

            For Each entry As ScanEntry In GlScanEntries
                Dim sb As New StringBuilder
                sb.Append(PadR(entry.ID, 20))
                sb.Append(PadR(entry.Code, 30))
                sb.Append(PadR(entry.CodeType, 10))

                If entry.IsBarcode Then
                    sb.Append("B ")
                Else
                    sb.Append("O ")
                End If

                sb.Append(PadR(String.Format("{0:yyyy-MM-dd}", entry.ScannedDate), 11))
                sb.Append(PadR(String.Format("{0:HH:mm:ss}", entry.ScannedDate), 9))
                sb.Append(PadR(String.Format("{0}", entry.Latitude), 21))
                sb.Append(PadR(String.Format("{0}", entry.Longitude), 21))
                sb.Append(PadR(String.Format("{0}", entry.Accuracy), 4))

                lbEntries.Items.Add(sb.ToString)
            Next

        End If

        setControlsEnabled(True)
    End Sub

    Private Sub ScanLogForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SafeCloseWorker(bw_logs)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim sfd As New SaveFileDialog

        sfd.Filter = "Comma-Seperated Value File|*.csv"
        sfd.FileName = String.Format("ScanData_{0:yyyyMMddHHmmss}.csv", Now)
        sfd.InitialDirectory = Application.StartupPath

        If sfd.ShowDialog() = DialogResult.OK Then
            Dim fileName As String = sfd.FileName
            Dim ExportFile As ExportFileClass

            If IO.File.Exists(fileName) Then IO.File.Delete(fileName)

            Select Case GetFileType(fileName)
                Case FileType.Excel : ExportFile = New ExcelFileClass()
                Case Else : ExportFile = New CSVFileClass()
            End Select

            ExportFile.CreateFile(fileName)

            For Each entry As ScanEntry In GlScanEntries
                ExportFile.AppendScanEntry(entry)
            Next

            ExportFile.FinalizeFile()
        End If

    End Sub

    Private Sub appendFileLine(ByVal FileName As String, ByVal Line As String)
        Try
            IO.File.AppendAllText(FileName, Line.Trim & vbNewLine)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        GbStartedWorker = True
        setControlsEnabled(False)
        bw_logs = New BackgroundWorker
        bw_logs.WorkerReportsProgress = True
        bw_logs.WorkerSupportsCancellation = True
        bw_logs.RunWorkerAsync()
    End Sub
End Class