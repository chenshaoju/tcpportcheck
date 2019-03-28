Imports System.Net.Sockets

Public Class Form1
    Private Function _CheckTCPPort(ByVal sIPAdress As String, ByVal iPort As Integer, Optional ByVal iTimeout As Integer = 1000)
        Dim socket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        ' Connect using a timeout (default 5 seconds)
        Dim result As IAsyncResult = socket.BeginConnect(sIPAdress, iPort, Nothing, Nothing)
        Dim success As Boolean = result.AsyncWaitHandle.WaitOne(iTimeout, True)
        If Not success Then
            ' NOTE, MUST CLOSE THE SOCKET
            socket.Close()
            Return False
        End If
        Return True
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Text = "测试中"
        If _CheckTCPPort(TextBox1.Text, TextBox2.Text) = True Then
            Label1.Text = "通"
        Else
            Label1.Text = "不通"
        End If

    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class
