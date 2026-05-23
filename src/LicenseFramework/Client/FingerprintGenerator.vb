Imports System
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 硬件指纹生成器
    ''' 
    ''' 脱敏原理：
    ''' 1. 将硬件信息拼接为原始字符串
    ''' 2. 使用SHA-256计算哈希摘要
    ''' 3. 哈希是单向的，无法从哈希反推出原始硬件信息
    ''' 4. 相同的硬件信息始终产生相同的哈希值
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' 指纹哈希生成与脱敏模块 (FingerprintGenerator.vb)
    ''' 将硬件信息转换为不可逆的哈希摘要，实现数据脱敏
    ''' ---------------------------------------------------------------
    '''
    Public Class FingerprintGenerator

        Private Const FIELD_SEPARATOR As String = "|#|"

        ''' <summary>
        ''' 生成硬件指纹哈希
        ''' </summary>
        Public Function GenerateFingerprint(hardwareInfo As HardwareInfo) As String
            Dim rawData As String = BuildRawString(hardwareInfo)
            Dim hash As String = ComputeSha256Hash(rawData)
            Return hash
        End Function

        ''' <summary>
        ''' 生成指纹并导出为脱敏文本文件
        ''' 该文件由客户发送给厂商用于离线授权
        ''' </summary>
        Public Function ExportFingerprintFile(hardwareInfo As HardwareInfo,
                                               productName As String,
                                               productVersion As String,
                                               filePath As String) As String
            Dim fingerprint As String = GenerateFingerprint(hardwareInfo)

            Dim sb As New StringBuilder()
            sb.AppendLine("========================================")
            sb.AppendLine("  硬件授权请求文件 - 请勿修改此文件内容")
            sb.AppendLine("========================================")
            sb.AppendLine()
            sb.AppendLine($"产品名称: {productName}")
            sb.AppendLine($"产品版本: {productVersion}")
            sb.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
            sb.AppendLine()
            sb.AppendLine("----------------------------------------")
            sb.AppendLine("硬件指纹哈希 (Hardware Fingerprint Hash):")
            sb.AppendLine(fingerprint)
            sb.AppendLine("----------------------------------------")
            sb.AppendLine()
            sb.AppendLine("说明: 请将此文件完整发送给软件供应商以获取授权密钥。")
            sb.AppendLine("      文件中的哈希值已脱敏处理，不会泄露您的硬件信息。")

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8)
            Return fingerprint
        End Function

        ''' <summary>
        ''' 将硬件信息拼接为原始字符串
        ''' 使用固定顺序和分隔符确保一致性
        ''' </summary>
        Private Function BuildRawString(hardwareInfo As HardwareInfo) As String
            Dim sb As New StringBuilder()

            ' CPU ID
            sb.Append("CPU=")
            sb.Append(hardwareInfo.CpuId)
            sb.Append(FIELD_SEPARATOR)

            ' 主板序列号
            sb.Append("MB=")
            sb.Append(hardwareInfo.MotherboardSerial)
            sb.Append(FIELD_SEPARATOR)

            ' MAC地址（取排序后的第一个，确保一致性）
            sb.Append("MAC=")
            If hardwareInfo.MacAddresses.Count > 0 Then
                Dim sortedMacs = hardwareInfo.MacAddresses.OrderBy(Function(m) m).ToList()
                sb.Append(sortedMacs(0))
            End If
            sb.Append(FIELD_SEPARATOR)

            ' BIOS序列号
            sb.Append("BIOS=")
            sb.Append(hardwareInfo.BiosSerial)
            sb.Append(FIELD_SEPARATOR)

            ' 磁盘序列号
            sb.Append("DISK=")
            sb.Append(hardwareInfo.DiskSerial)

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' 计算SHA-256哈希值
        ''' </summary>
        Private Function ComputeSha256Hash(input As String) As String
            Using sha256 As SHA256 = SHA256.Create()
                Dim inputBytes As Byte() = Encoding.UTF8.GetBytes(input)
                Dim hashBytes As Byte() = sha256.ComputeHash(inputBytes)

                Dim sb As New StringBuilder()
                For Each b As Byte In hashBytes
                    sb.Append(b.ToString("x2"))
                Next

                Return sb.ToString()
            End Using
        End Function

        ''' <summary>
        ''' 验证当前硬件指纹是否与给定的指纹匹配
        ''' </summary>
        Public Function VerifyFingerprint(hardwareInfo As HardwareInfo,
                                          expectedFingerprint As String) As Boolean
            Dim currentFingerprint As String = GenerateFingerprint(hardwareInfo)
            Return String.Equals(currentFingerprint, expectedFingerprint,
                                 StringComparison.OrdinalIgnoreCase)
        End Function

    End Class

End Namespace
