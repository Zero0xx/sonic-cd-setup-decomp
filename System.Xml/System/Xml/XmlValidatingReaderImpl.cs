using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x0200009C RID: 156
	internal sealed class XmlValidatingReaderImpl : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		// Token: 0x060008A9 RID: 2217 RVA: 0x00027C38 File Offset: 0x00026C38
		internal XmlValidatingReaderImpl(XmlReader reader)
		{
			this.outerReader = this;
			this.coreReader = reader;
			this.coreReaderNSResolver = (reader as IXmlNamespaceResolver);
			this.coreReaderImpl = (reader as XmlTextReaderImpl);
			if (this.coreReaderImpl == null)
			{
				XmlTextReader xmlTextReader = reader as XmlTextReader;
				if (xmlTextReader != null)
				{
					this.coreReaderImpl = xmlTextReader.Impl;
				}
			}
			if (this.coreReaderImpl == null)
			{
				throw new ArgumentException(Res.GetString("Arg_ExpectingXmlTextReader"), "reader");
			}
			this.coreReaderImpl.EntityHandling = EntityHandling.ExpandEntities;
			this.coreReaderImpl.XmlValidatingReaderCompatibilityMode = true;
			this.processIdentityConstraints = true;
			this.schemaCollection = new XmlSchemaCollection(this.coreReader.NameTable);
			this.schemaCollection.XmlResolver = this.GetResolver();
			this.internalEventHandler = new ValidationEventHandler(this.InternalValidationCallback);
			this.eventHandler = this.internalEventHandler;
			this.coreReaderImpl.ValidationEventHandler = this.internalEventHandler;
			this.validationType = ValidationType.Auto;
			this.SetupValidation(ValidationType.Auto);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00027D38 File Offset: 0x00026D38
		internal XmlValidatingReaderImpl(string xmlFragment, XmlNodeType fragType, XmlParserContext context) : this(new XmlTextReader(xmlFragment, fragType, context))
		{
			if (this.coreReader.BaseURI.Length > 0)
			{
				this.validator.BaseUri = this.GetResolver().ResolveUri(null, this.coreReader.BaseURI);
			}
			if (context != null)
			{
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.ParseDtdFromContext;
				this.parserContext = context;
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00027D9C File Offset: 0x00026D9C
		internal XmlValidatingReaderImpl(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context) : this(new XmlTextReader(xmlFragment, fragType, context))
		{
			if (this.coreReader.BaseURI.Length > 0)
			{
				this.validator.BaseUri = this.GetResolver().ResolveUri(null, this.coreReader.BaseURI);
			}
			if (context != null)
			{
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.ParseDtdFromContext;
				this.parserContext = context;
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00027E00 File Offset: 0x00026E00
		internal XmlValidatingReaderImpl(XmlReader reader, ValidationEventHandler settingsEventHandler, bool processIdentityConstraints)
		{
			this.outerReader = this;
			this.coreReader = reader;
			this.coreReaderImpl = (reader as XmlTextReaderImpl);
			if (this.coreReaderImpl == null)
			{
				XmlTextReader xmlTextReader = reader as XmlTextReader;
				if (xmlTextReader != null)
				{
					this.coreReaderImpl = xmlTextReader.Impl;
				}
			}
			if (this.coreReaderImpl == null)
			{
				throw new ArgumentException(Res.GetString("Arg_ExpectingXmlTextReader"), "reader");
			}
			this.coreReaderImpl.XmlValidatingReaderCompatibilityMode = true;
			this.coreReaderNSResolver = (reader as IXmlNamespaceResolver);
			this.processIdentityConstraints = processIdentityConstraints;
			this.schemaCollection = new XmlSchemaCollection(this.coreReader.NameTable);
			this.schemaCollection.XmlResolver = this.GetResolver();
			if (settingsEventHandler == null)
			{
				this.internalEventHandler = new ValidationEventHandler(this.InternalValidationCallback);
				this.eventHandler = this.internalEventHandler;
				this.coreReaderImpl.ValidationEventHandler = this.internalEventHandler;
			}
			else
			{
				this.eventHandler = settingsEventHandler;
				this.coreReaderImpl.ValidationEventHandler = settingsEventHandler;
			}
			this.validationType = ValidationType.DTD;
			this.SetupValidation(ValidationType.DTD);
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00027F0C File Offset: 0x00026F0C
		public override XmlReaderSettings Settings
		{
			get
			{
				XmlReaderSettings xmlReaderSettings;
				if (this.coreReaderImpl.V1Compat)
				{
					xmlReaderSettings = null;
				}
				else
				{
					xmlReaderSettings = this.coreReader.Settings;
				}
				if (xmlReaderSettings != null)
				{
					xmlReaderSettings = xmlReaderSettings.Clone();
				}
				else
				{
					xmlReaderSettings = new XmlReaderSettings();
				}
				xmlReaderSettings.ValidationType = ValidationType.DTD;
				if (!this.processIdentityConstraints)
				{
					xmlReaderSettings.ValidationFlags &= ~XmlSchemaValidationFlags.ProcessIdentityConstraints;
				}
				xmlReaderSettings.ReadOnly = true;
				return xmlReaderSettings;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00027F6E File Offset: 0x00026F6E
		public override XmlNodeType NodeType
		{
			get
			{
				return this.coreReader.NodeType;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00027F7B File Offset: 0x00026F7B
		public override string Name
		{
			get
			{
				return this.coreReader.Name;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00027F88 File Offset: 0x00026F88
		public override string LocalName
		{
			get
			{
				return this.coreReader.LocalName;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00027F95 File Offset: 0x00026F95
		public override string NamespaceURI
		{
			get
			{
				return this.coreReader.NamespaceURI;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00027FA2 File Offset: 0x00026FA2
		public override string Prefix
		{
			get
			{
				return this.coreReader.Prefix;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00027FAF File Offset: 0x00026FAF
		public override bool HasValue
		{
			get
			{
				return this.coreReader.HasValue;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00027FBC File Offset: 0x00026FBC
		public override string Value
		{
			get
			{
				return this.coreReader.Value;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00027FC9 File Offset: 0x00026FC9
		public override int Depth
		{
			get
			{
				return this.coreReader.Depth;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00027FD6 File Offset: 0x00026FD6
		public override string BaseURI
		{
			get
			{
				return this.coreReader.BaseURI;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00027FE3 File Offset: 0x00026FE3
		public override bool IsEmptyElement
		{
			get
			{
				return this.coreReader.IsEmptyElement;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00027FF0 File Offset: 0x00026FF0
		public override bool IsDefault
		{
			get
			{
				return this.coreReader.IsDefault;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00027FFD File Offset: 0x00026FFD
		public override char QuoteChar
		{
			get
			{
				return this.coreReader.QuoteChar;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0002800A File Offset: 0x0002700A
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.coreReader.XmlSpace;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00028017 File Offset: 0x00027017
		public override string XmlLang
		{
			get
			{
				return this.coreReader.XmlLang;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00028024 File Offset: 0x00027024
		public override ReadState ReadState
		{
			get
			{
				if (this.parsingFunction != XmlValidatingReaderImpl.ParsingFunction.Init)
				{
					return this.coreReader.ReadState;
				}
				return ReadState.Initial;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0002803C File Offset: 0x0002703C
		public override bool EOF
		{
			get
			{
				return this.coreReader.EOF;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00028049 File Offset: 0x00027049
		public override XmlNameTable NameTable
		{
			get
			{
				return this.coreReader.NameTable;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00028056 File Offset: 0x00027056
		internal Encoding Encoding
		{
			get
			{
				return this.coreReaderImpl.Encoding;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00028063 File Offset: 0x00027063
		public override int AttributeCount
		{
			get
			{
				return this.coreReader.AttributeCount;
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00028070 File Offset: 0x00027070
		public override string GetAttribute(string name)
		{
			return this.coreReader.GetAttribute(name);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002807E File Offset: 0x0002707E
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.coreReader.GetAttribute(localName, namespaceURI);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0002808D File Offset: 0x0002708D
		public override string GetAttribute(int i)
		{
			return this.coreReader.GetAttribute(i);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002809B File Offset: 0x0002709B
		public override bool MoveToAttribute(string name)
		{
			if (!this.coreReader.MoveToAttribute(name))
			{
				return false;
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			return true;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000280B5 File Offset: 0x000270B5
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			if (!this.coreReader.MoveToAttribute(localName, namespaceURI))
			{
				return false;
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			return true;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000280D0 File Offset: 0x000270D0
		public override void MoveToAttribute(int i)
		{
			this.coreReader.MoveToAttribute(i);
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x000280E5 File Offset: 0x000270E5
		public override bool MoveToFirstAttribute()
		{
			if (!this.coreReader.MoveToFirstAttribute())
			{
				return false;
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			return true;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000280FE File Offset: 0x000270FE
		public override bool MoveToNextAttribute()
		{
			if (!this.coreReader.MoveToNextAttribute())
			{
				return false;
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			return true;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00028117 File Offset: 0x00027117
		public override bool MoveToElement()
		{
			if (!this.coreReader.MoveToElement())
			{
				return false;
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			return true;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00028130 File Offset: 0x00027130
		public override bool Read()
		{
			switch (this.parsingFunction)
			{
			case XmlValidatingReaderImpl.ParsingFunction.Read:
				break;
			case XmlValidatingReaderImpl.ParsingFunction.Init:
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
				if (this.coreReader.ReadState == ReadState.Interactive)
				{
					this.ProcessCoreReaderEvent();
					return true;
				}
				break;
			case XmlValidatingReaderImpl.ParsingFunction.ParseDtdFromContext:
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
				this.ParseDtdFromParserContext();
				break;
			case XmlValidatingReaderImpl.ParsingFunction.ResolveEntityInternally:
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
				this.ResolveEntityInternally();
				break;
			case XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent:
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
				this.readBinaryHelper.Finish();
				break;
			case XmlValidatingReaderImpl.ParsingFunction.ReaderClosed:
			case XmlValidatingReaderImpl.ParsingFunction.Error:
				return false;
			default:
				return false;
			}
			if (this.coreReader.Read())
			{
				this.ProcessCoreReaderEvent();
				return true;
			}
			this.validator.CompleteValidation();
			return false;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000281DC File Offset: 0x000271DC
		public override void Close()
		{
			this.coreReader.Close();
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.ReaderClosed;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000281F0 File Offset: 0x000271F0
		public override string LookupNamespace(string prefix)
		{
			return this.coreReaderImpl.LookupNamespace(prefix);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000281FE File Offset: 0x000271FE
		public override bool ReadAttributeValue()
		{
			if (this.parsingFunction == XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent)
			{
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
				this.readBinaryHelper.Finish();
			}
			if (!this.coreReader.ReadAttributeValue())
			{
				return false;
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			return true;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00028232 File Offset: 0x00027232
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00028238 File Offset: 0x00027238
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.parsingFunction != XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this.outerReader);
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			int result = this.readBinaryHelper.ReadContentAsBase64(buffer, index, count);
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent;
			return result;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00028290 File Offset: 0x00027290
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.parsingFunction != XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this.outerReader);
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			int result = this.readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent;
			return result;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000282E8 File Offset: 0x000272E8
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.parsingFunction != XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this.outerReader);
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			int result = this.readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent;
			return result;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00028340 File Offset: 0x00027340
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.parsingFunction != XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this.outerReader);
			}
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			int result = this.readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.InReadBinaryContent;
			return result;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00028396 File Offset: 0x00027396
		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00028399 File Offset: 0x00027399
		public override void ResolveEntity()
		{
			if (this.parsingFunction == XmlValidatingReaderImpl.ParsingFunction.ResolveEntityInternally)
			{
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Read;
			}
			this.coreReader.ResolveEntity();
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x000283B6 File Offset: 0x000273B6
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x000283BE File Offset: 0x000273BE
		internal XmlReader OuterReader
		{
			get
			{
				return this.outerReader;
			}
			set
			{
				this.outerReader = value;
			}
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000283C7 File Offset: 0x000273C7
		internal void MoveOffEntityReference()
		{
			if (this.outerReader.NodeType == XmlNodeType.EntityReference && this.parsingFunction != XmlValidatingReaderImpl.ParsingFunction.ResolveEntityInternally && !this.outerReader.Read())
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000283FD File Offset: 0x000273FD
		public override string ReadString()
		{
			this.MoveOffEntityReference();
			return base.ReadString();
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002840B File Offset: 0x0002740B
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0002840E File Offset: 0x0002740E
		public int LineNumber
		{
			get
			{
				return ((IXmlLineInfo)this.coreReader).LineNumber;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00028420 File Offset: 0x00027420
		public int LinePosition
		{
			get
			{
				return ((IXmlLineInfo)this.coreReader).LinePosition;
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00028432 File Offset: 0x00027432
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.GetNamespacesInScope(scope);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002843B File Offset: 0x0002743B
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.LookupNamespace(prefix);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00028444 File Offset: 0x00027444
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.LookupPrefix(namespaceName);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002844D File Offset: 0x0002744D
		internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.coreReaderNSResolver.GetNamespacesInScope(scope);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002845B File Offset: 0x0002745B
		internal string LookupPrefix(string namespaceName)
		{
			return this.coreReaderNSResolver.LookupPrefix(namespaceName);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060008E1 RID: 2273 RVA: 0x0002846C File Offset: 0x0002746C
		// (remove) Token: 0x060008E2 RID: 2274 RVA: 0x000284C6 File Offset: 0x000274C6
		internal event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.eventHandler = (ValidationEventHandler)Delegate.Remove(this.eventHandler, this.internalEventHandler);
				this.eventHandler = (ValidationEventHandler)Delegate.Combine(this.eventHandler, value);
				if (this.eventHandler == null)
				{
					this.eventHandler = this.internalEventHandler;
				}
				this.UpdateHandlers();
			}
			remove
			{
				this.eventHandler = (ValidationEventHandler)Delegate.Remove(this.eventHandler, value);
				if (this.eventHandler == null)
				{
					this.eventHandler = this.internalEventHandler;
				}
				this.UpdateHandlers();
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x000284FC File Offset: 0x000274FC
		internal object SchemaType
		{
			get
			{
				if (this.validationType == ValidationType.None)
				{
					return null;
				}
				XmlSchemaType xmlSchemaType = this.coreReaderImpl.InternalSchemaType as XmlSchemaType;
				if (xmlSchemaType != null && xmlSchemaType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
				{
					return xmlSchemaType.Datatype;
				}
				return this.coreReaderImpl.InternalSchemaType;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00028550 File Offset: 0x00027550
		internal XmlReader Reader
		{
			get
			{
				return this.coreReader;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00028558 File Offset: 0x00027558
		internal XmlTextReaderImpl ReaderImpl
		{
			get
			{
				return this.coreReaderImpl;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00028560 File Offset: 0x00027560
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x00028568 File Offset: 0x00027568
		internal ValidationType ValidationType
		{
			get
			{
				return this.validationType;
			}
			set
			{
				if (this.ReadState != ReadState.Initial)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				this.validationType = value;
				this.SetupValidation(value);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00028590 File Offset: 0x00027590
		internal XmlSchemaCollection Schemas
		{
			get
			{
				return this.schemaCollection;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00028598 File Offset: 0x00027598
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x000285A5 File Offset: 0x000275A5
		internal EntityHandling EntityHandling
		{
			get
			{
				return this.coreReaderImpl.EntityHandling;
			}
			set
			{
				this.coreReaderImpl.EntityHandling = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x000285B3 File Offset: 0x000275B3
		internal XmlResolver XmlResolver
		{
			set
			{
				this.coreReaderImpl.XmlResolver = value;
				this.validator.XmlResolver = value;
				this.schemaCollection.XmlResolver = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x000285D9 File Offset: 0x000275D9
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x000285E6 File Offset: 0x000275E6
		internal bool Namespaces
		{
			get
			{
				return this.coreReaderImpl.Namespaces;
			}
			set
			{
				this.coreReaderImpl.Namespaces = value;
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x000285F4 File Offset: 0x000275F4
		public object ReadTypedValue()
		{
			if (this.validationType == ValidationType.None)
			{
				return null;
			}
			XmlNodeType nodeType = this.outerReader.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
			{
				if (this.SchemaType == null)
				{
					return null;
				}
				XmlSchemaDatatype xmlSchemaDatatype = (this.SchemaType is XmlSchemaDatatype) ? ((XmlSchemaDatatype)this.SchemaType) : ((XmlSchemaType)this.SchemaType).Datatype;
				if (xmlSchemaDatatype != null)
				{
					if (!this.outerReader.IsEmptyElement)
					{
						while (this.outerReader.Read())
						{
							XmlNodeType nodeType2 = this.outerReader.NodeType;
							if (nodeType2 != XmlNodeType.CDATA && nodeType2 != XmlNodeType.Text && nodeType2 != XmlNodeType.Whitespace && nodeType2 != XmlNodeType.SignificantWhitespace && nodeType2 != XmlNodeType.Comment && nodeType2 != XmlNodeType.ProcessingInstruction)
							{
								if (this.outerReader.NodeType != XmlNodeType.EndElement)
								{
									throw new XmlException("Xml_InvalidNodeType", this.outerReader.NodeType.ToString());
								}
								goto IL_F9;
							}
						}
						throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
					}
					IL_F9:
					return this.coreReaderImpl.InternalTypedValue;
				}
				return null;
			}
			case XmlNodeType.Attribute:
				return this.coreReaderImpl.InternalTypedValue;
			default:
				if (nodeType == XmlNodeType.EndElement)
				{
					return null;
				}
				if (this.coreReaderImpl.V1Compat)
				{
					return null;
				}
				return this.Value;
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00028720 File Offset: 0x00027720
		private void ParseDtdFromParserContext()
		{
			if (this.parserContext.DocTypeName == null || this.parserContext.DocTypeName.Length == 0)
			{
				return;
			}
			this.coreReaderImpl.DtdSchemaInfo = DtdParser.Parse(this.coreReaderImpl, this.parserContext.BaseURI, this.parserContext.DocTypeName, this.parserContext.PublicId, this.parserContext.SystemId, this.parserContext.InternalSubset);
			this.ValidateDtd();
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000287A0 File Offset: 0x000277A0
		private void ValidateDtd()
		{
			SchemaInfo dtdSchemaInfo = this.coreReaderImpl.DtdSchemaInfo;
			if (dtdSchemaInfo != null)
			{
				switch (this.validationType)
				{
				case ValidationType.None:
				case ValidationType.DTD:
					break;
				case ValidationType.Auto:
					this.SetupValidation(ValidationType.DTD);
					break;
				default:
					return;
				}
				this.validator.SchemaInfo = dtdSchemaInfo;
			}
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000287EC File Offset: 0x000277EC
		private void ResolveEntityInternally()
		{
			int depth = this.coreReader.Depth;
			this.outerReader.ResolveEntity();
			while (this.outerReader.Read() && this.coreReader.Depth > depth)
			{
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002882B File Offset: 0x0002782B
		private void UpdateHandlers()
		{
			this.validator.EventHandler = this.eventHandler;
			this.coreReaderImpl.ValidationEventHandler = ((this.validationType != ValidationType.None) ? this.eventHandler : null);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002885C File Offset: 0x0002785C
		private void SetupValidation(ValidationType valType)
		{
			this.validator = BaseValidator.CreateInstance(valType, this, this.schemaCollection, this.eventHandler, this.processIdentityConstraints);
			XmlResolver resolver = this.GetResolver();
			this.validator.XmlResolver = resolver;
			if (this.outerReader.BaseURI.Length > 0)
			{
				this.validator.BaseUri = ((resolver == null) ? new Uri(this.outerReader.BaseURI, UriKind.RelativeOrAbsolute) : resolver.ResolveUri(null, this.outerReader.BaseURI));
			}
			this.UpdateHandlers();
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000288E7 File Offset: 0x000278E7
		private XmlResolver GetResolver()
		{
			return this.coreReaderImpl.GetResolver();
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000288F4 File Offset: 0x000278F4
		private void InternalValidationCallback(object sender, ValidationEventArgs e)
		{
			if (this.validationType != ValidationType.None && e.Severity == XmlSeverityType.Error)
			{
				throw e.Exception;
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00028910 File Offset: 0x00027910
		private void ProcessCoreReaderEvent()
		{
			XmlNodeType nodeType = this.coreReader.NodeType;
			if (nodeType != XmlNodeType.EntityReference)
			{
				if (nodeType == XmlNodeType.DocumentType)
				{
					this.ValidateDtd();
					return;
				}
				if (nodeType == XmlNodeType.Whitespace && (this.coreReader.Depth > 0 || this.coreReaderImpl.FragmentType != XmlNodeType.Document) && this.validator.PreserveWhitespace)
				{
					this.coreReaderImpl.ChangeCurrentNodeType(XmlNodeType.SignificantWhitespace);
				}
			}
			else
			{
				this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.ResolveEntityInternally;
			}
			this.coreReaderImpl.InternalSchemaType = null;
			this.coreReaderImpl.InternalTypedValue = null;
			this.validator.Validate();
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000289A1 File Offset: 0x000279A1
		internal void Close(bool closeStream)
		{
			this.coreReaderImpl.Close(closeStream);
			this.parsingFunction = XmlValidatingReaderImpl.ParsingFunction.ReaderClosed;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x000289B6 File Offset: 0x000279B6
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x000289BE File Offset: 0x000279BE
		internal BaseValidator Validator
		{
			get
			{
				return this.validator;
			}
			set
			{
				this.validator = value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x000289C7 File Offset: 0x000279C7
		internal override XmlNamespaceManager NamespaceManager
		{
			get
			{
				return this.coreReaderImpl.NamespaceManager;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x000289D4 File Offset: 0x000279D4
		internal bool StandAlone
		{
			get
			{
				return this.coreReaderImpl.StandAlone;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x000289E1 File Offset: 0x000279E1
		internal object SchemaTypeObject
		{
			set
			{
				this.coreReaderImpl.InternalSchemaType = value;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x000289EF File Offset: 0x000279EF
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x000289FC File Offset: 0x000279FC
		internal object TypedValueObject
		{
			get
			{
				return this.coreReaderImpl.InternalTypedValue;
			}
			set
			{
				this.coreReaderImpl.InternalTypedValue = value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00028A0A File Offset: 0x00027A0A
		internal bool Normalization
		{
			get
			{
				return this.coreReaderImpl.Normalization;
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00028A17 File Offset: 0x00027A17
		internal bool AddDefaultAttribute(SchemaAttDef attdef)
		{
			return this.coreReaderImpl.AddDefaultAttribute(attdef, false);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00028A26 File Offset: 0x00027A26
		internal SchemaInfo GetSchemaInfo()
		{
			return this.validator.SchemaInfo;
		}

		// Token: 0x040007A3 RID: 1955
		private XmlReader coreReader;

		// Token: 0x040007A4 RID: 1956
		private XmlTextReaderImpl coreReaderImpl;

		// Token: 0x040007A5 RID: 1957
		private IXmlNamespaceResolver coreReaderNSResolver;

		// Token: 0x040007A6 RID: 1958
		private ValidationType validationType;

		// Token: 0x040007A7 RID: 1959
		private BaseValidator validator;

		// Token: 0x040007A8 RID: 1960
		private XmlSchemaCollection schemaCollection;

		// Token: 0x040007A9 RID: 1961
		private bool processIdentityConstraints;

		// Token: 0x040007AA RID: 1962
		private XmlValidatingReaderImpl.ParsingFunction parsingFunction = XmlValidatingReaderImpl.ParsingFunction.Init;

		// Token: 0x040007AB RID: 1963
		private ValidationEventHandler internalEventHandler;

		// Token: 0x040007AC RID: 1964
		private ValidationEventHandler eventHandler;

		// Token: 0x040007AD RID: 1965
		private XmlParserContext parserContext;

		// Token: 0x040007AE RID: 1966
		private ReadContentAsBinaryHelper readBinaryHelper;

		// Token: 0x040007AF RID: 1967
		private XmlReader outerReader;

		// Token: 0x0200009D RID: 157
		private enum ParsingFunction
		{
			// Token: 0x040007B1 RID: 1969
			Read,
			// Token: 0x040007B2 RID: 1970
			Init,
			// Token: 0x040007B3 RID: 1971
			ParseDtdFromContext,
			// Token: 0x040007B4 RID: 1972
			ResolveEntityInternally,
			// Token: 0x040007B5 RID: 1973
			InReadBinaryContent,
			// Token: 0x040007B6 RID: 1974
			ReaderClosed,
			// Token: 0x040007B7 RID: 1975
			Error,
			// Token: 0x040007B8 RID: 1976
			None
		}
	}
}
