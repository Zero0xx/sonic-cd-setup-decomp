using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000109 RID: 265
	public sealed class Surface : Resource, IDisposable
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x0006A754 File Offset: 0x00069B54
		public unsafe override void Dispose()
		{
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				if (this.internalDC != null || this.graphicsObject != null)
				{
					this.ReleaseGraphics();
				}
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
				GC.SuppressFinalize(this);
				if (this.pParentDevice != null && Device.IsUsingEventHandlers)
				{
					this.pParentDevice.DeviceLost -= this.OnParentLost;
					this.pParentDevice.DeviceReset -= this.OnParentReset;
					this.pParentDevice.Disposing -= this.OnParentDisposed;
				}
				if (this.pParentTexture != null && Device.IsUsingEventHandlers)
				{
					this.pParentTexture.Disposing -= this.OnParentDisposed;
				}
				if (this.pParentCubeTexture != null && Device.IsUsingEventHandlers)
				{
					this.pParentCubeTexture.Disposing -= this.OnParentDisposed;
				}
				if (this.pParentChain != null && Device.IsUsingEventHandlers)
				{
					this.pParentChain.Disposing -= this.OnParentDisposed;
				}
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00063650 File Offset: 0x00062A50
		[CLSCompliant(false)]
		public unsafe Surface(IDirect3DSurface9* pInterop) : base(null)
		{
			this.Disposing = null;
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DResource9*)pInterop);
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00063600 File Offset: 0x00062A00
		public unsafe Surface(IntPtr unmanagedObject) : base(null)
		{
			this.Disposing = null;
			IDirect3DSurface9* ptr = (IDirect3DSurface9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DResource9*)ptr);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0006AC64 File Offset: 0x0006A064
		[CLSCompliant(false)]
		public unsafe Surface(IDirect3DSurface9* lp, object device) : base((IDirect3DResource9*)lp)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.m_bLockArray = null;
			this.CreateObjects(device);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0006ACE8 File Offset: 0x0006A0E8
		public unsafe Surface(Device device, Stream data, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			this.CreateSurfaceFromBitmap(device, Image.FromStream(data), pool);
			this.CreateObjects(device);
			this.m_lpUM = (IDirect3DResource9*)this.m_lpUM;
			this.m_bLockArray = null;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0006AC9C File Offset: 0x0006A09C
		public unsafe Surface(Device device, Bitmap image, Pool pool) : base(null)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			this.CreateSurfaceFromBitmap(device, image, pool);
			this.m_lpUM = (IDirect3DResource9*)this.m_lpUM;
			this.CreateObjects(device);
			this.m_bLockArray = null;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000635AC File Offset: 0x000629AC
		public static Surface FromBitmap(Device device, Bitmap image, Pool pool)
		{
			return new Surface(device, image, pool);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000635C8 File Offset: 0x000629C8
		public static Surface FromStream(Device device, Stream data, Pool pool)
		{
			return new Surface(device, Image.FromStream(data), pool);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0006A2D0 File Offset: 0x000696D0
		private unsafe void CreateSurfaceFromBitmap(Device device, Bitmap image, Pool pool)
		{
			ref IDirect3DSurface9* direct3DSurface9*& = ref this.m_lpUM;
			int width = image.Width;
			int height = image.Height;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**,System.Void**), device.m_lpUM, width, height, 21, pool, ref direct3DSurface9*&, 0, *(*(int*)device.m_lpUM + 144));
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
			_D3DLOCKED_RECT d3DLOCKED_RECT;
			num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_RECT*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, ref d3DLOCKED_RECT, 0, 0, *(*(int*)this.m_lpUM + 52));
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
			num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 56));
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

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0006A468 File Offset: 0x00069868
		public unsafe SurfaceDescription Description
		{
			get
			{
				SurfaceDescription result = default(SurfaceDescription);
				result = new SurfaceDescription();
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DSURFACE_DESC*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 48));
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

		// Token: 0x060006D1 RID: 1745 RVA: 0x0006A4D0 File Offset: 0x000698D0
		public unsafe Graphics GetGraphics()
		{
			if (this.graphicsObject == null)
			{
				ref HDC__* hdc__*& = ref this.internalDC;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.HDC__**), this.m_lpUM, ref hdc__*&, *(*(int*)this.m_lpUM + 60));
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
				IntPtr hdc = 0;
				hdc = new IntPtr((void*)this.internalDC);
				this.graphicsObject = Graphics.FromHdc(hdc);
			}
			return this.graphicsObject;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0006ABC8 File Offset: 0x00069FC8
		public unsafe GraphicsStream LockRectangle(Rectangle rect, LockFlags flags, out int pitch)
		{
			return this.LockRectangleInternal((tagRECT*)(&rect), flags, out pitch);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0006ABAC File Offset: 0x00069FAC
		public GraphicsStream LockRectangle(LockFlags flags, out int pitch)
		{
			return this.LockRectangleInternal(null, flags, out pitch);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0006AB90 File Offset: 0x00069F90
		public unsafe GraphicsStream LockRectangle(Rectangle rect, LockFlags flags)
		{
			return this.LockRectangleInternal((tagRECT*)(&rect), flags, 0);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0006AB74 File Offset: 0x00069F74
		public GraphicsStream LockRectangle(LockFlags flags)
		{
			return this.LockRectangleInternal(null, flags, 0);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0006AC44 File Offset: 0x0006A044
		public unsafe Array LockRectangle(Type typeLock, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, (tagRECT*)(&rect), flags, out pitch, ranks);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0006AC24 File Offset: 0x0006A024
		public Array LockRectangle(Type typeLock, LockFlags flags, out int pitch, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, null, flags, out pitch, ranks);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0006AC04 File Offset: 0x0006A004
		public unsafe Array LockRectangle(Type typeLock, Rectangle rect, LockFlags flags, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, (tagRECT*)(&rect), flags, 0, ranks);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0006ABE4 File Offset: 0x00069FE4
		public Array LockRectangle(Type typeLock, LockFlags flags, params int[] ranks)
		{
			return this.LockRectangleInternal(typeLock, null, flags, 0, ranks);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0006A650 File Offset: 0x00069A50
		public unsafe void UnlockRectangle()
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
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 56));
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

		// Token: 0x060006DB RID: 1755 RVA: 0x0006A568 File Offset: 0x00069968
		public unsafe void ReleaseGraphics()
		{
			if (this.graphicsObject != null)
			{
				this.graphicsObject.Dispose();
				this.graphicsObject = null;
			}
			if (this.internalDC != null)
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.HDC__*), this.m_lpUM, this.internalDC, *(*(int*)this.m_lpUM + 64));
				this.internalDC = null;
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
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0006AD38 File Offset: 0x0006A138
		public unsafe object GetContainer(Guid interfaceId)
		{
			void* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void**), this.m_lpUM, ref interfaceId, ref ptr, *(*(int*)this.m_lpUM + 44));
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
				return new Texture((IDirect3DTexture9*)ptr, base.Device, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DCubeTexture9))
			{
				return new CubeTexture((IDirect3DCubeTexture9*)ptr, base.Device, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVolumeTexture9))
			{
				return new VolumeTexture((IDirect3DVolumeTexture9*)ptr, base.Device, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVertexBuffer9))
			{
				return new VertexBuffer((IDirect3DVertexBuffer9*)ptr, base.Device, Usage.None, VertexFormats.Texture0, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DIndexBuffer9))
			{
				return new IndexBuffer((IDirect3DIndexBuffer9*)ptr, base.Device, Usage.None, Pool.Managed);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DSurface9))
			{
				return new Surface((IDirect3DSurface9*)ptr, base.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVolume9))
			{
				return new Volume((IDirect3DVolume9*)ptr, null);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DSwapChain9))
			{
				return new SwapChain((IDirect3DSwapChain9*)ptr, base.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVertexDeclaration9))
			{
				return new VertexDeclaration((IDirect3DVertexDeclaration9*)ptr, base.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DVertexShader9))
			{
				return new VertexShader((IDirect3DVertexShader9*)ptr, base.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DPixelShader9))
			{
				return new PixelShader((IDirect3DPixelShader9*)ptr, base.Device);
			}
			if (interfaceId.Equals(<Module>.IID_IDirect3DStateBlock9))
			{
				return new StateBlock((IDirect3DStateBlock9*)ptr, base.Device);
			}
			return null;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0006A5F0 File Offset: 0x000699F0
		internal unsafe void LockRectangleInternal(tagRECT* rect, LockFlags flags)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_RECT*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, ref this.m_lockRectangle, rect, flags, *(*(int*)this.m_lpUM + 52));
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

		// Token: 0x060006DE RID: 1758 RVA: 0x0006A920 File Offset: 0x00069D20
		internal unsafe GraphicsStream LockRectangleInternal(tagRECT* rect, LockFlags flags, out int pitch)
		{
			if (rect != null)
			{
				*(int*)(rect + 8 / sizeof(tagRECT)) = *(int*)(rect + 8 / sizeof(tagRECT)) + *(int*)rect;
				*(int*)(rect + 12 / sizeof(tagRECT)) = *(int*)(rect + 12 / sizeof(tagRECT)) + *(int*)(rect + 4 / sizeof(tagRECT));
			}
			this.LockRectangleInternal(rect, flags);
			this.m_bLockArray = *(ref this.m_lockRectangle + 4);
			if (ref pitch != null)
			{
				pitch = this.m_lockRectangle;
			}
			IntPtr dataPointer = 0;
			dataPointer = new IntPtr((void*)this.m_bLockArray);
			this.stream = new GraphicsStream(dataPointer, 2147483647L, true, (byte)(~(flags >> 4) & (LockFlags)1) != 0);
			return this.stream;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0006A894 File Offset: 0x00069C94
		internal unsafe Array LockRectangleInternal(Type typeLock, tagRECT* rect, LockFlags flags, out int pitch, int[] ranks)
		{
			Array array = null;
			if (rect != null)
			{
				*(int*)(rect + 8 / sizeof(tagRECT)) = *(int*)(rect + 8 / sizeof(tagRECT)) + *(int*)rect;
				*(int*)(rect + 12 / sizeof(tagRECT)) = *(int*)(rect + 12 / sizeof(tagRECT)) + *(int*)(rect + 4 / sizeof(tagRECT));
			}
			this.LockRectangleInternal(rect, flags);
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

		// Token: 0x060006E0 RID: 1760 RVA: 0x0006A9C0 File Offset: 0x00069DC0
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0006A9E8 File Offset: 0x00069DE8
		private void CreateObjects(object device)
		{
			this.internalDC = null;
			this.graphicsObject = null;
			this.pParentTexture = null;
			this.pParentCubeTexture = null;
			this.pParentDevice = null;
			this.pParentChain = null;
			this.m_bDisposed = false;
			if (device != null)
			{
				if (device.GetType() == typeof(Device))
				{
					this.pParentDevice = device;
					if (this.pParentDevice != null && Device.IsUsingEventHandlers)
					{
						this.pParentDevice.DeviceLost += this.OnParentLost;
						this.pParentDevice.DeviceReset += this.OnParentReset;
						this.pParentDevice.Disposing += this.OnParentDisposed;
					}
				}
				else if (device.GetType() == typeof(CubeTexture))
				{
					this.pParentCubeTexture = device;
					if (this.pParentCubeTexture != null && Device.IsUsingEventHandlers)
					{
						this.pParentCubeTexture.Disposing += this.OnParentDisposed;
					}
				}
				else if (device.GetType() == typeof(SwapChain))
				{
					this.pParentChain = device;
					if (this.pParentChain != null && Device.IsUsingEventHandlers)
					{
						this.pParentChain.Disposing += this.OnParentDisposed;
					}
				}
				else if (device.GetType() == typeof(Texture))
				{
					this.pParentTexture = device;
					if (this.pParentTexture != null && Device.IsUsingEventHandlers)
					{
						this.pParentTexture.Disposing += this.OnParentDisposed;
					}
				}
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0006A728 File Offset: 0x00069B28
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0006A9A8 File Offset: 0x00069DA8
		private void OnParentLost(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0006A740 File Offset: 0x00069B40
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000635E8 File Offset: 0x000629E8
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0006A430 File Offset: 0x00069830
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

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00063638 File Offset: 0x00062A38
		[CLSCompliant(false)]
		public new unsafe IDirect3DSurface9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00063684 File Offset: 0x00062A84
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DSurface9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060006E9 RID: 1769 RVA: 0x000636C4 File Offset: 0x00062AC4
		// (remove) Token: 0x060006EA RID: 1770 RVA: 0x000636E8 File Offset: 0x00062AE8
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

		// Token: 0x060006EC RID: 1772 RVA: 0x00063734 File Offset: 0x00062B34
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x0400106A RID: 4202
		private unsafe byte* m_bLockArray;

		// Token: 0x0400106B RID: 4203
		private Array m_managedArray;

		// Token: 0x0400106C RID: 4204
		private GraphicsStream stream;

		// Token: 0x0400106D RID: 4205
		private LockFlags m_lockFlags;

		// Token: 0x0400106E RID: 4206
		private _D3DLOCKED_RECT m_lockRectangle;

		// Token: 0x0400106F RID: 4207
		internal Texture pParentTexture;

		// Token: 0x04001070 RID: 4208
		internal CubeTexture pParentCubeTexture;

		// Token: 0x04001071 RID: 4209
		internal Device pParentDevice;

		// Token: 0x04001072 RID: 4210
		internal SwapChain pParentChain;

		// Token: 0x04001073 RID: 4211
		private unsafe HDC__* internalDC;

		// Token: 0x04001074 RID: 4212
		private Graphics graphicsObject;

		// Token: 0x04001075 RID: 4213
		internal bool m_bDisposed;

		// Token: 0x04001077 RID: 4215
		internal new unsafe IDirect3DSurface9* m_lpUM;
	}
}
