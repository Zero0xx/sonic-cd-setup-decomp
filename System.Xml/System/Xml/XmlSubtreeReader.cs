using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x02000080 RID: 128
	internal sealed class XmlSubtreeReader : XmlWrappingReader, IXmlNamespaceResolver
	{
		// Token: 0x060005CA RID: 1482 RVA: 0x000171A8 File Offset: 0x000161A8
		internal XmlSubtreeReader(XmlReader reader) : base(reader)
		{
			this.initialDepth = reader.Depth;
			this.state = XmlSubtreeReader.State.Initial;
			this.nsManager = new XmlNamespaceManager(reader.NameTable);
			this.xmlns = reader.NameTable.Add("xmlns");
			this.xmlnsUri = reader.NameTable.Add("http://www.w3.org/2000/xmlns/");
			this.tmpNode = new XmlSubtreeReader.NodeData();
			this.tmpNode.Set(XmlNodeType.None, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			this.SetCurrentNode(this.tmpNode);
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00017256 File Offset: 0x00016256
		public override XmlReaderSettings Settings
		{
			get
			{
				return this.reader.Settings;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00017263 File Offset: 0x00016263
		public override XmlNodeType NodeType
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.NodeType;
				}
				return this.curNode.type;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00017284 File Offset: 0x00016284
		public override string Name
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.Name;
				}
				return this.curNode.name;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000172A5 File Offset: 0x000162A5
		public override string LocalName
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.LocalName;
				}
				return this.curNode.localName;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000172C6 File Offset: 0x000162C6
		public override string NamespaceURI
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.NamespaceURI;
				}
				return this.curNode.namespaceUri;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x000172E7 File Offset: 0x000162E7
		public override string Prefix
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.Prefix;
				}
				return this.curNode.prefix;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00017308 File Offset: 0x00016308
		public override string Value
		{
			get
			{
				if (!this.useCurNode)
				{
					return this.reader.Value;
				}
				return this.curNode.value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001732C File Offset: 0x0001632C
		public override int Depth
		{
			get
			{
				int num = this.reader.Depth - this.initialDepth;
				if (this.curNsAttr != -1)
				{
					if (this.curNode.type == XmlNodeType.Text)
					{
						num += 2;
					}
					else
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0001736E File Offset: 0x0001636E
		public override string BaseURI
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001737B File Offset: 0x0001637B
		public override bool CanResolveEntity
		{
			get
			{
				return this.reader.CanResolveEntity;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00017388 File Offset: 0x00016388
		public override bool EOF
		{
			get
			{
				return this.state == XmlSubtreeReader.State.EndOfFile || this.state == XmlSubtreeReader.State.Closed;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001739E File Offset: 0x0001639E
		public override ReadState ReadState
		{
			get
			{
				if (this.reader.ReadState == ReadState.Error)
				{
					return ReadState.Error;
				}
				if (this.state <= XmlSubtreeReader.State.Closed)
				{
					return (ReadState)this.state;
				}
				return ReadState.Interactive;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x000173C1 File Offset: 0x000163C1
		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x000173CE File Offset: 0x000163CE
		public override int AttributeCount
		{
			get
			{
				if (!this.InAttributeActiveState)
				{
					return 0;
				}
				return this.reader.AttributeCount + this.nsAttrCount;
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000173EC File Offset: 0x000163EC
		public override string GetAttribute(string name)
		{
			if (!this.InAttributeActiveState)
			{
				return null;
			}
			string attribute = this.reader.GetAttribute(name);
			if (attribute != null)
			{
				return attribute;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].name)
				{
					return this.nsAttributes[i].value;
				}
			}
			return null;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001744C File Offset: 0x0001644C
		public override string GetAttribute(string name, string namespaceURI)
		{
			if (!this.InAttributeActiveState)
			{
				return null;
			}
			string attribute = this.reader.GetAttribute(name, namespaceURI);
			if (attribute != null)
			{
				return attribute;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].localName && namespaceURI == this.xmlnsUri)
				{
					return this.nsAttributes[i].value;
				}
			}
			return null;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000174BC File Offset: 0x000164BC
		public override string GetAttribute(int i)
		{
			if (!this.InAttributeActiveState)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			int attributeCount = this.reader.AttributeCount;
			if (i < attributeCount)
			{
				return this.reader.GetAttribute(i);
			}
			if (i - attributeCount < this.nsAttrCount)
			{
				return this.nsAttributes[i - attributeCount].value;
			}
			throw new ArgumentOutOfRangeException("i");
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00017520 File Offset: 0x00016520
		public override bool MoveToAttribute(string name)
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.reader.MoveToAttribute(name))
			{
				this.curNsAttr = -1;
				this.useCurNode = false;
				return true;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].name)
				{
					this.MoveToNsAttribute(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00017588 File Offset: 0x00016588
		public override bool MoveToAttribute(string name, string ns)
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.reader.MoveToAttribute(name, ns))
			{
				this.curNsAttr = -1;
				this.useCurNode = false;
				return true;
			}
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (name == this.nsAttributes[i].localName && ns == this.xmlnsUri)
				{
					this.MoveToNsAttribute(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000175FC File Offset: 0x000165FC
		public override void MoveToAttribute(int i)
		{
			if (!this.InAttributeActiveState)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			int attributeCount = this.reader.AttributeCount;
			if (i < attributeCount)
			{
				this.reader.MoveToAttribute(i);
				this.curNsAttr = -1;
				this.useCurNode = false;
				return;
			}
			if (i - attributeCount < this.nsAttrCount)
			{
				this.MoveToNsAttribute(i - attributeCount);
				return;
			}
			throw new ArgumentOutOfRangeException("i");
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00017666 File Offset: 0x00016666
		public override bool MoveToFirstAttribute()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.reader.MoveToFirstAttribute())
			{
				this.useCurNode = false;
				return true;
			}
			if (this.nsAttrCount > 0)
			{
				this.MoveToNsAttribute(0);
				return true;
			}
			return false;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001769C File Offset: 0x0001669C
		public override bool MoveToNextAttribute()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.curNsAttr == -1 && this.reader.MoveToNextAttribute())
			{
				return true;
			}
			if (this.curNsAttr + 1 < this.nsAttrCount)
			{
				this.MoveToNsAttribute(this.curNsAttr + 1);
				return true;
			}
			return false;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000176EC File Offset: 0x000166EC
		public override bool MoveToElement()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			this.curNsAttr = -1;
			this.useCurNode = false;
			return this.reader.MoveToElement();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00017714 File Offset: 0x00016714
		public override bool ReadAttributeValue()
		{
			if (!this.InAttributeActiveState)
			{
				return false;
			}
			if (this.curNsAttr == -1)
			{
				return this.reader.ReadAttributeValue();
			}
			if (this.curNode.type == XmlNodeType.Text)
			{
				return false;
			}
			this.tmpNode.type = XmlNodeType.Text;
			this.tmpNode.value = this.curNode.value;
			this.SetCurrentNode(this.tmpNode);
			return true;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00017780 File Offset: 0x00016780
		public override bool Read()
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
				this.useCurNode = false;
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
				return true;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.Error:
				return false;
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return false;
			case XmlSubtreeReader.State.PopNamespaceScope:
				this.nsManager.PopScope();
				goto IL_E5;
			case XmlSubtreeReader.State.ClearNsAttributes:
				goto IL_E5;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				return this.FinishReadElementContentAsBinary() && this.Read();
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				return this.FinishReadContentAsBinary() && this.Read();
			default:
				return false;
			}
			IL_54:
			this.curNsAttr = -1;
			this.useCurNode = false;
			this.reader.MoveToElement();
			if (this.reader.Depth == this.initialDepth && (this.reader.NodeType == XmlNodeType.EndElement || (this.reader.NodeType == XmlNodeType.Element && this.reader.IsEmptyElement)))
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			if (this.reader.Read())
			{
				this.ProcessNamespaces();
				return true;
			}
			this.SetEmptyNode();
			return false;
			IL_E5:
			this.nsAttrCount = 0;
			this.state = XmlSubtreeReader.State.Interactive;
			goto IL_54;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000178A8 File Offset: 0x000168A8
		public override void Close()
		{
			if (this.state == XmlSubtreeReader.State.Closed)
			{
				return;
			}
			try
			{
				if (this.state != XmlSubtreeReader.State.EndOfFile)
				{
					this.reader.MoveToElement();
					if (this.reader.Depth == this.initialDepth && this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement)
					{
						this.reader.Read();
					}
					while (this.reader.Depth > this.initialDepth && this.reader.Read())
					{
					}
				}
			}
			catch
			{
			}
			finally
			{
				this.curNsAttr = -1;
				this.useCurNode = false;
				this.state = XmlSubtreeReader.State.Closed;
				this.SetEmptyNode();
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00017970 File Offset: 0x00016970
		public override void Skip()
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
				this.Read();
				return;
			case XmlSubtreeReader.State.Interactive:
				break;
			case XmlSubtreeReader.State.Error:
				return;
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return;
			case XmlSubtreeReader.State.PopNamespaceScope:
				this.nsManager.PopScope();
				goto IL_119;
			case XmlSubtreeReader.State.ClearNsAttributes:
				goto IL_119;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				if (this.FinishReadElementContentAsBinary())
				{
					this.Skip();
					return;
				}
				return;
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				if (this.FinishReadContentAsBinary())
				{
					this.Skip();
					return;
				}
				return;
			default:
				return;
			}
			IL_42:
			this.curNsAttr = -1;
			this.useCurNode = false;
			this.reader.MoveToElement();
			if (this.reader.Depth == this.initialDepth)
			{
				if (this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement && this.reader.Read())
				{
					while (this.reader.NodeType != XmlNodeType.EndElement && this.reader.Depth > this.initialDepth)
					{
						this.reader.Skip();
					}
				}
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return;
			}
			if (this.reader.NodeType == XmlNodeType.Element && !this.reader.IsEmptyElement)
			{
				this.nsManager.PopScope();
			}
			this.reader.Skip();
			this.ProcessNamespaces();
			return;
			IL_119:
			this.nsAttrCount = 0;
			this.state = XmlSubtreeReader.State.Interactive;
			goto IL_42;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00017AC8 File Offset: 0x00016AC8
		public override object ReadContentAsObject()
		{
			object result;
			try
			{
				this.InitReadContentAsType("ReadContentAsObject");
				object obj = this.reader.ReadContentAsObject();
				this.FinishReadContentAsType();
				result = obj;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00017B14 File Offset: 0x00016B14
		public override bool ReadContentAsBoolean()
		{
			bool result;
			try
			{
				this.InitReadContentAsType("ReadContentAsBoolean");
				bool flag = this.reader.ReadContentAsBoolean();
				this.FinishReadContentAsType();
				result = flag;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00017B60 File Offset: 0x00016B60
		public override DateTime ReadContentAsDateTime()
		{
			DateTime result;
			try
			{
				this.InitReadContentAsType("ReadContentAsDateTime");
				DateTime dateTime = this.reader.ReadContentAsDateTime();
				this.FinishReadContentAsType();
				result = dateTime;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00017BAC File Offset: 0x00016BAC
		public override double ReadContentAsDouble()
		{
			double result;
			try
			{
				this.InitReadContentAsType("ReadContentAsDouble");
				double num = this.reader.ReadContentAsDouble();
				this.FinishReadContentAsType();
				result = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00017BF8 File Offset: 0x00016BF8
		public override float ReadContentAsFloat()
		{
			float result;
			try
			{
				this.InitReadContentAsType("ReadContentAsFloat");
				float num = this.reader.ReadContentAsFloat();
				this.FinishReadContentAsType();
				result = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00017C44 File Offset: 0x00016C44
		public override decimal ReadContentAsDecimal()
		{
			decimal result;
			try
			{
				this.InitReadContentAsType("ReadContentAsDecimal");
				decimal num = this.reader.ReadContentAsDecimal();
				this.FinishReadContentAsType();
				result = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00017C90 File Offset: 0x00016C90
		public override int ReadContentAsInt()
		{
			int result;
			try
			{
				this.InitReadContentAsType("ReadContentAsInt");
				int num = this.reader.ReadContentAsInt();
				this.FinishReadContentAsType();
				result = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00017CDC File Offset: 0x00016CDC
		public override long ReadContentAsLong()
		{
			long result;
			try
			{
				this.InitReadContentAsType("ReadContentAsLong");
				long num = this.reader.ReadContentAsLong();
				this.FinishReadContentAsType();
				result = num;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00017D28 File Offset: 0x00016D28
		public override string ReadContentAsString()
		{
			string result;
			try
			{
				this.InitReadContentAsType("ReadContentAsString");
				string text = this.reader.ReadContentAsString();
				this.FinishReadContentAsType();
				result = text;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00017D74 File Offset: 0x00016D74
		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			object result;
			try
			{
				this.InitReadContentAsType("ReadContentAs");
				object obj = this.reader.ReadContentAs(returnType, namespaceResolver);
				this.FinishReadContentAsType();
				result = obj;
			}
			catch
			{
				this.state = XmlSubtreeReader.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00017DC0 File Offset: 0x00016DC0
		public override bool CanReadBinaryContent
		{
			get
			{
				return this.reader.CanReadBinaryContent;
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00017DD0 File Offset: 0x00016DD0
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				this.state = XmlSubtreeReader.State.ReadContentAsBase64;
				break;
			case XmlSubtreeReader.State.Error:
				return 0;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw base.CreateReadContentAsException("ReadContentAsBase64");
				case XmlNodeType.Attribute:
				case XmlNodeType.Text:
					return this.reader.ReadContentAsBase64(buffer, index, count);
				default:
					if (nodeType != XmlNodeType.EndElement)
					{
						return 0;
					}
					return 0;
				}
				break;
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
			case XmlSubtreeReader.State.ReadContentAsBase64:
				break;
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBase64(buffer, index, count);
			if (num == 0)
			{
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
			}
			return num;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00017E98 File Offset: 0x00016E98
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				if (!this.InitReadElementContentAsBinary(XmlSubtreeReader.State.ReadElementContentAsBase64))
				{
					return 0;
				}
				break;
			case XmlSubtreeReader.State.Error:
				return 0;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
				break;
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBase64(buffer, index, count);
			if (num > 0)
			{
				return num;
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
			}
			else
			{
				this.Read();
			}
			return 0;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00017F84 File Offset: 0x00016F84
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
				this.state = XmlSubtreeReader.State.ReadContentAsBinHex;
				break;
			case XmlSubtreeReader.State.Error:
				return 0;
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw base.CreateReadContentAsException("ReadContentAsBinHex");
				case XmlNodeType.Attribute:
				case XmlNodeType.Text:
					return this.reader.ReadContentAsBinHex(buffer, index, count);
				default:
					if (nodeType != XmlNodeType.EndElement)
					{
						return 0;
					}
					return 0;
				}
				break;
			}
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
				throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				break;
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBinHex(buffer, index, count);
			if (num == 0)
			{
				this.state = XmlSubtreeReader.State.Interactive;
				this.ProcessNamespaces();
			}
			return num;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001804C File Offset: 0x0001704C
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				if (!this.InitReadElementContentAsBinary(XmlSubtreeReader.State.ReadElementContentAsBinHex))
				{
					return 0;
				}
				break;
			case XmlSubtreeReader.State.Error:
				return 0;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
				break;
			default:
				return 0;
			}
			int num = this.reader.ReadContentAsBinHex(buffer, index, count);
			if (num > 0)
			{
				return num;
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
			}
			else
			{
				this.Read();
			}
			return 0;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00018137 File Offset: 0x00017137
		public override bool CanReadValueChunk
		{
			get
			{
				return this.reader.CanReadValueChunk;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00018144 File Offset: 0x00017144
		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Initial:
			case XmlSubtreeReader.State.Error:
			case XmlSubtreeReader.State.EndOfFile:
			case XmlSubtreeReader.State.Closed:
				return 0;
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				return this.reader.ReadValueChunk(buffer, index, count);
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("Xml_MixingReadValueChunkWithBinary"));
			default:
				return 0;
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000181AE File Offset: 0x000171AE
		protected override void Dispose(bool disposing)
		{
			this.Close();
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x000181B6 File Offset: 0x000171B6
		public override int LineNumber
		{
			get
			{
				if (this.readerAsIXmlLineInfo != null && !this.useCurNode)
				{
					return this.readerAsIXmlLineInfo.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x000181D5 File Offset: 0x000171D5
		public override int LinePosition
		{
			get
			{
				if (this.readerAsIXmlLineInfo != null && !this.useCurNode)
				{
					return this.readerAsIXmlLineInfo.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000181F4 File Offset: 0x000171F4
		public override string LookupNamespace(string prefix)
		{
			return ((IXmlNamespaceResolver)this).LookupNamespace(prefix);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000181FD File Offset: 0x000171FD
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			if (!this.InNamespaceActiveState)
			{
				return new Dictionary<string, string>();
			}
			return this.nsManager.GetNamespacesInScope(scope);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00018219 File Offset: 0x00017219
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			if (!this.InNamespaceActiveState)
			{
				return null;
			}
			return this.nsManager.LookupNamespace(prefix);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00018231 File Offset: 0x00017231
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			if (!this.InNamespaceActiveState)
			{
				return null;
			}
			return this.nsManager.LookupPrefix(namespaceName);
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00018249 File Offset: 0x00017249
		internal override SchemaInfo DtdSchemaInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001824C File Offset: 0x0001724C
		private void ProcessNamespaces()
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.EndElement)
				{
					return;
				}
				this.state = XmlSubtreeReader.State.PopNamespaceScope;
			}
			else
			{
				this.nsManager.PushScope();
				string text = this.reader.Prefix;
				string namespaceURI = this.reader.NamespaceURI;
				if (this.nsManager.LookupNamespace(text) != namespaceURI)
				{
					this.AddNamespace(text, namespaceURI);
				}
				if (this.reader.MoveToFirstAttribute())
				{
					do
					{
						text = this.reader.Prefix;
						namespaceURI = this.reader.NamespaceURI;
						if (Ref.Equal(namespaceURI, this.xmlnsUri))
						{
							if (text.Length == 0)
							{
								this.nsManager.AddNamespace(string.Empty, this.reader.Value);
								this.RemoveNamespace(string.Empty, this.xmlns);
							}
							else
							{
								text = this.reader.LocalName;
								this.nsManager.AddNamespace(text, this.reader.Value);
								this.RemoveNamespace(this.xmlns, text);
							}
						}
						else if (text.Length != 0 && this.nsManager.LookupNamespace(text) != namespaceURI)
						{
							this.AddNamespace(text, namespaceURI);
						}
					}
					while (this.reader.MoveToNextAttribute());
					this.reader.MoveToElement();
				}
				if (this.reader.IsEmptyElement)
				{
					this.state = XmlSubtreeReader.State.PopNamespaceScope;
					return;
				}
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000183AC File Offset: 0x000173AC
		private void AddNamespace(string prefix, string ns)
		{
			this.nsManager.AddNamespace(prefix, ns);
			int num = this.nsAttrCount++;
			if (this.nsAttributes == null)
			{
				this.nsAttributes = new XmlSubtreeReader.NodeData[this.InitialNamespaceAttributeCount];
			}
			if (num == this.nsAttributes.Length)
			{
				XmlSubtreeReader.NodeData[] destinationArray = new XmlSubtreeReader.NodeData[this.nsAttributes.Length * 2];
				Array.Copy(this.nsAttributes, 0, destinationArray, 0, num);
				this.nsAttributes = destinationArray;
			}
			if (this.nsAttributes[num] == null)
			{
				this.nsAttributes[num] = new XmlSubtreeReader.NodeData();
			}
			if (prefix.Length == 0)
			{
				this.nsAttributes[num].Set(XmlNodeType.Attribute, this.xmlns, string.Empty, this.xmlns, "http://www.w3.org/2000/xmlns/", ns);
			}
			else
			{
				this.nsAttributes[num].Set(XmlNodeType.Attribute, prefix, this.xmlns, this.xmlns + ":" + prefix, "http://www.w3.org/2000/xmlns/", ns);
			}
			this.state = XmlSubtreeReader.State.ClearNsAttributes;
			this.curNsAttr = -1;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000184A4 File Offset: 0x000174A4
		private void RemoveNamespace(string prefix, string localName)
		{
			for (int i = 0; i < this.nsAttrCount; i++)
			{
				if (Ref.Equal(prefix, this.nsAttributes[i].prefix) && Ref.Equal(localName, this.nsAttributes[i].localName))
				{
					if (i < this.nsAttrCount - 1)
					{
						XmlSubtreeReader.NodeData nodeData = this.nsAttributes[i];
						this.nsAttributes[i] = this.nsAttributes[this.nsAttrCount - 1];
						this.nsAttributes[this.nsAttrCount - 1] = nodeData;
					}
					this.nsAttrCount--;
					return;
				}
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00018539 File Offset: 0x00017539
		private void MoveToNsAttribute(int index)
		{
			this.reader.MoveToElement();
			this.curNsAttr = index;
			this.SetCurrentNode(this.nsAttributes[index]);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001855C File Offset: 0x0001755C
		private bool InitReadElementContentAsBinary(XmlSubtreeReader.State binaryState)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
			}
			bool isEmptyElement = this.IsEmptyElement;
			if (!this.Read() || isEmptyElement)
			{
				return false;
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			if (nodeType != XmlNodeType.EndElement)
			{
				this.state = binaryState;
				return true;
			}
			this.ProcessNamespaces();
			this.Read();
			return false;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000185EC File Offset: 0x000175EC
		private bool FinishReadElementContentAsBinary()
		{
			byte[] buffer = new byte[256];
			if (this.state == XmlSubtreeReader.State.ReadElementContentAsBase64)
			{
				while (this.reader.ReadContentAsBase64(buffer, 0, 256) > 0)
				{
				}
			}
			else
			{
				while (this.reader.ReadContentAsBinHex(buffer, 0, 256) > 0)
				{
				}
			}
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			return this.Read();
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000186A4 File Offset: 0x000176A4
		private bool FinishReadContentAsBinary()
		{
			byte[] buffer = new byte[256];
			if (this.state == XmlSubtreeReader.State.ReadContentAsBase64)
			{
				while (this.reader.ReadContentAsBase64(buffer, 0, 256) > 0)
				{
				}
			}
			else
			{
				while (this.reader.ReadContentAsBinHex(buffer, 0, 256) > 0)
				{
				}
			}
			this.state = XmlSubtreeReader.State.Interactive;
			this.ProcessNamespaces();
			if (this.reader.Depth == this.initialDepth)
			{
				this.state = XmlSubtreeReader.State.EndOfFile;
				this.SetEmptyNode();
				return false;
			}
			return true;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00018722 File Offset: 0x00017722
		private bool InAttributeActiveState
		{
			get
			{
				return 0 != (98 & 1 << (int)this.state);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00018738 File Offset: 0x00017738
		private bool InNamespaceActiveState
		{
			get
			{
				return 0 != (2018 & 1 << (int)this.state);
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00018751 File Offset: 0x00017751
		private void SetEmptyNode()
		{
			this.tmpNode.type = XmlNodeType.None;
			this.tmpNode.value = string.Empty;
			this.curNode = this.tmpNode;
			this.useCurNode = true;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00018782 File Offset: 0x00017782
		private void SetCurrentNode(XmlSubtreeReader.NodeData node)
		{
			this.curNode = node;
			this.useCurNode = true;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00018794 File Offset: 0x00017794
		private void InitReadContentAsType(string methodName)
		{
			switch (this.state)
			{
			case XmlSubtreeReader.State.Interactive:
			case XmlSubtreeReader.State.PopNamespaceScope:
			case XmlSubtreeReader.State.ClearNsAttributes:
				return;
			case XmlSubtreeReader.State.ReadElementContentAsBase64:
			case XmlSubtreeReader.State.ReadElementContentAsBinHex:
			case XmlSubtreeReader.State.ReadContentAsBase64:
			case XmlSubtreeReader.State.ReadContentAsBinHex:
				throw new InvalidOperationException(Res.GetString("Xml_MixingReadValueChunkWithBinary"));
			}
			throw base.CreateReadContentAsException(methodName);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000187F4 File Offset: 0x000177F4
		private void FinishReadContentAsType()
		{
			XmlNodeType nodeType = this.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				this.ProcessNamespaces();
				return;
			case XmlNodeType.Attribute:
				break;
			default:
				if (nodeType != XmlNodeType.EndElement)
				{
					return;
				}
				this.state = XmlSubtreeReader.State.PopNamespaceScope;
				break;
			}
		}

		// Token: 0x04000656 RID: 1622
		private const int AttributeActiveStates = 98;

		// Token: 0x04000657 RID: 1623
		private const int NamespaceActiveStates = 2018;

		// Token: 0x04000658 RID: 1624
		private int initialDepth;

		// Token: 0x04000659 RID: 1625
		private XmlSubtreeReader.State state;

		// Token: 0x0400065A RID: 1626
		private XmlNamespaceManager nsManager;

		// Token: 0x0400065B RID: 1627
		private XmlSubtreeReader.NodeData[] nsAttributes;

		// Token: 0x0400065C RID: 1628
		private int nsAttrCount;

		// Token: 0x0400065D RID: 1629
		private int curNsAttr = -1;

		// Token: 0x0400065E RID: 1630
		private string xmlns;

		// Token: 0x0400065F RID: 1631
		private string xmlnsUri;

		// Token: 0x04000660 RID: 1632
		private bool useCurNode;

		// Token: 0x04000661 RID: 1633
		private XmlSubtreeReader.NodeData curNode;

		// Token: 0x04000662 RID: 1634
		private XmlSubtreeReader.NodeData tmpNode;

		// Token: 0x04000663 RID: 1635
		internal int InitialNamespaceAttributeCount = 4;

		// Token: 0x02000081 RID: 129
		private class NodeData
		{
			// Token: 0x0600060C RID: 1548 RVA: 0x0001882C File Offset: 0x0001782C
			internal NodeData()
			{
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x00018834 File Offset: 0x00017834
			internal void Set(XmlNodeType nodeType, string localName, string prefix, string name, string namespaceUri, string value)
			{
				this.type = nodeType;
				this.localName = localName;
				this.prefix = prefix;
				this.name = name;
				this.namespaceUri = namespaceUri;
				this.value = value;
			}

			// Token: 0x04000664 RID: 1636
			internal XmlNodeType type;

			// Token: 0x04000665 RID: 1637
			internal string localName;

			// Token: 0x04000666 RID: 1638
			internal string prefix;

			// Token: 0x04000667 RID: 1639
			internal string name;

			// Token: 0x04000668 RID: 1640
			internal string namespaceUri;

			// Token: 0x04000669 RID: 1641
			internal string value;
		}

		// Token: 0x02000082 RID: 130
		private enum State
		{
			// Token: 0x0400066B RID: 1643
			Initial,
			// Token: 0x0400066C RID: 1644
			Interactive,
			// Token: 0x0400066D RID: 1645
			Error,
			// Token: 0x0400066E RID: 1646
			EndOfFile,
			// Token: 0x0400066F RID: 1647
			Closed,
			// Token: 0x04000670 RID: 1648
			PopNamespaceScope,
			// Token: 0x04000671 RID: 1649
			ClearNsAttributes,
			// Token: 0x04000672 RID: 1650
			ReadElementContentAsBase64,
			// Token: 0x04000673 RID: 1651
			ReadElementContentAsBinHex,
			// Token: 0x04000674 RID: 1652
			ReadContentAsBase64,
			// Token: 0x04000675 RID: 1653
			ReadContentAsBinHex
		}
	}
}
