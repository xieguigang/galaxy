'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Reflection

Namespace Shell
	''' <summary>
	''' Contains the GUID identifiers for well-known folders.
	''' </summary>
	Friend NotInheritable Class FolderIdentifiers
		Private Sub New()
		End Sub
		Private Shared folders As Dictionary(Of Guid, String)

		Shared Sub New()
			folders = New Dictionary(Of Guid, String)()
			Dim folderIDs As Type = GetType(FolderIdentifiers)

			Dim fields As FieldInfo() = folderIDs.GetFields(BindingFlags.NonPublic Or BindingFlags.[Static])
			For Each f As FieldInfo In fields
				' Ignore dictionary field.
				If f.FieldType Is GetType(Guid) Then
					Dim id As Guid = CType(f.GetValue(Nothing), Guid)
					Dim name As String = f.Name
					folders.Add(id, name)
				End If
			Next
		End Sub
		''' <summary>
		''' Returns the friendly name for a specified folder.
		''' </summary>
		''' <param name="folderId">The Guid identifier for a known folder.</param>
		''' <returns>A <see cref="T:System.String"/> value.</returns>
		Friend Shared Function NameForGuid(folderId As Guid) As String
			Dim folder As String
			If Not folders.TryGetValue(folderId, folder) Then
				Throw New ArgumentException(GlobalLocalizedMessages.FolderIdsUnknownGuid, "folderId")
			End If
			Return folder
		End Function
		''' <summary>
		''' Returns a sorted list of name, guid pairs for 
		''' all known folders.
		''' </summary>
		''' <returns></returns>
		Friend Shared Function GetAllFolders() As SortedList(Of String, Guid)
			' Make a copy of the dictionary
			' because the Keys and Values collections
			' are mutable.
			Dim keys As ICollection(Of Guid) = folders.Keys

			Dim slist As New SortedList(Of String, Guid)()
			For Each g As Guid In keys
				slist.Add(folders(g), g)
			Next

			Return slist
		End Function

		#Region "KnownFolder Guids"

		''' <summary>
		''' Computer
		''' </summary>
		Friend Shared Computer As New Guid(&Hac0837c, &Hbbf8, &H452a, &H85, &Hd, &H79, _
			&Hd0, &H8e, &H66, &H7c, &Ha7)

		''' <summary>
		''' Conflicts
		''' </summary>
		Friend Shared Conflict As New Guid(&H4bfefb45, &H347d, &H4006, &Ha5, &Hbe, &Hac, _
			&Hc, &Hb0, &H56, &H71, &H92)

		''' <summary>
		''' Control Panel
		''' </summary>
		Friend Shared ControlPanel As New Guid(&H82a74aebUI, &Haeb4, &H465c, &Ha0, &H14, &Hd0, _
			&H97, &Hee, &H34, &H6d, &H63)

		''' <summary>
		''' Desktop
		''' </summary>
		Friend Shared Desktop As New Guid(&Hb4bfcc3aUI, &Hdb2c, &H424c, &Hb0, &H29, &H7f, _
			&He9, &H9a, &H87, &Hc6, &H41)

		''' <summary>
		''' Internet Explorer
		''' </summary>
		Friend Shared Internet As New Guid(&H4d9f7874, &H4e0c, &H4904, &H96, &H7b, &H40, _
			&Hb0, &Hd2, &Hc, &H3e, &H4b)

		''' <summary>
		''' Network
		''' </summary>
		Friend Shared Network As New Guid(&Hd20beec4UI, &H5ca8, &H4905, &Hae, &H3b, &Hbf, _
			&H25, &H1e, &Ha0, &H9b, &H53)

		''' <summary>
		''' Printers
		''' </summary>
		Friend Shared Printers As New Guid(&H76fc4e2d, &Hd6ad, &H4519, &Ha6, &H63, &H37, _
			&Hbd, &H56, &H6, &H81, &H85)

		''' <summary>
		''' Sync Center
		''' </summary>
		Friend Shared SyncManager As New Guid(&H43668bf8, &Hc14e, &H49b2, &H97, &Hc9, &H74, _
			&H77, &H84, &Hd7, &H84, &Hb7)

		''' <summary>
		''' Network Connections
		''' </summary>
		Friend Shared Connections As New Guid(&H6f0cd92b, &H2e97, &H45d1, &H88, &Hff, &Hb0, _
			&Hd1, &H86, &Hb8, &Hde, &Hdd)

		''' <summary>
		''' Sync Setup
		''' </summary>
		Friend Shared SyncSetup As New Guid(&Hf214138, &Hb1d3, &H4a90, &Hbb, &Ha9, &H27, _
			&Hcb, &Hc0, &Hc5, &H38, &H9a)

		''' <summary>
		''' Sync Results
		''' </summary>
		Friend Shared SyncResults As New Guid(&H289a9a43, &Hbe44, &H4057, &Ha4, &H1b, &H58, _
			&H7a, &H76, &Hd7, &He7, &Hf9)

		''' <summary>
		''' Recycle Bin
		''' </summary>
		Friend Shared RecycleBin As New Guid(&Hb7534046UI, &H3ecb, &H4c18, &Hbe, &H4e, &H64, _
			&Hcd, &H4c, &Hb7, &Hd6, &Hac)

		''' <summary>
		''' Fonts
		''' </summary>
		Friend Shared Fonts As New Guid(&Hfd228cb7UI, &Hae11, &H4ae3, &H86, &H4c, &H16, _
			&Hf3, &H91, &Ha, &Hb8, &Hfe)

		''' <summary>
		''' Startup
		''' </summary>
		Friend Shared Startup As New Guid(&Hb97d20bbUI, &Hf46a, &H4c97, &Hba, &H10, &H5e, _
			&H36, &H8, &H43, &H8, &H54)

		''' <summary>
		''' Programs
		''' </summary>
		Friend Shared Programs As New Guid(&Ha77f5d77UI, &H2e2b, &H44c3, &Ha6, &Ha2, &Hab, _
			&Ha6, &H1, &H5, &H4a, &H51)

		''' <summary>
		''' Start Menu
		''' </summary>
		Friend Shared StartMenu As New Guid(&H625b53c3, &Hab48, &H4ec1, &Hba, &H1f, &Ha1, _
			&Hef, &H41, &H46, &Hfc, &H19)

		''' <summary>
		''' Recent Items
		''' </summary>
		Friend Shared Recent As New Guid(&Hae50c081UI, &Hebd2, &H438a, &H86, &H55, &H8a, _
			&H9, &H2e, &H34, &H98, &H7a)

		''' <summary>
		''' SendTo
		''' </summary>
		Friend Shared SendTo As New Guid(&H8983036cUI, &H27c0, &H404b, &H8f, &H8, &H10, _
			&H2d, &H10, &Hdc, &Hfd, &H74)

		''' <summary>
		''' Documents
		''' </summary>
		Friend Shared Documents As New Guid(&Hfdd39ad0UI, &H238f, &H46af, &Had, &Hb4, &H6c, _
			&H85, &H48, &H3, &H69, &Hc7)

		''' <summary>
		''' Favorites
		''' </summary>
		Friend Shared Favorites As New Guid(&H1777f761, &H68ad, &H4d8a, &H87, &Hbd, &H30, _
			&Hb7, &H59, &Hfa, &H33, &Hdd)

		''' <summary>
		''' Network Shortcuts
		''' </summary>
		Friend Shared NetHood As New Guid(&Hc5abbf53UI, &He17f, &H4121, &H89, &H0, &H86, _
			&H62, &H6f, &Hc2, &Hc9, &H73)

		''' <summary>
		''' Printer Shortcuts
		''' </summary>
		Friend Shared PrintHood As New Guid(&H9274bd8dUI, &Hcfd1, &H41c3, &Hb3, &H5e, &Hb1, _
			&H3f, &H55, &Ha7, &H58, &Hf4)

		''' <summary>
		''' Templates
		''' </summary>
		Friend Shared Templates As New Guid(&Ha63293e8UI, &H664e, &H48db, &Ha0, &H79, &Hdf, _
			&H75, &H9e, &H5, &H9, &Hf7)

		''' <summary>
		''' Startup
		''' </summary>
		Friend Shared CommonStartup As New Guid(&H82a5ea35UI, &Hd9cd, &H47c5, &H96, &H29, &He1, _
			&H5d, &H2f, &H71, &H4e, &H6e)

		''' <summary>
		''' Programs
		''' </summary>
		Friend Shared CommonPrograms As New Guid(&H139d44e, &H6afe, &H49f2, &H86, &H90, &H3d, _
			&Haf, &Hca, &He6, &Hff, &Hb8)

		''' <summary>
		''' Start Menu
		''' </summary>
		Friend Shared CommonStartMenu As New Guid(&Ha4115719UI, &Hd62e, &H491d, &Haa, &H7c, &He7, _
			&H4b, &H8b, &He3, &Hb0, &H67)

		''' <summary>
		''' Public Desktop
		''' </summary>
		Friend Shared PublicDesktop As New Guid(&Hc4aa340dUI, &Hf20f, &H4863, &Haf, &Hef, &Hf8, _
			&H7e, &Hf2, &He6, &Hba, &H25)

		''' <summary>
		''' ProgramData
		''' </summary>
		Friend Shared ProgramData As New Guid(&H62ab5d82, &Hfdc1, &H4dc3, &Ha9, &Hdd, &H7, _
			&Hd, &H1d, &H49, &H5d, &H97)

		''' <summary>
		''' Templates
		''' </summary>
		Friend Shared CommonTemplates As New Guid(&Hb94237e7UI, &H57ac, &H4347, &H91, &H51, &Hb0, _
			&H8c, &H6c, &H32, &Hd1, &Hf7)

		''' <summary>
		''' Public Documents
		''' </summary>
		Friend Shared PublicDocuments As New Guid(&Hed4824afUI, &Hdce4, &H45a8, &H81, &He2, &Hfc, _
			&H79, &H65, &H8, &H36, &H34)

		''' <summary>
		''' Roaming
		''' </summary>
		Friend Shared RoamingAppData As New Guid(&H3eb685db, &H65f9, &H4cf6, &Ha0, &H3a, &He3, _
			&Hef, &H65, &H72, &H9f, &H3d)

		''' <summary>
		''' Local
		''' </summary>
		Friend Shared LocalAppData As New Guid(&Hf1b32785UI, &H6fba, &H4fcf, &H9d, &H55, &H7b, _
			&H8e, &H7f, &H15, &H70, &H91)

		''' <summary>
		''' LocalLow
		''' </summary>
		Friend Shared LocalAppDataLow As New Guid(&Ha520a1a4UI, &H1780, &H4ff6, &Hbd, &H18, &H16, _
			&H73, &H43, &Hc5, &Haf, &H16)

		''' <summary>
		''' Temporary Internet Files
		''' </summary>
		Friend Shared InternetCache As New Guid(&H352481e8, &H33be, &H4251, &Hba, &H85, &H60, _
			&H7, &Hca, &Hed, &Hcf, &H9d)

		''' <summary>
		''' Cookies
		''' </summary>
		Friend Shared Cookies As New Guid(&H2b0f765d, &Hc0e9, &H4171, &H90, &H8e, &H8, _
			&Ha6, &H11, &Hb8, &H4f, &Hf6)

		''' <summary>
		''' History
		''' </summary>
		Friend Shared History As New Guid(&Hd9dc8a3bUI, &Hb784, &H432e, &Ha7, &H81, &H5a, _
			&H11, &H30, &Ha7, &H59, &H63)

		''' <summary>
		''' System32
		''' </summary>
		Friend Shared System As New Guid(&H1ac14e77, &H2e7, &H4e5d, &Hb7, &H44, &H2e, _
			&Hb1, &Hae, &H51, &H98, &Hb7)

		''' <summary>
		''' System32
		''' </summary>
		Friend Shared SystemX86 As New Guid(&Hd65231b0UI, &Hb2f1, &H4857, &Ha4, &Hce, &Ha8, _
			&He7, &Hc6, &Hea, &H7d, &H27)

		''' <summary>
		''' Windows
		''' </summary>
		Friend Shared Windows As New Guid(&Hf38bf404UI, &H1d43, &H42f2, &H93, &H5, &H67, _
			&Hde, &Hb, &H28, &Hfc, &H23)

		''' <summary>
		''' The user's username (%USERNAME%)
		''' </summary>
		Friend Shared Profile As New Guid(&H5e6c858f, &He22, &H4760, &H9a, &Hfe, &Hea, _
			&H33, &H17, &Hb6, &H71, &H73)

		''' <summary>
		''' Pictures
		''' </summary>
		Friend Shared Pictures As New Guid(&H33e28130, &H4e1e, &H4676, &H83, &H5a, &H98, _
			&H39, &H5c, &H3b, &Hc3, &Hbb)

		''' <summary>
		''' Program Files
		''' </summary>
		Friend Shared ProgramFilesX86 As New Guid(&H7c5a40ef, &Ha0fb, &H4bfc, &H87, &H4a, &Hc0, _
			&Hf2, &He0, &Hb9, &Hfa, &H8e)

		''' <summary>
		''' Common Files
		''' </summary>
		Friend Shared ProgramFilesCommonX86 As New Guid(&Hde974d24UI, &Hd9c6, &H4d3e, &Hbf, &H91, &Hf4, _
			&H45, &H51, &H20, &Hb9, &H17)

		''' <summary>
		''' Program Files
		''' </summary>
		Friend Shared ProgramFilesX64 As New Guid(&H6d809377, &H6af0, &H444b, &H89, &H57, &Ha3, _
			&H77, &H3f, &H2, &H20, &He)

		''' <summary>
		''' Common Files
		''' </summary>
		Friend Shared ProgramFilesCommonX64 As New Guid(&H6365d5a7, &Hf0d, &H45e5, &H87, &Hf6, &Hd, _
			&Ha5, &H6b, &H6a, &H4f, &H7d)

		''' <summary>
		''' Program Files
		''' </summary>
		Friend Shared ProgramFiles As New Guid(&H905e63b6UI, &Hc1bf, &H494e, &Hb2, &H9c, &H65, _
			&Hb7, &H32, &Hd3, &Hd2, &H1a)

		''' <summary>
		''' Common Files
		''' </summary>
		Friend Shared ProgramFilesCommon As New Guid(&Hf7f1ed05UI, &H9f6d, &H47a2, &Haa, &Hae, &H29, _
			&Hd3, &H17, &Hc6, &Hf0, &H66)

		''' <summary>
		''' Administrative Tools
		''' </summary>
		Friend Shared AdminTools As New Guid(&H724ef170, &Ha42d, &H4fef, &H9f, &H26, &Hb6, _
			&He, &H84, &H6f, &Hba, &H4f)

		''' <summary>
		''' Administrative Tools
		''' </summary>
		Friend Shared CommonAdminTools As New Guid(&Hd0384e7dUI, &Hbac3, &H4797, &H8f, &H14, &Hcb, _
			&Ha2, &H29, &Hb3, &H92, &Hb5)

		''' <summary>
		''' Music
		''' </summary>
		Friend Shared Music As New Guid(&H4bd8d571, &H6d19, &H48d3, &Hbe, &H97, &H42, _
			&H22, &H20, &H8, &He, &H43)

		''' <summary>
		''' Videos
		''' </summary>
		Friend Shared Videos As New Guid(&H18989b1d, &H99b5, &H455b, &H84, &H1c, &Hab, _
			&H7c, &H74, &He4, &Hdd, &Hfc)

		''' <summary>
		''' Public Pictures
		''' </summary>
		Friend Shared PublicPictures As New Guid(&Hb6ebfb86UI, &H6907, &H413c, &H9a, &Hf7, &H4f, _
			&Hc2, &Hab, &Hf0, &H7c, &Hc5)

		''' <summary>
		''' Public Music
		''' </summary>
		Friend Shared PublicMusic As New Guid(&H3214fab5, &H9757, &H4298, &Hbb, &H61, &H92, _
			&Ha9, &Hde, &Haa, &H44, &Hff)

		''' <summary>
		''' Public Videos
		''' </summary>
		Friend Shared PublicVideos As New Guid(&H2400183a, &H6185, &H49fb, &Ha2, &Hd8, &H4a, _
			&H39, &H2a, &H60, &H2b, &Ha3)

		''' <summary>
		''' Resources
		''' </summary>
		Friend Shared ResourceDir As New Guid(&H8ad10c31UI, &H2adb, &H4296, &Ha8, &Hf7, &He4, _
			&H70, &H12, &H32, &Hc9, &H72)

		''' <summary>
		''' None
		''' </summary>
		Friend Shared LocalizedResourcesDir As New Guid(&H2a00375e, &H224c, &H49de, &Hb8, &Hd1, &H44, _
			&Hd, &Hf7, &Hef, &H3d, &Hdc)

		''' <summary>
		''' OEM Links
		''' </summary>
		Friend Shared CommonOEMLinks As New Guid(&Hc1bae2d0UI, &H10df, &H4334, &Hbe, &Hdd, &H7a, _
			&Ha2, &Hb, &H22, &H7a, &H9d)

		''' <summary>
		''' Temporary Burn Folder
		''' </summary>
		Friend Shared CDBurning As New Guid(&H9e52ab10UI, &Hf80d, &H49df, &Hac, &Hb8, &H43, _
			&H30, &Hf5, &H68, &H78, &H55)

		''' <summary>
		''' Users
		''' </summary>
		Friend Shared UserProfiles As New Guid(&H762d272, &Hc50a, &H4bb0, &Ha3, &H82, &H69, _
			&H7d, &Hcd, &H72, &H9b, &H80)

		''' <summary>
		''' Playlists
		''' </summary>
		Friend Shared Playlists As New Guid(&Hde92c1c7UI, &H837f, &H4f69, &Ha3, &Hbb, &H86, _
			&He6, &H31, &H20, &H4a, &H23)

		''' <summary>
		''' Sample Playlists
		''' </summary>
		Friend Shared SamplePlaylists As New Guid(&H15ca69b3, &H30ee, &H49c1, &Hac, &He1, &H6b, _
			&H5e, &Hc3, &H72, &Haf, &Hb5)

		''' <summary>
		''' Sample Music
		''' </summary>
		Friend Shared SampleMusic As New Guid(&Hb250c668UI, &Hf57d, &H4ee1, &Ha6, &H3c, &H29, _
			&He, &He7, &Hd1, &Haa, &H1f)

		''' <summary>
		''' Sample Pictures
		''' </summary>
		Friend Shared SamplePictures As New Guid(&Hc4900540UI, &H2379, &H4c75, &H84, &H4b, &H64, _
			&He6, &Hfa, &Hf8, &H71, &H6b)

		''' <summary>
		''' Sample Videos
		''' </summary>
		Friend Shared SampleVideos As New Guid(&H859ead94UI, &H2e85, &H48ad, &Ha7, &H1a, &H9, _
			&H69, &Hcb, &H56, &Ha6, &Hcd)

		''' <summary>
		''' Slide Shows
		''' </summary>
		Friend Shared PhotoAlbums As New Guid(&H69d2cf90, &Hfc33, &H4fb7, &H9a, &Hc, &Heb, _
			&Hb0, &Hf0, &Hfc, &Hb4, &H3c)

		''' <summary>
		''' Public
		''' </summary>
		Friend Shared [Public] As New Guid(&Hdfdf76a2UI, &Hc82a, &H4d63, &H90, &H6a, &H56, _
			&H44, &Hac, &H45, &H73, &H85)

		''' <summary>
		''' Programs and Features
		''' </summary>
		Friend Shared ChangeRemovePrograms As New Guid(&Hdf7266acUI, &H9274, &H4867, &H8d, &H55, &H3b, _
			&Hd6, &H61, &Hde, &H87, &H2d)

		''' <summary>
		''' Installed Updates
		''' </summary>
		Friend Shared AppUpdates As New Guid(&Ha305ce99UI, &Hf527, &H492b, &H8b, &H1a, &H7e, _
			&H76, &Hfa, &H98, &Hd6, &He4)

		''' <summary>
		''' Get Programs
		''' </summary>
		Friend Shared AddNewPrograms As New Guid(&Hde61d971UI, &H5ebc, &H4f02, &Ha3, &Ha9, &H6c, _
			&H82, &H89, &H5e, &H5c, &H4)

		''' <summary>
		''' Downloads
		''' </summary>
		Friend Shared Downloads As New Guid(&H374de290, &H123f, &H4565, &H91, &H64, &H39, _
			&Hc4, &H92, &H5e, &H46, &H7b)

		''' <summary>
		''' Public Downloads
		''' </summary>
		Friend Shared PublicDownloads As New Guid(&H3d644c9b, &H1fb8, &H4f30, &H9b, &H45, &Hf6, _
			&H70, &H23, &H5f, &H79, &Hc0)

		''' <summary>
		''' Searches
		''' </summary>
		Friend Shared SavedSearches As New Guid(&H7d1d3a04, &Hdebb, &H4115, &H95, &Hcf, &H2f, _
			&H29, &Hda, &H29, &H20, &Hda)

		''' <summary>
		''' Quick Launch
		''' </summary>
		Friend Shared QuickLaunch As New Guid(&H52a4f021, &H7b75, &H48a9, &H9f, &H6b, &H4b, _
			&H87, &Ha2, &H10, &Hbc, &H8f)

		''' <summary>
		''' Contacts
		''' </summary>
		Friend Shared Contacts As New Guid(&H56784854, &Hc6cb, &H462b, &H81, &H69, &H88, _
			&He3, &H50, &Hac, &Hb8, &H82)

		''' <summary>
		''' Gadgets
		''' </summary>
		Friend Shared SidebarParts As New Guid(&Ha75d362eUI, &H50fc, &H4fb7, &Hac, &H2c, &Ha8, _
			&Hbe, &Haa, &H31, &H44, &H93)

		''' <summary>
		''' Gadgets
		''' </summary>
		Friend Shared SidebarDefaultParts As New Guid(&H7b396e54, &H9ec5, &H4300, &Hbe, &Ha, &H24, _
			&H82, &Heb, &Hae, &H1a, &H26)

		''' <summary>
		''' Tree property value folder
		''' </summary>
		Friend Shared TreeProperties As New Guid(&H5b3749ad, &Hb49f, &H49c1, &H83, &Heb, &H15, _
			&H37, &Hf, &Hbd, &H48, &H82)

		''' <summary>
		''' GameExplorer
		''' </summary>
		Friend Shared PublicGameTasks As New Guid(&Hdebf2536UI, &He1a8, &H4c59, &Hb6, &Ha2, &H41, _
			&H45, &H86, &H47, &H6a, &Hea)

		''' <summary>
		''' GameExplorer
		''' </summary>
		Friend Shared GameTasks As New Guid(&H54fae61, &H4dd8, &H4787, &H80, &Hb6, &H9, _
			&H2, &H20, &Hc4, &Hb7, &H0)

		''' <summary>
		''' Saved Games
		''' </summary>
		Friend Shared SavedGames As New Guid(&H4c5c32ff, &Hbb9d, &H43b0, &Hb5, &Hb4, &H2d, _
			&H72, &He5, &H4e, &Haa, &Ha4)

		''' <summary>
		''' Games
		''' </summary>
		Friend Shared Games As New Guid(&Hcac52c1aUI, &Hb53d, &H4edc, &H92, &Hd7, &H6b, _
			&H2e, &H8a, &Hc1, &H94, &H34)

		''' <summary>
		''' Recorded TV
		''' </summary>
		Friend Shared RecordedTV As New Guid(&Hbd85e001UI, &H112e, &H431e, &H98, &H3b, &H7b, _
			&H15, &Hac, &H9, &Hff, &Hf1)

		''' <summary>
		''' Microsoft Office Outlook
		''' </summary>
		Friend Shared SearchMapi As New Guid(&H98ec0e18UI, &H2098, &H4d44, &H86, &H44, &H66, _
			&H97, &H93, &H15, &Ha2, &H81)

		''' <summary>
		''' Offline Files
		''' </summary>
		Friend Shared SearchCsc As New Guid(&Hee32e446UI, &H31ca, &H4aba, &H81, &H4f, &Ha5, _
			&Heb, &Hd2, &Hfd, &H6d, &H5e)

		''' <summary>
		''' Links
		''' </summary>
		Friend Shared Links As New Guid(&Hbfb9d5e0UI, &Hc6a9, &H404c, &Hb2, &Hb2, &Hae, _
			&H6d, &Hb6, &Haf, &H49, &H68)

		''' <summary>
		''' The user's full name (for instance, Jean Philippe Bagel) entered when the user account was created.
		''' </summary>
		Friend Shared UsersFiles As New Guid(&Hf3ce0f7cUI, &H4901, &H4acc, &H86, &H48, &Hd5, _
			&Hd4, &H4b, &H4, &Hef, &H8f)

		''' <summary>
		''' Search home
		''' </summary>
		Friend Shared SearchHome As New Guid(&H190337d1, &Hb8ca, &H4121, &Ha6, &H39, &H6d, _
			&H47, &H2d, &H16, &H97, &H2a)

		''' <summary>
		''' Original Images
		''' </summary>
		Friend Shared OriginalImages As New Guid(&H2c36c0aa, &H5812, &H4b87, &Hbf, &Hd0, &H4c, _
			&Hd0, &Hdf, &Hb1, &H9b, &H39)

		#End Region

		#Region "Win7 KnownFolders Guids"

		''' <summary>
		''' UserProgramFiles
		''' </summary>
		Friend Shared UserProgramFiles As New Guid(&H5cd7aee2, &H2219, &H4a67, &Hb8, &H5d, &H6c, _
			&H9c, &He1, &H56, &H60, &Hcb)

		''' <summary>
		''' UserProgramFilesCommon
		''' </summary>
		Friend Shared UserProgramFilesCommon As New Guid(&Hbcbd3057UI, &Hca5c, &H4622, &Hb4, &H2d, &Hbc, _
			&H56, &Hdb, &Ha, &He5, &H16)

		''' <summary>
		''' Ringtones
		''' </summary>
		Friend Shared Ringtones As New Guid(&Hc870044bUI, &Hf49e, &H4126, &Ha9, &Hc3, &Hb5, _
			&H2a, &H1f, &Hf4, &H11, &He8)

		''' <summary>
		''' PublicRingtones
		''' </summary>
		Friend Shared PublicRingtones As New Guid(&He555ab60UI, &H153b, &H4d17, &H9f, &H4, &Ha5, _
			&Hfe, &H99, &Hfc, &H15, &Hec)

		''' <summary>
		''' UsersLibraries
		''' </summary>
		Friend Shared UsersLibraries As New Guid(&Ha302545dUI, &Hdeff, &H464b, &Hab, &He8, &H61, _
			&Hc8, &H64, &H8d, &H93, &H9b)

		''' <summary>
		''' DocumentsLibrary
		''' </summary>
		Friend Shared DocumentsLibrary As New Guid(&H7b0db17d, &H9cd2, &H4a93, &H97, &H33, &H46, _
			&Hcc, &H89, &H2, &H2e, &H7c)

		''' <summary>
		''' MusicLibrary
		''' </summary>
		Friend Shared MusicLibrary As New Guid(&H2112ab0a, &Hc86a, &H4ffe, &Ha3, &H68, &Hd, _
			&He9, &H6e, &H47, &H1, &H2e)

		''' <summary>
		''' PicturesLibrary
		''' </summary>
		Friend Shared PicturesLibrary As New Guid(&Ha990ae9fUI, &Ha03b, &H4e80, &H94, &Hbc, &H99, _
			&H12, &Hd7, &H50, &H41, &H4)

		''' <summary>
		''' VideosLibrary
		''' </summary>
		Friend Shared VideosLibrary As New Guid(&H491e922f, &H5643, &H4af4, &Ha7, &Heb, &H4e, _
			&H7a, &H13, &H8d, &H81, &H74)

		''' <summary>
		''' RecordedTVLibrary
		''' </summary>
		Friend Shared RecordedTVLibrary As New Guid(&H1a6fdba2, &Hf42d, &H4358, &Ha7, &H98, &Hb7, _
			&H4d, &H74, &H59, &H26, &Hc5)

		''' <summary>
		''' OtherUsers
		''' </summary>
		Friend Shared OtherUsers As New Guid(&H52528a6b, &Hb9e3, &H4add, &Hb6, &Hd, &H58, _
			&H8c, &H2d, &Hba, &H84, &H2d)

		''' <summary>
		''' DeviceMetadataStore
		''' </summary>
		Friend Shared DeviceMetadataStore As New Guid(&H5ce4a5e9, &He4eb, &H479d, &Hb8, &H9f, &H13, _
			&Hc, &H2, &H88, &H61, &H55)

		''' <summary>
		''' Libraries
		''' </summary>
		Friend Shared Libraries As New Guid(&H1b3ea5dc, &Hb587, &H4786, &Hb4, &Hef, &Hbd, _
			&H1d, &Hc3, &H32, &Hae, &Hae)

		''' <summary>
		''' UserPinned
		''' </summary>
		Friend Shared UserPinned As New Guid(&H9e3995abUI, &H1f9c, &H4f13, &Hb8, &H27, &H48, _
			&Hb2, &H4b, &H6c, &H71, &H74)

		''' <summary>
		''' ImplicitAppShortcuts
		''' </summary>
		Friend Shared ImplicitAppShortcuts As New Guid(&Hbcb5256fUI, &H79f6, &H4cee, &Hb7, &H25, &Hdc, _
			&H34, &He4, &H2, &Hfd, &H46)

		#End Region
	End Class
End Namespace
