using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020005DE RID: 1502
	[SRDescription("DescriptionRadioButton")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("CheckedChanged")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Checked")]
	[Designer("System.Windows.Forms.Design.RadioButtonDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultBindingProperty("Checked")]
	[ComVisible(true)]
	public class RadioButton : ButtonBase
	{
		// Token: 0x06004E60 RID: 20064 RVA: 0x001210EC File Offset: 0x001200EC
		public RadioButton()
		{
			base.SetStyle(ControlStyles.StandardClick, false);
			this.TextAlign = ContentAlignment.MiddleLeft;
			this.TabStop = false;
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06004E61 RID: 20065 RVA: 0x0012112C File Offset: 0x0012012C
		// (set) Token: 0x06004E62 RID: 20066 RVA: 0x00121134 File Offset: 0x00120134
		[SRDescription("RadioButtonAutoCheckDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool AutoCheck
		{
			get
			{
				return this.autoCheck;
			}
			set
			{
				if (this.autoCheck != value)
				{
					this.autoCheck = value;
					this.PerformAutoUpdates(false);
				}
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x0012114D File Offset: 0x0012014D
		// (set) Token: 0x06004E64 RID: 20068 RVA: 0x00121158 File Offset: 0x00120158
		[SRDescription("RadioButtonAppearanceDescr")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(Appearance.Normal)]
		public Appearance Appearance
		{
			get
			{
				return this.appearance;
			}
			set
			{
				if (this.appearance != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(Appearance));
					}
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Appearance))
					{
						this.appearance = value;
						if (base.OwnerDraw)
						{
							this.Refresh();
						}
						else
						{
							base.UpdateStyles();
						}
						this.OnAppearanceChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x140002DD RID: 733
		// (add) Token: 0x06004E65 RID: 20069 RVA: 0x001211F0 File Offset: 0x001201F0
		// (remove) Token: 0x06004E66 RID: 20070 RVA: 0x00121203 File Offset: 0x00120203
		[SRDescription("RadioButtonOnAppearanceChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler AppearanceChanged
		{
			add
			{
				base.Events.AddHandler(RadioButton.EVENT_APPEARANCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(RadioButton.EVENT_APPEARANCECHANGED, value);
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06004E67 RID: 20071 RVA: 0x00121216 File Offset: 0x00120216
		// (set) Token: 0x06004E68 RID: 20072 RVA: 0x0012121E File Offset: 0x0012021E
		[SRDescription("RadioButtonCheckAlignDescr")]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public ContentAlignment CheckAlign
		{
			get
			{
				return this.checkAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				this.checkAlign = value;
				if (base.OwnerDraw)
				{
					base.Invalidate();
					return;
				}
				base.UpdateStyles();
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06004E69 RID: 20073 RVA: 0x0012125A File Offset: 0x0012025A
		// (set) Token: 0x06004E6A RID: 20074 RVA: 0x00121264 File Offset: 0x00120264
		[SettingsBindable(true)]
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("RadioButtonCheckedDescr")]
		public bool Checked
		{
			get
			{
				return this.isChecked;
			}
			set
			{
				if (this.isChecked != value)
				{
					this.isChecked = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(241, value ? 1 : 0, 0);
					}
					base.Invalidate();
					base.Update();
					this.PerformAutoUpdates(false);
					this.OnCheckedChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140002DE RID: 734
		// (add) Token: 0x06004E6B RID: 20075 RVA: 0x001212BB File Offset: 0x001202BB
		// (remove) Token: 0x06004E6C RID: 20076 RVA: 0x001212C4 File Offset: 0x001202C4
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

		// Token: 0x140002DF RID: 735
		// (add) Token: 0x06004E6D RID: 20077 RVA: 0x001212CD File Offset: 0x001202CD
		// (remove) Token: 0x06004E6E RID: 20078 RVA: 0x001212D6 File Offset: 0x001202D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x001212E0 File Offset: 0x001202E0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "BUTTON";
				if (base.OwnerDraw)
				{
					createParams.Style |= 11;
				}
				else
				{
					createParams.Style |= 4;
					if (this.Appearance == Appearance.Button)
					{
						createParams.Style |= 4096;
					}
					ContentAlignment contentAlignment = base.RtlTranslateContent(this.CheckAlign);
					if ((contentAlignment & RadioButton.anyRight) != (ContentAlignment)0)
					{
						createParams.Style |= 32;
					}
				}
				return createParams;
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x00121367 File Offset: 0x00120367
		protected override Size DefaultSize
		{
			get
			{
				return new Size(104, 24);
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x00121374 File Offset: 0x00120374
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			if (base.FlatStyle != FlatStyle.System)
			{
				return base.GetPreferredSizeCore(proposedConstraints);
			}
			Size clientSize = TextRenderer.MeasureText(this.Text, this.Font);
			Size result = this.SizeFromClientSize(clientSize);
			result.Width += 24;
			result.Height += 5;
			return result;
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x001213CC File Offset: 0x001203CC
		internal override Rectangle OverChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button)
				{
					return base.OverChangeRectangle;
				}
				if (base.FlatStyle == FlatStyle.Standard)
				{
					return new Rectangle(-1, -1, 1, 1);
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x00121406 File Offset: 0x00120406
		internal override Rectangle DownChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button || base.FlatStyle == FlatStyle.System)
				{
					return base.DownChangeRectangle;
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x00121436 File Offset: 0x00120436
		// (set) Token: 0x06004E75 RID: 20085 RVA: 0x0012143E File Offset: 0x0012043E
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x00121447 File Offset: 0x00120447
		// (set) Token: 0x06004E77 RID: 20087 RVA: 0x0012144F File Offset: 0x0012044F
		[Localizable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		public override ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		// Token: 0x140002E0 RID: 736
		// (add) Token: 0x06004E78 RID: 20088 RVA: 0x00121458 File Offset: 0x00120458
		// (remove) Token: 0x06004E79 RID: 20089 RVA: 0x0012146B File Offset: 0x0012046B
		[SRDescription("RadioButtonOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(RadioButton.EVENT_CHECKEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(RadioButton.EVENT_CHECKEDCHANGED, value);
			}
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x0012147E File Offset: 0x0012047E
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new RadioButton.RadioButtonAccessibleObject(this);
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x00121486 File Offset: 0x00120486
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (base.IsHandleCreated)
			{
				base.SendMessage(241, this.isChecked ? 1 : 0, 0);
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001214B0 File Offset: 0x001204B0
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
			EventHandler eventHandler = (EventHandler)base.Events[RadioButton.EVENT_CHECKEDCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x001214F6 File Offset: 0x001204F6
		protected override void OnClick(EventArgs e)
		{
			if (this.autoCheck)
			{
				this.Checked = true;
			}
			base.OnClick(e);
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0012150E File Offset: 0x0012050E
		protected override void OnEnter(EventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.None)
			{
				if (UnsafeNativeMethods.GetKeyState(9) >= 0)
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						this.OnClick(e);
					}
				}
				else
				{
					this.PerformAutoUpdates(true);
					this.TabStop = true;
				}
			}
			base.OnEnter(e);
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x00121550 File Offset: 0x00120550
		private void PerformAutoUpdates(bool tabbedInto)
		{
			if (this.autoCheck)
			{
				if (this.firstfocus)
				{
					this.WipeTabStops(tabbedInto);
				}
				this.TabStop = this.isChecked;
				if (this.isChecked)
				{
					Control parentInternal = this.ParentInternal;
					if (parentInternal != null)
					{
						Control.ControlCollection controls = parentInternal.Controls;
						for (int i = 0; i < controls.Count; i++)
						{
							Control control = controls[i];
							if (control != this && control is RadioButton)
							{
								RadioButton radioButton = (RadioButton)control;
								if (radioButton.autoCheck && radioButton.Checked)
								{
									PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["Checked"];
									propertyDescriptor.SetValue(radioButton, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001215FC File Offset: 0x001205FC
		private void WipeTabStops(bool tabbedInto)
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				Control.ControlCollection controls = parentInternal.Controls;
				for (int i = 0; i < controls.Count; i++)
				{
					Control control = controls[i];
					if (control is RadioButton)
					{
						RadioButton radioButton = (RadioButton)control;
						if (!tabbedInto)
						{
							radioButton.firstfocus = false;
						}
						if (radioButton.autoCheck)
						{
							radioButton.TabStop = false;
						}
					}
				}
			}
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0012165F File Offset: 0x0012065F
		internal override ButtonBaseAdapter CreateFlatAdapter()
		{
			return new RadioButtonFlatAdapter(this);
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x00121667 File Offset: 0x00120667
		internal override ButtonBaseAdapter CreatePopupAdapter()
		{
			return new RadioButtonPopupAdapter(this);
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0012166F File Offset: 0x0012066F
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new RadioButtonStandardAdapter(this);
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x00121678 File Offset: 0x00120678
		private void OnAppearanceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[RadioButton.EVENT_APPEARANCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x001216A8 File Offset: 0x001206A8
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left && base.GetStyle(ControlStyles.UserPaint) && base.MouseIsDown)
			{
				Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
				if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						this.OnClick(mevent);
						this.OnMouseClick(mevent);
					}
				}
			}
			base.OnMouseUp(mevent);
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0012172E File Offset: 0x0012072E
		public void PerformClick()
		{
			if (base.CanSelect)
			{
				base.ResetFlagsandPaint();
				if (!base.ValidationCancelled)
				{
					this.OnClick(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00121751 File Offset: 0x00120751
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.UseMnemonic && Control.IsMnemonic(charCode, this.Text) && base.CanSelect)
			{
				if (!this.Focused)
				{
					this.FocusInternal();
				}
				else
				{
					this.PerformClick();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0012178C File Offset: 0x0012078C
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Checked: " + this.Checked.ToString();
		}

		// Token: 0x040032BB RID: 12987
		private static readonly object EVENT_CHECKEDCHANGED = new object();

		// Token: 0x040032BC RID: 12988
		private static readonly ContentAlignment anyRight = (ContentAlignment)1092;

		// Token: 0x040032BD RID: 12989
		private bool firstfocus = true;

		// Token: 0x040032BE RID: 12990
		private bool isChecked;

		// Token: 0x040032BF RID: 12991
		private bool autoCheck = true;

		// Token: 0x040032C0 RID: 12992
		private ContentAlignment checkAlign = ContentAlignment.MiddleLeft;

		// Token: 0x040032C1 RID: 12993
		private Appearance appearance;

		// Token: 0x040032C2 RID: 12994
		private static readonly object EVENT_APPEARANCECHANGED = new object();

		// Token: 0x020005DF RID: 1503
		[ComVisible(true)]
		public class RadioButtonAccessibleObject : ButtonBase.ButtonBaseAccessibleObject
		{
			// Token: 0x06004E8A RID: 20106 RVA: 0x001217D9 File Offset: 0x001207D9
			public RadioButtonAccessibleObject(RadioButton owner) : base(owner)
			{
			}

			// Token: 0x17000FF0 RID: 4080
			// (get) Token: 0x06004E8B RID: 20107 RVA: 0x001217E4 File Offset: 0x001207E4
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = base.Owner.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					return SR.GetString("AccessibleActionCheck");
				}
			}

			// Token: 0x17000FF1 RID: 4081
			// (get) Token: 0x06004E8C RID: 20108 RVA: 0x0012180C File Offset: 0x0012080C
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.RadioButton;
				}
			}

			// Token: 0x17000FF2 RID: 4082
			// (get) Token: 0x06004E8D RID: 20109 RVA: 0x0012182D File Offset: 0x0012082D
			public override AccessibleStates State
			{
				get
				{
					if (((RadioButton)base.Owner).Checked)
					{
						return AccessibleStates.Checked | base.State;
					}
					return base.State;
				}
			}

			// Token: 0x06004E8E RID: 20110 RVA: 0x00121851 File Offset: 0x00120851
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				((RadioButton)base.Owner).PerformClick();
			}
		}
	}
}
