using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200065A RID: 1626
	[Designer("System.Windows.Forms.Design.TabPageDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Text")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultEvent("Click")]
	public class TabPage : Panel
	{
		// Token: 0x06005550 RID: 21840 RVA: 0x00136ACE File Offset: 0x00135ACE
		public TabPage()
		{
			base.SetStyle(ControlStyles.CacheText, true);
			this.Text = null;
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06005551 RID: 21841 RVA: 0x00136AF4 File Offset: 0x00135AF4
		// (set) Token: 0x06005552 RID: 21842 RVA: 0x00136AF7 File Offset: 0x00135AF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override AutoSizeMode AutoSizeMode
		{
			get
			{
				return AutoSizeMode.GrowOnly;
			}
			set
			{
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x06005553 RID: 21843 RVA: 0x00136AF9 File Offset: 0x00135AF9
		// (set) Token: 0x06005554 RID: 21844 RVA: 0x00136B01 File Offset: 0x00135B01
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000324 RID: 804
		// (add) Token: 0x06005555 RID: 21845 RVA: 0x00136B0A File Offset: 0x00135B0A
		// (remove) Token: 0x06005556 RID: 21846 RVA: 0x00136B13 File Offset: 0x00135B13
		[SRCategory("CatPropertyChanged")]
		[Browsable(false)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
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

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x06005557 RID: 21847 RVA: 0x00136B1C File Offset: 0x00135B1C
		// (set) Token: 0x06005558 RID: 21848 RVA: 0x00136B6C File Offset: 0x00135B6C
		[SRDescription("ControlBackColorDescr")]
		[SRCategory("CatAppearance")]
		public override Color BackColor
		{
			get
			{
				Color backColor = base.BackColor;
				if (backColor != Control.DefaultBackColor)
				{
					return backColor;
				}
				TabControl tabControl = this.ParentInternal as TabControl;
				if (Application.RenderWithVisualStyles && this.UseVisualStyleBackColor && tabControl != null && tabControl.Appearance == TabAppearance.Normal)
				{
					return Color.Transparent;
				}
				return backColor;
			}
			set
			{
				if (base.DesignMode)
				{
					if (value != Color.Empty)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["UseVisualStyleBackColor"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(this, false);
						}
					}
				}
				else
				{
					this.UseVisualStyleBackColor = false;
				}
				base.BackColor = value;
			}
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x00136BBF File Offset: 0x00135BBF
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new TabPage.TabPageControlCollection(this);
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x0600555A RID: 21850 RVA: 0x00136BC7 File Offset: 0x00135BC7
		internal ImageList.Indexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ImageList.Indexer();
				}
				return this.imageIndexer;
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x0600555B RID: 21851 RVA: 0x00136BE2 File Offset: 0x00135BE2
		// (set) Token: 0x0600555C RID: 21852 RVA: 0x00136BF0 File Offset: 0x00135BF0
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[TypeConverter(typeof(ImageIndexConverter))]
		[SRDescription("TabItemImageIndexDescr")]
		[Localizable(true)]
		public int ImageIndex
		{
			get
			{
				return this.ImageIndexer.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"imageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				TabControl tabControl = this.ParentInternal as TabControl;
				if (tabControl != null)
				{
					this.ImageIndexer.ImageList = tabControl.ImageList;
				}
				this.ImageIndexer.Index = value;
				this.UpdateParent();
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x0600555D RID: 21853 RVA: 0x00136C78 File Offset: 0x00135C78
		// (set) Token: 0x0600555E RID: 21854 RVA: 0x00136C88 File Offset: 0x00135C88
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Localizable(true)]
		[SRDescription("TabItemImageIndexDescr")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				TabControl tabControl = this.ParentInternal as TabControl;
				if (tabControl != null)
				{
					this.ImageIndexer.ImageList = tabControl.ImageList;
				}
				this.UpdateParent();
			}
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x00136CC7 File Offset: 0x00135CC7
		public TabPage(string text) : this()
		{
			this.Text = text;
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06005560 RID: 21856 RVA: 0x00136CD6 File Offset: 0x00135CD6
		// (set) Token: 0x06005561 RID: 21857 RVA: 0x00136CDE File Offset: 0x00135CDE
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

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06005562 RID: 21858 RVA: 0x00136CE7 File Offset: 0x00135CE7
		// (set) Token: 0x06005563 RID: 21859 RVA: 0x00136CEF File Offset: 0x00135CEF
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

		// Token: 0x14000325 RID: 805
		// (add) Token: 0x06005564 RID: 21860 RVA: 0x00136CF8 File Offset: 0x00135CF8
		// (remove) Token: 0x06005565 RID: 21861 RVA: 0x00136D01 File Offset: 0x00135D01
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06005566 RID: 21862 RVA: 0x00136D0A File Offset: 0x00135D0A
		// (set) Token: 0x06005567 RID: 21863 RVA: 0x00136D12 File Offset: 0x00135D12
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000326 RID: 806
		// (add) Token: 0x06005568 RID: 21864 RVA: 0x00136D1B File Offset: 0x00135D1B
		// (remove) Token: 0x06005569 RID: 21865 RVA: 0x00136D24 File Offset: 0x00135D24
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x00136D2D File Offset: 0x00135D2D
		// (set) Token: 0x0600556B RID: 21867 RVA: 0x00136D35 File Offset: 0x00135D35
		[DefaultValue(false)]
		[SRDescription("TabItemUseVisualStyleBackColorDescr")]
		[SRCategory("CatAppearance")]
		public bool UseVisualStyleBackColor
		{
			get
			{
				return this.useVisualStyleBackColor;
			}
			set
			{
				this.useVisualStyleBackColor = value;
				base.Invalidate(true);
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x0600556C RID: 21868 RVA: 0x00136D45 File Offset: 0x00135D45
		// (set) Token: 0x0600556D RID: 21869 RVA: 0x00136D4D File Offset: 0x00135D4D
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

		// Token: 0x14000327 RID: 807
		// (add) Token: 0x0600556E RID: 21870 RVA: 0x00136D56 File Offset: 0x00135D56
		// (remove) Token: 0x0600556F RID: 21871 RVA: 0x00136D5F File Offset: 0x00135D5F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06005570 RID: 21872 RVA: 0x00136D68 File Offset: 0x00135D68
		// (set) Token: 0x06005571 RID: 21873 RVA: 0x00136D70 File Offset: 0x00135D70
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(typeof(Size), "0, 0")]
		public override Size MaximumSize
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

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x06005572 RID: 21874 RVA: 0x00136D79 File Offset: 0x00135D79
		// (set) Token: 0x06005573 RID: 21875 RVA: 0x00136D81 File Offset: 0x00135D81
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Size MinimumSize
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

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06005574 RID: 21876 RVA: 0x00136D8A File Offset: 0x00135D8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size PreferredSize
		{
			get
			{
				return base.PreferredSize;
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06005575 RID: 21877 RVA: 0x00136D92 File Offset: 0x00135D92
		// (set) Token: 0x06005576 RID: 21878 RVA: 0x00136D9A File Offset: 0x00135D9A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06005577 RID: 21879 RVA: 0x00136DA3 File Offset: 0x00135DA3
		internal override bool RenderTransparencyWithVisualStyles
		{
			get
			{
				return true;
			}
		}

		// Token: 0x14000328 RID: 808
		// (add) Token: 0x06005578 RID: 21880 RVA: 0x00136DA6 File Offset: 0x00135DA6
		// (remove) Token: 0x06005579 RID: 21881 RVA: 0x00136DAF File Offset: 0x00135DAF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x0600557A RID: 21882 RVA: 0x00136DB8 File Offset: 0x00135DB8
		// (set) Token: 0x0600557B RID: 21883 RVA: 0x00136DC0 File Offset: 0x00135DC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000329 RID: 809
		// (add) Token: 0x0600557C RID: 21884 RVA: 0x00136DC9 File Offset: 0x00135DC9
		// (remove) Token: 0x0600557D RID: 21885 RVA: 0x00136DD2 File Offset: 0x00135DD2
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x0600557E RID: 21886 RVA: 0x00136DDB File Offset: 0x00135DDB
		// (set) Token: 0x0600557F RID: 21887 RVA: 0x00136DE3 File Offset: 0x00135DE3
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Localizable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.UpdateParent();
			}
		}

		// Token: 0x1400032A RID: 810
		// (add) Token: 0x06005580 RID: 21888 RVA: 0x00136DF2 File Offset: 0x00135DF2
		// (remove) Token: 0x06005581 RID: 21889 RVA: 0x00136DFB File Offset: 0x00135DFB
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x06005582 RID: 21890 RVA: 0x00136E04 File Offset: 0x00135E04
		// (set) Token: 0x06005583 RID: 21891 RVA: 0x00136E0C File Offset: 0x00135E0C
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("TabItemToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value == this.toolTipText)
				{
					return;
				}
				this.toolTipText = value;
				this.UpdateParent();
			}
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06005584 RID: 21892 RVA: 0x00136E34 File Offset: 0x00135E34
		// (set) Token: 0x06005585 RID: 21893 RVA: 0x00136E3C File Offset: 0x00135E3C
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

		// Token: 0x1400032B RID: 811
		// (add) Token: 0x06005586 RID: 21894 RVA: 0x00136E45 File Offset: 0x00135E45
		// (remove) Token: 0x06005587 RID: 21895 RVA: 0x00136E4E File Offset: 0x00135E4E
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

		// Token: 0x06005588 RID: 21896 RVA: 0x00136E58 File Offset: 0x00135E58
		internal override void AssignParent(Control value)
		{
			if (value != null && !(value is TabControl))
			{
				throw new ArgumentException(SR.GetString("TABCONTROLTabPageNotOnTabControl", new object[]
				{
					value.GetType().FullName
				}));
			}
			base.AssignParent(value);
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x00136EA0 File Offset: 0x00135EA0
		public static TabPage GetTabPageOfComponent(object comp)
		{
			if (!(comp is Control))
			{
				return null;
			}
			Control control = (Control)comp;
			while (control != null && !(control is TabPage))
			{
				control = control.ParentInternal;
			}
			return (TabPage)control;
		}

		// Token: 0x0600558A RID: 21898 RVA: 0x00136ED8 File Offset: 0x00135ED8
		internal NativeMethods.TCITEM_T GetTCITEM()
		{
			NativeMethods.TCITEM_T tcitem_T = new NativeMethods.TCITEM_T();
			tcitem_T.mask = 0;
			tcitem_T.pszText = null;
			tcitem_T.cchTextMax = 0;
			tcitem_T.lParam = IntPtr.Zero;
			string text = this.Text;
			this.PrefixAmpersands(ref text);
			if (text != null)
			{
				tcitem_T.mask |= 1;
				tcitem_T.pszText = text;
				tcitem_T.cchTextMax = text.Length;
			}
			int imageIndex = this.ImageIndex;
			tcitem_T.mask |= 2;
			tcitem_T.iImage = this.ImageIndexer.ActualIndex;
			return tcitem_T;
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x00136F68 File Offset: 0x00135F68
		private void PrefixAmpersands(ref string value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			if (value.IndexOf('&') < 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '&')
				{
					if (i < value.Length - 1 && value[i + 1] == '&')
					{
						i++;
					}
					stringBuilder.Append("&&");
				}
				else
				{
					stringBuilder.Append(value[i]);
				}
			}
			value = stringBuilder.ToString();
		}

		// Token: 0x0600558C RID: 21900 RVA: 0x00136FF7 File Offset: 0x00135FF7
		internal void FireLeave(EventArgs e)
		{
			this.leaveFired = true;
			this.OnLeave(e);
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x00137007 File Offset: 0x00136007
		internal void FireEnter(EventArgs e)
		{
			this.enterFired = true;
			this.OnEnter(e);
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x00137018 File Offset: 0x00136018
		protected override void OnEnter(EventArgs e)
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (tabControl != null)
			{
				if (this.enterFired)
				{
					base.OnEnter(e);
				}
				this.enterFired = false;
			}
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x0013704C File Offset: 0x0013604C
		protected override void OnLeave(EventArgs e)
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (tabControl != null)
			{
				if (this.leaveFired)
				{
					base.OnLeave(e);
				}
				this.leaveFired = false;
			}
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x00137080 File Offset: 0x00136080
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (Application.RenderWithVisualStyles && this.UseVisualStyleBackColor && tabControl != null && tabControl.Appearance == TabAppearance.Normal)
			{
				Color backColor = this.UseVisualStyleBackColor ? Color.Transparent : this.BackColor;
				Rectangle rectangle = LayoutUtils.InflateRect(this.DisplayRectangle, base.Padding);
				Rectangle bounds = new Rectangle(rectangle.X - 4, rectangle.Y - 2, rectangle.Width + 8, rectangle.Height + 6);
				TabRenderer.DrawTabPage(e.Graphics, bounds);
				if (this.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(e.Graphics, this.BackgroundImage, backColor, this.BackgroundImageLayout, rectangle, rectangle, this.DisplayRectangle.Location);
					return;
				}
			}
			else
			{
				base.OnPaintBackground(e);
			}
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x00137158 File Offset: 0x00136158
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal is TabControl && parentInternal.IsHandleCreated)
			{
				Rectangle displayRectangle = parentInternal.DisplayRectangle;
				base.SetBoundsCore(displayRectangle.X, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height, (specified == BoundsSpecified.None) ? BoundsSpecified.None : BoundsSpecified.All);
				return;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x001371BC File Offset: 0x001361BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeLocation()
		{
			return base.Left != 0 || base.Top != 0;
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x001371D4 File Offset: 0x001361D4
		public override string ToString()
		{
			return "TabPage: {" + this.Text + "}";
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x001371EC File Offset: 0x001361EC
		internal void UpdateParent()
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (tabControl != null)
			{
				tabControl.UpdateTab(this);
			}
		}

		// Token: 0x04003704 RID: 14084
		private ImageList.Indexer imageIndexer;

		// Token: 0x04003705 RID: 14085
		private string toolTipText = "";

		// Token: 0x04003706 RID: 14086
		private bool enterFired;

		// Token: 0x04003707 RID: 14087
		private bool leaveFired;

		// Token: 0x04003708 RID: 14088
		private bool useVisualStyleBackColor;

		// Token: 0x0200065B RID: 1627
		[ComVisible(false)]
		public class TabPageControlCollection : Control.ControlCollection
		{
			// Token: 0x06005595 RID: 21909 RVA: 0x0013720F File Offset: 0x0013620F
			public TabPageControlCollection(TabPage owner) : base(owner)
			{
			}

			// Token: 0x06005596 RID: 21910 RVA: 0x00137218 File Offset: 0x00136218
			public override void Add(Control value)
			{
				if (value is TabPage)
				{
					throw new ArgumentException(SR.GetString("TABCONTROLTabPageOnTabPage"));
				}
				base.Add(value);
			}
		}
	}
}
