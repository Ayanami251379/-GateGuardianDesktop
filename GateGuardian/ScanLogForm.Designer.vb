<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScanLogForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lbEntries = New System.Windows.Forms.ListBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnClearServer = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lbEntries
        '
        Me.lbEntries.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbEntries.FormattingEnabled = True
        Me.lbEntries.Location = New System.Drawing.Point(0, 0)
        Me.lbEntries.Name = "lbEntries"
        Me.lbEntries.Size = New System.Drawing.Size(843, 446)
        Me.lbEntries.TabIndex = 0
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(676, 452)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(155, 40)
        Me.btnExport.TabIndex = 3
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnClearServer
        '
        Me.btnClearServer.Location = New System.Drawing.Point(12, 452)
        Me.btnClearServer.Name = "btnClearServer"
        Me.btnClearServer.Size = New System.Drawing.Size(155, 40)
        Me.btnClearServer.TabIndex = 4
        Me.btnClearServer.Text = "Clear Server"
        Me.btnClearServer.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(318, 452)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(155, 40)
        Me.btnRefresh.TabIndex = 5
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'ScanLogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 504)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.btnClearServer)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lbEntries)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ScanLogForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Scan Log"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lbEntries As ListBox
    Friend WithEvents btnExport As Button
    Friend WithEvents btnClearServer As Button
    Friend WithEvents btnRefresh As Button
End Class
