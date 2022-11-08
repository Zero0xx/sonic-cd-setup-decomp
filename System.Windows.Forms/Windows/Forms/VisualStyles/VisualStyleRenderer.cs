using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms.VisualStyles
{
	// Token: 0x02000894 RID: 2196
	public sealed class VisualStyleRenderer
	{
		// Token: 0x06006CCB RID: 27851 RVA: 0x0018F22A File Offset: 0x0018E22A
		static VisualStyleRenderer()
		{
			SystemEvents.UserPreferenceChanging += VisualStyleRenderer.OnUserPreferenceChanging;
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06006CCC RID: 27852 RVA: 0x0018F25B File Offset: 0x0018E25B
		private static bool AreClientAreaVisualStylesSupported
		{
			get
			{
				return VisualStyleInformation.IsEnabledByUser && (Application.VisualStyleState & VisualStyleState.ClientAreaEnabled) == VisualStyleState.ClientAreaEnabled;
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06006CCD RID: 27853 RVA: 0x0018F270 File Offset: 0x0018E270
		public static bool IsSupported
		{
			get
			{
				bool flag = VisualStyleRenderer.AreClientAreaVisualStylesSupported;
				if (flag)
				{
					IntPtr handle = VisualStyleRenderer.GetHandle("BUTTON", false);
					flag = (handle != IntPtr.Zero);
				}
				return flag;
			}
		}

		// Token: 0x06006CCE RID: 27854 RVA: 0x0018F29F File Offset: 0x0018E29F
		public static bool IsElementDefined(VisualStyleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return VisualStyleRenderer.IsCombinationDefined(element.ClassName, element.Part);
		}

		// Token: 0x06006CCF RID: 27855 RVA: 0x0018F2C0 File Offset: 0x0018E2C0
		private static bool IsCombinationDefined(string className, int part)
		{
			bool flag = false;
			if (!VisualStyleRenderer.IsSupported)
			{
				if (!VisualStyleInformation.IsEnabledByUser)
				{
					throw new InvalidOperationException(SR.GetString("VisualStyleNotActive"));
				}
				throw new InvalidOperationException(SR.GetString("VisualStylesDisabledInClientArea"));
			}
			else
			{
				if (className == null)
				{
					throw new ArgumentNullException("className");
				}
				IntPtr handle = VisualStyleRenderer.GetHandle(className, false);
				if (handle != IntPtr.Zero)
				{
					flag = (part == 0 || SafeNativeMethods.IsThemePartDefined(new HandleRef(null, handle), part, 0));
				}
				if (!flag)
				{
					using (VisualStyleRenderer.ThemeHandle themeHandle = VisualStyleRenderer.ThemeHandle.Create(className, false))
					{
						if (themeHandle != null)
						{
							flag = SafeNativeMethods.IsThemePartDefined(new HandleRef(null, themeHandle.NativeHandle), part, 0);
						}
						if (flag)
						{
							VisualStyleRenderer.RefreshCache();
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x06006CD0 RID: 27856 RVA: 0x0018F380 File Offset: 0x0018E380
		public VisualStyleRenderer(VisualStyleElement element) : this(element.ClassName, element.Part, element.State)
		{
		}

		// Token: 0x06006CD1 RID: 27857 RVA: 0x0018F39A File Offset: 0x0018E39A
		public VisualStyleRenderer(string className, int part, int state)
		{
			if (!VisualStyleRenderer.IsCombinationDefined(className, part))
			{
				throw new ArgumentException(SR.GetString("VisualStylesInvalidCombination"));
			}
			this._class = className;
			this.part = part;
			this.state = state;
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x0018F3D0 File Offset: 0x0018E3D0
		public string Class
		{
			get
			{
				return this._class;
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x0018F3D8 File Offset: 0x0018E3D8
		public int Part
		{
			get
			{
				return this.part;
			}
		}

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x0018F3E0 File Offset: 0x0018E3E0
		public int State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06006CD5 RID: 27861 RVA: 0x0018F3E8 File Offset: 0x0018E3E8
		public IntPtr Handle
		{
			get
			{
				if (VisualStyleRenderer.IsSupported)
				{
					return VisualStyleRenderer.GetHandle(this._class);
				}
				if (!VisualStyleInformation.IsEnabledByUser)
				{
					throw new InvalidOperationException(SR.GetString("VisualStyleNotActive"));
				}
				throw new InvalidOperationException(SR.GetString("VisualStylesDisabledInClientArea"));
			}
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x0018F423 File Offset: 0x0018E423
		public void SetParameters(VisualStyleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.SetParameters(element.ClassName, element.Part, element.State);
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x0018F44B File Offset: 0x0018E44B
		public void SetParameters(string className, int part, int state)
		{
			if (!VisualStyleRenderer.IsCombinationDefined(className, part))
			{
				throw new ArgumentException(SR.GetString("VisualStylesInvalidCombination"));
			}
			this._class = className;
			this.part = part;
			this.state = state;
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x0018F47C File Offset: 0x0018E47C
		public void DrawBackground(IDeviceContext dc, Rectangle bounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.DrawThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), null);
			}
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x0018F518 File Offset: 0x0018E518
		public void DrawBackground(IDeviceContext dc, Rectangle bounds, Rectangle clipRectangle)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			if (clipRectangle.Width < 0 || clipRectangle.Height < 0)
			{
				return;
			}
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.DrawThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), new NativeMethods.COMRECT(clipRectangle));
			}
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x0018F5D0 File Offset: 0x0018E5D0
		public Rectangle DrawEdge(IDeviceContext dc, Rectangle bounds, Edges edges, EdgeStyle style, EdgeEffects effects)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid_Masked(edges, (int)edges, 31U))
			{
				throw new InvalidEnumArgumentException("edges", (int)edges, typeof(Edges));
			}
			if (!ClientUtils.IsEnumValid_NotSequential(style, (int)style, new int[]
			{
				5,
				10,
				6,
				9
			}))
			{
				throw new InvalidEnumArgumentException("style", (int)style, typeof(EdgeStyle));
			}
			if (!ClientUtils.IsEnumValid_Masked(effects, (int)effects, 55296U))
			{
				throw new InvalidEnumArgumentException("effects", (int)effects, typeof(EdgeEffects));
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.DrawThemeEdge(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), (int)style, (int)(edges | (Edges)effects | (Edges)8192), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x0018F718 File Offset: 0x0018E718
		public void DrawImage(Graphics g, Rectangle bounds, Image image)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			ImageList imageList = new ImageList();
			try
			{
				imageList.Images.Add(image);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				g.DrawImage(image, bounds);
				return;
			}
			this.DrawImage(g, bounds, imageList, 0);
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x0018F79C File Offset: 0x0018E79C
		public void DrawImage(Graphics g, Rectangle bounds, ImageList imageList, int imageIndex)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (imageList == null)
			{
				throw new ArgumentNullException("imageList");
			}
			if (imageIndex < 0 || imageIndex >= imageList.Images.Count)
			{
				throw new ArgumentOutOfRangeException("imageIndex", SR.GetString("InvalidArgument", new object[]
				{
					"imageIndex",
					imageIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			g.DrawImage(imageList.Images[imageIndex], bounds);
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x0018F838 File Offset: 0x0018E838
		public void DrawParentBackground(IDeviceContext dc, Rectangle bounds, Control childControl)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (childControl == null)
			{
				throw new ArgumentNullException("childControl");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			if (childControl.Handle != IntPtr.Zero)
			{
				using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
				{
					HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
					this.lastHResult = SafeNativeMethods.DrawThemeParentBackground(new HandleRef(this, childControl.Handle), hdc, new NativeMethods.COMRECT(bounds));
				}
			}
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x0018F8E8 File Offset: 0x0018E8E8
		public void DrawText(IDeviceContext dc, Rectangle bounds, string textToDraw)
		{
			this.DrawText(dc, bounds, textToDraw, false);
		}

		// Token: 0x06006CDF RID: 27871 RVA: 0x0018F8F4 File Offset: 0x0018E8F4
		public void DrawText(IDeviceContext dc, Rectangle bounds, string textToDraw, bool drawDisabled)
		{
			this.DrawText(dc, bounds, textToDraw, drawDisabled, TextFormatFlags.HorizontalCenter);
		}

		// Token: 0x06006CE0 RID: 27872 RVA: 0x0018F904 File Offset: 0x0018E904
		public void DrawText(IDeviceContext dc, Rectangle bounds, string textToDraw, bool drawDisabled, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			int dwTextFlags = drawDisabled ? 1 : 0;
			if (!string.IsNullOrEmpty(textToDraw))
			{
				using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
				{
					HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
					this.lastHResult = SafeNativeMethods.DrawThemeText(new HandleRef(this, this.Handle), hdc, this.part, this.state, textToDraw, textToDraw.Length, (int)flags, dwTextFlags, new NativeMethods.COMRECT(bounds));
				}
			}
		}

		// Token: 0x06006CE1 RID: 27873 RVA: 0x0018F9BC File Offset: 0x0018E9BC
		public Rectangle GetBackgroundContentRectangle(IDeviceContext dc, Rectangle bounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeBackgroundContentRect(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x0018FA80 File Offset: 0x0018EA80
		public Rectangle GetBackgroundExtent(IDeviceContext dc, Rectangle contentBounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (contentBounds.Width < 0 || contentBounds.Height < 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeBackgroundExtent(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(contentBounds), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x0018FB44 File Offset: 0x0018EB44
		[SuppressUnmanagedCodeSecurity]
		public Region GetBackgroundRegion(IDeviceContext dc, Rectangle bounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeBackgroundRegion(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), ref zero);
			}
			if (zero == IntPtr.Zero)
			{
				return null;
			}
			Region result = Region.FromHrgn(zero);
			SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, zero));
			return result;
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x0018FC0C File Offset: 0x0018EC0C
		public bool GetBoolean(BooleanProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 2201, 2213))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(BooleanProperty));
			}
			bool result = false;
			this.lastHResult = SafeNativeMethods.GetThemeBool(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref result);
			return result;
		}

		// Token: 0x06006CE5 RID: 27877 RVA: 0x0018FC70 File Offset: 0x0018EC70
		public Color GetColor(ColorProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3801, 3823))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(ColorProperty));
			}
			int win32Color = 0;
			this.lastHResult = SafeNativeMethods.GetThemeColor(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref win32Color);
			return ColorTranslator.FromWin32(win32Color);
		}

		// Token: 0x06006CE6 RID: 27878 RVA: 0x0018FCDC File Offset: 0x0018ECDC
		public int GetEnumValue(EnumProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 4001, 4015))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(EnumProperty));
			}
			int result = 0;
			this.lastHResult = SafeNativeMethods.GetThemeEnumValue(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref result);
			return result;
		}

		// Token: 0x06006CE7 RID: 27879 RVA: 0x0018FD40 File Offset: 0x0018ED40
		public string GetFilename(FilenameProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3001, 3008))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(FilenameProperty));
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			this.lastHResult = SafeNativeMethods.GetThemeFilename(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		// Token: 0x06006CE8 RID: 27880 RVA: 0x0018FDB8 File Offset: 0x0018EDB8
		public Font GetFont(IDeviceContext dc, FontProperty prop)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 2601, 2601))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(FontProperty));
			}
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeFont(new HandleRef(this, this.Handle), hdc, this.part, this.state, (int)prop, logfont);
			}
			Font result = null;
			if (NativeMethods.Succeeded(this.lastHResult))
			{
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					result = Font.FromLogFont(logfont);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06006CE9 RID: 27881 RVA: 0x0018FEB0 File Offset: 0x0018EEB0
		public int GetInteger(IntegerProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 2401, 2424))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(IntegerProperty));
			}
			int result = 0;
			this.lastHResult = SafeNativeMethods.GetThemeInt(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref result);
			return result;
		}

		// Token: 0x06006CEA RID: 27882 RVA: 0x0018FF14 File Offset: 0x0018EF14
		public Size GetPartSize(IDeviceContext dc, ThemeSizeType type)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(type, (int)type, 0, 2))
			{
				throw new InvalidEnumArgumentException("type", (int)type, typeof(ThemeSizeType));
			}
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemePartSize(new HandleRef(this, this.Handle), hdc, this.part, this.state, null, type, size);
			}
			return new Size(size.cx, size.cy);
		}

		// Token: 0x06006CEB RID: 27883 RVA: 0x0018FFD4 File Offset: 0x0018EFD4
		public Size GetPartSize(IDeviceContext dc, Rectangle bounds, ThemeSizeType type)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(type, (int)type, 0, 2))
			{
				throw new InvalidEnumArgumentException("type", (int)type, typeof(ThemeSizeType));
			}
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemePartSize(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), type, size);
			}
			return new Size(size.cx, size.cy);
		}

		// Token: 0x06006CEC RID: 27884 RVA: 0x00190098 File Offset: 0x0018F098
		public Point GetPoint(PointProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3401, 3408))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(PointProperty));
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			this.lastHResult = SafeNativeMethods.GetThemePosition(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, point);
			return new Point(point.x, point.y);
		}

		// Token: 0x06006CED RID: 27885 RVA: 0x00190110 File Offset: 0x0018F110
		public Padding GetMargins(IDeviceContext dc, MarginProperty prop)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3601, 3603))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(MarginProperty));
			}
			NativeMethods.MARGINS margins = default(NativeMethods.MARGINS);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hDC = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeMargins(new HandleRef(this, this.Handle), hDC, this.part, this.state, (int)prop, ref margins);
			}
			return new Padding(margins.cxLeftWidth, margins.cyTopHeight, margins.cxRightWidth, margins.cyBottomHeight);
		}

		// Token: 0x06006CEE RID: 27886 RVA: 0x001901EC File Offset: 0x0018F1EC
		public string GetString(StringProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3201, 3201))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(StringProperty));
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			this.lastHResult = SafeNativeMethods.GetThemeString(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		// Token: 0x06006CEF RID: 27887 RVA: 0x00190264 File Offset: 0x0018F264
		public Rectangle GetTextExtent(IDeviceContext dc, string textToDraw, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(textToDraw))
			{
				throw new ArgumentNullException("textToDraw");
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeTextExtent(new HandleRef(this, this.Handle), hdc, this.part, this.state, textToDraw, textToDraw.Length, (int)flags, null, comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		// Token: 0x06006CF0 RID: 27888 RVA: 0x00190324 File Offset: 0x0018F324
		public Rectangle GetTextExtent(IDeviceContext dc, Rectangle bounds, string textToDraw, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(textToDraw))
			{
				throw new ArgumentNullException("textToDraw");
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeTextExtent(new HandleRef(this, this.Handle), hdc, this.part, this.state, textToDraw, textToDraw.Length, (int)flags, new NativeMethods.COMRECT(bounds), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		// Token: 0x06006CF1 RID: 27889 RVA: 0x001903EC File Offset: 0x0018F3EC
		public TextMetrics GetTextMetrics(IDeviceContext dc)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			TextMetrics result = default(TextMetrics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeTextMetrics(new HandleRef(this, this.Handle), hdc, this.part, this.state, ref result);
			}
			return result;
		}

		// Token: 0x06006CF2 RID: 27890 RVA: 0x00190478 File Offset: 0x0018F478
		public HitTestCode HitTestBackground(IDeviceContext dc, Rectangle backgroundRectangle, Point pt, HitTestOptions options)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			int result = 0;
			NativeMethods.POINTSTRUCT ptTest = new NativeMethods.POINTSTRUCT(pt.X, pt.Y);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.HitTestThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, (int)options, new NativeMethods.COMRECT(backgroundRectangle), NativeMethods.NullHandleRef, ptTest, ref result);
			}
			return (HitTestCode)result;
		}

		// Token: 0x06006CF3 RID: 27891 RVA: 0x00190520 File Offset: 0x0018F520
		public HitTestCode HitTestBackground(Graphics g, Rectangle backgroundRectangle, Region region, Point pt, HitTestOptions options)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			IntPtr hrgn = region.GetHrgn(g);
			return this.HitTestBackground(g, backgroundRectangle, hrgn, pt, options);
		}

		// Token: 0x06006CF4 RID: 27892 RVA: 0x00190550 File Offset: 0x0018F550
		public HitTestCode HitTestBackground(IDeviceContext dc, Rectangle backgroundRectangle, IntPtr hRgn, Point pt, HitTestOptions options)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			int result = 0;
			NativeMethods.POINTSTRUCT ptTest = new NativeMethods.POINTSTRUCT(pt.X, pt.Y);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.HitTestThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, (int)options, new NativeMethods.COMRECT(backgroundRectangle), new HandleRef(this, hRgn), ptTest, ref result);
			}
			return (HitTestCode)result;
		}

		// Token: 0x06006CF5 RID: 27893 RVA: 0x001905FC File Offset: 0x0018F5FC
		public bool IsBackgroundPartiallyTransparent()
		{
			return SafeNativeMethods.IsThemeBackgroundPartiallyTransparent(new HandleRef(this, this.Handle), this.part, this.state);
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06006CF6 RID: 27894 RVA: 0x0019061B File Offset: 0x0018F61B
		public int LastHResult
		{
			get
			{
				return this.lastHResult;
			}
		}

		// Token: 0x06006CF7 RID: 27895 RVA: 0x00190623 File Offset: 0x0018F623
		private static void CreateThemeHandleHashtable()
		{
			VisualStyleRenderer.themeHandles = new Hashtable(VisualStyleRenderer.numberOfPossibleClasses);
		}

		// Token: 0x06006CF8 RID: 27896 RVA: 0x00190634 File Offset: 0x0018F634
		private static void OnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs ea)
		{
			if (ea.Category == UserPreferenceCategory.VisualStyle)
			{
				VisualStyleRenderer.globalCacheVersion += 1L;
			}
		}

		// Token: 0x06006CF9 RID: 27897 RVA: 0x00190650 File Offset: 0x0018F650
		private static void RefreshCache()
		{
			if (VisualStyleRenderer.themeHandles != null)
			{
				string[] array = new string[VisualStyleRenderer.themeHandles.Keys.Count];
				VisualStyleRenderer.themeHandles.Keys.CopyTo(array, 0);
				foreach (string text in array)
				{
					VisualStyleRenderer.ThemeHandle themeHandle = (VisualStyleRenderer.ThemeHandle)VisualStyleRenderer.themeHandles[text];
					if (themeHandle != null)
					{
						themeHandle.Dispose();
					}
					if (VisualStyleRenderer.AreClientAreaVisualStylesSupported)
					{
						themeHandle = VisualStyleRenderer.ThemeHandle.Create(text, false);
						if (themeHandle != null)
						{
							VisualStyleRenderer.themeHandles[text] = themeHandle;
						}
					}
				}
			}
		}

		// Token: 0x06006CFA RID: 27898 RVA: 0x001906DD File Offset: 0x0018F6DD
		private static IntPtr GetHandle(string className)
		{
			return VisualStyleRenderer.GetHandle(className, true);
		}

		// Token: 0x06006CFB RID: 27899 RVA: 0x001906E8 File Offset: 0x0018F6E8
		private static IntPtr GetHandle(string className, bool throwExceptionOnFail)
		{
			if (VisualStyleRenderer.themeHandles == null)
			{
				VisualStyleRenderer.CreateThemeHandleHashtable();
			}
			if (VisualStyleRenderer.threadCacheVersion != VisualStyleRenderer.globalCacheVersion)
			{
				VisualStyleRenderer.RefreshCache();
				VisualStyleRenderer.threadCacheVersion = VisualStyleRenderer.globalCacheVersion;
			}
			VisualStyleRenderer.ThemeHandle themeHandle;
			if (!VisualStyleRenderer.themeHandles.Contains(className))
			{
				themeHandle = VisualStyleRenderer.ThemeHandle.Create(className, throwExceptionOnFail);
				if (themeHandle == null)
				{
					return IntPtr.Zero;
				}
				VisualStyleRenderer.themeHandles.Add(className, themeHandle);
			}
			else
			{
				themeHandle = (VisualStyleRenderer.ThemeHandle)VisualStyleRenderer.themeHandles[className];
			}
			return themeHandle.NativeHandle;
		}

		// Token: 0x040040B2 RID: 16562
		private const TextFormatFlags AllGraphicsProperties = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform;

		// Token: 0x040040B3 RID: 16563
		internal const int EdgeAdjust = 8192;

		// Token: 0x040040B4 RID: 16564
		private string _class;

		// Token: 0x040040B5 RID: 16565
		private int part;

		// Token: 0x040040B6 RID: 16566
		private int state;

		// Token: 0x040040B7 RID: 16567
		private int lastHResult;

		// Token: 0x040040B8 RID: 16568
		private static int numberOfPossibleClasses = VisualStyleElement.Count;

		// Token: 0x040040B9 RID: 16569
		[ThreadStatic]
		private static Hashtable themeHandles = null;

		// Token: 0x040040BA RID: 16570
		[ThreadStatic]
		private static long threadCacheVersion = 0L;

		// Token: 0x040040BB RID: 16571
		private static long globalCacheVersion = 0L;

		// Token: 0x02000895 RID: 2197
		private class ThemeHandle : IDisposable
		{
			// Token: 0x06006CFC RID: 27900 RVA: 0x0019075F File Offset: 0x0018F75F
			private ThemeHandle(IntPtr hTheme)
			{
				this._hTheme = hTheme;
			}

			// Token: 0x17001863 RID: 6243
			// (get) Token: 0x06006CFD RID: 27901 RVA: 0x00190779 File Offset: 0x0018F779
			public IntPtr NativeHandle
			{
				get
				{
					return this._hTheme;
				}
			}

			// Token: 0x06006CFE RID: 27902 RVA: 0x00190784 File Offset: 0x0018F784
			public static VisualStyleRenderer.ThemeHandle Create(string className, bool throwExceptionOnFail)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = SafeNativeMethods.OpenThemeData(new HandleRef(null, IntPtr.Zero), className);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					if (throwExceptionOnFail)
					{
						throw new InvalidOperationException(SR.GetString("VisualStyleHandleCreationFailed"), ex);
					}
					return null;
				}
				if (!(intPtr == IntPtr.Zero))
				{
					return new VisualStyleRenderer.ThemeHandle(intPtr);
				}
				if (throwExceptionOnFail)
				{
					throw new InvalidOperationException(SR.GetString("VisualStyleHandleCreationFailed"));
				}
				return null;
			}

			// Token: 0x06006CFF RID: 27903 RVA: 0x00190808 File Offset: 0x0018F808
			public void Dispose()
			{
				if (this._hTheme != IntPtr.Zero)
				{
					SafeNativeMethods.CloseThemeData(new HandleRef(null, this._hTheme));
					this._hTheme = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x06006D00 RID: 27904 RVA: 0x00190840 File Offset: 0x0018F840
			~ThemeHandle()
			{
				this.Dispose();
			}

			// Token: 0x040040BC RID: 16572
			private IntPtr _hTheme = IntPtr.Zero;
		}
	}
}
