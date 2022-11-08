using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x02000639 RID: 1593
	public class SystemInformation
	{
		// Token: 0x0600534F RID: 21327 RVA: 0x00130C09 File Offset: 0x0012FC09
		private SystemInformation()
		{
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06005350 RID: 21328 RVA: 0x00130C14 File Offset: 0x0012FC14
		public static bool DragFullWindows
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(38, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06005351 RID: 21329 RVA: 0x00130C38 File Offset: 0x0012FC38
		public static bool HighContrast
		{
			get
			{
				SystemInformation.EnsureSystemEvents();
				if (SystemInformation.systemEventsDirty)
				{
					NativeMethods.HIGHCONTRAST_I highcontrast_I = default(NativeMethods.HIGHCONTRAST_I);
					highcontrast_I.cbSize = Marshal.SizeOf(highcontrast_I);
					highcontrast_I.dwFlags = 0;
					highcontrast_I.lpszDefaultScheme = IntPtr.Zero;
					bool flag = UnsafeNativeMethods.SystemParametersInfo(66, highcontrast_I.cbSize, ref highcontrast_I, 0);
					if (flag)
					{
						SystemInformation.highContrast = ((highcontrast_I.dwFlags & 1) != 0);
					}
					else
					{
						SystemInformation.highContrast = false;
					}
					SystemInformation.systemEventsDirty = false;
				}
				return SystemInformation.highContrast;
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x00130CBC File Offset: 0x0012FCBC
		public static int MouseWheelScrollLines
		{
			get
			{
				if (SystemInformation.NativeMouseWheelSupport)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(104, 0, ref result, 0);
					return result;
				}
				IntPtr intPtr = IntPtr.Zero;
				intPtr = UnsafeNativeMethods.FindWindow("MouseZ", "Magellan MSWHEEL");
				if (intPtr != IntPtr.Zero)
				{
					int msg = SafeNativeMethods.RegisterWindowMessage("MSH_SCROLL_LINES_MSG");
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(null, intPtr), msg, 0, 0);
					if (num != 0)
					{
						return num;
					}
				}
				return 3;
			}
		}

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06005353 RID: 21331 RVA: 0x00130D29 File Offset: 0x0012FD29
		public static Size PrimaryMonitorSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(0), UnsafeNativeMethods.GetSystemMetrics(1));
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06005354 RID: 21332 RVA: 0x00130D3C File Offset: 0x0012FD3C
		public static int VerticalScrollBarWidth
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(2);
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06005355 RID: 21333 RVA: 0x00130D44 File Offset: 0x0012FD44
		public static int HorizontalScrollBarHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(3);
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06005356 RID: 21334 RVA: 0x00130D4C File Offset: 0x0012FD4C
		public static int CaptionHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(4);
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06005357 RID: 21335 RVA: 0x00130D54 File Offset: 0x0012FD54
		public static Size BorderSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(5), UnsafeNativeMethods.GetSystemMetrics(6));
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06005358 RID: 21336 RVA: 0x00130D67 File Offset: 0x0012FD67
		public static Size FixedFrameBorderSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(7), UnsafeNativeMethods.GetSystemMetrics(8));
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06005359 RID: 21337 RVA: 0x00130D7A File Offset: 0x0012FD7A
		public static int VerticalScrollBarThumbHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(9);
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x0600535A RID: 21338 RVA: 0x00130D83 File Offset: 0x0012FD83
		public static int HorizontalScrollBarThumbWidth
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(10);
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600535B RID: 21339 RVA: 0x00130D8C File Offset: 0x0012FD8C
		public static Size IconSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(11), UnsafeNativeMethods.GetSystemMetrics(12));
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x0600535C RID: 21340 RVA: 0x00130DA1 File Offset: 0x0012FDA1
		public static Size CursorSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(13), UnsafeNativeMethods.GetSystemMetrics(14));
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x0600535D RID: 21341 RVA: 0x00130DB8 File Offset: 0x0012FDB8
		public static Font MenuFont
		{
			get
			{
				Font result = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfMenuFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						result = Font.FromLogFont(nonclientmetrics.lfMenuFont);
					}
					catch
					{
						result = Control.DefaultFont;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return result;
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x00130E30 File Offset: 0x0012FE30
		public static int MenuHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(15);
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x0600535F RID: 21343 RVA: 0x00130E39 File Offset: 0x0012FE39
		public static PowerStatus PowerStatus
		{
			get
			{
				if (SystemInformation.powerStatus == null)
				{
					SystemInformation.powerStatus = new PowerStatus();
				}
				return SystemInformation.powerStatus;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06005360 RID: 21344 RVA: 0x00130E54 File Offset: 0x0012FE54
		public static Rectangle WorkingArea
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.SystemParametersInfo(48, 0, ref rect, 0);
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06005361 RID: 21345 RVA: 0x00130E96 File Offset: 0x0012FE96
		public static int KanjiWindowHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(18);
			}
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06005362 RID: 21346 RVA: 0x00130E9F File Offset: 0x0012FE9F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool MousePresent
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(19) != 0;
			}
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06005363 RID: 21347 RVA: 0x00130EAE File Offset: 0x0012FEAE
		public static int VerticalScrollBarArrowHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(20);
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06005364 RID: 21348 RVA: 0x00130EB7 File Offset: 0x0012FEB7
		public static int HorizontalScrollBarArrowWidth
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(21);
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06005365 RID: 21349 RVA: 0x00130EC0 File Offset: 0x0012FEC0
		public static bool DebugOS
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				return UnsafeNativeMethods.GetSystemMetrics(22) != 0;
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x00130ED9 File Offset: 0x0012FED9
		public static bool MouseButtonsSwapped
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(23) != 0;
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x00130EE8 File Offset: 0x0012FEE8
		public static Size MinimumWindowSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(28), UnsafeNativeMethods.GetSystemMetrics(29));
			}
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06005368 RID: 21352 RVA: 0x00130EFD File Offset: 0x0012FEFD
		public static Size CaptionButtonSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(30), UnsafeNativeMethods.GetSystemMetrics(31));
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06005369 RID: 21353 RVA: 0x00130F12 File Offset: 0x0012FF12
		public static Size FrameBorderSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(32), UnsafeNativeMethods.GetSystemMetrics(33));
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x0600536A RID: 21354 RVA: 0x00130F27 File Offset: 0x0012FF27
		public static Size MinWindowTrackSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(34), UnsafeNativeMethods.GetSystemMetrics(35));
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x0600536B RID: 21355 RVA: 0x00130F3C File Offset: 0x0012FF3C
		public static Size DoubleClickSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(36), UnsafeNativeMethods.GetSystemMetrics(37));
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x0600536C RID: 21356 RVA: 0x00130F51 File Offset: 0x0012FF51
		public static int DoubleClickTime
		{
			get
			{
				return SafeNativeMethods.GetDoubleClickTime();
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x0600536D RID: 21357 RVA: 0x00130F58 File Offset: 0x0012FF58
		public static Size IconSpacingSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(38), UnsafeNativeMethods.GetSystemMetrics(39));
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x0600536E RID: 21358 RVA: 0x00130F6D File Offset: 0x0012FF6D
		public static bool RightAlignedMenus
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(40) != 0;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x0600536F RID: 21359 RVA: 0x00130F7C File Offset: 0x0012FF7C
		public static bool PenWindows
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(41) != 0;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06005370 RID: 21360 RVA: 0x00130F8B File Offset: 0x0012FF8B
		public static bool DbcsEnabled
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(42) != 0;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06005371 RID: 21361 RVA: 0x00130F9A File Offset: 0x0012FF9A
		public static int MouseButtons
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(43);
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06005372 RID: 21362 RVA: 0x00130FA3 File Offset: 0x0012FFA3
		public static bool Secure
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				return UnsafeNativeMethods.GetSystemMetrics(44) != 0;
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06005373 RID: 21363 RVA: 0x00130FBC File Offset: 0x0012FFBC
		public static Size Border3DSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(45), UnsafeNativeMethods.GetSystemMetrics(46));
			}
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06005374 RID: 21364 RVA: 0x00130FD1 File Offset: 0x0012FFD1
		public static Size MinimizedWindowSpacingSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(47), UnsafeNativeMethods.GetSystemMetrics(48));
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06005375 RID: 21365 RVA: 0x00130FE6 File Offset: 0x0012FFE6
		public static Size SmallIconSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(49), UnsafeNativeMethods.GetSystemMetrics(50));
			}
		}

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06005376 RID: 21366 RVA: 0x00130FFB File Offset: 0x0012FFFB
		public static int ToolWindowCaptionHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(51);
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06005377 RID: 21367 RVA: 0x00131004 File Offset: 0x00130004
		public static Size ToolWindowCaptionButtonSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(52), UnsafeNativeMethods.GetSystemMetrics(53));
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x06005378 RID: 21368 RVA: 0x00131019 File Offset: 0x00130019
		public static Size MenuButtonSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(54), UnsafeNativeMethods.GetSystemMetrics(55));
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x06005379 RID: 21369 RVA: 0x00131030 File Offset: 0x00130030
		public static ArrangeStartingPosition ArrangeStartingPosition
		{
			get
			{
				ArrangeStartingPosition arrangeStartingPosition = ArrangeStartingPosition.BottomRight | ArrangeStartingPosition.Hide | ArrangeStartingPosition.TopLeft;
				int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(56);
				return arrangeStartingPosition & (ArrangeStartingPosition)systemMetrics;
			}
		}

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x0600537A RID: 21370 RVA: 0x0013104C File Offset: 0x0013004C
		public static ArrangeDirection ArrangeDirection
		{
			get
			{
				ArrangeDirection arrangeDirection = ArrangeDirection.Down;
				int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(56);
				return arrangeDirection & (ArrangeDirection)systemMetrics;
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x0600537B RID: 21371 RVA: 0x00131066 File Offset: 0x00130066
		public static Size MinimizedWindowSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(57), UnsafeNativeMethods.GetSystemMetrics(58));
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x0600537C RID: 21372 RVA: 0x0013107B File Offset: 0x0013007B
		public static Size MaxWindowTrackSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(59), UnsafeNativeMethods.GetSystemMetrics(60));
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x0600537D RID: 21373 RVA: 0x00131090 File Offset: 0x00130090
		public static Size PrimaryMonitorMaximizedWindowSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(61), UnsafeNativeMethods.GetSystemMetrics(62));
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x0600537E RID: 21374 RVA: 0x001310A5 File Offset: 0x001300A5
		public static bool Network
		{
			get
			{
				return (UnsafeNativeMethods.GetSystemMetrics(63) & 1) != 0;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x0600537F RID: 21375 RVA: 0x001310B6 File Offset: 0x001300B6
		public static bool TerminalServerSession
		{
			get
			{
				return (UnsafeNativeMethods.GetSystemMetrics(4096) & 1) != 0;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06005380 RID: 21376 RVA: 0x001310CA File Offset: 0x001300CA
		public static BootMode BootMode
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				return (BootMode)UnsafeNativeMethods.GetSystemMetrics(67);
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06005381 RID: 21377 RVA: 0x001310DD File Offset: 0x001300DD
		public static Size DragSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(68), UnsafeNativeMethods.GetSystemMetrics(69));
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06005382 RID: 21378 RVA: 0x001310F2 File Offset: 0x001300F2
		public static bool ShowSounds
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(70) != 0;
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06005383 RID: 21379 RVA: 0x00131101 File Offset: 0x00130101
		public static Size MenuCheckSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(71), UnsafeNativeMethods.GetSystemMetrics(72));
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06005384 RID: 21380 RVA: 0x00131116 File Offset: 0x00130116
		public static bool MidEastEnabled
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(74) != 0;
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06005385 RID: 21381 RVA: 0x00131125 File Offset: 0x00130125
		private static bool MultiMonitorSupport
		{
			get
			{
				if (!SystemInformation.checkMultiMonitorSupport)
				{
					SystemInformation.multiMonitorSupport = (UnsafeNativeMethods.GetSystemMetrics(80) != 0);
					SystemInformation.checkMultiMonitorSupport = true;
				}
				return SystemInformation.multiMonitorSupport;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06005386 RID: 21382 RVA: 0x0013114B File Offset: 0x0013014B
		public static bool NativeMouseWheelSupport
		{
			get
			{
				if (!SystemInformation.checkNativeMouseWheelSupport)
				{
					SystemInformation.nativeMouseWheelSupport = (UnsafeNativeMethods.GetSystemMetrics(75) != 0);
					SystemInformation.checkNativeMouseWheelSupport = true;
				}
				return SystemInformation.nativeMouseWheelSupport;
			}
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06005387 RID: 21383 RVA: 0x00131174 File Offset: 0x00130174
		public static bool MouseWheelPresent
		{
			get
			{
				bool result = false;
				if (!SystemInformation.NativeMouseWheelSupport)
				{
					IntPtr value = IntPtr.Zero;
					value = UnsafeNativeMethods.FindWindow("MouseZ", "Magellan MSWHEEL");
					if (value != IntPtr.Zero)
					{
						result = true;
					}
				}
				else
				{
					result = (UnsafeNativeMethods.GetSystemMetrics(75) != 0);
				}
				return result;
			}
		}

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x06005388 RID: 21384 RVA: 0x001311C0 File Offset: 0x001301C0
		public static Rectangle VirtualScreen
		{
			get
			{
				if (SystemInformation.MultiMonitorSupport)
				{
					return new Rectangle(UnsafeNativeMethods.GetSystemMetrics(76), UnsafeNativeMethods.GetSystemMetrics(77), UnsafeNativeMethods.GetSystemMetrics(78), UnsafeNativeMethods.GetSystemMetrics(79));
				}
				Size primaryMonitorSize = SystemInformation.PrimaryMonitorSize;
				return new Rectangle(0, 0, primaryMonitorSize.Width, primaryMonitorSize.Height);
			}
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x06005389 RID: 21385 RVA: 0x00131211 File Offset: 0x00130211
		public static int MonitorCount
		{
			get
			{
				if (SystemInformation.MultiMonitorSupport)
				{
					return UnsafeNativeMethods.GetSystemMetrics(80);
				}
				return 1;
			}
		}

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x0600538A RID: 21386 RVA: 0x00131223 File Offset: 0x00130223
		public static bool MonitorsSameDisplayFormat
		{
			get
			{
				return !SystemInformation.MultiMonitorSupport || UnsafeNativeMethods.GetSystemMetrics(81) != 0;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x0600538B RID: 21387 RVA: 0x0013123C File Offset: 0x0013023C
		public static string ComputerName
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				StringBuilder stringBuilder = new StringBuilder(256);
				UnsafeNativeMethods.GetComputerName(stringBuilder, new int[]
				{
					stringBuilder.Capacity
				});
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x0013127C File Offset: 0x0013027C
		public static string UserDomainName
		{
			get
			{
				return Environment.UserDomainName;
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x0600538D RID: 21389 RVA: 0x00131284 File Offset: 0x00130284
		public static bool UserInteractive
		{
			get
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					IntPtr intPtr = IntPtr.Zero;
					intPtr = UnsafeNativeMethods.GetProcessWindowStation();
					if (intPtr != IntPtr.Zero && SystemInformation.processWinStation != intPtr)
					{
						SystemInformation.isUserInteractive = true;
						int num = 0;
						NativeMethods.USEROBJECTFLAGS userobjectflags = new NativeMethods.USEROBJECTFLAGS();
						if (UnsafeNativeMethods.GetUserObjectInformation(new HandleRef(null, intPtr), 1, userobjectflags, Marshal.SizeOf(userobjectflags), ref num) && (userobjectflags.dwFlags & 1) == 0)
						{
							SystemInformation.isUserInteractive = false;
						}
						SystemInformation.processWinStation = intPtr;
					}
				}
				else
				{
					SystemInformation.isUserInteractive = true;
				}
				return SystemInformation.isUserInteractive;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x0600538E RID: 21390 RVA: 0x00131310 File Offset: 0x00130310
		public static string UserName
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				StringBuilder stringBuilder = new StringBuilder(256);
				UnsafeNativeMethods.GetUserName(stringBuilder, new int[]
				{
					stringBuilder.Capacity
				});
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x00131350 File Offset: 0x00130350
		private static void EnsureSystemEvents()
		{
			if (!SystemInformation.systemEventsAttached)
			{
				SystemEvents.UserPreferenceChanged += SystemInformation.OnUserPreferenceChanged;
				SystemInformation.systemEventsAttached = true;
			}
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x00131370 File Offset: 0x00130370
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			SystemInformation.systemEventsDirty = true;
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x00131378 File Offset: 0x00130378
		public static bool IsDropShadowEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4132, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06005392 RID: 21394 RVA: 0x001313AC File Offset: 0x001303AC
		public static bool IsFlatMenuEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4130, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x001313E0 File Offset: 0x001303E0
		public static bool IsFontSmoothingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(74, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06005394 RID: 21396 RVA: 0x00131404 File Offset: 0x00130404
		public static int FontSmoothingContrast
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(8204, 0, ref result, 0);
					return result;
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x06005395 RID: 21397 RVA: 0x00131440 File Offset: 0x00130440
		public static int FontSmoothingType
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(8202, 0, ref result, 0);
					return result;
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06005396 RID: 21398 RVA: 0x0013147C File Offset: 0x0013047C
		public static int IconHorizontalSpacing
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(13, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06005397 RID: 21399 RVA: 0x00131498 File Offset: 0x00130498
		public static int IconVerticalSpacing
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(24, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x06005398 RID: 21400 RVA: 0x001314B4 File Offset: 0x001304B4
		public static bool IsIconTitleWrappingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(25, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x06005399 RID: 21401 RVA: 0x001314D8 File Offset: 0x001304D8
		public static bool MenuAccessKeysUnderlined
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4106, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x0600539A RID: 21402 RVA: 0x00131500 File Offset: 0x00130500
		public static int KeyboardDelay
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(22, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x0600539B RID: 21403 RVA: 0x0013151C File Offset: 0x0013051C
		public static bool IsKeyboardPreferred
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(68, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x0600539C RID: 21404 RVA: 0x00131540 File Offset: 0x00130540
		public static int KeyboardSpeed
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(10, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x0013155C File Offset: 0x0013055C
		public static Size MouseHoverSize
		{
			get
			{
				int height = 0;
				int width = 0;
				UnsafeNativeMethods.SystemParametersInfo(100, 0, ref height, 0);
				UnsafeNativeMethods.SystemParametersInfo(98, 0, ref width, 0);
				return new Size(width, height);
			}
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x0600539E RID: 21406 RVA: 0x0013158C File Offset: 0x0013058C
		public static int MouseHoverTime
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(102, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x0600539F RID: 21407 RVA: 0x001315A8 File Offset: 0x001305A8
		public static int MouseSpeed
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(112, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x001315C4 File Offset: 0x001305C4
		public static bool IsSnapToDefaultEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(95, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060053A1 RID: 21409 RVA: 0x001315E8 File Offset: 0x001305E8
		public static LeftRightAlignment PopupMenuAlignment
		{
			get
			{
				bool flag = false;
				UnsafeNativeMethods.SystemParametersInfo(27, 0, ref flag, 0);
				if (flag)
				{
					return LeftRightAlignment.Left;
				}
				return LeftRightAlignment.Right;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x0013160C File Offset: 0x0013060C
		public static bool IsMenuFadeEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4114, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060053A3 RID: 21411 RVA: 0x0013164C File Offset: 0x0013064C
		public static int MenuShowDelay
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(106, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060053A4 RID: 21412 RVA: 0x00131668 File Offset: 0x00130668
		public static bool IsComboBoxAnimationEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4100, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060053A5 RID: 21413 RVA: 0x00131690 File Offset: 0x00130690
		public static bool IsTitleBarGradientEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4104, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060053A6 RID: 21414 RVA: 0x001316B8 File Offset: 0x001306B8
		public static bool IsHotTrackingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4110, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060053A7 RID: 21415 RVA: 0x001316E0 File Offset: 0x001306E0
		public static bool IsListBoxSmoothScrollingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4102, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x00131708 File Offset: 0x00130708
		public static bool IsMenuAnimationEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4098, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x060053A9 RID: 21417 RVA: 0x00131730 File Offset: 0x00130730
		public static bool IsSelectionFadeEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4116, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x00131770 File Offset: 0x00130770
		public static bool IsToolTipAnimationEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4118, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001317B0 File Offset: 0x001307B0
		public static bool UIEffectsEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4158, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x001317F0 File Offset: 0x001307F0
		public static bool IsActiveWindowTrackingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4096, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x00131818 File Offset: 0x00130818
		public static int ActiveWindowTrackingDelay
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(8194, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x060053AE RID: 21422 RVA: 0x00131838 File Offset: 0x00130838
		public static bool IsMinimizeRestoreAnimationEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(72, 0, ref num, 0);
				return num != 0;
			}
		}

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x0013185C File Offset: 0x0013085C
		public static int BorderMultiplierFactor
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(5, 0, ref result, 0);
				return result;
			}
		}

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x060053B0 RID: 21424 RVA: 0x00131877 File Offset: 0x00130877
		public static int CaretBlinkTime
		{
			get
			{
				return (int)SafeNativeMethods.GetCaretBlinkTime();
			}
		}

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x060053B1 RID: 21425 RVA: 0x00131880 File Offset: 0x00130880
		public static int CaretWidth
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(8198, 0, ref result, 0);
					return result;
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x060053B2 RID: 21426 RVA: 0x001318C7 File Offset: 0x001308C7
		public static int MouseWheelScrollDelta
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x060053B3 RID: 21427 RVA: 0x001318CB File Offset: 0x001308CB
		public static int VerticalFocusThickness
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					return UnsafeNativeMethods.GetSystemMetrics(84);
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x060053B4 RID: 21428 RVA: 0x001318F0 File Offset: 0x001308F0
		public static int HorizontalFocusThickness
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					return UnsafeNativeMethods.GetSystemMetrics(83);
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x060053B5 RID: 21429 RVA: 0x00131915 File Offset: 0x00130915
		public static int VerticalResizeBorderThickness
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(33);
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x060053B6 RID: 21430 RVA: 0x0013191E File Offset: 0x0013091E
		public static int HorizontalResizeBorderThickness
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(32);
			}
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x060053B7 RID: 21431 RVA: 0x00131928 File Offset: 0x00130928
		public static ScreenOrientation ScreenOrientation
		{
			get
			{
				ScreenOrientation result = ScreenOrientation.Angle0;
				NativeMethods.DEVMODE devmode = default(NativeMethods.DEVMODE);
				devmode.dmSize = (short)Marshal.SizeOf(typeof(NativeMethods.DEVMODE));
				devmode.dmDriverExtra = 0;
				try
				{
					SafeNativeMethods.EnumDisplaySettings(null, -1, ref devmode);
					if ((devmode.dmFields & 128) > 0)
					{
						result = devmode.dmDisplayOrientation;
					}
				}
				catch
				{
				}
				return result;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x060053B8 RID: 21432 RVA: 0x00131998 File Offset: 0x00130998
		public static int SizingBorderWidth
		{
			get
			{
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.iBorderWidth > 0)
				{
					return nonclientmetrics.iBorderWidth;
				}
				return 0;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x060053B9 RID: 21433 RVA: 0x001319D0 File Offset: 0x001309D0
		public static Size SmallCaptionButtonSize
		{
			get
			{
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.iSmCaptionHeight > 0 && nonclientmetrics.iSmCaptionWidth > 0)
				{
					return new Size(nonclientmetrics.iSmCaptionWidth, nonclientmetrics.iSmCaptionHeight);
				}
				return Size.Empty;
			}
		}

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x060053BA RID: 21434 RVA: 0x00131A20 File Offset: 0x00130A20
		public static Size MenuBarButtonSize
		{
			get
			{
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.iMenuHeight > 0 && nonclientmetrics.iMenuWidth > 0)
				{
					return new Size(nonclientmetrics.iMenuWidth, nonclientmetrics.iMenuHeight);
				}
				return Size.Empty;
			}
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x00131A70 File Offset: 0x00130A70
		internal static bool InLockedTerminalSession()
		{
			bool result = false;
			if (SystemInformation.TerminalServerSession)
			{
				IntPtr value = SafeNativeMethods.OpenInputDesktop(0, false, 256);
				if (value == IntPtr.Zero)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					result = (lastWin32Error == 5);
				}
			}
			return result;
		}

		// Token: 0x04003682 RID: 13954
		private const int DefaultMouseWheelScrollLines = 3;

		// Token: 0x04003683 RID: 13955
		private static bool checkMultiMonitorSupport = false;

		// Token: 0x04003684 RID: 13956
		private static bool multiMonitorSupport = false;

		// Token: 0x04003685 RID: 13957
		private static bool checkNativeMouseWheelSupport = false;

		// Token: 0x04003686 RID: 13958
		private static bool nativeMouseWheelSupport = true;

		// Token: 0x04003687 RID: 13959
		private static bool highContrast = false;

		// Token: 0x04003688 RID: 13960
		private static bool systemEventsAttached = false;

		// Token: 0x04003689 RID: 13961
		private static bool systemEventsDirty = true;

		// Token: 0x0400368A RID: 13962
		private static IntPtr processWinStation = IntPtr.Zero;

		// Token: 0x0400368B RID: 13963
		private static bool isUserInteractive = false;

		// Token: 0x0400368C RID: 13964
		private static PowerStatus powerStatus = null;
	}
}
