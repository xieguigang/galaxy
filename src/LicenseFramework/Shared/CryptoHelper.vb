Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Namespace LicenseFramework.Shared

    ''' <summary>
    ''' 加密辅助工具类
    ''' 
    ''' 加密体系：
    ''' - RSA 2048位：许可证签名与验证
    ''' - AES-256 CBC：数据加密传输
    ''' - HMAC-SHA256：请求完整性校验
    ''' - SHA-256：哈希摘要
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' 加密辅助工具模块 (CryptoHelper.vb)
    ''' 封装RSA签名/验证、AES加解密、HMAC等系统加密函数
    ''' ---------------------------------------------------------------
    '''
    Public Class CryptoHelper

        Public Const RSA_KEY_CONTAINER_NAME As String = "LicenseFramework_RSA_2048"
        Public Const AES_KEY_SIZE As Integer = 256
        Public Const AES_BLOCK_SIZE As Integer = 128

#Region "RSA 操作"

        ''' <summary>
        ''' 生成RSA 2048位密钥对
        ''' </summary>
        Public Shared Sub GenerateRsaKeyPair(ByRef publicKeyXml As String, ByRef privateKeyXml As String)
#Disable Warning CA1416 ' Validate platform compatibility
            Dim cspParams As New CspParameters() With {
                .KeyContainerName = RSA_KEY_CONTAINER_NAME,
                .Flags = CspProviderFlags.UseMachineKeyStore
            }
#Enable Warning CA1416 ' Validate platform compatibility

            Using rsa As New RSACryptoServiceProvider(2048, cspParams)
                privateKeyXml = rsa.ToXmlString(True)
                publicKeyXml = rsa.ToXmlString(False)
            End Using
        End Sub

        ''' <summary>
        ''' 删除指定的RSA密钥容器
        ''' </summary>
        Public Shared Sub DeleteRsaKeyContainer()
#Disable Warning CA1416 ' Validate platform compatibility
            Dim cspParams As New CspParameters() With {
                .KeyContainerName = RSA_KEY_CONTAINER_NAME, ' 必须与生成时的名称一致："LicenseFramework_RSA_2048"
                .Flags = CspProviderFlags.UseMachineKeyStore ' 必须与生成时的存储位置一致
            }

            Using rsa As New RSACryptoServiceProvider(cspParams)
                ' 告诉CSP：不要在容器中持久化保存此密钥
                rsa.PersistKeyInCsp = False
                ' 释放资源并从密钥容器中删除该密钥
                rsa.Clear()
            End Using
#Enable Warning CA1416 ' Validate platform compatibility
        End Sub


        ''' <summary>
        ''' 使用RSA私钥对字符串进行签名
        ''' 返回Base64编码的签名字符串
        ''' </summary>
        Public Shared Function SignString(data As String, privateKeyXml As String) As String
            Using rsa As New RSACryptoServiceProvider()
                rsa.FromXmlString(privateKeyXml)

                Dim dataBytes As Byte() = Encoding.UTF8.GetBytes(data)
                Dim signatureBytes As Byte() = rsa.SignData(dataBytes, CryptoConfig.MapNameToOID("SHA256"))

                Return Convert.ToBase64String(signatureBytes)
            End Using
        End Function

        ''' <summary>
        ''' 使用RSA公钥验证字符串签名
        ''' </summary>
        Public Shared Function VerifyString(data As String, signatureBase64 As String, publicKeyXml As String) As Boolean
            Try
                Using rsa As New RSACryptoServiceProvider()
                    rsa.FromXmlString(publicKeyXml)

                    Dim dataBytes As Byte() = Encoding.UTF8.GetBytes(data)
                    Dim signatureBytes As Byte() = Convert.FromBase64String(signatureBase64)

                    Return rsa.VerifyData(dataBytes, CryptoConfig.MapNameToOID("SHA256"), signatureBytes)
                End Using
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 使用RSA公钥加密数据
        ''' </summary>
        Public Shared Function RsaEncrypt(data As String, publicKeyXml As String) As String
            Using rsa As New RSACryptoServiceProvider()
                rsa.FromXmlString(publicKeyXml)
                Dim dataBytes As Byte() = Encoding.UTF8.GetBytes(data)
                Dim encrypted As Byte() = rsa.Encrypt(dataBytes, True)
                Return Convert.ToBase64String(encrypted)
            End Using
        End Function

        ''' <summary>
        ''' 使用RSA私钥解密数据
        ''' </summary>
        Public Shared Function RsaDecrypt(encryptedBase64 As String, privateKeyXml As String) As String
            Using rsa As New RSACryptoServiceProvider()
                rsa.FromXmlString(privateKeyXml)
                Dim encryptedBytes As Byte() = Convert.FromBase64String(encryptedBase64)
                Dim decrypted As Byte() = rsa.Decrypt(encryptedBytes, True)
                Return Encoding.UTF8.GetString(decrypted)
            End Using
        End Function

#End Region

#Region "AES 操作"

        ''' <summary>
        ''' AES-256 CBC模式加密
        ''' 返回格式: Base64(IV + 密文)
        ''' </summary>
        Public Shared Function AesEncrypt(plainText As String, key As Byte()) As String
            Using aes As New AesCryptoServiceProvider() With {
                .KeySize = AES_KEY_SIZE,
                .BlockSize = AES_BLOCK_SIZE,
                .Mode = CipherMode.CBC,
                .Padding = PaddingMode.PKCS7
            }
                aes.Key = key
                aes.GenerateIV()

                Using ms As New MemoryStream()
                    ' 先写入IV
                    ms.Write(aes.IV, 0, aes.IV.Length)

                    Using cs As New CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)
                        Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
                        cs.Write(plainBytes, 0, plainBytes.Length)
                        cs.FlushFinalBlock()
                    End Using

                    Return Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
        End Function

        ''' <summary>
        ''' AES-256 CBC模式解密
        ''' 输入格式: Base64(IV + 密文)
        ''' </summary>
        Public Shared Function AesDecrypt(cipherTextBase64 As String, key As Byte()) As String
            Dim fullBytes As Byte() = Convert.FromBase64String(cipherTextBase64)

            Using aes As New AesCryptoServiceProvider() With {
                .KeySize = AES_KEY_SIZE,
                .BlockSize = AES_BLOCK_SIZE,
                .Mode = CipherMode.CBC,
                .Padding = PaddingMode.PKCS7
            }
                aes.Key = key

                ' 提取IV（前16字节）
                Dim iv As Byte() = New Byte(15) {}
                Array.Copy(fullBytes, 0, iv, 0, 16)
                aes.IV = iv

                ' 提取密文
                Dim cipherBytes As Byte() = New Byte(fullBytes.Length - 17) {}
                Array.Copy(fullBytes, 16, cipherBytes, 0, cipherBytes.Length)

                Using ms As New MemoryStream(cipherBytes)
                    Using cs As New CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read)
                        Using sr As New StreamReader(cs, Encoding.UTF8)
                            Return sr.ReadToEnd()
                        End Using
                    End Using
                End Using
            End Using
        End Function

#End Region

#Region "HMAC 操作"

        ''' <summary>
        ''' 计算HMAC-SHA256
        ''' </summary>
        Public Shared Function ComputeHmac(data As String, key As String) As String
            Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(key)
            Dim dataBytes As Byte() = Encoding.UTF8.GetBytes(data)

            Using hmac As New HMACSHA256(keyBytes)
                Dim hashBytes As Byte() = hmac.ComputeHash(dataBytes)
                Return Convert.ToBase64String(hashBytes)
            End Using
        End Function

        ''' <summary>
        ''' 验证HMAC-SHA256
        ''' </summary>
        Public Shared Function VerifyHmac(data As String, key As String, expectedHmac As String) As Boolean
            Dim computedHmac As String = ComputeHmac(data, key)
            Return String.Equals(computedHmac, expectedHmac, StringComparison.Ordinal)
        End Function

#End Region

#Region "通用工具"

        Public Shared Function GenerateRandomKey(keySizeInBytes As Integer) As Byte()
            Dim key As Byte() = New Byte(keySizeInBytes - 1) {}
            Using rng As New RNGCryptoServiceProvider()
                rng.GetBytes(key)
            End Using
            Return key
        End Function

        Public Shared Function GenerateAesKey() As Byte()
            Return GenerateRandomKey(32)
        End Function

        Public Shared Function ComputeSha256(data As Byte()) As Byte()
            Using sha256 As SHA256 = SHA256.Create()
                Return sha256.ComputeHash(data)
            End Using
        End Function

        Public Shared Function ComputeSha256String(input As String) As String
            Dim inputBytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hashBytes As Byte() = ComputeSha256(inputBytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hashBytes
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Function

#End Region

    End Class

End Namespace
