using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020007A2 RID: 1954
	[SRDescription("DescriptionPrintPreviewControl")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Document")]
	public class PrintPreviewControl : Control
	{
		// Token: 0x06006667 RID: 26215 RVA: 0x0017821C File Offset: 0x0017721C
		public PrintPreviewControl()
		{
			this.ResetBackColor();
			this.ResetForeColor();
			base.Size = new Size(100, 100);
			base.SetStyle(ControlStyles.ResizeRedraw, false);
			base.SetStyle(ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06006668 RID: 26216 RVA: 0x001782B3 File Offset: 0x001772B3
		// (set) Token: 0x06006669 RID: 26217 RVA: 0x001782BB File Offset: 0x001772BB
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewAntiAliasDescr")]
		[DefaultValue(false)]
		public bool UseAntiAlias
		{
			get
			{
				return this.antiAlias;
			}
			set
			{
				this.antiAlias = value;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x0600666A RID: 26218 RVA: 0x001782C4 File Offset: 0x001772C4
		// (set) Token: 0x0600666B RID: 26219 RVA: 0x001782CC File Offset: 0x001772CC
		[SRDescription("PrintPreviewAutoZoomDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AutoZoom
		{
			get
			{
				return this.autoZoom;
			}
			set
			{
				if (this.autoZoom != value)
				{
					this.autoZoom = value;
					this.InvalidateLayout();
				}
			}
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x0600666C RID: 26220 RVA: 0x001782E4 File Offset: 0x001772E4
		// (set) Token: 0x0600666D RID: 26221 RVA: 0x001782EC File Offset: 0x001772EC
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewDocumentDescr")]
		[DefaultValue(null)]
		public PrintDocument Document
		{
			get
			{
				return this.document;
			}
			set
			{
				this.document = value;
			}
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x0600666E RID: 26222 RVA: 0x001782F5 File Offset: 0x001772F5
		// (set) Token: 0x0600666F RID: 26223 RVA: 0x00178300 File Offset: 0x00177300
		[DefaultValue(1)]
		[SRCategory("CatLayout")]
		[SRDescription("PrintPreviewColumnsDescr")]
		public int Columns
		{
			get
			{
				return this.columns;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("Columns", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"Columns",
						value.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.columns = value;
				this.InvalidateLayout();
			}
		}

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06006670 RID: 26224 RVA: 0x00178364 File Offset: 0x00177364
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 1048576;
				createParams.Style |= 2097152;
				return createParams;
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06006671 RID: 26225 RVA: 0x0017839D File Offset: 0x0017739D
		// (set) Token: 0x06006672 RID: 26226 RVA: 0x001783A5 File Offset: 0x001773A5
		[SRDescription("ControlWithScrollbarsPositionDescr")]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private Point Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.SetPositionNoInvalidate(value);
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06006673 RID: 26227 RVA: 0x001783AE File Offset: 0x001773AE
		// (set) Token: 0x06006674 RID: 26228 RVA: 0x001783B8 File Offset: 0x001773B8
		[SRDescription("PrintPreviewRowsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(1)]
		public int Rows
		{
			get
			{
				return this.rows;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("Rows", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"Rows",
						value.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.rows = value;
				this.InvalidateLayout();
			}
		}

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06006675 RID: 26229 RVA: 0x0017841B File Offset: 0x0017741B
		// (set) Token: 0x06006676 RID: 26230 RVA: 0x00178423 File Offset: 0x00177423
		[AmbientValue(RightToLeft.Inherit)]
		[SRCategory("CatAppearance")]
		[SRDescription("ControlRightToLeftDescr")]
		[Localizable(true)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
				this.InvalidatePreview();
			}
		}

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x06006677 RID: 26231 RVA: 0x00178432 File Offset: 0x00177432
		// (set) Token: 0x06006678 RID: 26232 RVA: 0x0017843A File Offset: 0x0017743A
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140003F7 RID: 1015
		// (add) Token: 0x06006679 RID: 26233 RVA: 0x00178443 File Offset: 0x00177443
		// (remove) Token: 0x0600667A RID: 26234 RVA: 0x0017844C File Offset: 0x0017744C
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

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x0600667B RID: 26235 RVA: 0x00178458 File Offset: 0x00177458
		// (set) Token: 0x0600667C RID: 26236 RVA: 0x0017849C File Offset: 0x0017749C
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewStartPageDescr")]
		[DefaultValue(0)]
		public int StartPage
		{
			get
			{
				int val = this.startPage;
				if (this.pageInfo != null)
				{
					val = Math.Min(val, this.pageInfo.Length - this.rows * this.columns);
				}
				return Math.Max(val, 0);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("StartPage", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"StartPage",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int num = this.StartPage;
				this.startPage = value;
				if (num != this.startPage)
				{
					this.InvalidateLayout();
					this.OnStartPageChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140003F8 RID: 1016
		// (add) Token: 0x0600667D RID: 26237 RVA: 0x0017851A File Offset: 0x0017751A
		// (remove) Token: 0x0600667E RID: 26238 RVA: 0x0017852D File Offset: 0x0017752D
		[SRDescription("RadioButtonOnStartPageChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler StartPageChanged
		{
			add
			{
				base.Events.AddHandler(PrintPreviewControl.EVENT_STARTPAGECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(PrintPreviewControl.EVENT_STARTPAGECHANGED, value);
			}
		}

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x0600667F RID: 26239 RVA: 0x00178540 File Offset: 0x00177540
		// (set) Token: 0x06006680 RID: 26240 RVA: 0x00178548 File Offset: 0x00177548
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWithScrollbarsVirtualSizeDescr")]
		[SRCategory("CatLayout")]
		private Size VirtualSize
		{
			get
			{
				return this.virtualSize;
			}
			set
			{
				this.SetVirtualSizeNoInvalidate(value);
				base.Invalidate();
			}
		}

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06006681 RID: 26241 RVA: 0x00178557 File Offset: 0x00177557
		// (set) Token: 0x06006682 RID: 26242 RVA: 0x0017855F File Offset: 0x0017755F
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewZoomDescr")]
		[DefaultValue(0.3)]
		public double Zoom
		{
			get
			{
				return this.zoom;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException(SR.GetString("PrintPreviewControlZoomNegative"));
				}
				this.autoZoom = false;
				this.zoom = value;
				this.InvalidateLayout();
			}
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x00178594 File Offset: 0x00177594
		private int AdjustScroll(Message m, int pos, int maxPos, bool horizontal)
		{
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 0:
				if (pos > 5)
				{
					pos -= 5;
				}
				else
				{
					pos = 0;
				}
				break;
			case 1:
				if (pos < maxPos - 5)
				{
					pos += 5;
				}
				else
				{
					pos = maxPos;
				}
				break;
			case 2:
				if (pos > 100)
				{
					pos -= 100;
				}
				else
				{
					pos = 0;
				}
				break;
			case 3:
				if (pos < maxPos - 100)
				{
					pos += 100;
				}
				else
				{
					pos = maxPos;
				}
				break;
			case 4:
			case 5:
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 16;
				int fnBar = horizontal ? 0 : 1;
				if (SafeNativeMethods.GetScrollInfo(new HandleRef(this, m.HWnd), fnBar, scrollinfo))
				{
					pos = scrollinfo.nTrackPos;
				}
				else
				{
					pos = NativeMethods.Util.HIWORD(m.WParam);
				}
				break;
			}
			}
			return pos;
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x00178674 File Offset: 0x00177674
		private void ComputeLayout()
		{
			this.layoutOk = true;
			if (this.pageInfo.Length == 0)
			{
				base.ClientSize = base.Size;
				return;
			}
			Graphics graphics = base.CreateGraphicsInternal();
			IntPtr hdc = graphics.GetHdc();
			this.screendpi = new Point(UnsafeNativeMethods.GetDeviceCaps(new HandleRef(graphics, hdc), 88), UnsafeNativeMethods.GetDeviceCaps(new HandleRef(graphics, hdc), 90));
			graphics.ReleaseHdcInternal(hdc);
			graphics.Dispose();
			Size physicalSize = this.pageInfo[this.StartPage].PhysicalSize;
			Size size = new Size(PrintPreviewControl.PixelsToPhysical(new Point(base.Size), this.screendpi));
			if (this.autoZoom)
			{
				double val = ((double)size.Width - (double)(10 * (this.columns + 1))) / (double)(this.columns * physicalSize.Width);
				double val2 = ((double)size.Height - (double)(10 * (this.rows + 1))) / (double)(this.rows * physicalSize.Height);
				this.zoom = Math.Min(val, val2);
			}
			this.imageSize = new Size((int)(this.zoom * (double)physicalSize.Width), (int)(this.zoom * (double)physicalSize.Height));
			int x = this.imageSize.Width * this.columns + 10 * (this.columns + 1);
			int y = this.imageSize.Height * this.rows + 10 * (this.rows + 1);
			this.SetVirtualSizeNoInvalidate(new Size(PrintPreviewControl.PhysicalToPixels(new Point(x, y), this.screendpi)));
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x00178800 File Offset: 0x00177800
		private void ComputePreview()
		{
			int num = this.StartPage;
			if (this.document == null)
			{
				this.pageInfo = new PreviewPageInfo[0];
			}
			else
			{
				IntSecurity.SafePrinting.Demand();
				PrintController printController = this.document.PrintController;
				PreviewPrintController previewPrintController = new PreviewPrintController();
				previewPrintController.UseAntiAlias = this.UseAntiAlias;
				this.document.PrintController = new PrintControllerWithStatusDialog(previewPrintController, SR.GetString("PrintControllerWithStatusDialog_DialogTitlePreview"));
				this.document.Print();
				this.pageInfo = previewPrintController.GetPreviewPageInfo();
				this.document.PrintController = printController;
			}
			if (num != this.StartPage)
			{
				this.OnStartPageChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x001788A4 File Offset: 0x001778A4
		private void InvalidateLayout()
		{
			this.layoutOk = false;
			base.Invalidate();
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x001788B3 File Offset: 0x001778B3
		public void InvalidatePreview()
		{
			this.pageInfo = null;
			this.InvalidateLayout();
		}

		// Token: 0x06006688 RID: 26248 RVA: 0x001788C2 File Offset: 0x001778C2
		protected override void OnResize(EventArgs eventargs)
		{
			this.InvalidateLayout();
			base.OnResize(eventargs);
		}

		// Token: 0x06006689 RID: 26249 RVA: 0x001788D4 File Offset: 0x001778D4
		private void CalculatePageInfo()
		{
			if (this.pageInfoCalcPending)
			{
				return;
			}
			this.pageInfoCalcPending = true;
			try
			{
				if (this.pageInfo == null)
				{
					try
					{
						this.ComputePreview();
					}
					catch
					{
						this.exceptionPrinting = true;
						throw;
					}
					finally
					{
						base.Invalidate();
					}
				}
			}
			finally
			{
				this.pageInfoCalcPending = false;
			}
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x00178948 File Offset: 0x00177948
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Brush brush = new SolidBrush(this.BackColor);
			try
			{
				if (this.pageInfo == null || this.pageInfo.Length == 0)
				{
					pevent.Graphics.FillRectangle(brush, base.ClientRectangle);
					if (this.pageInfo != null || this.exceptionPrinting)
					{
						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = ControlPaint.TranslateAlignment(ContentAlignment.MiddleCenter);
						stringFormat.LineAlignment = ControlPaint.TranslateLineAlignment(ContentAlignment.MiddleCenter);
						SolidBrush solidBrush = new SolidBrush(this.ForeColor);
						try
						{
							if (this.exceptionPrinting)
							{
								pevent.Graphics.DrawString(SR.GetString("PrintPreviewExceptionPrinting"), this.Font, solidBrush, base.ClientRectangle, stringFormat);
							}
							else
							{
								pevent.Graphics.DrawString(SR.GetString("PrintPreviewNoPages"), this.Font, solidBrush, base.ClientRectangle, stringFormat);
							}
							goto IL_4CE;
						}
						finally
						{
							solidBrush.Dispose();
							stringFormat.Dispose();
						}
					}
					base.BeginInvoke(new MethodInvoker(this.CalculatePageInfo));
				}
				else
				{
					if (!this.layoutOk)
					{
						this.ComputeLayout();
					}
					Size size = new Size(PrintPreviewControl.PixelsToPhysical(new Point(base.Size), this.screendpi));
					Point point = new Point(this.VirtualSize);
					Point point2 = new Point(Math.Max(0, (base.Size.Width - point.X) / 2), Math.Max(0, (base.Size.Height - point.Y) / 2));
					point2.X -= this.Position.X;
					point2.Y -= this.Position.Y;
					this.lastOffset = point2;
					int num = PrintPreviewControl.PhysicalToPixels(10, this.screendpi.X);
					int num2 = PrintPreviewControl.PhysicalToPixels(10, this.screendpi.Y);
					Region clip = pevent.Graphics.Clip;
					Rectangle[] array = new Rectangle[this.rows * this.columns];
					Point empty = Point.Empty;
					int num3 = 0;
					try
					{
						for (int i = 0; i < this.rows; i++)
						{
							empty.X = 0;
							empty.Y = num3 * i;
							for (int j = 0; j < this.columns; j++)
							{
								int num4 = this.StartPage + j + i * this.columns;
								if (num4 < this.pageInfo.Length)
								{
									Size physicalSize = this.pageInfo[num4].PhysicalSize;
									if (this.autoZoom)
									{
										double val = ((double)size.Width - (double)(10 * (this.columns + 1))) / (double)(this.columns * physicalSize.Width);
										double val2 = ((double)size.Height - (double)(10 * (this.rows + 1))) / (double)(this.rows * physicalSize.Height);
										this.zoom = Math.Min(val, val2);
									}
									this.imageSize = new Size((int)(this.zoom * (double)physicalSize.Width), (int)(this.zoom * (double)physicalSize.Height));
									Point point3 = PrintPreviewControl.PhysicalToPixels(new Point(this.imageSize), this.screendpi);
									int x = point2.X + num * (j + 1) + empty.X;
									int y = point2.Y + num2 * (i + 1) + empty.Y;
									empty.X += point3.X;
									num3 = Math.Max(num3, point3.Y);
									array[num4 - this.StartPage] = new Rectangle(x, y, point3.X, point3.Y);
									pevent.Graphics.ExcludeClip(array[num4 - this.StartPage]);
								}
							}
						}
						pevent.Graphics.FillRectangle(brush, base.ClientRectangle);
					}
					finally
					{
						pevent.Graphics.Clip = clip;
					}
					for (int k = 0; k < array.Length; k++)
					{
						if (k + this.StartPage < this.pageInfo.Length)
						{
							Rectangle rect = array[k];
							pevent.Graphics.DrawRectangle(Pens.Black, rect);
							pevent.Graphics.FillRectangle(new SolidBrush(this.ForeColor), rect);
							rect.Inflate(-1, -1);
							if (this.pageInfo[k + this.StartPage].Image != null)
							{
								pevent.Graphics.DrawImage(this.pageInfo[k + this.StartPage].Image, rect);
							}
							rect.Width--;
							rect.Height--;
							pevent.Graphics.DrawRectangle(Pens.Black, rect);
						}
					}
				}
				IL_4CE:;
			}
			finally
			{
				brush.Dispose();
			}
			base.OnPaint(pevent);
		}

		// Token: 0x0600668B RID: 26251 RVA: 0x00178E80 File Offset: 0x00177E80
		protected virtual void OnStartPageChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[PrintPreviewControl.EVENT_STARTPAGECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600668C RID: 26252 RVA: 0x00178EAE File Offset: 0x00177EAE
		private static int PhysicalToPixels(int physicalSize, int dpi)
		{
			return (int)((double)(physicalSize * dpi) / 100.0);
		}

		// Token: 0x0600668D RID: 26253 RVA: 0x00178EBF File Offset: 0x00177EBF
		private static Point PhysicalToPixels(Point physical, Point dpi)
		{
			return new Point(PrintPreviewControl.PhysicalToPixels(physical.X, dpi.X), PrintPreviewControl.PhysicalToPixels(physical.Y, dpi.Y));
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x00178EEC File Offset: 0x00177EEC
		private static Size PhysicalToPixels(Size physicalSize, Point dpi)
		{
			return new Size(PrintPreviewControl.PhysicalToPixels(physicalSize.Width, dpi.X), PrintPreviewControl.PhysicalToPixels(physicalSize.Height, dpi.Y));
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x00178F19 File Offset: 0x00177F19
		private static int PixelsToPhysical(int pixels, int dpi)
		{
			return (int)((double)pixels * 100.0 / (double)dpi);
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x00178F2B File Offset: 0x00177F2B
		private static Point PixelsToPhysical(Point pixels, Point dpi)
		{
			return new Point(PrintPreviewControl.PixelsToPhysical(pixels.X, dpi.X), PrintPreviewControl.PixelsToPhysical(pixels.Y, dpi.Y));
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x00178F58 File Offset: 0x00177F58
		private static Size PixelsToPhysical(Size pixels, Point dpi)
		{
			return new Size(PrintPreviewControl.PixelsToPhysical(pixels.Width, dpi.X), PrintPreviewControl.PixelsToPhysical(pixels.Height, dpi.Y));
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x00178F85 File Offset: 0x00177F85
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetBackColor()
		{
			this.BackColor = SystemColors.AppWorkspace;
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x00178F92 File Offset: 0x00177F92
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetForeColor()
		{
			this.ForeColor = Color.White;
		}

		// Token: 0x06006694 RID: 26260 RVA: 0x00178FA0 File Offset: 0x00177FA0
		private void WmHScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Point point = this.position;
			int x = point.X;
			int maxPos = Math.Max(base.Width, this.virtualSize.Width);
			point.X = this.AdjustScroll(m, x, maxPos, true);
			this.Position = point;
		}

		// Token: 0x06006695 RID: 26261 RVA: 0x0017900C File Offset: 0x0017800C
		private void SetPositionNoInvalidate(Point value)
		{
			Point point = this.position;
			this.position = value;
			this.position.X = Math.Min(this.position.X, this.virtualSize.Width - base.Width);
			this.position.Y = Math.Min(this.position.Y, this.virtualSize.Height - base.Height);
			if (this.position.X < 0)
			{
				this.position.X = 0;
			}
			if (this.position.Y < 0)
			{
				this.position.Y = 0;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
			SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), point.X - this.position.X, point.Y - this.position.Y, ref rect, ref rect);
			UnsafeNativeMethods.SetScrollPos(new HandleRef(this, base.Handle), 0, this.position.X, true);
			UnsafeNativeMethods.SetScrollPos(new HandleRef(this, base.Handle), 1, this.position.Y, true);
		}

		// Token: 0x06006696 RID: 26262 RVA: 0x00179158 File Offset: 0x00178158
		internal void SetVirtualSizeNoInvalidate(Size value)
		{
			this.virtualSize = value;
			this.SetPositionNoInvalidate(this.position);
			NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
			scrollinfo.fMask = 3;
			scrollinfo.nMin = 0;
			scrollinfo.nMax = Math.Max(base.Height, this.virtualSize.Height) - 1;
			scrollinfo.nPage = base.Height;
			UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 1, scrollinfo, true);
			scrollinfo.fMask = 3;
			scrollinfo.nMin = 0;
			scrollinfo.nMax = Math.Max(base.Width, this.virtualSize.Width) - 1;
			scrollinfo.nPage = base.Width;
			UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 0, scrollinfo, true);
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x00179218 File Offset: 0x00178218
		private void WmVScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Point point = this.Position;
			int y = point.Y;
			int maxPos = Math.Max(base.Height, this.virtualSize.Height);
			point.Y = this.AdjustScroll(m, y, maxPos, false);
			this.Position = point;
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x00179284 File Offset: 0x00178284
		private void WmKeyDown(ref Message msg)
		{
			Keys keys = (Keys)((int)msg.WParam | (int)Control.ModifierKeys);
			Point point = this.Position;
			switch (keys & Keys.KeyCode)
			{
			case Keys.Prior:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					int num = point.X;
					if (num > 100)
					{
						num -= 100;
					}
					else
					{
						num = 0;
					}
					point.X = num;
					this.Position = point;
					return;
				}
				if (this.StartPage > 0)
				{
					this.StartPage--;
					return;
				}
				break;
			case Keys.Next:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					int num = point.X;
					int num2 = Math.Max(base.Width, this.virtualSize.Width);
					if (num < num2 - 100)
					{
						num += 100;
					}
					else
					{
						num = num2;
					}
					point.X = num;
					this.Position = point;
					return;
				}
				if (this.StartPage < this.pageInfo.Length)
				{
					this.StartPage++;
					return;
				}
				break;
			case Keys.End:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					this.StartPage = this.pageInfo.Length;
					return;
				}
				break;
			case Keys.Home:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					this.StartPage = 0;
					return;
				}
				break;
			case Keys.Left:
			{
				int num = point.X;
				if (num > 5)
				{
					num -= 5;
				}
				else
				{
					num = 0;
				}
				point.X = num;
				this.Position = point;
				return;
			}
			case Keys.Up:
			{
				int num = point.Y;
				if (num > 5)
				{
					num -= 5;
				}
				else
				{
					num = 0;
				}
				point.Y = num;
				this.Position = point;
				return;
			}
			case Keys.Right:
			{
				int num = point.X;
				int num2 = Math.Max(base.Width, this.virtualSize.Width);
				if (num < num2 - 5)
				{
					num += 5;
				}
				else
				{
					num = num2;
				}
				point.X = num;
				this.Position = point;
				break;
			}
			case Keys.Down:
			{
				int num = point.Y;
				int num2 = Math.Max(base.Height, this.virtualSize.Height);
				if (num < num2 - 5)
				{
					num += 5;
				}
				else
				{
					num = num2;
				}
				point.Y = num;
				this.Position = point;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x0017949C File Offset: 0x0017849C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 256)
			{
				this.WmKeyDown(ref m);
				return;
			}
			switch (msg)
			{
			case 276:
				this.WmHScroll(ref m);
				return;
			case 277:
				this.WmVScroll(ref m);
				return;
			default:
				base.WndProc(ref m);
				return;
			}
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x001794F0 File Offset: 0x001784F0
		internal override bool ShouldSerializeBackColor()
		{
			return !this.BackColor.Equals(SystemColors.AppWorkspace);
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x00179520 File Offset: 0x00178520
		internal override bool ShouldSerializeForeColor()
		{
			return !this.ForeColor.Equals(Color.White);
		}

		// Token: 0x04003CE5 RID: 15589
		private const int SCROLL_PAGE = 100;

		// Token: 0x04003CE6 RID: 15590
		private const int SCROLL_LINE = 5;

		// Token: 0x04003CE7 RID: 15591
		private const double DefaultZoom = 0.3;

		// Token: 0x04003CE8 RID: 15592
		private const int border = 10;

		// Token: 0x04003CE9 RID: 15593
		private Size virtualSize = new Size(1, 1);

		// Token: 0x04003CEA RID: 15594
		private Point position = new Point(0, 0);

		// Token: 0x04003CEB RID: 15595
		private Point lastOffset;

		// Token: 0x04003CEC RID: 15596
		private bool antiAlias;

		// Token: 0x04003CED RID: 15597
		private PrintDocument document;

		// Token: 0x04003CEE RID: 15598
		private PreviewPageInfo[] pageInfo;

		// Token: 0x04003CEF RID: 15599
		private int startPage;

		// Token: 0x04003CF0 RID: 15600
		private int rows = 1;

		// Token: 0x04003CF1 RID: 15601
		private int columns = 1;

		// Token: 0x04003CF2 RID: 15602
		private bool autoZoom = true;

		// Token: 0x04003CF3 RID: 15603
		private bool layoutOk;

		// Token: 0x04003CF4 RID: 15604
		private Size imageSize = Size.Empty;

		// Token: 0x04003CF5 RID: 15605
		private Point screendpi = Point.Empty;

		// Token: 0x04003CF6 RID: 15606
		private double zoom = 0.3;

		// Token: 0x04003CF7 RID: 15607
		private bool pageInfoCalcPending;

		// Token: 0x04003CF8 RID: 15608
		private bool exceptionPrinting;

		// Token: 0x04003CF9 RID: 15609
		private static readonly object EVENT_STARTPAGECHANGED = new object();
	}
}
