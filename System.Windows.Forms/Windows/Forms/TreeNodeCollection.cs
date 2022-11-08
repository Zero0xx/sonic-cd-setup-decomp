using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020006FB RID: 1787
	[Editor("System.Windows.Forms.Design.TreeNodeCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public class TreeNodeCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06005F2B RID: 24363 RVA: 0x0015A45E File Offset: 0x0015945E
		internal TreeNodeCollection(TreeNode owner)
		{
			this.owner = owner;
		}

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06005F2C RID: 24364 RVA: 0x0015A47B File Offset: 0x0015947B
		// (set) Token: 0x06005F2D RID: 24365 RVA: 0x0015A483 File Offset: 0x00159483
		internal int FixedIndex
		{
			get
			{
				return this.fixedIndex;
			}
			set
			{
				this.fixedIndex = value;
			}
		}

		// Token: 0x17001420 RID: 5152
		public virtual TreeNode this[int index]
		{
			get
			{
				if (index < 0 || index >= this.owner.childCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.owner.children[index];
			}
			set
			{
				if (index < 0 || index >= this.owner.childCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				value.parent = this.owner;
				value.index = index;
				this.owner.children[index] = value;
				value.Realize(false);
			}
		}

		// Token: 0x17001421 RID: 5153
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value is TreeNode)
				{
					this[index] = (TreeNode)value;
					return;
				}
				throw new ArgumentException(SR.GetString("TreeNodeCollectionBadTreeNode"), "value");
			}
		}

		// Token: 0x17001422 RID: 5154
		public virtual TreeNode this[string key]
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

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x06005F33 RID: 24371 RVA: 0x0015A599 File Offset: 0x00159599
		[Browsable(false)]
		public int Count
		{
			get
			{
				return this.owner.childCount;
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x06005F34 RID: 24372 RVA: 0x0015A5A6 File Offset: 0x001595A6
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x06005F35 RID: 24373 RVA: 0x0015A5A9 File Offset: 0x001595A9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x06005F36 RID: 24374 RVA: 0x0015A5AC File Offset: 0x001595AC
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x06005F37 RID: 24375 RVA: 0x0015A5AF File Offset: 0x001595AF
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x0015A5B4 File Offset: 0x001595B4
		public virtual TreeNode Add(string text)
		{
			TreeNode treeNode = new TreeNode(text);
			this.Add(treeNode);
			return treeNode;
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x0015A5D4 File Offset: 0x001595D4
		public virtual TreeNode Add(string key, string text)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			this.Add(treeNode);
			return treeNode;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x0015A5F8 File Offset: 0x001595F8
		public virtual TreeNode Add(string key, string text, int imageIndex)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageIndex = imageIndex;
			this.Add(treeNode);
			return treeNode;
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x0015A624 File Offset: 0x00159624
		public virtual TreeNode Add(string key, string text, string imageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			this.Add(treeNode);
			return treeNode;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x0015A650 File Offset: 0x00159650
		public virtual TreeNode Add(string key, string text, int imageIndex, int selectedImageIndex)
		{
			TreeNode treeNode = new TreeNode(text, imageIndex, selectedImageIndex);
			treeNode.Name = key;
			this.Add(treeNode);
			return treeNode;
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x0015A678 File Offset: 0x00159678
		public virtual TreeNode Add(string key, string text, string imageKey, string selectedImageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			treeNode.SelectedImageKey = selectedImageKey;
			this.Add(treeNode);
			return treeNode;
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x0015A6AC File Offset: 0x001596AC
		public virtual void AddRange(TreeNode[] nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			if (nodes.Length == 0)
			{
				return;
			}
			TreeView treeView = this.owner.TreeView;
			if (treeView != null && nodes.Length > 200)
			{
				treeView.BeginUpdate();
			}
			this.owner.Nodes.FixedIndex = this.owner.childCount;
			this.owner.EnsureCapacity(nodes.Length);
			for (int i = nodes.Length - 1; i >= 0; i--)
			{
				this.AddInternal(nodes[i], i);
			}
			this.owner.Nodes.FixedIndex = -1;
			if (treeView != null && nodes.Length > 200)
			{
				treeView.EndUpdate();
			}
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x0015A754 File Offset: 0x00159754
		public TreeNode[] Find(string key, bool searchAllChildren)
		{
			ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
			TreeNode[] array = new TreeNode[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06005F40 RID: 24384 RVA: 0x0015A788 File Offset: 0x00159788
		private ArrayList FindInternal(string key, bool searchAllChildren, TreeNodeCollection treeNodeCollectionToLookIn, ArrayList foundTreeNodes)
		{
			if (treeNodeCollectionToLookIn == null || foundTreeNodes == null)
			{
				return null;
			}
			for (int i = 0; i < treeNodeCollectionToLookIn.Count; i++)
			{
				if (treeNodeCollectionToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(treeNodeCollectionToLookIn[i].Name, key, true))
				{
					foundTreeNodes.Add(treeNodeCollectionToLookIn[i]);
				}
			}
			if (searchAllChildren)
			{
				for (int j = 0; j < treeNodeCollectionToLookIn.Count; j++)
				{
					if (treeNodeCollectionToLookIn[j] != null && treeNodeCollectionToLookIn[j].Nodes != null && treeNodeCollectionToLookIn[j].Nodes.Count > 0)
					{
						foundTreeNodes = this.FindInternal(key, searchAllChildren, treeNodeCollectionToLookIn[j].Nodes, foundTreeNodes);
					}
				}
			}
			return foundTreeNodes;
		}

		// Token: 0x06005F41 RID: 24385 RVA: 0x0015A835 File Offset: 0x00159835
		public virtual int Add(TreeNode node)
		{
			return this.AddInternal(node, 0);
		}

		// Token: 0x06005F42 RID: 24386 RVA: 0x0015A840 File Offset: 0x00159840
		private int AddInternal(TreeNode node, int delta)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.handle != IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					node.Text
				}), "node");
			}
			TreeView treeView = this.owner.TreeView;
			if (treeView != null && treeView.Sorted)
			{
				return this.owner.AddSorted(node);
			}
			node.parent = this.owner;
			int num = this.owner.Nodes.FixedIndex;
			if (num != -1)
			{
				node.index = num + delta;
			}
			else
			{
				this.owner.EnsureCapacity(1);
				node.index = this.owner.childCount;
			}
			this.owner.children[node.index] = node;
			this.owner.childCount++;
			node.Realize(false);
			if (treeView != null && node == treeView.selectedNode)
			{
				treeView.SelectedNode = node;
			}
			if (treeView != null && treeView.TreeViewNodeSorter != null)
			{
				treeView.Sort();
			}
			return node.index;
		}

		// Token: 0x06005F43 RID: 24387 RVA: 0x0015A957 File Offset: 0x00159957
		int IList.Add(object node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node is TreeNode)
			{
				return this.Add((TreeNode)node);
			}
			return this.Add(node.ToString()).index;
		}

		// Token: 0x06005F44 RID: 24388 RVA: 0x0015A98D File Offset: 0x0015998D
		public bool Contains(TreeNode node)
		{
			return this.IndexOf(node) != -1;
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x0015A99C File Offset: 0x0015999C
		public virtual bool ContainsKey(string key)
		{
			return this.IsValidIndex(this.IndexOfKey(key));
		}

		// Token: 0x06005F46 RID: 24390 RVA: 0x0015A9AB File Offset: 0x001599AB
		bool IList.Contains(object node)
		{
			return node is TreeNode && this.Contains((TreeNode)node);
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x0015A9C4 File Offset: 0x001599C4
		public int IndexOf(TreeNode node)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i] == node)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x0015A9EF File Offset: 0x001599EF
		int IList.IndexOf(object node)
		{
			if (node is TreeNode)
			{
				return this.IndexOf((TreeNode)node);
			}
			return -1;
		}

		// Token: 0x06005F49 RID: 24393 RVA: 0x0015AA08 File Offset: 0x00159A08
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

		// Token: 0x06005F4A RID: 24394 RVA: 0x0015AA88 File Offset: 0x00159A88
		public virtual void Insert(int index, TreeNode node)
		{
			if (node.handle != IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					node.Text
				}), "node");
			}
			TreeView treeView = this.owner.TreeView;
			if (treeView != null && treeView.Sorted)
			{
				this.owner.AddSorted(node);
				return;
			}
			if (index < 0)
			{
				index = 0;
			}
			if (index > this.owner.childCount)
			{
				index = this.owner.childCount;
			}
			this.owner.InsertNodeAt(index, node);
		}

		// Token: 0x06005F4B RID: 24395 RVA: 0x0015AB21 File Offset: 0x00159B21
		void IList.Insert(int index, object node)
		{
			if (node is TreeNode)
			{
				this.Insert(index, (TreeNode)node);
				return;
			}
			throw new ArgumentException(SR.GetString("TreeNodeCollectionBadTreeNode"), "node");
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x0015AB50 File Offset: 0x00159B50
		public virtual TreeNode Insert(int index, string text)
		{
			TreeNode treeNode = new TreeNode(text);
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x06005F4D RID: 24397 RVA: 0x0015AB70 File Offset: 0x00159B70
		public virtual TreeNode Insert(int index, string key, string text)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x06005F4E RID: 24398 RVA: 0x0015AB94 File Offset: 0x00159B94
		public virtual TreeNode Insert(int index, string key, string text, int imageIndex)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageIndex = imageIndex;
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x0015ABC0 File Offset: 0x00159BC0
		public virtual TreeNode Insert(int index, string key, string text, string imageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x06005F50 RID: 24400 RVA: 0x0015ABEC File Offset: 0x00159BEC
		public virtual TreeNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
		{
			TreeNode treeNode = new TreeNode(text, imageIndex, selectedImageIndex);
			treeNode.Name = key;
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x0015AC14 File Offset: 0x00159C14
		public virtual TreeNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			treeNode.SelectedImageKey = selectedImageKey;
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x0015AC48 File Offset: 0x00159C48
		private bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.Count;
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x0015AC59 File Offset: 0x00159C59
		public virtual void Clear()
		{
			this.owner.Clear();
		}

		// Token: 0x06005F54 RID: 24404 RVA: 0x0015AC66 File Offset: 0x00159C66
		public void CopyTo(Array dest, int index)
		{
			if (this.owner.childCount > 0)
			{
				Array.Copy(this.owner.children, 0, dest, index, this.owner.childCount);
			}
		}

		// Token: 0x06005F55 RID: 24405 RVA: 0x0015AC94 File Offset: 0x00159C94
		public void Remove(TreeNode node)
		{
			node.Remove();
		}

		// Token: 0x06005F56 RID: 24406 RVA: 0x0015AC9C File Offset: 0x00159C9C
		void IList.Remove(object node)
		{
			if (node is TreeNode)
			{
				this.Remove((TreeNode)node);
			}
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x0015ACB2 File Offset: 0x00159CB2
		public virtual void RemoveAt(int index)
		{
			this[index].Remove();
		}

		// Token: 0x06005F58 RID: 24408 RVA: 0x0015ACC0 File Offset: 0x00159CC0
		public virtual void RemoveByKey(string key)
		{
			int index = this.IndexOfKey(key);
			if (this.IsValidIndex(index))
			{
				this.RemoveAt(index);
			}
		}

		// Token: 0x06005F59 RID: 24409 RVA: 0x0015ACE5 File Offset: 0x00159CE5
		public IEnumerator GetEnumerator()
		{
			return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.children, this.owner.childCount);
		}

		// Token: 0x040039C7 RID: 14791
		private TreeNode owner;

		// Token: 0x040039C8 RID: 14792
		private int lastAccessedIndex = -1;

		// Token: 0x040039C9 RID: 14793
		private int fixedIndex = -1;
	}
}
