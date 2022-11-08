using System;
using System.Xml.Schema;
using System.Xml.XmlConfiguration;

namespace System.Xml
{
	// Token: 0x0200007E RID: 126
	public sealed class XmlReaderSettings
	{
		// Token: 0x0600059C RID: 1436 RVA: 0x00016DAF File Offset: 0x00015DAF
		public XmlReaderSettings()
		{
			this.Reset();
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00016DBD File Offset: 0x00015DBD
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00016DC5 File Offset: 0x00015DC5
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
			set
			{
				this.CheckReadOnly("NameTable");
				this.nameTable = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00016DD9 File Offset: 0x00015DD9
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00016DE1 File Offset: 0x00015DE1
		internal bool IsXmlResolverSet
		{
			get
			{
				return this.isXmlResolverSet;
			}
			private set
			{
				this.isXmlResolverSet = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00016DEA File Offset: 0x00015DEA
		public XmlResolver XmlResolver
		{
			set
			{
				this.CheckReadOnly("XmlResolver");
				this.xmlResolver = value;
				this.IsXmlResolverSet = true;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00016E05 File Offset: 0x00015E05
		internal XmlResolver GetXmlResolver()
		{
			return this.xmlResolver;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00016E0D File Offset: 0x00015E0D
		internal XmlResolver GetXmlResolver_CheckConfig()
		{
			if (XmlReaderSection.ProhibitDefaultUrlResolver && !this.IsXmlResolverSet)
			{
				return null;
			}
			return this.xmlResolver;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00016E26 File Offset: 0x00015E26
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x00016E2E File Offset: 0x00015E2E
		public int LineNumberOffset
		{
			get
			{
				return this.lineNumberOffset;
			}
			set
			{
				this.CheckReadOnly("LineNumberOffset");
				if (this.lineNumberOffset < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.lineNumberOffset = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00016E56 File Offset: 0x00015E56
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x00016E5E File Offset: 0x00015E5E
		public int LinePositionOffset
		{
			get
			{
				return this.linePositionOffset;
			}
			set
			{
				this.CheckReadOnly("LinePositionOffset");
				if (this.linePositionOffset < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.linePositionOffset = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00016E86 File Offset: 0x00015E86
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00016E8E File Offset: 0x00015E8E
		public ConformanceLevel ConformanceLevel
		{
			get
			{
				return this.conformanceLevel;
			}
			set
			{
				this.CheckReadOnly("ConformanceLevel");
				if (value > ConformanceLevel.Document)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.conformanceLevel = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00016EB1 File Offset: 0x00015EB1
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00016EB9 File Offset: 0x00015EB9
		public bool CheckCharacters
		{
			get
			{
				return this.checkCharacters;
			}
			set
			{
				this.CheckReadOnly("CheckCharacters");
				this.checkCharacters = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00016ECD File Offset: 0x00015ECD
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00016ED5 File Offset: 0x00015ED5
		public long MaxCharactersInDocument
		{
			get
			{
				return this.maxCharactersInDocument;
			}
			set
			{
				this.CheckReadOnly("MaxCharactersInDocument");
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxCharactersInDocument = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00016EF9 File Offset: 0x00015EF9
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00016F01 File Offset: 0x00015F01
		public long MaxCharactersFromEntities
		{
			get
			{
				return this.maxCharactersFromEntities;
			}
			set
			{
				this.CheckReadOnly("MaxCharactersFromEntities");
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxCharactersFromEntities = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00016F25 File Offset: 0x00015F25
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x00016F2D File Offset: 0x00015F2D
		public ValidationType ValidationType
		{
			get
			{
				return this.validationType;
			}
			set
			{
				this.CheckReadOnly("ValidationType");
				if (value > ValidationType.Schema)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.validationType = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00016F50 File Offset: 0x00015F50
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x00016F58 File Offset: 0x00015F58
		public XmlSchemaValidationFlags ValidationFlags
		{
			get
			{
				return this.validationFlags;
			}
			set
			{
				this.CheckReadOnly("ValidationFlags");
				if (value > (XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ProcessSchemaLocation | XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.AllowXmlAttributes))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.validationFlags = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00016F7C File Offset: 0x00015F7C
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x00016F97 File Offset: 0x00015F97
		public XmlSchemaSet Schemas
		{
			get
			{
				if (this.schemas == null)
				{
					this.schemas = new XmlSchemaSet();
				}
				return this.schemas;
			}
			set
			{
				this.CheckReadOnly("Schemas");
				this.schemas = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060005B6 RID: 1462 RVA: 0x00016FAB File Offset: 0x00015FAB
		// (remove) Token: 0x060005B7 RID: 1463 RVA: 0x00016FCF File Offset: 0x00015FCF
		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.CheckReadOnly("ValidationEventHandler");
				this.valEventHandler = (ValidationEventHandler)Delegate.Combine(this.valEventHandler, value);
			}
			remove
			{
				this.CheckReadOnly("ValidationEventHandler");
				this.valEventHandler = (ValidationEventHandler)Delegate.Remove(this.valEventHandler, value);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00016FF3 File Offset: 0x00015FF3
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x00016FFB File Offset: 0x00015FFB
		public bool IgnoreWhitespace
		{
			get
			{
				return this.ignoreWhitespace;
			}
			set
			{
				this.CheckReadOnly("IgnoreWhitespace");
				this.ignoreWhitespace = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001700F File Offset: 0x0001600F
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x00017017 File Offset: 0x00016017
		public bool IgnoreProcessingInstructions
		{
			get
			{
				return this.ignorePIs;
			}
			set
			{
				this.CheckReadOnly("IgnoreProcessingInstructions");
				this.ignorePIs = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001702B File Offset: 0x0001602B
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00017033 File Offset: 0x00016033
		public bool IgnoreComments
		{
			get
			{
				return this.ignoreComments;
			}
			set
			{
				this.CheckReadOnly("IgnoreComments");
				this.ignoreComments = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00017047 File Offset: 0x00016047
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0001704F File Offset: 0x0001604F
		public bool ProhibitDtd
		{
			get
			{
				return this.prohibitDtd;
			}
			set
			{
				this.CheckReadOnly("ProhibitDtd");
				this.prohibitDtd = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00017063 File Offset: 0x00016063
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001706B File Offset: 0x0001606B
		public bool CloseInput
		{
			get
			{
				return this.closeInput;
			}
			set
			{
				this.CheckReadOnly("CloseInput");
				this.closeInput = value;
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00017080 File Offset: 0x00016080
		public void Reset()
		{
			this.CheckReadOnly("Reset");
			this.nameTable = null;
			this.xmlResolver = XmlReaderSettings.CreateDefaultResolver();
			this.lineNumberOffset = 0;
			this.linePositionOffset = 0;
			this.checkCharacters = true;
			this.conformanceLevel = ConformanceLevel.Document;
			this.schemas = null;
			this.validationType = ValidationType.None;
			this.validationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints;
			this.validationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;
			this.ignoreWhitespace = false;
			this.ignorePIs = false;
			this.ignoreComments = false;
			this.prohibitDtd = true;
			this.closeInput = false;
			this.maxCharactersFromEntities = 0L;
			this.maxCharactersInDocument = 0L;
			this.isReadOnly = false;
			this.IsXmlResolverSet = false;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001712B File Offset: 0x0001612B
		private static XmlResolver CreateDefaultResolver()
		{
			return new XmlUrlResolver();
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00017134 File Offset: 0x00016134
		public XmlReaderSettings Clone()
		{
			XmlReaderSettings xmlReaderSettings = base.MemberwiseClone() as XmlReaderSettings;
			xmlReaderSettings.isReadOnly = false;
			return xmlReaderSettings;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00017155 File Offset: 0x00016155
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001715D File Offset: 0x0001615D
		internal bool ReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
			set
			{
				this.isReadOnly = value;
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00017166 File Offset: 0x00016166
		internal ValidationEventHandler GetEventHandler()
		{
			return this.valEventHandler;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001716E File Offset: 0x0001616E
		private void CheckReadOnly(string propertyName)
		{
			if (this.isReadOnly)
			{
				throw new XmlException("Xml_ReadOnlyProperty", "XmlReaderSettings." + propertyName);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0001718E File Offset: 0x0001618E
		internal bool CanResolveExternals
		{
			get
			{
				return !this.prohibitDtd && this.xmlResolver != null;
			}
		}

		// Token: 0x0400063F RID: 1599
		private XmlNameTable nameTable;

		// Token: 0x04000640 RID: 1600
		private XmlResolver xmlResolver;

		// Token: 0x04000641 RID: 1601
		private int lineNumberOffset;

		// Token: 0x04000642 RID: 1602
		private int linePositionOffset;

		// Token: 0x04000643 RID: 1603
		private ConformanceLevel conformanceLevel;

		// Token: 0x04000644 RID: 1604
		private bool checkCharacters;

		// Token: 0x04000645 RID: 1605
		private long maxCharactersInDocument;

		// Token: 0x04000646 RID: 1606
		private long maxCharactersFromEntities;

		// Token: 0x04000647 RID: 1607
		private ValidationType validationType;

		// Token: 0x04000648 RID: 1608
		private XmlSchemaValidationFlags validationFlags;

		// Token: 0x04000649 RID: 1609
		private XmlSchemaSet schemas;

		// Token: 0x0400064A RID: 1610
		private ValidationEventHandler valEventHandler;

		// Token: 0x0400064B RID: 1611
		private bool ignoreWhitespace;

		// Token: 0x0400064C RID: 1612
		private bool ignorePIs;

		// Token: 0x0400064D RID: 1613
		private bool ignoreComments;

		// Token: 0x0400064E RID: 1614
		private bool prohibitDtd;

		// Token: 0x0400064F RID: 1615
		private bool closeInput;

		// Token: 0x04000650 RID: 1616
		private bool isReadOnly;

		// Token: 0x04000651 RID: 1617
		private bool isXmlResolverSet;
	}
}
