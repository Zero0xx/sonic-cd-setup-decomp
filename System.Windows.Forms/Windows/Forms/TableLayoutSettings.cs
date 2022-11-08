using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200064C RID: 1612
	[TypeConverter(typeof(TableLayoutSettingsTypeConverter))]
	[Serializable]
	public sealed class TableLayoutSettings : LayoutSettings, ISerializable
	{
		// Token: 0x060054C8 RID: 21704 RVA: 0x0013509B File Offset: 0x0013409B
		internal TableLayoutSettings() : base(null)
		{
			this._stub = new TableLayoutSettings.TableLayoutSettingsStub();
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x001350AF File Offset: 0x001340AF
		internal TableLayoutSettings(IArrangedElement owner) : base(owner)
		{
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x001350B8 File Offset: 0x001340B8
		internal TableLayoutSettings(SerializationInfo serializationInfo, StreamingContext context) : this()
		{
			TypeConverter converter = TypeDescriptor.GetConverter(this);
			string @string = serializationInfo.GetString("SerializedString");
			if (!string.IsNullOrEmpty(@string) && converter != null)
			{
				TableLayoutSettings tableLayoutSettings = converter.ConvertFromInvariantString(@string) as TableLayoutSettings;
				if (tableLayoutSettings != null)
				{
					this.ApplySettings(tableLayoutSettings);
				}
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x060054CB RID: 21707 RVA: 0x00135100 File Offset: 0x00134100
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return TableLayout.Instance;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x060054CC RID: 21708 RVA: 0x00135107 File Offset: 0x00134107
		private TableLayout TableLayout
		{
			get
			{
				return (TableLayout)this.LayoutEngine;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x060054CD RID: 21709 RVA: 0x00135114 File Offset: 0x00134114
		// (set) Token: 0x060054CE RID: 21710 RVA: 0x0013511C File Offset: 0x0013411C
		[DefaultValue(TableLayoutPanelCellBorderStyle.None)]
		[SRCategory("CatAppearance")]
		[SRDescription("TableLayoutPanelCellBorderStyleDescr")]
		internal TableLayoutPanelCellBorderStyle CellBorderStyle
		{
			get
			{
				return this._borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 6))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"CellBorderStyle",
						value.ToString()
					}));
				}
				this._borderStyle = value;
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				containerInfo.CellBorderWidth = TableLayoutSettings.borderStyleToOffset[(int)value];
				LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.CellBorderStyle);
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x060054CF RID: 21711 RVA: 0x0013519D File Offset: 0x0013419D
		[DefaultValue(0)]
		internal int CellBorderWidth
		{
			get
			{
				return TableLayout.GetContainerInfo(base.Owner).CellBorderWidth;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x060054D0 RID: 21712 RVA: 0x001351B0 File Offset: 0x001341B0
		// (set) Token: 0x060054D1 RID: 21713 RVA: 0x001351D0 File Offset: 0x001341D0
		[SRCategory("CatLayout")]
		[SRDescription("GridPanelColumnsDescr")]
		[DefaultValue(0)]
		public int ColumnCount
		{
			get
			{
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.MaxColumns;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ColumnCount", value, SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ColumnCount",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				containerInfo.MaxColumns = value;
				LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.Columns);
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x060054D2 RID: 21714 RVA: 0x00135258 File Offset: 0x00134258
		// (set) Token: 0x060054D3 RID: 21715 RVA: 0x00135278 File Offset: 0x00134278
		[SRDescription("GridPanelRowsDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		public int RowCount
		{
			get
			{
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.MaxRows;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("RowCount", value, SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"RowCount",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				containerInfo.MaxRows = value;
				LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.Rows);
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x060054D4 RID: 21716 RVA: 0x00135300 File Offset: 0x00134300
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("GridPanelRowStylesDescr")]
		public TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				if (this.IsStub)
				{
					return this._stub.RowStyles;
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.RowStyles;
			}
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x060054D5 RID: 21717 RVA: 0x00135334 File Offset: 0x00134334
		[SRDescription("GridPanelColumnStylesDescr")]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				if (this.IsStub)
				{
					return this._stub.ColumnStyles;
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.ColumnStyles;
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x060054D6 RID: 21718 RVA: 0x00135367 File Offset: 0x00134367
		// (set) Token: 0x060054D7 RID: 21719 RVA: 0x0013537C File Offset: 0x0013437C
		[DefaultValue(TableLayoutPanelGrowStyle.AddRows)]
		[SRDescription("TableLayoutPanelGrowStyleDescr")]
		[SRCategory("CatLayout")]
		public TableLayoutPanelGrowStyle GrowStyle
		{
			get
			{
				return TableLayout.GetContainerInfo(base.Owner).GrowStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"GrowStyle",
						value.ToString()
					}));
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				if (containerInfo.GrowStyle != value)
				{
					containerInfo.GrowStyle = value;
					LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.GrowStyle);
				}
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x060054D8 RID: 21720 RVA: 0x001353F9 File Offset: 0x001343F9
		internal bool IsStub
		{
			get
			{
				return this._stub != null;
			}
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x00135406 File Offset: 0x00134406
		internal void ApplySettings(TableLayoutSettings settings)
		{
			if (settings.IsStub)
			{
				if (!this.IsStub)
				{
					settings._stub.ApplySettings(this);
					return;
				}
				this._stub = settings._stub;
			}
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x00135434 File Offset: 0x00134434
		public int GetColumnSpan(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				return this._stub.GetColumnSpan(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			return TableLayout.GetLayoutInfo(element).ColumnSpan;
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x0013547C File Offset: 0x0013447C
		public void SetColumnSpan(object control, int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("ColumnSpan", SR.GetString("InvalidArgument", new object[]
				{
					"ColumnSpan",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.IsStub)
			{
				this._stub.SetColumnSpan(control, value);
				return;
			}
			IArrangedElement arrangedElement = this.LayoutEngine.CastToArrangedElement(control);
			if (arrangedElement.Container != null)
			{
				TableLayout.ClearCachedAssignments(TableLayout.GetContainerInfo(arrangedElement.Container));
			}
			TableLayout.GetLayoutInfo(arrangedElement).ColumnSpan = value;
			LayoutTransaction.DoLayout(arrangedElement.Container, arrangedElement, PropertyNames.ColumnSpan);
		}

		// Token: 0x060054DC RID: 21724 RVA: 0x0013551C File Offset: 0x0013451C
		public int GetRowSpan(object control)
		{
			if (this.IsStub)
			{
				return this._stub.GetRowSpan(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			return TableLayout.GetLayoutInfo(element).RowSpan;
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x00135558 File Offset: 0x00134558
		public void SetRowSpan(object control, int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("RowSpan", SR.GetString("InvalidArgument", new object[]
				{
					"RowSpan",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				this._stub.SetRowSpan(control, value);
				return;
			}
			IArrangedElement arrangedElement = this.LayoutEngine.CastToArrangedElement(control);
			if (arrangedElement.Container != null)
			{
				TableLayout.ClearCachedAssignments(TableLayout.GetContainerInfo(arrangedElement.Container));
			}
			TableLayout.GetLayoutInfo(arrangedElement).RowSpan = value;
			LayoutTransaction.DoLayout(arrangedElement.Container, arrangedElement, PropertyNames.RowSpan);
		}

		// Token: 0x060054DE RID: 21726 RVA: 0x00135604 File Offset: 0x00134604
		[DefaultValue(-1)]
		[SRCategory("CatLayout")]
		[SRDescription("GridPanelRowDescr")]
		public int GetRow(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				return this._stub.GetRow(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(element);
			return layoutInfo.RowPosition;
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x00135650 File Offset: 0x00134650
		public void SetRow(object control, int row)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (row < -1)
			{
				throw new ArgumentOutOfRangeException("Row", SR.GetString("InvalidArgument", new object[]
				{
					"Row",
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.SetCellPosition(control, row, -1, true, false);
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x001356AE File Offset: 0x001346AE
		[SRDescription("TableLayoutSettingsGetCellPositionDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		public TableLayoutPanelCellPosition GetCellPosition(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return new TableLayoutPanelCellPosition(this.GetColumn(control), this.GetRow(control));
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x001356D1 File Offset: 0x001346D1
		[SRDescription("TableLayoutSettingsSetCellPositionDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		public void SetCellPosition(object control, TableLayoutPanelCellPosition cellPosition)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			this.SetCellPosition(control, cellPosition.Row, cellPosition.Column, true, true);
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x001356F8 File Offset: 0x001346F8
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		[SRDescription("GridPanelColumnDescr")]
		public int GetColumn(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				return this._stub.GetColumn(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(element);
			return layoutInfo.ColumnPosition;
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x00135744 File Offset: 0x00134744
		public void SetColumn(object control, int column)
		{
			if (column < -1)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"Column",
					column.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.IsStub)
			{
				this._stub.SetColumn(control, column);
				return;
			}
			this.SetCellPosition(control, -1, column, false, true);
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x001357A8 File Offset: 0x001347A8
		private void SetCellPosition(object control, int row, int column, bool rowSpecified, bool colSpecified)
		{
			if (this.IsStub)
			{
				if (colSpecified)
				{
					this._stub.SetColumn(control, column);
				}
				if (rowSpecified)
				{
					this._stub.SetRow(control, row);
					return;
				}
			}
			else
			{
				IArrangedElement arrangedElement = this.LayoutEngine.CastToArrangedElement(control);
				if (arrangedElement.Container != null)
				{
					TableLayout.ClearCachedAssignments(TableLayout.GetContainerInfo(arrangedElement.Container));
				}
				TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(arrangedElement);
				if (colSpecified)
				{
					layoutInfo.ColumnPosition = column;
				}
				if (rowSpecified)
				{
					layoutInfo.RowPosition = row;
				}
				LayoutTransaction.DoLayout(arrangedElement.Container, arrangedElement, PropertyNames.TableIndex);
			}
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x00135833 File Offset: 0x00134833
		internal IArrangedElement GetControlFromPosition(int column, int row)
		{
			return this.TableLayout.GetControlFromPosition(base.Owner, column, row);
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x00135848 File Offset: 0x00134848
		internal TableLayoutPanelCellPosition GetPositionFromControl(IArrangedElement element)
		{
			return this.TableLayout.GetPositionFromControl(base.Owner, element);
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x0013585C File Offset: 0x0013485C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(this);
			string value = (converter != null) ? converter.ConvertToInvariantString(this) : null;
			if (!string.IsNullOrEmpty(value))
			{
				si.AddValue("SerializedString", value);
			}
		}

		// Token: 0x060054E8 RID: 21736 RVA: 0x00135894 File Offset: 0x00134894
		internal List<TableLayoutSettings.ControlInformation> GetControlsInformation()
		{
			if (this.IsStub)
			{
				return this._stub.GetControlsInformation();
			}
			List<TableLayoutSettings.ControlInformation> list = new List<TableLayoutSettings.ControlInformation>(base.Owner.Children.Count);
			foreach (object obj in base.Owner.Children)
			{
				IArrangedElement arrangedElement = (IArrangedElement)obj;
				Control control = arrangedElement as Control;
				if (control != null)
				{
					TableLayoutSettings.ControlInformation item = default(TableLayoutSettings.ControlInformation);
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["Name"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
					{
						item.Name = propertyDescriptor.GetValue(control);
					}
					item.Row = this.GetRow(control);
					item.RowSpan = this.GetRowSpan(control);
					item.Column = this.GetColumn(control);
					item.ColumnSpan = this.GetColumnSpan(control);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x040036EB RID: 14059
		private static int[] borderStyleToOffset = new int[]
		{
			0,
			1,
			2,
			3,
			2,
			3,
			3
		};

		// Token: 0x040036EC RID: 14060
		private TableLayoutPanelCellBorderStyle _borderStyle;

		// Token: 0x040036ED RID: 14061
		private TableLayoutSettings.TableLayoutSettingsStub _stub;

		// Token: 0x0200064D RID: 1613
		internal struct ControlInformation
		{
			// Token: 0x060054EA RID: 21738 RVA: 0x001359E4 File Offset: 0x001349E4
			internal ControlInformation(object name, int row, int column, int rowSpan, int columnSpan)
			{
				this.Name = name;
				this.Row = row;
				this.Column = column;
				this.RowSpan = rowSpan;
				this.ColumnSpan = columnSpan;
			}

			// Token: 0x040036EE RID: 14062
			internal object Name;

			// Token: 0x040036EF RID: 14063
			internal int Row;

			// Token: 0x040036F0 RID: 14064
			internal int Column;

			// Token: 0x040036F1 RID: 14065
			internal int RowSpan;

			// Token: 0x040036F2 RID: 14066
			internal int ColumnSpan;
		}

		// Token: 0x0200064E RID: 1614
		private class TableLayoutSettingsStub
		{
			// Token: 0x060054EC RID: 21740 RVA: 0x00135A1C File Offset: 0x00134A1C
			internal void ApplySettings(TableLayoutSettings settings)
			{
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(settings.Owner);
				Control control = containerInfo.Container as Control;
				if (control != null && this.controlsInfo != null)
				{
					foreach (object obj in this.controlsInfo.Keys)
					{
						TableLayoutSettings.ControlInformation controlInformation = this.controlsInfo[obj];
						foreach (object obj2 in control.Controls)
						{
							Control control2 = (Control)obj2;
							if (control2 != null)
							{
								string @string = null;
								PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control2)["Name"];
								if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
								{
									@string = (propertyDescriptor.GetValue(control2) as string);
								}
								if (WindowsFormsUtils.SafeCompareStrings(@string, obj as string, false))
								{
									settings.SetRow(control2, controlInformation.Row);
									settings.SetColumn(control2, controlInformation.Column);
									settings.SetRowSpan(control2, controlInformation.RowSpan);
									settings.SetColumnSpan(control2, controlInformation.ColumnSpan);
									break;
								}
							}
						}
					}
				}
				containerInfo.RowStyles = this.rowStyles;
				containerInfo.ColumnStyles = this.columnStyles;
				this.columnStyles = null;
				this.rowStyles = null;
				this.isValid = false;
			}

			// Token: 0x17001196 RID: 4502
			// (get) Token: 0x060054ED RID: 21741 RVA: 0x00135BB8 File Offset: 0x00134BB8
			public TableLayoutColumnStyleCollection ColumnStyles
			{
				get
				{
					if (this.columnStyles == null)
					{
						this.columnStyles = new TableLayoutColumnStyleCollection();
					}
					return this.columnStyles;
				}
			}

			// Token: 0x17001197 RID: 4503
			// (get) Token: 0x060054EE RID: 21742 RVA: 0x00135BD3 File Offset: 0x00134BD3
			public bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			// Token: 0x17001198 RID: 4504
			// (get) Token: 0x060054EF RID: 21743 RVA: 0x00135BDB File Offset: 0x00134BDB
			public TableLayoutRowStyleCollection RowStyles
			{
				get
				{
					if (this.rowStyles == null)
					{
						this.rowStyles = new TableLayoutRowStyleCollection();
					}
					return this.rowStyles;
				}
			}

			// Token: 0x060054F0 RID: 21744 RVA: 0x00135BF8 File Offset: 0x00134BF8
			internal List<TableLayoutSettings.ControlInformation> GetControlsInformation()
			{
				if (this.controlsInfo == null)
				{
					return new List<TableLayoutSettings.ControlInformation>();
				}
				List<TableLayoutSettings.ControlInformation> list = new List<TableLayoutSettings.ControlInformation>(this.controlsInfo.Count);
				foreach (object obj in this.controlsInfo.Keys)
				{
					TableLayoutSettings.ControlInformation item = this.controlsInfo[obj];
					item.Name = obj;
					list.Add(item);
				}
				return list;
			}

			// Token: 0x060054F1 RID: 21745 RVA: 0x00135C88 File Offset: 0x00134C88
			private TableLayoutSettings.ControlInformation GetControlInformation(object controlName)
			{
				if (this.controlsInfo == null)
				{
					return TableLayoutSettings.TableLayoutSettingsStub.DefaultControlInfo;
				}
				if (!this.controlsInfo.ContainsKey(controlName))
				{
					return TableLayoutSettings.TableLayoutSettingsStub.DefaultControlInfo;
				}
				return this.controlsInfo[controlName];
			}

			// Token: 0x060054F2 RID: 21746 RVA: 0x00135CB8 File Offset: 0x00134CB8
			public int GetColumn(object controlName)
			{
				return this.GetControlInformation(controlName).Column;
			}

			// Token: 0x060054F3 RID: 21747 RVA: 0x00135CC6 File Offset: 0x00134CC6
			public int GetColumnSpan(object controlName)
			{
				return this.GetControlInformation(controlName).ColumnSpan;
			}

			// Token: 0x060054F4 RID: 21748 RVA: 0x00135CD4 File Offset: 0x00134CD4
			public int GetRow(object controlName)
			{
				return this.GetControlInformation(controlName).Row;
			}

			// Token: 0x060054F5 RID: 21749 RVA: 0x00135CE2 File Offset: 0x00134CE2
			public int GetRowSpan(object controlName)
			{
				return this.GetControlInformation(controlName).RowSpan;
			}

			// Token: 0x060054F6 RID: 21750 RVA: 0x00135CF0 File Offset: 0x00134CF0
			private void SetControlInformation(object controlName, TableLayoutSettings.ControlInformation info)
			{
				if (this.controlsInfo == null)
				{
					this.controlsInfo = new Dictionary<object, TableLayoutSettings.ControlInformation>();
				}
				this.controlsInfo[controlName] = info;
			}

			// Token: 0x060054F7 RID: 21751 RVA: 0x00135D14 File Offset: 0x00134D14
			public void SetColumn(object controlName, int column)
			{
				if (this.GetColumn(controlName) != column)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.Column = column;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x060054F8 RID: 21752 RVA: 0x00135D44 File Offset: 0x00134D44
			public void SetColumnSpan(object controlName, int value)
			{
				if (this.GetColumnSpan(controlName) != value)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.ColumnSpan = value;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x060054F9 RID: 21753 RVA: 0x00135D74 File Offset: 0x00134D74
			public void SetRow(object controlName, int row)
			{
				if (this.GetRow(controlName) != row)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.Row = row;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x060054FA RID: 21754 RVA: 0x00135DA4 File Offset: 0x00134DA4
			public void SetRowSpan(object controlName, int value)
			{
				if (this.GetRowSpan(controlName) != value)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.RowSpan = value;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x040036F3 RID: 14067
			private static TableLayoutSettings.ControlInformation DefaultControlInfo = new TableLayoutSettings.ControlInformation(null, -1, -1, 1, 1);

			// Token: 0x040036F4 RID: 14068
			private TableLayoutColumnStyleCollection columnStyles;

			// Token: 0x040036F5 RID: 14069
			private TableLayoutRowStyleCollection rowStyles;

			// Token: 0x040036F6 RID: 14070
			private Dictionary<object, TableLayoutSettings.ControlInformation> controlsInfo;

			// Token: 0x040036F7 RID: 14071
			private bool isValid = true;
		}

		// Token: 0x0200064F RID: 1615
		internal class StyleConverter : TypeConverter
		{
			// Token: 0x060054FC RID: 21756 RVA: 0x00135DE4 File Offset: 0x00134DE4
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x060054FD RID: 21757 RVA: 0x00135E00 File Offset: 0x00134E00
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw new ArgumentNullException("destinationType");
				}
				if (destinationType == typeof(InstanceDescriptor) && value is TableLayoutStyle)
				{
					TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)value;
					switch (tableLayoutStyle.SizeType)
					{
					case SizeType.AutoSize:
						return new InstanceDescriptor(tableLayoutStyle.GetType().GetConstructor(new Type[0]), new object[0]);
					case SizeType.Absolute:
					case SizeType.Percent:
						return new InstanceDescriptor(tableLayoutStyle.GetType().GetConstructor(new Type[]
						{
							typeof(SizeType),
							typeof(int)
						}), new object[]
						{
							tableLayoutStyle.SizeType,
							tableLayoutStyle.Size
						});
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
