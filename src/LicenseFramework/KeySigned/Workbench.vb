Imports System.Runtime.CompilerServices
Imports LicenseVendor.Database
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Linq

Public Module Workbench

    ReadOnly simple_dbfile As String = App.ProductProgramData & "/license_db.csv"

    Public Function licenseDb() As IEnumerable(Of LicenseUser)
        Return simple_dbfile _
            .LoadCsv(Of LicenseTable) _
            .GroupBy(Function(a) a.user_name) _
            .Select(Function(user)
                        Return New LicenseUser With {
                            .user_name = user.Key,
                            .organization = user.First.organization,
                            .licenses = user _
                                .Select(Function(a)
                                            Return New SoftwareLicense With {
                                                .expired = a.expired,
                                                .hardware_checksum = a.hardware_checksum
                                            }
                                        End Function) _
                                .ToArray
                        }
                    End Function)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Async Function loadLicenses() As Task(Of LicenseUser())
        Return Await Task.Run(Function() licenseDb.ToArray)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub addLicense(license As LicenseTable)
        Call simple_dbfile _
            .LoadCsv(Of LicenseTable) _
            .JoinIterates(license) _
            .ToArray _
            .SaveTo(simple_dbfile)
    End Sub

End Module
