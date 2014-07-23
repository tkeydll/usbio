
Namespace usbiolib

    ''' <summary>
    ''' usbiolibのラッパークラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UsbioWrapper

        Private Const USBIO_PWR_OFF As Byte = &H0
        Private Const USBIO_PWR_ON As Byte = &H1

        ''' <summary>
        ''' usbiolib
        ''' </summary>
        ''' <remarks></remarks>
        Private _usbio As usbiolib = New usbiolib()


        ''' <summary>
        ''' デバイスをオープンします。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function OpenDevice() As Boolean
            Return Me._usbio.openDevice()
        End Function


        ''' <summary>
        ''' J0の電源をOff/Onします。
        ''' </summary>
        ''' <param name="power"></param>
        ''' <remarks></remarks>
        Public Sub SendRecv(power As Boolean)
            Dim sendData As Byte() = New Byte() {}
            Dim recvData As Byte() = New Byte() {}

            sendData(0) = &H20
            sendData(1) = &H1
            If (power = True) Then
                sendData(2) = USBIO_PWR_ON
            Else
                sendData(2) = USBIO_PWR_OFF
            End If
            sendData(3) = &H0

            Me._usbio.SendRecv(sendData, recvData)
        End Sub

        ''' <summary>
        ''' 電源を落として制御を終了します。
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloseDevice()
            If Not (Me._usbio Is Nothing) Then
                Me.SendRecv(False)
                Me._usbio.closeDevice()
            End If
        End Sub

    End Class

End Namespace
