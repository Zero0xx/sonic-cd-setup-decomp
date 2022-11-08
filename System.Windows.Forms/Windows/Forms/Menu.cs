using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200029E RID: 670
	[ListBindable(false)]
	[ToolboxItemFilter("System.Windows.Forms")]
	public abstract class Menu : Component
	{
		// Token: 0x0600241F RID: 9247 RVA: 0x0005311C File Offset: 0x0005211C
		protected Menu(MenuItem[] items)
		{
			if (items != null)
			{
				this.MenuItems.AddRange(items);
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x00053133 File Offset: 0x00052133
		[Browsable(false)]
		[SRDescription("ControlHandleDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					this.handle = this.CreateMenuHandle();
				}
				this.CreateMenuItems();
				return this.handle;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0005315F File Offset: 0x0005215F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("MenuIsParentDescr")]
		public virtual bool IsParent
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.items != null && this.ItemCount > 0;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x00053174 File Offset: 0x00052174
		internal int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x0005317C File Offset: 0x0005217C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MenuMDIListItemDescr")]
		public MenuItem MdiListItem
		{
			get
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					MenuItem menuItem = this.items[i];
					if (menuItem.MdiList)
					{
						return menuItem;
					}
					if (menuItem.IsParent)
					{
						menuItem = menuItem.MdiListItem;
						if (menuItem != null)
						{
							return menuItem;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000531C2 File Offset: 0x000521C2
		// (set) Token: 0x06002425 RID: 9253 RVA: 0x000531D0 File Offset: 0x000521D0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.name = null;
				}
				else
				{
					this.name = value;
				}
				if (this.Site != null)
				{
					this.Site.Name = this.name;
				}
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x00053206 File Offset: 0x00052206
		[Browsable(false)]
		[MergableProperty(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("MenuMenuItemsDescr")]
		public Menu.MenuItemCollection MenuItems
		{
			get
			{
				if (this.itemsCollection == null)
				{
					this.itemsCollection = new Menu.MenuItemCollection(this);
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x00053222 File Offset: 0x00052222
		internal virtual bool RenderIsRightToLeft
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x00053225 File Offset: 0x00052225
		// (set) Token: 0x06002429 RID: 9257 RVA: 0x0005322D File Offset: 0x0005222D
		[Bindable(true)]
		[DefaultValue(null)]
		[SRDescription("ControlTagDescr")]
		[SRCategory("CatData")]
		[Localizable(false)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
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

		// Token: 0x0600242A RID: 9258 RVA: 0x00053238 File Offset: 0x00052238
		internal void ClearHandles()
		{
			if (this.handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.DestroyMenu(new HandleRef(this, this.handle));
			}
			this.handle = IntPtr.Zero;
			if (this.created)
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					this.items[i].ClearHandles();
				}
				this.created = false;
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000532A4 File Offset: 0x000522A4
		protected internal void CloneMenu(Menu menuSrc)
		{
			MenuItem[] array = null;
			if (menuSrc.items != null)
			{
				int count = menuSrc.MenuItems.Count;
				array = new MenuItem[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = menuSrc.MenuItems[i].CloneMenu();
				}
			}
			this.MenuItems.Clear();
			if (array != null)
			{
				this.MenuItems.AddRange(array);
			}
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x00053308 File Offset: 0x00052308
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual IntPtr CreateMenuHandle()
		{
			return UnsafeNativeMethods.CreatePopupMenu();
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x00053310 File Offset: 0x00052310
		internal void CreateMenuItems()
		{
			if (!this.created)
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					this.items[i].CreateMenuItem();
				}
				this.created = true;
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0005334C File Offset: 0x0005234C
		internal void DestroyMenuItems()
		{
			if (this.created)
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					this.items[i].ClearHandles();
				}
				while (UnsafeNativeMethods.GetMenuItemCount(new HandleRef(this, this.handle)) > 0)
				{
					UnsafeNativeMethods.RemoveMenu(new HandleRef(this, this.handle), 0, 1024);
				}
				this.created = false;
			}
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000533B4 File Offset: 0x000523B4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				while (this.ItemCount > 0)
				{
					MenuItem menuItem = this.items[--this._itemCount];
					if (menuItem.Site != null && menuItem.Site.Container != null)
					{
						menuItem.Site.Container.Remove(menuItem);
					}
					menuItem.Menu = null;
					menuItem.Dispose();
				}
				this.items = null;
			}
			if (this.handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.DestroyMenu(new HandleRef(this, this.handle));
				this.handle = IntPtr.Zero;
				if (disposing)
				{
					this.ClearHandles();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x00053461 File Offset: 0x00052461
		public MenuItem FindMenuItem(int type, IntPtr value)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return this.FindMenuItemInternal(type, value);
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x00053478 File Offset: 0x00052478
		private MenuItem FindMenuItemInternal(int type, IntPtr value)
		{
			for (int i = 0; i < this.ItemCount; i++)
			{
				MenuItem menuItem = this.items[i];
				switch (type)
				{
				case 0:
					if (menuItem.handle == value)
					{
						return menuItem;
					}
					break;
				case 1:
					if (menuItem.Shortcut == (Shortcut)((int)value))
					{
						return menuItem;
					}
					break;
				}
				menuItem = menuItem.FindMenuItemInternal(type, value);
				if (menuItem != null)
				{
					return menuItem;
				}
			}
			return null;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000534E0 File Offset: 0x000524E0
		protected int FindMergePosition(int mergeOrder)
		{
			int i = 0;
			int num = this.ItemCount;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this.items[num2].MergeOrder <= mergeOrder)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0005351C File Offset: 0x0005251C
		internal int xFindMergePosition(int mergeOrder)
		{
			int result = 0;
			int num = 0;
			while (num < this.ItemCount && this.items[num].MergeOrder <= mergeOrder)
			{
				if (this.items[num].MergeOrder < mergeOrder)
				{
					result = num + 1;
				}
				else if (mergeOrder == this.items[num].MergeOrder)
				{
					result = num;
					break;
				}
				num++;
			}
			return result;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00053578 File Offset: 0x00052578
		internal void UpdateRtl(bool setRightToLeftBit)
		{
			foreach (object obj in this.MenuItems)
			{
				MenuItem menuItem = (MenuItem)obj;
				menuItem.UpdateItemRtl(setRightToLeftBit);
				menuItem.UpdateRtl(setRightToLeftBit);
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000535D8 File Offset: 0x000525D8
		public ContextMenu GetContextMenu()
		{
			Menu menu = this;
			while (!(menu is ContextMenu))
			{
				if (!(menu is MenuItem))
				{
					return null;
				}
				menu = ((MenuItem)menu).Menu;
			}
			return (ContextMenu)menu;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x00053610 File Offset: 0x00052610
		public MainMenu GetMainMenu()
		{
			Menu menu = this;
			while (!(menu is MainMenu))
			{
				if (!(menu is MenuItem))
				{
					return null;
				}
				menu = ((MenuItem)menu).Menu;
			}
			return (MainMenu)menu;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x00053648 File Offset: 0x00052648
		internal virtual void ItemsChanged(int change)
		{
			switch (change)
			{
			case 0:
			case 1:
				this.DestroyMenuItems();
				return;
			default:
				return;
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0005366C File Offset: 0x0005266C
		private IntPtr MatchKeyToMenuItem(int startItem, char key, Menu.MenuItemKeyComparer comparer)
		{
			int num = -1;
			bool flag = false;
			int num2 = 0;
			while (num2 < this.items.Length && !flag)
			{
				int num3 = (startItem + num2) % this.items.Length;
				MenuItem menuItem = this.items[num3];
				if (menuItem != null && comparer(menuItem, key))
				{
					if (num < 0)
					{
						num = menuItem.MenuIndex;
					}
					else
					{
						flag = true;
					}
				}
				num2++;
			}
			if (num < 0)
			{
				return IntPtr.Zero;
			}
			int high = flag ? 3 : 2;
			return (IntPtr)NativeMethods.Util.MAKELONG(num, high);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000536EC File Offset: 0x000526EC
		public virtual void MergeMenu(Menu menuSrc)
		{
			if (menuSrc == this)
			{
				throw new ArgumentException(SR.GetString("MenuMergeWithSelf"), "menuSrc");
			}
			if (menuSrc.items != null && this.items == null)
			{
				this.MenuItems.Clear();
			}
			for (int i = 0; i < menuSrc.ItemCount; i++)
			{
				MenuItem menuItem = menuSrc.items[i];
				switch (menuItem.MergeType)
				{
				case MenuMerge.Add:
					this.MenuItems.Add(this.FindMergePosition(menuItem.MergeOrder), menuItem.MergeMenu());
					break;
				case MenuMerge.Replace:
				case MenuMerge.MergeItems:
				{
					int mergeOrder = menuItem.MergeOrder;
					int j = this.xFindMergePosition(mergeOrder);
					while (j < this.ItemCount)
					{
						MenuItem menuItem2 = this.items[j];
						if (menuItem2.MergeOrder != mergeOrder)
						{
							this.MenuItems.Add(j, menuItem.MergeMenu());
							goto IL_125;
						}
						if (menuItem2.MergeType != MenuMerge.Add)
						{
							if (menuItem.MergeType != MenuMerge.MergeItems || menuItem2.MergeType != MenuMerge.MergeItems)
							{
								menuItem2.Dispose();
								this.MenuItems.Add(j, menuItem.MergeMenu());
								goto IL_125;
							}
							menuItem2.MergeMenu(menuItem);
							goto IL_125;
						}
						else
						{
							j++;
						}
					}
					this.MenuItems.Add(j, menuItem.MergeMenu());
					break;
				}
				}
				IL_125:;
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x00053830 File Offset: 0x00052830
		internal virtual bool ProcessInitMenuPopup(IntPtr handle)
		{
			MenuItem menuItem = this.FindMenuItemInternal(0, handle);
			if (menuItem != null)
			{
				menuItem._OnInitMenuPopup(EventArgs.Empty);
				menuItem.CreateMenuItems();
				return true;
			}
			return false;
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x00053860 File Offset: 0x00052860
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			MenuItem menuItem = this.FindMenuItemInternal(1, (IntPtr)((int)keyData));
			return menuItem != null && menuItem.ShortcutClick();
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x00053888 File Offset: 0x00052888
		internal int SelectedMenuItemIndex
		{
			get
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					MenuItem menuItem = this.items[i];
					if (menuItem != null && menuItem.Selected)
					{
						return i;
					}
				}
				return -1;
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000538C0 File Offset: 0x000528C0
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Items.Count: " + this.ItemCount.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000538F4 File Offset: 0x000528F4
		internal void WmMenuChar(ref Message m)
		{
			Menu menu = (m.LParam == this.handle) ? this : this.FindMenuItemInternal(0, m.LParam);
			if (menu == null)
			{
				return;
			}
			char key = char.ToUpper((char)NativeMethods.Util.LOWORD(m.WParam), CultureInfo.CurrentCulture);
			m.Result = menu.WmMenuCharInternal(key);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x00053950 File Offset: 0x00052950
		internal IntPtr WmMenuCharInternal(char key)
		{
			int startItem = (this.SelectedMenuItemIndex + 1) % this.items.Length;
			IntPtr intPtr = this.MatchKeyToMenuItem(startItem, key, new Menu.MenuItemKeyComparer(this.CheckOwnerDrawItemWithMnemonic));
			if (intPtr == IntPtr.Zero)
			{
				intPtr = this.MatchKeyToMenuItem(startItem, key, new Menu.MenuItemKeyComparer(this.CheckOwnerDrawItemNoMnemonic));
			}
			return intPtr;
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000539A7 File Offset: 0x000529A7
		private bool CheckOwnerDrawItemWithMnemonic(MenuItem mi, char key)
		{
			return mi.OwnerDraw && mi.Mnemonic == key;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000539BC File Offset: 0x000529BC
		private bool CheckOwnerDrawItemNoMnemonic(MenuItem mi, char key)
		{
			return mi.OwnerDraw && mi.Mnemonic == '\0' && mi.Text.Length > 0 && char.ToUpper(mi.Text[0], CultureInfo.CurrentCulture) == key;
		}

		// Token: 0x040015A3 RID: 5539
		internal const int CHANGE_ITEMS = 0;

		// Token: 0x040015A4 RID: 5540
		internal const int CHANGE_VISIBLE = 1;

		// Token: 0x040015A5 RID: 5541
		internal const int CHANGE_MDI = 2;

		// Token: 0x040015A6 RID: 5542
		internal const int CHANGE_MERGE = 3;

		// Token: 0x040015A7 RID: 5543
		internal const int CHANGE_ITEMADDED = 4;

		// Token: 0x040015A8 RID: 5544
		public const int FindHandle = 0;

		// Token: 0x040015A9 RID: 5545
		public const int FindShortcut = 1;

		// Token: 0x040015AA RID: 5546
		private Menu.MenuItemCollection itemsCollection;

		// Token: 0x040015AB RID: 5547
		internal MenuItem[] items;

		// Token: 0x040015AC RID: 5548
		private int _itemCount;

		// Token: 0x040015AD RID: 5549
		internal IntPtr handle;

		// Token: 0x040015AE RID: 5550
		internal bool created;

		// Token: 0x040015AF RID: 5551
		private object userData;

		// Token: 0x040015B0 RID: 5552
		private string name;

		// Token: 0x0200029F RID: 671
		// (Invoke) Token: 0x06002443 RID: 9283
		private delegate bool MenuItemKeyComparer(MenuItem mi, char key);

		// Token: 0x020002A0 RID: 672
		[ListBindable(false)]
		public class MenuItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06002446 RID: 9286 RVA: 0x000539F7 File Offset: 0x000529F7
			public MenuItemCollection(Menu owner)
			{
				this.owner = owner;
			}

			// Token: 0x1700058A RID: 1418
			public virtual MenuItem this[int index]
			{
				get
				{
					if (index < 0 || index >= this.owner.ItemCount)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.items[index];
				}
			}

			// Token: 0x1700058B RID: 1419
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x1700058C RID: 1420
			public virtual MenuItem this[string key]
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

			// Token: 0x1700058D RID: 1421
			// (get) Token: 0x0600244B RID: 9291 RVA: 0x00053AB1 File Offset: 0x00052AB1
			public int Count
			{
				get
				{
					return this.owner.ItemCount;
				}
			}

			// Token: 0x1700058E RID: 1422
			// (get) Token: 0x0600244C RID: 9292 RVA: 0x00053ABE File Offset: 0x00052ABE
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x1700058F RID: 1423
			// (get) Token: 0x0600244D RID: 9293 RVA: 0x00053AC1 File Offset: 0x00052AC1
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000590 RID: 1424
			// (get) Token: 0x0600244E RID: 9294 RVA: 0x00053AC4 File Offset: 0x00052AC4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000591 RID: 1425
			// (get) Token: 0x0600244F RID: 9295 RVA: 0x00053AC7 File Offset: 0x00052AC7
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002450 RID: 9296 RVA: 0x00053ACC File Offset: 0x00052ACC
			public virtual MenuItem Add(string caption)
			{
				MenuItem menuItem = new MenuItem(caption);
				this.Add(menuItem);
				return menuItem;
			}

			// Token: 0x06002451 RID: 9297 RVA: 0x00053AEC File Offset: 0x00052AEC
			public virtual MenuItem Add(string caption, EventHandler onClick)
			{
				MenuItem menuItem = new MenuItem(caption, onClick);
				this.Add(menuItem);
				return menuItem;
			}

			// Token: 0x06002452 RID: 9298 RVA: 0x00053B0C File Offset: 0x00052B0C
			public virtual MenuItem Add(string caption, MenuItem[] items)
			{
				MenuItem menuItem = new MenuItem(caption, items);
				this.Add(menuItem);
				return menuItem;
			}

			// Token: 0x06002453 RID: 9299 RVA: 0x00053B2A File Offset: 0x00052B2A
			public virtual int Add(MenuItem item)
			{
				return this.Add(this.owner.ItemCount, item);
			}

			// Token: 0x06002454 RID: 9300 RVA: 0x00053B40 File Offset: 0x00052B40
			public virtual int Add(int index, MenuItem item)
			{
				if (item.Menu != null)
				{
					if (this.owner is MenuItem)
					{
						for (MenuItem menuItem = (MenuItem)this.owner; menuItem != null; menuItem = (MenuItem)menuItem.Parent)
						{
							if (menuItem.Equals(item))
							{
								throw new ArgumentException(SR.GetString("MenuItemAlreadyExists", new object[]
								{
									item.Text
								}), "item");
							}
							if (!(menuItem.Parent is MenuItem))
							{
								break;
							}
						}
					}
					if (item.Menu.Equals(this.owner) && index > 0)
					{
						index--;
					}
					item.Menu.MenuItems.Remove(item);
				}
				if (index < 0 || index > this.owner.ItemCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.items == null || this.owner.items.Length == this.owner.ItemCount)
				{
					MenuItem[] array = new MenuItem[(this.owner.ItemCount < 2) ? 4 : (this.owner.ItemCount * 2)];
					if (this.owner.ItemCount > 0)
					{
						Array.Copy(this.owner.items, 0, array, 0, this.owner.ItemCount);
					}
					this.owner.items = array;
				}
				Array.Copy(this.owner.items, index, this.owner.items, index + 1, this.owner.ItemCount - index);
				this.owner.items[index] = item;
				this.owner._itemCount++;
				item.Menu = this.owner;
				this.owner.ItemsChanged(0);
				if (this.owner is MenuItem)
				{
					((MenuItem)this.owner).ItemsChanged(4, item);
				}
				return index;
			}

			// Token: 0x06002455 RID: 9301 RVA: 0x00053D3C File Offset: 0x00052D3C
			public virtual void AddRange(MenuItem[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				foreach (MenuItem item in items)
				{
					this.Add(item);
				}
			}

			// Token: 0x06002456 RID: 9302 RVA: 0x00053D73 File Offset: 0x00052D73
			int IList.Add(object value)
			{
				if (value is MenuItem)
				{
					return this.Add((MenuItem)value);
				}
				throw new ArgumentException(SR.GetString("MenuBadMenuItem"), "value");
			}

			// Token: 0x06002457 RID: 9303 RVA: 0x00053D9E File Offset: 0x00052D9E
			public bool Contains(MenuItem value)
			{
				return this.IndexOf(value) != -1;
			}

			// Token: 0x06002458 RID: 9304 RVA: 0x00053DAD File Offset: 0x00052DAD
			bool IList.Contains(object value)
			{
				return value is MenuItem && this.Contains((MenuItem)value);
			}

			// Token: 0x06002459 RID: 9305 RVA: 0x00053DC5 File Offset: 0x00052DC5
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x0600245A RID: 9306 RVA: 0x00053DD4 File Offset: 0x00052DD4
			public MenuItem[] Find(string key, bool searchAllChildren)
			{
				if (key == null || key.Length == 0)
				{
					throw new ArgumentNullException("key", SR.GetString("FindKeyMayNotBeEmptyOrNull"));
				}
				ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
				MenuItem[] array = new MenuItem[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}

			// Token: 0x0600245B RID: 9307 RVA: 0x00053E28 File Offset: 0x00052E28
			private ArrayList FindInternal(string key, bool searchAllChildren, Menu.MenuItemCollection menuItemsToLookIn, ArrayList foundMenuItems)
			{
				if (menuItemsToLookIn == null || foundMenuItems == null)
				{
					return null;
				}
				for (int i = 0; i < menuItemsToLookIn.Count; i++)
				{
					if (menuItemsToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(menuItemsToLookIn[i].Name, key, true))
					{
						foundMenuItems.Add(menuItemsToLookIn[i]);
					}
				}
				if (searchAllChildren)
				{
					for (int j = 0; j < menuItemsToLookIn.Count; j++)
					{
						if (menuItemsToLookIn[j] != null && menuItemsToLookIn[j].MenuItems != null && menuItemsToLookIn[j].MenuItems.Count > 0)
						{
							foundMenuItems = this.FindInternal(key, searchAllChildren, menuItemsToLookIn[j].MenuItems, foundMenuItems);
						}
					}
				}
				return foundMenuItems;
			}

			// Token: 0x0600245C RID: 9308 RVA: 0x00053ED8 File Offset: 0x00052ED8
			public int IndexOf(MenuItem value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == value)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x0600245D RID: 9309 RVA: 0x00053F03 File Offset: 0x00052F03
			int IList.IndexOf(object value)
			{
				if (value is MenuItem)
				{
					return this.IndexOf((MenuItem)value);
				}
				return -1;
			}

			// Token: 0x0600245E RID: 9310 RVA: 0x00053F1C File Offset: 0x00052F1C
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

			// Token: 0x0600245F RID: 9311 RVA: 0x00053F99 File Offset: 0x00052F99
			void IList.Insert(int index, object value)
			{
				if (value is MenuItem)
				{
					this.Add(index, (MenuItem)value);
					return;
				}
				throw new ArgumentException(SR.GetString("MenuBadMenuItem"), "value");
			}

			// Token: 0x06002460 RID: 9312 RVA: 0x00053FC6 File Offset: 0x00052FC6
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x06002461 RID: 9313 RVA: 0x00053FD8 File Offset: 0x00052FD8
			public virtual void Clear()
			{
				if (this.owner.ItemCount > 0)
				{
					for (int i = 0; i < this.owner.ItemCount; i++)
					{
						this.owner.items[i].Menu = null;
					}
					this.owner._itemCount = 0;
					this.owner.items = null;
					this.owner.ItemsChanged(0);
					if (this.owner is MenuItem)
					{
						((MenuItem)this.owner).UpdateMenuItem(true);
					}
				}
			}

			// Token: 0x06002462 RID: 9314 RVA: 0x0005405E File Offset: 0x0005305E
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.ItemCount > 0)
				{
					Array.Copy(this.owner.items, 0, dest, index, this.owner.ItemCount);
				}
			}

			// Token: 0x06002463 RID: 9315 RVA: 0x0005408C File Offset: 0x0005308C
			public IEnumerator GetEnumerator()
			{
				return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.items, this.owner.ItemCount);
			}

			// Token: 0x06002464 RID: 9316 RVA: 0x000540AC File Offset: 0x000530AC
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= this.owner.ItemCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				MenuItem menuItem = this.owner.items[index];
				menuItem.Menu = null;
				this.owner._itemCount--;
				Array.Copy(this.owner.items, index + 1, this.owner.items, index, this.owner.ItemCount - index);
				this.owner.items[this.owner.ItemCount] = null;
				this.owner.ItemsChanged(0);
				if (this.owner.ItemCount == 0)
				{
					this.Clear();
				}
			}

			// Token: 0x06002465 RID: 9317 RVA: 0x0005418C File Offset: 0x0005318C
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x06002466 RID: 9318 RVA: 0x000541B1 File Offset: 0x000531B1
			public virtual void Remove(MenuItem item)
			{
				if (item.Menu == this.owner)
				{
					this.RemoveAt(item.Index);
				}
			}

			// Token: 0x06002467 RID: 9319 RVA: 0x000541CD File Offset: 0x000531CD
			void IList.Remove(object value)
			{
				if (value is MenuItem)
				{
					this.Remove((MenuItem)value);
				}
			}

			// Token: 0x040015B1 RID: 5553
			private Menu owner;

			// Token: 0x040015B2 RID: 5554
			private int lastAccessedIndex = -1;
		}
	}
}
