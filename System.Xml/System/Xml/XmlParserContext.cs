using System;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200007D RID: 125
	public class XmlParserContext
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x00016B0C File Offset: 0x00015B0C
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string xmlLang, XmlSpace xmlSpace) : this(nt, nsMgr, null, null, null, null, string.Empty, xmlLang, xmlSpace)
		{
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00016B30 File Offset: 0x00015B30
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string xmlLang, XmlSpace xmlSpace, Encoding enc) : this(nt, nsMgr, null, null, null, null, string.Empty, xmlLang, xmlSpace, enc)
		{
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00016B54 File Offset: 0x00015B54
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, XmlSpace xmlSpace) : this(nt, nsMgr, docTypeName, pubId, sysId, internalSubset, baseURI, xmlLang, xmlSpace, null)
		{
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00016B78 File Offset: 0x00015B78
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, XmlSpace xmlSpace, Encoding enc)
		{
			if (nsMgr != null)
			{
				if (nt == null)
				{
					this._nt = nsMgr.NameTable;
				}
				else
				{
					if (nt != nsMgr.NameTable)
					{
						throw new XmlException("Xml_NotSameNametable", string.Empty);
					}
					this._nt = nt;
				}
			}
			else
			{
				this._nt = nt;
			}
			this._nsMgr = nsMgr;
			this._docTypeName = ((docTypeName == null) ? string.Empty : docTypeName);
			this._pubId = ((pubId == null) ? string.Empty : pubId);
			this._sysId = ((sysId == null) ? string.Empty : sysId);
			this._internalSubset = ((internalSubset == null) ? string.Empty : internalSubset);
			this._baseURI = ((baseURI == null) ? string.Empty : baseURI);
			this._xmlLang = ((xmlLang == null) ? string.Empty : xmlLang);
			this._xmlSpace = xmlSpace;
			this._encoding = enc;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00016C91 File Offset: 0x00015C91
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00016C99 File Offset: 0x00015C99
		public XmlNameTable NameTable
		{
			get
			{
				return this._nt;
			}
			set
			{
				this._nt = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00016CA2 File Offset: 0x00015CA2
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00016CAA File Offset: 0x00015CAA
		public XmlNamespaceManager NamespaceManager
		{
			get
			{
				return this._nsMgr;
			}
			set
			{
				this._nsMgr = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00016CB3 File Offset: 0x00015CB3
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00016CBB File Offset: 0x00015CBB
		public string DocTypeName
		{
			get
			{
				return this._docTypeName;
			}
			set
			{
				this._docTypeName = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00016CCE File Offset: 0x00015CCE
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00016CD6 File Offset: 0x00015CD6
		public string PublicId
		{
			get
			{
				return this._pubId;
			}
			set
			{
				this._pubId = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00016CE9 File Offset: 0x00015CE9
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00016CF1 File Offset: 0x00015CF1
		public string SystemId
		{
			get
			{
				return this._sysId;
			}
			set
			{
				this._sysId = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00016D04 File Offset: 0x00015D04
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00016D0C File Offset: 0x00015D0C
		public string BaseURI
		{
			get
			{
				return this._baseURI;
			}
			set
			{
				this._baseURI = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00016D1F File Offset: 0x00015D1F
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00016D27 File Offset: 0x00015D27
		public string InternalSubset
		{
			get
			{
				return this._internalSubset;
			}
			set
			{
				this._internalSubset = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00016D3A File Offset: 0x00015D3A
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00016D42 File Offset: 0x00015D42
		public string XmlLang
		{
			get
			{
				return this._xmlLang;
			}
			set
			{
				this._xmlLang = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00016D55 File Offset: 0x00015D55
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00016D5D File Offset: 0x00015D5D
		public XmlSpace XmlSpace
		{
			get
			{
				return this._xmlSpace;
			}
			set
			{
				this._xmlSpace = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00016D66 File Offset: 0x00015D66
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00016D6E File Offset: 0x00015D6E
		public Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
			set
			{
				this._encoding = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00016D77 File Offset: 0x00015D77
		internal bool HasDtdInfo
		{
			get
			{
				return this._internalSubset != string.Empty || this._pubId != string.Empty || this._sysId != string.Empty;
			}
		}

		// Token: 0x04000635 RID: 1589
		private XmlNameTable _nt;

		// Token: 0x04000636 RID: 1590
		private XmlNamespaceManager _nsMgr;

		// Token: 0x04000637 RID: 1591
		private string _docTypeName = string.Empty;

		// Token: 0x04000638 RID: 1592
		private string _pubId = string.Empty;

		// Token: 0x04000639 RID: 1593
		private string _sysId = string.Empty;

		// Token: 0x0400063A RID: 1594
		private string _internalSubset = string.Empty;

		// Token: 0x0400063B RID: 1595
		private string _xmlLang = string.Empty;

		// Token: 0x0400063C RID: 1596
		private XmlSpace _xmlSpace;

		// Token: 0x0400063D RID: 1597
		private string _baseURI = string.Empty;

		// Token: 0x0400063E RID: 1598
		private Encoding _encoding;
	}
}
