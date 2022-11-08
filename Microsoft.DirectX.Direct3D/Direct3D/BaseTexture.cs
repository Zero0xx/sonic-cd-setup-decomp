using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F7 RID: 247
	public abstract class BaseTexture : Resource
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x00064668 File Offset: 0x00063A68
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(BaseTexture) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000646A4 File Offset: 0x00063AA4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(BaseTexture left, BaseTexture right)
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

		// Token: 0x06000543 RID: 1347 RVA: 0x00064AAC File Offset: 0x00063EAC
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(BaseTexture left, BaseTexture right)
		{
			return !(left == right);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000646D4 File Offset: 0x00063AD4
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00064754 File Offset: 0x00063B54
		public unsafe int SetLevelOfDetail(int lodNew)
		{
			return calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, lodNew, *(*(int*)this.m_lpUM + 44));
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x000647AC File Offset: 0x00063BAC
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00064780 File Offset: 0x00063B80
		public unsafe int LevelOfDetail
		{
			get
			{
				return calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 48));
			}
			set
			{
				object obj = calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, value, *(*(int*)this.m_lpUM + 44));
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000647D4 File Offset: 0x00063BD4
		public unsafe int LevelCount
		{
			get
			{
				return calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 52));
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000647FC File Offset: 0x00063BFC
		public unsafe void GenerateMipSubLevels()
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 64));
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00064824 File Offset: 0x00063C24
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x0006484C File Offset: 0x00063C4C
		public unsafe TextureFilter AutoGenerateFilterType
		{
			get
			{
				return calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 60));
			}
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32), this.m_lpUM, value, *(*(int*)this.m_lpUM + 56));
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

		// Token: 0x0600054C RID: 1356 RVA: 0x0006472C File Offset: 0x00063B2C
		private protected unsafe virtual void SetObject(IDirect3DBaseTexture9* lp)
		{
			this.m_lpUM = lp;
			base.SetObject((IDirect3DResource9*)this.m_lpUM);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000646F4 File Offset: 0x00063AF4
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

		// Token: 0x0600054E RID: 1358 RVA: 0x00062250 File Offset: 0x00061650
		[CLSCompliant(false)]
		public unsafe BaseTexture(IDirect3DBaseTexture9* pInterop) : base(null)
		{
			if (pInterop != null)
			{
				this.m_lpUM = pInterop;
				base.SetObject((IDirect3DResource9*)pInterop);
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00062208 File Offset: 0x00061608
		public unsafe BaseTexture(IntPtr unmanagedObject) : base(null)
		{
			IDirect3DBaseTexture9* ptr = (IDirect3DBaseTexture9*)unmanagedObject.ToPointer();
			this.m_lpUM = ptr;
			base.SetObject((IDirect3DResource9*)ptr);
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00062238 File Offset: 0x00061638
		[CLSCompliant(false)]
		public new unsafe IDirect3DBaseTexture9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0006227C File Offset: 0x0006167C
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DBaseTexture9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x04001024 RID: 4132
		private Pool m_Pool;

		// Token: 0x04001025 RID: 4133
		private Device m_Device;

		// Token: 0x04001026 RID: 4134
		internal new unsafe IDirect3DBaseTexture9* m_lpUM;
	}
}
