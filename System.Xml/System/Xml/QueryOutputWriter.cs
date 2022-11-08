using System;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x0200005E RID: 94
	internal class QueryOutputWriter : XmlRawWriter
	{
		// Token: 0x0600034F RID: 847 RVA: 0x00010EF4 File Offset: 0x0000FEF4
		public QueryOutputWriter(XmlRawWriter writer, XmlWriterSettings settings)
		{
			this.wrapped = writer;
			this.systemId = settings.DocTypeSystem;
			this.publicId = settings.DocTypePublic;
			if (settings.OutputMethod == XmlOutputMethod.Xml)
			{
				if (this.systemId != null)
				{
					this.outputDocType = true;
					this.checkWellFormedDoc = true;
				}
				if (settings.AutoXmlDeclaration && settings.Standalone == XmlStandalone.Yes)
				{
					this.checkWellFormedDoc = true;
				}
				if (settings.CDataSectionElements.Count > 0)
				{
					this.bitsCData = new BitStack();
					this.lookupCDataElems = new Dictionary<XmlQualifiedName, int>();
					this.qnameCData = new XmlQualifiedName();
					foreach (XmlQualifiedName key in settings.CDataSectionElements)
					{
						this.lookupCDataElems[key] = 0;
					}
					this.bitsCData.PushBit(false);
					return;
				}
			}
			else if (settings.OutputMethod == XmlOutputMethod.Html && (this.systemId != null || this.publicId != null))
			{
				this.outputDocType = true;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0001100C File Offset: 0x0001000C
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00011014 File Offset: 0x00010014
		internal override IXmlNamespaceResolver NamespaceResolver
		{
			get
			{
				return this.resolver;
			}
			set
			{
				this.resolver = value;
				this.wrapped.NamespaceResolver = value;
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011029 File Offset: 0x00010029
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
			this.wrapped.WriteXmlDeclaration(standalone);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00011037 File Offset: 0x00010037
		internal override void WriteXmlDeclaration(string xmldecl)
		{
			this.wrapped.WriteXmlDeclaration(xmldecl);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00011048 File Offset: 0x00010048
		public override XmlWriterSettings Settings
		{
			get
			{
				XmlWriterSettings settings = this.wrapped.Settings;
				settings.ReadOnly = false;
				settings.DocTypeSystem = this.systemId;
				settings.DocTypePublic = this.publicId;
				settings.ReadOnly = true;
				return settings;
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00011088 File Offset: 0x00010088
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (this.publicId == null && this.systemId == null)
			{
				this.wrapped.WriteDocType(name, pubid, sysid, subset);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000110AC File Offset: 0x000100AC
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.EndCDataSection();
			if (this.checkWellFormedDoc)
			{
				if (this.depth == 0 && this.hasDocElem)
				{
					throw new XmlException("Xml_NoMultipleRoots", string.Empty);
				}
				this.depth++;
				this.hasDocElem = true;
			}
			if (this.outputDocType)
			{
				this.wrapped.WriteDocType((prefix.Length != 0) ? (prefix + ":" + localName) : localName, this.publicId, this.systemId, null);
				this.outputDocType = false;
			}
			this.wrapped.WriteStartElement(prefix, localName, ns);
			if (this.lookupCDataElems != null)
			{
				this.qnameCData.Init(localName, ns);
				this.bitsCData.PushBit(this.lookupCDataElems.ContainsKey(this.qnameCData));
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011179 File Offset: 0x00010179
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.EndCDataSection();
			this.wrapped.WriteEndElement(prefix, localName, ns);
			if (this.checkWellFormedDoc)
			{
				this.depth--;
			}
			if (this.lookupCDataElems != null)
			{
				this.bitsCData.PopBit();
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000111B9 File Offset: 0x000101B9
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.EndCDataSection();
			this.wrapped.WriteFullEndElement(prefix, localName, ns);
			if (this.checkWellFormedDoc)
			{
				this.depth--;
			}
			if (this.lookupCDataElems != null)
			{
				this.bitsCData.PopBit();
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000111F9 File Offset: 0x000101F9
		internal override void StartElementContent()
		{
			this.wrapped.StartElementContent();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011206 File Offset: 0x00010206
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.inAttr = true;
			this.wrapped.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001121D File Offset: 0x0001021D
		public override void WriteEndAttribute()
		{
			this.inAttr = false;
			this.wrapped.WriteEndAttribute();
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00011231 File Offset: 0x00010231
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
			this.wrapped.WriteNamespaceDeclaration(prefix, ns);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00011240 File Offset: 0x00010240
		public override void WriteCData(string text)
		{
			this.wrapped.WriteCData(text);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001124E File Offset: 0x0001024E
		public override void WriteComment(string text)
		{
			this.EndCDataSection();
			this.wrapped.WriteComment(text);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00011262 File Offset: 0x00010262
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.EndCDataSection();
			this.wrapped.WriteProcessingInstruction(name, text);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00011277 File Offset: 0x00010277
		public override void WriteWhitespace(string ws)
		{
			if (!this.inAttr && (this.inCDataSection || this.StartCDataSection()))
			{
				this.wrapped.WriteCData(ws);
				return;
			}
			this.wrapped.WriteWhitespace(ws);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000112AA File Offset: 0x000102AA
		public override void WriteString(string text)
		{
			if (!this.inAttr && (this.inCDataSection || this.StartCDataSection()))
			{
				this.wrapped.WriteCData(text);
				return;
			}
			this.wrapped.WriteString(text);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000112DD File Offset: 0x000102DD
		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (!this.inAttr && (this.inCDataSection || this.StartCDataSection()))
			{
				this.wrapped.WriteCData(new string(buffer, index, count));
				return;
			}
			this.wrapped.WriteChars(buffer, index, count);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00011319 File Offset: 0x00010319
		public override void WriteEntityRef(string name)
		{
			this.EndCDataSection();
			this.wrapped.WriteEntityRef(name);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001132D File Offset: 0x0001032D
		public override void WriteCharEntity(char ch)
		{
			this.EndCDataSection();
			this.wrapped.WriteCharEntity(ch);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00011341 File Offset: 0x00010341
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.EndCDataSection();
			this.wrapped.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00011356 File Offset: 0x00010356
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			if (!this.inAttr && (this.inCDataSection || this.StartCDataSection()))
			{
				this.wrapped.WriteCData(new string(buffer, index, count));
				return;
			}
			this.wrapped.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00011392 File Offset: 0x00010392
		public override void WriteRaw(string data)
		{
			if (!this.inAttr && (this.inCDataSection || this.StartCDataSection()))
			{
				this.wrapped.WriteCData(data);
				return;
			}
			this.wrapped.WriteRaw(data);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000113C5 File Offset: 0x000103C5
		public override void Close()
		{
			this.wrapped.Close();
			if (this.checkWellFormedDoc && !this.hasDocElem)
			{
				throw new XmlException("Xml_NoRoot", string.Empty);
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000113F2 File Offset: 0x000103F2
		public override void Flush()
		{
			this.wrapped.Flush();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000113FF File Offset: 0x000103FF
		private bool StartCDataSection()
		{
			if (this.lookupCDataElems != null && this.bitsCData.PeekBit())
			{
				this.inCDataSection = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00011420 File Offset: 0x00010420
		private void EndCDataSection()
		{
			this.inCDataSection = false;
		}

		// Token: 0x0400058D RID: 1421
		private XmlRawWriter wrapped;

		// Token: 0x0400058E RID: 1422
		private bool inCDataSection;

		// Token: 0x0400058F RID: 1423
		private Dictionary<XmlQualifiedName, int> lookupCDataElems;

		// Token: 0x04000590 RID: 1424
		private BitStack bitsCData;

		// Token: 0x04000591 RID: 1425
		private XmlQualifiedName qnameCData;

		// Token: 0x04000592 RID: 1426
		private bool outputDocType;

		// Token: 0x04000593 RID: 1427
		private bool checkWellFormedDoc;

		// Token: 0x04000594 RID: 1428
		private bool hasDocElem;

		// Token: 0x04000595 RID: 1429
		private bool inAttr;

		// Token: 0x04000596 RID: 1430
		private string systemId;

		// Token: 0x04000597 RID: 1431
		private string publicId;

		// Token: 0x04000598 RID: 1432
		private int depth;
	}
}
