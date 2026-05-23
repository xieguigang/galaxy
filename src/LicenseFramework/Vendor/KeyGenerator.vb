Imports System.IO
Imports System.Text
Imports LicenseVendor.LicenseFramework.Shared

Namespace LicenseFramework.Vendor

    ''' <summary>
    ''' RSA密钥对生成工具
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' RSA密钥对生成工具 (KeyGenerator.vb) - 厂商端
    ''' 生成RSA 2048位密钥对，并保存为文件
    ''' ---------------------------------------------------------------
    '''
    Public Class KeyGenerator

        ''' <summary>
        ''' 生成RSA 2048位密钥对并保存到文件
        ''' </summary>
        Public Shared Sub GenerateAndSave(privateKeyFilePath As String, publicKeyFilePath As String)
            Dim publicKeyXml As String = String.Empty
            Dim privateKeyXml As String = String.Empty

            CryptoHelper.GenerateRsaKeyPair(publicKeyXml, privateKeyXml)

            File.WriteAllText(privateKeyFilePath, privateKeyXml, Encoding.UTF8)
            File.WriteAllText(publicKeyFilePath, publicKeyXml, Encoding.UTF8)
        End Sub

        ''' <summary>
        ''' 生成HMAC密钥并保存到文件
        ''' </summary>
        Public Shared Function GenerateHmacKey() As String
            Dim keyBytes As Byte() = CryptoHelper.GenerateRandomKey(32)
            Return Convert.ToBase64String(keyBytes)
        End Function

    End Class

End Namespace
