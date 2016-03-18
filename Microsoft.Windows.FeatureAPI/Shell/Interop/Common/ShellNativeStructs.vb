Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Shell
	''' <summary>
	''' The STGM constants are flags that indicate 
	''' conditions for creating and deleting the object and access modes 
	''' for the object. 
	''' 
	''' You can combine these flags, but you can only choose one flag 
	''' from each group of related flags. Typically one flag from each 
	''' of the access and sharing groups must be specified for all 
	''' functions and methods which use these constants. 
	''' </summary>
	<Flags> _
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification := "Follows native api.")> _
	Public Enum AccessModes
		''' <summary>
		''' Indicates that, in direct mode, each change to a storage 
		''' or stream element is written as it occurs.
		''' </summary>
		Direct = &H0

		''' <summary>
		''' Indicates that, in transacted mode, changes are buffered 
		''' and written only if an explicit commit operation is called. 
		''' </summary>
		Transacted = &H10000

		''' <summary>
		''' Provides a faster implementation of a compound file 
		''' in a limited, but frequently used, case. 
		''' </summary>
		Simple = &H8000000

		''' <summary>
		''' Indicates that the object is read-only, 
		''' meaning that modifications cannot be made.
		''' </summary>
		Read = &H0

		''' <summary>
		''' Enables you to save changes to the object, 
		''' but does not permit access to its data. 
		''' </summary>
		Write = &H1

		''' <summary>
		''' Enables access and modification of object data.
		''' </summary>
		ReadWrite = &H2

		''' <summary>
		''' Specifies that subsequent openings of the object are 
		''' not denied read or write access. 
		''' </summary>
		ShareDenyNone = &H40

		''' <summary>
		''' Prevents others from subsequently opening the object in Read mode. 
		''' </summary>
		ShareDenyRead = &H30

		''' <summary>
		''' Prevents others from subsequently opening the object 
		''' for Write or ReadWrite access.
		''' </summary>
		ShareDenyWrite = &H20

		''' <summary>
		''' Prevents others from subsequently opening the object in any mode. 
		''' </summary>
		ShareExclusive = &H10

		''' <summary>
		''' Opens the storage object with exclusive access to the most 
		''' recently committed version.
		''' </summary>
		Priority = &H40000

		''' <summary>
		''' Indicates that the underlying file is to be automatically destroyed when the root 
		''' storage object is released. This feature is most useful for creating temporary files. 
		''' </summary>
		DeleteOnRelease = &H4000000

		''' <summary>
		''' Indicates that, in transacted mode, a temporary scratch file is usually used 
		''' to save modifications until the Commit method is called. 
		''' Specifying NoScratch permits the unused portion of the original file 
		''' to be used as work space instead of creating a new file for that purpose. 
		''' </summary>
		NoScratch = &H100000

		''' <summary>
		''' Indicates that an existing storage object 
		''' or stream should be removed before the new object replaces it. 
		''' </summary>
		Create = &H1000

		''' <summary>
		''' Creates the new object while preserving existing data in a stream named "Contents". 
		''' </summary>
		Convert = &H20000

		''' <summary>
		''' Causes the create operation to fail if an existing object with the specified name exists.
		''' </summary>
		FailIfThere = &H0

		''' <summary>
		''' This flag is used when opening a storage object with Transacted 
		''' and without ShareExclusive or ShareDenyWrite. 
		''' In this case, specifying NoSnapshot prevents the system-provided 
		''' implementation from creating a snapshot copy of the file. 
		''' Instead, changes to the file are written to the end of the file. 
		''' </summary>
		NoSnapshot = &H200000

		''' <summary>
		''' Supports direct mode for single-writer, multireader file operations. 
		''' </summary>
		DirectSingleWriterMultipleReader = &H400000
	End Enum
End Namespace
