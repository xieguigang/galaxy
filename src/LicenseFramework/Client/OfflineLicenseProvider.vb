'''
''' ---------------------------------------------------------------
''' 离线授权提供程序 (OfflineLicenseProvider.vb) - 客户端
''' 处理离线授权：导出指纹文件、导入许可证密钥、缓存管理
''' ---------------------------------------------------------------
'''
Imports System
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports LicenseFramework.Shared

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 离线授权提供程序
    ''' 
    ''' 离线授权流程：
    ''' 1. 采集硬件信息 → 生成硬件指纹哈希
    ''' 2. 导出指纹文件（txt）→ 客户发送给厂商
    ''' 3. 厂商生成许可证文件 → 发送给客户
    ''' 4. 客户导入许可证 → 验证并缓存
    ''' </summary>
    Public Class OfflineLicenseProvider

        Private _hwCollector As New HardwareInfoCollector()
        Private _fpGenerator As New FingerprintGenerator()
        Private _validator As LicenseValidator
        Private _cacheDir As String
        Private Const CACHE_FILENAME As String = "license.dat"

        Public Sub New(publicKeyXml As String, Optional cacheDirectory As String = Nothing)
            _validator = New LicenseValidator(publicKeyXml)

            If String.IsNullOrEmpty(cacheDirectory) Then
                ' 默认缓存到用户AppData目录
                Dim appDataDir As String = Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData)
                _cacheDir = Path.Combine(appDataDir, "LicenseFramework", "Cache")
            Else
                _cacheDir = cacheDirectory
            End If

            ' 确保缓存目录存在
            If Not Directory.Exists(_cacheDir) Then
                Directory.CreateDirectory(_cacheDir)
            End If
        End Sub

        ''' <summary>
        ''' 获取当前机器的硬件指纹哈希
        ''' </summary>
        Public Function GetHardwareFingerprint() As String
            Dim hwInfo As HardwareInfo = _hwCollector.Collect()
            Return _fpGenerator.GenerateFingerprint(hwInfo)
        End Function

        ''' <summary>
        ''' 导出硬件指纹请求文件
        ''' </summary>
        Public Function ExportFingerprintFile(productName As String,
                                               productVersion As String,
                                               filePath As String) As String
            Dim hwInfo As HardwareInfo = _hwCollector.Collect()
            Return _fpGenerator.ExportFingerprintFile(hwInfo, productName, productVersion, filePath)
        End Function

        ''' <summary>
        ''' 通过对话框导出指纹文件
        ''' </summary>
        Public Sub ExportFingerprintWithDialog(productName As String, productVersion As String)
            Using dlg As New SaveFileDialog With {
                .Filter = "文本文件|*.txt|所有文件|*.*",
                .FileName = $"hardware_fingerprint_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                .Title = "导出硬件指纹请求文件"
            }
                If dlg.ShowDialog() = DialogResult.OK Then
                    ExportFingerprintFile(productName, productVersion, dlg.FileName)
                    MessageBox.Show($"硬件指纹文件已导出到:" & Environment.NewLine & dlg.FileName &
                                    Environment.NewLine & Environment.NewLine &
                                    "请将此文件发送给软件供应商以获取许可证。",
                                    "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' 从许可证文件导入并验证
        ''' </summary>
        Public Function ImportLicenseFile(filePath As String) As LicenseValidationResult
            If Not File.Exists(filePath) Then
                Return LicenseValidationResult.Fail(LicenseStatus.NotFound, "许可证文件不存在")
            End If

            Try
                Dim signedLicense As String = File.ReadAllText(filePath, Encoding.UTF8)
                Dim currentFingerprint As String = GetHardwareFingerprint()
                Dim result As LicenseValidationResult = _validator.Validate(signedLicense, currentFingerprint)

                If result.IsValid Then
                    ' 验证通过，缓存许可证
                    CacheLicenseString(signedLicense)
                End If

                Return result

            Catch ex As Exception
                Return LicenseValidationResult.Fail(LicenseStatus.UnknownError, ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' 通过对话框导入许可证文件
        ''' </summary>
        Public Function ImportLicenseWithDialog() As LicenseValidationResult
            Using dlg As New OpenFileDialog With {
                .Filter = "许可证文件|*.lic;*.txt|所有文件|*.*",
                .Title = "导入许可证文件"
            }
                If dlg.ShowDialog() = DialogResult.OK Then
                    Return ImportLicenseFile(dlg.FileName)
                End If
            End Using

            Return LicenseValidationResult.Fail(LicenseStatus.NotFound, "未选择许可证文件")
        End Function

        ''' <summary>
        ''' 从缓存加载并验证许可证
        ''' </summary>
        Public Function ValidateCachedLicense() As LicenseValidationResult
            Dim cacheFile As String = Path.Combine(_cacheDir, CACHE_FILENAME)

            If Not File.Exists(cacheFile) Then
                Return LicenseValidationResult.Fail(LicenseStatus.NotFound, "未找到本地缓存的许可证")
            End If

            Try
                Dim signedLicense As String = File.ReadAllText(cacheFile, Encoding.UTF8)
                Dim currentFingerprint As String = GetHardwareFingerprint()
                Return _validator.Validate(signedLicense, currentFingerprint)
            Catch ex As Exception
                Return LicenseValidationResult.Fail(LicenseStatus.UnknownError, ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' 缓存许可证字符串
        ''' </summary>
        Public Sub CacheLicenseString(signedLicense As String)
            Try
                Dim cacheFile As String = Path.Combine(_cacheDir, CACHE_FILENAME)
                File.WriteAllText(cacheFile, signedLicense, Encoding.UTF8)
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"缓存许可证字符串失败: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' 清除本地缓存的许可证
        ''' </summary>
        Public Sub ClearCachedLicense()
            Try
                Dim cacheFile As String = Path.Combine(_cacheDir, CACHE_FILENAME)
                If File.Exists(cacheFile) Then
                    File.Delete(cacheFile)
                End If
            Catch ex As Exception
                Diagnostics.Debug.WriteLine($"清除许可证缓存失败: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' 检查是否存在缓存的许可证
        ''' </summary>
        Public ReadOnly Property HasCachedLicense As Boolean
            Get
                Return File.Exists(Path.Combine(_cacheDir, CACHE_FILENAME))
            End Get
        End Property

    End Class

End Namespace
