'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports Microsoft.Windows.Resources

Namespace Shell
	''' <summary>
	''' The FolderTypes values represent a view template applied to a folder, 
	''' usually based on its intended use and contents.
	''' </summary>
	Friend NotInheritable Class FolderTypes
		Private Sub New()
		End Sub
		''' <summary>
		''' No particular content type has been detected or specified. This value is not supported in Windows 7 and later systems.
		''' </summary>
		Friend Shared NotSpecified As New Guid(&H5c4f28b5, &Hf869, &H4e84, &H8e, &H60, &Hf1, _
			&H1d, &Hb9, &H7c, &H5c, &Hc7)

		''' <summary>
		''' The folder is invalid. There are several things that can cause this judgement: hard disk errors, file system errors, and compression errors among them.
		''' </summary>
		Friend Shared Invalid As New Guid(&H57807898, &H8c4f, &H4462, &Hbb, &H63, &H71, _
			&H4, &H23, &H80, &Hb1, &H9)

		''' <summary>
		''' The folder contains document files. These can be of mixed format.doc, .txt, and others.
		''' </summary>
		Friend Shared Documents As New Guid(&H7d49d726, &H3c21, &H4f05, &H99, &Haa, &Hfd, _
			&Hc2, &Hc9, &H47, &H46, &H56)

		''' <summary>
		''' Image files, such as .jpg, .tif, or .png files.
		''' </summary>
		Friend Shared Pictures As New Guid(&Hb3690e58UI, &He961, &H423b, &Hb6, &H87, &H38, _
			&H6e, &Hbf, &Hd8, &H32, &H39)

		''' <summary>
		''' Windows 7 and later. The folder contains audio files, such as .mp3 and .wma files.
		''' </summary>
		Friend Shared Music As New Guid(&Haf9c03d6UI, &H7db9, &H4a15, &H94, &H64, &H13, _
			&Hbf, &H9f, &Hb6, &H9a, &H2a)

		''' <summary>
		''' A list of music files displayed in Icons view. This value is not supported in Windows 7 and later systems.
		''' </summary>
		Friend Shared MusicIcons As New Guid(&Hb7467fb, &H84ba, &H4aae, &Ha0, &H9b, &H15, _
			&Hb7, &H10, &H97, &Haf, &H9e)

		''' <summary>
		''' The folder is the Games folder found in the Start menu.
		''' </summary>
		Friend Shared Games As New Guid(&Hb689b0d0UI, &H76d3, &H4cbb, &H87, &Hf7, &H58, _
			&H5d, &He, &Hc, &He0, &H70)

		''' <summary>
		''' The Control Panel in category view. This is a virtual folder.
		''' </summary>
		Friend Shared ControlPanelCategory As New Guid(&Hde4f0660UI, &Hfa10, &H4b8f, &Ha4, &H94, &H6, _
			&H8b, &H20, &Hb2, &H23, &H7)

		''' <summary>
		''' The Control Panel in classic view. This is a virtual folder.
		''' </summary>
		Friend Shared ControlPanelClassic As New Guid(&Hc3794f3, &Hb545, &H43aa, &Ha3, &H29, &Hc3, _
			&H74, &H30, &Hc5, &H8d, &H2a)

		''' <summary>
		''' Printers that have been added to the system. This is a virtual folder.
		''' </summary>
		Friend Shared Printers As New Guid(&H2c7bbec6, &Hc844, &H4a0a, &H91, &Hfa, &Hce, _
			&Hf6, &Hf5, &H9c, &Hfd, &Ha1)

		''' <summary>
		''' The Recycle Bin. This is a virtual folder.
		''' </summary>
		Friend Shared RecycleBin As New Guid(&Hd6d9e004UI, &Hcd87, &H442b, &H9d, &H57, &H5e, _
			&Ha, &Heb, &H4f, &H6f, &H72)

		''' <summary>
		''' The software explorer window used by the Add or Remove Programs control panel icon.
		''' </summary>
		Friend Shared SoftwareExplorer As New Guid(&Hd674391bUI, &H52d9, &H4e07, &H83, &H4e, &H67, _
			&Hc9, &H86, &H10, &Hf3, &H9d)

		''' <summary>
		''' The folder is a compressed archive, such as a compressed file with a .zip file name extension.
		''' </summary>
		Friend Shared CompressedFolder As New Guid(&H80213e82UI, &Hbcfd, &H4c4f, &H88, &H17, &Hbb, _
			&H27, &H60, &H12, &H67, &Ha9)

		''' <summary>
		''' An e-mail-related folder that contains contact information.
		''' </summary>
		Friend Shared Contacts As New Guid(&Hde2b70ecUI, &H9bf7, &H4a93, &Hbd, &H3d, &H24, _
			&H3f, &H78, &H81, &Hd4, &H92)

		''' <summary>
		''' A default library view without a more specific template. This value is not supported in Windows 7 and later systems.
		''' </summary>
		Friend Shared Library As New Guid(&H4badfc68, &Hc4ac, &H4716, &Ha0, &Ha0, &H4d, _
			&H5d, &Haa, &H6b, &Hf, &H3e)

		''' <summary>
		''' The Network Explorer folder.
		''' </summary>
		Friend Shared NetworkExplorer As New Guid(&H25cc242b, &H9a7c, &H4f51, &H80, &He0, &H7a, _
			&H29, &H28, &Hfe, &Hbe, &H42)

		''' <summary>
		''' The folder is the FOLDERID_UsersFiles folder.
		''' </summary>
		Friend Shared UserFiles As New Guid(&Hcd0fc69bUI, &H71e2, &H46e5, &H96, &H90, &H5b, _
			&Hcd, &H9f, &H57, &Haa, &Hb3)

		''' <summary>
		''' Windows 7 and later. The folder contains search results, but they are of mixed or no specific type.
		''' </summary>
		Friend Shared GenericSearchResults As New Guid(&H7fde1a1e, &H8b31, &H49a5, &H93, &Hb8, &H6b, _
			&He1, &H4c, &Hfa, &H49, &H43)

		''' <summary>
		''' Windows 7 and later. The folder is a library, but of no specified type.
		''' </summary>
		Friend Shared GenericLibrary As New Guid(&H5f4eab9a, &H6833, &H4f61, &H89, &H9d, &H31, _
			&Hcf, &H46, &H97, &H9d, &H49)

		''' <summary>
		''' Windows 7 and later. The folder contains video files. These can be of mixed format.wmv, .mov, and others.
		''' </summary>
		Friend Shared Videos As New Guid(&H5fa96407, &H7e77, &H483c, &Hac, &H93, &H69, _
			&H1d, &H5, &H85, &Hd, &He8)

		''' <summary>
		''' Windows 7 and later. The view shown when the user clicks the Windows Explorer button on the taskbar.
		''' </summary>
		Friend Shared UsersLibraries As New Guid(&Hc4d98f09UI, &H6124, &H4fe0, &H99, &H42, &H82, _
			&H64, &H16, &H8, &H2d, &Ha9)

		''' <summary>
		''' Windows 7 and later. The homegroup view.
		''' </summary>
		Friend Shared OtherUsers As New Guid(&Hb337fd00UI, &H9dd5, &H4635, &Ha6, &Hd4, &Hda, _
			&H33, &Hfd, &H10, &H2b, &H7a)

		''' <summary>
		''' Windows 7 and later. A folder that contains communication-related files such as e-mails, calendar information, and contact information.
		''' </summary>
		Friend Shared Communications As New Guid(&H91475fe5UI, &H586b, &H4eba, &H8d, &H75, &Hd1, _
			&H74, &H34, &Hb8, &Hcd, &Hf6)

		''' <summary>
		''' Windows 7 and later. The folder contains recorded television broadcasts.
		''' </summary>
		Friend Shared RecordedTV As New Guid(&H5557a28f, &H5da6, &H4f83, &H88, &H9, &Hc2, _
			&Hc9, &H8a, &H11, &Ha6, &Hfa)

		''' <summary>
		''' Windows 7 and later. The folder contains saved game states.
		''' </summary>
		Friend Shared SavedGames As New Guid(&Hd0363307UI, &H28cb, &H4106, &H9f, &H23, &H29, _
			&H56, &He3, &He5, &He0, &He7)

		''' <summary>
		''' Windows 7 and later. The folder contains federated search OpenSearch results.
		''' </summary>
		Friend Shared OpenSearch As New Guid(&H8faf9629UI, &H1980, &H46ff, &H80, &H23, &H9d, _
			&Hce, &Hab, &H9c, &H3e, &He3)

		''' <summary>
		''' Windows 7 and later. Before you search.
		''' </summary>
		Friend Shared SearchConnector As New Guid(&H982725eeUI, &H6f47, &H479e, &Hb4, &H47, &H81, _
			&H2b, &Hfa, &H7d, &H2e, &H8f)

		''' <summary>
		''' Windows 7 and later. A user's Searches folder, normally found at C:\Users\username\Searches.
		''' </summary>
		Friend Shared Searches As New Guid(&Hb0ba2e3, &H405f, &H415e, &Ha6, &Hee, &Hca, _
			&Hd6, &H25, &H20, &H78, &H53)


		Shared types As Dictionary(Of Guid, String)

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")> _
		Shared Sub New()
			types = New Dictionary(Of Guid, String)()
			' Review: These Localized messages could probably be a reflected value of the field's name.
			types.Add(NotSpecified, LocalizedMessages.FolderTypeNotSpecified)
			types.Add(Invalid, LocalizedMessages.FolderTypeInvalid)
			types.Add(Communications, LocalizedMessages.FolderTypeCommunications)
			types.Add(CompressedFolder, LocalizedMessages.FolderTypeCompressedFolder)
			types.Add(Contacts, LocalizedMessages.FolderTypeContacts)
			types.Add(ControlPanelCategory, LocalizedMessages.FolderTypeCategory)
			types.Add(ControlPanelClassic, LocalizedMessages.FolderTypeClassic)
			types.Add(Documents, LocalizedMessages.FolderTypeDocuments)
			types.Add(Games, LocalizedMessages.FolderTypeGames)
			types.Add(GenericSearchResults, LocalizedMessages.FolderTypeSearchResults)
			types.Add(GenericLibrary, LocalizedMessages.FolderTypeGenericLibrary)
			types.Add(Library, LocalizedMessages.FolderTypeLibrary)
			types.Add(Music, LocalizedMessages.FolderTypeMusic)
			types.Add(MusicIcons, LocalizedMessages.FolderTypeMusicIcons)
			types.Add(NetworkExplorer, LocalizedMessages.FolderTypeNetworkExplorer)
			types.Add(OtherUsers, LocalizedMessages.FolderTypeOtherUsers)
			types.Add(OpenSearch, LocalizedMessages.FolderTypeOpenSearch)
			types.Add(Pictures, LocalizedMessages.FolderTypePictures)
			types.Add(Printers, LocalizedMessages.FolderTypePrinters)
			types.Add(RecycleBin, LocalizedMessages.FolderTypeRecycleBin)
			types.Add(RecordedTV, LocalizedMessages.FolderTypeRecordedTV)
			types.Add(SoftwareExplorer, LocalizedMessages.FolderTypeSoftwareExplorer)
			types.Add(SavedGames, LocalizedMessages.FolderTypeSavedGames)
			types.Add(SearchConnector, LocalizedMessages.FolderTypeSearchConnector)
			types.Add(Searches, LocalizedMessages.FolderTypeSearches)
			types.Add(UsersLibraries, LocalizedMessages.FolderTypeUserLibraries)
			types.Add(UserFiles, LocalizedMessages.FolderTypeUserFiles)
			types.Add(Videos, LocalizedMessages.FolderTypeVideos)
		End Sub

		Friend Shared Function GetFolderType(typeId As Guid) As String
			Dim type As String
			Return If(types.TryGetValue(typeId, type), type, String.Empty)
		End Function
	End Class
End Namespace
