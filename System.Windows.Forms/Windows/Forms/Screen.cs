using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020005F8 RID: 1528
	public class Screen
	{
		// Token: 0x06005082 RID: 20610 RVA: 0x00126935 File Offset: 0x00125935
		internal Screen(IntPtr monitor) : this(monitor, IntPtr.Zero)
		{
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x00126944 File Offset: 0x00125944
		internal Screen(IntPtr monitor, IntPtr hdc)
		{
			IntPtr intPtr = hdc;
			if (!Screen.multiMonitorSupport || monitor == (IntPtr)(-1163005939))
			{
				this.bounds = SystemInformation.VirtualScreen;
				this.primary = true;
				this.deviceName = "DISPLAY";
			}
			else
			{
				NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
				SafeNativeMethods.GetMonitorInfo(new HandleRef(null, monitor), monitorinfoex);
				this.bounds = Rectangle.FromLTRB(monitorinfoex.rcMonitor.left, monitorinfoex.rcMonitor.top, monitorinfoex.rcMonitor.right, monitorinfoex.rcMonitor.bottom);
				this.primary = ((monitorinfoex.dwFlags & 1) != 0);
				int num = monitorinfoex.szDevice.Length;
				while (num > 0 && monitorinfoex.szDevice[num - 1] == '\0')
				{
					num--;
				}
				this.deviceName = new string(monitorinfoex.szDevice);
				string text = this.deviceName;
				char[] trimChars = new char[1];
				this.deviceName = text.TrimEnd(trimChars);
				if (hdc == IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.CreateDC(this.deviceName);
				}
			}
			this.hmonitor = monitor;
			this.bitDepth = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, intPtr), 12);
			this.bitDepth *= UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, intPtr), 14);
			if (hdc != intPtr)
			{
				UnsafeNativeMethods.DeleteDC(new HandleRef(null, intPtr));
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06005084 RID: 20612 RVA: 0x00126AB4 File Offset: 0x00125AB4
		public static Screen[] AllScreens
		{
			get
			{
				if (Screen.screens == null)
				{
					if (Screen.multiMonitorSupport)
					{
						Screen.MonitorEnumCallback monitorEnumCallback = new Screen.MonitorEnumCallback();
						NativeMethods.MonitorEnumProc lpfnEnum = new NativeMethods.MonitorEnumProc(monitorEnumCallback.Callback);
						IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
						try
						{
							SafeNativeMethods.EnumDisplayMonitors(new HandleRef(null, dc), null, lpfnEnum, IntPtr.Zero);
						}
						finally
						{
							UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
						}
						if (monitorEnumCallback.screens.Count > 0)
						{
							Screen[] array = new Screen[monitorEnumCallback.screens.Count];
							monitorEnumCallback.screens.CopyTo(array, 0);
							Screen.screens = array;
						}
						else
						{
							Screen.screens = new Screen[]
							{
								new Screen((IntPtr)(-1163005939))
							};
						}
					}
					else
					{
						Screen.screens = new Screen[]
						{
							Screen.PrimaryScreen
						};
					}
					SystemEvents.DisplaySettingsChanging += Screen.OnDisplaySettingsChanging;
				}
				return Screen.screens;
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06005085 RID: 20613 RVA: 0x00126BB4 File Offset: 0x00125BB4
		public int BitsPerPixel
		{
			get
			{
				return this.bitDepth;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06005086 RID: 20614 RVA: 0x00126BBC File Offset: 0x00125BBC
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06005087 RID: 20615 RVA: 0x00126BC4 File Offset: 0x00125BC4
		public string DeviceName
		{
			get
			{
				return this.deviceName;
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06005088 RID: 20616 RVA: 0x00126BCC File Offset: 0x00125BCC
		public bool Primary
		{
			get
			{
				return this.primary;
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x00126BD4 File Offset: 0x00125BD4
		public static Screen PrimaryScreen
		{
			get
			{
				if (Screen.multiMonitorSupport)
				{
					Screen[] allScreens = Screen.AllScreens;
					for (int i = 0; i < allScreens.Length; i++)
					{
						if (allScreens[i].primary)
						{
							return allScreens[i];
						}
					}
					return null;
				}
				return new Screen((IntPtr)(-1163005939), IntPtr.Zero);
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x0600508A RID: 20618 RVA: 0x00126C20 File Offset: 0x00125C20
		public Rectangle WorkingArea
		{
			get
			{
				if (this.currentDesktopChangedCount != Screen.DesktopChangedCount)
				{
					Interlocked.Exchange(ref this.currentDesktopChangedCount, Screen.DesktopChangedCount);
					if (!Screen.multiMonitorSupport || this.hmonitor == (IntPtr)(-1163005939))
					{
						this.workingArea = SystemInformation.WorkingArea;
					}
					else
					{
						NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
						SafeNativeMethods.GetMonitorInfo(new HandleRef(null, this.hmonitor), monitorinfoex);
						this.workingArea = Rectangle.FromLTRB(monitorinfoex.rcWork.left, monitorinfoex.rcWork.top, monitorinfoex.rcWork.right, monitorinfoex.rcWork.bottom);
					}
				}
				return this.workingArea;
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600508B RID: 20619 RVA: 0x00126CD0 File Offset: 0x00125CD0
		private static int DesktopChangedCount
		{
			get
			{
				if (Screen.desktopChangedCount == -1)
				{
					lock (Screen.syncLock)
					{
						if (Screen.desktopChangedCount == -1)
						{
							SystemEvents.UserPreferenceChanged += Screen.OnUserPreferenceChanged;
							Screen.desktopChangedCount = 0;
						}
					}
				}
				return Screen.desktopChangedCount;
			}
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x00126D30 File Offset: 0x00125D30
		public override bool Equals(object obj)
		{
			if (obj is Screen)
			{
				Screen screen = (Screen)obj;
				if (this.hmonitor == screen.hmonitor)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x00126D64 File Offset: 0x00125D64
		public static Screen FromPoint(Point point)
		{
			if (Screen.multiMonitorSupport)
			{
				NativeMethods.POINTSTRUCT pt = new NativeMethods.POINTSTRUCT(point.X, point.Y);
				return new Screen(SafeNativeMethods.MonitorFromPoint(pt, 2));
			}
			return new Screen((IntPtr)(-1163005939));
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x00126DAC File Offset: 0x00125DAC
		public static Screen FromRectangle(Rectangle rect)
		{
			if (Screen.multiMonitorSupport)
			{
				NativeMethods.RECT rect2 = NativeMethods.RECT.FromXYWH(rect.X, rect.Y, rect.Width, rect.Height);
				return new Screen(SafeNativeMethods.MonitorFromRect(ref rect2, 2));
			}
			return new Screen((IntPtr)(-1163005939), IntPtr.Zero);
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x00126E04 File Offset: 0x00125E04
		public static Screen FromControl(Control control)
		{
			return Screen.FromHandleInternal(control.Handle);
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x00126E11 File Offset: 0x00125E11
		public static Screen FromHandle(IntPtr hwnd)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return Screen.FromHandleInternal(hwnd);
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x00126E23 File Offset: 0x00125E23
		internal static Screen FromHandleInternal(IntPtr hwnd)
		{
			if (Screen.multiMonitorSupport)
			{
				return new Screen(SafeNativeMethods.MonitorFromWindow(new HandleRef(null, hwnd), 2));
			}
			return new Screen((IntPtr)(-1163005939), IntPtr.Zero);
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x00126E53 File Offset: 0x00125E53
		public static Rectangle GetWorkingArea(Point pt)
		{
			return Screen.FromPoint(pt).WorkingArea;
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x00126E60 File Offset: 0x00125E60
		public static Rectangle GetWorkingArea(Rectangle rect)
		{
			return Screen.FromRectangle(rect).WorkingArea;
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x00126E6D File Offset: 0x00125E6D
		public static Rectangle GetWorkingArea(Control ctl)
		{
			return Screen.FromControl(ctl).WorkingArea;
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x00126E7A File Offset: 0x00125E7A
		public static Rectangle GetBounds(Point pt)
		{
			return Screen.FromPoint(pt).Bounds;
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x00126E87 File Offset: 0x00125E87
		public static Rectangle GetBounds(Rectangle rect)
		{
			return Screen.FromRectangle(rect).Bounds;
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x00126E94 File Offset: 0x00125E94
		public static Rectangle GetBounds(Control ctl)
		{
			return Screen.FromControl(ctl).Bounds;
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x00126EA1 File Offset: 0x00125EA1
		public override int GetHashCode()
		{
			return (int)this.hmonitor;
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x00126EAE File Offset: 0x00125EAE
		private static void OnDisplaySettingsChanging(object sender, EventArgs e)
		{
			SystemEvents.DisplaySettingsChanging -= Screen.OnDisplaySettingsChanging;
			Screen.screens = null;
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x00126EC7 File Offset: 0x00125EC7
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Desktop)
			{
				Interlocked.Increment(ref Screen.desktopChangedCount);
			}
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x00126EE0 File Offset: 0x00125EE0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				"[Bounds=",
				this.bounds.ToString(),
				" WorkingArea=",
				this.WorkingArea.ToString(),
				" Primary=",
				this.primary.ToString(),
				" DeviceName=",
				this.deviceName
			});
		}

		// Token: 0x040034B7 RID: 13495
		private const int PRIMARY_MONITOR = -1163005939;

		// Token: 0x040034B8 RID: 13496
		private const int MONITOR_DEFAULTTONULL = 0;

		// Token: 0x040034B9 RID: 13497
		private const int MONITOR_DEFAULTTOPRIMARY = 1;

		// Token: 0x040034BA RID: 13498
		private const int MONITOR_DEFAULTTONEAREST = 2;

		// Token: 0x040034BB RID: 13499
		private const int MONITORINFOF_PRIMARY = 1;

		// Token: 0x040034BC RID: 13500
		private readonly IntPtr hmonitor;

		// Token: 0x040034BD RID: 13501
		private readonly Rectangle bounds;

		// Token: 0x040034BE RID: 13502
		private Rectangle workingArea = Rectangle.Empty;

		// Token: 0x040034BF RID: 13503
		private readonly bool primary;

		// Token: 0x040034C0 RID: 13504
		private readonly string deviceName;

		// Token: 0x040034C1 RID: 13505
		private readonly int bitDepth;

		// Token: 0x040034C2 RID: 13506
		private static object syncLock = new object();

		// Token: 0x040034C3 RID: 13507
		private static int desktopChangedCount = -1;

		// Token: 0x040034C4 RID: 13508
		private int currentDesktopChangedCount = -1;

		// Token: 0x040034C5 RID: 13509
		private static bool multiMonitorSupport = UnsafeNativeMethods.GetSystemMetrics(80) != 0;

		// Token: 0x040034C6 RID: 13510
		private static Screen[] screens;

		// Token: 0x020005F9 RID: 1529
		private class MonitorEnumCallback
		{
			// Token: 0x0600509D RID: 20637 RVA: 0x00126F95 File Offset: 0x00125F95
			public virtual bool Callback(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lparam)
			{
				this.screens.Add(new Screen(monitor, hdc));
				return true;
			}

			// Token: 0x040034C7 RID: 13511
			public ArrayList screens = new ArrayList();
		}
	}
}
