using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200051C RID: 1308
	internal abstract class SafeFreeCredentials : SafeHandle
	{
		// Token: 0x06002840 RID: 10304 RVA: 0x000A5BE7 File Offset: 0x000A4BE7
		protected SafeFreeCredentials() : base(IntPtr.Zero, true)
		{
			this._handle = default(SSPIHandle);
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x000A5C01 File Offset: 0x000A4C01
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this._handle.IsZero;
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000A5C18 File Offset: 0x000A4C18
		public static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			switch (dll)
			{
			case SecurDll.SECURITY:
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_8D;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
				}
				break;
			case SecurDll.SECUR32:
				break;
			default:
				goto IL_68;
			}
			outCredential = new SafeFreeCredential_SECUR32();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				goto IL_8D;
			}
			finally
			{
				long num2;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.AcquireCredentialsHandleA(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
			}
			IL_68:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
			{
				"SecurDll"
			}), "Dll");
			IL_8D:
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000A5CDC File Offset: 0x000A4CDC
		public static int AcquireDefaultCredential(SecurDll dll, string package, CredentialUse intent, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			switch (dll)
			{
			case SecurDll.SECURITY:
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_91;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, IntPtr.Zero, null, null, ref outCredential._handle, out num2);
				}
				break;
			case SecurDll.SECUR32:
				break;
			default:
				goto IL_6C;
			}
			outCredential = new SafeFreeCredential_SECUR32();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				goto IL_91;
			}
			finally
			{
				long num2;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.AcquireCredentialsHandleA(null, package, (int)intent, null, IntPtr.Zero, null, null, ref outCredential._handle, out num2);
			}
			IL_6C:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
			{
				"SecurDll"
			}), "Dll");
			IL_91:
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000A5DA4 File Offset: 0x000A4DA4
		public unsafe static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			IntPtr certContextArray = authdata.certContextArray;
			try
			{
				IntPtr certContextArray2 = new IntPtr((void*)(&certContextArray));
				if (certContextArray != IntPtr.Zero)
				{
					authdata.certContextArray = certContextArray2;
				}
				switch (dll)
				{
				case SecurDll.SECURITY:
					outCredential = new SafeFreeCredential_SECURITY();
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						goto IL_BB;
					}
					finally
					{
						long num2;
						num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
					}
					break;
				case SecurDll.SECUR32:
					goto IL_93;
				case SecurDll.SCHANNEL:
					break;
				default:
					goto IL_93;
				}
				outCredential = new SafeFreeCredential_SCHANNEL();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_BB;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.AcquireCredentialsHandleA(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
				}
				IL_93:
				throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
				{
					"SecurDll"
				}), "Dll");
				IL_BB:;
			}
			finally
			{
				authdata.certContextArray = certContextArray;
			}
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x04002772 RID: 10098
		internal SSPIHandle _handle;
	}
}
