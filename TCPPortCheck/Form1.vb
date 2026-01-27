Imports System.Net
Imports System.Net.Sockets

Public Class Form1
    Private Function _CheckTCPPort(ByVal sHost As String, ByVal iPort As Integer, Optional ByVal iTimeout As Integer = 1000) As Boolean
        Dim ipAddresses() As IPAddress = Nothing

        ' Try to parse as IP address directly first
        Dim ipAddr As IPAddress = Nothing
        If IPAddress.TryParse(sHost, ipAddr) Then
            ipAddresses = New IPAddress() {ipAddr}
        Else
            ' Try to resolve domain name
            Try
                ipAddresses = Dns.GetHostAddresses(sHost)
            Catch ex As Exception
                ' DNS resolution failed
                Return False
            End Try
        End If

        If ipAddresses Is Nothing OrElse ipAddresses.Length = 0 Then Return False

        ' Try each IP address (IPv4 or IPv6)
        For Each ip As IPAddress In ipAddresses
            Dim socket As Socket = Nothing
            Try
                socket = New Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

                ' Connect using a timeout
                Dim result As IAsyncResult = socket.BeginConnect(ip, iPort, Nothing, Nothing)
                Dim success As Boolean = result.AsyncWaitHandle.WaitOne(iTimeout, True)

                If success Then
                    ' Check if connection was actually successful
                    Try
                        socket.EndConnect(result)
                        socket.Close()
                        Return True
                    Catch
                        ' Connection failed for this IP, try next
                    End Try
                End If
            Catch
                ' Socket creation or connection error, try next
            Finally
                If socket IsNot Nothing Then socket.Close()
            End Try
        Next

        Return False
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Text = "测试中"
        ' Use a background thread or similar to avoid UI freezing in a real app, 
        ' but keeping original logic simple for now.
        ' Force UI refresh to show "Testing" state
        Application.DoEvents() 
        
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
