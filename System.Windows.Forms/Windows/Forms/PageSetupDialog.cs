using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200079D RID: 1949
	[DefaultProperty("Document")]
	[SRDescription("DescriptionPageSetupDialog")]
	public sealed class PageSetupDialog : CommonDialog
	{
		// Token: 0x0600661C RID: 26140 RVA: 0x00176B8E File Offset: 0x00175B8E
		public PageSetupDialog()
		{
			this.Reset();
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x0600661D RID: 26141 RVA: 0x00176B9C File Offset: 0x00175B9C
		// (set) Token: 0x0600661E RID: 26142 RVA: 0x00176BA4 File Offset: 0x00175BA4
		[SRDescription("PSDallowMarginsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowMargins
		{
			get
			{
				return this.allowMargins;
			}
			set
			{
				this.allowMargins = value;
			}
		}

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x0600661F RID: 26143 RVA: 0x00176BAD File Offset: 0x00175BAD
		// (set) Token: 0x06006620 RID: 26144 RVA: 0x00176BB5 File Offset: 0x00175BB5
		[SRDescription("PSDallowOrientationDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool AllowOrientation
		{
			get
			{
				return this.allowOrientation;
			}
			set
			{
				this.allowOrientation = value;
			}
		}

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06006621 RID: 26145 RVA: 0x00176BBE File Offset: 0x00175BBE
		// (set) Token: 0x06006622 RID: 26146 RVA: 0x00176BC6 File Offset: 0x00175BC6
		[SRCategory("CatBehavior")]
		[SRDescription("PSDallowPaperDescr")]
		[DefaultValue(true)]
		public bool AllowPaper
		{
			get
			{
				return this.allowPaper;
			}
			set
			{
				this.allowPaper = value;
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06006623 RID: 26147 RVA: 0x00176BCF File Offset: 0x00175BCF
		// (set) Token: 0x06006624 RID: 26148 RVA: 0x00176BD7 File Offset: 0x00175BD7
		[SRDescription("PSDallowPrinterDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowPrinter
		{
			get
			{
				return this.allowPrinter;
			}
			set
			{
				this.allowPrinter = value;
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06006625 RID: 26149 RVA: 0x00176BE0 File Offset: 0x00175BE0
		// (set) Token: 0x06006626 RID: 26150 RVA: 0x00176BE8 File Offset: 0x00175BE8
		[DefaultValue(null)]
		[SRDescription("PDdocumentDescr")]
		[SRCategory("CatData")]
		public PrintDocument Document
		{
			get
			{
				return this.printDocument;
			}
			set
			{
				this.printDocument = value;
				if (this.printDocument != null)
				{
					this.pageSettings = this.printDocument.DefaultPageSettings;
					this.printerSettings = this.printDocument.PrinterSettings;
				}
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x06006627 RID: 26151 RVA: 0x00176C1B File Offset: 0x00175C1B
		// (set) Token: 0x06006628 RID: 26152 RVA: 0x00176C23 File Offset: 0x00175C23
		[SRDescription("PSDenableMetricDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(false)]
		public bool EnableMetric
		{
			get
			{
				return this.enableMetric;
			}
			set
			{
				this.enableMetric = value;
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x00176C2C File Offset: 0x00175C2C
		// (set) Token: 0x0600662A RID: 26154 RVA: 0x00176C34 File Offset: 0x00175C34
		[SRDescription("PSDminMarginsDescr")]
		[SRCategory("CatData")]
		public Margins MinMargins
		{
			get
			{
				return this.minMargins;
			}
			set
			{
				if (value == null)
				{
					value = new Margins(0, 0, 0, 0);
				}
				this.minMargins = value;
			}
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600662B RID: 26155 RVA: 0x00176C51 File Offset: 0x00175C51
		// (set) Token: 0x0600662C RID: 26156 RVA: 0x00176C59 File Offset: 0x00175C59
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PSDpageSettingsDescr")]
		[DefaultValue(null)]
		[SRCategory("CatData")]
		public PageSettings PageSettings
		{
			get
			{
				return this.pageSettings;
			}
			set
			{
				this.pageSettings = value;
				this.printDocument = null;
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600662D RID: 26157 RVA: 0x00176C69 File Offset: 0x00175C69
		// (set) Token: 0x0600662E RID: 26158 RVA: 0x00176C71 File Offset: 0x00175C71
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(null)]
		[SRDescription("PSDprinterSettingsDescr")]
		[SRCategory("CatData")]
		public PrinterSettings PrinterSettings
		{
			get
			{
				return this.printerSettings;
			}
			set
			{
				this.printerSettings = value;
				this.printDocument = null;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x0600662F RID: 26159 RVA: 0x00176C81 File Offset: 0x00175C81
		// (set) Token: 0x06006630 RID: 26160 RVA: 0x00176C89 File Offset: 0x00175C89
		[SRDescription("PSDshowHelpDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
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

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06006631 RID: 26161 RVA: 0x00176C92 File Offset: 0x00175C92
		// (set) Token: 0x06006632 RID: 26162 RVA: 0x00176C9A File Offset: 0x00175C9A
		[SRCategory("CatBehavior")]
		[SRDescription("PSDshowNetworkDescr")]
		[DefaultValue(true)]
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

		// Token: 0x06006633 RID: 26163 RVA: 0x00176CA4 File Offset: 0x00175CA4
		private int GetFlags()
		{
			int num = 0;
			num |= 8192;
			if (!this.allowMargins)
			{
				num |= 16;
			}
			if (!this.allowOrientation)
			{
				num |= 256;
			}
			if (!this.allowPaper)
			{
				num |= 512;
			}
			if (!this.allowPrinter || this.printerSettings == null)
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
			if (this.minMargins != null)
			{
				num |= 1;
			}
			if (this.pageSettings.Margins != null)
			{
				num |= 2;
			}
			return num;
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x00176D48 File Offset: 0x00175D48
		public override void Reset()
		{
			this.allowMargins = true;
			this.allowOrientation = true;
			this.allowPaper = true;
			this.allowPrinter = true;
			this.MinMargins = null;
			this.pageSettings = null;
			this.printDocument = null;
			this.printerSettings = null;
			this.showHelp = false;
			this.showNetwork = true;
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x00176D9B File Offset: 0x00175D9B
		private void ResetMinMargins()
		{
			this.MinMargins = null;
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x00176DA4 File Offset: 0x00175DA4
		private bool ShouldSerializeMinMargins()
		{
			return this.minMargins.Left != 0 || this.minMargins.Right != 0 || this.minMargins.Top != 0 || this.minMargins.Bottom != 0;
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x00176DE0 File Offset: 0x00175DE0
		private static void UpdateSettings(NativeMethods.PAGESETUPDLG data, PageSettings pageSettings, PrinterSettings printerSettings)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				pageSettings.SetHdevmode(data.hDevMode);
				if (printerSettings != null)
				{
					printerSettings.SetHdevmode(data.hDevMode);
					printerSettings.SetHdevnames(data.hDevNames);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			Margins margins = new Margins();
			margins.Left = data.marginLeft;
			margins.Top = data.marginTop;
			margins.Right = data.marginRight;
			margins.Bottom = data.marginBottom;
			PrinterUnit fromUnit = ((data.Flags & 8) != 0) ? PrinterUnit.HundredthsOfAMillimeter : PrinterUnit.ThousandthsOfAnInch;
			pageSettings.Margins = PrinterUnitConvert.Convert(margins, fromUnit, PrinterUnit.Display);
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x00176E8C File Offset: 0x00175E8C
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			IntSecurity.SafePrinting.Demand();
			NativeMethods.WndProc lpfnPageSetupHook = new NativeMethods.WndProc(this.HookProc);
			if (this.pageSettings == null)
			{
				throw new ArgumentException(SR.GetString("PSDcantShowWithoutPage"));
			}
			NativeMethods.PAGESETUPDLG pagesetupdlg = new NativeMethods.PAGESETUPDLG();
			pagesetupdlg.lStructSize = Marshal.SizeOf(pagesetupdlg);
			pagesetupdlg.Flags = this.GetFlags();
			pagesetupdlg.hwndOwner = hwndOwner;
			pagesetupdlg.lpfnPageSetupHook = lpfnPageSetupHook;
			PrinterUnit toUnit = PrinterUnit.ThousandthsOfAnInch;
			if (this.EnableMetric)
			{
				StringBuilder stringBuilder = new StringBuilder(2);
				int localeInfo = UnsafeNativeMethods.GetLocaleInfo(NativeMethods.LOCALE_USER_DEFAULT, 13, stringBuilder, stringBuilder.Capacity);
				if (localeInfo > 0 && int.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture) == 0)
				{
					toUnit = PrinterUnit.HundredthsOfAMillimeter;
				}
			}
			if (this.MinMargins != null)
			{
				Margins margins = PrinterUnitConvert.Convert(this.MinMargins, PrinterUnit.Display, toUnit);
				pagesetupdlg.minMarginLeft = margins.Left;
				pagesetupdlg.minMarginTop = margins.Top;
				pagesetupdlg.minMarginRight = margins.Right;
				pagesetupdlg.minMarginBottom = margins.Bottom;
			}
			if (this.pageSettings.Margins != null)
			{
				Margins margins2 = PrinterUnitConvert.Convert(this.pageSettings.Margins, PrinterUnit.Display, toUnit);
				pagesetupdlg.marginLeft = margins2.Left;
				pagesetupdlg.marginTop = margins2.Top;
				pagesetupdlg.marginRight = margins2.Right;
				pagesetupdlg.marginBottom = margins2.Bottom;
			}
			pagesetupdlg.marginLeft = Math.Max(pagesetupdlg.marginLeft, pagesetupdlg.minMarginLeft);
			pagesetupdlg.marginTop = Math.Max(pagesetupdlg.marginTop, pagesetupdlg.minMarginTop);
			pagesetupdlg.marginRight = Math.Max(pagesetupdlg.marginRight, pagesetupdlg.minMarginRight);
			pagesetupdlg.marginBottom = Math.Max(pagesetupdlg.marginBottom, pagesetupdlg.minMarginBottom);
			PrinterSettings printerSettings = (this.printerSettings == null) ? this.pageSettings.PrinterSettings : this.printerSettings;
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				pagesetupdlg.hDevMode = printerSettings.GetHdevmode(this.pageSettings);
				pagesetupdlg.hDevNames = printerSettings.GetHdevnames();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool result;
			try
			{
				if (!UnsafeNativeMethods.PageSetupDlg(pagesetupdlg))
				{
					result = false;
				}
				else
				{
					PageSetupDialog.UpdateSettings(pagesetupdlg, this.pageSettings, this.printerSettings);
					result = true;
				}
			}
			finally
			{
				UnsafeNativeMethods.GlobalFree(new HandleRef(pagesetupdlg, pagesetupdlg.hDevMode));
				UnsafeNativeMethods.GlobalFree(new HandleRef(pagesetupdlg, pagesetupdlg.hDevNames));
			}
			return result;
		}

		// Token: 0x04003CC2 RID: 15554
		private PrintDocument printDocument;

		// Token: 0x04003CC3 RID: 15555
		private PageSettings pageSettings;

		// Token: 0x04003CC4 RID: 15556
		private PrinterSettings printerSettings;

		// Token: 0x04003CC5 RID: 15557
		private bool allowMargins;

		// Token: 0x04003CC6 RID: 15558
		private bool allowOrientation;

		// Token: 0x04003CC7 RID: 15559
		private bool allowPaper;

		// Token: 0x04003CC8 RID: 15560
		private bool allowPrinter;

		// Token: 0x04003CC9 RID: 15561
		private Margins minMargins;

		// Token: 0x04003CCA RID: 15562
		private bool showHelp;

		// Token: 0x04003CCB RID: 15563
		private bool showNetwork;

		// Token: 0x04003CCC RID: 15564
		private bool enableMetric;
	}
}
