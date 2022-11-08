using System;
using System.Drawing;
using System.Internal;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020005F4 RID: 1524
	[SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		// Token: 0x06004F95 RID: 20373
		[DllImport("shlwapi.dll")]
		public static extern int SHAutoComplete(HandleRef hwndEdit, int flags);

		// Token: 0x06004F96 RID: 20374
		[DllImport("user32.dll")]
		public static extern int OemKeyScan(short wAsciiVal);

		// Token: 0x06004F97 RID: 20375
		[DllImport("gdi32.dll")]
		public static extern int GetSystemPaletteEntries(HandleRef hdc, int iStartIndex, int nEntries, byte[] lppe);

		// Token: 0x06004F98 RID: 20376
		[DllImport("gdi32.dll")]
		public static extern int GetDIBits(HandleRef hdc, HandleRef hbm, int uStartScan, int cScanLines, byte[] lpvBits, ref NativeMethods.BITMAPINFO_FLAT bmi, int uUsage);

		// Token: 0x06004F99 RID: 20377
		[DllImport("gdi32.dll")]
		public static extern int StretchDIBits(HandleRef hdc, int XDest, int YDest, int nDestWidth, int nDestHeight, int XSrc, int YSrc, int nSrcWidth, int nSrcHeight, byte[] lpBits, ref NativeMethods.BITMAPINFO_FLAT lpBitsInfo, int iUsage, int dwRop);

		// Token: 0x06004F9A RID: 20378
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleBitmap", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleBitmap(HandleRef hDC, int width, int height);

		// Token: 0x06004F9B RID: 20379 RVA: 0x00126411 File Offset: 0x00125411
		public static IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateCompatibleBitmap(hDC, width, height), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004F9C RID: 20380
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, [In] [Out] NativeMethods.SCROLLINFO si);

		// Token: 0x06004F9D RID: 20381
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsAccelerator(HandleRef hAccel, int cAccelEntries, [In] ref NativeMethods.MSG lpMsg, short[] lpwCmd);

		// Token: 0x06004F9E RID: 20382
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ChooseFont([In] [Out] NativeMethods.CHOOSEFONT cf);

		// Token: 0x06004F9F RID: 20383
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBitmapBits(HandleRef hbmp, int cbBuffer, byte[] lpvBits);

		// Token: 0x06004FA0 RID: 20384
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int CommDlgExtendedError();

		// Token: 0x06004FA1 RID: 20385
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern void SysFreeString(HandleRef bstr);

		// Token: 0x06004FA2 RID: 20386
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void OleCreatePropertyFrame(HandleRef hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int objects, [MarshalAs(UnmanagedType.Interface)] ref object pobjs, int pages, HandleRef pClsid, int locale, int reserved1, IntPtr reserved2);

		// Token: 0x06004FA3 RID: 20387
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void OleCreatePropertyFrame(HandleRef hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int objects, [MarshalAs(UnmanagedType.Interface)] ref object pobjs, int pages, Guid[] pClsid, int locale, int reserved1, IntPtr reserved2);

		// Token: 0x06004FA4 RID: 20388
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void OleCreatePropertyFrame(HandleRef hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int objects, HandleRef lplpobjs, int pages, HandleRef pClsid, int locale, int reserved1, IntPtr reserved2);

		// Token: 0x06004FA5 RID: 20389
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, int dwData);

		// Token: 0x06004FA6 RID: 20390
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, string dwData);

		// Token: 0x06004FA7 RID: 20391
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.HH_POPUP dwData);

		// Token: 0x06004FA8 RID: 20392
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.HH_FTS_QUERY dwData);

		// Token: 0x06004FA9 RID: 20393
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.HH_AKLINK dwData);

		// Token: 0x06004FAA RID: 20394
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void VariantInit(HandleRef pObject);

		// Token: 0x06004FAB RID: 20395
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void VariantClear(HandleRef pObject);

		// Token: 0x06004FAC RID: 20396
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool LineTo(HandleRef hdc, int x, int y);

		// Token: 0x06004FAD RID: 20397
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool MoveToEx(HandleRef hdc, int x, int y, NativeMethods.POINT pt);

		// Token: 0x06004FAE RID: 20398
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom);

		// Token: 0x06004FAF RID: 20399
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool PatBlt(HandleRef hdc, int left, int top, int width, int height, int rop);

		// Token: 0x06004FB0 RID: 20400
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetThreadLocale")]
		public static extern int GetThreadLCID();

		// Token: 0x06004FB1 RID: 20401
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMessagePos();

		// Token: 0x06004FB2 RID: 20402
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int RegisterClipboardFormat(string format);

		// Token: 0x06004FB3 RID: 20403
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetClipboardFormatName(int format, StringBuilder lpString, int cchMax);

		// Token: 0x06004FB4 RID: 20404
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ChooseColor([In] [Out] NativeMethods.CHOOSECOLOR cc);

		// Token: 0x06004FB5 RID: 20405
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int RegisterWindowMessage(string msg);

		// Token: 0x06004FB6 RID: 20406
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		public static extern bool ExternalDeleteObject(HandleRef hObject);

		// Token: 0x06004FB7 RID: 20407
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		internal static extern bool IntDeleteObject(HandleRef hObject);

		// Token: 0x06004FB8 RID: 20408 RVA: 0x00126425 File Offset: 0x00125425
		public static bool DeleteObject(HandleRef hObject)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hObject, NativeMethods.CommonHandles.GDI);
			return SafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x06004FB9 RID: 20409
		[DllImport("oleaut32.dll", EntryPoint = "OleCreateFontIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern SafeNativeMethods.IFontDisp OleCreateIFontDispIndirect(NativeMethods.FONTDESC fd, ref Guid iid);

		// Token: 0x06004FBA RID: 20410
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateSolidBrush(int crColor);

		// Token: 0x06004FBB RID: 20411 RVA: 0x0012643E File Offset: 0x0012543E
		public static IntPtr CreateSolidBrush(int crColor)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateSolidBrush(crColor), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FBC RID: 20412
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetWindowExtEx(HandleRef hDC, int x, int y, [In] [Out] NativeMethods.SIZE size);

		// Token: 0x06004FBD RID: 20413
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, HandleRef arguments);

		// Token: 0x06004FBE RID: 20414
		[DllImport("comctl32.dll")]
		public static extern void InitCommonControls();

		// Token: 0x06004FBF RID: 20415
		[DllImport("comctl32.dll")]
		public static extern bool InitCommonControlsEx(NativeMethods.INITCOMMONCONTROLSEX icc);

		// Token: 0x06004FC0 RID: 20416
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_Create(int cx, int cy, int flags, int cInitial, int cGrow);

		// Token: 0x06004FC1 RID: 20417
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Destroy(HandleRef himl);

		// Token: 0x06004FC2 RID: 20418
		[DllImport("comctl32.dll")]
		public static extern int ImageList_GetImageCount(HandleRef himl);

		// Token: 0x06004FC3 RID: 20419
		[DllImport("comctl32.dll")]
		public static extern int ImageList_Add(HandleRef himl, HandleRef hbmImage, HandleRef hbmMask);

		// Token: 0x06004FC4 RID: 20420
		[DllImport("comctl32.dll")]
		public static extern int ImageList_ReplaceIcon(HandleRef himl, int index, HandleRef hicon);

		// Token: 0x06004FC5 RID: 20421
		[DllImport("comctl32.dll")]
		public static extern int ImageList_SetBkColor(HandleRef himl, int clrBk);

		// Token: 0x06004FC6 RID: 20422
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Draw(HandleRef himl, int i, HandleRef hdcDst, int x, int y, int fStyle);

		// Token: 0x06004FC7 RID: 20423
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Replace(HandleRef himl, int i, HandleRef hbmImage, HandleRef hbmMask);

		// Token: 0x06004FC8 RID: 20424
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_DrawEx(HandleRef himl, int i, HandleRef hdcDst, int x, int y, int dx, int dy, int rgbBk, int rgbFg, int fStyle);

		// Token: 0x06004FC9 RID: 20425
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_GetIconSize(HandleRef himl, out int x, out int y);

		// Token: 0x06004FCA RID: 20426
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_Duplicate(HandleRef himl);

		// Token: 0x06004FCB RID: 20427
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Remove(HandleRef himl, int i);

		// Token: 0x06004FCC RID: 20428
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_GetImageInfo(HandleRef himl, int i, NativeMethods.IMAGEINFO pImageInfo);

		// Token: 0x06004FCD RID: 20429
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_Read(UnsafeNativeMethods.IStream pstm);

		// Token: 0x06004FCE RID: 20430
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Write(HandleRef himl, UnsafeNativeMethods.IStream pstm);

		// Token: 0x06004FCF RID: 20431
		[DllImport("comctl32.dll")]
		public static extern int ImageList_WriteEx(HandleRef himl, int dwFlags, UnsafeNativeMethods.IStream pstm);

		// Token: 0x06004FD0 RID: 20432
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TrackPopupMenuEx(HandleRef hmenu, int fuFlags, int x, int y, HandleRef hwnd, NativeMethods.TPMPARAMS tpm);

		// Token: 0x06004FD1 RID: 20433
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetKeyboardLayout(int dwLayout);

		// Token: 0x06004FD2 RID: 20434
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr ActivateKeyboardLayout(HandleRef hkl, int uFlags);

		// Token: 0x06004FD3 RID: 20435
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetKeyboardLayoutList(int size, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] hkls);

		// Token: 0x06004FD4 RID: 20436
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref NativeMethods.DEVMODE lpDevMode);

		// Token: 0x06004FD5 RID: 20437
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(HandleRef hmonitor, [In] [Out] NativeMethods.MONITORINFOEX info);

		// Token: 0x06004FD6 RID: 20438
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromPoint(NativeMethods.POINTSTRUCT pt, int flags);

		// Token: 0x06004FD7 RID: 20439
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromRect(ref NativeMethods.RECT rect, int flags);

		// Token: 0x06004FD8 RID: 20440
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

		// Token: 0x06004FD9 RID: 20441
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool EnumDisplayMonitors(HandleRef hdc, NativeMethods.COMRECT rcClip, NativeMethods.MonitorEnumProc lpfnEnum, IntPtr dwData);

		// Token: 0x06004FDA RID: 20442
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateHalftonePalette", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateHalftonePalette(HandleRef hdc);

		// Token: 0x06004FDB RID: 20443 RVA: 0x00126450 File Offset: 0x00125450
		public static IntPtr CreateHalftonePalette(HandleRef hdc)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateHalftonePalette(hdc), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FDC RID: 20444
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetPaletteEntries(HandleRef hpal, int iStartIndex, int nEntries, int[] lppe);

		// Token: 0x06004FDD RID: 20445
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref NativeMethods.TEXTMETRIC lptm);

		// Token: 0x06004FDE RID: 20446
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref NativeMethods.TEXTMETRICA lptm);

		// Token: 0x06004FDF RID: 20447 RVA: 0x00126464 File Offset: 0x00125464
		public static int GetTextMetrics(HandleRef hDC, ref NativeMethods.TEXTMETRIC lptm)
		{
			if (Marshal.SystemDefaultCharSize == 1)
			{
				NativeMethods.TEXTMETRICA textmetrica = default(NativeMethods.TEXTMETRICA);
				int textMetricsA = SafeNativeMethods.GetTextMetricsA(hDC, ref textmetrica);
				lptm.tmHeight = textmetrica.tmHeight;
				lptm.tmAscent = textmetrica.tmAscent;
				lptm.tmDescent = textmetrica.tmDescent;
				lptm.tmInternalLeading = textmetrica.tmInternalLeading;
				lptm.tmExternalLeading = textmetrica.tmExternalLeading;
				lptm.tmAveCharWidth = textmetrica.tmAveCharWidth;
				lptm.tmMaxCharWidth = textmetrica.tmMaxCharWidth;
				lptm.tmWeight = textmetrica.tmWeight;
				lptm.tmOverhang = textmetrica.tmOverhang;
				lptm.tmDigitizedAspectX = textmetrica.tmDigitizedAspectX;
				lptm.tmDigitizedAspectY = textmetrica.tmDigitizedAspectY;
				lptm.tmFirstChar = (char)textmetrica.tmFirstChar;
				lptm.tmLastChar = (char)textmetrica.tmLastChar;
				lptm.tmDefaultChar = (char)textmetrica.tmDefaultChar;
				lptm.tmBreakChar = (char)textmetrica.tmBreakChar;
				lptm.tmItalic = textmetrica.tmItalic;
				lptm.tmUnderlined = textmetrica.tmUnderlined;
				lptm.tmStruckOut = textmetrica.tmStruckOut;
				lptm.tmPitchAndFamily = textmetrica.tmPitchAndFamily;
				lptm.tmCharSet = textmetrica.tmCharSet;
				return textMetricsA;
			}
			return SafeNativeMethods.GetTextMetricsW(hDC, ref lptm);
		}

		// Token: 0x06004FE0 RID: 20448
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDIBSection", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateDIBSection(HandleRef hdc, HandleRef pbmi, int iUsage, byte[] ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x06004FE1 RID: 20449 RVA: 0x0012659A File Offset: 0x0012559A
		public static IntPtr CreateDIBSection(HandleRef hdc, HandleRef pbmi, int iUsage, byte[] ppvBits, IntPtr hSection, int dwOffset)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateDIBSection(hdc, pbmi, iUsage, ppvBits, hSection, dwOffset), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FE2 RID: 20450
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, IntPtr lpvBits);

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001265B3 File Offset: 0x001255B3
		public static IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, IntPtr lpvBits)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmap(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FE4 RID: 20452
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBitmapShort(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits);

		// Token: 0x06004FE5 RID: 20453 RVA: 0x001265CA File Offset: 0x001255CA
		public static IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmapShort(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FE6 RID: 20454
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBitmapByte(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, byte[] lpvBits);

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001265E1 File Offset: 0x001255E1
		public static IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, byte[] lpvBits)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmapByte(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FE8 RID: 20456
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePatternBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePatternBrush(HandleRef hbmp);

		// Token: 0x06004FE9 RID: 20457 RVA: 0x001265F8 File Offset: 0x001255F8
		public static IntPtr CreatePatternBrush(HandleRef hbmp)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreatePatternBrush(hbmp), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FEA RID: 20458
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBrushIndirect", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBrushIndirect(NativeMethods.LOGBRUSH lb);

		// Token: 0x06004FEB RID: 20459 RVA: 0x0012660A File Offset: 0x0012560A
		public static IntPtr CreateBrushIndirect(NativeMethods.LOGBRUSH lb)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBrushIndirect(lb), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FEC RID: 20460
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePen(int nStyle, int nWidth, int crColor);

		// Token: 0x06004FED RID: 20461 RVA: 0x0012661C File Offset: 0x0012561C
		public static IntPtr CreatePen(int nStyle, int nWidth, int crColor)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreatePen(nStyle, nWidth, crColor), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FEE RID: 20462
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetViewportExtEx(HandleRef hDC, int x, int y, NativeMethods.SIZE size);

		// Token: 0x06004FEF RID: 20463
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr LoadCursor(HandleRef hInst, int iconId);

		// Token: 0x06004FF0 RID: 20464
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClipCursor([In] [Out] ref NativeMethods.RECT lpRect);

		// Token: 0x06004FF1 RID: 20465
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCursor();

		// Token: 0x06004FF2 RID: 20466
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetIconInfo(HandleRef hIcon, [In] [Out] NativeMethods.ICONINFO info);

		// Token: 0x06004FF3 RID: 20467
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int IntersectClipRect(HandleRef hDC, int x1, int y1, int x2, int y2);

		// Token: 0x06004FF4 RID: 20468
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CopyImage", ExactSpelling = true)]
		private static extern IntPtr IntCopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags);

		// Token: 0x06004FF5 RID: 20469 RVA: 0x00126630 File Offset: 0x00125630
		public static IntPtr CopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCopyImage(hImage, uType, cxDesired, cyDesired, fuFlags), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x00126647 File Offset: 0x00125647
		public static IntPtr CopyImageAsCursor(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCopyImage(hImage, uType, cxDesired, cyDesired, fuFlags), NativeMethods.CommonHandles.Cursor);
		}

		// Token: 0x06004FF7 RID: 20471
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool AdjustWindowRectEx(ref NativeMethods.RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

		// Token: 0x06004FF8 RID: 20472
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int DoDragDrop(IDataObject dataObject, UnsafeNativeMethods.IOleDropSource dropSource, int allowedEffects, int[] finalEffect);

		// Token: 0x06004FF9 RID: 20473
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetSysColorBrush(int nIndex);

		// Token: 0x06004FFA RID: 20474
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnableWindow(HandleRef hWnd, bool enable);

		// Token: 0x06004FFB RID: 20475
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x06004FFC RID: 20476
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetDoubleClickTime();

		// Token: 0x06004FFD RID: 20477
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetUpdateRgn(HandleRef hwnd, HandleRef hrgn, bool fErase);

		// Token: 0x06004FFE RID: 20478
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ValidateRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x06004FFF RID: 20479
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int FillRect(HandleRef hdc, [In] ref NativeMethods.RECT rect, HandleRef hbrush);

		// Token: 0x06005000 RID: 20480
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetTextColor(HandleRef hDC);

		// Token: 0x06005001 RID: 20481
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBkColor(HandleRef hDC);

		// Token: 0x06005002 RID: 20482
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextColor(HandleRef hDC, int crColor);

		// Token: 0x06005003 RID: 20483
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);

		// Token: 0x06005004 RID: 20484
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectPalette(HandleRef hdc, HandleRef hpal, int bForceBackground);

		// Token: 0x06005005 RID: 20485
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] NativeMethods.POINT point);

		// Token: 0x06005006 RID: 20486
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x06005007 RID: 20487 RVA: 0x0012665E File Offset: 0x0012565E
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06005008 RID: 20488
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int CombineRgn(HandleRef hRgn, HandleRef hRgn1, HandleRef hRgn2, int nCombineMode);

		// Token: 0x06005009 RID: 20489
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int RealizePalette(HandleRef hDC);

		// Token: 0x0600500A RID: 20490
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool LPtoDP(HandleRef hDC, [In] [Out] ref NativeMethods.RECT lpRect, int nCount);

		// Token: 0x0600500B RID: 20491
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetWindowOrgEx(HandleRef hDC, int x, int y, [In] [Out] NativeMethods.POINT point);

		// Token: 0x0600500C RID: 20492
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GetViewportOrgEx(HandleRef hDC, [In] [Out] NativeMethods.POINT point);

		// Token: 0x0600500D RID: 20493
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetMapMode(HandleRef hDC, int nMapMode);

		// Token: 0x0600500E RID: 20494
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowEnabled(HandleRef hWnd);

		// Token: 0x0600500F RID: 20495
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowVisible(HandleRef hWnd);

		// Token: 0x06005010 RID: 20496
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ReleaseCapture();

		// Token: 0x06005011 RID: 20497
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentThreadId();

		// Token: 0x06005012 RID: 20498
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumWindows(SafeNativeMethods.EnumThreadWindowsCallback callback, IntPtr extraData);

		// Token: 0x06005013 RID: 20499
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

		// Token: 0x06005014 RID: 20500
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(HandleRef hWnd, out int lpdwExitCode);

		// Token: 0x06005015 RID: 20501
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

		// Token: 0x06005016 RID: 20502
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		// Token: 0x06005017 RID: 20503
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		// Token: 0x06005018 RID: 20504
		[CLSCompliant(false)]
		[DllImport("comctl32.dll", ExactSpelling = true)]
		private static extern bool _TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme);

		// Token: 0x06005019 RID: 20505 RVA: 0x00126673 File Offset: 0x00125673
		public static bool TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme)
		{
			return SafeNativeMethods._TrackMouseEvent(tme);
		}

		// Token: 0x0600501A RID: 20506
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RedrawWindow(HandleRef hwnd, ref NativeMethods.RECT rcUpdate, HandleRef hrgnUpdate, int flags);

		// Token: 0x0600501B RID: 20507
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RedrawWindow(HandleRef hwnd, NativeMethods.COMRECT rcUpdate, HandleRef hrgnUpdate, int flags);

		// Token: 0x0600501C RID: 20508
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool InvalidateRect(HandleRef hWnd, ref NativeMethods.RECT rect, bool erase);

		// Token: 0x0600501D RID: 20509
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool InvalidateRect(HandleRef hWnd, NativeMethods.COMRECT rect, bool erase);

		// Token: 0x0600501E RID: 20510
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool InvalidateRgn(HandleRef hWnd, HandleRef hrgn, bool erase);

		// Token: 0x0600501F RID: 20511
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UpdateWindow(HandleRef hWnd);

		// Token: 0x06005020 RID: 20512
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentProcessId();

		// Token: 0x06005021 RID: 20513
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int ScrollWindowEx(HandleRef hWnd, int nXAmount, int nYAmount, NativeMethods.COMRECT rectScrollRegion, ref NativeMethods.RECT rectClip, HandleRef hrgnUpdate, ref NativeMethods.RECT prcUpdate, int flags);

		// Token: 0x06005022 RID: 20514
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetThreadLocale();

		// Token: 0x06005023 RID: 20515
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool MessageBeep(int type);

		// Token: 0x06005024 RID: 20516
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawMenuBar(HandleRef hWnd);

		// Token: 0x06005025 RID: 20517
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef parent, HandleRef child);

		// Token: 0x06005026 RID: 20518
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetTimer(HandleRef hWnd, int nIDEvent, int uElapse, IntPtr lpTimerFunc);

		// Token: 0x06005027 RID: 20519
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool KillTimer(HandleRef hwnd, int idEvent);

		// Token: 0x06005028 RID: 20520
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int MessageBox(HandleRef hWnd, string text, string caption, int type);

		// Token: 0x06005029 RID: 20521
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		// Token: 0x0600502A RID: 20522
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetTickCount();

		// Token: 0x0600502B RID: 20523
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ScrollWindow(HandleRef hWnd, int nXAmount, int nYAmount, ref NativeMethods.RECT rectScrollRegion, ref NativeMethods.RECT rectClip);

		// Token: 0x0600502C RID: 20524
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCurrentProcess();

		// Token: 0x0600502D RID: 20525
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCurrentThread();

		// Token: 0x0600502E RID: 20526
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetThreadLocale(int Locale);

		// Token: 0x0600502F RID: 20527
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowUnicode(HandleRef hWnd);

		// Token: 0x06005030 RID: 20528
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawEdge(HandleRef hDC, ref NativeMethods.RECT rect, int edge, int flags);

		// Token: 0x06005031 RID: 20529
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawFrameControl(HandleRef hDC, ref NativeMethods.RECT rect, int type, int state);

		// Token: 0x06005032 RID: 20530
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06005033 RID: 20531
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetRgnBox(HandleRef hRegion, ref NativeMethods.RECT clipRect);

		// Token: 0x06005034 RID: 20532
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06005035 RID: 20533
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		// Token: 0x06005036 RID: 20534
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawIcon(HandleRef hDC, int x, int y, HandleRef hIcon);

		// Token: 0x06005037 RID: 20535
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawIconEx(HandleRef hDC, int x, int y, HandleRef hIcon, int width, int height, int iStepIfAniCursor, HandleRef hBrushFlickerFree, int diFlags);

		// Token: 0x06005038 RID: 20536
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkMode(HandleRef hDC, int nBkMode);

		// Token: 0x06005039 RID: 20537
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

		// Token: 0x0600503A RID: 20538
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ShowCaret(HandleRef hWnd);

		// Token: 0x0600503B RID: 20539
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool HideCaret(HandleRef hWnd);

		// Token: 0x0600503C RID: 20540
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint GetCaretBlinkTime();

		// Token: 0x0600503D RID: 20541
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool IsAppThemed();

		// Token: 0x0600503E RID: 20542
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeAppProperties();

		// Token: 0x0600503F RID: 20543
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern void SetThemeAppProperties(int Flags);

		// Token: 0x06005040 RID: 20544
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr OpenThemeData(HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszClassList);

		// Token: 0x06005041 RID: 20545
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int CloseThemeData(HandleRef hTheme);

		// Token: 0x06005042 RID: 20546
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		// Token: 0x06005043 RID: 20547
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool IsThemePartDefined(HandleRef hTheme, int iPartId, int iStateId);

		// Token: 0x06005044 RID: 20548
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeBackground(HandleRef hTheme, HandleRef hdc, int partId, int stateId, [In] NativeMethods.COMRECT pRect, [In] NativeMethods.COMRECT pClipRect);

		// Token: 0x06005045 RID: 20549
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeEdge(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pDestRect, int uEdge, int uFlags, [Out] NativeMethods.COMRECT pContentRect);

		// Token: 0x06005046 RID: 20550
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeParentBackground(HandleRef hwnd, HandleRef hdc, [In] NativeMethods.COMRECT prc);

		// Token: 0x06005047 RID: 20551
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeText(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [MarshalAs(UnmanagedType.LPWStr)] string pszText, int iCharCount, int dwTextFlags, int dwTextFlags2, [In] NativeMethods.COMRECT pRect);

		// Token: 0x06005048 RID: 20552
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBackgroundContentRect(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pBoundingRect, [Out] NativeMethods.COMRECT pContentRect);

		// Token: 0x06005049 RID: 20553
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBackgroundExtent(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pContentRect, [Out] NativeMethods.COMRECT pExtentRect);

		// Token: 0x0600504A RID: 20554
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBackgroundRegion(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pRect, ref IntPtr pRegion);

		// Token: 0x0600504B RID: 20555
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBool(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref bool pfVal);

		// Token: 0x0600504C RID: 20556
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeColor(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref int pColor);

		// Token: 0x0600504D RID: 20557
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeEnumValue(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);

		// Token: 0x0600504E RID: 20558
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeFilename(HandleRef hTheme, int iPartId, int iStateId, int iPropId, StringBuilder pszThemeFilename, int cchMaxBuffChars);

		// Token: 0x0600504F RID: 20559
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeFont(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, int iPropId, NativeMethods.LOGFONT pFont);

		// Token: 0x06005050 RID: 20560
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeInt(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);

		// Token: 0x06005051 RID: 20561
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemePartSize(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT prc, ThemeSizeType eSize, [Out] NativeMethods.SIZE psz);

		// Token: 0x06005052 RID: 20562
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemePosition(HandleRef hTheme, int iPartId, int iStateId, int iPropId, [Out] NativeMethods.POINT pPoint);

		// Token: 0x06005053 RID: 20563
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeMargins(HandleRef hTheme, HandleRef hDC, int iPartId, int iStateId, int iPropId, ref NativeMethods.MARGINS margins);

		// Token: 0x06005054 RID: 20564
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeString(HandleRef hTheme, int iPartId, int iStateId, int iPropId, StringBuilder pszBuff, int cchMaxBuffChars);

		// Token: 0x06005055 RID: 20565
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeDocumentationProperty([MarshalAs(UnmanagedType.LPWStr)] string pszThemeName, [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, StringBuilder pszValueBuff, int cchMaxValChars);

		// Token: 0x06005056 RID: 20566
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeTextExtent(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [MarshalAs(UnmanagedType.LPWStr)] string pszText, int iCharCount, int dwTextFlags, [In] NativeMethods.COMRECT pBoundingRect, [Out] NativeMethods.COMRECT pExtentRect);

		// Token: 0x06005057 RID: 20567
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeTextMetrics(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, ref TextMetrics ptm);

		// Token: 0x06005058 RID: 20568
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int HitTestThemeBackground(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, int dwOptions, [In] NativeMethods.COMRECT pRect, HandleRef hrgn, [In] NativeMethods.POINTSTRUCT ptTest, ref int pwHitTestCode);

		// Token: 0x06005059 RID: 20569
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool IsThemeBackgroundPartiallyTransparent(HandleRef hTheme, int iPartId, int iStateId);

		// Token: 0x0600505A RID: 20570
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool GetThemeSysBool(HandleRef hTheme, int iBoolId);

		// Token: 0x0600505B RID: 20571
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeSysInt(HandleRef hTheme, int iIntId, ref int piValue);

		// Token: 0x0600505C RID: 20572
		[DllImport("user32.dll")]
		public static extern IntPtr OpenInputDesktop(int dwFlags, bool fInherit, int dwDesiredAccess);

		// Token: 0x0600505D RID: 20573 RVA: 0x0012667C File Offset: 0x0012567C
		public static int RGBToCOLORREF(int rgbValue)
		{
			int num = (rgbValue & 255) << 16;
			rgbValue &= 16776960;
			rgbValue |= (rgbValue >> 16 & 255);
			rgbValue &= 65535;
			rgbValue |= num;
			return rgbValue;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001266BC File Offset: 0x001256BC
		public static Color ColorFromCOLORREF(int colorref)
		{
			int red = colorref & 255;
			int green = colorref >> 8 & 255;
			int blue = colorref >> 16 & 255;
			return Color.FromArgb(red, green, blue);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001266EE File Offset: 0x001256EE
		public static int ColorToCOLORREF(Color color)
		{
			return (int)color.R | (int)color.G << 8 | (int)color.B << 16;
		}

		// Token: 0x020005F5 RID: 1525
		// (Invoke) Token: 0x06005061 RID: 20577
		internal delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

		// Token: 0x020005F6 RID: 1526
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("BEF6E003-A874-101A-8BBA-00AA00300CAB")]
		[ComImport]
		public interface IFontDisp
		{
			// Token: 0x1700102E RID: 4142
			// (get) Token: 0x06005064 RID: 20580
			// (set) Token: 0x06005065 RID: 20581
			string Name { get; set; }

			// Token: 0x1700102F RID: 4143
			// (get) Token: 0x06005066 RID: 20582
			// (set) Token: 0x06005067 RID: 20583
			long Size { get; set; }

			// Token: 0x17001030 RID: 4144
			// (get) Token: 0x06005068 RID: 20584
			// (set) Token: 0x06005069 RID: 20585
			bool Bold { get; set; }

			// Token: 0x17001031 RID: 4145
			// (get) Token: 0x0600506A RID: 20586
			// (set) Token: 0x0600506B RID: 20587
			bool Italic { get; set; }

			// Token: 0x17001032 RID: 4146
			// (get) Token: 0x0600506C RID: 20588
			// (set) Token: 0x0600506D RID: 20589
			bool Underline { get; set; }

			// Token: 0x17001033 RID: 4147
			// (get) Token: 0x0600506E RID: 20590
			// (set) Token: 0x0600506F RID: 20591
			bool Strikethrough { get; set; }

			// Token: 0x17001034 RID: 4148
			// (get) Token: 0x06005070 RID: 20592
			// (set) Token: 0x06005071 RID: 20593
			short Weight { get; set; }

			// Token: 0x17001035 RID: 4149
			// (get) Token: 0x06005072 RID: 20594
			// (set) Token: 0x06005073 RID: 20595
			short Charset { get; set; }
		}
	}
}
