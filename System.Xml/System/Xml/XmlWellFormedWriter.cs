using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200009E RID: 158
	internal class XmlWellFormedWriter : XmlWriter
	{
		// Token: 0x06000902 RID: 2306 RVA: 0x00028A34 File Offset: 0x00027A34
		internal XmlWellFormedWriter(XmlWriter writer, XmlWriterSettings settings)
		{
			this.writer = writer;
			this.rawWriter = (writer as XmlRawWriter);
			this.predefinedNamespaces = (writer as IXmlNamespaceResolver);
			if (this.rawWriter != null)
			{
				this.rawWriter.NamespaceResolver = new XmlWellFormedWriter.NamespaceResolverProxy(this);
			}
			this.checkCharacters = settings.CheckCharacters;
			this.conformanceLevel = settings.ConformanceLevel;
			this.stateTable = ((this.conformanceLevel == ConformanceLevel.Document) ? XmlWellFormedWriter.StateTableDocument : XmlWellFormedWriter.StateTableAuto);
			this.currentState = XmlWellFormedWriter.State.Start;
			this.nsStack = new XmlWellFormedWriter.Namespace[8];
			this.nsStack[0].Set("xmlns", "http://www.w3.org/2000/xmlns/", XmlWellFormedWriter.NamespaceKind.Special);
			this.nsStack[1].Set("xml", "http://www.w3.org/XML/1998/namespace", XmlWellFormedWriter.NamespaceKind.Special);
			if (this.predefinedNamespaces == null)
			{
				this.nsStack[2].Set(string.Empty, string.Empty, XmlWellFormedWriter.NamespaceKind.Implied);
			}
			else
			{
				string text = this.predefinedNamespaces.LookupNamespace(string.Empty);
				this.nsStack[2].Set(string.Empty, (text == null) ? string.Empty : text, XmlWellFormedWriter.NamespaceKind.Implied);
			}
			this.nsTop = 2;
			this.elemScopeStack = new XmlWellFormedWriter.ElementScope[8];
			this.elemScopeStack[0].Set(string.Empty, string.Empty, string.Empty, this.nsTop);
			this.elemScopeStack[0].xmlSpace = XmlSpace.None;
			this.elemScopeStack[0].xmlLang = null;
			this.elemTop = 0;
			this.attrStack = new XmlWellFormedWriter.AttrName[8];
			this.attrValue = new StringBuilder();
			this.hasher = new SecureStringHasher();
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00028BE7 File Offset: 0x00027BE7
		public override WriteState WriteState
		{
			get
			{
				if (this.currentState <= XmlWellFormedWriter.State.Error)
				{
					return XmlWellFormedWriter.state2WriteState[(int)this.currentState];
				}
				return WriteState.Error;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00028C04 File Offset: 0x00027C04
		public override XmlWriterSettings Settings
		{
			get
			{
				XmlWriterSettings settings = this.writer.Settings;
				settings.ReadOnly = false;
				settings.ConformanceLevel = this.conformanceLevel;
				settings.ReadOnly = true;
				return settings;
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00028C38 File Offset: 0x00027C38
		public override void WriteStartDocument()
		{
			this.WriteStartDocumentImpl(XmlStandalone.Omit);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00028C41 File Offset: 0x00027C41
		public override void WriteStartDocument(bool standalone)
		{
			this.WriteStartDocumentImpl(standalone ? XmlStandalone.Yes : XmlStandalone.No);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00028C50 File Offset: 0x00027C50
		public override void WriteEndDocument()
		{
			try
			{
				while (this.elemTop > 0)
				{
					this.WriteEndElement();
				}
				XmlWellFormedWriter.State state = this.currentState;
				this.AdvanceState(XmlWellFormedWriter.Token.EndDocument);
				if (state != XmlWellFormedWriter.State.AfterRootEle)
				{
					throw new ArgumentException(Res.GetString("Xml_NoRoot"));
				}
				if (this.rawWriter == null)
				{
					this.writer.WriteEndDocument();
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00028CC0 File Offset: 0x00027CC0
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			try
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyName"));
				}
				XmlConvert.VerifyQName(name);
				if (this.conformanceLevel == ConformanceLevel.Fragment)
				{
					throw new InvalidOperationException(Res.GetString("Xml_DtdNotAllowedInFragment"));
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Dtd);
				if (this.dtdWritten)
				{
					this.currentState = XmlWellFormedWriter.State.Error;
					throw new InvalidOperationException(Res.GetString("Xml_DtdAlreadyWritten"));
				}
				if (this.conformanceLevel == ConformanceLevel.Auto)
				{
					this.conformanceLevel = ConformanceLevel.Document;
					this.stateTable = XmlWellFormedWriter.StateTableDocument;
				}
				if (this.checkCharacters)
				{
					int index;
					if (pubid != null && (index = this.xmlCharType.IsPublicId(pubid)) >= 0)
					{
						throw new ArgumentException(Res.GetString("Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(pubid[index])), "pubid");
					}
					if (sysid != null && (index = this.xmlCharType.IsOnlyCharData(sysid)) >= 0)
					{
						throw new ArgumentException(Res.GetString("Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(sysid[index])), "sysid");
					}
					if (subset != null && (index = this.xmlCharType.IsOnlyCharData(subset)) >= 0)
					{
						throw new ArgumentException(Res.GetString("Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(subset[index])), "subset");
					}
				}
				this.writer.WriteDocType(name, pubid, sysid, subset);
				this.dtdWritten = true;
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00028E38 File Offset: 0x00027E38
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			try
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyLocalName"));
				}
				this.CheckNCName(localName);
				this.AdvanceState(XmlWellFormedWriter.Token.StartElement);
				if (prefix == null)
				{
					if (ns != null)
					{
						prefix = this.LookupPrefix(ns);
					}
					if (prefix == null)
					{
						prefix = string.Empty;
					}
				}
				else if (prefix.Length > 0)
				{
					this.CheckNCName(prefix);
					if (ns == null)
					{
						ns = this.LookupNamespace(prefix);
					}
					if (ns == null || (ns != null && ns.Length == 0))
					{
						throw new ArgumentException(Res.GetString("Xml_PrefixForEmptyNs"));
					}
				}
				if (ns == null)
				{
					ns = this.LookupNamespace(prefix);
					if (ns == null)
					{
						ns = string.Empty;
					}
				}
				if (this.elemTop == 0 && this.rawWriter != null)
				{
					this.rawWriter.OnRootElement(this.conformanceLevel);
				}
				this.writer.WriteStartElement(prefix, localName, ns);
				int num = ++this.elemTop;
				if (num == this.elemScopeStack.Length)
				{
					XmlWellFormedWriter.ElementScope[] destinationArray = new XmlWellFormedWriter.ElementScope[num * 2];
					Array.Copy(this.elemScopeStack, destinationArray, num);
					this.elemScopeStack = destinationArray;
				}
				this.elemScopeStack[num].Set(prefix, localName, ns, this.nsTop);
				this.PushNamespace(prefix, ns, false);
				if (this.attrCount >= 14)
				{
					this.attrHashTable.Clear();
				}
				this.attrCount = 0;
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00028FAC File Offset: 0x00027FAC
		public override void WriteEndElement()
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.EndElement);
				int num = this.elemTop;
				if (num == 0)
				{
					throw new XmlException("Xml_NoStartTag", string.Empty);
				}
				if (this.rawWriter != null)
				{
					this.elemScopeStack[num].WriteEndElement(this.rawWriter);
				}
				else
				{
					this.writer.WriteEndElement();
				}
				int prevNSTop = this.elemScopeStack[num].prevNSTop;
				if (this.useNsHashtable && prevNSTop < this.nsTop)
				{
					this.PopNamespaces(prevNSTop + 1, this.nsTop);
				}
				this.nsTop = prevNSTop;
				if ((this.elemTop = num - 1) == 0)
				{
					if (this.conformanceLevel == ConformanceLevel.Document)
					{
						this.currentState = XmlWellFormedWriter.State.AfterRootEle;
					}
					else
					{
						this.currentState = XmlWellFormedWriter.State.TopLevel;
					}
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00029084 File Offset: 0x00028084
		public override void WriteFullEndElement()
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.EndElement);
				int num = this.elemTop;
				if (num == 0)
				{
					throw new XmlException("Xml_NoStartTag", string.Empty);
				}
				if (this.rawWriter != null)
				{
					this.elemScopeStack[num].WriteFullEndElement(this.rawWriter);
				}
				else
				{
					this.writer.WriteFullEndElement();
				}
				int prevNSTop = this.elemScopeStack[num].prevNSTop;
				if (this.useNsHashtable && prevNSTop < this.nsTop)
				{
					this.PopNamespaces(prevNSTop + 1, this.nsTop);
				}
				this.nsTop = prevNSTop;
				if ((this.elemTop = num - 1) == 0)
				{
					if (this.conformanceLevel == ConformanceLevel.Document)
					{
						this.currentState = XmlWellFormedWriter.State.AfterRootEle;
					}
					else
					{
						this.currentState = XmlWellFormedWriter.State.TopLevel;
					}
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002915C File Offset: 0x0002815C
		public override void WriteStartAttribute(string prefix, string localName, string namespaceName)
		{
			try
			{
				if (localName == null || localName.Length == 0)
				{
					if (!(prefix == "xmlns"))
					{
						throw new ArgumentException(Res.GetString("Xml_EmptyLocalName"));
					}
					localName = "xmlns";
					prefix = string.Empty;
				}
				this.CheckNCName(localName);
				this.AdvanceState(XmlWellFormedWriter.Token.StartAttribute);
				if (prefix == null)
				{
					if (namespaceName != null && (!(localName == "xmlns") || !(namespaceName == "http://www.w3.org/2000/xmlns/")))
					{
						prefix = this.LookupPrefix(namespaceName);
					}
					if (prefix == null)
					{
						prefix = string.Empty;
					}
				}
				if (namespaceName == null)
				{
					if (prefix != null && prefix.Length > 0)
					{
						namespaceName = this.LookupNamespace(prefix);
					}
					if (namespaceName == null)
					{
						namespaceName = string.Empty;
					}
				}
				if (prefix.Length == 0)
				{
					if (localName[0] == 'x' && localName == "xmlns")
					{
						if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/2000/xmlns/")
						{
							throw new ArgumentException(Res.GetString("Xml_XmlnsPrefix"));
						}
						this.curDeclPrefix = string.Empty;
						this.SetSpecialAttribute(XmlWellFormedWriter.SpecialAttribute.DefaultXmlns);
						goto IL_238;
					}
					else if (namespaceName.Length > 0)
					{
						prefix = this.LookupPrefix(namespaceName);
						if (prefix == null || prefix.Length == 0)
						{
							prefix = this.GeneratePrefix();
						}
					}
				}
				else
				{
					if (prefix[0] == 'x')
					{
						if (prefix == "xmlns")
						{
							if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/2000/xmlns/")
							{
								throw new ArgumentException(Res.GetString("Xml_XmlnsPrefix"));
							}
							this.curDeclPrefix = localName;
							this.SetSpecialAttribute(XmlWellFormedWriter.SpecialAttribute.PrefixedXmlns);
							goto IL_238;
						}
						else if (prefix == "xml")
						{
							if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/XML/1998/namespace")
							{
								throw new ArgumentException(Res.GetString("Xml_XmlPrefix"));
							}
							string a;
							if ((a = localName) != null)
							{
								if (a == "space")
								{
									this.SetSpecialAttribute(XmlWellFormedWriter.SpecialAttribute.XmlSpace);
									goto IL_238;
								}
								if (a == "lang")
								{
									this.SetSpecialAttribute(XmlWellFormedWriter.SpecialAttribute.XmlLang);
									goto IL_238;
								}
							}
						}
					}
					this.CheckNCName(prefix);
					if (namespaceName.Length == 0)
					{
						prefix = string.Empty;
					}
					else
					{
						string text = this.LookupLocalNamespace(prefix);
						if (text != null && text != namespaceName)
						{
							prefix = this.GeneratePrefix();
						}
					}
				}
				if (prefix.Length != 0)
				{
					this.PushNamespace(prefix, namespaceName, false);
				}
				this.writer.WriteStartAttribute(prefix, localName, namespaceName);
				IL_238:
				this.AddAttribute(prefix, localName, namespaceName);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000293D4 File Offset: 0x000283D4
		public override void WriteEndAttribute()
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.EndAttribute);
				if (this.specAttr != XmlWellFormedWriter.SpecialAttribute.No)
				{
					string text;
					if (this.attrValue != null)
					{
						text = this.attrValue.ToString();
						this.attrValue.Length = 0;
					}
					else
					{
						text = string.Empty;
					}
					switch (this.specAttr)
					{
					case XmlWellFormedWriter.SpecialAttribute.DefaultXmlns:
						this.PushNamespace(string.Empty, text, true);
						if (this.rawWriter != null)
						{
							this.rawWriter.WriteNamespaceDeclaration(string.Empty, text);
						}
						else
						{
							this.writer.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", text);
						}
						this.curDeclPrefix = null;
						break;
					case XmlWellFormedWriter.SpecialAttribute.PrefixedXmlns:
						if (text.Length == 0)
						{
							throw new ArgumentException(Res.GetString("Xml_PrefixForEmptyNs"));
						}
						if (text == "http://www.w3.org/2000/xmlns/" || (text == "http://www.w3.org/XML/1998/namespace" && this.curDeclPrefix != "xml"))
						{
							throw new ArgumentException(Res.GetString("Xml_CanNotBindToReservedNamespace"));
						}
						this.PushNamespace(this.curDeclPrefix, text, true);
						if (this.rawWriter != null)
						{
							this.rawWriter.WriteNamespaceDeclaration(this.curDeclPrefix, text);
						}
						else
						{
							this.writer.WriteAttributeString("xmlns", this.curDeclPrefix, "http://www.w3.org/2000/xmlns/", text);
						}
						this.curDeclPrefix = null;
						break;
					case XmlWellFormedWriter.SpecialAttribute.XmlSpace:
						text = XmlConvert.TrimString(text);
						if (text == "default")
						{
							this.elemScopeStack[this.elemTop].xmlSpace = XmlSpace.Default;
						}
						else
						{
							if (!(text == "preserve"))
							{
								throw new ArgumentException(Res.GetString("Xml_InvalidXmlSpace", new object[]
								{
									text
								}));
							}
							this.elemScopeStack[this.elemTop].xmlSpace = XmlSpace.Preserve;
						}
						this.writer.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", text);
						break;
					case XmlWellFormedWriter.SpecialAttribute.XmlLang:
						this.elemScopeStack[this.elemTop].xmlLang = text;
						this.writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", text);
						break;
					}
					this.specAttr = XmlWellFormedWriter.SpecialAttribute.No;
				}
				else
				{
					this.writer.WriteEndAttribute();
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00029630 File Offset: 0x00028630
		public override void WriteCData(string text)
		{
			try
			{
				if (text == null)
				{
					text = string.Empty;
				}
				this.AdvanceState(XmlWellFormedWriter.Token.CData);
				this.writer.WriteCData(text);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00029678 File Offset: 0x00028678
		public override void WriteComment(string text)
		{
			try
			{
				if (text == null)
				{
					text = string.Empty;
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Comment);
				this.writer.WriteComment(text);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x000296C0 File Offset: 0x000286C0
		public override void WriteProcessingInstruction(string name, string text)
		{
			try
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyName"));
				}
				this.CheckNCName(name);
				if (text == null)
				{
					text = string.Empty;
				}
				if (name.Length == 3 && string.Compare(name, "xml", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (this.currentState != XmlWellFormedWriter.State.Start)
					{
						throw new ArgumentException(Res.GetString((this.conformanceLevel == ConformanceLevel.Document) ? "Xml_DupXmlDecl" : "Xml_CannotWriteXmlDecl"));
					}
					this.xmlDeclFollows = true;
					this.AdvanceState(XmlWellFormedWriter.Token.PI);
					if (this.rawWriter != null)
					{
						this.rawWriter.WriteXmlDeclaration(text);
					}
					else
					{
						this.writer.WriteProcessingInstruction(name, text);
					}
				}
				else
				{
					this.AdvanceState(XmlWellFormedWriter.Token.PI);
					this.writer.WriteProcessingInstruction(name, text);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002979C File Offset: 0x0002879C
		public override void WriteEntityRef(string name)
		{
			try
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyName"));
				}
				this.CheckNCName(name);
				this.AdvanceState(XmlWellFormedWriter.Token.Text);
				if (this.SaveAttrValue)
				{
					this.attrValue.Append('&');
					this.attrValue.Append(name);
					this.attrValue.Append(';');
				}
				else
				{
					this.writer.WriteEntityRef(name);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00029830 File Offset: 0x00028830
		public override void WriteCharEntity(char ch)
		{
			try
			{
				if (char.IsSurrogate(ch))
				{
					throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Text);
				if (this.SaveAttrValue)
				{
					this.attrValue.Append(ch);
				}
				else
				{
					this.writer.WriteCharEntity(ch);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000298A0 File Offset: 0x000288A0
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			try
			{
				if (!char.IsSurrogatePair(highChar, lowChar))
				{
					throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Text);
				if (this.SaveAttrValue)
				{
					this.attrValue.Append(highChar);
					this.attrValue.Append(lowChar);
				}
				else
				{
					this.writer.WriteSurrogateCharEntity(lowChar, highChar);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00029914 File Offset: 0x00028914
		public override void WriteWhitespace(string ws)
		{
			try
			{
				if (ws == null)
				{
					ws = string.Empty;
				}
				if (!XmlCharType.Instance.IsOnlyWhitespace(ws))
				{
					throw new ArgumentException(Res.GetString("Xml_NonWhitespace"));
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Whitespace);
				if (this.SaveAttrValue)
				{
					this.attrValue.Append(ws);
				}
				else
				{
					this.writer.WriteWhitespace(ws);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00029994 File Offset: 0x00028994
		public override void WriteString(string text)
		{
			try
			{
				if (text != null)
				{
					this.AdvanceState(XmlWellFormedWriter.Token.Text);
					if (this.SaveAttrValue)
					{
						this.attrValue.Append(text);
					}
					else
					{
						this.writer.WriteString(text);
					}
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000299F0 File Offset: 0x000289F0
		public override void WriteChars(char[] buffer, int index, int count)
		{
			try
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (count > buffer.Length - index)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Text);
				if (this.SaveAttrValue)
				{
					this.attrValue.Append(buffer, index, count);
				}
				else
				{
					this.writer.WriteChars(buffer, index, count);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00029A88 File Offset: 0x00028A88
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			try
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (count > buffer.Length - index)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				this.AdvanceState(XmlWellFormedWriter.Token.RawData);
				if (this.SaveAttrValue)
				{
					this.attrValue.Append(buffer, index, count);
				}
				else
				{
					this.writer.WriteRaw(buffer, index, count);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00029B20 File Offset: 0x00028B20
		public override void WriteRaw(string data)
		{
			try
			{
				if (data != null)
				{
					this.AdvanceState(XmlWellFormedWriter.Token.RawData);
					if (this.SaveAttrValue)
					{
						this.attrValue.Append(data);
					}
					else
					{
						this.writer.WriteRaw(data);
					}
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00029B7C File Offset: 0x00028B7C
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			try
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (count > buffer.Length - index)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				this.AdvanceState(XmlWellFormedWriter.Token.Base64);
				this.writer.WriteBase64(buffer, index, count);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00029BFC File Offset: 0x00028BFC
		public override void Close()
		{
			if (this.currentState != XmlWellFormedWriter.State.Closed)
			{
				while (this.currentState != XmlWellFormedWriter.State.Error && this.elemTop > 0)
				{
					this.WriteEndElement();
				}
				this.writer.Flush();
				if (this.rawWriter != null)
				{
					this.rawWriter.Close(this.WriteState);
				}
				else
				{
					this.writer.Close();
				}
				this.currentState = XmlWellFormedWriter.State.Closed;
			}
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00029C68 File Offset: 0x00028C68
		public override void Flush()
		{
			try
			{
				this.writer.Flush();
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00029CA0 File Offset: 0x00028CA0
		public override string LookupPrefix(string ns)
		{
			string result;
			try
			{
				if (ns == null)
				{
					throw new ArgumentNullException("ns");
				}
				for (int i = this.nsTop; i >= 0; i--)
				{
					if (this.nsStack[i].namespaceUri == ns)
					{
						string prefix = this.nsStack[i].prefix;
						for (i++; i <= this.nsTop; i++)
						{
							if (this.nsStack[i].prefix == prefix)
							{
								return null;
							}
						}
						return prefix;
					}
				}
				result = ((this.predefinedNamespaces != null) ? this.predefinedNamespaces.LookupPrefix(ns) : null);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
			return result;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x00029D64 File Offset: 0x00028D64
		public override XmlSpace XmlSpace
		{
			get
			{
				int num = this.elemTop;
				while (num >= 0 && this.elemScopeStack[num].xmlSpace == (XmlSpace)(-1))
				{
					num--;
				}
				return this.elemScopeStack[num].xmlSpace;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00029DA8 File Offset: 0x00028DA8
		public override string XmlLang
		{
			get
			{
				int num = this.elemTop;
				while (num > 0 && this.elemScopeStack[num].xmlLang == null)
				{
					num--;
				}
				return this.elemScopeStack[num].xmlLang;
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00029DEC File Offset: 0x00028DEC
		public override void WriteQualifiedName(string localName, string ns)
		{
			try
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyLocalName"));
				}
				this.CheckNCName(localName);
				this.AdvanceState(XmlWellFormedWriter.Token.Text);
				string text = string.Empty;
				if (ns != null && ns.Length != 0)
				{
					text = this.LookupPrefix(ns);
					if (text == null)
					{
						if (this.currentState != XmlWellFormedWriter.State.Attribute)
						{
							throw new ArgumentException(Res.GetString("Xml_UndefNamespace", new object[]
							{
								ns
							}));
						}
						text = this.GeneratePrefix();
						this.PushNamespace(text, ns, false);
					}
				}
				if (this.SaveAttrValue || this.rawWriter == null)
				{
					if (text.Length != 0)
					{
						this.WriteString(text);
						this.WriteString(":");
					}
					this.WriteString(localName);
				}
				else
				{
					this.rawWriter.WriteQualifiedName(text, localName, ns);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00029ED4 File Offset: 0x00028ED4
		public override void WriteValue(bool value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00029F14 File Offset: 0x00028F14
		public override void WriteValue(DateTime value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00029F54 File Offset: 0x00028F54
		public override void WriteValue(double value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00029F94 File Offset: 0x00028F94
		public override void WriteValue(float value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00029FD4 File Offset: 0x00028FD4
		public override void WriteValue(decimal value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002A014 File Offset: 0x00029014
		public override void WriteValue(int value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002A054 File Offset: 0x00029054
		public override void WriteValue(long value)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
				this.writer.WriteValue(value);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002A094 File Offset: 0x00029094
		public override void WriteValue(string value)
		{
			try
			{
				if (this.SaveAttrValue)
				{
					this.AdvanceState(XmlWellFormedWriter.Token.Text);
					this.attrValue.Append(value);
				}
				else
				{
					this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
					this.writer.WriteValue(value);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002A0F4 File Offset: 0x000290F4
		public override void WriteValue(object value)
		{
			try
			{
				if (this.SaveAttrValue && value is string)
				{
					this.AdvanceState(XmlWellFormedWriter.Token.Text);
					this.attrValue.Append(value);
				}
				else
				{
					this.AdvanceState(XmlWellFormedWriter.Token.AtomicValue);
					this.writer.WriteValue(value);
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0002A15C File Offset: 0x0002915C
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			if (this.IsClosedOrErrorState)
			{
				throw new InvalidOperationException(Res.GetString("Xml_ClosedOrError"));
			}
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.Text);
				base.WriteBinHex(buffer, index, count);
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002A1B0 File Offset: 0x000291B0
		internal XmlWriter InnerWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0002A1B8 File Offset: 0x000291B8
		private bool SaveAttrValue
		{
			get
			{
				return this.specAttr != XmlWellFormedWriter.SpecialAttribute.No;
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002A1C6 File Offset: 0x000291C6
		private void SetSpecialAttribute(XmlWellFormedWriter.SpecialAttribute special)
		{
			this.specAttr = special;
			if (XmlWellFormedWriter.State.Attribute == this.currentState)
			{
				this.currentState = XmlWellFormedWriter.State.SpecialAttr;
				return;
			}
			if (XmlWellFormedWriter.State.RootLevelAttr == this.currentState)
			{
				this.currentState = XmlWellFormedWriter.State.RootLevelSpecAttr;
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002A1F4 File Offset: 0x000291F4
		private void WriteStartDocumentImpl(XmlStandalone standalone)
		{
			try
			{
				this.AdvanceState(XmlWellFormedWriter.Token.StartDocument);
				if (this.conformanceLevel == ConformanceLevel.Auto)
				{
					this.conformanceLevel = ConformanceLevel.Document;
					this.stateTable = XmlWellFormedWriter.StateTableDocument;
				}
				else if (this.conformanceLevel == ConformanceLevel.Fragment)
				{
					throw new InvalidOperationException(Res.GetString("Xml_CannotStartDocumentOnFragment"));
				}
				if (this.rawWriter != null)
				{
					if (!this.xmlDeclFollows)
					{
						this.rawWriter.WriteXmlDeclaration(standalone);
					}
				}
				else
				{
					this.writer.WriteStartDocument();
				}
			}
			catch
			{
				this.currentState = XmlWellFormedWriter.State.Error;
				throw;
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002A284 File Offset: 0x00029284
		private void StartFragment()
		{
			this.conformanceLevel = ConformanceLevel.Fragment;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002A290 File Offset: 0x00029290
		private void PushNamespace(string prefix, string ns, bool explicitlyDefined)
		{
			int num = this.LookupNamespaceIndex(prefix);
			XmlWellFormedWriter.NamespaceKind kind;
			if (num != -1)
			{
				if (num > this.elemScopeStack[this.elemTop].prevNSTop)
				{
					if (this.nsStack[num].namespaceUri != ns)
					{
						throw new XmlException("Xml_RedefinePrefix", new string[]
						{
							prefix,
							this.nsStack[num].namespaceUri,
							ns
						});
					}
					if (explicitlyDefined)
					{
						if (this.nsStack[num].kind == XmlWellFormedWriter.NamespaceKind.Written)
						{
							throw XmlWellFormedWriter.DupAttrException((prefix.Length == 0) ? string.Empty : "xmlns", (prefix.Length == 0) ? "xmlns" : prefix);
						}
						this.nsStack[num].kind = XmlWellFormedWriter.NamespaceKind.Written;
					}
					return;
				}
				else if (!explicitlyDefined)
				{
					if (this.nsStack[num].kind != XmlWellFormedWriter.NamespaceKind.Special)
					{
						kind = ((this.nsStack[num].namespaceUri == ns) ? XmlWellFormedWriter.NamespaceKind.Implied : XmlWellFormedWriter.NamespaceKind.NeedToWrite);
						goto IL_231;
					}
					if (!(prefix == "xml"))
					{
						throw new ArgumentException(Res.GetString("Xml_XmlnsPrefix"));
					}
					if (ns != this.nsStack[num].namespaceUri)
					{
						throw new ArgumentException(Res.GetString("Xml_XmlPrefix"));
					}
					kind = XmlWellFormedWriter.NamespaceKind.Implied;
					goto IL_231;
				}
			}
			if ((ns == "http://www.w3.org/XML/1998/namespace" && prefix != "xml") || (ns == "http://www.w3.org/2000/xmlns/" && prefix != "xmlns"))
			{
				throw new ArgumentException(Res.GetString("Xml_NamespaceDeclXmlXmlns", new object[]
				{
					prefix
				}));
			}
			if (!explicitlyDefined)
			{
				if (this.predefinedNamespaces == null)
				{
					kind = XmlWellFormedWriter.NamespaceKind.NeedToWrite;
				}
				else
				{
					string a = this.predefinedNamespaces.LookupNamespace(prefix);
					kind = ((a == ns) ? XmlWellFormedWriter.NamespaceKind.Implied : XmlWellFormedWriter.NamespaceKind.NeedToWrite);
				}
			}
			else
			{
				if (prefix.Length > 0 && prefix[0] == 'x')
				{
					if (prefix == "xml")
					{
						if (ns != "http://www.w3.org/XML/1998/namespace")
						{
							throw new ArgumentException(Res.GetString("Xml_XmlPrefix"));
						}
					}
					else if (prefix == "xmlns")
					{
						throw new ArgumentException(Res.GetString("Xml_XmlnsPrefix"));
					}
				}
				kind = XmlWellFormedWriter.NamespaceKind.Written;
			}
			IL_231:
			int num2 = ++this.nsTop;
			if (num2 == this.nsStack.Length)
			{
				XmlWellFormedWriter.Namespace[] destinationArray = new XmlWellFormedWriter.Namespace[num2 * 2];
				Array.Copy(this.nsStack, destinationArray, num2);
				this.nsStack = destinationArray;
			}
			this.nsStack[num2].Set(prefix, ns, kind);
			if (this.useNsHashtable)
			{
				this.AddToNamespaceHashtable(this.nsTop);
				return;
			}
			if (this.nsTop == 16)
			{
				this.nsHashtable = new Dictionary<string, int>(this.hasher);
				for (int i = 0; i <= this.nsTop; i++)
				{
					this.AddToNamespaceHashtable(i);
				}
				this.useNsHashtable = true;
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002A578 File Offset: 0x00029578
		private void AddToNamespaceHashtable(int namespaceIndex)
		{
			string prefix = this.nsStack[namespaceIndex].prefix;
			int prevNsIndex;
			if (this.nsHashtable.TryGetValue(prefix, out prevNsIndex))
			{
				this.nsStack[namespaceIndex].prevNsIndex = prevNsIndex;
			}
			this.nsHashtable[prefix] = namespaceIndex;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002A5C8 File Offset: 0x000295C8
		private int LookupNamespaceIndex(string prefix)
		{
			if (this.useNsHashtable)
			{
				int result;
				if (this.nsHashtable.TryGetValue(prefix, out result))
				{
					return result;
				}
			}
			else
			{
				for (int i = this.nsTop; i >= 0; i--)
				{
					if (this.nsStack[i].prefix == prefix)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002A61C File Offset: 0x0002961C
		private void PopNamespaces(int indexFrom, int indexTo)
		{
			for (int i = indexTo; i >= indexFrom; i--)
			{
				if (this.nsStack[i].prevNsIndex == -1)
				{
					this.nsHashtable.Remove(this.nsStack[i].prefix);
				}
				else
				{
					this.nsHashtable[this.nsStack[i].prefix] = this.nsStack[i].prevNsIndex;
				}
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002A698 File Offset: 0x00029698
		private static XmlException DupAttrException(string prefix, string localName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (prefix.Length > 0)
			{
				stringBuilder.Append(prefix);
				stringBuilder.Append(':');
			}
			stringBuilder.Append(localName);
			return new XmlException("Xml_DupAttributeName", stringBuilder.ToString());
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002A6E0 File Offset: 0x000296E0
		private void AdvanceState(XmlWellFormedWriter.Token token)
		{
			if (this.currentState < XmlWellFormedWriter.State.Closed)
			{
				XmlWellFormedWriter.State state;
				for (;;)
				{
					state = this.stateTable[(int)(((int)token << 4) + (int)this.currentState)];
					if (state < XmlWellFormedWriter.State.Error)
					{
						break;
					}
					XmlWellFormedWriter.State state2 = state;
					if (state2 != XmlWellFormedWriter.State.Error)
					{
						switch (state2)
						{
						case XmlWellFormedWriter.State.StartContent:
							goto IL_E7;
						case XmlWellFormedWriter.State.StartContentEle:
							goto IL_F4;
						case XmlWellFormedWriter.State.StartContentB64:
							goto IL_101;
						case XmlWellFormedWriter.State.StartDoc:
							goto IL_10E;
						case XmlWellFormedWriter.State.StartDocEle:
							goto IL_11B;
						case XmlWellFormedWriter.State.EndAttrSEle:
							goto IL_128;
						case XmlWellFormedWriter.State.EndAttrEEle:
							goto IL_13B;
						case XmlWellFormedWriter.State.EndAttrSCont:
							goto IL_14E;
						case XmlWellFormedWriter.State.EndAttrSAttr:
							goto IL_161;
						case XmlWellFormedWriter.State.PostB64Cont:
							if (this.rawWriter != null)
							{
								this.rawWriter.WriteEndBase64();
							}
							this.currentState = XmlWellFormedWriter.State.Content;
							continue;
						case XmlWellFormedWriter.State.PostB64Attr:
							if (this.rawWriter != null)
							{
								this.rawWriter.WriteEndBase64();
							}
							this.currentState = XmlWellFormedWriter.State.Attribute;
							continue;
						case XmlWellFormedWriter.State.PostB64RootAttr:
							if (this.rawWriter != null)
							{
								this.rawWriter.WriteEndBase64();
							}
							this.currentState = XmlWellFormedWriter.State.RootLevelAttr;
							continue;
						case XmlWellFormedWriter.State.StartFragEle:
							goto IL_1CC;
						case XmlWellFormedWriter.State.StartFragCont:
							goto IL_1D6;
						case XmlWellFormedWriter.State.StartFragB64:
							goto IL_1E0;
						case XmlWellFormedWriter.State.StartRootLevelAttr:
							goto IL_1EA;
						}
						break;
					}
					goto IL_D5;
				}
				goto IL_1F3;
				IL_D5:
				this.ThrowInvalidStateTransition(token, this.currentState);
				goto IL_1F3;
				IL_E7:
				this.StartElementContent();
				state = XmlWellFormedWriter.State.Content;
				goto IL_1F3;
				IL_F4:
				this.StartElementContent();
				state = XmlWellFormedWriter.State.Element;
				goto IL_1F3;
				IL_101:
				this.StartElementContent();
				state = XmlWellFormedWriter.State.B64Content;
				goto IL_1F3;
				IL_10E:
				this.WriteStartDocument();
				state = XmlWellFormedWriter.State.Document;
				goto IL_1F3;
				IL_11B:
				this.WriteStartDocument();
				state = XmlWellFormedWriter.State.Element;
				goto IL_1F3;
				IL_128:
				this.WriteEndAttribute();
				this.StartElementContent();
				state = XmlWellFormedWriter.State.Element;
				goto IL_1F3;
				IL_13B:
				this.WriteEndAttribute();
				this.StartElementContent();
				state = XmlWellFormedWriter.State.Content;
				goto IL_1F3;
				IL_14E:
				this.WriteEndAttribute();
				this.StartElementContent();
				state = XmlWellFormedWriter.State.Content;
				goto IL_1F3;
				IL_161:
				this.WriteEndAttribute();
				state = XmlWellFormedWriter.State.Attribute;
				goto IL_1F3;
				IL_1CC:
				this.StartFragment();
				state = XmlWellFormedWriter.State.Element;
				goto IL_1F3;
				IL_1D6:
				this.StartFragment();
				state = XmlWellFormedWriter.State.Content;
				goto IL_1F3;
				IL_1E0:
				this.StartFragment();
				state = XmlWellFormedWriter.State.B64Content;
				goto IL_1F3;
				IL_1EA:
				this.WriteEndAttribute();
				state = XmlWellFormedWriter.State.RootLevelAttr;
				IL_1F3:
				this.currentState = state;
				return;
			}
			if (this.currentState == XmlWellFormedWriter.State.Closed || this.currentState == XmlWellFormedWriter.State.Error)
			{
				throw new InvalidOperationException(Res.GetString("Xml_ClosedOrError"));
			}
			throw new InvalidOperationException(Res.GetString("Xml_WrongToken", new object[]
			{
				XmlWellFormedWriter.tokenName[(int)token],
				XmlWellFormedWriter.GetStateName(this.currentState)
			}));
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002A8E8 File Offset: 0x000298E8
		private void StartElementContent()
		{
			int prevNSTop = this.elemScopeStack[this.elemTop].prevNSTop;
			for (int i = this.nsTop; i > prevNSTop; i--)
			{
				if (this.nsStack[i].kind == XmlWellFormedWriter.NamespaceKind.NeedToWrite)
				{
					this.nsStack[i].WriteDecl(this.writer, this.rawWriter);
				}
			}
			if (this.rawWriter != null)
			{
				this.rawWriter.StartElementContent();
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002A961 File Offset: 0x00029961
		private static string GetStateName(XmlWellFormedWriter.State state)
		{
			if (state >= XmlWellFormedWriter.State.Error)
			{
				return "Error";
			}
			return XmlWellFormedWriter.stateName[(int)state];
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002A978 File Offset: 0x00029978
		internal string LookupNamespace(string prefix)
		{
			for (int i = this.nsTop; i >= 0; i--)
			{
				if (this.nsStack[i].prefix == prefix)
				{
					return this.nsStack[i].namespaceUri;
				}
			}
			if (this.predefinedNamespaces == null)
			{
				return null;
			}
			return this.predefinedNamespaces.LookupNamespace(prefix);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002A9D8 File Offset: 0x000299D8
		private string LookupLocalNamespace(string prefix)
		{
			for (int i = this.nsTop; i > this.elemScopeStack[this.elemTop].prevNSTop; i--)
			{
				if (this.nsStack[i].prefix == prefix)
				{
					return this.nsStack[i].namespaceUri;
				}
			}
			return null;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002AA38 File Offset: 0x00029A38
		private string GeneratePrefix()
		{
			string text = "p" + (this.nsTop - 2).ToString("d", CultureInfo.InvariantCulture);
			if (this.LookupNamespace(text) == null)
			{
				return text;
			}
			int num = 0;
			string text2;
			do
			{
				text2 = text + num.ToString(CultureInfo.InvariantCulture);
				num++;
			}
			while (this.LookupNamespace(text2) != null);
			return text2;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002AA98 File Offset: 0x00029A98
		private unsafe void CheckNCName(string ncname)
		{
			if ((this.xmlCharType.charProperties[ncname[0]] & 4) != 0)
			{
				int i = 1;
				int length = ncname.Length;
				while (i < length)
				{
					if ((this.xmlCharType.charProperties[ncname[i]] & 8) == 0)
					{
						throw XmlWellFormedWriter.InvalidCharsException(ncname, ncname[i]);
					}
					i++;
				}
				return;
			}
			throw XmlWellFormedWriter.InvalidCharsException(ncname, ncname[0]);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002AB04 File Offset: 0x00029B04
		private static Exception InvalidCharsException(string name, char badChar)
		{
			string[] array = new string[3];
			array[0] = name;
			array[1] = badChar.ToString(CultureInfo.InvariantCulture);
			string[] array2 = array;
			int num = 2;
			int num2 = (int)badChar;
			array2[num] = num2.ToString("X2", CultureInfo.InvariantCulture);
			return new ArgumentException(Res.GetString("Xml_InvalidNameCharsDetail", array));
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002AB54 File Offset: 0x00029B54
		private void ThrowInvalidStateTransition(XmlWellFormedWriter.Token token, XmlWellFormedWriter.State currentState)
		{
			string @string = Res.GetString("Xml_WrongToken", new object[]
			{
				XmlWellFormedWriter.tokenName[(int)token],
				XmlWellFormedWriter.GetStateName(currentState)
			});
			if ((currentState == XmlWellFormedWriter.State.Start || currentState == XmlWellFormedWriter.State.AfterRootEle) && this.conformanceLevel == ConformanceLevel.Document)
			{
				throw new InvalidOperationException(@string + ' ' + Res.GetString("Xml_ConformanceLevelFragment"));
			}
			throw new InvalidOperationException(@string);
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0002ABBD File Offset: 0x00029BBD
		private bool IsClosedOrErrorState
		{
			get
			{
				return this.currentState >= XmlWellFormedWriter.State.Closed;
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002ABCC File Offset: 0x00029BCC
		private void AddAttribute(string prefix, string localName, string namespaceName)
		{
			int num = this.attrCount++;
			if (num == this.attrStack.Length)
			{
				XmlWellFormedWriter.AttrName[] destinationArray = new XmlWellFormedWriter.AttrName[num * 2];
				Array.Copy(this.attrStack, destinationArray, num);
				this.attrStack = destinationArray;
			}
			this.attrStack[num].Set(prefix, localName, namespaceName);
			if (this.attrCount < 14)
			{
				for (int i = 0; i < num; i++)
				{
					if (this.attrStack[i].IsDuplicate(prefix, localName, namespaceName))
					{
						throw XmlWellFormedWriter.DupAttrException(prefix, localName);
					}
				}
				return;
			}
			if (this.attrCount == 14)
			{
				if (this.attrHashTable == null)
				{
					this.attrHashTable = new Dictionary<string, int>(this.hasher);
				}
				for (int j = 0; j < num; j++)
				{
					this.AddToAttrHashTable(j);
				}
			}
			this.AddToAttrHashTable(num);
			for (int k = this.attrStack[num].prev; k > 0; k = this.attrStack[k].prev)
			{
				k--;
				if (this.attrStack[k].IsDuplicate(prefix, localName, namespaceName))
				{
					throw XmlWellFormedWriter.DupAttrException(prefix, localName);
				}
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002ACF0 File Offset: 0x00029CF0
		private void AddToAttrHashTable(int attributeIndex)
		{
			string localName = this.attrStack[attributeIndex].localName;
			int count = this.attrHashTable.Count;
			this.attrHashTable[localName] = 0;
			if (count != this.attrHashTable.Count)
			{
				return;
			}
			int num = attributeIndex - 1;
			while (num >= 0 && !(this.attrStack[num].localName == localName))
			{
				num--;
			}
			this.attrStack[attributeIndex].prev = num + 1;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0002AD72 File Offset: 0x00029D72
		internal XmlRawWriter RawWriter
		{
			get
			{
				return this.rawWriter;
			}
		}

		// Token: 0x040007B9 RID: 1977
		private const int ElementStackInitialSize = 8;

		// Token: 0x040007BA RID: 1978
		private const int NamespaceStackInitialSize = 8;

		// Token: 0x040007BB RID: 1979
		private const int AttributeArrayInitialSize = 8;

		// Token: 0x040007BC RID: 1980
		private const int MaxAttrDuplWalkCount = 14;

		// Token: 0x040007BD RID: 1981
		private const int MaxNamespacesWalkCount = 16;

		// Token: 0x040007BE RID: 1982
		private XmlWriter writer;

		// Token: 0x040007BF RID: 1983
		private XmlRawWriter rawWriter;

		// Token: 0x040007C0 RID: 1984
		private IXmlNamespaceResolver predefinedNamespaces;

		// Token: 0x040007C1 RID: 1985
		private XmlWellFormedWriter.Namespace[] nsStack;

		// Token: 0x040007C2 RID: 1986
		private int nsTop;

		// Token: 0x040007C3 RID: 1987
		private Dictionary<string, int> nsHashtable;

		// Token: 0x040007C4 RID: 1988
		private bool useNsHashtable;

		// Token: 0x040007C5 RID: 1989
		private XmlWellFormedWriter.ElementScope[] elemScopeStack;

		// Token: 0x040007C6 RID: 1990
		private int elemTop;

		// Token: 0x040007C7 RID: 1991
		private XmlWellFormedWriter.AttrName[] attrStack;

		// Token: 0x040007C8 RID: 1992
		private int attrCount;

		// Token: 0x040007C9 RID: 1993
		private Dictionary<string, int> attrHashTable;

		// Token: 0x040007CA RID: 1994
		private XmlWellFormedWriter.SpecialAttribute specAttr;

		// Token: 0x040007CB RID: 1995
		private StringBuilder attrValue;

		// Token: 0x040007CC RID: 1996
		private string curDeclPrefix;

		// Token: 0x040007CD RID: 1997
		private XmlWellFormedWriter.State[] stateTable;

		// Token: 0x040007CE RID: 1998
		private XmlWellFormedWriter.State currentState;

		// Token: 0x040007CF RID: 1999
		private bool checkCharacters;

		// Token: 0x040007D0 RID: 2000
		private ConformanceLevel conformanceLevel;

		// Token: 0x040007D1 RID: 2001
		private bool dtdWritten;

		// Token: 0x040007D2 RID: 2002
		private bool xmlDeclFollows;

		// Token: 0x040007D3 RID: 2003
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x040007D4 RID: 2004
		private SecureStringHasher hasher;

		// Token: 0x040007D5 RID: 2005
		internal static readonly string[] stateName = new string[]
		{
			"Start",
			"TopLevel",
			"Document",
			"Element Start Tag",
			"Element Content",
			"Element Content",
			"Attribute",
			"EndRootElement",
			"Attribute",
			"Special Attribute",
			"End Document",
			"Root Level Attribute Value",
			"Root Level Special Attribute Value",
			"Root Level Base64 Attribute Value",
			"After Root Level Attribute",
			"Closed",
			"Error"
		};

		// Token: 0x040007D6 RID: 2006
		internal static readonly string[] tokenName = new string[]
		{
			"StartDocument",
			"EndDocument",
			"PI",
			"Comment",
			"DTD",
			"StartElement",
			"EndElement",
			"StartAttribute",
			"EndAttribute",
			"Text",
			"CDATA",
			"Atomic value",
			"Base64",
			"RawData",
			"Whitespace"
		};

		// Token: 0x040007D7 RID: 2007
		private static WriteState[] state2WriteState = new WriteState[]
		{
			WriteState.Start,
			WriteState.Prolog,
			WriteState.Prolog,
			WriteState.Element,
			WriteState.Content,
			WriteState.Content,
			WriteState.Attribute,
			WriteState.Content,
			WriteState.Attribute,
			WriteState.Attribute,
			WriteState.Content,
			WriteState.Attribute,
			WriteState.Attribute,
			WriteState.Attribute,
			WriteState.Attribute,
			WriteState.Closed,
			WriteState.Error
		};

		// Token: 0x040007D8 RID: 2008
		private static readonly XmlWellFormedWriter.State[] StateTableDocument = new XmlWellFormedWriter.State[]
		{
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndDocument,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDoc,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDoc,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDoc,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDocEle,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.StartContentEle,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndAttrSEle,
			XmlWellFormedWriter.State.EndAttrSEle,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndAttrEEle,
			XmlWellFormedWriter.State.EndAttrEEle,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndAttrSAttr,
			XmlWellFormedWriter.State.EndAttrSAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.SpecialAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContentB64,
			XmlWellFormedWriter.State.B64Content,
			XmlWellFormedWriter.State.B64Content,
			XmlWellFormedWriter.State.B64Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.B64Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDoc,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.SpecialAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDoc,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.SpecialAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error
		};

		// Token: 0x040007D9 RID: 2009
		private static readonly XmlWellFormedWriter.State[] StateTableAuto = new XmlWellFormedWriter.State[]
		{
			XmlWellFormedWriter.State.Document,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndDocument,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartDoc,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartFragEle,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContentEle,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.EndAttrSEle,
			XmlWellFormedWriter.State.EndAttrSEle,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndAttrEEle,
			XmlWellFormedWriter.State.EndAttrEEle,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.EndAttrSAttr,
			XmlWellFormedWriter.State.EndAttrSAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartRootLevelAttr,
			XmlWellFormedWriter.State.StartRootLevelAttr,
			XmlWellFormedWriter.State.PostB64RootAttr,
			XmlWellFormedWriter.State.RootLevelAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.Element,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.AfterRootLevelAttr,
			XmlWellFormedWriter.State.AfterRootLevelAttr,
			XmlWellFormedWriter.State.PostB64RootAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.SpecialAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelAttr,
			XmlWellFormedWriter.State.RootLevelSpecAttr,
			XmlWellFormedWriter.State.PostB64RootAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.EndAttrSCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.PostB64RootAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartFragB64,
			XmlWellFormedWriter.State.StartFragB64,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContentB64,
			XmlWellFormedWriter.State.B64Content,
			XmlWellFormedWriter.State.B64Content,
			XmlWellFormedWriter.State.B64Attribute,
			XmlWellFormedWriter.State.B64Content,
			XmlWellFormedWriter.State.B64Attribute,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelB64Attr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartFragCont,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.SpecialAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelAttr,
			XmlWellFormedWriter.State.RootLevelSpecAttr,
			XmlWellFormedWriter.State.PostB64RootAttr,
			XmlWellFormedWriter.State.AfterRootLevelAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.TopLevel,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.StartContent,
			XmlWellFormedWriter.State.Content,
			XmlWellFormedWriter.State.PostB64Cont,
			XmlWellFormedWriter.State.PostB64Attr,
			XmlWellFormedWriter.State.AfterRootEle,
			XmlWellFormedWriter.State.Attribute,
			XmlWellFormedWriter.State.SpecialAttr,
			XmlWellFormedWriter.State.Error,
			XmlWellFormedWriter.State.RootLevelAttr,
			XmlWellFormedWriter.State.RootLevelSpecAttr,
			XmlWellFormedWriter.State.PostB64RootAttr,
			XmlWellFormedWriter.State.AfterRootLevelAttr,
			XmlWellFormedWriter.State.Error
		};

		// Token: 0x0200009F RID: 159
		private class NamespaceResolverProxy : IXmlNamespaceResolver
		{
			// Token: 0x06000942 RID: 2370 RVA: 0x0002BDA0 File Offset: 0x0002ADA0
			internal NamespaceResolverProxy(XmlWellFormedWriter wfWriter)
			{
				this.wfWriter = wfWriter;
			}

			// Token: 0x06000943 RID: 2371 RVA: 0x0002BDAF File Offset: 0x0002ADAF
			IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000944 RID: 2372 RVA: 0x0002BDB6 File Offset: 0x0002ADB6
			string IXmlNamespaceResolver.LookupNamespace(string prefix)
			{
				return this.wfWriter.LookupNamespace(prefix);
			}

			// Token: 0x06000945 RID: 2373 RVA: 0x0002BDC4 File Offset: 0x0002ADC4
			string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
			{
				return this.wfWriter.LookupPrefix(namespaceName);
			}

			// Token: 0x040007DA RID: 2010
			private XmlWellFormedWriter wfWriter;
		}

		// Token: 0x020000A0 RID: 160
		private struct ElementScope
		{
			// Token: 0x06000946 RID: 2374 RVA: 0x0002BDD2 File Offset: 0x0002ADD2
			internal void Set(string prefix, string localName, string namespaceUri, int prevNSTop)
			{
				this.prevNSTop = prevNSTop;
				this.prefix = prefix;
				this.namespaceUri = namespaceUri;
				this.localName = localName;
				this.xmlSpace = (XmlSpace)(-1);
				this.xmlLang = null;
			}

			// Token: 0x06000947 RID: 2375 RVA: 0x0002BDFF File Offset: 0x0002ADFF
			internal void WriteEndElement(XmlRawWriter rawWriter)
			{
				rawWriter.WriteEndElement(this.prefix, this.localName, this.namespaceUri);
			}

			// Token: 0x06000948 RID: 2376 RVA: 0x0002BE19 File Offset: 0x0002AE19
			internal void WriteFullEndElement(XmlRawWriter rawWriter)
			{
				rawWriter.WriteFullEndElement(this.prefix, this.localName, this.namespaceUri);
			}

			// Token: 0x040007DB RID: 2011
			internal int prevNSTop;

			// Token: 0x040007DC RID: 2012
			internal string prefix;

			// Token: 0x040007DD RID: 2013
			internal string localName;

			// Token: 0x040007DE RID: 2014
			internal string namespaceUri;

			// Token: 0x040007DF RID: 2015
			internal XmlSpace xmlSpace;

			// Token: 0x040007E0 RID: 2016
			internal string xmlLang;
		}

		// Token: 0x020000A1 RID: 161
		private enum NamespaceKind
		{
			// Token: 0x040007E2 RID: 2018
			Written,
			// Token: 0x040007E3 RID: 2019
			NeedToWrite,
			// Token: 0x040007E4 RID: 2020
			Implied,
			// Token: 0x040007E5 RID: 2021
			Special
		}

		// Token: 0x020000A2 RID: 162
		private struct Namespace
		{
			// Token: 0x06000949 RID: 2377 RVA: 0x0002BE33 File Offset: 0x0002AE33
			internal void Set(string prefix, string namespaceUri, XmlWellFormedWriter.NamespaceKind kind)
			{
				this.prefix = prefix;
				this.namespaceUri = namespaceUri;
				this.kind = kind;
				this.prevNsIndex = -1;
			}

			// Token: 0x0600094A RID: 2378 RVA: 0x0002BE54 File Offset: 0x0002AE54
			internal void WriteDecl(XmlWriter writer, XmlRawWriter rawWriter)
			{
				if (rawWriter != null)
				{
					rawWriter.WriteNamespaceDeclaration(this.prefix, this.namespaceUri);
					return;
				}
				if (this.prefix.Length == 0)
				{
					writer.WriteStartAttribute(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/");
				}
				else
				{
					writer.WriteStartAttribute("xmlns", this.prefix, "http://www.w3.org/2000/xmlns/");
				}
				writer.WriteString(this.namespaceUri);
				writer.WriteEndAttribute();
			}

			// Token: 0x040007E6 RID: 2022
			internal string prefix;

			// Token: 0x040007E7 RID: 2023
			internal string namespaceUri;

			// Token: 0x040007E8 RID: 2024
			internal XmlWellFormedWriter.NamespaceKind kind;

			// Token: 0x040007E9 RID: 2025
			internal int prevNsIndex;
		}

		// Token: 0x020000A3 RID: 163
		private struct AttrName
		{
			// Token: 0x0600094B RID: 2379 RVA: 0x0002BEC3 File Offset: 0x0002AEC3
			internal void Set(string prefix, string localName, string namespaceUri)
			{
				this.prefix = prefix;
				this.namespaceUri = namespaceUri;
				this.localName = localName;
				this.prev = 0;
			}

			// Token: 0x0600094C RID: 2380 RVA: 0x0002BEE1 File Offset: 0x0002AEE1
			internal bool IsDuplicate(string prefix, string localName, string namespaceUri)
			{
				return this.localName == localName && (this.prefix == prefix || this.namespaceUri == namespaceUri);
			}

			// Token: 0x040007EA RID: 2026
			internal string prefix;

			// Token: 0x040007EB RID: 2027
			internal string namespaceUri;

			// Token: 0x040007EC RID: 2028
			internal string localName;

			// Token: 0x040007ED RID: 2029
			internal int prev;
		}

		// Token: 0x020000A4 RID: 164
		private enum State
		{
			// Token: 0x040007EF RID: 2031
			Start,
			// Token: 0x040007F0 RID: 2032
			TopLevel,
			// Token: 0x040007F1 RID: 2033
			Document,
			// Token: 0x040007F2 RID: 2034
			Element,
			// Token: 0x040007F3 RID: 2035
			Content,
			// Token: 0x040007F4 RID: 2036
			B64Content,
			// Token: 0x040007F5 RID: 2037
			B64Attribute,
			// Token: 0x040007F6 RID: 2038
			AfterRootEle,
			// Token: 0x040007F7 RID: 2039
			Attribute,
			// Token: 0x040007F8 RID: 2040
			SpecialAttr,
			// Token: 0x040007F9 RID: 2041
			EndDocument,
			// Token: 0x040007FA RID: 2042
			RootLevelAttr,
			// Token: 0x040007FB RID: 2043
			RootLevelSpecAttr,
			// Token: 0x040007FC RID: 2044
			RootLevelB64Attr,
			// Token: 0x040007FD RID: 2045
			AfterRootLevelAttr,
			// Token: 0x040007FE RID: 2046
			Closed,
			// Token: 0x040007FF RID: 2047
			Error,
			// Token: 0x04000800 RID: 2048
			StartContent = 101,
			// Token: 0x04000801 RID: 2049
			StartContentEle,
			// Token: 0x04000802 RID: 2050
			StartContentB64,
			// Token: 0x04000803 RID: 2051
			StartDoc,
			// Token: 0x04000804 RID: 2052
			StartDocEle = 106,
			// Token: 0x04000805 RID: 2053
			EndAttrSEle,
			// Token: 0x04000806 RID: 2054
			EndAttrEEle,
			// Token: 0x04000807 RID: 2055
			EndAttrSCont,
			// Token: 0x04000808 RID: 2056
			EndAttrSAttr = 111,
			// Token: 0x04000809 RID: 2057
			PostB64Cont,
			// Token: 0x0400080A RID: 2058
			PostB64Attr,
			// Token: 0x0400080B RID: 2059
			PostB64RootAttr,
			// Token: 0x0400080C RID: 2060
			StartFragEle,
			// Token: 0x0400080D RID: 2061
			StartFragCont,
			// Token: 0x0400080E RID: 2062
			StartFragB64,
			// Token: 0x0400080F RID: 2063
			StartRootLevelAttr
		}

		// Token: 0x020000A5 RID: 165
		private enum Token
		{
			// Token: 0x04000811 RID: 2065
			StartDocument,
			// Token: 0x04000812 RID: 2066
			EndDocument,
			// Token: 0x04000813 RID: 2067
			PI,
			// Token: 0x04000814 RID: 2068
			Comment,
			// Token: 0x04000815 RID: 2069
			Dtd,
			// Token: 0x04000816 RID: 2070
			StartElement,
			// Token: 0x04000817 RID: 2071
			EndElement,
			// Token: 0x04000818 RID: 2072
			StartAttribute,
			// Token: 0x04000819 RID: 2073
			EndAttribute,
			// Token: 0x0400081A RID: 2074
			Text,
			// Token: 0x0400081B RID: 2075
			CData,
			// Token: 0x0400081C RID: 2076
			AtomicValue,
			// Token: 0x0400081D RID: 2077
			Base64,
			// Token: 0x0400081E RID: 2078
			RawData,
			// Token: 0x0400081F RID: 2079
			Whitespace
		}

		// Token: 0x020000A6 RID: 166
		private enum SpecialAttribute
		{
			// Token: 0x04000821 RID: 2081
			No,
			// Token: 0x04000822 RID: 2082
			DefaultXmlns,
			// Token: 0x04000823 RID: 2083
			PrefixedXmlns,
			// Token: 0x04000824 RID: 2084
			XmlSpace,
			// Token: 0x04000825 RID: 2085
			XmlLang
		}
	}
}
