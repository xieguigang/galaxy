'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic

Namespace Shell
	''' <summary>
	''' Collection of all the standard system stock icons
	''' </summary>
	Public Class StockIcons
		#Region "Private Members"

		Private stockIconCache As IDictionary(Of StockIconIdentifier, StockIcon)
		Private m_defaultSize As StockIconSize = StockIconSize.Large
		Private isSelected As Boolean
		Private isLinkOverlay As Boolean

		#End Region

		#Region "Public Constructors"

		''' <summary>
		''' Creates a stock icon collection using the default options for 
		''' size, link overlay and selection state.
		''' </summary>
		Public Sub New()
			' Create an empty dictionary. Stock icons will be created when requested
			' or when they are enumerated on this collection
			stockIconCache = New Dictionary(Of StockIconIdentifier, StockIcon)()

			Dim allIdentifiers As Array = [Enum].GetValues(GetType(StockIconIdentifier))

			For Each id As StockIconIdentifier In allIdentifiers
				stockIconCache.Add(id, Nothing)
			Next
		End Sub

		''' <summary>
		''' Overloaded constructor that takes in size and Boolean values for 
		''' link overlay and selected icon state. The settings are applied to 
		''' all the stock icons in the collection.
		''' </summary>
		''' <param name="size">StockIcon size for all the icons in the collection.</param>
		''' <param name="linkOverlay">Link Overlay state for all the icons in the collection.</param>
		''' <param name="selected">Selection state for all the icons in the collection.</param>
		Public Sub New(size As StockIconSize, linkOverlay As Boolean, selected As Boolean)
			m_defaultSize = size
			isLinkOverlay = linkOverlay
			isSelected = selected

			' Create an empty dictionary. Stock icons will be created when requested
			' or when they are enumerated on this collection
			stockIconCache = New Dictionary(Of StockIconIdentifier, StockIcon)()

			Dim allIdentifiers As Array = [Enum].GetValues(GetType(StockIconIdentifier))

			For Each id As StockIconIdentifier In allIdentifiers
				stockIconCache.Add(id, Nothing)
			Next
		End Sub

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' Gets the default stock icon size in one of the StockIconSize values.
		''' This size applies to all the stock icons in the collection.
		''' </summary>
		Public ReadOnly Property DefaultSize() As StockIconSize
			Get
				Return m_defaultSize
			End Get
		End Property

		''' <summary>
		''' Gets the default link overlay state for the icon. This property 
		''' applies to all the stock icons in the collection.
		''' </summary>
		Public ReadOnly Property DefaultLinkOverlay() As Boolean
			Get
				Return isLinkOverlay
			End Get
		End Property

		''' <summary>
		''' Gets the default selected state for the icon. This property 
		''' applies to all the stock icons in the collection.
		''' </summary>
		Public ReadOnly Property DefaultSelectedState() As Boolean
			Get
				Return isSelected
			End Get
		End Property

		''' <summary>
		''' Gets a collection of all the system stock icons
		''' </summary>
		Public ReadOnly Property AllStockIcons() As ICollection(Of StockIcon)
			Get
				Return GetAllStockIcons()
			End Get
		End Property

		''' <summary>
		''' Icon for a document (blank page), no associated program.
		''' </summary>
		Public ReadOnly Property DocumentNotAssociated() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DocumentNotAssociated)
			End Get
		End Property

		''' <summary>
		''' Icon for a document with an associated program.
		''' </summary>
		Public ReadOnly Property DocumentAssociated() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DocumentAssociated)
			End Get
		End Property

		''' <summary>
		'''  Icon for a generic application with no custom icon.
		''' </summary>
		Public ReadOnly Property Application() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Application)
			End Get
		End Property

		''' <summary>
		'''  Icon for a closed folder.
		''' </summary>
		Public ReadOnly Property Folder() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Folder)
			End Get
		End Property

		''' <summary>
		''' Icon for an open folder. 
		''' </summary>
		Public ReadOnly Property FolderOpen() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.FolderOpen)
			End Get
		End Property

		''' <summary>
		''' Icon for a 5.25" floppy disk drive.
		''' </summary>
		Public ReadOnly Property Drive525() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Drive525)
			End Get
		End Property

		''' <summary>
		'''  Icon for a 3.5" floppy disk drive. 
		''' </summary>
		Public ReadOnly Property Drive35() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Drive35)
			End Get
		End Property

		''' <summary>
		'''  Icon for a removable drive.
		''' </summary>
		Public ReadOnly Property DriveRemove() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveRemove)
			End Get
		End Property

		''' <summary>
		'''  Icon for a fixed (hard disk) drive.
		''' </summary>
		Public ReadOnly Property DriveFixed() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveFixed)
			End Get
		End Property

		''' <summary>
		'''  Icon for a network drive.
		''' </summary>
		Public ReadOnly Property DriveNetwork() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveNetwork)
			End Get
		End Property

		''' <summary>
		'''  Icon for a disconnected network drive.
		''' </summary>
		Public ReadOnly Property DriveNetworkDisabled() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveNetworkDisabled)
			End Get
		End Property

		''' <summary>
		'''  Icon for a CD drive.
		''' </summary>
		Public ReadOnly Property DriveCD() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveCD)
			End Get
		End Property

		''' <summary>
		'''  Icon for a RAM disk drive. 
		''' </summary>
		Public ReadOnly Property DriveRam() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveRam)
			End Get
		End Property

		''' <summary>
		'''  Icon for an entire network. 
		''' </summary>
		Public ReadOnly Property World() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.World)
			End Get
		End Property

		''' <summary>
		'''  Icon for a computer on the network.
		''' </summary>
		Public ReadOnly Property Server() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Server)
			End Get
		End Property

		''' <summary>
		'''  Icon for a printer. 
		''' </summary>
		Public ReadOnly Property Printer() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Printer)
			End Get
		End Property

		''' <summary>
		''' Icon for My Network places.
		''' </summary>
		Public ReadOnly Property MyNetwork() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MyNetwork)
			End Get
		End Property

		''' <summary>
		''' Icon for search (magnifying glass).
		''' </summary>
		Public ReadOnly Property Find() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Find)
			End Get
		End Property

		''' <summary>
		'''  Icon for help.     
		''' </summary>
		Public ReadOnly Property Help() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Help)
			End Get
		End Property

		''' <summary>
		'''  Icon for an overlay indicating shared items.        
		''' </summary>
		Public ReadOnly Property Share() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Share)
			End Get
		End Property

		''' <summary>
		'''  Icon for an overlay indicating shortcuts to items.
		''' </summary>
		Public ReadOnly Property Link() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Link)
			End Get
		End Property

		''' <summary>
		''' Icon for an overlay for slow items.
		''' </summary>
		Public ReadOnly Property SlowFile() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.SlowFile)
			End Get
		End Property

		''' <summary>
		'''  Icon for a empty recycle bin.
		''' </summary>
		Public ReadOnly Property Recycler() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Recycler)
			End Get
		End Property

		''' <summary>
		'''  Icon for a full recycle bin.
		''' </summary>
		Public ReadOnly Property RecyclerFull() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.RecyclerFull)
			End Get
		End Property

		''' <summary>
		'''  Icon for audio CD media.
		''' </summary>
		Public ReadOnly Property MediaCDAudio() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCDAudio)
			End Get
		End Property

		''' <summary>
		'''  Icon for a security lock.
		''' </summary>
		Public ReadOnly Property Lock() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Lock)
			End Get
		End Property

		''' <summary>
		'''  Icon for a auto list.
		''' </summary>
		Public ReadOnly Property AutoList() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.AutoList)
			End Get
		End Property

		''' <summary>
		''' Icon for a network printer.
		''' </summary>
		Public ReadOnly Property PrinterNet() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.PrinterNet)
			End Get
		End Property

		''' <summary>
		'''  Icon for a server share.
		''' </summary>
		Public ReadOnly Property ServerShare() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.ServerShare)
			End Get
		End Property

		''' <summary>
		'''  Icon for a Fax printer.
		''' </summary>
		Public ReadOnly Property PrinterFax() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.PrinterFax)
			End Get
		End Property

		''' <summary>
		''' Icon for a networked Fax printer.
		''' </summary>
		Public ReadOnly Property PrinterFaxNet() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.PrinterFaxNet)
			End Get
		End Property

		''' <summary>
		'''  Icon for print to file.
		''' </summary>
		Public ReadOnly Property PrinterFile() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.PrinterFile)
			End Get
		End Property

		''' <summary>
		''' Icon for a stack.
		''' </summary>
		Public ReadOnly Property Stack() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Stack)
			End Get
		End Property

		''' <summary>
		'''  Icon for a SVCD media.
		''' </summary>
		Public ReadOnly Property MediaSvcd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaSvcd)
			End Get
		End Property

		''' <summary>
		'''  Icon for a folder containing other items.
		''' </summary>
		Public ReadOnly Property StuffedFolder() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.StuffedFolder)
			End Get
		End Property

		''' <summary>
		'''  Icon for an unknown drive.
		''' </summary>
		Public ReadOnly Property DriveUnknown() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveUnknown)
			End Get
		End Property

		''' <summary>
		'''  Icon for a DVD drive. 
		''' </summary>
		Public ReadOnly Property DriveDvd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveDvd)
			End Get
		End Property

		''' <summary>
		''' Icon for DVD media.
		''' </summary>
		Public ReadOnly Property MediaDvd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvd)
			End Get
		End Property

		''' <summary>
		'''  Icon for DVD-RAM media.   
		''' </summary>
		Public ReadOnly Property MediaDvdRam() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvdRam)
			End Get
		End Property

		''' <summary>
		''' Icon for DVD-RW media.
		''' </summary>
		Public ReadOnly Property MediaDvdRW() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvdRW)
			End Get
		End Property

		''' <summary>
		'''  Icon for DVD-R media.
		''' </summary>
		Public ReadOnly Property MediaDvdR() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvdR)
			End Get
		End Property

		''' <summary>
		'''  Icon for a DVD-ROM media.
		''' </summary>
		Public ReadOnly Property MediaDvdRom() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvdRom)
			End Get
		End Property

		''' <summary>
		'''  Icon for CD+ (Enhanced CD) media.
		''' </summary>
		Public ReadOnly Property MediaCDAudioPlus() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCDAudioPlus)
			End Get
		End Property

		''' <summary>
		'''  Icon for CD-RW media.
		''' </summary>
		Public ReadOnly Property MediaCDRW() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCDRW)
			End Get
		End Property

		''' <summary>
		'''  Icon for a CD-R media.
		''' </summary>
		Public ReadOnly Property MediaCDR() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCDR)
			End Get
		End Property

		''' <summary>
		'''  Icon burning a CD.
		''' </summary>
		Public ReadOnly Property MediaCDBurn() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCDBurn)
			End Get
		End Property

		''' <summary>
		'''  Icon for blank CD media.
		''' </summary>
		Public ReadOnly Property MediaBlankCD() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaBlankCD)
			End Get
		End Property

		''' <summary>
		'''  Icon for CD-ROM media.
		''' </summary>
		Public ReadOnly Property MediaCDRom() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCDRom)
			End Get
		End Property

		''' <summary>
		'''  Icon for audio files.
		''' </summary>
		Public ReadOnly Property AudioFiles() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.AudioFiles)
			End Get
		End Property

		''' <summary>
		'''  Icon for image files.
		''' </summary>
		Public ReadOnly Property ImageFiles() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.ImageFiles)
			End Get
		End Property

		''' <summary>
		'''  Icon for video files.
		''' </summary>
		Public ReadOnly Property VideoFiles() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.VideoFiles)
			End Get
		End Property

		''' <summary>
		'''  Icon for mixed Files.
		''' </summary>
		Public ReadOnly Property MixedFiles() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MixedFiles)
			End Get
		End Property

		''' <summary>
		''' Icon for a folder back.
		''' </summary>
		Public ReadOnly Property FolderBack() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.FolderBack)
			End Get
		End Property

		''' <summary>
		'''  Icon for a folder front.
		''' </summary>
		Public ReadOnly Property FolderFront() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.FolderFront)
			End Get
		End Property

		''' <summary>
		'''  Icon for a security shield. Use for UAC prompts only.
		''' </summary>
		Public ReadOnly Property Shield() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Shield)
			End Get
		End Property

		''' <summary>
		'''  Icon for a warning.
		''' </summary>
		Public ReadOnly Property Warning() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Warning)
			End Get
		End Property

		''' <summary>
		'''  Icon for an informational message.
		''' </summary>
		Public ReadOnly Property Info() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Info)
			End Get
		End Property

		''' <summary>
		'''  Icon for an error message.
		''' </summary>
		Public ReadOnly Property [Error]() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.[Error])
			End Get
		End Property

		''' <summary>
		'''  Icon for a key.
		''' </summary>
		Public ReadOnly Property Key() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Key)
			End Get
		End Property

		''' <summary>
		'''  Icon for software.
		''' </summary>
		Public ReadOnly Property Software() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Software)
			End Get
		End Property

		''' <summary>
		'''  Icon for a rename.
		''' </summary>
		Public ReadOnly Property Rename() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Rename)
			End Get
		End Property

		''' <summary>
		'''  Icon for delete.
		''' </summary>
		Public ReadOnly Property Delete() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Delete)
			End Get
		End Property

		''' <summary>
		'''  Icon for audio DVD media.
		''' </summary>
		Public ReadOnly Property MediaAudioDvd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaAudioDvd)
			End Get
		End Property

		''' <summary>
		'''  Icon for movie DVD media.
		''' </summary>
		Public ReadOnly Property MediaMovieDvd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaMovieDvd)
			End Get
		End Property

		''' <summary>
		'''  Icon for enhanced CD media.
		''' </summary>
		Public ReadOnly Property MediaEnhancedCD() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaEnhancedCD)
			End Get
		End Property

		''' <summary>
		'''  Icon for enhanced DVD media.
		''' </summary>
		Public ReadOnly Property MediaEnhancedDvd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaEnhancedDvd)
			End Get
		End Property

		''' <summary>
		'''  Icon for HD-DVD media.
		''' </summary>
		Public ReadOnly Property MediaHDDvd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaHDDvd)
			End Get
		End Property

		''' <summary>
		'''  Icon for BluRay media.
		''' </summary>
		Public ReadOnly Property MediaBluRay() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaBluRay)
			End Get
		End Property

		''' <summary>
		'''  Icon for VCD media.
		''' </summary>
		Public ReadOnly Property MediaVcd() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaVcd)
			End Get
		End Property

		''' <summary>
		'''  Icon for DVD+R media.
		''' </summary>
		Public ReadOnly Property MediaDvdPlusR() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvdPlusR)
			End Get
		End Property

		''' <summary>
		'''  Icon for DVD+RW media.
		''' </summary>
		Public ReadOnly Property MediaDvdPlusRW() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaDvdPlusRW)
			End Get
		End Property

		''' <summary>
		'''  Icon for desktop computer.
		''' </summary>
		Public ReadOnly Property DesktopPC() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DesktopPC)
			End Get
		End Property

		''' <summary>
		'''  Icon for mobile computer (laptop/notebook).
		''' </summary>
		Public ReadOnly Property MobilePC() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MobilePC)
			End Get
		End Property

		''' <summary>
		'''  Icon for users.
		''' </summary>
		Public ReadOnly Property Users() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Users)
			End Get
		End Property

		''' <summary>
		'''  Icon for smart media.
		''' </summary>
		Public ReadOnly Property MediaSmartMedia() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaSmartMedia)
			End Get
		End Property

		''' <summary>
		'''  Icon for compact flash.
		''' </summary>
		Public ReadOnly Property MediaCompactFlash() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaCompactFlash)
			End Get
		End Property

		''' <summary>
		'''  Icon for a cell phone.
		''' </summary>
		Public ReadOnly Property DeviceCellPhone() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DeviceCellPhone)
			End Get
		End Property

		''' <summary>
		'''  Icon for a camera.
		''' </summary>
		Public ReadOnly Property DeviceCamera() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DeviceCamera)
			End Get
		End Property

		''' <summary>
		'''  Icon for video camera.
		''' </summary>
		Public ReadOnly Property DeviceVideoCamera() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DeviceVideoCamera)
			End Get
		End Property

		''' <summary>
		'''  Icon for audio player.
		''' </summary>
		Public ReadOnly Property DeviceAudioPlayer() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DeviceAudioPlayer)
			End Get
		End Property

		''' <summary>
		'''  Icon for connecting to network.
		''' </summary>
		Public ReadOnly Property NetworkConnect() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.NetworkConnect)
			End Get
		End Property

		''' <summary>
		'''  Icon for the Internet.
		''' </summary>
		Public ReadOnly Property Internet() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Internet)
			End Get
		End Property

		''' <summary>
		'''  Icon for a ZIP file.
		''' </summary>
		Public ReadOnly Property ZipFile() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.ZipFile)
			End Get
		End Property

		''' <summary>
		''' Icon for settings.
		''' </summary>
		Public ReadOnly Property Settings() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.Settings)
			End Get
		End Property

		''' <summary>
		''' HDDVD Drive (all types)
		''' </summary>
		Public ReadOnly Property DriveHDDVD() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveHDDVD)
			End Get
		End Property

		''' <summary>
		''' Icon for BluRay Drive (all types)
		''' </summary>
		Public ReadOnly Property DriveBluRay() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.DriveBluRay)
			End Get
		End Property

		''' <summary>
		''' Icon for HDDVD-ROM Media
		''' </summary>
		Public ReadOnly Property MediaHDDVDROM() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaHDDVDROM)
			End Get
		End Property

		''' <summary>
		''' Icon for HDDVD-R Media
		''' </summary>
		Public ReadOnly Property MediaHDDVDR() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaHDDVDR)
			End Get
		End Property

		''' <summary>
		''' Icon for HDDVD-RAM Media
		''' </summary>
		Public ReadOnly Property MediaHDDVDRAM() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaHDDVDRAM)
			End Get
		End Property

		''' <summary>
		''' Icon for BluRay ROM Media
		''' </summary>
		Public ReadOnly Property MediaBluRayROM() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaBluRayROM)
			End Get
		End Property

		''' <summary>
		''' Icon for BluRay R Media
		''' </summary>
		Public ReadOnly Property MediaBluRayR() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaBluRayR)
			End Get
		End Property

		''' <summary>
		''' Icon for BluRay RE Media (Rewriable and RAM)
		''' </summary>
		Public ReadOnly Property MediaBluRayRE() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.MediaBluRayRE)
			End Get
		End Property

		''' <summary>
		''' Icon for Clustered disk
		''' </summary>
		Public ReadOnly Property ClusteredDisk() As StockIcon
			Get
				Return GetStockIcon(StockIconIdentifier.ClusteredDisk)
			End Get
		End Property

		#End Region

		#Region "Private Methods"

		''' <summary>
		''' Returns the existing stock icon from the internal cache, or creates a new one
		''' based on the current settings if it's not in the cache.
		''' </summary>
		''' <param name="stockIconIdentifier">Unique identifier for the requested stock icon</param>
		''' <returns>Stock Icon based on the identifier given (either from the cache or created new)</returns>
		Private Function GetStockIcon(stockIconIdentifier As StockIconIdentifier) As StockIcon
			' Check the cache first
			If stockIconCache(stockIconIdentifier) IsNot Nothing Then
				Return stockIconCache(stockIconIdentifier)
			Else
				' Create a new icon based on our default settings
				Dim icon As New StockIcon(stockIconIdentifier, m_defaultSize, isLinkOverlay, isSelected)

				Try
					' Add it to the cache
					stockIconCache(stockIconIdentifier) = icon
				Catch
					icon.Dispose()
					Throw
				End Try

				' Return 
				Return icon
			End If
		End Function

		Private Function GetAllStockIcons() As ICollection(Of StockIcon)
			' Create a list of stock Identifiers
			Dim ids As StockIconIdentifier() = New StockIconIdentifier(stockIconCache.Count - 1) {}
			stockIconCache.Keys.CopyTo(ids, 0)

			' For each identifier, if our cache is null, create a new stock icon
			For Each id As StockIconIdentifier In ids
				If stockIconCache(id) Is Nothing Then
					GetStockIcon(id)
				End If
			Next

			' return the list of stock icons
			Return stockIconCache.Values
		End Function


		#End Region

	End Class


End Namespace
