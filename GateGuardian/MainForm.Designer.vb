﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.btnStudents = New System.Windows.Forms.Button()
        Me.btnParents = New System.Windows.Forms.Button()
        Me.btnScanLog = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnStudents
        '
        Me.btnStudents.Location = New System.Drawing.Point(12, 12)
        Me.btnStudents.Name = "btnStudents"
        Me.btnStudents.Size = New System.Drawing.Size(155, 40)
        Me.btnStudents.TabIndex = 0
        Me.btnStudents.Text = "Students"
        Me.btnStudents.UseVisualStyleBackColor = True
        '
        'btnParents
        '
        Me.btnParents.Location = New System.Drawing.Point(12, 58)
        Me.btnParents.Name = "btnParents"
        Me.btnParents.Size = New System.Drawing.Size(155, 40)
        Me.btnParents.TabIndex = 1
        Me.btnParents.Text = "Parents"
        Me.btnParents.UseVisualStyleBackColor = True
        '
        'btnScanLog
        '
        Me.btnScanLog.Location = New System.Drawing.Point(12, 104)
        Me.btnScanLog.Name = "btnScanLog"
        Me.btnScanLog.Size = New System.Drawing.Size(155, 40)
        Me.btnScanLog.TabIndex = 2
        Me.btnScanLog.Text = "Scan Log"
        Me.btnScanLog.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(178, 156)
        Me.Controls.Add(Me.btnScanLog)
        Me.Controls.Add(Me.btnParents)
        Me.Controls.Add(Me.btnStudents)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gate Guardian"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnStudents As Button
    Friend WithEvents btnParents As Button
    Friend WithEvents btnScanLog As Button
End Class
