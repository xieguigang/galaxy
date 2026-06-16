Imports System.ComponentModel
Imports LicenseVendor.LicenseFramework.Shared
Imports LicenseVendor.LicenseFramework.Vendor
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Oracle.LinuxCompatibility.MySQL.Uri

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/generate")>
    <Description("generates the software license file")>
    <Usage("/generate --data <request.txt> --mysqli <mysqli.txt> --private_key <private_key.xml> [--outfile <license.lic>]")>
    <Argument("--data", , CLITypes.File, PipelineTypes.std_in, Description:="the license request file")>
    <Argument("--mysqli",, CLITypes.File, Description:="the text file that contains the mysql connection uri string.")>
    <Argument("--outfile",, CLITypes.File, PipelineTypes.std_out, Description:="the generated license file. the generated license file text content will be print on the standard output if this parameter is missing.")>
    Public Function GenerateLicense(data As String, mysqli As String, private_key As String, args As CommandLine) As Integer
        Dim url As ConnectionUri = ConnectionUri.TryParsing(mysqli.ReadAllLines.FirstOrDefault)
        Dim outfile As String = args("--outfile")
        Dim privateKeyXml As String = private_key.ReadAllText

        If url Is Nothing Then
            Call Console.Error.WriteLine("no mysqli connection uri data was provided!")
            Call Console.Error.Flush()

            Return -1
        End If

        Dim generator As New LicenseGenerator(privateKeyXml)
        Dim licenseType As LicenseType = LicenseType.Standard
        Dim expiryDays As Integer = 365

        Dim signedLicense = generator.GenerateLicense(
            _currentFingerprint,
            txtProductName.Text,
            txtProductVersion.Text,
            txtCustomerName.Text,
            licenseType,
            expiryDays)

        If outfile.StringEmpty Then
            Call Console.WriteLine(signedLicense)
        Else
            Call signedLicense.SaveTo(outfile)
        End If

        Return 0
    End Function
End Module
