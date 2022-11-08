using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000730 RID: 1840
	internal static class WebBrowserHelper
	{
		// Token: 0x0600627C RID: 25212 RVA: 0x001665CB File Offset: 0x001655CB
		internal static int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x0600627D RID: 25213 RVA: 0x001665DA File Offset: 0x001655DA
		internal static int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x0600627E RID: 25214 RVA: 0x001665EC File Offset: 0x001655EC
		internal static int LogPixelsX
		{
			get
			{
				if (WebBrowserHelper.logPixelsX == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						WebBrowserHelper.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return WebBrowserHelper.logPixelsX;
			}
		}

		// Token: 0x0600627F RID: 25215 RVA: 0x00166643 File Offset: 0x00165643
		internal static void ResetLogPixelsX()
		{
			WebBrowserHelper.logPixelsX = -1;
		}

		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x06006280 RID: 25216 RVA: 0x0016664C File Offset: 0x0016564C
		internal static int LogPixelsY
		{
			get
			{
				if (WebBrowserHelper.logPixelsY == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						WebBrowserHelper.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return WebBrowserHelper.logPixelsY;
			}
		}

		// Token: 0x06006281 RID: 25217 RVA: 0x001666A3 File Offset: 0x001656A3
		internal static void ResetLogPixelsY()
		{
			WebBrowserHelper.logPixelsY = -1;
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x001666AC File Offset: 0x001656AC
		internal static ISelectionService GetSelectionService(Control ctl)
		{
			ISite site = ctl.Site;
			if (site != null)
			{
				object service = site.GetService(typeof(ISelectionService));
				if (service is ISelectionService)
				{
					return (ISelectionService)service;
				}
			}
			return null;
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x001666E4 File Offset: 0x001656E4
		internal static NativeMethods.COMRECT GetClipRect()
		{
			return new NativeMethods.COMRECT(new Rectangle(0, 0, 32000, 32000));
		}

		// Token: 0x04003AFD RID: 15101
		private const int HMperInch = 2540;

		// Token: 0x04003AFE RID: 15102
		internal const int REGMSG_RETVAL = 123;

		// Token: 0x04003AFF RID: 15103
		internal static readonly int sinkAttached = BitVector32.CreateMask();

		// Token: 0x04003B00 RID: 15104
		internal static readonly int manualUpdate = BitVector32.CreateMask(WebBrowserHelper.sinkAttached);

		// Token: 0x04003B01 RID: 15105
		internal static readonly int setClientSiteFirst = BitVector32.CreateMask(WebBrowserHelper.manualUpdate);

		// Token: 0x04003B02 RID: 15106
		internal static readonly int addedSelectionHandler = BitVector32.CreateMask(WebBrowserHelper.setClientSiteFirst);

		// Token: 0x04003B03 RID: 15107
		internal static readonly int siteProcessedInputKey = BitVector32.CreateMask(WebBrowserHelper.addedSelectionHandler);

		// Token: 0x04003B04 RID: 15108
		internal static readonly int inTransition = BitVector32.CreateMask(WebBrowserHelper.siteProcessedInputKey);

		// Token: 0x04003B05 RID: 15109
		internal static readonly int processingKeyUp = BitVector32.CreateMask(WebBrowserHelper.inTransition);

		// Token: 0x04003B06 RID: 15110
		internal static readonly int isMaskEdit = BitVector32.CreateMask(WebBrowserHelper.processingKeyUp);

		// Token: 0x04003B07 RID: 15111
		internal static readonly int recomputeContainingControl = BitVector32.CreateMask(WebBrowserHelper.isMaskEdit);

		// Token: 0x04003B08 RID: 15112
		private static int logPixelsX = -1;

		// Token: 0x04003B09 RID: 15113
		private static int logPixelsY = -1;

		// Token: 0x04003B0A RID: 15114
		private static Guid ifont_Guid = typeof(UnsafeNativeMethods.IFont).GUID;

		// Token: 0x04003B0B RID: 15115
		internal static Guid windowsMediaPlayer_Clsid = new Guid("{22d6f312-b0f6-11d0-94ab-0080c74c7e95}");

		// Token: 0x04003B0C RID: 15116
		internal static Guid comctlImageCombo_Clsid = new Guid("{a98a24c0-b06f-3684-8c12-c52ae341e0bc}");

		// Token: 0x04003B0D RID: 15117
		internal static Guid maskEdit_Clsid = new Guid("{c932ba85-4374-101b-a56c-00aa003668dc}");

		// Token: 0x04003B0E RID: 15118
		internal static readonly int REGMSG_MSG = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_subclassCheck");

		// Token: 0x02000731 RID: 1841
		internal enum AXState
		{
			// Token: 0x04003B10 RID: 15120
			Passive,
			// Token: 0x04003B11 RID: 15121
			Loaded,
			// Token: 0x04003B12 RID: 15122
			Running,
			// Token: 0x04003B13 RID: 15123
			InPlaceActive = 4,
			// Token: 0x04003B14 RID: 15124
			UIActive = 8
		}

		// Token: 0x02000732 RID: 1842
		internal enum AXEditMode
		{
			// Token: 0x04003B16 RID: 15126
			None,
			// Token: 0x04003B17 RID: 15127
			Object,
			// Token: 0x04003B18 RID: 15128
			Host
		}

		// Token: 0x02000733 RID: 1843
		internal enum SelectionStyle
		{
			// Token: 0x04003B1A RID: 15130
			NotSelected,
			// Token: 0x04003B1B RID: 15131
			Selected,
			// Token: 0x04003B1C RID: 15132
			Active
		}
	}
}
