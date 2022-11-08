using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000727 RID: 1831
	public class WebBrowserSiteBase : UnsafeNativeMethods.IOleControlSite, UnsafeNativeMethods.IOleClientSite, UnsafeNativeMethods.IOleInPlaceSite, UnsafeNativeMethods.ISimpleFrameSite, UnsafeNativeMethods.IPropertyNotifySink, IDisposable
	{
		// Token: 0x060061FB RID: 25083 RVA: 0x001653ED File Offset: 0x001643ED
		internal WebBrowserSiteBase(WebBrowserBase h)
		{
			if (h == null)
			{
				throw new ArgumentNullException("h");
			}
			this.host = h;
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x0016540A File Offset: 0x0016440A
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x00165413 File Offset: 0x00164413
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopEvents();
			}
		}

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x060061FE RID: 25086 RVA: 0x0016541E File Offset: 0x0016441E
		internal WebBrowserBase Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x00165426 File Offset: 0x00164426
		int UnsafeNativeMethods.IOleControlSite.OnControlInfoChanged()
		{
			return 0;
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x00165429 File Offset: 0x00164429
		int UnsafeNativeMethods.IOleControlSite.LockInPlaceActive(int fLock)
		{
			return -2147467263;
		}

		// Token: 0x06006201 RID: 25089 RVA: 0x00165430 File Offset: 0x00164430
		int UnsafeNativeMethods.IOleControlSite.GetExtendedControl(out object ppDisp)
		{
			ppDisp = null;
			return -2147467263;
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x0016543C File Offset: 0x0016443C
		int UnsafeNativeMethods.IOleControlSite.TransformCoords(NativeMethods._POINTL pPtlHimetric, NativeMethods.tagPOINTF pPtfContainer, int dwFlags)
		{
			if ((dwFlags & 4) != 0)
			{
				if ((dwFlags & 2) != 0)
				{
					pPtfContainer.x = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.x, WebBrowserHelper.LogPixelsX);
					pPtfContainer.y = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.y, WebBrowserHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtfContainer.x = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.x, WebBrowserHelper.LogPixelsX);
					pPtfContainer.y = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.y, WebBrowserHelper.LogPixelsY);
				}
			}
			else
			{
				if ((dwFlags & 8) == 0)
				{
					return -2147024809;
				}
				if ((dwFlags & 2) != 0)
				{
					pPtlHimetric.x = WebBrowserHelper.Pix2HM((int)pPtfContainer.x, WebBrowserHelper.LogPixelsX);
					pPtlHimetric.y = WebBrowserHelper.Pix2HM((int)pPtfContainer.y, WebBrowserHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtlHimetric.x = WebBrowserHelper.Pix2HM((int)pPtfContainer.x, WebBrowserHelper.LogPixelsX);
					pPtlHimetric.y = WebBrowserHelper.Pix2HM((int)pPtfContainer.y, WebBrowserHelper.LogPixelsY);
				}
			}
			return 0;
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x00165540 File Offset: 0x00164540
		int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref NativeMethods.MSG pMsg, int grfModifiers)
		{
			this.Host.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, true);
			Message message = default(Message);
			message.Msg = pMsg.message;
			message.WParam = pMsg.wParam;
			message.LParam = pMsg.lParam;
			message.HWnd = pMsg.hwnd;
			int result;
			try
			{
				result = ((this.Host.PreProcessControlMessage(ref message) == PreProcessControlState.MessageProcessed) ? 0 : 1);
			}
			finally
			{
				this.Host.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, false);
			}
			return result;
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x001655D8 File Offset: 0x001645D8
		int UnsafeNativeMethods.IOleControlSite.OnFocus(int fGotFocus)
		{
			return 0;
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x001655DB File Offset: 0x001645DB
		int UnsafeNativeMethods.IOleControlSite.ShowPropertyFrame()
		{
			return -2147467263;
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x001655E2 File Offset: 0x001645E2
		int UnsafeNativeMethods.IOleClientSite.SaveObject()
		{
			return -2147467263;
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x001655E9 File Offset: 0x001645E9
		int UnsafeNativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
		{
			moniker = null;
			return -2147467263;
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x001655F3 File Offset: 0x001645F3
		int UnsafeNativeMethods.IOleClientSite.GetContainer(out UnsafeNativeMethods.IOleContainer container)
		{
			container = this.Host.GetParentContainer();
			return 0;
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x00165604 File Offset: 0x00164604
		int UnsafeNativeMethods.IOleClientSite.ShowObject()
		{
			if (this.Host.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive)
			{
				IntPtr intPtr;
				if (NativeMethods.Succeeded(this.Host.AXInPlaceObject.GetWindow(out intPtr)))
				{
					if (this.Host.GetHandleNoCreate() != intPtr && intPtr != IntPtr.Zero)
					{
						this.Host.AttachWindow(intPtr);
						this.OnActiveXRectChange(new NativeMethods.COMRECT(this.Host.Bounds));
					}
				}
				else if (this.Host.AXInPlaceObject is UnsafeNativeMethods.IOleInPlaceObjectWindowless)
				{
					throw new InvalidOperationException(SR.GetString("AXWindowlessControl"));
				}
			}
			return 0;
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x001656A3 File Offset: 0x001646A3
		int UnsafeNativeMethods.IOleClientSite.OnShowWindow(int fShow)
		{
			return 0;
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x001656A6 File Offset: 0x001646A6
		int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
		{
			return -2147467263;
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x001656B0 File Offset: 0x001646B0
		IntPtr UnsafeNativeMethods.IOleInPlaceSite.GetWindow()
		{
			IntPtr parent;
			try
			{
				parent = UnsafeNativeMethods.GetParent(new HandleRef(this.Host, this.Host.Handle));
			}
			catch (Exception)
			{
				throw;
			}
			return parent;
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x001656F0 File Offset: 0x001646F0
		int UnsafeNativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
		{
			return -2147467263;
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x001656F7 File Offset: 0x001646F7
		int UnsafeNativeMethods.IOleInPlaceSite.CanInPlaceActivate()
		{
			return 0;
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x001656FA File Offset: 0x001646FA
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceActivate()
		{
			this.Host.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			this.OnActiveXRectChange(new NativeMethods.COMRECT(this.Host.Bounds));
			return 0;
		}

		// Token: 0x06006210 RID: 25104 RVA: 0x00165720 File Offset: 0x00164720
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIActivate()
		{
			this.Host.ActiveXState = WebBrowserHelper.AXState.UIActive;
			this.Host.GetParentContainer().OnUIActivate(this.Host);
			return 0;
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x00165748 File Offset: 0x00164748
		int UnsafeNativeMethods.IOleInPlaceSite.GetWindowContext(out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.tagOIFI lpFrameInfo)
		{
			ppDoc = null;
			ppFrame = this.Host.GetParentContainer();
			lprcPosRect.left = this.Host.Bounds.X;
			lprcPosRect.top = this.Host.Bounds.Y;
			lprcPosRect.right = this.Host.Bounds.Width + this.Host.Bounds.X;
			lprcPosRect.bottom = this.Host.Bounds.Height + this.Host.Bounds.Y;
			lprcClipRect = WebBrowserHelper.GetClipRect();
			if (lpFrameInfo != null)
			{
				lpFrameInfo.cb = Marshal.SizeOf(typeof(NativeMethods.tagOIFI));
				lpFrameInfo.fMDIApp = false;
				lpFrameInfo.hAccel = IntPtr.Zero;
				lpFrameInfo.cAccelEntries = 0;
				lpFrameInfo.hwndFrame = ((this.Host.ParentInternal == null) ? IntPtr.Zero : this.Host.ParentInternal.Handle);
			}
			return 0;
		}

		// Token: 0x06006212 RID: 25106 RVA: 0x0016585C File Offset: 0x0016485C
		int UnsafeNativeMethods.IOleInPlaceSite.Scroll(NativeMethods.tagSIZE scrollExtant)
		{
			return 1;
		}

		// Token: 0x06006213 RID: 25107 RVA: 0x0016585F File Offset: 0x0016485F
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
		{
			this.Host.GetParentContainer().OnUIDeactivate(this.Host);
			if (this.Host.ActiveXState > WebBrowserHelper.AXState.InPlaceActive)
			{
				this.Host.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			}
			return 0;
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x00165892 File Offset: 0x00164892
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
		{
			if (this.Host.ActiveXState == WebBrowserHelper.AXState.UIActive)
			{
				((UnsafeNativeMethods.IOleInPlaceSite)this).OnUIDeactivate(0);
			}
			this.Host.GetParentContainer().OnInPlaceDeactivate(this.Host);
			this.Host.ActiveXState = WebBrowserHelper.AXState.Running;
			return 0;
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x001658CD File Offset: 0x001648CD
		int UnsafeNativeMethods.IOleInPlaceSite.DiscardUndoState()
		{
			return 0;
		}

		// Token: 0x06006216 RID: 25110 RVA: 0x001658D0 File Offset: 0x001648D0
		int UnsafeNativeMethods.IOleInPlaceSite.DeactivateAndUndo()
		{
			return this.Host.AXInPlaceObject.UIDeactivate();
		}

		// Token: 0x06006217 RID: 25111 RVA: 0x001658E2 File Offset: 0x001648E2
		int UnsafeNativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			return this.OnActiveXRectChange(lprcPosRect);
		}

		// Token: 0x06006218 RID: 25112 RVA: 0x001658EB File Offset: 0x001648EB
		int UnsafeNativeMethods.ISimpleFrameSite.PreMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, ref int pdwCookie)
		{
			return 0;
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x001658EE File Offset: 0x001648EE
		int UnsafeNativeMethods.ISimpleFrameSite.PostMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, int dwCookie)
		{
			return 1;
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x001658F4 File Offset: 0x001648F4
		void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispid)
		{
			if (this.Host.NoComponentChangeEvents != 0)
			{
				return;
			}
			this.Host.NoComponentChangeEvents++;
			try
			{
				this.OnPropertyChanged(dispid);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				this.Host.NoComponentChangeEvents--;
			}
		}

		// Token: 0x0600621B RID: 25115 RVA: 0x00165960 File Offset: 0x00164960
		int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispid)
		{
			return 0;
		}

		// Token: 0x0600621C RID: 25116 RVA: 0x00165964 File Offset: 0x00164964
		internal virtual void OnPropertyChanged(int dispid)
		{
			try
			{
				ISite site = this.Host.Site;
				if (site != null)
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanging(this.Host, null);
						}
						catch (CheckoutException ex)
						{
							if (ex == CheckoutException.Canceled)
							{
								return;
							}
							throw ex;
						}
						componentChangeService.OnComponentChanged(this.Host, null, null, null);
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x001659E8 File Offset: 0x001649E8
		internal WebBrowserBase GetAXHost()
		{
			return this.Host;
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x001659F0 File Offset: 0x001649F0
		internal void StartEvents()
		{
			if (this.connectionPoint != null)
			{
				return;
			}
			object activeXInstance = this.Host.activeXInstance;
			if (activeXInstance != null)
			{
				try
				{
					this.connectionPoint = new AxHost.ConnectionPointCookie(activeXInstance, this, typeof(UnsafeNativeMethods.IPropertyNotifySink));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00165A4C File Offset: 0x00164A4C
		internal void StopEvents()
		{
			if (this.connectionPoint != null)
			{
				this.connectionPoint.Disconnect();
				this.connectionPoint = null;
			}
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x00165A68 File Offset: 0x00164A68
		private int OnActiveXRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			this.Host.AXInPlaceObject.SetObjectRects(NativeMethods.COMRECT.FromXYWH(0, 0, lprcPosRect.right - lprcPosRect.left, lprcPosRect.bottom - lprcPosRect.top), WebBrowserHelper.GetClipRect());
			this.Host.MakeDirty();
			return 0;
		}

		// Token: 0x04003ADD RID: 15069
		private WebBrowserBase host;

		// Token: 0x04003ADE RID: 15070
		private AxHost.ConnectionPointCookie connectionPoint;
	}
}
