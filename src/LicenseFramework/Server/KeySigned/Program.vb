Imports System.ComponentModel
Imports Flute.Http
Imports Flute.Http.Core
Imports Flute.Http.Core.Message
Imports KeySigned.license_svrModel
Imports LicenseVendor.LicenseFramework.Shared
Imports LicenseVendor.LicenseFramework.Vendor
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    Const TIMESTAMP_TOLERANCE_MINUTES As Integer = 5

    <ExportAPI("/service")>
    <Usage("/service --mysqli <mysqli.txt> --private_key <private_key.xml> [--port <default=80> --hmacKey <default='YOUR_HMAC_KEY_BASE64_HERE'>]")>
    <Description("Run http web services for license key signed")>
    <Argument("--mysqli",, CLITypes.File, Description:="the text file that contains the mysql connection uri string.")>
    <Argument("--private_key",, CLITypes.File, Description:="the RSA private key xml file of the license key signed tool.")>
    Public Function Listen(mysqli As String, private_key As String, Optional port As Integer = 80, Optional args As CommandLine = Nothing) As Integer
        Dim url As ConnectionUri = ConnectionUri.TryParsing(mysqli.ReadAllLines.FirstOrDefault)
        Dim privateKeyXml As String = private_key.ReadAllText
        Dim hmacKey As String = args("--hmacKey") Or "YOUR_HMAC_KEY_BASE64_HERE"
        Dim licenseDb As New LicenseDb(url)
        Dim generator As New LicenseGenerator(privateKeyXml)
        Dim keySigned As New LicenseKeySignedTool With {
            .generator = generator,
            .hmacKey = hmacKey,
            .licenseDb = licenseDb
        }
        Dim http = New HttpDriver().HttpMethod("post", keySigned).GetSocket(port)

        Return http.Run
    End Function

    Private Class LicenseKeySignedTool : Implements IAppHandler

        Friend generator As LicenseGenerator
        Friend licenseDb As LicenseDb
        Friend hmacKey As String

        Public Sub AppHandler(request As HttpRequest, response As HttpResponse) Implements IAppHandler.AppHandler
            Dim licenseType As LicenseType = LicenseType.Standard
            Dim post As HttpPOSTRequest = DirectCast(request, HttpPOSTRequest)
            Dim data As New OnlineLicenseRequest With {
                .HardwareFingerprint = post(NameOf(OnlineLicenseRequest.HardwareFingerprint)),
                .Password = post(NameOf(OnlineLicenseRequest.Password)),
                .ProductName = post(NameOf(OnlineLicenseRequest.ProductName)),
                .ProductVersion = post(NameOf(OnlineLicenseRequest.ProductVersion)),
                .RequestSignature = post(NameOf(OnlineLicenseRequest.RequestSignature)),
                .RequestTimestamp = post(NameOf(OnlineLicenseRequest.RequestTimestamp)),
                .User = post(NameOf(OnlineLicenseRequest.User))
            }
            Dim result As OnlineLicenseResponse = KeySign(request:=data)

            Try
                Call response.WriteJSON(result)
            Catch ex As Exception
            End Try
        End Sub

        Public Function KeySign(request As OnlineLicenseRequest) As OnlineLicenseResponse
            If Not VerifyRequest(request, hmacKey) Then
                Call Console.Error.WriteLine("Request signature verification failed!")
                Call Console.Error.Flush()

                Return New OnlineLicenseResponse With {.StatusCode = 500, .StatusMessage = "Request signature verification failed!"}
            ElseIf Not VerifyTimestamp(request.RequestTimestamp) Then
                Call Console.Error.WriteLine("The request has expired!")
                Call Console.Error.Flush()

                Return New OnlineLicenseResponse With {.StatusCode = 500, .StatusMessage = "The request has expired!"}
            End If

            Dim user As user = licenseDb.GetUser(request.User, request.Password)
            Dim expiryDays As Integer = 365

            If user Is Nothing Then
                Call Console.Error.WriteLine("User not found or password incorrect!")
                Call Console.Error.Flush()

                Return New OnlineLicenseResponse With {.StatusCode = 500, .StatusMessage = "User not found or password incorrect!"}
            End If

            ' check license
            Dim checkLicense As license_svrModel.license = licenseDb.CheckLicense(user, request.HardwareFingerprint)
            Dim licenseType As LicenseType = LicenseType.Standard

            If checkLicense Is Nothing Then
                ' create a new license
                If user.licenses >= user.max_licenses Then
                    Call Console.Error.WriteLine("The number of authorized licenses has reached the maximum limit.")
                    Call Console.Error.Flush()

                    Return New OnlineLicenseResponse With {.StatusCode = 500, .StatusMessage = "The number of authorized licenses has reached the maximum limit."}
                Else
                    Call licenseDb.license.add(
                        field("user_id") = user.id,
                        field("product_name") = request.ProductName,
                        field("product_version") = request.ProductVersion,
                        field("fingerprint") = request.HardwareFingerprint,
                        field("expired_time") = Now.AddDays(expiryDays))
                    Call licenseDb.user _
                        .where(field("id") = user.id) _
                        .save(field("licenses") = user.licenses + 1)
                End If
            End If

            Dim licenseData As New LicenseData With {
                .HardwareFingerprint = request.HardwareFingerprint,
                .ProductName = request.ProductName,
                .ProductVersion = request.ProductVersion,
                .CustomerName = request.User,
                .LicenseType = LicenseType,
                .IssueDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                .ExpiryDate = DateTime.UtcNow.AddDays(expiryDays).ToString("yyyy-MM-ddTHH:mm:ssZ")
            }
            Dim signedLicense = generator.GenerateSignedLicense(licenseData)
            Dim result As New OnlineLicenseResponse With {
                .StatusCode = 0,
                .StatusMessage = "OK",
                .SignedLicense = signedLicense,
                .ServerTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }

            Return result
        End Function
    End Class

    <ExportAPI("/generate")>
    <Description("generates the software license file")>
    <Usage("/generate --data <request.txt> --mysqli <mysqli.txt> --private_key <private_key.xml> [--outfile <license.lic> --hmacKey <default='YOUR_HMAC_KEY_BASE64_HERE'>]")>
    <Argument("--data", , CLITypes.File, PipelineTypes.std_in, Description:="the license request file")>
    <Argument("--mysqli",, CLITypes.File, Description:="the text file that contains the mysql connection uri string.")>
    <Argument("--outfile",, CLITypes.File, PipelineTypes.std_out, Description:="the generated license file. the generated license file text content will be print on the standard output if this parameter is missing.")>
    <Argument("--private_key",, CLITypes.File, Description:="the RSA private key xml file of the license key signed tool.")>
    Public Function GenerateLicense(data As String, mysqli As String, private_key As String, args As CommandLine) As Integer
        Dim url As ConnectionUri = ConnectionUri.TryParsing(mysqli.ReadAllLines.FirstOrDefault)
        Dim outfile As String = args("--outfile")
        Dim privateKeyXml As String = private_key.ReadAllText
        Dim hmacKey As String = args("--hmacKey") Or "YOUR_HMAC_KEY_BASE64_HERE"
        Dim request As OnlineLicenseRequest = data.LoadJsonFile(Of OnlineLicenseRequest)

        If url Is Nothing Then
            Call Console.Error.WriteLine("No mysqli connection uri data was provided!")
            Call Console.Error.Flush()

            Return -1
        End If

        Dim licenseDb As New LicenseDb(url)
        Dim generator As New LicenseGenerator(privateKeyXml)
        Dim keySign As New LicenseKeySignedTool With {.generator = generator, .hmacKey = hmacKey, .licenseDb = licenseDb}
        Dim result = keySign.KeySign(request)

        If outfile.StringEmpty Then
            Call Console.WriteLine(result.GetJson)
        Else
            Call result.GetJson.SaveTo(outfile)
        End If

        Return 0
    End Function

    ''' <summary>
    ''' 验证请求的HMAC签名
    ''' </summary>
    Private Function VerifyRequest(request As OnlineLicenseRequest, hmacKey As String) As Boolean
        Try
            Dim signData As String = $"{request.HardwareFingerprint}|{request.ProductName}|{request.ProductVersion}|{request.RequestTimestamp}"
            Return CryptoHelper.VerifyHmac(signData, hmacKey, request.RequestSignature)
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 验证请求时间戳是否在有效期内
    ''' </summary>
    Private Function VerifyTimestamp(timestampStr As String) As Boolean
        Try
            Dim requestTime As DateTime = DateTime.Parse(timestampStr)
            Dim diff As TimeSpan = DateTime.Now - requestTime
            Return Math.Abs(diff.TotalMinutes) <= TIMESTAMP_TOLERANCE_MINUTES
        Catch
            Return False
        End Try
    End Function
End Module
