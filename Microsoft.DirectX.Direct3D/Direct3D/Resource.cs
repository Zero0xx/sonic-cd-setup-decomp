using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F5 RID: 245
	public abstract class Resource : MarshalByRefObject
	{
		// Token: 0x0600052D RID: 1325 RVA: 0x00069EEC File Offset: 0x000692EC
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(Resource) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00069F28 File Offset: 0x00069328
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(Resource left, Resource right)
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

		// Token: 0x0600052F RID: 1327 RVA: 0x0006A2B4 File Offset: 0x000696B4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(Resource left, Resource right)
		{
			return !(left == right);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00069F58 File Offset: 0x00069358
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00069FCC File Offset: 0x000693CC
		public unsafe int SetPriority(int priorityNew)
		{
			return calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, priorityNew, *(*(int*)this.m_lpUM + 28));
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0006A024 File Offset: 0x00069424
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00069FF8 File Offset: 0x000693F8
		public unsafe int Priority
		{
			get
			{
				return calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 32));
			}
			set
			{
				object obj = calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, value, *(*(int*)this.m_lpUM + 28));
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0006A04C File Offset: 0x0006944C
		public unsafe void PreLoad()
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 36));
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0006A074 File Offset: 0x00069474
		public unsafe ResourceType Type
		{
			get
			{
				return calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 40));
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0006A09C File Offset: 0x0006949C
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

		// Token: 0x06000537 RID: 1335 RVA: 0x0006A124 File Offset: 0x00069524
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

		// Token: 0x06000538 RID: 1336 RVA: 0x0006A1B8 File Offset: 0x000695B8
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

		// Token: 0x06000539 RID: 1337 RVA: 0x0006A25C File Offset: 0x0006965C
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

		// Token: 0x0600053A RID: 1338
		public abstract void Dispose();

		// Token: 0x0600053B RID: 1339 RVA: 0x00069FB0 File Offset: 0x000693B0
		private protected unsafe virtual void SetObject(IDirect3DResource9* lp)
		{
			this.m_lpUM = lp;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00062168 File Offset: 0x00061568
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DResource9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00069F78 File Offset: 0x00069378
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

		// Token: 0x0600053E RID: 1342 RVA: 0x000621E8 File Offset: 0x000615E8
		[CLSCompliant(false)]
		public unsafe Resource(IDirect3DResource9* pUnk)
		{
			this.m_lpUM = pUnk;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000621A8 File Offset: 0x000615A8
		public unsafe Resource(IntPtr unmanagedObject)
		{
			IDirect3DResource9* lpUM = (IDirect3DResource9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x000621D0 File Offset: 0x000615D0
		[CLSCompliant(false)]
		public unsafe IDirect3DResource9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x04001022 RID: 4130
		internal Device pCachedResourcepDevice;

		// Token: 0x04001023 RID: 4131
		internal unsafe IDirect3DResource9* m_lpUM;
	}
}
