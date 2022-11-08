using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020006BE RID: 1726
	[Designer("System.Windows.Forms.Design.ToolStripContentPanelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[InitializationEvent("Load")]
	[DefaultEvent("Load")]
	[Docking(DockingBehavior.Never)]
	[ToolboxItem(false)]
	public class ToolStripContentPanel : Panel
	{
		// Token: 0x06005A7C RID: 23164 RVA: 0x001480CA File Offset: 0x001470CA
		public ToolStripContentPanel()
		{
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06005A7D RID: 23165 RVA: 0x001480EA File Offset: 0x001470EA
		// (set) Token: 0x06005A7E RID: 23166 RVA: 0x001480ED File Offset: 0x001470ED
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[Localizable(false)]
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

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06005A7F RID: 23167 RVA: 0x001480EF File Offset: 0x001470EF
		// (set) Token: 0x06005A80 RID: 23168 RVA: 0x001480F7 File Offset: 0x001470F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06005A81 RID: 23169 RVA: 0x00148100 File Offset: 0x00147100
		// (set) Token: 0x06005A82 RID: 23170 RVA: 0x00148108 File Offset: 0x00147108
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

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06005A83 RID: 23171 RVA: 0x00148111 File Offset: 0x00147111
		// (set) Token: 0x06005A84 RID: 23172 RVA: 0x00148119 File Offset: 0x00147119
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06005A85 RID: 23173 RVA: 0x00148122 File Offset: 0x00147122
		// (set) Token: 0x06005A86 RID: 23174 RVA: 0x0014812A File Offset: 0x0014712A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06005A87 RID: 23175 RVA: 0x00148133 File Offset: 0x00147133
		// (set) Token: 0x06005A88 RID: 23176 RVA: 0x0014813B File Offset: 0x0014713B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x00148144 File Offset: 0x00147144
		// (set) Token: 0x06005A8A RID: 23178 RVA: 0x0014814C File Offset: 0x0014714C
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (this.ParentInternal is ToolStripContainer && value == Color.Transparent)
				{
					this.ParentInternal.BackColor = Color.Transparent;
				}
				base.BackColor = value;
			}
		}

		// Token: 0x14000367 RID: 871
		// (add) Token: 0x06005A8B RID: 23179 RVA: 0x0014817F File Offset: 0x0014717F
		// (remove) Token: 0x06005A8C RID: 23180 RVA: 0x00148188 File Offset: 0x00147188
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x00148191 File Offset: 0x00147191
		// (set) Token: 0x06005A8E RID: 23182 RVA: 0x00148199 File Offset: 0x00147199
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000368 RID: 872
		// (add) Token: 0x06005A8F RID: 23183 RVA: 0x001481A2 File Offset: 0x001471A2
		// (remove) Token: 0x06005A90 RID: 23184 RVA: 0x001481AB File Offset: 0x001471AB
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

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06005A91 RID: 23185 RVA: 0x001481B4 File Offset: 0x001471B4
		// (set) Token: 0x06005A92 RID: 23186 RVA: 0x001481BC File Offset: 0x001471BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x14000369 RID: 873
		// (add) Token: 0x06005A93 RID: 23187 RVA: 0x001481C5 File Offset: 0x001471C5
		// (remove) Token: 0x06005A94 RID: 23188 RVA: 0x001481CE File Offset: 0x001471CE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1400036A RID: 874
		// (add) Token: 0x06005A95 RID: 23189 RVA: 0x001481D7 File Offset: 0x001471D7
		// (remove) Token: 0x06005A96 RID: 23190 RVA: 0x001481EA File Offset: 0x001471EA
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripContentPanelOnLoadDescr")]
		public event EventHandler Load
		{
			add
			{
				base.Events.AddHandler(ToolStripContentPanel.EventLoad, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripContentPanel.EventLoad, value);
			}
		}

		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06005A97 RID: 23191 RVA: 0x001481FD File Offset: 0x001471FD
		// (set) Token: 0x06005A98 RID: 23192 RVA: 0x00148205 File Offset: 0x00147205
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

		// Token: 0x1400036B RID: 875
		// (add) Token: 0x06005A99 RID: 23193 RVA: 0x0014820E File Offset: 0x0014720E
		// (remove) Token: 0x06005A9A RID: 23194 RVA: 0x00148217 File Offset: 0x00147217
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x06005A9B RID: 23195 RVA: 0x00148220 File Offset: 0x00147220
		// (set) Token: 0x06005A9C RID: 23196 RVA: 0x00148228 File Offset: 0x00147228
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x06005A9D RID: 23197 RVA: 0x00148231 File Offset: 0x00147231
		// (set) Token: 0x06005A9E RID: 23198 RVA: 0x00148239 File Offset: 0x00147239
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06005A9F RID: 23199 RVA: 0x00148242 File Offset: 0x00147242
		// (set) Token: 0x06005AA0 RID: 23200 RVA: 0x0014824A File Offset: 0x0014724A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x00148253 File Offset: 0x00147253
		// (set) Token: 0x06005AA2 RID: 23202 RVA: 0x0014825B File Offset: 0x0014725B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1400036C RID: 876
		// (add) Token: 0x06005AA3 RID: 23203 RVA: 0x00148264 File Offset: 0x00147264
		// (remove) Token: 0x06005AA4 RID: 23204 RVA: 0x0014826D File Offset: 0x0014726D
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

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06005AA5 RID: 23205 RVA: 0x00148276 File Offset: 0x00147276
		// (set) Token: 0x06005AA6 RID: 23206 RVA: 0x0014827E File Offset: 0x0014727E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1400036D RID: 877
		// (add) Token: 0x06005AA7 RID: 23207 RVA: 0x00148287 File Offset: 0x00147287
		// (remove) Token: 0x06005AA8 RID: 23208 RVA: 0x00148290 File Offset: 0x00147290
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

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x00148299 File Offset: 0x00147299
		private ToolStripRendererSwitcher RendererSwitcher
		{
			get
			{
				if (this.rendererSwitcher == null)
				{
					this.rendererSwitcher = new ToolStripRendererSwitcher(this, ToolStripRenderMode.System);
					this.HandleRendererChanged(this, EventArgs.Empty);
					this.rendererSwitcher.RendererChanged += this.HandleRendererChanged;
				}
				return this.rendererSwitcher;
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x001482D9 File Offset: 0x001472D9
		// (set) Token: 0x06005AAB RID: 23211 RVA: 0x001482E6 File Offset: 0x001472E6
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

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x001482F4 File Offset: 0x001472F4
		// (set) Token: 0x06005AAD RID: 23213 RVA: 0x00148301 File Offset: 0x00147301
		[SRDescription("ToolStripRenderModeDescr")]
		[SRCategory("CatAppearance")]
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

		// Token: 0x1400036E RID: 878
		// (add) Token: 0x06005AAE RID: 23214 RVA: 0x0014830F File Offset: 0x0014730F
		// (remove) Token: 0x06005AAF RID: 23215 RVA: 0x00148322 File Offset: 0x00147322
		[SRDescription("ToolStripRendererChanged")]
		[SRCategory("CatAppearance")]
		public event EventHandler RendererChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripContentPanel.EventRendererChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripContentPanel.EventRendererChanged, value);
			}
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x00148335 File Offset: 0x00147335
		private void HandleRendererChanged(object sender, EventArgs e)
		{
			this.OnRendererChanged(e);
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x0014833E File Offset: 0x0014733E
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!base.RecreatingHandle)
			{
				this.OnLoad(EventArgs.Empty);
			}
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x0014835C File Offset: 0x0014735C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripContentPanel.EventLoad];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x0014838C File Offset: 0x0014738C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			ToolStripContentPanelRenderEventArgs toolStripContentPanelRenderEventArgs = new ToolStripContentPanelRenderEventArgs(e.Graphics, this);
			this.Renderer.DrawToolStripContentPanelBackground(toolStripContentPanelRenderEventArgs);
			if (!toolStripContentPanelRenderEventArgs.Handled)
			{
				base.OnPaintBackground(e);
			}
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x001483C4 File Offset: 0x001473C4
		protected virtual void OnRendererChanged(EventArgs e)
		{
			if (this.Renderer is ToolStripProfessionalRenderer)
			{
				this.state[ToolStripContentPanel.stateLastDoubleBuffer] = this.DoubleBuffered;
				base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			}
			else
			{
				this.DoubleBuffered = this.state[ToolStripContentPanel.stateLastDoubleBuffer];
			}
			this.Renderer.InitializeContentPanel(this);
			base.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripContentPanel.EventRendererChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x0014844B File Offset: 0x0014744B
		private void ResetRenderMode()
		{
			this.RendererSwitcher.ResetRenderMode();
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x00148458 File Offset: 0x00147458
		private bool ShouldSerializeRenderMode()
		{
			return this.RendererSwitcher.ShouldSerializeRenderMode();
		}

		// Token: 0x040038BD RID: 14525
		private ToolStripRendererSwitcher rendererSwitcher;

		// Token: 0x040038BE RID: 14526
		private BitVector32 state = default(BitVector32);

		// Token: 0x040038BF RID: 14527
		private static readonly int stateLastDoubleBuffer = BitVector32.CreateMask();

		// Token: 0x040038C0 RID: 14528
		private static readonly object EventRendererChanged = new object();

		// Token: 0x040038C1 RID: 14529
		private static readonly object EventLoad = new object();
	}
}
