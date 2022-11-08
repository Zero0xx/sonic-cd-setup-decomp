using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006E1 RID: 1761
	[DefaultEvent("ButtonClick")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripSplitButton : ToolStripDropDownItem
	{
		// Token: 0x06005CE5 RID: 23781 RVA: 0x00151931 File Offset: 0x00150931
		public ToolStripSplitButton()
		{
			this.Initialize();
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x0015195C File Offset: 0x0015095C
		public ToolStripSplitButton(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		// Token: 0x06005CE7 RID: 23783 RVA: 0x0015198A File Offset: 0x0015098A
		public ToolStripSplitButton(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x001519B8 File Offset: 0x001509B8
		public ToolStripSplitButton(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x001519E6 File Offset: 0x001509E6
		public ToolStripSplitButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x00151A14 File Offset: 0x00150A14
		public ToolStripSplitButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x00151A44 File Offset: 0x00150A44
		public ToolStripSplitButton(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
		{
			this.Initialize();
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06005CEC RID: 23788 RVA: 0x00151A72 File Offset: 0x00150A72
		// (set) Token: 0x06005CED RID: 23789 RVA: 0x00151A7A File Offset: 0x00150A7A
		[DefaultValue(true)]
		public new bool AutoToolTip
		{
			get
			{
				return base.AutoToolTip;
			}
			set
			{
				base.AutoToolTip = value;
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06005CEE RID: 23790 RVA: 0x00151A83 File Offset: 0x00150A83
		[Browsable(false)]
		public Rectangle ButtonBounds
		{
			get
			{
				return this.SplitButtonButton.Bounds;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06005CEF RID: 23791 RVA: 0x00151A90 File Offset: 0x00150A90
		[Browsable(false)]
		public bool ButtonPressed
		{
			get
			{
				return this.SplitButtonButton.Pressed;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06005CF0 RID: 23792 RVA: 0x00151A9D File Offset: 0x00150A9D
		[Browsable(false)]
		public bool ButtonSelected
		{
			get
			{
				return this.SplitButtonButton.Selected || this.DropDownButtonPressed;
			}
		}

		// Token: 0x14000381 RID: 897
		// (add) Token: 0x06005CF1 RID: 23793 RVA: 0x00151AB4 File Offset: 0x00150AB4
		// (remove) Token: 0x06005CF2 RID: 23794 RVA: 0x00151AC7 File Offset: 0x00150AC7
		[SRCategory("CatAction")]
		[SRDescription("ToolStripSplitButtonOnButtonClickDescr")]
		public event EventHandler ButtonClick
		{
			add
			{
				base.Events.AddHandler(ToolStripSplitButton.EventButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripSplitButton.EventButtonClick, value);
			}
		}

		// Token: 0x14000382 RID: 898
		// (add) Token: 0x06005CF3 RID: 23795 RVA: 0x00151ADA File Offset: 0x00150ADA
		// (remove) Token: 0x06005CF4 RID: 23796 RVA: 0x00151AED File Offset: 0x00150AED
		[SRDescription("ToolStripSplitButtonOnButtonDoubleClickDescr")]
		[SRCategory("CatAction")]
		public event EventHandler ButtonDoubleClick
		{
			add
			{
				base.Events.AddHandler(ToolStripSplitButton.EventButtonDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripSplitButton.EventButtonDoubleClick, value);
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06005CF5 RID: 23797 RVA: 0x00151B00 File Offset: 0x00150B00
		protected override bool DefaultAutoToolTip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06005CF6 RID: 23798 RVA: 0x00151B03 File Offset: 0x00150B03
		// (set) Token: 0x06005CF7 RID: 23799 RVA: 0x00151B0B File Offset: 0x00150B0B
		[DefaultValue(null)]
		[Browsable(false)]
		public ToolStripItem DefaultItem
		{
			get
			{
				return this.defaultItem;
			}
			set
			{
				if (this.defaultItem != value)
				{
					this.OnDefaultItemChanged(new EventArgs());
					this.defaultItem = value;
				}
			}
		}

		// Token: 0x14000383 RID: 899
		// (add) Token: 0x06005CF8 RID: 23800 RVA: 0x00151B28 File Offset: 0x00150B28
		// (remove) Token: 0x06005CF9 RID: 23801 RVA: 0x00151B3B File Offset: 0x00150B3B
		[SRDescription("ToolStripSplitButtonOnDefaultItemChangedDescr")]
		[SRCategory("CatAction")]
		public event EventHandler DefaultItemChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripSplitButton.EventDefaultItemChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripSplitButton.EventDefaultItemChanged, value);
			}
		}

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06005CFA RID: 23802 RVA: 0x00151B4E File Offset: 0x00150B4E
		protected internal override bool DismissWhenClicked
		{
			get
			{
				return !base.DropDown.Visible;
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06005CFB RID: 23803 RVA: 0x00151B5E File Offset: 0x00150B5E
		internal override Rectangle DropDownButtonArea
		{
			get
			{
				return this.DropDownButtonBounds;
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06005CFC RID: 23804 RVA: 0x00151B66 File Offset: 0x00150B66
		[Browsable(false)]
		public Rectangle DropDownButtonBounds
		{
			get
			{
				return this.dropDownButtonBounds;
			}
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x06005CFD RID: 23805 RVA: 0x00151B6E File Offset: 0x00150B6E
		[Browsable(false)]
		public bool DropDownButtonPressed
		{
			get
			{
				return base.DropDown.Visible;
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06005CFE RID: 23806 RVA: 0x00151B7B File Offset: 0x00150B7B
		[Browsable(false)]
		public bool DropDownButtonSelected
		{
			get
			{
				return this.Selected;
			}
		}

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06005CFF RID: 23807 RVA: 0x00151B83 File Offset: 0x00150B83
		// (set) Token: 0x06005D00 RID: 23808 RVA: 0x00151B8C File Offset: 0x00150B8C
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripSplitButtonDropDownButtonWidthDescr")]
		public int DropDownButtonWidth
		{
			get
			{
				return this.dropDownButtonWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("DropDownButtonWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"DropDownButtonWidth",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.dropDownButtonWidth != value)
				{
					this.dropDownButtonWidth = value;
					this.InvalidateSplitButtonLayout();
					base.InvalidateItemLayout(PropertyNames.DropDownButtonWidth, true);
				}
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06005D01 RID: 23809 RVA: 0x00151C04 File Offset: 0x00150C04
		private int DefaultDropDownButtonWidth
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x00151C08 File Offset: 0x00150C08
		private ToolStripSplitButton.ToolStripSplitButtonButton SplitButtonButton
		{
			get
			{
				if (this.splitButtonButton == null)
				{
					this.splitButtonButton = new ToolStripSplitButton.ToolStripSplitButtonButton(this);
				}
				this.splitButtonButton.Image = this.Image;
				this.splitButtonButton.Text = this.Text;
				this.splitButtonButton.BackColor = this.BackColor;
				this.splitButtonButton.ForeColor = this.ForeColor;
				this.splitButtonButton.Font = this.Font;
				this.splitButtonButton.ImageAlign = base.ImageAlign;
				this.splitButtonButton.TextAlign = this.TextAlign;
				this.splitButtonButton.TextImageRelation = base.TextImageRelation;
				return this.splitButtonButton;
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06005D03 RID: 23811 RVA: 0x00151CB7 File Offset: 0x00150CB7
		internal ToolStripItemInternalLayout SplitButtonButtonLayout
		{
			get
			{
				if (base.InternalLayout != null && this.splitButtonButtonLayout == null)
				{
					this.splitButtonButtonLayout = new ToolStripSplitButton.ToolStripSplitButtonButtonLayout(this);
				}
				return this.splitButtonButtonLayout;
			}
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x00151CDB File Offset: 0x00150CDB
		// (set) Token: 0x06005D05 RID: 23813 RVA: 0x00151CE3 File Offset: 0x00150CE3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[SRDescription("ToolStripSplitButtonSplitterWidthDescr")]
		internal int SplitterWidth
		{
			get
			{
				return this.splitterWidth;
			}
			set
			{
				if (value < 0)
				{
					this.splitterWidth = 0;
				}
				else
				{
					this.splitterWidth = value;
				}
				this.InvalidateSplitButtonLayout();
			}
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06005D06 RID: 23814 RVA: 0x00151CFF File Offset: 0x00150CFF
		[Browsable(false)]
		public Rectangle SplitterBounds
		{
			get
			{
				return this.splitterBounds;
			}
		}

		// Token: 0x06005D07 RID: 23815 RVA: 0x00151D08 File Offset: 0x00150D08
		private void CalculateLayout()
		{
			Rectangle rectangle = new Rectangle(Point.Empty, this.Size);
			Rectangle empty = Rectangle.Empty;
			rectangle = new Rectangle(Point.Empty, new Size(Math.Min(base.Width, this.DropDownButtonWidth), base.Height));
			int width = Math.Max(0, base.Width - rectangle.Width);
			int height = Math.Max(0, base.Height);
			empty = new Rectangle(Point.Empty, new Size(width, height));
			empty.Width -= this.splitterWidth;
			if (this.RightToLeft == RightToLeft.No)
			{
				rectangle.Offset(empty.Right + this.splitterWidth, 0);
				this.splitterBounds = new Rectangle(empty.Right, empty.Top, this.splitterWidth, empty.Height);
			}
			else
			{
				empty.Offset(this.DropDownButtonWidth + this.splitterWidth, 0);
				this.splitterBounds = new Rectangle(rectangle.Right, rectangle.Top, this.splitterWidth, rectangle.Height);
			}
			this.SplitButtonButton.SetBounds(empty);
			this.SetDropDownButtonBounds(rectangle);
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x00151E32 File Offset: 0x00150E32
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripSplitButton.ToolStripSplitButtonAccessibleObject(this);
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x00151E3A File Offset: 0x00150E3A
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDownMenu(this, true);
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x00151E43 File Offset: 0x00150E43
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			this.splitButtonButtonLayout = null;
			return new ToolStripItemInternalLayout(this);
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x00151E54 File Offset: 0x00150E54
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size preferredSize = this.SplitButtonButtonLayout.GetPreferredSize(constrainingSize);
			preferredSize.Width += this.DropDownButtonWidth + this.SplitterWidth + this.Padding.Horizontal;
			return preferredSize;
		}

		// Token: 0x06005D0C RID: 23820 RVA: 0x00151E99 File Offset: 0x00150E99
		private void InvalidateSplitButtonLayout()
		{
			this.splitButtonButtonLayout = null;
			this.CalculateLayout();
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x00151EA8 File Offset: 0x00150EA8
		private void Initialize()
		{
			this.dropDownButtonWidth = this.DefaultDropDownButtonWidth;
			base.SupportsSpaceKey = true;
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x00151EBD File Offset: 0x00150EBD
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessDialogKey(Keys keyData)
		{
			if (this.Enabled && (keyData == Keys.Return || (base.SupportsSpaceKey && keyData == Keys.Space)))
			{
				this.PerformButtonClick();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06005D0F RID: 23823 RVA: 0x00151EE8 File Offset: 0x00150EE8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			this.PerformButtonClick();
			return true;
		}

		// Token: 0x06005D10 RID: 23824 RVA: 0x00151EF4 File Offset: 0x00150EF4
		protected virtual void OnButtonClick(EventArgs e)
		{
			if (this.DefaultItem != null)
			{
				this.DefaultItem.FireEvent(ToolStripItemEventType.Click);
			}
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripSplitButton.EventButtonClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x00151F38 File Offset: 0x00150F38
		public virtual void OnButtonDoubleClick(EventArgs e)
		{
			if (this.DefaultItem != null)
			{
				this.DefaultItem.FireEvent(ToolStripItemEventType.DoubleClick);
			}
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripSplitButton.EventButtonDoubleClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x00151F7C File Offset: 0x00150F7C
		protected virtual void OnDefaultItemChanged(EventArgs e)
		{
			this.InvalidateSplitButtonLayout();
			if (this.CanRaiseEvents)
			{
				EventHandler eventHandler = base.Events[ToolStripSplitButton.EventDefaultItemChanged] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x00151FB8 File Offset: 0x00150FB8
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (this.DropDownButtonBounds.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left && !base.DropDown.Visible)
				{
					this.openMouseId = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					base.ShowDropDown(true);
					return;
				}
			}
			else
			{
				this.SplitButtonButton.Push(true);
			}
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x00152028 File Offset: 0x00151028
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!this.Enabled)
			{
				return;
			}
			this.SplitButtonButton.Push(false);
			if (this.DropDownButtonBounds.Contains(e.Location) && e.Button == MouseButtons.Left && base.DropDown.Visible)
			{
				byte b = (base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId();
				if (b != this.openMouseId)
				{
					this.openMouseId = 0;
					ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
					base.Select();
				}
			}
			Point pt = new Point(e.X, e.Y);
			if (e.Button == MouseButtons.Left && this.SplitButtonButton.Bounds.Contains(pt))
			{
				bool flag = false;
				if (base.DoubleClickEnabled)
				{
					long ticks = DateTime.Now.Ticks;
					long num = ticks - this.lastClickTime;
					this.lastClickTime = ticks;
					if (num >= 0L && num < ToolStripItem.DoubleClickTicks)
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.OnButtonDoubleClick(new EventArgs());
					this.lastClickTime = 0L;
					return;
				}
				this.OnButtonClick(new EventArgs());
			}
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x00152148 File Offset: 0x00151148
		protected override void OnMouseLeave(EventArgs e)
		{
			this.openMouseId = 0;
			this.SplitButtonButton.Push(false);
			base.OnMouseLeave(e);
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x00152164 File Offset: 0x00151164
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			this.InvalidateSplitButtonLayout();
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x00152174 File Offset: 0x00151174
		protected override void OnPaint(PaintEventArgs e)
		{
			ToolStripRenderer renderer = base.Renderer;
			if (renderer != null)
			{
				this.InvalidateSplitButtonLayout();
				Graphics graphics = e.Graphics;
				renderer.DrawSplitButton(new ToolStripItemRenderEventArgs(graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) != ToolStripItemDisplayStyle.None)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, this.SplitButtonButtonLayout.ImageRectangle));
				}
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) != ToolStripItemDisplayStyle.None)
				{
					renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.SplitButtonButton.Text, this.SplitButtonButtonLayout.TextRectangle, this.ForeColor, this.Font, this.SplitButtonButtonLayout.TextFormat));
				}
			}
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x0015220E File Offset: 0x0015120E
		public void PerformButtonClick()
		{
			if (this.Enabled && base.Available)
			{
				base.PerformClick();
				this.OnButtonClick(EventArgs.Empty);
			}
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x00152231 File Offset: 0x00151231
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetDropDownButtonWidth()
		{
			this.DropDownButtonWidth = this.DefaultDropDownButtonWidth;
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x0015223F File Offset: 0x0015123F
		private void SetDropDownButtonBounds(Rectangle rect)
		{
			this.dropDownButtonBounds = rect;
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x00152248 File Offset: 0x00151248
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeDropDownButtonWidth()
		{
			return this.DropDownButtonWidth != this.DefaultDropDownButtonWidth;
		}

		// Token: 0x0400392E RID: 14638
		private const int DEFAULT_DROPDOWN_WIDTH = 11;

		// Token: 0x0400392F RID: 14639
		private ToolStripItem defaultItem;

		// Token: 0x04003930 RID: 14640
		private ToolStripSplitButton.ToolStripSplitButtonButton splitButtonButton;

		// Token: 0x04003931 RID: 14641
		private Rectangle dropDownButtonBounds = Rectangle.Empty;

		// Token: 0x04003932 RID: 14642
		private ToolStripSplitButton.ToolStripSplitButtonButtonLayout splitButtonButtonLayout;

		// Token: 0x04003933 RID: 14643
		private int dropDownButtonWidth;

		// Token: 0x04003934 RID: 14644
		private int splitterWidth = 1;

		// Token: 0x04003935 RID: 14645
		private Rectangle splitterBounds = Rectangle.Empty;

		// Token: 0x04003936 RID: 14646
		private byte openMouseId;

		// Token: 0x04003937 RID: 14647
		private long lastClickTime;

		// Token: 0x04003938 RID: 14648
		private static readonly object EventDefaultItemChanged = new object();

		// Token: 0x04003939 RID: 14649
		private static readonly object EventButtonClick = new object();

		// Token: 0x0400393A RID: 14650
		private static readonly object EventButtonDoubleClick = new object();

		// Token: 0x0400393B RID: 14651
		private static readonly object EventDropDownOpened = new object();

		// Token: 0x0400393C RID: 14652
		private static readonly object EventDropDownClosed = new object();

		// Token: 0x020006E2 RID: 1762
		private class ToolStripSplitButtonButton : ToolStripButton
		{
			// Token: 0x06005D1D RID: 23837 RVA: 0x0015228F File Offset: 0x0015128F
			public ToolStripSplitButtonButton(ToolStripSplitButton owner)
			{
				this.owner = owner;
			}

			// Token: 0x1700138E RID: 5006
			// (get) Token: 0x06005D1E RID: 23838 RVA: 0x0015229E File Offset: 0x0015129E
			// (set) Token: 0x06005D1F RID: 23839 RVA: 0x001522AB File Offset: 0x001512AB
			public override bool Enabled
			{
				get
				{
					return this.owner.Enabled;
				}
				set
				{
				}
			}

			// Token: 0x1700138F RID: 5007
			// (get) Token: 0x06005D20 RID: 23840 RVA: 0x001522AD File Offset: 0x001512AD
			// (set) Token: 0x06005D21 RID: 23841 RVA: 0x001522BA File Offset: 0x001512BA
			public override ToolStripItemDisplayStyle DisplayStyle
			{
				get
				{
					return this.owner.DisplayStyle;
				}
				set
				{
				}
			}

			// Token: 0x17001390 RID: 5008
			// (get) Token: 0x06005D22 RID: 23842 RVA: 0x001522BC File Offset: 0x001512BC
			// (set) Token: 0x06005D23 RID: 23843 RVA: 0x001522C9 File Offset: 0x001512C9
			public override Padding Padding
			{
				get
				{
					return this.owner.Padding;
				}
				set
				{
				}
			}

			// Token: 0x17001391 RID: 5009
			// (get) Token: 0x06005D24 RID: 23844 RVA: 0x001522CB File Offset: 0x001512CB
			public override ToolStripTextDirection TextDirection
			{
				get
				{
					return this.owner.TextDirection;
				}
			}

			// Token: 0x17001392 RID: 5010
			// (get) Token: 0x06005D25 RID: 23845 RVA: 0x001522D8 File Offset: 0x001512D8
			// (set) Token: 0x06005D26 RID: 23846 RVA: 0x001522F7 File Offset: 0x001512F7
			public override Image Image
			{
				get
				{
					if ((this.owner.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
					{
						return this.owner.Image;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x17001393 RID: 5011
			// (get) Token: 0x06005D27 RID: 23847 RVA: 0x001522F9 File Offset: 0x001512F9
			public override bool Selected
			{
				get
				{
					if (this.owner != null)
					{
						return this.owner.Selected;
					}
					return base.Selected;
				}
			}

			// Token: 0x17001394 RID: 5012
			// (get) Token: 0x06005D28 RID: 23848 RVA: 0x00152315 File Offset: 0x00151315
			// (set) Token: 0x06005D29 RID: 23849 RVA: 0x00152334 File Offset: 0x00151334
			public override string Text
			{
				get
				{
					if ((this.owner.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						return this.owner.Text;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x0400393D RID: 14653
			private ToolStripSplitButton owner;
		}

		// Token: 0x020006E3 RID: 1763
		private class ToolStripSplitButtonButtonLayout : ToolStripItemInternalLayout
		{
			// Token: 0x06005D2A RID: 23850 RVA: 0x00152336 File Offset: 0x00151336
			public ToolStripSplitButtonButtonLayout(ToolStripSplitButton owner) : base(owner.SplitButtonButton)
			{
				this.owner = owner;
			}

			// Token: 0x17001395 RID: 5013
			// (get) Token: 0x06005D2B RID: 23851 RVA: 0x0015234B File Offset: 0x0015134B
			protected override ToolStripItem Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17001396 RID: 5014
			// (get) Token: 0x06005D2C RID: 23852 RVA: 0x00152353 File Offset: 0x00151353
			protected override ToolStrip ParentInternal
			{
				get
				{
					return this.owner.ParentInternal;
				}
			}

			// Token: 0x17001397 RID: 5015
			// (get) Token: 0x06005D2D RID: 23853 RVA: 0x00152360 File Offset: 0x00151360
			public override Rectangle ImageRectangle
			{
				get
				{
					Rectangle imageRectangle = base.ImageRectangle;
					imageRectangle.Offset(this.owner.SplitButtonButton.Bounds.Location);
					return imageRectangle;
				}
			}

			// Token: 0x17001398 RID: 5016
			// (get) Token: 0x06005D2E RID: 23854 RVA: 0x00152394 File Offset: 0x00151394
			public override Rectangle TextRectangle
			{
				get
				{
					Rectangle textRectangle = base.TextRectangle;
					textRectangle.Offset(this.owner.SplitButtonButton.Bounds.Location);
					return textRectangle;
				}
			}

			// Token: 0x0400393E RID: 14654
			private ToolStripSplitButton owner;
		}

		// Token: 0x020006E4 RID: 1764
		public class ToolStripSplitButtonAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x06005D2F RID: 23855 RVA: 0x001523C8 File Offset: 0x001513C8
			public ToolStripSplitButtonAccessibleObject(ToolStripSplitButton item) : base(item)
			{
				this.owner = item;
			}

			// Token: 0x06005D30 RID: 23856 RVA: 0x001523D8 File Offset: 0x001513D8
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.owner.PerformButtonClick();
			}

			// Token: 0x0400393F RID: 14655
			private ToolStripSplitButton owner;
		}
	}
}
