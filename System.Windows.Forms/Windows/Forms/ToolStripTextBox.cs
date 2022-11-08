using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020006E9 RID: 1769
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	public class ToolStripTextBox : ToolStripControlHost
	{
		// Token: 0x06005D5B RID: 23899 RVA: 0x00153488 File Offset: 0x00152488
		public ToolStripTextBox() : base(ToolStripTextBox.CreateControlInstance())
		{
			ToolStripTextBox.ToolStripTextBoxControl toolStripTextBoxControl = base.Control as ToolStripTextBox.ToolStripTextBoxControl;
			toolStripTextBoxControl.Owner = this;
		}

		// Token: 0x06005D5C RID: 23900 RVA: 0x001534B3 File Offset: 0x001524B3
		public ToolStripTextBox(string name) : this()
		{
			base.Name = name;
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x001534C2 File Offset: 0x001524C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ToolStripTextBox(Control c) : base(c)
		{
			throw new NotSupportedException(SR.GetString("ToolStripMustSupplyItsOwnTextBox"));
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06005D5E RID: 23902 RVA: 0x001534DA File Offset: 0x001524DA
		// (set) Token: 0x06005D5F RID: 23903 RVA: 0x001534E2 File Offset: 0x001524E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x06005D60 RID: 23904 RVA: 0x001534EB File Offset: 0x001524EB
		// (set) Token: 0x06005D61 RID: 23905 RVA: 0x001534F3 File Offset: 0x001524F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x06005D62 RID: 23906 RVA: 0x001534FC File Offset: 0x001524FC
		protected internal override Padding DefaultMargin
		{
			get
			{
				if (base.IsOnDropDown)
				{
					return new Padding(1);
				}
				return new Padding(1, 0, 1, 0);
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x06005D63 RID: 23907 RVA: 0x00153516 File Offset: 0x00152516
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x06005D64 RID: 23908 RVA: 0x00153521 File Offset: 0x00152521
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextBox TextBox
		{
			get
			{
				return base.Control as TextBox;
			}
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x00153530 File Offset: 0x00152530
		private static Control CreateControlInstance()
		{
			return new ToolStripTextBox.ToolStripTextBoxControl
			{
				BorderStyle = BorderStyle.Fixed3D,
				AutoSize = true
			};
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x00153554 File Offset: 0x00152554
		public override Size GetPreferredSize(Size constrainingSize)
		{
			return new Size(CommonProperties.GetSpecifiedBounds(this.TextBox).Width, this.TextBox.PreferredHeight);
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x00153584 File Offset: 0x00152584
		private void HandleAcceptsTabChanged(object sender, EventArgs e)
		{
			this.OnAcceptsTabChanged(e);
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x0015358D File Offset: 0x0015258D
		private void HandleBorderStyleChanged(object sender, EventArgs e)
		{
			this.OnBorderStyleChanged(e);
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x00153596 File Offset: 0x00152596
		private void HandleHideSelectionChanged(object sender, EventArgs e)
		{
			this.OnHideSelectionChanged(e);
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x0015359F File Offset: 0x0015259F
		private void HandleModifiedChanged(object sender, EventArgs e)
		{
			this.OnModifiedChanged(e);
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x001535A8 File Offset: 0x001525A8
		private void HandleMultilineChanged(object sender, EventArgs e)
		{
			this.OnMultilineChanged(e);
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x001535B1 File Offset: 0x001525B1
		private void HandleReadOnlyChanged(object sender, EventArgs e)
		{
			this.OnReadOnlyChanged(e);
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x001535BA File Offset: 0x001525BA
		private void HandleTextBoxTextAlignChanged(object sender, EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventTextBoxTextAlignChanged, e);
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x001535C8 File Offset: 0x001525C8
		protected virtual void OnAcceptsTabChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventAcceptsTabChanged, e);
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x001535D6 File Offset: 0x001525D6
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventBorderStyleChanged, e);
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x001535E4 File Offset: 0x001525E4
		protected virtual void OnHideSelectionChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventHideSelectionChanged, e);
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x001535F2 File Offset: 0x001525F2
		protected virtual void OnModifiedChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventModifiedChanged, e);
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x00153600 File Offset: 0x00152600
		protected virtual void OnMultilineChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventMultilineChanged, e);
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x0015360E File Offset: 0x0015260E
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventReadOnlyChanged, e);
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x0015361C File Offset: 0x0015261C
		protected override void OnSubscribeControlEvents(Control control)
		{
			TextBox textBox = control as TextBox;
			if (textBox != null)
			{
				textBox.AcceptsTabChanged += this.HandleAcceptsTabChanged;
				textBox.BorderStyleChanged += this.HandleBorderStyleChanged;
				textBox.HideSelectionChanged += this.HandleHideSelectionChanged;
				textBox.ModifiedChanged += this.HandleModifiedChanged;
				textBox.MultilineChanged += this.HandleMultilineChanged;
				textBox.ReadOnlyChanged += this.HandleReadOnlyChanged;
				textBox.TextAlignChanged += this.HandleTextBoxTextAlignChanged;
			}
			base.OnSubscribeControlEvents(control);
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x001536B8 File Offset: 0x001526B8
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			TextBox textBox = control as TextBox;
			if (textBox != null)
			{
				textBox.AcceptsTabChanged -= this.HandleAcceptsTabChanged;
				textBox.BorderStyleChanged -= this.HandleBorderStyleChanged;
				textBox.HideSelectionChanged -= this.HandleHideSelectionChanged;
				textBox.ModifiedChanged -= this.HandleModifiedChanged;
				textBox.MultilineChanged -= this.HandleMultilineChanged;
				textBox.ReadOnlyChanged -= this.HandleReadOnlyChanged;
				textBox.TextAlignChanged -= this.HandleTextBoxTextAlignChanged;
			}
			base.OnUnsubscribeControlEvents(control);
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x00153754 File Offset: 0x00152754
		internal override bool ShouldSerializeFont()
		{
			return this.Font != ToolStripManager.DefaultFont;
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x06005D77 RID: 23927 RVA: 0x00153766 File Offset: 0x00152766
		// (set) Token: 0x06005D78 RID: 23928 RVA: 0x00153773 File Offset: 0x00152773
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsTabDescr")]
		public bool AcceptsTab
		{
			get
			{
				return this.TextBox.AcceptsTab;
			}
			set
			{
				this.TextBox.AcceptsTab = value;
			}
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06005D79 RID: 23929 RVA: 0x00153781 File Offset: 0x00152781
		// (set) Token: 0x06005D7A RID: 23930 RVA: 0x0015378E File Offset: 0x0015278E
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsReturnDescr")]
		public bool AcceptsReturn
		{
			get
			{
				return this.TextBox.AcceptsReturn;
			}
			set
			{
				this.TextBox.AcceptsReturn = value;
			}
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06005D7B RID: 23931 RVA: 0x0015379C File Offset: 0x0015279C
		// (set) Token: 0x06005D7C RID: 23932 RVA: 0x001537A9 File Offset: 0x001527A9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("TextBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				return this.TextBox.AutoCompleteCustomSource;
			}
			set
			{
				this.TextBox.AutoCompleteCustomSource = value;
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06005D7D RID: 23933 RVA: 0x001537B7 File Offset: 0x001527B7
		// (set) Token: 0x06005D7E RID: 23934 RVA: 0x001537C4 File Offset: 0x001527C4
		[Browsable(true)]
		[SRDescription("TextBoxAutoCompleteModeDescr")]
		[DefaultValue(AutoCompleteMode.None)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.TextBox.AutoCompleteMode;
			}
			set
			{
				this.TextBox.AutoCompleteMode = value;
			}
		}

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06005D7F RID: 23935 RVA: 0x001537D2 File Offset: 0x001527D2
		// (set) Token: 0x06005D80 RID: 23936 RVA: 0x001537DF File Offset: 0x001527DF
		[DefaultValue(AutoCompleteSource.None)]
		[SRDescription("TextBoxAutoCompleteSourceDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.TextBox.AutoCompleteSource;
			}
			set
			{
				this.TextBox.AutoCompleteSource = value;
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06005D81 RID: 23937 RVA: 0x001537ED File Offset: 0x001527ED
		// (set) Token: 0x06005D82 RID: 23938 RVA: 0x001537FA File Offset: 0x001527FA
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("TextBoxBorderDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.TextBox.BorderStyle;
			}
			set
			{
				this.TextBox.BorderStyle = value;
			}
		}

		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06005D83 RID: 23939 RVA: 0x00153808 File Offset: 0x00152808
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxCanUndoDescr")]
		public bool CanUndo
		{
			get
			{
				return this.TextBox.CanUndo;
			}
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x06005D84 RID: 23940 RVA: 0x00153815 File Offset: 0x00152815
		// (set) Token: 0x06005D85 RID: 23941 RVA: 0x00153822 File Offset: 0x00152822
		[SRCategory("CatBehavior")]
		[DefaultValue(CharacterCasing.Normal)]
		[SRDescription("TextBoxCharacterCasingDescr")]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return this.TextBox.CharacterCasing;
			}
			set
			{
				this.TextBox.CharacterCasing = value;
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06005D86 RID: 23942 RVA: 0x00153830 File Offset: 0x00152830
		// (set) Token: 0x06005D87 RID: 23943 RVA: 0x0015383D File Offset: 0x0015283D
		[SRDescription("TextBoxHideSelectionDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool HideSelection
		{
			get
			{
				return this.TextBox.HideSelection;
			}
			set
			{
				this.TextBox.HideSelection = value;
			}
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06005D88 RID: 23944 RVA: 0x0015384B File Offset: 0x0015284B
		// (set) Token: 0x06005D89 RID: 23945 RVA: 0x00153858 File Offset: 0x00152858
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("TextBoxLinesDescr")]
		[Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string[] Lines
		{
			get
			{
				return this.TextBox.Lines;
			}
			set
			{
				this.TextBox.Lines = value;
			}
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x06005D8A RID: 23946 RVA: 0x00153866 File Offset: 0x00152866
		// (set) Token: 0x06005D8B RID: 23947 RVA: 0x00153873 File Offset: 0x00152873
		[Localizable(true)]
		[DefaultValue(32767)]
		[SRCategory("CatBehavior")]
		[SRDescription("TextBoxMaxLengthDescr")]
		public int MaxLength
		{
			get
			{
				return this.TextBox.MaxLength;
			}
			set
			{
				this.TextBox.MaxLength = value;
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x06005D8C RID: 23948 RVA: 0x00153881 File Offset: 0x00152881
		// (set) Token: 0x06005D8D RID: 23949 RVA: 0x0015388E File Offset: 0x0015288E
		[SRDescription("TextBoxModifiedDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatBehavior")]
		public bool Modified
		{
			get
			{
				return this.TextBox.Modified;
			}
			set
			{
				this.TextBox.Modified = value;
			}
		}

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06005D8E RID: 23950 RVA: 0x0015389C File Offset: 0x0015289C
		// (set) Token: 0x06005D8F RID: 23951 RVA: 0x001538A9 File Offset: 0x001528A9
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("TextBoxMultilineDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool Multiline
		{
			get
			{
				return this.TextBox.Multiline;
			}
			set
			{
				this.TextBox.Multiline = value;
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06005D90 RID: 23952 RVA: 0x001538B7 File Offset: 0x001528B7
		// (set) Token: 0x06005D91 RID: 23953 RVA: 0x001538C4 File Offset: 0x001528C4
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("TextBoxReadOnlyDescr")]
		public bool ReadOnly
		{
			get
			{
				return this.TextBox.ReadOnly;
			}
			set
			{
				this.TextBox.ReadOnly = value;
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06005D92 RID: 23954 RVA: 0x001538D2 File Offset: 0x001528D2
		// (set) Token: 0x06005D93 RID: 23955 RVA: 0x001538DF File Offset: 0x001528DF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("TextBoxSelectedTextDescr")]
		public string SelectedText
		{
			get
			{
				return this.TextBox.SelectedText;
			}
			set
			{
				this.TextBox.SelectedText = value;
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06005D94 RID: 23956 RVA: 0x001538ED File Offset: 0x001528ED
		// (set) Token: 0x06005D95 RID: 23957 RVA: 0x001538FA File Offset: 0x001528FA
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionLengthDescr")]
		public int SelectionLength
		{
			get
			{
				return this.TextBox.SelectionLength;
			}
			set
			{
				this.TextBox.SelectionLength = value;
			}
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06005D96 RID: 23958 RVA: 0x00153908 File Offset: 0x00152908
		// (set) Token: 0x06005D97 RID: 23959 RVA: 0x00153915 File Offset: 0x00152915
		[SRDescription("TextBoxSelectionStartDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		public int SelectionStart
		{
			get
			{
				return this.TextBox.SelectionStart;
			}
			set
			{
				this.TextBox.SelectionStart = value;
			}
		}

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06005D98 RID: 23960 RVA: 0x00153923 File Offset: 0x00152923
		// (set) Token: 0x06005D99 RID: 23961 RVA: 0x00153930 File Offset: 0x00152930
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TextBoxShortcutsEnabledDescr")]
		public bool ShortcutsEnabled
		{
			get
			{
				return this.TextBox.ShortcutsEnabled;
			}
			set
			{
				this.TextBox.ShortcutsEnabled = value;
			}
		}

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06005D9A RID: 23962 RVA: 0x0015393E File Offset: 0x0015293E
		[Browsable(false)]
		public int TextLength
		{
			get
			{
				return this.TextBox.TextLength;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06005D9B RID: 23963 RVA: 0x0015394B File Offset: 0x0015294B
		// (set) Token: 0x06005D9C RID: 23964 RVA: 0x00153958 File Offset: 0x00152958
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(HorizontalAlignment.Left)]
		[SRDescription("TextBoxTextAlignDescr")]
		public HorizontalAlignment TextBoxTextAlign
		{
			get
			{
				return this.TextBox.TextAlign;
			}
			set
			{
				this.TextBox.TextAlign = value;
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06005D9D RID: 23965 RVA: 0x00153966 File Offset: 0x00152966
		// (set) Token: 0x06005D9E RID: 23966 RVA: 0x00153973 File Offset: 0x00152973
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TextBoxWordWrapDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool WordWrap
		{
			get
			{
				return this.TextBox.WordWrap;
			}
			set
			{
				this.TextBox.WordWrap = value;
			}
		}

		// Token: 0x14000384 RID: 900
		// (add) Token: 0x06005D9F RID: 23967 RVA: 0x00153981 File Offset: 0x00152981
		// (remove) Token: 0x06005DA0 RID: 23968 RVA: 0x00153994 File Offset: 0x00152994
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnAcceptsTabChangedDescr")]
		public event EventHandler AcceptsTabChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventAcceptsTabChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventAcceptsTabChanged, value);
			}
		}

		// Token: 0x14000385 RID: 901
		// (add) Token: 0x06005DA1 RID: 23969 RVA: 0x001539A7 File Offset: 0x001529A7
		// (remove) Token: 0x06005DA2 RID: 23970 RVA: 0x001539BA File Offset: 0x001529BA
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnBorderStyleChangedDescr")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventBorderStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventBorderStyleChanged, value);
			}
		}

		// Token: 0x14000386 RID: 902
		// (add) Token: 0x06005DA3 RID: 23971 RVA: 0x001539CD File Offset: 0x001529CD
		// (remove) Token: 0x06005DA4 RID: 23972 RVA: 0x001539E0 File Offset: 0x001529E0
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnHideSelectionChangedDescr")]
		public event EventHandler HideSelectionChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventHideSelectionChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventHideSelectionChanged, value);
			}
		}

		// Token: 0x14000387 RID: 903
		// (add) Token: 0x06005DA5 RID: 23973 RVA: 0x001539F3 File Offset: 0x001529F3
		// (remove) Token: 0x06005DA6 RID: 23974 RVA: 0x00153A06 File Offset: 0x00152A06
		[SRDescription("TextBoxBaseOnModifiedChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ModifiedChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventModifiedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventModifiedChanged, value);
			}
		}

		// Token: 0x14000388 RID: 904
		// (add) Token: 0x06005DA7 RID: 23975 RVA: 0x00153A19 File Offset: 0x00152A19
		// (remove) Token: 0x06005DA8 RID: 23976 RVA: 0x00153A2C File Offset: 0x00152A2C
		[SRCategory("CatPropertyChanged")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRDescription("TextBoxBaseOnMultilineChangedDescr")]
		[Browsable(false)]
		public event EventHandler MultilineChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventMultilineChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventMultilineChanged, value);
			}
		}

		// Token: 0x14000389 RID: 905
		// (add) Token: 0x06005DA9 RID: 23977 RVA: 0x00153A3F File Offset: 0x00152A3F
		// (remove) Token: 0x06005DAA RID: 23978 RVA: 0x00153A52 File Offset: 0x00152A52
		[SRDescription("TextBoxBaseOnReadOnlyChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventReadOnlyChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventReadOnlyChanged, value);
			}
		}

		// Token: 0x1400038A RID: 906
		// (add) Token: 0x06005DAB RID: 23979 RVA: 0x00153A65 File Offset: 0x00152A65
		// (remove) Token: 0x06005DAC RID: 23980 RVA: 0x00153A78 File Offset: 0x00152A78
		[SRDescription("ToolStripTextBoxTextBoxTextAlignChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler TextBoxTextAlignChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventTextBoxTextAlignChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventTextBoxTextAlignChanged, value);
			}
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x00153A8B File Offset: 0x00152A8B
		public void AppendText(string text)
		{
			this.TextBox.AppendText(text);
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x00153A99 File Offset: 0x00152A99
		public void Clear()
		{
			this.TextBox.Clear();
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x00153AA6 File Offset: 0x00152AA6
		public void ClearUndo()
		{
			this.TextBox.ClearUndo();
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x00153AB3 File Offset: 0x00152AB3
		public void Copy()
		{
			this.TextBox.Copy();
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x00153AC0 File Offset: 0x00152AC0
		public void Cut()
		{
			this.TextBox.Copy();
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x00153ACD File Offset: 0x00152ACD
		public void DeselectAll()
		{
			this.TextBox.DeselectAll();
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x00153ADA File Offset: 0x00152ADA
		public char GetCharFromPosition(Point pt)
		{
			return this.TextBox.GetCharFromPosition(pt);
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x00153AE8 File Offset: 0x00152AE8
		public int GetCharIndexFromPosition(Point pt)
		{
			return this.TextBox.GetCharIndexFromPosition(pt);
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x00153AF6 File Offset: 0x00152AF6
		public int GetFirstCharIndexFromLine(int lineNumber)
		{
			return this.TextBox.GetFirstCharIndexFromLine(lineNumber);
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x00153B04 File Offset: 0x00152B04
		public int GetFirstCharIndexOfCurrentLine()
		{
			return this.TextBox.GetFirstCharIndexOfCurrentLine();
		}

		// Token: 0x06005DB7 RID: 23991 RVA: 0x00153B11 File Offset: 0x00152B11
		public int GetLineFromCharIndex(int index)
		{
			return this.TextBox.GetLineFromCharIndex(index);
		}

		// Token: 0x06005DB8 RID: 23992 RVA: 0x00153B1F File Offset: 0x00152B1F
		public Point GetPositionFromCharIndex(int index)
		{
			return this.TextBox.GetPositionFromCharIndex(index);
		}

		// Token: 0x06005DB9 RID: 23993 RVA: 0x00153B2D File Offset: 0x00152B2D
		public void Paste()
		{
			this.TextBox.Paste();
		}

		// Token: 0x06005DBA RID: 23994 RVA: 0x00153B3A File Offset: 0x00152B3A
		public void ScrollToCaret()
		{
			this.TextBox.ScrollToCaret();
		}

		// Token: 0x06005DBB RID: 23995 RVA: 0x00153B47 File Offset: 0x00152B47
		public void Select(int start, int length)
		{
			this.TextBox.Select(start, length);
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x00153B56 File Offset: 0x00152B56
		public void SelectAll()
		{
			this.TextBox.SelectAll();
		}

		// Token: 0x06005DBD RID: 23997 RVA: 0x00153B63 File Offset: 0x00152B63
		public void Undo()
		{
			this.TextBox.Undo();
		}

		// Token: 0x04003953 RID: 14675
		internal static readonly object EventTextBoxTextAlignChanged = new object();

		// Token: 0x04003954 RID: 14676
		internal static readonly object EventAcceptsTabChanged = new object();

		// Token: 0x04003955 RID: 14677
		internal static readonly object EventBorderStyleChanged = new object();

		// Token: 0x04003956 RID: 14678
		internal static readonly object EventHideSelectionChanged = new object();

		// Token: 0x04003957 RID: 14679
		internal static readonly object EventReadOnlyChanged = new object();

		// Token: 0x04003958 RID: 14680
		internal static readonly object EventMultilineChanged = new object();

		// Token: 0x04003959 RID: 14681
		internal static readonly object EventModifiedChanged = new object();

		// Token: 0x020006EA RID: 1770
		private class ToolStripTextBoxControl : TextBox
		{
			// Token: 0x06005DBF RID: 23999 RVA: 0x00153BC3 File Offset: 0x00152BC3
			public ToolStripTextBoxControl()
			{
				this.Font = ToolStripManager.DefaultFont;
				this.isFontSet = false;
			}

			// Token: 0x170013BE RID: 5054
			// (get) Token: 0x06005DC0 RID: 24000 RVA: 0x00153BE4 File Offset: 0x00152BE4
			private NativeMethods.RECT AbsoluteClientRECT
			{
				get
				{
					NativeMethods.RECT result = default(NativeMethods.RECT);
					CreateParams createParams = this.CreateParams;
					SafeNativeMethods.AdjustWindowRectEx(ref result, createParams.Style, this.HasMenu, createParams.ExStyle);
					int num = -result.left;
					int num2 = -result.top;
					UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref result);
					result.left += num;
					result.right += num;
					result.top += num2;
					result.bottom += num2;
					return result;
				}
			}

			// Token: 0x170013BF RID: 5055
			// (get) Token: 0x06005DC1 RID: 24001 RVA: 0x00153C80 File Offset: 0x00152C80
			private Rectangle AbsoluteClientRectangle
			{
				get
				{
					NativeMethods.RECT absoluteClientRECT = this.AbsoluteClientRECT;
					return Rectangle.FromLTRB(absoluteClientRECT.top, absoluteClientRECT.top, absoluteClientRECT.right, absoluteClientRECT.bottom);
				}
			}

			// Token: 0x170013C0 RID: 5056
			// (get) Token: 0x06005DC2 RID: 24002 RVA: 0x00153CB8 File Offset: 0x00152CB8
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

			// Token: 0x170013C1 RID: 5057
			// (get) Token: 0x06005DC3 RID: 24003 RVA: 0x00153CED File Offset: 0x00152CED
			private bool IsPopupTextBox
			{
				get
				{
					return base.BorderStyle == BorderStyle.Fixed3D && this.Owner != null && this.Owner.Renderer is ToolStripProfessionalRenderer;
				}
			}

			// Token: 0x170013C2 RID: 5058
			// (get) Token: 0x06005DC4 RID: 24004 RVA: 0x00153D17 File Offset: 0x00152D17
			// (set) Token: 0x06005DC5 RID: 24005 RVA: 0x00153D1F File Offset: 0x00152D1F
			internal bool MouseIsOver
			{
				get
				{
					return this.mouseIsOver;
				}
				set
				{
					if (this.mouseIsOver != value)
					{
						this.mouseIsOver = value;
						if (!this.Focused)
						{
							this.InvalidateNonClient();
						}
					}
				}
			}

			// Token: 0x170013C3 RID: 5059
			// (get) Token: 0x06005DC6 RID: 24006 RVA: 0x00153D3F File Offset: 0x00152D3F
			// (set) Token: 0x06005DC7 RID: 24007 RVA: 0x00153D47 File Offset: 0x00152D47
			public override Font Font
			{
				get
				{
					return base.Font;
				}
				set
				{
					base.Font = value;
					this.isFontSet = this.ShouldSerializeFont();
				}
			}

			// Token: 0x170013C4 RID: 5060
			// (get) Token: 0x06005DC8 RID: 24008 RVA: 0x00153D5C File Offset: 0x00152D5C
			// (set) Token: 0x06005DC9 RID: 24009 RVA: 0x00153D64 File Offset: 0x00152D64
			public ToolStripTextBox Owner
			{
				get
				{
					return this.ownerItem;
				}
				set
				{
					this.ownerItem = value;
				}
			}

			// Token: 0x06005DCA RID: 24010 RVA: 0x00153D70 File Offset: 0x00152D70
			private void InvalidateNonClient()
			{
				if (!this.IsPopupTextBox)
				{
					return;
				}
				NativeMethods.RECT absoluteClientRECT = this.AbsoluteClientRECT;
				HandleRef handleRef = NativeMethods.NullHandleRef;
				HandleRef handleRef2 = NativeMethods.NullHandleRef;
				HandleRef handleRef3 = NativeMethods.NullHandleRef;
				try
				{
					handleRef3 = new HandleRef(this, SafeNativeMethods.CreateRectRgn(0, 0, base.Width, base.Height));
					handleRef2 = new HandleRef(this, SafeNativeMethods.CreateRectRgn(absoluteClientRECT.left, absoluteClientRECT.top, absoluteClientRECT.right, absoluteClientRECT.bottom));
					handleRef = new HandleRef(this, SafeNativeMethods.CreateRectRgn(0, 0, 0, 0));
					SafeNativeMethods.CombineRgn(handleRef, handleRef3, handleRef2, 3);
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), ref rect, handleRef, 1797);
				}
				finally
				{
					try
					{
						if (handleRef.Handle != IntPtr.Zero)
						{
							SafeNativeMethods.DeleteObject(handleRef);
						}
					}
					finally
					{
						try
						{
							if (handleRef2.Handle != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(handleRef2);
							}
						}
						finally
						{
							if (handleRef3.Handle != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(handleRef3);
							}
						}
					}
				}
			}

			// Token: 0x06005DCB RID: 24011 RVA: 0x00153E9C File Offset: 0x00152E9C
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.InvalidateNonClient();
			}

			// Token: 0x06005DCC RID: 24012 RVA: 0x00153EAB File Offset: 0x00152EAB
			protected override void OnLostFocus(EventArgs e)
			{
				base.OnLostFocus(e);
				this.InvalidateNonClient();
			}

			// Token: 0x06005DCD RID: 24013 RVA: 0x00153EBA File Offset: 0x00152EBA
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
				this.MouseIsOver = true;
			}

			// Token: 0x06005DCE RID: 24014 RVA: 0x00153ECA File Offset: 0x00152ECA
			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				this.MouseIsOver = false;
			}

			// Token: 0x06005DCF RID: 24015 RVA: 0x00153EDC File Offset: 0x00152EDC
			private void HookStaticEvents(bool hook)
			{
				if (hook)
				{
					try
					{
						SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
						return;
					}
					finally
					{
						this.alreadyHooked = true;
					}
				}
				if (this.alreadyHooked)
				{
					try
					{
						SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
					}
					finally
					{
						this.alreadyHooked = false;
					}
				}
			}

			// Token: 0x06005DD0 RID: 24016 RVA: 0x00153F48 File Offset: 0x00152F48
			private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
			{
				if (e.Category == UserPreferenceCategory.Window && !this.isFontSet)
				{
					this.Font = ToolStripManager.DefaultFont;
				}
			}

			// Token: 0x06005DD1 RID: 24017 RVA: 0x00153F67 File Offset: 0x00152F67
			protected override void OnVisibleChanged(EventArgs e)
			{
				base.OnVisibleChanged(e);
				if (!base.Disposing && !base.IsDisposed)
				{
					this.HookStaticEvents(base.Visible);
				}
			}

			// Token: 0x06005DD2 RID: 24018 RVA: 0x00153F8C File Offset: 0x00152F8C
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.HookStaticEvents(false);
				}
				base.Dispose(disposing);
			}

			// Token: 0x06005DD3 RID: 24019 RVA: 0x00153FA0 File Offset: 0x00152FA0
			private void WmNCPaint(ref Message m)
			{
				if (!this.IsPopupTextBox)
				{
					base.WndProc(ref m);
					return;
				}
				HandleRef hDC = new HandleRef(this, UnsafeNativeMethods.GetWindowDC(new HandleRef(this, m.HWnd)));
				if (hDC.Handle == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				try
				{
					Color color = (this.MouseIsOver || this.Focused) ? this.ColorTable.TextBoxBorder : this.BackColor;
					Color color2 = this.BackColor;
					if (!base.Enabled)
					{
						color = SystemColors.ControlDark;
						color2 = SystemColors.Control;
					}
					using (Graphics graphics = Graphics.FromHdcInternal(hDC.Handle))
					{
						Rectangle absoluteClientRectangle = this.AbsoluteClientRectangle;
						using (Brush brush = new SolidBrush(color2))
						{
							graphics.FillRectangle(brush, 0, 0, base.Width, absoluteClientRectangle.Top);
							graphics.FillRectangle(brush, 0, 0, absoluteClientRectangle.Left, base.Height);
							graphics.FillRectangle(brush, 0, absoluteClientRectangle.Bottom, base.Width, base.Height - absoluteClientRectangle.Height);
							graphics.FillRectangle(brush, absoluteClientRectangle.Right, 0, base.Width - absoluteClientRectangle.Right, base.Height);
						}
						using (Pen pen = new Pen(color))
						{
							graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
						}
					}
				}
				finally
				{
					UnsafeNativeMethods.ReleaseDC(new HandleRef(this, base.Handle), hDC);
				}
				m.Result = IntPtr.Zero;
			}

			// Token: 0x06005DD4 RID: 24020 RVA: 0x00154190 File Offset: 0x00153190
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 133)
				{
					this.WmNCPaint(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x0400395A RID: 14682
			private bool mouseIsOver;

			// Token: 0x0400395B RID: 14683
			private ToolStripTextBox ownerItem;

			// Token: 0x0400395C RID: 14684
			private bool isFontSet = true;

			// Token: 0x0400395D RID: 14685
			private bool alreadyHooked;
		}
	}
}
