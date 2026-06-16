Imports System.ComponentModel
Imports LicenseVendor.LicenseFramework.Shared
Imports LicenseVendor.LicenseFramework.Vendor
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.Uri

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    Const TIMESTAMP_TOLERANCE_MINUTES As Integer = 5

    <ExportAPI("/generate")>
    <Description("generates the software license file")>
    <Usage("/generate --data <request.txt> --mysqli <mysqli.txt> --private_key <private_key.xml> [--outfile <license.lic> --hmacKey <default='YOUR_HMAC_KEY_BASE64_HERE'>]")>
    <Argument("--data", , CLITypes.File, PipelineTypes.std_in, Description:="the license request file")>
    <Argument("--mysqli",, CLITypes.File, Description:="the text file that contains the mysql connection uri string.")>
    <Argument("--outfile",, CLITypes.File, PipelineTypes.std_out, Description:="the generated license file. the generated license file text content will be print on the standard output if this parameter is missing.")>
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
        ElseIf Not VerifyRequest(request, hmacKey) Then
            Call Console.Error.WriteLine("Request signature verification failed!")
            Call Console.Error.Flush()

            Return -1
        ElseIf Not VerifyTimestamp(request.RequestTimestamp) Then
            Call Console.Error.WriteLine("The request has expired!")
            Call Console.Error.Flush()

            Return -1
        End If

        Dim generator As New LicenseGenerator(privateKeyXml)
        Dim licenseType As LicenseType = LicenseType.Standard
        Dim expiryDays As Integer = 365
        Dim licenseData As New LicenseData With {
            .HardwareFingerprint = request.HardwareFingerprint,
            .ProductName = request.ProductName,
            .ProductVersion = request.ProductVersion,
            .CustomerName = request.User,
            .LicenseType = licenseType,
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
            Dim diff As TimeSpan = DateTime.UtcNow - requestTime
            Return Math.Abs(diff.TotalMinutes) <= TIMESTAMP_TOLERANCE_MINUTES
        Catch
            Return False
        End Try
    End Function
End Module
