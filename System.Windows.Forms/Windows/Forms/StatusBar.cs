using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000626 RID: 1574
	[Designer("System.Windows.Forms.Design.StatusBarDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("PanelClick")]
	[ComVisible(true)]
	[DefaultProperty("Text")]
	public class StatusBar : Control
	{
		// Token: 0x06005252 RID: 21074 RVA: 0x0012D70C File Offset: 0x0012C70C
		public StatusBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable, false);
			this.Dock = DockStyle.Bottom;
			this.TabStop = false;
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06005253 RID: 21075 RVA: 0x0012D758 File Offset: 0x0012C758
		private static VisualStyleRenderer VisualStyleRenderer
		{
			get
			{
				if (VisualStyleRenderer.IsSupported)
				{
					if (StatusBar.renderer == null)
					{
						StatusBar.renderer = new VisualStyleRenderer(VisualStyleElement.ToolBar.Button.Normal);
					}
				}
				else
				{
					StatusBar.renderer = null;
				}
				return StatusBar.renderer;
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06005254 RID: 21076 RVA: 0x0012D784 File Offset: 0x0012C784
		private int SizeGripWidth
		{
			get
			{
				if (this.sizeGripWidth == 0)
				{
					if (Application.RenderWithVisualStyles && StatusBar.VisualStyleRenderer != null)
					{
						VisualStyleRenderer visualStyleRenderer = StatusBar.VisualStyleRenderer;
						VisualStyleElement normal = VisualStyleElement.Status.GripperPane.Normal;
						visualStyleRenderer.SetParameters(normal);
						this.sizeGripWidth = visualStyleRenderer.GetPartSize(Graphics.FromHwndInternal(base.Handle), ThemeSizeType.True).Width;
						normal = VisualStyleElement.Status.Gripper.Normal;
						visualStyleRenderer.SetParameters(normal);
						Size partSize = visualStyleRenderer.GetPartSize(Graphics.FromHwndInternal(base.Handle), ThemeSizeType.True);
						this.sizeGripWidth += partSize.Width;
						this.sizeGripWidth = Math.Max(this.sizeGripWidth, 16);
					}
					else
					{
						this.sizeGripWidth = 16;
					}
				}
				return this.sizeGripWidth;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06005255 RID: 21077 RVA: 0x0012D837 File Offset: 0x0012C837
		// (set) Token: 0x06005256 RID: 21078 RVA: 0x0012D83E File Offset: 0x0012C83E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1400030D RID: 781
		// (add) Token: 0x06005257 RID: 21079 RVA: 0x0012D840 File Offset: 0x0012C840
		// (remove) Token: 0x06005258 RID: 21080 RVA: 0x0012D849 File Offset: 0x0012C849
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06005259 RID: 21081 RVA: 0x0012D852 File Offset: 0x0012C852
		// (set) Token: 0x0600525A RID: 21082 RVA: 0x0012D85A File Offset: 0x0012C85A
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

		// Token: 0x1400030E RID: 782
		// (add) Token: 0x0600525B RID: 21083 RVA: 0x0012D863 File Offset: 0x0012C863
		// (remove) Token: 0x0600525C RID: 21084 RVA: 0x0012D86C File Offset: 0x0012C86C
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

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x0600525D RID: 21085 RVA: 0x0012D875 File Offset: 0x0012C875
		// (set) Token: 0x0600525E RID: 21086 RVA: 0x0012D87D File Offset: 0x0012C87D
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

		// Token: 0x1400030F RID: 783
		// (add) Token: 0x0600525F RID: 21087 RVA: 0x0012D886 File Offset: 0x0012C886
		// (remove) Token: 0x06005260 RID: 21088 RVA: 0x0012D88F File Offset: 0x0012C88F
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

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x0012D898 File Offset: 0x0012C898
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "msctls_statusbar32";
				if (this.sizeGrip)
				{
					createParams.Style |= 256;
				}
				else
				{
					createParams.Style &= -257;
				}
				createParams.Style |= 12;
				return createParams;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x0012D8F5 File Offset: 0x0012C8F5
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06005263 RID: 21091 RVA: 0x0012D8F8 File Offset: 0x0012C8F8
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06005264 RID: 21092 RVA: 0x0012D903 File Offset: 0x0012C903
		// (set) Token: 0x06005265 RID: 21093 RVA: 0x0012D90B File Offset: 0x0012C90B
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

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06005266 RID: 21094 RVA: 0x0012D914 File Offset: 0x0012C914
		// (set) Token: 0x06005267 RID: 21095 RVA: 0x0012D91C File Offset: 0x0012C91C
		[DefaultValue(DockStyle.Bottom)]
		[Localizable(true)]
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

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06005268 RID: 21096 RVA: 0x0012D925 File Offset: 0x0012C925
		// (set) Token: 0x06005269 RID: 21097 RVA: 0x0012D92D File Offset: 0x0012C92D
		[Localizable(true)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				this.SetPanelContentsWidths(false);
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x0012D93D File Offset: 0x0012C93D
		// (set) Token: 0x0600526B RID: 21099 RVA: 0x0012D945 File Offset: 0x0012C945
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000310 RID: 784
		// (add) Token: 0x0600526C RID: 21100 RVA: 0x0012D94E File Offset: 0x0012C94E
		// (remove) Token: 0x0600526D RID: 21101 RVA: 0x0012D957 File Offset: 0x0012C957
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

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x0600526E RID: 21102 RVA: 0x0012D960 File Offset: 0x0012C960
		// (set) Token: 0x0600526F RID: 21103 RVA: 0x0012D968 File Offset: 0x0012C968
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000311 RID: 785
		// (add) Token: 0x06005270 RID: 21104 RVA: 0x0012D971 File Offset: 0x0012C971
		// (remove) Token: 0x06005271 RID: 21105 RVA: 0x0012D97A File Offset: 0x0012C97A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06005272 RID: 21106 RVA: 0x0012D983 File Offset: 0x0012C983
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("StatusBarPanelsDescr")]
		[Localizable(true)]
		[MergableProperty(false)]
		public StatusBar.StatusBarPanelCollection Panels
		{
			get
			{
				if (this.panelsCollection == null)
				{
					this.panelsCollection = new StatusBar.StatusBarPanelCollection(this);
				}
				return this.panelsCollection;
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x0012D99F File Offset: 0x0012C99F
		// (set) Token: 0x06005274 RID: 21108 RVA: 0x0012D9B5 File Offset: 0x0012C9B5
		[Localizable(true)]
		public override string Text
		{
			get
			{
				if (this.simpleText == null)
				{
					return "";
				}
				return this.simpleText;
			}
			set
			{
				this.SetSimpleText(value);
				if (this.simpleText != value)
				{
					this.simpleText = value;
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x0012D9DE File Offset: 0x0012C9DE
		// (set) Token: 0x06005276 RID: 21110 RVA: 0x0012D9E8 File Offset: 0x0012C9E8
		[SRDescription("StatusBarShowPanelsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ShowPanels
		{
			get
			{
				return this.showPanels;
			}
			set
			{
				if (this.showPanels != value)
				{
					this.showPanels = value;
					this.layoutDirty = true;
					if (base.IsHandleCreated)
					{
						int wparam = (!this.showPanels) ? 1 : 0;
						base.SendMessage(1033, wparam, 0);
						if (this.showPanels)
						{
							base.PerformLayout();
							this.RealizePanels();
						}
						else if (this.tooltips != null)
						{
							for (int i = 0; i < this.panels.Count; i++)
							{
								this.tooltips.SetTool(this.panels[i], null);
							}
						}
						this.SetSimpleText(this.simpleText);
					}
				}
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06005277 RID: 21111 RVA: 0x0012DA8A File Offset: 0x0012CA8A
		// (set) Token: 0x06005278 RID: 21112 RVA: 0x0012DA92 File Offset: 0x0012CA92
		[SRDescription("StatusBarSizingGripDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		public bool SizingGrip
		{
			get
			{
				return this.sizeGrip;
			}
			set
			{
				if (value != this.sizeGrip)
				{
					this.sizeGrip = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06005279 RID: 21113 RVA: 0x0012DAAA File Offset: 0x0012CAAA
		// (set) Token: 0x0600527A RID: 21114 RVA: 0x0012DAB2 File Offset: 0x0012CAB2
		[DefaultValue(false)]
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

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x0600527B RID: 21115 RVA: 0x0012DABB File Offset: 0x0012CABB
		internal bool ToolTipSet
		{
			get
			{
				return this.toolTipSet;
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x0600527C RID: 21116 RVA: 0x0012DAC3 File Offset: 0x0012CAC3
		internal ToolTip MainToolTip
		{
			get
			{
				return this.mainToolTip;
			}
		}

		// Token: 0x14000312 RID: 786
		// (add) Token: 0x0600527D RID: 21117 RVA: 0x0012DACB File Offset: 0x0012CACB
		// (remove) Token: 0x0600527E RID: 21118 RVA: 0x0012DADE File Offset: 0x0012CADE
		[SRDescription("StatusBarDrawItem")]
		[SRCategory("CatBehavior")]
		public event StatusBarDrawItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(StatusBar.EVENT_SBDRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(StatusBar.EVENT_SBDRAWITEM, value);
			}
		}

		// Token: 0x14000313 RID: 787
		// (add) Token: 0x0600527F RID: 21119 RVA: 0x0012DAF1 File Offset: 0x0012CAF1
		// (remove) Token: 0x06005280 RID: 21120 RVA: 0x0012DB04 File Offset: 0x0012CB04
		[SRCategory("CatMouse")]
		[SRDescription("StatusBarOnPanelClickDescr")]
		public event StatusBarPanelClickEventHandler PanelClick
		{
			add
			{
				base.Events.AddHandler(StatusBar.EVENT_PANELCLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(StatusBar.EVENT_PANELCLICK, value);
			}
		}

		// Token: 0x14000314 RID: 788
		// (add) Token: 0x06005281 RID: 21121 RVA: 0x0012DB17 File Offset: 0x0012CB17
		// (remove) Token: 0x06005282 RID: 21122 RVA: 0x0012DB20 File Offset: 0x0012CB20
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

		// Token: 0x06005283 RID: 21123 RVA: 0x0012DB29 File Offset: 0x0012CB29
		internal bool ArePanelsRealized()
		{
			return this.showPanels && base.IsHandleCreated;
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x0012DB3B File Offset: 0x0012CB3B
		internal void DirtyLayout()
		{
			this.layoutDirty = true;
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x0012DB44 File Offset: 0x0012CB44
		private void ApplyPanelWidths()
		{
			if (!base.IsHandleCreated)
			{
				return;
			}
			int count = this.panels.Count;
			if (count == 0)
			{
				int[] array = new int[]
				{
					base.Size.Width
				};
				if (this.sizeGrip)
				{
					array[0] -= this.SizeGripWidth;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1028, 1, array);
				base.SendMessage(1039, 0, IntPtr.Zero);
				return;
			}
			int[] array2 = new int[count];
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				num += statusBarPanel.Width;
				array2[i] = num;
				statusBarPanel.Right = array2[i];
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1028, count, array2);
			for (int j = 0; j < count; j++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[j];
				this.UpdateTooltip(statusBarPanel);
			}
			this.layoutDirty = false;
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x0012DC68 File Offset: 0x0012CC68
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 4
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x0012DCB8 File Offset: 0x0012CCB8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.panelsCollection != null)
			{
				StatusBarPanel[] array = new StatusBarPanel[this.panelsCollection.Count];
				((ICollection)this.panelsCollection).CopyTo(array, 0);
				this.panelsCollection.Clear();
				foreach (StatusBarPanel statusBarPanel in array)
				{
					statusBarPanel.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x0012DD1A File Offset: 0x0012CD1A
		private void ForcePanelUpdate()
		{
			if (this.ArePanelsRealized())
			{
				this.layoutDirty = true;
				this.SetPanelContentsWidths(true);
				base.PerformLayout();
				this.RealizePanels();
			}
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x0012DD40 File Offset: 0x0012CD40
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!base.DesignMode)
			{
				this.tooltips = new StatusBar.ControlToolTip(this);
			}
			if (!this.showPanels)
			{
				base.SendMessage(1033, 1, 0);
				this.SetSimpleText(this.simpleText);
				return;
			}
			this.ForcePanelUpdate();
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x0012DD91 File Offset: 0x0012CD91
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (this.tooltips != null)
			{
				this.tooltips.Dispose();
				this.tooltips = null;
			}
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x0012DDB4 File Offset: 0x0012CDB4
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.lastClick.X = e.X;
			this.lastClick.Y = e.Y;
			base.OnMouseDown(e);
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x0012DDE0 File Offset: 0x0012CDE0
		protected virtual void OnPanelClick(StatusBarPanelClickEventArgs e)
		{
			StatusBarPanelClickEventHandler statusBarPanelClickEventHandler = (StatusBarPanelClickEventHandler)base.Events[StatusBar.EVENT_PANELCLICK];
			if (statusBarPanelClickEventHandler != null)
			{
				statusBarPanelClickEventHandler(this, e);
			}
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x0012DE0E File Offset: 0x0012CE0E
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.showPanels)
			{
				this.LayoutPanels();
				if (base.IsHandleCreated && this.panelsRealized != this.panels.Count)
				{
					this.RealizePanels();
				}
			}
			base.OnLayout(levent);
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x0012DE48 File Offset: 0x0012CE48
		internal void RealizePanels()
		{
			int count = this.panels.Count;
			int num = this.panelsRealized;
			this.panelsRealized = 0;
			if (count == 0)
			{
				base.SendMessage(NativeMethods.SB_SETTEXT, 0, "");
			}
			int i;
			for (i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				try
				{
					statusBarPanel.Realize();
					this.panelsRealized++;
				}
				catch
				{
				}
			}
			while (i < num)
			{
				base.SendMessage(NativeMethods.SB_SETTEXT, 0, null);
				i++;
			}
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x0012DEE8 File Offset: 0x0012CEE8
		internal void RemoveAllPanelsWithoutUpdate()
		{
			int count = this.panels.Count;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				statusBarPanel.ParentInternal = null;
			}
			this.panels.Clear();
			if (this.showPanels)
			{
				this.ApplyPanelWidths();
				this.ForcePanelUpdate();
			}
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x0012DF48 File Offset: 0x0012CF48
		internal void SetPanelContentsWidths(bool newPanels)
		{
			int count = this.panels.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				if (statusBarPanel.AutoSize == StatusBarPanelAutoSize.Contents)
				{
					int contentsWidth = statusBarPanel.GetContentsWidth(newPanels);
					if (statusBarPanel.Width != contentsWidth)
					{
						statusBarPanel.Width = contentsWidth;
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.DirtyLayout();
				base.PerformLayout();
			}
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x0012DFB8 File Offset: 0x0012CFB8
		private void SetSimpleText(string simpleText)
		{
			if (!this.showPanels && base.IsHandleCreated)
			{
				int num = 511;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					num |= 1024;
				}
				base.SendMessage(NativeMethods.SB_SETTEXT, num, simpleText);
			}
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x0012DFFC File Offset: 0x0012CFFC
		private void LayoutPanels()
		{
			int num = 0;
			int num2 = 0;
			StatusBarPanel[] array = new StatusBarPanel[this.panels.Count];
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				if (statusBarPanel.AutoSize == StatusBarPanelAutoSize.Spring)
				{
					array[num2] = statusBarPanel;
					num2++;
				}
				else
				{
					num += statusBarPanel.Width;
				}
			}
			if (num2 > 0)
			{
				Rectangle bounds = base.Bounds;
				int j = num2;
				int num3 = bounds.Width - num;
				if (this.sizeGrip)
				{
					num3 -= this.SizeGripWidth;
				}
				int num4 = int.MinValue;
				while (j > 0)
				{
					int num5 = num3 / j;
					if (num3 == num4)
					{
						break;
					}
					num4 = num3;
					for (int k = 0; k < num2; k++)
					{
						StatusBarPanel statusBarPanel = array[k];
						if (statusBarPanel != null)
						{
							if (num5 < statusBarPanel.MinWidth)
							{
								if (statusBarPanel.Width != statusBarPanel.MinWidth)
								{
									flag = true;
								}
								statusBarPanel.Width = statusBarPanel.MinWidth;
								array[k] = null;
								j--;
								num3 -= statusBarPanel.MinWidth;
							}
							else
							{
								if (statusBarPanel.Width != num5)
								{
									flag = true;
								}
								statusBarPanel.Width = num5;
							}
						}
					}
				}
			}
			if (flag || this.layoutDirty)
			{
				this.ApplyPanelWidths();
			}
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x0012E134 File Offset: 0x0012D134
		protected virtual void OnDrawItem(StatusBarDrawItemEventArgs sbdievent)
		{
			StatusBarDrawItemEventHandler statusBarDrawItemEventHandler = (StatusBarDrawItemEventHandler)base.Events[StatusBar.EVENT_SBDRAWITEM];
			if (statusBarDrawItemEventHandler != null)
			{
				statusBarDrawItemEventHandler(this, sbdievent);
			}
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0012E162 File Offset: 0x0012D162
		protected override void OnResize(EventArgs e)
		{
			base.Invalidate();
			base.OnResize(e);
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x0012E174 File Offset: 0x0012D174
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Panels != null)
			{
				text = text + ", Panels.Count: " + this.Panels.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Panels.Count > 0)
				{
					text = text + ", Panels[0]: " + this.Panels[0].ToString();
				}
			}
			return text;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0012E1E0 File Offset: 0x0012D1E0
		internal void SetToolTip(ToolTip t)
		{
			this.mainToolTip = t;
			this.toolTipSet = true;
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x0012E1F0 File Offset: 0x0012D1F0
		internal void UpdateTooltip(StatusBarPanel panel)
		{
			if (this.tooltips == null)
			{
				if (!base.IsHandleCreated || base.DesignMode)
				{
					return;
				}
				this.tooltips = new StatusBar.ControlToolTip(this);
			}
			if (panel.Parent == this && panel.ToolTipText.Length > 0)
			{
				int width = SystemInformation.Border3DSize.Width;
				StatusBar.ControlToolTip.Tool tool = this.tooltips.GetTool(panel);
				if (tool == null)
				{
					tool = new StatusBar.ControlToolTip.Tool();
				}
				tool.text = panel.ToolTipText;
				tool.rect = new Rectangle(panel.Right - panel.Width + width, 0, panel.Width - width, base.Height);
				this.tooltips.SetTool(panel, tool);
				return;
			}
			this.tooltips.SetTool(panel, null);
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x0012E2B0 File Offset: 0x0012D2B0
		private void UpdatePanelIndex()
		{
			int count = this.panels.Count;
			for (int i = 0; i < count; i++)
			{
				((StatusBarPanel)this.panels[i]).Index = i;
			}
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x0012E2EC File Offset: 0x0012D2EC
		private void WmDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			int count = this.panels.Count;
			if (drawitemstruct.itemID >= 0)
			{
				int itemID = drawitemstruct.itemID;
			}
			StatusBarPanel panel = (StatusBarPanel)this.panels[drawitemstruct.itemID];
			Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC);
			Rectangle r = Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom);
			this.OnDrawItem(new StatusBarDrawItemEventArgs(graphics, this.Font, r, drawitemstruct.itemID, DrawItemState.None, panel, this.ForeColor, this.BackColor));
			graphics.Dispose();
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x0012E3B4 File Offset: 0x0012D3B4
		private void WmNotifyNMClick(NativeMethods.NMHDR note)
		{
			if (!this.showPanels)
			{
				return;
			}
			int count = this.panels.Count;
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				num += statusBarPanel.Width;
				if (this.lastClick.X < num)
				{
					num2 = i;
					break;
				}
			}
			if (num2 != -1)
			{
				MouseButtons button = MouseButtons.Left;
				int clicks = 0;
				switch (note.code)
				{
				case -6:
					button = MouseButtons.Right;
					clicks = 2;
					break;
				case -5:
					button = MouseButtons.Right;
					clicks = 1;
					break;
				case -3:
					button = MouseButtons.Left;
					clicks = 2;
					break;
				case -2:
					button = MouseButtons.Left;
					clicks = 1;
					break;
				}
				Point point = this.lastClick;
				StatusBarPanel statusBarPanel2 = (StatusBarPanel)this.panels[num2];
				StatusBarPanelClickEventArgs e = new StatusBarPanelClickEventArgs(statusBarPanel2, button, clicks, point.X, point.Y);
				this.OnPanelClick(e);
			}
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x0012E4BC File Offset: 0x0012D4BC
		private void WmNCHitTest(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.LParam);
			Rectangle bounds = base.Bounds;
			bool flag = true;
			if (num > bounds.X + bounds.Width - this.SizeGripWidth)
			{
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal is Form)
				{
					FormBorderStyle formBorderStyle = ((Form)parentInternal).FormBorderStyle;
					if (formBorderStyle != FormBorderStyle.Sizable && formBorderStyle != FormBorderStyle.SizableToolWindow)
					{
						flag = false;
					}
					if (!((Form)parentInternal).TopLevel || this.Dock != DockStyle.Bottom)
					{
						flag = false;
					}
					if (flag)
					{
						Control.ControlCollection controls = parentInternal.Controls;
						int count = controls.Count;
						for (int i = 0; i < count; i++)
						{
							Control control = controls[i];
							if (control != this && control.Dock == DockStyle.Bottom && control.Top > base.Top)
							{
								flag = false;
								break;
							}
						}
					}
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				base.WndProc(ref m);
				return;
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x0012E5B0 File Offset: 0x0012D5B0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 132)
			{
				if (msg != 78)
				{
					if (msg != 132)
					{
						goto IL_8D;
					}
					this.WmNCHitTest(ref m);
					return;
				}
			}
			else
			{
				if (msg == 8235)
				{
					this.WmDrawItem(ref m);
					return;
				}
				if (msg != 8270)
				{
					goto IL_8D;
				}
			}
			NativeMethods.NMHDR note = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
			switch (note.code)
			{
			case -6:
			case -5:
			case -3:
			case -2:
				this.WmNotifyNMClick(note);
				return;
			}
			base.WndProc(ref m);
			return;
			IL_8D:
			base.WndProc(ref m);
		}

		// Token: 0x04003629 RID: 13865
		private const int SIMPLE_INDEX = 255;

		// Token: 0x0400362A RID: 13866
		private int sizeGripWidth;

		// Token: 0x0400362B RID: 13867
		private static readonly object EVENT_PANELCLICK = new object();

		// Token: 0x0400362C RID: 13868
		private static readonly object EVENT_SBDRAWITEM = new object();

		// Token: 0x0400362D RID: 13869
		private bool showPanels;

		// Token: 0x0400362E RID: 13870
		private bool layoutDirty;

		// Token: 0x0400362F RID: 13871
		private int panelsRealized;

		// Token: 0x04003630 RID: 13872
		private bool sizeGrip = true;

		// Token: 0x04003631 RID: 13873
		private string simpleText;

		// Token: 0x04003632 RID: 13874
		private Point lastClick = new Point(0, 0);

		// Token: 0x04003633 RID: 13875
		private IList panels = new ArrayList();

		// Token: 0x04003634 RID: 13876
		private StatusBar.StatusBarPanelCollection panelsCollection;

		// Token: 0x04003635 RID: 13877
		private StatusBar.ControlToolTip tooltips;

		// Token: 0x04003636 RID: 13878
		private ToolTip mainToolTip;

		// Token: 0x04003637 RID: 13879
		private bool toolTipSet;

		// Token: 0x04003638 RID: 13880
		private static VisualStyleRenderer renderer = null;

		// Token: 0x02000627 RID: 1575
		[ListBindable(false)]
		public class StatusBarPanelCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600529E RID: 21150 RVA: 0x0012E66D File Offset: 0x0012D66D
			public StatusBarPanelCollection(StatusBar owner)
			{
				this.owner = owner;
			}

			// Token: 0x170010BD RID: 4285
			public virtual StatusBarPanel this[int index]
			{
				get
				{
					return (StatusBarPanel)this.owner.panels[index];
				}
				set
				{
					if (value == null)
					{
						throw new ArgumentNullException("StatusBarPanel");
					}
					this.owner.layoutDirty = true;
					if (value.Parent != null)
					{
						throw new ArgumentException(SR.GetString("ObjectHasParent"), "value");
					}
					int count = this.owner.panels.Count;
					if (index < 0 || index >= count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					StatusBarPanel statusBarPanel = (StatusBarPanel)this.owner.panels[index];
					statusBarPanel.ParentInternal = null;
					value.ParentInternal = this.owner;
					if (value.AutoSize == StatusBarPanelAutoSize.Contents)
					{
						value.Width = value.GetContentsWidth(true);
					}
					this.owner.panels[index] = value;
					value.Index = index;
					if (this.owner.ArePanelsRealized())
					{
						this.owner.PerformLayout();
						value.Realize();
					}
				}
			}

			// Token: 0x170010BE RID: 4286
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is StatusBarPanel)
					{
						this[index] = (StatusBarPanel)value;
						return;
					}
					throw new ArgumentException(SR.GetString("StatusBarBadStatusBarPanel"), "value");
				}
			}

			// Token: 0x170010BF RID: 4287
			public virtual StatusBarPanel this[string key]
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

			// Token: 0x170010C0 RID: 4288
			// (get) Token: 0x060052A4 RID: 21156 RVA: 0x0012E80D File Offset: 0x0012D80D
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Count
			{
				get
				{
					return this.owner.panels.Count;
				}
			}

			// Token: 0x170010C1 RID: 4289
			// (get) Token: 0x060052A5 RID: 21157 RVA: 0x0012E81F File Offset: 0x0012D81F
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170010C2 RID: 4290
			// (get) Token: 0x060052A6 RID: 21158 RVA: 0x0012E822 File Offset: 0x0012D822
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170010C3 RID: 4291
			// (get) Token: 0x060052A7 RID: 21159 RVA: 0x0012E825 File Offset: 0x0012D825
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170010C4 RID: 4292
			// (get) Token: 0x060052A8 RID: 21160 RVA: 0x0012E828 File Offset: 0x0012D828
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060052A9 RID: 21161 RVA: 0x0012E82C File Offset: 0x0012D82C
			public virtual StatusBarPanel Add(string text)
			{
				StatusBarPanel statusBarPanel = new StatusBarPanel();
				statusBarPanel.Text = text;
				this.Add(statusBarPanel);
				return statusBarPanel;
			}

			// Token: 0x060052AA RID: 21162 RVA: 0x0012E850 File Offset: 0x0012D850
			public virtual int Add(StatusBarPanel value)
			{
				int count = this.owner.panels.Count;
				this.Insert(count, value);
				return count;
			}

			// Token: 0x060052AB RID: 21163 RVA: 0x0012E877 File Offset: 0x0012D877
			int IList.Add(object value)
			{
				if (value is StatusBarPanel)
				{
					return this.Add((StatusBarPanel)value);
				}
				throw new ArgumentException(SR.GetString("StatusBarBadStatusBarPanel"), "value");
			}

			// Token: 0x060052AC RID: 21164 RVA: 0x0012E8A4 File Offset: 0x0012D8A4
			public virtual void AddRange(StatusBarPanel[] panels)
			{
				if (panels == null)
				{
					throw new ArgumentNullException("panels");
				}
				foreach (StatusBarPanel value in panels)
				{
					this.Add(value);
				}
			}

			// Token: 0x060052AD RID: 21165 RVA: 0x0012E8DB File Offset: 0x0012D8DB
			public bool Contains(StatusBarPanel panel)
			{
				return this.IndexOf(panel) != -1;
			}

			// Token: 0x060052AE RID: 21166 RVA: 0x0012E8EA File Offset: 0x0012D8EA
			bool IList.Contains(object panel)
			{
				return panel is StatusBarPanel && this.Contains((StatusBarPanel)panel);
			}

			// Token: 0x060052AF RID: 21167 RVA: 0x0012E902 File Offset: 0x0012D902
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x060052B0 RID: 21168 RVA: 0x0012E914 File Offset: 0x0012D914
			public int IndexOf(StatusBarPanel panel)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == panel)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060052B1 RID: 21169 RVA: 0x0012E93F File Offset: 0x0012D93F
			int IList.IndexOf(object panel)
			{
				if (panel is StatusBarPanel)
				{
					return this.IndexOf((StatusBarPanel)panel);
				}
				return -1;
			}

			// Token: 0x060052B2 RID: 21170 RVA: 0x0012E958 File Offset: 0x0012D958
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

			// Token: 0x060052B3 RID: 21171 RVA: 0x0012E9D8 File Offset: 0x0012D9D8
			public virtual void Insert(int index, StatusBarPanel value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.owner.layoutDirty = true;
				if (value.Parent != this.owner && value.Parent != null)
				{
					throw new ArgumentException(SR.GetString("ObjectHasParent"), "value");
				}
				int count = this.owner.panels.Count;
				if (index < 0 || index > count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				value.ParentInternal = this.owner;
				switch (value.AutoSize)
				{
				case StatusBarPanelAutoSize.Contents:
					value.Width = value.GetContentsWidth(true);
					break;
				}
				this.owner.panels.Insert(index, value);
				this.owner.UpdatePanelIndex();
				this.owner.ForcePanelUpdate();
			}

			// Token: 0x060052B4 RID: 21172 RVA: 0x0012EAD5 File Offset: 0x0012DAD5
			void IList.Insert(int index, object value)
			{
				if (value is StatusBarPanel)
				{
					this.Insert(index, (StatusBarPanel)value);
					return;
				}
				throw new ArgumentException(SR.GetString("StatusBarBadStatusBarPanel"), "value");
			}

			// Token: 0x060052B5 RID: 21173 RVA: 0x0012EB01 File Offset: 0x0012DB01
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x060052B6 RID: 21174 RVA: 0x0012EB12 File Offset: 0x0012DB12
			public virtual void Clear()
			{
				this.owner.RemoveAllPanelsWithoutUpdate();
				this.owner.PerformLayout();
			}

			// Token: 0x060052B7 RID: 21175 RVA: 0x0012EB2A File Offset: 0x0012DB2A
			public virtual void Remove(StatusBarPanel value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("StatusBarPanel");
				}
				if (value.Parent != this.owner)
				{
					return;
				}
				this.RemoveAt(value.Index);
			}

			// Token: 0x060052B8 RID: 21176 RVA: 0x0012EB55 File Offset: 0x0012DB55
			void IList.Remove(object value)
			{
				if (value is StatusBarPanel)
				{
					this.Remove((StatusBarPanel)value);
				}
			}

			// Token: 0x060052B9 RID: 21177 RVA: 0x0012EB6C File Offset: 0x0012DB6C
			public virtual void RemoveAt(int index)
			{
				int count = this.Count;
				if (index < 0 || index >= count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.owner.panels[index];
				this.owner.panels.RemoveAt(index);
				statusBarPanel.ParentInternal = null;
				this.owner.UpdateTooltip(statusBarPanel);
				this.owner.UpdatePanelIndex();
				this.owner.ForcePanelUpdate();
			}

			// Token: 0x060052BA RID: 21178 RVA: 0x0012EC10 File Offset: 0x0012DC10
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x060052BB RID: 21179 RVA: 0x0012EC35 File Offset: 0x0012DC35
			void ICollection.CopyTo(Array dest, int index)
			{
				this.owner.panels.CopyTo(dest, index);
			}

			// Token: 0x060052BC RID: 21180 RVA: 0x0012EC49 File Offset: 0x0012DC49
			public IEnumerator GetEnumerator()
			{
				if (this.owner.panels != null)
				{
					return this.owner.panels.GetEnumerator();
				}
				return new StatusBarPanel[0].GetEnumerator();
			}

			// Token: 0x04003639 RID: 13881
			private StatusBar owner;

			// Token: 0x0400363A RID: 13882
			private int lastAccessedIndex = -1;
		}

		// Token: 0x02000628 RID: 1576
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private class ControlToolTip
		{
			// Token: 0x060052BD RID: 21181 RVA: 0x0012EC74 File Offset: 0x0012DC74
			public ControlToolTip(Control parent)
			{
				this.window = new StatusBar.ControlToolTip.ToolTipNativeWindow(this);
				this.parent = parent;
			}

			// Token: 0x170010C5 RID: 4293
			// (get) Token: 0x060052BE RID: 21182 RVA: 0x0012EC9C File Offset: 0x0012DC9C
			protected CreateParams CreateParams
			{
				get
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 8
					});
					CreateParams createParams = new CreateParams();
					createParams.Parent = IntPtr.Zero;
					createParams.ClassName = "tooltips_class32";
					createParams.Style |= 1;
					createParams.ExStyle = 0;
					createParams.Caption = null;
					return createParams;
				}
			}

			// Token: 0x170010C6 RID: 4294
			// (get) Token: 0x060052BF RID: 21183 RVA: 0x0012ECF6 File Offset: 0x0012DCF6
			public IntPtr Handle
			{
				get
				{
					if (this.window.Handle == IntPtr.Zero)
					{
						this.CreateHandle();
					}
					return this.window.Handle;
				}
			}

			// Token: 0x170010C7 RID: 4295
			// (get) Token: 0x060052C0 RID: 21184 RVA: 0x0012ED20 File Offset: 0x0012DD20
			private bool IsHandleCreated
			{
				get
				{
					return this.window.Handle != IntPtr.Zero;
				}
			}

			// Token: 0x060052C1 RID: 21185 RVA: 0x0012ED37 File Offset: 0x0012DD37
			private void AssignId(StatusBar.ControlToolTip.Tool tool)
			{
				tool.id = (IntPtr)this.nextId;
				this.nextId++;
			}

			// Token: 0x060052C2 RID: 21186 RVA: 0x0012ED58 File Offset: 0x0012DD58
			public void SetTool(object key, StatusBar.ControlToolTip.Tool tool)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				StatusBar.ControlToolTip.Tool tool2 = null;
				if (this.tools.ContainsKey(key))
				{
					tool2 = (StatusBar.ControlToolTip.Tool)this.tools[key];
				}
				if (tool2 != null)
				{
					flag = true;
				}
				if (tool != null)
				{
					flag2 = true;
				}
				if (tool != null && tool2 != null && tool.id == tool2.id)
				{
					flag3 = true;
				}
				if (flag3)
				{
					this.UpdateTool(tool);
				}
				else
				{
					if (flag)
					{
						this.RemoveTool(tool2);
					}
					if (flag2)
					{
						this.AddTool(tool);
					}
				}
				if (tool != null)
				{
					this.tools[key] = tool;
					return;
				}
				this.tools.Remove(key);
			}

			// Token: 0x060052C3 RID: 21187 RVA: 0x0012EDEF File Offset: 0x0012DDEF
			public StatusBar.ControlToolTip.Tool GetTool(object key)
			{
				return (StatusBar.ControlToolTip.Tool)this.tools[key];
			}

			// Token: 0x060052C4 RID: 21188 RVA: 0x0012EE04 File Offset: 0x0012DE04
			private void AddTool(StatusBar.ControlToolTip.Tool tool)
			{
				if (tool != null && tool.text != null && tool.text.Length > 0)
				{
					StatusBar statusBar = (StatusBar)this.parent;
					int num;
					if (statusBar.ToolTipSet)
					{
						num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(statusBar.MainToolTip, statusBar.MainToolTip.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(tool));
					}
					else
					{
						num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(tool));
					}
					if (num == 0)
					{
						throw new InvalidOperationException(SR.GetString("StatusBarAddFailed"));
					}
				}
			}

			// Token: 0x060052C5 RID: 21189 RVA: 0x0012EEAC File Offset: 0x0012DEAC
			private void RemoveTool(StatusBar.ControlToolTip.Tool tool)
			{
				if (tool != null && tool.text != null && tool.text.Length > 0 && (int)tool.id >= 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetMinTOOLINFO(tool));
				}
			}

			// Token: 0x060052C6 RID: 21190 RVA: 0x0012EF00 File Offset: 0x0012DF00
			private void UpdateTool(StatusBar.ControlToolTip.Tool tool)
			{
				if (tool != null && tool.text != null && tool.text.Length > 0 && (int)tool.id >= 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, this.GetTOOLINFO(tool));
				}
			}

			// Token: 0x060052C7 RID: 21191 RVA: 0x0012EF54 File Offset: 0x0012DF54
			protected void CreateHandle()
			{
				if (this.IsHandleCreated)
				{
					return;
				}
				this.window.CreateHandle(this.CreateParams);
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
			}

			// Token: 0x060052C8 RID: 21192 RVA: 0x0012EFBD File Offset: 0x0012DFBD
			protected void DestroyHandle()
			{
				if (this.IsHandleCreated)
				{
					this.window.DestroyHandle();
					this.tools.Clear();
				}
			}

			// Token: 0x060052C9 RID: 21193 RVA: 0x0012EFDD File Offset: 0x0012DFDD
			public void Dispose()
			{
				this.DestroyHandle();
			}

			// Token: 0x060052CA RID: 21194 RVA: 0x0012EFE8 File Offset: 0x0012DFE8
			private NativeMethods.TOOLINFO_T GetMinTOOLINFO(StatusBar.ControlToolTip.Tool tool)
			{
				NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
				toolinfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				toolinfo_T.hwnd = this.parent.Handle;
				if ((int)tool.id < 0)
				{
					this.AssignId(tool);
				}
				StatusBar statusBar = (StatusBar)this.parent;
				if (statusBar != null && statusBar.ToolTipSet)
				{
					toolinfo_T.uId = this.parent.Handle;
				}
				else
				{
					toolinfo_T.uId = tool.id;
				}
				return toolinfo_T;
			}

			// Token: 0x060052CB RID: 21195 RVA: 0x0012F070 File Offset: 0x0012E070
			private NativeMethods.TOOLINFO_T GetTOOLINFO(StatusBar.ControlToolTip.Tool tool)
			{
				NativeMethods.TOOLINFO_T minTOOLINFO = this.GetMinTOOLINFO(tool);
				minTOOLINFO.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				minTOOLINFO.uFlags |= 272;
				Control control = this.parent;
				if (control != null && control.RightToLeft == RightToLeft.Yes)
				{
					minTOOLINFO.uFlags |= 4;
				}
				minTOOLINFO.lpszText = tool.text;
				minTOOLINFO.rect = NativeMethods.RECT.FromXYWH(tool.rect.X, tool.rect.Y, tool.rect.Width, tool.rect.Height);
				return minTOOLINFO;
			}

			// Token: 0x060052CC RID: 21196 RVA: 0x0012F114 File Offset: 0x0012E114
			~ControlToolTip()
			{
				this.DestroyHandle();
			}

			// Token: 0x060052CD RID: 21197 RVA: 0x0012F140 File Offset: 0x0012E140
			protected void WndProc(ref Message msg)
			{
				int msg2 = msg.Msg;
				if (msg2 == 7)
				{
					return;
				}
				this.window.DefWndProc(ref msg);
			}

			// Token: 0x0400363B RID: 13883
			private Hashtable tools = new Hashtable();

			// Token: 0x0400363C RID: 13884
			private StatusBar.ControlToolTip.ToolTipNativeWindow window;

			// Token: 0x0400363D RID: 13885
			private Control parent;

			// Token: 0x0400363E RID: 13886
			private int nextId;

			// Token: 0x02000629 RID: 1577
			public class Tool
			{
				// Token: 0x0400363F RID: 13887
				public Rectangle rect = Rectangle.Empty;

				// Token: 0x04003640 RID: 13888
				public string text;

				// Token: 0x04003641 RID: 13889
				internal IntPtr id = new IntPtr(-1);
			}

			// Token: 0x0200062A RID: 1578
			private class ToolTipNativeWindow : NativeWindow
			{
				// Token: 0x060052CF RID: 21199 RVA: 0x0012F184 File Offset: 0x0012E184
				internal ToolTipNativeWindow(StatusBar.ControlToolTip control)
				{
					this.control = control;
				}

				// Token: 0x060052D0 RID: 21200 RVA: 0x0012F193 File Offset: 0x0012E193
				protected override void WndProc(ref Message m)
				{
					if (this.control != null)
					{
						this.control.WndProc(ref m);
					}
				}

				// Token: 0x04003642 RID: 13890
				private StatusBar.ControlToolTip control;
			}
		}
	}
}
