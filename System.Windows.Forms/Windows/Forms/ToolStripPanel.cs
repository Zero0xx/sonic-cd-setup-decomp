using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006BF RID: 1727
	[ToolboxBitmap(typeof(ToolStripPanel), "ToolStripPanel_standalone.bmp")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolStripPanelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ToolStripPanel : ContainerControl, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x06005AB8 RID: 23224 RVA: 0x00148488 File Offset: 0x00147488
		public ToolStripPanel()
		{
			base.SuspendLayout();
			base.AutoScaleMode = AutoScaleMode.None;
			this.InitFlowLayout();
			this.AutoSize = true;
			this.MinimumSize = Size.Empty;
			this.state[ToolStripPanel.stateLocked | ToolStripPanel.stateBeginInit | ToolStripPanel.stateChangingZOrder] = false;
			this.TabStop = false;
			ToolStripManager.ToolStripPanels.Add(this);
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			base.ResumeLayout(true);
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x0014853A File Offset: 0x0014753A
		internal ToolStripPanel(ToolStripContainer owner) : this()
		{
			this.owner = owner;
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06005ABA RID: 23226 RVA: 0x00148549 File Offset: 0x00147549
		// (set) Token: 0x06005ABB RID: 23227 RVA: 0x00148551 File Offset: 0x00147551
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06005ABC RID: 23228 RVA: 0x0014855A File Offset: 0x0014755A
		// (set) Token: 0x06005ABD RID: 23229 RVA: 0x00148562 File Offset: 0x00147562
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06005ABE RID: 23230 RVA: 0x0014856B File Offset: 0x0014756B
		// (set) Token: 0x06005ABF RID: 23231 RVA: 0x00148573 File Offset: 0x00147573
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06005AC0 RID: 23232 RVA: 0x0014857C File Offset: 0x0014757C
		// (set) Token: 0x06005AC1 RID: 23233 RVA: 0x00148584 File Offset: 0x00147584
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x06005AC2 RID: 23234 RVA: 0x0014858D File Offset: 0x0014758D
		// (set) Token: 0x06005AC3 RID: 23235 RVA: 0x00148595 File Offset: 0x00147595
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

		// Token: 0x1400036F RID: 879
		// (add) Token: 0x06005AC4 RID: 23236 RVA: 0x0014859E File Offset: 0x0014759E
		// (remove) Token: 0x06005AC5 RID: 23237 RVA: 0x001485A7 File Offset: 0x001475A7
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06005AC6 RID: 23238 RVA: 0x001485B0 File Offset: 0x001475B0
		protected override Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06005AC7 RID: 23239 RVA: 0x001485B7 File Offset: 0x001475B7
		protected override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06005AC8 RID: 23240 RVA: 0x001485BE File Offset: 0x001475BE
		// (set) Token: 0x06005AC9 RID: 23241 RVA: 0x001485C6 File Offset: 0x001475C6
		public Padding RowMargin
		{
			get
			{
				return this.rowMargin;
			}
			set
			{
				this.rowMargin = value;
				LayoutTransaction.DoLayout(this, this, "RowMargin");
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06005ACA RID: 23242 RVA: 0x001485DB File Offset: 0x001475DB
		// (set) Token: 0x06005ACB RID: 23243 RVA: 0x001485E3 File Offset: 0x001475E3
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
				if (value == DockStyle.Left || value == DockStyle.Right)
				{
					this.Orientation = Orientation.Vertical;
					return;
				}
				this.Orientation = Orientation.Horizontal;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06005ACC RID: 23244 RVA: 0x00148603 File Offset: 0x00147603
		internal Rectangle DragBounds
		{
			get
			{
				return LayoutUtils.InflateRect(base.ClientRectangle, ToolStripPanel.DragMargin);
			}
		}

		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06005ACD RID: 23245 RVA: 0x00148615 File Offset: 0x00147615
		internal bool IsInDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06005ACE RID: 23246 RVA: 0x0014861D File Offset: 0x0014761D
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x00148624 File Offset: 0x00147624
		// (set) Token: 0x06005AD0 RID: 23248 RVA: 0x00148636 File Offset: 0x00147636
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(false)]
		[Browsable(false)]
		public bool Locked
		{
			get
			{
				return this.state[ToolStripPanel.stateLocked];
			}
			set
			{
				this.state[ToolStripPanel.stateLocked] = value;
			}
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x00148649 File Offset: 0x00147649
		// (set) Token: 0x06005AD2 RID: 23250 RVA: 0x00148654 File Offset: 0x00147654
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (this.orientation != value)
				{
					this.orientation = value;
					this.rowMargin = LayoutUtils.FlipPadding(this.rowMargin);
					this.InitFlowLayout();
					foreach (object obj in this.RowsInternal)
					{
						ToolStripPanelRow toolStripPanelRow = (ToolStripPanelRow)obj;
						toolStripPanelRow.OnOrientationChanged();
					}
				}
			}
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x001486D4 File Offset: 0x001476D4
		private ToolStripRendererSwitcher RendererSwitcher
		{
			get
			{
				if (this.rendererSwitcher == null)
				{
					this.rendererSwitcher = new ToolStripRendererSwitcher(this);
					this.HandleRendererChanged(this, EventArgs.Empty);
					this.rendererSwitcher.RendererChanged += this.HandleRendererChanged;
				}
				return this.rendererSwitcher;
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06005AD4 RID: 23252 RVA: 0x00148713 File Offset: 0x00147713
		// (set) Token: 0x06005AD5 RID: 23253 RVA: 0x00148720 File Offset: 0x00147720
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ToolStripRenderer Renderer
		{
			get
			{
				return this.RendererSwitcher.Renderer;
			}
			set
			{
				this.RendererSwitcher.Renderer = value;
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x0014872E File Offset: 0x0014772E
		// (set) Token: 0x06005AD7 RID: 23255 RVA: 0x0014873B File Offset: 0x0014773B
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripRenderModeDescr")]
		public ToolStripRenderMode RenderMode
		{
			get
			{
				return this.RendererSwitcher.RenderMode;
			}
			set
			{
				this.RendererSwitcher.RenderMode = value;
			}
		}

		// Token: 0x14000370 RID: 880
		// (add) Token: 0x06005AD8 RID: 23256 RVA: 0x00148749 File Offset: 0x00147749
		// (remove) Token: 0x06005AD9 RID: 23257 RVA: 0x0014875C File Offset: 0x0014775C
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripRendererChanged")]
		public event EventHandler RendererChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripPanel.EventRendererChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripPanel.EventRendererChanged, value);
			}
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x00148770 File Offset: 0x00147770
		[SRDescription("ToolStripPanelRowsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		internal ToolStripPanel.ToolStripPanelRowCollection RowsInternal
		{
			get
			{
				ToolStripPanel.ToolStripPanelRowCollection toolStripPanelRowCollection = (ToolStripPanel.ToolStripPanelRowCollection)base.Properties.GetObject(ToolStripPanel.PropToolStripPanelRowCollection);
				if (toolStripPanelRowCollection == null)
				{
					toolStripPanelRowCollection = this.CreateToolStripPanelRowCollection();
					base.Properties.SetObject(ToolStripPanel.PropToolStripPanelRowCollection, toolStripPanelRowCollection);
				}
				return toolStripPanelRowCollection;
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x001487B0 File Offset: 0x001477B0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ToolStripPanelRowsDescr")]
		[Browsable(false)]
		public ToolStripPanelRow[] Rows
		{
			get
			{
				ToolStripPanelRow[] array = new ToolStripPanelRow[this.RowsInternal.Count];
				this.RowsInternal.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x001487DC File Offset: 0x001477DC
		// (set) Token: 0x06005ADD RID: 23261 RVA: 0x001487E4 File Offset: 0x001477E4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000371 RID: 881
		// (add) Token: 0x06005ADE RID: 23262 RVA: 0x001487ED File Offset: 0x001477ED
		// (remove) Token: 0x06005ADF RID: 23263 RVA: 0x001487F6 File Offset: 0x001477F6
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

		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x001487FF File Offset: 0x001477FF
		// (set) Token: 0x06005AE1 RID: 23265 RVA: 0x00148807 File Offset: 0x00147807
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000372 RID: 882
		// (add) Token: 0x06005AE2 RID: 23266 RVA: 0x00148810 File Offset: 0x00147810
		// (remove) Token: 0x06005AE3 RID: 23267 RVA: 0x00148819 File Offset: 0x00147819
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

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x00148822 File Offset: 0x00147822
		// (set) Token: 0x06005AE5 RID: 23269 RVA: 0x0014882A File Offset: 0x0014782A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000373 RID: 883
		// (add) Token: 0x06005AE6 RID: 23270 RVA: 0x00148833 File Offset: 0x00147833
		// (remove) Token: 0x06005AE7 RID: 23271 RVA: 0x0014883C File Offset: 0x0014783C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x06005AE8 RID: 23272 RVA: 0x00148845 File Offset: 0x00147845
		public void BeginInit()
		{
			this.state[ToolStripPanel.stateBeginInit] = true;
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x00148858 File Offset: 0x00147858
		public void EndInit()
		{
			this.state[ToolStripPanel.stateBeginInit] = false;
			this.state[ToolStripPanel.stateEndInit] = true;
			try
			{
				if (!this.state[ToolStripPanel.stateInJoin])
				{
					this.JoinControls();
				}
			}
			finally
			{
				this.state[ToolStripPanel.stateEndInit] = false;
			}
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x001488C4 File Offset: 0x001478C4
		private ToolStripPanel.ToolStripPanelRowCollection CreateToolStripPanelRowCollection()
		{
			return new ToolStripPanel.ToolStripPanelRowCollection(this);
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x001488CC File Offset: 0x001478CC
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new ToolStripPanel.ToolStripPanelControlCollection(this);
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x001488D4 File Offset: 0x001478D4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToolStripManager.ToolStripPanels.Remove(this);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x001488EB File Offset: 0x001478EB
		private void InitFlowLayout()
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				FlowLayout.SetFlowDirection(this, FlowDirection.TopDown);
			}
			else
			{
				FlowLayout.SetFlowDirection(this, FlowDirection.LeftToRight);
			}
			FlowLayout.SetWrapContents(this, false);
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x0014890C File Offset: 0x0014790C
		private Point GetStartLocation(ToolStrip toolStripToDrag)
		{
			if (toolStripToDrag.IsCurrentlyDragging && this.Orientation == Orientation.Horizontal && toolStripToDrag.RightToLeft == RightToLeft.Yes)
			{
				return new Point(toolStripToDrag.Right, toolStripToDrag.Top);
			}
			return toolStripToDrag.Location;
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x0014893F File Offset: 0x0014793F
		private void HandleRendererChanged(object sender, EventArgs e)
		{
			this.OnRendererChanged(e);
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x00148948 File Offset: 0x00147948
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			ToolStripPanelRenderEventArgs toolStripPanelRenderEventArgs = new ToolStripPanelRenderEventArgs(e.Graphics, this);
			this.Renderer.DrawToolStripPanelBackground(toolStripPanelRenderEventArgs);
			if (!toolStripPanelRenderEventArgs.Handled)
			{
				base.OnPaintBackground(e);
			}
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x00148980 File Offset: 0x00147980
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			if (!this.state[ToolStripPanel.stateBeginInit] && !this.state[ToolStripPanel.stateInJoin])
			{
				if (!this.state[ToolStripPanel.stateLayoutSuspended])
				{
					this.Join(e.Control as ToolStrip, e.Control.Location);
					return;
				}
				this.BeginInit();
			}
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x001489F0 File Offset: 0x001479F0
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			ISupportToolStripPanel supportToolStripPanel = e.Control as ISupportToolStripPanel;
			if (supportToolStripPanel != null && supportToolStripPanel.ToolStripPanelRow != null)
			{
				supportToolStripPanel.ToolStripPanelRow.ControlsInternal.Remove(e.Control);
			}
			base.OnControlRemoved(e);
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x00148A34 File Offset: 0x00147A34
		protected override void OnLayout(LayoutEventArgs e)
		{
			if (e.AffectedComponent != this.ParentInternal && e.AffectedComponent is Control)
			{
				ISupportToolStripPanel supportToolStripPanel = e.AffectedComponent as ISupportToolStripPanel;
				if (supportToolStripPanel != null && this.RowsInternal.Contains(supportToolStripPanel.ToolStripPanelRow))
				{
					LayoutTransaction.DoLayout(supportToolStripPanel.ToolStripPanelRow, e.AffectedComponent as IArrangedElement, e.AffectedProperty);
				}
			}
			base.OnLayout(e);
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x00148AA1 File Offset: 0x00147AA1
		internal override void OnLayoutSuspended()
		{
			base.OnLayoutSuspended();
			this.state[ToolStripPanel.stateLayoutSuspended] = true;
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x00148ABA File Offset: 0x00147ABA
		internal override void OnLayoutResuming(bool resumeLayout)
		{
			base.OnLayoutResuming(resumeLayout);
			this.state[ToolStripPanel.stateLayoutSuspended] = false;
			if (this.state[ToolStripPanel.stateBeginInit])
			{
				this.EndInit();
			}
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x00148AEC File Offset: 0x00147AEC
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			if (!this.state[ToolStripPanel.stateBeginInit])
			{
				if (base.Controls.Count > 0)
				{
					base.SuspendLayout();
					Control[] array = new Control[base.Controls.Count];
					Point[] array2 = new Point[base.Controls.Count];
					int num = 0;
					foreach (object obj in this.RowsInternal)
					{
						ToolStripPanelRow toolStripPanelRow = (ToolStripPanelRow)obj;
						foreach (object obj2 in toolStripPanelRow.ControlsInternal)
						{
							Control control = (Control)obj2;
							array[num] = control;
							array2[num] = new Point(toolStripPanelRow.Bounds.Width - control.Right, control.Top);
							num++;
						}
					}
					base.Controls.Clear();
					for (int i = 0; i < array.Length; i++)
					{
						this.Join(array[i] as ToolStrip, array2[i]);
					}
					base.ResumeLayout(true);
					return;
				}
			}
			else
			{
				this.state[ToolStripPanel.stateRightToLeftChanged] = true;
			}
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x00148C78 File Offset: 0x00147C78
		protected virtual void OnRendererChanged(EventArgs e)
		{
			this.Renderer.InitializePanel(this);
			base.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripPanel.EventRendererChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x00148CB8 File Offset: 0x00147CB8
		protected override void OnParentChanged(EventArgs e)
		{
			this.PerformUpdate();
			base.OnParentChanged(e);
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x00148CC7 File Offset: 0x00147CC7
		protected override void OnDockChanged(EventArgs e)
		{
			this.PerformUpdate();
			base.OnDockChanged(e);
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x00148CD6 File Offset: 0x00147CD6
		internal void PerformUpdate()
		{
			this.PerformUpdate(false);
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x00148CDF File Offset: 0x00147CDF
		internal void PerformUpdate(bool forceLayout)
		{
			if (!this.state[ToolStripPanel.stateBeginInit] && !this.state[ToolStripPanel.stateInJoin])
			{
				this.JoinControls(forceLayout);
			}
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x00148D0C File Offset: 0x00147D0C
		private void ResetRenderMode()
		{
			this.RendererSwitcher.ResetRenderMode();
		}

		// Token: 0x06005AFD RID: 23293 RVA: 0x00148D19 File Offset: 0x00147D19
		private bool ShouldSerializeRenderMode()
		{
			return this.RendererSwitcher.ShouldSerializeRenderMode();
		}

		// Token: 0x06005AFE RID: 23294 RVA: 0x00148D26 File Offset: 0x00147D26
		private bool ShouldSerializeDock()
		{
			return this.owner == null && this.Dock != DockStyle.None;
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x00148D3E File Offset: 0x00147D3E
		private void JoinControls()
		{
			this.JoinControls(false);
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x00148D48 File Offset: 0x00147D48
		private void JoinControls(bool forceLayout)
		{
			ToolStripPanel.ToolStripPanelControlCollection toolStripPanelControlCollection = base.Controls as ToolStripPanel.ToolStripPanelControlCollection;
			if (toolStripPanelControlCollection.Count > 0)
			{
				toolStripPanelControlCollection.Sort();
				Control[] array = new Control[toolStripPanelControlCollection.Count];
				toolStripPanelControlCollection.CopyTo(array, 0);
				int i = 0;
				while (i < array.Length)
				{
					int count = this.RowsInternal.Count;
					ISupportToolStripPanel supportToolStripPanel = array[i] as ISupportToolStripPanel;
					if (supportToolStripPanel == null || supportToolStripPanel.ToolStripPanelRow == null || supportToolStripPanel.IsCurrentlyDragging)
					{
						goto IL_8B;
					}
					ToolStripPanelRow toolStripPanelRow = supportToolStripPanel.ToolStripPanelRow;
					if (!toolStripPanelRow.Bounds.Contains(array[i].Location))
					{
						goto IL_8B;
					}
					IL_116:
					i++;
					continue;
					IL_8B:
					if (array[i].AutoSize)
					{
						array[i].Size = array[i].PreferredSize;
					}
					Point location = array[i].Location;
					if (this.state[ToolStripPanel.stateRightToLeftChanged])
					{
						location = new Point(base.Width - array[i].Right, location.Y);
					}
					this.Join(array[i] as ToolStrip, array[i].Location);
					if (count < this.RowsInternal.Count || forceLayout)
					{
						this.OnLayout(new LayoutEventArgs(this, PropertyNames.Rows));
						goto IL_116;
					}
					goto IL_116;
				}
			}
			this.state[ToolStripPanel.stateRightToLeftChanged] = false;
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x00148E8C File Offset: 0x00147E8C
		private void GiveToolStripPanelFeedback(ToolStrip toolStripToDrag, Point screenLocation)
		{
			if (this.Orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes)
			{
				screenLocation.Offset(-toolStripToDrag.Width, 0);
			}
			if (ToolStripPanel.CurrentFeedbackRect == null)
			{
				ToolStripPanel.CurrentFeedbackRect = new ToolStripPanel.FeedbackRectangle(toolStripToDrag.ClientRectangle);
			}
			if (!ToolStripPanel.CurrentFeedbackRect.Visible)
			{
				toolStripToDrag.SuspendCaputureMode();
				try
				{
					ToolStripPanel.CurrentFeedbackRect.Show(screenLocation);
					toolStripToDrag.CaptureInternal = true;
					return;
				}
				finally
				{
					toolStripToDrag.ResumeCaputureMode();
				}
			}
			ToolStripPanel.CurrentFeedbackRect.Move(screenLocation);
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x00148F18 File Offset: 0x00147F18
		internal static void ClearDragFeedback()
		{
			ToolStripPanel.FeedbackRectangle feedbackRectangle = ToolStripPanel.feedbackRect;
			ToolStripPanel.feedbackRect = null;
			if (feedbackRectangle != null)
			{
				feedbackRectangle.Dispose();
			}
		}

		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06005B03 RID: 23299 RVA: 0x00148F3A File Offset: 0x00147F3A
		// (set) Token: 0x06005B04 RID: 23300 RVA: 0x00148F41 File Offset: 0x00147F41
		private static ToolStripPanel.FeedbackRectangle CurrentFeedbackRect
		{
			get
			{
				return ToolStripPanel.feedbackRect;
			}
			set
			{
				ToolStripPanel.feedbackRect = value;
			}
		}

		// Token: 0x06005B05 RID: 23301 RVA: 0x00148F49 File Offset: 0x00147F49
		public void Join(ToolStrip toolStripToDrag)
		{
			this.Join(toolStripToDrag, Point.Empty);
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x00148F58 File Offset: 0x00147F58
		public void Join(ToolStrip toolStripToDrag, int row)
		{
			if (row < 0)
			{
				throw new ArgumentOutOfRangeException("row", SR.GetString("IndexOutOfRange", new object[]
				{
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Point empty = Point.Empty;
			Rectangle rectangle = Rectangle.Empty;
			if (row >= this.RowsInternal.Count)
			{
				rectangle = this.DragBounds;
			}
			else
			{
				rectangle = this.RowsInternal[row].DragBounds;
			}
			if (this.Orientation == Orientation.Horizontal)
			{
				empty = new Point(0, rectangle.Bottom - 1);
			}
			else
			{
				empty = new Point(rectangle.Right - 1, 0);
			}
			this.Join(toolStripToDrag, empty);
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x00148FFE File Offset: 0x00147FFE
		public void Join(ToolStrip toolStripToDrag, int x, int y)
		{
			this.Join(toolStripToDrag, new Point(x, y));
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x00149010 File Offset: 0x00148010
		public void Join(ToolStrip toolStripToDrag, Point location)
		{
			if (toolStripToDrag == null)
			{
				throw new ArgumentNullException("toolStripToDrag");
			}
			if (!this.state[ToolStripPanel.stateBeginInit] && !this.state[ToolStripPanel.stateInJoin])
			{
				try
				{
					this.state[ToolStripPanel.stateInJoin] = true;
					toolStripToDrag.ParentInternal = this;
					this.MoveInsideContainer(toolStripToDrag, location);
					return;
				}
				finally
				{
					this.state[ToolStripPanel.stateInJoin] = false;
				}
			}
			base.Controls.Add(toolStripToDrag);
			toolStripToDrag.Location = location;
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x001490A8 File Offset: 0x001480A8
		internal void MoveControl(ToolStrip toolStripToDrag, Point screenLocation)
		{
			if (toolStripToDrag == null)
			{
				return;
			}
			Point point = base.PointToClient(screenLocation);
			if (!this.DragBounds.Contains(point))
			{
				this.MoveOutsideContainer(toolStripToDrag, screenLocation);
				return;
			}
			this.Join(toolStripToDrag, point);
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x001490E8 File Offset: 0x001480E8
		private void MoveInsideContainer(ToolStrip toolStripToDrag, Point clientLocation)
		{
			if (((ISupportToolStripPanel)toolStripToDrag).IsCurrentlyDragging && !this.DragBounds.Contains(clientLocation))
			{
				return;
			}
			ToolStripPanel.ClearDragFeedback();
			if (toolStripToDrag.Site != null && toolStripToDrag.Site.DesignMode && base.IsHandleCreated && (clientLocation.X < 0 || clientLocation.Y < 0))
			{
				Point point = base.PointToClient(WindowsFormsUtils.LastCursorPoint);
				if (base.ClientRectangle.Contains(point))
				{
					clientLocation = point;
				}
			}
			ToolStripPanelRow toolStripPanelRow = ((ISupportToolStripPanel)toolStripToDrag).ToolStripPanelRow;
			if (toolStripPanelRow != null && toolStripPanelRow.Visible && toolStripPanelRow.ToolStripPanel == this && toolStripPanelRow.DragBounds.Contains(clientLocation))
			{
				((ISupportToolStripPanel)toolStripToDrag).ToolStripPanelRow.MoveControl(toolStripToDrag, this.GetStartLocation(toolStripToDrag), clientLocation);
				return;
			}
			ToolStripPanelRow toolStripPanelRow2 = this.PointToRow(clientLocation);
			if (toolStripPanelRow2 == null)
			{
				int num = this.RowsInternal.Count;
				if (this.Orientation == Orientation.Horizontal)
				{
					num = ((clientLocation.Y <= base.Padding.Left) ? 0 : num);
				}
				else
				{
					num = ((clientLocation.X <= base.Padding.Left) ? 0 : num);
				}
				ToolStripPanelRow toolStripPanelRow3 = null;
				if (this.RowsInternal.Count > 0)
				{
					if (num == 0)
					{
						toolStripPanelRow3 = this.RowsInternal[0];
					}
					else if (num > 0)
					{
						toolStripPanelRow3 = this.RowsInternal[num - 1];
					}
				}
				if (toolStripPanelRow3 != null && toolStripPanelRow3.ControlsInternal.Count == 1 && toolStripPanelRow3.ControlsInternal.Contains(toolStripToDrag))
				{
					toolStripPanelRow2 = toolStripPanelRow3;
					if (toolStripToDrag.IsInDesignMode)
					{
						Point endClientLocation = (this.Orientation == Orientation.Horizontal) ? new Point(clientLocation.X, toolStripPanelRow2.Bounds.Y) : new Point(toolStripPanelRow2.Bounds.X, clientLocation.Y);
						((ISupportToolStripPanel)toolStripToDrag).ToolStripPanelRow.MoveControl(toolStripToDrag, this.GetStartLocation(toolStripToDrag), endClientLocation);
					}
				}
				else
				{
					toolStripPanelRow2 = new ToolStripPanelRow(this);
					this.RowsInternal.Insert(num, toolStripPanelRow2);
				}
			}
			else if (!toolStripPanelRow2.CanMove(toolStripToDrag))
			{
				int num2 = this.RowsInternal.IndexOf(toolStripPanelRow2);
				if (toolStripPanelRow != null && toolStripPanelRow.ControlsInternal.Count == 1 && num2 > 0 && num2 - 1 == this.RowsInternal.IndexOf(toolStripPanelRow))
				{
					return;
				}
				toolStripPanelRow2 = new ToolStripPanelRow(this);
				this.RowsInternal.Insert(num2, toolStripPanelRow2);
				clientLocation.Y = toolStripPanelRow2.Bounds.Y;
			}
			bool flag = toolStripPanelRow != toolStripPanelRow2;
			if (!flag && toolStripPanelRow != null && toolStripPanelRow.ControlsInternal.Count > 1)
			{
				toolStripPanelRow.LeaveRow(toolStripToDrag);
				toolStripPanelRow = null;
				flag = true;
			}
			if (flag)
			{
				if (toolStripPanelRow != null)
				{
					toolStripPanelRow.LeaveRow(toolStripToDrag);
				}
				toolStripPanelRow2.JoinRow(toolStripToDrag, clientLocation);
			}
			if (flag && ((ISupportToolStripPanel)toolStripToDrag).IsCurrentlyDragging)
			{
				for (int i = 0; i < this.RowsInternal.Count; i++)
				{
					LayoutTransaction.DoLayout(this.RowsInternal[i], this, PropertyNames.Rows);
				}
				if (this.RowsInternal.IndexOf(toolStripPanelRow2) > 0)
				{
					IntSecurity.AdjustCursorPosition.Assert();
					try
					{
						Point position = toolStripToDrag.PointToScreen(toolStripToDrag.GripRectangle.Location);
						if (this.Orientation == Orientation.Vertical)
						{
							position.X += toolStripToDrag.GripRectangle.Width / 2;
							position.Y = Cursor.Position.Y;
						}
						else
						{
							position.Y += toolStripToDrag.GripRectangle.Height / 2;
							position.X = Cursor.Position.X;
						}
						Cursor.Position = position;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x001494B0 File Offset: 0x001484B0
		private void MoveOutsideContainer(ToolStrip toolStripToDrag, Point screenLocation)
		{
			ToolStripPanel toolStripPanel = ToolStripManager.ToolStripPanelFromPoint(toolStripToDrag, screenLocation);
			if (toolStripPanel != null)
			{
				using (new LayoutTransaction(toolStripPanel, toolStripPanel, null))
				{
					toolStripPanel.MoveControl(toolStripToDrag, screenLocation);
				}
				toolStripToDrag.PerformLayout();
				return;
			}
			this.GiveToolStripPanelFeedback(toolStripToDrag, screenLocation);
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x00149504 File Offset: 0x00148504
		public ToolStripPanelRow PointToRow(Point clientLocation)
		{
			foreach (object obj in this.RowsInternal)
			{
				ToolStripPanelRow toolStripPanelRow = (ToolStripPanelRow)obj;
				Rectangle rectangle = LayoutUtils.InflateRect(toolStripPanelRow.Bounds, toolStripPanelRow.Margin);
				if (this.ParentInternal != null)
				{
					if (this.Orientation == Orientation.Horizontal && rectangle.Width == 0)
					{
						rectangle.Width = this.ParentInternal.DisplayRectangle.Width;
					}
					else if (this.Orientation == Orientation.Vertical && rectangle.Height == 0)
					{
						rectangle.Height = this.ParentInternal.DisplayRectangle.Height;
					}
				}
				if (rectangle.Contains(clientLocation))
				{
					return toolStripPanelRow;
				}
			}
			return null;
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x001495E8 File Offset: 0x001485E8
		[Conditional("DEBUG")]
		private void Debug_VerifyOneToOneCellRowControlMatchup()
		{
			for (int i = 0; i < this.RowsInternal.Count; i++)
			{
				ToolStripPanelRow toolStripPanelRow = this.RowsInternal[i];
				foreach (object obj in toolStripPanelRow.Cells)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)obj;
					if (toolStripPanelCell.Control != null)
					{
						ToolStripPanelRow toolStripPanelRow2 = ((ISupportToolStripPanel)toolStripPanelCell.Control).ToolStripPanelRow;
						if (toolStripPanelRow2 != toolStripPanelRow)
						{
							int num = (toolStripPanelRow2 != null) ? this.RowsInternal.IndexOf(toolStripPanelRow2) : -1;
						}
					}
				}
			}
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x001496A0 File Offset: 0x001486A0
		[Conditional("DEBUG")]
		private void Debug_PrintRows()
		{
			for (int i = 0; i < this.RowsInternal.Count; i++)
			{
				for (int j = 0; j < this.RowsInternal[i].ControlsInternal.Count; j++)
				{
				}
			}
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x001496E4 File Offset: 0x001486E4
		[Conditional("DEBUG")]
		private void Debug_VerifyCountRows()
		{
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x001496E8 File Offset: 0x001486E8
		[Conditional("DEBUG")]
		private void Debug_VerifyNoOverlaps()
		{
			foreach (object obj in base.Controls)
			{
				Control control = (Control)obj;
				foreach (object obj2 in base.Controls)
				{
					Control control2 = (Control)obj2;
					if (control != control2)
					{
						Rectangle bounds = control.Bounds;
						bounds.Intersect(control2.Bounds);
						if (!LayoutUtils.IsZeroWidthOrHeight(bounds))
						{
							ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
							ISupportToolStripPanel supportToolStripPanel2 = control2 as ISupportToolStripPanel;
							string str = string.Format(CultureInfo.CurrentCulture, "OVERLAP detection:\r\n{0}: {1} row {2} row bounds {3}", new object[]
							{
								(control.Name == null) ? "" : control.Name,
								control.Bounds,
								(!this.RowsInternal.Contains(supportToolStripPanel.ToolStripPanelRow)) ? "unknown" : this.RowsInternal.IndexOf(supportToolStripPanel.ToolStripPanelRow).ToString(CultureInfo.CurrentCulture),
								supportToolStripPanel.ToolStripPanelRow.Bounds
							});
							str += string.Format(CultureInfo.CurrentCulture, "\r\n{0}: {1} row {2} row bounds {3}", new object[]
							{
								(control2.Name == null) ? "" : control2.Name,
								control2.Bounds,
								(!this.RowsInternal.Contains(supportToolStripPanel2.ToolStripPanelRow)) ? "unknown" : this.RowsInternal.IndexOf(supportToolStripPanel2.ToolStripPanelRow).ToString(CultureInfo.CurrentCulture),
								supportToolStripPanel2.ToolStripPanelRow.Bounds
							});
						}
					}
				}
			}
		}

		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06005B11 RID: 23313 RVA: 0x00149914 File Offset: 0x00148914
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.RowsInternal;
			}
		}

		// Token: 0x040038C2 RID: 14530
		private Orientation orientation;

		// Token: 0x040038C3 RID: 14531
		private Padding rowMargin = new Padding(3, 0, 0, 0);

		// Token: 0x040038C4 RID: 14532
		private ToolStripRendererSwitcher rendererSwitcher;

		// Token: 0x040038C5 RID: 14533
		private Type currentRendererType = typeof(Type);

		// Token: 0x040038C6 RID: 14534
		private BitVector32 state = default(BitVector32);

		// Token: 0x040038C7 RID: 14535
		private ToolStripContainer owner;

		// Token: 0x040038C8 RID: 14536
		internal static TraceSwitch ToolStripPanelDebug;

		// Token: 0x040038C9 RID: 14537
		internal static TraceSwitch ToolStripPanelFeedbackDebug;

		// Token: 0x040038CA RID: 14538
		internal static TraceSwitch ToolStripPanelMissingRowDebug;

		// Token: 0x040038CB RID: 14539
		[ThreadStatic]
		private static Rectangle lastFeedbackRect = Rectangle.Empty;

		// Token: 0x040038CC RID: 14540
		private static readonly int PropToolStripPanelRowCollection = PropertyStore.CreateKey();

		// Token: 0x040038CD RID: 14541
		private static readonly int stateLocked = BitVector32.CreateMask();

		// Token: 0x040038CE RID: 14542
		private static readonly int stateBeginInit = BitVector32.CreateMask(ToolStripPanel.stateLocked);

		// Token: 0x040038CF RID: 14543
		private static readonly int stateChangingZOrder = BitVector32.CreateMask(ToolStripPanel.stateBeginInit);

		// Token: 0x040038D0 RID: 14544
		private static readonly int stateInJoin = BitVector32.CreateMask(ToolStripPanel.stateChangingZOrder);

		// Token: 0x040038D1 RID: 14545
		private static readonly int stateEndInit = BitVector32.CreateMask(ToolStripPanel.stateInJoin);

		// Token: 0x040038D2 RID: 14546
		private static readonly int stateLayoutSuspended = BitVector32.CreateMask(ToolStripPanel.stateEndInit);

		// Token: 0x040038D3 RID: 14547
		private static readonly int stateRightToLeftChanged = BitVector32.CreateMask(ToolStripPanel.stateLayoutSuspended);

		// Token: 0x040038D4 RID: 14548
		internal static readonly Padding DragMargin = new Padding(10);

		// Token: 0x040038D5 RID: 14549
		private static readonly object EventRendererChanged = new object();

		// Token: 0x040038D6 RID: 14550
		[ThreadStatic]
		private static ToolStripPanel.FeedbackRectangle feedbackRect;

		// Token: 0x020006C0 RID: 1728
		private class FeedbackRectangle : IDisposable
		{
			// Token: 0x06005B13 RID: 23315 RVA: 0x001499B7 File Offset: 0x001489B7
			public FeedbackRectangle(Rectangle bounds)
			{
				this.dropDown = new ToolStripPanel.FeedbackRectangle.FeedbackDropDown(bounds);
			}

			// Token: 0x17001304 RID: 4868
			// (get) Token: 0x06005B14 RID: 23316 RVA: 0x001499CB File Offset: 0x001489CB
			// (set) Token: 0x06005B15 RID: 23317 RVA: 0x001499EF File Offset: 0x001489EF
			public bool Visible
			{
				get
				{
					return this.dropDown != null && !this.dropDown.IsDisposed && this.dropDown.Visible;
				}
				set
				{
					if (this.dropDown != null && !this.dropDown.IsDisposed)
					{
						this.dropDown.Visible = value;
					}
				}
			}

			// Token: 0x06005B16 RID: 23318 RVA: 0x00149A12 File Offset: 0x00148A12
			public void Show(Point newLocation)
			{
				this.dropDown.Show(newLocation);
			}

			// Token: 0x06005B17 RID: 23319 RVA: 0x00149A20 File Offset: 0x00148A20
			public void Move(Point newLocation)
			{
				this.dropDown.MoveTo(newLocation);
			}

			// Token: 0x06005B18 RID: 23320 RVA: 0x00149A2E File Offset: 0x00148A2E
			protected void Dispose(bool disposing)
			{
				if (disposing && this.dropDown != null)
				{
					this.Visible = false;
					this.dropDown.Dispose();
					this.dropDown = null;
				}
			}

			// Token: 0x06005B19 RID: 23321 RVA: 0x00149A54 File Offset: 0x00148A54
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x06005B1A RID: 23322 RVA: 0x00149A60 File Offset: 0x00148A60
			~FeedbackRectangle()
			{
				this.Dispose(false);
			}

			// Token: 0x040038D7 RID: 14551
			private ToolStripPanel.FeedbackRectangle.FeedbackDropDown dropDown;

			// Token: 0x020006C1 RID: 1729
			private class FeedbackDropDown : ToolStripDropDown
			{
				// Token: 0x06005B1B RID: 23323 RVA: 0x00149A90 File Offset: 0x00148A90
				public FeedbackDropDown(Rectangle bounds)
				{
					base.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
					base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
					base.SetStyle(ControlStyles.CacheText, true);
					base.AutoClose = false;
					this.AutoSize = false;
					base.DropShadowEnabled = false;
					base.Bounds = bounds;
					Rectangle rect = bounds;
					rect.Inflate(-1, -1);
					Region region = new Region(bounds);
					region.Exclude(rect);
					IntSecurity.ChangeWindowRegionForTopLevel.Assert();
					base.Region = region;
				}

				// Token: 0x06005B1C RID: 23324 RVA: 0x00149B10 File Offset: 0x00148B10
				private void ForceSynchronousPaint()
				{
					if (!base.IsDisposed && this._numPaintsServiced == 0)
					{
						try
						{
							NativeMethods.MSG msg = default(NativeMethods.MSG);
							while (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, IntPtr.Zero), 15, 15, 1))
							{
								SafeNativeMethods.UpdateWindow(new HandleRef(null, msg.hwnd));
								if (this._numPaintsServiced++ > 20)
								{
									break;
								}
							}
						}
						finally
						{
							this._numPaintsServiced = 0;
						}
					}
				}

				// Token: 0x06005B1D RID: 23325 RVA: 0x00149B94 File Offset: 0x00148B94
				protected override void OnPaint(PaintEventArgs e)
				{
				}

				// Token: 0x06005B1E RID: 23326 RVA: 0x00149B96 File Offset: 0x00148B96
				protected override void OnPaintBackground(PaintEventArgs e)
				{
					base.Renderer.DrawToolStripBackground(new ToolStripRenderEventArgs(e.Graphics, this));
					base.Renderer.DrawToolStripBorder(new ToolStripRenderEventArgs(e.Graphics, this));
				}

				// Token: 0x06005B1F RID: 23327 RVA: 0x00149BC6 File Offset: 0x00148BC6
				protected override void OnOpening(CancelEventArgs e)
				{
					base.OnOpening(e);
					e.Cancel = false;
				}

				// Token: 0x06005B20 RID: 23328 RVA: 0x00149BD6 File Offset: 0x00148BD6
				public void MoveTo(Point newLocation)
				{
					base.Location = newLocation;
					this.ForceSynchronousPaint();
				}

				// Token: 0x06005B21 RID: 23329 RVA: 0x00149BE5 File Offset: 0x00148BE5
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				protected override void WndProc(ref Message m)
				{
					if (m.Msg == 132)
					{
						m.Result = (IntPtr)(-1);
					}
					base.WndProc(ref m);
				}

				// Token: 0x040038D8 RID: 14552
				private const int MAX_PAINTS_TO_SERVICE = 20;

				// Token: 0x040038D9 RID: 14553
				private int _numPaintsServiced;
			}
		}

		// Token: 0x020006C2 RID: 1730
		[ComVisible(false)]
		[ListBindable(false)]
		public class ToolStripPanelRowCollection : ArrangedElementCollection, IList, ICollection, IEnumerable
		{
			// Token: 0x06005B22 RID: 23330 RVA: 0x00149C07 File Offset: 0x00148C07
			public ToolStripPanelRowCollection(ToolStripPanel owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005B23 RID: 23331 RVA: 0x00149C16 File Offset: 0x00148C16
			public ToolStripPanelRowCollection(ToolStripPanel owner, ToolStripPanelRow[] value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			// Token: 0x17001305 RID: 4869
			public virtual ToolStripPanelRow this[int index]
			{
				get
				{
					return (ToolStripPanelRow)base.InnerList[index];
				}
			}

			// Token: 0x06005B25 RID: 23333 RVA: 0x00149C40 File Offset: 0x00148C40
			public int Add(ToolStripPanelRow value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				int num = base.InnerList.Add(value);
				this.OnAdd(value, num);
				return num;
			}

			// Token: 0x06005B26 RID: 23334 RVA: 0x00149C74 File Offset: 0x00148C74
			public void AddRange(ToolStripPanelRow[] value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ToolStripPanel toolStripPanel = this.owner;
				if (toolStripPanel != null)
				{
					toolStripPanel.SuspendLayout();
				}
				try
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.Add(value[i]);
					}
				}
				finally
				{
					if (toolStripPanel != null)
					{
						toolStripPanel.ResumeLayout();
					}
				}
			}

			// Token: 0x06005B27 RID: 23335 RVA: 0x00149CD4 File Offset: 0x00148CD4
			public void AddRange(ToolStripPanel.ToolStripPanelRowCollection value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ToolStripPanel toolStripPanel = this.owner;
				if (toolStripPanel != null)
				{
					toolStripPanel.SuspendLayout();
				}
				try
				{
					int count = value.Count;
					for (int i = 0; i < count; i++)
					{
						this.Add(value[i]);
					}
				}
				finally
				{
					if (toolStripPanel != null)
					{
						toolStripPanel.ResumeLayout();
					}
				}
			}

			// Token: 0x06005B28 RID: 23336 RVA: 0x00149D3C File Offset: 0x00148D3C
			public bool Contains(ToolStripPanelRow value)
			{
				return base.InnerList.Contains(value);
			}

			// Token: 0x06005B29 RID: 23337 RVA: 0x00149D4C File Offset: 0x00148D4C
			public virtual void Clear()
			{
				if (this.owner != null)
				{
					this.owner.SuspendLayout();
				}
				try
				{
					while (this.Count != 0)
					{
						this.RemoveAt(this.Count - 1);
					}
				}
				finally
				{
					if (this.owner != null)
					{
						this.owner.ResumeLayout();
					}
				}
			}

			// Token: 0x06005B2A RID: 23338 RVA: 0x00149DAC File Offset: 0x00148DAC
			void IList.Clear()
			{
				this.Clear();
			}

			// Token: 0x17001306 RID: 4870
			// (get) Token: 0x06005B2B RID: 23339 RVA: 0x00149DB4 File Offset: 0x00148DB4
			bool IList.IsFixedSize
			{
				get
				{
					return base.InnerList.IsFixedSize;
				}
			}

			// Token: 0x06005B2C RID: 23340 RVA: 0x00149DC1 File Offset: 0x00148DC1
			bool IList.Contains(object value)
			{
				return base.InnerList.Contains(value);
			}

			// Token: 0x17001307 RID: 4871
			// (get) Token: 0x06005B2D RID: 23341 RVA: 0x00149DCF File Offset: 0x00148DCF
			bool IList.IsReadOnly
			{
				get
				{
					return base.InnerList.IsReadOnly;
				}
			}

			// Token: 0x06005B2E RID: 23342 RVA: 0x00149DDC File Offset: 0x00148DDC
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			// Token: 0x06005B2F RID: 23343 RVA: 0x00149DE5 File Offset: 0x00148DE5
			void IList.Remove(object value)
			{
				this.Remove(value as ToolStripPanelRow);
			}

			// Token: 0x06005B30 RID: 23344 RVA: 0x00149DF3 File Offset: 0x00148DF3
			int IList.Add(object value)
			{
				return this.Add(value as ToolStripPanelRow);
			}

			// Token: 0x06005B31 RID: 23345 RVA: 0x00149E01 File Offset: 0x00148E01
			int IList.IndexOf(object value)
			{
				return this.IndexOf(value as ToolStripPanelRow);
			}

			// Token: 0x06005B32 RID: 23346 RVA: 0x00149E0F File Offset: 0x00148E0F
			void IList.Insert(int index, object value)
			{
				this.Insert(index, value as ToolStripPanelRow);
			}

			// Token: 0x17001308 RID: 4872
			object IList.this[int index]
			{
				get
				{
					return base.InnerList[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("ToolStripCollectionMustInsertAndRemove"));
				}
			}

			// Token: 0x06005B35 RID: 23349 RVA: 0x00149E3D File Offset: 0x00148E3D
			public int IndexOf(ToolStripPanelRow value)
			{
				return base.InnerList.IndexOf(value);
			}

			// Token: 0x06005B36 RID: 23350 RVA: 0x00149E4B File Offset: 0x00148E4B
			public void Insert(int index, ToolStripPanelRow value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.InnerList.Insert(index, value);
				this.OnAdd(value, index);
			}

			// Token: 0x06005B37 RID: 23351 RVA: 0x00149E70 File Offset: 0x00148E70
			private void OnAdd(ToolStripPanelRow value, int index)
			{
				if (this.owner != null)
				{
					LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
				}
			}

			// Token: 0x06005B38 RID: 23352 RVA: 0x00149E8B File Offset: 0x00148E8B
			private void OnAfterRemove(ToolStripPanelRow row)
			{
			}

			// Token: 0x06005B39 RID: 23353 RVA: 0x00149E8D File Offset: 0x00148E8D
			public void Remove(ToolStripPanelRow value)
			{
				base.InnerList.Remove(value);
				this.OnAfterRemove(value);
			}

			// Token: 0x06005B3A RID: 23354 RVA: 0x00149EA4 File Offset: 0x00148EA4
			public void RemoveAt(int index)
			{
				ToolStripPanelRow row = null;
				if (index < this.Count && index >= 0)
				{
					row = (ToolStripPanelRow)base.InnerList[index];
				}
				base.InnerList.RemoveAt(index);
				this.OnAfterRemove(row);
			}

			// Token: 0x06005B3B RID: 23355 RVA: 0x00149EE5 File Offset: 0x00148EE5
			public void CopyTo(ToolStripPanelRow[] array, int index)
			{
				base.InnerList.CopyTo(array, index);
			}

			// Token: 0x040038DA RID: 14554
			private ToolStripPanel owner;
		}

		// Token: 0x020006C3 RID: 1731
		internal class ToolStripPanelControlCollection : WindowsFormsUtils.TypedControlCollection
		{
			// Token: 0x06005B3C RID: 23356 RVA: 0x00149EF4 File Offset: 0x00148EF4
			public ToolStripPanelControlCollection(ToolStripPanel owner) : base(owner, typeof(ToolStrip))
			{
				this.owner = owner;
			}

			// Token: 0x06005B3D RID: 23357 RVA: 0x00149F10 File Offset: 0x00148F10
			internal override void AddInternal(Control value)
			{
				if (value != null)
				{
					using (new LayoutTransaction(value, value, PropertyNames.Parent))
					{
						base.AddInternal(value);
						return;
					}
				}
				base.AddInternal(value);
			}

			// Token: 0x06005B3E RID: 23358 RVA: 0x00149F58 File Offset: 0x00148F58
			internal void Sort()
			{
				if (this.owner.Orientation == Orientation.Horizontal)
				{
					base.InnerList.Sort(new ToolStripPanel.ToolStripPanelControlCollection.YXComparer());
					return;
				}
				base.InnerList.Sort(new ToolStripPanel.ToolStripPanelControlCollection.XYComparer());
			}

			// Token: 0x040038DB RID: 14555
			private ToolStripPanel owner;

			// Token: 0x020006C4 RID: 1732
			public class XYComparer : IComparer
			{
				// Token: 0x06005B40 RID: 23360 RVA: 0x00149F90 File Offset: 0x00148F90
				public int Compare(object first, object second)
				{
					Control control = first as Control;
					Control control2 = second as Control;
					if (control.Bounds.X < control2.Bounds.X)
					{
						return -1;
					}
					if (control.Bounds.X != control2.Bounds.X)
					{
						return 1;
					}
					if (control.Bounds.Y < control2.Bounds.Y)
					{
						return -1;
					}
					return 1;
				}
			}

			// Token: 0x020006C5 RID: 1733
			public class YXComparer : IComparer
			{
				// Token: 0x06005B42 RID: 23362 RVA: 0x0014A018 File Offset: 0x00149018
				public int Compare(object first, object second)
				{
					Control control = first as Control;
					Control control2 = second as Control;
					if (control.Bounds.Y < control2.Bounds.Y)
					{
						return -1;
					}
					if (control.Bounds.Y != control2.Bounds.Y)
					{
						return 1;
					}
					if (control.Bounds.X < control2.Bounds.X)
					{
						return -1;
					}
					return 1;
				}
			}
		}
	}
}
