using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F0 RID: 240
	public sealed class ClipPlanes
	{
		// Token: 0x1700025B RID: 603
		public ClipPlane this[int index]
		{
			get
			{
				if (index > 31)
				{
					throw new IndexOutOfRangeException("this");
				}
				if (this.ourClipPlanes != null && this.ourClipPlanes.Length != 0)
				{
					if (this.ourClipPlanes.Length > index)
					{
						if (this.ourClipPlanes[index] == null)
						{
							this.ourClipPlanes[index] = new ClipPlane(this.pDevice, this, index);
						}
					}
					else
					{
						ClipPlane[] destinationArray = new ClipPlane[index + 1];
						Array.Copy(this.ourClipPlanes, 0, destinationArray, 0, this.ourClipPlanes.Length);
						this.ourClipPlanes = destinationArray;
						this.ourClipPlanes[index] = new ClipPlane(this.pDevice, this, index);
					}
				}
				else
				{
					this.ourClipPlanes = new ClipPlane[index + 1];
					this.ourClipPlanes[index] = new ClipPlane(this.pDevice, this, index);
				}
				return this.ourClipPlanes[index];
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00072B04 File Offset: 0x00071F04
		public unsafe void EnableAll()
		{
			uint num = 0U;
			if (this.ourClipPlanes != null)
			{
				int num2 = 0;
				if (0 < this.ourClipPlanes.Length)
				{
					do
					{
						if (this.ourClipPlanes[num2] != null)
						{
							num |= this.ourClipPlanes[num2].enableIndex;
						}
						num2++;
					}
					while (num2 < this.ourClipPlanes.Length);
				}
			}
			int num3 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.pDevice.m_lpUM, 152, num, *(*(int*)this.pDevice.m_lpUM + 228));
			if (num3 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num3);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num3;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num3);
				}
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00072A98 File Offset: 0x00071E98
		public unsafe void DisableAll()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.pDevice.m_lpUM, 152, 0, *(*(int*)this.pDevice.m_lpUM + 228));
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

		// Token: 0x060004D1 RID: 1233 RVA: 0x00061F44 File Offset: 0x00061344
		internal ClipPlanes(Device dev)
		{
			this.pDevice = dev;
		}

		// Token: 0x04001018 RID: 4120
		private Device pDevice;

		// Token: 0x04001019 RID: 4121
		private ClipPlane[] ourClipPlanes;

		// Token: 0x0400101A RID: 4122
		internal uint dwCurrentClipPlanes;
	}
}
