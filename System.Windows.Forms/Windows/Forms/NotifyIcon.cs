using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020005A2 RID: 1442
	[DefaultEvent("MouseDoubleClick")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[SRDescription("DescriptionNotifyIcon")]
	[Designer("System.Windows.Forms.Design.NotifyIconDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Text")]
	public sealed class NotifyIcon : Component
	{
		// Token: 0x06004A7E RID: 19070 RVA: 0x0010E55C File Offset: 0x0010D55C
		public NotifyIcon()
		{
			this.id = ++NotifyIcon.nextId;
			this.window = new NotifyIcon.NotifyIconNativeWindow(this);
			this.UpdateIcon(this.visible);
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0010E5C6 File Offset: 0x0010D5C6
		public NotifyIcon(IContainer container) : this()
		{
			container.Add(this);
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06004A80 RID: 19072 RVA: 0x0010E5D5 File Offset: 0x0010D5D5
		// (set) Token: 0x06004A81 RID: 19073 RVA: 0x0010E5DD File Offset: 0x0010D5DD
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("NotifyIconBalloonTipTextDescr")]
		[Localizable(true)]
		public string BalloonTipText
		{
			get
			{
				return this.balloonTipText;
			}
			set
			{
				if (value != this.balloonTipText)
				{
					this.balloonTipText = value;
				}
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06004A82 RID: 19074 RVA: 0x0010E5F4 File Offset: 0x0010D5F4
		// (set) Token: 0x06004A83 RID: 19075 RVA: 0x0010E5FC File Offset: 0x0010D5FC
		[SRDescription("NotifyIconBalloonTipIconDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(ToolTipIcon.None)]
		public ToolTipIcon BalloonTipIcon
		{
			get
			{
				return this.balloonTipIcon;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolTipIcon));
				}
				if (value != this.balloonTipIcon)
				{
					this.balloonTipIcon = value;
				}
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06004A84 RID: 19076 RVA: 0x0010E634 File Offset: 0x0010D634
		// (set) Token: 0x06004A85 RID: 19077 RVA: 0x0010E63C File Offset: 0x0010D63C
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("NotifyIconBalloonTipTitleDescr")]
		public string BalloonTipTitle
		{
			get
			{
				return this.balloonTipTitle;
			}
			set
			{
				if (value != this.balloonTipTitle)
				{
					this.balloonTipTitle = value;
				}
			}
		}

		// Token: 0x1400029B RID: 667
		// (add) Token: 0x06004A86 RID: 19078 RVA: 0x0010E653 File Offset: 0x0010D653
		// (remove) Token: 0x06004A87 RID: 19079 RVA: 0x0010E666 File Offset: 0x0010D666
		[SRDescription("NotifyIconOnBalloonTipClickedDescr")]
		[SRCategory("CatAction")]
		public event EventHandler BalloonTipClicked
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_BALLOONTIPCLICKED, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_BALLOONTIPCLICKED, value);
			}
		}

		// Token: 0x1400029C RID: 668
		// (add) Token: 0x06004A88 RID: 19080 RVA: 0x0010E679 File Offset: 0x0010D679
		// (remove) Token: 0x06004A89 RID: 19081 RVA: 0x0010E68C File Offset: 0x0010D68C
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconOnBalloonTipClosedDescr")]
		public event EventHandler BalloonTipClosed
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_BALLOONTIPCLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_BALLOONTIPCLOSED, value);
			}
		}

		// Token: 0x1400029D RID: 669
		// (add) Token: 0x06004A8A RID: 19082 RVA: 0x0010E69F File Offset: 0x0010D69F
		// (remove) Token: 0x06004A8B RID: 19083 RVA: 0x0010E6B2 File Offset: 0x0010D6B2
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconOnBalloonTipShownDescr")]
		public event EventHandler BalloonTipShown
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_BALLOONTIPSHOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_BALLOONTIPSHOWN, value);
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06004A8C RID: 19084 RVA: 0x0010E6C5 File Offset: 0x0010D6C5
		// (set) Token: 0x06004A8D RID: 19085 RVA: 0x0010E6CD File Offset: 0x0010D6CD
		[DefaultValue(null)]
		[SRDescription("NotifyIconMenuDescr")]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		public ContextMenu ContextMenu
		{
			get
			{
				return this.contextMenu;
			}
			set
			{
				this.contextMenu = value;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06004A8E RID: 19086 RVA: 0x0010E6D6 File Offset: 0x0010D6D6
		// (set) Token: 0x06004A8F RID: 19087 RVA: 0x0010E6DE File Offset: 0x0010D6DE
		[DefaultValue(null)]
		[SRDescription("NotifyIconMenuDescr")]
		[SRCategory("CatBehavior")]
		public ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.contextMenuStrip;
			}
			set
			{
				this.contextMenuStrip = value;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06004A90 RID: 19088 RVA: 0x0010E6E7 File Offset: 0x0010D6E7
		// (set) Token: 0x06004A91 RID: 19089 RVA: 0x0010E6EF File Offset: 0x0010D6EF
		[SRDescription("NotifyIconIconDescr")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(null)]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (this.icon != value)
				{
					this.icon = value;
					this.UpdateIcon(this.visible);
				}
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06004A92 RID: 19090 RVA: 0x0010E70D File Offset: 0x0010D70D
		// (set) Token: 0x06004A93 RID: 19091 RVA: 0x0010E718 File Offset: 0x0010D718
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[SRDescription("NotifyIconTextDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value != null && !value.Equals(this.text))
				{
					if (value != null && value.Length > 63)
					{
						throw new ArgumentOutOfRangeException("Text", value, SR.GetString("TrayIcon_TextTooLong"));
					}
					this.text = value;
					if (this.added)
					{
						this.UpdateIcon(true);
					}
				}
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06004A94 RID: 19092 RVA: 0x0010E779 File Offset: 0x0010D779
		// (set) Token: 0x06004A95 RID: 19093 RVA: 0x0010E781 File Offset: 0x0010D781
		[SRCategory("CatBehavior")]
		[SRDescription("NotifyIconVisDescr")]
		[Localizable(true)]
		[DefaultValue(false)]
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.visible != value)
				{
					this.UpdateIcon(value);
					this.visible = value;
				}
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06004A96 RID: 19094 RVA: 0x0010E79A File Offset: 0x0010D79A
		// (set) Token: 0x06004A97 RID: 19095 RVA: 0x0010E7A2 File Offset: 0x0010D7A2
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x1400029E RID: 670
		// (add) Token: 0x06004A98 RID: 19096 RVA: 0x0010E7AB File Offset: 0x0010D7AB
		// (remove) Token: 0x06004A99 RID: 19097 RVA: 0x0010E7BE File Offset: 0x0010D7BE
		[SRDescription("ControlOnClickDescr")]
		[SRCategory("CatAction")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_CLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_CLICK, value);
			}
		}

		// Token: 0x1400029F RID: 671
		// (add) Token: 0x06004A9A RID: 19098 RVA: 0x0010E7D1 File Offset: 0x0010D7D1
		// (remove) Token: 0x06004A9B RID: 19099 RVA: 0x0010E7E4 File Offset: 0x0010D7E4
		[SRCategory("CatAction")]
		[SRDescription("ControlOnDoubleClickDescr")]
		public event EventHandler DoubleClick
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_DOUBLECLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_DOUBLECLICK, value);
			}
		}

		// Token: 0x140002A0 RID: 672
		// (add) Token: 0x06004A9C RID: 19100 RVA: 0x0010E7F7 File Offset: 0x0010D7F7
		// (remove) Token: 0x06004A9D RID: 19101 RVA: 0x0010E80A File Offset: 0x0010D80A
		[SRDescription("NotifyIconMouseClickDescr")]
		[SRCategory("CatAction")]
		public event MouseEventHandler MouseClick
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSECLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSECLICK, value);
			}
		}

		// Token: 0x140002A1 RID: 673
		// (add) Token: 0x06004A9E RID: 19102 RVA: 0x0010E81D File Offset: 0x0010D81D
		// (remove) Token: 0x06004A9F RID: 19103 RVA: 0x0010E830 File Offset: 0x0010D830
		[SRDescription("NotifyIconMouseDoubleClickDescr")]
		[SRCategory("CatAction")]
		public event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEDOUBLECLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEDOUBLECLICK, value);
			}
		}

		// Token: 0x140002A2 RID: 674
		// (add) Token: 0x06004AA0 RID: 19104 RVA: 0x0010E843 File Offset: 0x0010D843
		// (remove) Token: 0x06004AA1 RID: 19105 RVA: 0x0010E856 File Offset: 0x0010D856
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseDownDescr")]
		public event MouseEventHandler MouseDown
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEDOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEDOWN, value);
			}
		}

		// Token: 0x140002A3 RID: 675
		// (add) Token: 0x06004AA2 RID: 19106 RVA: 0x0010E869 File Offset: 0x0010D869
		// (remove) Token: 0x06004AA3 RID: 19107 RVA: 0x0010E87C File Offset: 0x0010D87C
		[SRDescription("ControlOnMouseMoveDescr")]
		[SRCategory("CatMouse")]
		public event MouseEventHandler MouseMove
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEMOVE, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEMOVE, value);
			}
		}

		// Token: 0x140002A4 RID: 676
		// (add) Token: 0x06004AA4 RID: 19108 RVA: 0x0010E88F File Offset: 0x0010D88F
		// (remove) Token: 0x06004AA5 RID: 19109 RVA: 0x0010E8A2 File Offset: 0x0010D8A2
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseUpDescr")]
		public event MouseEventHandler MouseUp
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEUP, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEUP, value);
			}
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x0010E8B8 File Offset: 0x0010D8B8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.window != null)
				{
					this.icon = null;
					this.Text = string.Empty;
					this.UpdateIcon(false);
					this.window.DestroyHandle();
					this.window = null;
					this.contextMenu = null;
					this.contextMenuStrip = null;
				}
			}
			else if (this.window != null && this.window.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this.window, this.window.Handle), 16, 0, 0);
				this.window.ReleaseHandle();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x0010E960 File Offset: 0x0010D960
		private void OnBalloonTipClicked()
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_BALLOONTIPCLICKED];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x0010E994 File Offset: 0x0010D994
		private void OnBalloonTipClosed()
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_BALLOONTIPCLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x0010E9C8 File Offset: 0x0010D9C8
		private void OnBalloonTipShown()
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_BALLOONTIPSHOWN];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x0010E9FC File Offset: 0x0010D9FC
		private void OnClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_CLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0010EA2C File Offset: 0x0010DA2C
		private void OnDoubleClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_DOUBLECLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x0010EA5C File Offset: 0x0010DA5C
		private void OnMouseClick(MouseEventArgs mea)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSECLICK];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, mea);
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0010EA8C File Offset: 0x0010DA8C
		private void OnMouseDoubleClick(MouseEventArgs mea)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEDOUBLECLICK];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, mea);
			}
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x0010EABC File Offset: 0x0010DABC
		private void OnMouseDown(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEDOWN];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x0010EAEC File Offset: 0x0010DAEC
		private void OnMouseMove(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEMOVE];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0010EB1C File Offset: 0x0010DB1C
		private void OnMouseUp(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEUP];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0010EB4A File Offset: 0x0010DB4A
		public void ShowBalloonTip(int timeout)
		{
			this.ShowBalloonTip(timeout, this.balloonTipTitle, this.balloonTipText, this.balloonTipIcon);
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x0010EB68 File Offset: 0x0010DB68
		public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon)
		{
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout", SR.GetString("InvalidArgument", new object[]
				{
					"timeout",
					timeout.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (string.IsNullOrEmpty(tipText))
			{
				throw new ArgumentException(SR.GetString("NotifyIconEmptyOrNullTipText"));
			}
			if (!ClientUtils.IsEnumValid(tipIcon, (int)tipIcon, 0, 3))
			{
				throw new InvalidEnumArgumentException("tipIcon", (int)tipIcon, typeof(ToolTipIcon));
			}
			if (this.added)
			{
				if (base.DesignMode)
				{
					return;
				}
				IntSecurity.UnrestrictedWindows.Demand();
				NativeMethods.NOTIFYICONDATA notifyicondata = new NativeMethods.NOTIFYICONDATA();
				if (this.window.Handle == IntPtr.Zero)
				{
					this.window.CreateHandle(new CreateParams());
				}
				notifyicondata.hWnd = this.window.Handle;
				notifyicondata.uID = this.id;
				notifyicondata.uFlags = 16;
				notifyicondata.uTimeoutOrVersion = timeout;
				notifyicondata.szInfoTitle = tipTitle;
				notifyicondata.szInfo = tipText;
				switch (tipIcon)
				{
				case ToolTipIcon.None:
					notifyicondata.dwInfoFlags = 0;
					break;
				case ToolTipIcon.Info:
					notifyicondata.dwInfoFlags = 1;
					break;
				case ToolTipIcon.Warning:
					notifyicondata.dwInfoFlags = 2;
					break;
				case ToolTipIcon.Error:
					notifyicondata.dwInfoFlags = 3;
					break;
				}
				UnsafeNativeMethods.Shell_NotifyIcon(1, notifyicondata);
			}
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0010ECB8 File Offset: 0x0010DCB8
		private void ShowContextMenu()
		{
			if (this.contextMenu != null || this.contextMenuStrip != null)
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				UnsafeNativeMethods.GetCursorPos(point);
				UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this.window, this.window.Handle));
				if (this.contextMenu != null)
				{
					this.contextMenu.OnPopup(EventArgs.Empty);
					SafeNativeMethods.TrackPopupMenuEx(new HandleRef(this.contextMenu, this.contextMenu.Handle), 72, point.x, point.y, new HandleRef(this.window, this.window.Handle), null);
					UnsafeNativeMethods.PostMessage(new HandleRef(this.window, this.window.Handle), 0, IntPtr.Zero, IntPtr.Zero);
					return;
				}
				if (this.contextMenuStrip != null)
				{
					this.contextMenuStrip.ShowInTaskbar(point.x, point.y);
				}
			}
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0010EDA4 File Offset: 0x0010DDA4
		private void UpdateIcon(bool showIconInTray)
		{
			lock (this.syncObj)
			{
				if (!base.DesignMode)
				{
					IntSecurity.UnrestrictedWindows.Demand();
					this.window.LockReference(showIconInTray);
					NativeMethods.NOTIFYICONDATA notifyicondata = new NativeMethods.NOTIFYICONDATA();
					notifyicondata.uCallbackMessage = 2048;
					notifyicondata.uFlags = 1;
					if (showIconInTray && this.window.Handle == IntPtr.Zero)
					{
						this.window.CreateHandle(new CreateParams());
					}
					notifyicondata.hWnd = this.window.Handle;
					notifyicondata.uID = this.id;
					notifyicondata.hIcon = IntPtr.Zero;
					notifyicondata.szTip = null;
					if (this.icon != null)
					{
						notifyicondata.uFlags |= 2;
						notifyicondata.hIcon = this.icon.Handle;
					}
					notifyicondata.uFlags |= 4;
					notifyicondata.szTip = this.text;
					if (showIconInTray && this.icon != null)
					{
						if (!this.added)
						{
							UnsafeNativeMethods.Shell_NotifyIcon(0, notifyicondata);
							this.added = true;
						}
						else
						{
							UnsafeNativeMethods.Shell_NotifyIcon(1, notifyicondata);
						}
					}
					else if (this.added)
					{
						UnsafeNativeMethods.Shell_NotifyIcon(2, notifyicondata);
						this.added = false;
					}
				}
			}
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0010EF00 File Offset: 0x0010DF00
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			if (clicks == 2)
			{
				this.OnDoubleClick(new MouseEventArgs(button, 2, 0, 0, 0));
				this.OnMouseDoubleClick(new MouseEventArgs(button, 2, 0, 0, 0));
				this.doubleClick = true;
			}
			this.OnMouseDown(new MouseEventArgs(button, clicks, 0, 0, 0));
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0010EF3D File Offset: 0x0010DF3D
		private void WmMouseMove(ref Message m)
		{
			this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, 0, 0, 0));
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0010EF54 File Offset: 0x0010DF54
		private void WmMouseUp(ref Message m, MouseButtons button)
		{
			this.OnMouseUp(new MouseEventArgs(button, 0, 0, 0, 0));
			if (!this.doubleClick)
			{
				this.OnClick(new MouseEventArgs(button, 0, 0, 0, 0));
				this.OnMouseClick(new MouseEventArgs(button, 0, 0, 0, 0));
			}
			this.doubleClick = false;
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0010EFA0 File Offset: 0x0010DFA0
		private void WmTaskbarCreated(ref Message m)
		{
			this.added = false;
			this.UpdateIcon(this.visible);
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0010EFB8 File Offset: 0x0010DFB8
		private void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 <= 44)
			{
				if (msg2 == 2)
				{
					this.UpdateIcon(false);
					return;
				}
				switch (msg2)
				{
				case 43:
					if (msg.WParam == IntPtr.Zero)
					{
						this.WmDrawItemMenuItem(ref msg);
						return;
					}
					return;
				case 44:
					if (msg.WParam == IntPtr.Zero)
					{
						this.WmMeasureMenuItem(ref msg);
						return;
					}
					return;
				}
			}
			else if (msg2 != 273)
			{
				if (msg2 == 279)
				{
					this.WmInitMenuPopup(ref msg);
					return;
				}
				if (msg2 == 2048)
				{
					int num = (int)msg.LParam;
					switch (num)
					{
					case 512:
						this.WmMouseMove(ref msg);
						return;
					case 513:
						this.WmMouseDown(ref msg, MouseButtons.Left, 1);
						return;
					case 514:
						this.WmMouseUp(ref msg, MouseButtons.Left);
						return;
					case 515:
						this.WmMouseDown(ref msg, MouseButtons.Left, 2);
						return;
					case 516:
						this.WmMouseDown(ref msg, MouseButtons.Right, 1);
						return;
					case 517:
						if (this.contextMenu != null || this.contextMenuStrip != null)
						{
							this.ShowContextMenu();
						}
						this.WmMouseUp(ref msg, MouseButtons.Right);
						return;
					case 518:
						this.WmMouseDown(ref msg, MouseButtons.Right, 2);
						return;
					case 519:
						this.WmMouseDown(ref msg, MouseButtons.Middle, 1);
						return;
					case 520:
						this.WmMouseUp(ref msg, MouseButtons.Middle);
						return;
					case 521:
						this.WmMouseDown(ref msg, MouseButtons.Middle, 2);
						return;
					default:
						switch (num)
						{
						case 1026:
							this.OnBalloonTipShown();
							return;
						case 1027:
							this.OnBalloonTipClosed();
							return;
						case 1028:
							this.OnBalloonTipClosed();
							return;
						case 1029:
							this.OnBalloonTipClicked();
							return;
						default:
							return;
						}
						break;
					}
				}
			}
			else
			{
				if (!(IntPtr.Zero == msg.LParam))
				{
					this.window.DefWndProc(ref msg);
					return;
				}
				if (Command.DispatchID((int)msg.WParam & 65535))
				{
					return;
				}
				return;
			}
			if (msg.Msg == NotifyIcon.WM_TASKBARCREATED)
			{
				this.WmTaskbarCreated(ref msg);
			}
			this.window.DefWndProc(ref msg);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0010F1BD File Offset: 0x0010E1BD
		private void WmInitMenuPopup(ref Message m)
		{
			if (this.contextMenu != null && this.contextMenu.ProcessInitMenuPopup(m.WParam))
			{
				return;
			}
			this.window.DefWndProc(ref m);
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0010F1E8 File Offset: 0x0010E1E8
		private void WmMeasureMenuItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(measureitemstruct.itemData);
			if (menuItemFromItemData != null)
			{
				menuItemFromItemData.WmMeasureItem(ref m);
			}
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0010F224 File Offset: 0x0010E224
		private void WmDrawItemMenuItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(drawitemstruct.itemData);
			if (menuItemFromItemData != null)
			{
				menuItemFromItemData.WmDrawItem(ref m);
			}
		}

		// Token: 0x040030B7 RID: 12471
		private const int WM_TRAYMOUSEMESSAGE = 2048;

		// Token: 0x040030B8 RID: 12472
		private static readonly object EVENT_MOUSEDOWN = new object();

		// Token: 0x040030B9 RID: 12473
		private static readonly object EVENT_MOUSEMOVE = new object();

		// Token: 0x040030BA RID: 12474
		private static readonly object EVENT_MOUSEUP = new object();

		// Token: 0x040030BB RID: 12475
		private static readonly object EVENT_CLICK = new object();

		// Token: 0x040030BC RID: 12476
		private static readonly object EVENT_DOUBLECLICK = new object();

		// Token: 0x040030BD RID: 12477
		private static readonly object EVENT_MOUSECLICK = new object();

		// Token: 0x040030BE RID: 12478
		private static readonly object EVENT_MOUSEDOUBLECLICK = new object();

		// Token: 0x040030BF RID: 12479
		private static readonly object EVENT_BALLOONTIPSHOWN = new object();

		// Token: 0x040030C0 RID: 12480
		private static readonly object EVENT_BALLOONTIPCLICKED = new object();

		// Token: 0x040030C1 RID: 12481
		private static readonly object EVENT_BALLOONTIPCLOSED = new object();

		// Token: 0x040030C2 RID: 12482
		private static int WM_TASKBARCREATED = SafeNativeMethods.RegisterWindowMessage("TaskbarCreated");

		// Token: 0x040030C3 RID: 12483
		private object syncObj = new object();

		// Token: 0x040030C4 RID: 12484
		private Icon icon;

		// Token: 0x040030C5 RID: 12485
		private string text = "";

		// Token: 0x040030C6 RID: 12486
		private int id;

		// Token: 0x040030C7 RID: 12487
		private bool added;

		// Token: 0x040030C8 RID: 12488
		private NotifyIcon.NotifyIconNativeWindow window;

		// Token: 0x040030C9 RID: 12489
		private ContextMenu contextMenu;

		// Token: 0x040030CA RID: 12490
		private ContextMenuStrip contextMenuStrip;

		// Token: 0x040030CB RID: 12491
		private ToolTipIcon balloonTipIcon;

		// Token: 0x040030CC RID: 12492
		private string balloonTipText = "";

		// Token: 0x040030CD RID: 12493
		private string balloonTipTitle = "";

		// Token: 0x040030CE RID: 12494
		private static int nextId = 0;

		// Token: 0x040030CF RID: 12495
		private object userData;

		// Token: 0x040030D0 RID: 12496
		private bool doubleClick;

		// Token: 0x040030D1 RID: 12497
		private bool visible;

		// Token: 0x020005A3 RID: 1443
		private class NotifyIconNativeWindow : NativeWindow
		{
			// Token: 0x06004ABE RID: 19134 RVA: 0x0010F2E6 File Offset: 0x0010E2E6
			internal NotifyIconNativeWindow(NotifyIcon component)
			{
				this.reference = component;
			}

			// Token: 0x06004ABF RID: 19135 RVA: 0x0010F2F8 File Offset: 0x0010E2F8
			~NotifyIconNativeWindow()
			{
				if (base.Handle != IntPtr.Zero)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 16, 0, 0);
				}
			}

			// Token: 0x06004AC0 RID: 19136 RVA: 0x0010F348 File Offset: 0x0010E348
			public void LockReference(bool locked)
			{
				if (locked)
				{
					if (!this.rootRef.IsAllocated)
					{
						this.rootRef = GCHandle.Alloc(this.reference, GCHandleType.Normal);
						return;
					}
				}
				else if (this.rootRef.IsAllocated)
				{
					this.rootRef.Free();
				}
			}

			// Token: 0x06004AC1 RID: 19137 RVA: 0x0010F385 File Offset: 0x0010E385
			protected override void OnThreadException(Exception e)
			{
				Application.OnThreadException(e);
			}

			// Token: 0x06004AC2 RID: 19138 RVA: 0x0010F38D File Offset: 0x0010E38D
			protected override void WndProc(ref Message m)
			{
				this.reference.WndProc(ref m);
			}

			// Token: 0x040030D2 RID: 12498
			internal NotifyIcon reference;

			// Token: 0x040030D3 RID: 12499
			private GCHandle rootRef;
		}
	}
}
