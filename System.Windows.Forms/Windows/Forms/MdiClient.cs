using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020004A3 RID: 1187
	[ToolboxItem(false)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[DesignTimeVisible(false)]
	public sealed class MdiClient : Control
	{
		// Token: 0x0600472C RID: 18220 RVA: 0x00102404 File Offset: 0x00101404
		public MdiClient()
		{
			base.SetStyle(ControlStyles.Selectable, false);
			this.BackColor = SystemColors.AppWorkspace;
			this.Dock = DockStyle.Fill;
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x0600472D RID: 18221 RVA: 0x00102438 File Offset: 0x00101438
		// (set) Token: 0x0600472E RID: 18222 RVA: 0x00102464 File Offset: 0x00101464
		[Localizable(true)]
		public override Image BackgroundImage
		{
			get
			{
				Image backgroundImage = base.BackgroundImage;
				if (backgroundImage == null && this.ParentInternal != null)
				{
					backgroundImage = this.ParentInternal.BackgroundImage;
				}
				return backgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x0600472F RID: 18223 RVA: 0x00102470 File Offset: 0x00101470
		// (set) Token: 0x06004730 RID: 18224 RVA: 0x001024B6 File Offset: 0x001014B6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				Image backgroundImage = this.BackgroundImage;
				if (backgroundImage != null && this.ParentInternal != null)
				{
					ImageLayout backgroundImageLayout = base.BackgroundImageLayout;
					if (backgroundImageLayout != this.ParentInternal.BackgroundImageLayout)
					{
						return this.ParentInternal.BackgroundImageLayout;
					}
				}
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06004731 RID: 18225 RVA: 0x001024C0 File Offset: 0x001014C0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "MDICLIENT";
				createParams.Style |= 3145728;
				createParams.ExStyle |= 512;
				createParams.Param = new NativeMethods.CLIENTCREATESTRUCT(IntPtr.Zero, 1);
				ISite site = (this.ParentInternal == null) ? null : this.ParentInternal.Site;
				if (site != null && site.DesignMode)
				{
					createParams.Style |= 134217728;
					base.SetState(4, false);
				}
				if (this.RightToLeft == RightToLeft.Yes && this.ParentInternal != null && this.ParentInternal.IsMirrored)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06004732 RID: 18226 RVA: 0x00102594 File Offset: 0x00101594
		public Form[] MdiChildren
		{
			get
			{
				Form[] array = new Form[this.children.Count];
				this.children.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x001025C0 File Offset: 0x001015C0
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new MdiClient.ControlCollection(this);
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x001025C8 File Offset: 0x001015C8
		public void LayoutMdi(MdiLayout value)
		{
			if (base.Handle == IntPtr.Zero)
			{
				return;
			}
			switch (value)
			{
			case MdiLayout.Cascade:
				base.SendMessage(551, 0, 0);
				return;
			case MdiLayout.TileHorizontal:
				base.SendMessage(550, 1, 0);
				return;
			case MdiLayout.TileVertical:
				base.SendMessage(550, 0, 0);
				return;
			case MdiLayout.ArrangeIcons:
				base.SendMessage(552, 0, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0010263C File Offset: 0x0010163C
		protected override void OnResize(EventArgs e)
		{
			ISite site = (this.ParentInternal == null) ? null : this.ParentInternal.Site;
			if (site != null && site.DesignMode && base.Handle != IntPtr.Zero)
			{
				this.SetWindowRgn();
			}
			base.OnResize(e);
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0010268C File Offset: 0x0010168C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			base.SuspendLayout();
			try
			{
				Rectangle bounds = base.Bounds;
				int num = (int)Math.Round((double)((float)bounds.X * dx));
				int num2 = (int)Math.Round((double)((float)bounds.Y * dy));
				int width = (int)Math.Round((double)((float)(bounds.X + bounds.Width) * dx - (float)num));
				int height = (int)Math.Round((double)((float)(bounds.Y + bounds.Height) * dy - (float)num2));
				base.SetBounds(num, num2, width, height, BoundsSpecified.All);
			}
			finally
			{
				base.ResumeLayout();
			}
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x0010272C File Offset: 0x0010172C
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			specified &= ~(BoundsSpecified.X | BoundsSpecified.Y);
			base.ScaleControl(factor, specified);
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0010273C File Offset: 0x0010173C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			ISite site = (this.ParentInternal == null) ? null : this.ParentInternal.Site;
			if (base.IsHandleCreated && (site == null || !site.DesignMode))
			{
				Rectangle bounds = base.Bounds;
				base.SetBoundsCore(x, y, width, height, specified);
				Rectangle bounds2 = base.Bounds;
				int num = bounds.Height - bounds2.Height;
				if (num != 0)
				{
					NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
					windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
					for (int i = 0; i < base.Controls.Count; i++)
					{
						Control control = base.Controls[i];
						if (control != null && control is Form)
						{
							Form form = (Form)control;
							if (form.CanRecreateHandle() && form.WindowState == FormWindowState.Minimized)
							{
								UnsafeNativeMethods.GetWindowPlacement(new HandleRef(form, form.Handle), ref windowplacement);
								windowplacement.ptMinPosition_y -= num;
								if (windowplacement.ptMinPosition_y == -1)
								{
									if (num < 0)
									{
										windowplacement.ptMinPosition_y = 0;
									}
									else
									{
										windowplacement.ptMinPosition_y = -2;
									}
								}
								windowplacement.flags = 1;
								UnsafeNativeMethods.SetWindowPlacement(new HandleRef(form, form.Handle), ref windowplacement);
								windowplacement.flags = 0;
							}
						}
					}
					return;
				}
			}
			else
			{
				base.SetBoundsCore(x, y, width, height, specified);
			}
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x001028A0 File Offset: 0x001018A0
		private void SetWindowRgn()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			CreateParams createParams = this.CreateParams;
			SafeNativeMethods.AdjustWindowRectEx(ref rect, createParams.Style, false, createParams.ExStyle);
			Rectangle bounds = base.Bounds;
			intPtr = SafeNativeMethods.CreateRectRgn(0, 0, bounds.Width, bounds.Height);
			try
			{
				intPtr2 = SafeNativeMethods.CreateRectRgn(-rect.left, -rect.top, bounds.Width - rect.right, bounds.Height - rect.bottom);
				try
				{
					if (intPtr == IntPtr.Zero || intPtr2 == IntPtr.Zero)
					{
						throw new InvalidOperationException(SR.GetString("ErrorSettingWindowRegion"));
					}
					if (SafeNativeMethods.CombineRgn(new HandleRef(null, intPtr), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), 4) == 0)
					{
						throw new InvalidOperationException(SR.GetString("ErrorSettingWindowRegion"));
					}
					if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, base.Handle), new HandleRef(null, intPtr), true) == 0)
					{
						throw new InvalidOperationException(SR.GetString("ErrorSettingWindowRegion"));
					}
					intPtr = IntPtr.Zero;
				}
				finally
				{
					if (intPtr2 != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr2));
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x00102A10 File Offset: 0x00101A10
		internal override bool ShouldSerializeBackColor()
		{
			return this.BackColor != SystemColors.AppWorkspace;
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x00102A22 File Offset: 0x00101A22
		private bool ShouldSerializeLocation()
		{
			return false;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x00102A25 File Offset: 0x00101A25
		internal override bool ShouldSerializeSize()
		{
			return false;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x00102A28 File Offset: 0x00101A28
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 1)
			{
				switch (msg)
				{
				case 7:
				{
					base.InvokeGotFocus(this.ParentInternal, EventArgs.Empty);
					Form form = null;
					if (this.ParentInternal is Form)
					{
						form = ((Form)this.ParentInternal).ActiveMdiChildInternal;
					}
					if (form == null && this.MdiChildren.Length > 0 && this.MdiChildren[0].IsMdiChildFocusable)
					{
						form = this.MdiChildren[0];
					}
					if (form != null && form.Visible)
					{
						form.Active = true;
					}
					base.WmImeSetFocus();
					this.DefWndProc(ref m);
					base.InvokeGotFocus(this, EventArgs.Empty);
					return;
				}
				case 8:
					base.InvokeLostFocus(this.ParentInternal, EventArgs.Empty);
					break;
				}
			}
			else if (this.ParentInternal != null && this.ParentInternal.Site != null && this.ParentInternal.Site.DesignMode && base.Handle != IntPtr.Zero)
			{
				this.SetWindowRgn();
			}
			base.WndProc(ref m);
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x00102B40 File Offset: 0x00101B40
		internal override void OnInvokedSetScrollPosition(object sender, EventArgs e)
		{
			Application.Idle += this.OnIdle;
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x00102B53 File Offset: 0x00101B53
		private void OnIdle(object sender, EventArgs e)
		{
			Application.Idle -= this.OnIdle;
			base.OnInvokedSetScrollPosition(sender, e);
		}

		// Token: 0x040021D5 RID: 8661
		private ArrayList children = new ArrayList();

		// Token: 0x020004A4 RID: 1188
		[ComVisible(false)]
		public new class ControlCollection : Control.ControlCollection
		{
			// Token: 0x06004740 RID: 18240 RVA: 0x00102B6E File Offset: 0x00101B6E
			public ControlCollection(MdiClient owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06004741 RID: 18241 RVA: 0x00102B80 File Offset: 0x00101B80
			public override void Add(Control value)
			{
				if (!(value is Form) || !((Form)value).IsMdiChild)
				{
					throw new ArgumentException(SR.GetString("MDIChildAddToNonMDIParent"), "value");
				}
				if (this.owner.CreateThreadId != value.CreateThreadId)
				{
					throw new ArgumentException(SR.GetString("AddDifferentThreads"), "value");
				}
				this.owner.children.Add((Form)value);
				base.Add(value);
			}

			// Token: 0x06004742 RID: 18242 RVA: 0x00102BFD File Offset: 0x00101BFD
			public override void Remove(Control value)
			{
				this.owner.children.Remove(value);
				base.Remove(value);
			}

			// Token: 0x040021D6 RID: 8662
			private MdiClient owner;
		}
	}
}
