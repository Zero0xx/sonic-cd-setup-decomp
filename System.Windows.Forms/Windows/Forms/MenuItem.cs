using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x020004B6 RID: 1206
	[DefaultProperty("Text")]
	[DesignTimeVisible(false)]
	[DefaultEvent("Click")]
	[ToolboxItem(false)]
	public class MenuItem : Menu
	{
		// Token: 0x06004815 RID: 18453 RVA: 0x00105E98 File Offset: 0x00104E98
		public MenuItem() : this(MenuMerge.Add, 0, Shortcut.None, null, null, null, null, null)
		{
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x00105EB4 File Offset: 0x00104EB4
		public MenuItem(string text) : this(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, null)
		{
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x00105ED0 File Offset: 0x00104ED0
		public MenuItem(string text, EventHandler onClick) : this(MenuMerge.Add, 0, Shortcut.None, text, onClick, null, null, null)
		{
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x00105EEC File Offset: 0x00104EEC
		public MenuItem(string text, EventHandler onClick, Shortcut shortcut) : this(MenuMerge.Add, 0, shortcut, text, onClick, null, null, null)
		{
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x00105F08 File Offset: 0x00104F08
		public MenuItem(string text, MenuItem[] items) : this(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, items)
		{
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x00105F23 File Offset: 0x00104F23
		internal MenuItem(MenuItem.MenuItemData data)
		{
			this.msaaMenuInfoPtr = IntPtr.Zero;
			base..ctor(null);
			data.AddItem(this);
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x00105F40 File Offset: 0x00104F40
		public MenuItem(MenuMerge mergeType, int mergeOrder, Shortcut shortcut, string text, EventHandler onClick, EventHandler onPopup, EventHandler onSelect, MenuItem[] items)
		{
			this.msaaMenuInfoPtr = IntPtr.Zero;
			base..ctor(items);
			new MenuItem.MenuItemData(this, mergeType, mergeOrder, shortcut, true, text, onClick, onPopup, onSelect, null, null);
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x0600481C RID: 18460 RVA: 0x00105F75 File Offset: 0x00104F75
		// (set) Token: 0x0600481D RID: 18461 RVA: 0x00105F8B File Offset: 0x00104F8B
		[Browsable(false)]
		[DefaultValue(false)]
		public bool BarBreak
		{
			get
			{
				return (this.data.State & 32) != 0;
			}
			set
			{
				this.data.SetState(32, value);
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x0600481E RID: 18462 RVA: 0x00105F9B File Offset: 0x00104F9B
		// (set) Token: 0x0600481F RID: 18463 RVA: 0x00105FB1 File Offset: 0x00104FB1
		[DefaultValue(false)]
		[Browsable(false)]
		public bool Break
		{
			get
			{
				return (this.data.State & 64) != 0;
			}
			set
			{
				this.data.SetState(64, value);
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06004820 RID: 18464 RVA: 0x00105FC1 File Offset: 0x00104FC1
		// (set) Token: 0x06004821 RID: 18465 RVA: 0x00105FD6 File Offset: 0x00104FD6
		[SRDescription("MenuItemCheckedDescr")]
		[DefaultValue(false)]
		public bool Checked
		{
			get
			{
				return (this.data.State & 8) != 0;
			}
			set
			{
				if (value && (base.ItemCount != 0 || (this.Parent != null && this.Parent is MainMenu)))
				{
					throw new ArgumentException(SR.GetString("MenuItemInvalidCheckProperty"));
				}
				this.data.SetState(8, value);
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06004822 RID: 18466 RVA: 0x00106015 File Offset: 0x00105015
		// (set) Token: 0x06004823 RID: 18467 RVA: 0x00106030 File Offset: 0x00105030
		[DefaultValue(false)]
		[SRDescription("MenuItemDefaultDescr")]
		public bool DefaultItem
		{
			get
			{
				return (this.data.State & 4096) != 0;
			}
			set
			{
				if (this.menu != null)
				{
					if (value)
					{
						UnsafeNativeMethods.SetMenuDefaultItem(new HandleRef(this.menu, this.menu.handle), this.MenuID, false);
					}
					else if (this.DefaultItem)
					{
						UnsafeNativeMethods.SetMenuDefaultItem(new HandleRef(this.menu, this.menu.handle), -1, false);
					}
				}
				this.data.SetState(4096, value);
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06004824 RID: 18468 RVA: 0x001060A4 File Offset: 0x001050A4
		// (set) Token: 0x06004825 RID: 18469 RVA: 0x001060BD File Offset: 0x001050BD
		[DefaultValue(false)]
		[SRDescription("MenuItemOwnerDrawDescr")]
		[SRCategory("CatBehavior")]
		public bool OwnerDraw
		{
			get
			{
				return (this.data.State & 256) != 0;
			}
			set
			{
				this.data.SetState(256, value);
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06004826 RID: 18470 RVA: 0x001060D0 File Offset: 0x001050D0
		// (set) Token: 0x06004827 RID: 18471 RVA: 0x001060E2 File Offset: 0x001050E2
		[Localizable(true)]
		[SRDescription("MenuItemEnabledDescr")]
		[DefaultValue(true)]
		public bool Enabled
		{
			get
			{
				return (this.data.State & 3) == 0;
			}
			set
			{
				this.data.SetState(3, !value);
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06004828 RID: 18472 RVA: 0x001060F4 File Offset: 0x001050F4
		// (set) Token: 0x06004829 RID: 18473 RVA: 0x00106134 File Offset: 0x00105134
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.menu != null)
				{
					for (int i = 0; i < this.menu.ItemCount; i++)
					{
						if (this.menu.items[i] == this)
						{
							return i;
						}
					}
				}
				return -1;
			}
			set
			{
				int index = this.Index;
				if (index >= 0)
				{
					if (value < 0 || value >= this.menu.ItemCount)
					{
						throw new ArgumentOutOfRangeException("Index", SR.GetString("InvalidArgument", new object[]
						{
							"Index",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value != index)
					{
						Menu menu = this.menu;
						menu.MenuItems.RemoveAt(index);
						menu.MenuItems.Add(value, this);
					}
				}
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x0600482A RID: 18474 RVA: 0x001061B8 File Offset: 0x001051B8
		[Browsable(false)]
		public override bool IsParent
		{
			get
			{
				bool flag = false;
				if (this.data != null && this.MdiList)
				{
					for (int i = 0; i < base.ItemCount; i++)
					{
						if (!(this.items[i].data.UserData is MenuItem.MdiListUserData))
						{
							flag = true;
							break;
						}
					}
					if (!flag && this.FindMdiForms().Length > 0)
					{
						flag = true;
					}
					if (!flag && this.menu != null && !(this.menu is MenuItem))
					{
						flag = true;
					}
				}
				else
				{
					flag = base.IsParent;
				}
				return flag;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x0600482B RID: 18475 RVA: 0x00106239 File Offset: 0x00105239
		// (set) Token: 0x0600482C RID: 18476 RVA: 0x00106252 File Offset: 0x00105252
		[SRDescription("MenuItemMDIListDescr")]
		[DefaultValue(false)]
		public bool MdiList
		{
			get
			{
				return (this.data.State & 131072) != 0;
			}
			set
			{
				this.data.MdiList = value;
				MenuItem.CleanListItems(this);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x0600482D RID: 18477 RVA: 0x00106266 File Offset: 0x00105266
		// (set) Token: 0x0600482E RID: 18478 RVA: 0x0010626E File Offset: 0x0010526E
		internal Menu Menu
		{
			get
			{
				return this.menu;
			}
			set
			{
				this.menu = value;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x0600482F RID: 18479 RVA: 0x00106277 File Offset: 0x00105277
		protected int MenuID
		{
			get
			{
				return this.data.GetMenuID();
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06004830 RID: 18480 RVA: 0x00106284 File Offset: 0x00105284
		internal bool Selected
		{
			get
			{
				if (this.menu == null)
				{
					return false;
				}
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
				menuiteminfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));
				menuiteminfo_T.fMask = 1;
				UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
				return (menuiteminfo_T.fState & 128) != 0;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06004831 RID: 18481 RVA: 0x001062F4 File Offset: 0x001052F4
		internal int MenuIndex
		{
			get
			{
				if (this.menu == null)
				{
					return -1;
				}
				int menuItemCount = UnsafeNativeMethods.GetMenuItemCount(new HandleRef(this.menu, this.menu.Handle));
				int menuID = this.MenuID;
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
				menuiteminfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));
				menuiteminfo_T.fMask = 6;
				for (int i = 0; i < menuItemCount; i++)
				{
					UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), i, true, menuiteminfo_T);
					if ((menuiteminfo_T.hSubMenu == IntPtr.Zero || menuiteminfo_T.hSubMenu == base.Handle) && menuiteminfo_T.wID == menuID)
					{
						return i;
					}
				}
				return -1;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06004832 RID: 18482 RVA: 0x001063AC File Offset: 0x001053AC
		// (set) Token: 0x06004833 RID: 18483 RVA: 0x001063B9 File Offset: 0x001053B9
		[DefaultValue(MenuMerge.Add)]
		[SRDescription("MenuItemMergeTypeDescr")]
		public MenuMerge MergeType
		{
			get
			{
				return this.data.mergeType;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MenuMerge));
				}
				this.data.MergeType = value;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06004834 RID: 18484 RVA: 0x001063ED File Offset: 0x001053ED
		// (set) Token: 0x06004835 RID: 18485 RVA: 0x001063FA File Offset: 0x001053FA
		[SRDescription("MenuItemMergeOrderDescr")]
		[DefaultValue(0)]
		public int MergeOrder
		{
			get
			{
				return this.data.mergeOrder;
			}
			set
			{
				this.data.MergeOrder = value;
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06004836 RID: 18486 RVA: 0x00106408 File Offset: 0x00105408
		[Browsable(false)]
		public char Mnemonic
		{
			get
			{
				return this.data.Mnemonic;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06004837 RID: 18487 RVA: 0x00106415 File Offset: 0x00105415
		[Browsable(false)]
		public Menu Parent
		{
			get
			{
				return this.menu;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06004838 RID: 18488 RVA: 0x0010641D File Offset: 0x0010541D
		// (set) Token: 0x06004839 RID: 18489 RVA: 0x00106436 File Offset: 0x00105436
		[SRDescription("MenuItemRadioCheckDescr")]
		[DefaultValue(false)]
		public bool RadioCheck
		{
			get
			{
				return (this.data.State & 512) != 0;
			}
			set
			{
				this.data.SetState(512, value);
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x0600483A RID: 18490 RVA: 0x00106449 File Offset: 0x00105449
		internal override bool RenderIsRightToLeft
		{
			get
			{
				return this.Parent != null && this.Parent.RenderIsRightToLeft;
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x0600483B RID: 18491 RVA: 0x00106460 File Offset: 0x00105460
		// (set) Token: 0x0600483C RID: 18492 RVA: 0x0010646D File Offset: 0x0010546D
		[Localizable(true)]
		[SRDescription("MenuItemTextDescr")]
		public string Text
		{
			get
			{
				return this.data.caption;
			}
			set
			{
				this.data.SetCaption(value);
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x0600483D RID: 18493 RVA: 0x0010647B File Offset: 0x0010547B
		// (set) Token: 0x0600483E RID: 18494 RVA: 0x00106488 File Offset: 0x00105488
		[SRDescription("MenuItemShortCutDescr")]
		[Localizable(true)]
		[DefaultValue(Shortcut.None)]
		public Shortcut Shortcut
		{
			get
			{
				return this.data.shortcut;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Shortcut), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Shortcut));
				}
				this.data.shortcut = value;
				this.UpdateMenuItem(true);
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x0600483F RID: 18495 RVA: 0x001064D5 File Offset: 0x001054D5
		// (set) Token: 0x06004840 RID: 18496 RVA: 0x001064E2 File Offset: 0x001054E2
		[Localizable(true)]
		[SRDescription("MenuItemShowShortCutDescr")]
		[DefaultValue(true)]
		public bool ShowShortcut
		{
			get
			{
				return this.data.showShortcut;
			}
			set
			{
				if (value != this.data.showShortcut)
				{
					this.data.showShortcut = value;
					this.UpdateMenuItem(true);
				}
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06004841 RID: 18497 RVA: 0x00106505 File Offset: 0x00105505
		// (set) Token: 0x06004842 RID: 18498 RVA: 0x0010651B File Offset: 0x0010551B
		[Localizable(true)]
		[DefaultValue(true)]
		[SRDescription("MenuItemVisibleDescr")]
		public bool Visible
		{
			get
			{
				return (this.data.State & 65536) == 0;
			}
			set
			{
				this.data.Visible = value;
			}
		}

		// Token: 0x14000289 RID: 649
		// (add) Token: 0x06004843 RID: 18499 RVA: 0x00106529 File Offset: 0x00105529
		// (remove) Token: 0x06004844 RID: 18500 RVA: 0x00106547 File Offset: 0x00105547
		[SRDescription("MenuItemOnClickDescr")]
		public event EventHandler Click
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onClick = (EventHandler)Delegate.Combine(menuItemData.onClick, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onClick = (EventHandler)Delegate.Remove(menuItemData.onClick, value);
			}
		}

		// Token: 0x1400028A RID: 650
		// (add) Token: 0x06004845 RID: 18501 RVA: 0x00106565 File Offset: 0x00105565
		// (remove) Token: 0x06004846 RID: 18502 RVA: 0x00106583 File Offset: 0x00105583
		[SRDescription("drawItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onDrawItem = (DrawItemEventHandler)Delegate.Combine(menuItemData.onDrawItem, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onDrawItem = (DrawItemEventHandler)Delegate.Remove(menuItemData.onDrawItem, value);
			}
		}

		// Token: 0x1400028B RID: 651
		// (add) Token: 0x06004847 RID: 18503 RVA: 0x001065A1 File Offset: 0x001055A1
		// (remove) Token: 0x06004848 RID: 18504 RVA: 0x001065BF File Offset: 0x001055BF
		[SRDescription("measureItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event MeasureItemEventHandler MeasureItem
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onMeasureItem = (MeasureItemEventHandler)Delegate.Combine(menuItemData.onMeasureItem, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onMeasureItem = (MeasureItemEventHandler)Delegate.Remove(menuItemData.onMeasureItem, value);
			}
		}

		// Token: 0x1400028C RID: 652
		// (add) Token: 0x06004849 RID: 18505 RVA: 0x001065DD File Offset: 0x001055DD
		// (remove) Token: 0x0600484A RID: 18506 RVA: 0x001065FB File Offset: 0x001055FB
		[SRDescription("MenuItemOnInitDescr")]
		public event EventHandler Popup
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onPopup = (EventHandler)Delegate.Combine(menuItemData.onPopup, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onPopup = (EventHandler)Delegate.Remove(menuItemData.onPopup, value);
			}
		}

		// Token: 0x1400028D RID: 653
		// (add) Token: 0x0600484B RID: 18507 RVA: 0x00106619 File Offset: 0x00105619
		// (remove) Token: 0x0600484C RID: 18508 RVA: 0x00106637 File Offset: 0x00105637
		[SRDescription("MenuItemOnSelectDescr")]
		public event EventHandler Select
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onSelect = (EventHandler)Delegate.Combine(menuItemData.onSelect, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onSelect = (EventHandler)Delegate.Remove(menuItemData.onSelect, value);
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00106658 File Offset: 0x00105658
		private static void CleanListItems(MenuItem senderMenu)
		{
			for (int i = senderMenu.MenuItems.Count - 1; i >= 0; i--)
			{
				MenuItem menuItem = senderMenu.MenuItems[i];
				if (menuItem.data.UserData is MenuItem.MdiListUserData)
				{
					menuItem.Dispose();
				}
			}
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x001066A4 File Offset: 0x001056A4
		public virtual MenuItem CloneMenu()
		{
			MenuItem menuItem = new MenuItem();
			menuItem.CloneMenu(this);
			return menuItem;
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x001066C0 File Offset: 0x001056C0
		protected void CloneMenu(MenuItem itemSrc)
		{
			base.CloneMenu(itemSrc);
			int state = itemSrc.data.State;
			new MenuItem.MenuItemData(this, itemSrc.MergeType, itemSrc.MergeOrder, itemSrc.Shortcut, itemSrc.ShowShortcut, itemSrc.Text, itemSrc.data.onClick, itemSrc.data.onPopup, itemSrc.data.onSelect, itemSrc.data.onDrawItem, itemSrc.data.onMeasureItem);
			this.data.SetState(state & 201579, true);
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x00106750 File Offset: 0x00105750
		internal virtual void CreateMenuItem()
		{
			if ((this.data.State & 65536) == 0)
			{
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = this.CreateMenuItemInfo();
				UnsafeNativeMethods.InsertMenuItem(new HandleRef(this.menu, this.menu.handle), -1, true, menuiteminfo_T);
				this.hasHandle = (menuiteminfo_T.hSubMenu != IntPtr.Zero);
				this.dataVersion = this.data.version;
				this.menuItemIsCreated = true;
				if (this.RenderIsRightToLeft)
				{
					this.Menu.UpdateRtl(true);
				}
			}
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x001067D8 File Offset: 0x001057D8
		private NativeMethods.MENUITEMINFO_T CreateMenuItemInfo()
		{
			NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
			menuiteminfo_T.fMask = 55;
			menuiteminfo_T.fType = (this.data.State & 864);
			bool flag = false;
			if (this.menu == base.GetMainMenu())
			{
				flag = true;
			}
			if (this.data.caption.Equals("-"))
			{
				if (flag)
				{
					this.data.caption = " ";
					menuiteminfo_T.fType |= 64;
				}
				else
				{
					menuiteminfo_T.fType |= 2048;
				}
			}
			menuiteminfo_T.fState = (this.data.State & 4107);
			menuiteminfo_T.wID = this.MenuID;
			if (this.IsParent)
			{
				menuiteminfo_T.hSubMenu = base.Handle;
			}
			menuiteminfo_T.hbmpChecked = IntPtr.Zero;
			menuiteminfo_T.hbmpUnchecked = IntPtr.Zero;
			if (this.uniqueID == 0U)
			{
				lock (MenuItem.allCreatedMenuItems)
				{
					this.uniqueID = (uint)Interlocked.Increment(ref MenuItem.nextUniqueID);
					MenuItem.allCreatedMenuItems.Add(this.uniqueID, new WeakReference(this));
				}
			}
			if (this.data.OwnerDraw)
			{
				menuiteminfo_T.dwItemData = this.AllocMsaaMenuInfo();
			}
			else
			{
				menuiteminfo_T.dwItemData = (IntPtr)((int)this.uniqueID);
			}
			if (this.data.showShortcut && this.data.shortcut != Shortcut.None && !this.IsParent && !flag)
			{
				menuiteminfo_T.dwTypeData = this.data.caption + "\t" + TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys)this.data.shortcut);
			}
			else
			{
				menuiteminfo_T.dwTypeData = ((this.data.caption.Length == 0) ? " " : this.data.caption);
			}
			menuiteminfo_T.cch = 0;
			return menuiteminfo_T;
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x001069D4 File Offset: 0x001059D4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.menu != null)
				{
					this.menu.MenuItems.Remove(this);
				}
				if (this.data != null)
				{
					this.data.RemoveItem(this);
				}
				lock (MenuItem.allCreatedMenuItems)
				{
					MenuItem.allCreatedMenuItems.Remove(this.uniqueID);
				}
				this.uniqueID = 0U;
			}
			this.FreeMsaaMenuInfo();
			base.Dispose(disposing);
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x00106A60 File Offset: 0x00105A60
		internal static MenuItem GetMenuItemFromUniqueID(uint uniqueID)
		{
			WeakReference weakReference = (WeakReference)MenuItem.allCreatedMenuItems[uniqueID];
			if (weakReference != null && weakReference.IsAlive)
			{
				return (MenuItem)weakReference.Target;
			}
			return null;
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x00106A9C File Offset: 0x00105A9C
		internal static MenuItem GetMenuItemFromItemData(IntPtr itemData)
		{
			uint num = (uint)((long)itemData);
			if (num == 0U)
			{
				return null;
			}
			if (num < 3221225472U)
			{
				num = ((MenuItem.MsaaMenuInfoWithId)Marshal.PtrToStructure(itemData, typeof(MenuItem.MsaaMenuInfoWithId))).uniqueID;
			}
			return MenuItem.GetMenuItemFromUniqueID(num);
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00106AE4 File Offset: 0x00105AE4
		private IntPtr AllocMsaaMenuInfo()
		{
			this.FreeMsaaMenuInfo();
			this.msaaMenuInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MenuItem.MsaaMenuInfoWithId)));
			MenuItem.MsaaMenuInfoWithId msaaMenuInfoWithId = new MenuItem.MsaaMenuInfoWithId(this.data.caption, this.uniqueID);
			Marshal.StructureToPtr(msaaMenuInfoWithId, this.msaaMenuInfoPtr, false);
			return this.msaaMenuInfoPtr;
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00106B41 File Offset: 0x00105B41
		private void FreeMsaaMenuInfo()
		{
			if (this.msaaMenuInfoPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(this.msaaMenuInfoPtr, typeof(MenuItem.MsaaMenuInfoWithId));
				Marshal.FreeHGlobal(this.msaaMenuInfoPtr);
				this.msaaMenuInfoPtr = IntPtr.Zero;
			}
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x00106B80 File Offset: 0x00105B80
		internal override void ItemsChanged(int change)
		{
			base.ItemsChanged(change);
			if (change == 0)
			{
				if (this.menu != null && this.menu.created)
				{
					this.UpdateMenuItem(true);
					base.CreateMenuItems();
					return;
				}
			}
			else
			{
				if (!this.hasHandle && this.IsParent)
				{
					this.UpdateMenuItem(true);
				}
				MainMenu mainMenu = base.GetMainMenu();
				if (mainMenu != null && (this.data.State & 512) == 0)
				{
					mainMenu.ItemsChanged(change, this);
				}
			}
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x00106BF8 File Offset: 0x00105BF8
		internal void ItemsChanged(int change, MenuItem item)
		{
			if (change == 4 && this.data != null && this.data.baseItem != null && this.data.baseItem.MenuItems.Contains(item))
			{
				if (this.menu != null && this.menu.created)
				{
					this.UpdateMenuItem(true);
					base.CreateMenuItems();
					return;
				}
				if (this.data != null)
				{
					for (MenuItem firstItem = this.data.firstItem; firstItem != null; firstItem = firstItem.nextLinkedItem)
					{
						if (firstItem.created)
						{
							MenuItem item2 = item.CloneMenu();
							item.data.AddItem(item2);
							firstItem.MenuItems.Add(item2);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00106CAC File Offset: 0x00105CAC
		internal Form[] FindMdiForms()
		{
			Form[] array = null;
			MainMenu mainMenu = base.GetMainMenu();
			Form form = null;
			if (mainMenu != null)
			{
				form = mainMenu.GetFormUnsafe();
			}
			if (form != null)
			{
				array = form.MdiChildren;
			}
			if (array == null)
			{
				array = new Form[0];
			}
			return array;
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x00106CE4 File Offset: 0x00105CE4
		private void PopulateMdiList()
		{
			this.data.SetState(512, true);
			try
			{
				MenuItem.CleanListItems(this);
				Form[] array = this.FindMdiForms();
				if (array != null && array.Length > 0)
				{
					Form activeMdiChild = base.GetMainMenu().GetFormUnsafe().ActiveMdiChild;
					if (this.MenuItems.Count > 0)
					{
						MenuItem menuItem = (MenuItem)Activator.CreateInstance(base.GetType());
						menuItem.data.UserData = new MenuItem.MdiListUserData();
						menuItem.Text = "-";
						this.MenuItems.Add(menuItem);
					}
					int num = 0;
					int num2 = 1;
					int num3 = 0;
					bool flag = false;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Visible)
						{
							num++;
							if ((flag && num3 < 9) || (!flag && num3 < 8) || array[i].Equals(activeMdiChild))
							{
								MenuItem menuItem2 = (MenuItem)Activator.CreateInstance(base.GetType());
								menuItem2.data.UserData = new MenuItem.MdiListFormData(this, i);
								if (array[i].Equals(activeMdiChild))
								{
									menuItem2.Checked = true;
									flag = true;
								}
								menuItem2.Text = string.Format(CultureInfo.CurrentUICulture, "&{0} {1}", new object[]
								{
									num2,
									array[i].Text
								});
								num2++;
								num3++;
								this.MenuItems.Add(menuItem2);
							}
						}
					}
					if (num > 9)
					{
						MenuItem menuItem3 = (MenuItem)Activator.CreateInstance(base.GetType());
						menuItem3.data.UserData = new MenuItem.MdiListMoreWindowsData(this);
						menuItem3.Text = SR.GetString("MDIMenuMoreWindows");
						this.MenuItems.Add(menuItem3);
					}
				}
			}
			finally
			{
				this.data.SetState(512, false);
			}
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x00106EDC File Offset: 0x00105EDC
		public virtual MenuItem MergeMenu()
		{
			MenuItem menuItem = (MenuItem)Activator.CreateInstance(base.GetType());
			this.data.AddItem(menuItem);
			menuItem.MergeMenu(this);
			return menuItem;
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x00106F0E File Offset: 0x00105F0E
		public void MergeMenu(MenuItem itemSrc)
		{
			base.MergeMenu(itemSrc);
			itemSrc.data.AddItem(this);
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x00106F24 File Offset: 0x00105F24
		protected virtual void OnClick(EventArgs e)
		{
			if (this.data.UserData is MenuItem.MdiListUserData)
			{
				((MenuItem.MdiListUserData)this.data.UserData).OnClick(e);
				return;
			}
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnClick(e);
				return;
			}
			if (this.data.onClick != null)
			{
				this.data.onClick(this, e);
			}
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00106F9C File Offset: 0x00105F9C
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnDrawItem(e);
				return;
			}
			if (this.data.onDrawItem != null)
			{
				this.data.onDrawItem(this, e);
			}
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00106FE8 File Offset: 0x00105FE8
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnMeasureItem(e);
				return;
			}
			if (this.data.onMeasureItem != null)
			{
				this.data.onMeasureItem(this, e);
			}
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00107034 File Offset: 0x00106034
		protected virtual void OnPopup(EventArgs e)
		{
			bool flag = false;
			for (int i = 0; i < base.ItemCount; i++)
			{
				if (this.items[i].MdiList)
				{
					flag = true;
					this.items[i].UpdateMenuItem(true);
				}
			}
			if (flag || (this.hasHandle && !this.IsParent))
			{
				this.UpdateMenuItem(true);
			}
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnPopup(e);
			}
			else if (this.data.onPopup != null)
			{
				this.data.onPopup(this, e);
			}
			for (int j = 0; j < base.ItemCount; j++)
			{
				this.items[j].UpdateMenuItemIfDirty();
			}
			if (this.MdiList)
			{
				this.PopulateMdiList();
			}
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x001070FC File Offset: 0x001060FC
		protected virtual void OnSelect(EventArgs e)
		{
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnSelect(e);
				return;
			}
			if (this.data.onSelect != null)
			{
				this.data.onSelect(this, e);
			}
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00107148 File Offset: 0x00106148
		protected virtual void OnInitMenuPopup(EventArgs e)
		{
			this.OnPopup(e);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x00107151 File Offset: 0x00106151
		internal virtual void _OnInitMenuPopup(EventArgs e)
		{
			this.OnInitMenuPopup(e);
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0010715A File Offset: 0x0010615A
		public void PerformClick()
		{
			this.OnClick(EventArgs.Empty);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x00107167 File Offset: 0x00106167
		public virtual void PerformSelect()
		{
			this.OnSelect(EventArgs.Empty);
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x00107174 File Offset: 0x00106174
		internal virtual bool ShortcutClick()
		{
			if (this.menu is MenuItem)
			{
				MenuItem menuItem = (MenuItem)this.menu;
				if (!menuItem.ShortcutClick() || this.menu != menuItem)
				{
					return false;
				}
			}
			if ((this.data.State & 3) != 0)
			{
				return false;
			}
			if (base.ItemCount > 0)
			{
				this.OnPopup(EventArgs.Empty);
			}
			else
			{
				this.OnClick(EventArgs.Empty);
			}
			return true;
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x001071E0 File Offset: 0x001061E0
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = string.Empty;
			if (this.data != null && this.data.caption != null)
			{
				str2 = this.data.caption;
			}
			return str + ", Text: " + str2;
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x00107227 File Offset: 0x00106227
		internal void UpdateMenuItemIfDirty()
		{
			if (this.dataVersion != this.data.version)
			{
				this.UpdateMenuItem(true);
			}
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x00107244 File Offset: 0x00106244
		internal void UpdateItemRtl(bool setRightToLeftBit)
		{
			if (!this.menuItemIsCreated)
			{
				return;
			}
			NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
			menuiteminfo_T.fMask = 21;
			menuiteminfo_T.dwTypeData = new string('\0', this.Text.Length + 2);
			menuiteminfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));
			menuiteminfo_T.cch = menuiteminfo_T.dwTypeData.Length - 1;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
			if (setRightToLeftBit)
			{
				menuiteminfo_T.fType |= 24576;
			}
			else
			{
				menuiteminfo_T.fType &= -24577;
			}
			UnsafeNativeMethods.SetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x0010731C File Offset: 0x0010631C
		internal void UpdateMenuItem(bool force)
		{
			if (this.menu == null || !this.menu.created)
			{
				return;
			}
			if (force || this.menu is MainMenu || this.menu is ContextMenu)
			{
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = this.CreateMenuItemInfo();
				UnsafeNativeMethods.SetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
				if (this.hasHandle && menuiteminfo_T.hSubMenu == IntPtr.Zero)
				{
					base.ClearHandles();
				}
				this.hasHandle = (menuiteminfo_T.hSubMenu != IntPtr.Zero);
				this.dataVersion = this.data.version;
				if (this.menu is MainMenu)
				{
					Form formUnsafe = ((MainMenu)this.menu).GetFormUnsafe();
					if (formUnsafe != null)
					{
						SafeNativeMethods.DrawMenuBar(new HandleRef(formUnsafe, formUnsafe.Handle));
					}
				}
			}
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x00107404 File Offset: 0x00106404
		internal void WmDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr intPtr = Control.SetUpPalette(drawitemstruct.hDC, false, false);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC);
				try
				{
					this.OnDrawItem(new DrawItemEventArgs(graphics, SystemInformation.MenuFont, Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom), this.Index, (DrawItemState)drawitemstruct.itemState));
				}
				finally
				{
					graphics.Dispose();
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SelectPalette(new HandleRef(null, drawitemstruct.hDC), new HandleRef(null, intPtr), 0);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x001074EC File Offset: 0x001064EC
		internal void WmMeasureItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			Graphics graphics = Graphics.FromHdcInternal(dc);
			MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, this.Index);
			try
			{
				this.OnMeasureItem(measureItemEventArgs);
			}
			finally
			{
				graphics.Dispose();
			}
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			measureitemstruct.itemHeight = measureItemEventArgs.ItemHeight;
			measureitemstruct.itemWidth = measureItemEventArgs.ItemWidth;
			Marshal.StructureToPtr(measureitemstruct, m.LParam, false);
			m.Result = (IntPtr)1;
		}

		// Token: 0x04002213 RID: 8723
		internal const int STATE_BARBREAK = 32;

		// Token: 0x04002214 RID: 8724
		internal const int STATE_BREAK = 64;

		// Token: 0x04002215 RID: 8725
		internal const int STATE_CHECKED = 8;

		// Token: 0x04002216 RID: 8726
		internal const int STATE_DEFAULT = 4096;

		// Token: 0x04002217 RID: 8727
		internal const int STATE_DISABLED = 3;

		// Token: 0x04002218 RID: 8728
		internal const int STATE_RADIOCHECK = 512;

		// Token: 0x04002219 RID: 8729
		internal const int STATE_HIDDEN = 65536;

		// Token: 0x0400221A RID: 8730
		internal const int STATE_MDILIST = 131072;

		// Token: 0x0400221B RID: 8731
		internal const int STATE_CLONE_MASK = 201579;

		// Token: 0x0400221C RID: 8732
		internal const int STATE_OWNERDRAW = 256;

		// Token: 0x0400221D RID: 8733
		internal const int STATE_INMDIPOPUP = 512;

		// Token: 0x0400221E RID: 8734
		internal const int STATE_HILITE = 128;

		// Token: 0x0400221F RID: 8735
		private const uint firstUniqueID = 3221225472U;

		// Token: 0x04002220 RID: 8736
		private Menu menu;

		// Token: 0x04002221 RID: 8737
		private bool hasHandle;

		// Token: 0x04002222 RID: 8738
		private MenuItem.MenuItemData data;

		// Token: 0x04002223 RID: 8739
		private int dataVersion;

		// Token: 0x04002224 RID: 8740
		private MenuItem nextLinkedItem;

		// Token: 0x04002225 RID: 8741
		private static Hashtable allCreatedMenuItems = new Hashtable();

		// Token: 0x04002226 RID: 8742
		private static long nextUniqueID = (long)((ulong)-1073741824);

		// Token: 0x04002227 RID: 8743
		private uint uniqueID;

		// Token: 0x04002228 RID: 8744
		private IntPtr msaaMenuInfoPtr;

		// Token: 0x04002229 RID: 8745
		private bool menuItemIsCreated;

		// Token: 0x020004B7 RID: 1207
		private struct MsaaMenuInfoWithId
		{
			// Token: 0x0600486E RID: 18542 RVA: 0x001075AB File Offset: 0x001065AB
			public MsaaMenuInfoWithId(string text, uint uniqueID)
			{
				this.msaaMenuInfo = new NativeMethods.MSAAMENUINFO(text);
				this.uniqueID = uniqueID;
			}

			// Token: 0x0400222A RID: 8746
			public NativeMethods.MSAAMENUINFO msaaMenuInfo;

			// Token: 0x0400222B RID: 8747
			public uint uniqueID;
		}

		// Token: 0x020004B8 RID: 1208
		internal class MenuItemData : ICommandExecutor
		{
			// Token: 0x0600486F RID: 18543 RVA: 0x001075C0 File Offset: 0x001065C0
			internal MenuItemData(MenuItem baseItem, MenuMerge mergeType, int mergeOrder, Shortcut shortcut, bool showShortcut, string caption, EventHandler onClick, EventHandler onPopup, EventHandler onSelect, DrawItemEventHandler onDrawItem, MeasureItemEventHandler onMeasureItem)
			{
				this.AddItem(baseItem);
				this.mergeType = mergeType;
				this.mergeOrder = mergeOrder;
				this.shortcut = shortcut;
				this.showShortcut = showShortcut;
				this.caption = ((caption == null) ? "" : caption);
				this.onClick = onClick;
				this.onPopup = onPopup;
				this.onSelect = onSelect;
				this.onDrawItem = onDrawItem;
				this.onMeasureItem = onMeasureItem;
				this.version = 1;
				this.mnemonic = -1;
			}

			// Token: 0x17000E77 RID: 3703
			// (get) Token: 0x06004870 RID: 18544 RVA: 0x00107641 File Offset: 0x00106641
			// (set) Token: 0x06004871 RID: 18545 RVA: 0x00107655 File Offset: 0x00106655
			internal bool OwnerDraw
			{
				get
				{
					return (this.State & 256) != 0;
				}
				set
				{
					this.SetState(256, value);
				}
			}

			// Token: 0x17000E78 RID: 3704
			// (get) Token: 0x06004872 RID: 18546 RVA: 0x00107663 File Offset: 0x00106663
			// (set) Token: 0x06004873 RID: 18547 RVA: 0x00107670 File Offset: 0x00106670
			internal bool MdiList
			{
				get
				{
					return this.HasState(131072);
				}
				set
				{
					if ((this.state & 131072) != 0 != value)
					{
						this.SetState(131072, value);
						for (MenuItem nextLinkedItem = this.firstItem; nextLinkedItem != null; nextLinkedItem = nextLinkedItem.nextLinkedItem)
						{
							nextLinkedItem.ItemsChanged(2);
						}
					}
				}
			}

			// Token: 0x17000E79 RID: 3705
			// (get) Token: 0x06004874 RID: 18548 RVA: 0x001076B8 File Offset: 0x001066B8
			// (set) Token: 0x06004875 RID: 18549 RVA: 0x001076C0 File Offset: 0x001066C0
			internal MenuMerge MergeType
			{
				get
				{
					return this.mergeType;
				}
				set
				{
					if (this.mergeType != value)
					{
						this.mergeType = value;
						this.ItemsChanged(3);
					}
				}
			}

			// Token: 0x17000E7A RID: 3706
			// (get) Token: 0x06004876 RID: 18550 RVA: 0x001076D9 File Offset: 0x001066D9
			// (set) Token: 0x06004877 RID: 18551 RVA: 0x001076E1 File Offset: 0x001066E1
			internal int MergeOrder
			{
				get
				{
					return this.mergeOrder;
				}
				set
				{
					if (this.mergeOrder != value)
					{
						this.mergeOrder = value;
						this.ItemsChanged(3);
					}
				}
			}

			// Token: 0x17000E7B RID: 3707
			// (get) Token: 0x06004878 RID: 18552 RVA: 0x001076FA File Offset: 0x001066FA
			internal char Mnemonic
			{
				get
				{
					if (this.mnemonic == -1)
					{
						this.mnemonic = (short)WindowsFormsUtils.GetMnemonic(this.caption, true);
					}
					return (char)this.mnemonic;
				}
			}

			// Token: 0x17000E7C RID: 3708
			// (get) Token: 0x06004879 RID: 18553 RVA: 0x0010771F File Offset: 0x0010671F
			internal int State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x17000E7D RID: 3709
			// (get) Token: 0x0600487A RID: 18554 RVA: 0x00107727 File Offset: 0x00106727
			// (set) Token: 0x0600487B RID: 18555 RVA: 0x00107738 File Offset: 0x00106738
			internal bool Visible
			{
				get
				{
					return (this.state & 65536) == 0;
				}
				set
				{
					if ((this.state & 65536) == 0 != value)
					{
						this.state = (value ? (this.state & -65537) : (this.state | 65536));
						this.ItemsChanged(1);
					}
				}
			}

			// Token: 0x17000E7E RID: 3710
			// (get) Token: 0x0600487C RID: 18556 RVA: 0x00107776 File Offset: 0x00106776
			// (set) Token: 0x0600487D RID: 18557 RVA: 0x0010777E File Offset: 0x0010677E
			internal object UserData
			{
				get
				{
					return this.userData;
				}
				set
				{
					this.userData = value;
				}
			}

			// Token: 0x0600487E RID: 18558 RVA: 0x00107788 File Offset: 0x00106788
			internal void AddItem(MenuItem item)
			{
				if (item.data != this)
				{
					if (item.data != null)
					{
						item.data.RemoveItem(item);
					}
					item.nextLinkedItem = this.firstItem;
					this.firstItem = item;
					if (this.baseItem == null)
					{
						this.baseItem = item;
					}
					item.data = this;
					item.dataVersion = 0;
					item.UpdateMenuItem(false);
				}
			}

			// Token: 0x0600487F RID: 18559 RVA: 0x001077E9 File Offset: 0x001067E9
			public void Execute()
			{
				if (this.baseItem != null)
				{
					this.baseItem.OnClick(EventArgs.Empty);
				}
			}

			// Token: 0x06004880 RID: 18560 RVA: 0x00107803 File Offset: 0x00106803
			internal int GetMenuID()
			{
				if (this.cmd == null)
				{
					this.cmd = new Command(this);
				}
				return this.cmd.ID;
			}

			// Token: 0x06004881 RID: 18561 RVA: 0x00107824 File Offset: 0x00106824
			internal void ItemsChanged(int change)
			{
				for (MenuItem nextLinkedItem = this.firstItem; nextLinkedItem != null; nextLinkedItem = nextLinkedItem.nextLinkedItem)
				{
					if (nextLinkedItem.menu != null)
					{
						nextLinkedItem.menu.ItemsChanged(change);
					}
				}
			}

			// Token: 0x06004882 RID: 18562 RVA: 0x00107858 File Offset: 0x00106858
			internal void RemoveItem(MenuItem item)
			{
				if (item == this.firstItem)
				{
					this.firstItem = item.nextLinkedItem;
				}
				else
				{
					MenuItem nextLinkedItem = this.firstItem;
					while (item != nextLinkedItem.nextLinkedItem)
					{
						nextLinkedItem = nextLinkedItem.nextLinkedItem;
					}
					nextLinkedItem.nextLinkedItem = item.nextLinkedItem;
				}
				item.nextLinkedItem = null;
				item.data = null;
				item.dataVersion = 0;
				if (item == this.baseItem)
				{
					this.baseItem = this.firstItem;
				}
				if (this.firstItem == null)
				{
					this.onClick = null;
					this.onPopup = null;
					this.onSelect = null;
					this.onDrawItem = null;
					this.onMeasureItem = null;
					if (this.cmd != null)
					{
						this.cmd.Dispose();
						this.cmd = null;
					}
				}
			}

			// Token: 0x06004883 RID: 18563 RVA: 0x00107910 File Offset: 0x00106910
			internal void SetCaption(string value)
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.caption.Equals(value))
				{
					this.caption = value;
					this.UpdateMenuItems();
				}
			}

			// Token: 0x06004884 RID: 18564 RVA: 0x00107937 File Offset: 0x00106937
			internal bool HasState(int flag)
			{
				return (this.State & flag) == flag;
			}

			// Token: 0x06004885 RID: 18565 RVA: 0x00107944 File Offset: 0x00106944
			internal void SetState(int flag, bool value)
			{
				if ((this.state & flag) != 0 != value)
				{
					this.state = (value ? (this.state | flag) : (this.state & ~flag));
					this.UpdateMenuItems();
				}
			}

			// Token: 0x06004886 RID: 18566 RVA: 0x0010797C File Offset: 0x0010697C
			internal void UpdateMenuItems()
			{
				this.version++;
				for (MenuItem nextLinkedItem = this.firstItem; nextLinkedItem != null; nextLinkedItem = nextLinkedItem.nextLinkedItem)
				{
					nextLinkedItem.UpdateMenuItem(true);
				}
			}

			// Token: 0x0400222C RID: 8748
			internal MenuItem baseItem;

			// Token: 0x0400222D RID: 8749
			internal MenuItem firstItem;

			// Token: 0x0400222E RID: 8750
			private int state;

			// Token: 0x0400222F RID: 8751
			internal int version;

			// Token: 0x04002230 RID: 8752
			internal MenuMerge mergeType;

			// Token: 0x04002231 RID: 8753
			internal int mergeOrder;

			// Token: 0x04002232 RID: 8754
			internal string caption;

			// Token: 0x04002233 RID: 8755
			internal short mnemonic;

			// Token: 0x04002234 RID: 8756
			internal Shortcut shortcut;

			// Token: 0x04002235 RID: 8757
			internal bool showShortcut;

			// Token: 0x04002236 RID: 8758
			internal EventHandler onClick;

			// Token: 0x04002237 RID: 8759
			internal EventHandler onPopup;

			// Token: 0x04002238 RID: 8760
			internal EventHandler onSelect;

			// Token: 0x04002239 RID: 8761
			internal DrawItemEventHandler onDrawItem;

			// Token: 0x0400223A RID: 8762
			internal MeasureItemEventHandler onMeasureItem;

			// Token: 0x0400223B RID: 8763
			private object userData;

			// Token: 0x0400223C RID: 8764
			internal Command cmd;
		}

		// Token: 0x020004B9 RID: 1209
		private class MdiListUserData
		{
			// Token: 0x06004887 RID: 18567 RVA: 0x001079B1 File Offset: 0x001069B1
			public virtual void OnClick(EventArgs e)
			{
			}
		}

		// Token: 0x020004BA RID: 1210
		private class MdiListFormData : MenuItem.MdiListUserData
		{
			// Token: 0x06004889 RID: 18569 RVA: 0x001079BB File Offset: 0x001069BB
			public MdiListFormData(MenuItem parentItem, int boundFormIndex)
			{
				this.boundIndex = boundFormIndex;
				this.parent = parentItem;
			}

			// Token: 0x0600488A RID: 18570 RVA: 0x001079D4 File Offset: 0x001069D4
			public override void OnClick(EventArgs e)
			{
				if (this.boundIndex != -1)
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						Form[] array = this.parent.FindMdiForms();
						if (array != null && array.Length > this.boundIndex)
						{
							Form form = array[this.boundIndex];
							form.Activate();
							if (form.ActiveControl != null && !form.ActiveControl.Focused)
							{
								form.ActiveControl.Focus();
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}

			// Token: 0x0400223D RID: 8765
			private MenuItem parent;

			// Token: 0x0400223E RID: 8766
			private int boundIndex;
		}

		// Token: 0x020004BB RID: 1211
		private class MdiListMoreWindowsData : MenuItem.MdiListUserData
		{
			// Token: 0x0600488B RID: 18571 RVA: 0x00107A58 File Offset: 0x00106A58
			public MdiListMoreWindowsData(MenuItem parent)
			{
				this.parent = parent;
			}

			// Token: 0x0600488C RID: 18572 RVA: 0x00107A68 File Offset: 0x00106A68
			public override void OnClick(EventArgs e)
			{
				Form[] array = this.parent.FindMdiForms();
				Form activeMdiChild = this.parent.GetMainMenu().GetFormUnsafe().ActiveMdiChild;
				if (array != null && array.Length > 0 && activeMdiChild != null)
				{
					IntSecurity.AllWindows.Assert();
					try
					{
						using (MdiWindowDialog mdiWindowDialog = new MdiWindowDialog())
						{
							mdiWindowDialog.SetItems(activeMdiChild, array);
							DialogResult dialogResult = mdiWindowDialog.ShowDialog();
							if (dialogResult == DialogResult.OK)
							{
								mdiWindowDialog.ActiveChildForm.Activate();
								if (mdiWindowDialog.ActiveChildForm.ActiveControl != null && !mdiWindowDialog.ActiveChildForm.ActiveControl.Focused)
								{
									mdiWindowDialog.ActiveChildForm.ActiveControl.Focus();
								}
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}

			// Token: 0x0400223F RID: 8767
			private MenuItem parent;
		}
	}
}
