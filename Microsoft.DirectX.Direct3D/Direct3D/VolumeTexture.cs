using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000116 RID: 278
	public sealed class VolumeTexture : BaseTexture, IDisposable
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x0006D25C File Offset: 0x0006C65C
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(VolumeTexture) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0006D298 File Offset: 0x0006C698
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(VolumeTexture left, VolumeTexture right)
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

		// Token: 0x06000789 RID: 1929 RVA: 0x0006D608 File Offset: 0x0006CA08
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(VolumeTexture left, VolumeTexture right)
		{
			return !(left == right);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0006D2C8 File Offset: 0x0006C6C8
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0006D624 File Offset: 0x0006CA24
		public unsafe override void Dispose()
		{
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				if (this.pDevice != null && Device.IsUsingEventHandlers)
				{
					this.pDevice.DeviceLost -= this.OnParentLost;
					this.pDevice.DeviceReset -= this.OnParentReset;
					this.pDevice.Disposing -= this.OnParentDisposed;
				}
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00064A78 File Offset: 0x00063E78
		[CLSCompliant(false)]
		public unsafe VolumeTexture(IDirect3DVolumeTexture9* pInterop) : base(null)
		{
			this.Disposing = null;
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DBaseTexture9*)pInterop);
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00064A40 File Offset: 0x00063E40
		public unsafe VolumeTexture(IntPtr unmanagedObject) : base(null)
		{
			this.Disposing = null;
			IDirect3DVolumeTexture9* ptr = (IDirect3DVolumeTexture9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DBaseTexture9*)ptr);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0006D9B0 File Offset: 0x0006CDB0
		[CLSCompliant(false)]
		public unsafe VolumeTexture(IDirect3DVolumeTexture9* lp, Device device, Pool pool) : base((IDirect3DBaseTexture9*)lp)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.m_bLockArray = null;
			this.CreateObjects(device, pool);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0006D9E8 File Offset: 0x0006CDE8
		public unsafe VolumeTexture(Device device, int width, int height, int depth, int numLevels, Usage usage, Format format, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DVolumeTexture9* direct3DVolumeTexture9*& = ref this.m_lpUM;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVolumeTexture9**,System.Void**), device.m_lpUM, width, height, depth, numLevels, usage, format, pool, ref direct3DVolumeTexture9*&, 0, *(*(int*)device.m_lpUM + 96));
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
			this.CreateObjects(device, pool);
			this.m_bLockArray = null;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0006D378 File Offset: 0x0006C778
		public unsafe void AddDirtyBox(Box box)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DBOX modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ref box, *(*(int*)this.m_lpUM + 84));
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

		// Token: 0x06000791 RID: 1937 RVA: 0x0006D320 File Offset: 0x0006C720
		public unsafe void AddDirtyBox()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DBOX modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, 0, *(*(int*)this.m_lpUM + 84));
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

		// Token: 0x06000792 RID: 1938 RVA: 0x0006D3D0 File Offset: 0x0006C7D0
		public unsafe VolumeDescription GetLevelDescription(int level)
		{
			VolumeDescription result = default(VolumeDescription);
			result = new VolumeDescription();
			initblk(ref result, 0, sizeof(VolumeDescription));
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DVOLUME_DESC*), this.m_lpUM, level, ref result, *(*(int*)this.m_lpUM + 68));
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

		// Token: 0x06000793 RID: 1939 RVA: 0x0006D908 File Offset: 0x0006CD08
		public unsafe GraphicsStream LockBox(int level, Box box, LockFlags flags, out LockedBox lockedVolume)
		{
			return this.LockBoxInternal(level, (_D3DBOX*)(&box), flags, out lockedVolume);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0006D8EC File Offset: 0x0006CCEC
		public GraphicsStream LockBox(int level, LockFlags flags, out LockedBox lockedVolume)
		{
			return this.LockBoxInternal(level, null, flags, out lockedVolume);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0006D8CC File Offset: 0x0006CCCC
		public unsafe GraphicsStream LockBox(int level, Box box, LockFlags flags)
		{
			return this.LockBoxInternal(level, (_D3DBOX*)(&box), flags, 0);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0006D8B0 File Offset: 0x0006CCB0
		public GraphicsStream LockBox(int level, LockFlags flags)
		{
			return this.LockBoxInternal(level, null, flags, 0);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0006D98C File Offset: 0x0006CD8C
		public unsafe Array LockBox(Type typeLock, int level, Box box, LockFlags flags, out LockedBox lockedVolume, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, level, (_D3DBOX*)(&box), flags, out lockedVolume, ranks);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0006D96C File Offset: 0x0006CD6C
		public Array LockBox(Type typeLock, int level, LockFlags flags, out LockedBox lockedVolume, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, level, null, flags, out lockedVolume, ranks);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0006D948 File Offset: 0x0006CD48
		public unsafe Array LockBox(Type typeLock, int level, Box box, LockFlags flags, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, level, (_D3DBOX*)(&box), flags, 0, ranks);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0006D928 File Offset: 0x0006CD28
		public Array LockBox(Type typeLock, int level, LockFlags flags, params int[] ranks)
		{
			return this.LockBoxInternal(typeLock, level, null, flags, 0, ranks);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0006D4A8 File Offset: 0x0006C8A8
		public unsafe void UnlockBox(int level)
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
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32), this.m_lpUM, level, *(*(int*)this.m_lpUM + 80));
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

		// Token: 0x0600079C RID: 1948 RVA: 0x0006D56C File Offset: 0x0006C96C
		public unsafe Volume GetVolumeLevel(int level)
		{
			IDirect3DVolume9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVolume9**), this.m_lpUM, level, ref ptr, *(*(int*)this.m_lpUM + 72));
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
				return new Volume(ptr, this);
			}
			return null;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0006D828 File Offset: 0x0006CC28
		internal unsafe void CreateObjects(Device device, Pool pool)
		{
			this.m_bDisposed = false;
			this.pDevice = device;
			this.m_Pool = pool;
			if (this.pDevice != null && Device.IsUsingEventHandlers)
			{
				this.pDevice.DeviceLost += this.OnParentLost;
				this.pDevice.DeviceReset += this.OnParentReset;
				this.pDevice.Disposing += this.OnParentDisposed;
			}
			base.SetObject((IDirect3DBaseTexture9*)this.m_lpUM);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0006D6DC File Offset: 0x0006CADC
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0006D810 File Offset: 0x0006CC10
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0006D5D4 File Offset: 0x0006C9D4
		private void OnParentLost(object sender, EventArgs e)
		{
			if (this.m_Pool == Pool.Default)
			{
				this.Dispose();
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0006D5F4 File Offset: 0x0006C9F4
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00064008 File Offset: 0x00063408
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0006D444 File Offset: 0x0006C844
		internal unsafe void LockBoxInternal(int level, _D3DBOX* rect, LockFlags flags)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_BOX*,Microsoft.DirectX.PrivateImplementationDetails._D3DBOX modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, level, ref this.m_LockBox, rect, flags, *(*(int*)this.m_lpUM + 76));
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

		// Token: 0x060007A4 RID: 1956 RVA: 0x0006D78C File Offset: 0x0006CB8C
		internal unsafe GraphicsStream LockBoxInternal(int level, _D3DBOX* rect, LockFlags flags, out LockedBox lockedVolume)
		{
			this.LockBoxInternal(level, rect, flags);
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

		// Token: 0x060007A5 RID: 1957 RVA: 0x0006D704 File Offset: 0x0006CB04
		internal unsafe Array LockBoxInternal(Type typeLock, int level, _D3DBOX* rect, LockFlags flags, out LockedBox lockedVolume, int[] ranks)
		{
			Array array = null;
			this.LockBoxInternal(level, rect, flags);
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

		// Token: 0x060007A6 RID: 1958 RVA: 0x0006D2E8 File Offset: 0x0006C6E8
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

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00064020 File Offset: 0x00063420
		[CLSCompliant(false)]
		public new unsafe IDirect3DVolumeTexture9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00064038 File Offset: 0x00063438
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DVolumeTexture9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060007A9 RID: 1961 RVA: 0x00064078 File Offset: 0x00063478
		// (remove) Token: 0x060007AA RID: 1962 RVA: 0x0006409C File Offset: 0x0006349C
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

		// Token: 0x060007AC RID: 1964 RVA: 0x000640E8 File Offset: 0x000634E8
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040010A2 RID: 4258
		private Pool m_Pool;

		// Token: 0x040010A3 RID: 4259
		private Device pDevice;

		// Token: 0x040010A4 RID: 4260
		internal bool m_bDisposed;

		// Token: 0x040010A6 RID: 4262
		private unsafe byte* m_bLockArray;

		// Token: 0x040010A7 RID: 4263
		private Array m_managedArray;

		// Token: 0x040010A8 RID: 4264
		private GraphicsStream stream;

		// Token: 0x040010A9 RID: 4265
		private LockFlags m_lockFlags;

		// Token: 0x040010AA RID: 4266
		private _D3DLOCKED_BOX m_LockBox;

		// Token: 0x040010AB RID: 4267
		internal new unsafe IDirect3DVolumeTexture9* m_lpUM;
	}
}
