using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200063D RID: 1597
	[DefaultProperty("TabPages")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SelectedIndexChanged")]
	[SRDescription("DescriptionTabControl")]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.TabControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class TabControl : Control
	{
		// Token: 0x060053BD RID: 21437 RVA: 0x00131AFC File Offset: 0x00130AFC
		public TabControl()
		{
			this.tabControlState = new BitVector32(0);
			this.tabCollection = new TabControl.TabPageCollection(this);
			base.SetStyle(ControlStyles.UserPaint, false);
		}

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x060053BE RID: 21438 RVA: 0x00131B87 File Offset: 0x00130B87
		// (set) Token: 0x060053BF RID: 21439 RVA: 0x00131B90 File Offset: 0x00130B90
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(TabAlignment.Top)]
		[SRCategory("CatBehavior")]
		[SRDescription("TabBaseAlignmentDescr")]
		public TabAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (this.alignment != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(TabAlignment));
					}
					this.alignment = value;
					if (this.alignment == TabAlignment.Left || this.alignment == TabAlignment.Right)
					{
						this.Multiline = true;
					}
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x060053C0 RID: 21440 RVA: 0x00131BF2 File Offset: 0x00130BF2
		// (set) Token: 0x060053C1 RID: 21441 RVA: 0x00131C10 File Offset: 0x00130C10
		[DefaultValue(TabAppearance.Normal)]
		[SRCategory("CatBehavior")]
		[SRDescription("TabBaseAppearanceDescr")]
		[Localizable(true)]
		public TabAppearance Appearance
		{
			get
			{
				if (this.appearance == TabAppearance.FlatButtons && this.alignment != TabAlignment.Top)
				{
					return TabAppearance.Buttons;
				}
				return this.appearance;
			}
			set
			{
				if (this.appearance != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(TabAppearance));
					}
					this.appearance = value;
					base.RecreateHandle();
					this.OnStyleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x00131C64 File Offset: 0x00130C64
		// (set) Token: 0x060053C3 RID: 21443 RVA: 0x00131C6B File Offset: 0x00130C6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return SystemColors.Control;
			}
			set
			{
			}
		}

		// Token: 0x14000316 RID: 790
		// (add) Token: 0x060053C4 RID: 21444 RVA: 0x00131C6D File Offset: 0x00130C6D
		// (remove) Token: 0x060053C5 RID: 21445 RVA: 0x00131C76 File Offset: 0x00130C76
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

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x060053C6 RID: 21446 RVA: 0x00131C7F File Offset: 0x00130C7F
		// (set) Token: 0x060053C7 RID: 21447 RVA: 0x00131C87 File Offset: 0x00130C87
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000317 RID: 791
		// (add) Token: 0x060053C8 RID: 21448 RVA: 0x00131C90 File Offset: 0x00130C90
		// (remove) Token: 0x060053C9 RID: 21449 RVA: 0x00131C99 File Offset: 0x00130C99
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

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x00131CA2 File Offset: 0x00130CA2
		// (set) Token: 0x060053CB RID: 21451 RVA: 0x00131CAA File Offset: 0x00130CAA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000318 RID: 792
		// (add) Token: 0x060053CC RID: 21452 RVA: 0x00131CB3 File Offset: 0x00130CB3
		// (remove) Token: 0x060053CD RID: 21453 RVA: 0x00131CBC File Offset: 0x00130CBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x060053CE RID: 21454 RVA: 0x00131CC5 File Offset: 0x00130CC5
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x060053CF RID: 21455 RVA: 0x00131CD3 File Offset: 0x00130CD3
		// (set) Token: 0x060053D0 RID: 21456 RVA: 0x00131CDB File Offset: 0x00130CDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x00131CE4 File Offset: 0x00130CE4
		// (set) Token: 0x060053D2 RID: 21458 RVA: 0x00131CEC File Offset: 0x00130CEC
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

		// Token: 0x14000319 RID: 793
		// (add) Token: 0x060053D3 RID: 21459 RVA: 0x00131CF5 File Offset: 0x00130CF5
		// (remove) Token: 0x060053D4 RID: 21460 RVA: 0x00131CFE File Offset: 0x00130CFE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x060053D5 RID: 21461 RVA: 0x00131D08 File Offset: 0x00130D08
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysTabControl32";
				if (this.Multiline)
				{
					createParams.Style |= 512;
				}
				if (this.drawMode == TabDrawMode.OwnerDrawFixed)
				{
					createParams.Style |= 8192;
				}
				if (this.ShowToolTips && !base.DesignMode)
				{
					createParams.Style |= 16384;
				}
				if (this.alignment == TabAlignment.Bottom || this.alignment == TabAlignment.Right)
				{
					createParams.Style |= 2;
				}
				if (this.alignment == TabAlignment.Left || this.alignment == TabAlignment.Right)
				{
					createParams.Style |= 640;
				}
				if (this.tabControlState[1])
				{
					createParams.Style |= 64;
				}
				if (this.appearance == TabAppearance.Normal)
				{
					CreateParams createParams2 = createParams;
					createParams2.Style = createParams2.Style;
				}
				else
				{
					createParams.Style |= 256;
					if (this.appearance == TabAppearance.FlatButtons && this.alignment == TabAlignment.Top)
					{
						createParams.Style |= 8;
					}
				}
				switch (this.sizeMode)
				{
				case TabSizeMode.Normal:
					createParams.Style |= 2048;
					break;
				case TabSizeMode.FillToRight:
				{
					CreateParams createParams3 = createParams;
					createParams3.Style = createParams3.Style;
					break;
				}
				case TabSizeMode.Fixed:
					createParams.Style |= 1024;
					break;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x060053D6 RID: 21462 RVA: 0x00131EAC File Offset: 0x00130EAC
		public override Rectangle DisplayRectangle
		{
			get
			{
				if (!this.cachedDisplayRect.IsEmpty)
				{
					return this.cachedDisplayRect;
				}
				Rectangle bounds = base.Bounds;
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(bounds.X, bounds.Y, bounds.Width, bounds.Height);
				if (!base.IsDisposed)
				{
					if (!base.IsActiveX && !base.IsHandleCreated)
					{
						this.CreateHandle();
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4904, 0, ref rect);
					}
				}
				Rectangle result = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
				Point location = base.Location;
				result.X -= location.X;
				result.Y -= location.Y;
				this.cachedDisplayRect = result;
				return result;
			}
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x060053D7 RID: 21463 RVA: 0x00131F86 File Offset: 0x00130F86
		// (set) Token: 0x060053D8 RID: 21464 RVA: 0x00131F8E File Offset: 0x00130F8E
		[SRCategory("CatBehavior")]
		[DefaultValue(TabDrawMode.Normal)]
		[SRDescription("TabBaseDrawModeDescr")]
		public TabDrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TabDrawMode));
				}
				if (this.drawMode != value)
				{
					this.drawMode = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x060053D9 RID: 21465 RVA: 0x00131FCC File Offset: 0x00130FCC
		// (set) Token: 0x060053DA RID: 21466 RVA: 0x00131FDA File Offset: 0x00130FDA
		[SRDescription("TabBaseHotTrackDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool HotTrack
		{
			get
			{
				return this.tabControlState[1];
			}
			set
			{
				if (this.HotTrack != value)
				{
					this.tabControlState[1] = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x060053DB RID: 21467 RVA: 0x00132000 File Offset: 0x00131000
		// (set) Token: 0x060053DC RID: 21468 RVA: 0x00132008 File Offset: 0x00131008
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TabBaseImageListDescr")]
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (this.imageList != value)
				{
					EventHandler value2 = new EventHandler(this.ImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					if (this.imageList != null)
					{
						this.imageList.RecreateHandle -= value2;
						this.imageList.Disposed -= value3;
					}
					this.imageList = value;
					IntPtr lparam = (value != null) ? value.Handle : IntPtr.Zero;
					if (base.IsHandleCreated)
					{
						base.SendMessage(4867, IntPtr.Zero, lparam);
					}
					foreach (object obj in this.TabPages)
					{
						TabPage tabPage = (TabPage)obj;
						tabPage.ImageIndexer.ImageList = value;
					}
					if (value != null)
					{
						value.RecreateHandle += value2;
						value.Disposed += value3;
					}
				}
			}
		}

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x001320F8 File Offset: 0x001310F8
		// (set) Token: 0x060053DE RID: 21470 RVA: 0x00132144 File Offset: 0x00131144
		[SRDescription("TabBaseItemSizeDescr")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		public Size ItemSize
		{
			get
			{
				if (!this.itemSize.IsEmpty)
				{
					return this.itemSize;
				}
				if (base.IsHandleCreated)
				{
					this.tabControlState[8] = true;
					return this.GetTabRect(0).Size;
				}
				return TabControl.DEFAULT_ITEMSIZE;
			}
			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					throw new ArgumentOutOfRangeException("ItemSize", SR.GetString("InvalidArgument", new object[]
					{
						"ItemSize",
						value.ToString()
					}));
				}
				this.itemSize = value;
				this.ApplyItemSize();
				this.UpdateSize();
				base.Invalidate();
			}
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x060053DF RID: 21471 RVA: 0x001321B3 File Offset: 0x001311B3
		// (set) Token: 0x060053E0 RID: 21472 RVA: 0x001321C5 File Offset: 0x001311C5
		private bool InsertingItem
		{
			get
			{
				return this.tabControlState[128];
			}
			set
			{
				this.tabControlState[128] = value;
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x060053E1 RID: 21473 RVA: 0x001321D8 File Offset: 0x001311D8
		// (set) Token: 0x060053E2 RID: 21474 RVA: 0x001321E6 File Offset: 0x001311E6
		[DefaultValue(false)]
		[SRDescription("TabBaseMultilineDescr")]
		[SRCategory("CatBehavior")]
		public bool Multiline
		{
			get
			{
				return this.tabControlState[2];
			}
			set
			{
				if (this.Multiline != value)
				{
					this.tabControlState[2] = value;
					if (!this.Multiline && (this.alignment == TabAlignment.Left || this.alignment == TabAlignment.Right))
					{
						this.alignment = TabAlignment.Top;
					}
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060053E3 RID: 21475 RVA: 0x00132225 File Offset: 0x00131225
		// (set) Token: 0x060053E4 RID: 21476 RVA: 0x00132230 File Offset: 0x00131230
		[SRDescription("TabBasePaddingDescr")]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		public new Point Padding
		{
			get
			{
				return this.padding;
			}
			set
			{
				if (value.X < 0 || value.Y < 0)
				{
					throw new ArgumentOutOfRangeException("Padding", SR.GetString("InvalidArgument", new object[]
					{
						"Padding",
						value.ToString()
					}));
				}
				if (this.padding != value)
				{
					this.padding = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060053E5 RID: 21477 RVA: 0x001322A9 File Offset: 0x001312A9
		// (set) Token: 0x060053E6 RID: 21478 RVA: 0x001322B4 File Offset: 0x001312B4
		[Localizable(true)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x060053E7 RID: 21479 RVA: 0x00132308 File Offset: 0x00131308
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TabBaseRowCountDescr")]
		[Browsable(false)]
		public int RowCount
		{
			get
			{
				return (int)base.SendMessage(4908, 0, 0);
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x060053E8 RID: 21480 RVA: 0x0013232C File Offset: 0x0013132C
		// (set) Token: 0x060053E9 RID: 21481 RVA: 0x0013235C File Offset: 0x0013135C
		[SRDescription("selectedIndexDescr")]
		[DefaultValue(-1)]
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		public int SelectedIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(4875, 0, 0);
				}
				return this.selectedIndex;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SelectedIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.SelectedIndex != value)
				{
					if (base.IsHandleCreated)
					{
						if (!this.tabControlState[16] && !this.tabControlState[64])
						{
							this.tabControlState[32] = true;
							if (this.WmSelChanging())
							{
								this.tabControlState[32] = false;
								return;
							}
							if (base.ValidationCancelled)
							{
								this.tabControlState[32] = false;
								return;
							}
						}
						base.SendMessage(4876, value, 0);
						if (!this.tabControlState[16] && !this.tabControlState[64])
						{
							this.tabControlState[64] = true;
							if (this.WmSelChange())
							{
								this.tabControlState[32] = false;
								this.tabControlState[64] = false;
								return;
							}
							this.tabControlState[64] = false;
							return;
						}
					}
					else
					{
						this.selectedIndex = value;
					}
				}
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x060053EA RID: 21482 RVA: 0x00132498 File Offset: 0x00131498
		// (set) Token: 0x060053EB RID: 21483 RVA: 0x001324A0 File Offset: 0x001314A0
		[SRDescription("TabControlSelectedTabDescr")]
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabPage SelectedTab
		{
			get
			{
				return this.SelectedTabInternal;
			}
			set
			{
				this.SelectedTabInternal = value;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x001324AC File Offset: 0x001314AC
		// (set) Token: 0x060053ED RID: 21485 RVA: 0x001324D0 File Offset: 0x001314D0
		internal TabPage SelectedTabInternal
		{
			get
			{
				int num = this.SelectedIndex;
				if (num == -1)
				{
					return null;
				}
				return this.tabPages[num];
			}
			set
			{
				int num = this.FindTabPage(value);
				this.SelectedIndex = num;
			}
		}

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x060053EE RID: 21486 RVA: 0x001324EC File Offset: 0x001314EC
		// (set) Token: 0x060053EF RID: 21487 RVA: 0x001324F4 File Offset: 0x001314F4
		[DefaultValue(TabSizeMode.Normal)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TabBaseSizeModeDescr")]
		public TabSizeMode SizeMode
		{
			get
			{
				return this.sizeMode;
			}
			set
			{
				if (this.sizeMode == value)
				{
					return;
				}
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TabSizeMode));
				}
				this.sizeMode = value;
				base.RecreateHandle();
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x060053F0 RID: 21488 RVA: 0x00132533 File Offset: 0x00131533
		// (set) Token: 0x060053F1 RID: 21489 RVA: 0x00132541 File Offset: 0x00131541
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("TabBaseShowToolTipsDescr")]
		public bool ShowToolTips
		{
			get
			{
				return this.tabControlState[4];
			}
			set
			{
				if (this.ShowToolTips != value)
				{
					this.tabControlState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x060053F2 RID: 21490 RVA: 0x0013255F File Offset: 0x0013155F
		[Browsable(false)]
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TabBaseTabCountDescr")]
		public int TabCount
		{
			get
			{
				return this.tabPageCount;
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x060053F3 RID: 21491 RVA: 0x00132567 File Offset: 0x00131567
		[Editor("System.Windows.Forms.Design.TabPageCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("TabControlTabsDescr")]
		[MergableProperty(false)]
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabControl.TabPageCollection TabPages
		{
			get
			{
				return this.tabCollection;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x060053F4 RID: 21492 RVA: 0x0013256F File Offset: 0x0013156F
		// (set) Token: 0x060053F5 RID: 21493 RVA: 0x00132577 File Offset: 0x00131577
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
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

		// Token: 0x1400031A RID: 794
		// (add) Token: 0x060053F6 RID: 21494 RVA: 0x00132580 File Offset: 0x00131580
		// (remove) Token: 0x060053F7 RID: 21495 RVA: 0x00132589 File Offset: 0x00131589
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

		// Token: 0x1400031B RID: 795
		// (add) Token: 0x060053F8 RID: 21496 RVA: 0x00132592 File Offset: 0x00131592
		// (remove) Token: 0x060053F9 RID: 21497 RVA: 0x001325AB File Offset: 0x001315AB
		[SRDescription("drawItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				this.onDrawItem = (DrawItemEventHandler)Delegate.Combine(this.onDrawItem, value);
			}
			remove
			{
				this.onDrawItem = (DrawItemEventHandler)Delegate.Remove(this.onDrawItem, value);
			}
		}

		// Token: 0x1400031C RID: 796
		// (add) Token: 0x060053FA RID: 21498 RVA: 0x001325C4 File Offset: 0x001315C4
		// (remove) Token: 0x060053FB RID: 21499 RVA: 0x001325D7 File Offset: 0x001315D7
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		// Token: 0x1400031D RID: 797
		// (add) Token: 0x060053FC RID: 21500 RVA: 0x001325EA File Offset: 0x001315EA
		// (remove) Token: 0x060053FD RID: 21501 RVA: 0x00132603 File Offset: 0x00131603
		[SRCategory("CatBehavior")]
		[SRDescription("selectedIndexChangedEventDescr")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				this.onSelectedIndexChanged = (EventHandler)Delegate.Combine(this.onSelectedIndexChanged, value);
			}
			remove
			{
				this.onSelectedIndexChanged = (EventHandler)Delegate.Remove(this.onSelectedIndexChanged, value);
			}
		}

		// Token: 0x1400031E RID: 798
		// (add) Token: 0x060053FE RID: 21502 RVA: 0x0013261C File Offset: 0x0013161C
		// (remove) Token: 0x060053FF RID: 21503 RVA: 0x0013262F File Offset: 0x0013162F
		[SRDescription("TabControlSelectingEventDescr")]
		[SRCategory("CatAction")]
		public event TabControlCancelEventHandler Selecting
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_SELECTING, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_SELECTING, value);
			}
		}

		// Token: 0x1400031F RID: 799
		// (add) Token: 0x06005400 RID: 21504 RVA: 0x00132642 File Offset: 0x00131642
		// (remove) Token: 0x06005401 RID: 21505 RVA: 0x00132655 File Offset: 0x00131655
		[SRDescription("TabControlSelectedEventDescr")]
		[SRCategory("CatAction")]
		public event TabControlEventHandler Selected
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_SELECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_SELECTED, value);
			}
		}

		// Token: 0x14000320 RID: 800
		// (add) Token: 0x06005402 RID: 21506 RVA: 0x00132668 File Offset: 0x00131668
		// (remove) Token: 0x06005403 RID: 21507 RVA: 0x0013267B File Offset: 0x0013167B
		[SRCategory("CatAction")]
		[SRDescription("TabControlDeselectingEventDescr")]
		public event TabControlCancelEventHandler Deselecting
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_DESELECTING, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_DESELECTING, value);
			}
		}

		// Token: 0x14000321 RID: 801
		// (add) Token: 0x06005404 RID: 21508 RVA: 0x0013268E File Offset: 0x0013168E
		// (remove) Token: 0x06005405 RID: 21509 RVA: 0x001326A1 File Offset: 0x001316A1
		[SRDescription("TabControlDeselectedEventDescr")]
		[SRCategory("CatAction")]
		public event TabControlEventHandler Deselected
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_DESELECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_DESELECTED, value);
			}
		}

		// Token: 0x14000322 RID: 802
		// (add) Token: 0x06005406 RID: 21510 RVA: 0x001326B4 File Offset: 0x001316B4
		// (remove) Token: 0x06005407 RID: 21511 RVA: 0x001326BD File Offset: 0x001316BD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x001326C8 File Offset: 0x001316C8
		internal int AddTabPage(TabPage tabPage, NativeMethods.TCITEM_T tcitem)
		{
			int num = this.AddNativeTabPage(tcitem);
			if (num >= 0)
			{
				this.Insert(num, tabPage);
			}
			return num;
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x001326EC File Offset: 0x001316EC
		internal int AddNativeTabPage(NativeMethods.TCITEM_T tcitem)
		{
			int result = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TCM_INSERTITEM, this.tabPageCount + 1, tcitem);
			UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), this.tabBaseReLayoutMessage, IntPtr.Zero, IntPtr.Zero);
			return result;
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x00132744 File Offset: 0x00131744
		internal void ApplyItemSize()
		{
			if (base.IsHandleCreated && this.ShouldSerializeItemSize())
			{
				base.SendMessage(4905, 0, (int)NativeMethods.Util.MAKELPARAM(this.itemSize.Width, this.itemSize.Height));
			}
			this.cachedDisplayRect = Rectangle.Empty;
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x00132799 File Offset: 0x00131799
		internal void BeginUpdate()
		{
			base.BeginUpdateInternal();
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x001327A1 File Offset: 0x001317A1
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new TabControl.ControlCollection(this);
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x001327AC File Offset: 0x001317AC
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 8
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x001327FC File Offset: 0x001317FC
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x00132808 File Offset: 0x00131808
		public void DeselectTab(int index)
		{
			TabPage tabPage = this.GetTabPage(index);
			if (this.SelectedTab == tabPage)
			{
				if (0 <= index && index < this.TabPages.Count - 1)
				{
					this.SelectedTab = this.GetTabPage(++index);
					return;
				}
				this.SelectedTab = this.GetTabPage(0);
			}
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x0013285C File Offset: 0x0013185C
		public void DeselectTab(TabPage tabPage)
		{
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}
			int index = this.FindTabPage(tabPage);
			this.DeselectTab(index);
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x00132888 File Offset: 0x00131888
		public void DeselectTab(string tabPageName)
		{
			if (tabPageName == null)
			{
				throw new ArgumentNullException("tabPageName");
			}
			TabPage tabPage = this.TabPages[tabPageName];
			this.DeselectTab(tabPage);
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x001328B7 File Offset: 0x001318B7
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.imageList != null)
			{
				this.imageList.Disposed -= this.DetachImageList;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x001328E2 File Offset: 0x001318E2
		internal void EndUpdate()
		{
			this.EndUpdate(true);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x001328EB File Offset: 0x001318EB
		internal void EndUpdate(bool invalidate)
		{
			base.EndUpdateInternal(invalidate);
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x001328F8 File Offset: 0x001318F8
		internal int FindTabPage(TabPage tabPage)
		{
			if (this.tabPages != null)
			{
				for (int i = 0; i < this.tabPageCount; i++)
				{
					if (this.tabPages[i].Equals(tabPage))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x00132931 File Offset: 0x00131931
		public Control GetControl(int index)
		{
			return this.GetTabPage(index);
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x0013293C File Offset: 0x0013193C
		internal TabPage GetTabPage(int index)
		{
			if (index < 0 || index >= this.tabPageCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return this.tabPages[index];
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x00132994 File Offset: 0x00131994
		protected virtual object[] GetItems()
		{
			TabPage[] array = new TabPage[this.tabPageCount];
			if (this.tabPageCount > 0)
			{
				Array.Copy(this.tabPages, 0, array, 0, this.tabPageCount);
			}
			return array;
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x001329CC File Offset: 0x001319CC
		protected virtual object[] GetItems(Type baseType)
		{
			object[] array = (object[])Array.CreateInstance(baseType, this.tabPageCount);
			if (this.tabPageCount > 0)
			{
				Array.Copy(this.tabPages, 0, array, 0, this.tabPageCount);
			}
			return array;
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x00132A09 File Offset: 0x00131A09
		internal TabPage[] GetTabPages()
		{
			return (TabPage[])this.GetItems();
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x00132A18 File Offset: 0x00131A18
		public Rectangle GetTabRect(int index)
		{
			if (index < 0 || (index >= this.tabPageCount && !this.tabControlState[8]))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.tabControlState[8] = false;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			base.SendMessage(4874, index, ref rect);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x00132AC7 File Offset: 0x00131AC7
		protected string GetToolTipText(object item)
		{
			return ((TabPage)item).ToolTipText;
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x00132AD4 File Offset: 0x00131AD4
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4867, 0, this.ImageList.Handle);
			}
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x00132AF8 File Offset: 0x00131AF8
		internal void Insert(int index, TabPage tabPage)
		{
			if (this.tabPages == null)
			{
				this.tabPages = new TabPage[4];
			}
			else if (this.tabPages.Length == this.tabPageCount)
			{
				TabPage[] destinationArray = new TabPage[this.tabPageCount * 2];
				Array.Copy(this.tabPages, 0, destinationArray, 0, this.tabPageCount);
				this.tabPages = destinationArray;
			}
			if (index < this.tabPageCount)
			{
				Array.Copy(this.tabPages, index, this.tabPages, index + 1, this.tabPageCount - index);
			}
			this.tabPages[index] = tabPage;
			this.tabPageCount++;
			this.cachedDisplayRect = Rectangle.Empty;
			this.ApplyItemSize();
			if (this.Appearance == TabAppearance.FlatButtons)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x00132BB4 File Offset: 0x00131BB4
		private void InsertItem(int index, TabPage tabPage)
		{
			if (index < 0 || (this.tabPages != null && index > this.tabPageCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.TCITEM_T tcitem = tabPage.GetTCITEM();
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TCM_INSERTITEM, index, tcitem);
				if (num >= 0)
				{
					this.Insert(num, tabPage);
				}
			}
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x00132C54 File Offset: 0x00131C54
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.End:
			case Keys.Home:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x00132CA0 File Offset: 0x00131CA0
		protected override void OnHandleCreated(EventArgs e)
		{
			NativeWindow.AddWindowToIDTable(this, base.Handle);
			this.handleInTable = true;
			if (!this.padding.IsEmpty)
			{
				base.SendMessage(4907, 0, NativeMethods.Util.MAKELPARAM(this.padding.X, this.padding.Y));
			}
			base.OnHandleCreated(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.ApplyItemSize();
			if (this.imageList != null)
			{
				base.SendMessage(4867, 0, this.imageList.Handle);
			}
			if (this.ShowToolTips)
			{
				IntPtr intPtr = base.SendMessage(4909, 0, 0);
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SetWindowPos(new HandleRef(this, intPtr), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				}
			}
			foreach (object obj in this.TabPages)
			{
				TabPage tabPage = (TabPage)obj;
				this.AddNativeTabPage(tabPage.GetTCITEM());
			}
			this.ResizePages();
			if (this.selectedIndex != -1)
			{
				try
				{
					this.tabControlState[16] = true;
					this.SelectedIndex = this.selectedIndex;
				}
				finally
				{
					this.tabControlState[16] = false;
				}
				this.selectedIndex = -1;
			}
			this.UpdateTabSelection(false);
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x00132E10 File Offset: 0x00131E10
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!base.Disposing)
			{
				this.selectedIndex = this.SelectedIndex;
			}
			if (this.handleInTable)
			{
				this.handleInTable = false;
				NativeWindow.RemoveWindowFromIDTable(base.Handle);
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x00132E47 File Offset: 0x00131E47
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			if (this.onDrawItem != null)
			{
				this.onDrawItem(this, e);
			}
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x00132E5E File Offset: 0x00131E5E
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireEnter(e);
			}
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x00132E7B File Offset: 0x00131E7B
		protected override void OnLeave(EventArgs e)
		{
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireLeave(e);
			}
			base.OnLeave(e);
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x00132E98 File Offset: 0x00131E98
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			if (ke.KeyCode == Keys.Tab && (ke.KeyData & Keys.Control) != Keys.None)
			{
				bool forward = (ke.KeyData & Keys.Shift) == Keys.None;
				this.SelectNextTab(ke, forward);
			}
			if (ke.KeyCode == Keys.Next && (ke.KeyData & Keys.Control) != Keys.None)
			{
				this.SelectNextTab(ke, true);
			}
			if (ke.KeyCode == Keys.Prior && (ke.KeyData & Keys.Control) != Keys.None)
			{
				this.SelectNextTab(ke, false);
			}
			base.OnKeyDown(ke);
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x00132F1C File Offset: 0x00131F1C
		internal override void OnParentHandleRecreated()
		{
			this.skipUpdateSize = true;
			try
			{
				base.OnParentHandleRecreated();
			}
			finally
			{
				this.skipUpdateSize = false;
			}
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x00132F50 File Offset: 0x00131F50
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x00132F6C File Offset: 0x00131F6C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			EventHandler eventHandler = base.Events[TabControl.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x00132FB4 File Offset: 0x00131FB4
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			int num = this.SelectedIndex;
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(this.tabControlState[32]);
			this.tabControlState[32] = false;
			if (this.onSelectedIndexChanged != null)
			{
				this.onSelectedIndexChanged(this, e);
			}
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x0013300C File Offset: 0x0013200C
		protected virtual void OnSelecting(TabControlCancelEventArgs e)
		{
			TabControlCancelEventHandler tabControlCancelEventHandler = (TabControlCancelEventHandler)base.Events[TabControl.EVENT_SELECTING];
			if (tabControlCancelEventHandler != null)
			{
				tabControlCancelEventHandler(this, e);
			}
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x0013303C File Offset: 0x0013203C
		protected virtual void OnSelected(TabControlEventArgs e)
		{
			TabControlEventHandler tabControlEventHandler = (TabControlEventHandler)base.Events[TabControl.EVENT_SELECTED];
			if (tabControlEventHandler != null)
			{
				tabControlEventHandler(this, e);
			}
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireEnter(EventArgs.Empty);
			}
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x00133084 File Offset: 0x00132084
		protected virtual void OnDeselecting(TabControlCancelEventArgs e)
		{
			TabControlCancelEventHandler tabControlCancelEventHandler = (TabControlCancelEventHandler)base.Events[TabControl.EVENT_DESELECTING];
			if (tabControlCancelEventHandler != null)
			{
				tabControlCancelEventHandler(this, e);
			}
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x001330B4 File Offset: 0x001320B4
		protected virtual void OnDeselected(TabControlEventArgs e)
		{
			TabControlEventHandler tabControlEventHandler = (TabControlEventHandler)base.Events[TabControl.EVENT_DESELECTED];
			if (tabControlEventHandler != null)
			{
				tabControlEventHandler(this, e);
			}
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireLeave(EventArgs.Empty);
			}
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x001330FA File Offset: 0x001320FA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			return this.ProcessKeyEventArgs(ref m) || base.ProcessKeyPreview(ref m);
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x00133110 File Offset: 0x00132110
		internal void UpdateSize()
		{
			if (this.skipUpdateSize)
			{
				return;
			}
			this.BeginUpdate();
			Size size = base.Size;
			base.Size = new Size(size.Width + 1, size.Height);
			base.Size = size;
			this.EndUpdate();
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x0013315B File Offset: 0x0013215B
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateSize();
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x00133178 File Offset: 0x00132178
		internal override void RecreateHandleCore()
		{
			TabPage[] array = this.GetTabPages();
			int num = (array.Length > 0 && this.SelectedIndex == -1) ? 0 : this.SelectedIndex;
			if (base.IsHandleCreated)
			{
				base.SendMessage(4873, 0, 0);
			}
			this.tabPages = null;
			this.tabPageCount = 0;
			base.RecreateHandleCore();
			for (int i = 0; i < array.Length; i++)
			{
				this.TabPages.Add(array[i]);
			}
			try
			{
				this.tabControlState[16] = true;
				this.SelectedIndex = num;
			}
			finally
			{
				this.tabControlState[16] = false;
			}
			this.UpdateSize();
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x00133228 File Offset: 0x00132228
		protected void RemoveAll()
		{
			base.Controls.Clear();
			base.SendMessage(4873, 0, 0);
			this.tabPages = null;
			this.tabPageCount = 0;
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x00133254 File Offset: 0x00132254
		internal void RemoveTabPage(int index)
		{
			if (index < 0 || index >= this.tabPageCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.tabPageCount--;
			if (index < this.tabPageCount)
			{
				Array.Copy(this.tabPages, index + 1, this.tabPages, index, this.tabPageCount - index);
			}
			this.tabPages[this.tabPageCount] = null;
			if (base.IsHandleCreated)
			{
				base.SendMessage(4872, index, 0);
			}
			this.cachedDisplayRect = Rectangle.Empty;
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x00133305 File Offset: 0x00132305
		private void ResetItemSize()
		{
			this.ItemSize = TabControl.DEFAULT_ITEMSIZE;
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x00133312 File Offset: 0x00132312
		private void ResetPadding()
		{
			this.Padding = TabControl.DEFAULT_PADDING;
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x00133320 File Offset: 0x00132320
		private void ResizePages()
		{
			Rectangle displayRectangle = this.DisplayRectangle;
			TabPage[] array = this.GetTabPages();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Bounds = displayRectangle;
			}
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x00133352 File Offset: 0x00132352
		internal void SetToolTip(ToolTip toolTip, string controlToolTipText)
		{
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4910, new HandleRef(toolTip, toolTip.Handle), 0);
			this.controlTipText = controlToolTipText;
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x00133380 File Offset: 0x00132380
		internal void SetTabPage(int index, TabPage tabPage, NativeMethods.TCITEM_T tcitem)
		{
			if (index < 0 || index >= this.tabPageCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TCM_SETITEM, index, tcitem);
			}
			if (base.DesignMode && base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4876, (IntPtr)index, IntPtr.Zero);
			}
			this.tabPages[index] = tabPage;
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x0013342C File Offset: 0x0013242C
		public void SelectTab(int index)
		{
			TabPage tabPage = this.GetTabPage(index);
			if (tabPage != null)
			{
				this.SelectedTab = tabPage;
			}
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x0013344C File Offset: 0x0013244C
		public void SelectTab(TabPage tabPage)
		{
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}
			int index = this.FindTabPage(tabPage);
			this.SelectTab(index);
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x00133478 File Offset: 0x00132478
		public void SelectTab(string tabPageName)
		{
			if (tabPageName == null)
			{
				throw new ArgumentNullException("tabPageName");
			}
			TabPage tabPage = this.TabPages[tabPageName];
			this.SelectTab(tabPage);
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x001334A8 File Offset: 0x001324A8
		private void SelectNextTab(KeyEventArgs ke, bool forward)
		{
			bool focused = this.Focused;
			if (this.WmSelChanging())
			{
				this.tabControlState[32] = false;
				return;
			}
			if (base.ValidationCancelled)
			{
				this.tabControlState[32] = false;
				return;
			}
			int num = this.SelectedIndex;
			if (num != -1)
			{
				int tabCount = this.TabCount;
				if (forward)
				{
					num = (num + 1) % tabCount;
				}
				else
				{
					num = (num + tabCount - 1) % tabCount;
				}
				try
				{
					this.tabControlState[32] = true;
					this.tabControlState[64] = true;
					this.SelectedIndex = num;
					this.tabControlState[64] = !focused;
					this.WmSelChange();
				}
				finally
				{
					this.tabControlState[64] = false;
					ke.Handled = true;
				}
			}
		}

		// Token: 0x0600543E RID: 21566 RVA: 0x00133574 File Offset: 0x00132574
		internal override bool ShouldPerformContainerValidation()
		{
			return true;
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x00133577 File Offset: 0x00132577
		private bool ShouldSerializeItemSize()
		{
			return !this.itemSize.Equals(TabControl.DEFAULT_ITEMSIZE);
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x00133597 File Offset: 0x00132597
		private new bool ShouldSerializePadding()
		{
			return !this.padding.Equals(TabControl.DEFAULT_PADDING);
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x001335B8 File Offset: 0x001325B8
		public override string ToString()
		{
			string text = base.ToString();
			if (this.TabPages != null)
			{
				text = text + ", TabPages.Count: " + this.TabPages.Count.ToString(CultureInfo.CurrentCulture);
				if (this.TabPages.Count > 0)
				{
					text = text + ", TabPages[0]: " + this.TabPages[0].ToString();
				}
			}
			return text;
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x00133624 File Offset: 0x00132624
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			this.currentlyScaling = true;
			base.ScaleCore(dx, dy);
			this.currentlyScaling = false;
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x0013363C File Offset: 0x0013263C
		protected void UpdateTabSelection(bool updateFocus)
		{
			if (base.IsHandleCreated)
			{
				int num = this.SelectedIndex;
				TabPage[] array = this.GetTabPages();
				if (num != -1)
				{
					if (this.currentlyScaling)
					{
						array[num].SuspendLayout();
					}
					array[num].Bounds = this.DisplayRectangle;
					if (this.currentlyScaling)
					{
						array[num].ResumeLayout(false);
					}
					array[num].Visible = true;
					if (updateFocus && (!this.Focused || this.tabControlState[64]))
					{
						this.tabControlState[32] = false;
						bool flag = false;
						IntSecurity.ModifyFocus.Assert();
						try
						{
							flag = array[num].SelectNextControl(null, true, true, false, false);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						if (flag)
						{
							if (!base.ContainsFocus)
							{
								IContainerControl containerControl = base.GetContainerControlInternal();
								if (containerControl != null)
								{
									while (containerControl.ActiveControl is ContainerControl)
									{
										containerControl = (IContainerControl)containerControl.ActiveControl;
									}
									if (containerControl.ActiveControl != null)
									{
										containerControl.ActiveControl.FocusInternal();
									}
								}
							}
						}
						else
						{
							IContainerControl containerControlInternal = base.GetContainerControlInternal();
							if (containerControlInternal != null && !base.DesignMode)
							{
								if (containerControlInternal is ContainerControl)
								{
									((ContainerControl)containerControlInternal).SetActiveControlInternal(this);
								}
								else
								{
									IntSecurity.ModifyFocus.Assert();
									try
									{
										containerControlInternal.ActiveControl = this;
									}
									finally
									{
										CodeAccessPermission.RevertAssert();
									}
								}
							}
						}
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (i != this.SelectedIndex)
					{
						array[i].Visible = false;
					}
				}
			}
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x001337C4 File Offset: 0x001327C4
		protected override void OnStyleChanged(EventArgs e)
		{
			base.OnStyleChanged(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x001337E0 File Offset: 0x001327E0
		internal void UpdateTab(TabPage tabPage)
		{
			int index = this.FindTabPage(tabPage);
			this.SetTabPage(index, tabPage, tabPage.GetTCITEM());
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x00133818 File Offset: 0x00132818
		private void WmNeedText(ref Message m)
		{
			NativeMethods.TOOLTIPTEXT tooltiptext = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			int index = (int)tooltiptext.hdr.idFrom;
			string toolTipText = this.GetToolTipText(this.GetTabPage(index));
			if (!string.IsNullOrEmpty(toolTipText))
			{
				tooltiptext.lpszText = toolTipText;
			}
			else
			{
				tooltiptext.lpszText = this.controlTipText;
			}
			tooltiptext.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptext.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptext, m.LParam, false);
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x001338A8 File Offset: 0x001328A8
		private void WmReflectDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr intPtr = Control.SetUpPalette(drawitemstruct.hDC, false, false);
			using (Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC))
			{
				this.OnDrawItem(new DrawItemEventArgs(graphics, this.Font, Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom), drawitemstruct.itemID, (DrawItemState)drawitemstruct.itemState));
			}
			if (intPtr != IntPtr.Zero)
			{
				SafeNativeMethods.SelectPalette(new HandleRef(null, drawitemstruct.hDC), new HandleRef(null, intPtr), 0);
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x00133984 File Offset: 0x00132984
		private bool WmSelChange()
		{
			TabControlCancelEventArgs tabControlCancelEventArgs = new TabControlCancelEventArgs(this.SelectedTab, this.SelectedIndex, false, TabControlAction.Selecting);
			this.OnSelecting(tabControlCancelEventArgs);
			if (!tabControlCancelEventArgs.Cancel)
			{
				this.OnSelected(new TabControlEventArgs(this.SelectedTab, this.SelectedIndex, TabControlAction.Selected));
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
			else
			{
				base.SendMessage(4876, this.lastSelection, 0);
				this.UpdateTabSelection(true);
			}
			return tabControlCancelEventArgs.Cancel;
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x001339FC File Offset: 0x001329FC
		private bool WmSelChanging()
		{
			IContainerControl containerControlInternal = base.GetContainerControlInternal();
			if (containerControlInternal != null && !base.DesignMode)
			{
				if (containerControlInternal is ContainerControl)
				{
					((ContainerControl)containerControlInternal).SetActiveControlInternal(this);
				}
				else
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						containerControlInternal.ActiveControl = this;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			this.lastSelection = this.SelectedIndex;
			TabControlCancelEventArgs tabControlCancelEventArgs = new TabControlCancelEventArgs(this.SelectedTab, this.SelectedIndex, false, TabControlAction.Deselecting);
			this.OnDeselecting(tabControlCancelEventArgs);
			if (!tabControlCancelEventArgs.Cancel)
			{
				this.OnDeselected(new TabControlEventArgs(this.SelectedTab, this.SelectedIndex, TabControlAction.Deselected));
			}
			return tabControlCancelEventArgs.Cancel;
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x00133AA8 File Offset: 0x00132AA8
		private void WmTabBaseReLayout(ref Message m)
		{
			this.BeginUpdate();
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
			this.EndUpdate();
			base.Invalidate(true);
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			IntPtr handle = base.Handle;
			while (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, handle), this.tabBaseReLayoutMessage, this.tabBaseReLayoutMessage, 1))
			{
			}
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x00133B08 File Offset: 0x00132B08
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 78)
			{
				switch (msg)
				{
				case 8235:
					this.WmReflectDrawItem(ref m);
					goto IL_14E;
				case 8236:
					goto IL_14E;
				default:
					if (msg != 8270)
					{
						goto IL_14E;
					}
					break;
				}
			}
			NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
			int code = nmhdr.code;
			switch (code)
			{
			case -552:
				if (this.WmSelChanging())
				{
					m.Result = (IntPtr)1;
					this.tabControlState[32] = false;
					return;
				}
				if (base.ValidationCancelled)
				{
					m.Result = (IntPtr)1;
					this.tabControlState[32] = false;
					return;
				}
				this.tabControlState[32] = true;
				break;
			case -551:
				if (this.WmSelChange())
				{
					m.Result = (IntPtr)1;
					this.tabControlState[32] = false;
					return;
				}
				this.tabControlState[32] = true;
				break;
			default:
				if (code == -530 || code == -520)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(nmhdr, nmhdr.hwndFrom), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
					this.WmNeedText(ref m);
					m.Result = (IntPtr)1;
					return;
				}
				break;
			}
			IL_14E:
			if (m.Msg == this.tabBaseReLayoutMessage)
			{
				this.WmTabBaseReLayout(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x040036A2 RID: 13986
		private const int TABCONTROLSTATE_hotTrack = 1;

		// Token: 0x040036A3 RID: 13987
		private const int TABCONTROLSTATE_multiline = 2;

		// Token: 0x040036A4 RID: 13988
		private const int TABCONTROLSTATE_showToolTips = 4;

		// Token: 0x040036A5 RID: 13989
		private const int TABCONTROLSTATE_getTabRectfromItemSize = 8;

		// Token: 0x040036A6 RID: 13990
		private const int TABCONTROLSTATE_fromCreateHandles = 16;

		// Token: 0x040036A7 RID: 13991
		private const int TABCONTROLSTATE_UISelection = 32;

		// Token: 0x040036A8 RID: 13992
		private const int TABCONTROLSTATE_selectFirstControl = 64;

		// Token: 0x040036A9 RID: 13993
		private const int TABCONTROLSTATE_insertingItem = 128;

		// Token: 0x040036AA RID: 13994
		private const int TABCONTROLSTATE_autoSize = 256;

		// Token: 0x040036AB RID: 13995
		private static readonly Size DEFAULT_ITEMSIZE = Size.Empty;

		// Token: 0x040036AC RID: 13996
		private static readonly Point DEFAULT_PADDING = new Point(6, 3);

		// Token: 0x040036AD RID: 13997
		private TabControl.TabPageCollection tabCollection;

		// Token: 0x040036AE RID: 13998
		private TabAlignment alignment;

		// Token: 0x040036AF RID: 13999
		private TabDrawMode drawMode;

		// Token: 0x040036B0 RID: 14000
		private ImageList imageList;

		// Token: 0x040036B1 RID: 14001
		private Size itemSize = TabControl.DEFAULT_ITEMSIZE;

		// Token: 0x040036B2 RID: 14002
		private Point padding = TabControl.DEFAULT_PADDING;

		// Token: 0x040036B3 RID: 14003
		private TabSizeMode sizeMode;

		// Token: 0x040036B4 RID: 14004
		private TabAppearance appearance;

		// Token: 0x040036B5 RID: 14005
		private Rectangle cachedDisplayRect = Rectangle.Empty;

		// Token: 0x040036B6 RID: 14006
		private bool currentlyScaling;

		// Token: 0x040036B7 RID: 14007
		private int selectedIndex = -1;

		// Token: 0x040036B8 RID: 14008
		private Size cachedSize = Size.Empty;

		// Token: 0x040036B9 RID: 14009
		private string controlTipText = string.Empty;

		// Token: 0x040036BA RID: 14010
		private bool handleInTable;

		// Token: 0x040036BB RID: 14011
		private EventHandler onSelectedIndexChanged;

		// Token: 0x040036BC RID: 14012
		private DrawItemEventHandler onDrawItem;

		// Token: 0x040036BD RID: 14013
		private static readonly object EVENT_DESELECTING = new object();

		// Token: 0x040036BE RID: 14014
		private static readonly object EVENT_DESELECTED = new object();

		// Token: 0x040036BF RID: 14015
		private static readonly object EVENT_SELECTING = new object();

		// Token: 0x040036C0 RID: 14016
		private static readonly object EVENT_SELECTED = new object();

		// Token: 0x040036C1 RID: 14017
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x040036C2 RID: 14018
		private BitVector32 tabControlState;

		// Token: 0x040036C3 RID: 14019
		private readonly int tabBaseReLayoutMessage = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_TabBaseReLayout");

		// Token: 0x040036C4 RID: 14020
		private TabPage[] tabPages;

		// Token: 0x040036C5 RID: 14021
		private int tabPageCount;

		// Token: 0x040036C6 RID: 14022
		private int lastSelection;

		// Token: 0x040036C7 RID: 14023
		private bool rightToLeftLayout;

		// Token: 0x040036C8 RID: 14024
		private bool skipUpdateSize;

		// Token: 0x0200063E RID: 1598
		public class TabPageCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600544D RID: 21581 RVA: 0x00133CD5 File Offset: 0x00132CD5
			public TabPageCollection(TabControl owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}
				this.owner = owner;
			}

			// Token: 0x17001170 RID: 4464
			public virtual TabPage this[int index]
			{
				get
				{
					return this.owner.GetTabPage(index);
				}
				set
				{
					this.owner.SetTabPage(index, value, value.GetTCITEM());
				}
			}

			// Token: 0x17001171 RID: 4465
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is TabPage)
					{
						this[index] = (TabPage)value;
						return;
					}
					throw new ArgumentException("value");
				}
			}

			// Token: 0x17001172 RID: 4466
			public virtual TabPage this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			// Token: 0x17001173 RID: 4467
			// (get) Token: 0x06005453 RID: 21587 RVA: 0x00133D79 File Offset: 0x00132D79
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.tabPageCount;
				}
			}

			// Token: 0x17001174 RID: 4468
			// (get) Token: 0x06005454 RID: 21588 RVA: 0x00133D86 File Offset: 0x00132D86
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17001175 RID: 4469
			// (get) Token: 0x06005455 RID: 21589 RVA: 0x00133D89 File Offset: 0x00132D89
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001176 RID: 4470
			// (get) Token: 0x06005456 RID: 21590 RVA: 0x00133D8C File Offset: 0x00132D8C
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001177 RID: 4471
			// (get) Token: 0x06005457 RID: 21591 RVA: 0x00133D8F File Offset: 0x00132D8F
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06005458 RID: 21592 RVA: 0x00133D92 File Offset: 0x00132D92
			public void Add(TabPage value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.owner.Controls.Add(value);
			}

			// Token: 0x06005459 RID: 21593 RVA: 0x00133DB3 File Offset: 0x00132DB3
			int IList.Add(object value)
			{
				if (value is TabPage)
				{
					this.Add((TabPage)value);
					return this.IndexOf((TabPage)value);
				}
				throw new ArgumentException("value");
			}

			// Token: 0x0600545A RID: 21594 RVA: 0x00133DE0 File Offset: 0x00132DE0
			public void Add(string text)
			{
				this.Add(new TabPage
				{
					Text = text
				});
			}

			// Token: 0x0600545B RID: 21595 RVA: 0x00133E04 File Offset: 0x00132E04
			public void Add(string key, string text)
			{
				this.Add(new TabPage
				{
					Name = key,
					Text = text
				});
			}

			// Token: 0x0600545C RID: 21596 RVA: 0x00133E2C File Offset: 0x00132E2C
			public void Add(string key, string text, int imageIndex)
			{
				this.Add(new TabPage
				{
					Name = key,
					Text = text,
					ImageIndex = imageIndex
				});
			}

			// Token: 0x0600545D RID: 21597 RVA: 0x00133E5C File Offset: 0x00132E5C
			public void Add(string key, string text, string imageKey)
			{
				this.Add(new TabPage
				{
					Name = key,
					Text = text,
					ImageKey = imageKey
				});
			}

			// Token: 0x0600545E RID: 21598 RVA: 0x00133E8C File Offset: 0x00132E8C
			public void AddRange(TabPage[] pages)
			{
				if (pages == null)
				{
					throw new ArgumentNullException("pages");
				}
				foreach (TabPage value in pages)
				{
					this.Add(value);
				}
			}

			// Token: 0x0600545F RID: 21599 RVA: 0x00133EC2 File Offset: 0x00132EC2
			public bool Contains(TabPage page)
			{
				if (page == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.IndexOf(page) != -1;
			}

			// Token: 0x06005460 RID: 21600 RVA: 0x00133EDF File Offset: 0x00132EDF
			bool IList.Contains(object page)
			{
				return page is TabPage && this.Contains((TabPage)page);
			}

			// Token: 0x06005461 RID: 21601 RVA: 0x00133EF7 File Offset: 0x00132EF7
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x06005462 RID: 21602 RVA: 0x00133F08 File Offset: 0x00132F08
			public int IndexOf(TabPage page)
			{
				if (page == null)
				{
					throw new ArgumentNullException("value");
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == page)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06005463 RID: 21603 RVA: 0x00133F41 File Offset: 0x00132F41
			int IList.IndexOf(object page)
			{
				if (page is TabPage)
				{
					return this.IndexOf((TabPage)page);
				}
				return -1;
			}

			// Token: 0x06005464 RID: 21604 RVA: 0x00133F5C File Offset: 0x00132F5C
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x06005465 RID: 21605 RVA: 0x00133FDC File Offset: 0x00132FDC
			public void Insert(int index, TabPage tabPage)
			{
				this.owner.InsertItem(index, tabPage);
				try
				{
					this.owner.InsertingItem = true;
					this.owner.Controls.Add(tabPage);
				}
				finally
				{
					this.owner.InsertingItem = false;
				}
				this.owner.Controls.SetChildIndex(tabPage, index);
			}

			// Token: 0x06005466 RID: 21606 RVA: 0x00134044 File Offset: 0x00133044
			void IList.Insert(int index, object tabPage)
			{
				if (tabPage is TabPage)
				{
					this.Insert(index, (TabPage)tabPage);
					return;
				}
				throw new ArgumentException("tabPage");
			}

			// Token: 0x06005467 RID: 21607 RVA: 0x00134068 File Offset: 0x00133068
			public void Insert(int index, string text)
			{
				this.Insert(index, new TabPage
				{
					Text = text
				});
			}

			// Token: 0x06005468 RID: 21608 RVA: 0x0013408C File Offset: 0x0013308C
			public void Insert(int index, string key, string text)
			{
				this.Insert(index, new TabPage
				{
					Name = key,
					Text = text
				});
			}

			// Token: 0x06005469 RID: 21609 RVA: 0x001340B8 File Offset: 0x001330B8
			public void Insert(int index, string key, string text, int imageIndex)
			{
				TabPage tabPage = new TabPage();
				tabPage.Name = key;
				tabPage.Text = text;
				this.Insert(index, tabPage);
				tabPage.ImageIndex = imageIndex;
			}

			// Token: 0x0600546A RID: 21610 RVA: 0x001340EC File Offset: 0x001330EC
			public void Insert(int index, string key, string text, string imageKey)
			{
				TabPage tabPage = new TabPage();
				tabPage.Name = key;
				tabPage.Text = text;
				this.Insert(index, tabPage);
				tabPage.ImageKey = imageKey;
			}

			// Token: 0x0600546B RID: 21611 RVA: 0x0013411D File Offset: 0x0013311D
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x0600546C RID: 21612 RVA: 0x0013412E File Offset: 0x0013312E
			public virtual void Clear()
			{
				this.owner.RemoveAll();
			}

			// Token: 0x0600546D RID: 21613 RVA: 0x0013413B File Offset: 0x0013313B
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.owner.GetTabPages(), 0, dest, index, this.Count);
				}
			}

			// Token: 0x0600546E RID: 21614 RVA: 0x00134160 File Offset: 0x00133160
			public IEnumerator GetEnumerator()
			{
				TabPage[] tabPages = this.owner.GetTabPages();
				if (tabPages != null)
				{
					return tabPages.GetEnumerator();
				}
				return new TabPage[0].GetEnumerator();
			}

			// Token: 0x0600546F RID: 21615 RVA: 0x0013418E File Offset: 0x0013318E
			public void Remove(TabPage value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.owner.Controls.Remove(value);
			}

			// Token: 0x06005470 RID: 21616 RVA: 0x001341AF File Offset: 0x001331AF
			void IList.Remove(object value)
			{
				if (value is TabPage)
				{
					this.Remove((TabPage)value);
				}
			}

			// Token: 0x06005471 RID: 21617 RVA: 0x001341C5 File Offset: 0x001331C5
			public void RemoveAt(int index)
			{
				this.owner.Controls.RemoveAt(index);
			}

			// Token: 0x06005472 RID: 21618 RVA: 0x001341D8 File Offset: 0x001331D8
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x040036C9 RID: 14025
			private TabControl owner;

			// Token: 0x040036CA RID: 14026
			private int lastAccessedIndex = -1;
		}

		// Token: 0x0200063F RID: 1599
		[ComVisible(false)]
		public new class ControlCollection : Control.ControlCollection
		{
			// Token: 0x06005473 RID: 21619 RVA: 0x001341FD File Offset: 0x001331FD
			public ControlCollection(TabControl owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005474 RID: 21620 RVA: 0x00134210 File Offset: 0x00133210
			public override void Add(Control value)
			{
				if (!(value is TabPage))
				{
					throw new ArgumentException(SR.GetString("TabControlInvalidTabPageType", new object[]
					{
						value.GetType().Name
					}));
				}
				TabPage tabPage = (TabPage)value;
				if (!this.owner.InsertingItem)
				{
					if (this.owner.IsHandleCreated)
					{
						this.owner.AddTabPage(tabPage, tabPage.GetTCITEM());
					}
					else
					{
						this.owner.Insert(this.owner.TabCount, tabPage);
					}
				}
				base.Add(tabPage);
				tabPage.Visible = false;
				if (this.owner.IsHandleCreated)
				{
					tabPage.Bounds = this.owner.DisplayRectangle;
				}
				ISite site = this.owner.Site;
				if (site != null && tabPage.Site == null)
				{
					IContainer container = site.Container;
					if (container != null)
					{
						container.Add(tabPage);
					}
				}
				this.owner.ApplyItemSize();
				this.owner.UpdateTabSelection(false);
			}

			// Token: 0x06005475 RID: 21621 RVA: 0x00134308 File Offset: 0x00133308
			public override void Remove(Control value)
			{
				base.Remove(value);
				if (!(value is TabPage))
				{
					return;
				}
				int num = this.owner.FindTabPage((TabPage)value);
				int selectedIndex = this.owner.SelectedIndex;
				if (num != -1)
				{
					this.owner.RemoveTabPage(num);
					if (num == selectedIndex)
					{
						this.owner.SelectedIndex = 0;
					}
				}
				this.owner.UpdateTabSelection(false);
			}

			// Token: 0x040036CB RID: 14027
			private TabControl owner;
		}
	}
}
