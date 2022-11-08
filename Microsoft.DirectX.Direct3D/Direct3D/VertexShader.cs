using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200011C RID: 284
	public class VertexShader : MarshalByRefObject, IDisposable
	{
		// Token: 0x060007DF RID: 2015 RVA: 0x0006DF98 File Offset: 0x0006D398
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(VertexShader) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0006DFD4 File Offset: 0x0006D3D4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(VertexShader left, VertexShader right)
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

		// Token: 0x060007E1 RID: 2017 RVA: 0x0006E1C0 File Offset: 0x0006D5C0
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(VertexShader left, VertexShader right)
		{
			return !(left == right);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0006E004 File Offset: 0x0006D404
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00064418 File Offset: 0x00063818
		[CLSCompliant(false)]
		public unsafe VertexShader(IDirect3DVertexShader9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000643D0 File Offset: 0x000637D0
		public unsafe VertexShader(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DVertexShader9* lpUM = (IDirect3DVertexShader9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0006E348 File Offset: 0x0006D748
		[CLSCompliant(false)]
		public unsafe VertexShader(IDirect3DVertexShader9* lp, Device device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.CreateObjects(device);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0006E378 File Offset: 0x0006D778
		public unsafe VertexShader(Device device, GraphicsStream functionToken)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DVertexShader9* direct3DVertexShader9*& = ref this.m_lpUM;
			int num;
			if (functionToken != null)
			{
				num = functionToken.InternalDataPointer;
			}
			else
			{
				num = 0;
			}
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexShader9**), device.m_lpUM, num, ref direct3DVertexShader9*&, *(*(int*)device.m_lpUM + 364));
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

		// Token: 0x060007E7 RID: 2023 RVA: 0x0006E408 File Offset: 0x0006D808
		public unsafe VertexShader(Device device, int[] functionToken)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DVertexShader9* direct3DVertexShader9*& = ref this.m_lpUM;
			ref void void& = ref functionToken[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexShader9**), device.m_lpUM, ref void&, ref direct3DVertexShader9*&, *(*(int*)device.m_lpUM + 364));
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

		// Token: 0x060007E8 RID: 2024 RVA: 0x0006E1DC File Offset: 0x0006D5DC
		public unsafe virtual void Dispose()
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

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0006E05C File Offset: 0x0006D45C
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

		// Token: 0x060007EA RID: 2026 RVA: 0x0006E0E4 File Offset: 0x0006D4E4
		public unsafe int[] GetFunction()
		{
			int[] result = null;
			uint num2;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Void*,System.UInt32*), this.m_lpUM, 0, ref num2, *(*(int*)this.m_lpUM + 16));
			if (num2 > 0U)
			{
				if ((num2 & 3U) > 0U)
				{
					throw new ArgumentException();
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

		// Token: 0x060007EB RID: 2027 RVA: 0x0006E2AC File Offset: 0x0006D6AC
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0006E2D4 File Offset: 0x0006D6D4
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

		// Token: 0x060007ED RID: 2029 RVA: 0x0006E294 File Offset: 0x0006D694
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0006E198 File Offset: 0x0006D598
		private void OnParentLost(object sender, EventArgs e)
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0006E1AC File Offset: 0x0006D5AC
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x000643B8 File Offset: 0x000637B8
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0006E024 File Offset: 0x0006D424
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

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00064400 File Offset: 0x00063800
		[CLSCompliant(false)]
		public unsafe IDirect3DVertexShader9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00064440 File Offset: 0x00063840
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DVertexShader9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060007F4 RID: 2036 RVA: 0x00064480 File Offset: 0x00063880
		// (remove) Token: 0x060007F5 RID: 2037 RVA: 0x000644A4 File Offset: 0x000638A4
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

		// Token: 0x060007F7 RID: 2039 RVA: 0x000644F0 File Offset: 0x000638F0
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040010B6 RID: 4278
		private Device parentDevice;

		// Token: 0x040010B7 RID: 4279
		internal Device pCachedResourcepDevice;

		// Token: 0x040010B8 RID: 4280
		internal bool m_bDisposed;

		// Token: 0x040010BA RID: 4282
		internal unsafe IDirect3DVertexShader9* m_lpUM;
	}
}
