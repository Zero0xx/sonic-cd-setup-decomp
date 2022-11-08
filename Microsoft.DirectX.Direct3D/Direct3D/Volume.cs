using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000112 RID: 274
	public sealed class Volume : MarshalByRefObject, IDisposable
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x0006C920 File Offset: 0x0006BD20
		public unsafe void Dispose()
		{
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				if (this.parentDevice != null && Device.IsUsingEventHandlers)
				{
					this.parentDevice.Disposing -= this.OnParentDisposed;
				}
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0006C9DC File Offset: 0x0006BDDC
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

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0006CA64 File Offset: 0x0006BE64
		public unsafe VolumeDescription Description
		{
			get
			{
				VolumeDescription result = default(VolumeDescription);
				result = new VolumeDescription();
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVOLUME_DESC*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 32));
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

		// Token: 0x06000769 RID: 1897 RVA: 0x0006CF6C File Offset: 0x0006C36C
		public unsafe GraphicsStream LockBox(Box box, LockFlags flags, out LockedBox lockedVolume)
		{
			return this.LockBoxInternal((_D3DBOX*)(&box), flags, out lockedVolume);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0006CF50 File Offset: 0x0006C350
		public GraphicsStream LockBox(LockFlags flags, out LockedBox lockedVolume)
		{
			return this.LockBoxInternal(null, flags, out lockedVolume);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0006CF34 File Offset: 0x0006C334
		public unsafe GraphicsStream LockBox(Box box, LockFlags flags)
		{
			return this.LockBoxInternal((_D3DBOX*)(&box), flags, 0);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0006CF18 File Offset: 0x0006C318
		public GraphicsStream LockBox(LockFlags flags)
		{
			return this.LockBoxInternal(null, flags, 0);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0006CFE8 File Offset: 0x0006C3E8
		public unsafe Array LockBox(Type typeLock, Box box, LockFlags flags, out LockedBox lockedVolume, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, (_D3DBOX*)(&box), flags, out lockedVolume, ranks);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0006CFC8 File Offset: 0x0006C3C8
		public Array LockBox(Type typeLock, LockFlags flags, out LockedBox lockedVolume, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, null, flags, out lockedVolume, ranks);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0006CFA8 File Offset: 0x0006C3A8
		public unsafe Array LockBox(Type typeLock, Box box, LockFlags flags, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, (_D3DBOX*)(&box), flags, 0, ranks);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0006CF88 File Offset: 0x0006C388
		public Array LockBox(Type typeLock, LockFlags flags, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, null, flags, 0, ranks);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0006CB2C File Offset: 0x0006BF2C
		public unsafe void UnlockBox()
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
			if (this.stream != null)
			{
				this.stream.Close();
				this.stream = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 40));
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

		// Token: 0x06000772 RID: 1906 RVA: 0x0006D008 File Offset: 0x0006C408
		public unsafe object GetContainer(Guid interfaceId)
		{
			void* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void**), this.m_lpUM, ref interfaceId, ref ptr, *(*(int*)this.m_lpUM + 28));
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
			if (interfaceId.Equals(<Module>.IID_IDirect3DDevice9))
			{
				return new Device((IDirect3DDevice9*)ptr);
			}
			interfaceId.Equals(<Module>.IID_IDirect3DBaseTexture9);
			if (interfaceId.Equals(<Module>.IID_IDirect3DTexture9))
			{
				return new Texture((IDirect3DTexture9*)ptr, this.Device, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DCubeTexture9))
			{
				return new CubeTexture((IDirect3DCubeTexture9*)ptr, this.Device, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVolumeTexture9))
			{
				return new VolumeTexture((IDirect3DVolumeTexture9*)ptr, this.Device, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVertexBuffer9))
			{
				return new VertexBuffer((IDirect3DVertexBuffer9*)ptr, this.Device, Usage.None, VertexFormats.Texture0, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DIndexBuffer9))
			{
				return new IndexBuffer((IDirect3DIndexBuffer9*)ptr, this.Device, Usage.None, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DSurface9))
			{
				return new Surface((IDirect3DSurface9*)ptr, this.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVolume9))
			{
				return new Volume((IDirect3DVolume9*)ptr, null);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DSwapChain9))
			{
				return new SwapChain((IDirect3DSwapChain9*)ptr, this.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVertexDeclaration9))
			{
				return new VertexDeclaration((IDirect3DVertexDeclaration9*)ptr, this.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVertexShader9))
			{
				return new VertexShader((IDirect3DVertexShader9*)ptr, this.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DPixelShader9))
			{
				return new PixelShader((IDirect3DPixelShader9*)ptr, this.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DStateBlock9))
			{
				return new StateBlock((IDirect3DStateBlock9*)ptr, this.Device);
			}
			return null;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0006CBF0 File Offset: 0x0006BFF0
		public unsafe void SetPrivateData(Guid guidData, byte[] privateData)
		{
			int num;
			if (privateData != null && privateData.Length != 0)
			{
				ref void void& = ref privateData[0];
				num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, ref guidData, ref void&, privateData.Length, 0, *(*(int*)this.m_lpUM + 16));
			}
			else
			{
				num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, ref guidData, 0, 0, 0, *(*(int*)this.m_lpUM + 16));
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

		// Token: 0x06000774 RID: 1908 RVA: 0x0006CC84 File Offset: 0x0006C084
		public unsafe byte[] GetPrivateData(Guid guidData)
		{
			byte[] result = null;
			uint num = 0U;
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, ref guidData, 0, ref num, *(*(int*)this.m_lpUM + 20));
			if (num > 0U)
			{
				byte[] array = new byte[num];
				array.Initialize();
				result = array;
				ref byte byte& = ref array[0];
				num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, ref guidData, ref byte&, ref num, *(*(int*)this.m_lpUM + 20));
			}
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
			return result;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0006CD28 File Offset: 0x0006C128
		public unsafe void FreePrivateData(Guid guidData)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier)), this.m_lpUM, ref guidData, *(*(int*)this.m_lpUM + 24));
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

		// Token: 0x06000776 RID: 1910 RVA: 0x00063F10 File Offset: 0x00063310
		[CLSCompliant(false)]
		public unsafe Volume(IDirect3DVolume9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00063EC8 File Offset: 0x000632C8
		public unsafe Volume(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DVolume9* lpUM = (IDirect3DVolume9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0006CEE4 File Offset: 0x0006C2E4
		[CLSCompliant(false)]
		public unsafe Volume(IDirect3DVolume9* lp, VolumeTexture device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.m_bLockArray = null;
			this.CreateObjects(device);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0006CACC File Offset: 0x0006BECC
		internal unsafe void LockBoxInternal(_D3DBOX* rect, LockFlags flags)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_BOX*,Microsoft.DirectX.PrivateImplementationDetails._D3DBOX modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, ref this.m_LockBox, rect, flags, *(*(int*)this.m_lpUM + 36));
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

		// Token: 0x0600077A RID: 1914 RVA: 0x0006CE64 File Offset: 0x0006C264
		internal unsafe GraphicsStream LockBoxInternal(_D3DBOX* rect, LockFlags flags, out LockedBox lockedVolume)
		{
			this.LockBoxInternal(rect, flags);
			this.m_bLockArray = *(ref this.m_LockBox + 8);
			if (ref lockedVolume != null)
			{
				lockedVolume.iRowPitch = this.m_LockBox;
				lockedVolume.iSlicePitch = *(ref this.m_LockBox + 4);
			}
			IntPtr dataPointer = 0;
			dataPointer = new IntPtr((void*)this.m_bLockArray);
			this.stream = new GraphicsStream(dataPointer, 2147483647L, true, (byte)(~(flags >> 4) & (LockFlags)1) != 0);
			return this.stream;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0006CDE0 File Offset: 0x0006C1E0
		internal unsafe Array LockBoxInternal(Type typeLock, _D3DBOX* rect, LockFlags flags, out LockedBox lockedVolume, int[] ranks)
		{
			Array array = null;
			this.LockBoxInternal(rect, flags);
			this.m_bLockArray = *(ref this.m_LockBox + 8);
			if (ref lockedVolume != null)
			{
				lockedVolume.iRowPitch = this.m_LockBox;
				lockedVolume.iSlicePitch = *(ref this.m_LockBox + 4);
			}
			array = Array.CreateInstance(typeLock, ranks);
			IntPtr pointerData = 0;
			pointerData = new IntPtr((void*)this.m_bLockArray);
			DXHelp.CopyPointerDataToObject(ref array, pointerData);
			this.m_managedArray = array;
			this.m_lockFlags = flags;
			return array;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0006C8BC File Offset: 0x0006BCBC
		protected unsafe override void Finalize()
		{
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
			}
			base.Finalize();
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0006CD98 File Offset: 0x0006C198
		private void CreateObjects(VolumeTexture device)
		{
			this.m_bDisposed = false;
			this.parentDevice = device;
			if (this.parentDevice != null && Device.IsUsingEventHandlers)
			{
				this.parentDevice.Disposing += this.OnParentDisposed;
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0006CD80 File Offset: 0x0006C180
		internal void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00063EB0 File Offset: 0x000632B0
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0006C9A4 File Offset: 0x0006BDA4
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

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00063EF8 File Offset: 0x000632F8
		[CLSCompliant(false)]
		public unsafe IDirect3DVolume9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00063F38 File Offset: 0x00063338
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DVolume9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000783 RID: 1923 RVA: 0x00063F78 File Offset: 0x00063378
		// (remove) Token: 0x06000784 RID: 1924 RVA: 0x00063F9C File Offset: 0x0006339C
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

		// Token: 0x06000786 RID: 1926 RVA: 0x00063FE8 File Offset: 0x000633E8
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001098 RID: 4248
		private unsafe byte* m_bLockArray;

		// Token: 0x04001099 RID: 4249
		private Array m_managedArray;

		// Token: 0x0400109A RID: 4250
		private GraphicsStream stream;

		// Token: 0x0400109B RID: 4251
		private LockFlags m_lockFlags;

		// Token: 0x0400109C RID: 4252
		private _D3DLOCKED_BOX m_LockBox;

		// Token: 0x0400109D RID: 4253
		private VolumeTexture parentDevice;

		// Token: 0x0400109E RID: 4254
		internal bool m_bDisposed;

		// Token: 0x040010A0 RID: 4256
		internal Device pCachedResourcepDevice;

		// Token: 0x040010A1 RID: 4257
		internal unsafe IDirect3DVolume9* m_lpUM;
	}
}
