using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200011E RID: 286
	public sealed class PixelShader : MarshalByRefObject, IDisposable
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x0006DA78 File Offset: 0x0006CE78
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(PixelShader) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0006DAB4 File Offset: 0x0006CEB4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(PixelShader left, PixelShader right)
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

		// Token: 0x060007FA RID: 2042 RVA: 0x0006DCA0 File Offset: 0x0006D0A0
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(PixelShader left, PixelShader right)
		{
			return !(left == right);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0006DAE4 File Offset: 0x0006CEE4
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00064570 File Offset: 0x00063970
		[CLSCompliant(false)]
		public unsafe PixelShader(IDirect3DPixelShader9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00064528 File Offset: 0x00063928
		public unsafe PixelShader(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DPixelShader9* lpUM = (IDirect3DPixelShader9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0006DE28 File Offset: 0x0006D228
		[CLSCompliant(false)]
		public unsafe PixelShader(IDirect3DPixelShader9* lp, Device device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.CreateObjects(device);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0006DEE8 File Offset: 0x0006D2E8
		public unsafe PixelShader(Device device, int[] functionToken)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DPixelShader9* direct3DPixelShader9*& = ref this.m_lpUM;
			ref void void& = ref functionToken[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DPixelShader9**), device.m_lpUM, ref void&, ref direct3DPixelShader9*&, *(*(int*)device.m_lpUM + 424));
			if (num < 0)
			{
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
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

		// Token: 0x06000800 RID: 2048 RVA: 0x0006DE58 File Offset: 0x0006D258
		public unsafe PixelShader(Device device, GraphicsStream functionToken)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DPixelShader9* direct3DPixelShader9*& = ref this.m_lpUM;
			int num;
			if (functionToken != null)
			{
				num = functionToken.InternalDataPointer;
			}
			else
			{
				num = 0;
			}
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DPixelShader9**), device.m_lpUM, num, ref direct3DPixelShader9*&, *(*(int*)device.m_lpUM + 424));
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
			this.CreateObjects(device);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0006DCBC File Offset: 0x0006D0BC
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

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0006DB3C File Offset: 0x0006CF3C
		public unsafe Device Device
		{
			get
			{
				IDirect3DDevice9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DDevice9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 12));
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

		// Token: 0x06000803 RID: 2051 RVA: 0x0006DBC4 File Offset: 0x0006CFC4
		public unsafe int[] GetFunction()
		{
			int[] result = null;
			uint num2;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Void*,System.UInt32*), this.m_lpUM, 0, ref num2, *(*(int*)this.m_lpUM + 16));
			if (num2 > 0U)
			{
				if ((num2 & 3U) > 0U)
				{
					throw new InvalidOperationException();
				}
				int[] array = new int[num2 >> 2];
				array.Initialize();
				result = array;
				ref int int32& = ref array[0];
				num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Void*,System.UInt32*), this.m_lpUM, ref int32&, ref num2, *(*(int*)this.m_lpUM + 16));
			}
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

		// Token: 0x06000804 RID: 2052 RVA: 0x0006DD8C File Offset: 0x0006D18C
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0006DDB4 File Offset: 0x0006D1B4
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

		// Token: 0x06000806 RID: 2054 RVA: 0x0006DD74 File Offset: 0x0006D174
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0006DC78 File Offset: 0x0006D078
		private void OnParentLost(object sender, EventArgs e)
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0006DC8C File Offset: 0x0006D08C
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00064510 File Offset: 0x00063910
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0006DB04 File Offset: 0x0006CF04
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

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00064558 File Offset: 0x00063958
		[CLSCompliant(false)]
		public unsafe IDirect3DPixelShader9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00064598 File Offset: 0x00063998
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DPixelShader9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600080D RID: 2061 RVA: 0x000645D8 File Offset: 0x000639D8
		// (remove) Token: 0x0600080E RID: 2062 RVA: 0x000645FC File Offset: 0x000639FC
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

		// Token: 0x06000810 RID: 2064 RVA: 0x00064648 File Offset: 0x00063A48
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040010BB RID: 4283
		private Device parentDevice;

		// Token: 0x040010BC RID: 4284
		internal Device pCachedResourcepDevice;

		// Token: 0x040010BD RID: 4285
		internal bool m_bDisposed;

		// Token: 0x040010BF RID: 4287
		internal unsafe IDirect3DPixelShader9* m_lpUM;
	}
}
