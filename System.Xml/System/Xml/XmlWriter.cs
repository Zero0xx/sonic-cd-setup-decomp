using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x02000050 RID: 80
	public abstract class XmlWriter : IDisposable
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000947B File Offset: 0x0000847B
		public virtual XmlWriterSettings Settings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600021D RID: 541
		public abstract void WriteStartDocument();

		// Token: 0x0600021E RID: 542
		public abstract void WriteStartDocument(bool standalone);

		// Token: 0x0600021F RID: 543
		public abstract void WriteEndDocument();

		// Token: 0x06000220 RID: 544
		public abstract void WriteDocType(string name, string pubid, string sysid, string subset);

		// Token: 0x06000221 RID: 545 RVA: 0x0000947E File Offset: 0x0000847E
		public void WriteStartElement(string localName, string ns)
		{
			this.WriteStartElement(null, localName, ns);
		}

		// Token: 0x06000222 RID: 546
		public abstract void WriteStartElement(string prefix, string localName, string ns);

		// Token: 0x06000223 RID: 547 RVA: 0x00009489 File Offset: 0x00008489
		public void WriteStartElement(string localName)
		{
			this.WriteStartElement(null, localName, null);
		}

		// Token: 0x06000224 RID: 548
		public abstract void WriteEndElement();

		// Token: 0x06000225 RID: 549
		public abstract void WriteFullEndElement();

		// Token: 0x06000226 RID: 550 RVA: 0x00009494 File Offset: 0x00008494
		public void WriteAttributeString(string localName, string ns, string value)
		{
			this.WriteStartAttribute(null, localName, ns);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000094AC File Offset: 0x000084AC
		public void WriteAttributeString(string localName, string value)
		{
			this.WriteStartAttribute(null, localName, null);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000094C4 File Offset: 0x000084C4
		public void WriteAttributeString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartAttribute(prefix, localName, ns);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000094DD File Offset: 0x000084DD
		public void WriteStartAttribute(string localName, string ns)
		{
			this.WriteStartAttribute(null, localName, ns);
		}

		// Token: 0x0600022A RID: 554
		public abstract void WriteStartAttribute(string prefix, string localName, string ns);

		// Token: 0x0600022B RID: 555 RVA: 0x000094E8 File Offset: 0x000084E8
		public void WriteStartAttribute(string localName)
		{
			this.WriteStartAttribute(null, localName, null);
		}

		// Token: 0x0600022C RID: 556
		public abstract void WriteEndAttribute();

		// Token: 0x0600022D RID: 557
		public abstract void WriteCData(string text);

		// Token: 0x0600022E RID: 558
		public abstract void WriteComment(string text);

		// Token: 0x0600022F RID: 559
		public abstract void WriteProcessingInstruction(string name, string text);

		// Token: 0x06000230 RID: 560
		public abstract void WriteEntityRef(string name);

		// Token: 0x06000231 RID: 561
		public abstract void WriteCharEntity(char ch);

		// Token: 0x06000232 RID: 562
		public abstract void WriteWhitespace(string ws);

		// Token: 0x06000233 RID: 563
		public abstract void WriteString(string text);

		// Token: 0x06000234 RID: 564
		public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);

		// Token: 0x06000235 RID: 565
		public abstract void WriteChars(char[] buffer, int index, int count);

		// Token: 0x06000236 RID: 566
		public abstract void WriteRaw(char[] buffer, int index, int count);

		// Token: 0x06000237 RID: 567
		public abstract void WriteRaw(string data);

		// Token: 0x06000238 RID: 568
		public abstract void WriteBase64(byte[] buffer, int index, int count);

		// Token: 0x06000239 RID: 569 RVA: 0x000094F3 File Offset: 0x000084F3
		public virtual void WriteBinHex(byte[] buffer, int index, int count)
		{
			BinHexEncoder.Encode(buffer, index, count, this);
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600023A RID: 570
		public abstract WriteState WriteState { get; }

		// Token: 0x0600023B RID: 571
		public abstract void Close();

		// Token: 0x0600023C RID: 572
		public abstract void Flush();

		// Token: 0x0600023D RID: 573
		public abstract string LookupPrefix(string ns);

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000094FE File Offset: 0x000084FE
		public virtual XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.Default;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00009501 File Offset: 0x00008501
		public virtual string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009508 File Offset: 0x00008508
		public virtual void WriteNmToken(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xml_EmptyName"));
			}
			this.WriteString(XmlConvert.VerifyNMTOKEN(name, ExceptionType.ArgumentException));
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009532 File Offset: 0x00008532
		public virtual void WriteName(string name)
		{
			this.WriteString(XmlConvert.VerifyQName(name, ExceptionType.ArgumentException));
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00009544 File Offset: 0x00008544
		public virtual void WriteQualifiedName(string localName, string ns)
		{
			if (ns != null && ns.Length > 0)
			{
				string text = this.LookupPrefix(ns);
				if (text == null)
				{
					throw new ArgumentException(Res.GetString("Xml_UndefNamespace", new object[]
					{
						ns
					}));
				}
				this.WriteString(text);
				this.WriteString(":");
			}
			this.WriteString(localName);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000959D File Offset: 0x0000859D
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value, null));
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000095BF File Offset: 0x000085BF
		public virtual void WriteValue(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(value);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000095D6 File Offset: 0x000085D6
		public virtual void WriteValue(bool value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000095E9 File Offset: 0x000085E9
		public virtual void WriteValue(DateTime value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000095FC File Offset: 0x000085FC
		public virtual void WriteValue(double value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000960F File Offset: 0x0000860F
		public virtual void WriteValue(float value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009622 File Offset: 0x00008622
		public virtual void WriteValue(decimal value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009635 File Offset: 0x00008635
		public virtual void WriteValue(int value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009648 File Offset: 0x00008648
		public virtual void WriteValue(long value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000965C File Offset: 0x0000865C
		public virtual void WriteAttributes(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.XmlDeclaration)
			{
				if (reader.MoveToFirstAttribute())
				{
					this.WriteAttributes(reader, defattr);
					reader.MoveToElement();
					return;
				}
			}
			else
			{
				if (reader.NodeType != XmlNodeType.Attribute)
				{
					throw new XmlException("Xml_InvalidPosition", string.Empty);
				}
				do
				{
					IXmlSchemaInfo schemaInfo;
					if (defattr || (!reader.IsDefault && ((schemaInfo = reader.SchemaInfo) == null || !schemaInfo.IsDefault)))
					{
						this.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
						while (reader.ReadAttributeValue())
						{
							if (reader.NodeType == XmlNodeType.EntityReference)
							{
								this.WriteEntityRef(reader.Name);
							}
							else
							{
								this.WriteString(reader.Value);
							}
						}
						this.WriteEndAttribute();
					}
				}
				while (reader.MoveToNextAttribute());
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009730 File Offset: 0x00008730
		public virtual void WriteNode(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			bool canReadValueChunk = reader.CanReadValueChunk;
			int num = (reader.NodeType == XmlNodeType.None) ? -1 : reader.Depth;
			do
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
					this.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
					this.WriteAttributes(reader, defattr);
					if (reader.IsEmptyElement)
					{
						this.WriteEndElement();
					}
					break;
				case XmlNodeType.Text:
					if (canReadValueChunk)
					{
						if (this.writeNodeBuffer == null)
						{
							this.writeNodeBuffer = new char[1024];
						}
						int count;
						while ((count = reader.ReadValueChunk(this.writeNodeBuffer, 0, 1024)) > 0)
						{
							this.WriteChars(this.writeNodeBuffer, 0, count);
						}
					}
					else
					{
						this.WriteString(reader.Value);
					}
					break;
				case XmlNodeType.CDATA:
					this.WriteCData(reader.Value);
					break;
				case XmlNodeType.EntityReference:
					this.WriteEntityRef(reader.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					this.WriteProcessingInstruction(reader.Name, reader.Value);
					break;
				case XmlNodeType.Comment:
					this.WriteComment(reader.Value);
					break;
				case XmlNodeType.DocumentType:
					this.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.WriteWhitespace(reader.Value);
					break;
				case XmlNodeType.EndElement:
					this.WriteFullEndElement();
					break;
				}
			}
			while (reader.Read() && (num < reader.Depth || (num == reader.Depth && reader.NodeType == XmlNodeType.EndElement)));
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000098F0 File Offset: 0x000088F0
		public virtual void WriteNode(XPathNavigator navigator, bool defattr)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}
			int num = 0;
			navigator = navigator.Clone();
			for (;;)
			{
				IL_18:
				bool flag = false;
				switch (navigator.NodeType)
				{
				case XPathNodeType.Root:
					flag = true;
					break;
				case XPathNodeType.Element:
					this.WriteStartElement(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
					if (navigator.MoveToFirstAttribute())
					{
						do
						{
							IXmlSchemaInfo schemaInfo = navigator.SchemaInfo;
							if (defattr || schemaInfo == null || !schemaInfo.IsDefault)
							{
								this.WriteStartAttribute(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
								this.WriteString(navigator.Value);
								this.WriteEndAttribute();
							}
						}
						while (navigator.MoveToNextAttribute());
						navigator.MoveToParent();
					}
					if (navigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
					{
						this.WriteLocalNamespaces(navigator);
						navigator.MoveToParent();
					}
					flag = true;
					break;
				case XPathNodeType.Text:
					this.WriteString(navigator.Value);
					break;
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					this.WriteWhitespace(navigator.Value);
					break;
				case XPathNodeType.ProcessingInstruction:
					this.WriteProcessingInstruction(navigator.LocalName, navigator.Value);
					break;
				case XPathNodeType.Comment:
					this.WriteComment(navigator.Value);
					break;
				}
				if (flag)
				{
					if (navigator.MoveToFirstChild())
					{
						num++;
						continue;
					}
					if (navigator.NodeType == XPathNodeType.Element)
					{
						if (navigator.IsEmptyElement)
						{
							this.WriteEndElement();
						}
						else
						{
							this.WriteFullEndElement();
						}
					}
				}
				while (num != 0)
				{
					if (navigator.MoveToNext())
					{
						goto IL_18;
					}
					num--;
					navigator.MoveToParent();
					if (navigator.NodeType == XPathNodeType.Element)
					{
						this.WriteFullEndElement();
					}
				}
				break;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009A77 File Offset: 0x00008A77
		public void WriteElementString(string localName, string value)
		{
			this.WriteElementString(localName, null, value);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009A82 File Offset: 0x00008A82
		public void WriteElementString(string localName, string ns, string value)
		{
			this.WriteStartElement(localName, ns);
			if (value != null && value.Length != 0)
			{
				this.WriteString(value);
			}
			this.WriteEndElement();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009AA4 File Offset: 0x00008AA4
		public void WriteElementString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartElement(prefix, localName, ns);
			if (value != null && value.Length != 0)
			{
				this.WriteString(value);
			}
			this.WriteEndElement();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009ACA File Offset: 0x00008ACA
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009AD4 File Offset: 0x00008AD4
		protected virtual void Dispose(bool disposing)
		{
			if (this.WriteState != WriteState.Closed)
			{
				try
				{
					this.Close();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009B08 File Offset: 0x00008B08
		private void WriteLocalNamespaces(XPathNavigator nsNav)
		{
			string localName = nsNav.LocalName;
			string value = nsNav.Value;
			if (nsNav.MoveToNextNamespace(XPathNamespaceScope.Local))
			{
				this.WriteLocalNamespaces(nsNav);
			}
			if (localName.Length == 0)
			{
				this.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", value);
				return;
			}
			this.WriteAttributeString("xmlns", localName, "http://www.w3.org/2000/xmlns/", value);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009B64 File Offset: 0x00008B64
		public static XmlWriter Create(string outputFileName)
		{
			return XmlWriter.Create(outputFileName, null);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009B70 File Offset: 0x00008B70
		public static XmlWriter Create(string outputFileName, XmlWriterSettings settings)
		{
			if (outputFileName == null)
			{
				throw new ArgumentNullException("outputFileName");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			FileStream fileStream = null;
			XmlWriter result;
			try
			{
				fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
				result = XmlWriter.CreateWriterImpl(fileStream, settings.Encoding, true, settings);
			}
			catch
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				throw;
			}
			return result;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00009BD0 File Offset: 0x00008BD0
		public static XmlWriter Create(Stream output)
		{
			return XmlWriter.Create(output, null);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009BD9 File Offset: 0x00008BD9
		public static XmlWriter Create(Stream output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.CreateWriterImpl(output, settings.Encoding, settings.CloseOutput, settings);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00009C06 File Offset: 0x00008C06
		public static XmlWriter Create(TextWriter output)
		{
			return XmlWriter.Create(output, null);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009C0F File Offset: 0x00008C0F
		public static XmlWriter Create(TextWriter output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.CreateWriterImpl(output, settings);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009C30 File Offset: 0x00008C30
		public static XmlWriter Create(StringBuilder output)
		{
			return XmlWriter.Create(output, null);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009C39 File Offset: 0x00008C39
		public static XmlWriter Create(StringBuilder output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.CreateWriterImpl(new StringWriter(output, CultureInfo.InvariantCulture), settings);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009C64 File Offset: 0x00008C64
		public static XmlWriter Create(XmlWriter output)
		{
			return XmlWriter.Create(output, null);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009C6D File Offset: 0x00008C6D
		public static XmlWriter Create(XmlWriter output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.AddConformanceWrapper(output, output.Settings, settings);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009C94 File Offset: 0x00008C94
		private static XmlWriter CreateWriterImpl(Stream output, Encoding encoding, bool closeOutput, XmlWriterSettings settings)
		{
			XmlWriter xmlWriter;
			if (encoding.CodePage == 65001)
			{
				switch (settings.OutputMethod)
				{
				case XmlOutputMethod.Xml:
					if (settings.Indent)
					{
						xmlWriter = new XmlUtf8RawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new XmlUtf8RawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Html:
					if (settings.Indent)
					{
						xmlWriter = new HtmlUtf8RawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new HtmlUtf8RawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Text:
					xmlWriter = new TextUtf8RawTextWriter(output, encoding, settings, closeOutput);
					break;
				case XmlOutputMethod.AutoDetect:
					xmlWriter = new XmlAutoDetectWriter(output, encoding, settings);
					break;
				default:
					return null;
				}
			}
			else
			{
				switch (settings.OutputMethod)
				{
				case XmlOutputMethod.Xml:
					if (settings.Indent)
					{
						xmlWriter = new XmlEncodedRawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new XmlEncodedRawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Html:
					if (settings.Indent)
					{
						xmlWriter = new HtmlEncodedRawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new HtmlEncodedRawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Text:
					xmlWriter = new TextEncodedRawTextWriter(output, encoding, settings, closeOutput);
					break;
				case XmlOutputMethod.AutoDetect:
					xmlWriter = new XmlAutoDetectWriter(output, encoding, settings);
					break;
				default:
					return null;
				}
			}
			if (settings.OutputMethod != XmlOutputMethod.AutoDetect && settings.IsQuerySpecific)
			{
				xmlWriter = new QueryOutputWriter((XmlRawWriter)xmlWriter, settings);
			}
			return new XmlWellFormedWriter(xmlWriter, settings);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009DD8 File Offset: 0x00008DD8
		private static XmlWriter CreateWriterImpl(TextWriter output, XmlWriterSettings settings)
		{
			XmlWriter xmlWriter;
			switch (settings.OutputMethod)
			{
			case XmlOutputMethod.Xml:
				if (settings.Indent)
				{
					xmlWriter = new XmlEncodedRawTextWriterIndent(output, settings);
				}
				else
				{
					xmlWriter = new XmlEncodedRawTextWriter(output, settings);
				}
				break;
			case XmlOutputMethod.Html:
				if (settings.Indent)
				{
					xmlWriter = new HtmlEncodedRawTextWriterIndent(output, settings);
				}
				else
				{
					xmlWriter = new HtmlEncodedRawTextWriter(output, settings);
				}
				break;
			case XmlOutputMethod.Text:
				xmlWriter = new TextEncodedRawTextWriter(output, settings);
				break;
			case XmlOutputMethod.AutoDetect:
				xmlWriter = new XmlAutoDetectWriter(output, settings);
				break;
			default:
				return null;
			}
			if (settings.OutputMethod != XmlOutputMethod.AutoDetect && settings.IsQuerySpecific)
			{
				xmlWriter = new QueryOutputWriter((XmlRawWriter)xmlWriter, settings);
			}
			return new XmlWellFormedWriter(xmlWriter, settings);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009E7C File Offset: 0x00008E7C
		private static XmlWriter AddConformanceWrapper(XmlWriter baseWriter, XmlWriterSettings baseWriterSettings, XmlWriterSettings settings)
		{
			ConformanceLevel conformanceLevel = ConformanceLevel.Auto;
			bool flag = false;
			bool checkNames = false;
			bool flag2 = false;
			bool flag3 = false;
			if (baseWriterSettings == null)
			{
				if (settings.NewLineHandling == NewLineHandling.Replace)
				{
					flag2 = true;
					flag3 = true;
				}
				if (settings.CheckCharacters)
				{
					flag = true;
					flag3 = true;
				}
			}
			else
			{
				if (settings.ConformanceLevel != baseWriterSettings.ConformanceLevel)
				{
					conformanceLevel = settings.ConformanceLevel;
					flag3 = true;
				}
				if (settings.CheckCharacters && !baseWriterSettings.CheckCharacters)
				{
					flag = true;
					checkNames = (conformanceLevel == ConformanceLevel.Auto);
					flag3 = true;
				}
				if (settings.NewLineHandling == NewLineHandling.Replace && baseWriterSettings.NewLineHandling == NewLineHandling.None)
				{
					flag2 = true;
					flag3 = true;
				}
			}
			if (flag3)
			{
				XmlWriter xmlWriter = baseWriter;
				if (conformanceLevel != ConformanceLevel.Auto)
				{
					xmlWriter = new XmlWellFormedWriter(xmlWriter, settings);
				}
				if (flag || flag2)
				{
					xmlWriter = new XmlCharCheckingWriter(xmlWriter, flag, checkNames, flag2, settings.NewLineChars);
				}
				return xmlWriter;
			}
			return baseWriter;
		}

		// Token: 0x04000523 RID: 1315
		private const int WriteNodeBufferSize = 1024;

		// Token: 0x04000524 RID: 1316
		private char[] writeNodeBuffer;
	}
}
