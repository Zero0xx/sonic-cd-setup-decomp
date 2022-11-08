using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200069C RID: 1692
	[Editor("System.Windows.Forms.Design.ToolStripCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ListBindable(false)]
	[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
	public class ToolStripItemCollection : ArrangedElementCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x06005916 RID: 22806 RVA: 0x00143E2F File Offset: 0x00142E2F
		internal ToolStripItemCollection(ToolStrip owner, bool itemsCollection) : this(owner, itemsCollection, false)
		{
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x00143E3A File Offset: 0x00142E3A
		internal ToolStripItemCollection(ToolStrip owner, bool itemsCollection, bool isReadOnly)
		{
			this.lastAccessedIndex = -1;
			base..ctor();
			this.owner = owner;
			this.itemsCollection = itemsCollection;
			this.isReadOnly = isReadOnly;
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x00143E5E File Offset: 0x00142E5E
		public ToolStripItemCollection(ToolStrip owner, ToolStripItem[] value)
		{
			this.lastAccessedIndex = -1;
			base..ctor();
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
			this.AddRange(value);
		}

		// Token: 0x17001273 RID: 4723
		public virtual ToolStripItem this[int index]
		{
			get
			{
				return (ToolStripItem)base.InnerList[index];
			}
		}

		// Token: 0x17001274 RID: 4724
		public virtual ToolStripItem this[string key]
		{
			get
			{
				if (key == null || key.Length == 0)
				{
					return null;
				}
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					return (ToolStripItem)base.InnerList[index];
				}
				return null;
			}
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x00143EDA File Offset: 0x00142EDA
		public ToolStripItem Add(string text)
		{
			return this.Add(text, null, null);
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x00143EE5 File Offset: 0x00142EE5
		public ToolStripItem Add(Image image)
		{
			return this.Add(null, image, null);
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x00143EF0 File Offset: 0x00142EF0
		public ToolStripItem Add(string text, Image image)
		{
			return this.Add(text, image, null);
		}

		// Token: 0x0600591E RID: 22814 RVA: 0x00143EFC File Offset: 0x00142EFC
		public ToolStripItem Add(string text, Image image, EventHandler onClick)
		{
			ToolStripItem toolStripItem = this.owner.CreateDefaultItem(text, image, onClick);
			this.Add(toolStripItem);
			return toolStripItem;
		}

		// Token: 0x0600591F RID: 22815 RVA: 0x00143F24 File Offset: 0x00142F24
		public int Add(ToolStripItem value)
		{
			this.CheckCanAddOrInsertItem(value);
			this.SetOwner(value);
			int result = base.InnerList.Add(value);
			if (this.itemsCollection && this.owner != null)
			{
				this.owner.OnItemAdded(new ToolStripItemEventArgs(value));
			}
			return result;
		}

		// Token: 0x06005920 RID: 22816 RVA: 0x00143F70 File Offset: 0x00142F70
		public void AddRange(ToolStripItem[] toolStripItems)
		{
			if (toolStripItems == null)
			{
				throw new ArgumentNullException("toolStripItems");
			}
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			using (new LayoutTransaction(this.owner, this.owner, PropertyNames.Items))
			{
				for (int i = 0; i < toolStripItems.Length; i++)
				{
					this.Add(toolStripItems[i]);
				}
			}
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x00143FF0 File Offset: 0x00142FF0
		public void AddRange(ToolStripItemCollection toolStripItems)
		{
			if (toolStripItems == null)
			{
				throw new ArgumentNullException("toolStripItems");
			}
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			using (new LayoutTransaction(this.owner, this.owner, PropertyNames.Items))
			{
				int count = toolStripItems.Count;
				for (int i = 0; i < count; i++)
				{
					this.Add(toolStripItems[i]);
				}
			}
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x00144078 File Offset: 0x00143078
		public bool Contains(ToolStripItem value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x00144088 File Offset: 0x00143088
		public virtual void Clear()
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			if (this.Count == 0)
			{
				return;
			}
			ToolStripOverflow toolStripOverflow = null;
			if (this.owner != null && !this.owner.IsDisposingItems)
			{
				this.owner.SuspendLayout();
				toolStripOverflow = this.owner.GetOverflow();
				if (toolStripOverflow != null)
				{
					toolStripOverflow.SuspendLayout();
				}
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
				if (toolStripOverflow != null)
				{
					toolStripOverflow.ResumeLayout(false);
				}
				if (this.owner != null && !this.owner.IsDisposingItems)
				{
					this.owner.ResumeLayout();
				}
			}
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x00144144 File Offset: 0x00143144
		public virtual bool ContainsKey(string key)
		{
			return this.IsValidIndex(this.IndexOfKey(key));
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x00144154 File Offset: 0x00143154
		private void CheckCanAddOrInsertItem(ToolStripItem value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			ToolStripDropDown toolStripDropDown = this.owner as ToolStripDropDown;
			if (toolStripDropDown != null)
			{
				if (toolStripDropDown.OwnerItem == value)
				{
					throw new NotSupportedException(SR.GetString("ToolStripItemCircularReference"));
				}
				if (value is ToolStripControlHost && !(value is ToolStripScrollButton) && toolStripDropDown.IsRestrictedWindow)
				{
					IntSecurity.AllWindows.Demand();
				}
			}
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x001441D4 File Offset: 0x001431D4
		public ToolStripItem[] Find(string key, bool searchAllChildren)
		{
			if (key == null || key.Length == 0)
			{
				throw new ArgumentNullException("key", SR.GetString("FindKeyMayNotBeEmptyOrNull"));
			}
			ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
			ToolStripItem[] array = new ToolStripItem[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x00144228 File Offset: 0x00143228
		private ArrayList FindInternal(string key, bool searchAllChildren, ToolStripItemCollection itemsToLookIn, ArrayList foundItems)
		{
			if (itemsToLookIn == null || foundItems == null)
			{
				return null;
			}
			try
			{
				for (int i = 0; i < itemsToLookIn.Count; i++)
				{
					if (itemsToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(itemsToLookIn[i].Name, key, true))
					{
						foundItems.Add(itemsToLookIn[i]);
					}
				}
				if (searchAllChildren)
				{
					for (int j = 0; j < itemsToLookIn.Count; j++)
					{
						ToolStripDropDownItem toolStripDropDownItem = itemsToLookIn[j] as ToolStripDropDownItem;
						if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
						{
							foundItems = this.FindInternal(key, searchAllChildren, toolStripDropDownItem.DropDownItems, foundItems);
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			return foundItems;
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06005928 RID: 22824 RVA: 0x001442DC File Offset: 0x001432DC
		public override bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x001442E4 File Offset: 0x001432E4
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x0600592A RID: 22826 RVA: 0x001442EC File Offset: 0x001432EC
		bool IList.IsFixedSize
		{
			get
			{
				return base.InnerList.IsFixedSize;
			}
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x001442F9 File Offset: 0x001432F9
		bool IList.Contains(object value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x00144307 File Offset: 0x00143307
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x00144310 File Offset: 0x00143310
		void IList.Remove(object value)
		{
			this.Remove(value as ToolStripItem);
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x0014431E File Offset: 0x0014331E
		int IList.Add(object value)
		{
			return this.Add(value as ToolStripItem);
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x0014432C File Offset: 0x0014332C
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ToolStripItem);
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x0014433A File Offset: 0x0014333A
		void IList.Insert(int index, object value)
		{
			this.Insert(index, value as ToolStripItem);
		}

		// Token: 0x17001277 RID: 4727
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

		// Token: 0x06005933 RID: 22835 RVA: 0x00144368 File Offset: 0x00143368
		public void Insert(int index, ToolStripItem value)
		{
			this.CheckCanAddOrInsertItem(value);
			this.SetOwner(value);
			base.InnerList.Insert(index, value);
			if (this.itemsCollection && this.owner != null)
			{
				if (this.owner.IsHandleCreated)
				{
					LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
				}
				else
				{
					CommonProperties.xClearPreferredSizeCache(this.owner);
				}
				this.owner.OnItemAdded(new ToolStripItemEventArgs(value));
			}
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x001443DC File Offset: 0x001433DC
		public int IndexOf(ToolStripItem value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x06005935 RID: 22837 RVA: 0x001443EC File Offset: 0x001433EC
		public virtual int IndexOfKey(string key)
		{
			if (key == null || key.Length == 0)
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

		// Token: 0x06005936 RID: 22838 RVA: 0x0014446C File Offset: 0x0014346C
		private bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.Count;
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x00144480 File Offset: 0x00143480
		private void OnAfterRemove(ToolStripItem item)
		{
			if (this.itemsCollection)
			{
				ToolStrip toolStrip = null;
				if (item != null)
				{
					toolStrip = item.ParentInternal;
					item.SetOwner(null);
				}
				if (this.owner != null && !this.owner.IsDisposingItems)
				{
					ToolStripItemEventArgs e = new ToolStripItemEventArgs(item);
					this.owner.OnItemRemoved(e);
					if (toolStrip != null && toolStrip != this.owner)
					{
						toolStrip.OnItemVisibleChanged(e, false);
					}
				}
			}
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x001444E4 File Offset: 0x001434E4
		public void Remove(ToolStripItem value)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			base.InnerList.Remove(value);
			this.OnAfterRemove(value);
		}

		// Token: 0x06005939 RID: 22841 RVA: 0x00144514 File Offset: 0x00143514
		public void RemoveAt(int index)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			ToolStripItem item = null;
			if (index < this.Count && index >= 0)
			{
				item = (ToolStripItem)base.InnerList[index];
			}
			base.InnerList.RemoveAt(index);
			this.OnAfterRemove(item);
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x00144570 File Offset: 0x00143570
		public virtual void RemoveByKey(string key)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("ToolStripItemCollectionIsReadOnly"));
			}
			int index = this.IndexOfKey(key);
			if (this.IsValidIndex(index))
			{
				this.RemoveAt(index);
			}
		}

		// Token: 0x0600593B RID: 22843 RVA: 0x001445AD File Offset: 0x001435AD
		public void CopyTo(ToolStripItem[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x001445BC File Offset: 0x001435BC
		internal void MoveItem(ToolStripItem value)
		{
			if (value.ParentInternal != null)
			{
				int num = value.ParentInternal.Items.IndexOf(value);
				if (num >= 0)
				{
					value.ParentInternal.Items.RemoveAt(num);
				}
			}
			this.Add(value);
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x00144600 File Offset: 0x00143600
		internal void MoveItem(int index, ToolStripItem value)
		{
			if (index == this.Count)
			{
				this.MoveItem(value);
				return;
			}
			if (value.ParentInternal != null)
			{
				int num = value.ParentInternal.Items.IndexOf(value);
				if (num >= 0)
				{
					value.ParentInternal.Items.RemoveAt(num);
					if (value.ParentInternal == this.owner && index > num)
					{
						index--;
					}
				}
			}
			this.Insert(index, value);
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x0014466C File Offset: 0x0014366C
		private void SetOwner(ToolStripItem item)
		{
			if (this.itemsCollection && item != null)
			{
				if (item.Owner != null)
				{
					item.Owner.Items.Remove(item);
				}
				item.SetOwner(this.owner);
				if (item.Renderer != null)
				{
					item.Renderer.InitializeItem(item);
				}
			}
		}

		// Token: 0x0400383B RID: 14395
		private ToolStrip owner;

		// Token: 0x0400383C RID: 14396
		private bool itemsCollection;

		// Token: 0x0400383D RID: 14397
		private bool isReadOnly;

		// Token: 0x0400383E RID: 14398
		private int lastAccessedIndex;
	}
}
