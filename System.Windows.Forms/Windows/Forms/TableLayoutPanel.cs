using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000646 RID: 1606
	[ProvideProperty("ColumnSpan", typeof(Control))]
	[Docking(DockingBehavior.Never)]
	[SRDescription("DescriptionTableLayoutPanel")]
	[ProvideProperty("Row", typeof(Control))]
	[ProvideProperty("Column", typeof(Control))]
	[ProvideProperty("CellPosition", typeof(Control))]
	[DefaultProperty("ColumnCount")]
	[DesignerSerializer("System.Windows.Forms.Design.TableLayoutPanelCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ProvideProperty("RowSpan", typeof(Control))]
	[Designer("System.Windows.Forms.Design.TableLayoutPanelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class TableLayoutPanel : Panel, IExtenderProvider
	{
		// Token: 0x06005486 RID: 21638 RVA: 0x001343DB File Offset: 0x001333DB
		public TableLayoutPanel()
		{
			this._tableLayoutSettings = TableLayout.CreateSettings(this);
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06005487 RID: 21639 RVA: 0x001343EF File Offset: 0x001333EF
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return TableLayout.Instance;
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06005488 RID: 21640 RVA: 0x001343F6 File Offset: 0x001333F6
		// (set) Token: 0x06005489 RID: 21641 RVA: 0x00134400 File Offset: 0x00133400
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public TableLayoutSettings LayoutSettings
		{
			get
			{
				return this._tableLayoutSettings;
			}
			set
			{
				if (value != null && value.IsStub)
				{
					using (new LayoutTransaction(this, this, PropertyNames.LayoutSettings))
					{
						this._tableLayoutSettings.ApplySettings(value);
						return;
					}
				}
				throw new NotSupportedException(SR.GetString("TableLayoutSettingSettingsIsNotSupported"));
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x0600548A RID: 21642 RVA: 0x00134460 File Offset: 0x00133460
		// (set) Token: 0x0600548B RID: 21643 RVA: 0x00134468 File Offset: 0x00133468
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600548C RID: 21644 RVA: 0x00134471 File Offset: 0x00133471
		// (set) Token: 0x0600548D RID: 21645 RVA: 0x0013447E File Offset: 0x0013347E
		[SRDescription("TableLayoutPanelCellBorderStyleDescr")]
		[Localizable(true)]
		[DefaultValue(TableLayoutPanelCellBorderStyle.None)]
		[SRCategory("CatAppearance")]
		public TableLayoutPanelCellBorderStyle CellBorderStyle
		{
			get
			{
				return this._tableLayoutSettings.CellBorderStyle;
			}
			set
			{
				this._tableLayoutSettings.CellBorderStyle = value;
				if (value != TableLayoutPanelCellBorderStyle.None)
				{
					base.SetStyle(ControlStyles.ResizeRedraw, true);
				}
				base.Invalidate();
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600548E RID: 21646 RVA: 0x0013449E File Offset: 0x0013349E
		private int CellBorderWidth
		{
			get
			{
				return this._tableLayoutSettings.CellBorderWidth;
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x0600548F RID: 21647 RVA: 0x001344AB File Offset: 0x001334AB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("ControlControlsDescr")]
		[Browsable(false)]
		public new TableLayoutControlCollection Controls
		{
			get
			{
				return (TableLayoutControlCollection)base.Controls;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06005490 RID: 21648 RVA: 0x001344B8 File Offset: 0x001334B8
		// (set) Token: 0x06005491 RID: 21649 RVA: 0x001344C5 File Offset: 0x001334C5
		[Localizable(true)]
		[SRDescription("GridPanelColumnsDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		public int ColumnCount
		{
			get
			{
				return this._tableLayoutSettings.ColumnCount;
			}
			set
			{
				this._tableLayoutSettings.ColumnCount = value;
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06005492 RID: 21650 RVA: 0x001344D3 File Offset: 0x001334D3
		// (set) Token: 0x06005493 RID: 21651 RVA: 0x001344E0 File Offset: 0x001334E0
		[SRDescription("TableLayoutPanelGrowStyleDescr")]
		[DefaultValue(TableLayoutPanelGrowStyle.AddRows)]
		[SRCategory("CatLayout")]
		public TableLayoutPanelGrowStyle GrowStyle
		{
			get
			{
				return this._tableLayoutSettings.GrowStyle;
			}
			set
			{
				this._tableLayoutSettings.GrowStyle = value;
			}
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06005494 RID: 21652 RVA: 0x001344EE File Offset: 0x001334EE
		// (set) Token: 0x06005495 RID: 21653 RVA: 0x001344FB File Offset: 0x001334FB
		[SRDescription("GridPanelRowsDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		public int RowCount
		{
			get
			{
				return this._tableLayoutSettings.RowCount;
			}
			set
			{
				this._tableLayoutSettings.RowCount = value;
			}
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06005496 RID: 21654 RVA: 0x00134509 File Offset: 0x00133509
		[SRDescription("GridPanelRowStylesDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatLayout")]
		[DisplayName("Rows")]
		[MergableProperty(false)]
		public TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				return this._tableLayoutSettings.RowStyles;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06005497 RID: 21655 RVA: 0x00134516 File Offset: 0x00133516
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("GridPanelColumnStylesDescr")]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[DisplayName("Columns")]
		[MergableProperty(false)]
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				return this._tableLayoutSettings.ColumnStyles;
			}
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x00134523 File Offset: 0x00133523
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new TableLayoutControlCollection(this);
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x0013452C File Offset: 0x0013352C
		private bool ShouldSerializeControls()
		{
			TableLayoutControlCollection controls = this.Controls;
			return controls != null && controls.Count > 0;
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x00134550 File Offset: 0x00133550
		bool IExtenderProvider.CanExtend(object obj)
		{
			Control control = obj as Control;
			return control != null && control.Parent == this;
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x00134572 File Offset: 0x00133572
		[DefaultValue(1)]
		[SRDescription("GridPanelGetColumnSpanDescr")]
		[SRCategory("CatLayout")]
		[DisplayName("ColumnSpan")]
		public int GetColumnSpan(Control control)
		{
			return this._tableLayoutSettings.GetColumnSpan(control);
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x00134580 File Offset: 0x00133580
		public void SetColumnSpan(Control control, int value)
		{
			this._tableLayoutSettings.SetColumnSpan(control, value);
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x0013458F File Offset: 0x0013358F
		[SRDescription("GridPanelGetRowSpanDescr")]
		[SRCategory("CatLayout")]
		[DisplayName("RowSpan")]
		[DefaultValue(1)]
		public int GetRowSpan(Control control)
		{
			return this._tableLayoutSettings.GetRowSpan(control);
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x0013459D File Offset: 0x0013359D
		public void SetRowSpan(Control control, int value)
		{
			this._tableLayoutSettings.SetRowSpan(control, value);
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x001345AC File Offset: 0x001335AC
		[SRDescription("GridPanelRowDescr")]
		[DefaultValue(-1)]
		[DisplayName("Row")]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int GetRow(Control control)
		{
			return this._tableLayoutSettings.GetRow(control);
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x001345BA File Offset: 0x001335BA
		public void SetRow(Control control, int row)
		{
			this._tableLayoutSettings.SetRow(control, row);
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x001345C9 File Offset: 0x001335C9
		[SRDescription("GridPanelCellPositionDescr")]
		[SRCategory("CatLayout")]
		[DisplayName("Cell")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(-1)]
		public TableLayoutPanelCellPosition GetCellPosition(Control control)
		{
			return this._tableLayoutSettings.GetCellPosition(control);
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x001345D7 File Offset: 0x001335D7
		public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
		{
			this._tableLayoutSettings.SetCellPosition(control, position);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x001345E6 File Offset: 0x001335E6
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Column")]
		[DefaultValue(-1)]
		[SRDescription("GridPanelColumnDescr")]
		public int GetColumn(Control control)
		{
			return this._tableLayoutSettings.GetColumn(control);
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x001345F4 File Offset: 0x001335F4
		public void SetColumn(Control control, int column)
		{
			this._tableLayoutSettings.SetColumn(control, column);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x00134603 File Offset: 0x00133603
		public Control GetControlFromPosition(int column, int row)
		{
			return (Control)this._tableLayoutSettings.GetControlFromPosition(column, row);
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x00134617 File Offset: 0x00133617
		public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
		{
			return this._tableLayoutSettings.GetPositionFromControl(control);
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x00134628 File Offset: 0x00133628
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetColumnWidths()
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			if (containerInfo.Columns == null)
			{
				return new int[0];
			}
			int[] array = new int[containerInfo.Columns.Length];
			for (int i = 0; i < containerInfo.Columns.Length; i++)
			{
				array[i] = containerInfo.Columns[i].MinSize;
			}
			return array;
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x00134684 File Offset: 0x00133684
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int[] GetRowHeights()
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			if (containerInfo.Rows == null)
			{
				return new int[0];
			}
			int[] array = new int[containerInfo.Rows.Length];
			for (int i = 0; i < containerInfo.Rows.Length; i++)
			{
				array[i] = containerInfo.Rows[i].MinSize;
			}
			return array;
		}

		// Token: 0x14000323 RID: 803
		// (add) Token: 0x060054A9 RID: 21673 RVA: 0x001346DD File Offset: 0x001336DD
		// (remove) Token: 0x060054AA RID: 21674 RVA: 0x001346F0 File Offset: 0x001336F0
		[SRDescription("TableLayoutPanelOnPaintCellDescr")]
		[SRCategory("CatAppearance")]
		public event TableLayoutCellPaintEventHandler CellPaint
		{
			add
			{
				base.Events.AddHandler(TableLayoutPanel.EventCellPaint, value);
			}
			remove
			{
				base.Events.RemoveHandler(TableLayoutPanel.EventCellPaint, value);
			}
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x00134703 File Offset: 0x00133703
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			base.Invalidate();
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x00134714 File Offset: 0x00133714
		protected virtual void OnCellPaint(TableLayoutCellPaintEventArgs e)
		{
			TableLayoutCellPaintEventHandler tableLayoutCellPaintEventHandler = (TableLayoutCellPaintEventHandler)base.Events[TableLayoutPanel.EventCellPaint];
			if (tableLayoutCellPaintEventHandler != null)
			{
				tableLayoutCellPaintEventHandler(this, e);
			}
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x00134744 File Offset: 0x00133744
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			int cellBorderWidth = this.CellBorderWidth;
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			TableLayout.Strip[] columns = containerInfo.Columns;
			TableLayout.Strip[] rows = containerInfo.Rows;
			TableLayoutPanelCellBorderStyle cellBorderStyle = this.CellBorderStyle;
			if (columns == null || rows == null)
			{
				return;
			}
			int num = columns.Length;
			int num2 = rows.Length;
			int num3 = 0;
			int num4 = 0;
			Graphics graphics = e.Graphics;
			Rectangle displayRectangle = this.DisplayRectangle;
			Rectangle clipRectangle = e.ClipRectangle;
			bool flag = this.RightToLeft == RightToLeft.Yes;
			int num5;
			if (flag)
			{
				num5 = displayRectangle.Right - cellBorderWidth / 2;
			}
			else
			{
				num5 = displayRectangle.X + cellBorderWidth / 2;
			}
			for (int i = 0; i < num; i++)
			{
				int num6 = displayRectangle.Y + cellBorderWidth / 2;
				if (flag)
				{
					num5 -= columns[i].MinSize;
				}
				for (int j = 0; j < num2; j++)
				{
					Rectangle bound = new Rectangle(num5, num6, columns[i].MinSize, rows[j].MinSize);
					Rectangle rectangle = new Rectangle(bound.X + (cellBorderWidth + 1) / 2, bound.Y + (cellBorderWidth + 1) / 2, bound.Width - (cellBorderWidth + 1) / 2, bound.Height - (cellBorderWidth + 1) / 2);
					if (clipRectangle.IntersectsWith(rectangle))
					{
						using (TableLayoutCellPaintEventArgs tableLayoutCellPaintEventArgs = new TableLayoutCellPaintEventArgs(graphics, clipRectangle, rectangle, i, j))
						{
							this.OnCellPaint(tableLayoutCellPaintEventArgs);
						}
						ControlPaint.PaintTableCellBorder(cellBorderStyle, graphics, bound);
					}
					num6 += rows[j].MinSize;
					if (i == 0)
					{
						num4 += rows[j].MinSize;
					}
				}
				if (!flag)
				{
					num5 += columns[i].MinSize;
				}
				num3 += columns[i].MinSize;
			}
			if (!base.HScroll && !base.VScroll && cellBorderStyle != TableLayoutPanelCellBorderStyle.None)
			{
				Rectangle bound2 = new Rectangle(cellBorderWidth / 2 + displayRectangle.X, cellBorderWidth / 2 + displayRectangle.Y, displayRectangle.Width - cellBorderWidth, displayRectangle.Height - cellBorderWidth);
				if (cellBorderStyle == TableLayoutPanelCellBorderStyle.Inset)
				{
					graphics.DrawLine(SystemPens.ControlDark, bound2.Right, bound2.Y, bound2.Right, bound2.Bottom);
					graphics.DrawLine(SystemPens.ControlDark, bound2.X, bound2.Y + bound2.Height - 1, bound2.X + bound2.Width - 1, bound2.Y + bound2.Height - 1);
				}
				else
				{
					if (cellBorderStyle == TableLayoutPanelCellBorderStyle.Outset)
					{
						using (Pen pen = new Pen(SystemColors.Window))
						{
							graphics.DrawLine(pen, bound2.X + bound2.Width - 1, bound2.Y, bound2.X + bound2.Width - 1, bound2.Y + bound2.Height - 1);
							graphics.DrawLine(pen, bound2.X, bound2.Y + bound2.Height - 1, bound2.X + bound2.Width - 1, bound2.Y + bound2.Height - 1);
							goto IL_33A;
						}
					}
					ControlPaint.PaintTableCellBorder(cellBorderStyle, graphics, bound2);
				}
				IL_33A:
				ControlPaint.PaintTableControlBorder(cellBorderStyle, graphics, displayRectangle);
				return;
			}
			ControlPaint.PaintTableControlBorder(cellBorderStyle, graphics, displayRectangle);
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x00134AC0 File Offset: 0x00133AC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			base.ScaleCore(dx, dy);
			this.ScaleAbsoluteStyles(new SizeF(dx, dy));
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x00134AD7 File Offset: 0x00133AD7
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			base.ScaleControl(factor, specified);
			this.ScaleAbsoluteStyles(factor);
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x00134AE8 File Offset: 0x00133AE8
		private void ScaleAbsoluteStyles(SizeF factor)
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			int num = 0;
			int num2 = -1;
			int num3 = containerInfo.Rows.Length - 1;
			if (containerInfo.Rows.Length > 0)
			{
				num2 = containerInfo.Rows[num3].MinSize;
			}
			int num4 = -1;
			int num5 = containerInfo.Columns.Length - 1;
			if (containerInfo.Columns.Length > 0)
			{
				num4 = containerInfo.Columns[containerInfo.Columns.Length - 1].MinSize;
			}
			foreach (object obj in ((IEnumerable)this.ColumnStyles))
			{
				ColumnStyle columnStyle = (ColumnStyle)obj;
				if (columnStyle.SizeType == SizeType.Absolute)
				{
					if (num == num5 && num4 > 0)
					{
						columnStyle.Width = (float)Math.Round((double)((float)num4 * factor.Width));
					}
					else
					{
						columnStyle.Width = (float)Math.Round((double)(columnStyle.Width * factor.Width));
					}
				}
				num++;
			}
			num = 0;
			foreach (object obj2 in ((IEnumerable)this.RowStyles))
			{
				RowStyle rowStyle = (RowStyle)obj2;
				if (rowStyle.SizeType == SizeType.Absolute)
				{
					if (num == num3 && num2 > 0)
					{
						rowStyle.Height = (float)Math.Round((double)((float)num2 * factor.Height));
					}
					else
					{
						rowStyle.Height = (float)Math.Round((double)(rowStyle.Height * factor.Height));
					}
				}
			}
		}

		// Token: 0x040036DA RID: 14042
		private TableLayoutSettings _tableLayoutSettings;

		// Token: 0x040036DB RID: 14043
		private static readonly object EventCellPaint = new object();
	}
}
