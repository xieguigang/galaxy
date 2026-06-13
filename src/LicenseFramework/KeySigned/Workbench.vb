Imports System.Runtime.CompilerServices
Imports LicenseVendor.Database
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Linq

Public Module Workbench

    ReadOnly simple_dbfile As String = App.ProductProgramData & "/license_db.csv"

    Public Function licenseDb() As IEnumerable(Of LicenseUser)
        Return simple_dbfile _
            .LoadCsv(Of SoftwareLicense) _
            .GroupBy(Function(a) a.user_name) _
            .Select(Function(user)
                        Return New LicenseUser With {
                            .user_name = user.Key,
                            .software_name = user.First.organization,
                            .licenses = user _
                                .Select(Function(a)
                                            Return New LicenseUser With {
                                                .expired = a.expired,
                                                .hardware_checksum = a.hardware_checksum,
                                                .software_name = user.First.organization,
                                                .user_name = user.Key
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
    Public Sub addLicense(license As SoftwareLicense)
        Call simple_dbfile _
            .LoadCsv(Of SoftwareLicense) _
            .JoinIterates(license) _
            .ToArray _
            .SaveTo(simple_dbfile)
    End Sub

    ''' <summary>
    ''' private key file
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property private_key As String = (App.HOME & "/data/private_key.xml").GetFullPath
    Public ReadOnly Property public_key As String = (App.HOME & "/data/public_key.xml").GetFullPath

    Public Function CheckPrivateKey() As Boolean
        Return private_key.FileLength > 1024
    End Function

    Public Sub WriteLicenseKeys(publicKey As String, privateKey As String)
        Call publicKey.SaveTo(public_key)
        Call privateKey.SaveTo(private_key)
    End Sub

    Public Function GetLicenseKey() As (publicKey$, privateKey$)
        Return (public_key.ReadAllText(throwEx:=False), private_key.ReadAllText(throwEx:=False))
    End Function

End Module
