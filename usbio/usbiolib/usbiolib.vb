Imports System.Runtime.InteropServices

Namespace usbio

    ''' <summary>
    ''' USB-IO 2.0 (AKI) 制御ライブラリ
    ''' </summary>
    ''' <remarks>
    ''' サンプルコードそのまま
    ''' </remarks>
    Public Class usbiolib

        Public Const DIGCF_PRESENT = &H2
        Public Const DIGCF_DEVICEINTERFACE = &H10

        Public Const FORMAT_MESSAGE_FROM_SYSTEM = &H1000
        Public Const GENERIC_READ = &H80000000
        Public Const GENERIC_WRITE = &H40000000
        Public Const FILE_SHARE_READ = &H1
        Public Const FILE_SHARE_WRITE = &H2
        Public Const OPEN_EXISTING = 3
        Public Const INVALID_HANDLE_VALUE = -1

        Public Const MyVendorID = &H1352        'Km2Net
        Public Const MyProductID = &H120        'USB-IO2.0
        Public Const MyProductID2 = &H121       'USB-IO2.0(AKI)

        <StructLayout(LayoutKind.Sequential, Pack:=1, CharSet:=CharSet.Ansi)> _
        Public Structure GUID
            Dim Data1 As Integer
            Dim Data2 As Short
            Dim Data3 As Short
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> Dim Data4() As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, pack:=1)> _
        Public Structure HIDD_ATTRIBUTES
            Dim Size As Integer
            Dim VendorID As Short
            Dim ProductID As Short
            Dim VersionNumber As Short
        End Structure

        <StructLayout(LayoutKind.Sequential, pack:=1)> _
        Public Structure HIDP_CAPS
            Dim Usage As Short
            Dim UsagePage As Short
            Dim InputReportByteLength As Short
            Dim OutputReportByteLength As Short
            Dim FeatureReportByteLength As Short
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> Dim Reserved() As Short
            Dim NumberLinkCollectionNodes As Short
            Dim NumberInputButtonCaps As Short
            Dim NumberInputValueCaps As Short
            Dim NumberInputDataIndices As Short
            Dim NumberOutputButtonCaps As Short
            Dim NumberOutputValueCaps As Short
            Dim NumberOutputDataIndices As Short
            Dim NumberFeatureButtonCaps As Short
            Dim NumberFeatureValueCaps As Short
            Dim NumberFeatureDataIndices As Short
        End Structure

        <StructLayout(LayoutKind.Sequential, pack:=1)> _
        Public Structure SECURITY_ATTRIBUTES
            Dim nLength As Integer
            Dim lpSecurityDescriptor As Integer
            Dim bInheritHandle As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=1, CharSet:=CharSet.Ansi)> _
        Public Structure SP_DEVICE_INTERFACE_DATA
            Dim cbSize As Integer
            Dim InterfaceClassGuid As GUID
            Dim Flags As Integer
            Dim Reserved As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=1, CharSet:=CharSet.Ansi)> _
        Public Structure SP_DEVINFO_DATA
            Dim cbSize As Integer
            Dim ClassGuid As GUID
            Dim DevInst As Integer
            Dim Reserved As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, pack:=1)> _
        Public Structure SP_DEVICE_INTERFACE_DETAIL_DATA
            Dim cbSize As Integer
            Dim DevicePath As Byte
        End Structure




        Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer

        Public Declare Function CreateFile _
            Lib "kernel32" _
            Alias "CreateFileA" _
            (ByVal lpFileName As String, _
            ByVal dwDesiredAccess As Integer, _
            ByVal dwShareMode As Integer, _
            ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, _
            ByVal dwCreationDisposition As Integer, _
            ByVal dwFlagsAndAttributes As Integer, _
            ByVal hTemplateFile As Integer) _
        As Integer

        Public Declare Function FormatMessage _
            Lib "kernel32" _
            Alias "FormatMessageA" _
            (ByVal dwFlags As Integer, _
            ByRef lpSource As Integer, _
            ByVal dwMessageId As Integer, _
            ByVal dwLanguageZId As Integer, _
            ByVal lpBuffer As String, _
            ByVal nSize As Integer, _
            ByVal Arguments As Integer) _
        As Integer

        Public Declare Function HidD_FreePreparsedData _
            Lib "hid.dll" _
            (ByRef PreparsedData As Integer) _
        As Integer

        Public Declare Function HidD_GetAttributes _
            Lib "hid.dll" _
            (ByVal HidDeviceObject As Integer, _
            ByRef Attributes As HIDD_ATTRIBUTES) _
        As Integer

        Public Declare Function HidD_GetHidGuid _
            Lib "hid.dll" _
            (ByRef HidGuid As GUID) _
        As Integer

        Public Declare Function HidD_GetPreparsedData _
            Lib "hid.dll" _
            (ByVal HidDeviceObject As Integer, _
            ByRef PreparsedData As Integer) _
        As Integer

        Public Declare Function HidP_GetCaps _
            Lib "hid.dll" _
            (ByVal PreparsedData As Integer, _
            ByRef Capabilities As HIDP_CAPS) _
        As Integer

        Public Declare Function HidP_GetValueCaps _
            Lib "hid.dll" _
            (ByVal ReportType As Short, _
            ByRef ValueCaps As Byte, _
            ByRef ValueCapsLength As Short, _
            ByVal PreparsedData As Integer) _
        As Integer

        Public Declare Function lstrcpy _
            Lib "kernel32" _
            Alias "lstrcpyA" _
            (ByVal dest As String, _
            ByVal source As Integer) _
        As String

        Public Declare Function lstrlen _
            Lib "kernel32" _
            Alias "lstrlenA" _
            (ByVal source As Integer) _
        As Integer

        Public Declare Function ReadFile _
            Lib "kernel32" _
            (ByVal hFile As Integer, _
            ByRef lpBuffer As Byte, _
            ByVal nNumberOfBytesToRead As Integer, _
            ByRef lpNumberOfBytesRead As Integer, _
            ByVal lpOverlapped As Integer) _
        As Integer

        Public Declare Sub RtlMoveMemory Lib "kernel32" (ByRef Destination As Byte, ByVal Source As IntPtr, ByVal Length As Short)

        Public Declare Function SetupDiCreateDeviceInfoList _
            Lib "setupapi.dll" _
            (ByRef ClassGuid As GUID, _
            ByVal hwndParent As Integer) _
        As Integer

        Public Declare Function SetupDiDestroyDeviceInfoList _
            Lib "setupapi.dll" _
            (ByVal DeviceInfoSet As Integer) _
        As Integer

        Public Declare Function SetupDiEnumDeviceInterfaces _
            Lib "setupapi.dll" _
            (ByVal DeviceInfoSet As Integer, _
            ByVal DeviceInfoData As Integer, _
            ByRef InterfaceClassGuid As GUID, _
            ByVal MemberIndex As Integer, _
            ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA) _
        As Integer

        Public Declare Function SetupDiGetClassDevs _
            Lib "setupapi.dll" _
            Alias "SetupDiGetClassDevsA" _
            (ByRef ClassGuid As GUID, _
            ByVal Enumerator As String, _
            ByVal hwndParent As Integer, _
            ByVal Flags As Integer) _
        As Integer

        Public Declare Function SetupDiGetDeviceInterfaceDetail _
           Lib "setupapi.dll" _
           Alias "SetupDiGetDeviceInterfaceDetailA" _
           (ByVal DeviceInfoSet As Integer, _
           ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
           ByVal DeviceInterfaceDetailData As Integer, _
           ByVal DeviceInterfaceDetailDataSize As Integer, _
           ByRef RequiredSize As Integer, _
           ByVal DeviceInfoData As Integer) _
        As Integer

        Public Declare Function WriteFile _
            Lib "kernel32" _
            (ByVal hFile As Integer, _
            ByRef lpBuffer As Byte, _
            ByVal nNumberOfBytesToWrite As Integer, _
            ByRef lpNumberOfBytesWritten As Integer, _
            ByVal lpOverlapped As Integer) _
        As Integer

        Public Declare Function GetTickCount Lib "kernel32" () As Integer


        Public HidDevice As Integer = INVALID_HANDLE_VALUE
        Public Capabilities As HIDP_CAPS



        Public Function openDevice() As Boolean
            Dim HidGuid As GUID
            Dim DeviceInfoSet As Integer
            Dim MyDeviceInterfaceData As SP_DEVICE_INTERFACE_DATA
            Dim MemberIndex As Integer
            Dim MyDeviceInfoData As SP_DEVINFO_DATA
            Dim Needed As Integer
            Dim DetailData As Integer
            Dim MyDeviceInterfaceDetailData As SP_DEVICE_INTERFACE_DETAIL_DATA
            Dim DetailDataBuffer() As Byte
            Dim gch As GCHandle
            Dim address As Integer
            Dim DevicePathName As String
            Dim sa As SECURITY_ATTRIBUTES
            Dim DeviceAttributes As HIDD_ATTRIBUTES
            Dim PreparsedData As Long

            Dim ipt As IntPtr
            Dim Result As Integer

            openDevice = False

            Result = HidD_GetHidGuid(HidGuid)
            DeviceInfoSet = SetupDiGetClassDevs _
                (HidGuid, vbNullString, 0, (DIGCF_PRESENT Or DIGCF_DEVICEINTERFACE))

            MemberIndex = 0

            Do
                MyDeviceInterfaceData.cbSize = Marshal.SizeOf(MyDeviceInterfaceData)
                Result = SetupDiEnumDeviceInterfaces _
                    (DeviceInfoSet, 0, HidGuid, MemberIndex, MyDeviceInterfaceData)

                If Result <> 0 Then
                    MyDeviceInfoData.cbSize = Marshal.SizeOf(MyDeviceInfoData)
                    Result = SetupDiGetDeviceInterfaceDetail _
                       (DeviceInfoSet, MyDeviceInterfaceData, 0, 0, Needed, 0)

                    DetailData = Needed
                    MyDeviceInterfaceDetailData.cbSize = Marshal.SizeOf(MyDeviceInterfaceDetailData)
                    ReDim DetailDataBuffer(Needed)
                    ipt = Marshal.AllocHGlobal(Marshal.SizeOf(MyDeviceInterfaceDetailData))
                    Marshal.StructureToPtr(MyDeviceInterfaceDetailData, ipt, False)
                    Call RtlMoveMemory(DetailDataBuffer(0), ipt, 4)

                    gch = GCHandle.Alloc(DetailDataBuffer, GCHandleType.Pinned)
                    address = gch.AddrOfPinnedObject().ToInt32()

                    Result = SetupDiGetDeviceInterfaceDetail _
                       (DeviceInfoSet, MyDeviceInterfaceData, address, DetailData, Needed, 0)

                    gch.Free()

                    DevicePathName = System.Text.Encoding.GetEncoding("Shift-JIS").GetString(DetailDataBuffer)
                    DevicePathName = DevicePathName.Substring(4)

                    sa.nLength = 12
                    sa.lpSecurityDescriptor = 0
                    sa.bInheritHandle = 0

                    HidDevice = CreateFile(DevicePathName, GENERIC_READ Or GENERIC_WRITE, (FILE_SHARE_READ Or FILE_SHARE_WRITE), _
                                            sa, OPEN_EXISTING, 0, 0)

                    If HidDevice <> INVALID_HANDLE_VALUE Then
                        DeviceAttributes.Size = Marshal.SizeOf(DeviceAttributes)
                        Result = HidD_GetAttributes(HidDevice, DeviceAttributes)

                        If (DeviceAttributes.VendorID = MyVendorID) _
                        And (DeviceAttributes.ProductID = MyProductID Or DeviceAttributes.ProductID = MyProductID2) Then

                            HidD_GetPreparsedData(HidDevice, PreparsedData)
                            HidP_GetCaps(PreparsedData, Capabilities)

                            openDevice = True
                            Exit Do
                        Else
                            Result = CloseHandle(HidDevice)
                        End If

                    End If
                Else
                    Exit Do
                End If
                MemberIndex = MemberIndex + 1
            Loop

        End Function

        Public Sub closeDevice()
            If HidDevice <> INVALID_HANDLE_VALUE Then
                CloseHandle(HidDevice)
            End If
        End Sub

        Public Sub SendRecv(ByVal sendData() As Byte, ByRef recvData() As Byte)
            Dim i As Integer
            Dim s As String
            Dim NumberOfBytesWritten As Integer
            Dim NumberOfBytesRead As Integer

            Dim wrtData(Capabilities.OutputReportByteLength - 1) As Byte
            Dim readData(Capabilities.InputReportByteLength - 1) As Byte

            On Error GoTo errJump

            wrtData(0) = &H0
            For i = 0 To 63
                wrtData(i + 1) = sendData(i)
            Next
            WriteFile(HidDevice, wrtData(0), Capabilities.OutputReportByteLength, NumberOfBytesWritten, 0)

            Do
                ReadFile(HidDevice, readData(0), Capabilities.InputReportByteLength, NumberOfBytesRead, 0)
                If wrtData(64) = readData(64) Then
                    Exit Do
                End If
            Loop

            For i = 0 To 63
                recvData(i) = readData(i + 1)
            Next

            Exit Sub
errJump:
            MsgBox(Err.Description)
        End Sub


    End Class

End Namespace
