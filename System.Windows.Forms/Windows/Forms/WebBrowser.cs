using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000726 RID: 1830
	[Designer("System.Windows.Forms.Design.WebBrowserDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Url")]
	[DefaultEvent("DocumentCompleted")]
	[Docking(DockingBehavior.AutoDock)]
	[SRDescription("DescriptionWebBrowser")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class WebBrowser : WebBrowserBase
	{
		// Token: 0x06006187 RID: 24967 RVA: 0x00164333 File Offset: 0x00163333
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public WebBrowser() : base("8856f961-340a-11d0-a96b-00c04fd705a2")
		{
			this.CheckIfCreatedInIE();
			this.webBrowserState = new BitVector32(37);
			this.AllowNavigation = true;
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x06006188 RID: 24968 RVA: 0x00164365 File Offset: 0x00163365
		// (set) Token: 0x06006189 RID: 24969 RVA: 0x00164374 File Offset: 0x00163374
		[SRCategory("CatBehavior")]
		[SRDescription("WebBrowserAllowNavigationDescr")]
		[DefaultValue(true)]
		public bool AllowNavigation
		{
			get
			{
				return this.webBrowserState[64];
			}
			set
			{
				this.webBrowserState[64] = value;
				if (this.webBrowserEvent != null)
				{
					this.webBrowserEvent.AllowNavigation = value;
				}
			}
		}

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x0600618A RID: 24970 RVA: 0x00164398 File Offset: 0x00163398
		// (set) Token: 0x0600618B RID: 24971 RVA: 0x001643A5 File Offset: 0x001633A5
		[SRCategory("CatBehavior")]
		[SRDescription("WebBrowserAllowWebBrowserDropDescr")]
		[DefaultValue(true)]
		public bool AllowWebBrowserDrop
		{
			get
			{
				return this.AxIWebBrowser2.RegisterAsDropTarget;
			}
			set
			{
				if (value != this.AllowWebBrowserDrop)
				{
					this.AxIWebBrowser2.RegisterAsDropTarget = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x0600618C RID: 24972 RVA: 0x001643C2 File Offset: 0x001633C2
		// (set) Token: 0x0600618D RID: 24973 RVA: 0x001643CF File Offset: 0x001633CF
		[SRDescription("WebBrowserScriptErrorsSuppressedDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ScriptErrorsSuppressed
		{
			get
			{
				return this.AxIWebBrowser2.Silent;
			}
			set
			{
				if (value != this.ScriptErrorsSuppressed)
				{
					this.AxIWebBrowser2.Silent = value;
				}
			}
		}

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x0600618E RID: 24974 RVA: 0x001643E6 File Offset: 0x001633E6
		// (set) Token: 0x0600618F RID: 24975 RVA: 0x001643F4 File Offset: 0x001633F4
		[SRDescription("WebBrowserWebBrowserShortcutsEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool WebBrowserShortcutsEnabled
		{
			get
			{
				return this.webBrowserState[1];
			}
			set
			{
				this.webBrowserState[1] = value;
			}
		}

		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x06006190 RID: 24976 RVA: 0x00164403 File Offset: 0x00163403
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool CanGoBack
		{
			get
			{
				return this.CanGoBackInternal;
			}
		}

		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x06006191 RID: 24977 RVA: 0x0016440B File Offset: 0x0016340B
		// (set) Token: 0x06006192 RID: 24978 RVA: 0x00164419 File Offset: 0x00163419
		internal bool CanGoBackInternal
		{
			get
			{
				return this.webBrowserState[8];
			}
			set
			{
				if (value != this.CanGoBackInternal)
				{
					this.webBrowserState[8] = value;
					this.OnCanGoBackChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x06006193 RID: 24979 RVA: 0x0016443C File Offset: 0x0016343C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool CanGoForward
		{
			get
			{
				return this.CanGoForwardInternal;
			}
		}

		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x06006194 RID: 24980 RVA: 0x00164444 File Offset: 0x00163444
		// (set) Token: 0x06006195 RID: 24981 RVA: 0x00164453 File Offset: 0x00163453
		internal bool CanGoForwardInternal
		{
			get
			{
				return this.webBrowserState[16];
			}
			set
			{
				if (value != this.CanGoForwardInternal)
				{
					this.webBrowserState[16] = value;
					this.OnCanGoForwardChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x06006196 RID: 24982 RVA: 0x00164478 File Offset: 0x00163478
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HtmlDocument Document
		{
			get
			{
				object document = this.AxIWebBrowser2.Document;
				if (document != null)
				{
					UnsafeNativeMethods.IHTMLDocument2 ihtmldocument = document as UnsafeNativeMethods.IHTMLDocument2;
					if (ihtmldocument != null)
					{
						UnsafeNativeMethods.IHTMLLocation location = ihtmldocument.GetLocation();
						if (location != null)
						{
							string href = location.GetHref();
							if (!string.IsNullOrEmpty(href))
							{
								Uri url = new Uri(href);
								WebBrowser.EnsureUrlConnectPermission(url);
								return new HtmlDocument(this.ShimManager, ihtmldocument as UnsafeNativeMethods.IHTMLDocument);
							}
						}
					}
				}
				return null;
			}
		}

		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x06006197 RID: 24983 RVA: 0x001644DC File Offset: 0x001634DC
		// (set) Token: 0x06006198 RID: 24984 RVA: 0x00164538 File Offset: 0x00163538
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Stream DocumentStream
		{
			get
			{
				HtmlDocument document = this.Document;
				if (document == null)
				{
					return null;
				}
				UnsafeNativeMethods.IPersistStreamInit persistStreamInit = document.DomDocument as UnsafeNativeMethods.IPersistStreamInit;
				if (persistStreamInit == null)
				{
					return null;
				}
				MemoryStream memoryStream = new MemoryStream();
				UnsafeNativeMethods.IStream pstm = new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream);
				persistStreamInit.Save(pstm, false);
				return new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, false);
			}
			set
			{
				this.documentStreamToSetOnLoad = value;
				try
				{
					this.webBrowserState[2] = true;
					this.Url = new Uri("about:blank");
				}
				finally
				{
					this.webBrowserState[2] = false;
				}
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x06006199 RID: 24985 RVA: 0x0016458C File Offset: 0x0016358C
		// (set) Token: 0x0600619A RID: 24986 RVA: 0x001645C0 File Offset: 0x001635C0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string DocumentText
		{
			get
			{
				Stream documentStream = this.DocumentStream;
				if (documentStream == null)
				{
					return "";
				}
				StreamReader streamReader = new StreamReader(documentStream);
				documentStream.Position = 0L;
				return streamReader.ReadToEnd();
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				MemoryStream memoryStream = new MemoryStream(value.Length);
				StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
				streamWriter.Write(value);
				streamWriter.Flush();
				memoryStream.Position = 0L;
				this.DocumentStream = memoryStream;
			}
		}

		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x0600619B RID: 24987 RVA: 0x0016460C File Offset: 0x0016360C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DocumentTitle
		{
			get
			{
				HtmlDocument document = this.Document;
				string result;
				if (document == null)
				{
					result = this.AxIWebBrowser2.LocationName;
				}
				else
				{
					UnsafeNativeMethods.IHTMLDocument2 ihtmldocument = document.DomDocument as UnsafeNativeMethods.IHTMLDocument2;
					try
					{
						result = ihtmldocument.GetTitle();
					}
					catch (COMException)
					{
						result = "";
					}
				}
				return result;
			}
		}

		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x0600619C RID: 24988 RVA: 0x00164668 File Offset: 0x00163668
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string DocumentType
		{
			get
			{
				string result = "";
				HtmlDocument document = this.Document;
				if (document != null)
				{
					UnsafeNativeMethods.IHTMLDocument2 ihtmldocument = document.DomDocument as UnsafeNativeMethods.IHTMLDocument2;
					try
					{
						result = ihtmldocument.GetMimeType();
					}
					catch (COMException)
					{
						result = "";
					}
				}
				return result;
			}
		}

		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x0600619D RID: 24989 RVA: 0x001646BC File Offset: 0x001636BC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public WebBrowserEncryptionLevel EncryptionLevel
		{
			get
			{
				if (this.Document == null)
				{
					this.encryptionLevel = WebBrowserEncryptionLevel.Unknown;
				}
				return this.encryptionLevel;
			}
		}

		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x0600619E RID: 24990 RVA: 0x001646D9 File Offset: 0x001636D9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsBusy
		{
			get
			{
				return !(this.Document == null) && this.AxIWebBrowser2.Busy;
			}
		}

		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x0600619F RID: 24991 RVA: 0x001646F6 File Offset: 0x001636F6
		[Browsable(false)]
		[SRDescription("WebBrowserIsOfflineDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOffline
		{
			get
			{
				return this.AxIWebBrowser2.Offline;
			}
		}

		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x060061A0 RID: 24992 RVA: 0x00164703 File Offset: 0x00163703
		// (set) Token: 0x060061A1 RID: 24993 RVA: 0x00164711 File Offset: 0x00163711
		[SRDescription("WebBrowserIsWebBrowserContextMenuEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool IsWebBrowserContextMenuEnabled
		{
			get
			{
				return this.webBrowserState[4];
			}
			set
			{
				this.webBrowserState[4] = value;
			}
		}

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x060061A2 RID: 24994 RVA: 0x00164720 File Offset: 0x00163720
		// (set) Token: 0x060061A3 RID: 24995 RVA: 0x00164728 File Offset: 0x00163728
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object ObjectForScripting
		{
			get
			{
				return this.objectForScripting;
			}
			set
			{
				if (value != null)
				{
					Type type = value.GetType();
					if (!Marshal.IsTypeVisibleFromCom(type))
					{
						throw new ArgumentException(SR.GetString("WebBrowserObjectForScriptingComVisibleOnly"));
					}
				}
				this.objectForScripting = value;
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x060061A4 RID: 24996 RVA: 0x0016475E File Offset: 0x0016375E
		// (set) Token: 0x060061A5 RID: 24997 RVA: 0x00164766 File Offset: 0x00163766
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x140003DF RID: 991
		// (add) Token: 0x060061A6 RID: 24998 RVA: 0x0016476F File Offset: 0x0016376F
		// (remove) Token: 0x060061A7 RID: 24999 RVA: 0x00164778 File Offset: 0x00163778
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnPaddingChangedDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x060061A8 RID: 25000 RVA: 0x00164781 File Offset: 0x00163781
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebBrowserReadyState ReadyState
		{
			get
			{
				if (this.Document == null)
				{
					return WebBrowserReadyState.Uninitialized;
				}
				return this.AxIWebBrowser2.ReadyState;
			}
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x060061A9 RID: 25001 RVA: 0x0016479E File Offset: 0x0016379E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual string StatusText
		{
			get
			{
				if (this.Document == null)
				{
					this.statusText = "";
				}
				return this.statusText;
			}
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x060061AA RID: 25002 RVA: 0x001647C0 File Offset: 0x001637C0
		// (set) Token: 0x060061AB RID: 25003 RVA: 0x00164804 File Offset: 0x00163804
		[Bindable(true)]
		[SRDescription("WebBrowserUrlDescr")]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(WebBrowserUriTypeConverter))]
		[DefaultValue(null)]
		public Uri Url
		{
			get
			{
				string locationURL = this.AxIWebBrowser2.LocationURL;
				if (string.IsNullOrEmpty(locationURL))
				{
					return null;
				}
				Uri result;
				try
				{
					result = new Uri(locationURL);
				}
				catch (UriFormatException)
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null && value.ToString() == "")
				{
					value = null;
				}
				this.PerformNavigateHelper(this.ReadyNavigateToUrl(value), false, null, null, null);
			}
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x060061AC RID: 25004 RVA: 0x00164838 File Offset: 0x00163838
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Version Version
		{
			get
			{
				string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "mshtml.dll");
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
				return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
			}
		}

		// Token: 0x060061AD RID: 25005 RVA: 0x0016487C File Offset: 0x0016387C
		public bool GoBack()
		{
			bool result = true;
			try
			{
				this.AxIWebBrowser2.GoBack();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x001648B8 File Offset: 0x001638B8
		public bool GoForward()
		{
			bool result = true;
			try
			{
				this.AxIWebBrowser2.GoForward();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x001648F4 File Offset: 0x001638F4
		public void GoHome()
		{
			this.AxIWebBrowser2.GoHome();
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x00164901 File Offset: 0x00163901
		public void GoSearch()
		{
			this.AxIWebBrowser2.GoSearch();
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x0016490E File Offset: 0x0016390E
		public void Navigate(Uri url)
		{
			this.Url = url;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x00164917 File Offset: 0x00163917
		public void Navigate(string urlString)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), false, null, null, null);
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x0016492A File Offset: 0x0016392A
		public void Navigate(Uri url, string targetFrameName)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(url), false, targetFrameName, null, null);
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x0016493D File Offset: 0x0016393D
		public void Navigate(string urlString, string targetFrameName)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), false, targetFrameName, null, null);
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x00164950 File Offset: 0x00163950
		public void Navigate(Uri url, bool newWindow)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(url), newWindow, null, null, null);
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x00164963 File Offset: 0x00163963
		public void Navigate(string urlString, bool newWindow)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), newWindow, null, null, null);
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x00164976 File Offset: 0x00163976
		public void Navigate(Uri url, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(url), false, targetFrameName, postData, additionalHeaders);
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x0016498A File Offset: 0x0016398A
		public void Navigate(string urlString, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), false, targetFrameName, postData, additionalHeaders);
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x001649A0 File Offset: 0x001639A0
		public void Print()
		{
			IntSecurity.DefaultPrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINT, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x001649EC File Offset: 0x001639EC
		public override void Refresh()
		{
			try
			{
				if (this.ShouldSerializeDocumentText())
				{
					string documentText = this.DocumentText;
					this.AxIWebBrowser2.Refresh();
					this.DocumentText = documentText;
				}
				else
				{
					this.AxIWebBrowser2.Refresh();
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x00164A48 File Offset: 0x00163A48
		public void Refresh(WebBrowserRefreshOption opt)
		{
			object obj = opt;
			try
			{
				if (this.ShouldSerializeDocumentText())
				{
					string documentText = this.DocumentText;
					this.AxIWebBrowser2.Refresh2(ref obj);
					this.DocumentText = documentText;
				}
				else
				{
					this.AxIWebBrowser2.Refresh2(ref obj);
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x060061BC RID: 25020 RVA: 0x00164AB0 File Offset: 0x00163AB0
		// (set) Token: 0x060061BD RID: 25021 RVA: 0x00164ABF File Offset: 0x00163ABF
		[SRDescription("WebBrowserScrollBarsEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ScrollBarsEnabled
		{
			get
			{
				return this.webBrowserState[32];
			}
			set
			{
				if (value != this.webBrowserState[32])
				{
					this.webBrowserState[32] = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x00164AE8 File Offset: 0x00163AE8
		public void ShowPageSetupDialog()
		{
			IntSecurity.SafePrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PAGESETUP, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x00164B34 File Offset: 0x00163B34
		public void ShowPrintDialog()
		{
			IntSecurity.SafePrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINT, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x00164B80 File Offset: 0x00163B80
		public void ShowPrintPreviewDialog()
		{
			IntSecurity.SafePrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINTPREVIEW, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00164BCC File Offset: 0x00163BCC
		public void ShowPropertiesDialog()
		{
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PROPERTIES, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x00164C10 File Offset: 0x00163C10
		public void ShowSaveAsDialog()
		{
			IntSecurity.FileDialogSaveFile.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_SAVEAS, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00164C5C File Offset: 0x00163C5C
		public void Stop()
		{
			try
			{
				this.AxIWebBrowser2.Stop();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			catch
			{
			}
		}

		// Token: 0x140003E0 RID: 992
		// (add) Token: 0x060061C4 RID: 25028 RVA: 0x00164CA4 File Offset: 0x00163CA4
		// (remove) Token: 0x060061C5 RID: 25029 RVA: 0x00164CBD File Offset: 0x00163CBD
		[SRDescription("WebBrowserCanGoBackChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		[Browsable(false)]
		public event EventHandler CanGoBackChanged;

		// Token: 0x140003E1 RID: 993
		// (add) Token: 0x060061C6 RID: 25030 RVA: 0x00164CD6 File Offset: 0x00163CD6
		// (remove) Token: 0x060061C7 RID: 25031 RVA: 0x00164CEF File Offset: 0x00163CEF
		[SRDescription("WebBrowserCanGoForwardChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		[Browsable(false)]
		public event EventHandler CanGoForwardChanged;

		// Token: 0x140003E2 RID: 994
		// (add) Token: 0x060061C8 RID: 25032 RVA: 0x00164D08 File Offset: 0x00163D08
		// (remove) Token: 0x060061C9 RID: 25033 RVA: 0x00164D21 File Offset: 0x00163D21
		[SRCategory("CatBehavior")]
		[SRDescription("WebBrowserDocumentCompletedDescr")]
		public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;

		// Token: 0x140003E3 RID: 995
		// (add) Token: 0x060061CA RID: 25034 RVA: 0x00164D3A File Offset: 0x00163D3A
		// (remove) Token: 0x060061CB RID: 25035 RVA: 0x00164D53 File Offset: 0x00163D53
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserDocumentTitleChangedDescr")]
		[Browsable(false)]
		public event EventHandler DocumentTitleChanged;

		// Token: 0x140003E4 RID: 996
		// (add) Token: 0x060061CC RID: 25036 RVA: 0x00164D6C File Offset: 0x00163D6C
		// (remove) Token: 0x060061CD RID: 25037 RVA: 0x00164D85 File Offset: 0x00163D85
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserEncryptionLevelChangedDescr")]
		[Browsable(false)]
		public event EventHandler EncryptionLevelChanged;

		// Token: 0x140003E5 RID: 997
		// (add) Token: 0x060061CE RID: 25038 RVA: 0x00164D9E File Offset: 0x00163D9E
		// (remove) Token: 0x060061CF RID: 25039 RVA: 0x00164DB7 File Offset: 0x00163DB7
		[SRCategory("CatBehavior")]
		[SRDescription("WebBrowserFileDownloadDescr")]
		public event EventHandler FileDownload;

		// Token: 0x140003E6 RID: 998
		// (add) Token: 0x060061D0 RID: 25040 RVA: 0x00164DD0 File Offset: 0x00163DD0
		// (remove) Token: 0x060061D1 RID: 25041 RVA: 0x00164DE9 File Offset: 0x00163DE9
		[SRCategory("CatAction")]
		[SRDescription("WebBrowserNavigatedDescr")]
		public event WebBrowserNavigatedEventHandler Navigated;

		// Token: 0x140003E7 RID: 999
		// (add) Token: 0x060061D2 RID: 25042 RVA: 0x00164E02 File Offset: 0x00163E02
		// (remove) Token: 0x060061D3 RID: 25043 RVA: 0x00164E1B File Offset: 0x00163E1B
		[SRDescription("WebBrowserNavigatingDescr")]
		[SRCategory("CatAction")]
		public event WebBrowserNavigatingEventHandler Navigating;

		// Token: 0x140003E8 RID: 1000
		// (add) Token: 0x060061D4 RID: 25044 RVA: 0x00164E34 File Offset: 0x00163E34
		// (remove) Token: 0x060061D5 RID: 25045 RVA: 0x00164E4D File Offset: 0x00163E4D
		[SRDescription("WebBrowserNewWindowDescr")]
		[SRCategory("CatAction")]
		public event CancelEventHandler NewWindow;

		// Token: 0x140003E9 RID: 1001
		// (add) Token: 0x060061D6 RID: 25046 RVA: 0x00164E66 File Offset: 0x00163E66
		// (remove) Token: 0x060061D7 RID: 25047 RVA: 0x00164E7F File Offset: 0x00163E7F
		[SRCategory("CatAction")]
		[SRDescription("WebBrowserProgressChangedDescr")]
		public event WebBrowserProgressChangedEventHandler ProgressChanged;

		// Token: 0x140003EA RID: 1002
		// (add) Token: 0x060061D8 RID: 25048 RVA: 0x00164E98 File Offset: 0x00163E98
		// (remove) Token: 0x060061D9 RID: 25049 RVA: 0x00164EB1 File Offset: 0x00163EB1
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserStatusTextChangedDescr")]
		[Browsable(false)]
		public event EventHandler StatusTextChanged;

		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x060061DA RID: 25050 RVA: 0x00164ECC File Offset: 0x00163ECC
		public override bool Focused
		{
			get
			{
				if (base.Focused)
				{
					return true;
				}
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				return focus != IntPtr.Zero && SafeNativeMethods.IsChild(new HandleRef(this, base.Handle), new HandleRef(null, focus));
			}
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x00164F10 File Offset: 0x00163F10
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.htmlShimManager != null)
				{
					this.htmlShimManager.Dispose();
				}
				this.DetachSink();
				base.ActiveXSite.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x060061DC RID: 25052 RVA: 0x00164F40 File Offset: 0x00163F40
		protected override Size DefaultSize
		{
			get
			{
				return new Size(250, 250);
			}
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x00164F51 File Offset: 0x00163F51
		protected override void AttachInterfaces(object nativeActiveXObject)
		{
			this.axIWebBrowser2 = (UnsafeNativeMethods.IWebBrowser2)nativeActiveXObject;
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x00164F5F File Offset: 0x00163F5F
		protected override void DetachInterfaces()
		{
			this.axIWebBrowser2 = null;
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x00164F68 File Offset: 0x00163F68
		protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
		{
			return new WebBrowser.WebBrowserSite(this);
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x00164F70 File Offset: 0x00163F70
		protected override void CreateSink()
		{
			object activeXInstance = this.activeXInstance;
			if (activeXInstance != null)
			{
				this.webBrowserEvent = new WebBrowser.WebBrowserEvent(this);
				this.webBrowserEvent.AllowNavigation = this.AllowNavigation;
				this.cookie = new AxHost.ConnectionPointCookie(activeXInstance, this.webBrowserEvent, typeof(UnsafeNativeMethods.DWebBrowserEvents2));
			}
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x00164FC0 File Offset: 0x00163FC0
		protected override void DetachSink()
		{
			if (this.cookie != null)
			{
				this.cookie.Disconnect();
				this.cookie = null;
			}
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x00164FDC File Offset: 0x00163FDC
		internal override void OnTopMostActiveXParentChanged(EventArgs e)
		{
			if (base.TopMostParent.IsIEParent)
			{
				WebBrowser.createdInIE = true;
				this.CheckIfCreatedInIE();
				return;
			}
			WebBrowser.createdInIE = false;
			base.OnTopMostActiveXParentChanged(e);
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x00165005 File Offset: 0x00164005
		protected virtual void OnCanGoBackChanged(EventArgs e)
		{
			if (this.CanGoBackChanged != null)
			{
				this.CanGoBackChanged(this, e);
			}
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x0016501C File Offset: 0x0016401C
		protected virtual void OnCanGoForwardChanged(EventArgs e)
		{
			if (this.CanGoForwardChanged != null)
			{
				this.CanGoForwardChanged(this, e);
			}
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x00165033 File Offset: 0x00164033
		protected virtual void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
		{
			this.AxIWebBrowser2.RegisterAsDropTarget = this.AllowWebBrowserDrop;
			if (this.DocumentCompleted != null)
			{
				this.DocumentCompleted(this, e);
			}
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x0016505B File Offset: 0x0016405B
		protected virtual void OnDocumentTitleChanged(EventArgs e)
		{
			if (this.DocumentTitleChanged != null)
			{
				this.DocumentTitleChanged(this, e);
			}
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x00165072 File Offset: 0x00164072
		protected virtual void OnEncryptionLevelChanged(EventArgs e)
		{
			if (this.EncryptionLevelChanged != null)
			{
				this.EncryptionLevelChanged(this, e);
			}
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x00165089 File Offset: 0x00164089
		protected virtual void OnFileDownload(EventArgs e)
		{
			if (this.FileDownload != null)
			{
				this.FileDownload(this, e);
			}
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x001650A0 File Offset: 0x001640A0
		protected virtual void OnNavigated(WebBrowserNavigatedEventArgs e)
		{
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		// Token: 0x060061EA RID: 25066 RVA: 0x001650B7 File Offset: 0x001640B7
		protected virtual void OnNavigating(WebBrowserNavigatingEventArgs e)
		{
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		// Token: 0x060061EB RID: 25067 RVA: 0x001650CE File Offset: 0x001640CE
		protected virtual void OnNewWindow(CancelEventArgs e)
		{
			if (this.NewWindow != null)
			{
				this.NewWindow(this, e);
			}
		}

		// Token: 0x060061EC RID: 25068 RVA: 0x001650E5 File Offset: 0x001640E5
		protected virtual void OnProgressChanged(WebBrowserProgressChangedEventArgs e)
		{
			if (this.ProgressChanged != null)
			{
				this.ProgressChanged(this, e);
			}
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x001650FC File Offset: 0x001640FC
		protected virtual void OnStatusTextChanged(EventArgs e)
		{
			if (this.StatusTextChanged != null)
			{
				this.StatusTextChanged(this, e);
			}
		}

		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x060061EE RID: 25070 RVA: 0x00165113 File Offset: 0x00164113
		internal HtmlShimManager ShimManager
		{
			get
			{
				if (this.htmlShimManager == null)
				{
					this.htmlShimManager = new HtmlShimManager();
				}
				return this.htmlShimManager;
			}
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x0016512E File Offset: 0x0016412E
		private void CheckIfCreatedInIE()
		{
			if (!WebBrowser.createdInIE)
			{
				return;
			}
			if (this.ParentInternal != null)
			{
				this.ParentInternal.Controls.Remove(this);
				base.Dispose();
				return;
			}
			base.Dispose();
			throw new NotSupportedException(SR.GetString("WebBrowserInIENotSupported"));
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x00165170 File Offset: 0x00164170
		internal static void EnsureUrlConnectPermission(Uri url)
		{
			WebPermission webPermission = new WebPermission(NetworkAccess.Connect, url.ToString());
			webPermission.Demand();
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x00165191 File Offset: 0x00164191
		private string ReadyNavigateToUrl(string urlString)
		{
			if (string.IsNullOrEmpty(urlString))
			{
				urlString = "about:blank";
			}
			if (!this.webBrowserState[2])
			{
				this.documentStreamToSetOnLoad = null;
			}
			return urlString;
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x001651B8 File Offset: 0x001641B8
		private string ReadyNavigateToUrl(Uri url)
		{
			string urlString;
			if (url == null)
			{
				urlString = "about:blank";
			}
			else
			{
				if (!url.IsAbsoluteUri)
				{
					throw new ArgumentException(SR.GetString("WebBrowserNavigateAbsoluteUri", new object[]
					{
						"uri"
					}));
				}
				urlString = url.ToString();
			}
			return this.ReadyNavigateToUrl(urlString);
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x0016520C File Offset: 0x0016420C
		private void PerformNavigateHelper(string urlString, bool newWindow, string targetFrameName, byte[] postData, string headers)
		{
			object obj = urlString;
			object obj2 = newWindow ? 1 : 0;
			object obj3 = targetFrameName;
			object obj4 = postData;
			object obj5 = headers;
			this.PerformNavigate2(ref obj, ref obj2, ref obj3, ref obj4, ref obj5);
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x00165244 File Offset: 0x00164244
		private void PerformNavigate2(ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers)
		{
			try
			{
				this.AxIWebBrowser2.Navigate2(ref URL, ref flags, ref targetFrameName, ref postData, ref headers);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147023673)
				{
					throw;
				}
			}
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x00165288 File Offset: 0x00164288
		private bool ShouldSerializeDocumentText()
		{
			return this.IsValidUrl;
		}

		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x060061F6 RID: 25078 RVA: 0x00165290 File Offset: 0x00164290
		private bool IsValidUrl
		{
			get
			{
				return this.Url == null || this.Url.AbsoluteUri == "about:blank";
			}
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x001652B7 File Offset: 0x001642B7
		private bool ShouldSerializeUrl()
		{
			return !this.ShouldSerializeDocumentText();
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x001652C4 File Offset: 0x001642C4
		private bool ShowContextMenu(int x, int y)
		{
			ContextMenuStrip contextMenuStrip = this.ContextMenuStrip;
			ContextMenu contextMenu = (contextMenuStrip != null) ? null : this.ContextMenu;
			if (contextMenuStrip == null && contextMenu == null)
			{
				return false;
			}
			bool isKeyboardActivated = false;
			Point point;
			if (x == -1)
			{
				isKeyboardActivated = true;
				point = new Point(base.Width / 2, base.Height / 2);
			}
			else
			{
				point = base.PointToClientInternal(new Point(x, y));
			}
			if (base.ClientRectangle.Contains(point))
			{
				if (contextMenuStrip != null)
				{
					contextMenuStrip.ShowInternal(this, point, isKeyboardActivated);
				}
				else if (contextMenu != null)
				{
					contextMenu.Show(this, point);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x0016534C File Offset: 0x0016434C
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 123)
			{
				int x = NativeMethods.Util.SignedLOWORD(m.LParam);
				int y = NativeMethods.Util.SignedHIWORD(m.LParam);
				if (!this.ShowContextMenu(x, y))
				{
					this.DefWndProc(ref m);
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x060061FA RID: 25082 RVA: 0x00165398 File Offset: 0x00164398
		private UnsafeNativeMethods.IWebBrowser2 AxIWebBrowser2
		{
			get
			{
				if (this.axIWebBrowser2 == null)
				{
					if (base.IsDisposed)
					{
						throw new ObjectDisposedException(base.GetType().Name);
					}
					base.TransitionUpTo(WebBrowserHelper.AXState.InPlaceActive);
				}
				if (this.axIWebBrowser2 == null)
				{
					throw new InvalidOperationException(SR.GetString("WebBrowserNoCastToIWebBrowser2"));
				}
				return this.axIWebBrowser2;
			}
		}

		// Token: 0x04003AC1 RID: 15041
		private const int WEBBROWSERSTATE_webBrowserShortcutsEnabled = 1;

		// Token: 0x04003AC2 RID: 15042
		private const int WEBBROWSERSTATE_documentStreamJustSet = 2;

		// Token: 0x04003AC3 RID: 15043
		private const int WEBBROWSERSTATE_isWebBrowserContextMenuEnabled = 4;

		// Token: 0x04003AC4 RID: 15044
		private const int WEBBROWSERSTATE_canGoBack = 8;

		// Token: 0x04003AC5 RID: 15045
		private const int WEBBROWSERSTATE_canGoForward = 16;

		// Token: 0x04003AC6 RID: 15046
		private const int WEBBROWSERSTATE_scrollbarsEnabled = 32;

		// Token: 0x04003AC7 RID: 15047
		private const int WEBBROWSERSTATE_allowNavigation = 64;

		// Token: 0x04003AC8 RID: 15048
		private static bool createdInIE;

		// Token: 0x04003AC9 RID: 15049
		private UnsafeNativeMethods.IWebBrowser2 axIWebBrowser2;

		// Token: 0x04003ACA RID: 15050
		private AxHost.ConnectionPointCookie cookie;

		// Token: 0x04003ACB RID: 15051
		private Stream documentStreamToSetOnLoad;

		// Token: 0x04003ACC RID: 15052
		private WebBrowserEncryptionLevel encryptionLevel;

		// Token: 0x04003ACD RID: 15053
		private object objectForScripting;

		// Token: 0x04003ACE RID: 15054
		private WebBrowser.WebBrowserEvent webBrowserEvent;

		// Token: 0x04003ACF RID: 15055
		internal string statusText = "";

		// Token: 0x04003AD0 RID: 15056
		private BitVector32 webBrowserState;

		// Token: 0x04003ADC RID: 15068
		private HtmlShimManager htmlShimManager;

		// Token: 0x02000728 RID: 1832
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected class WebBrowserSite : WebBrowserSiteBase, UnsafeNativeMethods.IDocHostUIHandler
		{
			// Token: 0x06006221 RID: 25121 RVA: 0x00165AB7 File Offset: 0x00164AB7
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public WebBrowserSite(WebBrowser host) : base(host)
			{
			}

			// Token: 0x06006222 RID: 25122 RVA: 0x00165AC0 File Offset: 0x00164AC0
			int UnsafeNativeMethods.IDocHostUIHandler.ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				if (webBrowser.IsWebBrowserContextMenuEnabled)
				{
					return 1;
				}
				if (pt.x == 0 && pt.y == 0)
				{
					pt.x = -1;
					pt.y = -1;
				}
				webBrowser.ShowContextMenu(pt.x, pt.y);
				return 0;
			}

			// Token: 0x06006223 RID: 25123 RVA: 0x00165B18 File Offset: 0x00164B18
			int UnsafeNativeMethods.IDocHostUIHandler.GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				info.dwDoubleClick = 0;
				info.dwFlags = 2097168;
				if (webBrowser.ScrollBarsEnabled)
				{
					info.dwFlags |= 128;
				}
				else
				{
					info.dwFlags |= 8;
				}
				if (Application.RenderWithVisualStyles)
				{
					info.dwFlags |= 262144;
				}
				else
				{
					info.dwFlags |= 524288;
				}
				return 0;
			}

			// Token: 0x06006224 RID: 25124 RVA: 0x00165B9B File Offset: 0x00164B9B
			int UnsafeNativeMethods.IDocHostUIHandler.EnableModeless(bool fEnable)
			{
				return -2147467263;
			}

			// Token: 0x06006225 RID: 25125 RVA: 0x00165BA2 File Offset: 0x00164BA2
			int UnsafeNativeMethods.IDocHostUIHandler.ShowUI(int dwID, UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, UnsafeNativeMethods.IOleInPlaceFrame frame, UnsafeNativeMethods.IOleInPlaceUIWindow doc)
			{
				return 1;
			}

			// Token: 0x06006226 RID: 25126 RVA: 0x00165BA5 File Offset: 0x00164BA5
			int UnsafeNativeMethods.IDocHostUIHandler.HideUI()
			{
				return -2147467263;
			}

			// Token: 0x06006227 RID: 25127 RVA: 0x00165BAC File Offset: 0x00164BAC
			int UnsafeNativeMethods.IDocHostUIHandler.UpdateUI()
			{
				return -2147467263;
			}

			// Token: 0x06006228 RID: 25128 RVA: 0x00165BB3 File Offset: 0x00164BB3
			int UnsafeNativeMethods.IDocHostUIHandler.OnDocWindowActivate(bool fActivate)
			{
				return -2147467263;
			}

			// Token: 0x06006229 RID: 25129 RVA: 0x00165BBA File Offset: 0x00164BBA
			int UnsafeNativeMethods.IDocHostUIHandler.OnFrameWindowActivate(bool fActivate)
			{
				return -2147467263;
			}

			// Token: 0x0600622A RID: 25130 RVA: 0x00165BC1 File Offset: 0x00164BC1
			int UnsafeNativeMethods.IDocHostUIHandler.ResizeBorder(NativeMethods.COMRECT rect, UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
			{
				return -2147467263;
			}

			// Token: 0x0600622B RID: 25131 RVA: 0x00165BC8 File Offset: 0x00164BC8
			int UnsafeNativeMethods.IDocHostUIHandler.GetOptionKeyPath(string[] pbstrKey, int dw)
			{
				return -2147467263;
			}

			// Token: 0x0600622C RID: 25132 RVA: 0x00165BCF File Offset: 0x00164BCF
			int UnsafeNativeMethods.IDocHostUIHandler.GetDropTarget(UnsafeNativeMethods.IOleDropTarget pDropTarget, out UnsafeNativeMethods.IOleDropTarget ppDropTarget)
			{
				ppDropTarget = null;
				return -2147467263;
			}

			// Token: 0x0600622D RID: 25133 RVA: 0x00165BDC File Offset: 0x00164BDC
			int UnsafeNativeMethods.IDocHostUIHandler.GetExternal(out object ppDispatch)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				ppDispatch = webBrowser.ObjectForScripting;
				return 0;
			}

			// Token: 0x0600622E RID: 25134 RVA: 0x00165C00 File Offset: 0x00164C00
			int UnsafeNativeMethods.IDocHostUIHandler.TranslateAccelerator(ref NativeMethods.MSG msg, ref Guid group, int nCmdID)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				if (webBrowser.WebBrowserShortcutsEnabled)
				{
					return 1;
				}
				int num = (int)msg.wParam | (int)Control.ModifierKeys;
				if (msg.message != 258 && Enum.IsDefined(typeof(Shortcut), (Shortcut)num))
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x0600622F RID: 25135 RVA: 0x00165C5C File Offset: 0x00164C5C
			int UnsafeNativeMethods.IDocHostUIHandler.TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
			{
				pstrUrlOut = null;
				return 1;
			}

			// Token: 0x06006230 RID: 25136 RVA: 0x00165C62 File Offset: 0x00164C62
			int UnsafeNativeMethods.IDocHostUIHandler.FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
			{
				ppDORet = null;
				return 1;
			}

			// Token: 0x06006231 RID: 25137 RVA: 0x00165C68 File Offset: 0x00164C68
			internal override void OnPropertyChanged(int dispid)
			{
				if (dispid != -525)
				{
					base.OnPropertyChanged(dispid);
				}
			}
		}

		// Token: 0x02000729 RID: 1833
		[ClassInterface(ClassInterfaceType.None)]
		private class WebBrowserEvent : StandardOleMarshalObject, UnsafeNativeMethods.DWebBrowserEvents2
		{
			// Token: 0x06006232 RID: 25138 RVA: 0x00165C79 File Offset: 0x00164C79
			public WebBrowserEvent(WebBrowser parent)
			{
				this.parent = parent;
			}

			// Token: 0x170014B3 RID: 5299
			// (get) Token: 0x06006233 RID: 25139 RVA: 0x00165C88 File Offset: 0x00164C88
			// (set) Token: 0x06006234 RID: 25140 RVA: 0x00165C90 File Offset: 0x00164C90
			public bool AllowNavigation
			{
				get
				{
					return this.allowNavigation;
				}
				set
				{
					this.allowNavigation = value;
				}
			}

			// Token: 0x06006235 RID: 25141 RVA: 0x00165C99 File Offset: 0x00164C99
			public void CommandStateChange(long command, bool enable)
			{
				if (command == 2L)
				{
					this.parent.CanGoBackInternal = enable;
					return;
				}
				if (command == 1L)
				{
					this.parent.CanGoForwardInternal = enable;
				}
			}

			// Token: 0x06006236 RID: 25142 RVA: 0x00165CC0 File Offset: 0x00164CC0
			public void BeforeNavigate2(object pDisp, ref object urlObject, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
			{
				if (this.AllowNavigation || !this.haveNavigated)
				{
					if (targetFrameName == null)
					{
						targetFrameName = "";
					}
					if (headers == null)
					{
						headers = "";
					}
					string uriString = (urlObject == null) ? "" : ((string)urlObject);
					WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs = new WebBrowserNavigatingEventArgs(new Uri(uriString), (targetFrameName == null) ? "" : ((string)targetFrameName));
					this.parent.OnNavigating(webBrowserNavigatingEventArgs);
					cancel = webBrowserNavigatingEventArgs.Cancel;
					return;
				}
				cancel = true;
			}

			// Token: 0x06006237 RID: 25143 RVA: 0x00165D44 File Offset: 0x00164D44
			public void DocumentComplete(object pDisp, ref object urlObject)
			{
				this.haveNavigated = true;
				if (this.parent.documentStreamToSetOnLoad != null && (string)urlObject == "about:blank")
				{
					HtmlDocument document = this.parent.Document;
					if (document != null)
					{
						UnsafeNativeMethods.IPersistStreamInit persistStreamInit = document.DomDocument as UnsafeNativeMethods.IPersistStreamInit;
						UnsafeNativeMethods.IStream pstm = new UnsafeNativeMethods.ComStreamFromDataStream(this.parent.documentStreamToSetOnLoad);
						persistStreamInit.Load(pstm);
						document.Encoding = "unicode";
					}
					this.parent.documentStreamToSetOnLoad = null;
					return;
				}
				string uriString = (urlObject == null) ? "" : urlObject.ToString();
				WebBrowserDocumentCompletedEventArgs e = new WebBrowserDocumentCompletedEventArgs(new Uri(uriString));
				this.parent.OnDocumentCompleted(e);
			}

			// Token: 0x06006238 RID: 25144 RVA: 0x00165DF6 File Offset: 0x00164DF6
			public void TitleChange(string text)
			{
				this.parent.OnDocumentTitleChanged(EventArgs.Empty);
			}

			// Token: 0x06006239 RID: 25145 RVA: 0x00165E08 File Offset: 0x00164E08
			public void SetSecureLockIcon(int secureLockIcon)
			{
				this.parent.encryptionLevel = (WebBrowserEncryptionLevel)secureLockIcon;
				this.parent.OnEncryptionLevelChanged(EventArgs.Empty);
			}

			// Token: 0x0600623A RID: 25146 RVA: 0x00165E28 File Offset: 0x00164E28
			public void NavigateComplete2(object pDisp, ref object urlObject)
			{
				string uriString = (urlObject == null) ? "" : ((string)urlObject);
				WebBrowserNavigatedEventArgs e = new WebBrowserNavigatedEventArgs(new Uri(uriString));
				this.parent.OnNavigated(e);
			}

			// Token: 0x0600623B RID: 25147 RVA: 0x00165E60 File Offset: 0x00164E60
			public void NewWindow2(ref object ppDisp, ref bool cancel)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				this.parent.OnNewWindow(cancelEventArgs);
				cancel = cancelEventArgs.Cancel;
			}

			// Token: 0x0600623C RID: 25148 RVA: 0x00165E88 File Offset: 0x00164E88
			public void ProgressChange(int progress, int progressMax)
			{
				WebBrowserProgressChangedEventArgs e = new WebBrowserProgressChangedEventArgs((long)progress, (long)progressMax);
				this.parent.OnProgressChanged(e);
			}

			// Token: 0x0600623D RID: 25149 RVA: 0x00165EAB File Offset: 0x00164EAB
			public void StatusTextChange(string text)
			{
				this.parent.statusText = ((text == null) ? "" : text);
				this.parent.OnStatusTextChanged(EventArgs.Empty);
			}

			// Token: 0x0600623E RID: 25150 RVA: 0x00165ED3 File Offset: 0x00164ED3
			public void DownloadBegin()
			{
				this.parent.OnFileDownload(EventArgs.Empty);
			}

			// Token: 0x0600623F RID: 25151 RVA: 0x00165EE5 File Offset: 0x00164EE5
			public void FileDownload(ref bool cancel)
			{
			}

			// Token: 0x06006240 RID: 25152 RVA: 0x00165EE7 File Offset: 0x00164EE7
			public void PrivacyImpactedStateChange(bool bImpacted)
			{
			}

			// Token: 0x06006241 RID: 25153 RVA: 0x00165EE9 File Offset: 0x00164EE9
			public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
			{
			}

			// Token: 0x06006242 RID: 25154 RVA: 0x00165EEB File Offset: 0x00164EEB
			public void PrintTemplateTeardown(object pDisp)
			{
			}

			// Token: 0x06006243 RID: 25155 RVA: 0x00165EED File Offset: 0x00164EED
			public void PrintTemplateInstantiation(object pDisp)
			{
			}

			// Token: 0x06006244 RID: 25156 RVA: 0x00165EEF File Offset: 0x00164EEF
			public void NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
			{
			}

			// Token: 0x06006245 RID: 25157 RVA: 0x00165EF1 File Offset: 0x00164EF1
			public void ClientToHostWindow(ref long cX, ref long cY)
			{
			}

			// Token: 0x06006246 RID: 25158 RVA: 0x00165EF3 File Offset: 0x00164EF3
			public void WindowClosing(bool isChildWindow, ref bool cancel)
			{
			}

			// Token: 0x06006247 RID: 25159 RVA: 0x00165EF5 File Offset: 0x00164EF5
			public void WindowSetHeight(int height)
			{
			}

			// Token: 0x06006248 RID: 25160 RVA: 0x00165EF7 File Offset: 0x00164EF7
			public void WindowSetWidth(int width)
			{
			}

			// Token: 0x06006249 RID: 25161 RVA: 0x00165EF9 File Offset: 0x00164EF9
			public void WindowSetTop(int top)
			{
			}

			// Token: 0x0600624A RID: 25162 RVA: 0x00165EFB File Offset: 0x00164EFB
			public void WindowSetLeft(int left)
			{
			}

			// Token: 0x0600624B RID: 25163 RVA: 0x00165EFD File Offset: 0x00164EFD
			public void WindowSetResizable(bool resizable)
			{
			}

			// Token: 0x0600624C RID: 25164 RVA: 0x00165EFF File Offset: 0x00164EFF
			public void OnTheaterMode(bool theaterMode)
			{
			}

			// Token: 0x0600624D RID: 25165 RVA: 0x00165F01 File Offset: 0x00164F01
			public void OnFullScreen(bool fullScreen)
			{
			}

			// Token: 0x0600624E RID: 25166 RVA: 0x00165F03 File Offset: 0x00164F03
			public void OnStatusBar(bool statusBar)
			{
			}

			// Token: 0x0600624F RID: 25167 RVA: 0x00165F05 File Offset: 0x00164F05
			public void OnMenuBar(bool menuBar)
			{
			}

			// Token: 0x06006250 RID: 25168 RVA: 0x00165F07 File Offset: 0x00164F07
			public void OnToolBar(bool toolBar)
			{
			}

			// Token: 0x06006251 RID: 25169 RVA: 0x00165F09 File Offset: 0x00164F09
			public void OnVisible(bool visible)
			{
			}

			// Token: 0x06006252 RID: 25170 RVA: 0x00165F0B File Offset: 0x00164F0B
			public void OnQuit()
			{
			}

			// Token: 0x06006253 RID: 25171 RVA: 0x00165F0D File Offset: 0x00164F0D
			public void PropertyChange(string szProperty)
			{
			}

			// Token: 0x06006254 RID: 25172 RVA: 0x00165F0F File Offset: 0x00164F0F
			public void DownloadComplete()
			{
			}

			// Token: 0x04003ADF RID: 15071
			private WebBrowser parent;

			// Token: 0x04003AE0 RID: 15072
			private bool allowNavigation;

			// Token: 0x04003AE1 RID: 15073
			private bool haveNavigated;
		}
	}
}
