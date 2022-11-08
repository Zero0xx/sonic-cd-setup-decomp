using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000270 RID: 624
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionCheckedListBox")]
	[ComVisible(true)]
	[LookupBindingProperties]
	public class CheckedListBox : ListBox
	{
		// Token: 0x06002179 RID: 8569 RVA: 0x000487F4 File Offset: 0x000477F4
		public CheckedListBox()
		{
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x0004881B File Offset: 0x0004781B
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x00048823 File Offset: 0x00047823
		[SRCategory("CatBehavior")]
		[SRDescription("CheckedListBoxCheckOnClickDescr")]
		[DefaultValue(false)]
		public bool CheckOnClick
		{
			get
			{
				return this.checkOnClick;
			}
			set
			{
				this.checkOnClick = value;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x0004882C File Offset: 0x0004782C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CheckedListBox.CheckedIndexCollection CheckedIndices
		{
			get
			{
				if (this.checkedIndexCollection == null)
				{
					this.checkedIndexCollection = new CheckedListBox.CheckedIndexCollection(this);
				}
				return this.checkedIndexCollection;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x00048848 File Offset: 0x00047848
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CheckedListBox.CheckedItemCollection CheckedItems
		{
			get
			{
				if (this.checkedItemCollection == null)
				{
					this.checkedItemCollection = new CheckedListBox.CheckedItemCollection(this);
				}
				return this.checkedItemCollection;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x00048864 File Offset: 0x00047864
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 1040;
				return createParams;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x0004888B File Offset: 0x0004788B
		// (set) Token: 0x06002180 RID: 8576 RVA: 0x00048893 File Offset: 0x00047893
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x0004889C File Offset: 0x0004789C
		// (set) Token: 0x06002182 RID: 8578 RVA: 0x000488A4 File Offset: 0x000478A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new string DisplayMember
		{
			get
			{
				return base.DisplayMember;
			}
			set
			{
				base.DisplayMember = value;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000488AD File Offset: 0x000478AD
		// (set) Token: 0x06002184 RID: 8580 RVA: 0x000488B0 File Offset: 0x000478B0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override DrawMode DrawMode
		{
			get
			{
				return DrawMode.Normal;
			}
			set
			{
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000488B2 File Offset: 0x000478B2
		// (set) Token: 0x06002186 RID: 8582 RVA: 0x000488C1 File Offset: 0x000478C1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int ItemHeight
		{
			get
			{
				return this.Font.Height + 2;
			}
			set
			{
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000488C3 File Offset: 0x000478C3
		[SRDescription("ListBoxItemsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[SRCategory("CatData")]
		public new CheckedListBox.ObjectCollection Items
		{
			get
			{
				return (CheckedListBox.ObjectCollection)base.Items;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000488D0 File Offset: 0x000478D0
		internal override int MaxItemWidth
		{
			get
			{
				return base.MaxItemWidth + this.idealCheckSize + 3;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000488E1 File Offset: 0x000478E1
		// (set) Token: 0x0600218A RID: 8586 RVA: 0x000488EC File Offset: 0x000478EC
		public override SelectionMode SelectionMode
		{
			get
			{
				return base.SelectionMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectionMode));
				}
				if (value != SelectionMode.One && value != SelectionMode.None)
				{
					throw new ArgumentException(SR.GetString("CheckedListBoxInvalidSelectionMode"));
				}
				if (value != this.SelectionMode)
				{
					base.SelectionMode = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x0004894C File Offset: 0x0004794C
		// (set) Token: 0x0600218C RID: 8588 RVA: 0x00048958 File Offset: 0x00047958
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("CheckedListBoxThreeDCheckBoxesDescr")]
		public bool ThreeDCheckBoxes
		{
			get
			{
				return !this.flat;
			}
			set
			{
				if (this.flat == value)
				{
					this.flat = !value;
					CheckedListBox.ObjectCollection items = this.Items;
					if (items != null && items.Count > 0)
					{
						base.Invalidate();
					}
				}
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x00048991 File Offset: 0x00047991
		// (set) Token: 0x0600218E RID: 8590 RVA: 0x00048999 File Offset: 0x00047999
		[SRDescription("UseCompatibleTextRenderingDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRenderingInt;
			}
			set
			{
				base.UseCompatibleTextRenderingInt = value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000489A2 File Offset: 0x000479A2
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000489A5 File Offset: 0x000479A5
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x000489AD File Offset: 0x000479AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string ValueMember
		{
			get
			{
				return base.ValueMember;
			}
			set
			{
				base.ValueMember = value;
			}
		}

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06002192 RID: 8594 RVA: 0x000489B6 File Offset: 0x000479B6
		// (remove) Token: 0x06002193 RID: 8595 RVA: 0x000489BF File Offset: 0x000479BF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DataSourceChanged
		{
			add
			{
				base.DataSourceChanged += value;
			}
			remove
			{
				base.DataSourceChanged -= value;
			}
		}

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06002194 RID: 8596 RVA: 0x000489C8 File Offset: 0x000479C8
		// (remove) Token: 0x06002195 RID: 8597 RVA: 0x000489D1 File Offset: 0x000479D1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DisplayMemberChanged
		{
			add
			{
				base.DisplayMemberChanged += value;
			}
			remove
			{
				base.DisplayMemberChanged -= value;
			}
		}

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06002196 RID: 8598 RVA: 0x000489DA File Offset: 0x000479DA
		// (remove) Token: 0x06002197 RID: 8599 RVA: 0x000489F3 File Offset: 0x000479F3
		[SRCategory("CatBehavior")]
		[SRDescription("CheckedListBoxItemCheckDescr")]
		public event ItemCheckEventHandler ItemCheck
		{
			add
			{
				this.onItemCheck = (ItemCheckEventHandler)Delegate.Combine(this.onItemCheck, value);
			}
			remove
			{
				this.onItemCheck = (ItemCheckEventHandler)Delegate.Remove(this.onItemCheck, value);
			}
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06002198 RID: 8600 RVA: 0x00048A0C File Offset: 0x00047A0C
		// (remove) Token: 0x06002199 RID: 8601 RVA: 0x00048A15 File Offset: 0x00047A15
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x0600219A RID: 8602 RVA: 0x00048A1E File Offset: 0x00047A1E
		// (remove) Token: 0x0600219B RID: 8603 RVA: 0x00048A27 File Offset: 0x00047A27
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x0600219C RID: 8604 RVA: 0x00048A30 File Offset: 0x00047A30
		// (remove) Token: 0x0600219D RID: 8605 RVA: 0x00048A39 File Offset: 0x00047A39
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DrawItemEventHandler DrawItem
		{
			add
			{
				base.DrawItem += value;
			}
			remove
			{
				base.DrawItem -= value;
			}
		}

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x0600219E RID: 8606 RVA: 0x00048A42 File Offset: 0x00047A42
		// (remove) Token: 0x0600219F RID: 8607 RVA: 0x00048A4B File Offset: 0x00047A4B
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MeasureItemEventHandler MeasureItem
		{
			add
			{
				base.MeasureItem += value;
			}
			remove
			{
				base.MeasureItem -= value;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x00048A54 File Offset: 0x00047A54
		// (set) Token: 0x060021A1 RID: 8609 RVA: 0x00048A5C File Offset: 0x00047A5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x060021A2 RID: 8610 RVA: 0x00048A65 File Offset: 0x00047A65
		// (remove) Token: 0x060021A3 RID: 8611 RVA: 0x00048A6E File Offset: 0x00047A6E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ValueMemberChanged
		{
			add
			{
				base.ValueMemberChanged += value;
			}
			remove
			{
				base.ValueMemberChanged -= value;
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00048A77 File Offset: 0x00047A77
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new CheckedListBox.CheckedListBoxAccessibleObject(this);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00048A7F File Offset: 0x00047A7F
		protected override ListBox.ObjectCollection CreateItemCollection()
		{
			return new CheckedListBox.ObjectCollection(this);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00048A88 File Offset: 0x00047A88
		public CheckState GetItemCheckState(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return this.CheckedItems.GetCheckedState(index);
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00048AE7 File Offset: 0x00047AE7
		public bool GetItemChecked(int index)
		{
			return this.GetItemCheckState(index) != CheckState.Unchecked;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00048AF8 File Offset: 0x00047AF8
		private void InvalidateItem(int index)
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				base.SendMessage(408, index, ref rect);
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), ref rect, false);
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00048B3C File Offset: 0x00047B3C
		private void LbnSelChange()
		{
			int selectedIndex = this.SelectedIndex;
			if (selectedIndex < 0 || selectedIndex >= this.Items.Count)
			{
				return;
			}
			base.AccessibilityNotifyClients(AccessibleEvents.Focus, selectedIndex);
			base.AccessibilityNotifyClients(AccessibleEvents.Selection, selectedIndex);
			if (!this.killnextselect && (selectedIndex == this.lastSelected || this.checkOnClick))
			{
				CheckState checkedState = this.CheckedItems.GetCheckedState(selectedIndex);
				CheckState newCheckValue = (checkedState != CheckState.Unchecked) ? CheckState.Unchecked : CheckState.Checked;
				ItemCheckEventArgs itemCheckEventArgs = new ItemCheckEventArgs(selectedIndex, newCheckValue, checkedState);
				this.OnItemCheck(itemCheckEventArgs);
				this.CheckedItems.SetCheckedState(selectedIndex, itemCheckEventArgs.NewValue);
			}
			this.lastSelected = selectedIndex;
			this.InvalidateItem(selectedIndex);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00048BD9 File Offset: 0x00047BD9
		protected override void OnClick(EventArgs e)
		{
			this.killnextselect = false;
			base.OnClick(e);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00048BE9 File Offset: 0x00047BE9
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(416, 0, this.ItemHeight);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x00048C08 File Offset: 0x00047C08
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (this.Font.Height < 0)
			{
				this.Font = Control.DefaultFont;
			}
			if (e.Index >= 0)
			{
				object item;
				if (e.Index < this.Items.Count)
				{
					item = this.Items[e.Index];
				}
				else
				{
					item = base.NativeGetItemText(e.Index);
				}
				Rectangle bounds = e.Bounds;
				int num = 1;
				int num2 = this.Font.Height + 2 * num;
				ButtonState buttonState = ButtonState.Normal;
				if (this.flat)
				{
					buttonState |= ButtonState.Flat;
				}
				if (e.Index < this.Items.Count)
				{
					switch (this.CheckedItems.GetCheckedState(e.Index))
					{
					case CheckState.Checked:
						buttonState |= ButtonState.Checked;
						break;
					case CheckState.Indeterminate:
						buttonState |= (ButtonState.Checked | ButtonState.Inactive);
						break;
					}
				}
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxState state = CheckBoxRenderer.ConvertFromButtonState(buttonState, false, (e.State & DrawItemState.HotLight) == DrawItemState.HotLight);
					this.idealCheckSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state).Width;
				}
				int num3 = Math.Max((num2 - this.idealCheckSize) / 2, 0);
				if (num3 + this.idealCheckSize > bounds.Height)
				{
					num3 = bounds.Height - this.idealCheckSize;
				}
				Rectangle rectangle = new Rectangle(bounds.X + num, bounds.Y + num3, this.idealCheckSize, this.idealCheckSize);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					rectangle.X = bounds.X + bounds.Width - this.idealCheckSize - num;
				}
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxState state2 = CheckBoxRenderer.ConvertFromButtonState(buttonState, false, (e.State & DrawItemState.HotLight) == DrawItemState.HotLight);
					CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(rectangle.X, rectangle.Y), state2);
				}
				else
				{
					ControlPaint.DrawCheckBox(e.Graphics, rectangle, buttonState);
				}
				Rectangle rectangle2 = new Rectangle(bounds.X + this.idealCheckSize + num * 2, bounds.Y, bounds.Width - (this.idealCheckSize + num * 2), bounds.Height);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					rectangle2.X = bounds.X;
				}
				string text = "";
				Color color = (this.SelectionMode != SelectionMode.None) ? e.BackColor : this.BackColor;
				Color color2 = (this.SelectionMode != SelectionMode.None) ? e.ForeColor : this.ForeColor;
				if (!base.Enabled)
				{
					color2 = SystemColors.GrayText;
				}
				Font font = this.Font;
				text = base.GetItemText(item);
				if (this.SelectionMode != SelectionMode.None && (e.State & DrawItemState.Selected) == DrawItemState.Selected)
				{
					if (base.Enabled)
					{
						color = SystemColors.Highlight;
						color2 = SystemColors.HighlightText;
					}
					else
					{
						color = SystemColors.InactiveBorder;
						color2 = SystemColors.GrayText;
					}
				}
				using (Brush brush = new SolidBrush(color))
				{
					e.Graphics.FillRectangle(brush, rectangle2);
				}
				Rectangle rectangle3 = new Rectangle(rectangle2.X + 1, rectangle2.Y, rectangle2.Width - 1, rectangle2.Height - num * 2);
				if (this.UseCompatibleTextRendering)
				{
					using (StringFormat stringFormat = new StringFormat())
					{
						if (base.UseTabStops)
						{
							float num4 = 3.6f * (float)this.Font.Height;
							float[] array = new float[15];
							float num5 = (float)(-(float)(this.idealCheckSize + num * 2));
							for (int i = 1; i < array.Length; i++)
							{
								array[i] = num4;
							}
							if (Math.Abs(num5) < num4)
							{
								array[0] = num4 + num5;
							}
							else
							{
								array[0] = num4;
							}
							stringFormat.SetTabStops(0f, array);
						}
						else if (base.UseCustomTabOffsets)
						{
							int count = base.CustomTabOffsets.Count;
							float[] array2 = new float[count];
							base.CustomTabOffsets.CopyTo(array2, 0);
							stringFormat.SetTabStops(0f, array2);
						}
						if (this.RightToLeft == RightToLeft.Yes)
						{
							stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
						}
						stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
						stringFormat.Trimming = StringTrimming.None;
						using (SolidBrush solidBrush = new SolidBrush(color2))
						{
							e.Graphics.DrawString(text, font, solidBrush, rectangle3, stringFormat);
						}
						goto IL_49B;
					}
				}
				TextFormatFlags textFormatFlags = TextFormatFlags.Default;
				textFormatFlags |= TextFormatFlags.NoPrefix;
				if (base.UseTabStops || base.UseCustomTabOffsets)
				{
					textFormatFlags |= TextFormatFlags.ExpandTabs;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					textFormatFlags |= TextFormatFlags.RightToLeft;
					textFormatFlags |= TextFormatFlags.Right;
				}
				TextRenderer.DrawText(e.Graphics, text, font, rectangle3, color2, textFormatFlags);
				IL_49B:
				if ((e.State & DrawItemState.Focus) == DrawItemState.Focus && (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
				{
					ControlPaint.DrawFocusRectangle(e.Graphics, rectangle2, color2, color);
				}
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00049130 File Offset: 0x00048130
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (base.IsHandleCreated)
			{
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), null, true);
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00049155 File Offset: 0x00048155
		protected override void OnFontChanged(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(416, 0, this.ItemHeight);
			}
			base.OnFontChanged(e);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x00049179 File Offset: 0x00048179
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ' && this.SelectionMode != SelectionMode.None)
			{
				this.LbnSelChange();
			}
			if (base.FormattingEnabled)
			{
				base.OnKeyPress(e);
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000491A2 File Offset: 0x000481A2
		protected virtual void OnItemCheck(ItemCheckEventArgs ice)
		{
			if (this.onItemCheck != null)
			{
				this.onItemCheck(this, ice);
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000491B9 File Offset: 0x000481B9
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);
			if (e.ItemHeight < this.idealCheckSize + 2)
			{
				e.ItemHeight = this.idealCheckSize + 2;
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000491E0 File Offset: 0x000481E0
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			this.lastSelected = this.SelectedIndex;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000491F8 File Offset: 0x000481F8
		protected override void RefreshItems()
		{
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < this.Items.Count; i++)
			{
				hashtable[i] = this.CheckedItems.GetCheckedState(i);
			}
			base.RefreshItems();
			for (int j = 0; j < this.Items.Count; j++)
			{
				this.CheckedItems.SetCheckedState(j, (CheckState)hashtable[j]);
			}
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00049278 File Offset: 0x00048278
		public void SetItemCheckState(int index, CheckState value)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
			{
				throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
			}
			CheckState checkedState = this.CheckedItems.GetCheckedState(index);
			if (value != checkedState)
			{
				ItemCheckEventArgs itemCheckEventArgs = new ItemCheckEventArgs(index, value, checkedState);
				this.OnItemCheck(itemCheckEventArgs);
				if (itemCheckEventArgs.NewValue != checkedState)
				{
					this.CheckedItems.SetCheckedState(index, itemCheckEventArgs.NewValue);
					this.InvalidateItem(index);
				}
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00049334 File Offset: 0x00048334
		public void SetItemChecked(int index, bool value)
		{
			this.SetItemCheckState(index, value ? CheckState.Checked : CheckState.Unchecked);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00049344 File Offset: 0x00048344
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WmReflectCommand(ref Message m)
		{
			switch (NativeMethods.Util.HIWORD(m.WParam))
			{
			case 1:
				this.LbnSelChange();
				base.WmReflectCommand(ref m);
				return;
			case 2:
				this.LbnSelChange();
				base.WmReflectCommand(ref m);
				return;
			default:
				base.WmReflectCommand(ref m);
				return;
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00049394 File Offset: 0x00048394
		private void WmReflectVKeyToItem(ref Message m)
		{
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
				this.killnextselect = true;
				break;
			default:
				this.killnextselect = false;
				break;
			}
			m.Result = NativeMethods.InvalidIntPtr;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000493F8 File Offset: 0x000483F8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
			case 8238:
				this.WmReflectVKeyToItem(ref m);
				return;
			case 8239:
				m.Result = NativeMethods.InvalidIntPtr;
				return;
			default:
				if (m.Msg == CheckedListBox.LBC_GETCHECKSTATE)
				{
					int num = (int)m.WParam;
					if (num < 0 || num >= this.Items.Count)
					{
						m.Result = (IntPtr)(-1);
						return;
					}
					m.Result = (IntPtr)(this.GetItemChecked(num) ? 1 : 0);
					return;
				}
				else
				{
					if (m.Msg != CheckedListBox.LBC_SETCHECKSTATE)
					{
						base.WndProc(ref m);
						return;
					}
					int num2 = (int)m.WParam;
					int num3 = (int)m.LParam;
					if (num2 < 0 || num2 >= this.Items.Count || (num3 != 1 && num3 != 0))
					{
						m.Result = IntPtr.Zero;
						return;
					}
					this.SetItemChecked(num2, num3 == 1);
					m.Result = (IntPtr)1;
					return;
				}
				break;
			}
		}

		// Token: 0x040014D1 RID: 5329
		private const int LB_CHECKED = 1;

		// Token: 0x040014D2 RID: 5330
		private const int LB_UNCHECKED = 0;

		// Token: 0x040014D3 RID: 5331
		private const int LB_ERROR = -1;

		// Token: 0x040014D4 RID: 5332
		private int idealCheckSize = 13;

		// Token: 0x040014D5 RID: 5333
		private bool killnextselect;

		// Token: 0x040014D6 RID: 5334
		private ItemCheckEventHandler onItemCheck;

		// Token: 0x040014D7 RID: 5335
		private bool checkOnClick;

		// Token: 0x040014D8 RID: 5336
		private bool flat = true;

		// Token: 0x040014D9 RID: 5337
		private int lastSelected = -1;

		// Token: 0x040014DA RID: 5338
		private CheckedListBox.CheckedItemCollection checkedItemCollection;

		// Token: 0x040014DB RID: 5339
		private CheckedListBox.CheckedIndexCollection checkedIndexCollection;

		// Token: 0x040014DC RID: 5340
		private static int LBC_GETCHECKSTATE = SafeNativeMethods.RegisterWindowMessage("LBC_GETCHECKSTATE");

		// Token: 0x040014DD RID: 5341
		private static int LBC_SETCHECKSTATE = SafeNativeMethods.RegisterWindowMessage("LBC_SETCHECKSTATE");

		// Token: 0x02000271 RID: 625
		public new class ObjectCollection : ListBox.ObjectCollection
		{
			// Token: 0x060021B9 RID: 8633 RVA: 0x000494F0 File Offset: 0x000484F0
			public ObjectCollection(CheckedListBox owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x060021BA RID: 8634 RVA: 0x00049500 File Offset: 0x00048500
			public int Add(object item, bool isChecked)
			{
				return this.Add(item, isChecked ? CheckState.Checked : CheckState.Unchecked);
			}

			// Token: 0x060021BB RID: 8635 RVA: 0x00049510 File Offset: 0x00048510
			public int Add(object item, CheckState check)
			{
				if (!ClientUtils.IsEnumValid(check, (int)check, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)check, typeof(CheckState));
				}
				int num = base.Add(item);
				this.owner.SetItemCheckState(num, check);
				return num;
			}

			// Token: 0x040014DE RID: 5342
			private CheckedListBox owner;
		}

		// Token: 0x02000272 RID: 626
		public class CheckedIndexCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060021BC RID: 8636 RVA: 0x00049559 File Offset: 0x00048559
			internal CheckedIndexCollection(CheckedListBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x060021BD RID: 8637 RVA: 0x00049568 File Offset: 0x00048568
			public int Count
			{
				get
				{
					return this.owner.CheckedItems.Count;
				}
			}

			// Token: 0x170004FC RID: 1276
			// (get) Token: 0x060021BE RID: 8638 RVA: 0x0004957A File Offset: 0x0004857A
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x060021BF RID: 8639 RVA: 0x0004957D File Offset: 0x0004857D
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x060021C0 RID: 8640 RVA: 0x00049580 File Offset: 0x00048580
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x060021C1 RID: 8641 RVA: 0x00049583 File Offset: 0x00048583
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000500 RID: 1280
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			public int this[int index]
			{
				get
				{
					object entryObject = this.InnerArray.GetEntryObject(index, CheckedListBox.CheckedItemCollection.AnyMask);
					return this.InnerArray.IndexOfIdentifier(entryObject, 0);
				}
			}

			// Token: 0x17000501 RID: 1281
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
				}
			}

			// Token: 0x060021C5 RID: 8645 RVA: 0x000495D3 File Offset: 0x000485D3
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			// Token: 0x060021C6 RID: 8646 RVA: 0x000495E4 File Offset: 0x000485E4
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			// Token: 0x060021C7 RID: 8647 RVA: 0x000495F5 File Offset: 0x000485F5
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			// Token: 0x060021C8 RID: 8648 RVA: 0x00049606 File Offset: 0x00048606
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			// Token: 0x060021C9 RID: 8649 RVA: 0x00049617 File Offset: 0x00048617
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			// Token: 0x060021CA RID: 8650 RVA: 0x00049628 File Offset: 0x00048628
			public bool Contains(int index)
			{
				return this.IndexOf(index) != -1;
			}

			// Token: 0x060021CB RID: 8651 RVA: 0x00049637 File Offset: 0x00048637
			bool IList.Contains(object index)
			{
				return index is int && this.Contains((int)index);
			}

			// Token: 0x060021CC RID: 8652 RVA: 0x00049650 File Offset: 0x00048650
			public void CopyTo(Array dest, int index)
			{
				int count = this.owner.CheckedItems.Count;
				for (int i = 0; i < count; i++)
				{
					dest.SetValue(this[i], i + index);
				}
			}

			// Token: 0x17000502 RID: 1282
			// (get) Token: 0x060021CD RID: 8653 RVA: 0x0004968F File Offset: 0x0004868F
			private ListBox.ItemArray InnerArray
			{
				get
				{
					return this.owner.Items.InnerArray;
				}
			}

			// Token: 0x060021CE RID: 8654 RVA: 0x000496A4 File Offset: 0x000486A4
			public IEnumerator GetEnumerator()
			{
				int[] array = new int[this.Count];
				this.CopyTo(array, 0);
				return array.GetEnumerator();
			}

			// Token: 0x060021CF RID: 8655 RVA: 0x000496CC File Offset: 0x000486CC
			public int IndexOf(int index)
			{
				if (index >= 0 && index < this.owner.Items.Count)
				{
					object entryObject = this.InnerArray.GetEntryObject(index, 0);
					return this.owner.CheckedItems.IndexOfIdentifier(entryObject);
				}
				return -1;
			}

			// Token: 0x060021D0 RID: 8656 RVA: 0x00049711 File Offset: 0x00048711
			int IList.IndexOf(object index)
			{
				if (index is int)
				{
					return this.IndexOf((int)index);
				}
				return -1;
			}

			// Token: 0x040014DF RID: 5343
			private CheckedListBox owner;
		}

		// Token: 0x02000273 RID: 627
		public class CheckedItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060021D1 RID: 8657 RVA: 0x00049729 File Offset: 0x00048729
			internal CheckedItemCollection(CheckedListBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000503 RID: 1283
			// (get) Token: 0x060021D2 RID: 8658 RVA: 0x00049738 File Offset: 0x00048738
			public int Count
			{
				get
				{
					return this.InnerArray.GetCount(CheckedListBox.CheckedItemCollection.AnyMask);
				}
			}

			// Token: 0x17000504 RID: 1284
			// (get) Token: 0x060021D3 RID: 8659 RVA: 0x0004974A File Offset: 0x0004874A
			private ListBox.ItemArray InnerArray
			{
				get
				{
					return this.owner.Items.InnerArray;
				}
			}

			// Token: 0x17000505 RID: 1285
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public object this[int index]
			{
				get
				{
					return this.InnerArray.GetItem(index, CheckedListBox.CheckedItemCollection.AnyMask);
				}
				set
				{
					throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
				}
			}

			// Token: 0x17000506 RID: 1286
			// (get) Token: 0x060021D6 RID: 8662 RVA: 0x00049780 File Offset: 0x00048780
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000507 RID: 1287
			// (get) Token: 0x060021D7 RID: 8663 RVA: 0x00049783 File Offset: 0x00048783
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00049786 File Offset: 0x00048786
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x060021D9 RID: 8665 RVA: 0x00049789 File Offset: 0x00048789
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060021DA RID: 8666 RVA: 0x0004978C File Offset: 0x0004878C
			public bool Contains(object item)
			{
				return this.IndexOf(item) != -1;
			}

			// Token: 0x060021DB RID: 8667 RVA: 0x0004979B File Offset: 0x0004879B
			public int IndexOf(object item)
			{
				return this.InnerArray.IndexOf(item, CheckedListBox.CheckedItemCollection.AnyMask);
			}

			// Token: 0x060021DC RID: 8668 RVA: 0x000497AE File Offset: 0x000487AE
			internal int IndexOfIdentifier(object item)
			{
				return this.InnerArray.IndexOfIdentifier(item, CheckedListBox.CheckedItemCollection.AnyMask);
			}

			// Token: 0x060021DD RID: 8669 RVA: 0x000497C1 File Offset: 0x000487C1
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			// Token: 0x060021DE RID: 8670 RVA: 0x000497D2 File Offset: 0x000487D2
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			// Token: 0x060021DF RID: 8671 RVA: 0x000497E3 File Offset: 0x000487E3
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			// Token: 0x060021E0 RID: 8672 RVA: 0x000497F4 File Offset: 0x000487F4
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			// Token: 0x060021E1 RID: 8673 RVA: 0x00049805 File Offset: 0x00048805
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			// Token: 0x060021E2 RID: 8674 RVA: 0x00049818 File Offset: 0x00048818
			public void CopyTo(Array dest, int index)
			{
				int count = this.InnerArray.GetCount(CheckedListBox.CheckedItemCollection.AnyMask);
				for (int i = 0; i < count; i++)
				{
					dest.SetValue(this.InnerArray.GetItem(i, CheckedListBox.CheckedItemCollection.AnyMask), i + index);
				}
			}

			// Token: 0x060021E3 RID: 8675 RVA: 0x0004985C File Offset: 0x0004885C
			internal CheckState GetCheckedState(int index)
			{
				bool state = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.CheckedItemMask);
				bool state2 = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.IndeterminateItemMask);
				if (state2)
				{
					return CheckState.Indeterminate;
				}
				if (state)
				{
					return CheckState.Checked;
				}
				return CheckState.Unchecked;
			}

			// Token: 0x060021E4 RID: 8676 RVA: 0x00049898 File Offset: 0x00048898
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator(CheckedListBox.CheckedItemCollection.AnyMask, true);
			}

			// Token: 0x060021E5 RID: 8677 RVA: 0x000498AC File Offset: 0x000488AC
			internal void SetCheckedState(int index, CheckState value)
			{
				bool flag;
				bool flag2;
				switch (value)
				{
				case CheckState.Checked:
					flag = true;
					flag2 = false;
					break;
				case CheckState.Indeterminate:
					flag = false;
					flag2 = true;
					break;
				default:
					flag = false;
					flag2 = false;
					break;
				}
				bool state = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.CheckedItemMask);
				bool state2 = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.IndeterminateItemMask);
				this.InnerArray.SetState(index, CheckedListBox.CheckedItemCollection.CheckedItemMask, flag);
				this.InnerArray.SetState(index, CheckedListBox.CheckedItemCollection.IndeterminateItemMask, flag2);
				if (state != flag || state2 != flag2)
				{
					this.owner.AccessibilityNotifyClients(AccessibleEvents.StateChange, index);
				}
			}

			// Token: 0x040014E0 RID: 5344
			internal static int CheckedItemMask = ListBox.ItemArray.CreateMask();

			// Token: 0x040014E1 RID: 5345
			internal static int IndeterminateItemMask = ListBox.ItemArray.CreateMask();

			// Token: 0x040014E2 RID: 5346
			internal static int AnyMask = CheckedListBox.CheckedItemCollection.CheckedItemMask | CheckedListBox.CheckedItemCollection.IndeterminateItemMask;

			// Token: 0x040014E3 RID: 5347
			private CheckedListBox owner;
		}

		// Token: 0x02000274 RID: 628
		[ComVisible(true)]
		internal class CheckedListBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x060021E7 RID: 8679 RVA: 0x00049966 File Offset: 0x00048966
			public CheckedListBoxAccessibleObject(CheckedListBox owner) : base(owner)
			{
			}

			// Token: 0x1700050A RID: 1290
			// (get) Token: 0x060021E8 RID: 8680 RVA: 0x0004996F File Offset: 0x0004896F
			private CheckedListBox CheckedListBox
			{
				get
				{
					return (CheckedListBox)base.Owner;
				}
			}

			// Token: 0x060021E9 RID: 8681 RVA: 0x0004997C File Offset: 0x0004897C
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < this.CheckedListBox.Items.Count)
				{
					return new CheckedListBox.CheckedListBoxItemAccessibleObject(this.CheckedListBox.GetItemText(this.CheckedListBox.Items[index]), index, this);
				}
				return null;
			}

			// Token: 0x060021EA RID: 8682 RVA: 0x000499BA File Offset: 0x000489BA
			public override int GetChildCount()
			{
				return this.CheckedListBox.Items.Count;
			}

			// Token: 0x060021EB RID: 8683 RVA: 0x000499CC File Offset: 0x000489CC
			public override AccessibleObject GetFocused()
			{
				int focusedIndex = this.CheckedListBox.FocusedIndex;
				if (focusedIndex >= 0)
				{
					return this.GetChild(focusedIndex);
				}
				return null;
			}

			// Token: 0x060021EC RID: 8684 RVA: 0x000499F4 File Offset: 0x000489F4
			public override AccessibleObject GetSelected()
			{
				int selectedIndex = this.CheckedListBox.SelectedIndex;
				if (selectedIndex >= 0)
				{
					return this.GetChild(selectedIndex);
				}
				return null;
			}

			// Token: 0x060021ED RID: 8685 RVA: 0x00049A1C File Offset: 0x00048A1C
			public override AccessibleObject HitTest(int x, int y)
			{
				int childCount = this.GetChildCount();
				for (int i = 0; i < childCount; i++)
				{
					AccessibleObject child = this.GetChild(i);
					if (child.Bounds.Contains(x, y))
					{
						return child;
					}
				}
				if (this.Bounds.Contains(x, y))
				{
					return this;
				}
				return null;
			}

			// Token: 0x060021EE RID: 8686 RVA: 0x00049A6E File Offset: 0x00048A6E
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation direction)
			{
				if (this.GetChildCount() > 0)
				{
					if (direction == AccessibleNavigation.FirstChild)
					{
						return this.GetChild(0);
					}
					if (direction == AccessibleNavigation.LastChild)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
				}
				return base.Navigate(direction);
			}
		}

		// Token: 0x02000275 RID: 629
		[ComVisible(true)]
		internal class CheckedListBoxItemAccessibleObject : AccessibleObject
		{
			// Token: 0x060021EF RID: 8687 RVA: 0x00049A9F File Offset: 0x00048A9F
			public CheckedListBoxItemAccessibleObject(string name, int index, CheckedListBox.CheckedListBoxAccessibleObject parent)
			{
				this.name = name;
				this.parent = parent;
				this.index = index;
			}

			// Token: 0x1700050B RID: 1291
			// (get) Token: 0x060021F0 RID: 8688 RVA: 0x00049ABC File Offset: 0x00048ABC
			public override Rectangle Bounds
			{
				get
				{
					Rectangle itemRectangle = this.ParentCheckedListBox.GetItemRectangle(this.index);
					NativeMethods.POINT point = new NativeMethods.POINT(itemRectangle.X, itemRectangle.Y);
					UnsafeNativeMethods.ClientToScreen(new HandleRef(this.ParentCheckedListBox, this.ParentCheckedListBox.Handle), point);
					return new Rectangle(point.x, point.y, itemRectangle.Width, itemRectangle.Height);
				}
			}

			// Token: 0x1700050C RID: 1292
			// (get) Token: 0x060021F1 RID: 8689 RVA: 0x00049B2B File Offset: 0x00048B2B
			public override string DefaultAction
			{
				get
				{
					if (this.ParentCheckedListBox.GetItemChecked(this.index))
					{
						return SR.GetString("AccessibleActionUncheck");
					}
					return SR.GetString("AccessibleActionCheck");
				}
			}

			// Token: 0x1700050D RID: 1293
			// (get) Token: 0x060021F2 RID: 8690 RVA: 0x00049B55 File Offset: 0x00048B55
			private CheckedListBox ParentCheckedListBox
			{
				get
				{
					return (CheckedListBox)this.parent.Owner;
				}
			}

			// Token: 0x1700050E RID: 1294
			// (get) Token: 0x060021F3 RID: 8691 RVA: 0x00049B67 File Offset: 0x00048B67
			// (set) Token: 0x060021F4 RID: 8692 RVA: 0x00049B6F File Offset: 0x00048B6F
			public override string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x060021F5 RID: 8693 RVA: 0x00049B78 File Offset: 0x00048B78
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.parent;
				}
			}

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x060021F6 RID: 8694 RVA: 0x00049B80 File Offset: 0x00048B80
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.CheckButton;
				}
			}

			// Token: 0x17000511 RID: 1297
			// (get) Token: 0x060021F7 RID: 8695 RVA: 0x00049B84 File Offset: 0x00048B84
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					switch (this.ParentCheckedListBox.GetItemCheckState(this.index))
					{
					case CheckState.Checked:
						accessibleStates |= AccessibleStates.Checked;
						break;
					case CheckState.Indeterminate:
						accessibleStates |= AccessibleStates.Mixed;
						break;
					}
					if (this.ParentCheckedListBox.SelectedIndex == this.index)
					{
						accessibleStates |= (AccessibleStates.Selected | AccessibleStates.Focused);
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000512 RID: 1298
			// (get) Token: 0x060021F8 RID: 8696 RVA: 0x00049BE4 File Offset: 0x00048BE4
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentCheckedListBox.GetItemChecked(this.index).ToString();
				}
			}

			// Token: 0x060021F9 RID: 8697 RVA: 0x00049C0A File Offset: 0x00048C0A
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.ParentCheckedListBox.SetItemChecked(this.index, !this.ParentCheckedListBox.GetItemChecked(this.index));
			}

			// Token: 0x060021FA RID: 8698 RVA: 0x00049C34 File Offset: 0x00048C34
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation direction)
			{
				if ((direction == AccessibleNavigation.Down || direction == AccessibleNavigation.Next) && this.index < this.parent.GetChildCount() - 1)
				{
					return this.parent.GetChild(this.index + 1);
				}
				if ((direction == AccessibleNavigation.Up || direction == AccessibleNavigation.Previous) && this.index > 0)
				{
					return this.parent.GetChild(this.index - 1);
				}
				return base.Navigate(direction);
			}

			// Token: 0x060021FB RID: 8699 RVA: 0x00049CA0 File Offset: 0x00048CA0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				try
				{
					this.ParentCheckedListBox.AccessibilityObject.GetSystemIAccessibleInternal().accSelect((int)flags, this.index + 1);
				}
				catch (ArgumentException)
				{
				}
			}

			// Token: 0x040014E4 RID: 5348
			private string name;

			// Token: 0x040014E5 RID: 5349
			private int index;

			// Token: 0x040014E6 RID: 5350
			private CheckedListBox.CheckedListBoxAccessibleObject parent;
		}
	}
}
