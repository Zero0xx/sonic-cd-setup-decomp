using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x0200006D RID: 109
	[DebuggerDisplay("{debuggerDisplayProxy}")]
	public abstract class XmlReader : IDisposable
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001257C File Offset: 0x0001157C
		public virtual XmlReaderSettings Settings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003FB RID: 1019
		public abstract XmlNodeType NodeType { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001257F File Offset: 0x0001157F
		public virtual string Name
		{
			get
			{
				if (this.Prefix.Length == 0)
				{
					return this.LocalName;
				}
				return this.NameTable.Add(this.Prefix + ":" + this.LocalName);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003FD RID: 1021
		public abstract string LocalName { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003FE RID: 1022
		public abstract string NamespaceURI { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003FF RID: 1023
		public abstract string Prefix { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000400 RID: 1024
		public abstract bool HasValue { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000401 RID: 1025
		public abstract string Value { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000402 RID: 1026
		public abstract int Depth { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000403 RID: 1027
		public abstract string BaseURI { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000404 RID: 1028
		public abstract bool IsEmptyElement { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000125B6 File Offset: 0x000115B6
		public virtual bool IsDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x000125B9 File Offset: 0x000115B9
		public virtual char QuoteChar
		{
			get
			{
				return '"';
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x000125BD File Offset: 0x000115BD
		public virtual XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.None;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x000125C0 File Offset: 0x000115C0
		public virtual string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x000125C7 File Offset: 0x000115C7
		public virtual IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this as IXmlSchemaInfo;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x000125CF File Offset: 0x000115CF
		public virtual Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000125DB File Offset: 0x000115DB
		public virtual object ReadContentAsObject()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsObject");
			}
			return this.InternalReadContentAsString();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000125F8 File Offset: 0x000115F8
		public virtual bool ReadContentAsBoolean()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsBoolean");
			}
			bool result;
			try
			{
				result = XmlConvert.ToBoolean(this.InternalReadContentAsString());
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Boolean", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00012650 File Offset: 0x00011650
		public virtual DateTime ReadContentAsDateTime()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsDateTime");
			}
			DateTime result;
			try
			{
				result = XmlConvert.ToDateTime(this.InternalReadContentAsString(), XmlDateTimeSerializationMode.RoundtripKind);
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "DateTime", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000126AC File Offset: 0x000116AC
		public virtual double ReadContentAsDouble()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsDouble");
			}
			double result;
			try
			{
				result = XmlConvert.ToDouble(this.InternalReadContentAsString());
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Double", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00012704 File Offset: 0x00011704
		public virtual float ReadContentAsFloat()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsFloat");
			}
			float result;
			try
			{
				result = XmlConvert.ToSingle(this.InternalReadContentAsString());
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Float", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001275C File Offset: 0x0001175C
		public virtual decimal ReadContentAsDecimal()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsDecimal");
			}
			decimal result;
			try
			{
				result = XmlConvert.ToDecimal(this.InternalReadContentAsString());
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Decimal", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000127B4 File Offset: 0x000117B4
		public virtual int ReadContentAsInt()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsInt");
			}
			int result;
			try
			{
				result = XmlConvert.ToInt32(this.InternalReadContentAsString());
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Int", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001280C File Offset: 0x0001180C
		public virtual long ReadContentAsLong()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsLong");
			}
			long result;
			try
			{
				result = XmlConvert.ToInt64(this.InternalReadContentAsString());
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Long", innerException, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00012864 File Offset: 0x00011864
		public virtual string ReadContentAsString()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsString");
			}
			return this.InternalReadContentAsString();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00012880 File Offset: 0x00011880
		public virtual object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAs");
			}
			string text = this.InternalReadContentAsString();
			if (returnType == typeof(string))
			{
				return text;
			}
			object result;
			try
			{
				result = XmlUntypedConverter.Untyped.ChangeType(text, returnType, this as IXmlNamespaceResolver);
			}
			catch (FormatException innerException)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), innerException, this as IXmlLineInfo);
			}
			catch (InvalidCastException innerException2)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), innerException2, this as IXmlLineInfo);
			}
			return result;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001291C File Offset: 0x0001191C
		public virtual object ReadElementContentAsObject()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsObject"))
			{
				object result = this.ReadContentAsObject();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return string.Empty;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001294A File Offset: 0x0001194A
		public virtual object ReadElementContentAsObject(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsObject();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001295C File Offset: 0x0001195C
		public virtual bool ReadElementContentAsBoolean()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsBoolean"))
			{
				bool result = this.ReadContentAsBoolean();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToBoolean(string.Empty);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001298F File Offset: 0x0001198F
		public virtual bool ReadElementContentAsBoolean(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsBoolean();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000129A0 File Offset: 0x000119A0
		public virtual DateTime ReadElementContentAsDateTime()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsDateTime"))
			{
				DateTime result = this.ReadContentAsDateTime();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToDateTime(string.Empty, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000129D4 File Offset: 0x000119D4
		public virtual DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsDateTime();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000129E4 File Offset: 0x000119E4
		public virtual double ReadElementContentAsDouble()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsDouble"))
			{
				double result = this.ReadContentAsDouble();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToDouble(string.Empty);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00012A17 File Offset: 0x00011A17
		public virtual double ReadElementContentAsDouble(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsDouble();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00012A28 File Offset: 0x00011A28
		public virtual float ReadElementContentAsFloat()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsFloat"))
			{
				float result = this.ReadContentAsFloat();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToSingle(string.Empty);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00012A5B File Offset: 0x00011A5B
		public virtual float ReadElementContentAsFloat(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsFloat();
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00012A6C File Offset: 0x00011A6C
		public virtual decimal ReadElementContentAsDecimal()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsDecimal"))
			{
				decimal result = this.ReadContentAsDecimal();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToDecimal(string.Empty);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00012A9F File Offset: 0x00011A9F
		public virtual decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsDecimal();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00012AB0 File Offset: 0x00011AB0
		public virtual int ReadElementContentAsInt()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsInt"))
			{
				int result = this.ReadContentAsInt();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToInt32(string.Empty);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00012AE3 File Offset: 0x00011AE3
		public virtual int ReadElementContentAsInt(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsInt();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00012AF4 File Offset: 0x00011AF4
		public virtual long ReadElementContentAsLong()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsLong"))
			{
				long result = this.ReadContentAsLong();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return XmlConvert.ToInt64(string.Empty);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00012B27 File Offset: 0x00011B27
		public virtual long ReadElementContentAsLong(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsLong();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00012B38 File Offset: 0x00011B38
		public virtual string ReadElementContentAsString()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsString"))
			{
				string result = this.ReadContentAsString();
				this.FinishReadElementContentAsXxx();
				return result;
			}
			return string.Empty;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00012B66 File Offset: 0x00011B66
		public virtual string ReadElementContentAsString(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsString();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00012B78 File Offset: 0x00011B78
		public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAs"))
			{
				object result = this.ReadContentAs(returnType, namespaceResolver);
				this.FinishReadElementContentAsXxx();
				return result;
			}
			if (returnType != typeof(string))
			{
				return XmlUntypedConverter.Untyped.ChangeType(string.Empty, returnType, namespaceResolver);
			}
			return string.Empty;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00012BC7 File Offset: 0x00011BC7
		public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAs(returnType, namespaceResolver);
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000429 RID: 1065
		public abstract int AttributeCount { get; }

		// Token: 0x0600042A RID: 1066
		public abstract string GetAttribute(string name);

		// Token: 0x0600042B RID: 1067
		public abstract string GetAttribute(string name, string namespaceURI);

		// Token: 0x0600042C RID: 1068
		public abstract string GetAttribute(int i);

		// Token: 0x1700007B RID: 123
		public virtual string this[int i]
		{
			get
			{
				return this.GetAttribute(i);
			}
		}

		// Token: 0x1700007C RID: 124
		public virtual string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}

		// Token: 0x1700007D RID: 125
		public virtual string this[string name, string namespaceURI]
		{
			get
			{
				return this.GetAttribute(name, namespaceURI);
			}
		}

		// Token: 0x06000430 RID: 1072
		public abstract bool MoveToAttribute(string name);

		// Token: 0x06000431 RID: 1073
		public abstract bool MoveToAttribute(string name, string ns);

		// Token: 0x06000432 RID: 1074 RVA: 0x00012BF8 File Offset: 0x00011BF8
		public virtual void MoveToAttribute(int i)
		{
			if (i < 0 || i >= this.AttributeCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			this.MoveToElement();
			this.MoveToFirstAttribute();
			for (int j = 0; j < i; j++)
			{
				this.MoveToNextAttribute();
			}
		}

		// Token: 0x06000433 RID: 1075
		public abstract bool MoveToFirstAttribute();

		// Token: 0x06000434 RID: 1076
		public abstract bool MoveToNextAttribute();

		// Token: 0x06000435 RID: 1077
		public abstract bool MoveToElement();

		// Token: 0x06000436 RID: 1078
		public abstract bool ReadAttributeValue();

		// Token: 0x06000437 RID: 1079
		public abstract bool Read();

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000438 RID: 1080
		public abstract bool EOF { get; }

		// Token: 0x06000439 RID: 1081
		public abstract void Close();

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600043A RID: 1082
		public abstract ReadState ReadState { get; }

		// Token: 0x0600043B RID: 1083 RVA: 0x00012C3E File Offset: 0x00011C3E
		public virtual void Skip()
		{
			this.SkipSubtree();
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600043C RID: 1084
		public abstract XmlNameTable NameTable { get; }

		// Token: 0x0600043D RID: 1085
		public abstract string LookupNamespace(string prefix);

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00012C46 File Offset: 0x00011C46
		public virtual bool CanResolveEntity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600043F RID: 1087
		public abstract void ResolveEntity();

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00012C49 File Offset: 0x00011C49
		public virtual bool CanReadBinaryContent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00012C4C File Offset: 0x00011C4C
		public virtual int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[]
			{
				"ReadContentAsBase64"
			}));
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00012C78 File Offset: 0x00011C78
		public virtual int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[]
			{
				"ReadElementContentAsBase64"
			}));
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00012CA4 File Offset: 0x00011CA4
		public virtual int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[]
			{
				"ReadContentAsBinHex"
			}));
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00012CD0 File Offset: 0x00011CD0
		public virtual int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[]
			{
				"ReadElementContentAsBinHex"
			}));
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00012CFC File Offset: 0x00011CFC
		public virtual bool CanReadValueChunk
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00012CFF File Offset: 0x00011CFF
		public virtual int ReadValueChunk(char[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadValueChunkNotSupported"));
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00012D10 File Offset: 0x00011D10
		public virtual string ReadString()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			this.MoveToElement();
			if (this.NodeType == XmlNodeType.Element)
			{
				if (this.IsEmptyElement)
				{
					return string.Empty;
				}
				if (!this.Read())
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				if (this.NodeType == XmlNodeType.EndElement)
				{
					return string.Empty;
				}
			}
			string text = string.Empty;
			while (XmlReader.IsTextualNode(this.NodeType))
			{
				text += this.Value;
				if (!this.Read())
				{
					break;
				}
			}
			return text;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00012DA0 File Offset: 0x00011DA0
		public virtual XmlNodeType MoveToContent()
		{
			for (;;)
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.EntityReference:
					goto IL_3D;
				case XmlNodeType.Attribute:
					goto IL_36;
				default:
					switch (nodeType)
					{
					case XmlNodeType.EndElement:
					case XmlNodeType.EndEntity:
						goto IL_3D;
					default:
						if (!this.Read())
						{
							goto Block_2;
						}
						break;
					}
					break;
				}
			}
			IL_36:
			this.MoveToElement();
			IL_3D:
			return this.NodeType;
			Block_2:
			return this.NodeType;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00012DFF File Offset: 0x00011DFF
		public virtual void ReadStartElement()
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			this.Read();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00012E34 File Offset: 0x00011E34
		public virtual void ReadStartElement(string name)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.Name == name)
			{
				this.Read();
				return;
			}
			throw new XmlException("Xml_ElementNotFound", name, this as IXmlLineInfo);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00012E94 File Offset: 0x00011E94
		public virtual void ReadStartElement(string localname, string ns)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.LocalName == localname && this.NamespaceURI == ns)
			{
				this.Read();
				return;
			}
			throw new XmlException("Xml_ElementNotFoundNs", new string[]
			{
				localname,
				ns
			}, this as IXmlLineInfo);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00012F10 File Offset: 0x00011F10
		public virtual string ReadElementString()
		{
			string result = string.Empty;
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (!this.IsEmptyElement)
			{
				this.Read();
				result = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("Xml_UnexpectedNodeInSimpleContent", new string[]
					{
						this.NodeType.ToString(),
						"ReadElementString"
					}, this as IXmlLineInfo);
				}
				this.Read();
			}
			else
			{
				this.Read();
			}
			return result;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00012FB4 File Offset: 0x00011FB4
		public virtual string ReadElementString(string name)
		{
			string result = string.Empty;
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.Name != name)
			{
				throw new XmlException("Xml_ElementNotFound", name, this as IXmlLineInfo);
			}
			if (!this.IsEmptyElement)
			{
				result = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
				}
				this.Read();
			}
			else
			{
				this.Read();
			}
			return result;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001305C File Offset: 0x0001205C
		public virtual string ReadElementString(string localname, string ns)
		{
			string result = string.Empty;
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.LocalName != localname || this.NamespaceURI != ns)
			{
				throw new XmlException("Xml_ElementNotFoundNs", new string[]
				{
					localname,
					ns
				}, this as IXmlLineInfo);
			}
			if (!this.IsEmptyElement)
			{
				result = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
				}
				this.Read();
			}
			else
			{
				this.Read();
			}
			return result;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00013121 File Offset: 0x00012121
		public virtual void ReadEndElement()
		{
			if (this.MoveToContent() != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			this.Read();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00013155 File Offset: 0x00012155
		public virtual bool IsStartElement()
		{
			return this.MoveToContent() == XmlNodeType.Element;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00013160 File Offset: 0x00012160
		public virtual bool IsStartElement(string name)
		{
			return this.MoveToContent() == XmlNodeType.Element && this.Name == name;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00013179 File Offset: 0x00012179
		public virtual bool IsStartElement(string localname, string ns)
		{
			return this.MoveToContent() == XmlNodeType.Element && this.LocalName == localname && this.NamespaceURI == ns;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000131A4 File Offset: 0x000121A4
		public virtual bool ReadToFollowing(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(name, "name");
			}
			name = this.NameTable.Add(name);
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(name, this.Name))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000131FC File Offset: 0x000121FC
		public virtual bool ReadToFollowing(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			localName = this.NameTable.Add(localName);
			namespaceURI = this.NameTable.Add(namespaceURI);
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(localName, this.LocalName) && Ref.Equal(namespaceURI, this.NamespaceURI))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001327C File Offset: 0x0001227C
		public virtual bool ReadToDescendant(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(name, "name");
			}
			int num = this.Depth;
			if (this.NodeType != XmlNodeType.Element)
			{
				if (this.ReadState != ReadState.Initial)
				{
					return false;
				}
				num--;
			}
			else if (this.IsEmptyElement)
			{
				return false;
			}
			name = this.NameTable.Add(name);
			while (this.Read() && this.Depth > num)
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(name, this.Name))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00013308 File Offset: 0x00012308
		public virtual bool ReadToDescendant(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			int num = this.Depth;
			if (this.NodeType != XmlNodeType.Element)
			{
				if (this.ReadState != ReadState.Initial)
				{
					return false;
				}
				num--;
			}
			else if (this.IsEmptyElement)
			{
				return false;
			}
			localName = this.NameTable.Add(localName);
			namespaceURI = this.NameTable.Add(namespaceURI);
			while (this.Read() && this.Depth > num)
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(localName, this.LocalName) && Ref.Equal(namespaceURI, this.NamespaceURI))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000133BC File Offset: 0x000123BC
		public virtual bool ReadToNextSibling(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(name, "name");
			}
			name = this.NameTable.Add(name);
			for (;;)
			{
				this.SkipSubtree();
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element && Ref.Equal(name, this.Name))
				{
					break;
				}
				if (nodeType == XmlNodeType.EndElement || this.EOF)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00013420 File Offset: 0x00012420
		public virtual bool ReadToNextSibling(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			localName = this.NameTable.Add(localName);
			namespaceURI = this.NameTable.Add(namespaceURI);
			for (;;)
			{
				this.SkipSubtree();
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element && Ref.Equal(localName, this.LocalName) && Ref.Equal(namespaceURI, this.NamespaceURI))
				{
					break;
				}
				if (nodeType == XmlNodeType.EndElement || this.EOF)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000134AC File Offset: 0x000124AC
		public static bool IsName(string str)
		{
			return XmlCharType.Instance.IsName(str);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000134C8 File Offset: 0x000124C8
		public static bool IsNameToken(string str)
		{
			return XmlCharType.Instance.IsNmToken(str);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000134E4 File Offset: 0x000124E4
		public virtual string ReadInnerXml()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			if (this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Element)
			{
				this.Read();
				return string.Empty;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			try
			{
				this.SetNamespacesFlag(xmlTextWriter);
				if (this.NodeType == XmlNodeType.Attribute)
				{
					xmlTextWriter.QuoteChar = this.QuoteChar;
					this.WriteAttributeValue(xmlTextWriter);
				}
				if (this.NodeType == XmlNodeType.Element)
				{
					this.WriteNode(xmlTextWriter, false);
				}
			}
			finally
			{
				xmlTextWriter.Close();
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00013584 File Offset: 0x00012584
		private void WriteNode(XmlTextWriter xtw, bool defattr)
		{
			int num = (this.NodeType == XmlNodeType.None) ? -1 : this.Depth;
			while (this.Read() && num < this.Depth)
			{
				switch (this.NodeType)
				{
				case XmlNodeType.Element:
					xtw.WriteStartElement(this.Prefix, this.LocalName, this.NamespaceURI);
					xtw.QuoteChar = this.QuoteChar;
					xtw.WriteAttributes(this, defattr);
					if (this.IsEmptyElement)
					{
						xtw.WriteEndElement();
					}
					break;
				case XmlNodeType.Text:
					xtw.WriteString(this.Value);
					break;
				case XmlNodeType.CDATA:
					xtw.WriteCData(this.Value);
					break;
				case XmlNodeType.EntityReference:
					xtw.WriteEntityRef(this.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					xtw.WriteProcessingInstruction(this.Name, this.Value);
					break;
				case XmlNodeType.Comment:
					xtw.WriteComment(this.Value);
					break;
				case XmlNodeType.DocumentType:
					xtw.WriteDocType(this.Name, this.GetAttribute("PUBLIC"), this.GetAttribute("SYSTEM"), this.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					xtw.WriteWhitespace(this.Value);
					break;
				case XmlNodeType.EndElement:
					xtw.WriteFullEndElement();
					break;
				}
			}
			if (num == this.Depth && this.NodeType == XmlNodeType.EndElement)
			{
				this.Read();
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000136FC File Offset: 0x000126FC
		private void WriteAttributeValue(XmlTextWriter xtw)
		{
			string name = this.Name;
			while (this.ReadAttributeValue())
			{
				if (this.NodeType == XmlNodeType.EntityReference)
				{
					xtw.WriteEntityRef(this.Name);
				}
				else
				{
					xtw.WriteString(this.Value);
				}
			}
			this.MoveToAttribute(name);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00013748 File Offset: 0x00012748
		public virtual string ReadOuterXml()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			if (this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Element)
			{
				this.Read();
				return string.Empty;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			try
			{
				this.SetNamespacesFlag(xmlTextWriter);
				if (this.NodeType == XmlNodeType.Attribute)
				{
					xmlTextWriter.WriteStartAttribute(this.Prefix, this.LocalName, this.NamespaceURI);
					this.WriteAttributeValue(xmlTextWriter);
					xmlTextWriter.WriteEndAttribute();
				}
				else
				{
					xmlTextWriter.WriteNode(this, false);
				}
			}
			finally
			{
				xmlTextWriter.Close();
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000137F4 File Offset: 0x000127F4
		private void SetNamespacesFlag(XmlTextWriter xtw)
		{
			XmlTextReader xmlTextReader = this as XmlTextReader;
			if (xmlTextReader != null)
			{
				xtw.Namespaces = xmlTextReader.Namespaces;
				return;
			}
			XmlValidatingReader xmlValidatingReader = this as XmlValidatingReader;
			if (xmlValidatingReader != null)
			{
				xtw.Namespaces = xmlValidatingReader.Namespaces;
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001382E File Offset: 0x0001282E
		public virtual XmlReader ReadSubtree()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new InvalidOperationException(Res.GetString("Xml_ReadSubtreeNotOnElement"));
			}
			return new XmlSubtreeReader(this);
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0001384F File Offset: 0x0001284F
		public virtual bool HasAttributes
		{
			get
			{
				return this.AttributeCount > 0;
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001385A File Offset: 0x0001285A
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00013863 File Offset: 0x00012863
		protected virtual void Dispose(bool disposing)
		{
			if (this.ReadState != ReadState.Closed)
			{
				this.Close();
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00013874 File Offset: 0x00012874
		internal virtual XmlNamespaceManager NamespaceManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00013877 File Offset: 0x00012877
		internal static bool IsTextualNode(XmlNodeType nodeType)
		{
			return 0UL != ((ulong)XmlReader.IsTextualNodeBitmap & (ulong)(1L << (int)(nodeType & (XmlNodeType)31)));
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001388E File Offset: 0x0001288E
		internal static bool CanReadContentAs(XmlNodeType nodeType)
		{
			return 0UL != ((ulong)XmlReader.CanReadContentAsBitmap & (ulong)(1L << (int)(nodeType & (XmlNodeType)31)));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000138A5 File Offset: 0x000128A5
		internal static bool HasValueInternal(XmlNodeType nodeType)
		{
			return 0UL != ((ulong)XmlReader.HasValueBitmap & (ulong)(1L << (int)(nodeType & (XmlNodeType)31)));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000138BC File Offset: 0x000128BC
		private void SkipSubtree()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return;
			}
			this.MoveToElement();
			if (this.NodeType == XmlNodeType.Element && !this.IsEmptyElement)
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
				if (this.NodeType == XmlNodeType.EndElement)
				{
					this.Read();
					return;
				}
			}
			else
			{
				this.Read();
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001391C File Offset: 0x0001291C
		internal void CheckElement(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.LocalName != localName || this.NamespaceURI != namespaceURI)
			{
				throw new XmlException("Xml_ElementNotFoundNs", new string[]
				{
					localName,
					namespaceURI
				}, this as IXmlLineInfo);
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000139B5 File Offset: 0x000129B5
		internal Exception CreateReadContentAsException(string methodName)
		{
			return XmlReader.CreateReadContentAsException(methodName, this.NodeType, this as IXmlLineInfo);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000139C9 File Offset: 0x000129C9
		internal Exception CreateReadElementContentAsException(string methodName)
		{
			return XmlReader.CreateReadElementContentAsException(methodName, this.NodeType, this as IXmlLineInfo);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000139DD File Offset: 0x000129DD
		internal bool CanReadContentAs()
		{
			return XmlReader.CanReadContentAs(this.NodeType);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000139EC File Offset: 0x000129EC
		internal static Exception CreateReadContentAsException(string methodName, XmlNodeType nodeType, IXmlLineInfo lineInfo)
		{
			return new InvalidOperationException(XmlReader.AddLineInfo(Res.GetString("Xml_InvalidReadContentAs", new string[]
			{
				methodName,
				nodeType.ToString()
			}), lineInfo));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00013A28 File Offset: 0x00012A28
		internal static Exception CreateReadElementContentAsException(string methodName, XmlNodeType nodeType, IXmlLineInfo lineInfo)
		{
			return new InvalidOperationException(XmlReader.AddLineInfo(Res.GetString("Xml_InvalidReadElementContentAs", new string[]
			{
				methodName,
				nodeType.ToString()
			}), lineInfo));
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00013A64 File Offset: 0x00012A64
		private static string AddLineInfo(string message, IXmlLineInfo lineInfo)
		{
			if (lineInfo != null)
			{
				message = message + " " + Res.GetString("Xml_ErrorPosition", new string[]
				{
					lineInfo.LineNumber.ToString(CultureInfo.InvariantCulture),
					lineInfo.LinePosition.ToString(CultureInfo.InvariantCulture)
				});
			}
			return message;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00013AC0 File Offset: 0x00012AC0
		internal string InternalReadContentAsString()
		{
			string text = string.Empty;
			BufferBuilder bufferBuilder = null;
			do
			{
				switch (this.NodeType)
				{
				case XmlNodeType.Attribute:
					goto IL_55;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if (text.Length == 0)
					{
						text = this.Value;
						goto IL_99;
					}
					if (bufferBuilder == null)
					{
						bufferBuilder = new BufferBuilder();
						bufferBuilder.Append(text);
					}
					bufferBuilder.Append(this.Value);
					goto IL_99;
				case XmlNodeType.EntityReference:
					if (this.CanResolveEntity)
					{
						this.ResolveEntity();
						goto IL_99;
					}
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					goto IL_99;
				}
				break;
				IL_99:;
			}
			while ((this.AttributeCount != 0) ? this.ReadAttributeValue() : this.Read());
			goto IL_B4;
			IL_55:
			return this.Value;
			IL_B4:
			if (bufferBuilder != null)
			{
				return bufferBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00013B8C File Offset: 0x00012B8C
		private bool SetupReadElementContentAsXxx(string methodName)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw this.CreateReadElementContentAsException(methodName);
			}
			bool isEmptyElement = this.IsEmptyElement;
			this.Read();
			if (isEmptyElement)
			{
				return false;
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.EndElement)
			{
				this.Read();
				return false;
			}
			if (nodeType == XmlNodeType.Element)
			{
				throw new XmlException("Xml_MixedReadElementContentAs", string.Empty, this as IXmlLineInfo);
			}
			return true;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00013BED File Offset: 0x00012BED
		private void FinishReadElementContentAsXxx()
		{
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString());
			}
			this.Read();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00013C1C File Offset: 0x00012C1C
		internal static SchemaInfo GetDtdSchemaInfo(XmlReader reader)
		{
			XmlWrappingReader xmlWrappingReader = reader as XmlWrappingReader;
			if (xmlWrappingReader != null)
			{
				return xmlWrappingReader.DtdSchemaInfo;
			}
			XmlTextReaderImpl xmlTextReaderImpl = XmlReader.GetXmlTextReaderImpl(reader);
			if (xmlTextReaderImpl == null)
			{
				return null;
			}
			return xmlTextReaderImpl.DtdSchemaInfo;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00013C4C File Offset: 0x00012C4C
		internal static Encoding GetEncoding(XmlReader reader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = XmlReader.GetXmlTextReaderImpl(reader);
			if (xmlTextReaderImpl == null)
			{
				return null;
			}
			return xmlTextReaderImpl.Encoding;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00013C6C File Offset: 0x00012C6C
		internal static ConformanceLevel GetV1ConformanceLevel(XmlReader reader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = XmlReader.GetXmlTextReaderImpl(reader);
			if (xmlTextReaderImpl == null)
			{
				return ConformanceLevel.Document;
			}
			return xmlTextReaderImpl.V1ComformanceLevel;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00013C8C File Offset: 0x00012C8C
		private static XmlTextReaderImpl GetXmlTextReaderImpl(XmlReader reader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = reader as XmlTextReaderImpl;
			if (xmlTextReaderImpl != null)
			{
				return xmlTextReaderImpl;
			}
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader != null)
			{
				return xmlTextReader.Impl;
			}
			XmlValidatingReaderImpl xmlValidatingReaderImpl = reader as XmlValidatingReaderImpl;
			if (xmlValidatingReaderImpl != null)
			{
				return xmlValidatingReaderImpl.ReaderImpl;
			}
			XmlValidatingReader xmlValidatingReader = reader as XmlValidatingReader;
			if (xmlValidatingReader != null)
			{
				return xmlValidatingReader.Impl.ReaderImpl;
			}
			return null;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00013CDE File Offset: 0x00012CDE
		public static XmlReader Create(string inputUri)
		{
			return XmlReader.Create(inputUri, null, null);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00013CE8 File Offset: 0x00012CE8
		public static XmlReader Create(string inputUri, XmlReaderSettings settings)
		{
			return XmlReader.Create(inputUri, settings, null);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00013CF4 File Offset: 0x00012CF4
		public static XmlReader Create(string inputUri, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			if (inputUri == null)
			{
				throw new ArgumentNullException("inputUri");
			}
			if (inputUri.Length == 0)
			{
				throw new ArgumentException(Res.GetString("XmlConvert_BadUri"), "inputUri");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			XmlResolver xmlResolver = settings.GetXmlResolver();
			if (xmlResolver == null)
			{
				xmlResolver = new XmlUrlResolver();
			}
			Uri uri = xmlResolver.ResolveUri(null, inputUri);
			Stream stream = (Stream)xmlResolver.GetEntity(uri, string.Empty, typeof(Stream));
			if (stream == null)
			{
				throw new XmlException("Xml_CannotResolveUrl", inputUri);
			}
			XmlReader result;
			try
			{
				result = XmlReader.CreateReaderImpl(stream, settings, uri, uri.ToString(), inputContext, true);
			}
			catch
			{
				stream.Close();
				throw;
			}
			return result;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00013DA8 File Offset: 0x00012DA8
		public static XmlReader Create(Stream input)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			return XmlReader.CreateReaderImpl(input, xmlReaderSettings, null, string.Empty, null, xmlReaderSettings.CloseInput);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00013DCF File Offset: 0x00012DCF
		public static XmlReader Create(Stream input, XmlReaderSettings settings)
		{
			return XmlReader.Create(input, settings, string.Empty);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00013DDD File Offset: 0x00012DDD
		public static XmlReader Create(Stream input, XmlReaderSettings settings, string baseUri)
		{
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			return XmlReader.CreateReaderImpl(input, settings, null, baseUri, null, settings.CloseInput);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00013DF9 File Offset: 0x00012DF9
		public static XmlReader Create(Stream input, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			return XmlReader.CreateReaderImpl(input, settings, null, string.Empty, inputContext, settings.CloseInput);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00013E19 File Offset: 0x00012E19
		public static XmlReader Create(TextReader input)
		{
			return XmlReader.CreateReaderImpl(input, null, string.Empty, null);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00013E28 File Offset: 0x00012E28
		public static XmlReader Create(TextReader input, XmlReaderSettings settings)
		{
			return XmlReader.Create(input, settings, string.Empty);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00013E36 File Offset: 0x00012E36
		public static XmlReader Create(TextReader input, XmlReaderSettings settings, string baseUri)
		{
			return XmlReader.CreateReaderImpl(input, settings, baseUri, null);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00013E41 File Offset: 0x00012E41
		public static XmlReader Create(TextReader input, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			return XmlReader.CreateReaderImpl(input, settings, string.Empty, inputContext);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00013E50 File Offset: 0x00012E50
		public static XmlReader Create(XmlReader reader, XmlReaderSettings settings)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			return XmlReader.CreateReaderImpl(reader, settings);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00013E74 File Offset: 0x00012E74
		internal static XmlReader CreateSqlReader(Stream input, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			byte[] array = new byte[XmlReader.CalcBufferSize(input)];
			int num = 0;
			int num2;
			do
			{
				num2 = input.Read(array, num, array.Length - num);
				num += num2;
			}
			while (num2 > 0 && num < 2);
			XmlReader xmlReader;
			if (num >= 2 && array[0] == 223 && array[1] == 255)
			{
				if (inputContext != null)
				{
					throw new ArgumentException(Res.GetString("XmlBinary_NoParserContext"), "inputContext");
				}
				xmlReader = new XmlSqlBinaryReader(input, array, num, string.Empty, settings.CloseInput, settings);
			}
			else
			{
				xmlReader = new XmlTextReaderImpl(input, array, num, settings, null, string.Empty, inputContext, settings.CloseInput);
			}
			if (settings.ValidationType != ValidationType.None)
			{
				xmlReader = XmlReader.AddValidation(xmlReader, settings);
			}
			return xmlReader;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00013F30 File Offset: 0x00012F30
		private static XmlReader CreateReaderImpl(Stream input, XmlReaderSettings settings, Uri baseUri, string baseUriStr, XmlParserContext inputContext, bool closeInput)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (baseUriStr == null)
			{
				baseUriStr = string.Empty;
			}
			XmlReader xmlReader = new XmlTextReaderImpl(input, null, 0, settings, baseUri, baseUriStr, inputContext, closeInput);
			if (settings.ValidationType != ValidationType.None)
			{
				xmlReader = XmlReader.AddValidation(xmlReader, settings);
			}
			return xmlReader;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00013F76 File Offset: 0x00012F76
		private static XmlReader AddValidation(XmlReader reader, XmlReaderSettings settings)
		{
			if (settings.ValidationType == ValidationType.Schema)
			{
				reader = new XsdValidatingReader(reader, settings.GetXmlResolver_CheckConfig(), settings);
			}
			else if (settings.ValidationType == ValidationType.DTD)
			{
				reader = XmlReader.CreateDtdValidatingReader(reader, settings);
			}
			return reader;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00013FA8 File Offset: 0x00012FA8
		internal static int CalcBufferSize(Stream input)
		{
			int num = 4096;
			if (input.CanSeek)
			{
				long length = input.Length;
				if (length < (long)num)
				{
					num = checked((int)length);
				}
				else if (length > 65536L)
				{
					num = 8192;
				}
			}
			return num;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00013FE4 File Offset: 0x00012FE4
		private static XmlReader CreateReaderImpl(TextReader input, XmlReaderSettings settings, string baseUriStr, XmlParserContext context)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			if (baseUriStr == null)
			{
				baseUriStr = string.Empty;
			}
			XmlReader xmlReader = new XmlTextReaderImpl(input, settings, baseUriStr, context);
			if (settings.ValidationType == ValidationType.Schema)
			{
				xmlReader = new XsdValidatingReader(xmlReader, settings.GetXmlResolver_CheckConfig(), settings);
			}
			else if (settings.ValidationType == ValidationType.DTD)
			{
				xmlReader = XmlReader.CreateDtdValidatingReader(xmlReader, settings);
			}
			return xmlReader;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00014048 File Offset: 0x00013048
		private static XmlReader CreateReaderImpl(XmlReader baseReader, XmlReaderSettings settings)
		{
			XmlReader xmlReader = baseReader;
			if (settings.ValidationType == ValidationType.DTD)
			{
				xmlReader = XmlReader.CreateDtdValidatingReader(xmlReader, settings);
			}
			xmlReader = XmlReader.AddWrapper(xmlReader, settings, xmlReader.Settings);
			if (settings.ValidationType == ValidationType.Schema)
			{
				xmlReader = new XsdValidatingReader(xmlReader, settings.GetXmlResolver_CheckConfig(), settings);
			}
			return xmlReader;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001408E File Offset: 0x0001308E
		private static XmlValidatingReaderImpl CreateDtdValidatingReader(XmlReader baseReader, XmlReaderSettings settings)
		{
			return new XmlValidatingReaderImpl(baseReader, settings.GetEventHandler(), (settings.ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) != XmlSchemaValidationFlags.None);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000140AC File Offset: 0x000130AC
		private static XmlReader AddWrapper(XmlReader baseReader, XmlReaderSettings settings, XmlReaderSettings baseReaderSettings)
		{
			bool checkCharacters = false;
			bool ignoreWhitespace = false;
			bool ignoreComments = false;
			bool ignorePis = false;
			bool flag = false;
			bool prohibitDtd = false;
			if (baseReaderSettings == null)
			{
				if (settings.ConformanceLevel != ConformanceLevel.Auto && settings.ConformanceLevel != XmlReader.GetV1ConformanceLevel(baseReader))
				{
					throw new InvalidOperationException(Res.GetString("Xml_IncompatibleConformanceLevel", new object[]
					{
						settings.ConformanceLevel.ToString()
					}));
				}
				if (settings.IgnoreWhitespace)
				{
					WhitespaceHandling whitespaceHandling = WhitespaceHandling.All;
					XmlTextReader xmlTextReader = baseReader as XmlTextReader;
					if (xmlTextReader != null)
					{
						whitespaceHandling = xmlTextReader.WhitespaceHandling;
					}
					else
					{
						XmlValidatingReader xmlValidatingReader = baseReader as XmlValidatingReader;
						if (xmlValidatingReader != null)
						{
							whitespaceHandling = ((XmlTextReader)xmlValidatingReader.Reader).WhitespaceHandling;
						}
					}
					if (whitespaceHandling == WhitespaceHandling.All)
					{
						ignoreWhitespace = true;
						flag = true;
					}
				}
				if (settings.IgnoreComments)
				{
					ignoreComments = true;
					flag = true;
				}
				if (settings.IgnoreProcessingInstructions)
				{
					ignorePis = true;
					flag = true;
				}
				if (settings.ProhibitDtd)
				{
					XmlTextReader xmlTextReader2 = baseReader as XmlTextReader;
					if (xmlTextReader2 == null)
					{
						XmlValidatingReader xmlValidatingReader2 = baseReader as XmlValidatingReader;
						if (xmlValidatingReader2 != null)
						{
							xmlTextReader2 = (XmlTextReader)xmlValidatingReader2.Reader;
						}
					}
					if (xmlTextReader2 == null || !xmlTextReader2.ProhibitDtd)
					{
						prohibitDtd = true;
						flag = true;
					}
				}
			}
			else
			{
				if (settings.ConformanceLevel != baseReaderSettings.ConformanceLevel && settings.ConformanceLevel != ConformanceLevel.Auto)
				{
					throw new InvalidOperationException(Res.GetString("Xml_IncompatibleConformanceLevel", new object[]
					{
						settings.ConformanceLevel.ToString()
					}));
				}
				if (settings.CheckCharacters && !baseReaderSettings.CheckCharacters)
				{
					checkCharacters = true;
					flag = true;
				}
				if (settings.IgnoreWhitespace && !baseReaderSettings.IgnoreWhitespace)
				{
					ignoreWhitespace = true;
					flag = true;
				}
				if (settings.IgnoreComments && !baseReaderSettings.IgnoreComments)
				{
					ignoreComments = true;
					flag = true;
				}
				if (settings.IgnoreProcessingInstructions && !baseReaderSettings.IgnoreProcessingInstructions)
				{
					ignorePis = true;
					flag = true;
				}
				if (settings.ProhibitDtd && !baseReaderSettings.ProhibitDtd)
				{
					prohibitDtd = true;
					flag = true;
				}
			}
			if (!flag)
			{
				return baseReader;
			}
			IXmlNamespaceResolver xmlNamespaceResolver = baseReader as IXmlNamespaceResolver;
			if (xmlNamespaceResolver != null)
			{
				return new XmlCharCheckingReaderWithNS(baseReader, xmlNamespaceResolver, checkCharacters, ignoreWhitespace, ignoreComments, ignorePis, prohibitDtd);
			}
			return new XmlCharCheckingReader(baseReader, checkCharacters, ignoreWhitespace, ignoreComments, ignorePis, prohibitDtd);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001429C File Offset: 0x0001329C
		private object debuggerDisplayProxy
		{
			get
			{
				return new XmlReader.DebuggerDisplayProxy(this);
			}
		}

		// Token: 0x040005DF RID: 1503
		internal const int DefaultBufferSize = 4096;

		// Token: 0x040005E0 RID: 1504
		internal const int BiggerBufferSize = 8192;

		// Token: 0x040005E1 RID: 1505
		internal const int MaxStreamLengthForDefaultBufferSize = 65536;

		// Token: 0x040005E2 RID: 1506
		private static uint IsTextualNodeBitmap = 24600U;

		// Token: 0x040005E3 RID: 1507
		private static uint CanReadContentAsBitmap = 123324U;

		// Token: 0x040005E4 RID: 1508
		private static uint HasValueBitmap = 157084U;

		// Token: 0x0200006E RID: 110
		[DebuggerDisplay("{ToString()}")]
		private struct DebuggerDisplayProxy
		{
			// Token: 0x0600048E RID: 1166 RVA: 0x000142D1 File Offset: 0x000132D1
			internal DebuggerDisplayProxy(XmlReader reader)
			{
				this.reader = reader;
			}

			// Token: 0x0600048F RID: 1167 RVA: 0x000142DC File Offset: 0x000132DC
			public override string ToString()
			{
				XmlNodeType nodeType = this.reader.NodeType;
				string text = nodeType.ToString();
				switch (nodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.EntityReference:
				case XmlNodeType.EndElement:
				case XmlNodeType.EndEntity:
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						", Name=\"",
						this.reader.Name,
						'"'
					});
					break;
				}
				case XmlNodeType.Attribute:
				case XmlNodeType.ProcessingInstruction:
				{
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						", Name=\"",
						this.reader.Name,
						"\", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.reader.Value),
						'"'
					});
					break;
				}
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Comment:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
				case XmlNodeType.XmlDeclaration:
				{
					object obj3 = text;
					text = string.Concat(new object[]
					{
						obj3,
						", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.reader.Value),
						'"'
					});
					break;
				}
				case XmlNodeType.DocumentType:
				{
					text = text + ", Name=\"" + this.reader.Name + "'";
					object obj4 = text;
					text = string.Concat(new object[]
					{
						obj4,
						", SYSTEM=\"",
						this.reader.GetAttribute("SYSTEM"),
						'"'
					});
					object obj5 = text;
					text = string.Concat(new object[]
					{
						obj5,
						", PUBLIC=\"",
						this.reader.GetAttribute("PUBLIC"),
						'"'
					});
					object obj6 = text;
					text = string.Concat(new object[]
					{
						obj6,
						", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.reader.Value),
						'"'
					});
					break;
				}
				}
				return text;
			}

			// Token: 0x040005E5 RID: 1509
			private XmlReader reader;
		}
	}
}
