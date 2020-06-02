Imports Firebase.Auth
Imports Firebase.Database

Public Class FirebaseHelper
    Private Shared apiKey As String = "AIzaSyBJI_FyA5vl6wbszTa7HPDSq1GLINqdQlg"
    Private Shared authDomain As String = "gate-guardian-d5416.firebaseapp.com"
    Private Shared databaseURL As String = "https://gate-guardian-d5416.firebaseio.com"
    Private Shared projectId As String = "gate-guardian-d5416"
    Private Shared storageBucket As String = "gate-guardian-d5416.appspot.com"
    Private Shared messagingSenderId As String = "308491852194"
    Private Shared appId As String = "1:308491852194:web:96e019c02fd5668ddabafb"
    Private Shared measurementId As String = "G-J8BKC7XJY8"

    Private Shared AuthEmail As String = "firebasesimpleauth@firebase.com"
    Private Shared AuthPassword As String = "2x9FKuYEaVPQ4vyo"



    Public Shared Function GetFirebase() As FirebaseClient
        Dim fbresult As Task(Of FirebaseClient) = FirebaseWrapper.Auth.AuthorizedDatabaseAsync(databaseURL, apiKey, AuthEmail, AuthPassword)
        fbresult.Wait()
        Return fbresult.Result
    End Function

    Public Shared Function GetScans(ByRef FB_Client As FirebaseClient) As Firebase.Database.Query.ChildQuery
        Return FB_Client.Child("Scans")
    End Function

End Class
