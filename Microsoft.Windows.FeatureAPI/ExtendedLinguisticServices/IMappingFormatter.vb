' Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Defines an interface with which the low-level data contained inside
	''' <see cref="MappingPropertyBag">MappingPropertyBag</see> and
	''' <see cref="MappingDataRange">MappingDataRange</see>
	''' objects can be formatted into .NET objects.
	''' </summary>
	''' <typeparam name="T">The type of the objects into which the low-level data should be converted.</typeparam>
	Public Interface IMappingFormatter(Of T)
		''' <summary>
		''' Formats a single <see cref="MappingDataRange">MappingDataRange</see> into the type specified by T.
		''' </summary>
		''' <param name="dataRange">The <see cref="MappingDataRange">MappingDataRange</see> object to format.</param>
		''' <returns>The formatted object constructed with the data contained inside the <see cref="MappingDataRange">MappingDataRange</see>.</returns>
		Function Format(dataRange As MappingDataRange) As T

		''' <summary>
		''' Formats all <see cref="MappingDataRange">MappingDataRanges</see> contained inside the <see cref="MappingPropertyBag">MappingPropertyBag</see>.
		''' </summary>
		''' <param name="bag">The <see cref="MappingPropertyBag">MappingPropertyBag</see> to format.</param>
		''' <returns>An array of T objects which represent the data contained inside each <see cref="MappingDataRange">MappingDataRange</see> of the
		''' provided <see cref="MappingPropertyBag">MappingPropertyBag</see> object.</returns>
		Function FormatAll(bag As MappingPropertyBag) As T()
	End Interface

End Namespace
