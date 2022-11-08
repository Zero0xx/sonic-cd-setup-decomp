using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x02000918 RID: 2328
	internal sealed class Win32
	{
		// Token: 0x0600545B RID: 21595 RVA: 0x0013090C File Offset: 0x0012F90C
		static Win32()
		{
			Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
			if (!Win32Native.GetVersionEx(osversioninfo))
			{
				throw new SystemException(Environment.GetResourceString("InvalidOperation_GetVersion"));
			}
			if (osversioninfo.PlatformId != 2 || osversioninfo.MajorVersion < 5)
			{
				Win32._LsaApisSupported = false;
				Win32._LsaLookupNames2Supported = false;
				Win32._ConvertStringSidToSidSupported = false;
				Win32._WellKnownSidApisSupported = false;
				return;
			}
			Win32._ConvertStringSidToSidSupported = true;
			Win32._LsaApisSupported = true;
			if (osversioninfo.MajorVersion > 5 || osversioninfo.MinorVersion > 0)
			{
				Win32._LsaLookupNames2Supported = true;
				Win32._WellKnownSidApisSupported = true;
				return;
			}
			Win32._LsaLookupNames2Supported = false;
			Win32Native.OSVERSIONINFOEX osversioninfoex = new Win32Native.OSVERSIONINFOEX();
			if (!Win32Native.GetVersionEx(osversioninfoex))
			{
				throw new SystemException(Environment.GetResourceString("InvalidOperation_GetVersion"));
			}
			if (osversioninfoex.ServicePackMajor < 3)
			{
				Win32._WellKnownSidApisSupported = false;
				return;
			}
			Win32._WellKnownSidApisSupported = true;
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x001309CB File Offset: 0x0012F9CB
		private Win32()
		{
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x0600545D RID: 21597 RVA: 0x001309D3 File Offset: 0x0012F9D3
		internal static bool SddlConversionSupported
		{
			get
			{
				return Win32._ConvertStringSidToSidSupported;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x0600545E RID: 21598 RVA: 0x001309DA File Offset: 0x0012F9DA
		internal static bool LsaApisSupported
		{
			get
			{
				return Win32._LsaApisSupported;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x0600545F RID: 21599 RVA: 0x001309E1 File Offset: 0x0012F9E1
		internal static bool LsaLookupNames2Supported
		{
			get
			{
				return Win32._LsaLookupNames2Supported;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005460 RID: 21600 RVA: 0x001309E8 File Offset: 0x0012F9E8
		internal static bool WellKnownSidApisSupported
		{
			get
			{
				return Win32._WellKnownSidApisSupported;
			}
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x001309F0 File Offset: 0x0012F9F0
		internal static SafeLsaPolicyHandle LsaOpenPolicy(string systemName, PolicyRights rights)
		{
			if (!Win32.LsaApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			Win32Native.LSA_OBJECT_ATTRIBUTES lsa_OBJECT_ATTRIBUTES;
			lsa_OBJECT_ATTRIBUTES.Length = Marshal.SizeOf(typeof(Win32Native.LSA_OBJECT_ATTRIBUTES));
			lsa_OBJECT_ATTRIBUTES.RootDirectory = IntPtr.Zero;
			lsa_OBJECT_ATTRIBUTES.ObjectName = IntPtr.Zero;
			lsa_OBJECT_ATTRIBUTES.Attributes = 0;
			lsa_OBJECT_ATTRIBUTES.SecurityDescriptor = IntPtr.Zero;
			lsa_OBJECT_ATTRIBUTES.SecurityQualityOfService = IntPtr.Zero;
			SafeLsaPolicyHandle result;
			uint num;
			if ((num = Win32Native.LsaOpenPolicy(systemName, ref lsa_OBJECT_ATTRIBUTES, (int)rights, out result)) == 0U)
			{
				return result;
			}
			if (num == 3221225506U)
			{
				throw new UnauthorizedAccessException();
			}
			if (num == 3221225626U || num == 3221225495U)
			{
				throw new OutOfMemoryException();
			}
			int errorCode = Win32Native.LsaNtStatusToWinError((int)num);
			throw new SystemException(Win32Native.GetMessage(errorCode));
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x00130AAC File Offset: 0x0012FAAC
		internal static byte[] ConvertIntPtrSidToByteArraySid(IntPtr binaryForm)
		{
			byte b = Marshal.ReadByte(binaryForm, 0);
			if (b != SecurityIdentifier.Revision)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_InvalidSidRevision"), "binaryForm");
			}
			byte b2 = Marshal.ReadByte(binaryForm, 1);
			if (b2 < 0 || b2 > SecurityIdentifier.MaxSubAuthorities)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_InvalidNumberOfSubauthorities", new object[]
				{
					SecurityIdentifier.MaxSubAuthorities
				}), "binaryForm");
			}
			int num = (int)(8 + b2 * 4);
			byte[] array = new byte[num];
			Marshal.Copy(binaryForm, array, 0, num);
			return array;
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x00130B38 File Offset: 0x0012FB38
		internal static int CreateSidFromString(string stringSid, out byte[] resultSid)
		{
			IntPtr zero = IntPtr.Zero;
			if (!Win32.SddlConversionSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			int lastWin32Error;
			try
			{
				if (1 != Win32Native.ConvertStringSidToSid(stringSid, out zero))
				{
					lastWin32Error = Marshal.GetLastWin32Error();
					goto IL_44;
				}
				resultSid = Win32.ConvertIntPtrSidToByteArraySid(zero);
			}
			finally
			{
				Win32Native.LocalFree(zero);
			}
			return 0;
			IL_44:
			resultSid = null;
			return lastWin32Error;
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x00130BA0 File Offset: 0x0012FBA0
		internal static int CreateWellKnownSid(WellKnownSidType sidType, SecurityIdentifier domainSid, out byte[] resultSid)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			uint maxBinaryLength = (uint)SecurityIdentifier.MaxBinaryLength;
			resultSid = new byte[maxBinaryLength];
			if (Win32Native.CreateWellKnownSid((int)sidType, (domainSid == null) ? null : domainSid.BinaryForm, resultSid, ref maxBinaryLength) != 0)
			{
				return 0;
			}
			resultSid = null;
			return Marshal.GetLastWin32Error();
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x00130BFC File Offset: 0x0012FBFC
		internal static bool IsEqualDomainSid(SecurityIdentifier sid1, SecurityIdentifier sid2)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			if (sid1 == null || sid2 == null)
			{
				return false;
			}
			byte[] array = new byte[sid1.BinaryLength];
			sid1.GetBinaryForm(array, 0);
			byte[] array2 = new byte[sid2.BinaryLength];
			sid2.GetBinaryForm(array2, 0);
			bool flag;
			return Win32Native.IsEqualDomainSid(array, array2, out flag) != 0 && flag;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x00130C6C File Offset: 0x0012FC6C
		internal static int GetWindowsAccountDomainSid(SecurityIdentifier sid, out SecurityIdentifier resultSid)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			byte[] array = new byte[sid.BinaryLength];
			sid.GetBinaryForm(array, 0);
			uint maxBinaryLength = (uint)SecurityIdentifier.MaxBinaryLength;
			byte[] array2 = new byte[maxBinaryLength];
			if (Win32Native.GetWindowsAccountDomainSid(array, array2, ref maxBinaryLength) != 0)
			{
				resultSid = new SecurityIdentifier(array2, 0);
				return 0;
			}
			resultSid = null;
			return Marshal.GetLastWin32Error();
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x00130CD0 File Offset: 0x0012FCD0
		internal static bool IsWellKnownSid(SecurityIdentifier sid, WellKnownSidType type)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			byte[] array = new byte[sid.BinaryLength];
			sid.GetBinaryForm(array, 0);
			return Win32Native.IsWellKnownSid(array, (int)type) != 0;
		}

		// Token: 0x06005468 RID: 21608
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ImpersonateLoggedOnUser(SafeTokenHandle hToken);

		// Token: 0x06005469 RID: 21609
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int OpenThreadToken(TokenAccessLevels dwDesiredAccess, WinSecurityContext OpenAs, out SafeTokenHandle phThreadToken);

		// Token: 0x0600546A RID: 21610
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RevertToSelf();

		// Token: 0x0600546B RID: 21611
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetThreadToken(SafeTokenHandle hToken);

		// Token: 0x04002BFB RID: 11259
		internal const int FALSE = 0;

		// Token: 0x04002BFC RID: 11260
		internal const int TRUE = 1;

		// Token: 0x04002BFD RID: 11261
		private static bool _LsaApisSupported;

		// Token: 0x04002BFE RID: 11262
		private static bool _LsaLookupNames2Supported;

		// Token: 0x04002BFF RID: 11263
		private static bool _ConvertStringSidToSidSupported;

		// Token: 0x04002C00 RID: 11264
		private static bool _WellKnownSidApisSupported;
	}
}
