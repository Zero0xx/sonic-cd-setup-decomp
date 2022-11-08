using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020004A5 RID: 1189
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionMenuStrip")]
	public class MenuStrip : ToolStrip
	{
		// Token: 0x06004743 RID: 18243 RVA: 0x00102C17 File Offset: 0x00101C17
		public MenuStrip()
		{
			this.CanOverflow = false;
			this.GripStyle = ToolStripGripStyle.Hidden;
			this.Stretch = true;
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06004744 RID: 18244 RVA: 0x00102C34 File Offset: 0x00101C34
		// (set) Token: 0x06004745 RID: 18245 RVA: 0x00102C3C File Offset: 0x00101C3C
		internal override bool KeyboardActive
		{
			get
			{
				return base.KeyboardActive;
			}
			set
			{
				if (base.KeyboardActive != value)
				{
					base.KeyboardActive = value;
					if (value)
					{
						this.OnMenuActivate(EventArgs.Empty);
						return;
					}
					this.OnMenuDeactivate(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06004746 RID: 18246 RVA: 0x00102C68 File Offset: 0x00101C68
		// (set) Token: 0x06004747 RID: 18247 RVA: 0x00102C70 File Offset: 0x00101C70
		[SRDescription("ToolStripCanOverflowDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[Browsable(false)]
		public new bool CanOverflow
		{
			get
			{
				return base.CanOverflow;
			}
			set
			{
				base.CanOverflow = value;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06004748 RID: 18248 RVA: 0x00102C79 File Offset: 0x00101C79
		protected override bool DefaultShowItemToolTips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06004749 RID: 18249 RVA: 0x00102C7C File Offset: 0x00101C7C
		protected override Padding DefaultGripMargin
		{
			get
			{
				return new Padding(2, 2, 0, 2);
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x0600474A RID: 18250 RVA: 0x00102C87 File Offset: 0x00101C87
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 24);
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x0600474B RID: 18251 RVA: 0x00102C95 File Offset: 0x00101C95
		protected override Padding DefaultPadding
		{
			get
			{
				if (this.GripStyle == ToolStripGripStyle.Visible)
				{
					return new Padding(3, 2, 0, 2);
				}
				return new Padding(6, 2, 0, 2);
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x0600474C RID: 18252 RVA: 0x00102CB3 File Offset: 0x00101CB3
		// (set) Token: 0x0600474D RID: 18253 RVA: 0x00102CBB File Offset: 0x00101CBB
		[DefaultValue(ToolStripGripStyle.Hidden)]
		public new ToolStripGripStyle GripStyle
		{
			get
			{
				return base.GripStyle;
			}
			set
			{
				base.GripStyle = value;
			}
		}

		// Token: 0x14000281 RID: 641
		// (add) Token: 0x0600474E RID: 18254 RVA: 0x00102CC4 File Offset: 0x00101CC4
		// (remove) Token: 0x0600474F RID: 18255 RVA: 0x00102CD7 File Offset: 0x00101CD7
		[SRCategory("CatBehavior")]
		[SRDescription("MenuStripMenuActivateDescr")]
		public event EventHandler MenuActivate
		{
			add
			{
				base.Events.AddHandler(MenuStrip.EventMenuActivate, value);
			}
			remove
			{
				base.Events.RemoveHandler(MenuStrip.EventMenuActivate, value);
			}
		}

		// Token: 0x14000282 RID: 642
		// (add) Token: 0x06004750 RID: 18256 RVA: 0x00102CEA File Offset: 0x00101CEA
		// (remove) Token: 0x06004751 RID: 18257 RVA: 0x00102CFD File Offset: 0x00101CFD
		[SRDescription("MenuStripMenuDeactivateDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler MenuDeactivate
		{
			add
			{
				base.Events.AddHandler(MenuStrip.EventMenuDeactivate, value);
			}
			remove
			{
				base.Events.RemoveHandler(MenuStrip.EventMenuDeactivate, value);
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06004752 RID: 18258 RVA: 0x00102D10 File Offset: 0x00101D10
		// (set) Token: 0x06004753 RID: 18259 RVA: 0x00102D18 File Offset: 0x00101D18
		[DefaultValue(false)]
		[SRDescription("ToolStripShowItemToolTipsDescr")]
		[SRCategory("CatBehavior")]
		public new bool ShowItemToolTips
		{
			get
			{
				return base.ShowItemToolTips;
			}
			set
			{
				base.ShowItemToolTips = value;
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06004754 RID: 18260 RVA: 0x00102D21 File Offset: 0x00101D21
		// (set) Token: 0x06004755 RID: 18261 RVA: 0x00102D29 File Offset: 0x00101D29
		[SRCategory("CatLayout")]
		[DefaultValue(true)]
		[SRDescription("ToolStripStretchDescr")]
		public new bool Stretch
		{
			get
			{
				return base.Stretch;
			}
			set
			{
				base.Stretch = value;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06004756 RID: 18262 RVA: 0x00102D32 File Offset: 0x00101D32
		// (set) Token: 0x06004757 RID: 18263 RVA: 0x00102D3A File Offset: 0x00101D3A
		[MergableProperty(false)]
		[DefaultValue(null)]
		[SRDescription("MenuStripMdiWindowListItem")]
		[TypeConverter(typeof(MdiWindowListItemConverter))]
		[SRCategory("CatBehavior")]
		public ToolStripMenuItem MdiWindowListItem
		{
			get
			{
				return this.mdiWindowListItem;
			}
			set
			{
				this.mdiWindowListItem = value;
			}
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x00102D43 File Offset: 0x00101D43
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new MenuStrip.MenuStripAccessibleObject(this);
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x00102D4B File Offset: 0x00101D4B
		protected internal override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			if (text == "-")
			{
				return new ToolStripSeparator();
			}
			return new ToolStripMenuItem(text, image, onClick);
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x00102D68 File Offset: 0x00101D68
		protected virtual void OnMenuActivate(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemMenuStart, -1);
			}
			EventHandler eventHandler = (EventHandler)base.Events[MenuStrip.EventMenuActivate];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x00102DA8 File Offset: 0x00101DA8
		protected virtual void OnMenuDeactivate(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemMenuEnd, -1);
			}
			EventHandler eventHandler = (EventHandler)base.Events[MenuStrip.EventMenuDeactivate];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x00102DE8 File Offset: 0x00101DE8
		internal bool OnMenuKey()
		{
			if (!this.Focused && !base.ContainsFocus)
			{
				ToolStripManager.ModalMenuFilter.SetActiveToolStrip(this, true);
				if (this.DisplayedItems.Count > 0)
				{
					if (this.DisplayedItems[0] is MdiControlStrip.SystemMenuItem)
					{
						base.SelectNextToolStripItem(this.DisplayedItems[0], true);
					}
					else
					{
						base.SelectNextToolStripItem(null, this.RightToLeft == RightToLeft.No);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00102E58 File Offset: 0x00101E58
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (ToolStripManager.ModalMenuFilter.InMenuMode && keyData == Keys.Space && (this.Focused || !base.ContainsFocus))
			{
				base.NotifySelectionChange(null);
				ToolStripManager.ModalMenuFilter.ExitMenuMode();
				UnsafeNativeMethods.PostMessage(WindowsFormsUtils.GetRootHWnd(this), 274, 61696, 32);
				return true;
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x00102EB0 File Offset: 0x00101EB0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 33 && base.ActiveDropDowns.Count == 0)
			{
				Point point = base.PointToClient(WindowsFormsUtils.LastCursorPoint);
				ToolStripItem itemAt = base.GetItemAt(point);
				if (itemAt != null && !(itemAt is ToolStripControlHost))
				{
					this.KeyboardActive = true;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x040021D7 RID: 8663
		private ToolStripMenuItem mdiWindowListItem;

		// Token: 0x040021D8 RID: 8664
		private static readonly object EventMenuActivate = new object();

		// Token: 0x040021D9 RID: 8665
		private static readonly object EventMenuDeactivate = new object();

		// Token: 0x020004A6 RID: 1190
		[ComVisible(true)]
		internal class MenuStripAccessibleObject : ToolStrip.ToolStripAccessibleObject
		{
			// Token: 0x06004760 RID: 18272 RVA: 0x00102F17 File Offset: 0x00101F17
			public MenuStripAccessibleObject(MenuStrip owner) : base(owner)
			{
			}

			// Token: 0x17000E3A RID: 3642
			// (get) Token: 0x06004761 RID: 18273 RVA: 0x00102F20 File Offset: 0x00101F20
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.MenuBar;
				}
			}
		}
	}
}
