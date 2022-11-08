using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020007A3 RID: 1955
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
	[SRDescription("DescriptionPrintPreviewDialog")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignTimeVisible(true)]
	[DefaultProperty("Document")]
	public partial class PrintPreviewDialog : Form
	{
		// Token: 0x0600669D RID: 26269 RVA: 0x0017955C File Offset: 0x0017855C
		public PrintPreviewDialog()
		{
			base.AutoScaleBaseSize = new Size(5, 13);
			this.previewControl = new PrintPreviewControl();
			this.imageList = new ImageList();
			Bitmap bitmap = new Bitmap(typeof(PrintPreviewDialog), "PrintPreviewStrip.bmp");
			bitmap.MakeTransparent();
			this.imageList.Images.AddStrip(bitmap);
			this.InitForm();
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x0600669E RID: 26270 RVA: 0x001795C6 File Offset: 0x001785C6
		// (set) Token: 0x0600669F RID: 26271 RVA: 0x001795CE File Offset: 0x001785CE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new IButtonControl AcceptButton
		{
			get
			{
				return base.AcceptButton;
			}
			set
			{
				base.AcceptButton = value;
			}
		}

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x060066A0 RID: 26272 RVA: 0x001795D7 File Offset: 0x001785D7
		// (set) Token: 0x060066A1 RID: 26273 RVA: 0x001795DF File Offset: 0x001785DF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool AutoScale
		{
			get
			{
				return base.AutoScale;
			}
			set
			{
				base.AutoScale = value;
			}
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x060066A2 RID: 26274 RVA: 0x001795E8 File Offset: 0x001785E8
		// (set) Token: 0x060066A3 RID: 26275 RVA: 0x001795F0 File Offset: 0x001785F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x060066A4 RID: 26276 RVA: 0x001795F9 File Offset: 0x001785F9
		// (set) Token: 0x060066A5 RID: 26277 RVA: 0x00179601 File Offset: 0x00178601
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x140003F9 RID: 1017
		// (add) Token: 0x060066A6 RID: 26278 RVA: 0x0017960A File Offset: 0x0017860A
		// (remove) Token: 0x060066A7 RID: 26279 RVA: 0x00179613 File Offset: 0x00178613
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x060066A8 RID: 26280 RVA: 0x0017961C File Offset: 0x0017861C
		// (set) Token: 0x060066A9 RID: 26281 RVA: 0x00179624 File Offset: 0x00178624
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		// Token: 0x140003FA RID: 1018
		// (add) Token: 0x060066AA RID: 26282 RVA: 0x0017962D File Offset: 0x0017862D
		// (remove) Token: 0x060066AB RID: 26283 RVA: 0x00179636 File Offset: 0x00178636
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoValidateChanged
		{
			add
			{
				base.AutoValidateChanged += value;
			}
			remove
			{
				base.AutoValidateChanged -= value;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x060066AC RID: 26284 RVA: 0x0017963F File Offset: 0x0017863F
		// (set) Token: 0x060066AD RID: 26285 RVA: 0x00179647 File Offset: 0x00178647
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x140003FB RID: 1019
		// (add) Token: 0x060066AE RID: 26286 RVA: 0x00179650 File Offset: 0x00178650
		// (remove) Token: 0x060066AF RID: 26287 RVA: 0x00179659 File Offset: 0x00178659
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x060066B0 RID: 26288 RVA: 0x00179662 File Offset: 0x00178662
		// (set) Token: 0x060066B1 RID: 26289 RVA: 0x0017966A File Offset: 0x0017866A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new IButtonControl CancelButton
		{
			get
			{
				return base.CancelButton;
			}
			set
			{
				base.CancelButton = value;
			}
		}

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x060066B2 RID: 26290 RVA: 0x00179673 File Offset: 0x00178673
		// (set) Token: 0x060066B3 RID: 26291 RVA: 0x0017967B File Offset: 0x0017867B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool ControlBox
		{
			get
			{
				return base.ControlBox;
			}
			set
			{
				base.ControlBox = value;
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x060066B4 RID: 26292 RVA: 0x00179684 File Offset: 0x00178684
		// (set) Token: 0x060066B5 RID: 26293 RVA: 0x0017968C File Offset: 0x0017868C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		// Token: 0x140003FC RID: 1020
		// (add) Token: 0x060066B6 RID: 26294 RVA: 0x00179695 File Offset: 0x00178695
		// (remove) Token: 0x060066B7 RID: 26295 RVA: 0x0017969E File Offset: 0x0017869E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ContextMenuStripChanged
		{
			add
			{
				base.ContextMenuStripChanged += value;
			}
			remove
			{
				base.ContextMenuStripChanged -= value;
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x060066B8 RID: 26296 RVA: 0x001796A7 File Offset: 0x001786A7
		// (set) Token: 0x060066B9 RID: 26297 RVA: 0x001796AF File Offset: 0x001786AF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormBorderStyle FormBorderStyle
		{
			get
			{
				return base.FormBorderStyle;
			}
			set
			{
				base.FormBorderStyle = value;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x060066BA RID: 26298 RVA: 0x001796B8 File Offset: 0x001786B8
		// (set) Token: 0x060066BB RID: 26299 RVA: 0x001796C0 File Offset: 0x001786C0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool HelpButton
		{
			get
			{
				return base.HelpButton;
			}
			set
			{
				base.HelpButton = value;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x060066BC RID: 26300 RVA: 0x001796C9 File Offset: 0x001786C9
		// (set) Token: 0x060066BD RID: 26301 RVA: 0x001796D1 File Offset: 0x001786D1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Icon Icon
		{
			get
			{
				return base.Icon;
			}
			set
			{
				base.Icon = value;
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x060066BE RID: 26302 RVA: 0x001796DA File Offset: 0x001786DA
		// (set) Token: 0x060066BF RID: 26303 RVA: 0x001796E2 File Offset: 0x001786E2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool IsMdiContainer
		{
			get
			{
				return base.IsMdiContainer;
			}
			set
			{
				base.IsMdiContainer = value;
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x060066C0 RID: 26304 RVA: 0x001796EB File Offset: 0x001786EB
		// (set) Token: 0x060066C1 RID: 26305 RVA: 0x001796F3 File Offset: 0x001786F3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool KeyPreview
		{
			get
			{
				return base.KeyPreview;
			}
			set
			{
				base.KeyPreview = value;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x060066C2 RID: 26306 RVA: 0x001796FC File Offset: 0x001786FC
		// (set) Token: 0x060066C3 RID: 26307 RVA: 0x00179704 File Offset: 0x00178704
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = value;
			}
		}

		// Token: 0x140003FD RID: 1021
		// (add) Token: 0x060066C4 RID: 26308 RVA: 0x0017970D File Offset: 0x0017870D
		// (remove) Token: 0x060066C5 RID: 26309 RVA: 0x00179716 File Offset: 0x00178716
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MaximumSizeChanged
		{
			add
			{
				base.MaximumSizeChanged += value;
			}
			remove
			{
				base.MaximumSizeChanged -= value;
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x060066C6 RID: 26310 RVA: 0x0017971F File Offset: 0x0017871F
		// (set) Token: 0x060066C7 RID: 26311 RVA: 0x00179727 File Offset: 0x00178727
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool MaximizeBox
		{
			get
			{
				return base.MaximizeBox;
			}
			set
			{
				base.MaximizeBox = value;
			}
		}

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x060066C8 RID: 26312 RVA: 0x00179730 File Offset: 0x00178730
		// (set) Token: 0x060066C9 RID: 26313 RVA: 0x00179738 File Offset: 0x00178738
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Margin
		{
			get
			{
				return base.Margin;
			}
			set
			{
				base.Margin = value;
			}
		}

		// Token: 0x140003FE RID: 1022
		// (add) Token: 0x060066CA RID: 26314 RVA: 0x00179741 File Offset: 0x00178741
		// (remove) Token: 0x060066CB RID: 26315 RVA: 0x0017974A File Offset: 0x0017874A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MarginChanged
		{
			add
			{
				base.MarginChanged += value;
			}
			remove
			{
				base.MarginChanged -= value;
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x060066CC RID: 26316 RVA: 0x00179753 File Offset: 0x00178753
		// (set) Token: 0x060066CD RID: 26317 RVA: 0x0017975B File Offset: 0x0017875B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new MainMenu Menu
		{
			get
			{
				return base.Menu;
			}
			set
			{
				base.Menu = value;
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x060066CE RID: 26318 RVA: 0x00179764 File Offset: 0x00178764
		// (set) Token: 0x060066CF RID: 26319 RVA: 0x0017976C File Offset: 0x0017876C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = value;
			}
		}

		// Token: 0x140003FF RID: 1023
		// (add) Token: 0x060066D0 RID: 26320 RVA: 0x00179775 File Offset: 0x00178775
		// (remove) Token: 0x060066D1 RID: 26321 RVA: 0x0017977E File Offset: 0x0017877E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MinimumSizeChanged
		{
			add
			{
				base.MinimumSizeChanged += value;
			}
			remove
			{
				base.MinimumSizeChanged -= value;
			}
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x060066D2 RID: 26322 RVA: 0x00179787 File Offset: 0x00178787
		// (set) Token: 0x060066D3 RID: 26323 RVA: 0x0017978F File Offset: 0x0017878F
		[Browsable(false)]
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

		// Token: 0x14000400 RID: 1024
		// (add) Token: 0x060066D4 RID: 26324 RVA: 0x00179798 File Offset: 0x00178798
		// (remove) Token: 0x060066D5 RID: 26325 RVA: 0x001797A1 File Offset: 0x001787A1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x060066D6 RID: 26326 RVA: 0x001797AA File Offset: 0x001787AA
		// (set) Token: 0x060066D7 RID: 26327 RVA: 0x001797B2 File Offset: 0x001787B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		// Token: 0x14000401 RID: 1025
		// (add) Token: 0x060066D8 RID: 26328 RVA: 0x001797BB File Offset: 0x001787BB
		// (remove) Token: 0x060066D9 RID: 26329 RVA: 0x001797C4 File Offset: 0x001787C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler SizeChanged
		{
			add
			{
				base.SizeChanged += value;
			}
			remove
			{
				base.SizeChanged -= value;
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x060066DA RID: 26330 RVA: 0x001797CD File Offset: 0x001787CD
		// (set) Token: 0x060066DB RID: 26331 RVA: 0x001797D5 File Offset: 0x001787D5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormStartPosition StartPosition
		{
			get
			{
				return base.StartPosition;
			}
			set
			{
				base.StartPosition = value;
			}
		}

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x060066DC RID: 26332 RVA: 0x001797DE File Offset: 0x001787DE
		// (set) Token: 0x060066DD RID: 26333 RVA: 0x001797E6 File Offset: 0x001787E6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TopMost
		{
			get
			{
				return base.TopMost;
			}
			set
			{
				base.TopMost = value;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x060066DE RID: 26334 RVA: 0x001797EF File Offset: 0x001787EF
		// (set) Token: 0x060066DF RID: 26335 RVA: 0x001797F7 File Offset: 0x001787F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Color TransparencyKey
		{
			get
			{
				return base.TransparencyKey;
			}
			set
			{
				base.TransparencyKey = value;
			}
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x060066E0 RID: 26336 RVA: 0x00179800 File Offset: 0x00178800
		// (set) Token: 0x060066E1 RID: 26337 RVA: 0x00179808 File Offset: 0x00178808
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseWaitCursor
		{
			get
			{
				return base.UseWaitCursor;
			}
			set
			{
				base.UseWaitCursor = value;
			}
		}

		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x060066E2 RID: 26338 RVA: 0x00179811 File Offset: 0x00178811
		// (set) Token: 0x060066E3 RID: 26339 RVA: 0x00179819 File Offset: 0x00178819
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormWindowState WindowState
		{
			get
			{
				return base.WindowState;
			}
			set
			{
				base.WindowState = value;
			}
		}

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x060066E4 RID: 26340 RVA: 0x00179822 File Offset: 0x00178822
		// (set) Token: 0x060066E5 RID: 26341 RVA: 0x0017982A File Offset: 0x0017882A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new AccessibleRole AccessibleRole
		{
			get
			{
				return base.AccessibleRole;
			}
			set
			{
				base.AccessibleRole = value;
			}
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x060066E6 RID: 26342 RVA: 0x00179833 File Offset: 0x00178833
		// (set) Token: 0x060066E7 RID: 26343 RVA: 0x0017983B File Offset: 0x0017883B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string AccessibleDescription
		{
			get
			{
				return base.AccessibleDescription;
			}
			set
			{
				base.AccessibleDescription = value;
			}
		}

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x060066E8 RID: 26344 RVA: 0x00179844 File Offset: 0x00178844
		// (set) Token: 0x060066E9 RID: 26345 RVA: 0x0017984C File Offset: 0x0017884C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string AccessibleName
		{
			get
			{
				return base.AccessibleName;
			}
			set
			{
				base.AccessibleName = value;
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x060066EA RID: 26346 RVA: 0x00179855 File Offset: 0x00178855
		// (set) Token: 0x060066EB RID: 26347 RVA: 0x0017985D File Offset: 0x0017885D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		// Token: 0x14000402 RID: 1026
		// (add) Token: 0x060066EC RID: 26348 RVA: 0x00179866 File Offset: 0x00178866
		// (remove) Token: 0x060066ED RID: 26349 RVA: 0x0017986F File Offset: 0x0017886F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x060066EE RID: 26350 RVA: 0x00179878 File Offset: 0x00178878
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ControlBindingsCollection DataBindings
		{
			get
			{
				return base.DataBindings;
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x060066EF RID: 26351 RVA: 0x00179880 File Offset: 0x00178880
		protected override Size DefaultMinimumSize
		{
			get
			{
				return new Size(375, 250);
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x060066F0 RID: 26352 RVA: 0x00179891 File Offset: 0x00178891
		// (set) Token: 0x060066F1 RID: 26353 RVA: 0x00179899 File Offset: 0x00178899
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x14000403 RID: 1027
		// (add) Token: 0x060066F2 RID: 26354 RVA: 0x001798A2 File Offset: 0x001788A2
		// (remove) Token: 0x060066F3 RID: 26355 RVA: 0x001798AB File Offset: 0x001788AB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				base.EnabledChanged += value;
			}
			remove
			{
				base.EnabledChanged -= value;
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x060066F4 RID: 26356 RVA: 0x001798B4 File Offset: 0x001788B4
		// (set) Token: 0x060066F5 RID: 26357 RVA: 0x001798BC File Offset: 0x001788BC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		// Token: 0x14000404 RID: 1028
		// (add) Token: 0x060066F6 RID: 26358 RVA: 0x001798C5 File Offset: 0x001788C5
		// (remove) Token: 0x060066F7 RID: 26359 RVA: 0x001798CE File Offset: 0x001788CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler LocationChanged
		{
			add
			{
				base.LocationChanged += value;
			}
			remove
			{
				base.LocationChanged -= value;
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x060066F8 RID: 26360 RVA: 0x001798D7 File Offset: 0x001788D7
		// (set) Token: 0x060066F9 RID: 26361 RVA: 0x001798DF File Offset: 0x001788DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new object Tag
		{
			get
			{
				return base.Tag;
			}
			set
			{
				base.Tag = value;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x060066FA RID: 26362 RVA: 0x001798E8 File Offset: 0x001788E8
		// (set) Token: 0x060066FB RID: 26363 RVA: 0x001798F0 File Offset: 0x001788F0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x060066FC RID: 26364 RVA: 0x001798F9 File Offset: 0x001788F9
		// (set) Token: 0x060066FD RID: 26365 RVA: 0x00179901 File Offset: 0x00178901
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		// Token: 0x14000405 RID: 1029
		// (add) Token: 0x060066FE RID: 26366 RVA: 0x0017990A File Offset: 0x0017890A
		// (remove) Token: 0x060066FF RID: 26367 RVA: 0x00179913 File Offset: 0x00178913
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06006700 RID: 26368 RVA: 0x0017991C File Offset: 0x0017891C
		// (set) Token: 0x06006701 RID: 26369 RVA: 0x00179924 File Offset: 0x00178924
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x14000406 RID: 1030
		// (add) Token: 0x06006702 RID: 26370 RVA: 0x0017992D File Offset: 0x0017892D
		// (remove) Token: 0x06006703 RID: 26371 RVA: 0x00179936 File Offset: 0x00178936
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06006704 RID: 26372 RVA: 0x0017993F File Offset: 0x0017893F
		// (set) Token: 0x06006705 RID: 26373 RVA: 0x00179947 File Offset: 0x00178947
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x14000407 RID: 1031
		// (add) Token: 0x06006706 RID: 26374 RVA: 0x00179950 File Offset: 0x00178950
		// (remove) Token: 0x06006707 RID: 26375 RVA: 0x00179959 File Offset: 0x00178959
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06006708 RID: 26376 RVA: 0x00179962 File Offset: 0x00178962
		// (set) Token: 0x06006709 RID: 26377 RVA: 0x0017996A File Offset: 0x0017896A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		// Token: 0x14000408 RID: 1032
		// (add) Token: 0x0600670A RID: 26378 RVA: 0x00179973 File Offset: 0x00178973
		// (remove) Token: 0x0600670B RID: 26379 RVA: 0x0017997C File Offset: 0x0017897C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x0600670C RID: 26380 RVA: 0x00179985 File Offset: 0x00178985
		// (set) Token: 0x0600670D RID: 26381 RVA: 0x0017998D File Offset: 0x0017898D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x0600670E RID: 26382 RVA: 0x00179996 File Offset: 0x00178996
		// (set) Token: 0x0600670F RID: 26383 RVA: 0x0017999E File Offset: 0x0017899E
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06006710 RID: 26384 RVA: 0x001799A7 File Offset: 0x001789A7
		// (set) Token: 0x06006711 RID: 26385 RVA: 0x001799AF File Offset: 0x001789AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06006712 RID: 26386 RVA: 0x001799B8 File Offset: 0x001789B8
		// (set) Token: 0x06006713 RID: 26387 RVA: 0x001799C0 File Offset: 0x001789C0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		// Token: 0x14000409 RID: 1033
		// (add) Token: 0x06006714 RID: 26388 RVA: 0x001799C9 File Offset: 0x001789C9
		// (remove) Token: 0x06006715 RID: 26389 RVA: 0x001799D2 File Offset: 0x001789D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler VisibleChanged
		{
			add
			{
				base.VisibleChanged += value;
			}
			remove
			{
				base.VisibleChanged -= value;
			}
		}

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06006716 RID: 26390 RVA: 0x001799DB File Offset: 0x001789DB
		// (set) Token: 0x06006717 RID: 26391 RVA: 0x001799E3 File Offset: 0x001789E3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x1400040A RID: 1034
		// (add) Token: 0x06006718 RID: 26392 RVA: 0x001799EC File Offset: 0x001789EC
		// (remove) Token: 0x06006719 RID: 26393 RVA: 0x001799F5 File Offset: 0x001789F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x0600671A RID: 26394 RVA: 0x001799FE File Offset: 0x001789FE
		// (set) Token: 0x0600671B RID: 26395 RVA: 0x00179A06 File Offset: 0x00178A06
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x0600671C RID: 26396 RVA: 0x00179A0F File Offset: 0x00178A0F
		// (set) Token: 0x0600671D RID: 26397 RVA: 0x00179A17 File Offset: 0x00178A17
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool RightToLeftLayout
		{
			get
			{
				return base.RightToLeftLayout;
			}
			set
			{
				base.RightToLeftLayout = value;
			}
		}

		// Token: 0x1400040B RID: 1035
		// (add) Token: 0x0600671E RID: 26398 RVA: 0x00179A20 File Offset: 0x00178A20
		// (remove) Token: 0x0600671F RID: 26399 RVA: 0x00179A29 File Offset: 0x00178A29
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		// Token: 0x1400040C RID: 1036
		// (add) Token: 0x06006720 RID: 26400 RVA: 0x00179A32 File Offset: 0x00178A32
		// (remove) Token: 0x06006721 RID: 26401 RVA: 0x00179A3B File Offset: 0x00178A3B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.RightToLeftLayoutChanged += value;
			}
			remove
			{
				base.RightToLeftLayoutChanged -= value;
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x06006722 RID: 26402 RVA: 0x00179A44 File Offset: 0x00178A44
		// (set) Token: 0x06006723 RID: 26403 RVA: 0x00179A4C File Offset: 0x00178A4C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x1400040D RID: 1037
		// (add) Token: 0x06006724 RID: 26404 RVA: 0x00179A55 File Offset: 0x00178A55
		// (remove) Token: 0x06006725 RID: 26405 RVA: 0x00179A5E File Offset: 0x00178A5E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x06006726 RID: 26406 RVA: 0x00179A67 File Offset: 0x00178A67
		// (set) Token: 0x06006727 RID: 26407 RVA: 0x00179A6F File Offset: 0x00178A6F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x1400040E RID: 1038
		// (add) Token: 0x06006728 RID: 26408 RVA: 0x00179A78 File Offset: 0x00178A78
		// (remove) Token: 0x06006729 RID: 26409 RVA: 0x00179A81 File Offset: 0x00178A81
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x0600672A RID: 26410 RVA: 0x00179A8A File Offset: 0x00178A8A
		// (set) Token: 0x0600672B RID: 26411 RVA: 0x00179A92 File Offset: 0x00178A92
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		// Token: 0x1400040F RID: 1039
		// (add) Token: 0x0600672C RID: 26412 RVA: 0x00179A9B File Offset: 0x00178A9B
		// (remove) Token: 0x0600672D RID: 26413 RVA: 0x00179AA4 File Offset: 0x00178AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DockChanged
		{
			add
			{
				base.DockChanged += value;
			}
			remove
			{
				base.DockChanged -= value;
			}
		}

		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x0600672E RID: 26414 RVA: 0x00179AAD File Offset: 0x00178AAD
		// (set) Token: 0x0600672F RID: 26415 RVA: 0x00179AB5 File Offset: 0x00178AB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x14000410 RID: 1040
		// (add) Token: 0x06006730 RID: 26416 RVA: 0x00179ABE File Offset: 0x00178ABE
		// (remove) Token: 0x06006731 RID: 26417 RVA: 0x00179AC7 File Offset: 0x00178AC7
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06006732 RID: 26418 RVA: 0x00179AD0 File Offset: 0x00178AD0
		// (set) Token: 0x06006733 RID: 26419 RVA: 0x00179AD8 File Offset: 0x00178AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
			}
		}

		// Token: 0x14000411 RID: 1041
		// (add) Token: 0x06006734 RID: 26420 RVA: 0x00179AE1 File Offset: 0x00178AE1
		// (remove) Token: 0x06006735 RID: 26421 RVA: 0x00179AEA File Offset: 0x00178AEA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ContextMenuChanged
		{
			add
			{
				base.ContextMenuChanged += value;
			}
			remove
			{
				base.ContextMenuChanged -= value;
			}
		}

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06006736 RID: 26422 RVA: 0x00179AF3 File Offset: 0x00178AF3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06006737 RID: 26423 RVA: 0x00179AFB File Offset: 0x00178AFB
		// (set) Token: 0x06006738 RID: 26424 RVA: 0x00179B08 File Offset: 0x00178B08
		[SRDescription("PrintPreviewAntiAliasDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool UseAntiAlias
		{
			get
			{
				return this.PrintPreviewControl.UseAntiAlias;
			}
			set
			{
				this.PrintPreviewControl.UseAntiAlias = value;
			}
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06006739 RID: 26425 RVA: 0x00179B16 File Offset: 0x00178B16
		// (set) Token: 0x0600673A RID: 26426 RVA: 0x00179B1E File Offset: 0x00178B1E
		[Browsable(false)]
		[Obsolete("This property has been deprecated. Use the AutoScaleDimensions property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Size AutoScaleBaseSize
		{
			get
			{
				return base.AutoScaleBaseSize;
			}
			set
			{
			}
		}

		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x0600673B RID: 26427 RVA: 0x00179B20 File Offset: 0x00178B20
		// (set) Token: 0x0600673C RID: 26428 RVA: 0x00179B2D File Offset: 0x00178B2D
		[SRDescription("PrintPreviewDocumentDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		public PrintDocument Document
		{
			get
			{
				return this.previewControl.Document;
			}
			set
			{
				this.previewControl.Document = value;
			}
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x0600673D RID: 26429 RVA: 0x00179B3B File Offset: 0x00178B3B
		// (set) Token: 0x0600673E RID: 26430 RVA: 0x00179B43 File Offset: 0x00178B43
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(false)]
		[Browsable(false)]
		public new bool MinimizeBox
		{
			get
			{
				return base.MinimizeBox;
			}
			set
			{
				base.MinimizeBox = value;
			}
		}

		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x0600673F RID: 26431 RVA: 0x00179B4C File Offset: 0x00178B4C
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewPrintPreviewControlDescr")]
		public PrintPreviewControl PrintPreviewControl
		{
			get
			{
				return this.previewControl;
			}
		}

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x06006740 RID: 26432 RVA: 0x00179B54 File Offset: 0x00178B54
		// (set) Token: 0x06006741 RID: 26433 RVA: 0x00179B5C File Offset: 0x00178B5C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new double Opacity
		{
			get
			{
				return base.Opacity;
			}
			set
			{
				base.Opacity = value;
			}
		}

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06006742 RID: 26434 RVA: 0x00179B65 File Offset: 0x00178B65
		// (set) Token: 0x06006743 RID: 26435 RVA: 0x00179B6D File Offset: 0x00178B6D
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool ShowInTaskbar
		{
			get
			{
				return base.ShowInTaskbar;
			}
			set
			{
				base.ShowInTaskbar = value;
			}
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06006744 RID: 26436 RVA: 0x00179B76 File Offset: 0x00178B76
		// (set) Token: 0x06006745 RID: 26437 RVA: 0x00179B7E File Offset: 0x00178B7E
		[Browsable(false)]
		[DefaultValue(SizeGripStyle.Hide)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new SizeGripStyle SizeGripStyle
		{
			get
			{
				return base.SizeGripStyle;
			}
			set
			{
				base.SizeGripStyle = value;
			}
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x00179B88 File Offset: 0x00178B88
		private void InitForm()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PrintPreviewDialog));
			this.toolStrip1 = new ToolStrip();
			this.printToolStripButton = new ToolStripButton();
			this.zoomToolStripSplitButton = new ToolStripSplitButton();
			this.autoToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripMenuItem1 = new ToolStripMenuItem();
			this.toolStripMenuItem2 = new ToolStripMenuItem();
			this.toolStripMenuItem3 = new ToolStripMenuItem();
			this.toolStripMenuItem4 = new ToolStripMenuItem();
			this.toolStripMenuItem5 = new ToolStripMenuItem();
			this.toolStripMenuItem6 = new ToolStripMenuItem();
			this.toolStripMenuItem7 = new ToolStripMenuItem();
			this.toolStripMenuItem8 = new ToolStripMenuItem();
			this.separatorToolStripSeparator = new ToolStripSeparator();
			this.onepageToolStripButton = new ToolStripButton();
			this.twopagesToolStripButton = new ToolStripButton();
			this.threepagesToolStripButton = new ToolStripButton();
			this.fourpagesToolStripButton = new ToolStripButton();
			this.sixpagesToolStripButton = new ToolStripButton();
			this.separatorToolStripSeparator1 = new ToolStripSeparator();
			this.closeToolStripButton = new ToolStripButton();
			this.pageCounter = new NumericUpDown();
			this.pageToolStripLabel = new ToolStripLabel();
			this.toolStrip1.SuspendLayout();
			((ISupportInitialize)this.pageCounter).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.printToolStripButton,
				this.zoomToolStripSplitButton,
				this.separatorToolStripSeparator,
				this.onepageToolStripButton,
				this.twopagesToolStripButton,
				this.threepagesToolStripButton,
				this.fourpagesToolStripButton,
				this.sixpagesToolStripButton,
				this.separatorToolStripSeparator1,
				this.closeToolStripButton
			});
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = ToolStripRenderMode.System;
			this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
			this.printToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.printToolStripButton.Name = "printToolStripButton";
			componentResourceManager.ApplyResources(this.printToolStripButton, "printToolStripButton");
			this.zoomToolStripSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.zoomToolStripSplitButton.DoubleClickEnabled = true;
			this.zoomToolStripSplitButton.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.autoToolStripMenuItem,
				this.toolStripMenuItem1,
				this.toolStripMenuItem2,
				this.toolStripMenuItem3,
				this.toolStripMenuItem4,
				this.toolStripMenuItem5,
				this.toolStripMenuItem6,
				this.toolStripMenuItem7,
				this.toolStripMenuItem8
			});
			this.zoomToolStripSplitButton.Name = "zoomToolStripSplitButton";
			this.zoomToolStripSplitButton.SplitterWidth = 1;
			componentResourceManager.ApplyResources(this.zoomToolStripSplitButton, "zoomToolStripSplitButton");
			this.autoToolStripMenuItem.CheckOnClick = true;
			this.autoToolStripMenuItem.DoubleClickEnabled = true;
			this.autoToolStripMenuItem.Checked = true;
			this.autoToolStripMenuItem.Name = "autoToolStripMenuItem";
			componentResourceManager.ApplyResources(this.autoToolStripMenuItem, "autoToolStripMenuItem");
			this.toolStripMenuItem1.CheckOnClick = true;
			this.toolStripMenuItem1.DoubleClickEnabled = true;
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem2.CheckOnClick = true;
			this.toolStripMenuItem2.DoubleClickEnabled = true;
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			componentResourceManager.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			this.toolStripMenuItem3.CheckOnClick = true;
			this.toolStripMenuItem3.DoubleClickEnabled = true;
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			componentResourceManager.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			this.toolStripMenuItem4.CheckOnClick = true;
			this.toolStripMenuItem4.DoubleClickEnabled = true;
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			componentResourceManager.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			this.toolStripMenuItem5.CheckOnClick = true;
			this.toolStripMenuItem5.DoubleClickEnabled = true;
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			componentResourceManager.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			this.toolStripMenuItem6.CheckOnClick = true;
			this.toolStripMenuItem6.DoubleClickEnabled = true;
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			componentResourceManager.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			this.toolStripMenuItem7.CheckOnClick = true;
			this.toolStripMenuItem7.DoubleClickEnabled = true;
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			componentResourceManager.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			this.toolStripMenuItem8.CheckOnClick = true;
			this.toolStripMenuItem8.DoubleClickEnabled = true;
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			componentResourceManager.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
			this.separatorToolStripSeparator.Name = "separatorToolStripSeparator";
			this.onepageToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.onepageToolStripButton.Name = "onepageToolStripButton";
			componentResourceManager.ApplyResources(this.onepageToolStripButton, "onepageToolStripButton");
			this.twopagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.twopagesToolStripButton.Name = "twopagesToolStripButton";
			componentResourceManager.ApplyResources(this.twopagesToolStripButton, "twopagesToolStripButton");
			this.threepagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.threepagesToolStripButton.Name = "threepagesToolStripButton";
			componentResourceManager.ApplyResources(this.threepagesToolStripButton, "threepagesToolStripButton");
			this.fourpagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.fourpagesToolStripButton.Name = "fourpagesToolStripButton";
			componentResourceManager.ApplyResources(this.fourpagesToolStripButton, "fourpagesToolStripButton");
			this.sixpagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.sixpagesToolStripButton.Name = "sixpagesToolStripButton";
			componentResourceManager.ApplyResources(this.sixpagesToolStripButton, "sixpagesToolStripButton");
			this.separatorToolStripSeparator1.Name = "separatorToolStripSeparator1";
			this.closeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.closeToolStripButton.Name = "closeToolStripButton";
			componentResourceManager.ApplyResources(this.closeToolStripButton, "closeToolStripButton");
			componentResourceManager.ApplyResources(this.pageCounter, "pageCounter");
			this.pageCounter.Text = "1";
			this.pageCounter.TextAlign = HorizontalAlignment.Right;
			this.pageCounter.DecimalPlaces = 0;
			this.pageCounter.Minimum = new decimal(0.0);
			this.pageCounter.Maximum = new decimal(1000.0);
			this.pageCounter.ValueChanged += this.UpdownMove;
			this.pageCounter.Name = "pageCounter";
			this.pageToolStripLabel.Alignment = ToolStripItemAlignment.Right;
			this.pageToolStripLabel.Name = "pageToolStripLabel";
			componentResourceManager.ApplyResources(this.pageToolStripLabel, "pageToolStripLabel");
			this.previewControl.Size = new Size(792, 610);
			this.previewControl.Location = new Point(0, 43);
			this.previewControl.Dock = DockStyle.Fill;
			this.previewControl.StartPageChanged += this.previewControl_StartPageChanged;
			this.printToolStripButton.Click += this.OnprintToolStripButtonClick;
			this.autoToolStripMenuItem.Click += this.ZoomAuto;
			this.toolStripMenuItem1.Click += this.Zoom500;
			this.toolStripMenuItem2.Click += this.Zoom250;
			this.toolStripMenuItem3.Click += this.Zoom150;
			this.toolStripMenuItem4.Click += this.Zoom100;
			this.toolStripMenuItem5.Click += this.Zoom75;
			this.toolStripMenuItem6.Click += this.Zoom50;
			this.toolStripMenuItem7.Click += this.Zoom25;
			this.toolStripMenuItem8.Click += this.Zoom10;
			this.onepageToolStripButton.Click += this.OnonepageToolStripButtonClick;
			this.twopagesToolStripButton.Click += this.OntwopagesToolStripButtonClick;
			this.threepagesToolStripButton.Click += this.OnthreepagesToolStripButtonClick;
			this.fourpagesToolStripButton.Click += this.OnfourpagesToolStripButtonClick;
			this.sixpagesToolStripButton.Click += this.OnsixpagesToolStripButtonClick;
			this.closeToolStripButton.Click += this.OncloseToolStripButtonClick;
			this.closeToolStripButton.Paint += this.OncloseToolStripButtonPaint;
			this.toolStrip1.ImageList = this.imageList;
			this.printToolStripButton.ImageIndex = 0;
			this.zoomToolStripSplitButton.ImageIndex = 1;
			this.onepageToolStripButton.ImageIndex = 2;
			this.twopagesToolStripButton.ImageIndex = 3;
			this.threepagesToolStripButton.ImageIndex = 4;
			this.fourpagesToolStripButton.ImageIndex = 5;
			this.sixpagesToolStripButton.ImageIndex = 6;
			this.previewControl.TabIndex = 0;
			this.toolStrip1.TabIndex = 1;
			this.zoomToolStripSplitButton.DefaultItem = this.autoToolStripMenuItem;
			ToolStripDropDownMenu toolStripDropDownMenu = this.zoomToolStripSplitButton.DropDown as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null)
			{
				toolStripDropDownMenu.ShowCheckMargin = true;
				toolStripDropDownMenu.ShowImageMargin = false;
				toolStripDropDownMenu.RenderMode = ToolStripRenderMode.System;
			}
			ToolStripControlHost toolStripControlHost = new ToolStripControlHost(this.pageCounter);
			toolStripControlHost.Alignment = ToolStripItemAlignment.Right;
			this.toolStrip1.Items.Add(toolStripControlHost);
			this.toolStrip1.Items.Add(this.pageToolStripLabel);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.previewControl);
			base.Controls.Add(this.toolStrip1);
			base.ClientSize = new Size(400, 300);
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = SizeGripStyle.Hide;
			this.toolStrip1.ResumeLayout(false);
			((ISupportInitialize)this.pageCounter).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06006747 RID: 26439 RVA: 0x0017A587 File Offset: 0x00179587
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			this.previewControl.InvalidatePreview();
		}

		// Token: 0x06006748 RID: 26440 RVA: 0x0017A59B File Offset: 0x0017959B
		protected override void CreateHandle()
		{
			if (this.Document != null && !this.Document.PrinterSettings.IsValid)
			{
				throw new InvalidPrinterException(this.Document.PrinterSettings);
			}
			base.CreateHandle();
		}

		// Token: 0x06006749 RID: 26441 RVA: 0x0017A5D0 File Offset: 0x001795D0
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				switch (keyData & Keys.KeyCode)
				{
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
					return false;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x0600674A RID: 26442 RVA: 0x0017A614 File Offset: 0x00179614
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessTabKey(bool forward)
		{
			if (base.ActiveControl == this.previewControl)
			{
				this.pageCounter.FocusInternal();
				return true;
			}
			return false;
		}

		// Token: 0x0600674B RID: 26443 RVA: 0x0017A633 File Offset: 0x00179633
		internal override bool ShouldSerializeAutoScaleBaseSize()
		{
			return false;
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x0017A636 File Offset: 0x00179636
		internal override bool ShouldSerializeText()
		{
			return !this.Text.Equals(SR.GetString("PrintPreviewDialog_PrintPreview"));
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x0017A650 File Offset: 0x00179650
		private void OncloseToolStripButtonClick(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600674E RID: 26446 RVA: 0x0017A658 File Offset: 0x00179658
		private void previewControl_StartPageChanged(object sender, EventArgs e)
		{
			this.pageCounter.Value = this.previewControl.StartPage + 1;
		}

		// Token: 0x0600674F RID: 26447 RVA: 0x0017A678 File Offset: 0x00179678
		private void CheckZoomMenu(ToolStripMenuItem toChecked)
		{
			foreach (object obj in this.zoomToolStripSplitButton.DropDownItems)
			{
				ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)obj;
				toolStripMenuItem.Checked = (toChecked == toolStripMenuItem);
			}
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x0017A6DC File Offset: 0x001796DC
		private void ZoomAuto(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.AutoZoom = true;
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x0017A704 File Offset: 0x00179704
		private void Zoom500(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 5.0;
		}

		// Token: 0x06006752 RID: 26450 RVA: 0x0017A734 File Offset: 0x00179734
		private void Zoom250(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 2.5;
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x0017A764 File Offset: 0x00179764
		private void Zoom150(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 1.5;
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x0017A794 File Offset: 0x00179794
		private void Zoom100(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 1.0;
		}

		// Token: 0x06006755 RID: 26453 RVA: 0x0017A7C4 File Offset: 0x001797C4
		private void Zoom75(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.75;
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x0017A7F4 File Offset: 0x001797F4
		private void Zoom50(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.5;
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x0017A824 File Offset: 0x00179824
		private void Zoom25(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.25;
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x0017A854 File Offset: 0x00179854
		private void Zoom10(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.1;
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x0017A884 File Offset: 0x00179884
		private void OncloseToolStripButtonPaint(object sender, PaintEventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null && !toolStripItem.Selected)
			{
				Rectangle rect = new Rectangle(0, 0, toolStripItem.Bounds.Width - 1, toolStripItem.Bounds.Height - 1);
				e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), rect);
			}
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x0017A8E2 File Offset: 0x001798E2
		private void OnprintToolStripButtonClick(object sender, EventArgs e)
		{
			if (this.previewControl.Document != null)
			{
				this.previewControl.Document.Print();
			}
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x0017A901 File Offset: 0x00179901
		private void OnzoomToolStripSplitButtonClick(object sender, EventArgs e)
		{
			this.ZoomAuto(null, EventArgs.Empty);
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x0017A90F File Offset: 0x0017990F
		private void OnonepageToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 1;
			this.previewControl.Columns = 1;
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x0017A929 File Offset: 0x00179929
		private void OntwopagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 1;
			this.previewControl.Columns = 2;
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x0017A943 File Offset: 0x00179943
		private void OnthreepagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 1;
			this.previewControl.Columns = 3;
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x0017A95D File Offset: 0x0017995D
		private void OnfourpagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 2;
			this.previewControl.Columns = 2;
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x0017A977 File Offset: 0x00179977
		private void OnsixpagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 2;
			this.previewControl.Columns = 3;
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x0017A994 File Offset: 0x00179994
		private void UpdownMove(object sender, EventArgs eventargs)
		{
			int num = (int)this.pageCounter.Value - 1;
			if (num >= 0)
			{
				this.previewControl.StartPage = num;
				return;
			}
			this.pageCounter.Value = this.previewControl.StartPage + 1;
		}

		// Token: 0x04003CFA RID: 15610
		private PrintPreviewControl previewControl;

		// Token: 0x04003CFB RID: 15611
		private ToolStrip toolStrip1;

		// Token: 0x04003CFC RID: 15612
		private NumericUpDown pageCounter;

		// Token: 0x04003CFD RID: 15613
		private ToolStripButton printToolStripButton;

		// Token: 0x04003CFE RID: 15614
		private ToolStripSplitButton zoomToolStripSplitButton;

		// Token: 0x04003CFF RID: 15615
		private ToolStripMenuItem autoToolStripMenuItem;

		// Token: 0x04003D00 RID: 15616
		private ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x04003D01 RID: 15617
		private ToolStripMenuItem toolStripMenuItem2;

		// Token: 0x04003D02 RID: 15618
		private ToolStripMenuItem toolStripMenuItem3;

		// Token: 0x04003D03 RID: 15619
		private ToolStripMenuItem toolStripMenuItem4;

		// Token: 0x04003D04 RID: 15620
		private ToolStripMenuItem toolStripMenuItem5;

		// Token: 0x04003D05 RID: 15621
		private ToolStripMenuItem toolStripMenuItem6;

		// Token: 0x04003D06 RID: 15622
		private ToolStripMenuItem toolStripMenuItem7;

		// Token: 0x04003D07 RID: 15623
		private ToolStripMenuItem toolStripMenuItem8;

		// Token: 0x04003D08 RID: 15624
		private ToolStripSeparator separatorToolStripSeparator;

		// Token: 0x04003D09 RID: 15625
		private ToolStripButton onepageToolStripButton;

		// Token: 0x04003D0A RID: 15626
		private ToolStripButton twopagesToolStripButton;

		// Token: 0x04003D0B RID: 15627
		private ToolStripButton threepagesToolStripButton;

		// Token: 0x04003D0C RID: 15628
		private ToolStripButton fourpagesToolStripButton;

		// Token: 0x04003D0D RID: 15629
		private ToolStripButton sixpagesToolStripButton;

		// Token: 0x04003D0E RID: 15630
		private ToolStripSeparator separatorToolStripSeparator1;

		// Token: 0x04003D0F RID: 15631
		private ToolStripButton closeToolStripButton;

		// Token: 0x04003D10 RID: 15632
		private ToolStripLabel pageToolStripLabel;

		// Token: 0x04003D11 RID: 15633
		private ImageList imageList;
	}
}
