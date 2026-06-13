Imports LicenseVendor
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Linq

Public Module Workbench

    ReadOnly simple_dbfile As String = App.ProductProgramData & "/license_db.csv"

    Public Function licenseDb() As IEnumerable(Of LicenseUser)
        Return simple_dbfile.LoadCsv(Of LicenseUser)
    End Function

    Public Async Function loadLicenses() As Task(Of LicenseUser())
        Return Await Task.Run(Function() licenseDb.ToArray)
    End Function

    Public Sub addLicense(license As LicenseUser)
        Call licenseDb.JoinIterates(license).ToArray.SaveTo(simple_dbfile)
    End Sub

End Module
