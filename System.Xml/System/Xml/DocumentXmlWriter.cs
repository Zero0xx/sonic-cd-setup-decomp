using System;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x020000B4 RID: 180
	internal sealed class DocumentXmlWriter : XmlRawWriter, IXmlNamespaceResolver
	{
		// Token: 0x06000A30 RID: 2608 RVA: 0x0002FFFC File Offset: 0x0002EFFC
		public DocumentXmlWriter(DocumentXmlWriterType type, XmlNode start, XmlDocument document)
		{
			this.type = type;
			this.start = start;
			this.document = document;
			this.state = this.StartState();
			this.fragment = new List<XmlNode>();
			this.settings = new XmlWriterSettings();
			this.settings.ReadOnly = false;
			this.settings.CheckCharacters = false;
			this.settings.CloseOutput = false;
			this.settings.ConformanceLevel = ((this.state == DocumentXmlWriter.State.Prolog) ? ConformanceLevel.Document : ConformanceLevel.Fragment);
			this.settings.ReadOnly = true;
		}

		// Token: 0x17000236 RID: 566
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x0003008E File Offset: 0x0002F08E
		public XmlNamespaceManager NamespaceManager
		{
			set
			{
				this.namespaceManager = value;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00030097 File Offset: 0x0002F097
		public override XmlWriterSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0003009F File Offset: 0x0002F09F
		internal void SetSettings(XmlWriterSettings value)
		{
			this.settings = value;
		}

		// Token: 0x17000238 RID: 568
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x000300A8 File Offset: 0x0002F0A8
		public DocumentXPathNavigator Navigator
		{
			set
			{
				this.navigator = value;
			}
		}

		// Token: 0x17000239 RID: 569
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x000300B1 File Offset: 0x0002F0B1
		public XmlNode EndNode
		{
			set
			{
				this.end = value;
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000300BC File Offset: 0x0002F0BC
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteXmlDeclaration);
			if (standalone != XmlStandalone.Omit)
			{
				XmlNode node = this.document.CreateXmlDeclaration("1.0", string.Empty, (standalone == XmlStandalone.Yes) ? "yes" : "no");
				this.AddChild(node, this.write);
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00030108 File Offset: 0x0002F108
		internal override void WriteXmlDeclaration(string xmldecl)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteXmlDeclaration);
			string version;
			string encoding;
			string standalone;
			XmlLoader.ParseXmlDeclarationValue(xmldecl, out version, out encoding, out standalone);
			XmlNode node = this.document.CreateXmlDeclaration(version, encoding, standalone);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00030144 File Offset: 0x0002F144
		public override void WriteStartDocument()
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteStartDocument);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0003014D File Offset: 0x0002F14D
		public override void WriteStartDocument(bool standalone)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteStartDocument);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00030156 File Offset: 0x0002F156
		public override void WriteEndDocument()
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteEndDocument);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00030160 File Offset: 0x0002F160
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteDocType);
			XmlNode node = this.document.CreateDocumentType(name, pubid, sysid, subset);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00030194 File Offset: 0x0002F194
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteStartElement);
			XmlNode node = this.document.CreateElement(prefix, localName, ns);
			this.AddChild(node, this.write);
			this.write = node;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x000301CB File Offset: 0x0002F1CB
		public override void WriteEndElement()
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteEndElement);
			if (this.write == null)
			{
				throw new InvalidOperationException();
			}
			this.write = this.write.ParentNode;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x000301F3 File Offset: 0x0002F1F3
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.WriteEndElement();
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x000301FC File Offset: 0x0002F1FC
		public override void WriteFullEndElement()
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteFullEndElement);
			XmlElement xmlElement = this.write as XmlElement;
			if (xmlElement == null)
			{
				throw new InvalidOperationException();
			}
			xmlElement.IsEmpty = false;
			this.write = xmlElement.ParentNode;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00030238 File Offset: 0x0002F238
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.WriteFullEndElement();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00030240 File Offset: 0x0002F240
		internal override void StartElementContent()
		{
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00030244 File Offset: 0x0002F244
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteStartAttribute);
			XmlAttribute attr = this.document.CreateAttribute(prefix, localName, ns);
			this.AddAttribute(attr, this.write);
			this.write = attr;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0003027C File Offset: 0x0002F27C
		public override void WriteEndAttribute()
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteEndAttribute);
			XmlAttribute xmlAttribute = this.write as XmlAttribute;
			if (xmlAttribute == null)
			{
				throw new InvalidOperationException();
			}
			if (!xmlAttribute.HasChildNodes)
			{
				XmlNode node = this.document.CreateTextNode(string.Empty);
				this.AddChild(node, xmlAttribute);
			}
			this.write = xmlAttribute.OwnerElement;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000302D4 File Offset: 0x0002F2D4
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteNamespaceDeclaration);
			XmlAttribute xmlAttribute;
			if (prefix.Length == 0)
			{
				xmlAttribute = this.document.CreateAttribute(prefix, this.document.strXmlns, this.document.strReservedXmlns);
			}
			else
			{
				xmlAttribute = this.document.CreateAttribute(this.document.strXmlns, prefix, this.document.strReservedXmlns);
			}
			this.AddAttribute(xmlAttribute, this.write);
			XmlNode node = this.document.CreateTextNode(ns);
			this.AddChild(node, xmlAttribute);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0003035C File Offset: 0x0002F35C
		public override void WriteCData(string text)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteCData);
			XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
			XmlNode node = this.document.CreateCDataSection(text);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00030394 File Offset: 0x0002F394
		public override void WriteComment(string text)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteComment);
			XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
			XmlNode node = this.document.CreateComment(text);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x000303CC File Offset: 0x0002F3CC
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteProcessingInstruction);
			XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
			XmlNode node = this.document.CreateProcessingInstruction(name, text);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00030404 File Offset: 0x0002F404
		public override void WriteEntityRef(string name)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteEntityRef);
			XmlNode node = this.document.CreateEntityReference(name);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00030433 File Offset: 0x0002F433
		public override void WriteCharEntity(char ch)
		{
			this.WriteString(new string(ch, 1));
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00030444 File Offset: 0x0002F444
		public override void WriteWhitespace(string text)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteWhitespace);
			XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
			if (this.document.PreserveWhitespace)
			{
				XmlNode node = this.document.CreateWhitespace(text);
				this.AddChild(node, this.write);
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00030488 File Offset: 0x0002F488
		public override void WriteString(string text)
		{
			this.VerifyState(DocumentXmlWriter.Method.WriteString);
			XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
			XmlNode node = this.document.CreateTextNode(text);
			this.AddChild(node, this.write);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000304C0 File Offset: 0x0002F4C0
		public override void WriteSurrogateCharEntity(char lowCh, char highCh)
		{
			this.WriteString(new string(new char[]
			{
				highCh,
				lowCh
			}));
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000304E8 File Offset: 0x0002F4E8
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000304F8 File Offset: 0x0002F4F8
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00030508 File Offset: 0x0002F508
		public override void WriteRaw(string data)
		{
			this.WriteString(data);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00030511 File Offset: 0x0002F511
		public override void Close()
		{
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00030514 File Offset: 0x0002F514
		internal override void Close(WriteState currentState)
		{
			if (currentState == WriteState.Error)
			{
				return;
			}
			try
			{
				switch (this.type)
				{
				case DocumentXmlWriterType.InsertSiblingAfter:
				{
					XmlNode parentNode = this.start.ParentNode;
					if (parentNode == null)
					{
						throw new InvalidOperationException(Res.GetString("Xpn_MissingParent"));
					}
					for (int i = this.fragment.Count - 1; i >= 0; i--)
					{
						parentNode.InsertAfter(this.fragment[i], this.start);
					}
					break;
				}
				case DocumentXmlWriterType.InsertSiblingBefore:
				{
					XmlNode parentNode = this.start.ParentNode;
					if (parentNode == null)
					{
						throw new InvalidOperationException(Res.GetString("Xpn_MissingParent"));
					}
					for (int j = 0; j < this.fragment.Count; j++)
					{
						parentNode.InsertBefore(this.fragment[j], this.start);
					}
					break;
				}
				case DocumentXmlWriterType.PrependChild:
					for (int k = this.fragment.Count - 1; k >= 0; k--)
					{
						this.start.PrependChild(this.fragment[k]);
					}
					break;
				case DocumentXmlWriterType.AppendChild:
					for (int l = 0; l < this.fragment.Count; l++)
					{
						this.start.AppendChild(this.fragment[l]);
					}
					break;
				case DocumentXmlWriterType.AppendAttribute:
					this.CloseWithAppendAttribute();
					break;
				case DocumentXmlWriterType.ReplaceToFollowingSibling:
					if (this.fragment.Count == 0)
					{
						throw new InvalidOperationException(Res.GetString("Xpn_NoContent"));
					}
					this.CloseWithReplaceToFollowingSibling();
					break;
				}
			}
			finally
			{
				this.fragment.Clear();
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000306B8 File Offset: 0x0002F6B8
		private void CloseWithAppendAttribute()
		{
			XmlElement xmlElement = this.start as XmlElement;
			XmlAttributeCollection attributes = xmlElement.Attributes;
			for (int i = 0; i < this.fragment.Count; i++)
			{
				XmlAttribute xmlAttribute = this.fragment[i] as XmlAttribute;
				int num = attributes.FindNodeOffsetNS(xmlAttribute);
				if (num != -1 && ((XmlAttribute)attributes.Nodes[num]).Specified)
				{
					throw new XmlException("Xml_DupAttributeName", (xmlAttribute.Prefix.Length == 0) ? xmlAttribute.LocalName : (xmlAttribute.Prefix + ":" + xmlAttribute.LocalName));
				}
			}
			for (int j = 0; j < this.fragment.Count; j++)
			{
				XmlAttribute node = this.fragment[j] as XmlAttribute;
				attributes.Append(node);
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00030798 File Offset: 0x0002F798
		private void CloseWithReplaceToFollowingSibling()
		{
			XmlNode parentNode = this.start.ParentNode;
			if (parentNode == null)
			{
				throw new InvalidOperationException(Res.GetString("Xpn_MissingParent"));
			}
			if (this.start != this.end)
			{
				if (!DocumentXPathNavigator.IsFollowingSibling(this.start, this.end))
				{
					throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
				}
				if (this.start.IsReadOnly)
				{
					throw new InvalidOperationException(Res.GetString("Xdom_Node_Modify_ReadOnly"));
				}
				DocumentXPathNavigator.DeleteToFollowingSibling(this.start.NextSibling, this.end);
			}
			XmlNode xmlNode = this.fragment[0];
			parentNode.ReplaceChild(xmlNode, this.start);
			for (int i = this.fragment.Count - 1; i >= 1; i--)
			{
				parentNode.InsertAfter(this.fragment[i], xmlNode);
			}
			this.navigator.ResetPosition(xmlNode);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0003087B File Offset: 0x0002F87B
		public override void Flush()
		{
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0003087D File Offset: 0x0002F87D
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.namespaceManager.GetNamespacesInScope(scope);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0003088B File Offset: 0x0002F88B
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.namespaceManager.LookupNamespace(prefix);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00030899 File Offset: 0x0002F899
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.namespaceManager.LookupPrefix(namespaceName);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x000308A8 File Offset: 0x0002F8A8
		private void AddAttribute(XmlAttribute attr, XmlNode parent)
		{
			if (parent == null)
			{
				this.fragment.Add(attr);
				return;
			}
			XmlElement xmlElement = parent as XmlElement;
			if (xmlElement == null)
			{
				throw new InvalidOperationException();
			}
			xmlElement.Attributes.Append(attr);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000308E2 File Offset: 0x0002F8E2
		private void AddChild(XmlNode node, XmlNode parent)
		{
			if (parent == null)
			{
				this.fragment.Add(node);
				return;
			}
			parent.AppendChild(node);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000308FC File Offset: 0x0002F8FC
		private DocumentXmlWriter.State StartState()
		{
			XmlNodeType xmlNodeType = XmlNodeType.None;
			switch (this.type)
			{
			case DocumentXmlWriterType.InsertSiblingAfter:
			case DocumentXmlWriterType.InsertSiblingBefore:
			{
				XmlNode parentNode = this.start.ParentNode;
				if (parentNode != null)
				{
					xmlNodeType = parentNode.NodeType;
				}
				if (xmlNodeType == XmlNodeType.Document)
				{
					return DocumentXmlWriter.State.Prolog;
				}
				if (xmlNodeType == XmlNodeType.DocumentFragment)
				{
					return DocumentXmlWriter.State.Fragment;
				}
				break;
			}
			case DocumentXmlWriterType.PrependChild:
			case DocumentXmlWriterType.AppendChild:
				xmlNodeType = this.start.NodeType;
				if (xmlNodeType == XmlNodeType.Document)
				{
					return DocumentXmlWriter.State.Prolog;
				}
				if (xmlNodeType == XmlNodeType.DocumentFragment)
				{
					return DocumentXmlWriter.State.Fragment;
				}
				break;
			case DocumentXmlWriterType.AppendAttribute:
				return DocumentXmlWriter.State.Attribute;
			}
			return DocumentXmlWriter.State.Content;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00030973 File Offset: 0x0002F973
		private void VerifyState(DocumentXmlWriter.Method method)
		{
			this.state = DocumentXmlWriter.changeState[(int)(method * DocumentXmlWriter.Method.WriteEndElement + (int)this.state)];
			if (this.state == DocumentXmlWriter.State.Error)
			{
				throw new InvalidOperationException(Res.GetString("Xml_ClosedOrError"));
			}
		}

		// Token: 0x040008A8 RID: 2216
		private DocumentXmlWriterType type;

		// Token: 0x040008A9 RID: 2217
		private XmlNode start;

		// Token: 0x040008AA RID: 2218
		private XmlDocument document;

		// Token: 0x040008AB RID: 2219
		private XmlNamespaceManager namespaceManager;

		// Token: 0x040008AC RID: 2220
		private DocumentXmlWriter.State state;

		// Token: 0x040008AD RID: 2221
		private XmlNode write;

		// Token: 0x040008AE RID: 2222
		private List<XmlNode> fragment;

		// Token: 0x040008AF RID: 2223
		private XmlWriterSettings settings;

		// Token: 0x040008B0 RID: 2224
		private DocumentXPathNavigator navigator;

		// Token: 0x040008B1 RID: 2225
		private XmlNode end;

		// Token: 0x040008B2 RID: 2226
		private static DocumentXmlWriter.State[] changeState = new DocumentXmlWriter.State[]
		{
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Prolog,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Prolog,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Prolog,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Prolog,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Prolog,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Error,
			DocumentXmlWriter.State.Content,
			DocumentXmlWriter.State.Content
		};

		// Token: 0x020000B5 RID: 181
		private enum State
		{
			// Token: 0x040008B4 RID: 2228
			Error,
			// Token: 0x040008B5 RID: 2229
			Attribute,
			// Token: 0x040008B6 RID: 2230
			Prolog,
			// Token: 0x040008B7 RID: 2231
			Fragment,
			// Token: 0x040008B8 RID: 2232
			Content,
			// Token: 0x040008B9 RID: 2233
			Last
		}

		// Token: 0x020000B6 RID: 182
		private enum Method
		{
			// Token: 0x040008BB RID: 2235
			WriteXmlDeclaration,
			// Token: 0x040008BC RID: 2236
			WriteStartDocument,
			// Token: 0x040008BD RID: 2237
			WriteEndDocument,
			// Token: 0x040008BE RID: 2238
			WriteDocType,
			// Token: 0x040008BF RID: 2239
			WriteStartElement,
			// Token: 0x040008C0 RID: 2240
			WriteEndElement,
			// Token: 0x040008C1 RID: 2241
			WriteFullEndElement,
			// Token: 0x040008C2 RID: 2242
			WriteStartAttribute,
			// Token: 0x040008C3 RID: 2243
			WriteEndAttribute,
			// Token: 0x040008C4 RID: 2244
			WriteNamespaceDeclaration,
			// Token: 0x040008C5 RID: 2245
			WriteCData,
			// Token: 0x040008C6 RID: 2246
			WriteComment,
			// Token: 0x040008C7 RID: 2247
			WriteProcessingInstruction,
			// Token: 0x040008C8 RID: 2248
			WriteEntityRef,
			// Token: 0x040008C9 RID: 2249
			WriteWhitespace,
			// Token: 0x040008CA RID: 2250
			WriteString
		}
	}
}
