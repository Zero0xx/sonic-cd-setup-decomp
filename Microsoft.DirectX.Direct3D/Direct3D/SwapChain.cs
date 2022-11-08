using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200010C RID: 268
	public sealed class SwapChain : MarshalByRefObject, IDisposable
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0006AF8C File Offset: 0x0006A38C
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(SwapChain) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0006AFC8 File Offset: 0x0006A3C8
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(SwapChain left, SwapChain right)
		{
			if (left == null)
			{
				if (right == null)
				{
					return true;
				}
			}
			else if (right != null)
			{
				return left.m_lpUM == right.m_lpUM;
			}
			return false;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0006B3A4 File Offset: 0x0006A7A4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(SwapChain left, SwapChain right)
		{
			return !(left == right);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0006AFF8 File Offset: 0x0006A3F8
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00063A40 File Offset: 0x00062E40
		[CLSCompliant(false)]
		public unsafe SwapChain(IDirect3DSwapChain9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000639F8 File Offset: 0x00062DF8
		public unsafe SwapChain(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DSwapChain9* lpUM = (IDirect3DSwapChain9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0006B52C File Offset: 0x0006A92C
		[CLSCompliant(false)]
		public unsafe SwapChain(IDirect3DSwapChain9* lp, Device device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.CreateObjects(device);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0006B55C File Offset: 0x0006A95C
		public unsafe SwapChain(Device device, PresentParameters presentationParameters)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DSwapChain9* direct3DSwapChain9*& = ref this.m_lpUM;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DPRESENT_PARAMETERS_*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSwapChain9**), device.m_lpUM, presentationParameters.RealStruct, ref direct3DSwapChain9*&, *(*(int*)device.m_lpUM + 52));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
			this.CreateObjects(device);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0006B3C0 File Offset: 0x0006A7C0
		public unsafe void Dispose()
		{
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				if (this.parentDevice != null && Device.IsUsingEventHandlers)
				{
					this.parentDevice.DeviceLost -= this.OnParentLost;
					this.parentDevice.DeviceReset -= this.OnParentReset;
					this.parentDevice.Disposing -= this.OnParentDisposed;
				}
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0006B050 File Offset: 0x0006A450
		internal unsafe void PresentInternal(tagRECT* sourceRectangle, tagRECT* destRectangle, IntPtr overrideWindow, int flags)
		{
			IntPtr value = 0;
			value = new IntPtr(0);
			int num;
			if (overrideWindow != value)
			{
				num = overrideWindow.ToPointer();
			}
			else
			{
				num = 0;
			}
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.HWND__*,Microsoft.DirectX.PrivateImplementationDetails._RGNDATA modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, sourceRectangle, destRectangle, num, 0, flags, *(*(int*)this.m_lpUM + 12));
			if (num2 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num2;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num2);
				}
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00063754 File Offset: 0x00062B54
		internal unsafe void PresentInternal(tagRECT* sourceRectangle, tagRECT* destRectangle, Control overrideWindow, int flags)
		{
			IntPtr* ptr;
			if (overrideWindow != null)
			{
				IntPtr handle = overrideWindow.Handle;
				ptr = &handle;
			}
			else
			{
				IntPtr intPtr = 0;
				intPtr = new IntPtr(0);
				ptr = &intPtr;
			}
			this.PresentInternal(sourceRectangle, destRectangle, *ptr, flags);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000639B0 File Offset: 0x00062DB0
		public unsafe void Present(Rectangle rectPresent, IntPtr overrideWindowHandle, Present flags, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, overrideWindowHandle, (int)flags);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), overrideWindowHandle, (int)flags);
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00063994 File Offset: 0x00062D94
		public void Present(IntPtr overrideWindowHandle, Present flags)
		{
			this.PresentInternal(null, null, overrideWindowHandle, (int)flags);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00063974 File Offset: 0x00062D74
		public unsafe void Present(Rectangle sourceRectangle, Rectangle destRectangle, IntPtr overrideWindowHandle, Present flags)
		{
			this.PresentInternal((tagRECT*)(&sourceRectangle), (tagRECT*)(&destRectangle), overrideWindowHandle, (int)flags);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00063944 File Offset: 0x00062D44
		public unsafe void Present(Rectangle rectPresent, IntPtr overrideWindowHandle, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, overrideWindowHandle, 0);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), overrideWindowHandle, 0);
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00063928 File Offset: 0x00062D28
		public void Present(IntPtr overrideWindowHandle)
		{
			this.PresentInternal(null, null, overrideWindowHandle, 0);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00063908 File Offset: 0x00062D08
		public unsafe void Present(Rectangle sourceRectangle, Rectangle destRectangle, IntPtr overrideWindowHandle)
		{
			this.PresentInternal((tagRECT*)(&sourceRectangle), (tagRECT*)(&destRectangle), overrideWindowHandle, 0);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000638D8 File Offset: 0x00062CD8
		public unsafe void Present(Rectangle rectPresent, Present flags, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, null, (int)flags);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), null, (int)flags);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x000638A8 File Offset: 0x00062CA8
		public unsafe void Present(Rectangle rectPresent, Control overrideWindow, Present flags, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, overrideWindow, (int)flags);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), overrideWindow, (int)flags);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0006388C File Offset: 0x00062C8C
		public void Present(Control overrideWindow, Present flags)
		{
			this.PresentInternal(null, null, overrideWindow, (int)flags);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00063870 File Offset: 0x00062C70
		public void Present(Present flags)
		{
			this.PresentInternal(null, null, null, (int)flags);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00063850 File Offset: 0x00062C50
		public unsafe void Present(Rectangle sourceRectangle, Rectangle destRectangle, Control overrideWindow, Present flags)
		{
			this.PresentInternal((tagRECT*)(&sourceRectangle), (tagRECT*)(&destRectangle), overrideWindow, (int)flags);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00063820 File Offset: 0x00062C20
		public unsafe void Present(Rectangle rectPresent, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, null, 0);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), null, 0);
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000637F0 File Offset: 0x00062BF0
		public unsafe void Present(Rectangle rectPresent, Control overrideWindow, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, overrideWindow, 0);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), overrideWindow, 0);
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000637D4 File Offset: 0x00062BD4
		public void Present(Control overrideWindow)
		{
			this.PresentInternal(null, null, overrideWindow, 0);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000637B8 File Offset: 0x00062BB8
		public void Present()
		{
			this.PresentInternal(null, null, null, 0);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00063798 File Offset: 0x00062B98
		public unsafe void Present(Rectangle sourceRectangle, Rectangle destRectangle, Control overrideWindow)
		{
			this.PresentInternal((tagRECT*)(&sourceRectangle), (tagRECT*)(&destRectangle), overrideWindow, 0);
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0006B28C File Offset: 0x0006A68C
		public unsafe Device Device
		{
			get
			{
				IDirect3DDevice9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DDevice9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 32));
				if (num < 0)
				{
					if (!DirectXException.IsExceptionIgnored)
					{
						Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
						DirectXException ex = exceptionFromResultInternal as DirectXException;
						if (ex != null)
						{
							ex.ErrorCode = num;
							throw ex;
						}
						throw exceptionFromResultInternal;
					}
					else
					{
						<Module>.SetLastError(num);
					}
				}
				if (ptr != null)
				{
					if (this.pCachedResourcepDevice != null)
					{
						this.pCachedResourcepDevice.UpdateUnmanagedPointer(ptr);
					}
					else
					{
						this.pCachedResourcepDevice = new Device(ptr);
					}
					return this.pCachedResourcepDevice;
				}
				return null;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0006B0D4 File Offset: 0x0006A4D4
		public unsafe Surface GetBackBuffer(int backBuffer, BackBufferType typeBuffer)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**), this.m_lpUM, backBuffer, typeBuffer, ref ptr, *(*(int*)this.m_lpUM + 20));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
			if (ptr != null)
			{
				return new Surface(ptr, this);
			}
			return null;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0006B13C File Offset: 0x0006A53C
		public unsafe void GetFrontBufferData(Surface frontBuffer)
		{
			IDirect3DSurface9* ptr;
			if (frontBuffer != null)
			{
				ptr = frontBuffer.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 16));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0006B314 File Offset: 0x0006A714
		public unsafe PresentParameters PresentParameters
		{
			get
			{
				PresentParameters presentParameters = new PresentParameters();
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DPRESENT_PARAMETERS_*), this.m_lpUM, presentParameters.RealStruct, *(*(int*)this.m_lpUM + 36));
				if (num < 0)
				{
					if (!DirectXException.IsExceptionIgnored)
					{
						Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
						DirectXException ex = exceptionFromResultInternal as DirectXException;
						if (ex != null)
						{
							ex.ErrorCode = num;
							throw ex;
						}
						throw exceptionFromResultInternal;
					}
					else
					{
						<Module>.SetLastError(num);
					}
				}
				return presentParameters;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0006B218 File Offset: 0x0006A618
		public unsafe RasterStatus RasterStatus
		{
			get
			{
				RasterStatus result = default(RasterStatus);
				result = new RasterStatus();
				initblk(ref result, 0, sizeof(RasterStatus));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DRASTER_STATUS*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 24));
				if (num < 0)
				{
					if (!DirectXException.IsExceptionIgnored)
					{
						Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
						DirectXException ex = exceptionFromResultInternal as DirectXException;
						if (ex != null)
						{
							ex.ErrorCode = num;
							throw ex;
						}
						throw exceptionFromResultInternal;
					}
					else
					{
						<Module>.SetLastError(num);
					}
				}
				return result;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0006B1A4 File Offset: 0x0006A5A4
		public unsafe DisplayMode DisplayMode
		{
			get
			{
				DisplayMode result = default(DisplayMode);
				result = new DisplayMode();
				initblk(ref result, 0, sizeof(DisplayMode));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DDISPLAYMODE*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 28));
				if (num < 0)
				{
					if (!DirectXException.IsExceptionIgnored)
					{
						Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
						DirectXException ex = exceptionFromResultInternal as DirectXException;
						if (ex != null)
						{
							ex.ErrorCode = num;
							throw ex;
						}
						throw exceptionFromResultInternal;
					}
					else
					{
						<Module>.SetLastError(num);
					}
				}
				return result;
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0006B490 File Offset: 0x0006A890
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0006B4B8 File Offset: 0x0006A8B8
		private void CreateObjects(Device device)
		{
			this.m_bDisposed = false;
			this.parentDevice = device;
			if (this.parentDevice != null && Device.IsUsingEventHandlers)
			{
				this.parentDevice.DeviceLost += this.OnParentLost;
				this.parentDevice.DeviceReset += this.OnParentReset;
				this.parentDevice.Disposing += this.OnParentDisposed;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0006B478 File Offset: 0x0006A878
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0006B378 File Offset: 0x0006A778
		private void OnParentLost(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0006B390 File Offset: 0x0006A790
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x000639E0 File Offset: 0x00062DE0
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0006B018 File Offset: 0x0006A418
		[EditorBrowsable(EditorBrowsableState.Never)]
		public unsafe IntPtr GetObjectByValue(int uniqueKey)
		{
			if (uniqueKey == -759872593)
			{
				IntPtr result = 0;
				result = new IntPtr((void*)this.m_lpUM);
				return result;
			}
			throw new ArgumentException();
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00063A28 File Offset: 0x00062E28
		[CLSCompliant(false)]
		public unsafe IDirect3DSwapChain9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00063A68 File Offset: 0x00062E68
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DSwapChain9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000717 RID: 1815 RVA: 0x00063AA8 File Offset: 0x00062EA8
		// (remove) Token: 0x06000718 RID: 1816 RVA: 0x00063ACC File Offset: 0x00062ECC
		public event EventHandler Disposing
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Disposing = Delegate.Combine(this.Disposing, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Disposing = Delegate.Remove(this.Disposing, value);
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00063B18 File Offset: 0x00062F18
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001078 RID: 4216
		private Device parentDevice;

		// Token: 0x04001079 RID: 4217
		internal Device pCachedResourcepDevice;

		// Token: 0x0400107A RID: 4218
		internal bool m_bDisposed;

		// Token: 0x0400107C RID: 4220
		internal unsafe IDirect3DSwapChain9* m_lpUM;
	}
}
