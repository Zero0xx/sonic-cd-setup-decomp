using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F9 RID: 249
	public sealed class CubeTexture : BaseTexture, IDisposable
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x00064AC8 File Offset: 0x00063EC8
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(CubeTexture) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00064B04 File Offset: 0x00063F04
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(CubeTexture left, CubeTexture right)
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

		// Token: 0x06000554 RID: 1364 RVA: 0x00064ED0 File Offset: 0x000642D0
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(CubeTexture left, CubeTexture right)
		{
			return !(left == right);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00064B34 File Offset: 0x00063F34
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00064EEC File Offset: 0x000642EC
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

		// Token: 0x06000557 RID: 1367 RVA: 0x0006496C File Offset: 0x00063D6C
		[CLSCompliant(false)]
		public unsafe CubeTexture(IDirect3DCubeTexture9* pInterop) : base(null)
		{
			this.Disposing = null;
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DBaseTexture9*)pInterop);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00064934 File Offset: 0x00063D34
		public unsafe CubeTexture(IntPtr unmanagedObject) : base(null)
		{
			this.Disposing = null;
			IDirect3DCubeTexture9* ptr = (IDirect3DCubeTexture9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DBaseTexture9*)ptr);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000652C0 File Offset: 0x000646C0
		[CLSCompliant(false)]
		public unsafe CubeTexture(IDirect3DCubeTexture9* lp, Device device, Pool pool) : base((IDirect3DBaseTexture9*)lp)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.m_bLockArray = null;
			this.stream = null;
			this.CreateObjects(device, pool);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00065300 File Offset: 0x00064700
		public unsafe CubeTexture(Device device, int edgeLength, int levels, Usage usage, Format format, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			this.m_Usage = (uint)usage;
			ref IDirect3DCubeTexture9* direct3DCubeTexture9*& = ref this.m_lpUM;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DCubeTexture9**,System.Void**), device.m_lpUM, edgeLength, levels, usage, format, pool, ref direct3DCubeTexture9*&, 0, *(*(int*)device.m_lpUM + 100));
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
			this.stream = null;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00064B8C File Offset: 0x00063F8C
		public unsafe void AddDirtyRectangle(CubeMapFace faceType)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, faceType, 0, *(*(int*)this.m_lpUM + 84));
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

		// Token: 0x0600055C RID: 1372 RVA: 0x00064BE4 File Offset: 0x00063FE4
		public unsafe void AddDirtyRectangle(CubeMapFace faceType, Rectangle rect)
		{
			rect.Width += rect.X;
			rect.Height += rect.Y;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, faceType, ref rect, *(*(int*)this.m_lpUM + 84));
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

		// Token: 0x0600055D RID: 1373 RVA: 0x00064C6C File Offset: 0x0006406C
		public unsafe SurfaceDescription GetLevelDescription(int level)
		{
			SurfaceDescription result = default(SurfaceDescription);
			result = new SurfaceDescription();
			initblk(ref result, 0, sizeof(SurfaceDescription));
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DSURFACE_DESC*), this.m_lpUM, level, ref result, *(*(int*)this.m_lpUM + 68));
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

		// Token: 0x0600055E RID: 1374 RVA: 0x00064CE0 File Offset: 0x000640E0
		public unsafe Surface GetCubeMapSurface(CubeMapFace faceType, int level)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**), this.m_lpUM, faceType, level, ref ptr, *(*(int*)this.m_lpUM + 72));
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
				if (this.pCachedSurfaces[(int)faceType] != null)
				{
					this.pCachedSurfaces[(int)faceType].UpdateUnmanagedPointer(ptr);
				}
				else
				{
					this.pCachedSurfaces[(int)faceType] = new Surface(ptr, this);
				}
				return this.pCachedSurfaces[(int)faceType];
			}
			return null;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00065210 File Offset: 0x00064610
		public unsafe GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch)
		{
			return this.LockRectangleInternal(faceType, level, (tagRECT*)(&rect), flags, out pitch);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000651F0 File Offset: 0x000645F0
		public GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags, out int pitch)
		{
			return this.LockRectangleInternal(faceType, level, null, flags, out pitch);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000651D0 File Offset: 0x000645D0
		public unsafe GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags)
		{
			return this.LockRectangleInternal(faceType, level, (tagRECT*)(&rect), flags, 0);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000651B0 File Offset: 0x000645B0
		public GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags)
		{
			return this.LockRectangleInternal(faceType, level, null, flags, 0);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0006529C File Offset: 0x0006469C
		public unsafe Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, faceType, level, (tagRECT*)(&rect), flags, out pitch, ranks);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00065278 File Offset: 0x00064678
		public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, out int pitch, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, faceType, level, null, flags, out pitch, ranks);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00065254 File Offset: 0x00064654
		public unsafe Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, faceType, level, (tagRECT*)(&rect), flags, 0, ranks);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00065230 File Offset: 0x00064630
		public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, faceType, level, null, flags, 0, ranks);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00064DD8 File Offset: 0x000641D8
		public unsafe void UnlockRectangle(CubeMapFace faceType, int level)
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
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32), this.m_lpUM, faceType, level, *(*(int*)this.m_lpUM + 80));
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

		// Token: 0x06000568 RID: 1384 RVA: 0x00064D74 File Offset: 0x00064174
		private unsafe void LockRectangleInternal(CubeMapFace faceType, int level, tagRECT* rect, LockFlags flags)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_RECT*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, faceType, level, ref this.m_lockRectangle, rect, flags, *(*(int*)this.m_lpUM + 76));
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

		// Token: 0x06000569 RID: 1385 RVA: 0x00065064 File Offset: 0x00064464
		private unsafe GraphicsStream LockRectangleInternal(CubeMapFace faceType, int level, tagRECT* rect, LockFlags flags, out int pitch)
		{
			if (rect != null)
			{
				*(int*)(rect + 8 / sizeof(tagRECT)) = *(int*)(rect + 8 / sizeof(tagRECT)) + *(int*)rect;
				*(int*)(rect + 12 / sizeof(tagRECT)) = *(int*)(rect + 12 / sizeof(tagRECT)) + *(int*)(rect + 4 / sizeof(tagRECT));
			}
			this.LockRectangleInternal(faceType, level, rect, flags);
			this.m_bLockArray = *(ref this.m_lockRectangle + 4);
			if (ref pitch != null)
			{
				pitch = this.m_lockRectangle;
			}
			IntPtr dataPointer = 0;
			dataPointer = new IntPtr((void*)this.m_bLockArray);
			this.stream = new GraphicsStream(dataPointer, 2147483647L, (byte)(~(this.m_Usage >> 3) & 1U) != 0, (byte)(~(flags >> 4) & (LockFlags)1) != 0);
			return this.stream;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00064FCC File Offset: 0x000643CC
		private unsafe Array LockRectangleInternal(Type typeLock, CubeMapFace faceType, int level, tagRECT* rect, LockFlags flags, out int pitch, int[] ranks)
		{
			Array array = null;
			if (rect != null)
			{
				*(int*)(rect + 8 / sizeof(tagRECT)) = *(int*)(rect + 8 / sizeof(tagRECT)) + *(int*)rect;
				*(int*)(rect + 12 / sizeof(tagRECT)) = *(int*)(rect + 12 / sizeof(tagRECT)) + *(int*)(rect + 4 / sizeof(tagRECT));
			}
			this.LockRectangleInternal(faceType, level, rect, flags);
			this.m_bLockArray = *(ref this.m_lockRectangle + 4);
			if (ref pitch != null)
			{
				pitch = this.m_lockRectangle;
			}
			array = Array.CreateInstance(typeLock, ranks);
			IntPtr pointerData = 0;
			pointerData = new IntPtr((void*)this.m_bLockArray);
			DXHelp.CopyPointerDataToObject(ref array, pointerData);
			this.m_managedArray = array;
			this.m_lockFlags = flags;
			return array;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00065114 File Offset: 0x00064514
		private unsafe void CreateObjects(Device device, Pool pool)
		{
			if (this.pCachedSurfaces == null)
			{
				this.pCachedSurfaces = new Surface[6];
			}
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

		// Token: 0x0600056C RID: 1388 RVA: 0x000650FC File Offset: 0x000644FC
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00064E9C File Offset: 0x0006429C
		private void OnParentLost(object sender, EventArgs e)
		{
			if (this.m_Pool == Pool.Default)
			{
				this.Dispose();
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00064EBC File Offset: 0x000642BC
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00064FA4 File Offset: 0x000643A4
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x000622BC File Offset: 0x000616BC
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00064B54 File Offset: 0x00063F54
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

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x000622D4 File Offset: 0x000616D4
		[CLSCompliant(false)]
		public new unsafe IDirect3DCubeTexture9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000622EC File Offset: 0x000616EC
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DCubeTexture9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000574 RID: 1396 RVA: 0x0006232C File Offset: 0x0006172C
		// (remove) Token: 0x06000575 RID: 1397 RVA: 0x00062350 File Offset: 0x00061750
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

		// Token: 0x06000577 RID: 1399 RVA: 0x0006239C File Offset: 0x0006179C
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001027 RID: 4135
		private unsafe byte* m_bLockArray;

		// Token: 0x04001028 RID: 4136
		private Device pDevice;

		// Token: 0x04001029 RID: 4137
		private Pool m_Pool;

		// Token: 0x0400102A RID: 4138
		private uint m_Usage;

		// Token: 0x0400102B RID: 4139
		private Array m_managedArray;

		// Token: 0x0400102C RID: 4140
		private GraphicsStream stream;

		// Token: 0x0400102D RID: 4141
		private LockFlags m_lockFlags;

		// Token: 0x0400102E RID: 4142
		private _D3DLOCKED_RECT m_lockRectangle;

		// Token: 0x0400102F RID: 4143
		private Surface[] pCachedSurfaces;

		// Token: 0x04001030 RID: 4144
		internal bool m_bDisposed;

		// Token: 0x04001032 RID: 4146
		internal new unsafe IDirect3DCubeTexture9* m_lpUM;
	}
}
