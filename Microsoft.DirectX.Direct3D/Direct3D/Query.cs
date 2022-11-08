using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000107 RID: 263
	public sealed class Query : MarshalByRefObject, IDisposable
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x0006EE20 File Offset: 0x0006E220
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(Query) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0006EE5C File Offset: 0x0006E25C
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(Query left, Query right)
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

		// Token: 0x060006AE RID: 1710 RVA: 0x0006F0FC File Offset: 0x0006E4FC
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(Query left, Query right)
		{
			return !(left == right);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0006EE8C File Offset: 0x0006E28C
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000634B4 File Offset: 0x000628B4
		[CLSCompliant(false)]
		public unsafe Query(IDirect3DQuery9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0006346C File Offset: 0x0006286C
		public unsafe Query(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DQuery9* lpUM = (IDirect3DQuery9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0006F284 File Offset: 0x0006E684
		[CLSCompliant(false)]
		public unsafe Query(IDirect3DQuery9* lp, Device device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.CreateObjects(device);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0006F2B4 File Offset: 0x0006E6B4
		public unsafe Query(Device device, QueryType queryType)
		{
			this.Disposing = null;
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			this.m_lpUM = null;
			ref IDirect3DQuery9* direct3DQuery9*& = ref this.m_lpUM;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DQuery9**), device.m_lpUM, queryType, ref direct3DQuery9*&, *(*(int*)device.m_lpUM + 472));
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

		// Token: 0x060006B4 RID: 1716 RVA: 0x0006F118 File Offset: 0x0006E518
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

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0006EEE4 File Offset: 0x0006E2E4
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

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0006EF6C File Offset: 0x0006E36C
		public unsafe QueryType QueryType
		{
			get
			{
				return calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 16));
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0006EF94 File Offset: 0x0006E394
		public unsafe void Issue(IssueFlags flags)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, flags, *(*(int*)this.m_lpUM + 24));
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

		// Token: 0x060006B8 RID: 1720 RVA: 0x00063438 File Offset: 0x00062838
		public object GetData(Type returnDataType, [MarshalAs(UnmanagedType.U1)] bool flush)
		{
			return this.GetData(returnDataType, flush, 0);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0006EFEC File Offset: 0x0006E3EC
		public unsafe object GetData(Type returnDataType, [MarshalAs(UnmanagedType.U1)] bool flush, out bool dataReturned)
		{
			int num = 1;
			object obj = null;
			uint typeSize = (uint)DXHelp.GetTypeSize(returnDataType);
			if (typeSize > 0U)
			{
				obj = Activator.CreateInstance(returnDataType);
				GCHandle gchandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
				GCHandle gchandle2 = gchandle;
				try
				{
					while (num != 0 && num >= 0)
					{
						IntPtr intPtr = gchandle2.AddrOfPinnedObject();
						IntPtr intPtr2 = intPtr;
						num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Void*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, intPtr2.ToPointer(), typeSize, flush ? 1 : 0, *(*(int*)this.m_lpUM + 28));
						if (ref dataReturned != null && num >= 0)
						{
							dataReturned = (num == 0);
							num = 0;
						}
					}
				}
				finally
				{
					if (gchandle2.IsAllocated)
					{
						gchandle2.Free();
					}
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
			}
			return obj;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0006F1E8 File Offset: 0x0006E5E8
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0006F210 File Offset: 0x0006E610
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

		// Token: 0x060006BC RID: 1724 RVA: 0x0006F1D0 File Offset: 0x0006E5D0
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0006F0D4 File Offset: 0x0006E4D4
		private void OnParentLost(object sender, EventArgs e)
		{
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0006F0E8 File Offset: 0x0006E4E8
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x00063454 File Offset: 0x00062854
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0006EEAC File Offset: 0x0006E2AC
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

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0006349C File Offset: 0x0006289C
		[CLSCompliant(false)]
		public unsafe IDirect3DQuery9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000634DC File Offset: 0x000628DC
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DQuery9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060006C3 RID: 1731 RVA: 0x0006351C File Offset: 0x0006291C
		// (remove) Token: 0x060006C4 RID: 1732 RVA: 0x00063540 File Offset: 0x00062940
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

		// Token: 0x060006C6 RID: 1734 RVA: 0x0006358C File Offset: 0x0006298C
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001065 RID: 4197
		private Device parentDevice;

		// Token: 0x04001066 RID: 4198
		internal bool m_bDisposed;

		// Token: 0x04001068 RID: 4200
		internal Device pCachedResourcepDevice;

		// Token: 0x04001069 RID: 4201
		internal unsafe IDirect3DQuery9* m_lpUM;
	}
}
