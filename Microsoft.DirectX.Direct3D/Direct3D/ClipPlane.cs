using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000EF RID: 239
	[DefaultMember("Plane")]
	public sealed class ClipPlane
	{
		// Token: 0x060004C6 RID: 1222 RVA: 0x00072BB0 File Offset: 0x00071FB0
		internal ClipPlane(Device dev, ClipPlanes p, int i)
		{
			this.pDevice = dev;
			this.index = i;
			this.pParent = p;
			this.enableIndex = 1U << i;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00061F2C File Offset: 0x0006132C
		private ClipPlane()
		{
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00072EAC File Offset: 0x000722AC
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00072DBC File Offset: 0x000721BC
		[IndexerName("Plane")]
		public unsafe Plane Plane
		{
			get
			{
				Plane result = default(Plane);
				result = new Plane();
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Single*), this.pDevice.m_lpUM, this.index, ref result, *(*(int*)this.pDevice.m_lpUM + 224));
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
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Single modopt(Microsoft.VisualC.IsConstModifier)*), this.pDevice.m_lpUM, this.index, ref value, *(*(int*)this.pDevice.m_lpUM + 220));
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

		// Token: 0x060004CA RID: 1226 RVA: 0x00072E28 File Offset: 0x00072228
		public unsafe float[] GetSingleArray()
		{
			float[] array = new float[4];
			array.Initialize();
			ref float single& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Single*), this.pDevice.m_lpUM, this.index, ref single&, *(*(int*)this.pDevice.m_lpUM + 224));
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
			return array;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00072D18 File Offset: 0x00072118
		public unsafe void SetSingleArray(float[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("plane");
			}
			if (value.Length == 0)
			{
				throw new ArgumentNullException("plane");
			}
			if (value.Length != 4)
			{
				throw new ArgumentException(string.Empty, "plane");
			}
			ref float single& = ref value[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Single modopt(Microsoft.VisualC.IsConstModifier)*), this.pDevice.m_lpUM, this.index, ref single&, *(*(int*)this.pDevice.m_lpUM + 220));
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

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00072BE8 File Offset: 0x00071FE8
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00072C64 File Offset: 0x00072064
		public unsafe bool Enabled
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				uint num2;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.pDevice.m_lpUM, 152, ref num2, *(*(int*)this.pDevice.m_lpUM + 232));
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
				return (this.enableIndex & num2) == this.enableIndex;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (value)
				{
					this.pParent.dwCurrentClipPlanes = (this.pParent.dwCurrentClipPlanes | this.enableIndex);
				}
				else
				{
					this.pParent.dwCurrentClipPlanes = (this.pParent.dwCurrentClipPlanes & ~this.enableIndex);
				}
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.pDevice.m_lpUM, 152, this.pParent.dwCurrentClipPlanes, *(*(int*)this.pDevice.m_lpUM + 228));
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

		// Token: 0x04001014 RID: 4116
		private Device pDevice;

		// Token: 0x04001015 RID: 4117
		private ClipPlanes pParent;

		// Token: 0x04001016 RID: 4118
		private int index;

		// Token: 0x04001017 RID: 4119
		internal uint enableIndex;
	}
}
