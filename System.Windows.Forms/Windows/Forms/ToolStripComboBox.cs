using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x02000679 RID: 1657
	[DefaultProperty("Items")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	public class ToolStripComboBox : ToolStripControlHost
	{
		// Token: 0x06005764 RID: 22372 RVA: 0x0013CC80 File Offset: 0x0013BC80
		public ToolStripComboBox() : base(ToolStripComboBox.CreateControlInstance())
		{
			ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = base.Control as ToolStripComboBox.ToolStripComboBoxControl;
			toolStripComboBoxControl.Owner = this;
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x0013CCAB File Offset: 0x0013BCAB
		public ToolStripComboBox(string name) : this()
		{
			base.Name = name;
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x0013CCBA File Offset: 0x0013BCBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ToolStripComboBox(Control c) : base(c)
		{
			throw new NotSupportedException(SR.GetString("ToolStripMustSupplyItsOwnComboBox"));
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x0013CCD4 File Offset: 0x0013BCD4
		private static Control CreateControlInstance()
		{
			return new ToolStripComboBox.ToolStripComboBoxControl
			{
				FlatStyle = FlatStyle.Popup,
				Font = ToolStripManager.DefaultFont
			};
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06005768 RID: 22376 RVA: 0x0013CCFA File Offset: 0x0013BCFA
		// (set) Token: 0x06005769 RID: 22377 RVA: 0x0013CD07 File Offset: 0x0013BD07
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ComboBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				return this.ComboBox.AutoCompleteCustomSource;
			}
			set
			{
				this.ComboBox.AutoCompleteCustomSource = value;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x0600576A RID: 22378 RVA: 0x0013CD15 File Offset: 0x0013BD15
		// (set) Token: 0x0600576B RID: 22379 RVA: 0x0013CD22 File Offset: 0x0013BD22
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(AutoCompleteMode.None)]
		[SRDescription("ComboBoxAutoCompleteModeDescr")]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.ComboBox.AutoCompleteMode;
			}
			set
			{
				this.ComboBox.AutoCompleteMode = value;
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x0600576C RID: 22380 RVA: 0x0013CD30 File Offset: 0x0013BD30
		// (set) Token: 0x0600576D RID: 22381 RVA: 0x0013CD3D File Offset: 0x0013BD3D
		[SRDescription("ComboBoxAutoCompleteSourceDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(AutoCompleteSource.None)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.ComboBox.AutoCompleteSource;
			}
			set
			{
				this.ComboBox.AutoCompleteSource = value;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x0600576E RID: 22382 RVA: 0x0013CD4B File Offset: 0x0013BD4B
		// (set) Token: 0x0600576F RID: 22383 RVA: 0x0013CD53 File Offset: 0x0013BD53
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06005770 RID: 22384 RVA: 0x0013CD5C File Offset: 0x0013BD5C
		// (set) Token: 0x06005771 RID: 22385 RVA: 0x0013CD64 File Offset: 0x0013BD64
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06005772 RID: 22386 RVA: 0x0013CD6D File Offset: 0x0013BD6D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ComboBox ComboBox
		{
			get
			{
				return base.Control as ComboBox;
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x0013CD7A File Offset: 0x0013BD7A
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06005774 RID: 22388 RVA: 0x0013CD85 File Offset: 0x0013BD85
		protected internal override Padding DefaultMargin
		{
			get
			{
				if (base.IsOnDropDown)
				{
					return new Padding(2);
				}
				return new Padding(1, 0, 1, 0);
			}
		}

		// Token: 0x14000345 RID: 837
		// (add) Token: 0x06005775 RID: 22389 RVA: 0x0013CD9F File Offset: 0x0013BD9F
		// (remove) Token: 0x06005776 RID: 22390 RVA: 0x0013CDA8 File Offset: 0x0013BDA8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000346 RID: 838
		// (add) Token: 0x06005777 RID: 22391 RVA: 0x0013CDB1 File Offset: 0x0013BDB1
		// (remove) Token: 0x06005778 RID: 22392 RVA: 0x0013CDC4 File Offset: 0x0013BDC4
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnDropDownDescr")]
		public event EventHandler DropDown
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventDropDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventDropDown, value);
			}
		}

		// Token: 0x14000347 RID: 839
		// (add) Token: 0x06005779 RID: 22393 RVA: 0x0013CDD7 File Offset: 0x0013BDD7
		// (remove) Token: 0x0600577A RID: 22394 RVA: 0x0013CDEA File Offset: 0x0013BDEA
		[SRDescription("ComboBoxOnDropDownClosedDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler DropDownClosed
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventDropDownClosed, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventDropDownClosed, value);
			}
		}

		// Token: 0x14000348 RID: 840
		// (add) Token: 0x0600577B RID: 22395 RVA: 0x0013CDFD File Offset: 0x0013BDFD
		// (remove) Token: 0x0600577C RID: 22396 RVA: 0x0013CE10 File Offset: 0x0013BE10
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownStyleChangedDescr")]
		public event EventHandler DropDownStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventDropDownStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventDropDownStyleChanged, value);
			}
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x0600577D RID: 22397 RVA: 0x0013CE23 File Offset: 0x0013BE23
		// (set) Token: 0x0600577E RID: 22398 RVA: 0x0013CE30 File Offset: 0x0013BE30
		[SRDescription("ComboBoxDropDownHeightDescr")]
		[DefaultValue(106)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		public int DropDownHeight
		{
			get
			{
				return this.ComboBox.DropDownHeight;
			}
			set
			{
				this.ComboBox.DropDownHeight = value;
			}
		}

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x0013CE3E File Offset: 0x0013BE3E
		// (set) Token: 0x06005780 RID: 22400 RVA: 0x0013CE4B File Offset: 0x0013BE4B
		[SRDescription("ComboBoxStyleDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(ComboBoxStyle.DropDown)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ComboBoxStyle DropDownStyle
		{
			get
			{
				return this.ComboBox.DropDownStyle;
			}
			set
			{
				this.ComboBox.DropDownStyle = value;
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06005781 RID: 22401 RVA: 0x0013CE59 File Offset: 0x0013BE59
		// (set) Token: 0x06005782 RID: 22402 RVA: 0x0013CE66 File Offset: 0x0013BE66
		[SRDescription("ComboBoxDropDownWidthDescr")]
		[SRCategory("CatBehavior")]
		public int DropDownWidth
		{
			get
			{
				return this.ComboBox.DropDownWidth;
			}
			set
			{
				this.ComboBox.DropDownWidth = value;
			}
		}

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06005783 RID: 22403 RVA: 0x0013CE74 File Offset: 0x0013BE74
		// (set) Token: 0x06005784 RID: 22404 RVA: 0x0013CE81 File Offset: 0x0013BE81
		[SRDescription("ComboBoxDroppedDownDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool DroppedDown
		{
			get
			{
				return this.ComboBox.DroppedDown;
			}
			set
			{
				this.ComboBox.DroppedDown = value;
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06005785 RID: 22405 RVA: 0x0013CE8F File Offset: 0x0013BE8F
		// (set) Token: 0x06005786 RID: 22406 RVA: 0x0013CE9C File Offset: 0x0013BE9C
		[SRCategory("CatAppearance")]
		[SRDescription("ComboBoxFlatStyleDescr")]
		[DefaultValue(FlatStyle.Popup)]
		[Localizable(true)]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.ComboBox.FlatStyle;
			}
			set
			{
				this.ComboBox.FlatStyle = value;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06005787 RID: 22407 RVA: 0x0013CEAA File Offset: 0x0013BEAA
		// (set) Token: 0x06005788 RID: 22408 RVA: 0x0013CEB7 File Offset: 0x0013BEB7
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ComboBoxIntegralHeightDescr")]
		public bool IntegralHeight
		{
			get
			{
				return this.ComboBox.IntegralHeight;
			}
			set
			{
				this.ComboBox.IntegralHeight = value;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06005789 RID: 22409 RVA: 0x0013CEC5 File Offset: 0x0013BEC5
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ComboBoxItemsDescr")]
		[SRCategory("CatData")]
		public ComboBox.ObjectCollection Items
		{
			get
			{
				return this.ComboBox.Items;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x0600578A RID: 22410 RVA: 0x0013CED2 File Offset: 0x0013BED2
		// (set) Token: 0x0600578B RID: 22411 RVA: 0x0013CEDF File Offset: 0x0013BEDF
		[DefaultValue(8)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ComboBoxMaxDropDownItemsDescr")]
		public int MaxDropDownItems
		{
			get
			{
				return this.ComboBox.MaxDropDownItems;
			}
			set
			{
				this.ComboBox.MaxDropDownItems = value;
			}
		}

		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x0600578C RID: 22412 RVA: 0x0013CEED File Offset: 0x0013BEED
		// (set) Token: 0x0600578D RID: 22413 RVA: 0x0013CEFA File Offset: 0x0013BEFA
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ComboBoxMaxLengthDescr")]
		public int MaxLength
		{
			get
			{
				return this.ComboBox.MaxLength;
			}
			set
			{
				this.ComboBox.MaxLength = value;
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x0600578E RID: 22414 RVA: 0x0013CF08 File Offset: 0x0013BF08
		// (set) Token: 0x0600578F RID: 22415 RVA: 0x0013CF15 File Offset: 0x0013BF15
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedIndexDescr")]
		public int SelectedIndex
		{
			get
			{
				return this.ComboBox.SelectedIndex;
			}
			set
			{
				this.ComboBox.SelectedIndex = value;
			}
		}

		// Token: 0x14000349 RID: 841
		// (add) Token: 0x06005790 RID: 22416 RVA: 0x0013CF23 File Offset: 0x0013BF23
		// (remove) Token: 0x06005791 RID: 22417 RVA: 0x0013CF36 File Offset: 0x0013BF36
		[SRDescription("selectedIndexChangedEventDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventSelectedIndexChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventSelectedIndexChanged, value);
			}
		}

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06005792 RID: 22418 RVA: 0x0013CF49 File Offset: 0x0013BF49
		// (set) Token: 0x06005793 RID: 22419 RVA: 0x0013CF56 File Offset: 0x0013BF56
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedItemDescr")]
		public object SelectedItem
		{
			get
			{
				return this.ComboBox.SelectedItem;
			}
			set
			{
				this.ComboBox.SelectedItem = value;
			}
		}

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06005794 RID: 22420 RVA: 0x0013CF64 File Offset: 0x0013BF64
		// (set) Token: 0x06005795 RID: 22421 RVA: 0x0013CF71 File Offset: 0x0013BF71
		[SRDescription("ComboBoxSelectedTextDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string SelectedText
		{
			get
			{
				return this.ComboBox.SelectedText;
			}
			set
			{
				this.ComboBox.SelectedText = value;
			}
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06005796 RID: 22422 RVA: 0x0013CF7F File Offset: 0x0013BF7F
		// (set) Token: 0x06005797 RID: 22423 RVA: 0x0013CF8C File Offset: 0x0013BF8C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ComboBoxSelectionLengthDescr")]
		public int SelectionLength
		{
			get
			{
				return this.ComboBox.SelectionLength;
			}
			set
			{
				this.ComboBox.SelectionLength = value;
			}
		}

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06005798 RID: 22424 RVA: 0x0013CF9A File Offset: 0x0013BF9A
		// (set) Token: 0x06005799 RID: 22425 RVA: 0x0013CFA7 File Offset: 0x0013BFA7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectionStartDescr")]
		public int SelectionStart
		{
			get
			{
				return this.ComboBox.SelectionStart;
			}
			set
			{
				this.ComboBox.SelectionStart = value;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x0600579A RID: 22426 RVA: 0x0013CFB5 File Offset: 0x0013BFB5
		// (set) Token: 0x0600579B RID: 22427 RVA: 0x0013CFC2 File Offset: 0x0013BFC2
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxSortedDescr")]
		public bool Sorted
		{
			get
			{
				return this.ComboBox.Sorted;
			}
			set
			{
				this.ComboBox.Sorted = value;
			}
		}

		// Token: 0x1400034A RID: 842
		// (add) Token: 0x0600579C RID: 22428 RVA: 0x0013CFD0 File Offset: 0x0013BFD0
		// (remove) Token: 0x0600579D RID: 22429 RVA: 0x0013CFE3 File Offset: 0x0013BFE3
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnTextUpdateDescr")]
		public event EventHandler TextUpdate
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventTextUpdate, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventTextUpdate, value);
			}
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x0013CFF6 File Offset: 0x0013BFF6
		public void BeginUpdate()
		{
			this.ComboBox.BeginUpdate();
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x0013D003 File Offset: 0x0013C003
		public void EndUpdate()
		{
			this.ComboBox.EndUpdate();
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x0013D010 File Offset: 0x0013C010
		public int FindString(string s)
		{
			return this.ComboBox.FindString(s);
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x0013D01E File Offset: 0x0013C01E
		public int FindString(string s, int startIndex)
		{
			return this.ComboBox.FindString(s, startIndex);
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x0013D02D File Offset: 0x0013C02D
		public int FindStringExact(string s)
		{
			return this.ComboBox.FindStringExact(s);
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x0013D03B File Offset: 0x0013C03B
		public int FindStringExact(string s, int startIndex)
		{
			return this.ComboBox.FindStringExact(s, startIndex);
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x0013D04A File Offset: 0x0013C04A
		public int GetItemHeight(int index)
		{
			return this.ComboBox.GetItemHeight(index);
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x0013D058 File Offset: 0x0013C058
		public void Select(int start, int length)
		{
			this.ComboBox.Select(start, length);
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x0013D067 File Offset: 0x0013C067
		public void SelectAll()
		{
			this.ComboBox.SelectAll();
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x0013D074 File Offset: 0x0013C074
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size preferredSize = base.GetPreferredSize(constrainingSize);
			preferredSize.Width = Math.Max(preferredSize.Width, 75);
			return preferredSize;
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x0013D09F File Offset: 0x0013C09F
		private void HandleDropDown(object sender, EventArgs e)
		{
			this.OnDropDown(e);
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x0013D0A8 File Offset: 0x0013C0A8
		private void HandleDropDownClosed(object sender, EventArgs e)
		{
			this.OnDropDownClosed(e);
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x0013D0B1 File Offset: 0x0013C0B1
		private void HandleDropDownStyleChanged(object sender, EventArgs e)
		{
			this.OnDropDownStyleChanged(e);
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x0013D0BA File Offset: 0x0013C0BA
		private void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			this.OnSelectedIndexChanged(e);
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x0013D0C3 File Offset: 0x0013C0C3
		private void HandleSelectionChangeCommitted(object sender, EventArgs e)
		{
			this.OnSelectionChangeCommitted(e);
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x0013D0CC File Offset: 0x0013C0CC
		private void HandleTextUpdate(object sender, EventArgs e)
		{
			this.OnTextUpdate(e);
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x0013D0D5 File Offset: 0x0013C0D5
		protected virtual void OnDropDown(EventArgs e)
		{
			if (base.ParentInternal != null)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(base.ParentInternal.RestoreFocusFilter);
				ToolStripManager.ModalMenuFilter.SuspendMenuMode();
			}
			base.RaiseEvent(ToolStripComboBox.EventDropDown, e);
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x0013D105 File Offset: 0x0013C105
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			if (base.ParentInternal != null)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(base.ParentInternal.RestoreFocusFilter);
				ToolStripManager.ModalMenuFilter.ResumeMenuMode();
			}
			base.RaiseEvent(ToolStripComboBox.EventDropDownClosed, e);
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x0013D135 File Offset: 0x0013C135
		protected virtual void OnDropDownStyleChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventDropDownStyleChanged, e);
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x0013D143 File Offset: 0x0013C143
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventSelectedIndexChanged, e);
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x0013D151 File Offset: 0x0013C151
		protected virtual void OnSelectionChangeCommitted(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventSelectionChangeCommitted, e);
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x0013D15F File Offset: 0x0013C15F
		protected virtual void OnTextUpdate(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventTextUpdate, e);
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x0013D170 File Offset: 0x0013C170
		protected override void OnSubscribeControlEvents(Control control)
		{
			ComboBox comboBox = control as ComboBox;
			if (comboBox != null)
			{
				comboBox.DropDown += this.HandleDropDown;
				comboBox.DropDownClosed += this.HandleDropDownClosed;
				comboBox.DropDownStyleChanged += this.HandleDropDownStyleChanged;
				comboBox.SelectedIndexChanged += this.HandleSelectedIndexChanged;
				comboBox.SelectionChangeCommitted += this.HandleSelectionChangeCommitted;
				comboBox.TextUpdate += this.HandleTextUpdate;
			}
			base.OnSubscribeControlEvents(control);
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x0013D1FC File Offset: 0x0013C1FC
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			ComboBox comboBox = control as ComboBox;
			if (comboBox != null)
			{
				comboBox.DropDown -= this.HandleDropDown;
				comboBox.DropDownClosed -= this.HandleDropDownClosed;
				comboBox.DropDownStyleChanged -= this.HandleDropDownStyleChanged;
				comboBox.SelectedIndexChanged -= this.HandleSelectedIndexChanged;
				comboBox.SelectionChangeCommitted -= this.HandleSelectionChangeCommitted;
				comboBox.TextUpdate -= this.HandleTextUpdate;
			}
			base.OnUnsubscribeControlEvents(control);
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x0013D286 File Offset: 0x0013C286
		private bool ShouldSerializeDropDownWidth()
		{
			return this.ComboBox.ShouldSerializeDropDownWidth();
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x0013D293 File Offset: 0x0013C293
		internal override bool ShouldSerializeFont()
		{
			return !object.Equals(this.Font, ToolStripManager.DefaultFont);
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x0013D2A8 File Offset: 0x0013C2A8
		public override string ToString()
		{
			return base.ToString() + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x04003794 RID: 14228
		internal static readonly object EventDropDown = new object();

		// Token: 0x04003795 RID: 14229
		internal static readonly object EventDropDownClosed = new object();

		// Token: 0x04003796 RID: 14230
		internal static readonly object EventDropDownStyleChanged = new object();

		// Token: 0x04003797 RID: 14231
		internal static readonly object EventSelectedIndexChanged = new object();

		// Token: 0x04003798 RID: 14232
		internal static readonly object EventSelectionChangeCommitted = new object();

		// Token: 0x04003799 RID: 14233
		internal static readonly object EventTextUpdate = new object();

		// Token: 0x0200067A RID: 1658
		internal class ToolStripComboBoxControl : ComboBox
		{
			// Token: 0x060057BA RID: 22458 RVA: 0x0013D31B File Offset: 0x0013C31B
			public ToolStripComboBoxControl()
			{
				base.FlatStyle = FlatStyle.Popup;
				base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			}

			// Token: 0x17001241 RID: 4673
			// (get) Token: 0x060057BB RID: 22459 RVA: 0x0013D336 File Offset: 0x0013C336
			// (set) Token: 0x060057BC RID: 22460 RVA: 0x0013D33E File Offset: 0x0013C33E
			public ToolStripComboBox Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					this.owner = value;
				}
			}

			// Token: 0x17001242 RID: 4674
			// (get) Token: 0x060057BD RID: 22461 RVA: 0x0013D348 File Offset: 0x0013C348
			private ProfessionalColorTable ColorTable
			{
				get
				{
					if (this.Owner != null)
					{
						ToolStripProfessionalRenderer toolStripProfessionalRenderer = this.Owner.Renderer as ToolStripProfessionalRenderer;
						if (toolStripProfessionalRenderer != null)
						{
							return toolStripProfessionalRenderer.ColorTable;
						}
					}
					return ProfessionalColors.ColorTable;
				}
			}

			// Token: 0x060057BE RID: 22462 RVA: 0x0013D37D File Offset: 0x0013C37D
			internal override ComboBox.FlatComboAdapter CreateFlatComboAdapterInstance()
			{
				return new ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter(this);
			}

			// Token: 0x060057BF RID: 22463 RVA: 0x0013D385 File Offset: 0x0013C385
			protected override bool IsInputKey(Keys keyData)
			{
				return ((keyData & Keys.Alt) == Keys.Alt && ((keyData & Keys.Down) == Keys.Down || (keyData & Keys.Up) == Keys.Up)) || base.IsInputKey(keyData);
			}

			// Token: 0x060057C0 RID: 22464 RVA: 0x0013D3AE File Offset: 0x0013C3AE
			protected override void OnDropDownClosed(EventArgs e)
			{
				base.OnDropDownClosed(e);
				base.Invalidate();
				base.Update();
			}

			// Token: 0x0400379A RID: 14234
			private ToolStripComboBox owner;

			// Token: 0x0200067B RID: 1659
			internal class ToolStripComboBoxFlatComboAdapter : ComboBox.FlatComboAdapter
			{
				// Token: 0x060057C1 RID: 22465 RVA: 0x0013D3C3 File Offset: 0x0013C3C3
				public ToolStripComboBoxFlatComboAdapter(ComboBox comboBox) : base(comboBox, true)
				{
				}

				// Token: 0x060057C2 RID: 22466 RVA: 0x0013D3D0 File Offset: 0x0013C3D0
				private static bool UseBaseAdapter(ComboBox comboBox)
				{
					ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = comboBox as ToolStripComboBox.ToolStripComboBoxControl;
					return toolStripComboBoxControl == null || !(toolStripComboBoxControl.Owner.Renderer is ToolStripProfessionalRenderer);
				}

				// Token: 0x060057C3 RID: 22467 RVA: 0x0013D3FC File Offset: 0x0013C3FC
				private static ProfessionalColorTable GetColorTable(ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl)
				{
					if (toolStripComboBoxControl != null)
					{
						return toolStripComboBoxControl.ColorTable;
					}
					return ProfessionalColors.ColorTable;
				}

				// Token: 0x060057C4 RID: 22468 RVA: 0x0013D40D File Offset: 0x0013C40D
				protected override Color GetOuterBorderColor(ComboBox comboBox)
				{
					if (ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.UseBaseAdapter(comboBox))
					{
						return base.GetOuterBorderColor(comboBox);
					}
					if (!comboBox.Enabled)
					{
						return ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.GetColorTable(comboBox as ToolStripComboBox.ToolStripComboBoxControl).ComboBoxBorder;
					}
					return SystemColors.Window;
				}

				// Token: 0x060057C5 RID: 22469 RVA: 0x0013D43D File Offset: 0x0013C43D
				protected override Color GetPopupOuterBorderColor(ComboBox comboBox, bool focused)
				{
					if (ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.UseBaseAdapter(comboBox))
					{
						return base.GetPopupOuterBorderColor(comboBox, focused);
					}
					if (!comboBox.Enabled)
					{
						return SystemColors.ControlDark;
					}
					if (!focused)
					{
						return SystemColors.Window;
					}
					return ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.GetColorTable(comboBox as ToolStripComboBox.ToolStripComboBoxControl).ComboBoxBorder;
				}

				// Token: 0x060057C6 RID: 22470 RVA: 0x0013D478 File Offset: 0x0013C478
				protected override void DrawFlatComboDropDown(ComboBox comboBox, Graphics g, Rectangle dropDownRect)
				{
					if (ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.UseBaseAdapter(comboBox))
					{
						base.DrawFlatComboDropDown(comboBox, g, dropDownRect);
						return;
					}
					if (!comboBox.Enabled || !ToolStripManager.VisualStylesEnabled)
					{
						g.FillRectangle(SystemBrushes.Control, dropDownRect);
					}
					else
					{
						ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = comboBox as ToolStripComboBox.ToolStripComboBoxControl;
						ProfessionalColorTable colorTable = ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.GetColorTable(toolStripComboBoxControl);
						if (!comboBox.DroppedDown)
						{
							bool flag = comboBox.ContainsFocus || comboBox.MouseIsOver;
							if (flag)
							{
								using (Brush brush = new LinearGradientBrush(dropDownRect, colorTable.ComboBoxButtonSelectedGradientBegin, colorTable.ComboBoxButtonSelectedGradientEnd, LinearGradientMode.Vertical))
								{
									g.FillRectangle(brush, dropDownRect);
									goto IL_114;
								}
							}
							if (toolStripComboBoxControl.Owner.IsOnOverflow)
							{
								using (Brush brush2 = new SolidBrush(colorTable.ComboBoxButtonOnOverflow))
								{
									g.FillRectangle(brush2, dropDownRect);
									goto IL_114;
								}
							}
							using (Brush brush3 = new LinearGradientBrush(dropDownRect, colorTable.ComboBoxButtonGradientBegin, colorTable.ComboBoxButtonGradientEnd, LinearGradientMode.Vertical))
							{
								g.FillRectangle(brush3, dropDownRect);
								goto IL_114;
							}
						}
						using (Brush brush4 = new LinearGradientBrush(dropDownRect, colorTable.ComboBoxButtonPressedGradientBegin, colorTable.ComboBoxButtonPressedGradientEnd, LinearGradientMode.Vertical))
						{
							g.FillRectangle(brush4, dropDownRect);
						}
					}
					IL_114:
					Brush brush5 = comboBox.Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText;
					Point point = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
					point.X += dropDownRect.Width % 2;
					g.FillPolygon(brush5, new Point[]
					{
						new Point(point.X - 2, point.Y - 1),
						new Point(point.X + 3, point.Y - 1),
						new Point(point.X, point.Y + 2)
					});
				}
			}
		}
	}
}
