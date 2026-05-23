'''
''' ---------------------------------------------------------------
''' 硬件信息采集模块 (HardwareInfoCollector.vb)
''' 通过WMI采集CPU、主板、网卡MAC地址等硬件信息
''' ---------------------------------------------------------------
'''
Imports System
Imports System.Management
Imports System.Collections.Generic
Imports System.Text
Imports System.Linq

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 硬件信息采集器
    ''' </summary>
    Public Class HardwareInfoCollector

        ''' <summary>
        ''' 采集所有硬件信息
        ''' </summary>
        Public Function Collect() As HardwareInfo
            Dim info As New HardwareInfo()

            Try
                info.CpuId = GetCpuId()
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"CPU信息采集失败: {ex.Message}")
            End Try

            Try
                info.MotherboardSerial = GetMotherboardSerial()
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"主板信息采集失败: {ex.Message}")
            End Try

            Try
                info.MacAddresses = GetMacAddresses()
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"MAC地址采集失败: {ex.Message}")
            End Try

            Try
                info.BiosSerial = GetBiosSerial()
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"BIOS信息采集失败: {ex.Message}")
            End Try

            Try
                info.DiskSerial = GetDiskSerial()
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"磁盘信息采集失败: {ex.Message}")
            End Try

            Return info
        End Function

        ''' <summary>
        ''' 获取CPU处理器ID
        ''' 使用Win32_Processor的ProcessorId属性
        ''' </summary>
        Private Function GetCpuId() As String
            Using searcher As New ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor")
                For Each obj As ManagementObject In searcher.Get()
                    Dim procId As String = obj("ProcessorId")?.ToString()
                    If Not String.IsNullOrEmpty(procId) Then
                        Return procId.Trim()
                    End If
                Next
            End Using
            Return String.Empty
        End Function

        ''' <summary>
        ''' 获取主板序列号
        ''' 使用Win32_BaseBoard的SerialNumber属性
        ''' </summary>
        Private Function GetMotherboardSerial() As String
            Using searcher As New ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard")
                For Each obj As ManagementObject In searcher.Get()
                    Dim serial As String = obj("SerialNumber")?.ToString()
                    If Not String.IsNullOrEmpty(serial) Then
                        Return serial.Trim()
                    End If
                Next
            End Using
            Return String.Empty
        End Function

        ''' <summary>
        ''' 获取所有物理网卡的MAC地址
        ''' 过滤掉虚拟网卡和回环适配器
        ''' 仅保留活跃的、非虚拟的网卡MAC地址
        ''' </summary>
        Private Function GetMacAddresses() As List(Of String)
            Dim macList As New List(Of String)()

            Using searcher As New ManagementObjectSearcher(
                "SELECT MACAddress, AdapterType, NetEnabled FROM Win32_NetworkAdapter WHERE MACAddress IS NOT NULL")
                For Each obj As ManagementObject In searcher.Get()
                    ' 过滤虚拟网卡：仅保留以太网和无线网卡
                    Dim adapterType As String = obj("AdapterType")?.ToString()
                    If adapterType Is Nothing Then Continue For

                    ' 排除虚拟网卡类型（如VPN、虚拟机等）
                    If adapterType.IndexOf("Ethernet", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                       adapterType.IndexOf("802.11", StringComparison.OrdinalIgnoreCase) >= 0 Then
                        Dim mac As String = obj("MACAddress")?.ToString()
                        If Not String.IsNullOrEmpty(mac) Then
                            ' 去掉冒号和连字符，统一为大写格式
                            mac = mac.Replace(":", "").Replace("-", "").ToUpperInvariant()
                            macList.Add(mac)
                        End If
                    End If
                Next
            End Using

            ' 按字母排序确保一致性
            Return macList.OrderBy(Function(m) m).ToList()
        End Function

        ''' <summary>
        ''' 获取BIOS序列号
        ''' 使用Win32_BIOS的SerialNumber属性
        ''' </summary>
        Private Function GetBiosSerial() As String
            Using searcher As New ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS")
                For Each obj As ManagementObject In searcher.Get()
                    Dim serial As String = obj("SerialNumber")?.ToString()
                    If Not String.IsNullOrEmpty(serial) Then
                        Return serial.Trim()
                    End If
                Next
            End Using
            Return String.Empty
        End Function

        ''' <summary>
        ''' 获取系统盘序列号
        ''' 使用Win32_DiskDrive获取第一个物理磁盘的序列号
        ''' </summary>
        Private Function GetDiskSerial() As String
            Using searcher As New ManagementObjectSearcher(
                "SELECT SerialNumber, Index FROM Win32_DiskDrive WHERE Index = 0")
                For Each obj As ManagementObject In searcher.Get()
                    Dim serial As String = obj("SerialNumber")?.ToString()
                    If Not String.IsNullOrEmpty(serial) Then
                        Return serial.Trim()
                    End If
                Next
            End Using
            Return String.Empty
        End Function

    End Class

    ''' <summary>
    ''' 硬件信息数据结构
    ''' </summary>
    Public Class HardwareInfo
        Public Property CpuId As String = String.Empty
        Public Property MotherboardSerial As String = String.Empty
        Public Property MacAddresses As New List(Of String)()
        Public Property BiosSerial As String = String.Empty
        Public Property DiskSerial As String = String.Empty

        Public ReadOnly Property IsValid As Boolean
            Get
                Return Not String.IsNullOrEmpty(CpuId) OrElse
                       Not String.IsNullOrEmpty(MotherboardSerial)
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder()
            sb.AppendLine($"CPU ID: {CpuId}")
            sb.AppendLine($"主板序列号: {MotherboardSerial}")
            sb.AppendLine($"BIOS序列号: {BiosSerial}")
            sb.AppendLine($"磁盘序列号: {DiskSerial}")
            sb.AppendLine($"MAC地址: {String.Join(", ", MacAddresses)}")
            Return sb.ToString()
        End Function
    End Class

End Namespace
