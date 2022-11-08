using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Xml
{
	// Token: 0x02000007 RID: 7
	internal sealed class Res
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (Res.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref Res.s_InternalSyncObject, value, null);
				}
				return Res.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal Res()
		{
			this.resources = new ResourceManager("System.Xml", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static Res GetLoader()
		{
			if (Res.loader == null)
			{
				lock (Res.InternalSyncObject)
				{
					if (Res.loader == null)
					{
						Res.loader = new Res();
					}
				}
			}
			return Res.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000011B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B7 File Offset: 0x000011B7
		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			string @string = res.resources.GetString(name, Res.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00001248
		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000031 RID: 49
		internal const string Xml_UserException = "Xml_UserException";

		// Token: 0x04000032 RID: 50
		internal const string Xml_DefaultException = "Xml_DefaultException";

		// Token: 0x04000033 RID: 51
		internal const string Xml_InvalidOperation = "Xml_InvalidOperation";

		// Token: 0x04000034 RID: 52
		internal const string Xml_StackOverflow = "Xml_StackOverflow";

		// Token: 0x04000035 RID: 53
		internal const string Xml_ErrorFilePosition = "Xml_ErrorFilePosition";

		// Token: 0x04000036 RID: 54
		internal const string Xslt_NoStylesheetLoaded = "Xslt_NoStylesheetLoaded";

		// Token: 0x04000037 RID: 55
		internal const string Xslt_NotCompiledStylesheet = "Xslt_NotCompiledStylesheet";

		// Token: 0x04000038 RID: 56
		internal const string Xml_UnclosedQuote = "Xml_UnclosedQuote";

		// Token: 0x04000039 RID: 57
		internal const string Xml_UnexpectedEOF = "Xml_UnexpectedEOF";

		// Token: 0x0400003A RID: 58
		internal const string Xml_UnexpectedEOF1 = "Xml_UnexpectedEOF1";

		// Token: 0x0400003B RID: 59
		internal const string Xml_UnexpectedEOFInElementContent = "Xml_UnexpectedEOFInElementContent";

		// Token: 0x0400003C RID: 60
		internal const string Xml_BadStartNameChar = "Xml_BadStartNameChar";

		// Token: 0x0400003D RID: 61
		internal const string Xml_BadNameChar = "Xml_BadNameChar";

		// Token: 0x0400003E RID: 62
		internal const string Xml_BadNameCharWithPos = "Xml_BadNameCharWithPos";

		// Token: 0x0400003F RID: 63
		internal const string Xml_BadDecimalEntity = "Xml_BadDecimalEntity";

		// Token: 0x04000040 RID: 64
		internal const string Xml_BadHexEntity = "Xml_BadHexEntity";

		// Token: 0x04000041 RID: 65
		internal const string Xml_MissingByteOrderMark = "Xml_MissingByteOrderMark";

		// Token: 0x04000042 RID: 66
		internal const string Xml_UnknownEncoding = "Xml_UnknownEncoding";

		// Token: 0x04000043 RID: 67
		internal const string Xml_InternalError = "Xml_InternalError";

		// Token: 0x04000044 RID: 68
		internal const string Xml_InvalidCharInThisEncoding = "Xml_InvalidCharInThisEncoding";

		// Token: 0x04000045 RID: 69
		internal const string Xml_ErrorPosition = "Xml_ErrorPosition";

		// Token: 0x04000046 RID: 70
		internal const string Xml_UnexpectedTokenEx = "Xml_UnexpectedTokenEx";

		// Token: 0x04000047 RID: 71
		internal const string Xml_UnexpectedTokens2 = "Xml_UnexpectedTokens2";

		// Token: 0x04000048 RID: 72
		internal const string Xml_ExpectingWhiteSpace = "Xml_ExpectingWhiteSpace";

		// Token: 0x04000049 RID: 73
		internal const string Xml_TagMismatch = "Xml_TagMismatch";

		// Token: 0x0400004A RID: 74
		internal const string Xml_UnexpectedEndTag = "Xml_UnexpectedEndTag";

		// Token: 0x0400004B RID: 75
		internal const string Xml_UnknownNs = "Xml_UnknownNs";

		// Token: 0x0400004C RID: 76
		internal const string Xml_BadAttributeChar = "Xml_BadAttributeChar";

		// Token: 0x0400004D RID: 77
		internal const string Xml_MissingRoot = "Xml_MissingRoot";

		// Token: 0x0400004E RID: 78
		internal const string Xml_MultipleRoots = "Xml_MultipleRoots";

		// Token: 0x0400004F RID: 79
		internal const string Xml_InvalidRootData = "Xml_InvalidRootData";

		// Token: 0x04000050 RID: 80
		internal const string Xml_XmlDeclNotFirst = "Xml_XmlDeclNotFirst";

		// Token: 0x04000051 RID: 81
		internal const string Xml_InvalidXmlDecl = "Xml_InvalidXmlDecl";

		// Token: 0x04000052 RID: 82
		internal const string Xml_InvalidNodeType = "Xml_InvalidNodeType";

		// Token: 0x04000053 RID: 83
		internal const string Xml_InvalidPIName = "Xml_InvalidPIName";

		// Token: 0x04000054 RID: 84
		internal const string Xml_InvalidXmlSpace = "Xml_InvalidXmlSpace";

		// Token: 0x04000055 RID: 85
		internal const string Xml_InvalidVersionNumber = "Xml_InvalidVersionNumber";

		// Token: 0x04000056 RID: 86
		internal const string Xml_DupAttributeName = "Xml_DupAttributeName";

		// Token: 0x04000057 RID: 87
		internal const string Xml_BadDTDLocation = "Xml_BadDTDLocation";

		// Token: 0x04000058 RID: 88
		internal const string Xml_ElementNotFound = "Xml_ElementNotFound";

		// Token: 0x04000059 RID: 89
		internal const string Xml_ElementNotFoundNs = "Xml_ElementNotFoundNs";

		// Token: 0x0400005A RID: 90
		internal const string Xml_PartialContentNodeTypeNotSupportedEx = "Xml_PartialContentNodeTypeNotSupportedEx";

		// Token: 0x0400005B RID: 91
		internal const string Xml_MultipleDTDsProvided = "Xml_MultipleDTDsProvided";

		// Token: 0x0400005C RID: 92
		internal const string Xml_CanNotBindToReservedNamespace = "Xml_CanNotBindToReservedNamespace";

		// Token: 0x0400005D RID: 93
		internal const string Xml_XmlnsBelongsToReservedNs = "Xml_XmlnsBelongsToReservedNs";

		// Token: 0x0400005E RID: 94
		internal const string Xml_InvalidCharacter = "Xml_InvalidCharacter";

		// Token: 0x0400005F RID: 95
		internal const string Xml_ExpectDtdMarkup = "Xml_ExpectDtdMarkup";

		// Token: 0x04000060 RID: 96
		internal const string Xml_InvalidBinHexValue = "Xml_InvalidBinHexValue";

		// Token: 0x04000061 RID: 97
		internal const string Xml_InvalidBinHexValueOddCount = "Xml_InvalidBinHexValueOddCount";

		// Token: 0x04000062 RID: 98
		internal const string Xml_InvalidTextDecl = "Xml_InvalidTextDecl";

		// Token: 0x04000063 RID: 99
		internal const string Xml_InvalidBase64Value = "Xml_InvalidBase64Value";

		// Token: 0x04000064 RID: 100
		internal const string Xml_ExpectExternalOrPublicId = "Xml_ExpectExternalOrPublicId";

		// Token: 0x04000065 RID: 101
		internal const string Xml_ExpectExternalIdOrEntityValue = "Xml_ExpectExternalIdOrEntityValue";

		// Token: 0x04000066 RID: 102
		internal const string Xml_ExpectAttType = "Xml_ExpectAttType";

		// Token: 0x04000067 RID: 103
		internal const string Xml_ExpectIgnoreOrInclude = "Xml_ExpectIgnoreOrInclude";

		// Token: 0x04000068 RID: 104
		internal const string Xml_ExpectSubOrClose = "Xml_ExpectSubOrClose";

		// Token: 0x04000069 RID: 105
		internal const string Xml_ExpectExternalOrClose = "Xml_ExpectExternalOrClose";

		// Token: 0x0400006A RID: 106
		internal const string Xml_ExpectOp = "Xml_ExpectOp";

		// Token: 0x0400006B RID: 107
		internal const string Xml_ExpectNoWhitespace = "Xml_ExpectNoWhitespace";

		// Token: 0x0400006C RID: 108
		internal const string Xml_ExpectPcData = "Xml_ExpectPcData";

		// Token: 0x0400006D RID: 109
		internal const string Xml_UndeclaredParEntity = "Xml_UndeclaredParEntity";

		// Token: 0x0400006E RID: 110
		internal const string Xml_UndeclaredEntity = "Xml_UndeclaredEntity";

		// Token: 0x0400006F RID: 111
		internal const string Xml_RecursiveParEntity = "Xml_RecursiveParEntity";

		// Token: 0x04000070 RID: 112
		internal const string Xml_RecursiveGenEntity = "Xml_RecursiveGenEntity";

		// Token: 0x04000071 RID: 113
		internal const string Xml_ExternalEntityInAttValue = "Xml_ExternalEntityInAttValue";

		// Token: 0x04000072 RID: 114
		internal const string Xml_UnparsedEntityRef = "Xml_UnparsedEntityRef";

		// Token: 0x04000073 RID: 115
		internal const string Xml_InvalidConditionalSection = "Xml_InvalidConditionalSection";

		// Token: 0x04000074 RID: 116
		internal const string Xml_UnclosedConditionalSection = "Xml_UnclosedConditionalSection";

		// Token: 0x04000075 RID: 117
		internal const string Xml_InvalidParEntityRef = "Xml_InvalidParEntityRef";

		// Token: 0x04000076 RID: 118
		internal const string Xml_InvalidContentModel = "Xml_InvalidContentModel";

		// Token: 0x04000077 RID: 119
		internal const string Xml_InvalidXmlDocument = "Xml_InvalidXmlDocument";

		// Token: 0x04000078 RID: 120
		internal const string Xml_FragmentId = "Xml_FragmentId";

		// Token: 0x04000079 RID: 121
		internal const string Xml_ColonInLocalName = "Xml_ColonInLocalName";

		// Token: 0x0400007A RID: 122
		internal const string Xml_InvalidAttributeType = "Xml_InvalidAttributeType";

		// Token: 0x0400007B RID: 123
		internal const string Xml_InvalidAttributeType1 = "Xml_InvalidAttributeType1";

		// Token: 0x0400007C RID: 124
		internal const string Xml_UnexpectedCDataEnd = "Xml_UnexpectedCDataEnd";

		// Token: 0x0400007D RID: 125
		internal const string Xml_EnumerationRequired = "Xml_EnumerationRequired";

		// Token: 0x0400007E RID: 126
		internal const string Xml_NotSameNametable = "Xml_NotSameNametable";

		// Token: 0x0400007F RID: 127
		internal const string Xml_NametableMismatch = "Xml_NametableMismatch";

		// Token: 0x04000080 RID: 128
		internal const string Xml_NoDTDPresent = "Xml_NoDTDPresent";

		// Token: 0x04000081 RID: 129
		internal const string Xml_MultipleValidaitonTypes = "Xml_MultipleValidaitonTypes";

		// Token: 0x04000082 RID: 130
		internal const string Xml_BadNamespaceDecl = "Xml_BadNamespaceDecl";

		// Token: 0x04000083 RID: 131
		internal const string Xml_ErrorParsingEntityName = "Xml_ErrorParsingEntityName";

		// Token: 0x04000084 RID: 132
		internal const string Xml_NoValidation = "Xml_NoValidation";

		// Token: 0x04000085 RID: 133
		internal const string Xml_WhitespaceHandling = "Xml_WhitespaceHandling";

		// Token: 0x04000086 RID: 134
		internal const string Xml_InvalidResetStateCall = "Xml_InvalidResetStateCall";

		// Token: 0x04000087 RID: 135
		internal const string Xml_EntityHandling = "Xml_EntityHandling";

		// Token: 0x04000088 RID: 136
		internal const string Xml_InvalidNmToken = "Xml_InvalidNmToken";

		// Token: 0x04000089 RID: 137
		internal const string Xml_EntityRefNesting = "Xml_EntityRefNesting";

		// Token: 0x0400008A RID: 138
		internal const string Xml_CannotResolveEntity = "Xml_CannotResolveEntity";

		// Token: 0x0400008B RID: 139
		internal const string Xml_CannotResolveExternalSubset = "Xml_CannotResolveExternalSubset";

		// Token: 0x0400008C RID: 140
		internal const string Xml_CannotResolveUrl = "Xml_CannotResolveUrl";

		// Token: 0x0400008D RID: 141
		internal const string Xml_CDATAEndInText = "Xml_CDATAEndInText";

		// Token: 0x0400008E RID: 142
		internal const string Xml_ExternalEntityInStandAloneDocument = "Xml_ExternalEntityInStandAloneDocument";

		// Token: 0x0400008F RID: 143
		internal const string Xml_DtdAfterRootElement = "Xml_DtdAfterRootElement";

		// Token: 0x04000090 RID: 144
		internal const string Xml_ReadOnlyProperty = "Xml_ReadOnlyProperty";

		// Token: 0x04000091 RID: 145
		internal const string Xml_DtdIsProhibited = "Xml_DtdIsProhibited";

		// Token: 0x04000092 RID: 146
		internal const string Xml_DtdIsProhibitedEx = "Xml_DtdIsProhibitedEx";

		// Token: 0x04000093 RID: 147
		internal const string Xml_AttlistDuplEnumValue = "Xml_AttlistDuplEnumValue";

		// Token: 0x04000094 RID: 148
		internal const string Xml_AttlistDuplNotationValue = "Xml_AttlistDuplNotationValue";

		// Token: 0x04000095 RID: 149
		internal const string Xml_EncodingSwitchAfterResetState = "Xml_EncodingSwitchAfterResetState";

		// Token: 0x04000096 RID: 150
		internal const string Xml_ReadSubtreeNotOnElement = "Xml_ReadSubtreeNotOnElement";

		// Token: 0x04000097 RID: 151
		internal const string Xml_DtdNotAllowedInFragment = "Xml_DtdNotAllowedInFragment";

		// Token: 0x04000098 RID: 152
		internal const string Xml_CannotStartDocumentOnFragment = "Xml_CannotStartDocumentOnFragment";

		// Token: 0x04000099 RID: 153
		internal const string Xml_InvalidWhitespaceCharacter = "Xml_InvalidWhitespaceCharacter";

		// Token: 0x0400009A RID: 154
		internal const string Xml_IncompatibleConformanceLevel = "Xml_IncompatibleConformanceLevel";

		// Token: 0x0400009B RID: 155
		internal const string Xml_BinaryXmlReadAsText = "Xml_BinaryXmlReadAsText";

		// Token: 0x0400009C RID: 156
		internal const string Xml_UnexpectedNodeType = "Xml_UnexpectedNodeType";

		// Token: 0x0400009D RID: 157
		internal const string Xml_ErrorOpeningExternalDtd = "Xml_ErrorOpeningExternalDtd";

		// Token: 0x0400009E RID: 158
		internal const string Xml_ErrorOpeningExternalEntity = "Xml_ErrorOpeningExternalEntity";

		// Token: 0x0400009F RID: 159
		internal const string Xml_ReadBinaryContentNotSupported = "Xml_ReadBinaryContentNotSupported";

		// Token: 0x040000A0 RID: 160
		internal const string Xml_ReadValueChunkNotSupported = "Xml_ReadValueChunkNotSupported";

		// Token: 0x040000A1 RID: 161
		internal const string Xml_InvalidReadContentAs = "Xml_InvalidReadContentAs";

		// Token: 0x040000A2 RID: 162
		internal const string Xml_InvalidReadElementContentAs = "Xml_InvalidReadElementContentAs";

		// Token: 0x040000A3 RID: 163
		internal const string Xml_MixedReadElementContentAs = "Xml_MixedReadElementContentAs";

		// Token: 0x040000A4 RID: 164
		internal const string Xml_MixingReadValueChunkWithBinary = "Xml_MixingReadValueChunkWithBinary";

		// Token: 0x040000A5 RID: 165
		internal const string Xml_MixingBinaryContentMethods = "Xml_MixingBinaryContentMethods";

		// Token: 0x040000A6 RID: 166
		internal const string Xml_MixingV1StreamingWithV2Binary = "Xml_MixingV1StreamingWithV2Binary";

		// Token: 0x040000A7 RID: 167
		internal const string Xml_InvalidReadValueChunk = "Xml_InvalidReadValueChunk";

		// Token: 0x040000A8 RID: 168
		internal const string Xml_ReadContentAsFormatException = "Xml_ReadContentAsFormatException";

		// Token: 0x040000A9 RID: 169
		internal const string Xml_DoubleBaseUri = "Xml_DoubleBaseUri";

		// Token: 0x040000AA RID: 170
		internal const string Xml_NotEnoughSpaceForSurrogatePair = "Xml_NotEnoughSpaceForSurrogatePair";

		// Token: 0x040000AB RID: 171
		internal const string Xml_EmptyUrl = "Xml_EmptyUrl";

		// Token: 0x040000AC RID: 172
		internal const string Xml_UnexpectedNodeInSimpleContent = "Xml_UnexpectedNodeInSimpleContent";

		// Token: 0x040000AD RID: 173
		internal const string Xml_UnsupportedClass = "Xml_UnsupportedClass";

		// Token: 0x040000AE RID: 174
		internal const string Xml_NullResolver = "Xml_NullResolver";

		// Token: 0x040000AF RID: 175
		internal const string Xml_UntrustedCodeSettingResolver = "Xml_UntrustedCodeSettingResolver";

		// Token: 0x040000B0 RID: 176
		internal const string Xml_InvalidQuote = "Xml_InvalidQuote";

		// Token: 0x040000B1 RID: 177
		internal const string Xml_UndefPrefix = "Xml_UndefPrefix";

		// Token: 0x040000B2 RID: 178
		internal const string Xml_PrefixForEmptyNs = "Xml_PrefixForEmptyNs";

		// Token: 0x040000B3 RID: 179
		internal const string Xml_NoNamespaces = "Xml_NoNamespaces";

		// Token: 0x040000B4 RID: 180
		internal const string Xml_InvalidCDataChars = "Xml_InvalidCDataChars";

		// Token: 0x040000B5 RID: 181
		internal const string Xml_InvalidCommentChars = "Xml_InvalidCommentChars";

		// Token: 0x040000B6 RID: 182
		internal const string Xml_NotTheFirst = "Xml_NotTheFirst";

		// Token: 0x040000B7 RID: 183
		internal const string Xml_InvalidPiChars = "Xml_InvalidPiChars";

		// Token: 0x040000B8 RID: 184
		internal const string Xml_UndefNamespace = "Xml_UndefNamespace";

		// Token: 0x040000B9 RID: 185
		internal const string Xml_EmptyName = "Xml_EmptyName";

		// Token: 0x040000BA RID: 186
		internal const string Xml_EmptyLocalName = "Xml_EmptyLocalName";

		// Token: 0x040000BB RID: 187
		internal const string Xml_InvalidNameChars = "Xml_InvalidNameChars";

		// Token: 0x040000BC RID: 188
		internal const string Xml_InvalidNameCharsDetail = "Xml_InvalidNameCharsDetail";

		// Token: 0x040000BD RID: 189
		internal const string Xml_NoStartTag = "Xml_NoStartTag";

		// Token: 0x040000BE RID: 190
		internal const string Xml_Closed = "Xml_Closed";

		// Token: 0x040000BF RID: 191
		internal const string Xml_ClosedOrError = "Xml_ClosedOrError";

		// Token: 0x040000C0 RID: 192
		internal const string Xml_WrongToken = "Xml_WrongToken";

		// Token: 0x040000C1 RID: 193
		internal const string Xml_InvalidPrefix = "Xml_InvalidPrefix";

		// Token: 0x040000C2 RID: 194
		internal const string Xml_XmlPrefix = "Xml_XmlPrefix";

		// Token: 0x040000C3 RID: 195
		internal const string Xml_XmlnsPrefix = "Xml_XmlnsPrefix";

		// Token: 0x040000C4 RID: 196
		internal const string Xml_NamespaceDeclXmlXmlns = "Xml_NamespaceDeclXmlXmlns";

		// Token: 0x040000C5 RID: 197
		internal const string Xml_NonWhitespace = "Xml_NonWhitespace";

		// Token: 0x040000C6 RID: 198
		internal const string Xml_DupXmlDecl = "Xml_DupXmlDecl";

		// Token: 0x040000C7 RID: 199
		internal const string Xml_CannotWriteXmlDecl = "Xml_CannotWriteXmlDecl";

		// Token: 0x040000C8 RID: 200
		internal const string Xml_NoRoot = "Xml_NoRoot";

		// Token: 0x040000C9 RID: 201
		internal const string Xml_InvalidIndentation = "Xml_InvalidIndentation";

		// Token: 0x040000CA RID: 202
		internal const string Xml_NotInWriteState = "Xml_NotInWriteState";

		// Token: 0x040000CB RID: 203
		internal const string Xml_InvalidPosition = "Xml_InvalidPosition";

		// Token: 0x040000CC RID: 204
		internal const string Xml_IncompleteEntity = "Xml_IncompleteEntity";

		// Token: 0x040000CD RID: 205
		internal const string Xml_IncompleteDtdContent = "Xml_IncompleteDtdContent";

		// Token: 0x040000CE RID: 206
		internal const string Xml_InvalidSurrogateHighChar = "Xml_InvalidSurrogateHighChar";

		// Token: 0x040000CF RID: 207
		internal const string Xml_InvalidSurrogateMissingLowChar = "Xml_InvalidSurrogateMissingLowChar";

		// Token: 0x040000D0 RID: 208
		internal const string Xml_InvalidSurrogatePairWithArgs = "Xml_InvalidSurrogatePairWithArgs";

		// Token: 0x040000D1 RID: 209
		internal const string Xml_SurrogatePairSplit = "Xml_SurrogatePairSplit";

		// Token: 0x040000D2 RID: 210
		internal const string Xml_NoMultipleRoots = "Xml_NoMultipleRoots";

		// Token: 0x040000D3 RID: 211
		internal const string Xml_RedefinePrefix = "Xml_RedefinePrefix";

		// Token: 0x040000D4 RID: 212
		internal const string Xml_DtdAlreadyWritten = "Xml_DtdAlreadyWritten";

		// Token: 0x040000D5 RID: 213
		internal const string XmlBadName = "XmlBadName";

		// Token: 0x040000D6 RID: 214
		internal const string XmlNoNameAllowed = "XmlNoNameAllowed";

		// Token: 0x040000D7 RID: 215
		internal const string Xml_InvalidCharsInIndent = "Xml_InvalidCharsInIndent";

		// Token: 0x040000D8 RID: 216
		internal const string Xml_IndentCharsNotWhitespace = "Xml_IndentCharsNotWhitespace";

		// Token: 0x040000D9 RID: 217
		internal const string Xml_ConformanceLevelFragment = "Xml_ConformanceLevelFragment";

		// Token: 0x040000DA RID: 218
		internal const string XmlDocument_ValidateInvalidNodeType = "XmlDocument_ValidateInvalidNodeType";

		// Token: 0x040000DB RID: 219
		internal const string XmlDocument_NodeNotFromDocument = "XmlDocument_NodeNotFromDocument";

		// Token: 0x040000DC RID: 220
		internal const string XmlDocument_NoNodeSchemaInfo = "XmlDocument_NoNodeSchemaInfo";

		// Token: 0x040000DD RID: 221
		internal const string XmlDocument_NoSchemaInfo = "XmlDocument_NoSchemaInfo";

		// Token: 0x040000DE RID: 222
		internal const string Sch_DefaultException = "Sch_DefaultException";

		// Token: 0x040000DF RID: 223
		internal const string Sch_ParEntityRefNesting = "Sch_ParEntityRefNesting";

		// Token: 0x040000E0 RID: 224
		internal const string Sch_DupElementDecl = "Sch_DupElementDecl";

		// Token: 0x040000E1 RID: 225
		internal const string Sch_IdAttrDeclared = "Sch_IdAttrDeclared";

		// Token: 0x040000E2 RID: 226
		internal const string Sch_RootMatchDocType = "Sch_RootMatchDocType";

		// Token: 0x040000E3 RID: 227
		internal const string Sch_DupId = "Sch_DupId";

		// Token: 0x040000E4 RID: 228
		internal const string Sch_UndeclaredElement = "Sch_UndeclaredElement";

		// Token: 0x040000E5 RID: 229
		internal const string Sch_UndeclaredAttribute = "Sch_UndeclaredAttribute";

		// Token: 0x040000E6 RID: 230
		internal const string Sch_UndeclaredNotation = "Sch_UndeclaredNotation";

		// Token: 0x040000E7 RID: 231
		internal const string Sch_UndeclaredId = "Sch_UndeclaredId";

		// Token: 0x040000E8 RID: 232
		internal const string Sch_SchemaRootExpected = "Sch_SchemaRootExpected";

		// Token: 0x040000E9 RID: 233
		internal const string Sch_XSDSchemaRootExpected = "Sch_XSDSchemaRootExpected";

		// Token: 0x040000EA RID: 234
		internal const string Sch_UnsupportedAttribute = "Sch_UnsupportedAttribute";

		// Token: 0x040000EB RID: 235
		internal const string Sch_UnsupportedElement = "Sch_UnsupportedElement";

		// Token: 0x040000EC RID: 236
		internal const string Sch_MissAttribute = "Sch_MissAttribute";

		// Token: 0x040000ED RID: 237
		internal const string Sch_AnnotationLocation = "Sch_AnnotationLocation";

		// Token: 0x040000EE RID: 238
		internal const string Sch_DataTypeTextOnly = "Sch_DataTypeTextOnly";

		// Token: 0x040000EF RID: 239
		internal const string Sch_UnknownModel = "Sch_UnknownModel";

		// Token: 0x040000F0 RID: 240
		internal const string Sch_UnknownOrder = "Sch_UnknownOrder";

		// Token: 0x040000F1 RID: 241
		internal const string Sch_UnknownContent = "Sch_UnknownContent";

		// Token: 0x040000F2 RID: 242
		internal const string Sch_UnknownRequired = "Sch_UnknownRequired";

		// Token: 0x040000F3 RID: 243
		internal const string Sch_UnknownDtType = "Sch_UnknownDtType";

		// Token: 0x040000F4 RID: 244
		internal const string Sch_MixedMany = "Sch_MixedMany";

		// Token: 0x040000F5 RID: 245
		internal const string Sch_GroupDisabled = "Sch_GroupDisabled";

		// Token: 0x040000F6 RID: 246
		internal const string Sch_MissDtvalue = "Sch_MissDtvalue";

		// Token: 0x040000F7 RID: 247
		internal const string Sch_MissDtvaluesAttribute = "Sch_MissDtvaluesAttribute";

		// Token: 0x040000F8 RID: 248
		internal const string Sch_DupDtType = "Sch_DupDtType";

		// Token: 0x040000F9 RID: 249
		internal const string Sch_DupAttribute = "Sch_DupAttribute";

		// Token: 0x040000FA RID: 250
		internal const string Sch_RequireEnumeration = "Sch_RequireEnumeration";

		// Token: 0x040000FB RID: 251
		internal const string Sch_DefaultIdValue = "Sch_DefaultIdValue";

		// Token: 0x040000FC RID: 252
		internal const string Sch_ElementNotAllowed = "Sch_ElementNotAllowed";

		// Token: 0x040000FD RID: 253
		internal const string Sch_ElementMissing = "Sch_ElementMissing";

		// Token: 0x040000FE RID: 254
		internal const string Sch_ManyMaxOccurs = "Sch_ManyMaxOccurs";

		// Token: 0x040000FF RID: 255
		internal const string Sch_MaxOccursInvalid = "Sch_MaxOccursInvalid";

		// Token: 0x04000100 RID: 256
		internal const string Sch_MinOccursInvalid = "Sch_MinOccursInvalid";

		// Token: 0x04000101 RID: 257
		internal const string Sch_DtMaxLengthInvalid = "Sch_DtMaxLengthInvalid";

		// Token: 0x04000102 RID: 258
		internal const string Sch_DtMinLengthInvalid = "Sch_DtMinLengthInvalid";

		// Token: 0x04000103 RID: 259
		internal const string Sch_DupDtMaxLength = "Sch_DupDtMaxLength";

		// Token: 0x04000104 RID: 260
		internal const string Sch_DupDtMinLength = "Sch_DupDtMinLength";

		// Token: 0x04000105 RID: 261
		internal const string Sch_DtMinMaxLength = "Sch_DtMinMaxLength";

		// Token: 0x04000106 RID: 262
		internal const string Sch_DupElement = "Sch_DupElement";

		// Token: 0x04000107 RID: 263
		internal const string Sch_DupGroupParticle = "Sch_DupGroupParticle";

		// Token: 0x04000108 RID: 264
		internal const string Sch_InvalidValue = "Sch_InvalidValue";

		// Token: 0x04000109 RID: 265
		internal const string Sch_InvalidValueDetailed = "Sch_InvalidValueDetailed";

		// Token: 0x0400010A RID: 266
		internal const string Sch_MissRequiredAttribute = "Sch_MissRequiredAttribute";

		// Token: 0x0400010B RID: 267
		internal const string Sch_FixedAttributeValue = "Sch_FixedAttributeValue";

		// Token: 0x0400010C RID: 268
		internal const string Sch_FixedElementValue = "Sch_FixedElementValue";

		// Token: 0x0400010D RID: 269
		internal const string Sch_AttributeValueDataTypeDetailed = "Sch_AttributeValueDataTypeDetailed";

		// Token: 0x0400010E RID: 270
		internal const string Sch_AttributeDefaultDataType = "Sch_AttributeDefaultDataType";

		// Token: 0x0400010F RID: 271
		internal const string Sch_IncludeLocation = "Sch_IncludeLocation";

		// Token: 0x04000110 RID: 272
		internal const string Sch_ImportLocation = "Sch_ImportLocation";

		// Token: 0x04000111 RID: 273
		internal const string Sch_RedefineLocation = "Sch_RedefineLocation";

		// Token: 0x04000112 RID: 274
		internal const string Sch_InvalidBlockDefaultValue = "Sch_InvalidBlockDefaultValue";

		// Token: 0x04000113 RID: 275
		internal const string Sch_InvalidFinalDefaultValue = "Sch_InvalidFinalDefaultValue";

		// Token: 0x04000114 RID: 276
		internal const string Sch_InvalidElementBlockValue = "Sch_InvalidElementBlockValue";

		// Token: 0x04000115 RID: 277
		internal const string Sch_InvalidElementFinalValue = "Sch_InvalidElementFinalValue";

		// Token: 0x04000116 RID: 278
		internal const string Sch_InvalidSimpleTypeFinalValue = "Sch_InvalidSimpleTypeFinalValue";

		// Token: 0x04000117 RID: 279
		internal const string Sch_InvalidComplexTypeBlockValue = "Sch_InvalidComplexTypeBlockValue";

		// Token: 0x04000118 RID: 280
		internal const string Sch_InvalidComplexTypeFinalValue = "Sch_InvalidComplexTypeFinalValue";

		// Token: 0x04000119 RID: 281
		internal const string Sch_DupIdentityConstraint = "Sch_DupIdentityConstraint";

		// Token: 0x0400011A RID: 282
		internal const string Sch_DupGlobalElement = "Sch_DupGlobalElement";

		// Token: 0x0400011B RID: 283
		internal const string Sch_DupGlobalAttribute = "Sch_DupGlobalAttribute";

		// Token: 0x0400011C RID: 284
		internal const string Sch_DupSimpleType = "Sch_DupSimpleType";

		// Token: 0x0400011D RID: 285
		internal const string Sch_DupComplexType = "Sch_DupComplexType";

		// Token: 0x0400011E RID: 286
		internal const string Sch_DupGroup = "Sch_DupGroup";

		// Token: 0x0400011F RID: 287
		internal const string Sch_DupAttributeGroup = "Sch_DupAttributeGroup";

		// Token: 0x04000120 RID: 288
		internal const string Sch_DupNotation = "Sch_DupNotation";

		// Token: 0x04000121 RID: 289
		internal const string Sch_DefaultFixedAttributes = "Sch_DefaultFixedAttributes";

		// Token: 0x04000122 RID: 290
		internal const string Sch_FixedInRef = "Sch_FixedInRef";

		// Token: 0x04000123 RID: 291
		internal const string Sch_FixedDefaultInRef = "Sch_FixedDefaultInRef";

		// Token: 0x04000124 RID: 292
		internal const string Sch_DupXsdElement = "Sch_DupXsdElement";

		// Token: 0x04000125 RID: 293
		internal const string Sch_ForbiddenAttribute = "Sch_ForbiddenAttribute";

		// Token: 0x04000126 RID: 294
		internal const string Sch_AttributeIgnored = "Sch_AttributeIgnored";

		// Token: 0x04000127 RID: 295
		internal const string Sch_ElementRef = "Sch_ElementRef";

		// Token: 0x04000128 RID: 296
		internal const string Sch_TypeMutualExclusive = "Sch_TypeMutualExclusive";

		// Token: 0x04000129 RID: 297
		internal const string Sch_ElementNameRef = "Sch_ElementNameRef";

		// Token: 0x0400012A RID: 298
		internal const string Sch_AttributeNameRef = "Sch_AttributeNameRef";

		// Token: 0x0400012B RID: 299
		internal const string Sch_TextNotAllowed = "Sch_TextNotAllowed";

		// Token: 0x0400012C RID: 300
		internal const string Sch_UndeclaredType = "Sch_UndeclaredType";

		// Token: 0x0400012D RID: 301
		internal const string Sch_UndeclaredSimpleType = "Sch_UndeclaredSimpleType";

		// Token: 0x0400012E RID: 302
		internal const string Sch_UndeclaredEquivClass = "Sch_UndeclaredEquivClass";

		// Token: 0x0400012F RID: 303
		internal const string Sch_AttListPresence = "Sch_AttListPresence";

		// Token: 0x04000130 RID: 304
		internal const string Sch_NotationValue = "Sch_NotationValue";

		// Token: 0x04000131 RID: 305
		internal const string Sch_EnumerationValue = "Sch_EnumerationValue";

		// Token: 0x04000132 RID: 306
		internal const string Sch_EmptyAttributeValue = "Sch_EmptyAttributeValue";

		// Token: 0x04000133 RID: 307
		internal const string Sch_InvalidLanguageId = "Sch_InvalidLanguageId";

		// Token: 0x04000134 RID: 308
		internal const string Sch_XmlSpace = "Sch_XmlSpace";

		// Token: 0x04000135 RID: 309
		internal const string Sch_InvalidXsdAttributeValue = "Sch_InvalidXsdAttributeValue";

		// Token: 0x04000136 RID: 310
		internal const string Sch_InvalidXsdAttributeDatatypeValue = "Sch_InvalidXsdAttributeDatatypeValue";

		// Token: 0x04000137 RID: 311
		internal const string Sch_ElementValueDataTypeDetailed = "Sch_ElementValueDataTypeDetailed";

		// Token: 0x04000138 RID: 312
		internal const string Sch_InvalidElementDefaultValue = "Sch_InvalidElementDefaultValue";

		// Token: 0x04000139 RID: 313
		internal const string Sch_NonDeterministic = "Sch_NonDeterministic";

		// Token: 0x0400013A RID: 314
		internal const string Sch_NonDeterministicAnyEx = "Sch_NonDeterministicAnyEx";

		// Token: 0x0400013B RID: 315
		internal const string Sch_NonDeterministicAnyAny = "Sch_NonDeterministicAnyAny";

		// Token: 0x0400013C RID: 316
		internal const string Sch_StandAlone = "Sch_StandAlone";

		// Token: 0x0400013D RID: 317
		internal const string Sch_XmlNsAttribute = "Sch_XmlNsAttribute";

		// Token: 0x0400013E RID: 318
		internal const string Sch_AllElement = "Sch_AllElement";

		// Token: 0x0400013F RID: 319
		internal const string Sch_MismatchTargetNamespaceInclude = "Sch_MismatchTargetNamespaceInclude";

		// Token: 0x04000140 RID: 320
		internal const string Sch_MismatchTargetNamespaceImport = "Sch_MismatchTargetNamespaceImport";

		// Token: 0x04000141 RID: 321
		internal const string Sch_MismatchTargetNamespaceEx = "Sch_MismatchTargetNamespaceEx";

		// Token: 0x04000142 RID: 322
		internal const string Sch_XsiTypeNotFound = "Sch_XsiTypeNotFound";

		// Token: 0x04000143 RID: 323
		internal const string Sch_XsiTypeAbstract = "Sch_XsiTypeAbstract";

		// Token: 0x04000144 RID: 324
		internal const string Sch_ListFromNonatomic = "Sch_ListFromNonatomic";

		// Token: 0x04000145 RID: 325
		internal const string Sch_UnionFromUnion = "Sch_UnionFromUnion";

		// Token: 0x04000146 RID: 326
		internal const string Sch_DupLengthFacet = "Sch_DupLengthFacet";

		// Token: 0x04000147 RID: 327
		internal const string Sch_DupMinLengthFacet = "Sch_DupMinLengthFacet";

		// Token: 0x04000148 RID: 328
		internal const string Sch_DupMaxLengthFacet = "Sch_DupMaxLengthFacet";

		// Token: 0x04000149 RID: 329
		internal const string Sch_DupWhiteSpaceFacet = "Sch_DupWhiteSpaceFacet";

		// Token: 0x0400014A RID: 330
		internal const string Sch_DupMaxInclusiveFacet = "Sch_DupMaxInclusiveFacet";

		// Token: 0x0400014B RID: 331
		internal const string Sch_DupMaxExclusiveFacet = "Sch_DupMaxExclusiveFacet";

		// Token: 0x0400014C RID: 332
		internal const string Sch_DupMinInclusiveFacet = "Sch_DupMinInclusiveFacet";

		// Token: 0x0400014D RID: 333
		internal const string Sch_DupMinExclusiveFacet = "Sch_DupMinExclusiveFacet";

		// Token: 0x0400014E RID: 334
		internal const string Sch_DupTotalDigitsFacet = "Sch_DupTotalDigitsFacet";

		// Token: 0x0400014F RID: 335
		internal const string Sch_DupFractionDigitsFacet = "Sch_DupFractionDigitsFacet";

		// Token: 0x04000150 RID: 336
		internal const string Sch_LengthFacetProhibited = "Sch_LengthFacetProhibited";

		// Token: 0x04000151 RID: 337
		internal const string Sch_MinLengthFacetProhibited = "Sch_MinLengthFacetProhibited";

		// Token: 0x04000152 RID: 338
		internal const string Sch_MaxLengthFacetProhibited = "Sch_MaxLengthFacetProhibited";

		// Token: 0x04000153 RID: 339
		internal const string Sch_PatternFacetProhibited = "Sch_PatternFacetProhibited";

		// Token: 0x04000154 RID: 340
		internal const string Sch_EnumerationFacetProhibited = "Sch_EnumerationFacetProhibited";

		// Token: 0x04000155 RID: 341
		internal const string Sch_WhiteSpaceFacetProhibited = "Sch_WhiteSpaceFacetProhibited";

		// Token: 0x04000156 RID: 342
		internal const string Sch_MaxInclusiveFacetProhibited = "Sch_MaxInclusiveFacetProhibited";

		// Token: 0x04000157 RID: 343
		internal const string Sch_MaxExclusiveFacetProhibited = "Sch_MaxExclusiveFacetProhibited";

		// Token: 0x04000158 RID: 344
		internal const string Sch_MinInclusiveFacetProhibited = "Sch_MinInclusiveFacetProhibited";

		// Token: 0x04000159 RID: 345
		internal const string Sch_MinExclusiveFacetProhibited = "Sch_MinExclusiveFacetProhibited";

		// Token: 0x0400015A RID: 346
		internal const string Sch_TotalDigitsFacetProhibited = "Sch_TotalDigitsFacetProhibited";

		// Token: 0x0400015B RID: 347
		internal const string Sch_FractionDigitsFacetProhibited = "Sch_FractionDigitsFacetProhibited";

		// Token: 0x0400015C RID: 348
		internal const string Sch_LengthFacetInvalid = "Sch_LengthFacetInvalid";

		// Token: 0x0400015D RID: 349
		internal const string Sch_MinLengthFacetInvalid = "Sch_MinLengthFacetInvalid";

		// Token: 0x0400015E RID: 350
		internal const string Sch_MaxLengthFacetInvalid = "Sch_MaxLengthFacetInvalid";

		// Token: 0x0400015F RID: 351
		internal const string Sch_MaxInclusiveFacetInvalid = "Sch_MaxInclusiveFacetInvalid";

		// Token: 0x04000160 RID: 352
		internal const string Sch_MaxExclusiveFacetInvalid = "Sch_MaxExclusiveFacetInvalid";

		// Token: 0x04000161 RID: 353
		internal const string Sch_MinInclusiveFacetInvalid = "Sch_MinInclusiveFacetInvalid";

		// Token: 0x04000162 RID: 354
		internal const string Sch_MinExclusiveFacetInvalid = "Sch_MinExclusiveFacetInvalid";

		// Token: 0x04000163 RID: 355
		internal const string Sch_TotalDigitsFacetInvalid = "Sch_TotalDigitsFacetInvalid";

		// Token: 0x04000164 RID: 356
		internal const string Sch_FractionDigitsFacetInvalid = "Sch_FractionDigitsFacetInvalid";

		// Token: 0x04000165 RID: 357
		internal const string Sch_PatternFacetInvalid = "Sch_PatternFacetInvalid";

		// Token: 0x04000166 RID: 358
		internal const string Sch_EnumerationFacetInvalid = "Sch_EnumerationFacetInvalid";

		// Token: 0x04000167 RID: 359
		internal const string Sch_InvalidWhiteSpace = "Sch_InvalidWhiteSpace";

		// Token: 0x04000168 RID: 360
		internal const string Sch_UnknownFacet = "Sch_UnknownFacet";

		// Token: 0x04000169 RID: 361
		internal const string Sch_LengthAndMinMax = "Sch_LengthAndMinMax";

		// Token: 0x0400016A RID: 362
		internal const string Sch_MinLengthGtMaxLength = "Sch_MinLengthGtMaxLength";

		// Token: 0x0400016B RID: 363
		internal const string Sch_FractionDigitsGtTotalDigits = "Sch_FractionDigitsGtTotalDigits";

		// Token: 0x0400016C RID: 364
		internal const string Sch_LengthConstraintFailed = "Sch_LengthConstraintFailed";

		// Token: 0x0400016D RID: 365
		internal const string Sch_MinLengthConstraintFailed = "Sch_MinLengthConstraintFailed";

		// Token: 0x0400016E RID: 366
		internal const string Sch_MaxLengthConstraintFailed = "Sch_MaxLengthConstraintFailed";

		// Token: 0x0400016F RID: 367
		internal const string Sch_PatternConstraintFailed = "Sch_PatternConstraintFailed";

		// Token: 0x04000170 RID: 368
		internal const string Sch_EnumerationConstraintFailed = "Sch_EnumerationConstraintFailed";

		// Token: 0x04000171 RID: 369
		internal const string Sch_MaxInclusiveConstraintFailed = "Sch_MaxInclusiveConstraintFailed";

		// Token: 0x04000172 RID: 370
		internal const string Sch_MaxExclusiveConstraintFailed = "Sch_MaxExclusiveConstraintFailed";

		// Token: 0x04000173 RID: 371
		internal const string Sch_MinInclusiveConstraintFailed = "Sch_MinInclusiveConstraintFailed";

		// Token: 0x04000174 RID: 372
		internal const string Sch_MinExclusiveConstraintFailed = "Sch_MinExclusiveConstraintFailed";

		// Token: 0x04000175 RID: 373
		internal const string Sch_TotalDigitsConstraintFailed = "Sch_TotalDigitsConstraintFailed";

		// Token: 0x04000176 RID: 374
		internal const string Sch_FractionDigitsConstraintFailed = "Sch_FractionDigitsConstraintFailed";

		// Token: 0x04000177 RID: 375
		internal const string Sch_UnionFailedEx = "Sch_UnionFailedEx";

		// Token: 0x04000178 RID: 376
		internal const string Sch_NotationRequired = "Sch_NotationRequired";

		// Token: 0x04000179 RID: 377
		internal const string Sch_DupNotationAttribute = "Sch_DupNotationAttribute";

		// Token: 0x0400017A RID: 378
		internal const string Sch_MissingPublicSystemAttribute = "Sch_MissingPublicSystemAttribute";

		// Token: 0x0400017B RID: 379
		internal const string Sch_NotationAttributeOnEmptyElement = "Sch_NotationAttributeOnEmptyElement";

		// Token: 0x0400017C RID: 380
		internal const string Sch_RefNotInScope = "Sch_RefNotInScope";

		// Token: 0x0400017D RID: 381
		internal const string Sch_UndeclaredIdentityConstraint = "Sch_UndeclaredIdentityConstraint";

		// Token: 0x0400017E RID: 382
		internal const string Sch_RefInvalidIdentityConstraint = "Sch_RefInvalidIdentityConstraint";

		// Token: 0x0400017F RID: 383
		internal const string Sch_RefInvalidCardin = "Sch_RefInvalidCardin";

		// Token: 0x04000180 RID: 384
		internal const string Sch_ReftoKeyref = "Sch_ReftoKeyref";

		// Token: 0x04000181 RID: 385
		internal const string Sch_EmptyXPath = "Sch_EmptyXPath";

		// Token: 0x04000182 RID: 386
		internal const string Sch_UnresolvedPrefix = "Sch_UnresolvedPrefix";

		// Token: 0x04000183 RID: 387
		internal const string Sch_UnresolvedKeyref = "Sch_UnresolvedKeyref";

		// Token: 0x04000184 RID: 388
		internal const string Sch_ICXpathError = "Sch_ICXpathError";

		// Token: 0x04000185 RID: 389
		internal const string Sch_SelectorAttr = "Sch_SelectorAttr";

		// Token: 0x04000186 RID: 390
		internal const string Sch_FieldSimpleTypeExpected = "Sch_FieldSimpleTypeExpected";

		// Token: 0x04000187 RID: 391
		internal const string Sch_FieldSingleValueExpected = "Sch_FieldSingleValueExpected";

		// Token: 0x04000188 RID: 392
		internal const string Sch_MissingKey = "Sch_MissingKey";

		// Token: 0x04000189 RID: 393
		internal const string Sch_DuplicateKey = "Sch_DuplicateKey";

		// Token: 0x0400018A RID: 394
		internal const string Sch_TargetNamespaceXsi = "Sch_TargetNamespaceXsi";

		// Token: 0x0400018B RID: 395
		internal const string Sch_UndeclaredEntity = "Sch_UndeclaredEntity";

		// Token: 0x0400018C RID: 396
		internal const string Sch_UnparsedEntityRef = "Sch_UnparsedEntityRef";

		// Token: 0x0400018D RID: 397
		internal const string Sch_MaxOccursInvalidXsd = "Sch_MaxOccursInvalidXsd";

		// Token: 0x0400018E RID: 398
		internal const string Sch_MinOccursInvalidXsd = "Sch_MinOccursInvalidXsd";

		// Token: 0x0400018F RID: 399
		internal const string Sch_MaxInclusiveExclusive = "Sch_MaxInclusiveExclusive";

		// Token: 0x04000190 RID: 400
		internal const string Sch_MinInclusiveExclusive = "Sch_MinInclusiveExclusive";

		// Token: 0x04000191 RID: 401
		internal const string Sch_MinInclusiveGtMaxInclusive = "Sch_MinInclusiveGtMaxInclusive";

		// Token: 0x04000192 RID: 402
		internal const string Sch_MinExclusiveGtMaxExclusive = "Sch_MinExclusiveGtMaxExclusive";

		// Token: 0x04000193 RID: 403
		internal const string Sch_MinInclusiveGtMaxExclusive = "Sch_MinInclusiveGtMaxExclusive";

		// Token: 0x04000194 RID: 404
		internal const string Sch_MinExclusiveGtMaxInclusive = "Sch_MinExclusiveGtMaxInclusive";

		// Token: 0x04000195 RID: 405
		internal const string Sch_SimpleTypeRestriction = "Sch_SimpleTypeRestriction";

		// Token: 0x04000196 RID: 406
		internal const string Sch_InvalidFacetPosition = "Sch_InvalidFacetPosition";

		// Token: 0x04000197 RID: 407
		internal const string Sch_AttributeMutuallyExclusive = "Sch_AttributeMutuallyExclusive";

		// Token: 0x04000198 RID: 408
		internal const string Sch_AnyAttributeLastChild = "Sch_AnyAttributeLastChild";

		// Token: 0x04000199 RID: 409
		internal const string Sch_ComplexTypeContentModel = "Sch_ComplexTypeContentModel";

		// Token: 0x0400019A RID: 410
		internal const string Sch_ComplexContentContentModel = "Sch_ComplexContentContentModel";

		// Token: 0x0400019B RID: 411
		internal const string Sch_NotNormalizedString = "Sch_NotNormalizedString";

		// Token: 0x0400019C RID: 412
		internal const string Sch_NotTokenString = "Sch_NotTokenString";

		// Token: 0x0400019D RID: 413
		internal const string Sch_FractionDigitsNotOnDecimal = "Sch_FractionDigitsNotOnDecimal";

		// Token: 0x0400019E RID: 414
		internal const string Sch_ContentInNill = "Sch_ContentInNill";

		// Token: 0x0400019F RID: 415
		internal const string Sch_NoElementSchemaFound = "Sch_NoElementSchemaFound";

		// Token: 0x040001A0 RID: 416
		internal const string Sch_NoAttributeSchemaFound = "Sch_NoAttributeSchemaFound";

		// Token: 0x040001A1 RID: 417
		internal const string Sch_InvalidNamespace = "Sch_InvalidNamespace";

		// Token: 0x040001A2 RID: 418
		internal const string Sch_InvalidTargetNamespaceAttribute = "Sch_InvalidTargetNamespaceAttribute";

		// Token: 0x040001A3 RID: 419
		internal const string Sch_InvalidNamespaceAttribute = "Sch_InvalidNamespaceAttribute";

		// Token: 0x040001A4 RID: 420
		internal const string Sch_InvalidSchemaLocation = "Sch_InvalidSchemaLocation";

		// Token: 0x040001A5 RID: 421
		internal const string Sch_ImportTargetNamespace = "Sch_ImportTargetNamespace";

		// Token: 0x040001A6 RID: 422
		internal const string Sch_ImportTargetNamespaceNull = "Sch_ImportTargetNamespaceNull";

		// Token: 0x040001A7 RID: 423
		internal const string Sch_GroupDoubleRedefine = "Sch_GroupDoubleRedefine";

		// Token: 0x040001A8 RID: 424
		internal const string Sch_ComponentRedefineNotFound = "Sch_ComponentRedefineNotFound";

		// Token: 0x040001A9 RID: 425
		internal const string Sch_GroupRedefineNotFound = "Sch_GroupRedefineNotFound";

		// Token: 0x040001AA RID: 426
		internal const string Sch_AttrGroupDoubleRedefine = "Sch_AttrGroupDoubleRedefine";

		// Token: 0x040001AB RID: 427
		internal const string Sch_AttrGroupRedefineNotFound = "Sch_AttrGroupRedefineNotFound";

		// Token: 0x040001AC RID: 428
		internal const string Sch_ComplexTypeDoubleRedefine = "Sch_ComplexTypeDoubleRedefine";

		// Token: 0x040001AD RID: 429
		internal const string Sch_ComplexTypeRedefineNotFound = "Sch_ComplexTypeRedefineNotFound";

		// Token: 0x040001AE RID: 430
		internal const string Sch_SimpleToComplexTypeRedefine = "Sch_SimpleToComplexTypeRedefine";

		// Token: 0x040001AF RID: 431
		internal const string Sch_SimpleTypeDoubleRedefine = "Sch_SimpleTypeDoubleRedefine";

		// Token: 0x040001B0 RID: 432
		internal const string Sch_ComplexToSimpleTypeRedefine = "Sch_ComplexToSimpleTypeRedefine";

		// Token: 0x040001B1 RID: 433
		internal const string Sch_SimpleTypeRedefineNotFound = "Sch_SimpleTypeRedefineNotFound";

		// Token: 0x040001B2 RID: 434
		internal const string Sch_MinMaxGroupRedefine = "Sch_MinMaxGroupRedefine";

		// Token: 0x040001B3 RID: 435
		internal const string Sch_MultipleGroupSelfRef = "Sch_MultipleGroupSelfRef";

		// Token: 0x040001B4 RID: 436
		internal const string Sch_MultipleAttrGroupSelfRef = "Sch_MultipleAttrGroupSelfRef";

		// Token: 0x040001B5 RID: 437
		internal const string Sch_InvalidTypeRedefine = "Sch_InvalidTypeRedefine";

		// Token: 0x040001B6 RID: 438
		internal const string Sch_InvalidElementRef = "Sch_InvalidElementRef";

		// Token: 0x040001B7 RID: 439
		internal const string Sch_MinGtMax = "Sch_MinGtMax";

		// Token: 0x040001B8 RID: 440
		internal const string Sch_DupSelector = "Sch_DupSelector";

		// Token: 0x040001B9 RID: 441
		internal const string Sch_IdConstraintNoSelector = "Sch_IdConstraintNoSelector";

		// Token: 0x040001BA RID: 442
		internal const string Sch_IdConstraintNoFields = "Sch_IdConstraintNoFields";

		// Token: 0x040001BB RID: 443
		internal const string Sch_IdConstraintNoRefer = "Sch_IdConstraintNoRefer";

		// Token: 0x040001BC RID: 444
		internal const string Sch_SelectorBeforeFields = "Sch_SelectorBeforeFields";

		// Token: 0x040001BD RID: 445
		internal const string Sch_NoSimpleTypeContent = "Sch_NoSimpleTypeContent";

		// Token: 0x040001BE RID: 446
		internal const string Sch_SimpleTypeRestRefBase = "Sch_SimpleTypeRestRefBase";

		// Token: 0x040001BF RID: 447
		internal const string Sch_SimpleTypeRestRefBaseNone = "Sch_SimpleTypeRestRefBaseNone";

		// Token: 0x040001C0 RID: 448
		internal const string Sch_SimpleTypeListRefBase = "Sch_SimpleTypeListRefBase";

		// Token: 0x040001C1 RID: 449
		internal const string Sch_SimpleTypeListRefBaseNone = "Sch_SimpleTypeListRefBaseNone";

		// Token: 0x040001C2 RID: 450
		internal const string Sch_SimpleTypeUnionNoBase = "Sch_SimpleTypeUnionNoBase";

		// Token: 0x040001C3 RID: 451
		internal const string Sch_NoRestOrExtQName = "Sch_NoRestOrExtQName";

		// Token: 0x040001C4 RID: 452
		internal const string Sch_NoRestOrExt = "Sch_NoRestOrExt";

		// Token: 0x040001C5 RID: 453
		internal const string Sch_NoGroupParticle = "Sch_NoGroupParticle";

		// Token: 0x040001C6 RID: 454
		internal const string Sch_InvalidAllMin = "Sch_InvalidAllMin";

		// Token: 0x040001C7 RID: 455
		internal const string Sch_InvalidAllMax = "Sch_InvalidAllMax";

		// Token: 0x040001C8 RID: 456
		internal const string Sch_InvalidFacet = "Sch_InvalidFacet";

		// Token: 0x040001C9 RID: 457
		internal const string Sch_AbstractElement = "Sch_AbstractElement";

		// Token: 0x040001CA RID: 458
		internal const string Sch_XsiTypeBlockedEx = "Sch_XsiTypeBlockedEx";

		// Token: 0x040001CB RID: 459
		internal const string Sch_InvalidXsiNill = "Sch_InvalidXsiNill";

		// Token: 0x040001CC RID: 460
		internal const string Sch_SubstitutionNotAllowed = "Sch_SubstitutionNotAllowed";

		// Token: 0x040001CD RID: 461
		internal const string Sch_SubstitutionBlocked = "Sch_SubstitutionBlocked";

		// Token: 0x040001CE RID: 462
		internal const string Sch_InvalidElementInEmptyEx = "Sch_InvalidElementInEmptyEx";

		// Token: 0x040001CF RID: 463
		internal const string Sch_InvalidElementInTextOnlyEx = "Sch_InvalidElementInTextOnlyEx";

		// Token: 0x040001D0 RID: 464
		internal const string Sch_InvalidTextInElement = "Sch_InvalidTextInElement";

		// Token: 0x040001D1 RID: 465
		internal const string Sch_InvalidElementContent = "Sch_InvalidElementContent";

		// Token: 0x040001D2 RID: 466
		internal const string Sch_InvalidElementContentComplex = "Sch_InvalidElementContentComplex";

		// Token: 0x040001D3 RID: 467
		internal const string Sch_IncompleteContent = "Sch_IncompleteContent";

		// Token: 0x040001D4 RID: 468
		internal const string Sch_IncompleteContentComplex = "Sch_IncompleteContentComplex";

		// Token: 0x040001D5 RID: 469
		internal const string Sch_InvalidTextInElementExpecting = "Sch_InvalidTextInElementExpecting";

		// Token: 0x040001D6 RID: 470
		internal const string Sch_InvalidElementContentExpecting = "Sch_InvalidElementContentExpecting";

		// Token: 0x040001D7 RID: 471
		internal const string Sch_InvalidElementContentExpectingComplex = "Sch_InvalidElementContentExpectingComplex";

		// Token: 0x040001D8 RID: 472
		internal const string Sch_IncompleteContentExpecting = "Sch_IncompleteContentExpecting";

		// Token: 0x040001D9 RID: 473
		internal const string Sch_IncompleteContentExpectingComplex = "Sch_IncompleteContentExpectingComplex";

		// Token: 0x040001DA RID: 474
		internal const string Sch_InvalidElementSubstitution = "Sch_InvalidElementSubstitution";

		// Token: 0x040001DB RID: 475
		internal const string Sch_ElementNameAndNamespace = "Sch_ElementNameAndNamespace";

		// Token: 0x040001DC RID: 476
		internal const string Sch_ElementName = "Sch_ElementName";

		// Token: 0x040001DD RID: 477
		internal const string Sch_ContinuationString = "Sch_ContinuationString";

		// Token: 0x040001DE RID: 478
		internal const string Sch_AnyElementNS = "Sch_AnyElementNS";

		// Token: 0x040001DF RID: 479
		internal const string Sch_AnyElement = "Sch_AnyElement";

		// Token: 0x040001E0 RID: 480
		internal const string Sch_InvalidTextInEmpty = "Sch_InvalidTextInEmpty";

		// Token: 0x040001E1 RID: 481
		internal const string Sch_InvalidWhitespaceInEmpty = "Sch_InvalidWhitespaceInEmpty";

		// Token: 0x040001E2 RID: 482
		internal const string Sch_InvalidPIComment = "Sch_InvalidPIComment";

		// Token: 0x040001E3 RID: 483
		internal const string Sch_InvalidAttributeRef = "Sch_InvalidAttributeRef";

		// Token: 0x040001E4 RID: 484
		internal const string Sch_OptionalDefaultAttribute = "Sch_OptionalDefaultAttribute";

		// Token: 0x040001E5 RID: 485
		internal const string Sch_AttributeCircularRef = "Sch_AttributeCircularRef";

		// Token: 0x040001E6 RID: 486
		internal const string Sch_IdentityConstraintCircularRef = "Sch_IdentityConstraintCircularRef";

		// Token: 0x040001E7 RID: 487
		internal const string Sch_SubstitutionCircularRef = "Sch_SubstitutionCircularRef";

		// Token: 0x040001E8 RID: 488
		internal const string Sch_InvalidAnyAttribute = "Sch_InvalidAnyAttribute";

		// Token: 0x040001E9 RID: 489
		internal const string Sch_DupIdAttribute = "Sch_DupIdAttribute";

		// Token: 0x040001EA RID: 490
		internal const string Sch_InvalidAllElementMax = "Sch_InvalidAllElementMax";

		// Token: 0x040001EB RID: 491
		internal const string Sch_InvalidAny = "Sch_InvalidAny";

		// Token: 0x040001EC RID: 492
		internal const string Sch_InvalidAnyDetailed = "Sch_InvalidAnyDetailed";

		// Token: 0x040001ED RID: 493
		internal const string Sch_InvalidExamplar = "Sch_InvalidExamplar";

		// Token: 0x040001EE RID: 494
		internal const string Sch_NoExamplar = "Sch_NoExamplar";

		// Token: 0x040001EF RID: 495
		internal const string Sch_InvalidSubstitutionMember = "Sch_InvalidSubstitutionMember";

		// Token: 0x040001F0 RID: 496
		internal const string Sch_RedefineNoSchema = "Sch_RedefineNoSchema";

		// Token: 0x040001F1 RID: 497
		internal const string Sch_ProhibitedAttribute = "Sch_ProhibitedAttribute";

		// Token: 0x040001F2 RID: 498
		internal const string Sch_TypeCircularRef = "Sch_TypeCircularRef";

		// Token: 0x040001F3 RID: 499
		internal const string Sch_TwoIdAttrUses = "Sch_TwoIdAttrUses";

		// Token: 0x040001F4 RID: 500
		internal const string Sch_AttrUseAndWildId = "Sch_AttrUseAndWildId";

		// Token: 0x040001F5 RID: 501
		internal const string Sch_MoreThanOneWildId = "Sch_MoreThanOneWildId";

		// Token: 0x040001F6 RID: 502
		internal const string Sch_BaseFinalExtension = "Sch_BaseFinalExtension";

		// Token: 0x040001F7 RID: 503
		internal const string Sch_NotSimpleContent = "Sch_NotSimpleContent";

		// Token: 0x040001F8 RID: 504
		internal const string Sch_NotComplexContent = "Sch_NotComplexContent";

		// Token: 0x040001F9 RID: 505
		internal const string Sch_BaseFinalRestriction = "Sch_BaseFinalRestriction";

		// Token: 0x040001FA RID: 506
		internal const string Sch_BaseFinalList = "Sch_BaseFinalList";

		// Token: 0x040001FB RID: 507
		internal const string Sch_BaseFinalUnion = "Sch_BaseFinalUnion";

		// Token: 0x040001FC RID: 508
		internal const string Sch_UndefBaseRestriction = "Sch_UndefBaseRestriction";

		// Token: 0x040001FD RID: 509
		internal const string Sch_UndefBaseExtension = "Sch_UndefBaseExtension";

		// Token: 0x040001FE RID: 510
		internal const string Sch_DifContentType = "Sch_DifContentType";

		// Token: 0x040001FF RID: 511
		internal const string Sch_InvalidContentRestriction = "Sch_InvalidContentRestriction";

		// Token: 0x04000200 RID: 512
		internal const string Sch_InvalidContentRestrictionDetailed = "Sch_InvalidContentRestrictionDetailed";

		// Token: 0x04000201 RID: 513
		internal const string Sch_InvalidBaseToEmpty = "Sch_InvalidBaseToEmpty";

		// Token: 0x04000202 RID: 514
		internal const string Sch_InvalidBaseToMixed = "Sch_InvalidBaseToMixed";

		// Token: 0x04000203 RID: 515
		internal const string Sch_DupAttributeUse = "Sch_DupAttributeUse";

		// Token: 0x04000204 RID: 516
		internal const string Sch_InvalidParticleRestriction = "Sch_InvalidParticleRestriction";

		// Token: 0x04000205 RID: 517
		internal const string Sch_InvalidParticleRestrictionDetailed = "Sch_InvalidParticleRestrictionDetailed";

		// Token: 0x04000206 RID: 518
		internal const string Sch_ForbiddenDerivedParticleForAll = "Sch_ForbiddenDerivedParticleForAll";

		// Token: 0x04000207 RID: 519
		internal const string Sch_ForbiddenDerivedParticleForElem = "Sch_ForbiddenDerivedParticleForElem";

		// Token: 0x04000208 RID: 520
		internal const string Sch_ForbiddenDerivedParticleForChoice = "Sch_ForbiddenDerivedParticleForChoice";

		// Token: 0x04000209 RID: 521
		internal const string Sch_ForbiddenDerivedParticleForSeq = "Sch_ForbiddenDerivedParticleForSeq";

		// Token: 0x0400020A RID: 522
		internal const string Sch_ElementFromElement = "Sch_ElementFromElement";

		// Token: 0x0400020B RID: 523
		internal const string Sch_ElementFromAnyRule1 = "Sch_ElementFromAnyRule1";

		// Token: 0x0400020C RID: 524
		internal const string Sch_ElementFromAnyRule2 = "Sch_ElementFromAnyRule2";

		// Token: 0x0400020D RID: 525
		internal const string Sch_AnyFromAnyRule1 = "Sch_AnyFromAnyRule1";

		// Token: 0x0400020E RID: 526
		internal const string Sch_AnyFromAnyRule2 = "Sch_AnyFromAnyRule2";

		// Token: 0x0400020F RID: 527
		internal const string Sch_AnyFromAnyRule3 = "Sch_AnyFromAnyRule3";

		// Token: 0x04000210 RID: 528
		internal const string Sch_GroupBaseFromAny1 = "Sch_GroupBaseFromAny1";

		// Token: 0x04000211 RID: 529
		internal const string Sch_GroupBaseFromAny2 = "Sch_GroupBaseFromAny2";

		// Token: 0x04000212 RID: 530
		internal const string Sch_ElementFromGroupBase1 = "Sch_ElementFromGroupBase1";

		// Token: 0x04000213 RID: 531
		internal const string Sch_ElementFromGroupBase2 = "Sch_ElementFromGroupBase2";

		// Token: 0x04000214 RID: 532
		internal const string Sch_ElementFromGroupBase3 = "Sch_ElementFromGroupBase3";

		// Token: 0x04000215 RID: 533
		internal const string Sch_GroupBaseRestRangeInvalid = "Sch_GroupBaseRestRangeInvalid";

		// Token: 0x04000216 RID: 534
		internal const string Sch_GroupBaseRestNoMap = "Sch_GroupBaseRestNoMap";

		// Token: 0x04000217 RID: 535
		internal const string Sch_GroupBaseRestNotEmptiable = "Sch_GroupBaseRestNotEmptiable";

		// Token: 0x04000218 RID: 536
		internal const string Sch_SeqFromAll = "Sch_SeqFromAll";

		// Token: 0x04000219 RID: 537
		internal const string Sch_SeqFromChoice = "Sch_SeqFromChoice";

		// Token: 0x0400021A RID: 538
		internal const string Sch_UndefGroupRef = "Sch_UndefGroupRef";

		// Token: 0x0400021B RID: 539
		internal const string Sch_GroupCircularRef = "Sch_GroupCircularRef";

		// Token: 0x0400021C RID: 540
		internal const string Sch_AllRefNotRoot = "Sch_AllRefNotRoot";

		// Token: 0x0400021D RID: 541
		internal const string Sch_AllRefMinMax = "Sch_AllRefMinMax";

		// Token: 0x0400021E RID: 542
		internal const string Sch_NotAllAlone = "Sch_NotAllAlone";

		// Token: 0x0400021F RID: 543
		internal const string Sch_AttributeGroupCircularRef = "Sch_AttributeGroupCircularRef";

		// Token: 0x04000220 RID: 544
		internal const string Sch_UndefAttributeGroupRef = "Sch_UndefAttributeGroupRef";

		// Token: 0x04000221 RID: 545
		internal const string Sch_InvalidAttributeExtension = "Sch_InvalidAttributeExtension";

		// Token: 0x04000222 RID: 546
		internal const string Sch_InvalidAnyAttributeRestriction = "Sch_InvalidAnyAttributeRestriction";

		// Token: 0x04000223 RID: 547
		internal const string Sch_AttributeRestrictionProhibited = "Sch_AttributeRestrictionProhibited";

		// Token: 0x04000224 RID: 548
		internal const string Sch_AttributeRestrictionInvalid = "Sch_AttributeRestrictionInvalid";

		// Token: 0x04000225 RID: 549
		internal const string Sch_AttributeFixedInvalid = "Sch_AttributeFixedInvalid";

		// Token: 0x04000226 RID: 550
		internal const string Sch_AttributeUseInvalid = "Sch_AttributeUseInvalid";

		// Token: 0x04000227 RID: 551
		internal const string Sch_AttributeRestrictionInvalidFromWildcard = "Sch_AttributeRestrictionInvalidFromWildcard";

		// Token: 0x04000228 RID: 552
		internal const string Sch_NoDerivedAttribute = "Sch_NoDerivedAttribute";

		// Token: 0x04000229 RID: 553
		internal const string Sch_UnexpressibleAnyAttribute = "Sch_UnexpressibleAnyAttribute";

		// Token: 0x0400022A RID: 554
		internal const string Sch_RefInvalidAttribute = "Sch_RefInvalidAttribute";

		// Token: 0x0400022B RID: 555
		internal const string Sch_ElementCircularRef = "Sch_ElementCircularRef";

		// Token: 0x0400022C RID: 556
		internal const string Sch_RefInvalidElement = "Sch_RefInvalidElement";

		// Token: 0x0400022D RID: 557
		internal const string Sch_ElementCannotHaveValue = "Sch_ElementCannotHaveValue";

		// Token: 0x0400022E RID: 558
		internal const string Sch_ElementInMixedWithFixed = "Sch_ElementInMixedWithFixed";

		// Token: 0x0400022F RID: 559
		internal const string Sch_ElementTypeCollision = "Sch_ElementTypeCollision";

		// Token: 0x04000230 RID: 560
		internal const string Sch_InvalidIncludeLocation = "Sch_InvalidIncludeLocation";

		// Token: 0x04000231 RID: 561
		internal const string Sch_CannotLoadSchema = "Sch_CannotLoadSchema";

		// Token: 0x04000232 RID: 562
		internal const string Sch_CannotLoadSchemaLocation = "Sch_CannotLoadSchemaLocation";

		// Token: 0x04000233 RID: 563
		internal const string Sch_LengthGtBaseLength = "Sch_LengthGtBaseLength";

		// Token: 0x04000234 RID: 564
		internal const string Sch_MinLengthGtBaseMinLength = "Sch_MinLengthGtBaseMinLength";

		// Token: 0x04000235 RID: 565
		internal const string Sch_MaxLengthGtBaseMaxLength = "Sch_MaxLengthGtBaseMaxLength";

		// Token: 0x04000236 RID: 566
		internal const string Sch_MaxMinLengthBaseLength = "Sch_MaxMinLengthBaseLength";

		// Token: 0x04000237 RID: 567
		internal const string Sch_MaxInclusiveMismatch = "Sch_MaxInclusiveMismatch";

		// Token: 0x04000238 RID: 568
		internal const string Sch_MaxExclusiveMismatch = "Sch_MaxExclusiveMismatch";

		// Token: 0x04000239 RID: 569
		internal const string Sch_MinInclusiveMismatch = "Sch_MinInclusiveMismatch";

		// Token: 0x0400023A RID: 570
		internal const string Sch_MinExclusiveMismatch = "Sch_MinExclusiveMismatch";

		// Token: 0x0400023B RID: 571
		internal const string Sch_MinExlIncMismatch = "Sch_MinExlIncMismatch";

		// Token: 0x0400023C RID: 572
		internal const string Sch_MinExlMaxExlMismatch = "Sch_MinExlMaxExlMismatch";

		// Token: 0x0400023D RID: 573
		internal const string Sch_MinIncMaxExlMismatch = "Sch_MinIncMaxExlMismatch";

		// Token: 0x0400023E RID: 574
		internal const string Sch_MinIncExlMismatch = "Sch_MinIncExlMismatch";

		// Token: 0x0400023F RID: 575
		internal const string Sch_MaxIncExlMismatch = "Sch_MaxIncExlMismatch";

		// Token: 0x04000240 RID: 576
		internal const string Sch_MaxExlIncMismatch = "Sch_MaxExlIncMismatch";

		// Token: 0x04000241 RID: 577
		internal const string Sch_TotalDigitsMismatch = "Sch_TotalDigitsMismatch";

		// Token: 0x04000242 RID: 578
		internal const string Sch_FacetBaseFixed = "Sch_FacetBaseFixed";

		// Token: 0x04000243 RID: 579
		internal const string Sch_WhiteSpaceRestriction1 = "Sch_WhiteSpaceRestriction1";

		// Token: 0x04000244 RID: 580
		internal const string Sch_WhiteSpaceRestriction2 = "Sch_WhiteSpaceRestriction2";

		// Token: 0x04000245 RID: 581
		internal const string Sch_UnSpecifiedDefaultAttributeInExternalStandalone = "Sch_UnSpecifiedDefaultAttributeInExternalStandalone";

		// Token: 0x04000246 RID: 582
		internal const string Sch_StandAloneNormalization = "Sch_StandAloneNormalization";

		// Token: 0x04000247 RID: 583
		internal const string Sch_XsiNilAndFixed = "Sch_XsiNilAndFixed";

		// Token: 0x04000248 RID: 584
		internal const string Sch_MixSchemaTypes = "Sch_MixSchemaTypes";

		// Token: 0x04000249 RID: 585
		internal const string Sch_XSDSchemaOnly = "Sch_XSDSchemaOnly";

		// Token: 0x0400024A RID: 586
		internal const string Sch_InvalidPublicAttribute = "Sch_InvalidPublicAttribute";

		// Token: 0x0400024B RID: 587
		internal const string Sch_InvalidSystemAttribute = "Sch_InvalidSystemAttribute";

		// Token: 0x0400024C RID: 588
		internal const string Sch_TypeAfterConstraints = "Sch_TypeAfterConstraints";

		// Token: 0x0400024D RID: 589
		internal const string Sch_XsiNilAndType = "Sch_XsiNilAndType";

		// Token: 0x0400024E RID: 590
		internal const string Sch_DupSimpleTypeChild = "Sch_DupSimpleTypeChild";

		// Token: 0x0400024F RID: 591
		internal const string Sch_InvalidIdAttribute = "Sch_InvalidIdAttribute";

		// Token: 0x04000250 RID: 592
		internal const string Sch_InvalidNameAttributeEx = "Sch_InvalidNameAttributeEx";

		// Token: 0x04000251 RID: 593
		internal const string Sch_InvalidAttribute = "Sch_InvalidAttribute";

		// Token: 0x04000252 RID: 594
		internal const string Sch_EmptyChoice = "Sch_EmptyChoice";

		// Token: 0x04000253 RID: 595
		internal const string Sch_DerivedNotFromBase = "Sch_DerivedNotFromBase";

		// Token: 0x04000254 RID: 596
		internal const string Sch_NeedSimpleTypeChild = "Sch_NeedSimpleTypeChild";

		// Token: 0x04000255 RID: 597
		internal const string Sch_InvalidCollection = "Sch_InvalidCollection";

		// Token: 0x04000256 RID: 598
		internal const string Sch_UnrefNS = "Sch_UnrefNS";

		// Token: 0x04000257 RID: 599
		internal const string Sch_InvalidSimpleTypeRestriction = "Sch_InvalidSimpleTypeRestriction";

		// Token: 0x04000258 RID: 600
		internal const string Sch_MultipleRedefine = "Sch_MultipleRedefine";

		// Token: 0x04000259 RID: 601
		internal const string Sch_NullValue = "Sch_NullValue";

		// Token: 0x0400025A RID: 602
		internal const string Sch_ComplexContentModel = "Sch_ComplexContentModel";

		// Token: 0x0400025B RID: 603
		internal const string Sch_SchemaNotPreprocessed = "Sch_SchemaNotPreprocessed";

		// Token: 0x0400025C RID: 604
		internal const string Sch_SchemaNotRemoved = "Sch_SchemaNotRemoved";

		// Token: 0x0400025D RID: 605
		internal const string Sch_ComponentAlreadySeenForNS = "Sch_ComponentAlreadySeenForNS";

		// Token: 0x0400025E RID: 606
		internal const string Sch_DefaultAttributeNotApplied = "Sch_DefaultAttributeNotApplied";

		// Token: 0x0400025F RID: 607
		internal const string Sch_NotXsiAttribute = "Sch_NotXsiAttribute";

		// Token: 0x04000260 RID: 608
		internal const string Sch_XsdDateTimeCompare = "Sch_XsdDateTimeCompare";

		// Token: 0x04000261 RID: 609
		internal const string Sch_InvalidNullCast = "Sch_InvalidNullCast";

		// Token: 0x04000262 RID: 610
		internal const string Sch_SchemaDoesNotExist = "Sch_SchemaDoesNotExist";

		// Token: 0x04000263 RID: 611
		internal const string Sch_InvalidDateTimeOption = "Sch_InvalidDateTimeOption";

		// Token: 0x04000264 RID: 612
		internal const string Sch_InvalidStartTransition = "Sch_InvalidStartTransition";

		// Token: 0x04000265 RID: 613
		internal const string Sch_InvalidStateTransition = "Sch_InvalidStateTransition";

		// Token: 0x04000266 RID: 614
		internal const string Sch_InvalidEndValidation = "Sch_InvalidEndValidation";

		// Token: 0x04000267 RID: 615
		internal const string Sch_InvalidEndElementCall = "Sch_InvalidEndElementCall";

		// Token: 0x04000268 RID: 616
		internal const string Sch_InvalidEndElementCallTyped = "Sch_InvalidEndElementCallTyped";

		// Token: 0x04000269 RID: 617
		internal const string Sch_InvalidEndElementMultiple = "Sch_InvalidEndElementMultiple";

		// Token: 0x0400026A RID: 618
		internal const string Sch_DuplicateAttribute = "Sch_DuplicateAttribute";

		// Token: 0x0400026B RID: 619
		internal const string Sch_InvalidPartialValidationType = "Sch_InvalidPartialValidationType";

		// Token: 0x0400026C RID: 620
		internal const string Sch_SchemaElementNameMismatch = "Sch_SchemaElementNameMismatch";

		// Token: 0x0400026D RID: 621
		internal const string Sch_SchemaAttributeNameMismatch = "Sch_SchemaAttributeNameMismatch";

		// Token: 0x0400026E RID: 622
		internal const string Sch_ValidateAttributeInvalidCall = "Sch_ValidateAttributeInvalidCall";

		// Token: 0x0400026F RID: 623
		internal const string Sch_ValidateElementInvalidCall = "Sch_ValidateElementInvalidCall";

		// Token: 0x04000270 RID: 624
		internal const string Sch_EnumNotStarted = "Sch_EnumNotStarted";

		// Token: 0x04000271 RID: 625
		internal const string Sch_EnumFinished = "Sch_EnumFinished";

		// Token: 0x04000272 RID: 626
		internal const string SchInf_schema = "SchInf_schema";

		// Token: 0x04000273 RID: 627
		internal const string SchInf_entity = "SchInf_entity";

		// Token: 0x04000274 RID: 628
		internal const string SchInf_simplecontent = "SchInf_simplecontent";

		// Token: 0x04000275 RID: 629
		internal const string SchInf_extension = "SchInf_extension";

		// Token: 0x04000276 RID: 630
		internal const string SchInf_particle = "SchInf_particle";

		// Token: 0x04000277 RID: 631
		internal const string SchInf_ct = "SchInf_ct";

		// Token: 0x04000278 RID: 632
		internal const string SchInf_seq = "SchInf_seq";

		// Token: 0x04000279 RID: 633
		internal const string SchInf_noseq = "SchInf_noseq";

		// Token: 0x0400027A RID: 634
		internal const string SchInf_noct = "SchInf_noct";

		// Token: 0x0400027B RID: 635
		internal const string SchInf_UnknownParticle = "SchInf_UnknownParticle";

		// Token: 0x0400027C RID: 636
		internal const string SchInf_schematype = "SchInf_schematype";

		// Token: 0x0400027D RID: 637
		internal const string SchInf_NoElement = "SchInf_NoElement";

		// Token: 0x0400027E RID: 638
		internal const string Xp_UnclosedString = "Xp_UnclosedString";

		// Token: 0x0400027F RID: 639
		internal const string Xp_ExprExpected = "Xp_ExprExpected";

		// Token: 0x04000280 RID: 640
		internal const string Xp_InvalidArgumentType = "Xp_InvalidArgumentType";

		// Token: 0x04000281 RID: 641
		internal const string Xp_InvalidNumArgs = "Xp_InvalidNumArgs";

		// Token: 0x04000282 RID: 642
		internal const string Xp_InvalidName = "Xp_InvalidName";

		// Token: 0x04000283 RID: 643
		internal const string Xp_InvalidToken = "Xp_InvalidToken";

		// Token: 0x04000284 RID: 644
		internal const string Xp_NodeSetExpected = "Xp_NodeSetExpected";

		// Token: 0x04000285 RID: 645
		internal const string Xp_NotSupported = "Xp_NotSupported";

		// Token: 0x04000286 RID: 646
		internal const string Xp_InvalidPattern = "Xp_InvalidPattern";

		// Token: 0x04000287 RID: 647
		internal const string Xp_InvalidKeyPattern = "Xp_InvalidKeyPattern";

		// Token: 0x04000288 RID: 648
		internal const string Xp_BadQueryObject = "Xp_BadQueryObject";

		// Token: 0x04000289 RID: 649
		internal const string Xp_UndefinedXsltContext = "Xp_UndefinedXsltContext";

		// Token: 0x0400028A RID: 650
		internal const string Xp_NoContext = "Xp_NoContext";

		// Token: 0x0400028B RID: 651
		internal const string Xp_UndefVar = "Xp_UndefVar";

		// Token: 0x0400028C RID: 652
		internal const string Xp_UndefFunc = "Xp_UndefFunc";

		// Token: 0x0400028D RID: 653
		internal const string Xp_FunctionFailed = "Xp_FunctionFailed";

		// Token: 0x0400028E RID: 654
		internal const string Xp_CurrentNotAllowed = "Xp_CurrentNotAllowed";

		// Token: 0x0400028F RID: 655
		internal const string Xdom_DualDocumentTypeNode = "Xdom_DualDocumentTypeNode";

		// Token: 0x04000290 RID: 656
		internal const string Xdom_DualDocumentElementNode = "Xdom_DualDocumentElementNode";

		// Token: 0x04000291 RID: 657
		internal const string Xdom_DualDeclarationNode = "Xdom_DualDeclarationNode";

		// Token: 0x04000292 RID: 658
		internal const string Xdom_Import = "Xdom_Import";

		// Token: 0x04000293 RID: 659
		internal const string Xdom_Import_NullNode = "Xdom_Import_NullNode";

		// Token: 0x04000294 RID: 660
		internal const string Xdom_NoRootEle = "Xdom_NoRootEle";

		// Token: 0x04000295 RID: 661
		internal const string Xdom_Attr_Name = "Xdom_Attr_Name";

		// Token: 0x04000296 RID: 662
		internal const string Xdom_AttrCol_Object = "Xdom_AttrCol_Object";

		// Token: 0x04000297 RID: 663
		internal const string Xdom_AttrCol_Insert = "Xdom_AttrCol_Insert";

		// Token: 0x04000298 RID: 664
		internal const string Xdom_NamedNode_Context = "Xdom_NamedNode_Context";

		// Token: 0x04000299 RID: 665
		internal const string Xdom_Version = "Xdom_Version";

		// Token: 0x0400029A RID: 666
		internal const string Xdom_standalone = "Xdom_standalone";

		// Token: 0x0400029B RID: 667
		internal const string Xdom_Ele_Prefix = "Xdom_Ele_Prefix";

		// Token: 0x0400029C RID: 668
		internal const string Xdom_Ent_Innertext = "Xdom_Ent_Innertext";

		// Token: 0x0400029D RID: 669
		internal const string Xdom_EntRef_SetVal = "Xdom_EntRef_SetVal";

		// Token: 0x0400029E RID: 670
		internal const string Xdom_WS_Char = "Xdom_WS_Char";

		// Token: 0x0400029F RID: 671
		internal const string Xdom_Node_SetVal = "Xdom_Node_SetVal";

		// Token: 0x040002A0 RID: 672
		internal const string Xdom_Empty_LocalName = "Xdom_Empty_LocalName";

		// Token: 0x040002A1 RID: 673
		internal const string Xdom_Set_InnerXml = "Xdom_Set_InnerXml";

		// Token: 0x040002A2 RID: 674
		internal const string Xdom_Attr_InUse = "Xdom_Attr_InUse";

		// Token: 0x040002A3 RID: 675
		internal const string Xdom_Enum_ElementList = "Xdom_Enum_ElementList";

		// Token: 0x040002A4 RID: 676
		internal const string Xdom_Invalid_NT_String = "Xdom_Invalid_NT_String";

		// Token: 0x040002A5 RID: 677
		internal const string Xdom_InvalidCharacter_EntityReference = "Xdom_InvalidCharacter_EntityReference";

		// Token: 0x040002A6 RID: 678
		internal const string Xdom_IndexOutOfRange = "Xdom_IndexOutOfRange";

		// Token: 0x040002A7 RID: 679
		internal const string Xpn_BadPosition = "Xpn_BadPosition";

		// Token: 0x040002A8 RID: 680
		internal const string Xpn_MissingParent = "Xpn_MissingParent";

		// Token: 0x040002A9 RID: 681
		internal const string Xpn_NoContent = "Xpn_NoContent";

		// Token: 0x040002AA RID: 682
		internal const string Xdom_Load_NoDocument = "Xdom_Load_NoDocument";

		// Token: 0x040002AB RID: 683
		internal const string Xdom_Load_NoReader = "Xdom_Load_NoReader";

		// Token: 0x040002AC RID: 684
		internal const string Xdom_Node_Null_Doc = "Xdom_Node_Null_Doc";

		// Token: 0x040002AD RID: 685
		internal const string Xdom_Node_Insert_Child = "Xdom_Node_Insert_Child";

		// Token: 0x040002AE RID: 686
		internal const string Xdom_Node_Insert_Contain = "Xdom_Node_Insert_Contain";

		// Token: 0x040002AF RID: 687
		internal const string Xdom_Node_Insert_Path = "Xdom_Node_Insert_Path";

		// Token: 0x040002B0 RID: 688
		internal const string Xdom_Node_Insert_Context = "Xdom_Node_Insert_Context";

		// Token: 0x040002B1 RID: 689
		internal const string Xdom_Node_Insert_Location = "Xdom_Node_Insert_Location";

		// Token: 0x040002B2 RID: 690
		internal const string Xdom_Node_Insert_TypeConflict = "Xdom_Node_Insert_TypeConflict";

		// Token: 0x040002B3 RID: 691
		internal const string Xdom_Node_Remove_Contain = "Xdom_Node_Remove_Contain";

		// Token: 0x040002B4 RID: 692
		internal const string Xdom_Node_Remove_Child = "Xdom_Node_Remove_Child";

		// Token: 0x040002B5 RID: 693
		internal const string Xdom_Node_Modify_ReadOnly = "Xdom_Node_Modify_ReadOnly";

		// Token: 0x040002B6 RID: 694
		internal const string Xdom_TextNode_SplitText = "Xdom_TextNode_SplitText";

		// Token: 0x040002B7 RID: 695
		internal const string Xdom_Attr_Reserved_XmlNS = "Xdom_Attr_Reserved_XmlNS";

		// Token: 0x040002B8 RID: 696
		internal const string Xdom_Node_Cloning = "Xdom_Node_Cloning";

		// Token: 0x040002B9 RID: 697
		internal const string Xnr_ResolveEntity = "Xnr_ResolveEntity";

		// Token: 0x040002BA RID: 698
		internal const string XmlMissingType = "XmlMissingType";

		// Token: 0x040002BB RID: 699
		internal const string XmlUnsupportedType = "XmlUnsupportedType";

		// Token: 0x040002BC RID: 700
		internal const string XmlSerializerUnsupportedType = "XmlSerializerUnsupportedType";

		// Token: 0x040002BD RID: 701
		internal const string XmlSerializerUnsupportedMember = "XmlSerializerUnsupportedMember";

		// Token: 0x040002BE RID: 702
		internal const string XmlUnsupportedTypeKind = "XmlUnsupportedTypeKind";

		// Token: 0x040002BF RID: 703
		internal const string XmlUnsupportedSoapTypeKind = "XmlUnsupportedSoapTypeKind";

		// Token: 0x040002C0 RID: 704
		internal const string XmlUnsupportedIDictionary = "XmlUnsupportedIDictionary";

		// Token: 0x040002C1 RID: 705
		internal const string XmlUnsupportedIDictionaryDetails = "XmlUnsupportedIDictionaryDetails";

		// Token: 0x040002C2 RID: 706
		internal const string XmlDuplicateTypeName = "XmlDuplicateTypeName";

		// Token: 0x040002C3 RID: 707
		internal const string XmlSerializableNameMissing1 = "XmlSerializableNameMissing1";

		// Token: 0x040002C4 RID: 708
		internal const string XmlConstructorInaccessible = "XmlConstructorInaccessible";

		// Token: 0x040002C5 RID: 709
		internal const string XmlTypeInaccessible = "XmlTypeInaccessible";

		// Token: 0x040002C6 RID: 710
		internal const string XmlTypeStatic = "XmlTypeStatic";

		// Token: 0x040002C7 RID: 711
		internal const string XmlNoDefaultAccessors = "XmlNoDefaultAccessors";

		// Token: 0x040002C8 RID: 712
		internal const string XmlNoAddMethod = "XmlNoAddMethod";

		// Token: 0x040002C9 RID: 713
		internal const string XmlAttributeSetAgain = "XmlAttributeSetAgain";

		// Token: 0x040002CA RID: 714
		internal const string XmlIllegalWildcard = "XmlIllegalWildcard";

		// Token: 0x040002CB RID: 715
		internal const string XmlIllegalArrayElement = "XmlIllegalArrayElement";

		// Token: 0x040002CC RID: 716
		internal const string XmlIllegalForm = "XmlIllegalForm";

		// Token: 0x040002CD RID: 717
		internal const string XmlBareTextMember = "XmlBareTextMember";

		// Token: 0x040002CE RID: 718
		internal const string XmlBareAttributeMember = "XmlBareAttributeMember";

		// Token: 0x040002CF RID: 719
		internal const string XmlReflectionError = "XmlReflectionError";

		// Token: 0x040002D0 RID: 720
		internal const string XmlTypeReflectionError = "XmlTypeReflectionError";

		// Token: 0x040002D1 RID: 721
		internal const string XmlPropertyReflectionError = "XmlPropertyReflectionError";

		// Token: 0x040002D2 RID: 722
		internal const string XmlFieldReflectionError = "XmlFieldReflectionError";

		// Token: 0x040002D3 RID: 723
		internal const string XmlInvalidDataTypeUsage = "XmlInvalidDataTypeUsage";

		// Token: 0x040002D4 RID: 724
		internal const string XmlInvalidXsdDataType = "XmlInvalidXsdDataType";

		// Token: 0x040002D5 RID: 725
		internal const string XmlDataTypeMismatch = "XmlDataTypeMismatch";

		// Token: 0x040002D6 RID: 726
		internal const string XmlIllegalTypeContext = "XmlIllegalTypeContext";

		// Token: 0x040002D7 RID: 727
		internal const string XmlUdeclaredXsdType = "XmlUdeclaredXsdType";

		// Token: 0x040002D8 RID: 728
		internal const string XmlAnyElementNamespace = "XmlAnyElementNamespace";

		// Token: 0x040002D9 RID: 729
		internal const string XmlInvalidConstantAttribute = "XmlInvalidConstantAttribute";

		// Token: 0x040002DA RID: 730
		internal const string XmlIllegalDefault = "XmlIllegalDefault";

		// Token: 0x040002DB RID: 731
		internal const string XmlIllegalAttributesArrayAttribute = "XmlIllegalAttributesArrayAttribute";

		// Token: 0x040002DC RID: 732
		internal const string XmlIllegalElementsArrayAttribute = "XmlIllegalElementsArrayAttribute";

		// Token: 0x040002DD RID: 733
		internal const string XmlIllegalArrayArrayAttribute = "XmlIllegalArrayArrayAttribute";

		// Token: 0x040002DE RID: 734
		internal const string XmlIllegalAttribute = "XmlIllegalAttribute";

		// Token: 0x040002DF RID: 735
		internal const string XmlIllegalType = "XmlIllegalType";

		// Token: 0x040002E0 RID: 736
		internal const string XmlIllegalAttrOrText = "XmlIllegalAttrOrText";

		// Token: 0x040002E1 RID: 737
		internal const string XmlIllegalSoapAttribute = "XmlIllegalSoapAttribute";

		// Token: 0x040002E2 RID: 738
		internal const string XmlIllegalAttrOrTextInterface = "XmlIllegalAttrOrTextInterface";

		// Token: 0x040002E3 RID: 739
		internal const string XmlIllegalAttributeFlagsArray = "XmlIllegalAttributeFlagsArray";

		// Token: 0x040002E4 RID: 740
		internal const string XmlIllegalAnyElement = "XmlIllegalAnyElement";

		// Token: 0x040002E5 RID: 741
		internal const string XmlInvalidIsNullable = "XmlInvalidIsNullable";

		// Token: 0x040002E6 RID: 742
		internal const string XmlInvalidNotNullable = "XmlInvalidNotNullable";

		// Token: 0x040002E7 RID: 743
		internal const string XmlInvalidFormUnqualified = "XmlInvalidFormUnqualified";

		// Token: 0x040002E8 RID: 744
		internal const string XmlDuplicateNamespace = "XmlDuplicateNamespace";

		// Token: 0x040002E9 RID: 745
		internal const string XmlElementHasNoName = "XmlElementHasNoName";

		// Token: 0x040002EA RID: 746
		internal const string XmlAttributeHasNoName = "XmlAttributeHasNoName";

		// Token: 0x040002EB RID: 747
		internal const string XmlElementImportedTwice = "XmlElementImportedTwice";

		// Token: 0x040002EC RID: 748
		internal const string XmlHiddenMember = "XmlHiddenMember";

		// Token: 0x040002ED RID: 749
		internal const string XmlInvalidXmlOverride = "XmlInvalidXmlOverride";

		// Token: 0x040002EE RID: 750
		internal const string XmlMembersDeriveError = "XmlMembersDeriveError";

		// Token: 0x040002EF RID: 751
		internal const string XmlTypeUsedTwice = "XmlTypeUsedTwice";

		// Token: 0x040002F0 RID: 752
		internal const string XmlMissingGroup = "XmlMissingGroup";

		// Token: 0x040002F1 RID: 753
		internal const string XmlMissingAttributeGroup = "XmlMissingAttributeGroup";

		// Token: 0x040002F2 RID: 754
		internal const string XmlMissingDataType = "XmlMissingDataType";

		// Token: 0x040002F3 RID: 755
		internal const string XmlInvalidEncoding = "XmlInvalidEncoding";

		// Token: 0x040002F4 RID: 756
		internal const string XmlMissingElement = "XmlMissingElement";

		// Token: 0x040002F5 RID: 757
		internal const string XmlMissingAttribute = "XmlMissingAttribute";

		// Token: 0x040002F6 RID: 758
		internal const string XmlMissingMethodEnum = "XmlMissingMethodEnum";

		// Token: 0x040002F7 RID: 759
		internal const string XmlNoAttributeHere = "XmlNoAttributeHere";

		// Token: 0x040002F8 RID: 760
		internal const string XmlNeedAttributeHere = "XmlNeedAttributeHere";

		// Token: 0x040002F9 RID: 761
		internal const string XmlElementNameMismatch = "XmlElementNameMismatch";

		// Token: 0x040002FA RID: 762
		internal const string XmlUnsupportedDefaultType = "XmlUnsupportedDefaultType";

		// Token: 0x040002FB RID: 763
		internal const string XmlUnsupportedDefaultValue = "XmlUnsupportedDefaultValue";

		// Token: 0x040002FC RID: 764
		internal const string XmlInvalidDefaultValue = "XmlInvalidDefaultValue";

		// Token: 0x040002FD RID: 765
		internal const string XmlInvalidDefaultEnumValue = "XmlInvalidDefaultEnumValue";

		// Token: 0x040002FE RID: 766
		internal const string XmlUnknownNode = "XmlUnknownNode";

		// Token: 0x040002FF RID: 767
		internal const string XmlUnknownConstant = "XmlUnknownConstant";

		// Token: 0x04000300 RID: 768
		internal const string XmlSerializeError = "XmlSerializeError";

		// Token: 0x04000301 RID: 769
		internal const string XmlSerializeErrorDetails = "XmlSerializeErrorDetails";

		// Token: 0x04000302 RID: 770
		internal const string XmlCompilerError = "XmlCompilerError";

		// Token: 0x04000303 RID: 771
		internal const string XmlSchemaDuplicateNamespace = "XmlSchemaDuplicateNamespace";

		// Token: 0x04000304 RID: 772
		internal const string XmlSchemaCompiled = "XmlSchemaCompiled";

		// Token: 0x04000305 RID: 773
		internal const string XmlInvalidSchemaExtension = "XmlInvalidSchemaExtension";

		// Token: 0x04000306 RID: 774
		internal const string XmlInvalidArrayDimentions = "XmlInvalidArrayDimentions";

		// Token: 0x04000307 RID: 775
		internal const string XmlInvalidArrayTypeName = "XmlInvalidArrayTypeName";

		// Token: 0x04000308 RID: 776
		internal const string XmlInvalidArrayTypeNamespace = "XmlInvalidArrayTypeNamespace";

		// Token: 0x04000309 RID: 777
		internal const string XmlMissingArrayType = "XmlMissingArrayType";

		// Token: 0x0400030A RID: 778
		internal const string XmlEmptyArrayType = "XmlEmptyArrayType";

		// Token: 0x0400030B RID: 779
		internal const string XmlInvalidArraySyntax = "XmlInvalidArraySyntax";

		// Token: 0x0400030C RID: 780
		internal const string XmlInvalidArrayTypeSyntax = "XmlInvalidArrayTypeSyntax";

		// Token: 0x0400030D RID: 781
		internal const string XmlMismatchedArrayBrackets = "XmlMismatchedArrayBrackets";

		// Token: 0x0400030E RID: 782
		internal const string XmlInvalidArrayLength = "XmlInvalidArrayLength";

		// Token: 0x0400030F RID: 783
		internal const string XmlMissingHref = "XmlMissingHref";

		// Token: 0x04000310 RID: 784
		internal const string XmlInvalidHref = "XmlInvalidHref";

		// Token: 0x04000311 RID: 785
		internal const string XmlUnknownType = "XmlUnknownType";

		// Token: 0x04000312 RID: 786
		internal const string XmlAbstractType = "XmlAbstractType";

		// Token: 0x04000313 RID: 787
		internal const string XmlMappingsScopeMismatch = "XmlMappingsScopeMismatch";

		// Token: 0x04000314 RID: 788
		internal const string XmlMethodTypeNameConflict = "XmlMethodTypeNameConflict";

		// Token: 0x04000315 RID: 789
		internal const string XmlCannotReconcileAccessor = "XmlCannotReconcileAccessor";

		// Token: 0x04000316 RID: 790
		internal const string XmlCannotReconcileAttributeAccessor = "XmlCannotReconcileAttributeAccessor";

		// Token: 0x04000317 RID: 791
		internal const string XmlCannotReconcileAccessorDefault = "XmlCannotReconcileAccessorDefault";

		// Token: 0x04000318 RID: 792
		internal const string XmlInvalidTypeAttributes = "XmlInvalidTypeAttributes";

		// Token: 0x04000319 RID: 793
		internal const string XmlInvalidAttributeUse = "XmlInvalidAttributeUse";

		// Token: 0x0400031A RID: 794
		internal const string XmlTypesDuplicate = "XmlTypesDuplicate";

		// Token: 0x0400031B RID: 795
		internal const string XmlInvalidSoapArray = "XmlInvalidSoapArray";

		// Token: 0x0400031C RID: 796
		internal const string XmlCannotIncludeInSchema = "XmlCannotIncludeInSchema";

		// Token: 0x0400031D RID: 797
		internal const string XmlSoapCannotIncludeInSchema = "XmlSoapCannotIncludeInSchema";

		// Token: 0x0400031E RID: 798
		internal const string XmlInvalidSerializable = "XmlInvalidSerializable";

		// Token: 0x0400031F RID: 799
		internal const string XmlInvalidUseOfType = "XmlInvalidUseOfType";

		// Token: 0x04000320 RID: 800
		internal const string XmlUnxpectedType = "XmlUnxpectedType";

		// Token: 0x04000321 RID: 801
		internal const string XmlUnknownAnyElement = "XmlUnknownAnyElement";

		// Token: 0x04000322 RID: 802
		internal const string XmlMultipleAttributeOverrides = "XmlMultipleAttributeOverrides";

		// Token: 0x04000323 RID: 803
		internal const string XmlInvalidEnumAttribute = "XmlInvalidEnumAttribute";

		// Token: 0x04000324 RID: 804
		internal const string XmlInvalidReturnPosition = "XmlInvalidReturnPosition";

		// Token: 0x04000325 RID: 805
		internal const string XmlInvalidElementAttribute = "XmlInvalidElementAttribute";

		// Token: 0x04000326 RID: 806
		internal const string XmlInvalidVoid = "XmlInvalidVoid";

		// Token: 0x04000327 RID: 807
		internal const string XmlInvalidContent = "XmlInvalidContent";

		// Token: 0x04000328 RID: 808
		internal const string XmlInvalidSchemaElementType = "XmlInvalidSchemaElementType";

		// Token: 0x04000329 RID: 809
		internal const string XmlInvalidSubstitutionGroupUse = "XmlInvalidSubstitutionGroupUse";

		// Token: 0x0400032A RID: 810
		internal const string XmlElementMissingType = "XmlElementMissingType";

		// Token: 0x0400032B RID: 811
		internal const string XmlInvalidAnyAttributeUse = "XmlInvalidAnyAttributeUse";

		// Token: 0x0400032C RID: 812
		internal const string XmlSoapInvalidAttributeUse = "XmlSoapInvalidAttributeUse";

		// Token: 0x0400032D RID: 813
		internal const string XmlSoapInvalidChoice = "XmlSoapInvalidChoice";

		// Token: 0x0400032E RID: 814
		internal const string XmlSoapUnsupportedGroupRef = "XmlSoapUnsupportedGroupRef";

		// Token: 0x0400032F RID: 815
		internal const string XmlSoapUnsupportedGroupRepeat = "XmlSoapUnsupportedGroupRepeat";

		// Token: 0x04000330 RID: 816
		internal const string XmlSoapUnsupportedGroupNested = "XmlSoapUnsupportedGroupNested";

		// Token: 0x04000331 RID: 817
		internal const string XmlSoapUnsupportedGroupAny = "XmlSoapUnsupportedGroupAny";

		// Token: 0x04000332 RID: 818
		internal const string XmlInvalidEnumContent = "XmlInvalidEnumContent";

		// Token: 0x04000333 RID: 819
		internal const string XmlInvalidAttributeType = "XmlInvalidAttributeType";

		// Token: 0x04000334 RID: 820
		internal const string XmlInvalidBaseType = "XmlInvalidBaseType";

		// Token: 0x04000335 RID: 821
		internal const string XmlPrimitiveBaseType = "XmlPrimitiveBaseType";

		// Token: 0x04000336 RID: 822
		internal const string XmlInvalidIdentifier = "XmlInvalidIdentifier";

		// Token: 0x04000337 RID: 823
		internal const string XmlGenError = "XmlGenError";

		// Token: 0x04000338 RID: 824
		internal const string XmlInvalidXmlns = "XmlInvalidXmlns";

		// Token: 0x04000339 RID: 825
		internal const string XmlCircularReference = "XmlCircularReference";

		// Token: 0x0400033A RID: 826
		internal const string XmlCircularReference2 = "XmlCircularReference2";

		// Token: 0x0400033B RID: 827
		internal const string XmlAnonymousBaseType = "XmlAnonymousBaseType";

		// Token: 0x0400033C RID: 828
		internal const string XmlMissingSchema = "XmlMissingSchema";

		// Token: 0x0400033D RID: 829
		internal const string XmlNoSerializableMembers = "XmlNoSerializableMembers";

		// Token: 0x0400033E RID: 830
		internal const string XmlIllegalOverride = "XmlIllegalOverride";

		// Token: 0x0400033F RID: 831
		internal const string XmlReadOnlyCollection = "XmlReadOnlyCollection";

		// Token: 0x04000340 RID: 832
		internal const string XmlRpcNestedValueType = "XmlRpcNestedValueType";

		// Token: 0x04000341 RID: 833
		internal const string XmlRpcRefsInValueType = "XmlRpcRefsInValueType";

		// Token: 0x04000342 RID: 834
		internal const string XmlRpcArrayOfValueTypes = "XmlRpcArrayOfValueTypes";

		// Token: 0x04000343 RID: 835
		internal const string XmlDuplicateElementName = "XmlDuplicateElementName";

		// Token: 0x04000344 RID: 836
		internal const string XmlDuplicateAttributeName = "XmlDuplicateAttributeName";

		// Token: 0x04000345 RID: 837
		internal const string XmlBadBaseElement = "XmlBadBaseElement";

		// Token: 0x04000346 RID: 838
		internal const string XmlBadBaseType = "XmlBadBaseType";

		// Token: 0x04000347 RID: 839
		internal const string XmlUndefinedAlias = "XmlUndefinedAlias";

		// Token: 0x04000348 RID: 840
		internal const string XmlChoiceIdentifierType = "XmlChoiceIdentifierType";

		// Token: 0x04000349 RID: 841
		internal const string XmlChoiceIdentifierArrayType = "XmlChoiceIdentifierArrayType";

		// Token: 0x0400034A RID: 842
		internal const string XmlChoiceIdentifierTypeEnum = "XmlChoiceIdentifierTypeEnum";

		// Token: 0x0400034B RID: 843
		internal const string XmlChoiceIdentiferMemberMissing = "XmlChoiceIdentiferMemberMissing";

		// Token: 0x0400034C RID: 844
		internal const string XmlChoiceIdentiferAmbiguous = "XmlChoiceIdentiferAmbiguous";

		// Token: 0x0400034D RID: 845
		internal const string XmlChoiceIdentiferMissing = "XmlChoiceIdentiferMissing";

		// Token: 0x0400034E RID: 846
		internal const string XmlChoiceMissingValue = "XmlChoiceMissingValue";

		// Token: 0x0400034F RID: 847
		internal const string XmlChoiceMissingAnyValue = "XmlChoiceMissingAnyValue";

		// Token: 0x04000350 RID: 848
		internal const string XmlChoiceMismatchChoiceException = "XmlChoiceMismatchChoiceException";

		// Token: 0x04000351 RID: 849
		internal const string XmlArrayItemAmbiguousTypes = "XmlArrayItemAmbiguousTypes";

		// Token: 0x04000352 RID: 850
		internal const string XmlUnsupportedInterface = "XmlUnsupportedInterface";

		// Token: 0x04000353 RID: 851
		internal const string XmlUnsupportedInterfaceDetails = "XmlUnsupportedInterfaceDetails";

		// Token: 0x04000354 RID: 852
		internal const string XmlUnsupportedRank = "XmlUnsupportedRank";

		// Token: 0x04000355 RID: 853
		internal const string XmlUnsupportedInheritance = "XmlUnsupportedInheritance";

		// Token: 0x04000356 RID: 854
		internal const string XmlIllegalMultipleText = "XmlIllegalMultipleText";

		// Token: 0x04000357 RID: 855
		internal const string XmlIllegalMultipleTextMembers = "XmlIllegalMultipleTextMembers";

		// Token: 0x04000358 RID: 856
		internal const string XmlIllegalArrayTextAttribute = "XmlIllegalArrayTextAttribute";

		// Token: 0x04000359 RID: 857
		internal const string XmlIllegalTypedTextAttribute = "XmlIllegalTypedTextAttribute";

		// Token: 0x0400035A RID: 858
		internal const string XmlIllegalSimpleContentExtension = "XmlIllegalSimpleContentExtension";

		// Token: 0x0400035B RID: 859
		internal const string XmlInvalidCast = "XmlInvalidCast";

		// Token: 0x0400035C RID: 860
		internal const string XmlInvalidCastWithId = "XmlInvalidCastWithId";

		// Token: 0x0400035D RID: 861
		internal const string XmlInvalidArrayRef = "XmlInvalidArrayRef";

		// Token: 0x0400035E RID: 862
		internal const string XmlInvalidNullCast = "XmlInvalidNullCast";

		// Token: 0x0400035F RID: 863
		internal const string XmlMultipleXmlns = "XmlMultipleXmlns";

		// Token: 0x04000360 RID: 864
		internal const string XmlMultipleXmlnsMembers = "XmlMultipleXmlnsMembers";

		// Token: 0x04000361 RID: 865
		internal const string XmlXmlnsInvalidType = "XmlXmlnsInvalidType";

		// Token: 0x04000362 RID: 866
		internal const string XmlSoleXmlnsAttribute = "XmlSoleXmlnsAttribute";

		// Token: 0x04000363 RID: 867
		internal const string XmlConstructorHasSecurityAttributes = "XmlConstructorHasSecurityAttributes";

		// Token: 0x04000364 RID: 868
		internal const string XmlPropertyHasSecurityAttributes = "XmlPropertyHasSecurityAttributes";

		// Token: 0x04000365 RID: 869
		internal const string XmlMethodHasSecurityAttributes = "XmlMethodHasSecurityAttributes";

		// Token: 0x04000366 RID: 870
		internal const string XmlDefaultAccessorHasSecurityAttributes = "XmlDefaultAccessorHasSecurityAttributes";

		// Token: 0x04000367 RID: 871
		internal const string XmlInvalidChoiceIdentifierValue = "XmlInvalidChoiceIdentifierValue";

		// Token: 0x04000368 RID: 872
		internal const string XmlAnyElementDuplicate = "XmlAnyElementDuplicate";

		// Token: 0x04000369 RID: 873
		internal const string XmlChoiceIdDuplicate = "XmlChoiceIdDuplicate";

		// Token: 0x0400036A RID: 874
		internal const string XmlChoiceIdentifierMismatch = "XmlChoiceIdentifierMismatch";

		// Token: 0x0400036B RID: 875
		internal const string XmlUnsupportedRedefine = "XmlUnsupportedRedefine";

		// Token: 0x0400036C RID: 876
		internal const string XmlDuplicateElementInScope = "XmlDuplicateElementInScope";

		// Token: 0x0400036D RID: 877
		internal const string XmlDuplicateElementInScope1 = "XmlDuplicateElementInScope1";

		// Token: 0x0400036E RID: 878
		internal const string XmlNoPartialTrust = "XmlNoPartialTrust";

		// Token: 0x0400036F RID: 879
		internal const string XmlInvalidEncodingNotEncoded1 = "XmlInvalidEncodingNotEncoded1";

		// Token: 0x04000370 RID: 880
		internal const string XmlInvalidEncoding3 = "XmlInvalidEncoding3";

		// Token: 0x04000371 RID: 881
		internal const string XmlInvalidSpecifiedType = "XmlInvalidSpecifiedType";

		// Token: 0x04000372 RID: 882
		internal const string XmlUnsupportedOpenGenericType = "XmlUnsupportedOpenGenericType";

		// Token: 0x04000373 RID: 883
		internal const string XmlMismatchSchemaObjects = "XmlMismatchSchemaObjects";

		// Token: 0x04000374 RID: 884
		internal const string XmlCircularTypeReference = "XmlCircularTypeReference";

		// Token: 0x04000375 RID: 885
		internal const string XmlCircularGroupReference = "XmlCircularGroupReference";

		// Token: 0x04000376 RID: 886
		internal const string XmlRpcLitElementNamespace = "XmlRpcLitElementNamespace";

		// Token: 0x04000377 RID: 887
		internal const string XmlRpcLitElementNullable = "XmlRpcLitElementNullable";

		// Token: 0x04000378 RID: 888
		internal const string XmlRpcLitElements = "XmlRpcLitElements";

		// Token: 0x04000379 RID: 889
		internal const string XmlRpcLitArrayElement = "XmlRpcLitArrayElement";

		// Token: 0x0400037A RID: 890
		internal const string XmlRpcLitAttributeAttributes = "XmlRpcLitAttributeAttributes";

		// Token: 0x0400037B RID: 891
		internal const string XmlRpcLitAttributes = "XmlRpcLitAttributes";

		// Token: 0x0400037C RID: 892
		internal const string XmlSequenceMembers = "XmlSequenceMembers";

		// Token: 0x0400037D RID: 893
		internal const string XmlRpcLitXmlns = "XmlRpcLitXmlns";

		// Token: 0x0400037E RID: 894
		internal const string XmlDuplicateNs = "XmlDuplicateNs";

		// Token: 0x0400037F RID: 895
		internal const string XmlAnonymousInclude = "XmlAnonymousInclude";

		// Token: 0x04000380 RID: 896
		internal const string XmlSchemaIncludeLocation = "XmlSchemaIncludeLocation";

		// Token: 0x04000381 RID: 897
		internal const string XmlSerializableSchemaError = "XmlSerializableSchemaError";

		// Token: 0x04000382 RID: 898
		internal const string XmlGetSchemaMethodName = "XmlGetSchemaMethodName";

		// Token: 0x04000383 RID: 899
		internal const string XmlGetSchemaMethodMissing = "XmlGetSchemaMethodMissing";

		// Token: 0x04000384 RID: 900
		internal const string XmlGetSchemaMethodReturnType = "XmlGetSchemaMethodReturnType";

		// Token: 0x04000385 RID: 901
		internal const string XmlGetSchemaEmptyTypeName = "XmlGetSchemaEmptyTypeName";

		// Token: 0x04000386 RID: 902
		internal const string XmlGetSchemaTypeMissing = "XmlGetSchemaTypeMissing";

		// Token: 0x04000387 RID: 903
		internal const string XmlGetSchemaInclude = "XmlGetSchemaInclude";

		// Token: 0x04000388 RID: 904
		internal const string XmlSerializableAttributes = "XmlSerializableAttributes";

		// Token: 0x04000389 RID: 905
		internal const string XmlSerializableMergeItem = "XmlSerializableMergeItem";

		// Token: 0x0400038A RID: 906
		internal const string XmlSerializableBadDerivation = "XmlSerializableBadDerivation";

		// Token: 0x0400038B RID: 907
		internal const string XmlSerializableMissingClrType = "XmlSerializableMissingClrType";

		// Token: 0x0400038C RID: 908
		internal const string XmlCircularDerivation = "XmlCircularDerivation";

		// Token: 0x0400038D RID: 909
		internal const string XmlSerializerAccessDenied = "XmlSerializerAccessDenied";

		// Token: 0x0400038E RID: 910
		internal const string XmlIdentityAccessDenied = "XmlIdentityAccessDenied";

		// Token: 0x0400038F RID: 911
		internal const string XmlMelformMapping = "XmlMelformMapping";

		// Token: 0x04000390 RID: 912
		internal const string XmlSerializableWriteLess = "XmlSerializableWriteLess";

		// Token: 0x04000391 RID: 913
		internal const string XmlSerializableWriteMore = "XmlSerializableWriteMore";

		// Token: 0x04000392 RID: 914
		internal const string XmlSerializableReadMore = "XmlSerializableReadMore";

		// Token: 0x04000393 RID: 915
		internal const string XmlSerializableReadLess = "XmlSerializableReadLess";

		// Token: 0x04000394 RID: 916
		internal const string XmlSerializableIllegalOperation = "XmlSerializableIllegalOperation";

		// Token: 0x04000395 RID: 917
		internal const string XmlSchemaSyntaxErrorDetails = "XmlSchemaSyntaxErrorDetails";

		// Token: 0x04000396 RID: 918
		internal const string XmlSchemaElementReference = "XmlSchemaElementReference";

		// Token: 0x04000397 RID: 919
		internal const string XmlSchemaAttributeReference = "XmlSchemaAttributeReference";

		// Token: 0x04000398 RID: 920
		internal const string XmlSchemaItem = "XmlSchemaItem";

		// Token: 0x04000399 RID: 921
		internal const string XmlSchemaNamedItem = "XmlSchemaNamedItem";

		// Token: 0x0400039A RID: 922
		internal const string XmlSchemaContentDef = "XmlSchemaContentDef";

		// Token: 0x0400039B RID: 923
		internal const string XmlSchema = "XmlSchema";

		// Token: 0x0400039C RID: 924
		internal const string XmlSerializerCompileFailed = "XmlSerializerCompileFailed";

		// Token: 0x0400039D RID: 925
		internal const string XmlSerializableRootDupName = "XmlSerializableRootDupName";

		// Token: 0x0400039E RID: 926
		internal const string XmlDropDefaultAttribute = "XmlDropDefaultAttribute";

		// Token: 0x0400039F RID: 927
		internal const string XmlDropAttributeValue = "XmlDropAttributeValue";

		// Token: 0x040003A0 RID: 928
		internal const string XmlDropArrayAttributeValue = "XmlDropArrayAttributeValue";

		// Token: 0x040003A1 RID: 929
		internal const string XmlDropNonPrimitiveAttributeValue = "XmlDropNonPrimitiveAttributeValue";

		// Token: 0x040003A2 RID: 930
		internal const string XmlNotKnownDefaultValue = "XmlNotKnownDefaultValue";

		// Token: 0x040003A3 RID: 931
		internal const string XmlRemarks = "XmlRemarks";

		// Token: 0x040003A4 RID: 932
		internal const string XmlCodegenWarningDetails = "XmlCodegenWarningDetails";

		// Token: 0x040003A5 RID: 933
		internal const string XmlExtensionComment = "XmlExtensionComment";

		// Token: 0x040003A6 RID: 934
		internal const string XmlExtensionDuplicateDefinition = "XmlExtensionDuplicateDefinition";

		// Token: 0x040003A7 RID: 935
		internal const string XmlImporterExtensionBadLocalTypeName = "XmlImporterExtensionBadLocalTypeName";

		// Token: 0x040003A8 RID: 936
		internal const string XmlImporterExtensionBadTypeName = "XmlImporterExtensionBadTypeName";

		// Token: 0x040003A9 RID: 937
		internal const string XmlConfigurationDuplicateExtension = "XmlConfigurationDuplicateExtension";

		// Token: 0x040003AA RID: 938
		internal const string XmlPregenMissingDirectory = "XmlPregenMissingDirectory";

		// Token: 0x040003AB RID: 939
		internal const string XmlPregenMissingTempDirectory = "XmlPregenMissingTempDirectory";

		// Token: 0x040003AC RID: 940
		internal const string XmlPregenTypeDynamic = "XmlPregenTypeDynamic";

		// Token: 0x040003AD RID: 941
		internal const string XmlSerializerExpiredDetails = "XmlSerializerExpiredDetails";

		// Token: 0x040003AE RID: 942
		internal const string XmlSerializerExpired = "XmlSerializerExpired";

		// Token: 0x040003AF RID: 943
		internal const string XmlPregenAssemblyDynamic = "XmlPregenAssemblyDynamic";

		// Token: 0x040003B0 RID: 944
		internal const string XmlNotSerializable = "XmlNotSerializable";

		// Token: 0x040003B1 RID: 945
		internal const string XmlPregenOrphanType = "XmlPregenOrphanType";

		// Token: 0x040003B2 RID: 946
		internal const string XmlPregenCannotLoad = "XmlPregenCannotLoad";

		// Token: 0x040003B3 RID: 947
		internal const string XmlPregenInvalidXmlSerializerAssemblyAttribute = "XmlPregenInvalidXmlSerializerAssemblyAttribute";

		// Token: 0x040003B4 RID: 948
		internal const string XmlSequenceInconsistent = "XmlSequenceInconsistent";

		// Token: 0x040003B5 RID: 949
		internal const string XmlSequenceUnique = "XmlSequenceUnique";

		// Token: 0x040003B6 RID: 950
		internal const string XmlSequenceHierarchy = "XmlSequenceHierarchy";

		// Token: 0x040003B7 RID: 951
		internal const string XmlSequenceMatch = "XmlSequenceMatch";

		// Token: 0x040003B8 RID: 952
		internal const string XmlDisallowNegativeValues = "XmlDisallowNegativeValues";

		// Token: 0x040003B9 RID: 953
		internal const string XmlInternalError = "XmlInternalError";

		// Token: 0x040003BA RID: 954
		internal const string XmlInternalErrorDetails = "XmlInternalErrorDetails";

		// Token: 0x040003BB RID: 955
		internal const string XmlInternalErrorMethod = "XmlInternalErrorMethod";

		// Token: 0x040003BC RID: 956
		internal const string XmlInternalErrorReaderAdvance = "XmlInternalErrorReaderAdvance";

		// Token: 0x040003BD RID: 957
		internal const string XmlNonCLSCompliantException = "XmlNonCLSCompliantException";

		// Token: 0x040003BE RID: 958
		internal const string XmlConvert_BadFormat = "XmlConvert_BadFormat";

		// Token: 0x040003BF RID: 959
		internal const string XmlConvert_Overflow = "XmlConvert_Overflow";

		// Token: 0x040003C0 RID: 960
		internal const string XmlConvert_TypeBadMapping = "XmlConvert_TypeBadMapping";

		// Token: 0x040003C1 RID: 961
		internal const string XmlConvert_TypeBadMapping2 = "XmlConvert_TypeBadMapping2";

		// Token: 0x040003C2 RID: 962
		internal const string XmlConvert_TypeListBadMapping = "XmlConvert_TypeListBadMapping";

		// Token: 0x040003C3 RID: 963
		internal const string XmlConvert_TypeListBadMapping2 = "XmlConvert_TypeListBadMapping2";

		// Token: 0x040003C4 RID: 964
		internal const string XmlConvert_TypeToString = "XmlConvert_TypeToString";

		// Token: 0x040003C5 RID: 965
		internal const string XmlConvert_TypeFromString = "XmlConvert_TypeFromString";

		// Token: 0x040003C6 RID: 966
		internal const string XmlConvert_TypeNoPrefix = "XmlConvert_TypeNoPrefix";

		// Token: 0x040003C7 RID: 967
		internal const string XmlConvert_TypeNoNamespace = "XmlConvert_TypeNoNamespace";

		// Token: 0x040003C8 RID: 968
		internal const string RefSyntaxNotSupportedForElements0 = "RefSyntaxNotSupportedForElements0";

		// Token: 0x040003C9 RID: 969
		internal const string XPathDocument_MissingSchemas = "XPathDocument_MissingSchemas";

		// Token: 0x040003CA RID: 970
		internal const string XPathDocument_NotEnoughSchemaInfo = "XPathDocument_NotEnoughSchemaInfo";

		// Token: 0x040003CB RID: 971
		internal const string XPathDocument_ValidateInvalidNodeType = "XPathDocument_ValidateInvalidNodeType";

		// Token: 0x040003CC RID: 972
		internal const string XPathDocument_SchemaSetNotAllowed = "XPathDocument_SchemaSetNotAllowed";

		// Token: 0x040003CD RID: 973
		internal const string XmlBin_MissingEndCDATA = "XmlBin_MissingEndCDATA";

		// Token: 0x040003CE RID: 974
		internal const string XmlBin_InvalidQNameID = "XmlBin_InvalidQNameID";

		// Token: 0x040003CF RID: 975
		internal const string XmlBinary_UnexpectedToken = "XmlBinary_UnexpectedToken";

		// Token: 0x040003D0 RID: 976
		internal const string XmlBinary_InvalidSqlDecimal = "XmlBinary_InvalidSqlDecimal";

		// Token: 0x040003D1 RID: 977
		internal const string XmlBinary_InvalidSignature = "XmlBinary_InvalidSignature";

		// Token: 0x040003D2 RID: 978
		internal const string XmlBinary_InvalidProtocolVersion = "XmlBinary_InvalidProtocolVersion";

		// Token: 0x040003D3 RID: 979
		internal const string XmlBinary_UnsupportedCodePage = "XmlBinary_UnsupportedCodePage";

		// Token: 0x040003D4 RID: 980
		internal const string XmlBinary_InvalidStandalone = "XmlBinary_InvalidStandalone";

		// Token: 0x040003D5 RID: 981
		internal const string XmlBinary_NoParserContext = "XmlBinary_NoParserContext";

		// Token: 0x040003D6 RID: 982
		internal const string XmlBinary_ListsOfValuesNotSupported = "XmlBinary_ListsOfValuesNotSupported";

		// Token: 0x040003D7 RID: 983
		internal const string XmlBinary_CastNotSupported = "XmlBinary_CastNotSupported";

		// Token: 0x040003D8 RID: 984
		internal const string XmlBinary_NoRemapPrefix = "XmlBinary_NoRemapPrefix";

		// Token: 0x040003D9 RID: 985
		internal const string XmlBinary_AttrWithNsNoPrefix = "XmlBinary_AttrWithNsNoPrefix";

		// Token: 0x040003DA RID: 986
		internal const string XmlBinary_ValueTooBig = "XmlBinary_ValueTooBig";

		// Token: 0x040003DB RID: 987
		internal const string SqlTypes_ArithOverflow = "SqlTypes_ArithOverflow";

		// Token: 0x040003DC RID: 988
		internal const string SqlTypes_ArithTruncation = "SqlTypes_ArithTruncation";

		// Token: 0x040003DD RID: 989
		internal const string SqlTypes_DivideByZero = "SqlTypes_DivideByZero";

		// Token: 0x040003DE RID: 990
		internal const string Enc_InvalidByteInEncoding = "Enc_InvalidByteInEncoding";

		// Token: 0x040003DF RID: 991
		internal const string Arg_ExpectingXmlTextReader = "Arg_ExpectingXmlTextReader";

		// Token: 0x040003E0 RID: 992
		internal const string Arg_CannotCreateNode = "Arg_CannotCreateNode";

		// Token: 0x040003E1 RID: 993
		internal const string Xml_BadComment = "Xml_BadComment";

		// Token: 0x040003E2 RID: 994
		internal const string Xml_NumEntityOverflow = "Xml_NumEntityOverflow";

		// Token: 0x040003E3 RID: 995
		internal const string Xml_UnexpectedCharacter = "Xml_UnexpectedCharacter";

		// Token: 0x040003E4 RID: 996
		internal const string Xml_UnexpectedToken1 = "Xml_UnexpectedToken1";

		// Token: 0x040003E5 RID: 997
		internal const string Xml_TagMismatchFileName = "Xml_TagMismatchFileName";

		// Token: 0x040003E6 RID: 998
		internal const string Xml_ReservedNs = "Xml_ReservedNs";

		// Token: 0x040003E7 RID: 999
		internal const string Xml_BadElementData = "Xml_BadElementData";

		// Token: 0x040003E8 RID: 1000
		internal const string Xml_UnexpectedElement = "Xml_UnexpectedElement";

		// Token: 0x040003E9 RID: 1001
		internal const string Xml_TagNotInTheSameEntity = "Xml_TagNotInTheSameEntity";

		// Token: 0x040003EA RID: 1002
		internal const string Xml_InvalidPartialContentData = "Xml_InvalidPartialContentData";

		// Token: 0x040003EB RID: 1003
		internal const string Xml_CanNotStartWithXmlInNamespace = "Xml_CanNotStartWithXmlInNamespace";

		// Token: 0x040003EC RID: 1004
		internal const string Xml_UnparsedEntity = "Xml_UnparsedEntity";

		// Token: 0x040003ED RID: 1005
		internal const string Xml_InvalidContentForThisNode = "Xml_InvalidContentForThisNode";

		// Token: 0x040003EE RID: 1006
		internal const string Xml_MissingEncodingDecl = "Xml_MissingEncodingDecl";

		// Token: 0x040003EF RID: 1007
		internal const string Xml_InvalidSurrogatePair = "Xml_InvalidSurrogatePair";

		// Token: 0x040003F0 RID: 1008
		internal const string Sch_ErrorPosition = "Sch_ErrorPosition";

		// Token: 0x040003F1 RID: 1009
		internal const string Sch_ReservedNsDecl = "Sch_ReservedNsDecl";

		// Token: 0x040003F2 RID: 1010
		internal const string Sch_NotInSchemaCollection = "Sch_NotInSchemaCollection";

		// Token: 0x040003F3 RID: 1011
		internal const string Sch_NotationNotAttr = "Sch_NotationNotAttr";

		// Token: 0x040003F4 RID: 1012
		internal const string Sch_InvalidContent = "Sch_InvalidContent";

		// Token: 0x040003F5 RID: 1013
		internal const string Sch_InvalidContentExpecting = "Sch_InvalidContentExpecting";

		// Token: 0x040003F6 RID: 1014
		internal const string Sch_InvalidTextWhiteSpace = "Sch_InvalidTextWhiteSpace";

		// Token: 0x040003F7 RID: 1015
		internal const string Sch_XSCHEMA = "Sch_XSCHEMA";

		// Token: 0x040003F8 RID: 1016
		internal const string Sch_DubSchema = "Sch_DubSchema";

		// Token: 0x040003F9 RID: 1017
		internal const string Xp_TokenExpected = "Xp_TokenExpected";

		// Token: 0x040003FA RID: 1018
		internal const string Xp_NodeTestExpected = "Xp_NodeTestExpected";

		// Token: 0x040003FB RID: 1019
		internal const string Xp_NumberExpected = "Xp_NumberExpected";

		// Token: 0x040003FC RID: 1020
		internal const string Xp_QueryExpected = "Xp_QueryExpected";

		// Token: 0x040003FD RID: 1021
		internal const string Xp_InvalidArgument = "Xp_InvalidArgument";

		// Token: 0x040003FE RID: 1022
		internal const string Xp_FunctionExpected = "Xp_FunctionExpected";

		// Token: 0x040003FF RID: 1023
		internal const string Xp_InvalidPatternString = "Xp_InvalidPatternString";

		// Token: 0x04000400 RID: 1024
		internal const string Xp_BadQueryString = "Xp_BadQueryString";

		// Token: 0x04000401 RID: 1025
		internal const string XdomXpNav_NullParam = "XdomXpNav_NullParam";

		// Token: 0x04000402 RID: 1026
		internal const string Xdom_Load_NodeType = "Xdom_Load_NodeType";

		// Token: 0x04000403 RID: 1027
		internal const string XmlMissingMethod = "XmlMissingMethod";

		// Token: 0x04000404 RID: 1028
		internal const string XmlIncludeSerializableError = "XmlIncludeSerializableError";

		// Token: 0x04000405 RID: 1029
		internal const string XmlCompilerDynModule = "XmlCompilerDynModule";

		// Token: 0x04000406 RID: 1030
		internal const string XmlInvalidSchemaType = "XmlInvalidSchemaType";

		// Token: 0x04000407 RID: 1031
		internal const string XmlInvalidAnyUse = "XmlInvalidAnyUse";

		// Token: 0x04000408 RID: 1032
		internal const string XmlSchemaSyntaxError = "XmlSchemaSyntaxError";

		// Token: 0x04000409 RID: 1033
		internal const string XmlDuplicateChoiceElement = "XmlDuplicateChoiceElement";

		// Token: 0x0400040A RID: 1034
		internal const string XmlConvert_BadTimeSpan = "XmlConvert_BadTimeSpan";

		// Token: 0x0400040B RID: 1035
		internal const string XmlConvert_BadBoolean = "XmlConvert_BadBoolean";

		// Token: 0x0400040C RID: 1036
		internal const string XmlConvert_BadUri = "XmlConvert_BadUri";

		// Token: 0x0400040D RID: 1037
		internal const string Xml_UnexpectedToken = "Xml_UnexpectedToken";

		// Token: 0x0400040E RID: 1038
		internal const string Xml_PartialContentNodeTypeNotSupported = "Xml_PartialContentNodeTypeNotSupported";

		// Token: 0x0400040F RID: 1039
		internal const string Sch_AttributeValueDataType = "Sch_AttributeValueDataType";

		// Token: 0x04000410 RID: 1040
		internal const string Sch_ElementValueDataType = "Sch_ElementValueDataType";

		// Token: 0x04000411 RID: 1041
		internal const string Sch_NonDeterministicAny = "Sch_NonDeterministicAny";

		// Token: 0x04000412 RID: 1042
		internal const string Sch_MismatchTargetNamespace = "Sch_MismatchTargetNamespace";

		// Token: 0x04000413 RID: 1043
		internal const string Sch_UnionFailed = "Sch_UnionFailed";

		// Token: 0x04000414 RID: 1044
		internal const string Sch_XsiTypeBlocked = "Sch_XsiTypeBlocked";

		// Token: 0x04000415 RID: 1045
		internal const string Sch_InvalidElementInEmpty = "Sch_InvalidElementInEmpty";

		// Token: 0x04000416 RID: 1046
		internal const string Sch_InvalidElementInTextOnly = "Sch_InvalidElementInTextOnly";

		// Token: 0x04000417 RID: 1047
		internal const string Sch_InvalidNameAttribute = "Sch_InvalidNameAttribute";

		// Token: 0x04000418 RID: 1048
		private static Res loader;

		// Token: 0x04000419 RID: 1049
		private ResourceManager resources;

		// Token: 0x0400041A RID: 1050
		private static object s_InternalSyncObject;
	}
}
