using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020006BC RID: 1724
	[ComVisible(true)]
	[SRDescription("ToolStripContainerDesc")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolStripContainerDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ToolStripContainer : ContainerControl
	{
		// Token: 0x06005A42 RID: 23106 RVA: 0x00147B1C File Offset: 0x00146B1C
		public ToolStripContainer()
		{
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
			base.SuspendLayout();
			try
			{
				this.topPanel = new ToolStripPanel(this);
				this.bottomPanel = new ToolStripPanel(this);
				this.leftPanel = new ToolStripPanel(this);
				this.rightPanel = new ToolStripPanel(this);
				this.contentPanel = new ToolStripContentPanel();
				this.contentPanel.Dock = DockStyle.Fill;
				this.topPanel.Dock = DockStyle.Top;
				this.bottomPanel.Dock = DockStyle.Bottom;
				this.rightPanel.Dock = DockStyle.Right;
				this.leftPanel.Dock = DockStyle.Left;
				ToolStripContainer.ToolStripContainerTypedControlCollection toolStripContainerTypedControlCollection = this.Controls as ToolStripContainer.ToolStripContainerTypedControlCollection;
				if (toolStripContainerTypedControlCollection != null)
				{
					toolStripContainerTypedControlCollection.AddInternal(this.contentPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.leftPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.rightPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.topPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.bottomPanel);
				}
			}
			finally
			{
				base.ResumeLayout(true);
			}
		}

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06005A43 RID: 23107 RVA: 0x00147C20 File Offset: 0x00146C20
		// (set) Token: 0x06005A44 RID: 23108 RVA: 0x00147C28 File Offset: 0x00146C28
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06005A45 RID: 23109 RVA: 0x00147C31 File Offset: 0x00146C31
		// (set) Token: 0x06005A46 RID: 23110 RVA: 0x00147C39 File Offset: 0x00146C39
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06005A47 RID: 23111 RVA: 0x00147C42 File Offset: 0x00146C42
		// (set) Token: 0x06005A48 RID: 23112 RVA: 0x00147C4A File Offset: 0x00146C4A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06005A49 RID: 23113 RVA: 0x00147C53 File Offset: 0x00146C53
		// (set) Token: 0x06005A4A RID: 23114 RVA: 0x00147C5B File Offset: 0x00146C5B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor
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

		// Token: 0x14000360 RID: 864
		// (add) Token: 0x06005A4B RID: 23115 RVA: 0x00147C64 File Offset: 0x00146C64
		// (remove) Token: 0x06005A4C RID: 23116 RVA: 0x00147C6D File Offset: 0x00146C6D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06005A4D RID: 23117 RVA: 0x00147C76 File Offset: 0x00146C76
		// (set) Token: 0x06005A4E RID: 23118 RVA: 0x00147C7E File Offset: 0x00146C7E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage
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

		// Token: 0x14000361 RID: 865
		// (add) Token: 0x06005A4F RID: 23119 RVA: 0x00147C87 File Offset: 0x00146C87
		// (remove) Token: 0x06005A50 RID: 23120 RVA: 0x00147C90 File Offset: 0x00146C90
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06005A51 RID: 23121 RVA: 0x00147C99 File Offset: 0x00146C99
		// (set) Token: 0x06005A52 RID: 23122 RVA: 0x00147CA1 File Offset: 0x00146CA1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000362 RID: 866
		// (add) Token: 0x06005A53 RID: 23123 RVA: 0x00147CAA File Offset: 0x00146CAA
		// (remove) Token: 0x06005A54 RID: 23124 RVA: 0x00147CB3 File Offset: 0x00146CB3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
				base.BackgroundImageLayoutChanged += value;
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06005A55 RID: 23125 RVA: 0x00147CBC File Offset: 0x00146CBC
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerBottomToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel BottomToolStripPanel
		{
			get
			{
				return this.bottomPanel;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06005A56 RID: 23126 RVA: 0x00147CC4 File Offset: 0x00146CC4
		// (set) Token: 0x06005A57 RID: 23127 RVA: 0x00147CD1 File Offset: 0x00146CD1
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerBottomToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool BottomToolStripPanelVisible
		{
			get
			{
				return this.BottomToolStripPanel.Visible;
			}
			set
			{
				this.BottomToolStripPanel.Visible = value;
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06005A58 RID: 23128 RVA: 0x00147CDF File Offset: 0x00146CDF
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerContentPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripContentPanel ContentPanel
		{
			get
			{
				return this.contentPanel;
			}
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06005A59 RID: 23129 RVA: 0x00147CE7 File Offset: 0x00146CE7
		// (set) Token: 0x06005A5A RID: 23130 RVA: 0x00147CEF File Offset: 0x00146CEF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000363 RID: 867
		// (add) Token: 0x06005A5B RID: 23131 RVA: 0x00147CF8 File Offset: 0x00146CF8
		// (remove) Token: 0x06005A5C RID: 23132 RVA: 0x00147D01 File Offset: 0x00146D01
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

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06005A5D RID: 23133 RVA: 0x00147D0A File Offset: 0x00146D0A
		// (set) Token: 0x06005A5E RID: 23134 RVA: 0x00147D12 File Offset: 0x00146D12
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContextMenuStrip ContextMenuStrip
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

		// Token: 0x14000364 RID: 868
		// (add) Token: 0x06005A5F RID: 23135 RVA: 0x00147D1B File Offset: 0x00146D1B
		// (remove) Token: 0x06005A60 RID: 23136 RVA: 0x00147D24 File Offset: 0x00146D24
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

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06005A61 RID: 23137 RVA: 0x00147D2D File Offset: 0x00146D2D
		// (set) Token: 0x06005A62 RID: 23138 RVA: 0x00147D35 File Offset: 0x00146D35
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000365 RID: 869
		// (add) Token: 0x06005A63 RID: 23139 RVA: 0x00147D3E File Offset: 0x00146D3E
		// (remove) Token: 0x06005A64 RID: 23140 RVA: 0x00147D47 File Offset: 0x00146D47
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06005A65 RID: 23141 RVA: 0x00147D50 File Offset: 0x00146D50
		protected override Size DefaultSize
		{
			get
			{
				return new Size(150, 175);
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x00147D61 File Offset: 0x00146D61
		// (set) Token: 0x06005A67 RID: 23143 RVA: 0x00147D69 File Offset: 0x00146D69
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ForeColor
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

		// Token: 0x14000366 RID: 870
		// (add) Token: 0x06005A68 RID: 23144 RVA: 0x00147D72 File Offset: 0x00146D72
		// (remove) Token: 0x06005A69 RID: 23145 RVA: 0x00147D7B File Offset: 0x00146D7B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x00147D84 File Offset: 0x00146D84
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerLeftToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel LeftToolStripPanel
		{
			get
			{
				return this.leftPanel;
			}
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06005A6B RID: 23147 RVA: 0x00147D8C File Offset: 0x00146D8C
		// (set) Token: 0x06005A6C RID: 23148 RVA: 0x00147D99 File Offset: 0x00146D99
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerLeftToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool LeftToolStripPanelVisible
		{
			get
			{
				return this.LeftToolStripPanel.Visible;
			}
			set
			{
				this.LeftToolStripPanel.Visible = value;
			}
		}

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06005A6D RID: 23149 RVA: 0x00147DA7 File Offset: 0x00146DA7
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerRightToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel RightToolStripPanel
		{
			get
			{
				return this.rightPanel;
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06005A6E RID: 23150 RVA: 0x00147DAF File Offset: 0x00146DAF
		// (set) Token: 0x06005A6F RID: 23151 RVA: 0x00147DBC File Offset: 0x00146DBC
		[SRDescription("ToolStripContainerRightToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		[SRCategory("CatAppearance")]
		public bool RightToolStripPanelVisible
		{
			get
			{
				return this.RightToolStripPanel.Visible;
			}
			set
			{
				this.RightToolStripPanel.Visible = value;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06005A70 RID: 23152 RVA: 0x00147DCA File Offset: 0x00146DCA
		[SRDescription("ToolStripContainerTopToolStripPanelDescr")]
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(false)]
		public ToolStripPanel TopToolStripPanel
		{
			get
			{
				return this.topPanel;
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06005A71 RID: 23153 RVA: 0x00147DD2 File Offset: 0x00146DD2
		// (set) Token: 0x06005A72 RID: 23154 RVA: 0x00147DDF File Offset: 0x00146DDF
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerTopToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool TopToolStripPanelVisible
		{
			get
			{
				return this.TopToolStripPanel.Visible;
			}
			set
			{
				this.TopToolStripPanel.Visible = value;
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06005A73 RID: 23155 RVA: 0x00147DED File Offset: 0x00146DED
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x00147DF5 File Offset: 0x00146DF5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new ToolStripContainer.ToolStripContainerTypedControlCollection(this, true);
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x00147E00 File Offset: 0x00146E00
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			RightToLeft rightToLeft = this.RightToLeft;
			if (rightToLeft == RightToLeft.Yes)
			{
				this.RightToolStripPanel.Dock = DockStyle.Left;
				this.LeftToolStripPanel.Dock = DockStyle.Right;
				return;
			}
			this.RightToolStripPanel.Dock = DockStyle.Right;
			this.LeftToolStripPanel.Dock = DockStyle.Left;
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x00147E50 File Offset: 0x00146E50
		protected override void OnSizeChanged(EventArgs e)
		{
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				control.SuspendLayout();
			}
			base.OnSizeChanged(e);
			foreach (object obj2 in this.Controls)
			{
				Control control2 = (Control)obj2;
				control2.ResumeLayout();
			}
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x00147F00 File Offset: 0x00146F00
		internal override void RecreateHandleCore()
		{
			if (base.IsHandleCreated)
			{
				foreach (object obj in this.Controls)
				{
					Control control = (Control)obj;
					control.CreateControl(true);
				}
			}
			base.RecreateHandleCore();
		}

		// Token: 0x040038B5 RID: 14517
		private ToolStripPanel topPanel;

		// Token: 0x040038B6 RID: 14518
		private ToolStripPanel bottomPanel;

		// Token: 0x040038B7 RID: 14519
		private ToolStripPanel leftPanel;

		// Token: 0x040038B8 RID: 14520
		private ToolStripPanel rightPanel;

		// Token: 0x040038B9 RID: 14521
		private ToolStripContentPanel contentPanel;

		// Token: 0x020006BD RID: 1725
		internal class ToolStripContainerTypedControlCollection : WindowsFormsUtils.ReadOnlyControlCollection
		{
			// Token: 0x06005A78 RID: 23160 RVA: 0x00147F68 File Offset: 0x00146F68
			public ToolStripContainerTypedControlCollection(Control c, bool isReadOnly) : base(c, isReadOnly)
			{
				this.owner = (c as ToolStripContainer);
			}

			// Token: 0x06005A79 RID: 23161 RVA: 0x00147FA0 File Offset: 0x00146FA0
			public override void Add(Control value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ToolStripContainerUseContentPanel"));
				}
				Type type = value.GetType();
				if (!this.contentPanelType.IsAssignableFrom(type) && !this.panelType.IsAssignableFrom(type))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("TypedControlCollectionShouldBeOfTypes", new object[]
					{
						this.contentPanelType.Name,
						this.panelType.Name
					}), new object[0]), value.GetType().Name);
				}
				base.Add(value);
			}

			// Token: 0x06005A7A RID: 23162 RVA: 0x0014804C File Offset: 0x0014704C
			public override void Remove(Control value)
			{
				if ((value is ToolStripPanel || value is ToolStripContentPanel) && !this.owner.DesignMode && this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.Remove(value);
			}

			// Token: 0x06005A7B RID: 23163 RVA: 0x0014808A File Offset: 0x0014708A
			internal override void SetChildIndexInternal(Control child, int newIndex)
			{
				if (child is ToolStripPanel || child is ToolStripContentPanel)
				{
					if (this.owner.DesignMode)
					{
						return;
					}
					if (this.IsReadOnly)
					{
						throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
					}
				}
				base.SetChildIndexInternal(child, newIndex);
			}

			// Token: 0x040038BA RID: 14522
			private ToolStripContainer owner;

			// Token: 0x040038BB RID: 14523
			private Type contentPanelType = typeof(ToolStripContentPanel);

			// Token: 0x040038BC RID: 14524
			private Type panelType = typeof(ToolStripPanel);
		}
	}
}
