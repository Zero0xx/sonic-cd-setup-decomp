using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x020007A1 RID: 1953
	[SRDescription("DescriptionPrintDialog")]
	[DefaultProperty("Document")]
	[Designer("System.Windows.Forms.Design.PrintDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class PrintDialog : CommonDialog
	{
		// Token: 0x06006649 RID: 26185 RVA: 0x0017767E File Offset: 0x0017667E
		public PrintDialog()
		{
			this.Reset();
		}

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x0600664A RID: 26186 RVA: 0x0017768C File Offset: 0x0017668C
		// (set) Token: 0x0600664B RID: 26187 RVA: 0x00177694 File Offset: 0x00176694
		[DefaultValue(false)]
		[SRDescription("PDallowCurrentPageDescr")]
		public bool AllowCurrentPage
		{
			get
			{
				return this.allowCurrentPage;
			}
			set
			{
				this.allowCurrentPage = value;
			}
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x0600664C RID: 26188 RVA: 0x0017769D File Offset: 0x0017669D
		// (set) Token: 0x0600664D RID: 26189 RVA: 0x001776A5 File Offset: 0x001766A5
		[SRDescription("PDallowPagesDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool AllowSomePages
		{
			get
			{
				return this.allowPages;
			}
			set
			{
				this.allowPages = value;
			}
		}

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x0600664E RID: 26190 RVA: 0x001776AE File Offset: 0x001766AE
		// (set) Token: 0x0600664F RID: 26191 RVA: 0x001776B6 File Offset: 0x001766B6
		[SRDescription("PDallowPrintToFileDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowPrintToFile
		{
			get
			{
				return this.allowPrintToFile;
			}
			set
			{
				this.allowPrintToFile = value;
			}
		}

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06006650 RID: 26192 RVA: 0x001776BF File Offset: 0x001766BF
		// (set) Token: 0x06006651 RID: 26193 RVA: 0x001776C7 File Offset: 0x001766C7
		[SRCategory("CatBehavior")]
		[SRDescription("PDallowSelectionDescr")]
		[DefaultValue(false)]
		public bool AllowSelection
		{
			get
			{
				return this.allowSelection;
			}
			set
			{
				this.allowSelection = value;
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06006652 RID: 26194 RVA: 0x001776D0 File Offset: 0x001766D0
		// (set) Token: 0x06006653 RID: 26195 RVA: 0x001776D8 File Offset: 0x001766D8
		[SRDescription("PDdocumentDescr")]
		[SRCategory("CatData")]
		[DefaultValue(null)]
		public PrintDocument Document
		{
			get
			{
				return this.printDocument;
			}
			set
			{
				this.printDocument = value;
				if (this.printDocument == null)
				{
					this.settings = new PrinterSettings();
					return;
				}
				this.settings = this.printDocument.PrinterSettings;
			}
		}

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06006654 RID: 26196 RVA: 0x00177706 File Offset: 0x00176706
		private PageSettings PageSettings
		{
			get
			{
				if (this.Document == null)
				{
					return this.PrinterSettings.DefaultPageSettings;
				}
				return this.Document.DefaultPageSettings;
			}
		}

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06006655 RID: 26197 RVA: 0x00177727 File Offset: 0x00176727
		// (set) Token: 0x06006656 RID: 26198 RVA: 0x00177742 File Offset: 0x00176742
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("PDprinterSettingsDescr")]
		[DefaultValue(null)]
		[SRCategory("CatData")]
		public PrinterSettings PrinterSettings
		{
			get
			{
				if (this.settings == null)
				{
					this.settings = new PrinterSettings();
				}
				return this.settings;
			}
			set
			{
				if (value != this.PrinterSettings)
				{
					this.settings = value;
					this.printDocument = null;
				}
			}
		}

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x06006657 RID: 26199 RVA: 0x0017775B File Offset: 0x0017675B
		// (set) Token: 0x06006658 RID: 26200 RVA: 0x00177763 File Offset: 0x00176763
		[SRDescription("PDprintToFileDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool PrintToFile
		{
			get
			{
				return this.printToFile;
			}
			set
			{
				this.printToFile = value;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x06006659 RID: 26201 RVA: 0x0017776C File Offset: 0x0017676C
		// (set) Token: 0x0600665A RID: 26202 RVA: 0x00177774 File Offset: 0x00176774
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PDshowHelpDescr")]
		public bool ShowHelp
		{
			get
			{
				return this.showHelp;
			}
			set
			{
				this.showHelp = value;
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x0017777D File Offset: 0x0017677D
		// (set) Token: 0x0600665C RID: 26204 RVA: 0x00177785 File Offset: 0x00176785
		[DefaultValue(true)]
		[SRDescription("PDshowNetworkDescr")]
		[SRCategory("CatBehavior")]
		public bool ShowNetwork
		{
			get
			{
				return this.showNetwork;
			}
			set
			{
				this.showNetwork = value;
			}
		}

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x0600665D RID: 26205 RVA: 0x0017778E File Offset: 0x0017678E
		// (set) Token: 0x0600665E RID: 26206 RVA: 0x00177796 File Offset: 0x00176796
		[SRDescription("PDuseEXDialog")]
		[DefaultValue(false)]
		public bool UseEXDialog
		{
			get
			{
				return this.useEXDialog;
			}
			set
			{
				this.useEXDialog = value;
			}
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x001777A0 File Offset: 0x001767A0
		private int GetFlags()
		{
			int num = 0;
			if (!this.UseEXDialog || Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				num |= 4096;
			}
			if (!this.allowCurrentPage)
			{
				num |= 8388608;
			}
			if (!this.allowPages)
			{
				num |= 8;
			}
			if (!this.allowPrintToFile)
			{
				num |= 524288;
			}
			if (!this.allowSelection)
			{
				num |= 4;
			}
			num |= (int)this.PrinterSettings.PrintRange;
			if (this.printToFile)
			{
				num |= 32;
			}
			if (this.showHelp)
			{
				num |= 2048;
			}
			if (!this.showNetwork)
			{
				num |= 2097152;
			}
			if (this.PrinterSettings.Collate)
			{
				num |= 16;
			}
			return num;
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x00177864 File Offset: 0x00176864
		public override void Reset()
		{
			this.allowCurrentPage = false;
			this.allowPages = false;
			this.allowPrintToFile = true;
			this.allowSelection = false;
			this.printDocument = null;
			this.printToFile = false;
			this.settings = null;
			this.showHelp = false;
			this.showNetwork = true;
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x001778B0 File Offset: 0x001768B0
		internal static NativeMethods.PRINTDLG CreatePRINTDLG()
		{
			NativeMethods.PRINTDLG printdlg = new NativeMethods.PRINTDLG();
			printdlg.lStructSize = Marshal.SizeOf(printdlg);
			printdlg.hwndOwner = IntPtr.Zero;
			printdlg.hDevMode = IntPtr.Zero;
			printdlg.hDevNames = IntPtr.Zero;
			printdlg.Flags = 0;
			printdlg.hDC = IntPtr.Zero;
			printdlg.nFromPage = 1;
			printdlg.nToPage = 1;
			printdlg.nMinPage = 0;
			printdlg.nMaxPage = 9999;
			printdlg.nCopies = 1;
			printdlg.hInstance = IntPtr.Zero;
			printdlg.lCustData = IntPtr.Zero;
			printdlg.lpfnPrintHook = null;
			printdlg.lpfnSetupHook = null;
			printdlg.lpPrintTemplateName = null;
			printdlg.lpSetupTemplateName = null;
			printdlg.hPrintTemplate = IntPtr.Zero;
			printdlg.hSetupTemplate = IntPtr.Zero;
			return printdlg;
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x00177974 File Offset: 0x00176974
		internal static NativeMethods.PRINTDLGEX CreatePRINTDLGEX()
		{
			NativeMethods.PRINTDLGEX printdlgex = new NativeMethods.PRINTDLGEX();
			printdlgex.lStructSize = Marshal.SizeOf(printdlgex);
			printdlgex.hwndOwner = IntPtr.Zero;
			printdlgex.hDevMode = IntPtr.Zero;
			printdlgex.hDevNames = IntPtr.Zero;
			printdlgex.hDC = IntPtr.Zero;
			printdlgex.Flags = 0;
			printdlgex.Flags2 = 0;
			printdlgex.ExclusionFlags = 0;
			printdlgex.nPageRanges = 0;
			printdlgex.nMaxPageRanges = 1;
			printdlgex.pageRanges = UnsafeNativeMethods.GlobalAlloc(64, printdlgex.nMaxPageRanges * Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
			printdlgex.nMinPage = 0;
			printdlgex.nMaxPage = 9999;
			printdlgex.nCopies = 1;
			printdlgex.hInstance = IntPtr.Zero;
			printdlgex.lpPrintTemplateName = null;
			printdlgex.nPropertyPages = 0;
			printdlgex.lphPropertyPages = IntPtr.Zero;
			printdlgex.nStartPage = NativeMethods.START_PAGE_GENERAL;
			printdlgex.dwResultAction = 0;
			return printdlgex;
		}

		// Token: 0x06006663 RID: 26211 RVA: 0x00177A58 File Offset: 0x00176A58
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			IntSecurity.SafePrinting.Demand();
			NativeMethods.WndProc hookProcPtr = new NativeMethods.WndProc(this.HookProc);
			bool result;
			if (!this.UseEXDialog || Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				NativeMethods.PRINTDLG data = PrintDialog.CreatePRINTDLG();
				result = this.ShowPrintDialog(hwndOwner, hookProcPtr, data);
			}
			else
			{
				NativeMethods.PRINTDLGEX data2 = PrintDialog.CreatePRINTDLGEX();
				result = this.ShowPrintDialog(hwndOwner, data2);
			}
			return result;
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x00177AC8 File Offset: 0x00176AC8
		private bool ShowPrintDialog(IntPtr hwndOwner, NativeMethods.WndProc hookProcPtr, NativeMethods.PRINTDLG data)
		{
			data.Flags = this.GetFlags();
			data.nCopies = this.PrinterSettings.Copies;
			data.hwndOwner = hwndOwner;
			data.lpfnPrintHook = hookProcPtr;
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				if (this.PageSettings == null)
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode();
				}
				else
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode(this.PageSettings);
				}
				data.hDevNames = this.PrinterSettings.GetHdevnames();
			}
			catch (InvalidPrinterException)
			{
				data.hDevMode = IntPtr.Zero;
				data.hDevNames = IntPtr.Zero;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool result;
			try
			{
				if (this.AllowSomePages)
				{
					if (this.PrinterSettings.FromPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.FromPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.ToPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"ToPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.FromPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					data.nFromPage = (short)this.PrinterSettings.FromPage;
					data.nToPage = (short)this.PrinterSettings.ToPage;
					data.nMinPage = (short)this.PrinterSettings.MinimumPage;
					data.nMaxPage = (short)this.PrinterSettings.MaximumPage;
				}
				if (!UnsafeNativeMethods.PrintDlg(data))
				{
					result = false;
				}
				else
				{
					IntSecurity.AllPrintingAndUnmanagedCode.Assert();
					try
					{
						PrintDialog.UpdatePrinterSettings(data.hDevMode, data.hDevNames, data.nCopies, data.Flags, this.settings, this.PageSettings);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.PrintToFile = ((data.Flags & 32) != 0);
					this.PrinterSettings.PrintToFile = this.PrintToFile;
					if (this.AllowSomePages)
					{
						this.PrinterSettings.FromPage = (int)data.nFromPage;
						this.PrinterSettings.ToPage = (int)data.nToPage;
					}
					if ((data.Flags & 262144) == 0 && Environment.OSVersion.Version.Major >= 6)
					{
						this.PrinterSettings.Copies = data.nCopies;
						this.PrinterSettings.Collate = ((data.Flags & 16) == 16);
					}
					result = true;
				}
			}
			finally
			{
				UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevMode));
				UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevNames));
			}
			return result;
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x00177E0C File Offset: 0x00176E0C
		private unsafe bool ShowPrintDialog(IntPtr hwndOwner, NativeMethods.PRINTDLGEX data)
		{
			data.Flags = this.GetFlags();
			data.nCopies = (int)this.PrinterSettings.Copies;
			data.hwndOwner = hwndOwner;
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				if (this.PageSettings == null)
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode();
				}
				else
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode(this.PageSettings);
				}
				data.hDevNames = this.PrinterSettings.GetHdevnames();
			}
			catch (InvalidPrinterException)
			{
				data.hDevMode = IntPtr.Zero;
				data.hDevNames = IntPtr.Zero;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool result;
			try
			{
				if (this.AllowSomePages)
				{
					if (this.PrinterSettings.FromPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.FromPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.ToPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"ToPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.FromPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					int* ptr = (int*)((void*)data.pageRanges);
					*ptr = this.PrinterSettings.FromPage;
					ptr++;
					*ptr = this.PrinterSettings.ToPage;
					data.nPageRanges = 1;
					data.nMinPage = this.PrinterSettings.MinimumPage;
					data.nMaxPage = this.PrinterSettings.MaximumPage;
				}
				data.Flags &= -2099201;
				int hr = UnsafeNativeMethods.PrintDlgEx(data);
				if (NativeMethods.Failed(hr) || data.dwResultAction == 0)
				{
					result = false;
				}
				else
				{
					IntSecurity.AllPrintingAndUnmanagedCode.Assert();
					try
					{
						PrintDialog.UpdatePrinterSettings(data.hDevMode, data.hDevNames, (short)data.nCopies, data.Flags, this.PrinterSettings, this.PageSettings);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.PrintToFile = ((data.Flags & 32) != 0);
					this.PrinterSettings.PrintToFile = this.PrintToFile;
					if (this.AllowSomePages)
					{
						int* ptr2 = (int*)((void*)data.pageRanges);
						this.PrinterSettings.FromPage = *ptr2;
						ptr2++;
						this.PrinterSettings.ToPage = *ptr2;
					}
					if ((data.Flags & 262144) == 0 && Environment.OSVersion.Version.Major >= 6)
					{
						this.PrinterSettings.Copies = (short)data.nCopies;
						this.PrinterSettings.Collate = ((data.Flags & 16) == 16);
					}
					result = (data.dwResultAction == 1);
				}
			}
			finally
			{
				if (data.hDevMode != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevMode));
				}
				if (data.hDevNames != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevNames));
				}
				if (data.pageRanges != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.pageRanges));
				}
			}
			return result;
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x001781DC File Offset: 0x001771DC
		private static void UpdatePrinterSettings(IntPtr hDevMode, IntPtr hDevNames, short copies, int flags, PrinterSettings settings, PageSettings pageSettings)
		{
			settings.SetHdevmode(hDevMode);
			settings.SetHdevnames(hDevNames);
			if (pageSettings != null)
			{
				pageSettings.SetHdevmode(hDevMode);
			}
			if (settings.Copies == 1)
			{
				settings.Copies = copies;
			}
			settings.PrintRange = (PrintRange)(flags & 4194307);
		}

		// Token: 0x04003CDA RID: 15578
		private const int printRangeMask = 4194307;

		// Token: 0x04003CDB RID: 15579
		private PrinterSettings settings;

		// Token: 0x04003CDC RID: 15580
		private PrintDocument printDocument;

		// Token: 0x04003CDD RID: 15581
		private bool allowCurrentPage;

		// Token: 0x04003CDE RID: 15582
		private bool allowPages;

		// Token: 0x04003CDF RID: 15583
		private bool allowPrintToFile;

		// Token: 0x04003CE0 RID: 15584
		private bool allowSelection;

		// Token: 0x04003CE1 RID: 15585
		private bool printToFile;

		// Token: 0x04003CE2 RID: 15586
		private bool showHelp;

		// Token: 0x04003CE3 RID: 15587
		private bool showNetwork;

		// Token: 0x04003CE4 RID: 15588
		private bool useEXDialog;
	}
}
