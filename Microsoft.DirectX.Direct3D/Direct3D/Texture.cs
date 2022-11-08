using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200010E RID: 270
	public sealed class Texture : BaseTexture, IDisposable
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x0006B5DC File Offset: 0x0006A9DC
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(Texture) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0006B618 File Offset: 0x0006AA18
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(Texture left, Texture right)
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

		// Token: 0x0600071D RID: 1821 RVA: 0x0006BB98 File Offset: 0x0006AF98
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(Texture left, Texture right)
		{
			return !(left == right);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0006B648 File Offset: 0x0006AA48
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0006BBB4 File Offset: 0x0006AFB4
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

		// Token: 0x06000720 RID: 1824 RVA: 0x00064A0C File Offset: 0x00063E0C
		[CLSCompliant(false)]
		public unsafe Texture(IDirect3DTexture9* pInterop) : base(null)
		{
			this.Disposing = null;
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DBaseTexture9*)pInterop);
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000649D4 File Offset: 0x00063DD4
		public unsafe Texture(IntPtr unmanagedObject) : base(null)
		{
			this.Disposing = null;
			IDirect3DTexture9* ptr = (IDirect3DTexture9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DBaseTexture9*)ptr);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0006BF5C File Offset: 0x0006B35C
		[CLSCompliant(false)]
		public unsafe Texture(IDirect3DTexture9* lp, Device device, Pool pool) : base((IDirect3DBaseTexture9*)lp)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.m_bLockArray = null;
			this.CreateObjects(device, pool);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0006C070 File Offset: 0x0006B470
		public Texture(Device device, Stream data, Usage usage, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			this.CreateTextureFromBitmap(device, Image.FromStream(data), usage, pool);
			this.CreateObjects(device, pool);
			this.m_bLockArray = null;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0006C02C File Offset: 0x0006B42C
		public Texture(Device device, Bitmap image, Usage usage, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			this.CreateTextureFromBitmap(device, image, usage, pool);
			this.CreateObjects(device, pool);
			this.m_bLockArray = null;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0006BF94 File Offset: 0x0006B394
		public unsafe Texture(Device device, int width, int height, int numLevels, Usage usage, Format format, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DTexture9* direct3DTexture9*& = ref this.m_lpUM;
			this.m_Usage = (uint)usage;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DTexture9**,System.Void**), device.m_lpUM, width, height, numLevels, usage, format, pool, ref direct3DTexture9*&, 0, *(*(int*)device.m_lpUM + 92));
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

		// Token: 0x06000726 RID: 1830 RVA: 0x00063B38 File Offset: 0x00062F38
		public static Texture FromBitmap(Device device, Bitmap image, Usage usage, Pool pool)
		{
			return new Texture(device, image, usage, pool);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00063B54 File Offset: 0x00062F54
		public static Texture FromStream(Device device, Stream data, Usage usage, Pool pool)
		{
			return new Texture(device, Image.FromStream(data), usage, pool);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0006B6A0 File Offset: 0x0006AAA0
		private unsafe void CreateTextureFromBitmap(Device device, Bitmap image, Usage usage, Pool pool)
		{
			ref IDirect3DTexture9* direct3DTexture9*& = ref this.m_lpUM;
			int width = image.Width;
			int height = image.Height;
			bool flag = true;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DTexture9**,System.Void**), device.m_lpUM, width, height, 1, usage, 21, pool, ref direct3DTexture9*&, 0, *(*(int*)device.m_lpUM + 92));
			if (num < 0)
			{
				flag = false;
				num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DTexture9**,System.Void**), device.m_lpUM, width, height, 1, usage, 26, pool, ref direct3DTexture9*&, 0, *(*(int*)device.m_lpUM + 92));
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
			_D3DLOCKED_RECT d3DLOCKED_RECT;
			num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_RECT*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, 0, ref d3DLOCKED_RECT, 0, 0, *(*(int*)this.m_lpUM + 76));
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
			byte* ptr = *(ref d3DLOCKED_RECT + 4);
			if (flag)
			{
				int num2 = 0;
				if (0 < height)
				{
					do
					{
						int num3 = 0;
						if (0 < width)
						{
							do
							{
								Color pixel = image.GetPixel(num3, num2);
								*(int*)(num3 * 4 + ptr) = pixel.ToArgb();
								num3++;
							}
							while (num3 < width);
						}
						ptr = d3DLOCKED_RECT + ptr;
						num2++;
					}
					while (num2 < height);
				}
			}
			else
			{
				Rectangle rect = default(Rectangle);
				rect = new Rectangle(0, 0, width, height);
				BitmapData bitmapData = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
				void* ptr2 = bitmapData.Scan0.ToPointer();
				cpblk(ptr, ptr2, height * width << 1);
				image.UnlockBits(bitmapData);
			}
			num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32), this.m_lpUM, 0, *(*(int*)this.m_lpUM + 80));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal3 = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex3 = exceptionFromResultInternal3 as DirectXException;
					if (ex3 != null)
					{
						ex3.ErrorCode = num;
						throw ex3;
					}
					throw exceptionFromResultInternal3;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0006B884 File Offset: 0x0006AC84
		public unsafe void AddDirtyRectangle()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, 0, *(*(int*)this.m_lpUM + 84));
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

		// Token: 0x0600072A RID: 1834 RVA: 0x0006B8DC File Offset: 0x0006ACDC
		public unsafe void AddDirtyRectangle(Rectangle rect)
		{
			rect.Width += rect.X;
			rect.Height += rect.Y;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ref rect, *(*(int*)this.m_lpUM + 84));
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

		// Token: 0x0600072B RID: 1835 RVA: 0x0006B960 File Offset: 0x0006AD60
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

		// Token: 0x0600072C RID: 1836 RVA: 0x0006BEB4 File Offset: 0x0006B2B4
		public unsafe GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags, out int pitch)
		{
			return this.LockRectangleInternal(level, (tagRECT*)(&rect), flags, out pitch);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0006BE98 File Offset: 0x0006B298
		public GraphicsStream LockRectangle(int level, LockFlags flags, out int pitch)
		{
			return this.LockRectangleInternal(level, null, flags, out pitch);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0006BE78 File Offset: 0x0006B278
		public unsafe GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags)
		{
			return this.LockRectangleInternal(level, (tagRECT*)(&rect), flags, 0);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0006BE5C File Offset: 0x0006B25C
		public GraphicsStream LockRectangle(int level, LockFlags flags)
		{
			return this.LockRectangleInternal(level, null, flags, 0);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0006BF38 File Offset: 0x0006B338
		public unsafe Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, level, (tagRECT*)(&rect), flags, out pitch, ranks);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0006BF18 File Offset: 0x0006B318
		public Array LockRectangle(Type typeLock, int level, LockFlags flags, out int pitch, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, level, null, flags, out pitch, ranks);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0006BEF4 File Offset: 0x0006B2F4
		public unsafe Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, level, (tagRECT*)(&rect), flags, 0, ranks);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0006BED4 File Offset: 0x0006B2D4
		public Array LockRectangle(Type typeLock, int level, LockFlags flags, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, level, null, flags, 0, ranks);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0006BA38 File Offset: 0x0006AE38
		public unsafe void UnlockRectangle(int level)
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

		// Token: 0x06000735 RID: 1845 RVA: 0x0006BAFC File Offset: 0x0006AEFC
		public unsafe Surface GetSurfaceLevel(int level)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**), this.m_lpUM, level, ref ptr, *(*(int*)this.m_lpUM + 72));
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

		// Token: 0x06000736 RID: 1846 RVA: 0x0006B9D4 File Offset: 0x0006ADD4
		private unsafe void LockRectangleInternal(int level, tagRECT* rect, LockFlags flags)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_RECT*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, level, ref this.m_lockRectangle, rect, flags, *(*(int*)this.m_lpUM + 76));
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

		// Token: 0x06000737 RID: 1847 RVA: 0x0006BD24 File Offset: 0x0006B124
		private unsafe GraphicsStream LockRectangleInternal(int level, tagRECT* rect, LockFlags flags, out int pitch)
		{
			if (rect != null)
			{
				*(int*)(rect + 8 / sizeof(tagRECT)) = *(int*)(rect + 8 / sizeof(tagRECT)) + *(int*)rect;
				*(int*)(rect + 12 / sizeof(tagRECT)) = *(int*)(rect + 12 / sizeof(tagRECT)) + *(int*)(rect + 4 / sizeof(tagRECT));
			}
			this.LockRectangleInternal(level, rect, flags);
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

		// Token: 0x06000738 RID: 1848 RVA: 0x0006BC94 File Offset: 0x0006B094
		private unsafe Array LockRectangleInternal(Type typeLock, int level, tagRECT* rect, LockFlags flags, out int pitch, int[] ranks)
		{
			Array array = null;
			if (rect != null)
			{
				*(int*)(rect + 8 / sizeof(tagRECT)) = *(int*)(rect + 8 / sizeof(tagRECT)) + *(int*)rect;
				*(int*)(rect + 12 / sizeof(tagRECT)) = *(int*)(rect + 12 / sizeof(tagRECT)) + *(int*)(rect + 4 / sizeof(tagRECT));
			}
			this.LockRectangleInternal(level, rect, flags);
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

		// Token: 0x06000739 RID: 1849 RVA: 0x0006BDD4 File Offset: 0x0006B1D4
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

		// Token: 0x0600073A RID: 1850 RVA: 0x0006BC6C File Offset: 0x0006B06C
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0006BDBC File Offset: 0x0006B1BC
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0006BB64 File Offset: 0x0006AF64
		private void OnParentLost(object sender, EventArgs e)
		{
			if (this.m_Pool == Pool.Default)
			{
				this.Dispose();
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0006BB84 File Offset: 0x0006AF84
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x00063B74 File Offset: 0x00062F74
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0006B668 File Offset: 0x0006AA68
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

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00063B8C File Offset: 0x00062F8C
		[CLSCompliant(false)]
		public new unsafe IDirect3DTexture9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00063BA4 File Offset: 0x00062FA4
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DTexture9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000742 RID: 1858 RVA: 0x00063BE4 File Offset: 0x00062FE4
		// (remove) Token: 0x06000743 RID: 1859 RVA: 0x00063C08 File Offset: 0x00063008
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

		// Token: 0x06000745 RID: 1861 RVA: 0x00063C54 File Offset: 0x00063054
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x0400107D RID: 4221
		private unsafe byte* m_bLockArray;

		// Token: 0x0400107E RID: 4222
		private Device pDevice;

		// Token: 0x0400107F RID: 4223
		private Pool m_Pool;

		// Token: 0x04001080 RID: 4224
		private uint m_Usage;

		// Token: 0x04001081 RID: 4225
		private Array m_managedArray;

		// Token: 0x04001082 RID: 4226
		private GraphicsStream stream;

		// Token: 0x04001083 RID: 4227
		private LockFlags m_lockFlags;

		// Token: 0x04001084 RID: 4228
		private _D3DLOCKED_RECT m_lockRectangle;

		// Token: 0x04001085 RID: 4229
		internal bool m_bDisposed;

		// Token: 0x04001087 RID: 4231
		internal new unsafe IDirect3DTexture9* m_lpUM;
	}
}
