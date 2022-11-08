using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BF RID: 2239
	internal static class X509Utils
	{
		// Token: 0x0600515F RID: 20831 RVA: 0x00123D95 File Offset: 0x00122D95
		internal static int OidToAlgId(string oid)
		{
			return X509Utils.OidToAlgId(oid, OidGroup.AllGroups);
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x00123DA0 File Offset: 0x00122DA0
		internal static int OidToAlgId(string oid, OidGroup group)
		{
			if (oid == null)
			{
				return 32772;
			}
			string text = CryptoConfig.MapNameToOID(oid, group);
			if (text == null)
			{
				text = oid;
			}
			return X509Utils.OidToAlgIdStrict(text, group);
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x00123DCC File Offset: 0x00122DCC
		internal static int OidToAlgIdStrict(string oid, OidGroup group)
		{
			int num;
			if (string.Equals(oid, "2.16.840.1.101.3.4.2.1", StringComparison.Ordinal))
			{
				num = 32780;
			}
			else if (string.Equals(oid, "2.16.840.1.101.3.4.2.2", StringComparison.Ordinal))
			{
				num = 32781;
			}
			else if (string.Equals(oid, "2.16.840.1.101.3.4.2.3", StringComparison.Ordinal))
			{
				num = 32782;
			}
			else
			{
				num = X509Utils._GetAlgIdFromOid(oid, group);
			}
			if (num == 0 || num == -1)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidOID"));
			}
			return num;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x00123E40 File Offset: 0x00122E40
		internal static X509ContentType MapContentType(uint contentType)
		{
			switch (contentType)
			{
			case 1U:
				return X509ContentType.Cert;
			case 4U:
				return X509ContentType.SerializedStore;
			case 5U:
				return X509ContentType.SerializedCert;
			case 8U:
			case 9U:
				return X509ContentType.Pkcs7;
			case 10U:
				return X509ContentType.Authenticode;
			case 12U:
				return X509ContentType.Pfx;
			}
			return X509ContentType.Unknown;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x00123E98 File Offset: 0x00122E98
		internal static uint MapKeyStorageFlags(X509KeyStorageFlags keyStorageFlags)
		{
			uint num = 0U;
			if ((keyStorageFlags & X509KeyStorageFlags.UserKeySet) == X509KeyStorageFlags.UserKeySet)
			{
				num |= 4096U;
			}
			else if ((keyStorageFlags & X509KeyStorageFlags.MachineKeySet) == X509KeyStorageFlags.MachineKeySet)
			{
				num |= 32U;
			}
			if ((keyStorageFlags & X509KeyStorageFlags.Exportable) == X509KeyStorageFlags.Exportable)
			{
				num |= 1U;
			}
			if ((keyStorageFlags & X509KeyStorageFlags.UserProtected) == X509KeyStorageFlags.UserProtected)
			{
				num |= 2U;
			}
			return num;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x00123ED8 File Offset: 0x00122ED8
		internal static SafeCertStoreHandle ExportCertToMemoryStore(X509Certificate certificate)
		{
			SafeCertStoreHandle invalidHandle = SafeCertStoreHandle.InvalidHandle;
			X509Utils._OpenX509Store(2U, 8704U, null, ref invalidHandle);
			X509Utils._AddCertificateToStore(invalidHandle, certificate.CertContext);
			return invalidHandle;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x00123F08 File Offset: 0x00122F08
		internal static IntPtr PasswordToCoTaskMemUni(object password)
		{
			if (password != null)
			{
				string text = password as string;
				if (text != null)
				{
					return Marshal.StringToCoTaskMemUni(text);
				}
				SecureString secureString = password as SecureString;
				if (secureString != null)
				{
					return Marshal.SecureStringToCoTaskMemUnicode(secureString);
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06005166 RID: 20838
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _AddCertificateToStore(SafeCertStoreHandle safeCertStoreHandle, SafeCertContextHandle safeCertContext);

		// Token: 0x06005167 RID: 20839
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _DuplicateCertContext(IntPtr handle, ref SafeCertContextHandle safeCertContext);

		// Token: 0x06005168 RID: 20840
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _ExportCertificatesToBlob(SafeCertStoreHandle safeCertStoreHandle, X509ContentType contentType, IntPtr password);

		// Token: 0x06005169 RID: 20841
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _GetAlgIdFromOid(string oid, OidGroup group);

		// Token: 0x0600516A RID: 20842
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetCertRawData(SafeCertContextHandle safeCertContext);

		// Token: 0x0600516B RID: 20843
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetDateNotAfter(SafeCertContextHandle safeCertContext, ref Win32Native.FILE_TIME fileTime);

		// Token: 0x0600516C RID: 20844
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetDateNotBefore(SafeCertContextHandle safeCertContext, ref Win32Native.FILE_TIME fileTime);

		// Token: 0x0600516D RID: 20845
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetFriendlyNameFromOid(string oid);

		// Token: 0x0600516E RID: 20846
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetIssuerName(SafeCertContextHandle safeCertContext, bool legacyV1Mode);

		// Token: 0x0600516F RID: 20847
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetOidFromFriendlyName(string oid, OidGroup group);

		// Token: 0x06005170 RID: 20848
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetPublicKeyOid(SafeCertContextHandle safeCertContext);

		// Token: 0x06005171 RID: 20849
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetPublicKeyParameters(SafeCertContextHandle safeCertContext);

		// Token: 0x06005172 RID: 20850
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetPublicKeyValue(SafeCertContextHandle safeCertContext);

		// Token: 0x06005173 RID: 20851
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetSubjectInfo(SafeCertContextHandle safeCertContext, uint displayType, bool legacyV1Mode);

		// Token: 0x06005174 RID: 20852
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetSerialNumber(SafeCertContextHandle safeCertContext);

		// Token: 0x06005175 RID: 20853
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetThumbprint(SafeCertContextHandle safeCertContext);

		// Token: 0x06005176 RID: 20854
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _LoadCertFromBlob(byte[] rawData, IntPtr password, uint dwFlags, bool persistKeySet, ref SafeCertContextHandle pCertCtx);

		// Token: 0x06005177 RID: 20855
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _LoadCertFromFile(string fileName, IntPtr password, uint dwFlags, bool persistKeySet, ref SafeCertContextHandle pCertCtx);

		// Token: 0x06005178 RID: 20856
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _OpenX509Store(uint storeType, uint flags, string storeName, ref SafeCertStoreHandle safeCertStoreHandle);

		// Token: 0x06005179 RID: 20857
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint _QueryCertBlobType(byte[] rawData);

		// Token: 0x0600517A RID: 20858
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint _QueryCertFileType(string fileName);
	}
}
