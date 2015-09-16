Public Class KeyDelayEntry

    Public value As Keys
    Public delayRemaining As Integer
    Public Sub New(inValue As Keys, inDelayRemaining As Integer)
        value = inValue
        delayRemaining = inDelayRemaining
    End Sub
End Class
