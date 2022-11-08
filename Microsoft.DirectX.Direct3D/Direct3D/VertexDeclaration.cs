using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200011A RID: 282
	public sealed class VertexDeclaration : MarshalByRefObject, IDisposable
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x0006E4B8 File Offset: 0x0006D8B8
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(VertexDeclaration) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0006E4F4 File Offset: 0x0006D8F4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(VertexDeclaration left, VertexDeclaration right)
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

		// Token: 0x060007C8 RID: 1992 RVA: 0x0006E6C8 File Offset: 0x0006DAC8
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(VertexDeclaration left, VertexDeclaration right)
		{
			return !(left == right);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0006E524 File Offset: 0x0006D924
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000642C0 File Offset: 0x000636C0
		[CLSCompliant(false)]
		public unsafe VertexDeclaration(IDirect3DVertexDeclaration9* pUnk)
		{
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00064278 File Offset: 0x00063678
		public unsafe VertexDeclaration(IntPtr unmanagedObject)
		{
			this.Disposing = null;
			IDirect3DVertexDeclaration9* lpUM = (IDirect3DVertexDeclaration9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0006E850 File Offset: 0x0006DC50
		[CLSCompliant(false)]
		public unsafe VertexDeclaration(IDirect3DVertexDeclaration9* lp, Device device)
		{
			this.Disposing = null;
			this.m_lpUM = lp;
			this.CreateObjects(device);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0006E910 File Offset: 0x0006DD10
		public unsafe VertexDeclaration(Device device, VertexElement[] vertexElements)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DVertexDeclaration9* direct3DVertexDeclaration9*& = ref this.m_lpUM;
			ref void void& = ref vertexElements[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVERTEXELEMENT9 modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexDeclaration9**), device.m_lpUM, ref void&, ref direct3DVertexDeclaration9*&, *(*(int*)device.m_lpUM + 344));
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

		// Token: 0x060007CE RID: 1998 RVA: 0x0006E880 File Offset: 0x0006DC80
		public unsafe VertexDeclaration(Device device, GraphicsStream vertexElements)
		{
			this.Disposing = null;
			this.m_lpUM = null;
			ref IDirect3DVertexDeclaration9* direct3DVertexDeclaration9*& = ref this.m_lpUM;
			int num;
			if (vertexElements != null)
			{
				num = vertexElements.InternalDataPointer;
			}
			else
			{
				num = 0;
			}
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVERTEXELEMENT9 modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexDeclaration9**), device.m_lpUM, num, ref direct3DVertexDeclaration9*&, *(*(int*)device.m_lpUM + 344));
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

		// Token: 0x060007CF RID: 1999 RVA: 0x0006E6E4 File Offset: 0x0006DAE4
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

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0006E57C File Offset: 0x0006D97C
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

		// Token: 0x060007D1 RID: 2001 RVA: 0x0006E604 File Offset: 0x0006DA04
		public unsafe VertexElement[] GetDeclaration()
		{
			VertexElement[] array = null;
			uint num = 0U;
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVERTEXELEMENT9*,System.UInt32*), this.m_lpUM, 0, ref num, *(*(int*)this.m_lpUM + 16));
			if (num > 0U)
			{
				array = new VertexElement[num];
				ref VertexElement vertexElement& = ref array[0];
				num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVERTEXELEMENT9*,System.UInt32*), this.m_lpUM, ref vertexElement&, ref num, *(*(int*)this.m_lpUM + 16));
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
			return array;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0006E7B4 File Offset: 0x0006DBB4
		protected override void Finalize()
		{
			this.Dispose();
			base.Finalize();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0006E7DC File Offset: 0x0006DBDC
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

		// Token: 0x060007D4 RID: 2004 RVA: 0x0006E79C File Offset: 0x0006DB9C
		private void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0006E6A0 File Offset: 0x0006DAA0
		private void OnParentLost(object sender, EventArgs e)
		{
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0006E6B4 File Offset: 0x0006DAB4
		private void OnParentReset(object sender, EventArgs e)
		{
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00064260 File Offset: 0x00063660
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0006E544 File Offset: 0x0006D944
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

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000642A8 File Offset: 0x000636A8
		[CLSCompliant(false)]
		public unsafe IDirect3DVertexDeclaration9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000642E8 File Offset: 0x000636E8
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DVertexDeclaration9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060007DB RID: 2011 RVA: 0x00064328 File Offset: 0x00063728
		// (remove) Token: 0x060007DC RID: 2012 RVA: 0x0006434C File Offset: 0x0006374C
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

		// Token: 0x060007DE RID: 2014 RVA: 0x00064398 File Offset: 0x00063798
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040010B1 RID: 4273
		private Device parentDevice;

		// Token: 0x040010B2 RID: 4274
		internal Device pCachedResourcepDevice;

		// Token: 0x040010B3 RID: 4275
		internal bool m_bDisposed;

		// Token: 0x040010B5 RID: 4277
		internal unsafe IDirect3DVertexDeclaration9* m_lpUM;
	}
}
