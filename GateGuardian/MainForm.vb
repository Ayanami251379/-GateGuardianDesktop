Imports System.ComponentModel
Imports Firebase.Database

Public Class MainForm

    Private GbStartedWorker As Boolean = False
    Private WithEvents bw_FirebaseFetcher As BackgroundWorker
    Private GcFirebaseDB As FirebaseClient

    Private GsLastError As String = ""

    Private Sub btnStudents_Click(sender As Object, e As EventArgs) Handles btnStudents.Click

    End Sub

    Private Sub btnScanLog_Click(sender As Object, e As EventArgs) Handles btnScanLog.Click
        Dim f As New ScanLogForm
        f.runForm(GcFirebaseDB)
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub setControlsEnabled(ByVal enabled As Boolean)
        btnStudents.Enabled = enabled
        btnParents.Enabled = enabled
        btnScanLog.Enabled = enabled
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not GbStartedWorker Then
            GbStartedWorker = True

            setControlsEnabled(False)

            bw_FirebaseFetcher = New BackgroundWorker
            bw_FirebaseFetcher.WorkerReportsProgress = True
            bw_FirebaseFetcher.WorkerSupportsCancellation = True
            bw_FirebaseFetcher.RunWorkerAsync()
        End If
    End Sub

    Private Sub bw_FirebaseFetcher_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw_FirebaseFetcher.DoWork
        Try
            GsLastError = ""
            GcFirebaseDB = FirebaseHelper.GetFirebase
        Catch ex As Exception
            GcFirebaseDB = Nothing
            GsLastError = ErrorClass.ExtractErrors(ErrorClass.ExtractErrors(ex))
        End Try

    End Sub

    Private Sub bw_FirebaseFetcher_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw_FirebaseFetcher.RunWorkerCompleted
        If IsNothing(GcFirebaseDB) Then
            MsgBox(GsLastError, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Unable To Connect")
            Application.Exit()
        Else
            setControlsEnabled(True)
        End If
    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SafeCloseWorker(bw_FirebaseFetcher)
    End Sub
End Class
