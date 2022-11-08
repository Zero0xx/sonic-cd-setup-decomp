using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020006F6 RID: 1782
	[DefaultProperty("Text")]
	[TypeConverter(typeof(TreeNodeConverter))]
	[Serializable]
	public class TreeNode : MarshalByRefObject, ICloneable, ISerializable
	{
		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06005EBB RID: 24251 RVA: 0x001582C1 File Offset: 0x001572C1
		internal TreeNode.TreeNodeImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new TreeNode.TreeNodeImageIndexer(this, TreeNode.TreeNodeImageIndexer.ImageListType.Default);
				}
				return this.imageIndexer;
			}
		}

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06005EBC RID: 24252 RVA: 0x001582DE File Offset: 0x001572DE
		internal TreeNode.TreeNodeImageIndexer SelectedImageIndexer
		{
			get
			{
				if (this.selectedImageIndexer == null)
				{
					this.selectedImageIndexer = new TreeNode.TreeNodeImageIndexer(this, TreeNode.TreeNodeImageIndexer.ImageListType.Default);
				}
				return this.selectedImageIndexer;
			}
		}

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06005EBD RID: 24253 RVA: 0x001582FB File Offset: 0x001572FB
		internal TreeNode.TreeNodeImageIndexer StateImageIndexer
		{
			get
			{
				if (this.stateImageIndexer == null)
				{
					this.stateImageIndexer = new TreeNode.TreeNodeImageIndexer(this, TreeNode.TreeNodeImageIndexer.ImageListType.State);
				}
				return this.stateImageIndexer;
			}
		}

		// Token: 0x06005EBE RID: 24254 RVA: 0x00158318 File Offset: 0x00157318
		public TreeNode()
		{
			this.treeNodeState = default(BitVector32);
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x00158337 File Offset: 0x00157337
		internal TreeNode(TreeView treeView) : this()
		{
			this.treeView = treeView;
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x00158346 File Offset: 0x00157346
		public TreeNode(string text) : this()
		{
			this.text = text;
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00158355 File Offset: 0x00157355
		public TreeNode(string text, TreeNode[] children) : this()
		{
			this.text = text;
			this.Nodes.AddRange(children);
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x00158370 File Offset: 0x00157370
		public TreeNode(string text, int imageIndex, int selectedImageIndex) : this()
		{
			this.text = text;
			this.ImageIndexer.Index = imageIndex;
			this.SelectedImageIndexer.Index = selectedImageIndex;
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x00158397 File Offset: 0x00157397
		public TreeNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children) : this()
		{
			this.text = text;
			this.ImageIndexer.Index = imageIndex;
			this.SelectedImageIndexer.Index = selectedImageIndex;
			this.Nodes.AddRange(children);
		}

		// Token: 0x06005EC4 RID: 24260 RVA: 0x001583CB File Offset: 0x001573CB
		protected TreeNode(SerializationInfo serializationInfo, StreamingContext context) : this()
		{
			this.Deserialize(serializationInfo, context);
		}

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06005EC5 RID: 24261 RVA: 0x001583DB File Offset: 0x001573DB
		// (set) Token: 0x06005EC6 RID: 24262 RVA: 0x001583F8 File Offset: 0x001573F8
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeBackColorDescr")]
		public Color BackColor
		{
			get
			{
				if (this.propBag == null)
				{
					return Color.Empty;
				}
				return this.propBag.BackColor;
			}
			set
			{
				Color backColor = this.BackColor;
				if (value.IsEmpty)
				{
					if (this.propBag != null)
					{
						this.propBag.BackColor = Color.Empty;
						this.RemovePropBagIfEmpty();
					}
					if (!backColor.IsEmpty)
					{
						this.InvalidateHostTree();
					}
					return;
				}
				if (this.propBag == null)
				{
					this.propBag = new OwnerDrawPropertyBag();
				}
				this.propBag.BackColor = value;
				if (!value.Equals(backColor))
				{
					this.InvalidateHostTree();
				}
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06005EC7 RID: 24263 RVA: 0x00158480 File Offset: 0x00157480
		[Browsable(false)]
		public unsafe Rectangle Bounds
		{
			get
			{
				if (this.TreeView == null)
				{
					return Rectangle.Empty;
				}
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				*(IntPtr*)(&rect.left) = this.Handle;
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4356, 1, ref rect) == 0)
				{
					return Rectangle.Empty;
				}
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06005EC8 RID: 24264 RVA: 0x00158508 File Offset: 0x00157508
		internal unsafe Rectangle RowBounds
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				*(IntPtr*)(&rect.left) = this.Handle;
				if (this.TreeView == null)
				{
					return Rectangle.Empty;
				}
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4356, 0, ref rect) == 0)
				{
					return Rectangle.Empty;
				}
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x06005EC9 RID: 24265 RVA: 0x0015858F File Offset: 0x0015758F
		// (set) Token: 0x06005ECA RID: 24266 RVA: 0x0015859D File Offset: 0x0015759D
		internal bool CheckedStateInternal
		{
			get
			{
				return this.treeNodeState[1];
			}
			set
			{
				this.treeNodeState[1] = value;
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06005ECB RID: 24267 RVA: 0x001585AC File Offset: 0x001575AC
		// (set) Token: 0x06005ECC RID: 24268 RVA: 0x001585B4 File Offset: 0x001575B4
		internal bool CheckedInternal
		{
			get
			{
				return this.CheckedStateInternal;
			}
			set
			{
				this.CheckedStateInternal = value;
				if (this.handle == IntPtr.Zero)
				{
					return;
				}
				TreeView treeView = this.TreeView;
				if (treeView == null || !treeView.IsHandleCreated)
				{
					return;
				}
				NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
				tv_ITEM.mask = 24;
				tv_ITEM.hItem = this.handle;
				tv_ITEM.stateMask = 61440;
				tv_ITEM.state |= (value ? 8192 : 4096);
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06005ECD RID: 24269 RVA: 0x0015864F File Offset: 0x0015764F
		// (set) Token: 0x06005ECE RID: 24270 RVA: 0x00158658 File Offset: 0x00157658
		[SRDescription("TreeNodeCheckedDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool Checked
		{
			get
			{
				return this.CheckedInternal;
			}
			set
			{
				TreeView treeView = this.TreeView;
				if (treeView != null)
				{
					if (!treeView.TreeViewBeforeCheck(this, TreeViewAction.Unknown))
					{
						this.CheckedInternal = value;
						treeView.TreeViewAfterCheck(this, TreeViewAction.Unknown);
						return;
					}
				}
				else
				{
					this.CheckedInternal = value;
				}
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06005ECF RID: 24271 RVA: 0x00158692 File Offset: 0x00157692
		// (set) Token: 0x06005ED0 RID: 24272 RVA: 0x0015869A File Offset: 0x0015769A
		[SRCategory("CatBehavior")]
		[SRDescription("ControlContextMenuDescr")]
		[DefaultValue(null)]
		public virtual ContextMenu ContextMenu
		{
			get
			{
				return this.contextMenu;
			}
			set
			{
				this.contextMenu = value;
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x06005ED1 RID: 24273 RVA: 0x001586A3 File Offset: 0x001576A3
		// (set) Token: 0x06005ED2 RID: 24274 RVA: 0x001586AB File Offset: 0x001576AB
		[SRCategory("CatBehavior")]
		[SRDescription("ControlContextMenuDescr")]
		[DefaultValue(null)]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.contextMenuStrip;
			}
			set
			{
				this.contextMenuStrip = value;
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x06005ED3 RID: 24275 RVA: 0x001586B4 File Offset: 0x001576B4
		[Browsable(false)]
		public TreeNode FirstNode
		{
			get
			{
				if (this.childCount == 0)
				{
					return null;
				}
				return this.children[0];
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x001586C8 File Offset: 0x001576C8
		private TreeNode FirstVisibleParent
		{
			get
			{
				TreeNode treeNode = this;
				while (treeNode != null && treeNode.Bounds.IsEmpty)
				{
					treeNode = treeNode.Parent;
				}
				return treeNode;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06005ED5 RID: 24277 RVA: 0x001586F4 File Offset: 0x001576F4
		// (set) Token: 0x06005ED6 RID: 24278 RVA: 0x00158710 File Offset: 0x00157710
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeForeColorDescr")]
		public Color ForeColor
		{
			get
			{
				if (this.propBag == null)
				{
					return Color.Empty;
				}
				return this.propBag.ForeColor;
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (value.IsEmpty)
				{
					if (this.propBag != null)
					{
						this.propBag.ForeColor = Color.Empty;
						this.RemovePropBagIfEmpty();
					}
					if (!foreColor.IsEmpty)
					{
						this.InvalidateHostTree();
					}
					return;
				}
				if (this.propBag == null)
				{
					this.propBag = new OwnerDrawPropertyBag();
				}
				this.propBag.ForeColor = value;
				if (!value.Equals(foreColor))
				{
					this.InvalidateHostTree();
				}
			}
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x00158798 File Offset: 0x00157798
		[Browsable(false)]
		public string FullPath
		{
			get
			{
				TreeView treeView = this.TreeView;
				if (treeView != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					this.GetFullPath(stringBuilder, treeView.PathSeparator);
					return stringBuilder.ToString();
				}
				throw new InvalidOperationException(SR.GetString("TreeNodeNoParent"));
			}
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06005ED8 RID: 24280 RVA: 0x001587D8 File Offset: 0x001577D8
		[Browsable(false)]
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					this.TreeView.CreateControl();
				}
				return this.handle;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06005ED9 RID: 24281 RVA: 0x001587FD File Offset: 0x001577FD
		// (set) Token: 0x06005EDA RID: 24282 RVA: 0x0015880A File Offset: 0x0015780A
		[Localizable(true)]
		[SRDescription("TreeNodeImageIndexDescr")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(-1)]
		[RelatedImageList("TreeView.ImageList")]
		[SRCategory("CatBehavior")]
		public int ImageIndex
		{
			get
			{
				return this.ImageIndexer.Index;
			}
			set
			{
				this.ImageIndexer.Index = value;
				this.UpdateNode(2);
			}
		}

		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06005EDB RID: 24283 RVA: 0x0015881F File Offset: 0x0015781F
		// (set) Token: 0x06005EDC RID: 24284 RVA: 0x0015882C File Offset: 0x0015782C
		[DefaultValue("")]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(TreeViewImageKeyConverter))]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.ImageList")]
		[SRDescription("TreeNodeImageKeyDescr")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				this.UpdateNode(2);
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06005EDD RID: 24285 RVA: 0x00158841 File Offset: 0x00157841
		[SRDescription("TreeNodeIndexDescr")]
		[SRCategory("CatBehavior")]
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x06005EDE RID: 24286 RVA: 0x0015884C File Offset: 0x0015784C
		[Browsable(false)]
		public bool IsEditing
		{
			get
			{
				TreeView treeView = this.TreeView;
				return treeView != null && treeView.editNode == this;
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06005EDF RID: 24287 RVA: 0x0015886E File Offset: 0x0015786E
		[Browsable(false)]
		public bool IsExpanded
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return this.expandOnRealization;
				}
				return (this.State & 32) != 0;
			}
		}

		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x06005EE0 RID: 24288 RVA: 0x00158898 File Offset: 0x00157898
		[Browsable(false)]
		public bool IsSelected
		{
			get
			{
				return !(this.handle == IntPtr.Zero) && (this.State & 2) != 0;
			}
		}

		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x001588BC File Offset: 0x001578BC
		[Browsable(false)]
		public unsafe bool IsVisible
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return false;
				}
				TreeView treeView = this.TreeView;
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				*(IntPtr*)(&rect.left) = this.Handle;
				bool flag = (int)UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4356, 1, ref rect) != 0;
				if (flag)
				{
					Size clientSize = treeView.ClientSize;
					flag = (rect.bottom > 0 && rect.right > 0 && rect.top < clientSize.Height && rect.left < clientSize.Width);
				}
				return flag;
			}
		}

		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x00158967 File Offset: 0x00157967
		[Browsable(false)]
		public TreeNode LastNode
		{
			get
			{
				if (this.childCount == 0)
				{
					return null;
				}
				return this.children[this.childCount - 1];
			}
		}

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x06005EE3 RID: 24291 RVA: 0x00158982 File Offset: 0x00157982
		[Browsable(false)]
		public int Level
		{
			get
			{
				if (this.Parent == null)
				{
					return 0;
				}
				return this.Parent.Level + 1;
			}
		}

		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x06005EE4 RID: 24292 RVA: 0x0015899B File Offset: 0x0015799B
		[Browsable(false)]
		public TreeNode NextNode
		{
			get
			{
				if (this.index + 1 < this.parent.Nodes.Count)
				{
					return this.parent.Nodes[this.index + 1];
				}
				return null;
			}
		}

		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x06005EE5 RID: 24293 RVA: 0x001589D4 File Offset: 0x001579D4
		[Browsable(false)]
		public TreeNode NextVisibleNode
		{
			get
			{
				if (this.TreeView == null)
				{
					return null;
				}
				TreeNode firstVisibleParent = this.FirstVisibleParent;
				if (firstVisibleParent != null)
				{
					IntPtr value = UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4362, 6, firstVisibleParent.Handle);
					if (value != IntPtr.Zero)
					{
						return this.TreeView.NodeFromHandle(value);
					}
				}
				return null;
			}
		}

		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x06005EE6 RID: 24294 RVA: 0x00158A38 File Offset: 0x00157A38
		// (set) Token: 0x06005EE7 RID: 24295 RVA: 0x00158A50 File Offset: 0x00157A50
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeNodeFontDescr")]
		[DefaultValue(null)]
		[Localizable(true)]
		public Font NodeFont
		{
			get
			{
				if (this.propBag == null)
				{
					return null;
				}
				return this.propBag.Font;
			}
			set
			{
				Font nodeFont = this.NodeFont;
				if (value == null)
				{
					if (this.propBag != null)
					{
						this.propBag.Font = null;
						this.RemovePropBagIfEmpty();
					}
					if (nodeFont != null)
					{
						this.InvalidateHostTree();
					}
					return;
				}
				if (this.propBag == null)
				{
					this.propBag = new OwnerDrawPropertyBag();
				}
				this.propBag.Font = value;
				if (!value.Equals(nodeFont))
				{
					this.InvalidateHostTree();
				}
			}
		}

		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06005EE8 RID: 24296 RVA: 0x00158AB9 File Offset: 0x00157AB9
		[Browsable(false)]
		[ListBindable(false)]
		public TreeNodeCollection Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new TreeNodeCollection(this);
				}
				return this.nodes;
			}
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06005EE9 RID: 24297 RVA: 0x00158AD8 File Offset: 0x00157AD8
		[Browsable(false)]
		public TreeNode Parent
		{
			get
			{
				TreeView treeView = this.TreeView;
				if (treeView != null && this.parent == treeView.root)
				{
					return null;
				}
				return this.parent;
			}
		}

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x06005EEA RID: 24298 RVA: 0x00158B08 File Offset: 0x00157B08
		[Browsable(false)]
		public TreeNode PrevNode
		{
			get
			{
				int num = this.index;
				int fixedIndex = this.parent.Nodes.FixedIndex;
				if (fixedIndex > 0)
				{
					num = fixedIndex;
				}
				if (num > 0 && num <= this.parent.Nodes.Count)
				{
					return this.parent.Nodes[num - 1];
				}
				return null;
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x06005EEB RID: 24299 RVA: 0x00158B60 File Offset: 0x00157B60
		[Browsable(false)]
		public TreeNode PrevVisibleNode
		{
			get
			{
				TreeNode firstVisibleParent = this.FirstVisibleParent;
				if (firstVisibleParent != null)
				{
					if (this.TreeView == null)
					{
						return null;
					}
					IntPtr value = UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4362, 7, firstVisibleParent.Handle);
					if (value != IntPtr.Zero)
					{
						return this.TreeView.NodeFromHandle(value);
					}
				}
				return null;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06005EEC RID: 24300 RVA: 0x00158BC4 File Offset: 0x00157BC4
		// (set) Token: 0x06005EED RID: 24301 RVA: 0x00158BD1 File Offset: 0x00157BD1
		[DefaultValue(-1)]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RelatedImageList("TreeView.ImageList")]
		[SRDescription("TreeNodeSelectedImageIndexDescr")]
		public int SelectedImageIndex
		{
			get
			{
				return this.SelectedImageIndexer.Index;
			}
			set
			{
				this.SelectedImageIndexer.Index = value;
				this.UpdateNode(32);
			}
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06005EEE RID: 24302 RVA: 0x00158BE7 File Offset: 0x00157BE7
		// (set) Token: 0x06005EEF RID: 24303 RVA: 0x00158BF4 File Offset: 0x00157BF4
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeSelectedImageKeyDescr")]
		[TypeConverter(typeof(TreeViewImageKeyConverter))]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RelatedImageList("TreeView.ImageList")]
		[Localizable(true)]
		public string SelectedImageKey
		{
			get
			{
				return this.SelectedImageIndexer.Key;
			}
			set
			{
				this.SelectedImageIndexer.Key = value;
				this.UpdateNode(32);
			}
		}

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x06005EF0 RID: 24304 RVA: 0x00158C0C File Offset: 0x00157C0C
		internal int State
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return 0;
				}
				if (this.TreeView == null)
				{
					return 0;
				}
				NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
				tv_ITEM.hItem = this.Handle;
				tv_ITEM.mask = 24;
				tv_ITEM.stateMask = 34;
				UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), NativeMethods.TVM_GETITEM, 0, ref tv_ITEM);
				return tv_ITEM.state;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x06005EF1 RID: 24305 RVA: 0x00158C89 File Offset: 0x00157C89
		// (set) Token: 0x06005EF2 RID: 24306 RVA: 0x00158C96 File Offset: 0x00157C96
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[DefaultValue("")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.StateImageList")]
		[SRDescription("TreeNodeStateImageKeyDescr")]
		public string StateImageKey
		{
			get
			{
				return this.StateImageIndexer.Key;
			}
			set
			{
				if (this.StateImageIndexer.Key != value)
				{
					this.StateImageIndexer.Key = value;
					if (this.treeView != null && !this.treeView.CheckBoxes)
					{
						this.UpdateNode(8);
					}
				}
			}
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06005EF3 RID: 24307 RVA: 0x00158CD3 File Offset: 0x00157CD3
		// (set) Token: 0x06005EF4 RID: 24308 RVA: 0x00158CF8 File Offset: 0x00157CF8
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeStateImageIndexDescr")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.StateImageList")]
		[DefaultValue(-1)]
		public int StateImageIndex
		{
			get
			{
				if (this.treeView != null && this.treeView.StateImageList != null)
				{
					return this.StateImageIndexer.Index;
				}
				return -1;
			}
			set
			{
				if (value < -1 || value > 14)
				{
					throw new ArgumentOutOfRangeException("StateImageIndex", SR.GetString("InvalidArgument", new object[]
					{
						"StateImageIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.StateImageIndexer.Index = value;
				if (this.treeView != null && !this.treeView.CheckBoxes)
				{
					this.UpdateNode(8);
				}
			}
		}

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06005EF5 RID: 24309 RVA: 0x00158D6A File Offset: 0x00157D6A
		// (set) Token: 0x06005EF6 RID: 24310 RVA: 0x00158D72 File Offset: 0x00157D72
		[Localizable(false)]
		[Bindable(true)]
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		[SRDescription("ControlTagDescr")]
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

		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x06005EF7 RID: 24311 RVA: 0x00158D7B File Offset: 0x00157D7B
		// (set) Token: 0x06005EF8 RID: 24312 RVA: 0x00158D91 File Offset: 0x00157D91
		[Localizable(true)]
		[SRDescription("TreeNodeTextDescr")]
		[SRCategory("CatAppearance")]
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return "";
			}
			set
			{
				this.text = value;
				this.UpdateNode(1);
			}
		}

		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x06005EF9 RID: 24313 RVA: 0x00158DA1 File Offset: 0x00157DA1
		// (set) Token: 0x06005EFA RID: 24314 RVA: 0x00158DA9 File Offset: 0x00157DA9
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		[Localizable(false)]
		[SRDescription("TreeNodeToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				this.toolTipText = value;
			}
		}

		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x06005EFB RID: 24315 RVA: 0x00158DB2 File Offset: 0x00157DB2
		// (set) Token: 0x06005EFC RID: 24316 RVA: 0x00158DC8 File Offset: 0x00157DC8
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeNodeNameDescr")]
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return "";
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x06005EFD RID: 24317 RVA: 0x00158DD1 File Offset: 0x00157DD1
		[Browsable(false)]
		public TreeView TreeView
		{
			get
			{
				if (this.treeView == null)
				{
					this.treeView = this.FindTreeView();
				}
				return this.treeView;
			}
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x00158DF0 File Offset: 0x00157DF0
		internal int AddSorted(TreeNode node)
		{
			int result = 0;
			string @string = node.Text;
			TreeView treeView = this.TreeView;
			if (this.childCount > 0)
			{
				if (treeView.TreeViewNodeSorter == null)
				{
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					if (compareInfo.Compare(this.children[this.childCount - 1].Text, @string) <= 0)
					{
						result = this.childCount;
					}
					else
					{
						int i = 0;
						int num = this.childCount;
						while (i < num)
						{
							int num2 = (i + num) / 2;
							if (compareInfo.Compare(this.children[num2].Text, @string) <= 0)
							{
								i = num2 + 1;
							}
							else
							{
								num = num2;
							}
						}
						result = i;
					}
				}
				else
				{
					IComparer treeViewNodeSorter = treeView.TreeViewNodeSorter;
					int i = 0;
					int num = this.childCount;
					while (i < num)
					{
						int num2 = (i + num) / 2;
						if (treeViewNodeSorter.Compare(this.children[num2], node) <= 0)
						{
							i = num2 + 1;
						}
						else
						{
							num = num2;
						}
					}
					result = i;
				}
			}
			node.SortChildren(treeView);
			this.InsertNodeAt(result, node);
			return result;
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x00158EDF File Offset: 0x00157EDF
		public static TreeNode FromHandle(TreeView tree, IntPtr handle)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return tree.NodeFromHandle(handle);
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x00158EF4 File Offset: 0x00157EF4
		private void SortChildren(TreeView parentTreeView)
		{
			if (this.childCount > 0)
			{
				TreeNode[] array = new TreeNode[this.childCount];
				if (parentTreeView == null || parentTreeView.TreeViewNodeSorter == null)
				{
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					for (int i = 0; i < this.childCount; i++)
					{
						int num = -1;
						for (int j = 0; j < this.childCount; j++)
						{
							if (this.children[j] != null)
							{
								if (num == -1)
								{
									num = j;
								}
								else if (compareInfo.Compare(this.children[j].Text, this.children[num].Text) <= 0)
								{
									num = j;
								}
							}
						}
						array[i] = this.children[num];
						this.children[num] = null;
						array[i].index = i;
						array[i].SortChildren(parentTreeView);
					}
					this.children = array;
					return;
				}
				IComparer treeViewNodeSorter = parentTreeView.TreeViewNodeSorter;
				for (int k = 0; k < this.childCount; k++)
				{
					int num2 = -1;
					for (int l = 0; l < this.childCount; l++)
					{
						if (this.children[l] != null)
						{
							if (num2 == -1)
							{
								num2 = l;
							}
							else if (treeViewNodeSorter.Compare(this.children[l], this.children[num2]) <= 0)
							{
								num2 = l;
							}
						}
					}
					array[k] = this.children[num2];
					this.children[num2] = null;
					array[k].index = k;
					array[k].SortChildren(parentTreeView);
				}
				this.children = array;
			}
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x0015906C File Offset: 0x0015806C
		public void BeginEdit()
		{
			if (this.handle != IntPtr.Zero)
			{
				TreeView treeView = this.TreeView;
				if (!treeView.LabelEdit)
				{
					throw new InvalidOperationException(SR.GetString("TreeNodeBeginEditFailed"));
				}
				if (!treeView.Focused)
				{
					treeView.FocusInternal();
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_EDITLABEL, 0, this.handle);
			}
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x001590D8 File Offset: 0x001580D8
		internal void Clear()
		{
			bool flag = false;
			TreeView treeView = this.TreeView;
			try
			{
				if (treeView != null)
				{
					treeView.nodesCollectionClear = true;
					if (treeView != null && this.childCount > 200)
					{
						flag = true;
						treeView.BeginUpdate();
					}
				}
				while (this.childCount > 0)
				{
					this.children[this.childCount - 1].Remove(true);
				}
				this.children = null;
				if (treeView != null && flag)
				{
					treeView.EndUpdate();
				}
			}
			finally
			{
				if (treeView != null)
				{
					treeView.nodesCollectionClear = false;
				}
				this.nodesCleared = true;
			}
		}

		// Token: 0x06005F03 RID: 24323 RVA: 0x00159168 File Offset: 0x00158168
		public virtual object Clone()
		{
			Type type = base.GetType();
			TreeNode treeNode;
			if (type == typeof(TreeNode))
			{
				treeNode = new TreeNode(this.text, this.ImageIndexer.Index, this.SelectedImageIndexer.Index);
			}
			else
			{
				treeNode = (TreeNode)Activator.CreateInstance(type);
			}
			treeNode.Text = this.text;
			treeNode.Name = this.name;
			treeNode.ImageIndexer.Index = this.ImageIndexer.Index;
			treeNode.SelectedImageIndexer.Index = this.SelectedImageIndexer.Index;
			treeNode.StateImageIndexer.Index = this.StateImageIndexer.Index;
			treeNode.ToolTipText = this.toolTipText;
			treeNode.ContextMenu = this.contextMenu;
			treeNode.ContextMenuStrip = this.contextMenuStrip;
			if (!string.IsNullOrEmpty(this.ImageIndexer.Key))
			{
				treeNode.ImageIndexer.Key = this.ImageIndexer.Key;
			}
			if (!string.IsNullOrEmpty(this.SelectedImageIndexer.Key))
			{
				treeNode.SelectedImageIndexer.Key = this.SelectedImageIndexer.Key;
			}
			if (!string.IsNullOrEmpty(this.StateImageIndexer.Key))
			{
				treeNode.StateImageIndexer.Key = this.StateImageIndexer.Key;
			}
			if (this.childCount > 0)
			{
				treeNode.children = new TreeNode[this.childCount];
				for (int i = 0; i < this.childCount; i++)
				{
					treeNode.Nodes.Add((TreeNode)this.children[i].Clone());
				}
			}
			if (this.propBag != null)
			{
				treeNode.propBag = OwnerDrawPropertyBag.Copy(this.propBag);
			}
			treeNode.Checked = this.Checked;
			treeNode.Tag = this.Tag;
			return treeNode;
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x0015932C File Offset: 0x0015832C
		private void CollapseInternal(bool ignoreChildren)
		{
			TreeView treeView = this.TreeView;
			bool flag = false;
			this.collapseOnRealization = false;
			this.expandOnRealization = false;
			if (treeView == null || !treeView.IsHandleCreated)
			{
				this.collapseOnRealization = true;
				return;
			}
			if (ignoreChildren)
			{
				this.DoCollapse(treeView);
			}
			else
			{
				if (!ignoreChildren && this.childCount > 0)
				{
					for (int i = 0; i < this.childCount; i++)
					{
						if (treeView.SelectedNode == this.children[i])
						{
							flag = true;
						}
						this.children[i].DoCollapse(treeView);
						this.children[i].Collapse();
					}
				}
				this.DoCollapse(treeView);
			}
			if (flag)
			{
				treeView.SelectedNode = this;
			}
			treeView.Invalidate();
			this.collapseOnRealization = false;
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x001593D7 File Offset: 0x001583D7
		public void Collapse(bool ignoreChildren)
		{
			this.CollapseInternal(ignoreChildren);
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x001593E0 File Offset: 0x001583E0
		public void Collapse()
		{
			this.CollapseInternal(false);
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x001593EC File Offset: 0x001583EC
		private void DoCollapse(TreeView tv)
		{
			if ((this.State & 32) != 0)
			{
				TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(this, false, TreeViewAction.Collapse);
				tv.OnBeforeCollapse(treeViewCancelEventArgs);
				if (!treeViewCancelEventArgs.Cancel)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(tv, tv.Handle), 4354, 1, this.Handle);
					tv.OnAfterCollapse(new TreeViewEventArgs(this));
				}
			}
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x00159448 File Offset: 0x00158448
		protected virtual void Deserialize(SerializationInfo serializationInfo, StreamingContext context)
		{
			int num = 0;
			int num2 = -1;
			string text = null;
			int num3 = -1;
			string text2 = null;
			int num4 = -1;
			string text3 = null;
			foreach (SerializationEntry serializationEntry in serializationInfo)
			{
				string key;
				switch (key = serializationEntry.Name)
				{
				case "PropBag":
					this.propBag = (OwnerDrawPropertyBag)serializationInfo.GetValue(serializationEntry.Name, typeof(OwnerDrawPropertyBag));
					break;
				case "Text":
					this.Text = serializationInfo.GetString(serializationEntry.Name);
					break;
				case "Name":
					this.Name = serializationInfo.GetString(serializationEntry.Name);
					break;
				case "IsChecked":
					this.CheckedStateInternal = serializationInfo.GetBoolean(serializationEntry.Name);
					break;
				case "ImageIndex":
					num2 = serializationInfo.GetInt32(serializationEntry.Name);
					break;
				case "SelectedImageIndex":
					num3 = serializationInfo.GetInt32(serializationEntry.Name);
					break;
				case "ImageKey":
					text = serializationInfo.GetString(serializationEntry.Name);
					break;
				case "SelectedImageKey":
					text2 = serializationInfo.GetString(serializationEntry.Name);
					break;
				case "StateImageKey":
					text3 = serializationInfo.GetString(serializationEntry.Name);
					break;
				case "StateImageIndex":
					num4 = serializationInfo.GetInt32(serializationEntry.Name);
					break;
				case "ChildCount":
					num = serializationInfo.GetInt32(serializationEntry.Name);
					break;
				case "UserData":
					this.userData = serializationEntry.Value;
					break;
				}
			}
			if (text != null)
			{
				this.ImageKey = text;
			}
			else if (num2 != -1)
			{
				this.ImageIndex = num2;
			}
			if (text2 != null)
			{
				this.SelectedImageKey = text2;
			}
			else if (num3 != -1)
			{
				this.SelectedImageIndex = num3;
			}
			if (text3 != null)
			{
				this.StateImageKey = text3;
			}
			else if (num4 != -1)
			{
				this.StateImageIndex = num4;
			}
			if (num > 0)
			{
				TreeNode[] array = new TreeNode[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = (TreeNode)serializationInfo.GetValue("children" + i, typeof(TreeNode));
				}
				this.Nodes.AddRange(array);
			}
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x00159723 File Offset: 0x00158723
		public void EndEdit(bool cancel)
		{
			if (this.TreeView == null)
			{
				return;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4374, cancel ? 1 : 0, 0);
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x00159758 File Offset: 0x00158758
		internal void EnsureCapacity(int num)
		{
			int num2 = num;
			if (num2 < 4)
			{
				num2 = 4;
			}
			if (this.children == null)
			{
				this.children = new TreeNode[num2];
				return;
			}
			if (this.childCount + num > this.children.Length)
			{
				int num3 = this.childCount + num;
				if (num == 1)
				{
					num3 = this.childCount * 2;
				}
				TreeNode[] destinationArray = new TreeNode[num3];
				Array.Copy(this.children, 0, destinationArray, 0, this.childCount);
				this.children = destinationArray;
			}
		}

		// Token: 0x06005F0B RID: 24331 RVA: 0x001597CC File Offset: 0x001587CC
		private void EnsureStateImageValue()
		{
			if (this.treeView == null)
			{
				return;
			}
			if (this.treeView.CheckBoxes && this.treeView.StateImageList != null)
			{
				if (!string.IsNullOrEmpty(this.StateImageKey))
				{
					this.StateImageIndex = (this.Checked ? 1 : 0);
					this.StateImageKey = this.treeView.StateImageList.Images.Keys[this.StateImageIndex];
					return;
				}
				this.StateImageIndex = (this.Checked ? 1 : 0);
			}
		}

		// Token: 0x06005F0C RID: 24332 RVA: 0x00159854 File Offset: 0x00158854
		public void EnsureVisible()
		{
			if (this.TreeView == null)
			{
				return;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4372, 0, this.Handle);
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x00159888 File Offset: 0x00158888
		public void Expand()
		{
			TreeView treeView = this.TreeView;
			if (treeView == null || !treeView.IsHandleCreated)
			{
				this.expandOnRealization = true;
				return;
			}
			this.ResetExpandedState(treeView);
			if (!this.IsExpanded)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4354, 2, this.Handle);
			}
			this.expandOnRealization = false;
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x001598E4 File Offset: 0x001588E4
		public void ExpandAll()
		{
			this.Expand();
			for (int i = 0; i < this.childCount; i++)
			{
				this.children[i].ExpandAll();
			}
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x00159918 File Offset: 0x00158918
		internal TreeView FindTreeView()
		{
			TreeNode treeNode = this;
			while (treeNode.parent != null)
			{
				treeNode = treeNode.parent;
			}
			return treeNode.treeView;
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x0015993E File Offset: 0x0015893E
		private void GetFullPath(StringBuilder path, string pathSeparator)
		{
			if (this.parent != null)
			{
				this.parent.GetFullPath(path, pathSeparator);
				if (this.parent.parent != null)
				{
					path.Append(pathSeparator);
				}
				path.Append(this.text);
			}
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x00159978 File Offset: 0x00158978
		public int GetNodeCount(bool includeSubTrees)
		{
			int num = this.childCount;
			if (includeSubTrees)
			{
				for (int i = 0; i < this.childCount; i++)
				{
					num += this.children[i].GetNodeCount(true);
				}
			}
			return num;
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x001599B4 File Offset: 0x001589B4
		internal void InsertNodeAt(int index, TreeNode node)
		{
			this.EnsureCapacity(1);
			node.parent = this;
			node.index = index;
			for (int i = this.childCount; i > index; i--)
			{
				(this.children[i] = this.children[i - 1]).index = i;
			}
			this.children[index] = node;
			this.childCount++;
			node.Realize(false);
			if (this.TreeView != null && node == this.TreeView.selectedNode)
			{
				this.TreeView.SelectedNode = node;
			}
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x00159A42 File Offset: 0x00158A42
		private void InvalidateHostTree()
		{
			if (this.treeView != null && this.treeView.IsHandleCreated)
			{
				this.treeView.Invalidate();
			}
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x00159A64 File Offset: 0x00158A64
		internal void Realize(bool insertFirst)
		{
			TreeView treeView = this.TreeView;
			if (treeView == null || !treeView.IsHandleCreated)
			{
				return;
			}
			if (this.parent == null && !treeView.IsUpdating() && Application.RenderWithVisualStyles)
			{
				treeView.SendMessage(11, 0, 0);
			}
			try
			{
				if (this.parent != null)
				{
					if (treeView.InvokeRequired)
					{
						throw new InvalidOperationException(SR.GetString("InvalidCrossThreadControlCall"));
					}
					NativeMethods.TV_INSERTSTRUCT tv_INSERTSTRUCT = default(NativeMethods.TV_INSERTSTRUCT);
					tv_INSERTSTRUCT.item_mask = TreeNode.insertMask;
					tv_INSERTSTRUCT.hParent = this.parent.handle;
					TreeNode prevNode = this.PrevNode;
					if (insertFirst || prevNode == null)
					{
						tv_INSERTSTRUCT.hInsertAfter = (IntPtr)(-65535);
					}
					else
					{
						tv_INSERTSTRUCT.hInsertAfter = prevNode.handle;
					}
					tv_INSERTSTRUCT.item_pszText = Marshal.StringToHGlobalAuto(this.text);
					tv_INSERTSTRUCT.item_iImage = ((this.ImageIndexer.ActualIndex == -1) ? treeView.ImageIndexer.ActualIndex : this.ImageIndexer.ActualIndex);
					tv_INSERTSTRUCT.item_iSelectedImage = ((this.SelectedImageIndexer.ActualIndex == -1) ? treeView.SelectedImageIndexer.ActualIndex : this.SelectedImageIndexer.ActualIndex);
					tv_INSERTSTRUCT.item_mask = 1;
					tv_INSERTSTRUCT.item_stateMask = 0;
					tv_INSERTSTRUCT.item_state = 0;
					if (treeView.CheckBoxes)
					{
						tv_INSERTSTRUCT.item_mask |= 8;
						tv_INSERTSTRUCT.item_stateMask |= 61440;
						tv_INSERTSTRUCT.item_state |= (this.CheckedInternal ? 8192 : 4096);
					}
					else if (treeView.StateImageList != null && this.StateImageIndexer.ActualIndex >= 0)
					{
						tv_INSERTSTRUCT.item_mask |= 8;
						tv_INSERTSTRUCT.item_stateMask = 61440;
						tv_INSERTSTRUCT.item_state = this.StateImageIndexer.ActualIndex + 1 << 12;
					}
					if (tv_INSERTSTRUCT.item_iImage >= 0)
					{
						tv_INSERTSTRUCT.item_mask |= 2;
					}
					if (tv_INSERTSTRUCT.item_iSelectedImage >= 0)
					{
						tv_INSERTSTRUCT.item_mask |= 32;
					}
					bool flag = false;
					IntPtr value = UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4367, 0, 0);
					if (value != IntPtr.Zero)
					{
						flag = true;
						UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4374, 0, 0);
					}
					this.handle = UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), NativeMethods.TVM_INSERTITEM, 0, ref tv_INSERTSTRUCT);
					treeView.nodeTable[this.handle] = this;
					this.UpdateNode(4);
					Marshal.FreeHGlobal(tv_INSERTSTRUCT.item_pszText);
					if (flag)
					{
						UnsafeNativeMethods.PostMessage(new HandleRef(this.TreeView, this.TreeView.Handle), NativeMethods.TVM_EDITLABEL, IntPtr.Zero, this.handle);
					}
					SafeNativeMethods.InvalidateRect(new HandleRef(treeView, treeView.Handle), null, false);
					if (this.parent.nodesCleared && (insertFirst || prevNode == null) && !treeView.Scrollable)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 11, 1, 0);
						this.nodesCleared = false;
					}
				}
				for (int i = this.childCount - 1; i >= 0; i--)
				{
					this.children[i].Realize(true);
				}
				if (this.expandOnRealization)
				{
					this.Expand();
				}
				if (this.collapseOnRealization)
				{
					this.Collapse();
				}
			}
			finally
			{
				if (!treeView.IsUpdating() && this.parent == null && Application.RenderWithVisualStyles)
				{
					treeView.SendMessage(11, -1, 0);
				}
			}
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x00159E20 File Offset: 0x00158E20
		public void Remove()
		{
			this.Remove(true);
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x00159E2C File Offset: 0x00158E2C
		internal void Remove(bool notify)
		{
			bool isExpanded = this.IsExpanded;
			for (int i = 0; i < this.childCount; i++)
			{
				this.children[i].Remove(false);
			}
			if (notify && this.parent != null)
			{
				if (this.index == this.parent.childCount - 1)
				{
					this.parent.children[this.index] = null;
				}
				for (int j = this.index; j < this.parent.childCount - 1; j++)
				{
					(this.parent.children[j] = this.parent.children[j + 1]).index = j;
				}
				this.parent.childCount--;
				this.parent = null;
			}
			this.expandOnRealization = isExpanded;
			if (this.TreeView == null)
			{
				return;
			}
			if (this.handle != IntPtr.Zero)
			{
				if (notify && this.TreeView.IsHandleCreated)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), 4353, 0, this.handle);
				}
				this.treeView.nodeTable.Remove(this.handle);
				this.handle = IntPtr.Zero;
			}
			this.treeView = null;
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x00159F7B File Offset: 0x00158F7B
		private void RemovePropBagIfEmpty()
		{
			if (this.propBag == null)
			{
				return;
			}
			if (this.propBag.IsEmpty())
			{
				this.propBag = null;
			}
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x00159F9C File Offset: 0x00158F9C
		private void ResetExpandedState(TreeView tv)
		{
			NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
			tv_ITEM.mask = 24;
			tv_ITEM.hItem = this.handle;
			tv_ITEM.stateMask = 64;
			tv_ITEM.state = 0;
			UnsafeNativeMethods.SendMessage(new HandleRef(tv, tv.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x00159FF2 File Offset: 0x00158FF2
		private bool ShouldSerializeBackColor()
		{
			return this.BackColor != Color.Empty;
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x0015A004 File Offset: 0x00159004
		private bool ShouldSerializeForeColor()
		{
			return this.ForeColor != Color.Empty;
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x0015A018 File Offset: 0x00159018
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected virtual void Serialize(SerializationInfo si, StreamingContext context)
		{
			if (this.propBag != null)
			{
				si.AddValue("PropBag", this.propBag, typeof(OwnerDrawPropertyBag));
			}
			si.AddValue("Text", this.text);
			si.AddValue("Name", this.Name);
			si.AddValue("IsChecked", this.treeNodeState[1]);
			si.AddValue("ImageIndex", this.ImageIndexer.Index);
			si.AddValue("ImageKey", this.ImageIndexer.Key);
			si.AddValue("SelectedImageIndex", this.SelectedImageIndexer.Index);
			si.AddValue("SelectedImageKey", this.SelectedImageIndexer.Key);
			if (this.treeView != null && this.treeView.StateImageList != null)
			{
				si.AddValue("StateImageIndex", this.StateImageIndexer.Index);
			}
			if (this.treeView != null && this.treeView.StateImageList != null)
			{
				si.AddValue("StateImageKey", this.StateImageIndexer.Key);
			}
			si.AddValue("ChildCount", this.childCount);
			if (this.childCount > 0)
			{
				for (int i = 0; i < this.childCount; i++)
				{
					si.AddValue("children" + i, this.children[i], typeof(TreeNode));
				}
			}
			if (this.userData != null && this.userData.GetType().IsSerializable)
			{
				si.AddValue("UserData", this.userData, this.userData.GetType());
			}
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x0015A1B8 File Offset: 0x001591B8
		public void Toggle()
		{
			if (this.IsExpanded)
			{
				this.Collapse();
				return;
			}
			this.Expand();
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x0015A1CF File Offset: 0x001591CF
		public override string ToString()
		{
			return "TreeNode: " + ((this.text == null) ? "" : this.text);
		}

		// Token: 0x06005F1E RID: 24350 RVA: 0x0015A1F0 File Offset: 0x001591F0
		private void UpdateNode(int mask)
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			TreeView treeView = this.TreeView;
			NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
			tv_ITEM.mask = (16 | mask);
			tv_ITEM.hItem = this.handle;
			if ((mask & 1) != 0)
			{
				tv_ITEM.pszText = Marshal.StringToHGlobalAuto(this.text);
			}
			if ((mask & 2) != 0)
			{
				tv_ITEM.iImage = ((this.ImageIndexer.ActualIndex == -1) ? treeView.ImageIndexer.ActualIndex : this.ImageIndexer.ActualIndex);
			}
			if ((mask & 32) != 0)
			{
				tv_ITEM.iSelectedImage = ((this.SelectedImageIndexer.ActualIndex == -1) ? treeView.SelectedImageIndexer.ActualIndex : this.SelectedImageIndexer.ActualIndex);
			}
			if ((mask & 8) != 0)
			{
				tv_ITEM.stateMask = 61440;
				if (this.StateImageIndexer.ActualIndex != -1)
				{
					tv_ITEM.state = this.StateImageIndexer.ActualIndex + 1 << 12;
				}
			}
			if ((mask & 4) != 0)
			{
				tv_ITEM.lParam = this.handle;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
			if ((mask & 1) != 0)
			{
				Marshal.FreeHGlobal(tv_ITEM.pszText);
				if (treeView.Scrollable)
				{
					treeView.ForceScrollbarUpdate(false);
				}
			}
		}

		// Token: 0x06005F1F RID: 24351 RVA: 0x0015A338 File Offset: 0x00159338
		internal void UpdateImage()
		{
			NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
			tv_ITEM.mask = 18;
			tv_ITEM.hItem = this.Handle;
			tv_ITEM.iImage = Math.Max(0, (this.ImageIndexer.ActualIndex >= this.TreeView.ImageList.Images.Count) ? (this.TreeView.ImageList.Images.Count - 1) : this.ImageIndexer.ActualIndex);
			UnsafeNativeMethods.SendMessage(new HandleRef(this.TreeView, this.TreeView.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x0015A3DA File Offset: 0x001593DA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			this.Serialize(si, context);
		}

		// Token: 0x040039A5 RID: 14757
		private const int SHIFTVAL = 12;

		// Token: 0x040039A6 RID: 14758
		private const int CHECKED = 8192;

		// Token: 0x040039A7 RID: 14759
		private const int UNCHECKED = 4096;

		// Token: 0x040039A8 RID: 14760
		private const int ALLOWEDIMAGES = 14;

		// Token: 0x040039A9 RID: 14761
		internal const int MAX_TREENODES_OPS = 200;

		// Token: 0x040039AA RID: 14762
		private const int TREENODESTATE_isChecked = 1;

		// Token: 0x040039AB RID: 14763
		internal OwnerDrawPropertyBag propBag;

		// Token: 0x040039AC RID: 14764
		internal IntPtr handle;

		// Token: 0x040039AD RID: 14765
		internal string text;

		// Token: 0x040039AE RID: 14766
		internal string name;

		// Token: 0x040039AF RID: 14767
		private BitVector32 treeNodeState;

		// Token: 0x040039B0 RID: 14768
		private TreeNode.TreeNodeImageIndexer imageIndexer;

		// Token: 0x040039B1 RID: 14769
		private TreeNode.TreeNodeImageIndexer selectedImageIndexer;

		// Token: 0x040039B2 RID: 14770
		private TreeNode.TreeNodeImageIndexer stateImageIndexer;

		// Token: 0x040039B3 RID: 14771
		private string toolTipText = "";

		// Token: 0x040039B4 RID: 14772
		private ContextMenu contextMenu;

		// Token: 0x040039B5 RID: 14773
		private ContextMenuStrip contextMenuStrip;

		// Token: 0x040039B6 RID: 14774
		internal bool nodesCleared;

		// Token: 0x040039B7 RID: 14775
		internal int index;

		// Token: 0x040039B8 RID: 14776
		internal int childCount;

		// Token: 0x040039B9 RID: 14777
		internal TreeNode[] children;

		// Token: 0x040039BA RID: 14778
		internal TreeNode parent;

		// Token: 0x040039BB RID: 14779
		internal TreeView treeView;

		// Token: 0x040039BC RID: 14780
		private bool expandOnRealization;

		// Token: 0x040039BD RID: 14781
		private bool collapseOnRealization;

		// Token: 0x040039BE RID: 14782
		private TreeNodeCollection nodes;

		// Token: 0x040039BF RID: 14783
		private object userData;

		// Token: 0x040039C0 RID: 14784
		private static readonly int insertMask = 35;

		// Token: 0x020006F7 RID: 1783
		internal class TreeNodeImageIndexer : ImageList.Indexer
		{
			// Token: 0x06005F22 RID: 24354 RVA: 0x0015A3ED File Offset: 0x001593ED
			public TreeNodeImageIndexer(TreeNode node, TreeNode.TreeNodeImageIndexer.ImageListType imageListType)
			{
				this.owner = node;
				this.imageListType = imageListType;
			}

			// Token: 0x1700141D RID: 5149
			// (get) Token: 0x06005F23 RID: 24355 RVA: 0x0015A403 File Offset: 0x00159403
			// (set) Token: 0x06005F24 RID: 24356 RVA: 0x0015A43E File Offset: 0x0015943E
			public override ImageList ImageList
			{
				get
				{
					if (this.owner.TreeView == null)
					{
						return null;
					}
					if (this.imageListType == TreeNode.TreeNodeImageIndexer.ImageListType.State)
					{
						return this.owner.TreeView.StateImageList;
					}
					return this.owner.TreeView.ImageList;
				}
				set
				{
				}
			}

			// Token: 0x040039C1 RID: 14785
			private TreeNode owner;

			// Token: 0x040039C2 RID: 14786
			private TreeNode.TreeNodeImageIndexer.ImageListType imageListType;

			// Token: 0x020006F8 RID: 1784
			public enum ImageListType
			{
				// Token: 0x040039C4 RID: 14788
				Default,
				// Token: 0x040039C5 RID: 14789
				State
			}
		}
	}
}
