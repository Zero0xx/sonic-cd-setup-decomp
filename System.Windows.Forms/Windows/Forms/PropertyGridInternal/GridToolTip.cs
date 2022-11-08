using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B5 RID: 1973
	internal class GridToolTip : Control
	{
		// Token: 0x0600686A RID: 26730 RVA: 0x0017ECE8 File Offset: 0x0017DCE8
		internal GridToolTip(Control[] controls)
		{
			this.controls = controls;
			base.SetStyle(ControlStyles.UserPaint, false);
			this.Font = controls[0].Font;
			this.toolInfos = new NativeMethods.TOOLINFO_T[controls.Length];
			for (int i = 0; i < controls.Length; i++)
			{
				controls[i].HandleCreated += this.OnControlCreateHandle;
				controls[i].HandleDestroyed += this.OnControlDestroyHandle;
				if (controls[i].IsHandleCreated)
				{
					this.SetupToolTip(controls[i]);
				}
			}
		}

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x0600686B RID: 26731 RVA: 0x0017ED85 File Offset: 0x0017DD85
		// (set) Token: 0x0600686C RID: 26732 RVA: 0x0017ED90 File Offset: 0x0017DD90
		public string ToolTip
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				if (base.IsHandleCreated || !string.IsNullOrEmpty(value))
				{
					this.Reset();
				}
				if (value != null && value.Length > this.maximumToolTipLength)
				{
					value = value.Substring(0, this.maximumToolTipLength) + "...";
				}
				this.toolTipText = value;
				if (base.IsHandleCreated)
				{
					bool visible = base.Visible;
					if (visible)
					{
						base.Visible = false;
					}
					if (value == null || value.Length == 0)
					{
						this.dontShow = true;
						value = "";
					}
					else
					{
						this.dontShow = false;
					}
					for (int i = 0; i < this.controls.Length; i++)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_UPDATETIPTEXT, 0, this.GetTOOLINFO(this.controls[i]));
					}
					if (visible && !this.dontShow)
					{
						base.Visible = true;
					}
				}
			}
		}

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600686D RID: 26733 RVA: 0x0017EE6C File Offset: 0x0017DE6C
		protected override CreateParams CreateParams
		{
			get
			{
				SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
				{
					dwICC = 8
				});
				CreateParams createParams = new CreateParams();
				createParams.Parent = IntPtr.Zero;
				createParams.ClassName = "tooltips_class32";
				createParams.Style |= 3;
				createParams.ExStyle = 0;
				createParams.Caption = this.ToolTip;
				return createParams;
			}
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x0017EECC File Offset: 0x0017DECC
		private NativeMethods.TOOLINFO_T GetTOOLINFO(Control c)
		{
			int num = Array.IndexOf<Control>(this.controls, c);
			if (this.toolInfos[num] == null)
			{
				this.toolInfos[num] = new NativeMethods.TOOLINFO_T();
				this.toolInfos[num].cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				this.toolInfos[num].uFlags |= 273;
			}
			this.toolInfos[num].lpszText = this.toolTipText;
			this.toolInfos[num].hwnd = c.Handle;
			this.toolInfos[num].uId = c.Handle;
			return this.toolInfos[num];
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x0017EF73 File Offset: 0x0017DF73
		private void OnControlCreateHandle(object sender, EventArgs e)
		{
			this.SetupToolTip((Control)sender);
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x0017EF81 File Offset: 0x0017DF81
		private void OnControlDestroyHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetTOOLINFO((Control)sender));
			}
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x0017EFB0 File Offset: 0x0017DFB0
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			for (int i = 0; i < this.controls.Length; i++)
			{
				if (this.controls[i].IsHandleCreated)
				{
					this.SetupToolTip(this.controls[i]);
				}
			}
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x0017EFF4 File Offset: 0x0017DFF4
		private void SetupToolTip(Control c)
		{
			if (base.IsHandleCreated)
			{
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(c));
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
			}
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x0017F070 File Offset: 0x0017E070
		public void Reset()
		{
			string toolTip = this.ToolTip;
			this.toolTipText = "";
			for (int i = 0; i < this.controls.Length; i++)
			{
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_UPDATETIPTEXT, 0, this.GetTOOLINFO(this.controls[i]));
			}
			this.toolTipText = toolTip;
			base.SendMessage(1053, 0, 0);
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x0017F0E4 File Offset: 0x0017E0E4
		protected override void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 != 24)
			{
				if (msg2 == 132)
				{
					msg.Result = (IntPtr)(-1);
					return;
				}
			}
			else if ((int)msg.WParam != 0 && this.dontShow)
			{
				msg.WParam = IntPtr.Zero;
			}
			base.WndProc(ref msg);
		}

		// Token: 0x04003D7C RID: 15740
		private Control[] controls;

		// Token: 0x04003D7D RID: 15741
		private string toolTipText;

		// Token: 0x04003D7E RID: 15742
		private NativeMethods.TOOLINFO_T[] toolInfos;

		// Token: 0x04003D7F RID: 15743
		private bool dontShow;

		// Token: 0x04003D80 RID: 15744
		private Point lastMouseMove = Point.Empty;

		// Token: 0x04003D81 RID: 15745
		private int maximumToolTipLength = 1000;
	}
}
