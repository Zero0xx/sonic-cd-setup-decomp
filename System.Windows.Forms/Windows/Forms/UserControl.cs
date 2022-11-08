using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200071F RID: 1823
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignerCategory("UserControl")]
	[DefaultEvent("Load")]
	public class UserControl : ContainerControl
	{
		// Token: 0x060060AA RID: 24746 RVA: 0x001623A0 File Offset: 0x001613A0
		public UserControl()
		{
			base.SetScrollState(1, false);
			base.SetState(2, true);
			base.SetState(524288, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x060060AB RID: 24747 RVA: 0x001623D0 File Offset: 0x001613D0
		// (set) Token: 0x060060AC RID: 24748 RVA: 0x001623D8 File Offset: 0x001613D8
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x140003B2 RID: 946
		// (add) Token: 0x060060AD RID: 24749 RVA: 0x001623E1 File Offset: 0x001613E1
		// (remove) Token: 0x060060AE RID: 24750 RVA: 0x001623EA File Offset: 0x001613EA
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x001623F3 File Offset: 0x001613F3
		// (set) Token: 0x060060B0 RID: 24752 RVA: 0x001623FC File Offset: 0x001613FC
		[Browsable(true)]
		[SRCategory("CatLayout")]
		[SRDescription("ControlAutoSizeModeDescr")]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[Localizable(true)]
		public AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					Control control = (base.DesignMode || this.ParentInternal == null) ? this : this.ParentInternal;
					if (control != null)
					{
						if (control.LayoutEngine == DefaultLayout.Instance)
						{
							control.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(control, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x060060B1 RID: 24753 RVA: 0x00162483 File Offset: 0x00161483
		// (set) Token: 0x060060B2 RID: 24754 RVA: 0x0016248B File Offset: 0x0016148B
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		// Token: 0x140003B3 RID: 947
		// (add) Token: 0x060060B3 RID: 24755 RVA: 0x00162494 File Offset: 0x00161494
		// (remove) Token: 0x060060B4 RID: 24756 RVA: 0x0016249D File Offset: 0x0016149D
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler AutoValidateChanged
		{
			add
			{
				base.AutoValidateChanged += value;
			}
			remove
			{
				base.AutoValidateChanged -= value;
			}
		}

		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x060060B5 RID: 24757 RVA: 0x001624A6 File Offset: 0x001614A6
		// (set) Token: 0x060060B6 RID: 24758 RVA: 0x001624AE File Offset: 0x001614AE
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.None)]
		[SRDescription("UserControlBorderStyleDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x060060B7 RID: 24759 RVA: 0x001624EC File Offset: 0x001614EC
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 65536;
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x060060B8 RID: 24760 RVA: 0x00162576 File Offset: 0x00161576
		protected override Size DefaultSize
		{
			get
			{
				return new Size(150, 150);
			}
		}

		// Token: 0x140003B4 RID: 948
		// (add) Token: 0x060060B9 RID: 24761 RVA: 0x00162587 File Offset: 0x00161587
		// (remove) Token: 0x060060BA RID: 24762 RVA: 0x0016259A File Offset: 0x0016159A
		[SRDescription("UserControlOnLoadDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler Load
		{
			add
			{
				base.Events.AddHandler(UserControl.EVENT_LOAD, value);
			}
			remove
			{
				base.Events.RemoveHandler(UserControl.EVENT_LOAD, value);
			}
		}

		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x060060BB RID: 24763 RVA: 0x001625AD File Offset: 0x001615AD
		// (set) Token: 0x060060BC RID: 24764 RVA: 0x001625B5 File Offset: 0x001615B5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140003B5 RID: 949
		// (add) Token: 0x060060BD RID: 24765 RVA: 0x001625BE File Offset: 0x001615BE
		// (remove) Token: 0x060060BE RID: 24766 RVA: 0x001625C7 File Offset: 0x001615C7
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

		// Token: 0x060060BF RID: 24767 RVA: 0x001625D0 File Offset: 0x001615D0
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool ValidateChildren()
		{
			return base.ValidateChildren();
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x001625D8 File Offset: 0x001615D8
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			return base.ValidateChildren(validationConstraints);
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x001625E4 File Offset: 0x001615E4
		private bool FocusInside()
		{
			if (!base.IsHandleCreated)
			{
				return false;
			}
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			if (focus == IntPtr.Zero)
			{
				return false;
			}
			IntPtr handle = base.Handle;
			return handle == focus || SafeNativeMethods.IsChild(new HandleRef(this, handle), new HandleRef(null, focus));
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x00162638 File Offset: 0x00161638
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.OnLoad(EventArgs.Empty);
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x0016264C File Offset: 0x0016164C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[UserControl.EVENT_LOAD];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0016267A File Offset: 0x0016167A
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.BackgroundImage != null)
			{
				base.Invalidate();
			}
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x00162691 File Offset: 0x00161691
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.FocusInside())
			{
				this.FocusInternal();
			}
			base.OnMouseDown(e);
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x001626AC File Offset: 0x001616AC
		private void WmSetFocus(ref Message m)
		{
			if (!base.HostedInWin32DialogManager)
			{
				IntSecurity.ModifyFocus.Assert();
				try
				{
					if (base.ActiveControl == null)
					{
						base.SelectNextControl(null, true, true, true, false);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			if (!base.ValidationCancelled)
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x00162708 File Offset: 0x00161708
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 7)
			{
				this.WmSetFocus(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x04003A9B RID: 15003
		private static readonly object EVENT_LOAD = new object();

		// Token: 0x04003A9C RID: 15004
		private BorderStyle borderStyle;
	}
}
