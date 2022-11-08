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

namespace System.Windows.Forms
{
	// Token: 0x02000492 RID: 1170
	[DefaultProperty("Text")]
	[TypeConverter(typeof(ListViewItemConverter))]
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	[Serializable]
	public class ListViewItem : ICloneable, ISerializable
	{
		// Token: 0x060045CB RID: 17867 RVA: 0x000FD8F4 File Offset: 0x000FC8F4
		public ListViewItem()
		{
			this.StateSelected = false;
			this.UseItemStyleForSubItems = true;
			this.SavedStateImageIndex = -1;
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x000FD94E File Offset: 0x000FC94E
		protected ListViewItem(SerializationInfo info, StreamingContext context) : this()
		{
			this.Deserialize(info, context);
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x000FD95E File Offset: 0x000FC95E
		public ListViewItem(string text) : this(text, -1)
		{
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x000FD968 File Offset: 0x000FC968
		public ListViewItem(string text, int imageIndex) : this()
		{
			this.ImageIndexer.Index = imageIndex;
			this.Text = text;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x000FD983 File Offset: 0x000FC983
		public ListViewItem(string[] items) : this(items, -1)
		{
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x000FD990 File Offset: 0x000FC990
		public ListViewItem(string[] items, int imageIndex) : this()
		{
			this.ImageIndexer.Index = imageIndex;
			if (items != null && items.Length > 0)
			{
				this.subItems = new ListViewItem.ListViewSubItem[items.Length];
				for (int i = 0; i < items.Length; i++)
				{
					this.subItems[i] = new ListViewItem.ListViewSubItem(this, items[i]);
				}
				this.SubItemCount = items.Length;
			}
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x000FD9EE File Offset: 0x000FC9EE
		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font) : this(items, imageIndex)
		{
			this.ForeColor = foreColor;
			this.BackColor = backColor;
			this.Font = font;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x000FDA10 File Offset: 0x000FCA10
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, int imageIndex) : this()
		{
			this.ImageIndexer.Index = imageIndex;
			this.subItems = subItems;
			this.SubItemCount = this.subItems.Length;
			for (int i = 0; i < subItems.Length; i++)
			{
				subItems[i].owner = this;
			}
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x000FDA5B File Offset: 0x000FCA5B
		public ListViewItem(ListViewGroup group) : this()
		{
			this.Group = group;
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x000FDA6A File Offset: 0x000FCA6A
		public ListViewItem(string text, ListViewGroup group) : this(text)
		{
			this.Group = group;
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x000FDA7A File Offset: 0x000FCA7A
		public ListViewItem(string text, int imageIndex, ListViewGroup group) : this(text, imageIndex)
		{
			this.Group = group;
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x000FDA8B File Offset: 0x000FCA8B
		public ListViewItem(string[] items, ListViewGroup group) : this(items)
		{
			this.Group = group;
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x000FDA9B File Offset: 0x000FCA9B
		public ListViewItem(string[] items, int imageIndex, ListViewGroup group) : this(items, imageIndex)
		{
			this.Group = group;
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x000FDAAC File Offset: 0x000FCAAC
		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font, ListViewGroup group) : this(items, imageIndex, foreColor, backColor, font)
		{
			this.Group = group;
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x000FDAC3 File Offset: 0x000FCAC3
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, int imageIndex, ListViewGroup group) : this(subItems, imageIndex)
		{
			this.Group = group;
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x000FDAD4 File Offset: 0x000FCAD4
		public ListViewItem(string text, string imageKey) : this()
		{
			this.ImageIndexer.Key = imageKey;
			this.Text = text;
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x000FDAF0 File Offset: 0x000FCAF0
		public ListViewItem(string[] items, string imageKey) : this()
		{
			this.ImageIndexer.Key = imageKey;
			if (items != null && items.Length > 0)
			{
				this.subItems = new ListViewItem.ListViewSubItem[items.Length];
				for (int i = 0; i < items.Length; i++)
				{
					this.subItems[i] = new ListViewItem.ListViewSubItem(this, items[i]);
				}
				this.SubItemCount = items.Length;
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x000FDB4E File Offset: 0x000FCB4E
		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font) : this(items, imageKey)
		{
			this.ForeColor = foreColor;
			this.BackColor = backColor;
			this.Font = font;
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x000FDB70 File Offset: 0x000FCB70
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, string imageKey) : this()
		{
			this.ImageIndexer.Key = imageKey;
			this.subItems = subItems;
			this.SubItemCount = this.subItems.Length;
			for (int i = 0; i < subItems.Length; i++)
			{
				subItems[i].owner = this;
			}
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x000FDBBB File Offset: 0x000FCBBB
		public ListViewItem(string text, string imageKey, ListViewGroup group) : this(text, imageKey)
		{
			this.Group = group;
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x000FDBCC File Offset: 0x000FCBCC
		public ListViewItem(string[] items, string imageKey, ListViewGroup group) : this(items, imageKey)
		{
			this.Group = group;
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x000FDBDD File Offset: 0x000FCBDD
		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group) : this(items, imageKey, foreColor, backColor, font)
		{
			this.Group = group;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x000FDBF4 File Offset: 0x000FCBF4
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, string imageKey, ListViewGroup group) : this(subItems, imageKey)
		{
			this.Group = group;
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x000FDC05 File Offset: 0x000FCC05
		// (set) Token: 0x060045E3 RID: 17891 RVA: 0x000FDC36 File Offset: 0x000FCC36
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color BackColor
		{
			get
			{
				if (this.SubItemCount != 0)
				{
					return this.subItems[0].BackColor;
				}
				if (this.listView != null)
				{
					return this.listView.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				this.SubItems[0].BackColor = value;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x000FDC4C File Offset: 0x000FCC4C
		[Browsable(false)]
		public Rectangle Bounds
		{
			get
			{
				if (this.listView != null)
				{
					return this.listView.GetItemRect(this.Index);
				}
				return default(Rectangle);
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x060045E5 RID: 17893 RVA: 0x000FDC7C File Offset: 0x000FCC7C
		// (set) Token: 0x060045E6 RID: 17894 RVA: 0x000FDC88 File Offset: 0x000FCC88
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool Checked
		{
			get
			{
				return this.StateImageIndex > 0;
			}
			set
			{
				if (this.Checked != value)
				{
					if (this.listView != null && this.listView.IsHandleCreated)
					{
						this.StateImageIndex = (value ? 1 : 0);
						if (this.listView != null && !this.listView.UseCompatibleStateImageBehavior && !this.listView.CheckBoxes)
						{
							this.listView.UpdateSavedCheckedItems(this, value);
							return;
						}
					}
					else
					{
						this.SavedStateImageIndex = (value ? 1 : 0);
					}
				}
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x000FDCFD File Offset: 0x000FCCFD
		// (set) Token: 0x060045E8 RID: 17896 RVA: 0x000FDD2E File Offset: 0x000FCD2E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool Focused
		{
			get
			{
				return this.listView != null && this.listView.IsHandleCreated && this.listView.GetItemState(this.Index, 1) != 0;
			}
			set
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemState(this.Index, value ? 1 : 0, 1);
				}
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x060045E9 RID: 17897 RVA: 0x000FDD5E File Offset: 0x000FCD5E
		// (set) Token: 0x060045EA RID: 17898 RVA: 0x000FDD8F File Offset: 0x000FCD8F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public Font Font
		{
			get
			{
				if (this.SubItemCount != 0)
				{
					return this.subItems[0].Font;
				}
				if (this.listView != null)
				{
					return this.listView.Font;
				}
				return Control.DefaultFont;
			}
			set
			{
				this.SubItems[0].Font = value;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x060045EB RID: 17899 RVA: 0x000FDDA3 File Offset: 0x000FCDA3
		// (set) Token: 0x060045EC RID: 17900 RVA: 0x000FDDD4 File Offset: 0x000FCDD4
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color ForeColor
		{
			get
			{
				if (this.SubItemCount != 0)
				{
					return this.subItems[0].ForeColor;
				}
				if (this.listView != null)
				{
					return this.listView.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				this.SubItems[0].ForeColor = value;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x060045ED RID: 17901 RVA: 0x000FDDE8 File Offset: 0x000FCDE8
		// (set) Token: 0x060045EE RID: 17902 RVA: 0x000FDDF0 File Offset: 0x000FCDF0
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[Localizable(true)]
		public ListViewGroup Group
		{
			get
			{
				return this.group;
			}
			set
			{
				if (this.group != value)
				{
					if (value != null)
					{
						value.Items.Add(this);
					}
					else
					{
						this.group.Items.Remove(this);
					}
				}
				this.groupName = null;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x000FDE28 File Offset: 0x000FCE28
		// (set) Token: 0x060045F0 RID: 17904 RVA: 0x000FDE88 File Offset: 0x000FCE88
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[DefaultValue(-1)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemImageIndexDescr")]
		[Localizable(true)]
		public int ImageIndex
		{
			get
			{
				if (this.ImageIndexer.Index != -1 && this.ImageList != null && this.ImageIndexer.Index >= this.ImageList.Images.Count)
				{
					return this.ImageList.Images.Count - 1;
				}
				return this.ImageIndexer.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.ImageIndexer.Index = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemImage(this.Index, this.ImageIndexer.ActualIndex);
				}
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x060045F1 RID: 17905 RVA: 0x000FDF1B File Offset: 0x000FCF1B
		internal ListViewItem.ListViewItemImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ListViewItem.ListViewItemImageIndexer(this);
				}
				return this.imageIndexer;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x060045F2 RID: 17906 RVA: 0x000FDF37 File Offset: 0x000FCF37
		// (set) Token: 0x060045F3 RID: 17907 RVA: 0x000FDF44 File Offset: 0x000FCF44
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatBehavior")]
		[DefaultValue("")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemImage(this.Index, this.ImageIndexer.ActualIndex);
				}
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x000FDF84 File Offset: 0x000FCF84
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				if (this.listView != null)
				{
					switch (this.listView.View)
					{
					case View.LargeIcon:
					case View.Tile:
						return this.listView.LargeImageList;
					case View.Details:
					case View.SmallIcon:
					case View.List:
						return this.listView.SmallImageList;
					}
				}
				return null;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x000FDFDA File Offset: 0x000FCFDA
		// (set) Token: 0x060045F6 RID: 17910 RVA: 0x000FDFE4 File Offset: 0x000FCFE4
		[DefaultValue(0)]
		[SRCategory("CatDisplay")]
		[SRDescription("ListViewItemIndentCountDescr")]
		public int IndentCount
		{
			get
			{
				return this.indentCount;
			}
			set
			{
				if (value == this.indentCount)
				{
					return;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("IndentCount", SR.GetString("ListViewIndentCountCantBeNegative"));
				}
				this.indentCount = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemIndentCount(this.Index, this.indentCount);
				}
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x060045F7 RID: 17911 RVA: 0x000FE047 File Offset: 0x000FD047
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.listView != null)
				{
					if (!this.listView.VirtualMode)
					{
						this.lastIndex = this.listView.GetDisplayIndex(this, this.lastIndex);
					}
					return this.lastIndex;
				}
				return -1;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x000FE07E File Offset: 0x000FD07E
		[Browsable(false)]
		public ListView ListView
		{
			get
			{
				return this.listView;
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x000FE086 File Offset: 0x000FD086
		// (set) Token: 0x060045FA RID: 17914 RVA: 0x000FE0A3 File Offset: 0x000FD0A3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[Localizable(true)]
		public string Name
		{
			get
			{
				if (this.SubItemCount == 0)
				{
					return string.Empty;
				}
				return this.subItems[0].Name;
			}
			set
			{
				this.SubItems[0].Name = value;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x000FE0B7 File Offset: 0x000FD0B7
		// (set) Token: 0x060045FC RID: 17916 RVA: 0x000FE0EC File Offset: 0x000FD0EC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRCategory("CatDisplay")]
		public Point Position
		{
			get
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.position = this.listView.GetItemPosition(this.Index);
				}
				return this.position;
			}
			set
			{
				if (value.Equals(this.position))
				{
					return;
				}
				this.position = value;
				if (this.listView != null && this.listView.IsHandleCreated && !this.listView.VirtualMode)
				{
					this.listView.SetItemPosition(this.Index, this.position.X, this.position.Y);
				}
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x000FE164 File Offset: 0x000FD164
		internal int RawStateImageIndex
		{
			get
			{
				return this.SavedStateImageIndex + 1 << 12;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x000FE171 File Offset: 0x000FD171
		// (set) Token: 0x060045FF RID: 17919 RVA: 0x000FE185 File Offset: 0x000FD185
		private int SavedStateImageIndex
		{
			get
			{
				return this.state[ListViewItem.SavedStateImageIndexSection] - 1;
			}
			set
			{
				this.state[ListViewItem.StateImageMaskSet] = ((value == -1) ? 0 : 1);
				this.state[ListViewItem.SavedStateImageIndexSection] = value + 1;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004600 RID: 17920 RVA: 0x000FE1B2 File Offset: 0x000FD1B2
		// (set) Token: 0x06004601 RID: 17921 RVA: 0x000FE1E8 File Offset: 0x000FD1E8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Selected
		{
			get
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					return this.listView.GetItemState(this.Index, 2) != 0;
				}
				return this.StateSelected;
			}
			set
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemState(this.Index, value ? 2 : 0, 2);
					this.listView.SetSelectionMark(this.Index);
					return;
				}
				this.StateSelected = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.CacheSelectedStateForItem(this, value);
				}
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x000FE260 File Offset: 0x000FD260
		// (set) Token: 0x06004603 RID: 17923 RVA: 0x000FE2A8 File Offset: 0x000FD2A8
		[RelatedImageList("ListView.StateImageList")]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[SRDescription("ListViewItemStateImageIndexDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(-1)]
		public int StateImageIndex
		{
			get
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					int itemState = this.listView.GetItemState(this.Index, 61440);
					return (itemState >> 12) - 1;
				}
				return this.SavedStateImageIndex;
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
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.state[ListViewItem.StateImageMaskSet] = ((value == -1) ? 0 : 1);
					int num = value + 1 << 12;
					this.listView.SetItemState(this.Index, num, 61440);
				}
				this.SavedStateImageIndex = value;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06004604 RID: 17924 RVA: 0x000FE344 File Offset: 0x000FD344
		internal bool StateImageSet
		{
			get
			{
				return this.state[ListViewItem.StateImageMaskSet] != 0;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06004605 RID: 17925 RVA: 0x000FE35C File Offset: 0x000FD35C
		// (set) Token: 0x06004606 RID: 17926 RVA: 0x000FE371 File Offset: 0x000FD371
		internal bool StateSelected
		{
			get
			{
				return this.state[ListViewItem.StateSelectedSection] == 1;
			}
			set
			{
				this.state[ListViewItem.StateSelectedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004607 RID: 17927 RVA: 0x000FE38A File Offset: 0x000FD38A
		// (set) Token: 0x06004608 RID: 17928 RVA: 0x000FE39C File Offset: 0x000FD39C
		private int SubItemCount
		{
			get
			{
				return this.state[ListViewItem.SubItemCountSection];
			}
			set
			{
				this.state[ListViewItem.SubItemCountSection] = value;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x000FE3B0 File Offset: 0x000FD3B0
		[Editor("System.Windows.Forms.Design.ListViewSubItemCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewItemSubItemsDescr")]
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListViewItem.ListViewSubItemCollection SubItems
		{
			get
			{
				if (this.SubItemCount == 0)
				{
					this.subItems = new ListViewItem.ListViewSubItem[1];
					this.subItems[0] = new ListViewItem.ListViewSubItem(this, string.Empty);
					this.SubItemCount = 1;
				}
				if (this.listViewSubItemCollection == null)
				{
					this.listViewSubItemCollection = new ListViewItem.ListViewSubItemCollection(this);
				}
				return this.listViewSubItemCollection;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x000FE405 File Offset: 0x000FD405
		// (set) Token: 0x0600460B RID: 17931 RVA: 0x000FE40D File Offset: 0x000FD40D
		[SRCategory("CatData")]
		[SRDescription("ControlTagDescr")]
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(null)]
		[Localizable(false)]
		[Bindable(true)]
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

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x0600460C RID: 17932 RVA: 0x000FE416 File Offset: 0x000FD416
		// (set) Token: 0x0600460D RID: 17933 RVA: 0x000FE433 File Offset: 0x000FD433
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Text
		{
			get
			{
				if (this.SubItemCount == 0)
				{
					return string.Empty;
				}
				return this.subItems[0].Text;
			}
			set
			{
				this.SubItems[0].Text = value;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x000FE447 File Offset: 0x000FD447
		// (set) Token: 0x0600460F RID: 17935 RVA: 0x000FE450 File Offset: 0x000FD450
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (WindowsFormsUtils.SafeCompareStrings(this.toolTipText, value, false))
				{
					return;
				}
				this.toolTipText = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.ListViewItemToolTipChanged(this);
				}
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x000FE49F File Offset: 0x000FD49F
		// (set) Token: 0x06004611 RID: 17937 RVA: 0x000FE4B4 File Offset: 0x000FD4B4
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		public bool UseItemStyleForSubItems
		{
			get
			{
				return this.state[ListViewItem.StateWholeRowOneStyleSection] == 1;
			}
			set
			{
				this.state[ListViewItem.StateWholeRowOneStyleSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x000FE4D0 File Offset: 0x000FD4D0
		public void BeginEdit()
		{
			if (this.Index >= 0)
			{
				ListView listView = this.ListView;
				if (!listView.LabelEdit)
				{
					throw new InvalidOperationException(SR.GetString("ListViewBeginEditFailed"));
				}
				if (!listView.Focused)
				{
					listView.FocusInternal();
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(listView, listView.Handle), NativeMethods.LVM_EDITLABEL, this.Index, 0);
			}
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x000FE534 File Offset: 0x000FD534
		public virtual object Clone()
		{
			ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[this.SubItems.Count];
			for (int i = 0; i < this.SubItems.Count; i++)
			{
				ListViewItem.ListViewSubItem listViewSubItem = this.SubItems[i];
				array[i] = new ListViewItem.ListViewSubItem(null, listViewSubItem.Text, listViewSubItem.ForeColor, listViewSubItem.BackColor, listViewSubItem.Font);
				array[i].Tag = listViewSubItem.Tag;
			}
			Type type = base.GetType();
			ListViewItem listViewItem;
			if (type == typeof(ListViewItem))
			{
				listViewItem = new ListViewItem(array, this.ImageIndexer.Index);
			}
			else
			{
				listViewItem = (ListViewItem)Activator.CreateInstance(type);
			}
			listViewItem.subItems = array;
			listViewItem.ImageIndexer.Index = this.ImageIndexer.Index;
			listViewItem.SubItemCount = this.SubItemCount;
			listViewItem.Checked = this.Checked;
			listViewItem.UseItemStyleForSubItems = this.UseItemStyleForSubItems;
			listViewItem.Tag = this.Tag;
			if (!string.IsNullOrEmpty(this.ImageIndexer.Key))
			{
				listViewItem.ImageIndexer.Key = this.ImageIndexer.Key;
			}
			listViewItem.indentCount = this.indentCount;
			listViewItem.StateImageIndex = this.StateImageIndex;
			listViewItem.toolTipText = this.toolTipText;
			listViewItem.BackColor = this.BackColor;
			listViewItem.ForeColor = this.ForeColor;
			listViewItem.Font = this.Font;
			listViewItem.Text = this.Text;
			listViewItem.Group = this.Group;
			return listViewItem;
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x000FE6C3 File Offset: 0x000FD6C3
		public virtual void EnsureVisible()
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				this.listView.EnsureVisible(this.Index);
			}
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x000FE6EC File Offset: 0x000FD6EC
		public ListViewItem FindNearestItem(SearchDirectionHint searchDirection)
		{
			Rectangle bounds = this.Bounds;
			switch (searchDirection)
			{
			case SearchDirectionHint.Left:
				return this.ListView.FindNearestItem(searchDirection, bounds.Left, bounds.Top);
			case SearchDirectionHint.Up:
				return this.ListView.FindNearestItem(searchDirection, bounds.Left, bounds.Top);
			case SearchDirectionHint.Right:
				return this.ListView.FindNearestItem(searchDirection, bounds.Right, bounds.Top);
			case SearchDirectionHint.Down:
				return this.ListView.FindNearestItem(searchDirection, bounds.Left, bounds.Bottom);
			default:
				return null;
			}
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x000FE78C File Offset: 0x000FD78C
		public Rectangle GetBounds(ItemBoundsPortion portion)
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				return this.listView.GetItemRect(this.Index, portion);
			}
			return default(Rectangle);
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x000FE7CC File Offset: 0x000FD7CC
		public ListViewItem.ListViewSubItem GetSubItemAt(int x, int y)
		{
			if (this.listView == null || !this.listView.IsHandleCreated || this.listView.View != View.Details)
			{
				return null;
			}
			int num = -1;
			int num2 = -1;
			this.listView.GetSubItemAt(x, y, out num, out num2);
			if (num == this.Index && num2 != -1 && num2 < this.SubItems.Count)
			{
				return this.SubItems[num2];
			}
			return null;
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x000FE83C File Offset: 0x000FD83C
		internal void Host(ListView parent, int ID, int index)
		{
			this.ID = ID;
			this.listView = parent;
			if (index != -1)
			{
				this.UpdateStateToListView(index);
			}
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x000FE858 File Offset: 0x000FD858
		internal void UpdateGroupFromName()
		{
			if (string.IsNullOrEmpty(this.groupName))
			{
				return;
			}
			ListViewGroup listViewGroup = this.listView.Groups[this.groupName];
			this.Group = listViewGroup;
			this.groupName = null;
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x000FE898 File Offset: 0x000FD898
		internal void UpdateStateToListView(int index)
		{
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			this.UpdateStateToListView(index, ref lvitem, true);
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x000FE8B8 File Offset: 0x000FD8B8
		internal void UpdateStateToListView(int index, ref NativeMethods.LVITEM lvItem, bool updateOwner)
		{
			if (index == -1)
			{
				index = this.Index;
			}
			else
			{
				this.lastIndex = index;
			}
			int num = 0;
			int num2 = 0;
			if (this.StateSelected)
			{
				num |= 2;
				num2 |= 2;
			}
			if (this.SavedStateImageIndex > -1)
			{
				num |= this.SavedStateImageIndex + 1 << 12;
				num2 |= 61440;
			}
			lvItem.mask |= 8;
			lvItem.iItem = index;
			lvItem.stateMask |= num2;
			lvItem.state |= num;
			if (this.listView.GroupsEnabled)
			{
				lvItem.mask |= 256;
				lvItem.iGroupId = this.listView.GetNativeGroupId(this);
			}
			if (updateOwner)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), NativeMethods.LVM_SETITEM, 0, ref lvItem);
			}
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x000FE998 File Offset: 0x000FD998
		internal void UpdateStateFromListView(int displayIndex, bool checkSelection)
		{
			if (this.listView != null && this.listView.IsHandleCreated && displayIndex != -1)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 268;
				if (checkSelection)
				{
					lvitem.stateMask = 2;
				}
				lvitem.stateMask |= 61440;
				if (lvitem.stateMask == 0)
				{
					return;
				}
				lvitem.iItem = displayIndex;
				UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), NativeMethods.LVM_GETITEM, 0, ref lvitem);
				if (checkSelection)
				{
					this.StateSelected = ((lvitem.state & 2) != 0);
				}
				this.SavedStateImageIndex = ((lvitem.state & 61440) >> 12) - 1;
				this.group = null;
				foreach (object obj in this.ListView.Groups)
				{
					ListViewGroup listViewGroup = (ListViewGroup)obj;
					if (listViewGroup.ID == lvitem.iGroupId)
					{
						this.group = listViewGroup;
						break;
					}
				}
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x000FEAC8 File Offset: 0x000FDAC8
		internal void UnHost(bool checkSelection)
		{
			this.UnHost(this.Index, checkSelection);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x000FEAD8 File Offset: 0x000FDAD8
		internal void UnHost(int displayIndex, bool checkSelection)
		{
			this.UpdateStateFromListView(displayIndex, checkSelection);
			if (this.listView != null && (this.listView.Site == null || !this.listView.Site.DesignMode) && this.group != null)
			{
				this.group.Items.Remove(this);
			}
			this.ID = -1;
			this.listView = null;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x000FEB3B File Offset: 0x000FDB3B
		public virtual void Remove()
		{
			if (this.listView != null)
			{
				this.listView.Items.Remove(this);
			}
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x000FEB58 File Offset: 0x000FDB58
		protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
		{
			bool flag = false;
			string text = null;
			int num = -1;
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "Text")
				{
					this.Text = info.GetString(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "ImageIndex")
				{
					num = info.GetInt32(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "ImageKey")
				{
					text = info.GetString(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "SubItemCount")
				{
					this.SubItemCount = info.GetInt32(serializationEntry.Name);
					if (this.SubItemCount > 0)
					{
						flag = true;
					}
				}
				else if (serializationEntry.Name == "BackColor")
				{
					this.BackColor = (Color)info.GetValue(serializationEntry.Name, typeof(Color));
				}
				else if (serializationEntry.Name == "Checked")
				{
					this.Checked = info.GetBoolean(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "Font")
				{
					this.Font = (Font)info.GetValue(serializationEntry.Name, typeof(Font));
				}
				else if (serializationEntry.Name == "ForeColor")
				{
					this.ForeColor = (Color)info.GetValue(serializationEntry.Name, typeof(Color));
				}
				else if (serializationEntry.Name == "UseItemStyleForSubItems")
				{
					this.UseItemStyleForSubItems = info.GetBoolean(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "Group")
				{
					ListViewGroup listViewGroup = (ListViewGroup)info.GetValue(serializationEntry.Name, typeof(ListViewGroup));
					this.groupName = listViewGroup.Name;
				}
			}
			if (text != null)
			{
				this.ImageKey = text;
			}
			else if (num != -1)
			{
				this.ImageIndex = num;
			}
			if (flag)
			{
				ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[this.SubItemCount];
				for (int i = 1; i < this.SubItemCount; i++)
				{
					ListViewItem.ListViewSubItem listViewSubItem = (ListViewItem.ListViewSubItem)info.GetValue("SubItem" + i.ToString(CultureInfo.InvariantCulture), typeof(ListViewItem.ListViewSubItem));
					listViewSubItem.owner = this;
					array[i] = listViewSubItem;
				}
				array[0] = this.subItems[0];
				this.subItems = array;
			}
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x000FEE04 File Offset: 0x000FDE04
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected virtual void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Text", this.Text);
			info.AddValue("ImageIndex", this.ImageIndexer.Index);
			if (!string.IsNullOrEmpty(this.ImageIndexer.Key))
			{
				info.AddValue("ImageKey", this.ImageIndexer.Key);
			}
			if (this.SubItemCount > 1)
			{
				info.AddValue("SubItemCount", this.SubItemCount);
				for (int i = 1; i < this.SubItemCount; i++)
				{
					info.AddValue("SubItem" + i.ToString(CultureInfo.InvariantCulture), this.subItems[i], typeof(ListViewItem.ListViewSubItem));
				}
			}
			info.AddValue("BackColor", this.BackColor);
			info.AddValue("Checked", this.Checked);
			info.AddValue("Font", this.Font);
			info.AddValue("ForeColor", this.ForeColor);
			info.AddValue("UseItemStyleForSubItems", this.UseItemStyleForSubItems);
			if (this.Group != null)
			{
				info.AddValue("Group", this.Group);
			}
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x000FEF31 File Offset: 0x000FDF31
		internal void SetItemIndex(ListView listView, int index)
		{
			this.listView = listView;
			this.lastIndex = index;
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x000FEF41 File Offset: 0x000FDF41
		internal bool ShouldSerializeText()
		{
			return false;
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x000FEF44 File Offset: 0x000FDF44
		private bool ShouldSerializePosition()
		{
			return !this.position.Equals(new Point(-1, -1));
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x000FEF66 File Offset: 0x000FDF66
		public override string ToString()
		{
			return "ListViewItem: {" + this.Text + "}";
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x000FEF7D File Offset: 0x000FDF7D
		internal void InvalidateListView()
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				this.listView.Invalidate();
			}
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x000FEF9F File Offset: 0x000FDF9F
		internal void UpdateSubItems(int index)
		{
			this.UpdateSubItems(index, this.SubItemCount);
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x000FEFB0 File Offset: 0x000FDFB0
		internal void UpdateSubItems(int index, int oldCount)
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				int subItemCount = this.SubItemCount;
				int index2 = this.Index;
				if (index != -1)
				{
					this.listView.SetItemText(index2, index, this.subItems[index].Text);
				}
				else
				{
					for (int i = 0; i < subItemCount; i++)
					{
						this.listView.SetItemText(index2, i, this.subItems[i].Text);
					}
				}
				for (int j = subItemCount; j < oldCount; j++)
				{
					this.listView.SetItemText(index2, j, string.Empty);
				}
			}
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x000FF044 File Offset: 0x000FE044
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.Serialize(info, context);
		}

		// Token: 0x0400217A RID: 8570
		private const int MAX_SUBITEMS = 4096;

		// Token: 0x0400217B RID: 8571
		private static readonly BitVector32.Section StateSelectedSection = BitVector32.CreateSection(1);

		// Token: 0x0400217C RID: 8572
		private static readonly BitVector32.Section StateImageMaskSet = BitVector32.CreateSection(1, ListViewItem.StateSelectedSection);

		// Token: 0x0400217D RID: 8573
		private static readonly BitVector32.Section StateWholeRowOneStyleSection = BitVector32.CreateSection(1, ListViewItem.StateImageMaskSet);

		// Token: 0x0400217E RID: 8574
		private static readonly BitVector32.Section SavedStateImageIndexSection = BitVector32.CreateSection(15, ListViewItem.StateWholeRowOneStyleSection);

		// Token: 0x0400217F RID: 8575
		private static readonly BitVector32.Section SubItemCountSection = BitVector32.CreateSection(4096, ListViewItem.SavedStateImageIndexSection);

		// Token: 0x04002180 RID: 8576
		private int indentCount;

		// Token: 0x04002181 RID: 8577
		private Point position = new Point(-1, -1);

		// Token: 0x04002182 RID: 8578
		internal ListView listView;

		// Token: 0x04002183 RID: 8579
		internal ListViewGroup group;

		// Token: 0x04002184 RID: 8580
		private string groupName;

		// Token: 0x04002185 RID: 8581
		private ListViewItem.ListViewSubItemCollection listViewSubItemCollection;

		// Token: 0x04002186 RID: 8582
		private ListViewItem.ListViewSubItem[] subItems;

		// Token: 0x04002187 RID: 8583
		private int lastIndex = -1;

		// Token: 0x04002188 RID: 8584
		internal int ID = -1;

		// Token: 0x04002189 RID: 8585
		private BitVector32 state = default(BitVector32);

		// Token: 0x0400218A RID: 8586
		private ListViewItem.ListViewItemImageIndexer imageIndexer;

		// Token: 0x0400218B RID: 8587
		private string toolTipText = string.Empty;

		// Token: 0x0400218C RID: 8588
		private object userData;

		// Token: 0x02000493 RID: 1171
		internal class ListViewItemImageIndexer : ImageList.Indexer
		{
			// Token: 0x0600462B RID: 17963 RVA: 0x000FF0AD File Offset: 0x000FE0AD
			public ListViewItemImageIndexer(ListViewItem item)
			{
				this.owner = item;
			}

			// Token: 0x17000DE6 RID: 3558
			// (get) Token: 0x0600462C RID: 17964 RVA: 0x000FF0BC File Offset: 0x000FE0BC
			// (set) Token: 0x0600462D RID: 17965 RVA: 0x000FF0D3 File Offset: 0x000FE0D3
			public override ImageList ImageList
			{
				get
				{
					if (this.owner != null)
					{
						return this.owner.ImageList;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x0400218D RID: 8589
			private ListViewItem owner;
		}

		// Token: 0x02000494 RID: 1172
		[TypeConverter(typeof(ListViewSubItemConverter))]
		[ToolboxItem(false)]
		[DesignTimeVisible(false)]
		[DefaultProperty("Text")]
		[Serializable]
		public class ListViewSubItem
		{
			// Token: 0x0600462E RID: 17966 RVA: 0x000FF0D5 File Offset: 0x000FE0D5
			public ListViewSubItem()
			{
			}

			// Token: 0x0600462F RID: 17967 RVA: 0x000FF0DD File Offset: 0x000FE0DD
			public ListViewSubItem(ListViewItem owner, string text)
			{
				this.owner = owner;
				this.text = text;
			}

			// Token: 0x06004630 RID: 17968 RVA: 0x000FF0F4 File Offset: 0x000FE0F4
			public ListViewSubItem(ListViewItem owner, string text, Color foreColor, Color backColor, Font font)
			{
				this.owner = owner;
				this.text = text;
				this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
				this.style.foreColor = foreColor;
				this.style.backColor = backColor;
				this.style.font = font;
			}

			// Token: 0x17000DE7 RID: 3559
			// (get) Token: 0x06004631 RID: 17969 RVA: 0x000FF148 File Offset: 0x000FE148
			// (set) Token: 0x06004632 RID: 17970 RVA: 0x000FF1AC File Offset: 0x000FE1AC
			public Color BackColor
			{
				get
				{
					if (this.style != null && this.style.backColor != Color.Empty)
					{
						return this.style.backColor;
					}
					if (this.owner != null && this.owner.listView != null)
					{
						return this.owner.listView.BackColor;
					}
					return SystemColors.Window;
				}
				set
				{
					if (this.style == null)
					{
						this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
					}
					if (this.style.backColor != value)
					{
						this.style.backColor = value;
						if (this.owner != null)
						{
							this.owner.InvalidateListView();
						}
					}
				}
			}

			// Token: 0x17000DE8 RID: 3560
			// (get) Token: 0x06004633 RID: 17971 RVA: 0x000FF200 File Offset: 0x000FE200
			[Browsable(false)]
			public Rectangle Bounds
			{
				get
				{
					if (this.owner != null && this.owner.listView != null && this.owner.listView.IsHandleCreated)
					{
						return this.owner.listView.GetSubItemRect(this.owner.Index, this.owner.SubItems.IndexOf(this));
					}
					return Rectangle.Empty;
				}
			}

			// Token: 0x17000DE9 RID: 3561
			// (get) Token: 0x06004634 RID: 17972 RVA: 0x000FF266 File Offset: 0x000FE266
			internal bool CustomBackColor
			{
				get
				{
					return this.style != null && !this.style.backColor.IsEmpty;
				}
			}

			// Token: 0x17000DEA RID: 3562
			// (get) Token: 0x06004635 RID: 17973 RVA: 0x000FF285 File Offset: 0x000FE285
			internal bool CustomFont
			{
				get
				{
					return this.style != null && this.style.font != null;
				}
			}

			// Token: 0x17000DEB RID: 3563
			// (get) Token: 0x06004636 RID: 17974 RVA: 0x000FF2A2 File Offset: 0x000FE2A2
			internal bool CustomForeColor
			{
				get
				{
					return this.style != null && !this.style.foreColor.IsEmpty;
				}
			}

			// Token: 0x17000DEC RID: 3564
			// (get) Token: 0x06004637 RID: 17975 RVA: 0x000FF2C1 File Offset: 0x000FE2C1
			internal bool CustomStyle
			{
				get
				{
					return this.style != null;
				}
			}

			// Token: 0x17000DED RID: 3565
			// (get) Token: 0x06004638 RID: 17976 RVA: 0x000FF2D0 File Offset: 0x000FE2D0
			// (set) Token: 0x06004639 RID: 17977 RVA: 0x000FF32C File Offset: 0x000FE32C
			[Localizable(true)]
			public Font Font
			{
				get
				{
					if (this.style != null && this.style.font != null)
					{
						return this.style.font;
					}
					if (this.owner != null && this.owner.listView != null)
					{
						return this.owner.listView.Font;
					}
					return Control.DefaultFont;
				}
				set
				{
					if (this.style == null)
					{
						this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
					}
					if (this.style.font != value)
					{
						this.style.font = value;
						if (this.owner != null)
						{
							this.owner.InvalidateListView();
						}
					}
				}
			}

			// Token: 0x17000DEE RID: 3566
			// (get) Token: 0x0600463A RID: 17978 RVA: 0x000FF37C File Offset: 0x000FE37C
			// (set) Token: 0x0600463B RID: 17979 RVA: 0x000FF3E0 File Offset: 0x000FE3E0
			public Color ForeColor
			{
				get
				{
					if (this.style != null && this.style.foreColor != Color.Empty)
					{
						return this.style.foreColor;
					}
					if (this.owner != null && this.owner.listView != null)
					{
						return this.owner.listView.ForeColor;
					}
					return SystemColors.WindowText;
				}
				set
				{
					if (this.style == null)
					{
						this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
					}
					if (this.style.foreColor != value)
					{
						this.style.foreColor = value;
						if (this.owner != null)
						{
							this.owner.InvalidateListView();
						}
					}
				}
			}

			// Token: 0x17000DEF RID: 3567
			// (get) Token: 0x0600463C RID: 17980 RVA: 0x000FF432 File Offset: 0x000FE432
			// (set) Token: 0x0600463D RID: 17981 RVA: 0x000FF43A File Offset: 0x000FE43A
			[SRDescription("ControlTagDescr")]
			[TypeConverter(typeof(StringConverter))]
			[Localizable(false)]
			[Bindable(true)]
			[DefaultValue(null)]
			[SRCategory("CatData")]
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

			// Token: 0x17000DF0 RID: 3568
			// (get) Token: 0x0600463E RID: 17982 RVA: 0x000FF443 File Offset: 0x000FE443
			// (set) Token: 0x0600463F RID: 17983 RVA: 0x000FF459 File Offset: 0x000FE459
			[Localizable(true)]
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
					if (this.owner != null)
					{
						this.owner.UpdateSubItems(-1);
					}
				}
			}

			// Token: 0x17000DF1 RID: 3569
			// (get) Token: 0x06004640 RID: 17984 RVA: 0x000FF476 File Offset: 0x000FE476
			// (set) Token: 0x06004641 RID: 17985 RVA: 0x000FF48C File Offset: 0x000FE48C
			[Localizable(true)]
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
					if (this.owner != null)
					{
						this.owner.UpdateSubItems(-1);
					}
				}
			}

			// Token: 0x06004642 RID: 17986 RVA: 0x000FF4A9 File Offset: 0x000FE4A9
			[OnDeserializing]
			private void OnDeserializing(StreamingContext ctx)
			{
			}

			// Token: 0x06004643 RID: 17987 RVA: 0x000FF4AB File Offset: 0x000FE4AB
			[OnDeserialized]
			private void OnDeserialized(StreamingContext ctx)
			{
				this.name = null;
				this.userData = null;
			}

			// Token: 0x06004644 RID: 17988 RVA: 0x000FF4BB File Offset: 0x000FE4BB
			[OnSerializing]
			private void OnSerializing(StreamingContext ctx)
			{
			}

			// Token: 0x06004645 RID: 17989 RVA: 0x000FF4BD File Offset: 0x000FE4BD
			[OnSerialized]
			private void OnSerialized(StreamingContext ctx)
			{
			}

			// Token: 0x06004646 RID: 17990 RVA: 0x000FF4BF File Offset: 0x000FE4BF
			public void ResetStyle()
			{
				if (this.style != null)
				{
					this.style = null;
					if (this.owner != null)
					{
						this.owner.InvalidateListView();
					}
				}
			}

			// Token: 0x06004647 RID: 17991 RVA: 0x000FF4E3 File Offset: 0x000FE4E3
			public override string ToString()
			{
				return "ListViewSubItem: {" + this.Text + "}";
			}

			// Token: 0x0400218E RID: 8590
			[NonSerialized]
			internal ListViewItem owner;

			// Token: 0x0400218F RID: 8591
			private string text;

			// Token: 0x04002190 RID: 8592
			[OptionalField(VersionAdded = 2)]
			private string name;

			// Token: 0x04002191 RID: 8593
			private ListViewItem.ListViewSubItem.SubItemStyle style;

			// Token: 0x04002192 RID: 8594
			[OptionalField(VersionAdded = 2)]
			private object userData;

			// Token: 0x02000495 RID: 1173
			[Serializable]
			private class SubItemStyle
			{
				// Token: 0x04002193 RID: 8595
				public Color backColor = Color.Empty;

				// Token: 0x04002194 RID: 8596
				public Color foreColor = Color.Empty;

				// Token: 0x04002195 RID: 8597
				public Font font;
			}
		}

		// Token: 0x02000496 RID: 1174
		public class ListViewSubItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06004649 RID: 17993 RVA: 0x000FF518 File Offset: 0x000FE518
			public ListViewSubItemCollection(ListViewItem owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000DF2 RID: 3570
			// (get) Token: 0x0600464A RID: 17994 RVA: 0x000FF52E File Offset: 0x000FE52E
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.SubItemCount;
				}
			}

			// Token: 0x17000DF3 RID: 3571
			// (get) Token: 0x0600464B RID: 17995 RVA: 0x000FF53B File Offset: 0x000FE53B
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000DF4 RID: 3572
			// (get) Token: 0x0600464C RID: 17996 RVA: 0x000FF53E File Offset: 0x000FE53E
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000DF5 RID: 3573
			// (get) Token: 0x0600464D RID: 17997 RVA: 0x000FF541 File Offset: 0x000FE541
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000DF6 RID: 3574
			// (get) Token: 0x0600464E RID: 17998 RVA: 0x000FF544 File Offset: 0x000FE544
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000DF7 RID: 3575
			public ListViewItem.ListViewSubItem this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.subItems[index];
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.owner.subItems[index] = value;
					this.owner.UpdateSubItems(index);
				}
			}

			// Token: 0x17000DF8 RID: 3576
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ListViewItem.ListViewSubItem)
					{
						this[index] = (ListViewItem.ListViewSubItem)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ListViewBadListViewSubItem"), "value");
				}
			}

			// Token: 0x17000DF9 RID: 3577
			public virtual ListViewItem.ListViewSubItem this[string key]
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

			// Token: 0x06004654 RID: 18004 RVA: 0x000FF678 File Offset: 0x000FE678
			public ListViewItem.ListViewSubItem Add(ListViewItem.ListViewSubItem item)
			{
				this.EnsureSubItemSpace(1, -1);
				item.owner = this.owner;
				this.owner.subItems[this.owner.SubItemCount] = item;
				this.owner.UpdateSubItems(this.owner.SubItemCount++);
				return item;
			}

			// Token: 0x06004655 RID: 18005 RVA: 0x000FF6D4 File Offset: 0x000FE6D4
			public ListViewItem.ListViewSubItem Add(string text)
			{
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem(this.owner, text);
				this.Add(listViewSubItem);
				return listViewSubItem;
			}

			// Token: 0x06004656 RID: 18006 RVA: 0x000FF6F8 File Offset: 0x000FE6F8
			public ListViewItem.ListViewSubItem Add(string text, Color foreColor, Color backColor, Font font)
			{
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem(this.owner, text, foreColor, backColor, font);
				this.Add(listViewSubItem);
				return listViewSubItem;
			}

			// Token: 0x06004657 RID: 18007 RVA: 0x000FF720 File Offset: 0x000FE720
			public void AddRange(ListViewItem.ListViewSubItem[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSubItemSpace(items.Length, -1);
				foreach (ListViewItem.ListViewSubItem listViewSubItem in items)
				{
					if (listViewSubItem != null)
					{
						this.owner.subItems[this.owner.SubItemCount++] = listViewSubItem;
					}
				}
				this.owner.UpdateSubItems(-1);
			}

			// Token: 0x06004658 RID: 18008 RVA: 0x000FF78C File Offset: 0x000FE78C
			public void AddRange(string[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSubItemSpace(items.Length, -1);
				foreach (string text in items)
				{
					if (text != null)
					{
						this.owner.subItems[this.owner.SubItemCount++] = new ListViewItem.ListViewSubItem(this.owner, text);
					}
				}
				this.owner.UpdateSubItems(-1);
			}

			// Token: 0x06004659 RID: 18009 RVA: 0x000FF804 File Offset: 0x000FE804
			public void AddRange(string[] items, Color foreColor, Color backColor, Font font)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSubItemSpace(items.Length, -1);
				foreach (string text in items)
				{
					if (text != null)
					{
						this.owner.subItems[this.owner.SubItemCount++] = new ListViewItem.ListViewSubItem(this.owner, text, foreColor, backColor, font);
					}
				}
				this.owner.UpdateSubItems(-1);
			}

			// Token: 0x0600465A RID: 18010 RVA: 0x000FF87E File Offset: 0x000FE87E
			int IList.Add(object item)
			{
				if (item is ListViewItem.ListViewSubItem)
				{
					return this.IndexOf(this.Add((ListViewItem.ListViewSubItem)item));
				}
				throw new ArgumentException(SR.GetString("ListViewSubItemCollectionInvalidArgument"));
			}

			// Token: 0x0600465B RID: 18011 RVA: 0x000FF8AC File Offset: 0x000FE8AC
			public void Clear()
			{
				int subItemCount = this.owner.SubItemCount;
				if (subItemCount > 0)
				{
					this.owner.SubItemCount = 0;
					this.owner.UpdateSubItems(-1, subItemCount);
				}
			}

			// Token: 0x0600465C RID: 18012 RVA: 0x000FF8E2 File Offset: 0x000FE8E2
			public bool Contains(ListViewItem.ListViewSubItem subItem)
			{
				return this.IndexOf(subItem) != -1;
			}

			// Token: 0x0600465D RID: 18013 RVA: 0x000FF8F1 File Offset: 0x000FE8F1
			bool IList.Contains(object subItem)
			{
				return subItem is ListViewItem.ListViewSubItem && this.Contains((ListViewItem.ListViewSubItem)subItem);
			}

			// Token: 0x0600465E RID: 18014 RVA: 0x000FF909 File Offset: 0x000FE909
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x0600465F RID: 18015 RVA: 0x000FF918 File Offset: 0x000FE918
			private void EnsureSubItemSpace(int size, int index)
			{
				if (this.owner.SubItemCount == 4096)
				{
					throw new InvalidOperationException(SR.GetString("ErrorCollectionFull"));
				}
				if (this.owner.SubItemCount + size <= this.owner.subItems.Length)
				{
					if (index != -1)
					{
						for (int i = this.owner.SubItemCount - 1; i >= index; i--)
						{
							this.owner.subItems[i + size] = this.owner.subItems[i];
						}
					}
					return;
				}
				if (this.owner.subItems == null)
				{
					int num = (size > 4) ? size : 4;
					this.owner.subItems = new ListViewItem.ListViewSubItem[num];
					return;
				}
				int num2 = this.owner.subItems.Length * 2;
				while (num2 - this.owner.SubItemCount < size)
				{
					num2 *= 2;
				}
				ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[num2];
				if (index != -1)
				{
					Array.Copy(this.owner.subItems, 0, array, 0, index);
					Array.Copy(this.owner.subItems, index, array, index + size, this.owner.SubItemCount - index);
				}
				else
				{
					Array.Copy(this.owner.subItems, array, this.owner.SubItemCount);
				}
				this.owner.subItems = array;
			}

			// Token: 0x06004660 RID: 18016 RVA: 0x000FFA58 File Offset: 0x000FEA58
			public int IndexOf(ListViewItem.ListViewSubItem subItem)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.owner.subItems[i] == subItem)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06004661 RID: 18017 RVA: 0x000FFA89 File Offset: 0x000FEA89
			int IList.IndexOf(object subItem)
			{
				if (subItem is ListViewItem.ListViewSubItem)
				{
					return this.IndexOf((ListViewItem.ListViewSubItem)subItem);
				}
				return -1;
			}

			// Token: 0x06004662 RID: 18018 RVA: 0x000FFAA4 File Offset: 0x000FEAA4
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

			// Token: 0x06004663 RID: 18019 RVA: 0x000FFB21 File Offset: 0x000FEB21
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x06004664 RID: 18020 RVA: 0x000FFB34 File Offset: 0x000FEB34
			public void Insert(int index, ListViewItem.ListViewSubItem item)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				item.owner = this.owner;
				this.EnsureSubItemSpace(1, index);
				this.owner.subItems[index] = item;
				this.owner.SubItemCount++;
				this.owner.UpdateSubItems(-1);
			}

			// Token: 0x06004665 RID: 18021 RVA: 0x000FFB9A File Offset: 0x000FEB9A
			void IList.Insert(int index, object item)
			{
				if (item is ListViewItem.ListViewSubItem)
				{
					this.Insert(index, (ListViewItem.ListViewSubItem)item);
					return;
				}
				throw new ArgumentException(SR.GetString("ListViewBadListViewSubItem"), "item");
			}

			// Token: 0x06004666 RID: 18022 RVA: 0x000FFBC8 File Offset: 0x000FEBC8
			public void Remove(ListViewItem.ListViewSubItem item)
			{
				int num = this.IndexOf(item);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06004667 RID: 18023 RVA: 0x000FFBE8 File Offset: 0x000FEBE8
			void IList.Remove(object item)
			{
				if (item is ListViewItem.ListViewSubItem)
				{
					this.Remove((ListViewItem.ListViewSubItem)item);
				}
			}

			// Token: 0x06004668 RID: 18024 RVA: 0x000FFC00 File Offset: 0x000FEC00
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				for (int i = index + 1; i < this.owner.SubItemCount; i++)
				{
					this.owner.subItems[i - 1] = this.owner.subItems[i];
				}
				int subItemCount = this.owner.SubItemCount;
				this.owner.SubItemCount--;
				this.owner.subItems[this.owner.SubItemCount] = null;
				this.owner.UpdateSubItems(-1, subItemCount);
			}

			// Token: 0x06004669 RID: 18025 RVA: 0x000FFCA0 File Offset: 0x000FECA0
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x0600466A RID: 18026 RVA: 0x000FFCC5 File Offset: 0x000FECC5
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.owner.subItems, 0, dest, index, this.Count);
				}
			}

			// Token: 0x0600466B RID: 18027 RVA: 0x000FFCE9 File Offset: 0x000FECE9
			public IEnumerator GetEnumerator()
			{
				if (this.owner.subItems != null)
				{
					return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.subItems, this.owner.SubItemCount);
				}
				return new ListViewItem.ListViewSubItem[0].GetEnumerator();
			}

			// Token: 0x04002196 RID: 8598
			private ListViewItem owner;

			// Token: 0x04002197 RID: 8599
			private int lastAccessedIndex = -1;
		}
	}
}
