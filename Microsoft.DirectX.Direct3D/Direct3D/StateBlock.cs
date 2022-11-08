using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000118 RID: 280
	public sealed class StateBlock : MarshalByRefObject, IDisposable
	{
		// Token: 0x060007AD RID: 1965 RVA: 0x0006E9C0 File Offset: 0x0006DDC0
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(StateBlock) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0006E9FC File Offset: 0x0006DDFC
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(StateBlock left, StateBlock right)
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

		// Token: 0x060007AF RID: 1967 RVA: 0x0006EBE8 File Offset: 0x0006DFE8
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(StateBlock left, StateBlock right)
		{
			return !(left == right);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0006EA2C File Offset: 0x0006DE2C
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00064168 File Offset: 0x00063568
		[CLSCompliant(false)]
		public unsafe StateBlock(IDirect3DStateBlock9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00064120 File Offset: 0x00063520
		public unsafe StateBlock(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DStateBlock9* lpUM = (IDirect3DStateBlock9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0006ED70 File Offset: 0x0006E170
		[CLSCompliant(false)]
		public unsafe StateBlock(IDirect3DStateBlock9* lp, Device device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.CreateObjects(device);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0006EDA0 File Offset: 0x0006E1A0
		public unsafe StateBlock(Device device, StateBlockType stateBlockType)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DStateBlock9* direct3DStateBlock9*& = ref this.m_lpUM;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DStateBlock9**), device.m_lpUM, stateBlockType, ref direct3DStateBlock9*&, *(*(int*)device.m_lpUM + 236));
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

		// Token: 0x060007B5 RID: 1973 RVA: 0x0006EC04 File Offset: 0x0006E004
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

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0006EA84 File Offset: 0x0006DE84
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

		// Token: 0x060007B7 RID: 1975 RVA: 0x0006EB0C File Offset: 0x0006DF0C
		public unsafe void Capture()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 16));
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

		// Token: 0x060007B8 RID: 1976 RVA: 0x0006EB64 File Offset: 0x0006DF64
		public unsafe void Apply()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 20));
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

		// Token: 0x060007B9 RID: 1977 RVA: 0x0006ECD4 File Offset: 0x0006E0D4
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0006ECFC File Offset: 0x0006E0FC
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

		// Token: 0x060007BB RID: 1979 RVA: 0x0006ECBC File Offset: 0x0006E0BC
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0006EBBC File Offset: 0x0006DFBC
		private void OnParentLost(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0006EBD4 File Offset: 0x0006DFD4
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x00064108 File Offset: 0x00063508
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0006EA4C File Offset: 0x0006DE4C
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

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00064150 File Offset: 0x00063550
		[CLSCompliant(false)]
		public unsafe IDirect3DStateBlock9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00064190 File Offset: 0x00063590
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DStateBlock9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060007C2 RID: 1986 RVA: 0x000641D0 File Offset: 0x000635D0
		// (remove) Token: 0x060007C3 RID: 1987 RVA: 0x000641F4 File Offset: 0x000635F4
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

		// Token: 0x060007C5 RID: 1989 RVA: 0x00064240 File Offset: 0x00063640
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040010AC RID: 4268
		private Device parentDevice;

		// Token: 0x040010AD RID: 4269
		internal Device pCachedResourcepDevice;

		// Token: 0x040010AE RID: 4270
		internal bool m_bDisposed;

		// Token: 0x040010B0 RID: 4272
		internal unsafe IDirect3DStateBlock9* m_lpUM;
	}
}
