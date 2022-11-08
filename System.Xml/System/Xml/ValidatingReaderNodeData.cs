using System;

namespace System.Xml
{
	// Token: 0x02000069 RID: 105
	internal class ValidatingReaderNodeData
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00011DE4 File Offset: 0x00010DE4
		public ValidatingReaderNodeData()
		{
			this.Clear(XmlNodeType.None);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00011DF3 File Offset: 0x00010DF3
		public ValidatingReaderNodeData(XmlNodeType nodeType)
		{
			this.Clear(nodeType);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00011E02 File Offset: 0x00010E02
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x00011E0A File Offset: 0x00010E0A
		public string LocalName
		{
			get
			{
				return this.localName;
			}
			set
			{
				this.localName = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00011E13 File Offset: 0x00010E13
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00011E1B File Offset: 0x00010E1B
		public string Namespace
		{
			get
			{
				return this.namespaceUri;
			}
			set
			{
				this.namespaceUri = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00011E24 File Offset: 0x00010E24
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00011E2C File Offset: 0x00010E2C
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00011E38 File Offset: 0x00010E38
		public string GetAtomizedNameWPrefix(XmlNameTable nameTable)
		{
			if (this.nameWPrefix == null)
			{
				if (this.prefix.Length == 0)
				{
					this.nameWPrefix = this.localName;
				}
				else
				{
					this.nameWPrefix = nameTable.Add(this.prefix + ":" + this.localName);
				}
			}
			return this.nameWPrefix;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00011E90 File Offset: 0x00010E90
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x00011E98 File Offset: 0x00010E98
		public int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00011EA1 File Offset: 0x00010EA1
		// (set) Token: 0x060003BB RID: 955 RVA: 0x00011EA9 File Offset: 0x00010EA9
		public string RawValue
		{
			get
			{
				return this.rawValue;
			}
			set
			{
				this.rawValue = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00011EB2 File Offset: 0x00010EB2
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00011EBA File Offset: 0x00010EBA
		public string OriginalStringValue
		{
			get
			{
				return this.originalStringValue;
			}
			set
			{
				this.originalStringValue = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00011EC3 File Offset: 0x00010EC3
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00011ECB File Offset: 0x00010ECB
		public XmlNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
			set
			{
				this.nodeType = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00011ED4 File Offset: 0x00010ED4
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00011EDC File Offset: 0x00010EDC
		public AttributePSVIInfo AttInfo
		{
			get
			{
				return this.attributePSVIInfo;
			}
			set
			{
				this.attributePSVIInfo = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00011EE5 File Offset: 0x00010EE5
		public int LineNumber
		{
			get
			{
				return this.lineNo;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00011EED File Offset: 0x00010EED
		public int LinePosition
		{
			get
			{
				return this.linePos;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00011EF8 File Offset: 0x00010EF8
		internal void Clear(XmlNodeType nodeType)
		{
			this.nodeType = nodeType;
			this.localName = string.Empty;
			this.prefix = string.Empty;
			this.namespaceUri = string.Empty;
			this.rawValue = string.Empty;
			if (this.attributePSVIInfo != null)
			{
				this.attributePSVIInfo.Reset();
			}
			this.nameWPrefix = null;
			this.lineNo = 0;
			this.linePos = 0;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00011F60 File Offset: 0x00010F60
		internal void ClearName()
		{
			this.localName = string.Empty;
			this.prefix = string.Empty;
			this.namespaceUri = string.Empty;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00011F83 File Offset: 0x00010F83
		internal void SetLineInfo(int lineNo, int linePos)
		{
			this.lineNo = lineNo;
			this.linePos = linePos;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00011F93 File Offset: 0x00010F93
		internal void SetLineInfo(IXmlLineInfo lineInfo)
		{
			if (lineInfo != null)
			{
				this.lineNo = lineInfo.LineNumber;
				this.linePos = lineInfo.LinePosition;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00011FB0 File Offset: 0x00010FB0
		internal void SetItemData(string localName, string prefix, string ns, string value)
		{
			this.localName = localName;
			this.prefix = prefix;
			this.namespaceUri = ns;
			this.rawValue = value;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00011FCF File Offset: 0x00010FCF
		internal void SetItemData(string localName, string prefix, string ns, int depth)
		{
			this.localName = localName;
			this.prefix = prefix;
			this.namespaceUri = ns;
			this.depth = depth;
			this.rawValue = string.Empty;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00011FF9 File Offset: 0x00010FF9
		internal void SetItemData(string value)
		{
			this.SetItemData(value, value);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00012003 File Offset: 0x00011003
		internal void SetItemData(string value, string originalStringValue)
		{
			this.rawValue = value;
			this.originalStringValue = originalStringValue;
		}

		// Token: 0x040005C4 RID: 1476
		private string localName;

		// Token: 0x040005C5 RID: 1477
		private string namespaceUri;

		// Token: 0x040005C6 RID: 1478
		private string prefix;

		// Token: 0x040005C7 RID: 1479
		private string nameWPrefix;

		// Token: 0x040005C8 RID: 1480
		private string rawValue;

		// Token: 0x040005C9 RID: 1481
		private string originalStringValue;

		// Token: 0x040005CA RID: 1482
		private int depth;

		// Token: 0x040005CB RID: 1483
		private AttributePSVIInfo attributePSVIInfo;

		// Token: 0x040005CC RID: 1484
		private XmlNodeType nodeType;

		// Token: 0x040005CD RID: 1485
		private int lineNo;

		// Token: 0x040005CE RID: 1486
		private int linePos;
	}
}
