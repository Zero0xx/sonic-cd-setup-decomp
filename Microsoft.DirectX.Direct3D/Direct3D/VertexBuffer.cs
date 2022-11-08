using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000110 RID: 272
	public sealed class VertexBuffer : Resource, IDisposable
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x0006C3F0 File Offset: 0x0006B7F0
		public unsafe override void Dispose()
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

		// Token: 0x06000747 RID: 1863 RVA: 0x00063D38 File Offset: 0x00063138
		[CLSCompliant(false)]
		public unsafe VertexBuffer(IDirect3DVertexBuffer9* pInterop) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DResource9*)pInterop);
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00063CE0 File Offset: 0x000630E0
		public unsafe VertexBuffer(IntPtr unmanagedObject) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			IDirect3DVertexBuffer9* ptr = (IDirect3DVertexBuffer9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DResource9*)ptr);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0006C734 File Offset: 0x0006BB34
		[CLSCompliant(false)]
		public unsafe VertexBuffer(IDirect3DVertexBuffer9* lp, Type tType, int iNumVerts, Device device, Usage usage, VertexFormats vertexFormat, Pool Pool) : base((IDirect3DResource9*)lp)
		{
			this.Created = null;
			this.Disposing = null;
			this.CreateObject(lp, tType, iNumVerts, device, usage, vertexFormat, Pool, false, true);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0006C7B8 File Offset: 0x0006BBB8
		[CLSCompliant(false)]
		public unsafe VertexBuffer(IDirect3DVertexBuffer9* lp, Device device, Usage usage, VertexFormats vertexFormat, Pool pool) : base((IDirect3DResource9*)lp)
		{
			this.Created = null;
			this.Disposing = null;
			this.CreateObject(lp, null, 0, device, usage, vertexFormat, pool, false, true);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0006C82C File Offset: 0x0006BC2C
		public unsafe VertexBuffer(Device device, int sizeOfBufferInBytes, Usage usage, VertexFormats vertexFormat, Pool pool) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			IDirect3DVertexBuffer9* lp = null;
			this.bufferSize = sizeOfBufferInBytes;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexBuffer9**,System.Void**), device.m_lpUM, sizeOfBufferInBytes, usage, vertexFormat, pool, ref lp, 0, *(*(int*)device.m_lpUM + 104));
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
			this.CreateObject(lp, null, 0, device, usage, vertexFormat, pool, false, true);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0006C7F0 File Offset: 0x0006BBF0
		public VertexBuffer(Type typeVertexType, int numVerts, Device device, Usage usage, VertexFormats vertexFormat, Pool pool) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			this.CreateObject(null, typeVertexType, numVerts, device, usage, vertexFormat, pool, true, true);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00063CA4 File Offset: 0x000630A4
		public Array Lock(int offsetToLock, LockFlags flags)
		{
			int[] array = new int[<Module>.$ConstGCArrayBound$0x092d518d$128$];
			array.Initialize();
			array[0] = this.m_NumVerts;
			return this.Lock(offsetToLock, this.m_Type, flags, array);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0006C53C File Offset: 0x0006B93C
		public unsafe GraphicsStream Lock(int offsetToLock, int sizeToLock, LockFlags flags)
		{
			this.InternalLock(offsetToLock, sizeToLock, (int)flags);
			int num;
			if (sizeToLock == 0)
			{
				num = this.bufferSize;
			}
			else
			{
				num = sizeToLock;
			}
			IntPtr dataPointer = 0;
			dataPointer = new IntPtr((void*)this.m_bLockArray);
			this.returnedStream = new GraphicsStream(dataPointer, (long)num, (byte)(~(this.m_usage >> 3) & Usage.RenderTarget) != 0, (byte)(~(flags >> 4) & (LockFlags)1) != 0);
			return this.returnedStream;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0006C4D0 File Offset: 0x0006B8D0
		public unsafe Array Lock(int offsetToLock, Type typeVertex, LockFlags flags, params int[] ranks)
		{
			Array array = null;
			int num = ranks[0];
			int size = DXHelp.GetObjectSize(typeVertex) * num;
			this.InternalLock(offsetToLock, size, (int)flags);
			array = Array.CreateInstance(typeVertex, ranks);
			if ((this.m_usage & Usage.WriteOnly) != Usage.WriteOnly)
			{
				IntPtr pointerData = 0;
				pointerData = new IntPtr((void*)this.m_bLockArray);
				DXHelp.CopyPointerDataToObject(ref array, pointerData);
			}
			this.m_managedArray = array;
			this.m_lockFlags = flags;
			return array;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0006C0F0 File Offset: 0x0006B4F0
		internal unsafe void InternalLock(int offsetToLock, int size, int flags)
		{
			if (this.bufferSize == 0)
			{
				this.bufferSize = int.MaxValue;
			}
			ref byte* byte*& = ref this.m_bLockArray;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Void**,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, offsetToLock, size, ref byte*&, flags, *(*(int*)this.m_lpUM + 44));
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

		// Token: 0x06000751 RID: 1873 RVA: 0x0006C164 File Offset: 0x0006B564
		public unsafe void Unlock()
		{
			if (this.m_bLockArray == null)
			{
				throw new InvalidOperationException();
			}
			if (this.m_managedArray != null)
			{
				if ((this.m_lockFlags & LockFlags.ReadOnly) != LockFlags.ReadOnly)
				{
					IntPtr pointerData = 0;
					pointerData = new IntPtr((void*)this.m_bLockArray);
					DXHelp.CopyObjectDataToPointer(this.m_managedArray, pointerData);
				}
				this.m_managedArray = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 48));
			if (this.returnedStream != null)
			{
				this.returnedStream.Close();
				this.returnedStream = null;
			}
			this.m_bLockArray = null;
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

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0006C228 File Offset: 0x0006B628
		public unsafe VertexBufferDescription Description
		{
			get
			{
				VertexBufferDescription result = default(VertexBufferDescription);
				result = new VertexBufferDescription();
				initblk(ref result, 0, sizeof(VertexBufferDescription));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVERTEXBUFFER_DESC*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 52));
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

		// Token: 0x06000753 RID: 1875 RVA: 0x0006C29C File Offset: 0x0006B69C
		public unsafe void SetData(object data, int lockAtOffset, LockFlags flags)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			ref byte* byte*& = ref this.m_bLockArray;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Void**,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, lockAtOffset, DXHelp.GetObjectSize(data), ref byte*&, flags, *(*(int*)this.m_lpUM + 44));
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
			IntPtr pointerData = 0;
			pointerData = new IntPtr((void*)this.m_bLockArray);
			DXHelp.CopyObjectDataToPointer(data, pointerData);
			num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 48));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal2 = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex2 = exceptionFromResultInternal2 as DirectXException;
					if (ex2 != null)
					{
						ex2.ErrorCode = num;
						throw ex2;
					}
					throw exceptionFromResultInternal2;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0006C378 File Offset: 0x0006B778
		internal int TypeSize
		{
			get
			{
				return this.m_iTypeSize;
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0006C4A8 File Offset: 0x0006B8A8
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0006C5BC File Offset: 0x0006B9BC
		private unsafe void CreateObject(IDirect3DVertexBuffer9* lp, Type typeVertexType, int iNumVerts, Device device, Usage usage, VertexFormats vertexFormat, Pool pool, [MarshalAs(UnmanagedType.U1)] bool createNewObject, [MarshalAs(UnmanagedType.U1)] bool firstCreate)
		{
			this.m_bDisposed = false;
			this.m_lpUM = null;
			if (createNewObject)
			{
				ref IDirect3DVertexBuffer9* direct3DVertexBuffer9*& = ref this.m_lpUM;
				this.bufferSize = DXHelp.GetObjectSize(typeVertexType) * iNumVerts;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexBuffer9**,System.Void**), device.m_lpUM, this.bufferSize, usage, vertexFormat, pool, ref direct3DVertexBuffer9*&, 0, *(*(int*)device.m_lpUM + 104));
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
			else
			{
				this.m_lpUM = lp;
			}
			this.m_Type = typeVertexType;
			this.m_NumVerts = iNumVerts;
			this.m_bLockArray = null;
			int iTypeSize;
			if (typeVertexType == null)
			{
				iTypeSize = 0;
			}
			else
			{
				iTypeSize = DXHelp.GetObjectSize(typeVertexType);
			}
			this.m_iTypeSize = iTypeSize;
			this.m_usage = usage;
			this.m_FVF = vertexFormat;
			this.m_Pool = pool;
			this.returnedStream = null;
			this.parentDevice = device;
			if (firstCreate && this.parentDevice != null && Device.IsUsingEventHandlers)
			{
				this.parentDevice.DeviceLost += this.OnParentLost;
				this.parentDevice.DeviceReset += this.OnParentReset;
				this.parentDevice.Disposing += this.OnParentDisposed;
			}
			base.SetObject((IDirect3DResource9*)this.m_lpUM);
			this.raise_Created(this, EventArgs.Empty);
			if (this.bufferSize == 0)
			{
				this.bufferSize = int.MaxValue;
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0006C5A4 File Offset: 0x0006B9A4
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0006C390 File Offset: 0x0006B790
		private void OnParentLost(object sender, EventArgs e)
		{
			if (this.m_Pool == Pool.Default)
			{
				this.Dispose();
				if (sender != null && Device.IsUsingEventHandlers)
				{
					sender.DeviceLost += this.OnParentLost;
					sender.DeviceReset += this.OnParentReset;
					sender.Disposing += this.OnParentDisposed;
				}
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0006C770 File Offset: 0x0006BB70
		private void OnParentReset(object sender, EventArgs e)
		{
			if (this.m_Pool == Pool.Default && this.m_Type != null)
			{
				this.CreateObject(null, this.m_Type, this.m_NumVerts, sender, this.m_usage, this.m_FVF, Pool.Default, true, false);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00063C74 File Offset: 0x00063074
		public int SizeInBytes
		{
			get
			{
				return this.bufferSize;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x00063C8C File Offset: 0x0006308C
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0006C0B8 File Offset: 0x0006B4B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new unsafe IntPtr GetObjectByValue(int uniqueKey)
		{
			if (uniqueKey == -759872593)
			{
				IntPtr result = 0;
				result = new IntPtr((void*)this.m_lpUM);
				return result;
			}
			throw new ArgumentException();
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00063D20 File Offset: 0x00063120
		[CLSCompliant(false)]
		public new unsafe IDirect3DVertexBuffer9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00063D70 File Offset: 0x00063170
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DVertexBuffer9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600075F RID: 1887 RVA: 0x00063DB0 File Offset: 0x000631B0
		// (remove) Token: 0x06000760 RID: 1888 RVA: 0x00063DD4 File Offset: 0x000631D4
		public event EventHandler Created
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Created = Delegate.Combine(this.Created, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Created = Delegate.Remove(this.Created, value);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000762 RID: 1890 RVA: 0x00063E20 File Offset: 0x00063220
		// (remove) Token: 0x06000763 RID: 1891 RVA: 0x00063E44 File Offset: 0x00063244
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

		// Token: 0x06000765 RID: 1893 RVA: 0x00063E90 File Offset: 0x00063290
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001088 RID: 4232
		private Type m_Type;

		// Token: 0x04001089 RID: 4233
		private int m_NumVerts;

		// Token: 0x0400108A RID: 4234
		private unsafe byte* m_bLockArray;

		// Token: 0x0400108B RID: 4235
		private Array m_managedArray;

		// Token: 0x0400108C RID: 4236
		private GraphicsStream returnedStream;

		// Token: 0x0400108D RID: 4237
		private int m_iTypeSize;

		// Token: 0x0400108E RID: 4238
		private Usage m_usage;

		// Token: 0x0400108F RID: 4239
		private VertexFormats m_FVF;

		// Token: 0x04001090 RID: 4240
		private Pool m_Pool;

		// Token: 0x04001091 RID: 4241
		private LockFlags m_lockFlags;

		// Token: 0x04001092 RID: 4242
		private int bufferSize;

		// Token: 0x04001093 RID: 4243
		private Device parentDevice;

		// Token: 0x04001095 RID: 4245
		internal bool m_bDisposed;

		// Token: 0x04001097 RID: 4247
		internal new unsafe IDirect3DVertexBuffer9* m_lpUM;
	}
}
