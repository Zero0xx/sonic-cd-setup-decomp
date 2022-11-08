using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000105 RID: 261
	public sealed class IndexBuffer : Resource, IDisposable
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x000699E0 File Offset: 0x00068DE0
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

		// Token: 0x0600068E RID: 1678 RVA: 0x000632C0 File Offset: 0x000626C0
		[CLSCompliant(false)]
		public unsafe IndexBuffer(IDirect3DIndexBuffer9* pInterop) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DResource9*)pInterop);
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00063268 File Offset: 0x00062668
		public unsafe IndexBuffer(IntPtr unmanagedObject) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			IDirect3DIndexBuffer9* ptr = (IDirect3DIndexBuffer9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DResource9*)ptr);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00069D60 File Offset: 0x00069160
		[CLSCompliant(false)]
		public unsafe IndexBuffer(IDirect3DIndexBuffer9* lp, Type typeIndexType, int numberIndices, Device device, Usage usage, Pool pool) : base((IDirect3DResource9*)lp)
		{
			this.Created = null;
			this.Disposing = null;
			this.CreateObject(lp, typeIndexType, numberIndices, device, usage, pool, false, true);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00069DD8 File Offset: 0x000691D8
		[CLSCompliant(false)]
		public unsafe IndexBuffer(IDirect3DIndexBuffer9* lp, Device device, Usage usage, Pool pool) : base((IDirect3DResource9*)lp)
		{
			this.Created = null;
			this.Disposing = null;
			this.CreateObject(lp, null, 0, device, usage, pool, false, true);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00069E48 File Offset: 0x00069248
		public unsafe IndexBuffer(Device device, int sizeOfBufferInBytes, Usage usage, Pool pool, [MarshalAs(UnmanagedType.U1)] bool sixteenBitIndices) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			IDirect3DIndexBuffer9* lp = null;
			this.bufferSize = sizeOfBufferInBytes;
			this.m_format = (sixteenBitIndices ? 101 : 102);
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DIndexBuffer9**,System.Void**), device.m_lpUM, sizeOfBufferInBytes, usage, this.m_format, pool, ref lp, 0, *(*(int*)device.m_lpUM + 108));
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
			this.CreateObject(lp, null, 0, device, usage, pool, false, true);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00069E10 File Offset: 0x00069210
		public IndexBuffer(Type typeIndexType, int numberIndices, Device device, Usage usage, Pool pool) : base(null)
		{
			this.Created = null;
			this.Disposing = null;
			this.CreateObject(null, typeIndexType, numberIndices, device, usage, pool, true, true);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0006322C File Offset: 0x0006262C
		public Array Lock(int offsetToLock, LockFlags flags)
		{
			int[] array = new int[<Module>.$ConstGCArrayBound$0x092d518d$127$];
			array.Initialize();
			array[0] = this.m_NumVerts;
			return this.Lock(offsetToLock, this.m_Type, flags, array);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00069B34 File Offset: 0x00068F34
		public GraphicsStream Lock(int offsetToLock, int sizeToLock, LockFlags flags)
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
			dataPointer = new IntPtr(this.pUnmanagedData);
			this.returnedStream = new GraphicsStream(dataPointer, (long)num, (byte)(~(this.m_usage >> 3) & Usage.RenderTarget) != 0, (byte)(~(flags >> 4) & (LockFlags)1) != 0);
			return this.returnedStream;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00069AC0 File Offset: 0x00068EC0
		public Array Lock(int offsetToLock, Type typeIndex, LockFlags flags, params int[] ranks)
		{
			Array array = null;
			int size = 0;
			int num = ranks[0];
			if (typeIndex != null && num > 0)
			{
				size = DXHelp.GetObjectSize(typeIndex) * num;
			}
			this.InternalLock(offsetToLock, size, (int)flags);
			array = Array.CreateInstance(typeIndex, ranks);
			if ((this.m_usage & Usage.WriteOnly) != Usage.WriteOnly)
			{
				IntPtr pointerData = 0;
				pointerData = new IntPtr(this.pUnmanagedData);
				DXHelp.CopyPointerDataToObject(ref array, pointerData);
			}
			this.m_managedArray = array;
			this.m_lockFlags = flags;
			return array;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000696F8 File Offset: 0x00068AF8
		internal unsafe void InternalLock(int offsetToLock, int size, int flags)
		{
			if (this.bufferSize == 0)
			{
				this.bufferSize = int.MaxValue;
			}
			ref void* void*& = ref this.pUnmanagedData;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Void**,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, offsetToLock, size, ref void*&, flags, *(*(int*)this.m_lpUM + 44));
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

		// Token: 0x06000698 RID: 1688 RVA: 0x0006976C File Offset: 0x00068B6C
		public unsafe void Unlock()
		{
			if (this.pUnmanagedData == null)
			{
				throw new InvalidOperationException();
			}
			if (this.m_managedArray != null)
			{
				if ((this.m_lockFlags & LockFlags.ReadOnly) != LockFlags.ReadOnly)
				{
					IntPtr pointerData = 0;
					pointerData = new IntPtr(this.pUnmanagedData);
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
			this.pUnmanagedData = null;
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

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00069830 File Offset: 0x00068C30
		public unsafe IndexBufferDescription Description
		{
			get
			{
				IndexBufferDescription result = default(IndexBufferDescription);
				result = new IndexBufferDescription();
				initblk(ref result, 0, sizeof(IndexBufferDescription));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DINDEXBUFFER_DESC*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 52));
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

		// Token: 0x0600069A RID: 1690 RVA: 0x000698A4 File Offset: 0x00068CA4
		public unsafe void SetData(object data, int lockAtOffset, LockFlags flags)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			ref void* void*& = ref this.pUnmanagedData;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Void**,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, lockAtOffset, DXHelp.GetObjectSize(data), ref void*&, flags, *(*(int*)this.m_lpUM + 44));
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
			pointerData = new IntPtr(this.pUnmanagedData);
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

		// Token: 0x0600069B RID: 1691 RVA: 0x00069A98 File Offset: 0x00068E98
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00069BB4 File Offset: 0x00068FB4
		private unsafe void CreateObject(IDirect3DIndexBuffer9* lp, Type typeVertexType, int iNumVerts, Device device, Usage usage, Pool pool, [MarshalAs(UnmanagedType.U1)] bool createNewObject, [MarshalAs(UnmanagedType.U1)] bool firstCreate)
		{
			this.m_bDisposed = false;
			this.m_lpUM = null;
			if (createNewObject)
			{
				if (DXHelp.GetObjectSize(typeVertexType) == 4)
				{
					this.m_format = 102;
				}
				else
				{
					if (DXHelp.GetObjectSize(typeVertexType) != 2)
					{
						throw new ArgumentException(string.Empty, "typeIndexType");
					}
					this.m_format = 101;
				}
				ref IDirect3DIndexBuffer9* direct3DIndexBuffer9*& = ref this.m_lpUM;
				this.bufferSize = DXHelp.GetObjectSize(typeVertexType) * iNumVerts;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DIndexBuffer9**,System.Void**), device.m_lpUM, this.bufferSize, usage, this.m_format, pool, ref direct3DIndexBuffer9*&, 0, *(*(int*)device.m_lpUM + 108));
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
			this.pUnmanagedData = null;
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

		// Token: 0x0600069D RID: 1693 RVA: 0x00069B9C File Offset: 0x00068F9C
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00069980 File Offset: 0x00068D80
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

		// Token: 0x0600069F RID: 1695 RVA: 0x00069D98 File Offset: 0x00069198
		private void OnParentReset(object sender, EventArgs e)
		{
			if (this.m_Pool == Pool.Default && this.m_Type != null)
			{
				this.CreateObject(null, this.m_Type, this.m_NumVerts, sender, this.m_usage, Pool.Default, true, false);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000631FC File Offset: 0x000625FC
		public int SizeInBytes
		{
			get
			{
				return this.bufferSize;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00063214 File Offset: 0x00062614
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000696C0 File Offset: 0x00068AC0
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

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x000632A8 File Offset: 0x000626A8
		[CLSCompliant(false)]
		public new unsafe IDirect3DIndexBuffer9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000632F8 File Offset: 0x000626F8
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DIndexBuffer9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060006A5 RID: 1701 RVA: 0x00063338 File Offset: 0x00062738
		// (remove) Token: 0x060006A6 RID: 1702 RVA: 0x0006335C File Offset: 0x0006275C
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

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006A8 RID: 1704 RVA: 0x000633A8 File Offset: 0x000627A8
		// (remove) Token: 0x060006A9 RID: 1705 RVA: 0x000633CC File Offset: 0x000627CC
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

		// Token: 0x060006AB RID: 1707 RVA: 0x00063418 File Offset: 0x00062818
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001055 RID: 4181
		private Type m_Type;

		// Token: 0x04001056 RID: 4182
		private int m_NumVerts;

		// Token: 0x04001057 RID: 4183
		private unsafe void* pUnmanagedData;

		// Token: 0x04001058 RID: 4184
		private Array m_managedArray;

		// Token: 0x04001059 RID: 4185
		private GraphicsStream returnedStream;

		// Token: 0x0400105A RID: 4186
		private Usage m_usage;

		// Token: 0x0400105B RID: 4187
		private int m_format;

		// Token: 0x0400105C RID: 4188
		private Pool m_Pool;

		// Token: 0x0400105D RID: 4189
		private int m_iTypeSize;

		// Token: 0x0400105E RID: 4190
		private LockFlags m_lockFlags;

		// Token: 0x0400105F RID: 4191
		private int bufferSize;

		// Token: 0x04001060 RID: 4192
		private Device parentDevice;

		// Token: 0x04001062 RID: 4194
		internal bool m_bDisposed;

		// Token: 0x04001064 RID: 4196
		internal new unsafe IDirect3DIndexBuffer9* m_lpUM;
	}
}
