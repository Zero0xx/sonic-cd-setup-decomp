using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Internal;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Internal;
using Microsoft.Win32;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007C7 RID: 1991
	internal class PropertyGridView : Control, IWin32Window, IWindowsFormsEditorService, IServiceProvider
	{
		// Token: 0x06006918 RID: 26904 RVA: 0x001826BC File Offset: 0x001816BC
		public PropertyGridView(IServiceProvider serviceProvider, PropertyGrid propertyGrid)
		{
			this.ehValueClick = new EventHandler(this.OnGridEntryValueClick);
			this.ehLabelClick = new EventHandler(this.OnGridEntryLabelClick);
			this.ehOutlineClick = new EventHandler(this.OnGridEntryOutlineClick);
			this.ehValueDblClick = new EventHandler(this.OnGridEntryValueDoubleClick);
			this.ehLabelDblClick = new EventHandler(this.OnGridEntryLabelDoubleClick);
			this.ehRecreateChildren = new GridEntryRecreateChildrenEventHandler(this.OnRecreateChildren);
			this.ownerGrid = propertyGrid;
			this.serviceProvider = serviceProvider;
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, false);
			base.SetStyle(ControlStyles.UserMouse, true);
			this.BackColor = SystemColors.Window;
			this.ForeColor = SystemColors.WindowText;
			this.backgroundBrush = SystemBrushes.Window;
			base.TabStop = true;
			this.Text = "PropertyGridView";
			this.CreateUI();
			this.LayoutWindow(true);
		}

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06006919 RID: 26905 RVA: 0x00182824 File Offset: 0x00181824
		// (set) Token: 0x0600691A RID: 26906 RVA: 0x0018282C File Offset: 0x0018182C
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				this.backgroundBrush = new SolidBrush(value);
				base.BackColor = value;
			}
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x00182841 File Offset: 0x00181841
		internal Brush GetBackgroundBrush(Graphics g)
		{
			return this.backgroundBrush;
		}

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x0600691C RID: 26908 RVA: 0x0018284C File Offset: 0x0018184C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool CanCopy
		{
			get
			{
				if (this.selectedGridEntry == null)
				{
					return false;
				}
				if (!this.Edit.Focused)
				{
					string propertyTextValue = this.selectedGridEntry.GetPropertyTextValue();
					return propertyTextValue != null && propertyTextValue.Length > 0;
				}
				return true;
			}
		}

		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x0600691D RID: 26909 RVA: 0x0018288C File Offset: 0x0018188C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public bool CanCut
		{
			get
			{
				return this.CanCopy && this.selectedGridEntry.IsTextEditable;
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x0600691E RID: 26910 RVA: 0x001828A3 File Offset: 0x001818A3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public bool CanPaste
		{
			get
			{
				return this.selectedGridEntry != null && this.selectedGridEntry.IsTextEditable;
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x0600691F RID: 26911 RVA: 0x001828BA File Offset: 0x001818BA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool CanUndo
		{
			get
			{
				return this.Edit.Visible && this.Edit.Focused && 0 != (int)this.Edit.SendMessage(198, 0, 0);
			}
		}

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x06006920 RID: 26912 RVA: 0x001828F8 File Offset: 0x001818F8
		private DropDownButton DropDownButton
		{
			get
			{
				if (this.btnDropDown == null)
				{
					this.btnDropDown = new DropDownButton();
					this.btnDropDown.UseComboBoxTheme = true;
					Bitmap image = this.CreateDownArrow();
					this.btnDropDown.Image = image;
					this.btnDropDown.BackColor = SystemColors.Control;
					this.btnDropDown.ForeColor = SystemColors.ControlText;
					this.btnDropDown.Click += this.OnBtnClick;
					this.btnDropDown.LostFocus += this.OnChildLostFocus;
					this.btnDropDown.TabIndex = 2;
					this.CommonEditorSetup(this.btnDropDown);
					this.btnDropDown.Size = new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight);
				}
				return this.btnDropDown;
			}
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x06006921 RID: 26913 RVA: 0x001829C4 File Offset: 0x001819C4
		private Button DialogButton
		{
			get
			{
				if (this.btnDialog == null)
				{
					this.btnDialog = new DropDownButton();
					this.btnDialog.BackColor = SystemColors.Control;
					this.btnDialog.ForeColor = SystemColors.ControlText;
					this.btnDialog.TabIndex = 3;
					this.btnDialog.Image = new Bitmap(typeof(PropertyGrid), "dotdotdot.png");
					this.btnDialog.Click += this.OnBtnClick;
					this.btnDialog.KeyDown += this.OnBtnKeyDown;
					this.btnDialog.LostFocus += this.OnChildLostFocus;
					this.btnDialog.Size = new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight);
					this.CommonEditorSetup(this.btnDialog);
				}
				return this.btnDialog;
			}
		}

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x06006922 RID: 26914 RVA: 0x00182AA4 File Offset: 0x00181AA4
		private PropertyGridView.GridViewEdit Edit
		{
			get
			{
				if (this.edit == null)
				{
					this.edit = new PropertyGridView.GridViewEdit(this);
					this.edit.BorderStyle = BorderStyle.None;
					this.edit.AutoSize = false;
					this.edit.TabStop = false;
					this.edit.AcceptsReturn = true;
					this.edit.BackColor = this.BackColor;
					this.edit.ForeColor = this.ForeColor;
					this.edit.KeyDown += this.OnEditKeyDown;
					this.edit.KeyPress += this.OnEditKeyPress;
					this.edit.GotFocus += this.OnEditGotFocus;
					this.edit.LostFocus += this.OnEditLostFocus;
					this.edit.MouseDown += this.OnEditMouseDown;
					this.edit.TextChanged += this.OnEditChange;
					this.edit.TabIndex = 1;
					this.CommonEditorSetup(this.edit);
				}
				return this.edit;
			}
		}

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06006923 RID: 26915 RVA: 0x00182BC4 File Offset: 0x00181BC4
		private PropertyGridView.GridViewListBox DropDownListBox
		{
			get
			{
				if (this.listBox == null)
				{
					this.listBox = new PropertyGridView.GridViewListBox(this);
					this.listBox.DrawMode = DrawMode.OwnerDrawFixed;
					this.listBox.MouseUp += this.OnListMouseUp;
					this.listBox.DrawItem += this.OnListDrawItem;
					this.listBox.SelectedIndexChanged += this.OnListChange;
					this.listBox.KeyDown += this.OnListKeyDown;
					this.listBox.LostFocus += this.OnChildLostFocus;
					this.listBox.Visible = true;
					this.listBox.ItemHeight = this.RowHeight;
				}
				return this.listBox;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06006924 RID: 26916 RVA: 0x00182C8C File Offset: 0x00181C8C
		internal bool DrawValuesRightToLeft
		{
			get
			{
				if (this.edit != null && this.edit.IsHandleCreated)
				{
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this.edit, this.edit.Handle), -20));
					return (num & 8192) != 0;
				}
				return false;
			}
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06006925 RID: 26917 RVA: 0x00182CE1 File Offset: 0x00181CE1
		public bool FocusInside
		{
			get
			{
				return base.ContainsFocus || (this.dropDownHolder != null && this.dropDownHolder.ContainsFocus);
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06006926 RID: 26918 RVA: 0x00182D04 File Offset: 0x00181D04
		internal Color GrayTextColor
		{
			get
			{
				if (this.ForeColor.ToArgb() == SystemColors.WindowText.ToArgb())
				{
					return SystemColors.GrayText;
				}
				int num = this.ForeColor.ToArgb();
				int num2 = num >> 24 & 255;
				if (num2 != 0)
				{
					num2 /= 2;
					num &= 16777215;
					num |= (int)((long)((long)num2 << 24) & (long)((ulong)-16777216));
				}
				else
				{
					num /= 2;
				}
				return Color.FromArgb(num);
			}
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x06006927 RID: 26919 RVA: 0x00182D7A File Offset: 0x00181D7A
		private GridErrorDlg ErrorDialog
		{
			get
			{
				if (this.errorDlg == null)
				{
					this.errorDlg = new GridErrorDlg(this.ownerGrid);
				}
				return this.errorDlg;
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06006928 RID: 26920 RVA: 0x00182D9B File Offset: 0x00181D9B
		private bool HasEntries
		{
			get
			{
				return this.topLevelGridEntries != null && this.topLevelGridEntries.Count > 0;
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06006929 RID: 26921 RVA: 0x00182DB5 File Offset: 0x00181DB5
		protected int InternalLabelWidth
		{
			get
			{
				if (this.GetFlag(128))
				{
					this.UpdateUIBasedOnFont(true);
				}
				if (this.labelWidth == -1)
				{
					this.SetConstants();
				}
				return this.labelWidth;
			}
		}

		// Token: 0x17001677 RID: 5751
		// (set) Token: 0x0600692A RID: 26922 RVA: 0x00182DE0 File Offset: 0x00181DE0
		internal int LabelPaintMargin
		{
			set
			{
				this.requiredLabelPaintMargin = (short)Math.Max(Math.Max(value, (int)this.requiredLabelPaintMargin), 2);
			}
		}

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x0600692B RID: 26923 RVA: 0x00182DFC File Offset: 0x00181DFC
		protected bool NeedsCommit
		{
			get
			{
				if (this.edit == null || !this.Edit.Visible)
				{
					return false;
				}
				string text = this.Edit.Text;
				return ((text != null && text.Length != 0) || (this.originalTextValue != null && this.originalTextValue.Length != 0)) && (text == null || this.originalTextValue == null || !text.Equals(this.originalTextValue));
			}
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x0600692C RID: 26924 RVA: 0x00182E68 File Offset: 0x00181E68
		public PropertyGrid OwnerGrid
		{
			get
			{
				return this.ownerGrid;
			}
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600692D RID: 26925 RVA: 0x00182E70 File Offset: 0x00181E70
		protected int RowHeight
		{
			get
			{
				if (this.cachedRowHeight == -1)
				{
					this.cachedRowHeight = this.Font.Height + 2;
				}
				return this.cachedRowHeight;
			}
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x0600692E RID: 26926 RVA: 0x00182E94 File Offset: 0x00181E94
		public Point ContextMenuDefaultLocation
		{
			get
			{
				Rectangle rectangle = this.GetRectangle(this.selectedRow, 1);
				Point point = base.PointToScreen(new Point(rectangle.X, rectangle.Y));
				return new Point(point.X + rectangle.Width / 2, point.Y + rectangle.Height / 2);
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x0600692F RID: 26927 RVA: 0x00182EF0 File Offset: 0x00181EF0
		private ScrollBar ScrollBar
		{
			get
			{
				if (this.scrollBar == null)
				{
					this.scrollBar = new VScrollBar();
					this.scrollBar.Scroll += this.OnScroll;
					base.Controls.Add(this.scrollBar);
				}
				return this.scrollBar;
			}
		}

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06006930 RID: 26928 RVA: 0x00182F3E File Offset: 0x00181F3E
		// (set) Token: 0x06006931 RID: 26929 RVA: 0x00182F48 File Offset: 0x00181F48
		internal GridEntry SelectedGridEntry
		{
			get
			{
				return this.selectedGridEntry;
			}
			set
			{
				if (this.allGridEntries != null)
				{
					foreach (object obj in this.allGridEntries)
					{
						GridEntry gridEntry = (GridEntry)obj;
						if (gridEntry == value)
						{
							this.SelectGridEntry(value, true);
							return;
						}
					}
				}
				GridEntry gridEntry2 = this.FindEquivalentGridEntry(new GridEntryCollection(null, new GridEntry[]
				{
					value
				}));
				if (gridEntry2 == null)
				{
					throw new ArgumentException(SR.GetString("PropertyGridInvalidGridEntry"));
				}
				this.SelectGridEntry(gridEntry2, true);
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06006932 RID: 26930 RVA: 0x00182FE8 File Offset: 0x00181FE8
		// (set) Token: 0x06006933 RID: 26931 RVA: 0x00182FF0 File Offset: 0x00181FF0
		public IServiceProvider ServiceProvider
		{
			get
			{
				return this.serviceProvider;
			}
			set
			{
				if (value != this.serviceProvider)
				{
					this.serviceProvider = value;
					this.topHelpService = null;
					if (this.helpService != null && this.helpService is IDisposable)
					{
						((IDisposable)this.helpService).Dispose();
					}
					this.helpService = null;
				}
			}
		}

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06006934 RID: 26932 RVA: 0x00183040 File Offset: 0x00182040
		// (set) Token: 0x06006935 RID: 26933 RVA: 0x00183051 File Offset: 0x00182051
		private int TipColumn
		{
			get
			{
				return (this.tipInfo & -65536) >> 16;
			}
			set
			{
				this.tipInfo &= 65535;
				this.tipInfo |= (value & 65535) << 16;
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x06006936 RID: 26934 RVA: 0x0018307C File Offset: 0x0018207C
		// (set) Token: 0x06006937 RID: 26935 RVA: 0x0018308A File Offset: 0x0018208A
		private int TipRow
		{
			get
			{
				return this.tipInfo & 65535;
			}
			set
			{
				this.tipInfo &= -65536;
				this.tipInfo |= (value & 65535);
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x06006938 RID: 26936 RVA: 0x001830B4 File Offset: 0x001820B4
		private GridToolTip ToolTip
		{
			get
			{
				if (this.toolTip == null)
				{
					this.toolTip = new GridToolTip(new Control[]
					{
						this,
						this.Edit
					});
					this.toolTip.ToolTip = "";
					this.toolTip.Font = this.Font;
				}
				return this.toolTip;
			}
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x00183110 File Offset: 0x00182110
		internal GridEntryCollection AccessibilityGetGridEntries()
		{
			return this.GetAllGridEntries();
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x00183118 File Offset: 0x00182118
		internal Rectangle AccessibilityGetGridEntryBounds(GridEntry gridEntry)
		{
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			if (rowFromGridEntry == -1)
			{
				return new Rectangle(0, 0, 0, 0);
			}
			Rectangle rectangle = this.GetRectangle(rowFromGridEntry, 3);
			NativeMethods.POINT point = new NativeMethods.POINT(rectangle.X, rectangle.Y);
			UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
			return new Rectangle(point.x, point.y, rectangle.Width, rectangle.Height);
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x0018318C File Offset: 0x0018218C
		internal int AccessibilityGetGridEntryChildID(GridEntry gridEntry)
		{
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection == null)
			{
				return -1;
			}
			for (int i = 0; i < gridEntryCollection.Count; i++)
			{
				if (gridEntryCollection[i].Equals(gridEntry))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x001831C8 File Offset: 0x001821C8
		internal void AccessibilitySelect(GridEntry entry)
		{
			this.SelectGridEntry(entry, true);
			this.FocusInternal();
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x001831DC File Offset: 0x001821DC
		private void AddGridEntryEvents(GridEntryCollection ipeArray, int startIndex, int count)
		{
			if (ipeArray == null)
			{
				return;
			}
			if (count == -1)
			{
				count = ipeArray.Count - startIndex;
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (ipeArray[i] != null)
				{
					GridEntry entry = ipeArray.GetEntry(i);
					entry.AddOnValueClick(this.ehValueClick);
					entry.AddOnLabelClick(this.ehLabelClick);
					entry.AddOnOutlineClick(this.ehOutlineClick);
					entry.AddOnOutlineDoubleClick(this.ehOutlineClick);
					entry.AddOnValueDoubleClick(this.ehValueDblClick);
					entry.AddOnLabelDoubleClick(this.ehLabelDblClick);
					entry.AddOnRecreateChildren(this.ehRecreateChildren);
				}
			}
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x0018326E File Offset: 0x0018226E
		protected virtual void AdjustOrigin(Graphics g, Point newOrigin, ref Rectangle r)
		{
			g.ResetTransform();
			g.TranslateTransform((float)newOrigin.X, (float)newOrigin.Y);
			r.Offset(-newOrigin.X, -newOrigin.Y);
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x001832A2 File Offset: 0x001822A2
		private void CancelSplitterMove()
		{
			if (this.GetFlag(4))
			{
				this.SetFlag(4, false);
				base.CaptureInternal = false;
				if (this.selectedRow != -1)
				{
					this.SelectRow(this.selectedRow);
				}
			}
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x001832D1 File Offset: 0x001822D1
		internal PropertyGridView.GridPositionData CaptureGridPositionData()
		{
			return new PropertyGridView.GridPositionData(this);
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x001832DC File Offset: 0x001822DC
		private void ClearGridEntryEvents(GridEntryCollection ipeArray, int startIndex, int count)
		{
			if (ipeArray == null)
			{
				return;
			}
			if (count == -1)
			{
				count = ipeArray.Count - startIndex;
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (ipeArray[i] != null)
				{
					GridEntry entry = ipeArray.GetEntry(i);
					entry.RemoveOnValueClick(this.ehValueClick);
					entry.RemoveOnLabelClick(this.ehLabelClick);
					entry.RemoveOnOutlineClick(this.ehOutlineClick);
					entry.RemoveOnOutlineDoubleClick(this.ehOutlineClick);
					entry.RemoveOnValueDoubleClick(this.ehValueDblClick);
					entry.RemoveOnLabelDoubleClick(this.ehLabelDblClick);
					entry.RemoveOnRecreateChildren(this.ehRecreateChildren);
				}
			}
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x0018336E File Offset: 0x0018236E
		public void ClearProps()
		{
			if (!this.HasEntries)
			{
				return;
			}
			this.CommonEditorHide();
			this.topLevelGridEntries = null;
			this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
			this.allGridEntries = null;
			this.selectedRow = -1;
			this.tipInfo = -1;
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x001833A9 File Offset: 0x001823A9
		public void CloseDropDown()
		{
			this.CloseDropDownInternal(true);
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x001833B4 File Offset: 0x001823B4
		private void CloseDropDownInternal(bool resetFocus)
		{
			if (this.GetFlag(32))
			{
				return;
			}
			try
			{
				this.SetFlag(32, true);
				if (this.dropDownHolder != null && this.dropDownHolder.Visible)
				{
					if (this.dropDownHolder.Component == this.DropDownListBox && this.GetFlag(64))
					{
						this.OnListClick(null, null);
					}
					this.Edit.Filter = false;
					this.dropDownHolder.SetComponent(null, false);
					this.dropDownHolder.Visible = false;
					if (resetFocus)
					{
						if (this.DialogButton.Visible)
						{
							this.DialogButton.FocusInternal();
						}
						else if (this.DropDownButton.Visible)
						{
							this.DropDownButton.FocusInternal();
						}
						else if (this.Edit.Visible)
						{
							this.Edit.FocusInternal();
						}
						else
						{
							this.FocusInternal();
						}
						if (this.selectedRow != -1)
						{
							this.SelectRow(this.selectedRow);
						}
					}
				}
			}
			finally
			{
				this.SetFlag(32, false);
			}
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x001834C8 File Offset: 0x001824C8
		private void CommonEditorHide()
		{
			this.CommonEditorHide(false);
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x001834D4 File Offset: 0x001824D4
		private void CommonEditorHide(bool always)
		{
			if (!always && !this.HasEntries)
			{
				return;
			}
			this.CloseDropDown();
			bool flag = false;
			if ((this.Edit.Focused || this.DialogButton.Focused || this.DropDownButton.Focused) && base.IsHandleCreated && base.Visible && base.Enabled)
			{
				flag = (IntPtr.Zero != UnsafeNativeMethods.SetFocus(new HandleRef(this, base.Handle)));
			}
			try
			{
				this.Edit.DontFocus = true;
				if (this.Edit.Focused && !flag)
				{
					flag = this.FocusInternal();
				}
				this.Edit.Visible = false;
				this.Edit.SelectionStart = 0;
				this.Edit.SelectionLength = 0;
				if (this.DialogButton.Focused && !flag)
				{
					flag = this.FocusInternal();
				}
				this.DialogButton.Visible = false;
				if (this.DropDownButton.Focused && !flag)
				{
					flag = this.FocusInternal();
				}
				this.DropDownButton.Visible = false;
				this.currentEditor = null;
			}
			finally
			{
				this.Edit.DontFocus = false;
			}
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x00183604 File Offset: 0x00182604
		protected virtual void CommonEditorSetup(Control ctl)
		{
			ctl.Visible = false;
			base.Controls.Add(ctl);
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x0018361C File Offset: 0x0018261C
		protected virtual void CommonEditorUse(Control ctl, Rectangle rectTarget)
		{
			Rectangle bounds = ctl.Bounds;
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Inflate(-1, -1);
			try
			{
				rectTarget = Rectangle.Intersect(clientRectangle, rectTarget);
				if (!rectTarget.IsEmpty)
				{
					if (!rectTarget.Equals(bounds))
					{
						ctl.SetBounds(rectTarget.X, rectTarget.Y, rectTarget.Width, rectTarget.Height);
					}
					ctl.Visible = true;
				}
			}
			catch
			{
				rectTarget = Rectangle.Empty;
			}
			if (rectTarget.IsEmpty)
			{
				ctl.Visible = false;
			}
			this.currentEditor = ctl;
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x001836C4 File Offset: 0x001826C4
		private int CountPropsFromOutline(GridEntryCollection rgipes)
		{
			if (rgipes == null)
			{
				return 0;
			}
			int num = rgipes.Count;
			for (int i = 0; i < rgipes.Count; i++)
			{
				if (((GridEntry)rgipes[i]).InternalExpanded)
				{
					num += this.CountPropsFromOutline(((GridEntry)rgipes[i]).Children);
				}
			}
			return num;
		}

		// Token: 0x0600694A RID: 26954 RVA: 0x0018371C File Offset: 0x0018271C
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new PropertyGridView.PropertyGridViewAccessibleObject(this);
		}

		// Token: 0x0600694B RID: 26955 RVA: 0x00183724 File Offset: 0x00182724
		private Bitmap CreateDownArrow()
		{
			Bitmap result = null;
			try
			{
				Icon icon = new Icon(typeof(PropertyGrid), "Arrow.ico");
				result = icon.ToBitmap();
				icon.Dispose();
			}
			catch (Exception)
			{
				result = new Bitmap(16, 16);
			}
			return result;
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x00183778 File Offset: 0x00182778
		protected virtual void CreateUI()
		{
			this.UpdateUIBasedOnFont(false);
		}

		// Token: 0x0600694D RID: 26957 RVA: 0x00183784 File Offset: 0x00182784
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.Dispose();
				}
				if (this.listBox != null)
				{
					this.listBox.Dispose();
				}
				if (this.dropDownHolder != null)
				{
					this.dropDownHolder.Dispose();
				}
				this.scrollBar = null;
				this.listBox = null;
				this.dropDownHolder = null;
				this.ownerGrid = null;
				this.topLevelGridEntries = null;
				this.allGridEntries = null;
				this.serviceProvider = null;
				this.topHelpService = null;
				if (this.helpService != null && this.helpService is IDisposable)
				{
					((IDisposable)this.helpService).Dispose();
				}
				this.helpService = null;
				if (this.edit != null)
				{
					this.edit.Dispose();
					this.edit = null;
				}
				if (this.fontBold != null)
				{
					this.fontBold.Dispose();
					this.fontBold = null;
				}
				if (this.btnDropDown != null)
				{
					this.btnDropDown.Dispose();
					this.btnDropDown = null;
				}
				if (this.btnDialog != null)
				{
					this.btnDialog.Dispose();
					this.btnDialog = null;
				}
				if (this.toolTip != null)
				{
					this.toolTip.Dispose();
					this.toolTip = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600694E RID: 26958 RVA: 0x001838BD File Offset: 0x001828BD
		public void DoCopyCommand()
		{
			if (this.CanCopy)
			{
				if (this.Edit.Focused)
				{
					this.Edit.Copy();
					return;
				}
				Clipboard.SetDataObject(this.selectedGridEntry.GetPropertyTextValue());
			}
		}

		// Token: 0x0600694F RID: 26959 RVA: 0x001838F0 File Offset: 0x001828F0
		public void DoCutCommand()
		{
			if (this.CanCut)
			{
				this.DoCopyCommand();
				if (this.Edit.Visible)
				{
					this.Edit.Cut();
				}
			}
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x00183918 File Offset: 0x00182918
		public void DoPasteCommand()
		{
			if (this.CanPaste && this.Edit.Visible)
			{
				if (this.Edit.Focused)
				{
					this.Edit.Paste();
					return;
				}
				IDataObject dataObject = Clipboard.GetDataObject();
				if (dataObject != null)
				{
					string text = (string)dataObject.GetData(typeof(string));
					if (text != null)
					{
						this.Edit.FocusInternal();
						this.Edit.Text = text;
						this.SetCommitError(0, true);
					}
				}
			}
		}

		// Token: 0x06006951 RID: 26961 RVA: 0x00183995 File Offset: 0x00182995
		public void DoUndoCommand()
		{
			if (this.CanUndo && this.Edit.Visible)
			{
				this.Edit.SendMessage(772, 0, 0);
			}
		}

		// Token: 0x06006952 RID: 26962 RVA: 0x001839C0 File Offset: 0x001829C0
		internal void DumpPropsToConsole(GridEntry entry, string prefix)
		{
			Type type = entry.PropertyType;
			if (entry.PropertyValue != null)
			{
				type = entry.PropertyValue.GetType();
			}
			Console.WriteLine(string.Concat(new string[]
			{
				prefix,
				entry.PropertyLabel,
				", value type=",
				(type == null) ? "(null)" : type.FullName,
				", value=",
				(entry.PropertyValue == null) ? "(null)" : entry.PropertyValue.ToString(),
				", flags=",
				entry.Flags.ToString(CultureInfo.InvariantCulture),
				", TypeConverter=",
				(entry.TypeConverter == null) ? "(null)" : entry.TypeConverter.GetType().FullName,
				", UITypeEditor=",
				(entry.UITypeEditor == null) ? "(null)" : entry.UITypeEditor.GetType().FullName
			}));
			GridEntryCollection children = entry.Children;
			if (children != null)
			{
				foreach (object obj in children)
				{
					GridEntry entry2 = (GridEntry)obj;
					this.DumpPropsToConsole(entry2, prefix + "\t");
				}
			}
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x00183B24 File Offset: 0x00182B24
		private int GetIPELabelIndent(GridEntry gridEntry)
		{
			return gridEntry.PropertyLabelIndent + 1;
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x00183B30 File Offset: 0x00182B30
		private int GetIPELabelLength(Graphics g, GridEntry gridEntry)
		{
			SizeF value = PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, g, gridEntry.PropertyLabel, this.Font);
			Size size = Size.Ceiling(value);
			return this.ptOurLocation.X + this.GetIPELabelIndent(gridEntry) + size.Width;
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x00183B78 File Offset: 0x00182B78
		private bool IsIPELabelLong(Graphics g, GridEntry gridEntry)
		{
			if (gridEntry == null)
			{
				return false;
			}
			int ipelabelLength = this.GetIPELabelLength(g, gridEntry);
			return ipelabelLength > this.ptOurLocation.X + this.InternalLabelWidth;
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x00183BA8 File Offset: 0x00182BA8
		protected virtual void DrawLabel(Graphics g, int row, Rectangle rect, bool selected, bool fLongLabelRequest, ref Rectangle clipRect)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow == null || rect.IsEmpty)
			{
				return;
			}
			Point newOrigin = new Point(rect.X, rect.Y);
			Rectangle clipRect2 = Rectangle.Intersect(rect, clipRect);
			if (clipRect2.IsEmpty)
			{
				return;
			}
			this.AdjustOrigin(g, newOrigin, ref rect);
			clipRect2.Offset(-newOrigin.X, -newOrigin.Y);
			try
			{
				bool paintFullLabel = false;
				this.GetIPELabelIndent(gridEntryFromRow);
				if (fLongLabelRequest)
				{
					this.GetIPELabelLength(g, gridEntryFromRow);
					paintFullLabel = this.IsIPELabelLong(g, gridEntryFromRow);
				}
				gridEntryFromRow.PaintLabel(g, rect, clipRect2, selected, paintFullLabel);
			}
			catch (Exception)
			{
			}
			finally
			{
				this.ResetOrigin(g);
			}
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x00183C70 File Offset: 0x00182C70
		protected virtual void DrawValueEntry(Graphics g, int row, ref Rectangle clipRect)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow == null)
			{
				return;
			}
			Rectangle rectangle = this.GetRectangle(row, 2);
			Point newOrigin = new Point(rectangle.X, rectangle.Y);
			Rectangle clipRect2 = Rectangle.Intersect(clipRect, rectangle);
			if (clipRect2.IsEmpty)
			{
				return;
			}
			this.AdjustOrigin(g, newOrigin, ref rectangle);
			clipRect2.Offset(-newOrigin.X, -newOrigin.Y);
			try
			{
				this.DrawValueEntry(g, rectangle, clipRect2, gridEntryFromRow, null, true);
			}
			catch
			{
			}
			finally
			{
				this.ResetOrigin(g);
			}
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x00183D18 File Offset: 0x00182D18
		private void DrawValueEntry(Graphics g, Rectangle rect, Rectangle clipRect, GridEntry gridEntry, object value, bool fetchValue)
		{
			this.DrawValue(g, rect, clipRect, gridEntry, value, false, true, fetchValue, true);
		}

		// Token: 0x06006959 RID: 26969 RVA: 0x00183D38 File Offset: 0x00182D38
		private void DrawValue(Graphics g, Rectangle rect, Rectangle clipRect, GridEntry gridEntry, object value, bool drawSelected, bool checkShouldSerialize, bool fetchValue, bool paintInPlace)
		{
			GridEntry.PaintValueFlags paintValueFlags = GridEntry.PaintValueFlags.None;
			if (drawSelected)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.DrawSelected;
			}
			if (checkShouldSerialize)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.CheckShouldSerialize;
			}
			if (fetchValue)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.FetchValue;
			}
			if (paintInPlace)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.PaintInPlace;
			}
			gridEntry.PaintValue(value, g, rect, clipRect, paintValueFlags);
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x00183D74 File Offset: 0x00182D74
		private void F4Selection(bool popupModalDialog)
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return;
			}
			if (this.errorState != 0 && this.Edit.Visible)
			{
				this.Edit.FocusInternal();
				return;
			}
			if (this.DropDownButton.Visible)
			{
				this.PopupDialog(this.selectedRow);
				return;
			}
			if (!this.DialogButton.Visible)
			{
				if (this.Edit.Visible)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
				}
				return;
			}
			if (popupModalDialog)
			{
				this.PopupDialog(this.selectedRow);
				return;
			}
			this.DialogButton.FocusInternal();
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x00183E18 File Offset: 0x00182E18
		public void DoubleClickRow(int row, bool toggleExpand, int type)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow == null)
			{
				return;
			}
			if (toggleExpand)
			{
				if (type != 2)
				{
					goto IL_40;
				}
			}
			try
			{
				bool flag = gridEntryFromRow.DoubleClickPropertyValue();
				if (flag)
				{
					this.SelectRow(row);
					return;
				}
			}
			catch (Exception ex)
			{
				this.SetCommitError(1);
				this.ShowInvalidMessage(gridEntryFromRow.PropertyLabel, null, ex);
				return;
			}
			IL_40:
			this.SelectGridEntry(gridEntryFromRow, true);
			if (type != 1 || !toggleExpand || !gridEntryFromRow.Expandable)
			{
				if (gridEntryFromRow.IsValueEditable && gridEntryFromRow.Enumerable)
				{
					int num = this.GetCurrentValueIndex(gridEntryFromRow);
					if (num != -1)
					{
						object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
						if (propertyValueList == null || num >= propertyValueList.Length - 1)
						{
							num = 0;
						}
						else
						{
							num++;
						}
						this.CommitValue(propertyValueList[num]);
						this.SelectRow(this.selectedRow);
						this.Refresh();
						return;
					}
				}
				if (this.Edit.Visible)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
				}
				return;
			}
			this.SetExpand(gridEntryFromRow, !gridEntryFromRow.InternalExpanded);
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x00183F14 File Offset: 0x00182F14
		public Font GetBaseFont()
		{
			return this.Font;
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x00183F1C File Offset: 0x00182F1C
		public Font GetBoldFont()
		{
			if (this.fontBold == null)
			{
				this.fontBold = new Font(this.Font, FontStyle.Bold);
			}
			return this.fontBold;
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x00183F3E File Offset: 0x00182F3E
		internal IntPtr GetBaseHfont()
		{
			if (this.baseHfont == IntPtr.Zero)
			{
				this.baseHfont = this.GetBaseFont().ToHfont();
			}
			return this.baseHfont;
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x00183F69 File Offset: 0x00182F69
		internal IntPtr GetBoldHfont()
		{
			if (this.boldHfont == IntPtr.Zero)
			{
				this.boldHfont = this.GetBoldFont().ToHfont();
			}
			return this.boldHfont;
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x00183F94 File Offset: 0x00182F94
		private bool GetFlag(short flag)
		{
			return (this.flags & flag) != 0;
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x00183FA4 File Offset: 0x00182FA4
		public virtual Color GetLineColor()
		{
			return this.ownerGrid.LineColor;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x00183FB4 File Offset: 0x00182FB4
		public virtual Brush GetLineBrush(Graphics g)
		{
			if (this.ownerGrid.lineBrush == null)
			{
				Color nearestColor = g.GetNearestColor(this.ownerGrid.LineColor);
				this.ownerGrid.lineBrush = new SolidBrush(nearestColor);
			}
			return this.ownerGrid.lineBrush;
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x00183FFC File Offset: 0x00182FFC
		public virtual IntPtr GetHostHandle()
		{
			return base.Handle;
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x00184004 File Offset: 0x00183004
		public virtual int GetLabelWidth()
		{
			return this.InternalLabelWidth;
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x0018400C File Offset: 0x0018300C
		public virtual int GetOutlineIconSize()
		{
			return 9;
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x00184010 File Offset: 0x00183010
		public virtual int GetGridEntryHeight()
		{
			return this.RowHeight;
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x00184018 File Offset: 0x00183018
		internal int GetPropertyLocation(string propName, bool getXY, bool rowValue)
		{
			if (this.allGridEntries != null && this.allGridEntries.Count > 0)
			{
				int i = 0;
				while (i < this.allGridEntries.Count)
				{
					if (string.Compare(propName, this.allGridEntries.GetEntry(i).PropertyLabel, true, CultureInfo.InvariantCulture) == 0)
					{
						if (!getXY)
						{
							return i;
						}
						int rowFromGridEntry = this.GetRowFromGridEntry(this.allGridEntries.GetEntry(i));
						if (rowFromGridEntry < 0 || rowFromGridEntry >= this.visibleRows)
						{
							return -1;
						}
						Rectangle rectangle = this.GetRectangle(rowFromGridEntry, rowValue ? 2 : 1);
						return rectangle.Y << 16 | (rectangle.X & 65535);
					}
					else
					{
						i++;
					}
				}
			}
			return -1;
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x001840C6 File Offset: 0x001830C6
		public new object GetService(Type classService)
		{
			if (classService == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			if (this.ServiceProvider != null)
			{
				return this.serviceProvider.GetService(classService);
			}
			return null;
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x001840ED File Offset: 0x001830ED
		public virtual int GetSplitterWidth()
		{
			return 1;
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x001840F0 File Offset: 0x001830F0
		public virtual int GetTotalWidth()
		{
			return this.GetLabelWidth() + this.GetSplitterWidth() + this.GetValueWidth();
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x00184106 File Offset: 0x00183106
		public virtual int GetValuePaintIndent()
		{
			return 26;
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x0018410A File Offset: 0x0018310A
		public virtual int GetValuePaintWidth()
		{
			return 20;
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x0018410E File Offset: 0x0018310E
		public virtual int GetValueStringIndent()
		{
			return 0;
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x00184111 File Offset: 0x00183111
		public virtual int GetValueWidth()
		{
			return (int)((double)this.InternalLabelWidth * (this.labelRatio - 1.0));
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x0018412C File Offset: 0x0018312C
		public void DropDownControl(Control ctl)
		{
			if (this.dropDownHolder == null)
			{
				this.dropDownHolder = new PropertyGridView.DropDownHolder(this);
			}
			this.dropDownHolder.Visible = false;
			this.dropDownHolder.SetComponent(ctl, this.GetFlag(1024));
			Rectangle rectangle = this.GetRectangle(this.selectedRow, 2);
			Size size = this.dropDownHolder.Size;
			Point point = base.PointToScreen(new Point(0, 0));
			Rectangle workingArea = Screen.FromControl(this.Edit).WorkingArea;
			size.Width = Math.Max(rectangle.Width + 1, size.Width);
			point.X = Math.Min(workingArea.X + workingArea.Width - size.Width, Math.Max(workingArea.X, point.X + rectangle.X + rectangle.Width - size.Width));
			point.Y += rectangle.Y;
			if (workingArea.Y + workingArea.Height < size.Height + point.Y + this.Edit.Height)
			{
				point.Y -= size.Height;
				this.dropDownHolder.ResizeUp = true;
			}
			else
			{
				point.Y += rectangle.Height + 1;
				this.dropDownHolder.ResizeUp = false;
			}
			UnsafeNativeMethods.SetWindowLong(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), -8, new HandleRef(this, base.Handle));
			this.dropDownHolder.SetBounds(point.X, point.Y, size.Width, size.Height);
			SafeNativeMethods.ShowWindow(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), 8);
			this.Edit.Filter = true;
			this.dropDownHolder.Visible = true;
			this.dropDownHolder.FocusComponent();
			this.SelectEdit(false);
			try
			{
				this.DropDownButton.IgnoreMouse = true;
				this.dropDownHolder.DoModalLoop();
			}
			finally
			{
				this.DropDownButton.IgnoreMouse = false;
			}
			if (this.selectedRow != -1)
			{
				this.FocusInternal();
				this.SelectRow(this.selectedRow);
			}
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x00184384 File Offset: 0x00183384
		public virtual void DropDownDone()
		{
			this.CloseDropDown();
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x0018438C File Offset: 0x0018338C
		public virtual void DropDownUpdate()
		{
			if (this.dropDownHolder != null && this.dropDownHolder.GetUsed())
			{
				int row = this.selectedRow;
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
				this.Edit.Text = gridEntryFromRow.GetPropertyTextValue();
			}
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x001843CE File Offset: 0x001833CE
		public bool EnsurePendingChangesCommitted()
		{
			this.CloseDropDown();
			return this.Commit();
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x001843DC File Offset: 0x001833DC
		private bool FilterEditWndProc(ref Message m)
		{
			if (this.dropDownHolder != null && this.dropDownHolder.Visible && m.Msg == 256 && (int)m.WParam != 9)
			{
				Control component = this.dropDownHolder.Component;
				if (component != null)
				{
					m.Result = component.SendMessage(m.Msg, m.WParam, m.LParam);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x0018444C File Offset: 0x0018344C
		private bool FilterReadOnlyEditKeyPress(char keyChar)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow.Enumerable && gridEntryFromRow.IsValueEditable)
			{
				int currentValueIndex = this.GetCurrentValueIndex(gridEntryFromRow);
				object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
				string strB = new string(new char[]
				{
					keyChar
				});
				for (int i = 0; i < propertyValueList.Length; i++)
				{
					object value = propertyValueList[(i + currentValueIndex + 1) % propertyValueList.Length];
					string propertyTextValue = gridEntryFromRow.GetPropertyTextValue(value);
					if (propertyTextValue != null && propertyTextValue.Length > 0 && string.Compare(propertyTextValue.Substring(0, 1), strB, true, CultureInfo.InvariantCulture) == 0)
					{
						this.CommitValue(value);
						if (this.Edit.Focused)
						{
							this.SelectEdit(false);
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x00184510 File Offset: 0x00183510
		public virtual bool WillFilterKeyPress(char charPressed)
		{
			if (!this.Edit.Visible)
			{
				return false;
			}
			Keys modifierKeys = Control.ModifierKeys;
			if ((modifierKeys & ~Keys.Shift) != Keys.None)
			{
				return false;
			}
			if (this.selectedGridEntry != null)
			{
				if (charPressed == '\t')
				{
					return false;
				}
				switch (charPressed)
				{
				case '*':
				case '+':
				case '-':
					return !this.selectedGridEntry.Expandable;
				}
			}
			return true;
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x0018457C File Offset: 0x0018357C
		public void FilterKeyPress(char keyChar)
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return;
			}
			this.Edit.FilterKeyPress(keyChar);
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x001845A8 File Offset: 0x001835A8
		private GridEntry FindEquivalentGridEntry(GridEntryCollection ipeHier)
		{
			if (ipeHier == null || ipeHier.Count == 0)
			{
				return null;
			}
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection == null || gridEntryCollection.Count == 0)
			{
				return null;
			}
			GridEntry gridEntry = null;
			int num = 0;
			int num2 = gridEntryCollection.Count;
			for (int i = 0; i < ipeHier.Count; i++)
			{
				if (ipeHier[i] != null)
				{
					if (gridEntry != null)
					{
						int count = gridEntryCollection.Count;
						if (!gridEntry.InternalExpanded)
						{
							this.SetExpand(gridEntry, true);
							gridEntryCollection = this.GetAllGridEntries();
						}
						num2 = gridEntry.VisibleChildCount;
					}
					int num3 = num;
					gridEntry = null;
					while (num < gridEntryCollection.Count && num - num3 <= num2)
					{
						if (ipeHier.GetEntry(i).NonParentEquals(gridEntryCollection[num]))
						{
							gridEntry = gridEntryCollection.GetEntry(num);
							num++;
							break;
						}
						num++;
					}
					if (gridEntry == null)
					{
						break;
					}
				}
			}
			return gridEntry;
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x00184670 File Offset: 0x00183670
		protected virtual Point FindPosition(int x, int y)
		{
			if (this.RowHeight == -1)
			{
				return PropertyGridView.InvalidPosition;
			}
			Size ourSize = this.GetOurSize();
			if (x < 0 || x > ourSize.Width + this.ptOurLocation.X)
			{
				return PropertyGridView.InvalidPosition;
			}
			Point result = new Point(1, 0);
			if (x > this.InternalLabelWidth + this.ptOurLocation.X)
			{
				result.X = 2;
			}
			result.Y = (y - this.ptOurLocation.Y) / (1 + this.RowHeight);
			return result;
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x001846F7 File Offset: 0x001836F7
		public virtual void Flush()
		{
			if (this.Commit() && this.Edit.Focused)
			{
				this.FocusInternal();
			}
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x00184715 File Offset: 0x00183715
		private GridEntryCollection GetAllGridEntries()
		{
			return this.GetAllGridEntries(false);
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x00184720 File Offset: 0x00183720
		private GridEntryCollection GetAllGridEntries(bool fUpdateCache)
		{
			if (this.visibleRows == -1 || this.totalProps == -1 || !this.HasEntries)
			{
				return null;
			}
			if (this.allGridEntries != null && !fUpdateCache)
			{
				return this.allGridEntries;
			}
			GridEntry[] array = new GridEntry[this.totalProps];
			try
			{
				this.GetGridEntriesFromOutline(this.topLevelGridEntries, 0, 0, array);
			}
			catch (Exception)
			{
			}
			this.allGridEntries = new GridEntryCollection(null, array);
			this.AddGridEntryEvents(this.allGridEntries, 0, -1);
			return this.allGridEntries;
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x001847B0 File Offset: 0x001837B0
		private int GetCurrentValueIndex(GridEntry gridEntry)
		{
			if (!gridEntry.Enumerable)
			{
				return -1;
			}
			try
			{
				object[] propertyValueList = gridEntry.GetPropertyValueList();
				object propertyValue = gridEntry.PropertyValue;
				string strA = gridEntry.TypeConverter.ConvertToString(gridEntry, propertyValue);
				if (propertyValueList != null && propertyValueList.Length > 0)
				{
					int num = -1;
					int num2 = -1;
					for (int i = 0; i < propertyValueList.Length; i++)
					{
						object obj = propertyValueList[i];
						string strB = gridEntry.TypeConverter.ConvertToString(obj);
						if (propertyValue == obj || string.Compare(strA, strB, true, CultureInfo.InvariantCulture) == 0)
						{
							num = i;
						}
						if (propertyValue != null && obj != null && obj.Equals(propertyValue))
						{
							num2 = i;
						}
						if (num == num2 && num != -1)
						{
							return num;
						}
					}
					if (num != -1)
					{
						return num;
					}
					if (num2 != -1)
					{
						return num2;
					}
				}
			}
			catch (Exception)
			{
			}
			catch
			{
			}
			return -1;
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x0018489C File Offset: 0x0018389C
		public virtual int GetDefaultOutlineIndent()
		{
			return 10;
		}

		// Token: 0x0600697E RID: 27006 RVA: 0x001848A0 File Offset: 0x001838A0
		private IHelpService GetHelpService()
		{
			if (this.helpService == null && this.ServiceProvider != null)
			{
				this.topHelpService = (IHelpService)this.ServiceProvider.GetService(typeof(IHelpService));
				if (this.topHelpService != null)
				{
					IHelpService helpService = this.topHelpService.CreateLocalContext(HelpContextType.ToolWindowSelection);
					if (helpService != null)
					{
						this.helpService = helpService;
					}
				}
			}
			return this.helpService;
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x00184904 File Offset: 0x00183904
		public virtual int GetScrollOffset()
		{
			if (this.scrollBar == null)
			{
				return 0;
			}
			return this.ScrollBar.Value;
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x00184928 File Offset: 0x00183928
		private GridEntryCollection GetGridEntryHierarchy(GridEntry gridEntry)
		{
			if (gridEntry == null)
			{
				return null;
			}
			int propertyDepth = gridEntry.PropertyDepth;
			if (propertyDepth > 0)
			{
				GridEntry[] array = new GridEntry[propertyDepth + 1];
				while (gridEntry != null && propertyDepth >= 0)
				{
					array[propertyDepth] = gridEntry;
					gridEntry = gridEntry.ParentGridEntry;
					propertyDepth = gridEntry.PropertyDepth;
				}
				return new GridEntryCollection(null, array);
			}
			return new GridEntryCollection(null, new GridEntry[]
			{
				gridEntry
			});
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x00184984 File Offset: 0x00183984
		private GridEntry GetGridEntryFromRow(int row)
		{
			return this.GetGridEntryFromOffset(row + this.GetScrollOffset());
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x00184994 File Offset: 0x00183994
		private GridEntry GetGridEntryFromOffset(int offset)
		{
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection != null && offset >= 0 && offset < gridEntryCollection.Count)
			{
				return gridEntryCollection.GetEntry(offset);
			}
			return null;
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x001849C4 File Offset: 0x001839C4
		private int GetGridEntriesFromOutline(GridEntryCollection rgipe, int cCur, int cTarget, GridEntry[] rgipeTarget)
		{
			if (rgipe == null || rgipe.Count == 0)
			{
				return cCur;
			}
			cCur--;
			for (int i = 0; i < rgipe.Count; i++)
			{
				cCur++;
				if (cCur >= cTarget + rgipeTarget.Length)
				{
					break;
				}
				GridEntry entry = rgipe.GetEntry(i);
				if (cCur >= cTarget)
				{
					rgipeTarget[cCur - cTarget] = entry;
				}
				if (entry.InternalExpanded)
				{
					GridEntryCollection children = entry.Children;
					if (children != null && children.Count > 0)
					{
						cCur = this.GetGridEntriesFromOutline(children, cCur + 1, cTarget, rgipeTarget);
					}
				}
			}
			return cCur;
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x00184A40 File Offset: 0x00183A40
		private Size GetOurSize()
		{
			Size clientSize = base.ClientSize;
			if (clientSize.Width == 0)
			{
				Size size = base.Size;
				if (size.Width > 10)
				{
					clientSize.Width = size.Width;
					clientSize.Height = size.Height;
				}
			}
			if (!this.GetScrollbarHidden())
			{
				Size size2 = this.ScrollBar.Size;
				clientSize.Width -= size2.Width;
			}
			clientSize.Width -= 2;
			clientSize.Height -= 2;
			return clientSize;
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x00184AD4 File Offset: 0x00183AD4
		public Rectangle GetRectangle(int row, int flRow)
		{
			Rectangle result = new Rectangle(0, 0, 0, 0);
			Size ourSize = this.GetOurSize();
			result.X = this.ptOurLocation.X;
			bool flag = (flRow & 1) != 0;
			bool flag2 = (flRow & 2) != 0;
			if (flag && flag2)
			{
				result.X = 1;
				result.Width = ourSize.Width - 1;
			}
			else if (flag)
			{
				result.X = 1;
				result.Width = this.InternalLabelWidth - 1;
			}
			else if (flag2)
			{
				result.X = this.ptOurLocation.X + this.InternalLabelWidth;
				result.Width = ourSize.Width - this.InternalLabelWidth;
			}
			result.Y = row * (this.RowHeight + 1) + 1 + this.ptOurLocation.Y;
			result.Height = this.RowHeight;
			return result;
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x00184BB4 File Offset: 0x00183BB4
		private int GetRowFromGridEntry(GridEntry gridEntry)
		{
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntry == null || gridEntryCollection == null)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i < gridEntryCollection.Count; i++)
			{
				if (gridEntry == gridEntryCollection[i])
				{
					return i - this.GetScrollOffset();
				}
				if (num == -1 && gridEntry.Equals(gridEntryCollection[i]))
				{
					num = i - this.GetScrollOffset();
				}
			}
			if (num != -1)
			{
				return num;
			}
			return -1 - this.GetScrollOffset();
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x00184C20 File Offset: 0x00183C20
		public virtual bool GetInPropertySet()
		{
			return this.GetFlag(16);
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x00184C2A File Offset: 0x00183C2A
		protected virtual bool GetScrollbarHidden()
		{
			return this.scrollBar == null || !this.ScrollBar.Visible;
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x00184C44 File Offset: 0x00183C44
		public virtual string GetTestingInfo(int entry)
		{
			GridEntry gridEntry = (entry < 0) ? this.GetGridEntryFromRow(this.selectedRow) : this.GetGridEntryFromOffset(entry);
			if (gridEntry == null)
			{
				return "";
			}
			return gridEntry.GetTestingInfo();
		}

		// Token: 0x0600698A RID: 27018 RVA: 0x00184C7A File Offset: 0x00183C7A
		public Color GetTextColor()
		{
			return this.ForeColor;
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x00184C84 File Offset: 0x00183C84
		private void LayoutWindow(bool invalidate)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			Size size = new Size(clientRectangle.Width, clientRectangle.Height);
			if (this.scrollBar != null)
			{
				Rectangle bounds = this.ScrollBar.Bounds;
				bounds.X = size.Width - bounds.Width - 1;
				bounds.Y = 1;
				bounds.Height = size.Height - 2;
				this.ScrollBar.Bounds = bounds;
			}
			if (invalidate)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600698C RID: 27020 RVA: 0x00184D08 File Offset: 0x00183D08
		internal void InvalidateGridEntryValue(GridEntry ge)
		{
			int rowFromGridEntry = this.GetRowFromGridEntry(ge);
			if (rowFromGridEntry != -1)
			{
				this.InvalidateRows(rowFromGridEntry, rowFromGridEntry, 2);
			}
		}

		// Token: 0x0600698D RID: 27021 RVA: 0x00184D2A File Offset: 0x00183D2A
		private void InvalidateRow(int row)
		{
			this.InvalidateRows(row, row, 3);
		}

		// Token: 0x0600698E RID: 27022 RVA: 0x00184D35 File Offset: 0x00183D35
		private void InvalidateRows(int startRow, int endRow)
		{
			this.InvalidateRows(startRow, endRow, 3);
		}

		// Token: 0x0600698F RID: 27023 RVA: 0x00184D40 File Offset: 0x00183D40
		private void InvalidateRows(int startRow, int endRow, int type)
		{
			if (endRow == -1)
			{
				Rectangle rectangle = this.GetRectangle(startRow, type);
				rectangle.Height = base.Size.Height - rectangle.Y - 1;
				base.Invalidate(rectangle);
				return;
			}
			for (int i = startRow; i <= endRow; i++)
			{
				Rectangle rectangle = this.GetRectangle(i, type);
				base.Invalidate(rectangle);
			}
		}

		// Token: 0x06006990 RID: 27024 RVA: 0x00184DA0 File Offset: 0x00183DA0
		protected override bool IsInputKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.Return)
			{
				if (keys != Keys.Tab)
				{
					if (keys != Keys.Return)
					{
						goto IL_34;
					}
					if (this.Edit.Focused)
					{
						return false;
					}
					goto IL_34;
				}
			}
			else if (keys != Keys.Escape && keys != Keys.F4)
			{
				goto IL_34;
			}
			return false;
			IL_34:
			return base.IsInputKey(keyData);
		}

		// Token: 0x06006991 RID: 27025 RVA: 0x00184DE8 File Offset: 0x00183DE8
		private bool IsMyChild(Control c)
		{
			if (c == this || c == null)
			{
				return false;
			}
			for (Control parentInternal = c.ParentInternal; parentInternal != null; parentInternal = parentInternal.ParentInternal)
			{
				if (parentInternal == this)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006992 RID: 27026 RVA: 0x00184E18 File Offset: 0x00183E18
		private bool IsScrollValueValid(int newValue)
		{
			return newValue != this.ScrollBar.Value && newValue >= 0 && newValue <= this.ScrollBar.Maximum && newValue + (this.ScrollBar.LargeChange - 1) < this.totalProps;
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x00184E54 File Offset: 0x00183E54
		internal bool IsSiblingControl(Control c1, Control c2)
		{
			Control parentInternal = c1.ParentInternal;
			for (Control parentInternal2 = c2.ParentInternal; parentInternal2 != null; parentInternal2 = parentInternal2.ParentInternal)
			{
				if (parentInternal == parentInternal2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x00184E84 File Offset: 0x00183E84
		private void MoveSplitterTo(int xpos)
		{
			int width = this.GetOurSize().Width;
			int x = this.ptOurLocation.X;
			int num = Math.Max(Math.Min(xpos, width - 10), this.GetOutlineIconSize() * 2);
			int internalLabelWidth = this.InternalLabelWidth;
			this.labelRatio = (double)width / (double)(num - x);
			this.SetConstants();
			if (this.selectedRow != -1)
			{
				this.SelectRow(this.selectedRow);
			}
			Rectangle clientRectangle = base.ClientRectangle;
			if (internalLabelWidth > this.InternalLabelWidth)
			{
				int num2 = this.InternalLabelWidth - (int)this.requiredLabelPaintMargin;
				base.Invalidate(new Rectangle(num2, 0, base.Size.Width - num2, base.Size.Height));
				return;
			}
			clientRectangle.X = internalLabelWidth - (int)this.requiredLabelPaintMargin;
			clientRectangle.Width -= clientRectangle.X;
			base.Invalidate(clientRectangle);
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x00184F70 File Offset: 0x00183F70
		private void OnBtnClick(object sender, EventArgs e)
		{
			if (this.GetFlag(256))
			{
				return;
			}
			if (sender == this.DialogButton && !this.Commit())
			{
				return;
			}
			this.SetCommitError(0);
			try
			{
				this.Commit();
				this.SetFlag(256, true);
				this.PopupDialog(this.selectedRow);
			}
			finally
			{
				this.SetFlag(256, false);
			}
		}

		// Token: 0x06006996 RID: 27030 RVA: 0x00184FE4 File Offset: 0x00183FE4
		private void OnBtnKeyDown(object sender, KeyEventArgs ke)
		{
			this.OnKeyDown(sender, ke);
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x00184FEE File Offset: 0x00183FEE
		private void OnChildLostFocus(object sender, EventArgs e)
		{
			this.OnLostFocus(null);
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x00184FF8 File Offset: 0x00183FF8
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (e != null && !this.GetInPropertySet() && !this.Commit())
			{
				this.Edit.FocusInternal();
				return;
			}
			if (this.selectedGridEntry != null && this.GetRowFromGridEntry(this.selectedGridEntry) != -1)
			{
				this.selectedGridEntry.Focus = true;
				this.SelectGridEntry(this.selectedGridEntry, false);
			}
			else
			{
				this.SelectRow(0);
			}
			if (this.selectedGridEntry != null && this.selectedGridEntry.GetValueOwner() != null)
			{
				this.UpdateHelpAttributes(null, this.selectedGridEntry);
			}
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x00185087 File Offset: 0x00184087
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.OnSysColorChange;
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x001850A1 File Offset: 0x001840A1
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.OnSysColorChange;
			if (this.toolTip != null && !base.RecreatingHandle)
			{
				this.toolTip.Dispose();
				this.toolTip = null;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x001850E0 File Offset: 0x001840E0
		private void OnListChange(object sender, EventArgs e)
		{
			if (!this.DropDownListBox.InSetSelectedIndex())
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				this.Edit.Text = gridEntryFromRow.GetPropertyTextValue(this.DropDownListBox.SelectedItem);
				this.Edit.FocusInternal();
				this.SelectEdit(false);
			}
			this.SetFlag(64, true);
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x0018513F File Offset: 0x0018413F
		private void OnListMouseUp(object sender, MouseEventArgs me)
		{
			this.OnListClick(sender, me);
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x0018514C File Offset: 0x0018414C
		private void OnListClick(object sender, EventArgs e)
		{
			this.GetGridEntryFromRow(this.selectedRow);
			if (this.DropDownListBox.Items.Count == 0)
			{
				this.CommonEditorHide();
				this.SetCommitError(0);
				this.SelectRow(this.selectedRow);
				return;
			}
			object selectedItem = this.DropDownListBox.SelectedItem;
			this.SetFlag(64, false);
			if (selectedItem != null && !this.CommitText((string)selectedItem))
			{
				this.SetCommitError(0);
				this.SelectRow(this.selectedRow);
			}
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x001851CC File Offset: 0x001841CC
		private void OnListDrawItem(object sender, DrawItemEventArgs die)
		{
			int index = die.Index;
			if (index < 0 || this.selectedGridEntry == null)
			{
				return;
			}
			string text = (string)this.DropDownListBox.Items[die.Index];
			die.DrawBackground();
			die.DrawFocusRectangle();
			Rectangle bounds = die.Bounds;
			bounds.Y++;
			bounds.X--;
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			try
			{
				this.DrawValue(die.Graphics, bounds, bounds, gridEntryFromRow, gridEntryFromRow.ConvertTextToValue(text), (die.State & DrawItemState.Selected) != DrawItemState.None, false, false, false);
			}
			catch (FormatException ex)
			{
				this.ShowFormatExceptionMessage(gridEntryFromRow.PropertyLabel, text, ex);
				if (this.DropDownListBox.IsHandleCreated)
				{
					this.DropDownListBox.Visible = false;
				}
			}
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x001852AC File Offset: 0x001842AC
		private void OnListKeyDown(object sender, KeyEventArgs ke)
		{
			if (ke.KeyCode == Keys.Return)
			{
				this.OnListClick(null, null);
				if (this.selectedGridEntry != null)
				{
					this.selectedGridEntry.OnValueReturnKey();
				}
			}
			this.OnKeyDown(sender, ke);
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x001852DC File Offset: 0x001842DC
		protected override void OnLostFocus(EventArgs e)
		{
			if (e != null)
			{
				base.OnLostFocus(e);
			}
			if (this.FocusInside)
			{
				base.OnLostFocus(e);
				return;
			}
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow != null)
			{
				gridEntryFromRow.Focus = false;
				this.CommonEditorHide();
				this.InvalidateRow(this.selectedRow);
			}
			base.OnLostFocus(e);
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x00185334 File Offset: 0x00184334
		private void OnEditChange(object sender, EventArgs e)
		{
			this.SetCommitError(0, this.Edit.Focused);
			this.ToolTip.ToolTip = "";
			this.ToolTip.Visible = false;
			if (!this.Edit.InSetText())
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				if (gridEntryFromRow != null && (gridEntryFromRow.Flags & 8) != 0)
				{
					this.Commit();
				}
			}
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x001853A0 File Offset: 0x001843A0
		private void OnEditGotFocus(object sender, EventArgs e)
		{
			if (!this.Edit.Visible)
			{
				this.FocusInternal();
				return;
			}
			switch (this.errorState)
			{
			case 1:
				if (this.Edit.Visible)
				{
					this.Edit.HookMouseDown = true;
				}
				break;
			case 2:
				return;
			default:
				if (this.NeedsCommit)
				{
					this.SetCommitError(0, true);
				}
				break;
			}
			if (this.selectedGridEntry != null && this.GetRowFromGridEntry(this.selectedGridEntry) != -1)
			{
				this.selectedGridEntry.Focus = true;
				this.InvalidateRow(this.selectedRow);
				(this.Edit.AccessibilityObject as Control.ControlAccessibleObject).NotifyClients(AccessibleEvents.Focus);
				return;
			}
			this.SelectRow(0);
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x00185458 File Offset: 0x00184458
		private void OnEditKeyDown(object sender, KeyEventArgs ke)
		{
			if (!ke.Alt && (ke.KeyCode == Keys.Up || ke.KeyCode == Keys.Down))
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				if (!gridEntryFromRow.Enumerable || !gridEntryFromRow.IsValueEditable)
				{
					return;
				}
				object propertyValue = gridEntryFromRow.PropertyValue;
				object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
				ke.Handled = true;
				if (propertyValueList != null)
				{
					for (int i = 0; i < propertyValueList.Length; i++)
					{
						object obj = propertyValueList[i];
						if (propertyValue != null && obj != null && propertyValue.GetType() != obj.GetType() && gridEntryFromRow.TypeConverter.CanConvertTo(gridEntryFromRow, propertyValue.GetType()))
						{
							obj = gridEntryFromRow.TypeConverter.ConvertTo(gridEntryFromRow, CultureInfo.CurrentCulture, obj, propertyValue.GetType());
						}
						bool flag = propertyValue == obj || (propertyValue != null && propertyValue.Equals(obj));
						if (!flag && propertyValue is string && obj != null)
						{
							flag = (0 == string.Compare((string)propertyValue, obj.ToString(), true, CultureInfo.CurrentCulture));
						}
						if (flag)
						{
							object value;
							if (ke.KeyCode == Keys.Up)
							{
								if (i == 0)
								{
									return;
								}
								value = propertyValueList[i - 1];
							}
							else
							{
								if (i == propertyValueList.Length - 1)
								{
									return;
								}
								value = propertyValueList[i + 1];
							}
							this.CommitValue(value);
							this.SelectEdit(false);
							return;
						}
					}
				}
			}
			else if ((ke.KeyCode == Keys.Left || ke.KeyCode == Keys.Right) && (ke.Modifiers & ~Keys.Shift) != Keys.None)
			{
				return;
			}
			this.OnKeyDown(sender, ke);
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x001855DC File Offset: 0x001845DC
		private void OnEditKeyPress(object sender, KeyPressEventArgs ke)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow == null)
			{
				return;
			}
			if (!gridEntryFromRow.IsTextEditable)
			{
				ke.Handled = this.FilterReadOnlyEditKeyPress(ke.KeyChar);
			}
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x00185614 File Offset: 0x00184614
		private void OnEditLostFocus(object sender, EventArgs e)
		{
			if (this.Edit.Focused || this.errorState == 2 || this.errorState == 1 || this.GetInPropertySet())
			{
				return;
			}
			if (this.dropDownHolder != null && this.dropDownHolder.Visible)
			{
				bool flag = false;
				IntPtr intPtr = UnsafeNativeMethods.GetForegroundWindow();
				while (intPtr != IntPtr.Zero)
				{
					if (intPtr == this.dropDownHolder.Handle)
					{
						flag = true;
					}
					intPtr = UnsafeNativeMethods.GetParent(new HandleRef(null, intPtr));
				}
				if (flag)
				{
					return;
				}
			}
			if (this.FocusInside)
			{
				return;
			}
			if (!this.Commit())
			{
				this.Edit.FocusInternal();
				return;
			}
			this.OnLostFocus(null);
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x001856C0 File Offset: 0x001846C0
		private void OnEditMouseDown(object sender, MouseEventArgs me)
		{
			if (!this.FocusInside)
			{
				this.SelectGridEntry(this.selectedGridEntry, false);
			}
			if (me.Clicks % 2 == 0)
			{
				this.DoubleClickRow(this.selectedRow, false, 2);
				this.Edit.SelectAll();
			}
			if (this.rowSelectTime == 0L)
			{
				return;
			}
			long ticks = DateTime.Now.Ticks;
			int num = (int)((ticks - this.rowSelectTime) / 10000L);
			if (num < SystemInformation.DoubleClickTime)
			{
				Point point = this.Edit.PointToScreen(new Point(me.X, me.Y));
				if (Math.Abs(point.X - this.rowSelectPos.X) < SystemInformation.DoubleClickSize.Width && Math.Abs(point.Y - this.rowSelectPos.Y) < SystemInformation.DoubleClickSize.Height)
				{
					this.DoubleClickRow(this.selectedRow, false, 2);
					this.Edit.SendMessage(514, 0, me.Y << 16 | (me.X & 65535));
					this.Edit.SelectAll();
				}
				this.rowSelectPos = Point.Empty;
				this.rowSelectTime = 0L;
			}
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x001857FB File Offset: 0x001847FB
		private bool OnF4(Control sender)
		{
			if (Control.ModifierKeys != Keys.None)
			{
				return false;
			}
			if (sender == this || sender == this.ownerGrid)
			{
				this.F4Selection(true);
			}
			else
			{
				this.UnfocusSelection();
			}
			return true;
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x00185824 File Offset: 0x00184824
		private bool OnEscape(Control sender)
		{
			if ((Control.ModifierKeys & (Keys.Control | Keys.Alt)) != Keys.None)
			{
				return false;
			}
			this.SetFlag(64, false);
			if (sender != this.Edit || !this.Edit.Focused)
			{
				if (sender != this)
				{
					this.CloseDropDown();
					this.FocusInternal();
				}
				return false;
			}
			if (this.errorState == 0)
			{
				this.Edit.Text = this.originalTextValue;
				this.FocusInternal();
				return true;
			}
			if (this.NeedsCommit)
			{
				bool flag = false;
				this.Edit.Text = this.originalTextValue;
				bool flag2 = true;
				if (this.selectedGridEntry != null)
				{
					string propertyTextValue = this.selectedGridEntry.GetPropertyTextValue();
					flag2 = (this.originalTextValue != propertyTextValue && (!string.IsNullOrEmpty(this.originalTextValue) || !string.IsNullOrEmpty(propertyTextValue)));
				}
				if (flag2)
				{
					try
					{
						flag = this.CommitText(this.originalTextValue);
						goto IL_CC;
					}
					catch
					{
						goto IL_CC;
					}
				}
				flag = true;
				IL_CC:
				if (!flag)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
					return true;
				}
			}
			this.SetCommitError(0);
			this.FocusInternal();
			return true;
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x00185948 File Offset: 0x00184948
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			this.OnKeyDown(this, ke);
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x00185954 File Offset: 0x00184954
		private void OnKeyDown(object sender, KeyEventArgs ke)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow == null)
			{
				return;
			}
			ke.Handled = true;
			bool control = ke.Control;
			bool shift = ke.Shift;
			bool flag = control && shift;
			bool alt = ke.Alt;
			Keys keyCode = ke.KeyCode;
			bool flag2 = false;
			if (keyCode == Keys.Tab && this.ProcessDialogKey(ke.KeyData))
			{
				ke.Handled = true;
				return;
			}
			if (keyCode == Keys.Down && alt && this.DropDownButton.Visible)
			{
				this.F4Selection(false);
				return;
			}
			if (keyCode == Keys.Up && alt && this.DropDownButton.Visible && this.dropDownHolder != null && this.dropDownHolder.Visible)
			{
				this.UnfocusSelection();
				return;
			}
			if (this.ToolTip.Visible)
			{
				this.ToolTip.ToolTip = "";
			}
			if (flag || sender == this || sender == this.ownerGrid)
			{
				Keys keys = keyCode;
				if (keys <= Keys.D8)
				{
					if (keys != Keys.Return)
					{
						switch (keys)
						{
						case Keys.Prior:
						case Keys.Next:
						{
							bool flag3 = keyCode == Keys.Next;
							int num = flag3 ? (this.visibleRows - 1) : (1 - this.visibleRows);
							int row = this.selectedRow;
							if (control && !shift)
							{
								return;
							}
							if (this.selectedRow != -1)
							{
								int scrollOffset = this.GetScrollOffset();
								this.SetScrollOffset(scrollOffset + num);
								this.SetConstants();
								if (this.GetScrollOffset() != scrollOffset + num)
								{
									if (flag3)
									{
										row = this.visibleRows - 1;
									}
									else
									{
										row = 0;
									}
								}
							}
							this.SelectRow(row);
							this.Refresh();
							return;
						}
						case Keys.End:
						case Keys.Home:
						{
							GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
							int index = (keyCode == Keys.Home) ? 0 : (gridEntryCollection.Count - 1);
							this.SelectGridEntry(gridEntryCollection.GetEntry(index), true);
							return;
						}
						case Keys.Left:
							if (control)
							{
								this.MoveSplitterTo(this.InternalLabelWidth - 3);
								return;
							}
							if (gridEntryFromRow.InternalExpanded)
							{
								this.SetExpand(gridEntryFromRow, false);
								return;
							}
							this.SelectGridEntry(this.GetGridEntryFromRow(this.selectedRow - 1), true);
							return;
						case Keys.Up:
						case Keys.Down:
						{
							int row2 = (keyCode == Keys.Up) ? (this.selectedRow - 1) : (this.selectedRow + 1);
							this.SelectGridEntry(this.GetGridEntryFromRow(row2), true);
							this.SetFlag(512, false);
							return;
						}
						case Keys.Right:
							if (control)
							{
								this.MoveSplitterTo(this.InternalLabelWidth + 3);
								return;
							}
							if (!gridEntryFromRow.Expandable)
							{
								this.SelectGridEntry(this.GetGridEntryFromRow(this.selectedRow + 1), true);
								return;
							}
							if (gridEntryFromRow.InternalExpanded)
							{
								GridEntryCollection children = gridEntryFromRow.Children;
								this.SelectGridEntry(children.GetEntry(0), true);
								return;
							}
							this.SetExpand(gridEntryFromRow, true);
							return;
						case Keys.Select:
						case Keys.Print:
						case Keys.Execute:
						case Keys.Snapshot:
							goto IL_444;
						case Keys.Insert:
							if (shift && !control && !alt)
							{
								flag2 = true;
								goto IL_400;
							}
							goto IL_3B6;
						case Keys.Delete:
							if (shift && !control && !alt)
							{
								flag2 = true;
								goto IL_3DA;
							}
							goto IL_444;
						default:
							if (keys != Keys.D8)
							{
								goto IL_444;
							}
							if (!shift)
							{
								goto IL_444;
							}
							break;
						}
					}
					else
					{
						if (gridEntryFromRow.Expandable)
						{
							this.SetExpand(gridEntryFromRow, !gridEntryFromRow.InternalExpanded);
							return;
						}
						gridEntryFromRow.OnValueReturnKey();
						return;
					}
				}
				else if (keys <= Keys.X)
				{
					switch (keys)
					{
					case Keys.A:
						if (control && !alt && !shift && this.Edit.Visible)
						{
							this.Edit.FocusInternal();
							this.Edit.SelectAll();
							goto IL_444;
						}
						goto IL_444;
					case Keys.B:
						goto IL_444;
					case Keys.C:
						goto IL_3B6;
					default:
						switch (keys)
						{
						case Keys.V:
							goto IL_400;
						case Keys.W:
							goto IL_444;
						case Keys.X:
							goto IL_3DA;
						default:
							goto IL_444;
						}
						break;
					}
				}
				else
				{
					switch (keys)
					{
					case Keys.Multiply:
						goto IL_30C;
					case Keys.Add:
					case Keys.Subtract:
						break;
					case Keys.Separator:
						goto IL_444;
					default:
						switch (keys)
						{
						case Keys.Oemplus:
						case Keys.OemMinus:
							break;
						case Keys.Oemcomma:
							goto IL_444;
						default:
							goto IL_444;
						}
						break;
					}
					if (gridEntryFromRow.Expandable)
					{
						this.SetFlag(8, true);
						bool value = keyCode == Keys.Add || keyCode == Keys.Oemplus;
						this.SetExpand(gridEntryFromRow, value);
						base.Invalidate();
						ke.Handled = true;
						return;
					}
					goto IL_444;
				}
				IL_30C:
				this.SetFlag(8, true);
				this.RecursivelyExpand(gridEntryFromRow, true, true, 10);
				ke.Handled = false;
				return;
				IL_3B6:
				if (control && !alt && !shift)
				{
					this.DoCopyCommand();
					return;
				}
				goto IL_444;
				IL_3DA:
				if (flag2 || (control && !alt && !shift))
				{
					Clipboard.SetDataObject(gridEntryFromRow.GetPropertyTextValue());
					this.CommitText("");
					return;
				}
				goto IL_444;
				IL_400:
				if (flag2 || (control && !alt && !shift))
				{
					this.DoPasteCommand();
				}
			}
			IL_444:
			if (gridEntryFromRow != null && ke.KeyData == (Keys)458819)
			{
				Clipboard.SetDataObject(gridEntryFromRow.GetTestingInfo());
				return;
			}
			ke.Handled = false;
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x00185DC8 File Offset: 0x00184DC8
		protected override void OnKeyPress(KeyPressEventArgs ke)
		{
			bool flag = false;
			bool flag2 = false;
			if ((!flag || !flag2) && this.WillFilterKeyPress(ke.KeyChar))
			{
				this.FilterKeyPress(ke.KeyChar);
			}
			this.SetFlag(8, false);
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x00185E08 File Offset: 0x00184E08
		protected override void OnMouseDown(MouseEventArgs me)
		{
			if (me.Button == MouseButtons.Left && this.SplitterInside(me.X, me.Y) && this.totalProps != 0)
			{
				if (!this.Commit())
				{
					return;
				}
				if (me.Clicks == 2)
				{
					this.MoveSplitterTo(base.Width / 2);
					return;
				}
				this.UnfocusSelection();
				this.SetFlag(4, true);
				this.tipInfo = -1;
				base.CaptureInternal = true;
				return;
			}
			else
			{
				Point left = this.FindPosition(me.X, me.Y);
				if (left == PropertyGridView.InvalidPosition)
				{
					return;
				}
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(left.Y);
				if (gridEntryFromRow != null)
				{
					Rectangle rectangle = this.GetRectangle(left.Y, 1);
					this.lastMouseDown = new Point(me.X, me.Y);
					if (me.Button == MouseButtons.Left)
					{
						gridEntryFromRow.OnMouseClick(me.X - rectangle.X, me.Y - rectangle.Y, me.Clicks, me.Button);
					}
					else
					{
						this.SelectGridEntry(gridEntryFromRow, false);
					}
					this.lastMouseDown = PropertyGridView.InvalidPosition;
					gridEntryFromRow.Focus = true;
					this.SetFlag(512, false);
				}
				return;
			}
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x00185F3C File Offset: 0x00184F3C
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!this.GetFlag(4))
			{
				this.Cursor = Cursors.Default;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x00185F5C File Offset: 0x00184F5C
		protected override void OnMouseMove(MouseEventArgs me)
		{
			Point left = Point.Empty;
			bool flag = false;
			int num;
			if (me == null)
			{
				num = -1;
				left = PropertyGridView.InvalidPosition;
			}
			else
			{
				left = this.FindPosition(me.X, me.Y);
				if (left == PropertyGridView.InvalidPosition || (left.X != 1 && left.X != 2))
				{
					num = -1;
					this.ToolTip.ToolTip = "";
				}
				else
				{
					num = left.Y;
					flag = (left.X == 1);
				}
			}
			if (left == PropertyGridView.InvalidPosition || me == null)
			{
				return;
			}
			if (this.GetFlag(4))
			{
				this.MoveSplitterTo(me.X);
			}
			if ((num != this.TipRow || left.X != this.TipColumn) && !this.GetFlag(4))
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(num);
				string text = "";
				this.tipInfo = -1;
				if (gridEntryFromRow != null)
				{
					Rectangle rectangle = this.GetRectangle(left.Y, left.X);
					if (flag && gridEntryFromRow.GetLabelToolTipLocation(me.X - rectangle.X, me.Y - rectangle.Y) != PropertyGridView.InvalidPoint)
					{
						text = gridEntryFromRow.LabelToolTipText;
						this.TipRow = num;
						this.TipColumn = left.X;
					}
					else if (!flag && gridEntryFromRow.ValueToolTipLocation != PropertyGridView.InvalidPoint && !this.Edit.Focused)
					{
						if (!this.NeedsCommit)
						{
							text = gridEntryFromRow.GetPropertyTextValue();
						}
						this.TipRow = num;
						this.TipColumn = left.X;
					}
				}
				IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
				if (UnsafeNativeMethods.IsChild(new HandleRef(null, foregroundWindow), new HandleRef(null, base.Handle)))
				{
					if (this.dropDownHolder == null || this.dropDownHolder.Component == null || num == this.selectedRow)
					{
						this.ToolTip.ToolTip = text;
					}
				}
				else
				{
					this.ToolTip.ToolTip = "";
				}
			}
			if (this.totalProps != 0 && (this.SplitterInside(me.X, me.Y) || this.GetFlag(4)))
			{
				this.Cursor = Cursors.VSplit;
			}
			else
			{
				this.Cursor = Cursors.Default;
			}
			base.OnMouseMove(me);
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x00186190 File Offset: 0x00185190
		protected override void OnMouseUp(MouseEventArgs me)
		{
			this.CancelSplitterMove();
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x00186198 File Offset: 0x00185198
		protected override void OnMouseWheel(MouseEventArgs me)
		{
			this.ownerGrid.OnGridViewMouseWheel(me);
			HandledMouseEventArgs handledMouseEventArgs = me as HandledMouseEventArgs;
			if (handledMouseEventArgs != null)
			{
				if (handledMouseEventArgs.Handled)
				{
					return;
				}
				handledMouseEventArgs.Handled = true;
			}
			if ((Control.ModifierKeys & (Keys.Shift | Keys.Alt)) != Keys.None || Control.MouseButtons != MouseButtons.None)
			{
				return;
			}
			int mouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;
			if (mouseWheelScrollLines == 0)
			{
				return;
			}
			if (this.selectedGridEntry != null && this.selectedGridEntry.Enumerable && this.Edit.Focused && this.selectedGridEntry.IsValueEditable)
			{
				int num = this.GetCurrentValueIndex(this.selectedGridEntry);
				if (num != -1)
				{
					int num2 = (me.Delta > 0) ? -1 : 1;
					object[] propertyValueList = this.selectedGridEntry.GetPropertyValueList();
					if (num2 > 0 && num >= propertyValueList.Length - 1)
					{
						num = 0;
					}
					else if (num2 < 0 && num == 0)
					{
						num = propertyValueList.Length - 1;
					}
					else
					{
						num += num2;
					}
					this.CommitValue(propertyValueList[num]);
					this.SelectGridEntry(this.selectedGridEntry, true);
					this.Edit.FocusInternal();
					return;
				}
			}
			int num3 = this.GetScrollOffset();
			this.cumulativeVerticalWheelDelta += me.Delta;
			float num4 = (float)this.cumulativeVerticalWheelDelta / 120f;
			int num5 = (int)num4;
			if (mouseWheelScrollLines == -1)
			{
				if (num5 != 0)
				{
					int num6 = num3;
					int num7 = num5 * this.scrollBar.LargeChange;
					int num8 = Math.Max(0, num3 - num7);
					num8 = Math.Min(num8, this.totalProps - this.visibleRows + 1);
					num3 -= num5 * this.scrollBar.LargeChange;
					if (Math.Abs(num3 - num6) >= Math.Abs(num5 * this.scrollBar.LargeChange))
					{
						this.cumulativeVerticalWheelDelta -= num5 * 120;
					}
					else
					{
						this.cumulativeVerticalWheelDelta = 0;
					}
					if (!this.ScrollRows(num8))
					{
						this.cumulativeVerticalWheelDelta = 0;
						return;
					}
				}
			}
			else
			{
				int num9 = (int)((float)mouseWheelScrollLines * num4);
				if (num9 != 0)
				{
					if (this.ToolTip.Visible)
					{
						this.ToolTip.ToolTip = "";
					}
					int num10 = Math.Max(0, num3 - num9);
					num10 = Math.Min(num10, this.totalProps - this.visibleRows + 1);
					if (num9 > 0)
					{
						if (this.scrollBar.Value <= this.scrollBar.Minimum)
						{
							this.cumulativeVerticalWheelDelta = 0;
						}
						else
						{
							this.cumulativeVerticalWheelDelta -= (int)((float)num9 * (120f / (float)mouseWheelScrollLines));
						}
					}
					else if (this.scrollBar.Value > this.scrollBar.Maximum - this.visibleRows + 1)
					{
						this.cumulativeVerticalWheelDelta = 0;
					}
					else
					{
						this.cumulativeVerticalWheelDelta -= (int)((float)num9 * (120f / (float)mouseWheelScrollLines));
					}
					if (!this.ScrollRows(num10))
					{
						this.cumulativeVerticalWheelDelta = 0;
						return;
					}
				}
				else
				{
					this.cumulativeVerticalWheelDelta = 0;
				}
			}
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x00186460 File Offset: 0x00185460
		protected override void OnMove(EventArgs e)
		{
			this.CloseDropDown();
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x00186468 File Offset: 0x00185468
		protected override void OnPaintBackground(PaintEventArgs pe)
		{
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x0018646C File Offset: 0x0018546C
		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics graphics = pe.Graphics;
			int num = 0;
			int num2 = 0;
			int num3 = this.visibleRows - 1;
			Rectangle clipRectangle = pe.ClipRectangle;
			clipRectangle.Inflate(0, 2);
			try
			{
				Size size = base.Size;
				Point left = this.FindPosition(clipRectangle.X, clipRectangle.Y);
				Point left2 = this.FindPosition(clipRectangle.X, clipRectangle.Y + clipRectangle.Height);
				if (left != PropertyGridView.InvalidPosition)
				{
					num2 = Math.Max(0, left.Y);
				}
				if (left2 != PropertyGridView.InvalidPosition)
				{
					num3 = left2.Y;
				}
				int num4 = Math.Min(this.totalProps - this.GetScrollOffset(), 1 + this.visibleRows);
				this.SetFlag(1, false);
				Size ourSize = this.GetOurSize();
				Point point = this.ptOurLocation;
				if (this.GetGridEntryFromRow(num4 - 1) == null)
				{
					num4--;
				}
				if (this.totalProps > 0)
				{
					num4 = Math.Min(num4, num3 + 1);
					Pen pen = new Pen(this.ownerGrid.LineColor, (float)this.GetSplitterWidth());
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, this.labelWidth, point.Y, this.labelWidth, num4 * (this.RowHeight + 1) + point.Y);
					pen.Dispose();
					Pen pen2 = new Pen(graphics.GetNearestColor(this.ownerGrid.LineColor));
					int x = point.X + ourSize.Width;
					int x2 = point.X;
					this.GetTotalWidth();
					int num5;
					for (int i = num2; i < num4; i++)
					{
						try
						{
							num5 = i * (this.RowHeight + 1) + point.Y;
							graphics.DrawLine(pen2, x2, num5, x, num5);
							this.DrawValueEntry(graphics, i, ref clipRectangle);
							Rectangle rectangle = this.GetRectangle(i, 1);
							num = rectangle.Y + rectangle.Height;
							this.DrawLabel(graphics, i, rectangle, i == this.selectedRow, false, ref clipRectangle);
							if (i == this.selectedRow)
							{
								this.Edit.Invalidate();
							}
						}
						catch
						{
						}
					}
					num5 = num4 * (this.RowHeight + 1) + point.Y;
					graphics.DrawLine(pen2, x2, num5, x, num5);
					pen2.Dispose();
				}
				if (num < base.Size.Height)
				{
					num++;
					Rectangle rect = new Rectangle(1, num, base.Size.Width - 2, base.Size.Height - num - 1);
					graphics.FillRectangle(this.backgroundBrush, rect);
				}
				graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, size.Width - 1, size.Height - 1);
				this.fontBold = null;
			}
			catch
			{
			}
			finally
			{
				this.ClearCachedFontInfo();
			}
		}

		// Token: 0x060069B4 RID: 27060 RVA: 0x00186790 File Offset: 0x00185790
		private void OnGridEntryLabelDoubleClick(object s, EventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			if (gridEntry != this.lastClickedEntry)
			{
				return;
			}
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			this.DoubleClickRow(rowFromGridEntry, gridEntry.Expandable, 1);
		}

		// Token: 0x060069B5 RID: 27061 RVA: 0x001867C4 File Offset: 0x001857C4
		private void OnGridEntryValueDoubleClick(object s, EventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			if (gridEntry != this.lastClickedEntry)
			{
				return;
			}
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			this.DoubleClickRow(rowFromGridEntry, gridEntry.Expandable, 2);
		}

		// Token: 0x060069B6 RID: 27062 RVA: 0x001867F8 File Offset: 0x001857F8
		private void OnGridEntryLabelClick(object s, EventArgs e)
		{
			this.lastClickedEntry = (GridEntry)s;
			this.SelectGridEntry(this.lastClickedEntry, true);
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x00186814 File Offset: 0x00185814
		private void OnGridEntryOutlineClick(object s, EventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			Cursor cursor = this.Cursor;
			if (!this.ShouldSerializeCursor())
			{
				cursor = null;
			}
			this.Cursor = Cursors.WaitCursor;
			try
			{
				this.SetExpand(gridEntry, !gridEntry.InternalExpanded);
				this.SelectGridEntry(gridEntry, false);
			}
			finally
			{
				this.Cursor = cursor;
			}
		}

		// Token: 0x060069B8 RID: 27064 RVA: 0x00186878 File Offset: 0x00185878
		private void OnGridEntryValueClick(object s, EventArgs e)
		{
			this.lastClickedEntry = (GridEntry)s;
			bool flag = s != this.selectedGridEntry;
			this.SelectGridEntry(this.lastClickedEntry, true);
			this.Edit.FocusInternal();
			if (this.lastMouseDown != PropertyGridView.InvalidPosition)
			{
				this.rowSelectTime = 0L;
				Point p = base.PointToScreen(this.lastMouseDown);
				p = this.Edit.PointToClientInternal(p);
				this.Edit.SendMessage(513, 0, p.Y << 16 | (p.X & 65535));
				this.Edit.SendMessage(514, 0, p.Y << 16 | (p.X & 65535));
			}
			if (flag)
			{
				this.rowSelectTime = DateTime.Now.Ticks;
				this.rowSelectPos = base.PointToScreen(this.lastMouseDown);
				return;
			}
			this.rowSelectTime = 0L;
			this.rowSelectPos = Point.Empty;
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x0018697C File Offset: 0x0018597C
		private void ClearCachedFontInfo()
		{
			if (this.baseHfont != IntPtr.Zero)
			{
				SafeNativeMethods.ExternalDeleteObject(new HandleRef(this, this.baseHfont));
				this.baseHfont = IntPtr.Zero;
			}
			if (this.boldHfont != IntPtr.Zero)
			{
				SafeNativeMethods.ExternalDeleteObject(new HandleRef(this, this.boldHfont));
				this.boldHfont = IntPtr.Zero;
			}
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x001869E8 File Offset: 0x001859E8
		protected override void OnFontChanged(EventArgs e)
		{
			this.ClearCachedFontInfo();
			this.cachedRowHeight = -1;
			if (base.Disposing || this.ParentInternal == null || this.ParentInternal.Disposing)
			{
				return;
			}
			this.fontBold = null;
			this.ToolTip.Font = this.Font;
			this.SetFlag(128, true);
			this.UpdateUIBasedOnFont(true);
			base.OnFontChanged(e);
			if (this.selectedGridEntry != null)
			{
				this.SelectGridEntry(this.selectedGridEntry, true);
			}
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x00186A68 File Offset: 0x00185A68
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Disposing || this.ParentInternal == null || this.ParentInternal.Disposing)
			{
				return;
			}
			if (base.Visible && this.ParentInternal != null)
			{
				this.SetConstants();
				if (this.selectedGridEntry != null)
				{
					this.SelectGridEntry(this.selectedGridEntry, true);
				}
				if (this.toolTip != null)
				{
					this.ToolTip.Font = this.Font;
				}
			}
			base.OnVisibleChanged(e);
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x00186AE0 File Offset: 0x00185AE0
		protected virtual void OnRecreateChildren(object s, GridEntryRecreateChildrenEventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			if (gridEntry.Expanded)
			{
				GridEntry[] array = new GridEntry[this.allGridEntries.Count];
				this.allGridEntries.CopyTo(array, 0);
				int num = -1;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == gridEntry)
					{
						num = i;
						break;
					}
				}
				this.ClearGridEntryEvents(this.allGridEntries, num + 1, e.OldChildCount);
				if (e.OldChildCount != e.NewChildCount)
				{
					int num2 = array.Length + (e.NewChildCount - e.OldChildCount);
					GridEntry[] array2 = new GridEntry[num2];
					Array.Copy(array, 0, array2, 0, num + 1);
					Array.Copy(array, num + e.OldChildCount + 1, array2, num + e.NewChildCount + 1, array.Length - (num + e.OldChildCount + 1));
					array = array2;
				}
				GridEntryCollection children = gridEntry.Children;
				int count = children.Count;
				for (int j = 0; j < count; j++)
				{
					array[num + j + 1] = children.GetEntry(j);
				}
				this.allGridEntries.Clear();
				this.allGridEntries.AddRange(array);
				this.AddGridEntryEvents(this.allGridEntries, num + 1, count);
			}
			if (e.OldChildCount != e.NewChildCount)
			{
				this.totalProps = this.CountPropsFromOutline(this.topLevelGridEntries);
				this.SetConstants();
			}
			base.Invalidate();
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x00186C38 File Offset: 0x00185C38
		protected override void OnResize(EventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			int num = (this.lastClientRect == Rectangle.Empty) ? 0 : (clientRectangle.Height - this.lastClientRect.Height);
			bool visible = this.ScrollBar.Visible;
			if (!this.lastClientRect.IsEmpty && clientRectangle.Width > this.lastClientRect.Width)
			{
				Rectangle rc = new Rectangle(this.lastClientRect.Width - 1, 0, clientRectangle.Width - this.lastClientRect.Width + 1, this.lastClientRect.Height);
				base.Invalidate(rc);
			}
			if (!this.lastClientRect.IsEmpty && num > 0)
			{
				Rectangle rc2 = new Rectangle(0, this.lastClientRect.Height - 1, this.lastClientRect.Width, clientRectangle.Height - this.lastClientRect.Height + 1);
				base.Invalidate(rc2);
			}
			int scrollOffset = this.GetScrollOffset();
			this.SetScrollOffset(0);
			this.SetConstants();
			this.SetScrollOffset(scrollOffset);
			this.CommonEditorHide();
			this.LayoutWindow(false);
			bool fPageIn = this.selectedGridEntry != null && this.selectedRow >= 0 && this.selectedRow <= this.visibleRows;
			this.SelectGridEntry(this.selectedGridEntry, fPageIn);
			this.lastClientRect = clientRectangle;
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x00186D94 File Offset: 0x00185D94
		private void OnScroll(object sender, ScrollEventArgs se)
		{
			if (!this.Commit() || !this.IsScrollValueValid(se.NewValue))
			{
				se.NewValue = this.ScrollBar.Value;
				return;
			}
			int num = -1;
			GridEntry gridEntry = this.selectedGridEntry;
			if (this.selectedGridEntry != null)
			{
				num = this.GetRowFromGridEntry(gridEntry);
			}
			this.ScrollBar.Value = se.NewValue;
			if (gridEntry != null)
			{
				this.selectedRow = -1;
				this.SelectGridEntry(gridEntry, this.ScrollBar.Value == this.totalProps);
				int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				if (num != rowFromGridEntry)
				{
					base.Invalidate();
					return;
				}
			}
			else
			{
				base.Invalidate();
			}
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x00186E34 File Offset: 0x00185E34
		private void OnSysColorChange(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Color || e.Category == UserPreferenceCategory.Accessibility)
			{
				this.SetFlag(128, true);
			}
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x00186E54 File Offset: 0x00185E54
		public virtual void PopupDialog(int row)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow != null)
			{
				if (this.dropDownHolder != null && this.dropDownHolder.GetUsed())
				{
					this.CloseDropDown();
					return;
				}
				bool needsDropDownButton = gridEntryFromRow.NeedsDropDownButton;
				bool enumerable = gridEntryFromRow.Enumerable;
				bool needsCustomEditorButton = gridEntryFromRow.NeedsCustomEditorButton;
				if (enumerable && !needsDropDownButton)
				{
					this.DropDownListBox.Items.Clear();
					object propertyValue = gridEntryFromRow.PropertyValue;
					object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
					int num = 0;
					IntPtr dc = UnsafeNativeMethods.GetDC(new HandleRef(this.DropDownListBox, this.DropDownListBox.Handle));
					IntPtr handle = this.Font.ToHfont();
					System.Internal.HandleCollector.Add(handle, NativeMethods.CommonHandles.GDI);
					NativeMethods.TEXTMETRIC textmetric = default(NativeMethods.TEXTMETRIC);
					int num2 = -1;
					try
					{
						handle = SafeNativeMethods.SelectObject(new HandleRef(this.DropDownListBox, dc), new HandleRef(this.Font, handle));
						num2 = this.GetCurrentValueIndex(gridEntryFromRow);
						if (propertyValueList != null && propertyValueList.Length > 0)
						{
							IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
							for (int i = 0; i < propertyValueList.Length; i++)
							{
								string propertyTextValue = gridEntryFromRow.GetPropertyTextValue(propertyValueList[i]);
								this.DropDownListBox.Items.Add(propertyTextValue);
								IntUnsafeNativeMethods.GetTextExtentPoint32(new HandleRef(this.DropDownListBox, dc), propertyTextValue, size);
								num = Math.Max(size.cx, num);
							}
						}
						SafeNativeMethods.GetTextMetrics(new HandleRef(this.DropDownListBox, dc), ref textmetric);
						num += 2 + textmetric.tmMaxCharWidth + SystemInformation.VerticalScrollBarWidth;
						handle = SafeNativeMethods.SelectObject(new HandleRef(this.DropDownListBox, dc), new HandleRef(this.Font, handle));
					}
					finally
					{
						SafeNativeMethods.DeleteObject(new HandleRef(this.Font, handle));
						UnsafeNativeMethods.ReleaseDC(new HandleRef(this.DropDownListBox, this.DropDownListBox.Handle), new HandleRef(this.DropDownListBox, dc));
					}
					if (num2 != -1)
					{
						this.DropDownListBox.SelectedIndex = num2;
					}
					this.SetFlag(64, false);
					this.DropDownListBox.Height = Math.Max(textmetric.tmHeight + 2, Math.Min(200, this.DropDownListBox.PreferredHeight));
					this.DropDownListBox.Width = Math.Max(num, this.GetRectangle(row, 2).Width);
					try
					{
						bool value = this.DropDownListBox.Items.Count > this.DropDownListBox.Height / this.DropDownListBox.ItemHeight;
						this.SetFlag(1024, value);
						this.DropDownControl(this.DropDownListBox);
					}
					finally
					{
						this.SetFlag(1024, false);
					}
					this.Refresh();
					return;
				}
				if (!needsCustomEditorButton)
				{
					if (!needsDropDownButton)
					{
						return;
					}
				}
				try
				{
					this.SetFlag(16, true);
					this.Edit.DisableMouseHook = true;
					try
					{
						this.SetFlag(1024, gridEntryFromRow.UITypeEditor.IsDropDownResizable);
						gridEntryFromRow.EditPropertyValue(this);
					}
					finally
					{
						this.SetFlag(1024, false);
					}
				}
				finally
				{
					this.SetFlag(16, false);
					this.Edit.DisableMouseHook = false;
				}
				this.Refresh();
				if (this.FocusInside)
				{
					this.SelectGridEntry(gridEntryFromRow, false);
				}
			}
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x001871A0 File Offset: 0x001861A0
		internal static void PositionTooltip(Control parent, GridToolTip ToolTip, Rectangle itemRect)
		{
			ToolTip.Visible = false;
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height);
			ToolTip.SendMessage(1055, 1, ref rect);
			Point location = parent.PointToScreen(new Point(rect.left, rect.top));
			ToolTip.Location = location;
			int num = ToolTip.Location.X + ToolTip.Size.Width - SystemInformation.VirtualScreen.Width;
			if (num > 0)
			{
				location.X -= num;
				ToolTip.Location = location;
			}
			ToolTip.Visible = true;
		}

		// Token: 0x060069C2 RID: 27074 RVA: 0x00187258 File Offset: 0x00186258
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (this.HasEntries)
			{
				Keys keys = keyData & Keys.KeyCode;
				Keys keys2 = keys;
				if (keys2 <= Keys.Return)
				{
					if (keys2 != Keys.Tab)
					{
						if (keys2 == Keys.Return)
						{
							if (this.DialogButton.Focused || this.DropDownButton.Focused)
							{
								this.OnBtnClick(this.DialogButton.Focused ? this.DialogButton : this.DropDownButton, new EventArgs());
								return true;
							}
							if (this.selectedGridEntry != null && this.selectedGridEntry.Expandable)
							{
								this.SetExpand(this.selectedGridEntry, !this.selectedGridEntry.InternalExpanded);
								return true;
							}
						}
					}
					else if ((keyData & Keys.Control) == Keys.None && (keyData & Keys.Alt) == Keys.None)
					{
						bool flag = (keyData & Keys.Shift) == Keys.None;
						Control control = Control.FromHandleInternal(UnsafeNativeMethods.GetFocus());
						if (control == null || !this.IsMyChild(control))
						{
							if (flag)
							{
								this.TabSelection();
								control = Control.FromHandleInternal(UnsafeNativeMethods.GetFocus());
								return this.IsMyChild(control) || base.ProcessDialogKey(keyData);
							}
						}
						else if (this.Edit.Focused)
						{
							if (!flag)
							{
								this.SelectGridEntry(this.GetGridEntryFromRow(this.selectedRow), false);
								return true;
							}
							if (this.DropDownButton.Visible)
							{
								this.DropDownButton.FocusInternal();
								return true;
							}
							if (this.DialogButton.Visible)
							{
								this.DialogButton.FocusInternal();
								return true;
							}
						}
						else if ((this.DialogButton.Focused || this.DropDownButton.Focused) && !flag && this.Edit.Visible)
						{
							this.Edit.FocusInternal();
							return true;
						}
					}
				}
				else
				{
					switch (keys2)
					{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						return false;
					default:
						if (keys2 == Keys.F4 && this.FocusInside)
						{
							return this.OnF4(this);
						}
						break;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x060069C3 RID: 27075 RVA: 0x00187444 File Offset: 0x00186444
		protected virtual void RecalculateProps()
		{
			int num = this.CountPropsFromOutline(this.topLevelGridEntries);
			if (this.totalProps != num)
			{
				this.totalProps = num;
				this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
				this.allGridEntries = null;
			}
		}

		// Token: 0x060069C4 RID: 27076 RVA: 0x00187484 File Offset: 0x00186484
		internal void RecursivelyExpand(GridEntry gridEntry, bool fInit, bool expand, int maxExpands)
		{
			if (gridEntry == null || (expand && --maxExpands < 0))
			{
				return;
			}
			this.SetExpand(gridEntry, expand);
			GridEntryCollection children = gridEntry.Children;
			if (children != null)
			{
				for (int i = 0; i < children.Count; i++)
				{
					this.RecursivelyExpand(children.GetEntry(i), false, expand, maxExpands);
				}
			}
			if (fInit)
			{
				GridEntry gridEntry2 = this.selectedGridEntry;
				this.Refresh();
				this.SelectGridEntry(gridEntry2, false);
				base.Invalidate();
			}
		}

		// Token: 0x060069C5 RID: 27077 RVA: 0x001874F4 File Offset: 0x001864F4
		public override void Refresh()
		{
			this.Refresh(false, -1, -1);
			base.Invalidate();
		}

		// Token: 0x060069C6 RID: 27078 RVA: 0x00187505 File Offset: 0x00186505
		public void Refresh(bool fullRefresh)
		{
			this.Refresh(fullRefresh, -1, -1);
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x00187510 File Offset: 0x00186510
		private void Refresh(bool fullRefresh, int rowStart, int rowEnd)
		{
			this.SetFlag(1, true);
			GridEntry gridEntry = null;
			if (base.IsDisposed)
			{
				return;
			}
			bool fPageIn = true;
			if (rowStart == -1)
			{
				rowStart = 0;
			}
			if (fullRefresh || this.ownerGrid.HavePropEntriesChanged())
			{
				if (this.HasEntries && !this.GetInPropertySet() && !this.Commit())
				{
					this.OnEscape(this);
				}
				int num = this.totalProps;
				object obj = (this.topLevelGridEntries == null || this.topLevelGridEntries.Count == 0) ? null : ((GridEntry)this.topLevelGridEntries[0]).GetValueOwner();
				if (fullRefresh)
				{
					this.ownerGrid.RefreshProperties(true);
				}
				if (num > 0 && !this.GetFlag(512))
				{
					this.positionData = this.CaptureGridPositionData();
					this.CommonEditorHide(true);
				}
				this.UpdateHelpAttributes(this.selectedGridEntry, null);
				this.selectedGridEntry = null;
				this.SetFlag(2, true);
				this.topLevelGridEntries = this.ownerGrid.GetPropEntries();
				this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
				this.allGridEntries = null;
				this.RecalculateProps();
				int num2 = this.totalProps;
				if (num2 > 0)
				{
					if (num2 < num)
					{
						this.SetScrollbarLength();
						this.SetScrollOffset(0);
					}
					this.SetConstants();
					if (this.positionData != null)
					{
						gridEntry = this.positionData.Restore(this);
						object obj2 = (this.topLevelGridEntries == null || this.topLevelGridEntries.Count == 0) ? null : ((GridEntry)this.topLevelGridEntries[0]).GetValueOwner();
						fPageIn = (gridEntry == null || num != num2 || obj2 != obj);
					}
					if (gridEntry == null)
					{
						gridEntry = this.ownerGrid.GetDefaultGridEntry();
						this.SetFlag(512, gridEntry == null && this.totalProps > 0);
					}
					this.InvalidateRows(rowStart, rowEnd);
					if (gridEntry == null)
					{
						this.selectedRow = 0;
						this.selectedGridEntry = this.GetGridEntryFromRow(this.selectedRow);
					}
					this.positionData = null;
				}
				else
				{
					if (num == 0)
					{
						return;
					}
					this.SetConstants();
				}
			}
			if (!this.HasEntries)
			{
				this.CommonEditorHide(this.selectedRow != -1);
				this.ownerGrid.SetStatusBox(null, null);
				this.SetScrollOffset(0);
				this.selectedRow = -1;
				base.Invalidate();
				return;
			}
			this.ownerGrid.ClearValueCaches();
			this.InvalidateRows(rowStart, rowEnd);
			if (gridEntry != null)
			{
				this.SelectGridEntry(gridEntry, fPageIn);
			}
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x0018775C File Offset: 0x0018675C
		public virtual void Reset()
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow == null)
			{
				return;
			}
			gridEntryFromRow.ResetPropertyValue();
			this.SelectRow(this.selectedRow);
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x0018778C File Offset: 0x0018678C
		protected virtual void ResetOrigin(Graphics g)
		{
			g.ResetTransform();
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x00187794 File Offset: 0x00186794
		internal void RestoreHierarchyState(ArrayList expandedItems)
		{
			if (expandedItems == null)
			{
				return;
			}
			foreach (object obj in expandedItems)
			{
				GridEntryCollection ipeHier = (GridEntryCollection)obj;
				this.FindEquivalentGridEntry(ipeHier);
			}
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x001877F0 File Offset: 0x001867F0
		public virtual DialogResult RunDialog(Form dialog)
		{
			return this.ShowDialog(dialog);
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x001877F9 File Offset: 0x001867F9
		internal ArrayList SaveHierarchyState(GridEntryCollection entries)
		{
			return this.SaveHierarchyState(entries, null);
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x00187804 File Offset: 0x00186804
		private ArrayList SaveHierarchyState(GridEntryCollection entries, ArrayList expandedItems)
		{
			if (entries == null)
			{
				return new ArrayList();
			}
			if (expandedItems == null)
			{
				expandedItems = new ArrayList();
			}
			for (int i = 0; i < entries.Count; i++)
			{
				if (((GridEntry)entries[i]).InternalExpanded)
				{
					GridEntry entry = entries.GetEntry(i);
					expandedItems.Add(this.GetGridEntryHierarchy(entry.Children.GetEntry(0)));
					this.SaveHierarchyState(entry.Children, expandedItems);
				}
			}
			return expandedItems;
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x00187878 File Offset: 0x00186878
		private bool ScrollRows(int newOffset)
		{
			GridEntry gridEntry = this.selectedGridEntry;
			if (!this.IsScrollValueValid(newOffset) || !this.Commit())
			{
				return false;
			}
			bool visible = this.Edit.Visible;
			bool visible2 = this.DropDownButton.Visible;
			bool visible3 = this.DialogButton.Visible;
			this.Edit.Visible = false;
			this.DialogButton.Visible = false;
			this.DropDownButton.Visible = false;
			this.SetScrollOffset(newOffset);
			if (gridEntry != null)
			{
				int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				if (rowFromGridEntry >= 0 && rowFromGridEntry < this.visibleRows - 1)
				{
					this.Edit.Visible = visible;
					this.DialogButton.Visible = visible3;
					this.DropDownButton.Visible = visible2;
					this.SelectGridEntry(gridEntry, true);
				}
				else
				{
					this.CommonEditorHide();
				}
			}
			else
			{
				this.CommonEditorHide();
			}
			base.Invalidate();
			return true;
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x0018794E File Offset: 0x0018694E
		private void SelectEdit(bool caretAtEnd)
		{
			if (this.edit != null)
			{
				this.Edit.SelectAll();
			}
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x00187964 File Offset: 0x00186964
		internal void SelectGridEntry(GridEntry gridEntry, bool fPageIn)
		{
			if (gridEntry == null)
			{
				return;
			}
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			if (rowFromGridEntry + this.GetScrollOffset() < 0)
			{
				return;
			}
			int num = (int)Math.Ceiling((double)this.GetOurSize().Height / (double)(1 + this.RowHeight));
			if (!fPageIn || (rowFromGridEntry >= 0 && rowFromGridEntry < num - 1))
			{
				this.SelectRow(rowFromGridEntry);
				return;
			}
			this.selectedRow = -1;
			int scrollOffset = this.GetScrollOffset();
			if (rowFromGridEntry < 0)
			{
				this.SetScrollOffset(rowFromGridEntry + scrollOffset);
				base.Invalidate();
				this.SelectRow(0);
				return;
			}
			int num2 = rowFromGridEntry + scrollOffset - (num - 2);
			if (num2 >= this.ScrollBar.Minimum && num2 < this.ScrollBar.Maximum)
			{
				this.SetScrollOffset(num2);
			}
			base.Invalidate();
			this.SelectGridEntry(gridEntry, false);
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x00187A24 File Offset: 0x00186A24
		private void SelectRow(int row)
		{
			if (!this.GetFlag(2))
			{
				if (this.FocusInside)
				{
					if (this.errorState != 0 || (row != this.selectedRow && !this.Commit()))
					{
						return;
					}
				}
				else
				{
					this.FocusInternal();
				}
			}
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (row != this.selectedRow)
			{
				this.UpdateResetCommand(gridEntryFromRow);
			}
			if (this.GetFlag(2) && this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				this.CommonEditorHide();
			}
			this.UpdateHelpAttributes(this.selectedGridEntry, gridEntryFromRow);
			if (this.selectedGridEntry != null)
			{
				this.selectedGridEntry.Focus = false;
			}
			if (row < 0 || row >= this.visibleRows)
			{
				this.CommonEditorHide();
				this.selectedRow = row;
				this.selectedGridEntry = gridEntryFromRow;
				this.Refresh();
				return;
			}
			if (gridEntryFromRow == null)
			{
				return;
			}
			bool flag = false;
			int row2 = this.selectedRow;
			if (this.selectedRow != row || !gridEntryFromRow.Equals(this.selectedGridEntry))
			{
				this.CommonEditorHide();
				flag = true;
			}
			if (!flag)
			{
				this.CloseDropDown();
			}
			Rectangle rectangle = this.GetRectangle(row, 2);
			string propertyTextValue = gridEntryFromRow.GetPropertyTextValue();
			bool flag2 = gridEntryFromRow.NeedsDropDownButton | gridEntryFromRow.Enumerable;
			bool needsCustomEditorButton = gridEntryFromRow.NeedsCustomEditorButton;
			bool isTextEditable = gridEntryFromRow.IsTextEditable;
			bool isCustomPaint = gridEntryFromRow.IsCustomPaint;
			rectangle.X++;
			rectangle.Width--;
			if ((needsCustomEditorButton || flag2) && !gridEntryFromRow.ShouldRenderReadOnly && this.FocusInside)
			{
				Control control = flag2 ? this.DropDownButton : this.DialogButton;
				Size size = new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight);
				Rectangle rectTarget = new Rectangle(rectangle.X + rectangle.Width - size.Width, rectangle.Y, size.Width, rectangle.Height);
				this.CommonEditorUse(control, rectTarget);
				size = control.Size;
				rectangle.Width -= size.Width;
				control.Invalidate();
			}
			if (isCustomPaint)
			{
				rectangle.X += 27;
				rectangle.Width -= 27;
			}
			else
			{
				rectangle.X++;
				rectangle.Width--;
			}
			if ((this.GetFlag(2) || !this.Edit.Focused) && propertyTextValue != null && !propertyTextValue.Equals(this.Edit.Text))
			{
				this.Edit.Text = propertyTextValue;
				this.originalTextValue = propertyTextValue;
				this.Edit.SelectionStart = 0;
				this.Edit.SelectionLength = 0;
			}
			this.Edit.AccessibleName = gridEntryFromRow.Label;
			switch (PropertyGridView.inheritRenderMode)
			{
			case 2:
				if (gridEntryFromRow.ShouldSerializePropertyValue())
				{
					rectangle.X += 8;
					rectangle.Width -= 8;
				}
				break;
			case 3:
				if (gridEntryFromRow.ShouldSerializePropertyValue())
				{
					this.Edit.Font = this.GetBoldFont();
				}
				else
				{
					this.Edit.Font = this.Font;
				}
				break;
			}
			if (this.GetFlag(4) || !gridEntryFromRow.HasValue || !this.FocusInside)
			{
				this.Edit.Visible = false;
			}
			else
			{
				rectangle.Offset(1, 1);
				rectangle.Height--;
				rectangle.Width--;
				this.CommonEditorUse(this.Edit, rectangle);
				bool shouldRenderReadOnly = gridEntryFromRow.ShouldRenderReadOnly;
				this.Edit.ForeColor = (shouldRenderReadOnly ? this.GrayTextColor : this.ForeColor);
				this.Edit.BackColor = this.BackColor;
				this.Edit.ReadOnly = (shouldRenderReadOnly || !gridEntryFromRow.IsTextEditable);
				this.Edit.UseSystemPasswordChar = gridEntryFromRow.ShouldRenderPassword;
			}
			GridEntry gridEntry = this.selectedGridEntry;
			this.selectedRow = row;
			this.selectedGridEntry = gridEntryFromRow;
			this.ownerGrid.SetStatusBox(gridEntryFromRow.PropertyLabel, gridEntryFromRow.PropertyDescription);
			if (this.selectedGridEntry != null)
			{
				this.selectedGridEntry.Focus = this.FocusInside;
			}
			if (!this.GetFlag(2))
			{
				this.FocusInternal();
			}
			this.InvalidateRow(row2);
			this.InvalidateRow(row);
			if (this.FocusInside)
			{
				this.SetFlag(2, false);
			}
			try
			{
				if (this.selectedGridEntry != gridEntry)
				{
					this.ownerGrid.OnSelectedGridItemChanged(gridEntry, this.selectedGridEntry);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x00187E9C File Offset: 0x00186E9C
		public virtual void SetConstants()
		{
			this.visibleRows = (int)Math.Ceiling((double)this.GetOurSize().Height / (double)(1 + this.RowHeight));
			Size ourSize = this.GetOurSize();
			if (ourSize.Width >= 0)
			{
				this.labelRatio = Math.Max(Math.Min(this.labelRatio, 9.0), 1.1);
				this.labelWidth = this.ptOurLocation.X + (int)((double)ourSize.Width / this.labelRatio);
			}
			int num = this.labelWidth;
			bool flag = this.SetScrollbarLength();
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection != null)
			{
				int scrollOffset = this.GetScrollOffset();
				if (scrollOffset + this.visibleRows >= gridEntryCollection.Count)
				{
					this.visibleRows = gridEntryCollection.Count - scrollOffset;
				}
			}
			if (flag && ourSize.Width >= 0)
			{
				this.labelRatio = (double)this.GetOurSize().Width / (double)(num - this.ptOurLocation.X);
			}
		}

		// Token: 0x060069D3 RID: 27091 RVA: 0x00187F9B File Offset: 0x00186F9B
		private void SetCommitError(short error)
		{
			this.SetCommitError(error, error == 1);
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x00187FA8 File Offset: 0x00186FA8
		private void SetCommitError(short error, bool capture)
		{
			this.errorState = error;
			if (error != 0)
			{
				this.CancelSplitterMove();
			}
			this.Edit.HookMouseDown = capture;
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x00187FC8 File Offset: 0x00186FC8
		internal void SetExpand(GridEntry gridEntry, bool value)
		{
			if (gridEntry != null && gridEntry.Expandable)
			{
				int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				int num = this.selectedRow;
				if (this.selectedRow != -1 && rowFromGridEntry < this.selectedRow && this.Edit.Visible)
				{
					this.FocusInternal();
				}
				int scrollOffset = this.GetScrollOffset();
				int num2 = this.totalProps;
				gridEntry.InternalExpanded = value;
				this.RecalculateProps();
				GridEntry gridEntry2 = this.selectedGridEntry;
				if (!value)
				{
					for (GridEntry gridEntry3 = gridEntry2; gridEntry3 != null; gridEntry3 = gridEntry3.ParentGridEntry)
					{
						if (gridEntry3.Equals(gridEntry))
						{
							gridEntry2 = gridEntry;
						}
					}
				}
				rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				this.SetConstants();
				int num3 = this.totalProps - num2;
				if (value && num3 > 0 && num3 < this.visibleRows && rowFromGridEntry + num3 >= this.visibleRows && num3 < num)
				{
					this.SetScrollOffset(this.totalProps - num2 + scrollOffset);
				}
				base.Invalidate();
				this.SelectGridEntry(gridEntry2, false);
				int scrollOffset2 = this.GetScrollOffset();
				this.SetScrollOffset(0);
				this.SetConstants();
				this.SetScrollOffset(scrollOffset2);
			}
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x001880D8 File Offset: 0x001870D8
		private void SetFlag(short flag, bool value)
		{
			if (value)
			{
				this.flags = (short)((ushort)this.flags | (ushort)flag);
				return;
			}
			this.flags &= ~flag;
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x00188100 File Offset: 0x00187100
		public virtual void SetScrollOffset(int cOffset)
		{
			int num = Math.Max(0, Math.Min(this.totalProps - this.visibleRows + 1, cOffset));
			int value = this.ScrollBar.Value;
			if (num != value && this.IsScrollValueValid(num) && this.visibleRows > 0)
			{
				this.ScrollBar.Value = num;
				base.Invalidate();
				this.selectedRow = this.GetRowFromGridEntry(this.selectedGridEntry);
			}
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x0018816F File Offset: 0x0018716F
		internal virtual bool _Commit()
		{
			return this.Commit();
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x00188178 File Offset: 0x00187178
		private bool Commit()
		{
			if (this.errorState == 2)
			{
				return false;
			}
			if (!this.NeedsCommit)
			{
				this.SetCommitError(0);
				return true;
			}
			if (this.GetInPropertySet())
			{
				return false;
			}
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return true;
			}
			bool flag = false;
			try
			{
				flag = this.CommitText(this.Edit.Text);
			}
			finally
			{
				if (!flag)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
				}
				else
				{
					this.SetCommitError(0);
				}
			}
			return flag;
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x00188204 File Offset: 0x00187204
		private bool CommitValue(object value)
		{
			GridEntry gridEntryFromRow = this.selectedGridEntry;
			if (this.selectedGridEntry == null && this.selectedRow != -1)
			{
				gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			}
			return gridEntryFromRow == null || this.CommitValue(gridEntryFromRow, value);
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x00188244 File Offset: 0x00187244
		internal bool CommitValue(GridEntry ipeCur, object value)
		{
			int childCount = ipeCur.ChildCount;
			bool hookMouseDown = this.Edit.HookMouseDown;
			object oldValue = null;
			try
			{
				oldValue = ipeCur.PropertyValue;
			}
			catch
			{
			}
			try
			{
				this.SetFlag(16, true);
				if (ipeCur != null && ipeCur.Enumerable)
				{
					this.CloseDropDown();
				}
				try
				{
					this.Edit.DisableMouseHook = true;
					ipeCur.PropertyValue = value;
				}
				finally
				{
					this.Edit.DisableMouseHook = false;
					this.Edit.HookMouseDown = hookMouseDown;
				}
			}
			catch (Exception ex)
			{
				this.SetCommitError(1);
				this.ShowInvalidMessage(ipeCur.PropertyLabel, value, ex);
				return false;
			}
			finally
			{
				this.SetFlag(16, false);
			}
			this.SetCommitError(0);
			string propertyTextValue = ipeCur.GetPropertyTextValue();
			if (!string.Equals(propertyTextValue, this.Edit.Text))
			{
				this.Edit.Text = propertyTextValue;
				this.Edit.SelectionStart = 0;
				this.Edit.SelectionLength = 0;
			}
			this.originalTextValue = propertyTextValue;
			this.UpdateResetCommand(ipeCur);
			if (ipeCur.ChildCount != childCount)
			{
				this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
				this.allGridEntries = null;
				this.SelectGridEntry(ipeCur, true);
			}
			if (ipeCur.Disposed)
			{
				bool flag = this.edit != null && this.edit.Focused;
				this.SelectGridEntry(ipeCur, true);
				ipeCur = this.selectedGridEntry;
				if (flag && this.edit != null)
				{
					this.edit.Focus();
				}
			}
			this.ownerGrid.OnPropertyValueSet(ipeCur, oldValue);
			return true;
		}

		// Token: 0x060069DC RID: 27100 RVA: 0x001883F0 File Offset: 0x001873F0
		private bool CommitText(string text)
		{
			object value = null;
			GridEntry gridEntryFromRow = this.selectedGridEntry;
			if (this.selectedGridEntry == null && this.selectedRow != -1)
			{
				gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			}
			if (gridEntryFromRow == null)
			{
				return true;
			}
			try
			{
				value = gridEntryFromRow.ConvertTextToValue(text);
			}
			catch (Exception ex)
			{
				this.SetCommitError(1);
				this.ShowInvalidMessage(gridEntryFromRow.PropertyLabel, text, ex);
				return false;
			}
			this.SetCommitError(0);
			return this.CommitValue(value);
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x00188470 File Offset: 0x00187470
		internal void ReverseFocus()
		{
			if (this.selectedGridEntry == null)
			{
				this.FocusInternal();
				return;
			}
			this.SelectGridEntry(this.selectedGridEntry, true);
			if (this.DialogButton.Visible)
			{
				this.DialogButton.FocusInternal();
				return;
			}
			if (this.DropDownButton.Visible)
			{
				this.DropDownButton.FocusInternal();
				return;
			}
			if (this.Edit.Visible)
			{
				this.Edit.SelectAll();
				this.Edit.FocusInternal();
			}
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x001884F4 File Offset: 0x001874F4
		private bool SetScrollbarLength()
		{
			bool result = false;
			if (this.totalProps != -1)
			{
				if (this.totalProps < this.visibleRows)
				{
					this.SetScrollOffset(0);
				}
				else if (this.GetScrollOffset() > this.totalProps)
				{
					this.SetScrollOffset(this.totalProps + 1 - this.visibleRows);
				}
				bool flag = !this.ScrollBar.Visible;
				if (this.visibleRows > 0)
				{
					this.ScrollBar.LargeChange = this.visibleRows - 1;
				}
				this.ScrollBar.Maximum = Math.Max(0, this.totalProps - 1);
				if (flag != this.totalProps < this.visibleRows)
				{
					result = true;
					this.ScrollBar.Visible = flag;
					Size ourSize = this.GetOurSize();
					if (this.labelWidth != -1 && ourSize.Width > 0)
					{
						if (this.labelWidth > this.ptOurLocation.X + ourSize.Width)
						{
							this.labelWidth = this.ptOurLocation.X + (int)((double)ourSize.Width / this.labelRatio);
						}
						else
						{
							this.labelRatio = (double)this.GetOurSize().Width / (double)(this.labelWidth - this.ptOurLocation.X);
						}
					}
					base.Invalidate();
				}
			}
			return result;
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x00188638 File Offset: 0x00187638
		public DialogResult ShowDialog(Form dialog)
		{
			if (dialog.StartPosition == FormStartPosition.CenterScreen)
			{
				Control control = this;
				if (control != null)
				{
					while (control.ParentInternal != null)
					{
						control = control.ParentInternal;
					}
					if (control.Size.Equals(dialog.Size))
					{
						dialog.StartPosition = FormStartPosition.Manual;
						Point location = control.Location;
						location.Offset(25, 25);
						dialog.Location = location;
					}
				}
			}
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			DialogResult result;
			if (iuiservice != null)
			{
				result = iuiservice.ShowDialog(dialog);
			}
			else
			{
				result = dialog.ShowDialog(this);
			}
			if (focus != IntPtr.Zero)
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
			}
			return result;
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x001886F8 File Offset: 0x001876F8
		private void ShowFormatExceptionMessage(string propName, object value, Exception ex)
		{
			if (value == null)
			{
				value = "(null)";
			}
			if (propName == null)
			{
				propName = "(unknown)";
			}
			bool hookMouseDown = this.Edit.HookMouseDown;
			this.Edit.DisableMouseHook = true;
			this.SetCommitError(2, false);
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			while (UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 512, 522, 1))
			{
			}
			if (ex is TargetInvocationException)
			{
				ex = ex.InnerException;
			}
			string message = ex.Message;
			while (message == null || message.Length == 0)
			{
				ex = ex.InnerException;
				if (ex == null)
				{
					break;
				}
				message = ex.Message;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			this.ErrorDialog.Message = SR.GetString("PBRSFormatExceptionMessage");
			this.ErrorDialog.Text = SR.GetString("PBRSErrorTitle");
			this.ErrorDialog.Details = message;
			bool flag;
			if (iuiservice != null)
			{
				flag = (DialogResult.Cancel == iuiservice.ShowDialog(this.ErrorDialog));
			}
			else
			{
				flag = (DialogResult.Cancel == this.ShowDialog(this.ErrorDialog));
			}
			this.Edit.DisableMouseHook = false;
			if (hookMouseDown)
			{
				this.SelectGridEntry(this.selectedGridEntry, true);
			}
			this.SetCommitError(1, hookMouseDown);
			if (flag)
			{
				this.OnEscape(this.Edit);
			}
		}

		// Token: 0x060069E1 RID: 27105 RVA: 0x0018883C File Offset: 0x0018783C
		private void ShowInvalidMessage(string propName, object value, Exception ex)
		{
			if (value == null)
			{
				value = "(null)";
			}
			if (propName == null)
			{
				propName = "(unknown)";
			}
			bool hookMouseDown = this.Edit.HookMouseDown;
			this.Edit.DisableMouseHook = true;
			this.SetCommitError(2, false);
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			while (UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 512, 522, 1))
			{
			}
			if (ex is TargetInvocationException)
			{
				ex = ex.InnerException;
			}
			string message = ex.Message;
			while (message == null || message.Length == 0)
			{
				ex = ex.InnerException;
				if (ex == null)
				{
					break;
				}
				message = ex.Message;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			this.ErrorDialog.Message = SR.GetString("PBRSErrorInvalidPropertyValue");
			this.ErrorDialog.Text = SR.GetString("PBRSErrorTitle");
			this.ErrorDialog.Details = message;
			bool flag;
			if (iuiservice != null)
			{
				flag = (DialogResult.Cancel == iuiservice.ShowDialog(this.ErrorDialog));
			}
			else
			{
				flag = (DialogResult.Cancel == this.ShowDialog(this.ErrorDialog));
			}
			this.Edit.DisableMouseHook = false;
			if (hookMouseDown)
			{
				this.SelectGridEntry(this.selectedGridEntry, true);
			}
			this.SetCommitError(1, hookMouseDown);
			if (flag)
			{
				this.OnEscape(this.Edit);
			}
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x0018897E File Offset: 0x0018797E
		private bool SplitterInside(int x, int y)
		{
			return Math.Abs(x - this.InternalLabelWidth) < 4;
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x00188990 File Offset: 0x00187990
		private void TabSelection()
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return;
			}
			if (this.Edit.Visible)
			{
				this.Edit.FocusInternal();
				this.SelectEdit(false);
				return;
			}
			if (this.dropDownHolder != null && this.dropDownHolder.Visible)
			{
				this.dropDownHolder.FocusComponent();
				return;
			}
			if (this.currentEditor != null)
			{
				this.currentEditor.FocusInternal();
			}
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x00188A04 File Offset: 0x00187A04
		internal void RemoveSelectedEntryHelpAttributes()
		{
			this.UpdateHelpAttributes(this.selectedGridEntry, null);
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x00188A14 File Offset: 0x00187A14
		private void UpdateHelpAttributes(GridEntry oldEntry, GridEntry newEntry)
		{
			IHelpService helpService = this.GetHelpService();
			if (helpService == null || oldEntry == newEntry)
			{
				return;
			}
			GridEntry gridEntry = oldEntry;
			if (oldEntry != null && !oldEntry.Disposed)
			{
				while (gridEntry != null)
				{
					helpService.RemoveContextAttribute("Keyword", gridEntry.HelpKeyword);
					gridEntry = gridEntry.ParentGridEntry;
				}
			}
			if (newEntry != null)
			{
				this.UpdateHelpAttributes(helpService, newEntry, true);
			}
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x00188A68 File Offset: 0x00187A68
		private void UpdateHelpAttributes(IHelpService helpSvc, GridEntry entry, bool addAsF1)
		{
			if (entry == null)
			{
				return;
			}
			this.UpdateHelpAttributes(helpSvc, entry.ParentGridEntry, false);
			string helpKeyword = entry.HelpKeyword;
			if (helpKeyword != null)
			{
				helpSvc.AddContextAttribute("Keyword", helpKeyword, addAsF1 ? HelpKeywordType.F1Keyword : HelpKeywordType.GeneralKeyword);
			}
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x00188AA4 File Offset: 0x00187AA4
		private void UpdateUIBasedOnFont(bool layoutRequired)
		{
			if (base.IsHandleCreated && this.GetFlag(128))
			{
				try
				{
					if (this.listBox != null)
					{
						this.DropDownListBox.ItemHeight = this.RowHeight + 2;
					}
					if (this.btnDropDown != null)
					{
						this.btnDropDown.Size = new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight);
						if (this.btnDialog != null)
						{
							this.DialogButton.Size = this.DropDownButton.Size;
						}
					}
					if (layoutRequired)
					{
						this.LayoutWindow(true);
					}
				}
				finally
				{
					this.SetFlag(128, false);
				}
			}
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x00188B50 File Offset: 0x00187B50
		private bool UnfocusSelection()
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return true;
			}
			bool flag = this.Commit();
			if (flag && this.FocusInside)
			{
				this.FocusInternal();
			}
			return flag;
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x00188B8C File Offset: 0x00187B8C
		private void UpdateResetCommand(GridEntry gridEntry)
		{
			if (this.totalProps > 0)
			{
				IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
				if (menuCommandService != null)
				{
					MenuCommand menuCommand = menuCommandService.FindCommand(PropertyGridCommands.Reset);
					if (menuCommand != null)
					{
						menuCommand.Enabled = (gridEntry != null && gridEntry.CanResetPropertyValue());
					}
				}
			}
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x00188BDC File Offset: 0x00187BDC
		internal bool WantsTab(bool forward)
		{
			if (forward)
			{
				if (this.Focused)
				{
					if (this.DropDownButton.Visible || this.DialogButton.Visible || this.Edit.Visible)
					{
						return true;
					}
				}
				else if (this.Edit.Focused && (this.DropDownButton.Visible || this.DialogButton.Visible))
				{
					return true;
				}
				return this.ownerGrid.WantsTab(forward);
			}
			return this.Edit.Focused || this.DropDownButton.Focused || this.DialogButton.Focused || this.ownerGrid.WantsTab(forward);
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x00188C88 File Offset: 0x00187C88
		private unsafe bool WmNotify(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
				if (ptr->hwndFrom == this.ToolTip.Handle)
				{
					switch (ptr->code)
					{
					case -521:
					{
						Point point = Cursor.Position;
						point = base.PointToClientInternal(point);
						point = this.FindPosition(point.X, point.Y);
						if (!(point == PropertyGridView.InvalidPosition))
						{
							GridEntry gridEntryFromRow = this.GetGridEntryFromRow(point.Y);
							if (gridEntryFromRow != null)
							{
								Rectangle rectangle = this.GetRectangle(point.Y, point.X);
								Point point2 = Point.Empty;
								if (point.X == 1)
								{
									point2 = gridEntryFromRow.GetLabelToolTipLocation(point.X - rectangle.X, point.Y - rectangle.Y);
								}
								else
								{
									if (point.X != 2)
									{
										break;
									}
									point2 = gridEntryFromRow.ValueToolTipLocation;
								}
								if (point2 != PropertyGridView.InvalidPoint)
								{
									rectangle.Offset(point2);
									PropertyGridView.PositionTooltip(this, this.ToolTip, rectangle);
									m.Result = (IntPtr)1;
									return true;
								}
							}
						}
						break;
					}
					}
				}
			}
			return false;
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x00188DD0 File Offset: 0x00187DD0
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 78)
			{
				if (msg != 7)
				{
					if (msg != 21)
					{
						if (msg == 78)
						{
							if (this.WmNotify(ref m))
							{
								return;
							}
						}
					}
					else
					{
						base.Invalidate();
					}
				}
				else if (!this.GetInPropertySet() && this.Edit.Visible && (this.errorState != 0 || !this.Commit()))
				{
					base.WndProc(ref m);
					this.Edit.FocusInternal();
					return;
				}
			}
			else if (msg <= 271)
			{
				if (msg == 135)
				{
					int num = 129;
					if (this.selectedGridEntry != null && (Control.ModifierKeys & Keys.Shift) == Keys.None && this.edit.Visible)
					{
						num |= 2;
					}
					m.Result = (IntPtr)num;
					return;
				}
				switch (msg)
				{
				case 269:
					this.Edit.FocusInternal();
					this.Edit.Clear();
					UnsafeNativeMethods.PostMessage(new HandleRef(this.Edit, this.Edit.Handle), 269, 0, 0);
					return;
				case 271:
					this.Edit.FocusInternal();
					UnsafeNativeMethods.PostMessage(new HandleRef(this.Edit, this.Edit.Handle), 271, m.WParam, m.LParam);
					return;
				}
			}
			else if (msg != 512)
			{
				switch (msg)
				{
				case 1110:
					m.Result = (IntPtr)Math.Min(this.visibleRows, this.totalProps);
					return;
				case 1111:
					m.Result = (IntPtr)this.GetRowFromGridEntry(this.selectedGridEntry);
					return;
				}
			}
			else
			{
				if ((int)m.LParam == this.lastMouseMove)
				{
					return;
				}
				this.lastMouseMove = (int)m.LParam;
			}
			base.WndProc(ref m);
		}

		// Token: 0x04003DBE RID: 15806
		public const int RENDERMODE_LEFTDOT = 2;

		// Token: 0x04003DBF RID: 15807
		public const int RENDERMODE_BOLD = 3;

		// Token: 0x04003DC0 RID: 15808
		public const int RENDERMODE_TRIANGLE = 4;

		// Token: 0x04003DC1 RID: 15809
		private const int LEFTDOT_SIZE = 4;

		// Token: 0x04003DC2 RID: 15810
		protected const int EDIT_INDENT = 0;

		// Token: 0x04003DC3 RID: 15811
		protected const int OUTLINE_INDENT = 10;

		// Token: 0x04003DC4 RID: 15812
		protected const int OUTLINE_SIZE = 9;

		// Token: 0x04003DC5 RID: 15813
		protected const int PAINT_WIDTH = 20;

		// Token: 0x04003DC6 RID: 15814
		protected const int PAINT_INDENT = 26;

		// Token: 0x04003DC7 RID: 15815
		protected const int ROWLABEL = 1;

		// Token: 0x04003DC8 RID: 15816
		protected const int ROWVALUE = 2;

		// Token: 0x04003DC9 RID: 15817
		protected const int MAX_LISTBOX_HEIGHT = 200;

		// Token: 0x04003DCA RID: 15818
		protected const short ERROR_NONE = 0;

		// Token: 0x04003DCB RID: 15819
		protected const short ERROR_THROWN = 1;

		// Token: 0x04003DCC RID: 15820
		protected const short ERROR_MSGBOX_UP = 2;

		// Token: 0x04003DCD RID: 15821
		internal const short GDIPLUS_SPACE = 2;

		// Token: 0x04003DCE RID: 15822
		internal const int MaxRecurseExpand = 10;

		// Token: 0x04003DCF RID: 15823
		private const short FlagNeedsRefresh = 1;

		// Token: 0x04003DD0 RID: 15824
		private const short FlagIsNewSelection = 2;

		// Token: 0x04003DD1 RID: 15825
		private const short FlagIsSplitterMove = 4;

		// Token: 0x04003DD2 RID: 15826
		private const short FlagIsSpecialKey = 8;

		// Token: 0x04003DD3 RID: 15827
		private const short FlagInPropertySet = 16;

		// Token: 0x04003DD4 RID: 15828
		private const short FlagDropDownClosing = 32;

		// Token: 0x04003DD5 RID: 15829
		private const short FlagDropDownCommit = 64;

		// Token: 0x04003DD6 RID: 15830
		private const short FlagNeedUpdateUIBasedOnFont = 128;

		// Token: 0x04003DD7 RID: 15831
		private const short FlagBtnLaunchedEditor = 256;

		// Token: 0x04003DD8 RID: 15832
		private const short FlagNoDefault = 512;

		// Token: 0x04003DD9 RID: 15833
		private const short FlagResizableDropDown = 1024;

		// Token: 0x04003DDA RID: 15834
		protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		// Token: 0x04003DDB RID: 15835
		public static int inheritRenderMode = 3;

		// Token: 0x04003DDC RID: 15836
		public static TraceSwitch GridViewDebugPaint = new TraceSwitch("GridViewDebugPaint", "PropertyGridView: Debug property painting");

		// Token: 0x04003DDD RID: 15837
		private PropertyGrid ownerGrid;

		// Token: 0x04003DDE RID: 15838
		protected static readonly Point InvalidPosition = new Point(int.MinValue, int.MinValue);

		// Token: 0x04003DDF RID: 15839
		private Brush backgroundBrush;

		// Token: 0x04003DE0 RID: 15840
		private Font fontBold;

		// Token: 0x04003DE1 RID: 15841
		private GridEntryCollection topLevelGridEntries;

		// Token: 0x04003DE2 RID: 15842
		private GridEntryCollection allGridEntries;

		// Token: 0x04003DE3 RID: 15843
		internal int totalProps = -1;

		// Token: 0x04003DE4 RID: 15844
		private int visibleRows = -1;

		// Token: 0x04003DE5 RID: 15845
		private int labelWidth = -1;

		// Token: 0x04003DE6 RID: 15846
		public double labelRatio = 2.0;

		// Token: 0x04003DE7 RID: 15847
		private short requiredLabelPaintMargin = 2;

		// Token: 0x04003DE8 RID: 15848
		private int selectedRow = -1;

		// Token: 0x04003DE9 RID: 15849
		private GridEntry selectedGridEntry;

		// Token: 0x04003DEA RID: 15850
		private int tipInfo = -1;

		// Token: 0x04003DEB RID: 15851
		private PropertyGridView.GridViewEdit edit;

		// Token: 0x04003DEC RID: 15852
		private DropDownButton btnDropDown;

		// Token: 0x04003DED RID: 15853
		private DropDownButton btnDialog;

		// Token: 0x04003DEE RID: 15854
		private PropertyGridView.GridViewListBox listBox;

		// Token: 0x04003DEF RID: 15855
		private PropertyGridView.DropDownHolder dropDownHolder;

		// Token: 0x04003DF0 RID: 15856
		private Rectangle lastClientRect = Rectangle.Empty;

		// Token: 0x04003DF1 RID: 15857
		private Control currentEditor;

		// Token: 0x04003DF2 RID: 15858
		private ScrollBar scrollBar;

		// Token: 0x04003DF3 RID: 15859
		internal GridToolTip toolTip;

		// Token: 0x04003DF4 RID: 15860
		private GridErrorDlg errorDlg;

		// Token: 0x04003DF5 RID: 15861
		private short flags = 131;

		// Token: 0x04003DF6 RID: 15862
		private short errorState;

		// Token: 0x04003DF7 RID: 15863
		private Point ptOurLocation = new Point(1, 1);

		// Token: 0x04003DF8 RID: 15864
		private string originalTextValue;

		// Token: 0x04003DF9 RID: 15865
		private int cumulativeVerticalWheelDelta;

		// Token: 0x04003DFA RID: 15866
		private long rowSelectTime;

		// Token: 0x04003DFB RID: 15867
		private Point rowSelectPos = Point.Empty;

		// Token: 0x04003DFC RID: 15868
		private Point lastMouseDown = PropertyGridView.InvalidPosition;

		// Token: 0x04003DFD RID: 15869
		private int lastMouseMove;

		// Token: 0x04003DFE RID: 15870
		private GridEntry lastClickedEntry;

		// Token: 0x04003DFF RID: 15871
		private IServiceProvider serviceProvider;

		// Token: 0x04003E00 RID: 15872
		private IHelpService topHelpService;

		// Token: 0x04003E01 RID: 15873
		private IHelpService helpService;

		// Token: 0x04003E02 RID: 15874
		private EventHandler ehValueClick;

		// Token: 0x04003E03 RID: 15875
		private EventHandler ehLabelClick;

		// Token: 0x04003E04 RID: 15876
		private EventHandler ehOutlineClick;

		// Token: 0x04003E05 RID: 15877
		private EventHandler ehValueDblClick;

		// Token: 0x04003E06 RID: 15878
		private EventHandler ehLabelDblClick;

		// Token: 0x04003E07 RID: 15879
		private GridEntryRecreateChildrenEventHandler ehRecreateChildren;

		// Token: 0x04003E08 RID: 15880
		private int cachedRowHeight = -1;

		// Token: 0x04003E09 RID: 15881
		private IntPtr baseHfont;

		// Token: 0x04003E0A RID: 15882
		private IntPtr boldHfont;

		// Token: 0x04003E0B RID: 15883
		private PropertyGridView.GridPositionData positionData;

		// Token: 0x020007C8 RID: 1992
		internal interface IMouseHookClient
		{
			// Token: 0x060069EE RID: 27118
			bool OnClickHooked();
		}

		// Token: 0x020007C9 RID: 1993
		private class DropDownHolder : Form, PropertyGridView.IMouseHookClient
		{
			// Token: 0x060069EF RID: 27119 RVA: 0x0018900B File Offset: 0x0018800B
			static DropDownHolder()
			{
				PropertyGridView.DropDownHolder.ResizeGripSize = SystemInformation.HorizontalScrollBarHeight;
				PropertyGridView.DropDownHolder.ResizeBarSize = PropertyGridView.DropDownHolder.ResizeGripSize + 1;
				PropertyGridView.DropDownHolder.ResizeBorderSize = PropertyGridView.DropDownHolder.ResizeBarSize / 2;
			}

			// Token: 0x060069F0 RID: 27120 RVA: 0x00189048 File Offset: 0x00188048
			internal DropDownHolder(PropertyGridView psheet)
			{
				base.ShowInTaskbar = false;
				base.ControlBox = false;
				base.MinimizeBox = false;
				base.MaximizeBox = false;
				this.Text = "";
				base.FormBorderStyle = FormBorderStyle.None;
				base.AutoScaleMode = AutoScaleMode.None;
				this.mouseHook = new PropertyGridView.MouseHook(this, this, psheet);
				base.Visible = false;
				this.gridView = psheet;
				this.BackColor = this.gridView.BackColor;
			}

			// Token: 0x17001682 RID: 5762
			// (get) Token: 0x060069F1 RID: 27121 RVA: 0x001890DC File Offset: 0x001880DC
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= 128;
					createParams.Style |= -2139095040;
					if (OSFeature.IsPresent(SystemParameter.DropShadow))
					{
						createParams.ClassStyle |= 131072;
					}
					if (this.gridView != null)
					{
						createParams.Parent = this.gridView.ParentInternal.Handle;
					}
					return createParams;
				}
			}

			// Token: 0x17001683 RID: 5763
			// (get) Token: 0x060069F2 RID: 27122 RVA: 0x0018914D File Offset: 0x0018814D
			private LinkLabel CreateNewLink
			{
				get
				{
					if (this.createNewLink == null)
					{
						this.createNewLink = new LinkLabel();
						this.createNewLink.LinkClicked += this.OnNewLinkClicked;
					}
					return this.createNewLink;
				}
			}

			// Token: 0x17001684 RID: 5764
			// (get) Token: 0x060069F3 RID: 27123 RVA: 0x0018917F File Offset: 0x0018817F
			// (set) Token: 0x060069F4 RID: 27124 RVA: 0x0018918C File Offset: 0x0018818C
			public virtual bool HookMouseDown
			{
				get
				{
					return this.mouseHook.HookMouseDown;
				}
				set
				{
					this.mouseHook.HookMouseDown = value;
				}
			}

			// Token: 0x17001685 RID: 5765
			// (set) Token: 0x060069F5 RID: 27125 RVA: 0x0018919C File Offset: 0x0018819C
			public bool ResizeUp
			{
				set
				{
					if (this.resizeUp != value)
					{
						this.sizeGripGlyph = null;
						this.resizeUp = value;
						if (this.resizable)
						{
							base.DockPadding.Bottom = 0;
							base.DockPadding.Top = 0;
							if (value)
							{
								base.DockPadding.Top = PropertyGridView.DropDownHolder.ResizeBarSize;
								return;
							}
							base.DockPadding.Bottom = PropertyGridView.DropDownHolder.ResizeBarSize;
						}
					}
				}
			}

			// Token: 0x060069F6 RID: 27126 RVA: 0x00189204 File Offset: 0x00188204
			protected override void DestroyHandle()
			{
				this.mouseHook.HookMouseDown = false;
				base.DestroyHandle();
			}

			// Token: 0x060069F7 RID: 27127 RVA: 0x00189218 File Offset: 0x00188218
			protected override void Dispose(bool disposing)
			{
				if (disposing && this.createNewLink != null)
				{
					this.createNewLink.Dispose();
					this.createNewLink = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x060069F8 RID: 27128 RVA: 0x0018923E File Offset: 0x0018823E
			public void DoModalLoop()
			{
				while (base.Visible)
				{
					Application.DoEventsModal();
					UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
				}
			}

			// Token: 0x17001686 RID: 5766
			// (get) Token: 0x060069F9 RID: 27129 RVA: 0x00189266 File Offset: 0x00188266
			public virtual Control Component
			{
				get
				{
					return this.currentControl;
				}
			}

			// Token: 0x060069FA RID: 27130 RVA: 0x00189270 File Offset: 0x00188270
			private InstanceCreationEditor GetInstanceCreationEditor(PropertyDescriptorGridEntry entry)
			{
				if (entry == null)
				{
					return null;
				}
				InstanceCreationEditor instanceCreationEditor = null;
				PropertyDescriptor propertyDescriptor = entry.PropertyDescriptor;
				if (propertyDescriptor != null)
				{
					instanceCreationEditor = (propertyDescriptor.GetEditor(typeof(InstanceCreationEditor)) as InstanceCreationEditor);
				}
				if (instanceCreationEditor == null)
				{
					UITypeEditor uitypeEditor = entry.UITypeEditor;
					if (uitypeEditor != null && uitypeEditor.GetEditStyle() == UITypeEditorEditStyle.DropDown)
					{
						instanceCreationEditor = (InstanceCreationEditor)TypeDescriptor.GetEditor(uitypeEditor, typeof(InstanceCreationEditor));
					}
				}
				return instanceCreationEditor;
			}

			// Token: 0x060069FB RID: 27131 RVA: 0x001892D4 File Offset: 0x001882D4
			private Bitmap GetSizeGripGlyph(Graphics g)
			{
				if (this.sizeGripGlyph != null)
				{
					return this.sizeGripGlyph;
				}
				this.sizeGripGlyph = new Bitmap(PropertyGridView.DropDownHolder.ResizeGripSize, PropertyGridView.DropDownHolder.ResizeGripSize, g);
				using (Graphics graphics = Graphics.FromImage(this.sizeGripGlyph))
				{
					Matrix matrix = new Matrix();
					matrix.Translate((float)(PropertyGridView.DropDownHolder.ResizeGripSize + 1), (float)(this.resizeUp ? (PropertyGridView.DropDownHolder.ResizeGripSize + 1) : 0));
					matrix.Scale(-1f, (float)(this.resizeUp ? -1 : 1));
					graphics.Transform = matrix;
					ControlPaint.DrawSizeGrip(graphics, this.BackColor, 0, 0, PropertyGridView.DropDownHolder.ResizeGripSize, PropertyGridView.DropDownHolder.ResizeGripSize);
					graphics.ResetTransform();
				}
				this.sizeGripGlyph.MakeTransparent(this.BackColor);
				return this.sizeGripGlyph;
			}

			// Token: 0x060069FC RID: 27132 RVA: 0x001893AC File Offset: 0x001883AC
			public virtual bool GetUsed()
			{
				return this.currentControl != null;
			}

			// Token: 0x060069FD RID: 27133 RVA: 0x001893BA File Offset: 0x001883BA
			public virtual void FocusComponent()
			{
				if (this.currentControl != null && base.Visible)
				{
					this.currentControl.FocusInternal();
				}
			}

			// Token: 0x060069FE RID: 27134 RVA: 0x001893D8 File Offset: 0x001883D8
			private bool OwnsWindow(IntPtr hWnd)
			{
				while (hWnd != IntPtr.Zero)
				{
					hWnd = UnsafeNativeMethods.GetWindowLong(new HandleRef(null, hWnd), -8);
					if (hWnd == IntPtr.Zero)
					{
						return false;
					}
					if (hWnd == base.Handle)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060069FF RID: 27135 RVA: 0x00189424 File Offset: 0x00188424
			public bool OnClickHooked()
			{
				this.gridView.CloseDropDownInternal(false);
				return false;
			}

			// Token: 0x06006A00 RID: 27136 RVA: 0x00189434 File Offset: 0x00188434
			private void OnCurrentControlResize(object o, EventArgs e)
			{
				if (this.currentControl != null && !this.resizing)
				{
					int width = base.Width;
					Size size = new Size(2 + this.currentControl.Width, 2 + this.currentControl.Height);
					if (this.resizable)
					{
						size.Height += PropertyGridView.DropDownHolder.ResizeBarSize;
					}
					try
					{
						this.resizing = true;
						base.SuspendLayout();
						base.Size = size;
					}
					finally
					{
						this.resizing = false;
						base.ResumeLayout(false);
					}
					base.Left -= base.Width - width;
				}
			}

			// Token: 0x06006A01 RID: 27137 RVA: 0x001894E4 File Offset: 0x001884E4
			protected override void OnLayout(LayoutEventArgs levent)
			{
				try
				{
					this.resizing = true;
					base.OnLayout(levent);
				}
				finally
				{
					this.resizing = false;
				}
			}

			// Token: 0x06006A02 RID: 27138 RVA: 0x0018951C File Offset: 0x0018851C
			private void OnNewLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				InstanceCreationEditor instanceCreationEditor = e.Link.LinkData as InstanceCreationEditor;
				if (instanceCreationEditor != null)
				{
					Type propertyType = this.gridView.SelectedGridEntry.PropertyType;
					if (propertyType != null)
					{
						this.gridView.CloseDropDown();
						object obj = instanceCreationEditor.CreateInstance(this.gridView.SelectedGridEntry, propertyType);
						if (obj != null)
						{
							if (!propertyType.IsInstanceOfType(obj))
							{
								throw new InvalidCastException(SR.GetString("PropertyGridViewEditorCreatedInvalidObject", new object[]
								{
									propertyType
								}));
							}
							this.gridView.CommitValue(obj);
						}
					}
				}
			}

			// Token: 0x06006A03 RID: 27139 RVA: 0x001895A4 File Offset: 0x001885A4
			private int MoveTypeFromPoint(int x, int y)
			{
				Rectangle rectangle = new Rectangle(0, base.Height - PropertyGridView.DropDownHolder.ResizeGripSize, PropertyGridView.DropDownHolder.ResizeGripSize, PropertyGridView.DropDownHolder.ResizeGripSize);
				Rectangle rectangle2 = new Rectangle(0, 0, PropertyGridView.DropDownHolder.ResizeGripSize, PropertyGridView.DropDownHolder.ResizeGripSize);
				if (!this.resizeUp && rectangle.Contains(x, y))
				{
					return 3;
				}
				if (this.resizeUp && rectangle2.Contains(x, y))
				{
					return 6;
				}
				if (!this.resizeUp && Math.Abs(base.Height - y) < PropertyGridView.DropDownHolder.ResizeBorderSize)
				{
					return 1;
				}
				if (this.resizeUp && Math.Abs(y) < PropertyGridView.DropDownHolder.ResizeBorderSize)
				{
					return 4;
				}
				return 0;
			}

			// Token: 0x06006A04 RID: 27140 RVA: 0x00189644 File Offset: 0x00188644
			protected override void OnMouseDown(MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.currentMoveType = this.MoveTypeFromPoint(e.X, e.Y);
					if (this.currentMoveType != 0)
					{
						this.dragStart = base.PointToScreen(new Point(e.X, e.Y));
						this.dragBaseRect = base.Bounds;
						base.Capture = true;
					}
					else
					{
						this.gridView.CloseDropDown();
					}
				}
				base.OnMouseDown(e);
			}

			// Token: 0x06006A05 RID: 27141 RVA: 0x001896C4 File Offset: 0x001886C4
			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (this.currentMoveType == 0)
				{
					switch (this.MoveTypeFromPoint(e.X, e.Y))
					{
					case 1:
					case 4:
						this.Cursor = Cursors.SizeNS;
						goto IL_1D8;
					case 3:
						this.Cursor = Cursors.SizeNESW;
						goto IL_1D8;
					case 6:
						this.Cursor = Cursors.SizeNWSE;
						goto IL_1D8;
					}
					this.Cursor = null;
				}
				else
				{
					Point point = base.PointToScreen(new Point(e.X, e.Y));
					Rectangle bounds = base.Bounds;
					if ((this.currentMoveType & 1) == 1)
					{
						bounds.Height = Math.Max(PropertyGridView.DropDownHolder.MinDropDownSize.Height, this.dragBaseRect.Height + (point.Y - this.dragStart.Y));
					}
					if ((this.currentMoveType & 4) == 4)
					{
						int num = point.Y - this.dragStart.Y;
						if (this.dragBaseRect.Height - num > PropertyGridView.DropDownHolder.MinDropDownSize.Height)
						{
							bounds.Y = this.dragBaseRect.Top + num;
							bounds.Height = this.dragBaseRect.Height - num;
						}
					}
					if ((this.currentMoveType & 2) == 2)
					{
						int num2 = point.X - this.dragStart.X;
						if (this.dragBaseRect.Width - num2 > PropertyGridView.DropDownHolder.MinDropDownSize.Width)
						{
							bounds.X = this.dragBaseRect.Left + num2;
							bounds.Width = this.dragBaseRect.Width - num2;
						}
					}
					if (bounds != base.Bounds)
					{
						try
						{
							this.resizing = true;
							base.Bounds = bounds;
						}
						finally
						{
							this.resizing = false;
						}
					}
					base.Invalidate();
				}
				IL_1D8:
				base.OnMouseMove(e);
			}

			// Token: 0x06006A06 RID: 27142 RVA: 0x001898C0 File Offset: 0x001888C0
			protected override void OnMouseLeave(EventArgs e)
			{
				this.Cursor = null;
				base.OnMouseLeave(e);
			}

			// Token: 0x06006A07 RID: 27143 RVA: 0x001898D0 File Offset: 0x001888D0
			protected override void OnMouseUp(MouseEventArgs e)
			{
				base.OnMouseUp(e);
				if (e.Button == MouseButtons.Left)
				{
					this.currentMoveType = 0;
					this.dragStart = Point.Empty;
					this.dragBaseRect = Rectangle.Empty;
					base.Capture = false;
				}
			}

			// Token: 0x06006A08 RID: 27144 RVA: 0x0018990C File Offset: 0x0018890C
			protected override void OnPaint(PaintEventArgs pe)
			{
				base.OnPaint(pe);
				if (this.resizable)
				{
					Rectangle rect = new Rectangle(0, this.resizeUp ? 0 : (base.Height - PropertyGridView.DropDownHolder.ResizeGripSize), PropertyGridView.DropDownHolder.ResizeGripSize, PropertyGridView.DropDownHolder.ResizeGripSize);
					pe.Graphics.DrawImage(this.GetSizeGripGlyph(pe.Graphics), rect);
					int num = this.resizeUp ? (PropertyGridView.DropDownHolder.ResizeBarSize - 1) : (base.Height - PropertyGridView.DropDownHolder.ResizeBarSize);
					Pen pen = new Pen(SystemColors.ControlDark, 1f);
					pen.DashStyle = DashStyle.Solid;
					pe.Graphics.DrawLine(pen, 0, num, base.Width, num);
					pen.Dispose();
				}
			}

			// Token: 0x06006A09 RID: 27145 RVA: 0x001899BC File Offset: 0x001889BC
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.None)
				{
					Keys keys = keyData & Keys.KeyCode;
					if (keys == Keys.Return)
					{
						if (this.gridView.UnfocusSelection() && this.gridView.SelectedGridEntry != null)
						{
							this.gridView.SelectedGridEntry.OnValueReturnKey();
						}
						return true;
					}
					if (keys == Keys.Escape)
					{
						this.gridView.OnEscape(this);
						return true;
					}
					if (keys == Keys.F4)
					{
						this.gridView.F4Selection(true);
						return true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06006A0A RID: 27146 RVA: 0x00189A3C File Offset: 0x00188A3C
			public void SetComponent(Control ctl, bool resizable)
			{
				this.resizable = resizable;
				this.Font = this.gridView.Font;
				InstanceCreationEditor instanceCreationEditor = (ctl == null) ? null : this.GetInstanceCreationEditor(this.gridView.SelectedGridEntry as PropertyDescriptorGridEntry);
				if (this.currentControl != null)
				{
					this.currentControl.Resize -= this.OnCurrentControlResize;
					base.Controls.Remove(this.currentControl);
					this.currentControl = null;
				}
				if (this.createNewLink != null && this.createNewLink.Parent == this)
				{
					base.Controls.Remove(this.createNewLink);
				}
				if (ctl != null)
				{
					this.currentControl = ctl;
					base.DockPadding.All = 0;
					if (this.currentControl is PropertyGridView.GridViewListBox)
					{
						ListBox listBox = (ListBox)this.currentControl;
						if (listBox.Items.Count == 0)
						{
							listBox.Height = Math.Max(listBox.Height, listBox.ItemHeight);
						}
					}
					try
					{
						base.SuspendLayout();
						base.Controls.Add(ctl);
						Size size = new Size(2 + ctl.Width, 2 + ctl.Height);
						if (instanceCreationEditor != null)
						{
							this.CreateNewLink.Text = instanceCreationEditor.Text;
							this.CreateNewLink.Links.Clear();
							this.CreateNewLink.Links.Add(0, instanceCreationEditor.Text.Length, instanceCreationEditor);
							int num = this.CreateNewLink.Height;
							using (Graphics graphics = this.gridView.CreateGraphics())
							{
								num = (int)PropertyGrid.MeasureTextHelper.MeasureText(this.gridView.ownerGrid, graphics, instanceCreationEditor.Text, this.gridView.GetBaseFont()).Height;
							}
							this.CreateNewLink.Height = num + 1;
							size.Height += num + 2;
						}
						if (resizable)
						{
							size.Height += PropertyGridView.DropDownHolder.ResizeBarSize;
							if (this.resizeUp)
							{
								base.DockPadding.Top = PropertyGridView.DropDownHolder.ResizeBarSize;
							}
							else
							{
								base.DockPadding.Bottom = PropertyGridView.DropDownHolder.ResizeBarSize;
							}
						}
						base.Size = size;
						ctl.Dock = DockStyle.Fill;
						ctl.Visible = true;
						if (instanceCreationEditor != null)
						{
							this.CreateNewLink.Dock = DockStyle.Bottom;
							base.Controls.Add(this.CreateNewLink);
						}
					}
					finally
					{
						base.ResumeLayout(true);
					}
					this.currentControl.Resize += this.OnCurrentControlResize;
				}
				base.Enabled = (this.currentControl != null);
			}

			// Token: 0x06006A0B RID: 27147 RVA: 0x00189CF0 File Offset: 0x00188CF0
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 6)
				{
					base.SetState(32, true);
					IntPtr lparam = m.LParam;
					if (base.Visible && NativeMethods.Util.LOWORD(m.WParam) == 0 && !this.OwnsWindow(lparam))
					{
						this.gridView.CloseDropDownInternal(false);
						return;
					}
				}
				else if (m.Msg == 16)
				{
					if (base.Visible)
					{
						this.gridView.CloseDropDown();
					}
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x04003E0C RID: 15884
			private const int DropDownHolderBorder = 1;

			// Token: 0x04003E0D RID: 15885
			private const int MoveTypeNone = 0;

			// Token: 0x04003E0E RID: 15886
			private const int MoveTypeBottom = 1;

			// Token: 0x04003E0F RID: 15887
			private const int MoveTypeLeft = 2;

			// Token: 0x04003E10 RID: 15888
			private const int MoveTypeTop = 4;

			// Token: 0x04003E11 RID: 15889
			private Control currentControl;

			// Token: 0x04003E12 RID: 15890
			private PropertyGridView gridView;

			// Token: 0x04003E13 RID: 15891
			private PropertyGridView.MouseHook mouseHook;

			// Token: 0x04003E14 RID: 15892
			private LinkLabel createNewLink;

			// Token: 0x04003E15 RID: 15893
			private bool resizable = true;

			// Token: 0x04003E16 RID: 15894
			private bool resizing;

			// Token: 0x04003E17 RID: 15895
			private bool resizeUp;

			// Token: 0x04003E18 RID: 15896
			private Point dragStart = Point.Empty;

			// Token: 0x04003E19 RID: 15897
			private Rectangle dragBaseRect = Rectangle.Empty;

			// Token: 0x04003E1A RID: 15898
			private int currentMoveType;

			// Token: 0x04003E1B RID: 15899
			private static readonly int ResizeBarSize;

			// Token: 0x04003E1C RID: 15900
			private static readonly int ResizeBorderSize;

			// Token: 0x04003E1D RID: 15901
			private static readonly int ResizeGripSize;

			// Token: 0x04003E1E RID: 15902
			private static readonly Size MinDropDownSize = new Size(SystemInformation.VerticalScrollBarWidth * 4, SystemInformation.HorizontalScrollBarHeight * 4);

			// Token: 0x04003E1F RID: 15903
			private Bitmap sizeGripGlyph;
		}

		// Token: 0x020007CA RID: 1994
		private class GridViewListBox : ListBox
		{
			// Token: 0x06006A0C RID: 27148 RVA: 0x00189D66 File Offset: 0x00188D66
			public GridViewListBox(PropertyGridView gridView)
			{
				base.IntegralHeight = false;
			}

			// Token: 0x17001687 RID: 5767
			// (get) Token: 0x06006A0D RID: 27149 RVA: 0x00189D78 File Offset: 0x00188D78
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.Style &= -8388609;
					createParams.ExStyle &= -513;
					return createParams;
				}
			}

			// Token: 0x06006A0E RID: 27150 RVA: 0x00189DB1 File Offset: 0x00188DB1
			public virtual bool InSetSelectedIndex()
			{
				return this.fInSetSelectedIndex;
			}

			// Token: 0x06006A0F RID: 27151 RVA: 0x00189DB9 File Offset: 0x00188DB9
			protected override void OnSelectedIndexChanged(EventArgs e)
			{
				this.fInSetSelectedIndex = true;
				base.OnSelectedIndexChanged(e);
				this.fInSetSelectedIndex = false;
			}

			// Token: 0x04003E20 RID: 15904
			internal bool fInSetSelectedIndex;
		}

		// Token: 0x020007CB RID: 1995
		private class GridViewEdit : TextBox, PropertyGridView.IMouseHookClient
		{
			// Token: 0x17001688 RID: 5768
			// (set) Token: 0x06006A10 RID: 27152 RVA: 0x00189DD0 File Offset: 0x00188DD0
			public bool DontFocus
			{
				set
				{
					this.dontFocusMe = value;
				}
			}

			// Token: 0x17001689 RID: 5769
			// (get) Token: 0x06006A11 RID: 27153 RVA: 0x00189DD9 File Offset: 0x00188DD9
			// (set) Token: 0x06006A12 RID: 27154 RVA: 0x00189DE1 File Offset: 0x00188DE1
			public virtual bool Filter
			{
				get
				{
					return this.filter;
				}
				set
				{
					this.filter = value;
				}
			}

			// Token: 0x1700168A RID: 5770
			// (get) Token: 0x06006A13 RID: 27155 RVA: 0x00189DEA File Offset: 0x00188DEA
			public override bool Focused
			{
				get
				{
					return !this.dontFocusMe && base.Focused;
				}
			}

			// Token: 0x1700168B RID: 5771
			// (get) Token: 0x06006A14 RID: 27156 RVA: 0x00189DFC File Offset: 0x00188DFC
			// (set) Token: 0x06006A15 RID: 27157 RVA: 0x00189E04 File Offset: 0x00188E04
			public override string Text
			{
				get
				{
					return base.Text;
				}
				set
				{
					this.fInSetText = true;
					base.Text = value;
					this.fInSetText = false;
				}
			}

			// Token: 0x1700168C RID: 5772
			// (set) Token: 0x06006A16 RID: 27158 RVA: 0x00189E1B File Offset: 0x00188E1B
			public bool DisableMouseHook
			{
				set
				{
					this.mouseHook.DisableMouseHook = value;
				}
			}

			// Token: 0x1700168D RID: 5773
			// (get) Token: 0x06006A17 RID: 27159 RVA: 0x00189E29 File Offset: 0x00188E29
			// (set) Token: 0x06006A18 RID: 27160 RVA: 0x00189E36 File Offset: 0x00188E36
			public virtual bool HookMouseDown
			{
				get
				{
					return this.mouseHook.HookMouseDown;
				}
				set
				{
					this.mouseHook.HookMouseDown = value;
					if (value)
					{
						this.FocusInternal();
					}
				}
			}

			// Token: 0x06006A19 RID: 27161 RVA: 0x00189E4E File Offset: 0x00188E4E
			public GridViewEdit(PropertyGridView psheet)
			{
				this.psheet = psheet;
				this.mouseHook = new PropertyGridView.MouseHook(this, this, psheet);
			}

			// Token: 0x06006A1A RID: 27162 RVA: 0x00189E6B File Offset: 0x00188E6B
			protected override void DestroyHandle()
			{
				this.mouseHook.HookMouseDown = false;
				base.DestroyHandle();
			}

			// Token: 0x06006A1B RID: 27163 RVA: 0x00189E7F File Offset: 0x00188E7F
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.mouseHook.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06006A1C RID: 27164 RVA: 0x00189E96 File Offset: 0x00188E96
			public void FilterKeyPress(char keyChar)
			{
				if (this.IsInputChar(keyChar))
				{
					this.FocusInternal();
					base.SelectAll();
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 258, (IntPtr)((int)keyChar), IntPtr.Zero);
				}
			}

			// Token: 0x06006A1D RID: 27165 RVA: 0x00189ED0 File Offset: 0x00188ED0
			protected override bool IsInputKey(Keys keyData)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys <= Keys.Return)
				{
					if (keys != Keys.Tab && keys != Keys.Return)
					{
						goto IL_2A;
					}
				}
				else if (keys != Keys.Escape && keys != Keys.F1 && keys != Keys.F4)
				{
					goto IL_2A;
				}
				return false;
				IL_2A:
				return !this.psheet.NeedsCommit && base.IsInputKey(keyData);
			}

			// Token: 0x06006A1E RID: 27166 RVA: 0x00189F20 File Offset: 0x00188F20
			protected override bool IsInputChar(char keyChar)
			{
				return keyChar != '\t' && keyChar != '\r' && base.IsInputChar(keyChar);
			}

			// Token: 0x06006A1F RID: 27167 RVA: 0x00189F42 File Offset: 0x00188F42
			protected override void OnKeyDown(KeyEventArgs ke)
			{
				if (this.ProcessDialogKey(ke.KeyData))
				{
					ke.Handled = true;
					return;
				}
				base.OnKeyDown(ke);
			}

			// Token: 0x06006A20 RID: 27168 RVA: 0x00189F61 File Offset: 0x00188F61
			protected override void OnKeyPress(KeyPressEventArgs ke)
			{
				if (!this.IsInputChar(ke.KeyChar))
				{
					ke.Handled = true;
					return;
				}
				base.OnKeyPress(ke);
			}

			// Token: 0x06006A21 RID: 27169 RVA: 0x00189F80 File Offset: 0x00188F80
			public bool OnClickHooked()
			{
				return !this.psheet._Commit();
			}

			// Token: 0x06006A22 RID: 27170 RVA: 0x00189F90 File Offset: 0x00188F90
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
				if (!this.Focused)
				{
					Graphics graphics = base.CreateGraphics();
					if (this.psheet.SelectedGridEntry != null && base.ClientRectangle.Width <= this.psheet.SelectedGridEntry.GetValueTextWidth(this.Text, graphics, this.Font))
					{
						this.psheet.ToolTip.ToolTip = (this.PasswordProtect ? "" : this.Text);
					}
					graphics.Dispose();
				}
			}

			// Token: 0x06006A23 RID: 27171 RVA: 0x0018A018 File Offset: 0x00189018
			protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
			{
				Keys keys = keyData & Keys.KeyCode;
				switch (keys)
				{
				case Keys.Insert:
					if ((keyData & Keys.Alt) == Keys.None && ((keyData & Keys.Control) != Keys.None ^ (keyData & Keys.Shift) == Keys.None))
					{
						return false;
					}
					break;
				case Keys.Delete:
					if ((keyData & Keys.Control) == Keys.None && (keyData & Keys.Shift) != Keys.None && (keyData & Keys.Alt) == Keys.None)
					{
						return false;
					}
					if ((keyData & Keys.Control) == Keys.None && (keyData & Keys.Shift) == Keys.None && (keyData & Keys.Alt) == Keys.None && this.psheet.SelectedGridEntry != null && !this.psheet.SelectedGridEntry.Enumerable && !this.psheet.SelectedGridEntry.IsTextEditable && this.psheet.SelectedGridEntry.CanResetPropertyValue())
					{
						object propertyValue = this.psheet.SelectedGridEntry.PropertyValue;
						this.psheet.SelectedGridEntry.ResetPropertyValue();
						this.psheet.UnfocusSelection();
						this.psheet.ownerGrid.OnPropertyValueSet(this.psheet.SelectedGridEntry, propertyValue);
					}
					break;
				default:
					switch (keys)
					{
					case Keys.A:
						if ((keyData & Keys.Control) != Keys.None && (keyData & Keys.Shift) == Keys.None && (keyData & Keys.Alt) == Keys.None)
						{
							base.SelectAll();
							return true;
						}
						goto IL_19A;
					case Keys.B:
						goto IL_19A;
					case Keys.C:
						break;
					default:
						switch (keys)
						{
						case Keys.V:
						case Keys.X:
						case Keys.Z:
							break;
						case Keys.W:
						case Keys.Y:
							goto IL_19A;
						default:
							goto IL_19A;
						}
						break;
					}
					if ((keyData & Keys.Control) != Keys.None && (keyData & Keys.Shift) == Keys.None && (keyData & Keys.Alt) == Keys.None)
					{
						return false;
					}
					break;
				}
				IL_19A:
				return base.ProcessCmdKey(ref msg, keyData);
			}

			// Token: 0x06006A24 RID: 27172 RVA: 0x0018A1C8 File Offset: 0x001891C8
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.None)
				{
					Keys keys = keyData & Keys.KeyCode;
					if (keys == Keys.Return)
					{
						bool flag = !this.psheet.NeedsCommit;
						if (this.psheet.UnfocusSelection() && flag)
						{
							this.psheet.SelectedGridEntry.OnValueReturnKey();
						}
						return true;
					}
					if (keys == Keys.Escape)
					{
						this.psheet.OnEscape(this);
						return true;
					}
					if (keys == Keys.F4)
					{
						this.psheet.F4Selection(true);
						return true;
					}
				}
				if ((keyData & Keys.KeyCode) == Keys.Tab && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
				{
					return !this.psheet._Commit();
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06006A25 RID: 27173 RVA: 0x0018A270 File Offset: 0x00189270
			protected override void SetVisibleCore(bool value)
			{
				if (!value && this.HookMouseDown)
				{
					this.mouseHook.HookMouseDown = false;
				}
				base.SetVisibleCore(value);
			}

			// Token: 0x06006A26 RID: 27174 RVA: 0x0018A290 File Offset: 0x00189290
			internal bool WantsTab(bool forward)
			{
				return this.psheet.WantsTab(forward);
			}

			// Token: 0x06006A27 RID: 27175 RVA: 0x0018A2A0 File Offset: 0x001892A0
			private unsafe bool WmNotify(ref Message m)
			{
				if (m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
					if (ptr->hwndFrom == this.psheet.ToolTip.Handle)
					{
						int code = ptr->code;
						if (code == -521)
						{
							PropertyGridView.PositionTooltip(this, this.psheet.ToolTip, base.ClientRectangle);
							m.Result = (IntPtr)1;
							return true;
						}
						this.psheet.WndProc(ref m);
					}
				}
				return false;
			}

			// Token: 0x06006A28 RID: 27176 RVA: 0x0018A32C File Offset: 0x0018932C
			protected override void WndProc(ref Message m)
			{
				if (this.filter && this.psheet.FilterEditWndProc(ref m))
				{
					return;
				}
				int msg = m.Msg;
				if (msg <= 78)
				{
					if (msg != 2)
					{
						if (msg != 24)
						{
							if (msg == 78)
							{
								if (this.WmNotify(ref m))
								{
									return;
								}
							}
						}
						else if (IntPtr.Zero == m.WParam)
						{
							this.mouseHook.HookMouseDown = false;
						}
					}
					else
					{
						this.mouseHook.HookMouseDown = false;
					}
				}
				else if (msg <= 135)
				{
					if (msg != 125)
					{
						if (msg == 135)
						{
							m.Result = (IntPtr)((long)m.Result | 1L | 128L);
							if (this.psheet.NeedsCommit || this.WantsTab((Control.ModifierKeys & Keys.Shift) == Keys.None))
							{
								m.Result = (IntPtr)((long)m.Result | 4L | 2L);
							}
							return;
						}
					}
					else if (((int)m.WParam & -20) != 0)
					{
						this.psheet.Invalidate();
					}
				}
				else if (msg != 512)
				{
					if (msg == 770)
					{
						if (base.ReadOnly)
						{
							return;
						}
					}
				}
				else
				{
					if ((int)m.LParam == this.lastMove)
					{
						return;
					}
					this.lastMove = (int)m.LParam;
				}
				base.WndProc(ref m);
			}

			// Token: 0x06006A29 RID: 27177 RVA: 0x0018A4A8 File Offset: 0x001894A8
			public virtual bool InSetText()
			{
				return this.fInSetText;
			}

			// Token: 0x04003E21 RID: 15905
			internal bool fInSetText;

			// Token: 0x04003E22 RID: 15906
			internal bool filter;

			// Token: 0x04003E23 RID: 15907
			internal PropertyGridView psheet;

			// Token: 0x04003E24 RID: 15908
			private bool dontFocusMe;

			// Token: 0x04003E25 RID: 15909
			private int lastMove;

			// Token: 0x04003E26 RID: 15910
			private PropertyGridView.MouseHook mouseHook;
		}

		// Token: 0x020007CC RID: 1996
		internal class MouseHook
		{
			// Token: 0x06006A2A RID: 27178 RVA: 0x0018A4B0 File Offset: 0x001894B0
			public MouseHook(Control control, PropertyGridView.IMouseHookClient client, PropertyGridView gridView)
			{
				this.control = control;
				this.gridView = gridView;
				this.client = client;
			}

			// Token: 0x1700168E RID: 5774
			// (set) Token: 0x06006A2B RID: 27179 RVA: 0x0018A4D8 File Offset: 0x001894D8
			public bool DisableMouseHook
			{
				set
				{
					this.hookDisable = value;
					if (value)
					{
						this.UnhookMouse();
					}
				}
			}

			// Token: 0x1700168F RID: 5775
			// (get) Token: 0x06006A2C RID: 27180 RVA: 0x0018A4EA File Offset: 0x001894EA
			// (set) Token: 0x06006A2D RID: 27181 RVA: 0x0018A502 File Offset: 0x00189502
			public virtual bool HookMouseDown
			{
				get
				{
					GC.KeepAlive(this);
					return this.mouseHookHandle != IntPtr.Zero;
				}
				set
				{
					if (value && !this.hookDisable)
					{
						this.HookMouse();
						return;
					}
					this.UnhookMouse();
				}
			}

			// Token: 0x06006A2E RID: 27182 RVA: 0x0018A51C File Offset: 0x0018951C
			public void Dispose()
			{
				this.UnhookMouse();
			}

			// Token: 0x06006A2F RID: 27183 RVA: 0x0018A524 File Offset: 0x00189524
			private void HookMouse()
			{
				GC.KeepAlive(this);
				lock (this)
				{
					if (!(this.mouseHookHandle != IntPtr.Zero))
					{
						if (this.thisProcessID == 0)
						{
							SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this.control, this.control.Handle), out this.thisProcessID);
						}
						NativeMethods.HookProc hookProc = new NativeMethods.HookProc(new PropertyGridView.MouseHook.MouseHookObject(this).Callback);
						this.mouseHookRoot = GCHandle.Alloc(hookProc);
						this.mouseHookHandle = UnsafeNativeMethods.SetWindowsHookEx(7, hookProc, NativeMethods.NullHandleRef, SafeNativeMethods.GetCurrentThreadId());
					}
				}
			}

			// Token: 0x06006A30 RID: 27184 RVA: 0x0018A5CC File Offset: 0x001895CC
			private IntPtr MouseHookProc(int nCode, IntPtr wparam, IntPtr lparam)
			{
				GC.KeepAlive(this);
				if (nCode == 0)
				{
					NativeMethods.MOUSEHOOKSTRUCT mousehookstruct = (NativeMethods.MOUSEHOOKSTRUCT)UnsafeNativeMethods.PtrToStructure(lparam, typeof(NativeMethods.MOUSEHOOKSTRUCT));
					if (mousehookstruct != null)
					{
						int num = (int)wparam;
						if (num <= 164)
						{
							if (num != 33 && num != 161 && num != 164)
							{
								goto IL_96;
							}
						}
						else if (num <= 513)
						{
							if (num != 167 && num != 513)
							{
								goto IL_96;
							}
						}
						else if (num != 516 && num != 519)
						{
							goto IL_96;
						}
						if (this.ProcessMouseDown(mousehookstruct.hWnd, mousehookstruct.pt_x, mousehookstruct.pt_y))
						{
							return (IntPtr)1;
						}
					}
				}
				IL_96:
				return UnsafeNativeMethods.CallNextHookEx(new HandleRef(this, this.mouseHookHandle), nCode, wparam, lparam);
			}

			// Token: 0x06006A31 RID: 27185 RVA: 0x0018A684 File Offset: 0x00189684
			private void UnhookMouse()
			{
				GC.KeepAlive(this);
				lock (this)
				{
					if (this.mouseHookHandle != IntPtr.Zero)
					{
						UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(this, this.mouseHookHandle));
						this.mouseHookRoot.Free();
						this.mouseHookHandle = IntPtr.Zero;
					}
				}
			}

			// Token: 0x06006A32 RID: 27186 RVA: 0x0018A6F4 File Offset: 0x001896F4
			private bool ProcessMouseDown(IntPtr hWnd, int x, int y)
			{
				if (this.processing)
				{
					return false;
				}
				IntPtr handle = this.control.Handle;
				Control control = Control.FromHandleInternal(hWnd);
				if (hWnd != handle && !this.control.Contains(control))
				{
					int num;
					SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, hWnd), out num);
					if (num != this.thisProcessID)
					{
						this.HookMouseDown = false;
						return false;
					}
					bool flag = control == null || !this.gridView.IsSiblingControl(this.control, control);
					try
					{
						this.processing = true;
						if (flag && this.client.OnClickHooked())
						{
							return true;
						}
					}
					finally
					{
						this.processing = false;
					}
					this.HookMouseDown = false;
					return false;
				}
				return false;
			}

			// Token: 0x04003E27 RID: 15911
			private PropertyGridView gridView;

			// Token: 0x04003E28 RID: 15912
			private Control control;

			// Token: 0x04003E29 RID: 15913
			private PropertyGridView.IMouseHookClient client;

			// Token: 0x04003E2A RID: 15914
			internal int thisProcessID;

			// Token: 0x04003E2B RID: 15915
			private GCHandle mouseHookRoot;

			// Token: 0x04003E2C RID: 15916
			private IntPtr mouseHookHandle = IntPtr.Zero;

			// Token: 0x04003E2D RID: 15917
			private bool hookDisable;

			// Token: 0x04003E2E RID: 15918
			private bool processing;

			// Token: 0x020007CD RID: 1997
			private class MouseHookObject
			{
				// Token: 0x06006A33 RID: 27187 RVA: 0x0018A7BC File Offset: 0x001897BC
				public MouseHookObject(PropertyGridView.MouseHook parent)
				{
					this.reference = new WeakReference(parent, false);
				}

				// Token: 0x06006A34 RID: 27188 RVA: 0x0018A7D4 File Offset: 0x001897D4
				public virtual IntPtr Callback(int nCode, IntPtr wparam, IntPtr lparam)
				{
					IntPtr result = IntPtr.Zero;
					try
					{
						PropertyGridView.MouseHook mouseHook = (PropertyGridView.MouseHook)this.reference.Target;
						if (mouseHook != null)
						{
							result = mouseHook.MouseHookProc(nCode, wparam, lparam);
						}
					}
					catch
					{
					}
					return result;
				}

				// Token: 0x04003E2F RID: 15919
				internal WeakReference reference;
			}
		}

		// Token: 0x020007CE RID: 1998
		[ComVisible(true)]
		internal class PropertyGridViewAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06006A35 RID: 27189 RVA: 0x0018A81C File Offset: 0x0018981C
			public PropertyGridViewAccessibleObject(PropertyGridView owner) : base(owner)
			{
			}

			// Token: 0x17001690 RID: 5776
			// (get) Token: 0x06006A36 RID: 27190 RVA: 0x0018A828 File Offset: 0x00189828
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					return SR.GetString("PropertyGridDefaultAccessibleName");
				}
			}

			// Token: 0x17001691 RID: 5777
			// (get) Token: 0x06006A37 RID: 27191 RVA: 0x0018A850 File Offset: 0x00189850
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Table;
				}
			}

			// Token: 0x06006A38 RID: 27192 RVA: 0x0018A874 File Offset: 0x00189874
			public AccessibleObject Next(GridEntry current)
			{
				int rowFromGridEntry = ((PropertyGridView)base.Owner).GetRowFromGridEntry(current);
				GridEntry gridEntryFromRow = ((PropertyGridView)base.Owner).GetGridEntryFromRow(rowFromGridEntry + 1);
				if (gridEntryFromRow != null)
				{
					return gridEntryFromRow.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006A39 RID: 27193 RVA: 0x0018A8B4 File Offset: 0x001898B4
			public AccessibleObject Previous(GridEntry current)
			{
				int rowFromGridEntry = ((PropertyGridView)base.Owner).GetRowFromGridEntry(current);
				GridEntry gridEntryFromRow = ((PropertyGridView)base.Owner).GetGridEntryFromRow(rowFromGridEntry - 1);
				if (gridEntryFromRow != null)
				{
					return gridEntryFromRow.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006A3A RID: 27194 RVA: 0x0018A8F4 File Offset: 0x001898F4
			public override AccessibleObject GetChild(int index)
			{
				GridEntryCollection gridEntryCollection = ((PropertyGridView)base.Owner).AccessibilityGetGridEntries();
				if (gridEntryCollection != null && index >= 0 && index < gridEntryCollection.Count)
				{
					return gridEntryCollection.GetEntry(index).AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006A3B RID: 27195 RVA: 0x0018A930 File Offset: 0x00189930
			public override int GetChildCount()
			{
				GridEntryCollection gridEntryCollection = ((PropertyGridView)base.Owner).AccessibilityGetGridEntries();
				if (gridEntryCollection != null)
				{
					return gridEntryCollection.Count;
				}
				return 0;
			}

			// Token: 0x06006A3C RID: 27196 RVA: 0x0018A95C File Offset: 0x0018995C
			public override AccessibleObject GetFocused()
			{
				GridEntry selectedGridEntry = ((PropertyGridView)base.Owner).SelectedGridEntry;
				if (selectedGridEntry != null && selectedGridEntry.Focus)
				{
					return selectedGridEntry.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006A3D RID: 27197 RVA: 0x0018A990 File Offset: 0x00189990
			public override AccessibleObject GetSelected()
			{
				GridEntry selectedGridEntry = ((PropertyGridView)base.Owner).SelectedGridEntry;
				if (selectedGridEntry != null)
				{
					return selectedGridEntry.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006A3E RID: 27198 RVA: 0x0018A9BC File Offset: 0x001899BC
			public override AccessibleObject HitTest(int x, int y)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(x, y);
				UnsafeNativeMethods.ScreenToClient(new HandleRef(base.Owner, base.Owner.Handle), point);
				Point left = ((PropertyGridView)base.Owner).FindPosition(point.x, point.y);
				if (left != PropertyGridView.InvalidPosition)
				{
					GridEntry gridEntryFromRow = ((PropertyGridView)base.Owner).GetGridEntryFromRow(left.Y);
					if (gridEntryFromRow != null)
					{
						return gridEntryFromRow.AccessibilityObject;
					}
				}
				return null;
			}

			// Token: 0x06006A3F RID: 27199 RVA: 0x0018AA3C File Offset: 0x00189A3C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				if (this.GetChildCount() > 0)
				{
					switch (navdir)
					{
					case AccessibleNavigation.FirstChild:
						return this.GetChild(0);
					case AccessibleNavigation.LastChild:
						return this.GetChild(this.GetChildCount() - 1);
					}
				}
				return null;
			}
		}

		// Token: 0x020007CF RID: 1999
		internal class GridPositionData
		{
			// Token: 0x06006A40 RID: 27200 RVA: 0x0018AA80 File Offset: 0x00189A80
			public GridPositionData(PropertyGridView gridView)
			{
				this.selectedItemTree = gridView.GetGridEntryHierarchy(gridView.selectedGridEntry);
				this.expandedState = gridView.SaveHierarchyState(gridView.topLevelGridEntries);
				this.itemRow = gridView.selectedRow;
				this.itemCount = gridView.totalProps;
			}

			// Token: 0x06006A41 RID: 27201 RVA: 0x0018AAD0 File Offset: 0x00189AD0
			public GridEntry Restore(PropertyGridView gridView)
			{
				gridView.RestoreHierarchyState(this.expandedState);
				GridEntry gridEntry = gridView.FindEquivalentGridEntry(this.selectedItemTree);
				if (gridEntry != null)
				{
					gridView.SelectGridEntry(gridEntry, true);
					int num = gridView.selectedRow - this.itemRow;
					if (num != 0 && gridView.ScrollBar.Visible && this.itemRow < gridView.visibleRows)
					{
						num += gridView.GetScrollOffset();
						if (num < 0)
						{
							num = 0;
						}
						else if (num > gridView.ScrollBar.Maximum)
						{
							num = gridView.ScrollBar.Maximum - 1;
						}
						gridView.SetScrollOffset(num);
					}
				}
				return gridEntry;
			}

			// Token: 0x04003E30 RID: 15920
			private ArrayList expandedState;

			// Token: 0x04003E31 RID: 15921
			private GridEntryCollection selectedItemTree;

			// Token: 0x04003E32 RID: 15922
			private int itemRow;

			// Token: 0x04003E33 RID: 15923
			private int itemCount;
		}
	}
}
