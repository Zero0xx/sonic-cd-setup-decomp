using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000290 RID: 656
	[DefaultBindingProperty("Text")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.ComboBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionComboBox")]
	public class ComboBox : ListControl
	{
		// Token: 0x060022ED RID: 8941 RVA: 0x0004CFB8 File Offset: 0x0004BFB8
		public ComboBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.UseTextForAccessibility, false);
			this.requestedHeight = 150;
			base.SetState2(2048, true);
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x0004D047 File Offset: 0x0004C047
		// (set) Token: 0x060022EF RID: 8943 RVA: 0x0004D050 File Offset: 0x0004C050
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(AutoCompleteMode.None)]
		[SRDescription("ComboBoxAutoCompleteModeDescr")]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.autoCompleteMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteMode));
				}
				if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.AutoCompleteSource != AutoCompleteSource.ListItems && value != AutoCompleteMode.None)
				{
					throw new NotSupportedException(SR.GetString("ComboBoxAutoCompleteModeOnlyNoneAllowed"));
				}
				if (Application.OleRequired() != ApartmentState.STA)
				{
					throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
				}
				bool reset = false;
				if (this.autoCompleteMode != AutoCompleteMode.None && value == AutoCompleteMode.None)
				{
					reset = true;
				}
				this.autoCompleteMode = value;
				this.SetAutoComplete(reset, true);
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x0004D0E1 File Offset: 0x0004C0E1
		// (set) Token: 0x060022F1 RID: 8945 RVA: 0x0004D0EC File Offset: 0x0004C0EC
		[SRDescription("ComboBoxAutoCompleteSourceDescr")]
		[DefaultValue(AutoCompleteSource.None)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.autoCompleteSource;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					128,
					7,
					6,
					64,
					1,
					32,
					2,
					256,
					4
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteSource));
				}
				if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.AutoCompleteMode != AutoCompleteMode.None && value != AutoCompleteSource.ListItems)
				{
					throw new NotSupportedException(SR.GetString("ComboBoxAutoCompleteSourceOnlyListItemsAllowed"));
				}
				if (Application.OleRequired() != ApartmentState.STA)
				{
					throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
				}
				if (value != AutoCompleteSource.None && value != AutoCompleteSource.CustomSource && value != AutoCompleteSource.ListItems)
				{
					new FileIOPermission(PermissionState.Unrestricted)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Demand();
				}
				this.autoCompleteSource = value;
				this.SetAutoComplete(false, true);
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x0004D1CC File Offset: 0x0004C1CC
		// (set) Token: 0x060022F3 RID: 8947 RVA: 0x0004D200 File Offset: 0x0004C200
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Localizable(true)]
		[SRDescription("ComboBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				if (this.autoCompleteCustomSource == null)
				{
					this.autoCompleteCustomSource = new AutoCompleteStringCollection();
					this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
				}
				return this.autoCompleteCustomSource;
			}
			set
			{
				if (this.autoCompleteCustomSource != value)
				{
					if (this.autoCompleteCustomSource != null)
					{
						this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
					}
					this.autoCompleteCustomSource = value;
					if (this.autoCompleteCustomSource != null)
					{
						this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
					}
					this.SetAutoComplete(false, true);
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x0004D263 File Offset: 0x0004C263
		// (set) Token: 0x060022F5 RID: 8949 RVA: 0x0004D279 File Offset: 0x0004C279
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
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x0004D282 File Offset: 0x0004C282
		// (set) Token: 0x060022F7 RID: 8951 RVA: 0x0004D28A File Offset: 0x0004C28A
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

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0004D293 File Offset: 0x0004C293
		// (set) Token: 0x060022F9 RID: 8953 RVA: 0x0004D29B File Offset: 0x0004C29B
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

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x060022FA RID: 8954 RVA: 0x0004D2A4 File Offset: 0x0004C2A4
		// (remove) Token: 0x060022FB RID: 8955 RVA: 0x0004D2AD File Offset: 0x0004C2AD
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

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x060022FC RID: 8956 RVA: 0x0004D2B6 File Offset: 0x0004C2B6
		// (remove) Token: 0x060022FD RID: 8957 RVA: 0x0004D2BF File Offset: 0x0004C2BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x0004D2C8 File Offset: 0x0004C2C8
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "COMBOBOX";
				createParams.Style |= 2097728;
				createParams.ExStyle |= 512;
				if (!this.integralHeight)
				{
					createParams.Style |= 1024;
				}
				switch (this.DropDownStyle)
				{
				case ComboBoxStyle.Simple:
					createParams.Style |= 1;
					break;
				case ComboBoxStyle.DropDown:
					createParams.Style |= 2;
					createParams.Height = this.PreferredHeight;
					break;
				case ComboBoxStyle.DropDownList:
					createParams.Style |= 3;
					createParams.Height = this.PreferredHeight;
					break;
				}
				switch (this.DrawMode)
				{
				case DrawMode.OwnerDrawFixed:
					createParams.Style |= 16;
					break;
				case DrawMode.OwnerDrawVariable:
					createParams.Style |= 32;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x0004D3C0 File Offset: 0x0004C3C0
		protected override Size DefaultSize
		{
			get
			{
				return new Size(121, this.PreferredHeight);
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x0004D3CF File Offset: 0x0004C3CF
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x0004D3D7 File Offset: 0x0004C3D7
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRCategory("CatData")]
		[SRDescription("ListControlDataSourceDescr")]
		[DefaultValue(null)]
		public new object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x0004D3E0 File Offset: 0x0004C3E0
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x0004D408 File Offset: 0x0004C408
		[DefaultValue(DrawMode.Normal)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ComboBoxDrawModeDescr")]
		public DrawMode DrawMode
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropDrawMode, out flag);
				if (flag)
				{
					return (DrawMode)integer;
				}
				return DrawMode.Normal;
			}
			set
			{
				if (this.DrawMode != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(DrawMode));
					}
					this.ResetHeightCache();
					base.Properties.SetInteger(ComboBox.PropDrawMode, (int)value);
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x0004D464 File Offset: 0x0004C464
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x0004D490 File Offset: 0x0004C490
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownWidthDescr")]
		public int DropDownWidth
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropDropDownWidth, out flag);
				if (flag)
				{
					return integer;
				}
				return base.Width;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("DropDownWidth", SR.GetString("InvalidArgument", new object[]
					{
						"DropDownWidth",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.Properties.GetInteger(ComboBox.PropDropDownWidth) != value)
				{
					base.Properties.SetInteger(ComboBox.PropDropDownWidth, value);
					if (base.IsHandleCreated)
					{
						base.SendMessage(352, value, 0);
					}
				}
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x0004D510 File Offset: 0x0004C510
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x0004D538 File Offset: 0x0004C538
		[SRDescription("ComboBoxDropDownHeightDescr")]
		[DefaultValue(106)]
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public int DropDownHeight
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropDropDownHeight, out flag);
				if (flag)
				{
					return integer;
				}
				return 106;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("DropDownHeight", SR.GetString("InvalidArgument", new object[]
					{
						"DropDownHeight",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.Properties.GetInteger(ComboBox.PropDropDownHeight) != value)
				{
					base.Properties.SetInteger(ComboBox.PropDropDownHeight, value);
					this.IntegralHeight = false;
				}
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x0004D5A8 File Offset: 0x0004C5A8
		// (set) Token: 0x06002309 RID: 8969 RVA: 0x0004D5CC File Offset: 0x0004C5CC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ComboBoxDroppedDownDescr")]
		public bool DroppedDown
		{
			get
			{
				return base.IsHandleCreated && (int)base.SendMessage(343, 0, 0) != 0;
			}
			set
			{
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				base.SendMessage(335, value ? -1 : 0, 0);
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x0004D5F0 File Offset: 0x0004C5F0
		// (set) Token: 0x0600230B RID: 8971 RVA: 0x0004D5F8 File Offset: 0x0004C5F8
		[Localizable(true)]
		[DefaultValue(FlatStyle.Standard)]
		[SRDescription("ComboBoxFlatStyleDescr")]
		[SRCategory("CatAppearance")]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.flatStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				this.flatStyle = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x0004D630 File Offset: 0x0004C630
		public override bool Focused
		{
			get
			{
				if (base.Focused)
				{
					return true;
				}
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				return focus != IntPtr.Zero && ((this.childEdit != null && focus == this.childEdit.Handle) || (this.childListBox != null && focus == this.childListBox.Handle));
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600230D RID: 8973 RVA: 0x0004D694 File Offset: 0x0004C694
		// (set) Token: 0x0600230E RID: 8974 RVA: 0x0004D6AA File Offset: 0x0004C6AA
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
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x0004D6B3 File Offset: 0x0004C6B3
		// (set) Token: 0x06002310 RID: 8976 RVA: 0x0004D6BB File Offset: 0x0004C6BB
		[Localizable(true)]
		[DefaultValue(true)]
		[SRDescription("ComboBoxIntegralHeightDescr")]
		[SRCategory("CatBehavior")]
		public bool IntegralHeight
		{
			get
			{
				return this.integralHeight;
			}
			set
			{
				if (this.integralHeight != value)
				{
					this.integralHeight = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x0004D6D4 File Offset: 0x0004C6D4
		// (set) Token: 0x06002312 RID: 8978 RVA: 0x0004D738 File Offset: 0x0004C738
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxItemHeightDescr")]
		[Localizable(true)]
		public int ItemHeight
		{
			get
			{
				DrawMode drawMode = this.DrawMode;
				if (drawMode == DrawMode.OwnerDrawFixed || drawMode == DrawMode.OwnerDrawVariable || !base.IsHandleCreated)
				{
					bool flag;
					int integer = base.Properties.GetInteger(ComboBox.PropItemHeight, out flag);
					if (flag)
					{
						return integer;
					}
					return base.FontHeight + 2;
				}
				else
				{
					int num = (int)base.SendMessage(340, 0, 0);
					if (num == -1)
					{
						throw new Win32Exception();
					}
					return num;
				}
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidArgument", new object[]
					{
						"ItemHeight",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.ResetHeightCache();
				if (base.Properties.GetInteger(ComboBox.PropItemHeight) != value)
				{
					base.Properties.SetInteger(ComboBox.PropItemHeight, value);
					if (this.DrawMode != DrawMode.Normal)
					{
						this.UpdateItemHeight();
					}
				}
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x0004D7B5 File Offset: 0x0004C7B5
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ComboBoxItemsDescr")]
		[MergableProperty(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRCategory("CatData")]
		public ComboBox.ObjectCollection Items
		{
			get
			{
				if (this.itemsCollection == null)
				{
					this.itemsCollection = new ComboBox.ObjectCollection(this);
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x0004D7D4 File Offset: 0x0004C7D4
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x0004D801 File Offset: 0x0004C801
		private string MatchingText
		{
			get
			{
				string text = (string)base.Properties.GetObject(ComboBox.PropMatchingText);
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(ComboBox.PropMatchingText))
				{
					base.Properties.SetObject(ComboBox.PropMatchingText, value);
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x0004D829 File Offset: 0x0004C829
		// (set) Token: 0x06002317 RID: 8983 RVA: 0x0004D834 File Offset: 0x0004C834
		[SRDescription("ComboBoxMaxDropDownItemsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(8)]
		[Localizable(true)]
		public int MaxDropDownItems
		{
			get
			{
				return (int)this.maxDropDownItems;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("MaxDropDownItems", SR.GetString("InvalidBoundArgument", new object[]
					{
						"MaxDropDownItems",
						value.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture),
						100.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.maxDropDownItems = (short)value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0004D8A9 File Offset: 0x0004C8A9
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x0004D8B1 File Offset: 0x0004C8B1
		public override Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = new Size(value.Width, 0);
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x0004D8C6 File Offset: 0x0004C8C6
		// (set) Token: 0x0600231B RID: 8987 RVA: 0x0004D8CE File Offset: 0x0004C8CE
		public override Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = new Size(value.Width, 0);
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x0004D8E3 File Offset: 0x0004C8E3
		// (set) Token: 0x0600231D RID: 8989 RVA: 0x0004D8F5 File Offset: 0x0004C8F5
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxMaxLengthDescr")]
		[Localizable(true)]
		[DefaultValue(0)]
		public int MaxLength
		{
			get
			{
				return base.Properties.GetInteger(ComboBox.PropMaxLength);
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (this.MaxLength != value)
				{
					base.Properties.SetInteger(ComboBox.PropMaxLength, value);
					if (base.IsHandleCreated)
					{
						base.SendMessage(321, value, 0);
					}
				}
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x0004D92E File Offset: 0x0004C92E
		// (set) Token: 0x0600231F RID: 8991 RVA: 0x0004D936 File Offset: 0x0004C936
		internal bool MouseIsOver
		{
			get
			{
				return this.mouseOver;
			}
			set
			{
				if (this.mouseOver != value)
				{
					this.mouseOver = value;
					if ((!base.ContainsFocus || !Application.RenderWithVisualStyles) && this.FlatStyle == FlatStyle.Popup)
					{
						base.Invalidate();
						base.Update();
					}
				}
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x0004D96C File Offset: 0x0004C96C
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x0004D974 File Offset: 0x0004C974
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06002322 RID: 8994 RVA: 0x0004D97D File Offset: 0x0004C97D
		// (remove) Token: 0x06002323 RID: 8995 RVA: 0x0004D986 File Offset: 0x0004C986
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

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x0004D990 File Offset: 0x0004C990
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxPreferredHeightDescr")]
		[Browsable(false)]
		public int PreferredHeight
		{
			get
			{
				if (!base.FormattingEnabled)
				{
					this.prefHeightCache = (short)(TextRenderer.MeasureText(LayoutUtils.TestString, this.Font, new Size(32767, (int)((double)base.FontHeight * 1.25)), TextFormatFlags.SingleLine).Height + SystemInformation.BorderSize.Height * 8 + this.Padding.Size.Height);
					return (int)this.prefHeightCache;
				}
				if (this.prefHeightCache < 0)
				{
					Size size = TextRenderer.MeasureText(LayoutUtils.TestString, this.Font, new Size(32767, (int)((double)base.FontHeight * 1.25)), TextFormatFlags.SingleLine);
					if (this.DropDownStyle == ComboBoxStyle.Simple)
					{
						int num = this.Items.Count + 1;
						this.prefHeightCache = (short)(size.Height * num + SystemInformation.BorderSize.Height * 16 + this.Padding.Size.Height);
					}
					else
					{
						this.prefHeightCache = (short)this.GetComboHeight();
					}
				}
				return (int)this.prefHeightCache;
			}
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0004DAB4 File Offset: 0x0004CAB4
		private int GetComboHeight()
		{
			Size size = Size.Empty;
			using (WindowsFont windowsFont = WindowsFont.FromFont(this.Font))
			{
				size = WindowsGraphicsCacheManager.MeasurementGraphics.GetTextExtent("0", windowsFont);
			}
			int num = size.Height + SystemInformation.Border3DSize.Height;
			if (this.DrawMode != DrawMode.Normal)
			{
				num = this.ItemHeight;
			}
			return 2 * SystemInformation.FixedFrameBorderSize.Height + num;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0004DB3C File Offset: 0x0004CB3C
		private string[] GetStringsForAutoComplete(IList collection)
		{
			if (collection is AutoCompleteStringCollection)
			{
				string[] array = new string[this.AutoCompleteCustomSource.Count];
				for (int i = 0; i < this.AutoCompleteCustomSource.Count; i++)
				{
					array[i] = this.AutoCompleteCustomSource[i];
				}
				return array;
			}
			if (collection is ComboBox.ObjectCollection)
			{
				string[] array2 = new string[this.itemsCollection.Count];
				for (int j = 0; j < this.itemsCollection.Count; j++)
				{
					array2[j] = base.GetItemText(this.itemsCollection[j]);
				}
				return array2;
			}
			return new string[0];
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0004DBD5 File Offset: 0x0004CBD5
		// (set) Token: 0x06002328 RID: 9000 RVA: 0x0004DBF8 File Offset: 0x0004CBF8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedIndexDescr")]
		public override int SelectedIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(327, 0, 0);
				}
				return this.selectedIndex;
			}
			set
			{
				if (this.SelectedIndex != value)
				{
					int num = 0;
					if (this.itemsCollection != null)
					{
						num = this.itemsCollection.Count;
					}
					if (value < -1 || value >= num)
					{
						throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidArgument", new object[]
						{
							"SelectedIndex",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(334, value, 0);
					}
					else
					{
						this.selectedIndex = value;
					}
					this.UpdateText();
					if (base.IsHandleCreated)
					{
						this.OnTextChanged(EventArgs.Empty);
					}
					this.OnSelectedItemChanged(EventArgs.Empty);
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x0004DCB4 File Offset: 0x0004CCB4
		// (set) Token: 0x0600232A RID: 9002 RVA: 0x0004DCDC File Offset: 0x0004CCDC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedItemDescr")]
		[Browsable(false)]
		[Bindable(true)]
		public object SelectedItem
		{
			get
			{
				int num = this.SelectedIndex;
				if (num != -1)
				{
					return this.Items[num];
				}
				return null;
			}
			set
			{
				int num = -1;
				if (this.itemsCollection != null)
				{
					if (value != null)
					{
						num = this.itemsCollection.IndexOf(value);
					}
					else
					{
						this.SelectedIndex = -1;
					}
				}
				if (num != -1)
				{
					this.SelectedIndex = num;
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x0004DD17 File Offset: 0x0004CD17
		// (set) Token: 0x0600232C RID: 9004 RVA: 0x0004DD40 File Offset: 0x0004CD40
		[SRDescription("ComboBoxSelectedTextDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string SelectedText
		{
			get
			{
				if (this.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					return "";
				}
				return this.Text.Substring(this.SelectionStart, this.SelectionLength);
			}
			set
			{
				if (this.DropDownStyle != ComboBoxStyle.DropDownList)
				{
					string lParam = (value == null) ? "" : value;
					base.CreateControl();
					if (base.IsHandleCreated && this.childEdit != null)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 194, NativeMethods.InvalidIntPtr, lParam);
					}
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x0004DD9C File Offset: 0x0004CD9C
		// (set) Token: 0x0600232E RID: 9006 RVA: 0x0004DDDB File Offset: 0x0004CDDB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectionLengthDescr")]
		[Browsable(false)]
		public int SelectionLength
		{
			get
			{
				int[] array = new int[1];
				int[] array2 = array;
				int[] array3 = new int[1];
				int[] array4 = array3;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 320, array4, array2);
				return array2[0] - array4[0];
			}
			set
			{
				this.Select(this.SelectionStart, value);
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600232F RID: 9007 RVA: 0x0004DDEC File Offset: 0x0004CDEC
		// (set) Token: 0x06002330 RID: 9008 RVA: 0x0004DE20 File Offset: 0x0004CE20
		[SRDescription("ComboBoxSelectionStartDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int SelectionStart
		{
			get
			{
				int[] array = new int[1];
				int[] array2 = array;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 320, array2, null);
				return array2[0];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidArgument", new object[]
					{
						"SelectionStart",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.Select(value, this.SelectionLength);
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x0004DE72 File Offset: 0x0004CE72
		// (set) Token: 0x06002332 RID: 9010 RVA: 0x0004DE7A File Offset: 0x0004CE7A
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxSortedDescr")]
		[DefaultValue(false)]
		public bool Sorted
		{
			get
			{
				return this.sorted;
			}
			set
			{
				if (this.sorted != value)
				{
					if (this.DataSource != null && value)
					{
						throw new ArgumentException(SR.GetString("ComboBoxSortWithDataSource"));
					}
					this.sorted = value;
					this.RefreshItems();
					this.SelectedIndex = -1;
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x0004DEB4 File Offset: 0x0004CEB4
		// (set) Token: 0x06002334 RID: 9012 RVA: 0x0004DEDC File Offset: 0x0004CEDC
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ComboBoxStyleDescr")]
		[DefaultValue(ComboBoxStyle.DropDown)]
		[SRCategory("CatAppearance")]
		public ComboBoxStyle DropDownStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropStyle, out flag);
				if (flag)
				{
					return (ComboBoxStyle)integer;
				}
				return ComboBoxStyle.DropDown;
			}
			set
			{
				if (this.DropDownStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ComboBoxStyle));
					}
					if (value == ComboBoxStyle.DropDownList && this.AutoCompleteSource != AutoCompleteSource.ListItems && this.AutoCompleteMode != AutoCompleteMode.None)
					{
						this.AutoCompleteMode = AutoCompleteMode.None;
					}
					this.ResetHeightCache();
					base.Properties.SetInteger(ComboBox.PropStyle, (int)value);
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
					this.OnDropDownStyleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002335 RID: 9013 RVA: 0x0004DF68 File Offset: 0x0004CF68
		// (set) Token: 0x06002336 RID: 9014 RVA: 0x0004DFD0 File Offset: 0x0004CFD0
		[Localizable(true)]
		[Bindable(true)]
		public override string Text
		{
			get
			{
				if (this.SelectedItem != null && !base.BindingFieldEmpty)
				{
					if (!base.FormattingEnabled)
					{
						return base.FilterItemOnProperty(this.SelectedItem).ToString();
					}
					string itemText = base.GetItemText(this.SelectedItem);
					if (!string.IsNullOrEmpty(itemText) && string.Compare(itemText, base.Text, true, CultureInfo.CurrentCulture) == 0)
					{
						return itemText;
					}
				}
				return base.Text;
			}
			set
			{
				if (this.DropDownStyle == ComboBoxStyle.DropDownList && !base.IsHandleCreated && !string.IsNullOrEmpty(value) && this.FindStringExact(value) == -1)
				{
					return;
				}
				base.Text = value;
				object selectedItem = this.SelectedItem;
				if (!base.DesignMode)
				{
					if (value == null)
					{
						this.SelectedIndex = -1;
						return;
					}
					if (value != null && (selectedItem == null || string.Compare(value, base.GetItemText(selectedItem), false, CultureInfo.CurrentCulture) != 0))
					{
						int num = this.FindStringIgnoreCase(value);
						if (num != -1)
						{
							this.SelectedIndex = num;
						}
					}
				}
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0004E054 File Offset: 0x0004D054
		private int FindStringIgnoreCase(string value)
		{
			int num = this.FindStringExact(value, -1, false);
			if (num == -1)
			{
				num = this.FindStringExact(value, -1, true);
			}
			return num;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0004E07A File Offset: 0x0004D07A
		private void NotifyAutoComplete()
		{
			this.NotifyAutoComplete(true);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0004E084 File Offset: 0x0004D084
		private void NotifyAutoComplete(bool setSelectedIndex)
		{
			string text = this.Text;
			bool flag = text != this.lastTextChangedValue;
			bool flag2 = false;
			if (setSelectedIndex)
			{
				int num = this.FindStringIgnoreCase(text);
				if (num != -1 && num != this.SelectedIndex)
				{
					this.SelectedIndex = num;
					this.SelectionStart = 0;
					this.SelectionLength = text.Length;
					flag2 = true;
				}
			}
			if (flag && !flag2)
			{
				this.OnTextChanged(EventArgs.Empty);
			}
			this.lastTextChangedValue = text;
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x0004E0F3 File Offset: 0x0004D0F3
		private bool SystemAutoCompleteEnabled
		{
			get
			{
				return this.autoCompleteMode != AutoCompleteMode.None && this.DropDownStyle != ComboBoxStyle.DropDownList;
			}
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x0600233B RID: 9019 RVA: 0x0004E10B File Offset: 0x0004D10B
		// (remove) Token: 0x0600233C RID: 9020 RVA: 0x0004E114 File Offset: 0x0004D114
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x0600233D RID: 9021 RVA: 0x0004E11D File Offset: 0x0004D11D
		// (remove) Token: 0x0600233E RID: 9022 RVA: 0x0004E130 File Offset: 0x0004D130
		[SRDescription("drawItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DRAWITEM, value);
			}
		}

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x0600233F RID: 9023 RVA: 0x0004E143 File Offset: 0x0004D143
		// (remove) Token: 0x06002340 RID: 9024 RVA: 0x0004E156 File Offset: 0x0004D156
		[SRDescription("ComboBoxOnDropDownDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler DropDown
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DROPDOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DROPDOWN, value);
			}
		}

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06002341 RID: 9025 RVA: 0x0004E169 File Offset: 0x0004D169
		// (remove) Token: 0x06002342 RID: 9026 RVA: 0x0004E182 File Offset: 0x0004D182
		[SRDescription("measureItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event MeasureItemEventHandler MeasureItem
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_MEASUREITEM, value);
				this.UpdateItemHeight();
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_MEASUREITEM, value);
				this.UpdateItemHeight();
			}
		}

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06002343 RID: 9027 RVA: 0x0004E19B File Offset: 0x0004D19B
		// (remove) Token: 0x06002344 RID: 9028 RVA: 0x0004E1AE File Offset: 0x0004D1AE
		[SRDescription("selectedIndexChangedEventDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
		}

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06002345 RID: 9029 RVA: 0x0004E1C1 File Offset: 0x0004D1C1
		// (remove) Token: 0x06002346 RID: 9030 RVA: 0x0004E1D4 File Offset: 0x0004D1D4
		[SRCategory("CatBehavior")]
		[SRDescription("selectionChangeCommittedEventDescr")]
		public event EventHandler SelectionChangeCommitted
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_SELECTIONCHANGECOMMITTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_SELECTIONCHANGECOMMITTED, value);
			}
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x06002347 RID: 9031 RVA: 0x0004E1E7 File Offset: 0x0004D1E7
		// (remove) Token: 0x06002348 RID: 9032 RVA: 0x0004E1FA File Offset: 0x0004D1FA
		[SRDescription("ComboBoxDropDownStyleChangedDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler DropDownStyleChanged
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DROPDOWNSTYLE, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DROPDOWNSTYLE, value);
			}
		}

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x06002349 RID: 9033 RVA: 0x0004E20D File Offset: 0x0004D20D
		// (remove) Token: 0x0600234A RID: 9034 RVA: 0x0004E216 File Offset: 0x0004D216
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

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x0600234B RID: 9035 RVA: 0x0004E21F File Offset: 0x0004D21F
		// (remove) Token: 0x0600234C RID: 9036 RVA: 0x0004E232 File Offset: 0x0004D232
		[SRDescription("ComboBoxOnTextUpdateDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler TextUpdate
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_TEXTUPDATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_TEXTUPDATE, value);
			}
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x0600234D RID: 9037 RVA: 0x0004E245 File Offset: 0x0004D245
		// (remove) Token: 0x0600234E RID: 9038 RVA: 0x0004E258 File Offset: 0x0004D258
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnDropDownClosedDescr")]
		public event EventHandler DropDownClosed
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DROPDOWNCLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DROPDOWNCLOSED, value);
			}
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0004E26C File Offset: 0x0004D26C
		[Obsolete("This method has been deprecated.  There is no replacement.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void AddItemsCore(object[] value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			this.BeginUpdate();
			try
			{
				this.Items.AddRangeInternal(value);
			}
			finally
			{
				this.EndUpdate();
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0004E2B4 File Offset: 0x0004D2B4
		public void BeginUpdate()
		{
			this.updateCount++;
			base.BeginUpdateInternal();
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0004E2CA File Offset: 0x0004D2CA
		private void CheckNoDataSource()
		{
			if (this.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0004E2E4 File Offset: 0x0004D2E4
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ComboBox.ComboBoxAccessibleObject(this);
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0004E2EC File Offset: 0x0004D2EC
		internal bool UpdateNeeded()
		{
			return this.updateCount == 0;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0004E2F8 File Offset: 0x0004D2F8
		internal Point EditToComboboxMapping(Message m)
		{
			if (this.childEdit == null)
			{
				return new Point(0, 0);
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
			NativeMethods.RECT rect2 = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.childEdit.Handle), ref rect2);
			int x = NativeMethods.Util.SignedLOWORD(m.LParam) + (rect2.left - rect.left);
			int y = NativeMethods.Util.SignedHIWORD(m.LParam) + (rect2.top - rect.top);
			return new Point(x, y);
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0004E394 File Offset: 0x0004D394
		private void ChildWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 81)
			{
				if (msg <= 32)
				{
					switch (msg)
					{
					case 7:
						if (!base.DesignMode)
						{
							ImeContext.SetImeStatus(base.CachedImeMode, m.HWnd);
						}
						if (!base.HostedInWin32DialogManager)
						{
							IContainerControl containerControlInternal = base.GetContainerControlInternal();
							if (containerControlInternal != null)
							{
								ContainerControl containerControl = containerControlInternal as ContainerControl;
								if (containerControl != null && !containerControl.ActivateControlInternal(this, false))
								{
									return;
								}
							}
						}
						this.DefChildWndProc(ref m);
						if (this.fireSetFocus)
						{
							this.OnGotFocus(EventArgs.Empty);
						}
						if (this.FlatStyle == FlatStyle.Popup)
						{
							base.Invalidate();
							return;
						}
						return;
					case 8:
						if (!base.DesignMode)
						{
							base.OnImeContextStatusChanged(m.HWnd);
						}
						this.DefChildWndProc(ref m);
						if (this.fireLostFocus)
						{
							this.OnLostFocus(EventArgs.Empty);
						}
						if (this.FlatStyle == FlatStyle.Popup)
						{
							base.Invalidate();
							return;
						}
						return;
					default:
						if (msg == 32)
						{
							if (this.Cursor != this.DefaultCursor && this.childEdit != null && m.HWnd == this.childEdit.Handle && NativeMethods.Util.LOWORD(m.LParam) == 1)
							{
								Cursor.CurrentInternal = this.Cursor;
								return;
							}
							this.DefChildWndProc(ref m);
							return;
						}
						break;
					}
				}
				else if (msg != 48)
				{
					if (msg == 81)
					{
						this.DefChildWndProc(ref m);
						return;
					}
				}
				else
				{
					this.DefChildWndProc(ref m);
					if (this.childEdit != null && m.HWnd == this.childEdit.Handle)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 211, 3, 0);
						return;
					}
					return;
				}
			}
			else if (msg <= 262)
			{
				if (msg != 123)
				{
					switch (msg)
					{
					case 256:
					case 260:
						if (this.SystemAutoCompleteEnabled && !ComboBox.ACNativeWindow.AutoCompleteActive)
						{
							this.finder.FindDropDowns(false);
						}
						if (this.AutoCompleteMode != AutoCompleteMode.None)
						{
							char c = (char)((long)m.WParam);
							if (c == '\u001b')
							{
								this.DroppedDown = false;
							}
							else if (c == '\r' && this.DroppedDown)
							{
								this.UpdateText();
								this.OnSelectionChangeCommittedInternal(EventArgs.Empty);
								this.DroppedDown = false;
							}
						}
						if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
						{
							this.DefChildWndProc(ref m);
							return;
						}
						if (base.PreProcessControlMessage(ref m) == PreProcessControlState.MessageProcessed)
						{
							return;
						}
						if (this.ProcessKeyMessage(ref m))
						{
							return;
						}
						this.DefChildWndProc(ref m);
						return;
					case 257:
					case 261:
						if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
						{
							this.DefChildWndProc(ref m);
						}
						else if (base.PreProcessControlMessage(ref m) != PreProcessControlState.MessageProcessed)
						{
							if (this.ProcessKeyMessage(ref m))
							{
								return;
							}
							this.DefChildWndProc(ref m);
						}
						if (this.SystemAutoCompleteEnabled && !ComboBox.ACNativeWindow.AutoCompleteActive)
						{
							this.finder.FindDropDowns();
							return;
						}
						return;
					case 258:
						if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
						{
							this.DefChildWndProc(ref m);
							return;
						}
						if (base.PreProcessControlMessage(ref m) == PreProcessControlState.MessageProcessed)
						{
							return;
						}
						if (this.ProcessKeyMessage(ref m))
						{
							return;
						}
						this.DefChildWndProc(ref m);
						return;
					case 262:
						if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
						{
							this.DefChildWndProc(ref m);
							return;
						}
						if (base.PreProcessControlMessage(ref m) == PreProcessControlState.MessageProcessed)
						{
							return;
						}
						if (this.ProcessKeyEventArgs(ref m))
						{
							return;
						}
						this.DefChildWndProc(ref m);
						return;
					}
				}
				else
				{
					if (this.ContextMenu != null || this.ContextMenuStrip != null)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 123, m.WParam, m.LParam);
						return;
					}
					this.DefChildWndProc(ref m);
					return;
				}
			}
			else
			{
				switch (msg)
				{
				case 512:
				{
					Point point = this.EditToComboboxMapping(m);
					this.DefChildWndProc(ref m);
					this.OnMouseEnterInternal(EventArgs.Empty);
					this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
					return;
				}
				case 513:
				{
					this.mousePressed = true;
					this.mouseEvents = true;
					base.CaptureInternal = true;
					this.DefChildWndProc(ref m);
					Point point2 = this.EditToComboboxMapping(m);
					this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, point2.X, point2.Y, 0));
					return;
				}
				case 514:
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
					Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
					int x = NativeMethods.Util.SignedLOWORD(m.LParam);
					int y = NativeMethods.Util.SignedHIWORD(m.LParam);
					Point point3 = new Point(x, y);
					point3 = base.PointToScreen(point3);
					if (this.mouseEvents && !base.ValidationCancelled)
					{
						this.mouseEvents = false;
						if (this.mousePressed)
						{
							if (rectangle.Contains(point3))
							{
								this.mousePressed = false;
								this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							}
							else
							{
								this.mousePressed = false;
								this.mouseInEdit = false;
								this.OnMouseLeave(EventArgs.Empty);
							}
						}
					}
					this.DefChildWndProc(ref m);
					base.CaptureInternal = false;
					point3 = this.EditToComboboxMapping(m);
					this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, point3.X, point3.Y, 0));
					return;
				}
				case 515:
				{
					this.mousePressed = true;
					this.mouseEvents = true;
					base.CaptureInternal = true;
					this.DefChildWndProc(ref m);
					Point point4 = this.EditToComboboxMapping(m);
					this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, point4.X, point4.Y, 0));
					return;
				}
				case 516:
				{
					this.mousePressed = true;
					this.mouseEvents = true;
					if (this.ContextMenu != null || this.ContextMenuStrip != null)
					{
						base.CaptureInternal = true;
					}
					this.DefChildWndProc(ref m);
					Point point5 = this.EditToComboboxMapping(m);
					this.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, point5.X, point5.Y, 0));
					return;
				}
				case 517:
				{
					this.mousePressed = false;
					this.mouseEvents = false;
					if (this.ContextMenu != null)
					{
						base.CaptureInternal = false;
					}
					this.DefChildWndProc(ref m);
					Point point6 = this.EditToComboboxMapping(m);
					this.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1, point6.X, point6.Y, 0));
					return;
				}
				case 518:
				{
					this.mousePressed = true;
					this.mouseEvents = true;
					base.CaptureInternal = true;
					this.DefChildWndProc(ref m);
					Point point7 = this.EditToComboboxMapping(m);
					this.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, point7.X, point7.Y, 0));
					return;
				}
				case 519:
				{
					this.mousePressed = true;
					this.mouseEvents = true;
					base.CaptureInternal = true;
					this.DefChildWndProc(ref m);
					Point point8 = this.EditToComboboxMapping(m);
					this.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, point8.X, point8.Y, 0));
					return;
				}
				case 520:
					this.mousePressed = false;
					this.mouseEvents = false;
					base.CaptureInternal = false;
					this.DefChildWndProc(ref m);
					this.OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					return;
				case 521:
				{
					this.mousePressed = true;
					this.mouseEvents = true;
					base.CaptureInternal = true;
					this.DefChildWndProc(ref m);
					Point point9 = this.EditToComboboxMapping(m);
					this.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, point9.X, point9.Y, 0));
					return;
				}
				default:
					if (msg == 675)
					{
						this.DefChildWndProc(ref m);
						this.OnMouseLeaveInternal(EventArgs.Empty);
						return;
					}
					break;
				}
			}
			this.DefChildWndProc(ref m);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0004EBBB File Offset: 0x0004DBBB
		private void OnMouseEnterInternal(EventArgs args)
		{
			if (!this.mouseInEdit)
			{
				this.OnMouseEnter(args);
				this.mouseInEdit = true;
			}
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0004EBD4 File Offset: 0x0004DBD4
		private void OnMouseLeaveInternal(EventArgs args)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
			Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
			Point mousePosition = Control.MousePosition;
			if (!rectangle.Contains(mousePosition))
			{
				this.OnMouseLeave(args);
				this.mouseInEdit = false;
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0004EC50 File Offset: 0x0004DC50
		private void DefChildWndProc(ref Message m)
		{
			if (this.childEdit != null)
			{
				NativeWindow nativeWindow = (m.HWnd == this.childEdit.Handle) ? this.childEdit : this.childListBox;
				if (nativeWindow != null)
				{
					nativeWindow.DefWndProc(ref m);
				}
			}
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0004EC98 File Offset: 0x0004DC98
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.autoCompleteCustomSource != null)
				{
					this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
				}
				if (this.stringSource != null)
				{
					this.stringSource.ReleaseAutoComplete();
					this.stringSource = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0004ECE8 File Offset: 0x0004DCE8
		public void EndUpdate()
		{
			this.updateCount--;
			if (this.updateCount == 0 && this.AutoCompleteSource == AutoCompleteSource.ListItems)
			{
				this.SetAutoComplete(false, false);
			}
			if (base.EndUpdateInternal())
			{
				if (this.childEdit != null && this.childEdit.Handle != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(this, this.childEdit.Handle), null, false);
				}
				if (this.childListBox != null && this.childListBox.Handle != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(this, this.childListBox.Handle), null, false);
				}
			}
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0004ED98 File Offset: 0x0004DD98
		public int FindString(string s)
		{
			return this.FindString(s, -1);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0004EDA4 File Offset: 0x0004DDA4
		public int FindString(string s, int startIndex)
		{
			if (s == null)
			{
				return -1;
			}
			if (this.itemsCollection == null || this.itemsCollection.Count == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= this.itemsCollection.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, false);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0004EDF9 File Offset: 0x0004DDF9
		public int FindStringExact(string s)
		{
			return this.FindStringExact(s, -1, true);
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0004EE04 File Offset: 0x0004DE04
		public int FindStringExact(string s, int startIndex)
		{
			return this.FindStringExact(s, startIndex, true);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0004EE10 File Offset: 0x0004DE10
		internal int FindStringExact(string s, int startIndex, bool ignorecase)
		{
			if (s == null)
			{
				return -1;
			}
			if (this.itemsCollection == null || this.itemsCollection.Count == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= this.itemsCollection.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, true, ignorecase);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0004EE66 File Offset: 0x0004DE66
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			if (this.DropDownStyle == ComboBoxStyle.DropDown || this.DropDownStyle == ComboBoxStyle.DropDownList)
			{
				proposedHeight = this.PreferredHeight;
			}
			return base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, proposedHeight);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0004EE8D File Offset: 0x0004DE8D
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			if (factor.Width != 1f && factor.Height != 1f)
			{
				this.ResetHeightCache();
			}
			base.ScaleControl(factor, specified);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0004EEBC File Offset: 0x0004DEBC
		public int GetItemHeight(int index)
		{
			if (this.DrawMode != DrawMode.OwnerDrawVariable)
			{
				return this.ItemHeight;
			}
			if (index < 0 || this.itemsCollection == null || index >= this.itemsCollection.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!base.IsHandleCreated)
			{
				return this.ItemHeight;
			}
			int num = (int)base.SendMessage(340, index, 0);
			if (num == -1)
			{
				throw new Win32Exception();
			}
			return num;
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0004EF54 File Offset: 0x0004DF54
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (msg == 312 && !this.ShouldSerializeBackColor())
			{
				return IntPtr.Zero;
			}
			if (msg == 308 && base.GetStyle(ControlStyles.UserPaint))
			{
				SafeNativeMethods.SetTextColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.ForeColor));
				SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.BackColor));
				return base.BackColorBrush;
			}
			return base.InitializeDCForWmCtlColor(dc, msg);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0004EFC8 File Offset: 0x0004DFC8
		private bool InterceptAutoCompleteKeystroke(Message m)
		{
			if (m.Msg == 256)
			{
				if ((int)m.WParam == 46)
				{
					this.MatchingText = "";
					this.autoCompleteTimeStamp = DateTime.Now.Ticks;
					if (this.Items.Count > 0)
					{
						this.SelectedIndex = 0;
					}
					return false;
				}
			}
			else if (m.Msg == 258)
			{
				char c = (char)((long)m.WParam);
				if (c == '\b')
				{
					if (DateTime.Now.Ticks - this.autoCompleteTimeStamp > 10000000L || this.MatchingText.Length <= 1)
					{
						this.MatchingText = "";
						if (this.Items.Count > 0)
						{
							this.SelectedIndex = 0;
						}
					}
					else
					{
						this.MatchingText = this.MatchingText.Remove(this.MatchingText.Length - 1);
						this.SelectedIndex = this.FindString(this.MatchingText);
					}
					this.autoCompleteTimeStamp = DateTime.Now.Ticks;
					return false;
				}
				if (c == '\u001b')
				{
					this.MatchingText = "";
				}
				if (c != '\u001b' && c != '\r' && !this.DroppedDown && this.AutoCompleteMode != AutoCompleteMode.Append)
				{
					this.DroppedDown = true;
				}
				string text;
				if (DateTime.Now.Ticks - this.autoCompleteTimeStamp > 10000000L)
				{
					text = new string(c, 1);
					if (this.FindString(text) != -1)
					{
						this.MatchingText = text;
					}
					this.autoCompleteTimeStamp = DateTime.Now.Ticks;
					return false;
				}
				text = this.MatchingText + c;
				int num = this.FindString(text);
				if (num != -1)
				{
					this.MatchingText = text;
					if (num != this.SelectedIndex)
					{
						this.SelectedIndex = num;
					}
				}
				this.autoCompleteTimeStamp = DateTime.Now.Ticks;
				return true;
			}
			return false;
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0004F1AE File Offset: 0x0004E1AE
		private void InvalidateEverything()
		{
			SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1157);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0004F1D0 File Offset: 0x0004E1D0
		protected override bool IsInputKey(Keys keyData)
		{
			Keys keys = keyData & (Keys.KeyCode | Keys.Alt);
			if (keys == Keys.Return || keys == Keys.Escape)
			{
				if (this.DroppedDown || this.autoCompleteDroppedDown)
				{
					return true;
				}
				if (this.SystemAutoCompleteEnabled && ComboBox.ACNativeWindow.AutoCompleteActive)
				{
					this.autoCompleteDroppedDown = true;
					return true;
				}
			}
			return base.IsInputKey(keyData);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0004F220 File Offset: 0x0004E220
		private int NativeAdd(object item)
		{
			int num = (int)base.SendMessage(323, 0, base.GetItemText(item));
			if (num < 0)
			{
				throw new OutOfMemoryException(SR.GetString("ComboBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0004F25C File Offset: 0x0004E25C
		private void NativeClear()
		{
			string text = null;
			if (this.DropDownStyle != ComboBoxStyle.DropDownList)
			{
				text = this.WindowText;
			}
			base.SendMessage(331, 0, 0);
			if (text != null)
			{
				this.WindowText = text;
			}
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0004F294 File Offset: 0x0004E294
		private string NativeGetItemText(int index)
		{
			int num = (int)base.SendMessage(329, index, 0);
			StringBuilder stringBuilder = new StringBuilder(num + 1);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 328, index, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x0004F2DC File Offset: 0x0004E2DC
		private int NativeInsert(int index, object item)
		{
			int num = (int)base.SendMessage(330, index, base.GetItemText(item));
			if (num < 0)
			{
				throw new OutOfMemoryException(SR.GetString("ComboBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0004F317 File Offset: 0x0004E317
		private void NativeRemoveAt(int index)
		{
			if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.SelectedIndex == index)
			{
				base.Invalidate();
			}
			base.SendMessage(324, index, 0);
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0004F340 File Offset: 0x0004E340
		internal override void RecreateHandleCore()
		{
			string windowText = this.WindowText;
			base.RecreateHandleCore();
			if (!string.IsNullOrEmpty(windowText) && string.IsNullOrEmpty(this.WindowText))
			{
				this.WindowText = windowText;
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0004F378 File Offset: 0x0004E378
		protected override void CreateHandle()
		{
			using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Bounds))
			{
				base.CreateHandle();
			}
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0004F3BC File Offset: 0x0004E3BC
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (this.MaxLength > 0)
			{
				base.SendMessage(321, this.MaxLength, 0);
			}
			bool flag = this.childEdit == null && this.childListBox == null;
			if (flag && this.DropDownStyle != ComboBoxStyle.DropDownList)
			{
				IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(this, base.Handle), 5);
				if (window != IntPtr.Zero)
				{
					if (this.DropDownStyle == ComboBoxStyle.Simple)
					{
						this.childListBox = new ComboBox.ComboBoxChildNativeWindow(this);
						this.childListBox.AssignHandle(window);
						window = UnsafeNativeMethods.GetWindow(new HandleRef(this, window), 2);
					}
					this.childEdit = new ComboBox.ComboBoxChildNativeWindow(this);
					this.childEdit.AssignHandle(window);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 211, 3, 0);
				}
			}
			bool flag2;
			int integer = base.Properties.GetInteger(ComboBox.PropDropDownWidth, out flag2);
			if (flag2)
			{
				base.SendMessage(352, integer, 0);
			}
			flag2 = false;
			base.Properties.GetInteger(ComboBox.PropItemHeight, out flag2);
			if (flag2)
			{
				this.UpdateItemHeight();
			}
			if (this.DropDownStyle == ComboBoxStyle.Simple)
			{
				base.Height = this.requestedHeight;
			}
			try
			{
				this.fromHandleCreate = true;
				this.SetAutoComplete(false, false);
			}
			finally
			{
				this.fromHandleCreate = false;
			}
			if (this.itemsCollection != null)
			{
				foreach (object item in this.itemsCollection)
				{
					this.NativeAdd(item);
				}
				if (this.selectedIndex >= 0)
				{
					base.SendMessage(334, this.selectedIndex, 0);
					this.UpdateText();
					this.selectedIndex = -1;
				}
			}
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0004F594 File Offset: 0x0004E594
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.dropDownHandle = IntPtr.Zero;
			if (base.Disposing)
			{
				this.itemsCollection = null;
				this.selectedIndex = -1;
			}
			else
			{
				this.selectedIndex = this.SelectedIndex;
			}
			if (this.stringSource != null)
			{
				this.stringSource.ReleaseAutoComplete();
				this.stringSource = null;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0004F5F4 File Offset: 0x0004E5F4
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			DrawItemEventHandler drawItemEventHandler = (DrawItemEventHandler)base.Events[ComboBox.EVENT_DRAWITEM];
			if (drawItemEventHandler != null)
			{
				drawItemEventHandler(this, e);
			}
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0004F624 File Offset: 0x0004E624
		protected virtual void OnDropDown(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_DROPDOWN];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0004F654 File Offset: 0x0004E654
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (this.SystemAutoCompleteEnabled)
			{
				if (e.KeyCode == Keys.Return)
				{
					this.NotifyAutoComplete(true);
				}
				else if (e.KeyCode == Keys.Escape && this.autoCompleteDroppedDown)
				{
					this.NotifyAutoComplete(false);
				}
				this.autoCompleteDroppedDown = false;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0004F6A4 File Offset: 0x0004E6A4
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (!e.Handled && (e.KeyChar == '\r' || e.KeyChar == '\u001b') && this.DroppedDown)
			{
				this.dropDown = false;
				if (base.FormattingEnabled)
				{
					this.Text = this.WindowText;
					this.SelectAll();
					e.Handled = false;
					return;
				}
				this.DroppedDown = false;
				e.Handled = true;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0004F714 File Offset: 0x0004E714
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			MeasureItemEventHandler measureItemEventHandler = (MeasureItemEventHandler)base.Events[ComboBox.EVENT_MEASUREITEM];
			if (measureItemEventHandler != null)
			{
				measureItemEventHandler(this, e);
			}
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0004F742 File Offset: 0x0004E742
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.MouseIsOver = true;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0004F752 File Offset: 0x0004E752
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.MouseIsOver = false;
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0004F764 File Offset: 0x0004E764
		private void OnSelectionChangeCommittedInternal(EventArgs e)
		{
			if (this.allowCommit)
			{
				try
				{
					this.allowCommit = false;
					this.OnSelectionChangeCommitted(e);
				}
				finally
				{
					this.allowCommit = true;
				}
			}
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0004F7A4 File Offset: 0x0004E7A4
		protected virtual void OnSelectionChangeCommitted(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_SELECTIONCHANGECOMMITTED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0004F7D4 File Offset: 0x0004E7D4
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_SELECTEDINDEXCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (base.DataManager != null && base.DataManager.Position != this.SelectedIndex && (!base.FormattingEnabled || this.SelectedIndex != -1))
			{
				base.DataManager.Position = this.SelectedIndex;
			}
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0004F846 File Offset: 0x0004E846
		protected override void OnSelectedValueChanged(EventArgs e)
		{
			base.OnSelectedValueChanged(e);
			this.selectedValueChangedFired = true;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0004F858 File Offset: 0x0004E858
		protected virtual void OnSelectedItemChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_SELECTEDITEMCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0004F888 File Offset: 0x0004E888
		protected virtual void OnDropDownStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_DROPDOWNSTYLE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0004F8B6 File Offset: 0x0004E8B6
		protected override void OnParentBackColorChanged(EventArgs e)
		{
			base.OnParentBackColorChanged(e);
			if (this.DropDownStyle == ComboBoxStyle.Simple)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0004F8CD File Offset: 0x0004E8CD
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.ResetHeightCache();
			if (this.AutoCompleteMode == AutoCompleteMode.None)
			{
				this.UpdateControl(true);
			}
			else
			{
				base.RecreateHandle();
			}
			CommonProperties.xClearPreferredSizeCache(this);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0004F8F9 File Offset: 0x0004E8F9
		private void OnAutoCompleteCustomSourceChanged(object sender, CollectionChangeEventArgs e)
		{
			if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
			{
				if (this.AutoCompleteCustomSource.Count == 0)
				{
					this.SetAutoComplete(true, true);
					return;
				}
				this.SetAutoComplete(true, false);
			}
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0004F923 File Offset: 0x0004E923
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.UpdateControl(false);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x0004F933 File Offset: 0x0004E933
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.UpdateControl(false);
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0004F943 File Offset: 0x0004E943
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnGotFocus(EventArgs e)
		{
			if (!this.canFireLostFocus)
			{
				base.OnGotFocus(e);
				this.canFireLostFocus = true;
			}
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0004F95C File Offset: 0x0004E95C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLostFocus(EventArgs e)
		{
			if (this.canFireLostFocus)
			{
				if (this.AutoCompleteMode != AutoCompleteMode.None && this.AutoCompleteSource == AutoCompleteSource.ListItems && this.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					this.MatchingText = "";
				}
				base.OnLostFocus(e);
				this.canFireLostFocus = false;
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0004F9A8 File Offset: 0x0004E9A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnTextChanged(EventArgs e)
		{
			if (this.SystemAutoCompleteEnabled)
			{
				string text = this.Text;
				if (text != this.lastTextChangedValue)
				{
					base.OnTextChanged(e);
					this.lastTextChangedValue = text;
					return;
				}
			}
			else
			{
				base.OnTextChanged(e);
			}
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0004F9E8 File Offset: 0x0004E9E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnValidating(CancelEventArgs e)
		{
			if (this.SystemAutoCompleteEnabled)
			{
				this.NotifyAutoComplete();
			}
			base.OnValidating(e);
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x0004F9FF File Offset: 0x0004E9FF
		private void UpdateControl(bool recreate)
		{
			this.ResetHeightCache();
			if (base.IsHandleCreated)
			{
				if (this.DropDownStyle == ComboBoxStyle.Simple && recreate)
				{
					base.RecreateHandle();
					return;
				}
				this.UpdateItemHeight();
				this.InvalidateEverything();
			}
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0004FA2D File Offset: 0x0004EA2D
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.DropDownStyle == ComboBoxStyle.Simple)
			{
				this.InvalidateEverything();
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0004FA44 File Offset: 0x0004EA44
		protected override void OnDataSourceChanged(EventArgs e)
		{
			if (this.Sorted && this.DataSource != null && base.Created)
			{
				this.DataSource = null;
				throw new InvalidOperationException(SR.GetString("ComboBoxDataSourceWithSort"));
			}
			if (this.DataSource == null)
			{
				this.BeginUpdate();
				this.SelectedIndex = -1;
				this.Items.ClearInternal();
				this.EndUpdate();
			}
			if (!this.Sorted && base.Created)
			{
				base.OnDataSourceChanged(e);
			}
			this.RefreshItems();
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0004FAC3 File Offset: 0x0004EAC3
		protected override void OnDisplayMemberChanged(EventArgs e)
		{
			base.OnDisplayMemberChanged(e);
			this.RefreshItems();
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0004FAD4 File Offset: 0x0004EAD4
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_DROPDOWNCLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0004FB04 File Offset: 0x0004EB04
		protected virtual void OnTextUpdate(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_TEXTUPDATE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0004FB32 File Offset: 0x0004EB32
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyEventArgs(ref Message m)
		{
			return (this.AutoCompleteMode != AutoCompleteMode.None && this.AutoCompleteSource == AutoCompleteSource.ListItems && this.DropDownStyle == ComboBoxStyle.DropDownList && this.InterceptAutoCompleteKeystroke(m)) || base.ProcessKeyEventArgs(ref m);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0004FB69 File Offset: 0x0004EB69
		private void ResetHeightCache()
		{
			this.prefHeightCache = -1;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0004FB74 File Offset: 0x0004EB74
		protected override void RefreshItems()
		{
			int num = this.SelectedIndex;
			ComboBox.ObjectCollection objectCollection = this.itemsCollection;
			this.itemsCollection = null;
			object[] array = null;
			if (base.DataManager != null && base.DataManager.Count != -1)
			{
				array = new object[base.DataManager.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = base.DataManager[i];
				}
			}
			else if (objectCollection != null)
			{
				array = new object[objectCollection.Count];
				objectCollection.CopyTo(array, 0);
			}
			this.BeginUpdate();
			try
			{
				if (base.IsHandleCreated)
				{
					this.NativeClear();
				}
				if (array != null)
				{
					this.Items.AddRangeInternal(array);
				}
				if (base.DataManager != null)
				{
					this.SelectedIndex = base.DataManager.Position;
				}
				else
				{
					this.SelectedIndex = num;
				}
			}
			finally
			{
				this.EndUpdate();
			}
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0004FC54 File Offset: 0x0004EC54
		protected override void RefreshItem(int index)
		{
			this.Items.SetItemInternal(index, this.Items[index]);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0004FC6E File Offset: 0x0004EC6E
		private void ReleaseChildWindow()
		{
			if (this.childEdit != null)
			{
				this.childEdit.ReleaseHandle();
				this.childEdit = null;
			}
			if (this.childListBox != null)
			{
				this.childListBox.ReleaseHandle();
				this.childListBox = null;
			}
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0004FCA4 File Offset: 0x0004ECA4
		private void ResetAutoCompleteCustomSource()
		{
			this.AutoCompleteCustomSource = null;
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0004FCAD File Offset: 0x0004ECAD
		private void ResetDropDownWidth()
		{
			base.Properties.RemoveInteger(ComboBox.PropDropDownWidth);
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0004FCBF File Offset: 0x0004ECBF
		private void ResetItemHeight()
		{
			base.Properties.RemoveInteger(ComboBox.PropItemHeight);
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0004FCD1 File Offset: 0x0004ECD1
		public override void ResetText()
		{
			base.ResetText();
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0004FCDC File Offset: 0x0004ECDC
		private void SetAutoComplete(bool reset, bool recreate)
		{
			if (!base.IsHandleCreated || this.childEdit == null)
			{
				return;
			}
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				if (!this.fromHandleCreate && recreate && base.IsHandleCreated)
				{
					AutoCompleteMode autoCompleteMode = this.AutoCompleteMode;
					this.autoCompleteMode = AutoCompleteMode.None;
					base.RecreateHandle();
					this.autoCompleteMode = autoCompleteMode;
				}
				if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
				{
					if (this.AutoCompleteCustomSource == null)
					{
						return;
					}
					if (this.AutoCompleteCustomSource.Count == 0)
					{
						int flags = -1610612736;
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags);
						return;
					}
					if (this.stringSource != null)
					{
						this.stringSource.RefreshList(this.GetStringsForAutoComplete(this.AutoCompleteCustomSource));
						return;
					}
					this.stringSource = new StringSource(this.GetStringsForAutoComplete(this.AutoCompleteCustomSource));
					if (!this.stringSource.Bind(new HandleRef(this, this.childEdit.Handle), (int)this.AutoCompleteMode))
					{
						throw new ArgumentException(SR.GetString("AutoCompleteFailure"));
					}
					return;
				}
				else if (this.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					if (this.DropDownStyle == ComboBoxStyle.DropDownList)
					{
						int flags2 = -1610612736;
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags2);
						return;
					}
					if (this.itemsCollection == null)
					{
						return;
					}
					if (this.itemsCollection.Count == 0)
					{
						int flags3 = -1610612736;
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags3);
						return;
					}
					if (this.stringSource != null)
					{
						this.stringSource.RefreshList(this.GetStringsForAutoComplete(this.Items));
						return;
					}
					this.stringSource = new StringSource(this.GetStringsForAutoComplete(this.Items));
					if (!this.stringSource.Bind(new HandleRef(this, this.childEdit.Handle), (int)this.AutoCompleteMode))
					{
						throw new ArgumentException(SR.GetString("AutoCompleteFailureListItems"));
					}
					return;
				}
				else
				{
					try
					{
						int num = 0;
						if (this.AutoCompleteMode == AutoCompleteMode.Suggest)
						{
							num |= -1879048192;
						}
						if (this.AutoCompleteMode == AutoCompleteMode.Append)
						{
							num |= 1610612736;
						}
						if (this.AutoCompleteMode == AutoCompleteMode.SuggestAppend)
						{
							num |= 268435456;
							num |= 1073741824;
						}
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), (int)(this.AutoCompleteSource | (AutoCompleteSource)num));
						return;
					}
					catch (SecurityException)
					{
						return;
					}
				}
			}
			if (reset)
			{
				int flags4 = -1610612736;
				SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags4);
			}
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0004FF64 File Offset: 0x0004EF64
		public void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidArgument", new object[]
				{
					"start",
					start.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = start + length;
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("length", SR.GetString("InvalidArgument", new object[]
				{
					"length",
					length.ToString(CultureInfo.CurrentCulture)
				}));
			}
			base.SendMessage(322, 0, NativeMethods.Util.MAKELPARAM(start, num));
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0004FFF9 File Offset: 0x0004EFF9
		public void SelectAll()
		{
			this.Select(0, int.MaxValue);
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x00050007 File Offset: 0x0004F007
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				this.requestedHeight = height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x00050024 File Offset: 0x0004F024
		protected override void SetItemsCore(IList value)
		{
			this.BeginUpdate();
			this.Items.ClearInternal();
			this.Items.AddRangeInternal(value);
			if (base.DataManager != null)
			{
				if (this.DataSource is ICurrencyManagerProvider)
				{
					this.selectedValueChangedFired = false;
				}
				if (base.IsHandleCreated)
				{
					base.SendMessage(334, base.DataManager.Position, 0);
				}
				else
				{
					this.selectedIndex = base.DataManager.Position;
				}
				if (!this.selectedValueChangedFired)
				{
					this.OnSelectedValueChanged(EventArgs.Empty);
					this.selectedValueChangedFired = false;
				}
			}
			this.EndUpdate();
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000500BD File Offset: 0x0004F0BD
		protected override void SetItemCore(int index, object value)
		{
			this.Items.SetItemInternal(index, value);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000500CC File Offset: 0x0004F0CC
		private bool ShouldSerializeAutoCompleteCustomSource()
		{
			return this.autoCompleteCustomSource != null && this.autoCompleteCustomSource.Count > 0;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000500E6 File Offset: 0x0004F0E6
		internal bool ShouldSerializeDropDownWidth()
		{
			return base.Properties.ContainsInteger(ComboBox.PropDropDownWidth);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000500F8 File Offset: 0x0004F0F8
		internal bool ShouldSerializeItemHeight()
		{
			return base.Properties.ContainsInteger(ComboBox.PropItemHeight);
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0005010A File Offset: 0x0004F10A
		internal override bool ShouldSerializeText()
		{
			return this.SelectedIndex == -1 && base.ShouldSerializeText();
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x00050120 File Offset: 0x0004F120
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Items.Count: " + ((this.itemsCollection == null) ? 0.ToString(CultureInfo.CurrentCulture) : this.itemsCollection.Count.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x00050170 File Offset: 0x0004F170
		private void UpdateDropDownHeight()
		{
			if (this.dropDownHandle != IntPtr.Zero)
			{
				int num = this.DropDownHeight;
				if (num == 106)
				{
					int val = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
					int num2 = Math.Min(Math.Max(val, 1), (int)this.maxDropDownItems);
					num = this.ItemHeight * num2 + 2;
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.dropDownHandle), NativeMethods.NullHandleRef, 0, 0, this.DropDownWidth, num, 6);
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000501F4 File Offset: 0x0004F1F4
		private void UpdateItemHeight()
		{
			if (!base.IsHandleCreated)
			{
				base.CreateControl();
			}
			if (this.DrawMode == DrawMode.OwnerDrawFixed)
			{
				base.SendMessage(339, -1, this.ItemHeight);
				base.SendMessage(339, 0, this.ItemHeight);
				return;
			}
			if (this.DrawMode == DrawMode.OwnerDrawVariable)
			{
				base.SendMessage(339, -1, this.ItemHeight);
				Graphics graphics = base.CreateGraphicsInternal();
				for (int i = 0; i < this.Items.Count; i++)
				{
					int num = (int)base.SendMessage(340, i, 0);
					MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, i, num);
					this.OnMeasureItem(measureItemEventArgs);
					if (measureItemEventArgs.ItemHeight != num)
					{
						base.SendMessage(339, i, measureItemEventArgs.ItemHeight);
					}
				}
				graphics.Dispose();
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000502C0 File Offset: 0x0004F2C0
		private void UpdateText()
		{
			string text = null;
			if (this.SelectedIndex != -1)
			{
				object obj = this.Items[this.SelectedIndex];
				if (obj != null)
				{
					text = base.GetItemText(obj);
				}
			}
			this.Text = text;
			if (this.DropDownStyle == ComboBoxStyle.DropDown && this.childEdit != null && this.childEdit.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 12, IntPtr.Zero, text);
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x00050344 File Offset: 0x0004F344
		private void WmEraseBkgnd(ref Message m)
		{
			if (this.DropDownStyle == ComboBoxStyle.Simple && this.ParentInternal != null)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
				Control parentInternal = this.ParentInternal;
				Graphics graphics = Graphics.FromHdcInternal(m.WParam);
				if (parentInternal != null)
				{
					Brush brush = new SolidBrush(parentInternal.BackColor);
					graphics.FillRectangle(brush, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
					brush.Dispose();
				}
				else
				{
					graphics.FillRectangle(SystemBrushes.Control, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
				}
				graphics.Dispose();
				m.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x00050431 File Offset: 0x0004F431
		private void WmParentNotify(ref Message m)
		{
			base.WndProc(ref m);
			if ((int)((long)m.WParam) == 65536001)
			{
				this.dropDownHandle = m.LParam;
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0005045C File Offset: 0x0004F45C
		private void WmReflectCommand(ref Message m)
		{
			switch (NativeMethods.Util.HIWORD(m.WParam))
			{
			case 1:
				this.UpdateText();
				this.OnSelectedIndexChanged(EventArgs.Empty);
				return;
			case 2:
			case 3:
			case 4:
				break;
			case 5:
				this.OnTextChanged(EventArgs.Empty);
				return;
			case 6:
				this.OnTextUpdate(EventArgs.Empty);
				return;
			case 7:
				this.currentText = this.Text;
				this.dropDown = true;
				this.OnDropDown(EventArgs.Empty);
				this.UpdateDropDownHeight();
				return;
			case 8:
				this.OnDropDownClosed(EventArgs.Empty);
				if (base.FormattingEnabled && this.Text != this.currentText && this.dropDown)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
				this.dropDown = false;
				return;
			case 9:
				this.OnSelectionChangeCommittedInternal(EventArgs.Empty);
				break;
			default:
				return;
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x00050540 File Offset: 0x0004F540
		private void WmReflectDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr intPtr = Control.SetUpPalette(drawitemstruct.hDC, false, false);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC);
				try
				{
					this.OnDrawItem(new DrawItemEventArgs(graphics, this.Font, Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom), drawitemstruct.itemID, (DrawItemState)drawitemstruct.itemState, this.ForeColor, this.BackColor));
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
					SafeNativeMethods.SelectPalette(new HandleRef(this, drawitemstruct.hDC), new HandleRef(null, intPtr), 0);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x00050634 File Offset: 0x0004F634
		private void WmReflectMeasureItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			if (this.DrawMode == DrawMode.OwnerDrawVariable && measureitemstruct.itemID >= 0)
			{
				Graphics graphics = base.CreateGraphicsInternal();
				MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, measureitemstruct.itemID, this.ItemHeight);
				this.OnMeasureItem(measureItemEventArgs);
				measureitemstruct.itemHeight = measureItemEventArgs.ItemHeight;
				graphics.Dispose();
			}
			else
			{
				measureitemstruct.itemHeight = this.ItemHeight;
			}
			Marshal.StructureToPtr(measureitemstruct, m.LParam, false);
			m.Result = (IntPtr)1;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000506C4 File Offset: 0x0004F6C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 130)
			{
				if (msg <= 20)
				{
					switch (msg)
					{
					case 7:
						try
						{
							this.fireSetFocus = false;
							base.WndProc(ref m);
							return;
						}
						finally
						{
							this.fireSetFocus = true;
						}
						break;
					case 8:
						break;
					default:
						if (msg == 15)
						{
							if (!base.GetStyle(ControlStyles.UserPaint) && (this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup))
							{
								using (WindowsRegion windowsRegion = new WindowsRegion(this.FlatComboBoxAdapter.dropDownRect))
								{
									using (WindowsRegion windowsRegion2 = new WindowsRegion(base.Bounds))
									{
										NativeMethods.RegionFlags updateRgn = (NativeMethods.RegionFlags)SafeNativeMethods.GetUpdateRgn(new HandleRef(this, base.Handle), new HandleRef(this, windowsRegion2.HRegion), true);
										windowsRegion.CombineRegion(windowsRegion2, windowsRegion, RegionCombineMode.DIFF);
										Rectangle updateRegionBox = windowsRegion2.ToRectangle();
										this.FlatComboBoxAdapter.ValidateOwnerDrawRegions(this, updateRegionBox);
										NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
										bool flag = false;
										IntPtr intPtr;
										if (m.WParam == IntPtr.Zero)
										{
											intPtr = UnsafeNativeMethods.BeginPaint(new HandleRef(this, base.Handle), ref paintstruct);
											flag = true;
										}
										else
										{
											intPtr = m.WParam;
										}
										using (DeviceContext deviceContext = DeviceContext.FromHdc(intPtr))
										{
											using (WindowsGraphics windowsGraphics = new WindowsGraphics(deviceContext))
											{
												if (updateRgn != NativeMethods.RegionFlags.ERROR)
												{
													windowsGraphics.DeviceContext.SetClip(windowsRegion);
												}
												m.WParam = intPtr;
												this.DefWndProc(ref m);
												if (updateRgn != NativeMethods.RegionFlags.ERROR)
												{
													windowsGraphics.DeviceContext.SetClip(windowsRegion2);
												}
												using (Graphics graphics = Graphics.FromHdcInternal(intPtr))
												{
													this.FlatComboBoxAdapter.DrawFlatCombo(this, graphics);
												}
											}
										}
										if (flag)
										{
											UnsafeNativeMethods.EndPaint(new HandleRef(this, base.Handle), ref paintstruct);
										}
									}
									return;
								}
							}
							base.WndProc(ref m);
							return;
						}
						if (msg != 20)
						{
							goto IL_53A;
						}
						this.WmEraseBkgnd(ref m);
						return;
					}
					try
					{
						this.fireLostFocus = false;
						base.WndProc(ref m);
						if (!Application.RenderWithVisualStyles && !base.GetStyle(ControlStyles.UserPaint) && this.DropDownStyle == ComboBoxStyle.DropDownList && (this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup))
						{
							UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 675, 0, 0);
						}
						return;
					}
					finally
					{
						this.fireLostFocus = true;
					}
				}
				else if (msg <= 48)
				{
					if (msg == 32)
					{
						base.WndProc(ref m);
						return;
					}
					if (msg != 48)
					{
						goto IL_53A;
					}
					if (base.Width == 0)
					{
						this.suppressNextWindosPos = true;
					}
					base.WndProc(ref m);
					return;
				}
				else
				{
					if (msg == 71)
					{
						if (!this.suppressNextWindosPos)
						{
							base.WndProc(ref m);
						}
						this.suppressNextWindosPos = false;
						return;
					}
					if (msg != 130)
					{
						goto IL_53A;
					}
					base.WndProc(ref m);
					this.ReleaseChildWindow();
					return;
				}
			}
			else if (msg <= 528)
			{
				switch (msg)
				{
				case 307:
				case 308:
					break;
				default:
					switch (msg)
					{
					case 513:
						this.mouseEvents = true;
						base.WndProc(ref m);
						return;
					case 514:
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
						Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
						int x = NativeMethods.Util.SignedLOWORD(m.LParam);
						int y = NativeMethods.Util.SignedHIWORD(m.LParam);
						Point point = new Point(x, y);
						point = base.PointToScreen(point);
						if (this.mouseEvents && !base.ValidationCancelled)
						{
							this.mouseEvents = false;
							bool capture = base.Capture;
							if (capture && rectangle.Contains(point))
							{
								this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							}
							base.WndProc(ref m);
							return;
						}
						base.CaptureInternal = false;
						this.DefWndProc(ref m);
						return;
					}
					default:
						if (msg != 528)
						{
							goto IL_53A;
						}
						this.WmParentNotify(ref m);
						return;
					}
					break;
				}
			}
			else if (msg <= 792)
			{
				if (msg == 675)
				{
					this.DefWndProc(ref m);
					this.OnMouseLeaveInternal(EventArgs.Empty);
					return;
				}
				if (msg != 792)
				{
					goto IL_53A;
				}
				if ((!base.GetStyle(ControlStyles.UserPaint) && this.FlatStyle == FlatStyle.Flat) || this.FlatStyle == FlatStyle.Popup)
				{
					this.DefWndProc(ref m);
					if (((int)m.LParam & 4) == 4)
					{
						if ((!base.GetStyle(ControlStyles.UserPaint) && this.FlatStyle == FlatStyle.Flat) || this.FlatStyle == FlatStyle.Popup)
						{
							using (Graphics graphics2 = Graphics.FromHdcInternal(m.WParam))
							{
								this.FlatComboBoxAdapter.DrawFlatCombo(this, graphics2);
							}
							return;
						}
						return;
					}
				}
				base.WndProc(ref m);
				return;
			}
			else
			{
				switch (msg)
				{
				case 8235:
					this.WmReflectDrawItem(ref m);
					return;
				case 8236:
					this.WmReflectMeasureItem(ref m);
					return;
				default:
					if (msg != 8465)
					{
						goto IL_53A;
					}
					this.WmReflectCommand(ref m);
					return;
				}
			}
			m.Result = this.InitializeDCForWmCtlColor(m.WParam, m.Msg);
			return;
			IL_53A:
			if (m.Msg == NativeMethods.WM_MOUSEENTER)
			{
				this.DefWndProc(ref m);
				this.OnMouseEnterInternal(EventArgs.Empty);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060023A9 RID: 9129 RVA: 0x00050CF8 File Offset: 0x0004FCF8
		private ComboBox.FlatComboAdapter FlatComboBoxAdapter
		{
			get
			{
				ComboBox.FlatComboAdapter flatComboAdapter = base.Properties.GetObject(ComboBox.PropFlatComboAdapter) as ComboBox.FlatComboAdapter;
				if (flatComboAdapter == null || !flatComboAdapter.IsValid(this))
				{
					flatComboAdapter = this.CreateFlatComboAdapterInstance();
					base.Properties.SetObject(ComboBox.PropFlatComboAdapter, flatComboAdapter);
				}
				return flatComboAdapter;
			}
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x00050D40 File Offset: 0x0004FD40
		internal virtual ComboBox.FlatComboAdapter CreateFlatComboAdapterInstance()
		{
			return new ComboBox.FlatComboAdapter(this, false);
		}

		// Token: 0x0400153D RID: 5437
		private const int DefaultSimpleStyleHeight = 150;

		// Token: 0x0400153E RID: 5438
		private const int DefaultDropDownHeight = 106;

		// Token: 0x0400153F RID: 5439
		private const int AutoCompleteTimeout = 10000000;

		// Token: 0x04001540 RID: 5440
		private static readonly object EVENT_DROPDOWN = new object();

		// Token: 0x04001541 RID: 5441
		private static readonly object EVENT_DRAWITEM = new object();

		// Token: 0x04001542 RID: 5442
		private static readonly object EVENT_MEASUREITEM = new object();

		// Token: 0x04001543 RID: 5443
		private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

		// Token: 0x04001544 RID: 5444
		private static readonly object EVENT_SELECTIONCHANGECOMMITTED = new object();

		// Token: 0x04001545 RID: 5445
		private static readonly object EVENT_SELECTEDITEMCHANGED = new object();

		// Token: 0x04001546 RID: 5446
		private static readonly object EVENT_DROPDOWNSTYLE = new object();

		// Token: 0x04001547 RID: 5447
		private static readonly object EVENT_TEXTUPDATE = new object();

		// Token: 0x04001548 RID: 5448
		private static readonly object EVENT_DROPDOWNCLOSED = new object();

		// Token: 0x04001549 RID: 5449
		private static readonly int PropMaxLength = PropertyStore.CreateKey();

		// Token: 0x0400154A RID: 5450
		private static readonly int PropItemHeight = PropertyStore.CreateKey();

		// Token: 0x0400154B RID: 5451
		private static readonly int PropDropDownWidth = PropertyStore.CreateKey();

		// Token: 0x0400154C RID: 5452
		private static readonly int PropDropDownHeight = PropertyStore.CreateKey();

		// Token: 0x0400154D RID: 5453
		private static readonly int PropStyle = PropertyStore.CreateKey();

		// Token: 0x0400154E RID: 5454
		private static readonly int PropDrawMode = PropertyStore.CreateKey();

		// Token: 0x0400154F RID: 5455
		private static readonly int PropMatchingText = PropertyStore.CreateKey();

		// Token: 0x04001550 RID: 5456
		private static readonly int PropFlatComboAdapter = PropertyStore.CreateKey();

		// Token: 0x04001551 RID: 5457
		private bool autoCompleteDroppedDown;

		// Token: 0x04001552 RID: 5458
		private FlatStyle flatStyle = FlatStyle.Standard;

		// Token: 0x04001553 RID: 5459
		private int updateCount;

		// Token: 0x04001554 RID: 5460
		private long autoCompleteTimeStamp;

		// Token: 0x04001555 RID: 5461
		private int selectedIndex = -1;

		// Token: 0x04001556 RID: 5462
		private bool allowCommit = true;

		// Token: 0x04001557 RID: 5463
		private int requestedHeight;

		// Token: 0x04001558 RID: 5464
		private ComboBox.ComboBoxChildNativeWindow childEdit;

		// Token: 0x04001559 RID: 5465
		private ComboBox.ComboBoxChildNativeWindow childListBox;

		// Token: 0x0400155A RID: 5466
		private IntPtr dropDownHandle;

		// Token: 0x0400155B RID: 5467
		private ComboBox.ObjectCollection itemsCollection;

		// Token: 0x0400155C RID: 5468
		private short prefHeightCache = -1;

		// Token: 0x0400155D RID: 5469
		private short maxDropDownItems = 8;

		// Token: 0x0400155E RID: 5470
		private bool integralHeight = true;

		// Token: 0x0400155F RID: 5471
		private bool mousePressed;

		// Token: 0x04001560 RID: 5472
		private bool mouseEvents;

		// Token: 0x04001561 RID: 5473
		private bool mouseInEdit;

		// Token: 0x04001562 RID: 5474
		private bool sorted;

		// Token: 0x04001563 RID: 5475
		private bool fireSetFocus = true;

		// Token: 0x04001564 RID: 5476
		private bool fireLostFocus = true;

		// Token: 0x04001565 RID: 5477
		private bool mouseOver;

		// Token: 0x04001566 RID: 5478
		private bool suppressNextWindosPos;

		// Token: 0x04001567 RID: 5479
		private bool canFireLostFocus;

		// Token: 0x04001568 RID: 5480
		private string currentText = "";

		// Token: 0x04001569 RID: 5481
		private string lastTextChangedValue;

		// Token: 0x0400156A RID: 5482
		private bool dropDown;

		// Token: 0x0400156B RID: 5483
		private ComboBox.AutoCompleteDropDownFinder finder = new ComboBox.AutoCompleteDropDownFinder();

		// Token: 0x0400156C RID: 5484
		private bool selectedValueChangedFired;

		// Token: 0x0400156D RID: 5485
		private AutoCompleteMode autoCompleteMode;

		// Token: 0x0400156E RID: 5486
		private AutoCompleteSource autoCompleteSource = AutoCompleteSource.None;

		// Token: 0x0400156F RID: 5487
		private AutoCompleteStringCollection autoCompleteCustomSource;

		// Token: 0x04001570 RID: 5488
		private StringSource stringSource;

		// Token: 0x04001571 RID: 5489
		private bool fromHandleCreate;

		// Token: 0x02000291 RID: 657
		private class ComboBoxChildNativeWindow : NativeWindow
		{
			// Token: 0x060023AC RID: 9132 RVA: 0x00050E03 File Offset: 0x0004FE03
			internal ComboBoxChildNativeWindow(ComboBox comboBox)
			{
				this._owner = comboBox;
			}

			// Token: 0x060023AD RID: 9133 RVA: 0x00050E14 File Offset: 0x0004FE14
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 61)
				{
					this.WmGetObject(ref m);
					return;
				}
				this._owner.ChildWndProc(ref m);
			}

			// Token: 0x060023AE RID: 9134 RVA: 0x00050E44 File Offset: 0x0004FE44
			private void WmGetObject(ref Message m)
			{
				if (-4 == (int)((long)m.LParam))
				{
					Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
					try
					{
						if (this._accessibilityObject == null)
						{
							IntSecurity.UnmanagedCode.Assert();
							try
							{
								AccessibleObject accessibleImplemention = new ComboBox.ChildAccessibleObject(this._owner, base.Handle);
								this._accessibilityObject = new InternalAccessibleObject(accessibleImplemention);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
						UnsafeNativeMethods.IAccessibleInternal accessibilityObject = this._accessibilityObject;
						IntPtr iunknownForObject = Marshal.GetIUnknownForObject(accessibilityObject);
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							m.Result = UnsafeNativeMethods.LresultFromObject(ref guid, m.WParam, new HandleRef(this, iunknownForObject));
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							Marshal.Release(iunknownForObject);
						}
						return;
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(SR.GetString("RichControlLresult"), innerException);
					}
				}
				base.DefWndProc(ref m);
			}

			// Token: 0x04001572 RID: 5490
			private ComboBox _owner;

			// Token: 0x04001573 RID: 5491
			private InternalAccessibleObject _accessibilityObject;
		}

		// Token: 0x02000292 RID: 658
		private sealed class ItemComparer : IComparer
		{
			// Token: 0x060023AF RID: 9135 RVA: 0x00050F38 File Offset: 0x0004FF38
			public ItemComparer(ComboBox comboBox)
			{
				this.comboBox = comboBox;
			}

			// Token: 0x060023B0 RID: 9136 RVA: 0x00050F48 File Offset: 0x0004FF48
			public int Compare(object item1, object item2)
			{
				if (item1 == null)
				{
					if (item2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (item2 == null)
					{
						return 1;
					}
					string itemText = this.comboBox.GetItemText(item1);
					string itemText2 = this.comboBox.GetItemText(item2);
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					return compareInfo.Compare(itemText, itemText2, CompareOptions.StringSort);
				}
			}

			// Token: 0x04001574 RID: 5492
			private ComboBox comboBox;
		}

		// Token: 0x02000293 RID: 659
		[ListBindable(false)]
		public class ObjectCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060023B1 RID: 9137 RVA: 0x00050F96 File Offset: 0x0004FF96
			public ObjectCollection(ComboBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000572 RID: 1394
			// (get) Token: 0x060023B2 RID: 9138 RVA: 0x00050FA5 File Offset: 0x0004FFA5
			private IComparer Comparer
			{
				get
				{
					if (this.comparer == null)
					{
						this.comparer = new ComboBox.ItemComparer(this.owner);
					}
					return this.comparer;
				}
			}

			// Token: 0x17000573 RID: 1395
			// (get) Token: 0x060023B3 RID: 9139 RVA: 0x00050FC6 File Offset: 0x0004FFC6
			private ArrayList InnerList
			{
				get
				{
					if (this.innerList == null)
					{
						this.innerList = new ArrayList();
					}
					return this.innerList;
				}
			}

			// Token: 0x17000574 RID: 1396
			// (get) Token: 0x060023B4 RID: 9140 RVA: 0x00050FE1 File Offset: 0x0004FFE1
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			// Token: 0x17000575 RID: 1397
			// (get) Token: 0x060023B5 RID: 9141 RVA: 0x00050FEE File Offset: 0x0004FFEE
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000576 RID: 1398
			// (get) Token: 0x060023B6 RID: 9142 RVA: 0x00050FF1 File Offset: 0x0004FFF1
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000577 RID: 1399
			// (get) Token: 0x060023B7 RID: 9143 RVA: 0x00050FF4 File Offset: 0x0004FFF4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000578 RID: 1400
			// (get) Token: 0x060023B8 RID: 9144 RVA: 0x00050FF7 File Offset: 0x0004FFF7
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060023B9 RID: 9145 RVA: 0x00050FFC File Offset: 0x0004FFFC
			public int Add(object item)
			{
				this.owner.CheckNoDataSource();
				int result = this.AddInternal(item);
				if (this.owner.UpdateNeeded() && this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, false);
				}
				return result;
			}

			// Token: 0x060023BA RID: 9146 RVA: 0x0005104C File Offset: 0x0005004C
			private int AddInternal(object item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				int num = -1;
				if (!this.owner.sorted)
				{
					this.InnerList.Add(item);
				}
				else
				{
					num = this.InnerList.BinarySearch(item, this.Comparer);
					if (num < 0)
					{
						num = ~num;
					}
					this.InnerList.Insert(num, item);
				}
				bool flag = false;
				try
				{
					if (this.owner.sorted)
					{
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeInsert(num, item);
						}
					}
					else
					{
						num = this.InnerList.Count - 1;
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeAdd(item);
						}
					}
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.InnerList.Remove(item);
					}
				}
				return num;
			}

			// Token: 0x060023BB RID: 9147 RVA: 0x00051128 File Offset: 0x00050128
			int IList.Add(object item)
			{
				return this.Add(item);
			}

			// Token: 0x060023BC RID: 9148 RVA: 0x00051134 File Offset: 0x00050134
			public void AddRange(object[] items)
			{
				this.owner.CheckNoDataSource();
				this.owner.BeginUpdate();
				try
				{
					this.AddRangeInternal(items);
				}
				finally
				{
					this.owner.EndUpdate();
				}
			}

			// Token: 0x060023BD RID: 9149 RVA: 0x0005117C File Offset: 0x0005017C
			internal void AddRangeInternal(IList items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				foreach (object item in items)
				{
					this.AddInternal(item);
				}
				if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, false);
				}
			}

			// Token: 0x17000579 RID: 1401
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			public virtual object this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerList.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerList[index];
				}
				set
				{
					this.owner.CheckNoDataSource();
					this.SetItemInternal(index, value);
				}
			}

			// Token: 0x060023C0 RID: 9152 RVA: 0x00051270 File Offset: 0x00050270
			public void Clear()
			{
				this.owner.CheckNoDataSource();
				this.ClearInternal();
			}

			// Token: 0x060023C1 RID: 9153 RVA: 0x00051284 File Offset: 0x00050284
			internal void ClearInternal()
			{
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeClear();
				}
				this.InnerList.Clear();
				this.owner.selectedIndex = -1;
				if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, true);
				}
			}

			// Token: 0x060023C2 RID: 9154 RVA: 0x000512DF File Offset: 0x000502DF
			public bool Contains(object value)
			{
				return this.IndexOf(value) != -1;
			}

			// Token: 0x060023C3 RID: 9155 RVA: 0x000512EE File Offset: 0x000502EE
			public void CopyTo(object[] destination, int arrayIndex)
			{
				this.InnerList.CopyTo(destination, arrayIndex);
			}

			// Token: 0x060023C4 RID: 9156 RVA: 0x000512FD File Offset: 0x000502FD
			void ICollection.CopyTo(Array destination, int index)
			{
				this.InnerList.CopyTo(destination, index);
			}

			// Token: 0x060023C5 RID: 9157 RVA: 0x0005130C File Offset: 0x0005030C
			public IEnumerator GetEnumerator()
			{
				return this.InnerList.GetEnumerator();
			}

			// Token: 0x060023C6 RID: 9158 RVA: 0x00051319 File Offset: 0x00050319
			public int IndexOf(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerList.IndexOf(value);
			}

			// Token: 0x060023C7 RID: 9159 RVA: 0x00051338 File Offset: 0x00050338
			public void Insert(int index, object item)
			{
				this.owner.CheckNoDataSource();
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (index < 0 || index > this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.sorted)
				{
					this.Add(item);
					return;
				}
				this.InnerList.Insert(index, item);
				if (this.owner.IsHandleCreated)
				{
					bool flag = false;
					try
					{
						this.owner.NativeInsert(index, item);
						flag = true;
					}
					finally
					{
						if (flag)
						{
							if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
							{
								this.owner.SetAutoComplete(false, false);
							}
						}
						else
						{
							this.InnerList.RemoveAt(index);
						}
					}
				}
			}

			// Token: 0x060023C8 RID: 9160 RVA: 0x0005142C File Offset: 0x0005042C
			public void RemoveAt(int index)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeRemoveAt(index);
				}
				this.InnerList.RemoveAt(index);
				if (!this.owner.IsHandleCreated && index < this.owner.selectedIndex)
				{
					this.owner.selectedIndex--;
				}
				if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, false);
				}
			}

			// Token: 0x060023C9 RID: 9161 RVA: 0x000514FC File Offset: 0x000504FC
			public void Remove(object value)
			{
				int num = this.InnerList.IndexOf(value);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x060023CA RID: 9162 RVA: 0x00051524 File Offset: 0x00050524
			internal void SetItemInternal(int index, object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerList[index] = value;
				if (this.owner.IsHandleCreated)
				{
					bool flag = index == this.owner.SelectedIndex;
					if (string.Compare(this.owner.GetItemText(value), this.owner.NativeGetItemText(index), true, CultureInfo.CurrentCulture) != 0)
					{
						this.owner.NativeRemoveAt(index);
						this.owner.NativeInsert(index, value);
						if (flag)
						{
							this.owner.SelectedIndex = index;
							this.owner.UpdateText();
						}
						if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
						{
							this.owner.SetAutoComplete(false, false);
							return;
						}
					}
					else if (flag)
					{
						this.owner.OnSelectedItemChanged(EventArgs.Empty);
						this.owner.OnSelectedIndexChanged(EventArgs.Empty);
					}
				}
			}

			// Token: 0x04001575 RID: 5493
			private ComboBox owner;

			// Token: 0x04001576 RID: 5494
			private ArrayList innerList;

			// Token: 0x04001577 RID: 5495
			private IComparer comparer;
		}

		// Token: 0x02000294 RID: 660
		[ComVisible(true)]
		public class ChildAccessibleObject : AccessibleObject
		{
			// Token: 0x060023CB RID: 9163 RVA: 0x0005164D File Offset: 0x0005064D
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public ChildAccessibleObject(ComboBox owner, IntPtr handle)
			{
				this.owner = owner;
				base.UseStdAccessibleObjects(handle);
			}

			// Token: 0x1700057A RID: 1402
			// (get) Token: 0x060023CC RID: 9164 RVA: 0x00051663 File Offset: 0x00050663
			public override string Name
			{
				get
				{
					return this.owner.AccessibilityObject.Name;
				}
			}

			// Token: 0x04001578 RID: 5496
			private ComboBox owner;
		}

		// Token: 0x02000295 RID: 661
		[ComVisible(true)]
		internal class ComboBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x060023CD RID: 9165 RVA: 0x00051675 File Offset: 0x00050675
			public ComboBoxAccessibleObject(Control ownerControl) : base(ownerControl)
			{
			}

			// Token: 0x060023CE RID: 9166 RVA: 0x0005167E File Offset: 0x0005067E
			internal override string get_accNameInternal(object childID)
			{
				base.ValidateChildID(ref childID);
				if (childID != null && (int)childID == 1)
				{
					return this.Name;
				}
				return base.get_accNameInternal(childID);
			}

			// Token: 0x060023CF RID: 9167 RVA: 0x000516A2 File Offset: 0x000506A2
			internal override string get_accKeyboardShortcutInternal(object childID)
			{
				base.ValidateChildID(ref childID);
				if (childID != null && (int)childID == 1)
				{
					return this.KeyboardShortcut;
				}
				return base.get_accKeyboardShortcutInternal(childID);
			}

			// Token: 0x04001579 RID: 5497
			private const int COMBOBOX_ACC_ITEM_INDEX = 1;
		}

		// Token: 0x02000296 RID: 662
		private sealed class ACNativeWindow : NativeWindow
		{
			// Token: 0x060023D0 RID: 9168 RVA: 0x000516C6 File Offset: 0x000506C6
			internal ACNativeWindow(IntPtr acHandle)
			{
				base.AssignHandle(acHandle);
				ComboBox.ACNativeWindow.ACWindows.Add(acHandle, this);
				UnsafeNativeMethods.EnumChildWindows(new HandleRef(this, acHandle), new NativeMethods.EnumChildrenCallback(ComboBox.ACNativeWindow.RegisterACWindowRecursive), NativeMethods.NullHandleRef);
			}

			// Token: 0x060023D1 RID: 9169 RVA: 0x00051704 File Offset: 0x00050704
			private static bool RegisterACWindowRecursive(IntPtr handle, IntPtr lparam)
			{
				if (!ComboBox.ACNativeWindow.ACWindows.ContainsKey(handle))
				{
					new ComboBox.ACNativeWindow(handle);
				}
				return true;
			}

			// Token: 0x1700057B RID: 1403
			// (get) Token: 0x060023D2 RID: 9170 RVA: 0x00051720 File Offset: 0x00050720
			internal bool Visible
			{
				get
				{
					return SafeNativeMethods.IsWindowVisible(new HandleRef(this, base.Handle));
				}
			}

			// Token: 0x1700057C RID: 1404
			// (get) Token: 0x060023D3 RID: 9171 RVA: 0x00051734 File Offset: 0x00050734
			internal static bool AutoCompleteActive
			{
				get
				{
					if (ComboBox.ACNativeWindow.inWndProcCnt > 0)
					{
						return true;
					}
					foreach (object obj in ComboBox.ACNativeWindow.ACWindows.Values)
					{
						ComboBox.ACNativeWindow acnativeWindow = obj as ComboBox.ACNativeWindow;
						if (acnativeWindow != null && acnativeWindow.Visible)
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x060023D4 RID: 9172 RVA: 0x000517AC File Offset: 0x000507AC
			protected override void WndProc(ref Message m)
			{
				ComboBox.ACNativeWindow.inWndProcCnt++;
				try
				{
					base.WndProc(ref m);
				}
				finally
				{
					ComboBox.ACNativeWindow.inWndProcCnt--;
				}
				if (m.Msg == 130)
				{
					ComboBox.ACNativeWindow.ACWindows.Remove(base.Handle);
				}
			}

			// Token: 0x060023D5 RID: 9173 RVA: 0x00051810 File Offset: 0x00050810
			internal static void RegisterACWindow(IntPtr acHandle, bool subclass)
			{
				if (subclass && ComboBox.ACNativeWindow.ACWindows.ContainsKey(acHandle) && ComboBox.ACNativeWindow.ACWindows[acHandle] == null)
				{
					ComboBox.ACNativeWindow.ACWindows.Remove(acHandle);
				}
				if (!ComboBox.ACNativeWindow.ACWindows.ContainsKey(acHandle))
				{
					if (subclass)
					{
						new ComboBox.ACNativeWindow(acHandle);
						return;
					}
					ComboBox.ACNativeWindow.ACWindows.Add(acHandle, null);
				}
			}

			// Token: 0x060023D6 RID: 9174 RVA: 0x00051884 File Offset: 0x00050884
			internal static void ClearNullACWindows()
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in ComboBox.ACNativeWindow.ACWindows)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (dictionaryEntry.Value == null)
					{
						arrayList.Add(dictionaryEntry.Key);
					}
				}
				foreach (object obj2 in arrayList)
				{
					IntPtr intPtr = (IntPtr)obj2;
					ComboBox.ACNativeWindow.ACWindows.Remove(intPtr);
				}
			}

			// Token: 0x0400157A RID: 5498
			internal static int inWndProcCnt;

			// Token: 0x0400157B RID: 5499
			private static Hashtable ACWindows = new Hashtable();
		}

		// Token: 0x02000297 RID: 663
		private class AutoCompleteDropDownFinder
		{
			// Token: 0x060023D8 RID: 9176 RVA: 0x00051954 File Offset: 0x00050954
			internal void FindDropDowns()
			{
				this.FindDropDowns(true);
			}

			// Token: 0x060023D9 RID: 9177 RVA: 0x0005195D File Offset: 0x0005095D
			internal void FindDropDowns(bool subclass)
			{
				if (!subclass)
				{
					ComboBox.ACNativeWindow.ClearNullACWindows();
				}
				this.shouldSubClass = subclass;
				UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(this.Callback), new HandleRef(null, IntPtr.Zero));
			}

			// Token: 0x060023DA RID: 9178 RVA: 0x00051990 File Offset: 0x00050990
			private bool Callback(IntPtr hWnd, IntPtr lParam)
			{
				HandleRef hRef = new HandleRef(null, hWnd);
				if (ComboBox.AutoCompleteDropDownFinder.GetClassName(hRef) == "Auto-Suggest Dropdown")
				{
					ComboBox.ACNativeWindow.RegisterACWindow(hRef.Handle, this.shouldSubClass);
				}
				return true;
			}

			// Token: 0x060023DB RID: 9179 RVA: 0x000519CC File Offset: 0x000509CC
			private static string GetClassName(HandleRef hRef)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				UnsafeNativeMethods.GetClassName(hRef, stringBuilder, 256);
				return stringBuilder.ToString();
			}

			// Token: 0x0400157C RID: 5500
			private const int MaxClassName = 256;

			// Token: 0x0400157D RID: 5501
			private const string AutoCompleteClassName = "Auto-Suggest Dropdown";

			// Token: 0x0400157E RID: 5502
			private bool shouldSubClass;
		}

		// Token: 0x02000298 RID: 664
		internal class FlatComboAdapter
		{
			// Token: 0x060023DD RID: 9181 RVA: 0x00051A00 File Offset: 0x00050A00
			public FlatComboAdapter(ComboBox comboBox, bool smallButton)
			{
				this.clientRect = comboBox.ClientRectangle;
				int horizontalScrollBarArrowWidth = SystemInformation.HorizontalScrollBarArrowWidth;
				this.outerBorder = new Rectangle(this.clientRect.Location, new Size(this.clientRect.Width - 1, this.clientRect.Height - 1));
				this.innerBorder = new Rectangle(this.outerBorder.X + 1, this.outerBorder.Y + 1, this.outerBorder.Width - horizontalScrollBarArrowWidth - 2, this.outerBorder.Height - 2);
				this.innerInnerBorder = new Rectangle(this.innerBorder.X + 1, this.innerBorder.Y + 1, this.innerBorder.Width - 2, this.innerBorder.Height - 2);
				this.dropDownRect = new Rectangle(this.innerBorder.Right + 1, this.innerBorder.Y, horizontalScrollBarArrowWidth, this.innerBorder.Height + 1);
				if (smallButton)
				{
					this.whiteFillRect = this.dropDownRect;
					this.whiteFillRect.Width = 5;
					this.dropDownRect.X = this.dropDownRect.X + 5;
					this.dropDownRect.Width = this.dropDownRect.Width - 5;
				}
				this.origRightToLeft = comboBox.RightToLeft;
				if (this.origRightToLeft == RightToLeft.Yes)
				{
					this.innerBorder.X = this.clientRect.Width - this.innerBorder.Right;
					this.innerInnerBorder.X = this.clientRect.Width - this.innerInnerBorder.Right;
					this.dropDownRect.X = this.clientRect.Width - this.dropDownRect.Right;
					this.whiteFillRect.X = this.clientRect.Width - this.whiteFillRect.Right + 1;
				}
			}

			// Token: 0x060023DE RID: 9182 RVA: 0x00051BEE File Offset: 0x00050BEE
			public bool IsValid(ComboBox combo)
			{
				return combo.ClientRectangle == this.clientRect && combo.RightToLeft == this.origRightToLeft;
			}

			// Token: 0x060023DF RID: 9183 RVA: 0x00051C14 File Offset: 0x00050C14
			public virtual void DrawFlatCombo(ComboBox comboBox, Graphics g)
			{
				if (comboBox.DropDownStyle == ComboBoxStyle.Simple)
				{
					return;
				}
				Color outerBorderColor = this.GetOuterBorderColor(comboBox);
				Color innerBorderColor = this.GetInnerBorderColor(comboBox);
				bool flag = comboBox.RightToLeft == RightToLeft.Yes;
				this.DrawFlatComboDropDown(comboBox, g, this.dropDownRect);
				if (!LayoutUtils.IsZeroWidthOrHeight(this.whiteFillRect))
				{
					using (Brush brush = new SolidBrush(innerBorderColor))
					{
						g.FillRectangle(brush, this.whiteFillRect);
					}
				}
				if (outerBorderColor.IsSystemColor)
				{
					Pen pen = SystemPens.FromSystemColor(outerBorderColor);
					g.DrawRectangle(pen, this.outerBorder);
					if (flag)
					{
						g.DrawRectangle(pen, new Rectangle(this.outerBorder.X, this.outerBorder.Y, this.dropDownRect.Width + 1, this.outerBorder.Height));
					}
					else
					{
						g.DrawRectangle(pen, new Rectangle(this.dropDownRect.X, this.outerBorder.Y, this.outerBorder.Right - this.dropDownRect.X, this.outerBorder.Height));
					}
				}
				else
				{
					using (Pen pen2 = new Pen(outerBorderColor))
					{
						g.DrawRectangle(pen2, this.outerBorder);
						if (flag)
						{
							g.DrawRectangle(pen2, new Rectangle(this.outerBorder.X, this.outerBorder.Y, this.dropDownRect.Width + 1, this.outerBorder.Height));
						}
						else
						{
							g.DrawRectangle(pen2, new Rectangle(this.dropDownRect.X, this.outerBorder.Y, this.outerBorder.Right - this.dropDownRect.X, this.outerBorder.Height));
						}
					}
				}
				if (innerBorderColor.IsSystemColor)
				{
					Pen pen3 = SystemPens.FromSystemColor(innerBorderColor);
					g.DrawRectangle(pen3, this.innerBorder);
					g.DrawRectangle(pen3, this.innerInnerBorder);
				}
				else
				{
					using (Pen pen4 = new Pen(innerBorderColor))
					{
						g.DrawRectangle(pen4, this.innerBorder);
						g.DrawRectangle(pen4, this.innerInnerBorder);
					}
				}
				if (!comboBox.Enabled || comboBox.FlatStyle == FlatStyle.Popup)
				{
					bool focused = comboBox.ContainsFocus || comboBox.MouseIsOver;
					Color popupOuterBorderColor = this.GetPopupOuterBorderColor(comboBox, focused);
					using (Pen pen5 = new Pen(popupOuterBorderColor))
					{
						Pen pen6 = comboBox.Enabled ? pen5 : SystemPens.Control;
						if (flag)
						{
							g.DrawRectangle(pen6, new Rectangle(this.outerBorder.X, this.outerBorder.Y, this.dropDownRect.Width + 1, this.outerBorder.Height));
						}
						else
						{
							g.DrawRectangle(pen6, new Rectangle(this.dropDownRect.X, this.outerBorder.Y, this.outerBorder.Right - this.dropDownRect.X, this.outerBorder.Height));
						}
						g.DrawRectangle(pen5, this.outerBorder);
					}
				}
			}

			// Token: 0x060023E0 RID: 9184 RVA: 0x00051F5C File Offset: 0x00050F5C
			protected virtual void DrawFlatComboDropDown(ComboBox comboBox, Graphics g, Rectangle dropDownRect)
			{
				g.FillRectangle(SystemBrushes.Control, dropDownRect);
				Brush brush = comboBox.Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark;
				Point point = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
				if (this.origRightToLeft == RightToLeft.Yes)
				{
					point.X -= dropDownRect.Width % 2;
				}
				else
				{
					point.X += dropDownRect.Width % 2;
				}
				g.FillPolygon(brush, new Point[]
				{
					new Point(point.X - 2, point.Y - 1),
					new Point(point.X + 3, point.Y - 1),
					new Point(point.X, point.Y + 2)
				});
			}

			// Token: 0x060023E1 RID: 9185 RVA: 0x00052062 File Offset: 0x00051062
			protected virtual Color GetOuterBorderColor(ComboBox comboBox)
			{
				if (!comboBox.Enabled)
				{
					return SystemColors.ControlDark;
				}
				return SystemColors.Window;
			}

			// Token: 0x060023E2 RID: 9186 RVA: 0x00052077 File Offset: 0x00051077
			protected virtual Color GetPopupOuterBorderColor(ComboBox comboBox, bool focused)
			{
				if (!comboBox.Enabled)
				{
					return SystemColors.ControlDark;
				}
				if (!focused)
				{
					return SystemColors.Window;
				}
				return SystemColors.ControlDark;
			}

			// Token: 0x060023E3 RID: 9187 RVA: 0x00052095 File Offset: 0x00051095
			protected virtual Color GetInnerBorderColor(ComboBox comboBox)
			{
				if (!comboBox.Enabled)
				{
					return SystemColors.Control;
				}
				return comboBox.BackColor;
			}

			// Token: 0x060023E4 RID: 9188 RVA: 0x000520AC File Offset: 0x000510AC
			public void ValidateOwnerDrawRegions(ComboBox comboBox, Rectangle updateRegionBox)
			{
				if (comboBox != null)
				{
					return;
				}
				Rectangle r = new Rectangle(0, 0, comboBox.Width, this.innerBorder.Top);
				Rectangle r2 = new Rectangle(0, this.innerBorder.Bottom, comboBox.Width, comboBox.Height - this.innerBorder.Bottom);
				Rectangle r3 = new Rectangle(0, 0, this.innerBorder.Left, comboBox.Height);
				Rectangle r4 = new Rectangle(this.innerBorder.Right, 0, comboBox.Width - this.innerBorder.Right, comboBox.Height);
				if (r.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
				if (r2.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r2);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
				if (r3.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r3);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
				if (r4.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r4);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
			}

			// Token: 0x0400157F RID: 5503
			private const int WhiteFillRectWidth = 5;

			// Token: 0x04001580 RID: 5504
			private Rectangle outerBorder;

			// Token: 0x04001581 RID: 5505
			private Rectangle innerBorder;

			// Token: 0x04001582 RID: 5506
			private Rectangle innerInnerBorder;

			// Token: 0x04001583 RID: 5507
			internal Rectangle dropDownRect;

			// Token: 0x04001584 RID: 5508
			private Rectangle whiteFillRect;

			// Token: 0x04001585 RID: 5509
			private Rectangle clientRect;

			// Token: 0x04001586 RID: 5510
			private RightToLeft origRightToLeft;
		}
	}
}
