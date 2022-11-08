using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.XmlConfiguration;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000129 RID: 297
	internal class XsltLoader : IErrorHelper
	{
		// Token: 0x06000CD3 RID: 3283 RVA: 0x00040810 File Offset: 0x0003F810
		public void Load(Compiler compiler, object stylesheet, XmlResolver xmlResolver)
		{
			this.compiler = compiler;
			this.xmlResolver = (xmlResolver ?? XmlNullResolver.Singleton);
			XmlReader xmlReader = stylesheet as XmlReader;
			if (xmlReader != null)
			{
				this.readerSettings = new QueryReaderSettings(xmlReader);
				this.LoadStylesheet(xmlReader, false);
			}
			else
			{
				string text = stylesheet as string;
				if (text != null)
				{
					XmlResolver xmlResolver2 = xmlResolver;
					if (xmlResolver == null || xmlResolver == XmlNullResolver.Singleton)
					{
						xmlResolver2 = new XmlUrlResolver();
					}
					Uri uri = xmlResolver2.ResolveUri(null, text);
					if (uri == null)
					{
						throw new XslLoadException("Xslt_CantResolve", new string[]
						{
							text
						});
					}
					this.readerSettings = new QueryReaderSettings(new NameTable());
					XmlReader xmlReader2;
					xmlReader = (xmlReader2 = this.CreateReader(uri, xmlResolver2));
					try
					{
						this.LoadStylesheet(xmlReader, false);
						goto IL_EA;
					}
					finally
					{
						if (xmlReader2 != null)
						{
							((IDisposable)xmlReader2).Dispose();
						}
					}
				}
				IXPathNavigable ixpathNavigable = stylesheet as IXPathNavigable;
				if (ixpathNavigable != null)
				{
					xmlReader = XPathNavigatorReader.Create(ixpathNavigable.CreateNavigator());
					this.readerSettings = new QueryReaderSettings(xmlReader.NameTable);
					this.LoadStylesheet(xmlReader, false);
				}
			}
			IL_EA:
			this.Process();
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00040920 File Offset: 0x0003F920
		private void Process()
		{
			this.compiler.StartApplyTemplates = AstFactory.ApplyTemplates(XsltLoader.nullMode);
			this.ProcessOutputSettings();
			this.ProcessAttributeSets();
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00040944 File Offset: 0x0003F944
		private Uri ResolveUri(string relativeUri, string baseUri)
		{
			Uri baseUri2 = (baseUri.Length != 0) ? this.xmlResolver.ResolveUri(null, baseUri) : null;
			Uri uri = this.xmlResolver.ResolveUri(baseUri2, relativeUri);
			if (uri == null)
			{
				throw new XslLoadException("Xslt_CantResolve", new string[]
				{
					relativeUri
				});
			}
			return uri;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0004099C File Offset: 0x0003F99C
		private XmlReader CreateReader(Uri uri, XmlResolver xmlResolver)
		{
			object entity = xmlResolver.GetEntity(uri, null, null);
			Stream stream = entity as Stream;
			if (stream != null)
			{
				return this.readerSettings.CreateReader(stream, uri.ToString());
			}
			XmlReader xmlReader = entity as XmlReader;
			if (xmlReader != null)
			{
				return xmlReader;
			}
			IXPathNavigable ixpathNavigable = entity as IXPathNavigable;
			if (ixpathNavigable != null)
			{
				return XPathNavigatorReader.Create(ixpathNavigable.CreateNavigator());
			}
			throw new XslLoadException("Xslt_CannotLoadStylesheet", new string[]
			{
				uri.ToString(),
				(entity == null) ? "null" : entity.GetType().ToString()
			});
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00040A2C File Offset: 0x0003FA2C
		private Stylesheet LoadStylesheet(Uri uri, bool include)
		{
			Stylesheet result;
			using (XmlReader xmlReader = this.CreateReader(uri, this.xmlResolver))
			{
				result = this.LoadStylesheet(xmlReader, include);
			}
			return result;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00040A70 File Offset: 0x0003FA70
		private Stylesheet LoadStylesheet(XmlReader reader, bool include)
		{
			string baseURI = reader.BaseURI;
			this.documentUriInUse.Add(baseURI, null);
			Stylesheet stylesheet = this.curStylesheet;
			XsltInput xsltInput = this.input;
			Stylesheet result = include ? this.curStylesheet : this.compiler.CreateStylesheet();
			this.input = new XsltInput(reader, this.compiler);
			this.curStylesheet = result;
			try
			{
				this.LoadDocument();
				if (!include)
				{
					this.compiler.MergeWithStylesheet(this.curStylesheet);
					List<Uri> importHrefs = this.curStylesheet.ImportHrefs;
					this.curStylesheet.Imports = new Stylesheet[importHrefs.Count];
					int num = importHrefs.Count - 1;
					while (0 <= num)
					{
						this.curStylesheet.Imports[num] = this.LoadStylesheet(importHrefs[num], false);
						num--;
					}
				}
			}
			catch (XslLoadException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (!XmlException.IsCatchableException(ex))
				{
					throw;
				}
				XmlException ex2 = ex as XmlException;
				if (ex2 != null)
				{
					SourceLineInfo lineInfo = new SourceLineInfo(this.input.Uri, ex2.LineNumber, ex2.LinePosition, ex2.LineNumber, ex2.LinePosition);
					throw new XslLoadException(ex2, lineInfo);
				}
				this.input.FixLastLineInfo();
				throw new XslLoadException(ex, this.input.BuildLineInfo());
			}
			finally
			{
				this.documentUriInUse.Remove(baseURI);
				this.input = xsltInput;
				this.curStylesheet = stylesheet;
			}
			return result;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00040C00 File Offset: 0x0003FC00
		private void LoadDocument()
		{
			if (!this.input.Start())
			{
				this.ReportError("Xslt_WrongStylesheetElement", new string[0]);
				return;
			}
			if (this.input.IsXsltNamespace())
			{
				if (this.input.IsKeyword(this.input.Atoms.Stylesheet) || this.input.IsKeyword(this.input.Atoms.Transform))
				{
					this.LoadRealStylesheet();
				}
				else
				{
					this.ReportError("Xslt_WrongStylesheetElement", new string[0]);
					this.input.SkipNode();
				}
			}
			else
			{
				this.LoadSimplifiedStylesheet();
			}
			this.input.Finish();
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00040CAC File Offset: 0x0003FCAC
		private void LoadSimplifiedStylesheet()
		{
			this.curTemplate = AstFactory.Template(null, "/", XsltLoader.nullMode, double.NaN, this.input.XslVersion);
			this.input.CanHaveApplyImports = true;
			XslNode xslNode = this.LoadLiteralResultElement(true);
			if (xslNode != null)
			{
				XsltLoader.SetLineInfo(this.curTemplate, xslNode.SourceLine);
				List<XslNode> list = new List<XslNode>();
				list.Add(xslNode);
				XsltLoader.SetContent(this.curTemplate, list);
				this.curStylesheet.AddTemplate(this.curTemplate);
			}
			this.curTemplate = null;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00040D40 File Offset: 0x0003FD40
		private void InsertExNamespaces(string value, ref NsDecl nsList, bool extensions)
		{
			if (value != null && value.Length != 0)
			{
				this.compiler.EnterForwardsCompatible();
				string[] array = XmlConvert.SplitString(value);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.input.LookupXmlNamespace((array[i] == "#default") ? string.Empty : array[i]);
				}
				if (!this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility))
				{
					return;
				}
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] != null)
					{
						nsList = new NsDecl(nsList, null, array[j]);
						if (extensions)
						{
							this.input.AddExtensionNamespace(array[j]);
						}
					}
				}
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00040DF0 File Offset: 0x0003FDF0
		private void LoadRealStylesheet()
		{
			string text;
			string value;
			string value2;
			string text2;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Version, out text, this.input.Atoms.ExtensionElementPrefixes, out value, this.input.Atoms.ExcludeResultPrefixes, out value2, this.input.Atoms.Id, out text2);
			if (text == null)
			{
				this.input.SetVersion("1.0", this.input.Atoms.Version);
			}
			this.InsertExNamespaces(value, ref attributes.nsList, true);
			this.InsertExNamespaces(value2, ref attributes.nsList, false);
			string qualifiedName = this.input.QualifiedName;
			if (this.input.MoveToFirstChild())
			{
				bool flag = true;
				do
				{
					bool flag2 = false;
					XPathNodeType nodeType = this.input.NodeType;
					if (nodeType != XPathNodeType.Element)
					{
						switch (nodeType)
						{
						case XPathNodeType.SignificantWhitespace:
						case XPathNodeType.Whitespace:
							break;
						default:
							this.ReportError("Xslt_TextNodesNotAllowed", new string[]
							{
								this.input.Atoms.Stylesheet
							});
							break;
						}
					}
					else
					{
						if (this.input.IsXsltNamespace())
						{
							if (this.input.IsKeyword(this.input.Atoms.Import))
							{
								if (!flag)
								{
									this.ReportError("Xslt_NotAtTop", new string[]
									{
										this.input.QualifiedName,
										qualifiedName
									});
									this.input.SkipNode();
								}
								else
								{
									flag2 = true;
									this.LoadImport();
								}
							}
							else if (this.input.IsKeyword(this.input.Atoms.Include))
							{
								this.LoadInclude();
							}
							else if (this.input.IsKeyword(this.input.Atoms.StripSpace))
							{
								this.LoadStripSpace(attributes.nsList);
							}
							else if (this.input.IsKeyword(this.input.Atoms.PreserveSpace))
							{
								this.LoadPreserveSpace(attributes.nsList);
							}
							else if (this.input.IsKeyword(this.input.Atoms.Output))
							{
								this.LoadOutput();
							}
							else if (this.input.IsKeyword(this.input.Atoms.Key))
							{
								this.LoadKey(attributes.nsList);
							}
							else if (this.input.IsKeyword(this.input.Atoms.DecimalFormat))
							{
								this.LoadDecimalFormat(attributes.nsList);
							}
							else if (this.input.IsKeyword(this.input.Atoms.NamespaceAlias))
							{
								this.LoadNamespaceAlias(attributes.nsList);
							}
							else if (this.input.IsKeyword(this.input.Atoms.AttributeSet))
							{
								this.LoadAttributeSet(attributes.nsList);
							}
							else if (this.input.IsKeyword(this.input.Atoms.Variable))
							{
								this.LoadGlobalVariableOrParameter(attributes.nsList, XslNodeType.Variable);
							}
							else if (this.input.IsKeyword(this.input.Atoms.Param))
							{
								this.LoadGlobalVariableOrParameter(attributes.nsList, XslNodeType.Param);
							}
							else if (this.input.IsKeyword(this.input.Atoms.Template))
							{
								this.LoadTemplate(attributes.nsList);
							}
							else
							{
								if (!this.input.ForwardCompatibility)
								{
									this.ReportError("Xslt_UnexpectedElementQ", new string[]
									{
										this.input.QualifiedName,
										qualifiedName
									});
								}
								this.input.SkipNode();
							}
						}
						else if (this.input.IsNs(this.input.Atoms.UrnMsxsl) && this.input.IsKeyword(this.input.Atoms.Script))
						{
							this.LoadScript(attributes.nsList);
						}
						else if (this.input.IsNullNamespace())
						{
							this.ReportError("Xslt_NullNsAtTopLevel", new string[]
							{
								this.input.LocalName
							});
							this.input.SkipNode();
						}
						else
						{
							this.input.SkipNode();
						}
						flag = flag2;
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0004127C File Offset: 0x0004027C
		private void LoadImport()
		{
			string text;
			this.input.GetAttributes(1, this.input.Atoms.Href, out text);
			this.CheckNoContent();
			if (text == null)
			{
				return;
			}
			Uri uri = this.ResolveUri(text, this.input.BaseUri);
			if (this.documentUriInUse.Contains(uri.ToString()))
			{
				this.ReportError("Xslt_CircularInclude", new string[]
				{
					text
				});
				return;
			}
			this.curStylesheet.ImportHrefs.Add(uri);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00041304 File Offset: 0x00040304
		private void LoadInclude()
		{
			string text;
			this.input.GetAttributes(1, this.input.Atoms.Href, out text);
			this.CheckNoContent();
			if (text == null)
			{
				return;
			}
			Uri uri = this.ResolveUri(text, this.input.BaseUri);
			if (this.documentUriInUse.Contains(uri.ToString()))
			{
				this.ReportError("Xslt_CircularInclude", new string[]
				{
					text
				});
				return;
			}
			this.LoadStylesheet(uri, true);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00041384 File Offset: 0x00040384
		private void LoadStripSpace(NsDecl stylesheetNsList)
		{
			string elements;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Elements, out elements);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			this.ParseWhitespaceRules(elements, false);
			this.CheckNoContent();
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000413D0 File Offset: 0x000403D0
		private void LoadPreserveSpace(NsDecl stylesheetNsList)
		{
			string elements;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Elements, out elements);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			this.ParseWhitespaceRules(elements, true);
			this.CheckNoContent();
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0004141C File Offset: 0x0004041C
		private void LoadOutput()
		{
			string text;
			string text2;
			string text3;
			string text4;
			string text5;
			string text6;
			string text7;
			string text8;
			string text9;
			string text10;
			this.input.GetAttributes(0, this.input.Atoms.Method, out text, this.input.Atoms.Version, out text2, this.input.Atoms.Encoding, out text3, this.input.Atoms.OmitXmlDeclaration, out text4, this.input.Atoms.Standalone, out text5, this.input.Atoms.DocTypePublic, out text6, this.input.Atoms.DocTypeSystem, out text7, this.input.Atoms.CDataSectionElements, out text8, this.input.Atoms.Indent, out text9, this.input.Atoms.MediaType, out text10);
			Output output = this.compiler.Output;
			XmlWriterSettings settings = output.Settings;
			int currentPrecedence = this.compiler.CurrentPrecedence;
			if (text != null && currentPrecedence >= output.MethodPrec)
			{
				this.compiler.EnterForwardsCompatible();
				XmlOutputMethod outputMethod;
				XmlQualifiedName xmlQualifiedName = this.ParseOutputMethod(text, out outputMethod);
				if (this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility) && xmlQualifiedName != null)
				{
					if (currentPrecedence == output.MethodPrec && !output.Method.Equals(xmlQualifiedName))
					{
						this.ReportWarning("Xslt_AttributeRedefinition", new string[]
						{
							this.input.Atoms.Method
						});
					}
					settings.OutputMethod = outputMethod;
					output.Method = xmlQualifiedName;
					output.MethodPrec = currentPrecedence;
				}
			}
			if (text2 != null && currentPrecedence >= output.VersionPrec)
			{
				if (currentPrecedence == output.VersionPrec && output.Version != text2)
				{
					this.ReportWarning("Xslt_AttributeRedefinition", new string[]
					{
						this.input.Atoms.Version
					});
				}
				output.Version = text2;
				output.VersionPrec = currentPrecedence;
			}
			if (text3 != null && currentPrecedence >= output.EncodingPrec)
			{
				try
				{
					Encoding encoding = Encoding.GetEncoding(text3);
					if (currentPrecedence == output.EncodingPrec && output.Encoding != text3)
					{
						this.ReportWarning("Xslt_AttributeRedefinition", new string[]
						{
							this.input.Atoms.Encoding
						});
					}
					settings.Encoding = encoding;
					output.Encoding = text3;
					output.EncodingPrec = currentPrecedence;
				}
				catch (ArgumentException)
				{
					if (!this.input.ForwardCompatibility)
					{
						this.ReportWarning("Xslt_InvalidEncoding", new string[]
						{
							text3
						});
					}
				}
			}
			if (text4 != null && currentPrecedence >= output.OmitXmlDeclarationPrec)
			{
				TriState triState = this.ParseYesNo(text4, this.input.Atoms.OmitXmlDeclaration);
				if (triState != TriState.Unknown)
				{
					bool flag = triState == TriState.True;
					if (currentPrecedence == output.OmitXmlDeclarationPrec && settings.OmitXmlDeclaration != flag)
					{
						this.ReportWarning("Xslt_AttributeRedefinition", new string[]
						{
							this.input.Atoms.OmitXmlDeclaration
						});
					}
					settings.OmitXmlDeclaration = flag;
					output.OmitXmlDeclarationPrec = currentPrecedence;
				}
			}
			if (text5 != null && currentPrecedence >= output.StandalonePrec)
			{
				TriState triState = this.ParseYesNo(text5, this.input.Atoms.Standalone);
				if (triState != TriState.Unknown)
				{
					XmlStandalone xmlStandalone = (triState == TriState.True) ? XmlStandalone.Yes : XmlStandalone.No;
					if (currentPrecedence == output.StandalonePrec && settings.Standalone != xmlStandalone)
					{
						this.ReportWarning("Xslt_AttributeRedefinition", new string[]
						{
							this.input.Atoms.Standalone
						});
					}
					settings.Standalone = xmlStandalone;
					output.StandalonePrec = currentPrecedence;
				}
			}
			if (text6 != null && currentPrecedence >= output.DocTypePublicPrec)
			{
				if (currentPrecedence == output.DocTypePublicPrec && settings.DocTypePublic != text6)
				{
					this.ReportWarning("Xslt_AttributeRedefinition", new string[]
					{
						this.input.Atoms.DocTypePublic
					});
				}
				settings.DocTypePublic = text6;
				output.DocTypePublicPrec = currentPrecedence;
			}
			if (text7 != null && currentPrecedence >= output.DocTypeSystemPrec)
			{
				if (currentPrecedence == output.DocTypeSystemPrec && settings.DocTypeSystem != text7)
				{
					this.ReportWarning("Xslt_AttributeRedefinition", new string[]
					{
						this.input.Atoms.DocTypeSystem
					});
				}
				settings.DocTypeSystem = text7;
				output.DocTypeSystemPrec = currentPrecedence;
			}
			if (text8 != null && text8.Length != 0)
			{
				this.compiler.EnterForwardsCompatible();
				string[] array = XmlConvert.SplitString(text8);
				List<XmlQualifiedName> list = new List<XmlQualifiedName>();
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(this.ResolveQName(false, array[i]));
				}
				if (this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility))
				{
					settings.CDataSectionElements.AddRange(list);
				}
			}
			if (text9 != null && currentPrecedence >= output.IndentPrec)
			{
				TriState triState = this.ParseYesNo(text9, this.input.Atoms.Indent);
				if (triState != TriState.Unknown)
				{
					bool flag2 = triState == TriState.True;
					if (currentPrecedence == output.IndentPrec && settings.Indent != flag2)
					{
						this.ReportWarning("Xslt_AttributeRedefinition", new string[]
						{
							this.input.Atoms.Indent
						});
					}
					settings.Indent = flag2;
					output.IndentPrec = currentPrecedence;
				}
			}
			if (text10 != null && currentPrecedence >= output.MediaTypePrec)
			{
				if (currentPrecedence == output.MediaTypePrec && settings.MediaType != text10)
				{
					this.ReportWarning("Xslt_AttributeRedefinition", new string[]
					{
						this.input.Atoms.MediaType
					});
				}
				settings.MediaType = text10;
				output.MediaTypePrec = currentPrecedence;
			}
			this.CheckNoContent();
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00041A18 File Offset: 0x00040A18
		private void ProcessOutputSettings()
		{
			Output output = this.compiler.Output;
			XmlWriterSettings settings = output.Settings;
			if (settings.OutputMethod == XmlOutputMethod.Html && output.IndentPrec == -2147483648)
			{
				settings.Indent = true;
			}
			if (output.MediaTypePrec == -2147483648)
			{
				settings.MediaType = ((settings.OutputMethod == XmlOutputMethod.Xml) ? "text/xml" : ((settings.OutputMethod == XmlOutputMethod.Html) ? "text/html" : ((settings.OutputMethod == XmlOutputMethod.Text) ? "text/plain" : null)));
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00041A98 File Offset: 0x00040A98
		private void AttributeSetsDfs(AttributeSet attSet)
		{
			switch (attSet.CycleCheck)
			{
			case CycleCheck.NotStarted:
				attSet.CycleCheck = CycleCheck.Processing;
				foreach (QilName key in attSet.UsedAttributeSets)
				{
					AttributeSet attSet2;
					if (this.compiler.AttributeSets.TryGetValue(key, out attSet2))
					{
						this.AttributeSetsDfs(attSet2);
					}
				}
				attSet.CycleCheck = CycleCheck.Completed;
				return;
			case CycleCheck.Completed:
				return;
			}
			this.compiler.ReportError(attSet.Content[0].SourceLine, "Xslt_CircularAttributeSet", new string[]
			{
				attSet.Name.QualifiedName
			});
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00041B64 File Offset: 0x00040B64
		private void ProcessAttributeSets()
		{
			foreach (AttributeSet attSet in this.compiler.AttributeSets.Values)
			{
				this.AttributeSetsDfs(attSet);
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00041BC4 File Offset: 0x00040BC4
		private void LoadKey(NsDecl stylesheetNsList)
		{
			string qname;
			string match;
			string use;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(3, this.input.Atoms.Name, out qname, this.input.Atoms.Match, out match, this.input.Atoms.Use, out use);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			this.CheckNoContent();
			QilName qilName = this.CreateXPathQName(qname);
			Key item = (Key)XsltLoader.SetInfo(AstFactory.Key(qilName, match, use, this.input.XslVersion), null, attributes);
			if (this.compiler.Keys.Contains(qilName))
			{
				this.compiler.Keys[qilName].Add(item);
				return;
			}
			List<Key> list = new List<Key>();
			list.Add(item);
			this.compiler.Keys.Add(list);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00041CA8 File Offset: 0x00040CA8
		private void LoadDecimalFormat(NsDecl stylesheetNsList)
		{
			string[] array = new string[11];
			string[] array2 = new string[]
			{
				this.input.Atoms.DecimalSeparator,
				this.input.Atoms.GroupingSeparator,
				this.input.Atoms.Percent,
				this.input.Atoms.PerMille,
				this.input.Atoms.ZeroDigit,
				this.input.Atoms.Digit,
				this.input.Atoms.PatternSeparator,
				this.input.Atoms.MinusSign,
				this.input.Atoms.Infinity,
				this.input.Atoms.NaN,
				this.input.Atoms.Name
			};
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, 11, array2, array);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			char[] characters = DecimalFormatDecl.Default.Characters;
			char[] array3 = new char[8];
			int i;
			for (i = 0; i < 8; i++)
			{
				array3[i] = this.ParseCharAttribute(array[i], characters[i], array2[i]);
			}
			string text = array[i++];
			string text2 = array[i++];
			string text3 = array[i++];
			if (text == null)
			{
				text = DecimalFormatDecl.Default.InfinitySymbol;
			}
			if (text2 == null)
			{
				text2 = DecimalFormatDecl.Default.NanSymbol;
			}
			for (int j = 0; j < 7; j++)
			{
				for (int k = j + 1; k < 7; k++)
				{
					if (array3[j] == array3[k])
					{
						this.ReportError("Xslt_DecimalFormatSignsNotDistinct", new string[]
						{
							array2[j],
							array2[k]
						});
						break;
					}
				}
			}
			XmlQualifiedName xmlQualifiedName;
			if (text3 == null)
			{
				xmlQualifiedName = new XmlQualifiedName();
			}
			else
			{
				this.compiler.EnterForwardsCompatible();
				xmlQualifiedName = this.ResolveQName(true, text3);
				if (!this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility))
				{
					xmlQualifiedName = new XmlQualifiedName();
				}
			}
			if (this.compiler.DecimalFormats.Contains(xmlQualifiedName))
			{
				DecimalFormatDecl decimalFormatDecl = this.compiler.DecimalFormats[xmlQualifiedName];
				for (i = 0; i < 8; i++)
				{
					if (array3[i] != decimalFormatDecl.Characters[i])
					{
						this.ReportError("Xslt_DecimalFormatRedefined", new string[]
						{
							array2[i],
							char.ToString(array3[i])
						});
					}
				}
				if (text != decimalFormatDecl.InfinitySymbol)
				{
					this.ReportError("Xslt_DecimalFormatRedefined", new string[]
					{
						array2[i],
						text
					});
				}
				i++;
				if (text2 != decimalFormatDecl.NanSymbol)
				{
					this.ReportError("Xslt_DecimalFormatRedefined", new string[]
					{
						array2[i],
						text2
					});
				}
				i++;
				i++;
			}
			else
			{
				DecimalFormatDecl item = new DecimalFormatDecl(xmlQualifiedName, text, text2, new string(array3));
				this.compiler.DecimalFormats.Add(item);
			}
			this.CheckNoContent();
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00041FFC File Offset: 0x00040FFC
		private void LoadNamespaceAlias(NsDecl stylesheetNsList)
		{
			string empty;
			string empty2;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(2, this.input.Atoms.StylesheetPrefix, out empty, this.input.Atoms.ResultPrefix, out empty2);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			this.CheckNoContent();
			string text = null;
			string text2 = null;
			if (empty != null)
			{
				if (empty.Length == 0)
				{
					this.ReportError("Xslt_EmptyNsAlias", new string[]
					{
						this.input.Atoms.StylesheetPrefix
					});
				}
				else
				{
					if (empty == "#default")
					{
						empty = string.Empty;
					}
					text = this.input.LookupXmlNamespace(empty);
				}
			}
			if (empty2 != null)
			{
				if (empty2.Length == 0)
				{
					this.ReportError("Xslt_EmptyNsAlias", new string[]
					{
						this.input.Atoms.ResultPrefix
					});
				}
				else
				{
					if (empty2 == "#default")
					{
						empty2 = string.Empty;
					}
					text2 = this.input.LookupXmlNamespace(empty2);
				}
			}
			if (text == null || text2 == null)
			{
				return;
			}
			if (this.compiler.SetNsAlias(text, text2, empty2, this.curStylesheet.ImportPrecedence))
			{
				this.ReportWarning("Xslt_DupNsAlias", new string[]
				{
					text
				});
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00042144 File Offset: 0x00041144
		private void LoadAttributeSet(NsDecl stylesheetNsList)
		{
			string qname;
			string useAttributeSets;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Name, out qname, this.input.Atoms.UseAttributeSets, out useAttributeSets);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			QilName qilName = this.CreateXPathQName(qname);
			AttributeSet attributeSet;
			if (!this.curStylesheet.AttributeSets.TryGetValue(qilName, out attributeSet))
			{
				attributeSet = (this.curStylesheet.AttributeSets[qilName] = AstFactory.AttributeSet(qilName));
				if (!this.compiler.AttributeSets.ContainsKey(qilName))
				{
					this.compiler.AllTemplates.Add(attributeSet);
				}
			}
			List<XslNode> list = this.ParseUseAttributeSets(useAttributeSets, attributes.lineInfo);
			foreach (XslNode xslNode in list)
			{
				attributeSet.UsedAttributeSets.Add(xslNode.Name);
			}
			if (this.input.MoveToFirstChild())
			{
				do
				{
					XPathNodeType nodeType = this.input.NodeType;
					if (nodeType != XPathNodeType.Element)
					{
						switch (nodeType)
						{
						case XPathNodeType.SignificantWhitespace:
						case XPathNodeType.Whitespace:
							break;
						default:
							this.ReportError("Xslt_TextNodesNotAllowed", new string[]
							{
								this.input.Atoms.AttributeSet
							});
							break;
						}
					}
					else if (this.input.IsXsltNamespace() && this.input.IsKeyword(this.input.Atoms.Attribute))
					{
						XsltLoader.AddInstruction(list, this.XslAttribute());
					}
					else
					{
						this.ReportError("Xslt_UnexpectedElement", new string[]
						{
							this.input.QualifiedName,
							this.input.Atoms.AttributeSet
						});
						this.input.SkipNode();
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			attributeSet.AddContent(XsltLoader.SetInfo(AstFactory.List(), this.LoadEndTag(list), attributes));
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00042364 File Offset: 0x00041364
		private void LoadGlobalVariableOrParameter(NsDecl stylesheetNsList, XslNodeType nodeType)
		{
			VarPar varPar = this.XslVarPar(nodeType);
			varPar.Namespaces = XsltLoader.MergeNamespaces(varPar.Namespaces, stylesheetNsList);
			if (!this.curStylesheet.AddVarPar(varPar))
			{
				this.ReportError("Xslt_DupGlobalVariable", new string[]
				{
					varPar.Name.QualifiedName
				});
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x000423BC File Offset: 0x000413BC
		private void LoadTemplate(NsDecl stylesheetNsList)
		{
			string text;
			string text2;
			string text3;
			string text4;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.Match, out text, this.input.Atoms.Name, out text2, this.input.Atoms.Priority, out text3, this.input.Atoms.Mode, out text4);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			if (text == null)
			{
				if (text2 == null)
				{
					this.ReportError("Xslt_BothMatchNameAbsent", new string[0]);
				}
				if (text4 != null)
				{
					this.ReportError("Xslt_ModeWithoutMatch", new string[0]);
					text4 = null;
				}
				if (text3 != null)
				{
					this.ReportWarning("Xslt_PriorityWithoutMatch", new string[0]);
				}
			}
			QilName name = null;
			if (text2 != null)
			{
				this.compiler.EnterForwardsCompatible();
				name = this.CreateXPathQName(text2);
				if (!this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility))
				{
					name = null;
				}
			}
			double num = double.NaN;
			if (text3 != null)
			{
				num = XPathConvert.StringToDouble(text3);
				if (double.IsNaN(num) && !this.input.ForwardCompatibility)
				{
					this.ReportError("Xslt_InvalidAttrValue", new string[]
					{
						this.input.Atoms.Priority,
						text3
					});
				}
			}
			this.curTemplate = AstFactory.Template(name, text, this.ParseMode(text4), num, this.input.XslVersion);
			this.input.CanHaveApplyImports = (text != null);
			XsltLoader.SetInfo(this.curTemplate, this.LoadEndTag(this.LoadInstructions(XsltLoader.InstructionFlags.AllowParam)), attributes);
			if (!this.curStylesheet.AddTemplate(this.curTemplate))
			{
				this.ReportError("Xslt_DupTemplateName", new string[]
				{
					this.curTemplate.Name.QualifiedName
				});
			}
			this.curTemplate = null;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00042598 File Offset: 0x00041598
		private void LoadScript(NsDecl stylesheetNsList)
		{
			string text;
			string text2;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.ImplementsPrefix, out text, this.input.Atoms.Language, out text2);
			attributes.nsList = XsltLoader.MergeNamespaces(attributes.nsList, stylesheetNsList);
			string text3 = null;
			if (text != null)
			{
				if (text.Length == 0)
				{
					this.ReportError("Xslt_EmptyAttrValue", new string[]
					{
						this.input.Atoms.ImplementsPrefix,
						text
					});
				}
				else
				{
					text3 = this.input.LookupXmlNamespace(text);
					if (text3 == "http://www.w3.org/1999/XSL/Transform")
					{
						this.ReportError("Xslt_ScriptXsltNamespace", new string[0]);
						text3 = null;
					}
				}
			}
			if (text3 == null)
			{
				text3 = this.compiler.CreatePhantomNamespace();
			}
			if (text2 == null)
			{
				text2 = "jscript";
			}
			if (!this.compiler.Settings.EnableScript)
			{
				this.compiler.Scripts.ScriptClasses[text3] = null;
				this.input.SkipNode();
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string uri = this.input.Uri;
			int lineNumber = 0;
			int num = 0;
			ScriptClass scriptClass = this.compiler.Scripts.GetScriptClass(text3, text2, this);
			if (scriptClass == null)
			{
				this.input.SkipNode();
				return;
			}
			if (this.input.MoveToFirstChild())
			{
				do
				{
					XPathNodeType nodeType = this.input.NodeType;
					if (nodeType != XPathNodeType.Element)
					{
						if (nodeType == XPathNodeType.Text || stringBuilder.Length != 0)
						{
							int startLine = this.input.StartLine;
							int endLine = this.input.EndLine;
							if (stringBuilder.Length == 0)
							{
								lineNumber = startLine;
							}
							else if (num < startLine)
							{
								stringBuilder.Append('\n', startLine - num);
							}
							stringBuilder.Append(this.input.Value);
							num = endLine;
						}
					}
					else if (this.input.IsNs(this.input.Atoms.UrnMsxsl) && (this.input.IsKeyword(this.input.Atoms.Assembly) || this.input.IsKeyword(this.input.Atoms.Using)))
					{
						if (stringBuilder.Length != 0)
						{
							this.ReportError("Xslt_ScriptNotAtTop", new string[]
							{
								this.input.QualifiedName
							});
							this.input.SkipNode();
						}
						if (this.input.IsKeyword(this.input.Atoms.Assembly))
						{
							this.LoadMsAssembly(scriptClass);
						}
						else if (this.input.IsKeyword(this.input.Atoms.Using))
						{
							this.LoadMsUsing(scriptClass);
						}
					}
					else
					{
						this.ReportError("Xslt_UnexpectedElementQ", new string[]
						{
							this.input.QualifiedName,
							"msxsl:script"
						});
						this.input.SkipNode();
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			if (stringBuilder.Length == 0)
			{
				lineNumber = this.input.StartLine;
			}
			scriptClass.AddScriptBlock(stringBuilder.ToString(), uri, lineNumber, this.input.StartLine, this.input.StartPos);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000428E8 File Offset: 0x000418E8
		private void LoadMsAssembly(ScriptClass scriptClass)
		{
			string text;
			string text2;
			this.input.GetAttributes(0, this.input.Atoms.Name, out text, this.input.Atoms.Href, out text2);
			string text3 = null;
			if (text != null)
			{
				if (text2 != null)
				{
					this.ReportError("Xslt_AssemblyBothNameHrefPresent", new string[0]);
					goto IL_DB;
				}
				try
				{
					text3 = Assembly.Load(text).Location;
					goto IL_DB;
				}
				catch
				{
					AssemblyName assemblyName = new AssemblyName(text);
					byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
					if ((publicKeyToken == null || publicKeyToken.Length == 0) && assemblyName.Version == null)
					{
						text3 = assemblyName.Name + ".dll";
						goto IL_DB;
					}
					throw;
				}
			}
			if (text2 != null)
			{
				text3 = Assembly.LoadFrom(this.ResolveUri(text2, this.input.BaseUri).ToString()).Location;
				scriptClass.refAssembliesByHref = true;
			}
			else
			{
				this.ReportError("Xslt_AssemblyBothNameHrefAbsent", new string[0]);
			}
			IL_DB:
			if (text3 != null)
			{
				scriptClass.refAssemblies.Add(text3);
			}
			this.CheckNoContent();
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x000429F8 File Offset: 0x000419F8
		private void LoadMsUsing(ScriptClass scriptClass)
		{
			string text;
			this.input.GetAttributes(1, this.input.Atoms.Namespace, out text);
			if (text != null)
			{
				scriptClass.nsImports.Add(text);
			}
			this.CheckNoContent();
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00042A3A File Offset: 0x00041A3A
		private List<XslNode> LoadInstructions()
		{
			return this.LoadInstructions(new List<XslNode>(), XsltLoader.InstructionFlags.NoParamNoSort);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00042A48 File Offset: 0x00041A48
		private List<XslNode> LoadInstructions(XsltLoader.InstructionFlags flags)
		{
			return this.LoadInstructions(new List<XslNode>(), flags);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00042A56 File Offset: 0x00041A56
		private List<XslNode> LoadInstructions(List<XslNode> content)
		{
			return this.LoadInstructions(content, XsltLoader.InstructionFlags.NoParamNoSort);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00042A60 File Offset: 0x00041A60
		private List<XslNode> LoadInstructions(List<XslNode> content, XsltLoader.InstructionFlags flags)
		{
			if (++this.loadInstructionsDepth > 1024 && XsltConfigSection.LimitXPathComplexity)
			{
				throw XsltException.Create("Xslt_CompileError2", new string[0]);
			}
			string qualifiedName = this.input.QualifiedName;
			if (this.input.MoveToFirstChild())
			{
				bool flag = true;
				for (;;)
				{
					XPathNodeType nodeType = this.input.NodeType;
					XslNode instruction;
					if (nodeType != XPathNodeType.Element)
					{
						switch (nodeType)
						{
						case XPathNodeType.SignificantWhitespace:
							break;
						case XPathNodeType.Whitespace:
							goto IL_4DF;
						default:
							flag = false;
							break;
						}
						instruction = XsltLoader.SetLineInfo(AstFactory.Text(this.input.Value), this.input.BuildLineInfo());
						goto IL_4D8;
					}
					string namespaceUri = this.input.NamespaceUri;
					string localName = this.input.LocalName;
					if (!(namespaceUri == this.input.Atoms.UriXsl))
					{
						flag = false;
						instruction = this.LoadLiteralResultElement(false);
						goto IL_4D8;
					}
					bool flag2 = false;
					if (Ref.Equal(localName, this.input.Atoms.Param))
					{
						if ((flags & XsltLoader.InstructionFlags.AllowParam) == XsltLoader.InstructionFlags.NoParamNoSort)
						{
							this.ReportError("Xslt_UnexpectedElementQ", new string[]
							{
								this.input.QualifiedName,
								qualifiedName
							});
							flag2 = true;
						}
						else if (!flag)
						{
							this.ReportError("Xslt_NotAtTop", new string[]
							{
								this.input.QualifiedName,
								qualifiedName
							});
							flag2 = true;
						}
					}
					else if (Ref.Equal(localName, this.input.Atoms.Sort))
					{
						if ((flags & XsltLoader.InstructionFlags.AllowSort) == XsltLoader.InstructionFlags.NoParamNoSort)
						{
							this.ReportError("Xslt_UnexpectedElementQ", new string[]
							{
								this.input.QualifiedName,
								qualifiedName
							});
							flag2 = true;
						}
						else if (!flag)
						{
							this.ReportError("Xslt_NotAtTop", new string[]
							{
								this.input.QualifiedName,
								qualifiedName
							});
							flag2 = true;
						}
					}
					else
					{
						flag = false;
					}
					if (!flag2)
					{
						instruction = (Ref.Equal(localName, this.input.Atoms.ApplyImports) ? this.XslApplyImports() : (Ref.Equal(localName, this.input.Atoms.ApplyTemplates) ? this.XslApplyTemplates() : (Ref.Equal(localName, this.input.Atoms.CallTemplate) ? this.XslCallTemplate() : (Ref.Equal(localName, this.input.Atoms.Copy) ? this.XslCopy() : (Ref.Equal(localName, this.input.Atoms.CopyOf) ? this.XslCopyOf() : (Ref.Equal(localName, this.input.Atoms.Fallback) ? this.XslFallback() : (Ref.Equal(localName, this.input.Atoms.If) ? this.XslIf() : (Ref.Equal(localName, this.input.Atoms.Choose) ? this.XslChoose() : (Ref.Equal(localName, this.input.Atoms.ForEach) ? this.XslForEach() : (Ref.Equal(localName, this.input.Atoms.Message) ? this.XslMessage() : (Ref.Equal(localName, this.input.Atoms.Number) ? this.XslNumber() : (Ref.Equal(localName, this.input.Atoms.ValueOf) ? this.XslValueOf() : (Ref.Equal(localName, this.input.Atoms.Comment) ? this.XslComment() : (Ref.Equal(localName, this.input.Atoms.ProcessingInstruction) ? this.XslProcessingInstruction() : (Ref.Equal(localName, this.input.Atoms.Text) ? this.XslText() : (Ref.Equal(localName, this.input.Atoms.Element) ? this.XslElement() : (Ref.Equal(localName, this.input.Atoms.Attribute) ? this.XslAttribute() : (Ref.Equal(localName, this.input.Atoms.Variable) ? this.XslVarPar(XslNodeType.Variable) : (Ref.Equal(localName, this.input.Atoms.Param) ? this.XslVarPar(XslNodeType.Param) : (Ref.Equal(localName, this.input.Atoms.Sort) ? this.XslSort() : this.LoadUnknownXsltInstruction(qualifiedName)))))))))))))))))))));
						goto IL_4D8;
					}
					flag = false;
					this.input.SkipNode();
					IL_4DF:
					if (!this.input.MoveToNextSibling())
					{
						break;
					}
					continue;
					IL_4D8:
					XsltLoader.AddInstruction(content, instruction);
					goto IL_4DF;
				}
				this.input.MoveToParent();
			}
			this.loadInstructionsDepth--;
			return content;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00042F78 File Offset: 0x00041F78
		private XslNode XslApplyImports()
		{
			XsltInput.ContextInfo attributes = this.input.GetAttributes();
			if (!this.input.CanHaveApplyImports)
			{
				this.ReportError("Xslt_InvalidApplyImports", new string[0]);
				this.input.SkipNode();
				return null;
			}
			this.CheckNoContent();
			return XsltLoader.SetInfo(AstFactory.ApplyImports(this.curTemplate.Mode, this.curStylesheet, this.input.XslVersion), null, attributes);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00042FEC File Offset: 0x00041FEC
		private XslNode XslApplyTemplates()
		{
			string text;
			string qname;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.Select, out text, this.input.Atoms.Mode, out qname);
			if (text == null)
			{
				text = "node()";
			}
			QilName mode = this.ParseMode(qname);
			List<XslNode> content = new List<XslNode>();
			if (this.input.MoveToFirstChild())
			{
				do
				{
					XPathNodeType nodeType = this.input.NodeType;
					if (nodeType != XPathNodeType.Element)
					{
						switch (nodeType)
						{
						case XPathNodeType.SignificantWhitespace:
						case XPathNodeType.Whitespace:
							break;
						default:
							this.ReportError("Xslt_TextNodesNotAllowed", new string[]
							{
								this.input.Atoms.ApplyTemplates
							});
							break;
						}
					}
					else
					{
						if (this.input.IsXsltNamespace())
						{
							if (this.input.IsKeyword(this.input.Atoms.WithParam))
							{
								XslNode xslNode = this.XslVarPar(XslNodeType.WithParam);
								this.CheckWithParam(content, xslNode);
								XsltLoader.AddInstruction(content, xslNode);
								goto IL_167;
							}
							if (this.input.IsKeyword(this.input.Atoms.Sort))
							{
								XsltLoader.AddInstruction(content, this.XslSort());
								goto IL_167;
							}
						}
						this.ReportError("Xslt_UnexpectedElement", new string[]
						{
							this.input.QualifiedName,
							this.input.Atoms.ApplyTemplates
						});
						this.input.SkipNode();
					}
					IL_167:;
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			attributes.SaveExtendedLineInfo(this.input);
			return XsltLoader.SetInfo(AstFactory.ApplyTemplates(mode, text, attributes, this.input.XslVersion), content, attributes);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000431A4 File Offset: 0x000421A4
		private XslNode XslCallTemplate()
		{
			string qname;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Name, out qname);
			List<XslNode> content = new List<XslNode>();
			if (this.input.MoveToFirstChild())
			{
				do
				{
					XPathNodeType nodeType = this.input.NodeType;
					if (nodeType != XPathNodeType.Element)
					{
						switch (nodeType)
						{
						case XPathNodeType.SignificantWhitespace:
						case XPathNodeType.Whitespace:
							break;
						default:
							this.ReportError("Xslt_TextNodesNotAllowed", new string[]
							{
								this.input.Atoms.CallTemplate
							});
							break;
						}
					}
					else if (this.input.IsXsltNamespace() && this.input.IsKeyword(this.input.Atoms.WithParam))
					{
						XslNode xslNode = this.XslVarPar(XslNodeType.WithParam);
						this.CheckWithParam(content, xslNode);
						XsltLoader.AddInstruction(content, xslNode);
					}
					else
					{
						this.ReportError("Xslt_UnexpectedElement", new string[]
						{
							this.input.QualifiedName,
							this.input.Atoms.CallTemplate
						});
						this.input.SkipNode();
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			attributes.SaveExtendedLineInfo(this.input);
			return XsltLoader.SetInfo(AstFactory.CallTemplate(this.CreateXPathQName(qname), attributes), content, attributes);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x000432FC File Offset: 0x000422FC
		private XslNode XslCopy()
		{
			string useAttributeSets;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.UseAttributeSets, out useAttributeSets);
			List<XslNode> content = this.ParseUseAttributeSets(useAttributeSets, attributes.lineInfo);
			return XsltLoader.SetInfo(AstFactory.Copy(), this.LoadEndTag(this.LoadInstructions(content)), attributes);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00043350 File Offset: 0x00042350
		private XslNode XslCopyOf()
		{
			string select;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Select, out select);
			this.CheckNoContent();
			return XsltLoader.SetInfo(AstFactory.CopyOf(select, this.input.XslVersion), null, attributes);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0004339A File Offset: 0x0004239A
		private XslNode XslFallback()
		{
			this.input.GetAttributes();
			this.input.SkipNode();
			return null;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000433B4 File Offset: 0x000423B4
		private XslNode XslIf()
		{
			string test;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Test, out test);
			return XsltLoader.SetInfo(AstFactory.If(test, this.input.XslVersion), this.LoadInstructions(), attributes);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00043400 File Offset: 0x00042400
		private XslNode XslChoose()
		{
			XsltInput.ContextInfo attributes = this.input.GetAttributes();
			List<XslNode> content = new List<XslNode>();
			bool flag = false;
			bool flag2 = false;
			if (this.input.MoveToFirstChild())
			{
				do
				{
					XPathNodeType nodeType = this.input.NodeType;
					if (nodeType != XPathNodeType.Element)
					{
						switch (nodeType)
						{
						case XPathNodeType.SignificantWhitespace:
						case XPathNodeType.Whitespace:
							break;
						default:
							this.ReportError("Xslt_TextNodesNotAllowed", new string[]
							{
								this.input.Atoms.Choose
							});
							break;
						}
					}
					else
					{
						XslNode xslNode = null;
						if (Ref.Equal(this.input.NamespaceUri, this.input.Atoms.UriXsl))
						{
							if (Ref.Equal(this.input.LocalName, this.input.Atoms.When))
							{
								if (flag)
								{
									this.ReportError("Xslt_WhenAfterOtherwise", new string[0]);
									this.input.SkipNode();
									goto IL_194;
								}
								flag2 = true;
								xslNode = this.XslIf();
							}
							else if (Ref.Equal(this.input.LocalName, this.input.Atoms.Otherwise))
							{
								if (flag)
								{
									this.ReportError("Xslt_DupOtherwise", new string[0]);
									this.input.SkipNode();
									goto IL_194;
								}
								flag = true;
								xslNode = this.XslOtherwise();
							}
						}
						if (xslNode == null)
						{
							this.ReportError("Xslt_UnexpectedElement", new string[]
							{
								this.input.QualifiedName,
								this.input.Atoms.Choose
							});
							this.input.SkipNode();
						}
						else
						{
							XsltLoader.AddInstruction(content, xslNode);
						}
					}
					IL_194:;
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			if (!flag2)
			{
				this.ReportError("Xslt_NoWhen", new string[0]);
			}
			return XsltLoader.SetInfo(AstFactory.Choose(), content, attributes);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000435E0 File Offset: 0x000425E0
		private XslNode XslOtherwise()
		{
			XsltInput.ContextInfo attributes = this.input.GetAttributes();
			return XsltLoader.SetInfo(AstFactory.Otherwise(), this.LoadInstructions(), attributes);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0004360C File Offset: 0x0004260C
		private XslNode XslForEach()
		{
			string select;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Select, out select);
			this.input.CanHaveApplyImports = false;
			List<XslNode> content = this.LoadInstructions(XsltLoader.InstructionFlags.AllowSort);
			attributes.SaveExtendedLineInfo(this.input);
			return XsltLoader.SetInfo(AstFactory.ForEach(select, attributes, this.input.XslVersion), content, attributes);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00043674 File Offset: 0x00042674
		private XslNode XslMessage()
		{
			string val;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.Terminate, out val);
			bool term = this.ParseYesNo(val, this.input.Atoms.Terminate) == TriState.True;
			return XsltLoader.SetInfo(AstFactory.Message(term), this.LoadEndTag(this.LoadInstructions()), attributes);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x000436D4 File Offset: 0x000426D4
		private XslNode XslNumber()
		{
			string text;
			string count;
			string from;
			string value;
			string text2;
			string lang;
			string letterValue;
			string groupingSeparator;
			string groupingSize;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.Level, out text, this.input.Atoms.Count, out count, this.input.Atoms.From, out from, this.input.Atoms.Value, out value, this.input.Atoms.Format, out text2, this.input.Atoms.Lang, out lang, this.input.Atoms.LetterValue, out letterValue, this.input.Atoms.GroupingSeparator, out groupingSeparator, this.input.Atoms.GroupingSize, out groupingSize);
			string a;
			if ((a = text) == null)
			{
				goto IL_F1;
			}
			NumberLevel level;
			if (!(a == "single"))
			{
				if (a == "multiple")
				{
					level = NumberLevel.Multiple;
					goto IL_131;
				}
				if (!(a == "any"))
				{
					goto IL_F1;
				}
				level = NumberLevel.Any;
				goto IL_131;
			}
			IL_E2:
			level = NumberLevel.Single;
			goto IL_131;
			IL_F1:
			if (text != null && !this.input.ForwardCompatibility)
			{
				this.ReportError("Xslt_InvalidAttrValue", new string[]
				{
					this.input.Atoms.Level,
					text
				});
				goto IL_E2;
			}
			goto IL_E2;
			IL_131:
			if (text2 == null)
			{
				text2 = "1";
			}
			this.CheckNoContent();
			return XsltLoader.SetInfo(AstFactory.Number(level, count, from, value, text2, lang, letterValue, groupingSeparator, groupingSize, this.input.XslVersion), null, attributes);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0004384C File Offset: 0x0004284C
		private XslNode XslValueOf()
		{
			string arg;
			string val;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Select, out arg, this.input.Atoms.DisableOutputEscaping, out val);
			bool flag = this.ParseYesNo(val, this.input.Atoms.DisableOutputEscaping) == TriState.True;
			this.CheckNoContent();
			return XsltLoader.SetInfo(AstFactory.XslNode(flag ? XslNodeType.ValueOfDoe : XslNodeType.ValueOf, null, arg, this.input.XslVersion), null, attributes);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000438D0 File Offset: 0x000428D0
		private VarPar XslVarPar(XslNodeType nodeType)
		{
			string text;
			string text2;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Name, out text, this.input.Atoms.Select, out text2);
			List<XslNode> list = this.LoadInstructions();
			if (list.Count != 0)
			{
				list = this.LoadEndTag(list);
			}
			if (text2 != null && list.Count != 0)
			{
				this.ReportError("Xslt_VariableCntSel2", new string[]
				{
					text
				});
			}
			VarPar varPar = AstFactory.VarPar(nodeType, this.CreateXPathQName(text), text2, this.input.XslVersion);
			XsltLoader.SetInfo(varPar, list, attributes);
			return varPar;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00043974 File Offset: 0x00042974
		private XslNode XslComment()
		{
			XsltInput.ContextInfo attributes = this.input.GetAttributes();
			return XsltLoader.SetInfo(AstFactory.Comment(), this.LoadEndTag(this.LoadInstructions()), attributes);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000439A4 File Offset: 0x000429A4
		private XslNode XslProcessingInstruction()
		{
			string phantomNCName;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Name, out phantomNCName);
			if (phantomNCName == null)
			{
				phantomNCName = this.compiler.PhantomNCName;
			}
			return XsltLoader.SetInfo(AstFactory.PI(phantomNCName, this.input.XslVersion), this.LoadEndTag(this.LoadInstructions()), attributes);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00043A04 File Offset: 0x00042A04
		private XslNode XslText()
		{
			string val;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.DisableOutputEscaping, out val);
			SerializationHints hints = (this.ParseYesNo(val, this.input.Atoms.DisableOutputEscaping) == TriState.True) ? SerializationHints.DisableOutputEscaping : SerializationHints.None;
			List<XslNode> list = new List<XslNode>();
			if (this.input.MoveToFirstChild())
			{
				do
				{
					switch (this.input.NodeType)
					{
					case XPathNodeType.Text:
					case XPathNodeType.SignificantWhitespace:
					case XPathNodeType.Whitespace:
						list.Add(AstFactory.Text(this.input.Value, hints));
						break;
					default:
						this.ReportError("Xslt_UnexpectedElement", new string[]
						{
							this.input.QualifiedName,
							this.input.Atoms.Text
						});
						this.input.SkipNode();
						break;
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			return XsltLoader.SetInfo(AstFactory.List(), list, attributes);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00043B14 File Offset: 0x00042B14
		private XslNode XslElement()
		{
			string phantomNCName;
			string text;
			string useAttributeSets;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Name, out phantomNCName, this.input.Atoms.Namespace, out text, this.input.Atoms.UseAttributeSets, out useAttributeSets);
			if (phantomNCName == null)
			{
				phantomNCName = this.compiler.PhantomNCName;
			}
			if (text == "http://www.w3.org/2000/xmlns/")
			{
				this.ReportError("Xslt_ReservedNS", new string[]
				{
					text
				});
			}
			List<XslNode> content = this.ParseUseAttributeSets(useAttributeSets, attributes.lineInfo);
			return XsltLoader.SetInfo(AstFactory.Element(phantomNCName, text, this.input.XslVersion), this.LoadEndTag(this.LoadInstructions(content)), attributes);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00043BD0 File Offset: 0x00042BD0
		private XslNode XslAttribute()
		{
			string phantomNCName;
			string text;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(1, this.input.Atoms.Name, out phantomNCName, this.input.Atoms.Namespace, out text);
			if (phantomNCName == null)
			{
				phantomNCName = this.compiler.PhantomNCName;
			}
			if (text == "http://www.w3.org/2000/xmlns/")
			{
				this.ReportError("Xslt_ReservedNS", new string[]
				{
					text
				});
			}
			return XsltLoader.SetInfo(AstFactory.Attribute(phantomNCName, text, this.input.XslVersion), this.LoadEndTag(this.LoadInstructions()), attributes);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00043C68 File Offset: 0x00042C68
		private XslNode XslSort()
		{
			string text;
			string lang;
			string dataType;
			string order;
			string caseOrder;
			XsltInput.ContextInfo attributes = this.input.GetAttributes(0, this.input.Atoms.Select, out text, this.input.Atoms.Lang, out lang, this.input.Atoms.DataType, out dataType, this.input.Atoms.Order, out order, this.input.Atoms.CaseOrder, out caseOrder);
			if (text == null)
			{
				text = ".";
			}
			this.CheckNoContent();
			return XsltLoader.SetInfo(AstFactory.Sort(text, lang, dataType, order, caseOrder, this.input.XslVersion), null, attributes);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00043D0C File Offset: 0x00042D0C
		private XslNode LoadLiteralResultElement(bool asStylesheet)
		{
			string prefix = this.input.Prefix;
			string localName = this.input.LocalName;
			string namespaceUri = this.input.NamespaceUri;
			string text = null;
			string value = null;
			string value2 = null;
			string useAttributeSets = null;
			string attName = null;
			List<XslNode> list = new List<XslNode>();
			XsltInput.ContextInfo contextInfo = new XsltInput.ContextInfo(this.input);
			while (this.input.MoveToNextAttOrNs())
			{
				if (this.input.NodeType == XPathNodeType.Namespace)
				{
					contextInfo.AddNamespace(this.input);
				}
				else
				{
					contextInfo.AddAttribute(this.input);
					if (this.input.IsXsltNamespace())
					{
						if (this.input.LocalName == this.input.Atoms.Version)
						{
							text = this.input.Value;
							attName = this.input.QualifiedName;
						}
						else if (this.input.LocalName == this.input.Atoms.ExtensionElementPrefixes)
						{
							value = this.input.Value;
						}
						else if (this.input.LocalName == this.input.Atoms.ExcludeResultPrefixes)
						{
							value2 = this.input.Value;
						}
						else if (this.input.LocalName == this.input.Atoms.UseAttributeSets)
						{
							useAttributeSets = this.input.Value;
						}
					}
					else
					{
						XslNode node = AstFactory.LiteralAttribute(AstFactory.QName(this.input.LocalName, this.input.NamespaceUri, this.input.Prefix), this.input.Value, this.input.XslVersion);
						XsltLoader.AddInstruction(list, XsltLoader.SetLineInfo(node, contextInfo.lineInfo));
					}
				}
			}
			contextInfo.Finish(this.input);
			if (text != null)
			{
				this.input.SetVersion(text, attName);
			}
			else if (asStylesheet)
			{
				if (Ref.Equal(namespaceUri, this.input.Atoms.UriWdXsl) && Ref.Equal(localName, this.input.Atoms.Stylesheet))
				{
					this.ReportError("Xslt_WdXslNamespace", new string[0]);
				}
				else
				{
					this.ReportError("Xslt_WrongStylesheetElement", new string[0]);
				}
				this.input.SkipNode();
				return null;
			}
			this.InsertExNamespaces(value, ref contextInfo.nsList, true);
			XslNode to;
			if (this.input.IsExtensionNamespace(namespaceUri))
			{
				list = this.LoadFallbacks(localName);
				to = AstFactory.List();
			}
			else
			{
				this.InsertExNamespaces(value2, ref contextInfo.nsList, false);
				list.InsertRange(0, this.ParseUseAttributeSets(useAttributeSets, contextInfo.lineInfo));
				list = this.LoadEndTag(this.LoadInstructions(list));
				to = AstFactory.LiteralElement(AstFactory.QName(localName, namespaceUri, prefix));
			}
			return XsltLoader.SetInfo(to, list, contextInfo);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00043FE8 File Offset: 0x00042FE8
		private void CheckWithParam(List<XslNode> content, XslNode withParam)
		{
			foreach (XslNode xslNode in content)
			{
				if (xslNode.NodeType == XslNodeType.WithParam && xslNode.Name.Equals(withParam.Name))
				{
					this.ReportError("Xslt_DuplicateWithParam", new string[]
					{
						withParam.Name.QualifiedName
					});
					break;
				}
			}
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00044070 File Offset: 0x00043070
		private static void AddInstruction(List<XslNode> content, XslNode instruction)
		{
			if (instruction != null)
			{
				content.Add(instruction);
			}
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0004407C File Offset: 0x0004307C
		private List<XslNode> LoadEndTag(List<XslNode> content)
		{
			if (this.compiler.IsDebug && !this.input.IsEmptyElement)
			{
				XsltLoader.AddInstruction(content, XsltLoader.SetLineInfo(AstFactory.Nop(), this.input.BuildLineInfo()));
			}
			return content;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x000440B4 File Offset: 0x000430B4
		private XslNode LoadUnknownXsltInstruction(string parentName)
		{
			if (!this.input.ForwardCompatibility)
			{
				this.ReportError("Xslt_UnexpectedElementQ", new string[]
				{
					this.input.QualifiedName,
					parentName
				});
				this.input.SkipNode();
				return null;
			}
			XsltInput.ContextInfo attributes = this.input.GetAttributes();
			List<XslNode> content = this.LoadFallbacks(this.input.LocalName);
			return XsltLoader.SetInfo(AstFactory.List(), content, attributes);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0004412C File Offset: 0x0004312C
		private List<XslNode> LoadFallbacks(string instrName)
		{
			List<XslNode> list = new List<XslNode>();
			if (this.input.MoveToFirstChild())
			{
				do
				{
					if (Ref.Equal(this.input.NamespaceUri, this.input.Atoms.UriXsl) && Ref.Equal(this.input.LocalName, this.input.Atoms.Fallback))
					{
						XsltInput.ContextInfo attributes = this.input.GetAttributes();
						list.Add(XsltLoader.SetInfo(AstFactory.List(), this.LoadInstructions(), attributes));
					}
					else
					{
						this.input.SkipNode();
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
			if (list.Count == 0)
			{
				list.Add(AstFactory.Error(XslLoadException.CreateMessage(this.input.BuildLineInfo(), "Xslt_UnknownExtensionElement", new string[]
				{
					instrName
				})));
			}
			return list;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00044214 File Offset: 0x00043214
		private QilName ParseMode(string qname)
		{
			if (qname == null)
			{
				return XsltLoader.nullMode;
			}
			this.compiler.EnterForwardsCompatible();
			QilName result = this.CreateXPathQName(qname);
			if (!this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility))
			{
				result = XsltLoader.nullMode;
			}
			return result;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004425C File Offset: 0x0004325C
		private void ResolveQName(bool ignoreDefaultNs, string qname, out string localName, out string namespaceName, out string prefix)
		{
			if (qname == null)
			{
				prefix = this.compiler.PhantomNCName;
				localName = this.compiler.PhantomNCName;
				namespaceName = this.compiler.CreatePhantomNamespace();
				return;
			}
			if (!this.compiler.ParseQName(qname, out prefix, out localName, this))
			{
				namespaceName = this.compiler.CreatePhantomNamespace();
				return;
			}
			if (ignoreDefaultNs && prefix.Length == 0)
			{
				namespaceName = string.Empty;
				return;
			}
			namespaceName = this.input.LookupXmlNamespace(prefix);
			if (namespaceName == null)
			{
				namespaceName = this.compiler.CreatePhantomNamespace();
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000442F4 File Offset: 0x000432F4
		private QilName CreateXPathQName(string qname)
		{
			string local;
			string uri;
			string prefix;
			this.ResolveQName(true, qname, out local, out uri, out prefix);
			return AstFactory.QName(local, uri, prefix);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00044318 File Offset: 0x00043318
		private XmlQualifiedName ResolveQName(bool ignoreDefaultNs, string qname)
		{
			string name;
			string ns;
			string text;
			this.ResolveQName(ignoreDefaultNs, qname, out name, out ns, out text);
			return new XmlQualifiedName(name, ns);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0004433C File Offset: 0x0004333C
		private void ParseWhitespaceRules(string elements, bool preserveSpace)
		{
			if (elements != null && elements.Length != 0)
			{
				string[] array = XmlConvert.SplitString(elements);
				for (int i = 0; i < array.Length; i++)
				{
					string text;
					string text2;
					string text3;
					if (!this.compiler.ParseNameTest(array[i], out text, out text2, this))
					{
						text3 = this.compiler.CreatePhantomNamespace();
					}
					else if (text == null || text.Length == 0)
					{
						text3 = text;
					}
					else
					{
						text3 = this.input.LookupXmlNamespace(text);
						if (text3 == null)
						{
							text3 = this.compiler.CreatePhantomNamespace();
						}
					}
					int index = ((text2 == null) ? 1 : 0) + ((text3 == null) ? 1 : 0);
					this.curStylesheet.AddWhitespaceRule(index, new WhitespaceRule(text2, text3, preserveSpace));
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000443F0 File Offset: 0x000433F0
		private XmlQualifiedName ParseOutputMethod(string attValue, out XmlOutputMethod method)
		{
			string text;
			string text2;
			string text3;
			this.ResolveQName(true, attValue, out text, out text2, out text3);
			method = XmlOutputMethod.AutoDetect;
			if (this.compiler.IsPhantomNamespace(text2))
			{
				return null;
			}
			if (text3.Length == 0)
			{
				string a;
				if ((a = text) != null)
				{
					if (a == "xml")
					{
						method = XmlOutputMethod.Xml;
						goto IL_BD;
					}
					if (a == "html")
					{
						method = XmlOutputMethod.Html;
						goto IL_BD;
					}
					if (a == "text")
					{
						method = XmlOutputMethod.Text;
						goto IL_BD;
					}
				}
				this.ReportError("Xslt_InvalidAttrValue", new string[]
				{
					this.input.Atoms.Method,
					attValue
				});
				return null;
			}
			if (!this.input.ForwardCompatibility)
			{
				this.ReportWarning("Xslt_InvalidMethod", new string[]
				{
					attValue
				});
			}
			IL_BD:
			return new XmlQualifiedName(text, text2);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000444C4 File Offset: 0x000434C4
		private List<XslNode> ParseUseAttributeSets(string useAttributeSets, ISourceLineInfo lineInfo)
		{
			List<XslNode> list = new List<XslNode>();
			if (useAttributeSets != null && useAttributeSets.Length != 0)
			{
				this.compiler.EnterForwardsCompatible();
				string[] array = XmlConvert.SplitString(useAttributeSets);
				for (int i = 0; i < array.Length; i++)
				{
					XsltLoader.AddInstruction(list, XsltLoader.SetLineInfo(AstFactory.UseAttributeSet(this.CreateXPathQName(array[i])), lineInfo));
				}
				if (!this.compiler.ExitForwardsCompatible(this.input.ForwardCompatibility))
				{
					list = new List<XslNode>();
				}
			}
			return list;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0004453C File Offset: 0x0004353C
		private TriState ParseYesNo(string val, string attName)
		{
			if (val == null)
			{
				return TriState.Unknown;
			}
			if (val == "yes")
			{
				return TriState.True;
			}
			if (!(val == "no"))
			{
				if (!this.input.ForwardCompatibility)
				{
					this.ReportError("Xslt_BistateAttribute", new string[]
					{
						attName,
						"yes",
						"no"
					});
				}
				return TriState.Unknown;
			}
			return TriState.False;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000445A8 File Offset: 0x000435A8
		private char ParseCharAttribute(string attValue, char defaultValue, string attName)
		{
			if (attValue == null)
			{
				return defaultValue;
			}
			if (attValue.Length != 1)
			{
				if (!this.input.ForwardCompatibility)
				{
					this.ReportError("Xslt_CharAttribute", new string[]
					{
						attName
					});
				}
				return defaultValue;
			}
			return attValue[0];
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x000445F0 File Offset: 0x000435F0
		private void CheckNoContent()
		{
			string qualifiedName = this.input.QualifiedName;
			bool flag = false;
			if (this.input.MoveToFirstChild())
			{
				do
				{
					if (this.input.NodeType != XPathNodeType.Whitespace)
					{
						if (!flag)
						{
							this.ReportError("Xslt_NotEmptyContents", new string[]
							{
								qualifiedName
							});
							flag = true;
						}
						this.input.SkipNode();
					}
				}
				while (this.input.MoveToNextSibling());
				this.input.MoveToParent();
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00044666 File Offset: 0x00043666
		private static XslNode SetLineInfo(XslNode node, ISourceLineInfo lineInfo)
		{
			node.SourceLine = lineInfo;
			return node;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00044670 File Offset: 0x00043670
		private static void SetContent(XslNode node, List<XslNode> content)
		{
			if (content != null && content.Count == 0)
			{
				content = null;
			}
			node.SetContent(content);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00044687 File Offset: 0x00043687
		private static XslNode SetInfo(XslNode to, List<XslNode> content, XsltInput.ContextInfo info)
		{
			to.Namespaces = info.nsList;
			XsltLoader.SetContent(to, content);
			XsltLoader.SetLineInfo(to, info.lineInfo);
			return to;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x000446AC File Offset: 0x000436AC
		private static NsDecl MergeNamespaces(NsDecl thisList, NsDecl parentList)
		{
			if (parentList == null)
			{
				return thisList;
			}
			if (thisList == null)
			{
				return parentList;
			}
			while (parentList != null)
			{
				bool flag = false;
				for (NsDecl nsDecl = thisList; nsDecl != null; nsDecl = nsDecl.Prev)
				{
					if (Ref.Equal(nsDecl.Prefix, parentList.Prefix) && (nsDecl.Prefix != null || nsDecl.NsUri == parentList.NsUri))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					thisList = new NsDecl(thisList, parentList.Prefix, parentList.NsUri);
				}
				parentList = parentList.Prev;
			}
			return thisList;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00044728 File Offset: 0x00043728
		public void ReportError(string res, params string[] args)
		{
			this.compiler.ReportError(this.input.BuildLineInfo(), res, args);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00044742 File Offset: 0x00043742
		public void ReportWarning(string res, params string[] args)
		{
			this.compiler.ReportWarning(this.input.BuildLineInfo(), res, args);
		}

		// Token: 0x040008D1 RID: 2257
		private const int MAX_LOADINSTRUCTIONS_DEPTH = 1024;

		// Token: 0x040008D2 RID: 2258
		private Compiler compiler;

		// Token: 0x040008D3 RID: 2259
		private XmlResolver xmlResolver;

		// Token: 0x040008D4 RID: 2260
		private QueryReaderSettings readerSettings;

		// Token: 0x040008D5 RID: 2261
		private XsltInput input;

		// Token: 0x040008D6 RID: 2262
		private Stylesheet curStylesheet;

		// Token: 0x040008D7 RID: 2263
		private Template curTemplate;

		// Token: 0x040008D8 RID: 2264
		private static QilName nullMode = AstFactory.QName(string.Empty);

		// Token: 0x040008D9 RID: 2265
		private HybridDictionary documentUriInUse = new HybridDictionary();

		// Token: 0x040008DA RID: 2266
		private int loadInstructionsDepth;

		// Token: 0x0200012A RID: 298
		private enum InstructionFlags
		{
			// Token: 0x040008DC RID: 2268
			NoParamNoSort,
			// Token: 0x040008DD RID: 2269
			AllowParam,
			// Token: 0x040008DE RID: 2270
			AllowSort
		}
	}
}
