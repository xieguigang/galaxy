'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Defines properties for known folders that identify the path of standard known folders.
	''' </summary>
	Public NotInheritable Class KnownFolders
		Private Sub New()
		End Sub
		''' <summary>
		''' Gets a strongly-typed read-only collection of all the registered known folders.
		''' </summary>
		Public Shared ReadOnly Property All() As ICollection(Of IKnownFolder)
			Get
				Return GetAllFolders()
			End Get
		End Property

		Private Shared Function GetAllFolders() As ReadOnlyCollection(Of IKnownFolder)
			' Should this method be thread-safe?? (It'll take a while
			' to get a list of all the known folders, create the managed wrapper
			' and return the read-only collection.

			Dim foldersList As IList(Of IKnownFolder) = New List(Of IKnownFolder)()
			Dim count As UInteger
			Dim folders As IntPtr = IntPtr.Zero

			Try

				Dim knownFolderManager As New KnownFolderManagerClass()
				knownFolderManager.GetFolderIds(folders, count)

				If count > 0 AndAlso folders <> IntPtr.Zero Then
                    ' Loop through all the KnownFolderID elements
                    Dim Len As Integer = CInt(count) - 1

                    For i As Integer = 0 To Len
                        ' Read the current pointer
                        Dim current As New IntPtr(folders.ToInt64() + (Marshal.SizeOf(GetType(Guid)) * i))

                        ' Convert to Guid
                        Dim knownFolderID As Guid = CType(Marshal.PtrToStructure(current, GetType(Guid)), Guid)

                        Dim kf As IKnownFolder = KnownFolderHelper.FromKnownFolderIdInternal(knownFolderID)

                        ' Add to our collection if it's not null (some folders might not exist on the system
                        ' or we could have an exception that resulted in the null return from above method call
                        If kf IsNot Nothing Then
                            foldersList.Add(kf)
                        End If
                    Next
                End If
			Finally
				If folders <> IntPtr.Zero Then
					Marshal.FreeCoTaskMem(folders)
				End If
			End Try

			Return New ReadOnlyCollection(Of IKnownFolder)(foldersList)
		End Function

		Private Shared Function GetKnownFolder(guid As Guid) As IKnownFolder
			Return KnownFolderHelper.FromKnownFolderId(guid)
		End Function

		#Region "Default Known Folders"

		''' <summary>
		''' Gets the metadata for the <b>Computer</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Computer() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Computer)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Conflict</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Conflict() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Conflict)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ControlPanel</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ControlPanel() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ControlPanel)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Desktop</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Desktop() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Desktop)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Internet</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Internet() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Internet)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Network</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Network() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Network)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Printers</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Printers() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Printers)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SyncManager</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SyncManager() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SyncManager)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Connections</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Connections() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Connections)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SyncSetup</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SyncSetup() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SyncSetup)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SyncResults</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SyncResults() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SyncResults)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>RecycleBin</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property RecycleBin() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.RecycleBin)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Fonts</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Fonts() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Fonts)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Startup</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Startup() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Startup)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Programs</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Programs() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Programs)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>StartMenu</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property StartMenu() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.StartMenu)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Recent</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Recent() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Recent)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>SendTo</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SendTo() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SendTo)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Documents</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Documents() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Documents)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Favorites</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Favorites() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Favorites)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>NetHood</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property NetHood() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.NetHood)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PrintHood</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PrintHood() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PrintHood)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Templates</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Templates() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Templates)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CommonStartup</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CommonStartup() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CommonStartup)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CommonPrograms</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CommonPrograms() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CommonPrograms)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CommonStartMenu</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CommonStartMenu() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CommonStartMenu)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicDesktop</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicDesktop() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicDesktop)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ProgramData</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramData() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramData)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CommonTemplates</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CommonTemplates() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CommonTemplates)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicDocuments</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicDocuments() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicDocuments)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>RoamingAppData</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property RoamingAppData() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.RoamingAppData)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>LocalAppData</b>  
		''' folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property LocalAppData() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.LocalAppData)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>LocalAppDataLow</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property LocalAppDataLow() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.LocalAppDataLow)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>InternetCache</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property InternetCache() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.InternetCache)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Cookies</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Cookies() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Cookies)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>History</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property History() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.History)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>System</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property System() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.System)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SystemX86</b>  
		''' folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SystemX86() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SystemX86)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Windows</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Windows() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Windows)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Profile</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Profile() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Profile)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Pictures</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Pictures() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Pictures)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ProgramFilesX86</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramFilesX86() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramFilesX86)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ProgramFilesCommonX86</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramFilesCommonX86() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramFilesCommonX86)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ProgramsFilesX64</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramFilesX64() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramFilesX64)
			End Get
		End Property

		''' <summary>
		'''  Gets the metadata for the <b> ProgramFilesCommonX64</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramFilesCommonX64() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramFilesCommonX64)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ProgramFiles</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramFiles() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramFiles)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ProgramFilesCommon</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ProgramFilesCommon() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ProgramFilesCommon)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>AdminTools</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property AdminTools() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.AdminTools)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CommonAdminTools</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CommonAdminTools() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CommonAdminTools)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Music</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Music() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Music)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Videos</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Videos() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Videos)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicPictures</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicPictures() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicPictures)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicMusic</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicMusic() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicMusic)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicVideos</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicVideos() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicVideos)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ResourceDir</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ResourceDir() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ResourceDir)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>LocalizedResourcesDir</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property LocalizedResourcesDir() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.LocalizedResourcesDir)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CommonOEMLinks</b> folder. 
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CommonOemLinks() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CommonOEMLinks)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>CDBurning</b> folder. 
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property CDBurning() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.CDBurning)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>UserProfiles</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property UserProfiles() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.UserProfiles)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Playlists</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Playlists() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Playlists)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SamplePlaylists</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SamplePlaylists() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SamplePlaylists)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SampleMusic</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SampleMusic() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SampleMusic)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SamplePictures</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SamplePictures() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SamplePictures)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SampleVideos</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SampleVideos() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SampleVideos)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PhotoAlbums</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PhotoAlbums() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PhotoAlbums)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Public</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property [Public]() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.[Public])
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ChangeRemovePrograms</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property ChangeRemovePrograms() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.ChangeRemovePrograms)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>AppUpdates</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property AppUpdates() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.AppUpdates)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>AddNewPrograms</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property AddNewPrograms() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.AddNewPrograms)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Downloads</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Downloads() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Downloads)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicDownloads</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicDownloads() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicDownloads)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>SavedSearches</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SavedSearches() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SavedSearches)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>QuickLaunch</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property QuickLaunch() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.QuickLaunch)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Contacts</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Contacts() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Contacts)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SidebarParts</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SidebarParts() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SidebarParts)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SidebarDefaultParts</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SidebarDefaultParts() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SidebarDefaultParts)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>TreeProperties</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property TreeProperties() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.TreeProperties)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicGameTasks</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property PublicGameTasks() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.PublicGameTasks)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>GameTasks</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property GameTasks() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.GameTasks)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>SavedGames</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SavedGames() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SavedGames)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Games</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Games() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Games)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>RecordedTV</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		''' <remarks>This folder is not used.</remarks>
		Public Shared ReadOnly Property RecordedTV() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.RecordedTV)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SearchMapi</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SearchMapi() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SearchMapi)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SearchCsc</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SearchCsc() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SearchCsc)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the per-user <b>Links</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property Links() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.Links)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>UsersFiles</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property UsersFiles() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.UsersFiles)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>SearchHome</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property SearchHome() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.SearchHome)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>OriginalImages</b> folder.
		''' </summary>
		''' <value>An <see cref="IKnownFolder"/> object.</value>
		Public Shared ReadOnly Property OriginalImages() As IKnownFolder
			Get
				Return GetKnownFolder(FolderIdentifiers.OriginalImages)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>UserProgramFiles</b> folder.
		''' </summary>
		Public Shared ReadOnly Property UserProgramFiles() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.UserProgramFiles)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>UserProgramFilesCommon</b> folder.
		''' </summary>
		Public Shared ReadOnly Property UserProgramFilesCommon() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.UserProgramFilesCommon)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Ringtones</b> folder.
		''' </summary>
		Public Shared ReadOnly Property Ringtones() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.Ringtones)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PublicRingtones</b> folder.
		''' </summary>
		Public Shared ReadOnly Property PublicRingtones() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.PublicRingtones)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>UsersLibraries</b> folder.
		''' </summary>
		Public Shared ReadOnly Property UsersLibraries() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.UsersLibraries)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>DocumentsLibrary</b> folder.
		''' </summary>
		Public Shared ReadOnly Property DocumentsLibrary() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.DocumentsLibrary)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>MusicLibrary</b> folder.
		''' </summary>
		Public Shared ReadOnly Property MusicLibrary() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.MusicLibrary)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>PicturesLibrary</b> folder.
		''' </summary>
		Public Shared ReadOnly Property PicturesLibrary() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.PicturesLibrary)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>VideosLibrary</b> folder.
		''' </summary>
		Public Shared ReadOnly Property VideosLibrary() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.VideosLibrary)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>RecordedTVLibrary</b> folder.
		''' </summary>
		Public Shared ReadOnly Property RecordedTVLibrary() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.RecordedTVLibrary)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>OtherUsers</b> folder.
		''' </summary>
		Public Shared ReadOnly Property OtherUsers() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.OtherUsers)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>DeviceMetadataStore</b> folder.
		''' </summary>
		Public Shared ReadOnly Property DeviceMetadataStore() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.DeviceMetadataStore)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>Libraries</b> folder.
		''' </summary>
		Public Shared ReadOnly Property Libraries() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.Libraries)
			End Get
		End Property

		''' <summary>
		'''Gets the metadata for the <b>UserPinned</b> folder.
		''' </summary>
		Public Shared ReadOnly Property UserPinned() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.UserPinned)
			End Get
		End Property

		''' <summary>
		''' Gets the metadata for the <b>ImplicitAppShortcuts</b> folder.
		''' </summary>
		Public Shared ReadOnly Property ImplicitAppShortcuts() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return GetKnownFolder(FolderIdentifiers.ImplicitAppShortcuts)
			End Get
		End Property

		#End Region

	End Class
End Namespace
