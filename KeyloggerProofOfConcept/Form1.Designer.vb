<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Me.GoTimer = New System.Windows.Forms.Timer(Me.components)
        Me.output = New System.Windows.Forms.RichTextBox()
        Me.dataFile = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'GoTimer
        '
        Me.GoTimer.Enabled = True
        Me.GoTimer.Interval = 1
        '
        'output
        '
        Me.output.Location = New System.Drawing.Point(25, 12)
        Me.output.Name = "output"
        Me.output.Size = New System.Drawing.Size(16, 16)
        Me.output.TabIndex = 0
        Me.output.Text = ""
        '
        'dataFile
        '
        Me.dataFile.Enabled = True
        Me.dataFile.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(142, 42)
        Me.Controls.Add(Me.output)
        Me.Name = "Form1"
        Me.Opacity = 0.0R
        Me.ShowInTaskbar = False
        Me.Text = "Nope.avi"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GoTimer As System.Windows.Forms.Timer
    Friend WithEvents output As System.Windows.Forms.RichTextBox
    Friend WithEvents dataFile As System.Windows.Forms.Timer

End Class
