Imports Galaxy.Workbench.CommonDialogs
Imports Galaxy.Workbench.LicenseFramework.Shared
Imports Windows.Win32.System

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 授权管理器，许可证系统的统一入口
    ''' 
    ''' 验证策略：先离线后在线
    ''' 1. 首先尝试从本地缓存加载并验证许可证（离线模式）
    ''' 2. 如果离线验证失败，则尝试在线授权
    ''' 3. 在线授权成功后自动缓存许可证
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' 授权管理器 (LicenseManager.vb) - 客户端统一入口
    ''' 协调离线授权和在线授权，实现"先离线后在线"的验证策略
    ''' ---------------------------------------------------------------
    '''
    Public Class LicenseManager

        Private _offlineProvider As OfflineLicenseProvider
        Private _onlineProvider As OnlineLicenseProvider
        Private _productName As String
        Private _productVersion As String
        Private _enableOnline As Boolean
        Private _lastResult As LicenseValidationResult

        ''' <summary>
        ''' 构造函数（完整版，支持离线+在线双模式）
        ''' </summary>
        Public Sub New(publicKeyXml As String,
                        productName As String,
                        productVersion As String,
                        Optional serverUrl As String = Nothing,
                        Optional hmacKey As String = Nothing,
                        Optional cacheDirectory As String = Nothing)
            _productName = productName
            _productVersion = productVersion

            ' 初始化离线授权提供程序
            _offlineProvider = New OfflineLicenseProvider(publicKeyXml, cacheDirectory)

            ' 初始化在线授权提供程序（如果提供了服务器URL）
            _enableOnline = Not String.IsNullOrEmpty(serverUrl) AndAlso Not String.IsNullOrEmpty(hmacKey)
            If _enableOnline Then
                _onlineProvider = New OnlineLicenseProvider(publicKeyXml, serverUrl, hmacKey, _offlineProvider)
            End If
        End Sub

        ''' <summary>
        ''' 执行授权验证（先离线后在线）
        ''' </summary>
        Public Function Validate() As LicenseValidationResult
            ' 第一步：尝试离线验证（从缓存加载）
            _lastResult = _offlineProvider.ValidateCachedLicense()

            If _lastResult.IsValid Then
                Diagnostics.Debug.WriteLine("离线授权验证通过")
                Return _lastResult
            End If

            Diagnostics.Debug.WriteLine($"离线验证失败: {_lastResult.Message}")

            ' 第二步：尝试在线授权
            If _enableOnline Then
                Diagnostics.Debug.WriteLine("尝试在线授权...")
                _lastResult = _onlineProvider.RequestOnlineLicense(_productName, _productVersion)

                If _lastResult.IsValid Then
                    Diagnostics.Debug.WriteLine("在线授权验证通过")
                    Return _lastResult
                End If

                Diagnostics.Debug.WriteLine($"在线授权失败: {_lastResult.Message}")
            End If

            Return _lastResult
        End Function

        ''' <summary>
        ''' 执行授权验证，失败时弹出授权对话框
        ''' </summary>
        Public Function ValidateWithDialog() As LicenseValidationResult
            _lastResult = Validate()

            If _lastResult.IsValid Then
                Return _lastResult
            End If

            ' 验证失败，弹出授权对话框
            Call OpenLicenseDialog()

            Return _lastResult
        End Function

        Public Sub OpenLicenseDialog()
            Dim dlg As New LicenseDialog()

            Call dlg.SetLicenseData(_offlineProvider, _onlineProvider, _productName, _productVersion, _lastResult)
            Call InputDialog.Input(Sub(dlg2)
                                       If DirectCast(dlg, LicenseDialog).IsAuthorized Then
                                           _lastResult = Validate()
                                       End If
                                   End Sub, config:=dlg)
        End Sub

        ''' <summary>
        ''' 手动激活离线授权（弹出导入对话框）
        ''' </summary>
        Public Function ActivateOffline() As LicenseValidationResult
            _lastResult = _offlineProvider.ImportLicenseWithDialog()
            Return _lastResult
        End Function

        ''' <summary>
        ''' 手动激活在线授权
        ''' </summary>
        Public Function ActivateOnline() As LicenseValidationResult
            If _onlineProvider Is Nothing Then
                Return LicenseValidationResult.Fail(LicenseStatus.OnlineVerificationFailed,
                                                    "未配置在线授权服务")
            End If
            Return _onlineProvider.RequestOnlineLicense(_productName, _productVersion)
        End Function

        ''' <summary>
        ''' 导出硬件指纹文件
        ''' </summary>
        Public Sub ExportFingerprint()
            _offlineProvider.ExportFingerprintWithDialog(_productName, _productVersion)
        End Sub

        ''' <summary>
        ''' 注销当前许可证
        ''' </summary>
        Public Sub Deactivate()
            _offlineProvider.ClearCachedLicense()
            _lastResult = Nothing
        End Sub

        Public ReadOnly Property LastValidationResult As LicenseValidationResult
            Get
                Return _lastResult
            End Get
        End Property

        Public ReadOnly Property CurrentLicense As LicenseData
            Get
                If _lastResult IsNot Nothing AndAlso _lastResult.IsValid Then
                    Return _lastResult.License
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property IsLicensed As Boolean
            Get
                Return _lastResult IsNot Nothing AndAlso _lastResult.IsValid
            End Get
        End Property

    End Class

End Namespace
