using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000700 RID: 1792
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.TreeViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTreeView")]
	[DefaultEvent("AfterSelect")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Nodes")]
	public class TreeView : Control
	{
		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x06005F63 RID: 24419 RVA: 0x0015AFA0 File Offset: 0x00159FA0
		internal ImageList.Indexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ImageList.Indexer();
				}
				this.imageIndexer.ImageList = this.ImageList;
				return this.imageIndexer;
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x06005F64 RID: 24420 RVA: 0x0015AFCC File Offset: 0x00159FCC
		internal ImageList.Indexer SelectedImageIndexer
		{
			get
			{
				if (this.selectedImageIndexer == null)
				{
					this.selectedImageIndexer = new ImageList.Indexer();
				}
				this.selectedImageIndexer.ImageList = this.ImageList;
				return this.selectedImageIndexer;
			}
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x0015AFF8 File Offset: 0x00159FF8
		public TreeView()
		{
			this.treeViewState = new BitVector32(117);
			this.root = new TreeNode(this);
			this.SelectedImageIndexer.Index = 0;
			this.ImageIndexer.Index = 0;
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x06005F66 RID: 24422 RVA: 0x0015B092 File Offset: 0x0015A092
		// (set) Token: 0x06005F67 RID: 24423 RVA: 0x0015B0A8 File Offset: 0x0015A0A8
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(4381, 0, ColorTranslator.ToWin32(this.BackColor));
					base.SendMessage(4359, this.Indent, 0);
				}
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06005F68 RID: 24424 RVA: 0x0015B0E4 File Offset: 0x0015A0E4
		// (set) Token: 0x06005F69 RID: 24425 RVA: 0x0015B0EC File Offset: 0x0015A0EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1400039D RID: 925
		// (add) Token: 0x06005F6A RID: 24426 RVA: 0x0015B0F5 File Offset: 0x0015A0F5
		// (remove) Token: 0x06005F6B RID: 24427 RVA: 0x0015B0FE File Offset: 0x0015A0FE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06005F6C RID: 24428 RVA: 0x0015B107 File Offset: 0x0015A107
		// (set) Token: 0x06005F6D RID: 24429 RVA: 0x0015B10F File Offset: 0x0015A10F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1400039E RID: 926
		// (add) Token: 0x06005F6E RID: 24430 RVA: 0x0015B118 File Offset: 0x0015A118
		// (remove) Token: 0x06005F6F RID: 24431 RVA: 0x0015B121 File Offset: 0x0015A121
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

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06005F70 RID: 24432 RVA: 0x0015B12A File Offset: 0x0015A12A
		// (set) Token: 0x06005F71 RID: 24433 RVA: 0x0015B132 File Offset: 0x0015A132
		[SRDescription("borderStyleDescr")]
		[DispId(-504)]
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06005F72 RID: 24434 RVA: 0x0015B170 File Offset: 0x0015A170
		// (set) Token: 0x06005F73 RID: 24435 RVA: 0x0015B180 File Offset: 0x0015A180
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("TreeViewCheckBoxesDescr")]
		public bool CheckBoxes
		{
			get
			{
				return this.treeViewState[8];
			}
			set
			{
				if (this.CheckBoxes != value)
				{
					this.treeViewState[8] = value;
					if (base.IsHandleCreated)
					{
						if (this.CheckBoxes)
						{
							base.UpdateStyles();
							return;
						}
						this.UpdateCheckedState(this.root, false);
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06005F74 RID: 24436 RVA: 0x0015B1D0 File Offset: 0x0015A1D0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysTreeView32";
				if (base.IsHandleCreated)
				{
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16));
					createParams.Style |= (num & 3145728);
				}
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				if (!this.Scrollable)
				{
					createParams.Style |= 8192;
				}
				if (!this.HideSelection)
				{
					createParams.Style |= 32;
				}
				if (this.LabelEdit)
				{
					createParams.Style |= 8;
				}
				if (this.ShowLines)
				{
					createParams.Style |= 2;
				}
				if (this.ShowPlusMinus)
				{
					createParams.Style |= 1;
				}
				if (this.ShowRootLines)
				{
					createParams.Style |= 4;
				}
				if (this.HotTracking)
				{
					createParams.Style |= 512;
				}
				if (this.FullRowSelect)
				{
					createParams.Style |= 4096;
				}
				if (this.setOddHeight)
				{
					createParams.Style |= 16384;
				}
				if (this.ShowNodeToolTips && base.IsHandleCreated && !base.DesignMode)
				{
					createParams.Style |= 2048;
				}
				if (this.CheckBoxes && base.IsHandleCreated)
				{
					createParams.Style |= 256;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					if (this.RightToLeftLayout)
					{
						createParams.ExStyle |= 4194304;
						createParams.ExStyle &= -28673;
					}
					else
					{
						createParams.Style |= 64;
					}
				}
				return createParams;
			}
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06005F75 RID: 24437 RVA: 0x0015B3CE File Offset: 0x0015A3CE
		protected override Size DefaultSize
		{
			get
			{
				return new Size(121, 97);
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x06005F76 RID: 24438 RVA: 0x0015B3D9 File Offset: 0x0015A3D9
		// (set) Token: 0x06005F77 RID: 24439 RVA: 0x0015B3E1 File Offset: 0x0015A3E1
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

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x06005F78 RID: 24440 RVA: 0x0015B3EA File Offset: 0x0015A3EA
		// (set) Token: 0x06005F79 RID: 24441 RVA: 0x0015B400 File Offset: 0x0015A400
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(4382, 0, ColorTranslator.ToWin32(this.ForeColor));
				}
			}
		}

		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x06005F7A RID: 24442 RVA: 0x0015B429 File Offset: 0x0015A429
		// (set) Token: 0x06005F7B RID: 24443 RVA: 0x0015B43B File Offset: 0x0015A43B
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewFullRowSelectDescr")]
		public bool FullRowSelect
		{
			get
			{
				return this.treeViewState[512];
			}
			set
			{
				if (this.FullRowSelect != value)
				{
					this.treeViewState[512] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x06005F7C RID: 24444 RVA: 0x0015B465 File Offset: 0x0015A465
		// (set) Token: 0x06005F7D RID: 24445 RVA: 0x0015B473 File Offset: 0x0015A473
		[SRDescription("TreeViewHideSelectionDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool HideSelection
		{
			get
			{
				return this.treeViewState[1];
			}
			set
			{
				if (this.HideSelection != value)
				{
					this.treeViewState[1] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06005F7E RID: 24446 RVA: 0x0015B499 File Offset: 0x0015A499
		// (set) Token: 0x06005F7F RID: 24447 RVA: 0x0015B4AB File Offset: 0x0015A4AB
		[DefaultValue(false)]
		[SRDescription("TreeViewHotTrackingDescr")]
		[SRCategory("CatBehavior")]
		public bool HotTracking
		{
			get
			{
				return this.treeViewState[256];
			}
			set
			{
				if (this.HotTracking != value)
				{
					this.treeViewState[256] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06005F80 RID: 24448 RVA: 0x0015B4D8 File Offset: 0x0015A4D8
		// (set) Token: 0x06005F81 RID: 24449 RVA: 0x0015B530 File Offset: 0x0015A530
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[DefaultValue(-1)]
		[SRDescription("TreeViewImageIndexDescr")]
		[RelatedImageList("ImageList")]
		[Localizable(true)]
		public int ImageIndex
		{
			get
			{
				if (this.imageList == null)
				{
					return -1;
				}
				if (this.ImageIndexer.Index >= this.imageList.Images.Count)
				{
					return Math.Max(0, this.imageList.Images.Count - 1);
				}
				return this.ImageIndexer.Index;
			}
			set
			{
				if (value == -1)
				{
					value = 0;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.ImageIndexer.Index != value)
				{
					this.ImageIndexer.Index = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06005F82 RID: 24450 RVA: 0x0015B5B5 File Offset: 0x0015A5B5
		// (set) Token: 0x06005F83 RID: 24451 RVA: 0x0015B5C4 File Offset: 0x0015A5C4
		[Localizable(true)]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TreeViewImageKeyDescr")]
		[RelatedImageList("ImageList")]
		[SRCategory("CatBehavior")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				if (this.ImageIndexer.Key != value)
				{
					this.ImageIndexer.Key = value;
					if (string.IsNullOrEmpty(value) || value.Equals(SR.GetString("toStringNone")))
					{
						this.ImageIndex = ((this.ImageList != null) ? 0 : -1);
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x06005F84 RID: 24452 RVA: 0x0015B62A File Offset: 0x0015A62A
		// (set) Token: 0x06005F85 RID: 24453 RVA: 0x0015B634 File Offset: 0x0015A634
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewImageListDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (value != this.imageList)
				{
					this.DetachImageListHandlers();
					this.imageList = value;
					this.AttachImageListHandlers();
					if (base.IsHandleCreated)
					{
						base.SendMessage(4361, 0, (value == null) ? IntPtr.Zero : value.Handle);
						if (this.StateImageList != null && this.StateImageList.Images.Count > 0)
						{
							base.SendMessage(4361, 2, this.internalStateImageList.Handle);
						}
					}
					this.UpdateCheckedState(this.root, true);
				}
			}
		}

		// Token: 0x06005F86 RID: 24454 RVA: 0x0015B6C4 File Offset: 0x0015A6C4
		private void AttachImageListHandlers()
		{
			if (this.imageList != null)
			{
				this.imageList.RecreateHandle += this.ImageListRecreateHandle;
				this.imageList.Disposed += this.DetachImageList;
				this.imageList.ChangeHandle += this.ImageListChangedHandle;
			}
		}

		// Token: 0x06005F87 RID: 24455 RVA: 0x0015B720 File Offset: 0x0015A720
		private void DetachImageListHandlers()
		{
			if (this.imageList != null)
			{
				this.imageList.RecreateHandle -= this.ImageListRecreateHandle;
				this.imageList.Disposed -= this.DetachImageList;
				this.imageList.ChangeHandle -= this.ImageListChangedHandle;
			}
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x0015B77C File Offset: 0x0015A77C
		private void AttachStateImageListHandlers()
		{
			if (this.stateImageList != null)
			{
				this.stateImageList.RecreateHandle += this.StateImageListRecreateHandle;
				this.stateImageList.Disposed += this.DetachStateImageList;
				this.stateImageList.ChangeHandle += this.StateImageListChangedHandle;
			}
		}

		// Token: 0x06005F89 RID: 24457 RVA: 0x0015B7D8 File Offset: 0x0015A7D8
		private void DetachStateImageListHandlers()
		{
			if (this.stateImageList != null)
			{
				this.stateImageList.RecreateHandle -= this.StateImageListRecreateHandle;
				this.stateImageList.Disposed -= this.DetachStateImageList;
				this.stateImageList.ChangeHandle -= this.StateImageListChangedHandle;
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x06005F8A RID: 24458 RVA: 0x0015B832 File Offset: 0x0015A832
		// (set) Token: 0x06005F8B RID: 24459 RVA: 0x0015B83C File Offset: 0x0015A83C
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewStateImageListDescr")]
		[DefaultValue(null)]
		public ImageList StateImageList
		{
			get
			{
				return this.stateImageList;
			}
			set
			{
				if (value != this.stateImageList)
				{
					this.DetachStateImageListHandlers();
					this.stateImageList = value;
					if (this.stateImageList != null && this.stateImageList.Images.Count > 0)
					{
						Image[] array = new Image[this.stateImageList.Images.Count + 1];
						array[0] = this.stateImageList.Images[0];
						for (int i = 1; i <= this.stateImageList.Images.Count; i++)
						{
							array[i] = this.stateImageList.Images[i - 1];
						}
						this.internalStateImageList = new ImageList();
						this.internalStateImageList.Images.AddRange(array);
					}
					this.AttachStateImageListHandlers();
					if (base.IsHandleCreated)
					{
						if (this.stateImageList != null && this.stateImageList.Images.Count > 0 && this.internalStateImageList != null)
						{
							base.SendMessage(4361, 2, this.internalStateImageList.Handle);
						}
						this.UpdateCheckedState(this.root, true);
						if ((value == null || this.stateImageList.Images.Count == 0) && this.CheckBoxes)
						{
							base.RecreateHandle();
							return;
						}
						this.RefreshNodes();
					}
				}
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x06005F8C RID: 24460 RVA: 0x0015B979 File Offset: 0x0015A979
		// (set) Token: 0x06005F8D RID: 24461 RVA: 0x0015B9A8 File Offset: 0x0015A9A8
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewIndentDescr")]
		[Localizable(true)]
		public int Indent
		{
			get
			{
				if (this.indent != -1)
				{
					return this.indent;
				}
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(4358, 0, 0);
				}
				return 19;
			}
			set
			{
				if (this.indent != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Indent", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Indent",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value > TreeView.MaxIndent)
					{
						throw new ArgumentOutOfRangeException("Indent", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"Indent",
							value.ToString(CultureInfo.CurrentCulture),
							TreeView.MaxIndent.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.indent = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(4359, value, 0);
						this.indent = (int)base.SendMessage(4358, 0, 0);
					}
				}
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06005F8E RID: 24462 RVA: 0x0015BA90 File Offset: 0x0015AA90
		// (set) Token: 0x06005F8F RID: 24463 RVA: 0x0015BAF4 File Offset: 0x0015AAF4
		[SRCategory("CatAppearance")]
		[SRDescription("TreeViewItemHeightDescr")]
		public int ItemHeight
		{
			get
			{
				if (this.itemHeight != -1)
				{
					return this.itemHeight;
				}
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(4380, 0, 0);
				}
				if (this.CheckBoxes && this.DrawMode == TreeViewDrawMode.OwnerDrawAll)
				{
					return Math.Max(16, base.FontHeight + 3);
				}
				return base.FontHeight + 3;
			}
			set
			{
				if (this.itemHeight != value)
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"ItemHeight",
							value.ToString(CultureInfo.CurrentCulture),
							1.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value >= 32767)
					{
						throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidHighBoundArgument", new object[]
						{
							"ItemHeight",
							value.ToString(CultureInfo.CurrentCulture),
							short.MaxValue.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.itemHeight = value;
					if (base.IsHandleCreated)
					{
						if (this.itemHeight % 2 != 0)
						{
							this.setOddHeight = true;
							try
							{
								base.RecreateHandle();
							}
							finally
							{
								this.setOddHeight = false;
							}
						}
						base.SendMessage(4379, value, 0);
						this.itemHeight = (int)base.SendMessage(4380, 0, 0);
					}
				}
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06005F90 RID: 24464 RVA: 0x0015BC10 File Offset: 0x0015AC10
		// (set) Token: 0x06005F91 RID: 24465 RVA: 0x0015BC1E File Offset: 0x0015AC1E
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewLabelEditDescr")]
		[DefaultValue(false)]
		public bool LabelEdit
		{
			get
			{
				return this.treeViewState[2];
			}
			set
			{
				if (this.LabelEdit != value)
				{
					this.treeViewState[2] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06005F92 RID: 24466 RVA: 0x0015BC44 File Offset: 0x0015AC44
		// (set) Token: 0x06005F93 RID: 24467 RVA: 0x0015BC7A File Offset: 0x0015AC7A
		[SRDescription("TreeViewLineColorDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(typeof(Color), "Black")]
		public Color LineColor
		{
			get
			{
				if (base.IsHandleCreated)
				{
					int win32Color = (int)((long)base.SendMessage(4393, 0, 0));
					return ColorTranslator.FromWin32(win32Color);
				}
				return this.lineColor;
			}
			set
			{
				if (this.lineColor != value)
				{
					this.lineColor = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(4392, 0, ColorTranslator.ToWin32(this.lineColor));
					}
				}
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06005F94 RID: 24468 RVA: 0x0015BCB1 File Offset: 0x0015ACB1
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewNodesDescr")]
		[MergableProperty(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TreeNodeCollection Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new TreeNodeCollection(this.root);
				}
				return this.nodes;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06005F95 RID: 24469 RVA: 0x0015BCD2 File Offset: 0x0015ACD2
		// (set) Token: 0x06005F96 RID: 24470 RVA: 0x0015BCDC File Offset: 0x0015ACDC
		[SRCategory("CatBehavior")]
		[DefaultValue(TreeViewDrawMode.Normal)]
		[SRDescription("TreeViewDrawModeDescr")]
		public TreeViewDrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TreeViewDrawMode));
				}
				if (this.drawMode != value)
				{
					this.drawMode = value;
					base.Invalidate();
					if (this.DrawMode == TreeViewDrawMode.OwnerDrawAll)
					{
						base.SetStyle(ControlStyles.ResizeRedraw, true);
					}
				}
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06005F97 RID: 24471 RVA: 0x0015BD37 File Offset: 0x0015AD37
		// (set) Token: 0x06005F98 RID: 24472 RVA: 0x0015BD3F File Offset: 0x0015AD3F
		[DefaultValue("\\")]
		[SRDescription("TreeViewPathSeparatorDescr")]
		[SRCategory("CatBehavior")]
		public string PathSeparator
		{
			get
			{
				return this.pathSeparator;
			}
			set
			{
				this.pathSeparator = value;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x06005F99 RID: 24473 RVA: 0x0015BD48 File Offset: 0x0015AD48
		// (set) Token: 0x06005F9A RID: 24474 RVA: 0x0015BD50 File Offset: 0x0015AD50
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x1400039F RID: 927
		// (add) Token: 0x06005F9B RID: 24475 RVA: 0x0015BD59 File Offset: 0x0015AD59
		// (remove) Token: 0x06005F9C RID: 24476 RVA: 0x0015BD62 File Offset: 0x0015AD62
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06005F9D RID: 24477 RVA: 0x0015BD6B File Offset: 0x0015AD6B
		// (set) Token: 0x06005F9E RID: 24478 RVA: 0x0015BD74 File Offset: 0x0015AD74
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[SRCategory("CatAppearance")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06005F9F RID: 24479 RVA: 0x0015BDC8 File Offset: 0x0015ADC8
		// (set) Token: 0x06005FA0 RID: 24480 RVA: 0x0015BDD6 File Offset: 0x0015ADD6
		[SRDescription("TreeViewScrollableDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool Scrollable
		{
			get
			{
				return this.treeViewState[4];
			}
			set
			{
				if (this.Scrollable != value)
				{
					this.treeViewState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06005FA1 RID: 24481 RVA: 0x0015BDF4 File Offset: 0x0015ADF4
		// (set) Token: 0x06005FA2 RID: 24482 RVA: 0x0015BE4C File Offset: 0x0015AE4C
		[Localizable(true)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("TreeViewSelectedImageIndexDescr")]
		[RelatedImageList("ImageList")]
		[SRCategory("CatBehavior")]
		public int SelectedImageIndex
		{
			get
			{
				if (this.imageList == null)
				{
					return -1;
				}
				if (this.SelectedImageIndexer.Index >= this.imageList.Images.Count)
				{
					return Math.Max(0, this.imageList.Images.Count - 1);
				}
				return this.SelectedImageIndexer.Index;
			}
			set
			{
				if (value == -1)
				{
					value = 0;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectedImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SelectedImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.SelectedImageIndexer.Index != value)
				{
					this.SelectedImageIndexer.Index = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06005FA3 RID: 24483 RVA: 0x0015BED1 File Offset: 0x0015AED1
		// (set) Token: 0x06005FA4 RID: 24484 RVA: 0x0015BEE0 File Offset: 0x0015AEE0
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewSelectedImageKeyDescr")]
		[Localizable(true)]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("ImageList")]
		public string SelectedImageKey
		{
			get
			{
				return this.SelectedImageIndexer.Key;
			}
			set
			{
				if (this.SelectedImageIndexer.Key != value)
				{
					this.SelectedImageIndexer.Key = value;
					if (string.IsNullOrEmpty(value) || value.Equals(SR.GetString("toStringNone")))
					{
						this.SelectedImageIndex = ((this.ImageList != null) ? 0 : -1);
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x0015BF48 File Offset: 0x0015AF48
		// (set) Token: 0x06005FA6 RID: 24486 RVA: 0x0015BFA4 File Offset: 0x0015AFA4
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("TreeViewSelectedNodeDescr")]
		public TreeNode SelectedNode
		{
			get
			{
				if (base.IsHandleCreated)
				{
					IntPtr intPtr = base.SendMessage(4362, 9, 0);
					if (intPtr == IntPtr.Zero)
					{
						return null;
					}
					return this.NodeFromHandle(intPtr);
				}
				else
				{
					if (this.selectedNode != null && this.selectedNode.TreeView == this)
					{
						return this.selectedNode;
					}
					return null;
				}
			}
			set
			{
				if (base.IsHandleCreated && (value == null || value.TreeView == this))
				{
					IntPtr lparam = (value == null) ? IntPtr.Zero : value.Handle;
					base.SendMessage(4363, 9, lparam);
					this.selectedNode = null;
					return;
				}
				this.selectedNode = value;
			}
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x0015BFF4 File Offset: 0x0015AFF4
		// (set) Token: 0x06005FA8 RID: 24488 RVA: 0x0015C003 File Offset: 0x0015B003
		[SRDescription("TreeViewShowLinesDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ShowLines
		{
			get
			{
				return this.treeViewState[16];
			}
			set
			{
				if (this.ShowLines != value)
				{
					this.treeViewState[16] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x06005FA9 RID: 24489 RVA: 0x0015C02A File Offset: 0x0015B02A
		// (set) Token: 0x06005FAA RID: 24490 RVA: 0x0015C03C File Offset: 0x0015B03C
		[SRDescription("TreeViewShowShowNodeToolTipsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ShowNodeToolTips
		{
			get
			{
				return this.treeViewState[1024];
			}
			set
			{
				if (this.ShowNodeToolTips != value)
				{
					this.treeViewState[1024] = value;
					if (this.ShowNodeToolTips)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x06005FAB RID: 24491 RVA: 0x0015C066 File Offset: 0x0015B066
		// (set) Token: 0x06005FAC RID: 24492 RVA: 0x0015C075 File Offset: 0x0015B075
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewShowPlusMinusDescr")]
		public bool ShowPlusMinus
		{
			get
			{
				return this.treeViewState[32];
			}
			set
			{
				if (this.ShowPlusMinus != value)
				{
					this.treeViewState[32] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06005FAD RID: 24493 RVA: 0x0015C09C File Offset: 0x0015B09C
		// (set) Token: 0x06005FAE RID: 24494 RVA: 0x0015C0AB File Offset: 0x0015B0AB
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TreeViewShowRootLinesDescr")]
		public bool ShowRootLines
		{
			get
			{
				return this.treeViewState[64];
			}
			set
			{
				if (this.ShowRootLines != value)
				{
					this.treeViewState[64] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x06005FAF RID: 24495 RVA: 0x0015C0D2 File Offset: 0x0015B0D2
		// (set) Token: 0x06005FB0 RID: 24496 RVA: 0x0015C0E4 File Offset: 0x0015B0E4
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(false)]
		[SRDescription("TreeViewSortedDescr")]
		[Browsable(false)]
		public bool Sorted
		{
			get
			{
				return this.treeViewState[128];
			}
			set
			{
				if (this.Sorted != value)
				{
					this.treeViewState[128] = value;
					if (this.Sorted && this.TreeViewNodeSorter == null && this.Nodes.Count >= 1)
					{
						this.RefreshNodes();
					}
				}
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x0015C124 File Offset: 0x0015B124
		// (set) Token: 0x06005FB2 RID: 24498 RVA: 0x0015C12C File Offset: 0x0015B12C
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("TreeViewNodeSorterDescr")]
		public IComparer TreeViewNodeSorter
		{
			get
			{
				return this.treeViewNodeSorter;
			}
			set
			{
				if (this.treeViewNodeSorter != value)
				{
					this.treeViewNodeSorter = value;
					if (value != null)
					{
						this.Sort();
					}
				}
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x0015C147 File Offset: 0x0015B147
		// (set) Token: 0x06005FB4 RID: 24500 RVA: 0x0015C14F File Offset: 0x0015B14F
		[Bindable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140003A0 RID: 928
		// (add) Token: 0x06005FB5 RID: 24501 RVA: 0x0015C158 File Offset: 0x0015B158
		// (remove) Token: 0x06005FB6 RID: 24502 RVA: 0x0015C161 File Offset: 0x0015B161
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06005FB7 RID: 24503 RVA: 0x0015C16C File Offset: 0x0015B16C
		// (set) Token: 0x06005FB8 RID: 24504 RVA: 0x0015C1AC File Offset: 0x0015B1AC
		[SRCategory("CatAppearance")]
		[SRDescription("TreeViewTopNodeDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeNode TopNode
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					return this.topNode;
				}
				IntPtr intPtr = base.SendMessage(4362, 5, 0);
				if (!(intPtr == IntPtr.Zero))
				{
					return this.NodeFromHandle(intPtr);
				}
				return null;
			}
			set
			{
				if (base.IsHandleCreated && (value == null || value.TreeView == this))
				{
					IntPtr lparam = (value == null) ? IntPtr.Zero : value.Handle;
					base.SendMessage(4363, 5, lparam);
					this.topNode = null;
					return;
				}
				this.topNode = value;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06005FB9 RID: 24505 RVA: 0x0015C1FB File Offset: 0x0015B1FB
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TreeViewVisibleCountDescr")]
		public int VisibleCount
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(4368, 0, 0);
				}
				return 0;
			}
		}

		// Token: 0x140003A1 RID: 929
		// (add) Token: 0x06005FBA RID: 24506 RVA: 0x0015C219 File Offset: 0x0015B219
		// (remove) Token: 0x06005FBB RID: 24507 RVA: 0x0015C232 File Offset: 0x0015B232
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeEditDescr")]
		public event NodeLabelEditEventHandler BeforeLabelEdit
		{
			add
			{
				this.onBeforeLabelEdit = (NodeLabelEditEventHandler)Delegate.Combine(this.onBeforeLabelEdit, value);
			}
			remove
			{
				this.onBeforeLabelEdit = (NodeLabelEditEventHandler)Delegate.Remove(this.onBeforeLabelEdit, value);
			}
		}

		// Token: 0x140003A2 RID: 930
		// (add) Token: 0x06005FBC RID: 24508 RVA: 0x0015C24B File Offset: 0x0015B24B
		// (remove) Token: 0x06005FBD RID: 24509 RVA: 0x0015C264 File Offset: 0x0015B264
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterEditDescr")]
		public event NodeLabelEditEventHandler AfterLabelEdit
		{
			add
			{
				this.onAfterLabelEdit = (NodeLabelEditEventHandler)Delegate.Combine(this.onAfterLabelEdit, value);
			}
			remove
			{
				this.onAfterLabelEdit = (NodeLabelEditEventHandler)Delegate.Remove(this.onAfterLabelEdit, value);
			}
		}

		// Token: 0x140003A3 RID: 931
		// (add) Token: 0x06005FBE RID: 24510 RVA: 0x0015C27D File Offset: 0x0015B27D
		// (remove) Token: 0x06005FBF RID: 24511 RVA: 0x0015C296 File Offset: 0x0015B296
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeCheckDescr")]
		public event TreeViewCancelEventHandler BeforeCheck
		{
			add
			{
				this.onBeforeCheck = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeCheck, value);
			}
			remove
			{
				this.onBeforeCheck = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeCheck, value);
			}
		}

		// Token: 0x140003A4 RID: 932
		// (add) Token: 0x06005FC0 RID: 24512 RVA: 0x0015C2AF File Offset: 0x0015B2AF
		// (remove) Token: 0x06005FC1 RID: 24513 RVA: 0x0015C2C8 File Offset: 0x0015B2C8
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterCheckDescr")]
		public event TreeViewEventHandler AfterCheck
		{
			add
			{
				this.onAfterCheck = (TreeViewEventHandler)Delegate.Combine(this.onAfterCheck, value);
			}
			remove
			{
				this.onAfterCheck = (TreeViewEventHandler)Delegate.Remove(this.onAfterCheck, value);
			}
		}

		// Token: 0x140003A5 RID: 933
		// (add) Token: 0x06005FC2 RID: 24514 RVA: 0x0015C2E1 File Offset: 0x0015B2E1
		// (remove) Token: 0x06005FC3 RID: 24515 RVA: 0x0015C2FA File Offset: 0x0015B2FA
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeCollapseDescr")]
		public event TreeViewCancelEventHandler BeforeCollapse
		{
			add
			{
				this.onBeforeCollapse = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeCollapse, value);
			}
			remove
			{
				this.onBeforeCollapse = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeCollapse, value);
			}
		}

		// Token: 0x140003A6 RID: 934
		// (add) Token: 0x06005FC4 RID: 24516 RVA: 0x0015C313 File Offset: 0x0015B313
		// (remove) Token: 0x06005FC5 RID: 24517 RVA: 0x0015C32C File Offset: 0x0015B32C
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterCollapseDescr")]
		public event TreeViewEventHandler AfterCollapse
		{
			add
			{
				this.onAfterCollapse = (TreeViewEventHandler)Delegate.Combine(this.onAfterCollapse, value);
			}
			remove
			{
				this.onAfterCollapse = (TreeViewEventHandler)Delegate.Remove(this.onAfterCollapse, value);
			}
		}

		// Token: 0x140003A7 RID: 935
		// (add) Token: 0x06005FC6 RID: 24518 RVA: 0x0015C345 File Offset: 0x0015B345
		// (remove) Token: 0x06005FC7 RID: 24519 RVA: 0x0015C35E File Offset: 0x0015B35E
		[SRDescription("TreeViewBeforeExpandDescr")]
		[SRCategory("CatBehavior")]
		public event TreeViewCancelEventHandler BeforeExpand
		{
			add
			{
				this.onBeforeExpand = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeExpand, value);
			}
			remove
			{
				this.onBeforeExpand = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeExpand, value);
			}
		}

		// Token: 0x140003A8 RID: 936
		// (add) Token: 0x06005FC8 RID: 24520 RVA: 0x0015C377 File Offset: 0x0015B377
		// (remove) Token: 0x06005FC9 RID: 24521 RVA: 0x0015C390 File Offset: 0x0015B390
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterExpandDescr")]
		public event TreeViewEventHandler AfterExpand
		{
			add
			{
				this.onAfterExpand = (TreeViewEventHandler)Delegate.Combine(this.onAfterExpand, value);
			}
			remove
			{
				this.onAfterExpand = (TreeViewEventHandler)Delegate.Remove(this.onAfterExpand, value);
			}
		}

		// Token: 0x140003A9 RID: 937
		// (add) Token: 0x06005FCA RID: 24522 RVA: 0x0015C3A9 File Offset: 0x0015B3A9
		// (remove) Token: 0x06005FCB RID: 24523 RVA: 0x0015C3C2 File Offset: 0x0015B3C2
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewDrawNodeEventDescr")]
		public event DrawTreeNodeEventHandler DrawNode
		{
			add
			{
				this.onDrawNode = (DrawTreeNodeEventHandler)Delegate.Combine(this.onDrawNode, value);
			}
			remove
			{
				this.onDrawNode = (DrawTreeNodeEventHandler)Delegate.Remove(this.onDrawNode, value);
			}
		}

		// Token: 0x140003AA RID: 938
		// (add) Token: 0x06005FCC RID: 24524 RVA: 0x0015C3DB File Offset: 0x0015B3DB
		// (remove) Token: 0x06005FCD RID: 24525 RVA: 0x0015C3F4 File Offset: 0x0015B3F4
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemDragDescr")]
		public event ItemDragEventHandler ItemDrag
		{
			add
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Combine(this.onItemDrag, value);
			}
			remove
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Remove(this.onItemDrag, value);
			}
		}

		// Token: 0x140003AB RID: 939
		// (add) Token: 0x06005FCE RID: 24526 RVA: 0x0015C40D File Offset: 0x0015B40D
		// (remove) Token: 0x06005FCF RID: 24527 RVA: 0x0015C426 File Offset: 0x0015B426
		[SRDescription("TreeViewNodeMouseHoverDescr")]
		[SRCategory("CatAction")]
		public event TreeNodeMouseHoverEventHandler NodeMouseHover
		{
			add
			{
				this.onNodeMouseHover = (TreeNodeMouseHoverEventHandler)Delegate.Combine(this.onNodeMouseHover, value);
			}
			remove
			{
				this.onNodeMouseHover = (TreeNodeMouseHoverEventHandler)Delegate.Remove(this.onNodeMouseHover, value);
			}
		}

		// Token: 0x140003AC RID: 940
		// (add) Token: 0x06005FD0 RID: 24528 RVA: 0x0015C43F File Offset: 0x0015B43F
		// (remove) Token: 0x06005FD1 RID: 24529 RVA: 0x0015C458 File Offset: 0x0015B458
		[SRDescription("TreeViewBeforeSelectDescr")]
		[SRCategory("CatBehavior")]
		public event TreeViewCancelEventHandler BeforeSelect
		{
			add
			{
				this.onBeforeSelect = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeSelect, value);
			}
			remove
			{
				this.onBeforeSelect = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeSelect, value);
			}
		}

		// Token: 0x140003AD RID: 941
		// (add) Token: 0x06005FD2 RID: 24530 RVA: 0x0015C471 File Offset: 0x0015B471
		// (remove) Token: 0x06005FD3 RID: 24531 RVA: 0x0015C48A File Offset: 0x0015B48A
		[SRDescription("TreeViewAfterSelectDescr")]
		[SRCategory("CatBehavior")]
		public event TreeViewEventHandler AfterSelect
		{
			add
			{
				this.onAfterSelect = (TreeViewEventHandler)Delegate.Combine(this.onAfterSelect, value);
			}
			remove
			{
				this.onAfterSelect = (TreeViewEventHandler)Delegate.Remove(this.onAfterSelect, value);
			}
		}

		// Token: 0x140003AE RID: 942
		// (add) Token: 0x06005FD4 RID: 24532 RVA: 0x0015C4A3 File Offset: 0x0015B4A3
		// (remove) Token: 0x06005FD5 RID: 24533 RVA: 0x0015C4AC File Offset: 0x0015B4AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140003AF RID: 943
		// (add) Token: 0x06005FD6 RID: 24534 RVA: 0x0015C4B5 File Offset: 0x0015B4B5
		// (remove) Token: 0x06005FD7 RID: 24535 RVA: 0x0015C4CE File Offset: 0x0015B4CE
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewNodeMouseClickDescr")]
		public event TreeNodeMouseClickEventHandler NodeMouseClick
		{
			add
			{
				this.onNodeMouseClick = (TreeNodeMouseClickEventHandler)Delegate.Combine(this.onNodeMouseClick, value);
			}
			remove
			{
				this.onNodeMouseClick = (TreeNodeMouseClickEventHandler)Delegate.Remove(this.onNodeMouseClick, value);
			}
		}

		// Token: 0x140003B0 RID: 944
		// (add) Token: 0x06005FD8 RID: 24536 RVA: 0x0015C4E7 File Offset: 0x0015B4E7
		// (remove) Token: 0x06005FD9 RID: 24537 RVA: 0x0015C500 File Offset: 0x0015B500
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewNodeMouseDoubleClickDescr")]
		public event TreeNodeMouseClickEventHandler NodeMouseDoubleClick
		{
			add
			{
				this.onNodeMouseDoubleClick = (TreeNodeMouseClickEventHandler)Delegate.Combine(this.onNodeMouseDoubleClick, value);
			}
			remove
			{
				this.onNodeMouseDoubleClick = (TreeNodeMouseClickEventHandler)Delegate.Remove(this.onNodeMouseDoubleClick, value);
			}
		}

		// Token: 0x140003B1 RID: 945
		// (add) Token: 0x06005FDA RID: 24538 RVA: 0x0015C519 File Offset: 0x0015B519
		// (remove) Token: 0x06005FDB RID: 24539 RVA: 0x0015C532 File Offset: 0x0015B532
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Combine(this.onRightToLeftLayoutChanged, value);
			}
			remove
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Remove(this.onRightToLeftLayoutChanged, value);
			}
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x0015C54B File Offset: 0x0015B54B
		public void BeginUpdate()
		{
			base.BeginUpdateInternal();
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x0015C553 File Offset: 0x0015B553
		public void CollapseAll()
		{
			this.root.Collapse();
		}

		// Token: 0x06005FDE RID: 24542 RVA: 0x0015C560 File Offset: 0x0015B560
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 2
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x06005FDF RID: 24543 RVA: 0x0015C5B0 File Offset: 0x0015B5B0
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x0015C5B9 File Offset: 0x0015B5B9
		private void DetachStateImageList(object sender, EventArgs e)
		{
			this.internalStateImageList = null;
			this.StateImageList = null;
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x0015C5CC File Offset: 0x0015B5CC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (object obj in this.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj;
					treeNode.ContextMenu = null;
				}
				lock (this)
				{
					this.DetachImageListHandlers();
					this.imageList = null;
					this.DetachStateImageListHandlers();
					this.stateImageList = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0015C668 File Offset: 0x0015B668
		public void EndUpdate()
		{
			base.EndUpdateInternal();
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0015C671 File Offset: 0x0015B671
		public void ExpandAll()
		{
			this.root.ExpandAll();
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0015C680 File Offset: 0x0015B680
		internal void ForceScrollbarUpdate(bool delayed)
		{
			if (!base.IsUpdating() && base.IsHandleCreated)
			{
				base.SendMessage(11, 0, 0);
				if (delayed)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 11, (IntPtr)1, IntPtr.Zero);
					return;
				}
				base.SendMessage(11, 1, 0);
			}
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0015C6D8 File Offset: 0x0015B6D8
		internal void SetToolTip(ToolTip toolTip, string toolTipText)
		{
			if (toolTip != null)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(toolTip, toolTip.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4376, new HandleRef(toolTip, toolTip.Handle), 0);
				this.controlToolTipText = toolTipText;
			}
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x0015C738 File Offset: 0x0015B738
		public TreeViewHitTestInfo HitTest(Point pt)
		{
			return this.HitTest(pt.X, pt.Y);
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x0015C750 File Offset: 0x0015B750
		public TreeViewHitTestInfo HitTest(int x, int y)
		{
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			tv_HITTESTINFO.pt_x = x;
			tv_HITTESTINFO.pt_y = y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			TreeNode hitNode = (intPtr == IntPtr.Zero) ? null : this.NodeFromHandle(intPtr);
			TreeViewHitTestLocations flags = (TreeViewHitTestLocations)tv_HITTESTINFO.flags;
			return new TreeViewHitTestInfo(hitNode, flags);
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x0015C7B0 File Offset: 0x0015B7B0
		internal bool TreeViewBeforeCheck(TreeNode node, TreeViewAction actionTaken)
		{
			TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(node, false, actionTaken);
			this.OnBeforeCheck(treeViewCancelEventArgs);
			return treeViewCancelEventArgs.Cancel;
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x0015C7D3 File Offset: 0x0015B7D3
		internal void TreeViewAfterCheck(TreeNode node, TreeViewAction actionTaken)
		{
			this.OnAfterCheck(new TreeViewEventArgs(node, actionTaken));
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x0015C7E2 File Offset: 0x0015B7E2
		public int GetNodeCount(bool includeSubTrees)
		{
			return this.root.GetNodeCount(includeSubTrees);
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x0015C7F0 File Offset: 0x0015B7F0
		public TreeNode GetNodeAt(Point pt)
		{
			return this.GetNodeAt(pt.X, pt.Y);
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x0015C808 File Offset: 0x0015B808
		public TreeNode GetNodeAt(int x, int y)
		{
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			tv_HITTESTINFO.pt_x = x;
			tv_HITTESTINFO.pt_y = y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (!(intPtr == IntPtr.Zero))
			{
				return this.NodeFromHandle(intPtr);
			}
			return null;
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x0015C858 File Offset: 0x0015B858
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr lparam = (this.ImageList == null) ? IntPtr.Zero : this.ImageList.Handle;
				base.SendMessage(4361, 0, lparam);
			}
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x0015C898 File Offset: 0x0015B898
		private void UpdateImagesRecursive(TreeNode node)
		{
			node.UpdateImage();
			foreach (object obj in node.Nodes)
			{
				TreeNode node2 = (TreeNode)obj;
				this.UpdateImagesRecursive(node2);
			}
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x0015C8F8 File Offset: 0x0015B8F8
		private void ImageListChangedHandle(object sender, EventArgs e)
		{
			if (sender != null && sender == this.imageList && base.IsHandleCreated)
			{
				this.BeginUpdate();
				foreach (object obj in this.Nodes)
				{
					TreeNode node = (TreeNode)obj;
					this.UpdateImagesRecursive(node);
				}
				this.EndUpdate();
			}
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x0015C974 File Offset: 0x0015B974
		private void StateImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr lparam = IntPtr.Zero;
				if (this.internalStateImageList != null)
				{
					lparam = this.internalStateImageList.Handle;
				}
				base.SendMessage(4361, 2, lparam);
			}
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x0015C9B4 File Offset: 0x0015B9B4
		private void StateImageListChangedHandle(object sender, EventArgs e)
		{
			if (sender != null && sender == this.stateImageList && base.IsHandleCreated)
			{
				if (this.stateImageList != null && this.stateImageList.Images.Count > 0)
				{
					Image[] array = new Image[this.stateImageList.Images.Count + 1];
					array[0] = this.stateImageList.Images[0];
					for (int i = 1; i <= this.stateImageList.Images.Count; i++)
					{
						array[i] = this.stateImageList.Images[i - 1];
					}
					if (this.internalStateImageList != null)
					{
						this.internalStateImageList.Images.Clear();
						this.internalStateImageList.Images.AddRange(array);
					}
					else
					{
						this.internalStateImageList = new ImageList();
						this.internalStateImageList.Images.AddRange(array);
					}
					if (this.internalStateImageList != null)
					{
						base.SendMessage(4361, 2, this.internalStateImageList.Handle);
						return;
					}
				}
				else
				{
					this.UpdateCheckedState(this.root, true);
				}
			}
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x0015CAD4 File Offset: 0x0015BAD4
		protected override bool IsInputKey(Keys keyData)
		{
			if (this.editNode != null && (keyData & Keys.Alt) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys != Keys.Return && keys != Keys.Escape)
				{
					switch (keys)
					{
					case Keys.Prior:
					case Keys.Next:
					case Keys.End:
					case Keys.Home:
						break;
					default:
						goto IL_40;
					}
				}
				return true;
			}
			IL_40:
			return base.IsInputKey(keyData);
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x0015CB28 File Offset: 0x0015BB28
		internal TreeNode NodeFromHandle(IntPtr handle)
		{
			return (TreeNode)this.nodeTable[handle];
		}

		// Token: 0x06005FF4 RID: 24564 RVA: 0x0015CB4D File Offset: 0x0015BB4D
		protected virtual void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			if (this.onDrawNode != null)
			{
				this.onDrawNode(this, e);
			}
		}

		// Token: 0x06005FF5 RID: 24565 RVA: 0x0015CB64 File Offset: 0x0015BB64
		protected override void OnHandleCreated(EventArgs e)
		{
			TreeNode treeNode = this.selectedNode;
			this.selectedNode = null;
			base.OnHandleCreated(e);
			int num = (int)base.SendMessage(8200, 0, 0);
			if (num < 5)
			{
				base.SendMessage(8199, 5, 0);
			}
			if (this.CheckBoxes)
			{
				int num2 = (int)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16);
				num2 |= 256;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -16, new HandleRef(null, (IntPtr)num2));
			}
			if (this.ShowNodeToolTips && !base.DesignMode)
			{
				int num3 = (int)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16);
				num3 |= 2048;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -16, new HandleRef(null, (IntPtr)num3));
			}
			Color color = this.BackColor;
			if (color != SystemColors.Window)
			{
				base.SendMessage(4381, 0, ColorTranslator.ToWin32(color));
			}
			color = this.ForeColor;
			if (color != SystemColors.WindowText)
			{
				base.SendMessage(4382, 0, ColorTranslator.ToWin32(color));
			}
			if (this.lineColor != Color.Empty)
			{
				base.SendMessage(4392, 0, ColorTranslator.ToWin32(this.lineColor));
			}
			if (this.imageList != null)
			{
				base.SendMessage(4361, 0, this.imageList.Handle);
			}
			if (this.stateImageList != null && this.stateImageList.Images.Count > 0)
			{
				Image[] array = new Image[this.stateImageList.Images.Count + 1];
				array[0] = this.stateImageList.Images[0];
				for (int i = 1; i <= this.stateImageList.Images.Count; i++)
				{
					array[i] = this.stateImageList.Images[i - 1];
				}
				this.internalStateImageList = new ImageList();
				this.internalStateImageList.Images.AddRange(array);
				base.SendMessage(4361, 2, this.internalStateImageList.Handle);
			}
			if (this.indent != -1)
			{
				base.SendMessage(4359, this.indent, 0);
			}
			if (this.itemHeight != -1)
			{
				base.SendMessage(4379, this.ItemHeight, 0);
			}
			try
			{
				this.treeViewState[32768] = true;
				int width = base.Width;
				int flags = 22;
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef, base.Left, base.Top, int.MaxValue, base.Height, flags);
				this.root.Realize(false);
				if (width != 0)
				{
					SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef, base.Left, base.Top, width, base.Height, flags);
				}
			}
			finally
			{
				this.treeViewState[32768] = false;
			}
			this.SelectedNode = treeNode;
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x0015CE90 File Offset: 0x0015BE90
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.selectedNode = this.SelectedNode;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x0015CEA5 File Offset: 0x0015BEA5
		protected override void OnMouseLeave(EventArgs e)
		{
			this.hoveredAlready = false;
			base.OnMouseLeave(e);
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x0015CEB8 File Offset: 0x0015BEB8
		protected override void OnMouseHover(EventArgs e)
		{
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (intPtr != IntPtr.Zero && (tv_HITTESTINFO.flags & 70) != 0)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (treeNode != this.prevHoveredNode && treeNode != null)
				{
					this.OnNodeMouseHover(new TreeNodeMouseHoverEventArgs(treeNode));
					this.prevHoveredNode = treeNode;
				}
			}
			if (!this.hoveredAlready)
			{
				base.OnMouseHover(e);
				this.hoveredAlready = true;
			}
			base.ResetMouseEventArgs();
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x0015CF67 File Offset: 0x0015BF67
		protected virtual void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
		{
			if (this.onBeforeLabelEdit != null)
			{
				this.onBeforeLabelEdit(this, e);
			}
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x0015CF7E File Offset: 0x0015BF7E
		protected virtual void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			if (this.onAfterLabelEdit != null)
			{
				this.onAfterLabelEdit(this, e);
			}
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x0015CF95 File Offset: 0x0015BF95
		protected virtual void OnBeforeCheck(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeCheck != null)
			{
				this.onBeforeCheck(this, e);
			}
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x0015CFAC File Offset: 0x0015BFAC
		protected virtual void OnAfterCheck(TreeViewEventArgs e)
		{
			if (this.onAfterCheck != null)
			{
				this.onAfterCheck(this, e);
			}
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x0015CFC3 File Offset: 0x0015BFC3
		protected internal virtual void OnBeforeCollapse(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeCollapse != null)
			{
				this.onBeforeCollapse(this, e);
			}
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x0015CFDA File Offset: 0x0015BFDA
		protected internal virtual void OnAfterCollapse(TreeViewEventArgs e)
		{
			if (this.onAfterCollapse != null)
			{
				this.onAfterCollapse(this, e);
			}
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x0015CFF1 File Offset: 0x0015BFF1
		protected virtual void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeExpand != null)
			{
				this.onBeforeExpand(this, e);
			}
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x0015D008 File Offset: 0x0015C008
		protected virtual void OnAfterExpand(TreeViewEventArgs e)
		{
			if (this.onAfterExpand != null)
			{
				this.onAfterExpand(this, e);
			}
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x0015D01F File Offset: 0x0015C01F
		protected virtual void OnItemDrag(ItemDragEventArgs e)
		{
			if (this.onItemDrag != null)
			{
				this.onItemDrag(this, e);
			}
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x0015D036 File Offset: 0x0015C036
		protected virtual void OnNodeMouseHover(TreeNodeMouseHoverEventArgs e)
		{
			if (this.onNodeMouseHover != null)
			{
				this.onNodeMouseHover(this, e);
			}
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x0015D04D File Offset: 0x0015C04D
		protected virtual void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeSelect != null)
			{
				this.onBeforeSelect(this, e);
			}
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x0015D064 File Offset: 0x0015C064
		protected virtual void OnAfterSelect(TreeViewEventArgs e)
		{
			if (this.onAfterSelect != null)
			{
				this.onAfterSelect(this, e);
			}
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x0015D07B File Offset: 0x0015C07B
		protected virtual void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			if (this.onNodeMouseClick != null)
			{
				this.onNodeMouseClick(this, e);
			}
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x0015D092 File Offset: 0x0015C092
		protected virtual void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
		{
			if (this.onNodeMouseDoubleClick != null)
			{
				this.onNodeMouseDoubleClick(this, e);
			}
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x0015D0AC File Offset: 0x0015C0AC
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this.CheckBoxes && (e.KeyData & Keys.KeyCode) == Keys.Space)
			{
				TreeNode treeNode = this.SelectedNode;
				if (treeNode != null)
				{
					if (!this.TreeViewBeforeCheck(treeNode, TreeViewAction.ByKeyboard))
					{
						treeNode.CheckedInternal = !treeNode.CheckedInternal;
						this.TreeViewAfterCheck(treeNode, TreeViewAction.ByKeyboard);
					}
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x0015D115 File Offset: 0x0015C115
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Handled)
			{
				return;
			}
			if ((e.KeyData & Keys.KeyCode) == Keys.Space)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x0015D13E File Offset: 0x0015C13E
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (e.Handled)
			{
				return;
			}
			if (e.KeyChar == ' ')
			{
				e.Handled = true;
			}
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x0015D161 File Offset: 0x0015C161
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			if (this.onRightToLeftLayoutChanged != null)
			{
				this.onRightToLeftLayoutChanged(this, e);
			}
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x0015D190 File Offset: 0x0015C190
		private void RefreshNodes()
		{
			TreeNode[] dest = new TreeNode[this.Nodes.Count];
			this.Nodes.CopyTo(dest, 0);
			this.Nodes.Clear();
			this.Nodes.AddRange(dest);
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x0015D1D2 File Offset: 0x0015C1D2
		private void ResetIndent()
		{
			this.indent = -1;
			base.RecreateHandle();
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x0015D1E1 File Offset: 0x0015C1E1
		private void ResetItemHeight()
		{
			this.itemHeight = -1;
			base.RecreateHandle();
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x0015D1F0 File Offset: 0x0015C1F0
		private bool ShouldSerializeIndent()
		{
			return this.indent != -1;
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x0015D1FE File Offset: 0x0015C1FE
		private bool ShouldSerializeItemHeight()
		{
			return this.itemHeight != -1;
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x0015D20C File Offset: 0x0015C20C
		private bool ShouldSerializeSelectedImageIndex()
		{
			if (this.imageList != null)
			{
				return this.SelectedImageIndex != 0;
			}
			return this.SelectedImageIndex != -1;
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x0015D22F File Offset: 0x0015C22F
		private bool ShouldSerializeImageIndex()
		{
			if (this.imageList != null)
			{
				return this.ImageIndex != 0;
			}
			return this.ImageIndex != -1;
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x0015D252 File Offset: 0x0015C252
		public void Sort()
		{
			this.Sorted = true;
			this.RefreshNodes();
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x0015D264 File Offset: 0x0015C264
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Nodes != null)
			{
				text = text + ", Nodes.Count: " + this.Nodes.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Nodes.Count > 0)
				{
					text = text + ", Nodes[0]: " + this.Nodes[0].ToString();
				}
			}
			return text;
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x0015D2D0 File Offset: 0x0015C2D0
		private unsafe void TvnBeginDrag(MouseButtons buttons, NativeMethods.NMTREEVIEW* nmtv)
		{
			NativeMethods.TV_ITEM itemNew = nmtv->itemNew;
			if (itemNew.hItem == IntPtr.Zero)
			{
				return;
			}
			TreeNode item = this.NodeFromHandle(itemNew.hItem);
			this.OnItemDrag(new ItemDragEventArgs(buttons, item));
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0015D314 File Offset: 0x0015C314
		private unsafe IntPtr TvnExpanding(NativeMethods.NMTREEVIEW* nmtv)
		{
			NativeMethods.TV_ITEM itemNew = nmtv->itemNew;
			if (itemNew.hItem == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			TreeViewCancelEventArgs treeViewCancelEventArgs;
			if ((itemNew.state & 32) == 0)
			{
				treeViewCancelEventArgs = new TreeViewCancelEventArgs(this.NodeFromHandle(itemNew.hItem), false, TreeViewAction.Expand);
				this.OnBeforeExpand(treeViewCancelEventArgs);
			}
			else
			{
				treeViewCancelEventArgs = new TreeViewCancelEventArgs(this.NodeFromHandle(itemNew.hItem), false, TreeViewAction.Collapse);
				this.OnBeforeCollapse(treeViewCancelEventArgs);
			}
			return (IntPtr)(treeViewCancelEventArgs.Cancel ? 1 : 0);
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0015D39C File Offset: 0x0015C39C
		private unsafe void TvnExpanded(NativeMethods.NMTREEVIEW* nmtv)
		{
			NativeMethods.TV_ITEM itemNew = nmtv->itemNew;
			if (itemNew.hItem == IntPtr.Zero)
			{
				return;
			}
			TreeNode node = this.NodeFromHandle(itemNew.hItem);
			TreeViewEventArgs e;
			if ((itemNew.state & 32) == 0)
			{
				e = new TreeViewEventArgs(node, TreeViewAction.Collapse);
				this.OnAfterCollapse(e);
				return;
			}
			e = new TreeViewEventArgs(node, TreeViewAction.Expand);
			this.OnAfterExpand(e);
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x0015D400 File Offset: 0x0015C400
		private unsafe IntPtr TvnSelecting(NativeMethods.NMTREEVIEW* nmtv)
		{
			if (this.treeViewState[65536])
			{
				return (IntPtr)1;
			}
			if (nmtv->itemNew.hItem == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			TreeNode node = this.NodeFromHandle(nmtv->itemNew.hItem);
			TreeViewAction action = TreeViewAction.Unknown;
			switch (nmtv->action)
			{
			case 1:
				action = TreeViewAction.ByMouse;
				break;
			case 2:
				action = TreeViewAction.ByKeyboard;
				break;
			}
			TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(node, false, action);
			this.OnBeforeSelect(treeViewCancelEventArgs);
			return (IntPtr)(treeViewCancelEventArgs.Cancel ? 1 : 0);
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0015D498 File Offset: 0x0015C498
		private unsafe void TvnSelected(NativeMethods.NMTREEVIEW* nmtv)
		{
			if (this.nodesCollectionClear)
			{
				return;
			}
			if (nmtv->itemNew.hItem != IntPtr.Zero)
			{
				TreeViewAction action = TreeViewAction.Unknown;
				switch (nmtv->action)
				{
				case 1:
					action = TreeViewAction.ByMouse;
					break;
				case 2:
					action = TreeViewAction.ByKeyboard;
					break;
				}
				this.OnAfterSelect(new TreeViewEventArgs(this.NodeFromHandle(nmtv->itemNew.hItem), action));
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			*(IntPtr*)(&rect.left) = nmtv->itemOld.hItem;
			if (nmtv->itemOld.hItem != IntPtr.Zero && (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4356, 1, ref rect) != 0)
			{
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), ref rect, true);
			}
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x0015D570 File Offset: 0x0015C570
		private IntPtr TvnBeginLabelEdit(NativeMethods.NMTVDISPINFO nmtvdi)
		{
			if (nmtvdi.item.hItem == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			TreeNode node = this.NodeFromHandle(nmtvdi.item.hItem);
			NodeLabelEditEventArgs nodeLabelEditEventArgs = new NodeLabelEditEventArgs(node);
			this.OnBeforeLabelEdit(nodeLabelEditEventArgs);
			if (!nodeLabelEditEventArgs.CancelEdit)
			{
				this.editNode = node;
			}
			return (IntPtr)(nodeLabelEditEventArgs.CancelEdit ? 1 : 0);
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x0015D5DC File Offset: 0x0015C5DC
		private IntPtr TvnEndLabelEdit(NativeMethods.NMTVDISPINFO nmtvdi)
		{
			this.editNode = null;
			if (nmtvdi.item.hItem == IntPtr.Zero)
			{
				return (IntPtr)1;
			}
			TreeNode treeNode = this.NodeFromHandle(nmtvdi.item.hItem);
			string text = (nmtvdi.item.pszText == IntPtr.Zero) ? null : Marshal.PtrToStringAuto(nmtvdi.item.pszText);
			NodeLabelEditEventArgs nodeLabelEditEventArgs = new NodeLabelEditEventArgs(treeNode, text);
			this.OnAfterLabelEdit(nodeLabelEditEventArgs);
			if (text != null && !nodeLabelEditEventArgs.CancelEdit && treeNode != null)
			{
				treeNode.text = text;
				if (this.Scrollable)
				{
					this.ForceScrollbarUpdate(true);
				}
			}
			return (IntPtr)(nodeLabelEditEventArgs.CancelEdit ? 0 : 1);
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x0015D68F File Offset: 0x0015C68F
		internal override void UpdateStylesCore()
		{
			base.UpdateStylesCore();
			if (base.IsHandleCreated && this.CheckBoxes && this.StateImageList != null && this.internalStateImageList != null)
			{
				base.SendMessage(4361, 2, this.internalStateImageList.Handle);
			}
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x0015D6D0 File Offset: 0x0015C6D0
		private void UpdateCheckedState(TreeNode node, bool update)
		{
			if (update)
			{
				node.CheckedInternal = node.CheckedInternal;
				for (int i = node.Nodes.Count - 1; i >= 0; i--)
				{
					this.UpdateCheckedState(node.Nodes[i], update);
				}
				return;
			}
			node.CheckedInternal = false;
			for (int j = node.Nodes.Count - 1; j >= 0; j--)
			{
				this.UpdateCheckedState(node.Nodes[j], update);
			}
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0015D74C File Offset: 0x0015C74C
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			base.SendMessage(4363, 8, null);
			this.OnMouseDown(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
			if (!base.ValidationCancelled)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x0015D79C File Offset: 0x0015C79C
		private void CustomDraw(ref Message m)
		{
			NativeMethods.NMTVCUSTOMDRAW nmtvcustomdraw = (NativeMethods.NMTVCUSTOMDRAW)m.GetLParam(typeof(NativeMethods.NMTVCUSTOMDRAW));
			int dwDrawStage = nmtvcustomdraw.nmcd.dwDrawStage;
			if (dwDrawStage != 1)
			{
				switch (dwDrawStage)
				{
				case 65537:
				{
					TreeNode treeNode = this.NodeFromHandle(nmtvcustomdraw.nmcd.dwItemSpec);
					if (treeNode == null)
					{
						m.Result = (IntPtr)4;
						return;
					}
					int uItemState = nmtvcustomdraw.nmcd.uItemState;
					if (this.drawMode == TreeViewDrawMode.OwnerDrawText)
					{
						nmtvcustomdraw.clrText = nmtvcustomdraw.clrTextBk;
						Marshal.StructureToPtr(nmtvcustomdraw, m.LParam, false);
						m.Result = (IntPtr)18;
						return;
					}
					if (this.drawMode == TreeViewDrawMode.OwnerDrawAll)
					{
						Graphics graphics = Graphics.FromHdcInternal(nmtvcustomdraw.nmcd.hdc);
						DrawTreeNodeEventArgs drawTreeNodeEventArgs;
						try
						{
							Rectangle rowBounds = treeNode.RowBounds;
							NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
							scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
							scrollinfo.fMask = 4;
							if (UnsafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), 0, scrollinfo))
							{
								int nPos = scrollinfo.nPos;
								if (nPos > 0)
								{
									rowBounds.X -= nPos;
									rowBounds.Width += nPos;
								}
							}
							drawTreeNodeEventArgs = new DrawTreeNodeEventArgs(graphics, treeNode, rowBounds, (TreeNodeStates)uItemState);
							this.OnDrawNode(drawTreeNodeEventArgs);
						}
						finally
						{
							graphics.Dispose();
						}
						if (!drawTreeNodeEventArgs.DrawDefault)
						{
							m.Result = (IntPtr)4;
							return;
						}
					}
					OwnerDrawPropertyBag itemRenderStyles = this.GetItemRenderStyles(treeNode, uItemState);
					bool flag = false;
					Color foreColor = itemRenderStyles.ForeColor;
					Color backColor = itemRenderStyles.BackColor;
					if (itemRenderStyles != null && !foreColor.IsEmpty)
					{
						nmtvcustomdraw.clrText = ColorTranslator.ToWin32(foreColor);
						flag = true;
					}
					if (itemRenderStyles != null && !backColor.IsEmpty)
					{
						nmtvcustomdraw.clrTextBk = ColorTranslator.ToWin32(backColor);
						flag = true;
					}
					if (flag)
					{
						Marshal.StructureToPtr(nmtvcustomdraw, m.LParam, false);
					}
					if (itemRenderStyles != null && itemRenderStyles.Font != null)
					{
						SafeNativeMethods.SelectObject(new HandleRef(nmtvcustomdraw.nmcd, nmtvcustomdraw.nmcd.hdc), new HandleRef(itemRenderStyles, itemRenderStyles.FontHandle));
						m.Result = (IntPtr)2;
						return;
					}
					break;
				}
				case 65538:
					if (this.drawMode == TreeViewDrawMode.OwnerDrawText)
					{
						TreeNode treeNode = this.NodeFromHandle(nmtvcustomdraw.nmcd.dwItemSpec);
						if (treeNode == null)
						{
							return;
						}
						Graphics graphics2 = Graphics.FromHdcInternal(nmtvcustomdraw.nmcd.hdc);
						try
						{
							Rectangle bounds = treeNode.Bounds;
							Size size = TextRenderer.MeasureText(treeNode.Text, treeNode.TreeView.Font);
							Point location = new Point(bounds.X - 1, bounds.Y);
							bounds = new Rectangle(location, new Size(size.Width, bounds.Height));
							DrawTreeNodeEventArgs drawTreeNodeEventArgs2 = new DrawTreeNodeEventArgs(graphics2, treeNode, bounds, (TreeNodeStates)nmtvcustomdraw.nmcd.uItemState);
							this.OnDrawNode(drawTreeNodeEventArgs2);
							if (drawTreeNodeEventArgs2.DrawDefault)
							{
								TreeNodeStates state = drawTreeNodeEventArgs2.State;
								Font font = (treeNode.NodeFont != null) ? treeNode.NodeFont : treeNode.TreeView.Font;
								Color foreColor2 = ((state & TreeNodeStates.Selected) == TreeNodeStates.Selected && treeNode.TreeView.Focused) ? SystemColors.HighlightText : ((treeNode.ForeColor != Color.Empty) ? treeNode.ForeColor : treeNode.TreeView.ForeColor);
								if ((state & TreeNodeStates.Selected) == TreeNodeStates.Selected)
								{
									graphics2.FillRectangle(SystemBrushes.Highlight, bounds);
									ControlPaint.DrawFocusRectangle(graphics2, bounds, foreColor2, SystemColors.Highlight);
									TextRenderer.DrawText(graphics2, drawTreeNodeEventArgs2.Node.Text, font, bounds, foreColor2, TextFormatFlags.Default);
								}
								else
								{
									graphics2.FillRectangle(SystemBrushes.Window, bounds);
									TextRenderer.DrawText(graphics2, drawTreeNodeEventArgs2.Node.Text, font, bounds, foreColor2, TextFormatFlags.Default);
								}
							}
						}
						finally
						{
							graphics2.Dispose();
						}
						m.Result = (IntPtr)32;
						return;
					}
					break;
				}
				m.Result = (IntPtr)0;
				return;
			}
			m.Result = (IntPtr)32;
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x0015DBAC File Offset: 0x0015CBAC
		protected OwnerDrawPropertyBag GetItemRenderStyles(TreeNode node, int state)
		{
			OwnerDrawPropertyBag ownerDrawPropertyBag = new OwnerDrawPropertyBag();
			if (node == null || node.propBag == null)
			{
				return ownerDrawPropertyBag;
			}
			if ((state & 71) == 0)
			{
				ownerDrawPropertyBag.ForeColor = node.propBag.ForeColor;
				ownerDrawPropertyBag.BackColor = node.propBag.BackColor;
			}
			ownerDrawPropertyBag.Font = node.propBag.Font;
			return ownerDrawPropertyBag;
		}

		// Token: 0x06006020 RID: 24608 RVA: 0x0015DC08 File Offset: 0x0015CC08
		private unsafe bool WmShowToolTip(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			IntPtr hwndFrom = ptr->hwndFrom;
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (intPtr != IntPtr.Zero && (tv_HITTESTINFO.flags & 70) != 0)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (treeNode != null && !this.ShowNodeToolTips)
				{
					Rectangle bounds = treeNode.Bounds;
					bounds.Location = base.PointToScreen(bounds.Location);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, hwndFrom), 1055, 1, ref bounds);
					SafeNativeMethods.SetWindowPos(new HandleRef(this, hwndFrom), NativeMethods.HWND_TOPMOST, bounds.Left, bounds.Top, 0, 0, 21);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006021 RID: 24609 RVA: 0x0015DCF8 File Offset: 0x0015CCF8
		private void WmNeedText(ref Message m)
		{
			NativeMethods.TOOLTIPTEXT tooltiptext = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			string lpszText = this.controlToolTipText;
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (intPtr != IntPtr.Zero && (tv_HITTESTINFO.flags & 70) != 0)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (this.ShowNodeToolTips && treeNode != null && !string.IsNullOrEmpty(treeNode.ToolTipText))
				{
					lpszText = treeNode.ToolTipText;
				}
				else if (treeNode != null && treeNode.Bounds.Right > base.Bounds.Right)
				{
					lpszText = treeNode.Text;
				}
				else
				{
					lpszText = null;
				}
			}
			tooltiptext.lpszText = lpszText;
			tooltiptext.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptext.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptext, m.LParam, false);
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x0015DE18 File Offset: 0x0015CE18
		private unsafe void WmNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			if (ptr->code == -12)
			{
				this.CustomDraw(ref m);
				return;
			}
			NativeMethods.NMTREEVIEW* ptr2 = (NativeMethods.NMTREEVIEW*)((void*)m.LParam);
			int code = ptr2->nmhdr.code;
			if (code <= -401)
			{
				switch (code)
				{
				case -460:
					goto IL_12E;
				case -459:
					goto IL_10C;
				case -458:
				case -453:
				case -452:
					return;
				case -457:
					goto IL_FF;
				case -456:
					goto IL_F2;
				case -455:
					goto IL_D4;
				case -454:
					break;
				case -451:
					goto IL_EA;
				case -450:
					goto IL_DC;
				default:
					switch (code)
					{
					case -411:
						goto IL_12E;
					case -410:
						goto IL_10C;
					case -409:
					case -404:
					case -403:
						return;
					case -408:
						goto IL_FF;
					case -407:
						goto IL_F2;
					case -406:
						goto IL_D4;
					case -405:
						break;
					case -402:
						goto IL_EA;
					case -401:
						goto IL_DC;
					default:
						return;
					}
					break;
				}
				m.Result = this.TvnExpanding(ptr2);
				return;
				IL_D4:
				this.TvnExpanded(ptr2);
				return;
				IL_DC:
				m.Result = this.TvnSelecting(ptr2);
				return;
				IL_EA:
				this.TvnSelected(ptr2);
				return;
				IL_F2:
				this.TvnBeginDrag(MouseButtons.Left, ptr2);
				return;
				IL_FF:
				this.TvnBeginDrag(MouseButtons.Right, ptr2);
				return;
				IL_10C:
				m.Result = this.TvnBeginLabelEdit((NativeMethods.NMTVDISPINFO)m.GetLParam(typeof(NativeMethods.NMTVDISPINFO)));
				return;
				IL_12E:
				m.Result = this.TvnEndLabelEdit((NativeMethods.NMTVDISPINFO)m.GetLParam(typeof(NativeMethods.NMTVDISPINFO)));
				return;
			}
			if (code != -5 && code != -2)
			{
				return;
			}
			MouseButtons button = MouseButtons.Left;
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (ptr2->nmhdr.code != -2 || (tv_HITTESTINFO.flags & 70) != 0)
			{
				button = ((ptr2->nmhdr.code == -2) ? MouseButtons.Left : MouseButtons.Right);
			}
			if ((ptr2->nmhdr.code != -2 || (tv_HITTESTINFO.flags & 70) != 0 || this.FullRowSelect) && intPtr != IntPtr.Zero && !base.ValidationCancelled)
			{
				this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(intPtr), button, 1, p.X, p.Y));
				this.OnClick(new MouseEventArgs(button, 1, p.X, p.Y, 0));
				this.OnMouseClick(new MouseEventArgs(button, 1, p.X, p.Y, 0));
			}
			if (ptr2->nmhdr.code == -5)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (treeNode != null && (treeNode.ContextMenu != null || treeNode.ContextMenuStrip != null))
				{
					this.ShowContextMenu(treeNode);
				}
				else
				{
					this.treeViewState[8192] = true;
					base.SendMessage(123, base.Handle, SafeNativeMethods.GetMessagePos());
				}
				m.Result = (IntPtr)1;
			}
			if (!this.treeViewState[4096] && (ptr2->nmhdr.code != -2 || (tv_HITTESTINFO.flags & 70) != 0))
			{
				this.OnMouseUp(new MouseEventArgs(button, 1, p.X, p.Y, 0));
				this.treeViewState[4096] = true;
			}
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0015E154 File Offset: 0x0015D154
		private void ShowContextMenu(TreeNode treeNode)
		{
			if (treeNode.ContextMenu != null || treeNode.ContextMenuStrip != null)
			{
				ContextMenu contextMenu = treeNode.ContextMenu;
				ContextMenuStrip contextMenuStrip = treeNode.ContextMenuStrip;
				if (contextMenu != null)
				{
					NativeMethods.POINT point = new NativeMethods.POINT();
					UnsafeNativeMethods.GetCursorPos(point);
					UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this, base.Handle));
					contextMenu.OnPopup(EventArgs.Empty);
					SafeNativeMethods.TrackPopupMenuEx(new HandleRef(contextMenu, contextMenu.Handle), 64, point.x, point.y, new HandleRef(this, base.Handle), null);
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 0, IntPtr.Zero, IntPtr.Zero);
					return;
				}
				if (contextMenuStrip != null)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 4363, 8, treeNode.Handle);
					contextMenuStrip.ShowInternal(this, base.PointToClient(Control.MousePosition), false);
					contextMenuStrip.Closing += this.ContextMenuStripClosing;
				}
			}
		}

		// Token: 0x06006024 RID: 24612 RVA: 0x0015E240 File Offset: 0x0015D240
		private void ContextMenuStripClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
			contextMenuStrip.Closing -= this.ContextMenuStripClosing;
			base.SendMessage(4363, 8, null);
		}

		// Token: 0x06006025 RID: 24613 RVA: 0x0015E274 File Offset: 0x0015D274
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && this.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rect = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder), rect);
						rect.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rect);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x0015E344 File Offset: 0x0015D344
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 131)
			{
				if (msg <= 71)
				{
					switch (msg)
					{
					case 5:
						break;
					case 6:
						goto IL_889;
					case 7:
						if (this.treeViewState[16384])
						{
							this.treeViewState[16384] = false;
							base.WmImeSetFocus();
							this.DefWndProc(ref m);
							this.OnGotFocus(EventArgs.Empty);
							return;
						}
						base.WndProc(ref m);
						return;
					default:
						if (msg == 21)
						{
							base.SendMessage(4359, this.Indent, 0);
							base.WndProc(ref m);
							return;
						}
						switch (msg)
						{
						case 70:
						case 71:
							break;
						default:
							goto IL_889;
						}
						break;
					}
				}
				else
				{
					if (msg == 78)
					{
						NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
						int code = nmhdr.code;
						if (code != -530)
						{
							switch (code)
							{
							case -521:
								if (this.WmShowToolTip(ref m))
								{
									m.Result = (IntPtr)1;
									return;
								}
								base.WndProc(ref m);
								return;
							case -520:
								break;
							default:
								base.WndProc(ref m);
								return;
							}
						}
						UnsafeNativeMethods.SendMessage(new HandleRef(nmhdr, nmhdr.hwndFrom), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
						this.WmNeedText(ref m);
						m.Result = (IntPtr)1;
						return;
					}
					if (msg != 123)
					{
						if (msg != 131)
						{
							goto IL_889;
						}
					}
					else
					{
						if (this.treeViewState[8192])
						{
							this.treeViewState[8192] = false;
							base.WndProc(ref m);
							return;
						}
						TreeNode treeNode = this.SelectedNode;
						if (treeNode == null || (treeNode.ContextMenu == null && treeNode.ContextMenuStrip == null))
						{
							base.WndProc(ref m);
							return;
						}
						Point point = new Point(treeNode.Bounds.X, treeNode.Bounds.Y + treeNode.Bounds.Height / 2);
						if (!base.ClientRectangle.Contains(point))
						{
							return;
						}
						if (treeNode.ContextMenu != null)
						{
							treeNode.ContextMenu.Show(this, point);
							return;
						}
						if (treeNode.ContextMenuStrip != null)
						{
							bool isKeyboardActivated = (int)((long)m.LParam) == -1;
							treeNode.ContextMenuStrip.ShowInternal(this, point, isKeyboardActivated);
							return;
						}
						return;
					}
				}
				if (this.treeViewState[32768])
				{
					this.DefWndProc(ref m);
					return;
				}
				base.WndProc(ref m);
				return;
			}
			else if (msg <= 675)
			{
				if (msg != 276)
				{
					switch (msg)
					{
					case 513:
					{
						try
						{
							this.treeViewState[65536] = true;
							this.FocusInternal();
						}
						finally
						{
							this.treeViewState[65536] = false;
						}
						this.treeViewState[4096] = false;
						NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
						tv_HITTESTINFO.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_HITTESTINFO.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						this.hNodeMouseDown = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
						if ((tv_HITTESTINFO.flags & 64) != 0)
						{
							this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							if (!base.ValidationCancelled && this.CheckBoxes)
							{
								TreeNode treeNode2 = this.NodeFromHandle(this.hNodeMouseDown);
								if (!this.TreeViewBeforeCheck(treeNode2, TreeViewAction.ByMouse) && treeNode2 != null)
								{
									treeNode2.CheckedInternal = !treeNode2.CheckedInternal;
									this.TreeViewAfterCheck(treeNode2, TreeViewAction.ByMouse);
								}
							}
							m.Result = IntPtr.Zero;
						}
						else
						{
							this.WmMouseDown(ref m, MouseButtons.Left, 1);
						}
						this.downButton = MouseButtons.Left;
						return;
					}
					case 514:
					case 517:
					{
						NativeMethods.TV_HITTESTINFO tv_HITTESTINFO2 = new NativeMethods.TV_HITTESTINFO();
						tv_HITTESTINFO2.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_HITTESTINFO2.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO2);
						if (intPtr != IntPtr.Zero)
						{
							if (!base.ValidationCancelled && (!this.treeViewState[2048] & !this.treeViewState[4096]))
							{
								if (intPtr == this.hNodeMouseDown)
								{
									this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(intPtr), this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam)));
								}
								this.OnClick(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								this.OnMouseClick(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							}
							if (this.treeViewState[2048])
							{
								this.treeViewState[2048] = false;
								if (!base.ValidationCancelled)
								{
									this.OnNodeMouseDoubleClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(intPtr), this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam)));
									this.OnDoubleClick(new MouseEventArgs(this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
									this.OnMouseDoubleClick(new MouseEventArgs(this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								}
							}
						}
						if (!this.treeViewState[4096])
						{
							this.OnMouseUp(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						}
						this.treeViewState[2048] = false;
						this.treeViewState[4096] = false;
						base.CaptureInternal = false;
						this.hNodeMouseDown = IntPtr.Zero;
						return;
					}
					case 515:
						this.WmMouseDown(ref m, MouseButtons.Left, 2);
						this.treeViewState[2048] = true;
						this.treeViewState[4096] = false;
						base.CaptureInternal = true;
						return;
					case 516:
					{
						this.treeViewState[4096] = false;
						NativeMethods.TV_HITTESTINFO tv_HITTESTINFO3 = new NativeMethods.TV_HITTESTINFO();
						tv_HITTESTINFO3.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_HITTESTINFO3.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						this.hNodeMouseDown = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO3);
						this.WmMouseDown(ref m, MouseButtons.Right, 1);
						this.downButton = MouseButtons.Right;
						return;
					}
					case 518:
						this.WmMouseDown(ref m, MouseButtons.Right, 2);
						this.treeViewState[2048] = true;
						this.treeViewState[4096] = false;
						base.CaptureInternal = true;
						return;
					case 519:
						this.treeViewState[4096] = false;
						this.WmMouseDown(ref m, MouseButtons.Middle, 1);
						this.downButton = MouseButtons.Middle;
						return;
					case 520:
						break;
					case 521:
						this.treeViewState[4096] = false;
						this.WmMouseDown(ref m, MouseButtons.Middle, 2);
						return;
					default:
						if (msg == 675)
						{
							this.prevHoveredNode = null;
							base.WndProc(ref m);
							return;
						}
						break;
					}
				}
				else
				{
					base.WndProc(ref m);
					if (this.DrawMode == TreeViewDrawMode.OwnerDrawAll)
					{
						base.Invalidate();
						return;
					}
					return;
				}
			}
			else
			{
				if (msg <= 4365)
				{
					if (msg == 791)
					{
						this.WmPrint(ref m);
						return;
					}
					if (msg != 4365)
					{
						goto IL_889;
					}
				}
				else if (msg != 4415)
				{
					if (msg != 8270)
					{
						goto IL_889;
					}
					this.WmNotify(ref m);
					return;
				}
				base.WndProc(ref m);
				if (!this.CheckBoxes)
				{
					return;
				}
				NativeMethods.TV_ITEM tv_ITEM = (NativeMethods.TV_ITEM)m.GetLParam(typeof(NativeMethods.TV_ITEM));
				if (tv_ITEM.hItem != IntPtr.Zero)
				{
					NativeMethods.TV_ITEM tv_ITEM2 = default(NativeMethods.TV_ITEM);
					tv_ITEM2.mask = 24;
					tv_ITEM2.hItem = tv_ITEM.hItem;
					tv_ITEM2.stateMask = 61440;
					UnsafeNativeMethods.SendMessage(new HandleRef(null, base.Handle), NativeMethods.TVM_GETITEM, 0, ref tv_ITEM2);
					TreeNode treeNode3 = this.NodeFromHandle(tv_ITEM.hItem);
					treeNode3.CheckedStateInternal = (tv_ITEM2.state >> 12 > 1);
					return;
				}
				return;
			}
			IL_889:
			base.WndProc(ref m);
		}

		// Token: 0x040039D5 RID: 14805
		private const int DefaultTreeViewIndent = 19;

		// Token: 0x040039D6 RID: 14806
		private const int TREEVIEWSTATE_hideSelection = 1;

		// Token: 0x040039D7 RID: 14807
		private const int TREEVIEWSTATE_labelEdit = 2;

		// Token: 0x040039D8 RID: 14808
		private const int TREEVIEWSTATE_scrollable = 4;

		// Token: 0x040039D9 RID: 14809
		private const int TREEVIEWSTATE_checkBoxes = 8;

		// Token: 0x040039DA RID: 14810
		private const int TREEVIEWSTATE_showLines = 16;

		// Token: 0x040039DB RID: 14811
		private const int TREEVIEWSTATE_showPlusMinus = 32;

		// Token: 0x040039DC RID: 14812
		private const int TREEVIEWSTATE_showRootLines = 64;

		// Token: 0x040039DD RID: 14813
		private const int TREEVIEWSTATE_sorted = 128;

		// Token: 0x040039DE RID: 14814
		private const int TREEVIEWSTATE_hotTracking = 256;

		// Token: 0x040039DF RID: 14815
		private const int TREEVIEWSTATE_fullRowSelect = 512;

		// Token: 0x040039E0 RID: 14816
		private const int TREEVIEWSTATE_showNodeToolTips = 1024;

		// Token: 0x040039E1 RID: 14817
		private const int TREEVIEWSTATE_doubleclickFired = 2048;

		// Token: 0x040039E2 RID: 14818
		private const int TREEVIEWSTATE_mouseUpFired = 4096;

		// Token: 0x040039E3 RID: 14819
		private const int TREEVIEWSTATE_showTreeViewContextMenu = 8192;

		// Token: 0x040039E4 RID: 14820
		private const int TREEVIEWSTATE_lastControlValidated = 16384;

		// Token: 0x040039E5 RID: 14821
		private const int TREEVIEWSTATE_stopResizeWindowMsgs = 32768;

		// Token: 0x040039E6 RID: 14822
		private const int TREEVIEWSTATE_ignoreSelects = 65536;

		// Token: 0x040039E7 RID: 14823
		private static readonly int MaxIndent = 32000;

		// Token: 0x040039E8 RID: 14824
		private static readonly string backSlash = "\\";

		// Token: 0x040039E9 RID: 14825
		private DrawTreeNodeEventHandler onDrawNode;

		// Token: 0x040039EA RID: 14826
		private NodeLabelEditEventHandler onBeforeLabelEdit;

		// Token: 0x040039EB RID: 14827
		private NodeLabelEditEventHandler onAfterLabelEdit;

		// Token: 0x040039EC RID: 14828
		private TreeViewCancelEventHandler onBeforeCheck;

		// Token: 0x040039ED RID: 14829
		private TreeViewEventHandler onAfterCheck;

		// Token: 0x040039EE RID: 14830
		private TreeViewCancelEventHandler onBeforeCollapse;

		// Token: 0x040039EF RID: 14831
		private TreeViewEventHandler onAfterCollapse;

		// Token: 0x040039F0 RID: 14832
		private TreeViewCancelEventHandler onBeforeExpand;

		// Token: 0x040039F1 RID: 14833
		private TreeViewEventHandler onAfterExpand;

		// Token: 0x040039F2 RID: 14834
		private TreeViewCancelEventHandler onBeforeSelect;

		// Token: 0x040039F3 RID: 14835
		private TreeViewEventHandler onAfterSelect;

		// Token: 0x040039F4 RID: 14836
		private ItemDragEventHandler onItemDrag;

		// Token: 0x040039F5 RID: 14837
		private TreeNodeMouseHoverEventHandler onNodeMouseHover;

		// Token: 0x040039F6 RID: 14838
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x040039F7 RID: 14839
		internal TreeNode selectedNode;

		// Token: 0x040039F8 RID: 14840
		private ImageList.Indexer imageIndexer;

		// Token: 0x040039F9 RID: 14841
		private ImageList.Indexer selectedImageIndexer;

		// Token: 0x040039FA RID: 14842
		private bool setOddHeight;

		// Token: 0x040039FB RID: 14843
		private TreeNode prevHoveredNode;

		// Token: 0x040039FC RID: 14844
		private bool hoveredAlready;

		// Token: 0x040039FD RID: 14845
		private bool rightToLeftLayout;

		// Token: 0x040039FE RID: 14846
		private IntPtr hNodeMouseDown = IntPtr.Zero;

		// Token: 0x040039FF RID: 14847
		private BitVector32 treeViewState;

		// Token: 0x04003A00 RID: 14848
		private ImageList imageList;

		// Token: 0x04003A01 RID: 14849
		private int indent = -1;

		// Token: 0x04003A02 RID: 14850
		private int itemHeight = -1;

		// Token: 0x04003A03 RID: 14851
		private string pathSeparator = TreeView.backSlash;

		// Token: 0x04003A04 RID: 14852
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x04003A05 RID: 14853
		internal TreeNodeCollection nodes;

		// Token: 0x04003A06 RID: 14854
		internal TreeNode editNode;

		// Token: 0x04003A07 RID: 14855
		internal TreeNode root;

		// Token: 0x04003A08 RID: 14856
		internal Hashtable nodeTable = new Hashtable();

		// Token: 0x04003A09 RID: 14857
		internal bool nodesCollectionClear;

		// Token: 0x04003A0A RID: 14858
		private MouseButtons downButton;

		// Token: 0x04003A0B RID: 14859
		private TreeViewDrawMode drawMode;

		// Token: 0x04003A0C RID: 14860
		private ImageList internalStateImageList;

		// Token: 0x04003A0D RID: 14861
		private TreeNode topNode;

		// Token: 0x04003A0E RID: 14862
		private ImageList stateImageList;

		// Token: 0x04003A0F RID: 14863
		private Color lineColor;

		// Token: 0x04003A10 RID: 14864
		private string controlToolTipText;

		// Token: 0x04003A11 RID: 14865
		private IComparer treeViewNodeSorter;

		// Token: 0x04003A12 RID: 14866
		private TreeNodeMouseClickEventHandler onNodeMouseClick;

		// Token: 0x04003A13 RID: 14867
		private TreeNodeMouseClickEventHandler onNodeMouseDoubleClick;
	}
}
