using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033A RID: 826
	public sealed class X509KeyUsageExtension : X509Extension
	{
		// Token: 0x06001A06 RID: 6662 RVA: 0x0005A87C File Offset: 0x0005987C
		public X509KeyUsageExtension() : base("2.5.29.15")
		{
			this.m_decoded = true;
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0005A890 File Offset: 0x00059890
		public X509KeyUsageExtension(X509KeyUsageFlags keyUsages, bool critical) : base("2.5.29.15", X509KeyUsageExtension.EncodeExtension(keyUsages), critical)
		{
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0005A8A4 File Offset: 0x000598A4
		public X509KeyUsageExtension(AsnEncodedData encodedKeyUsage, bool critical) : base("2.5.29.15", encodedKeyUsage.RawData, critical)
		{
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0005A8B8 File Offset: 0x000598B8
		public X509KeyUsageFlags KeyUsages
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return (X509KeyUsageFlags)this.m_keyUsages;
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0005A8CE File Offset: 0x000598CE
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0005A8E0 File Offset: 0x000598E0
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(14L), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			if (cryptoapi_BLOB.cbData > 4U)
			{
				cryptoapi_BLOB.cbData = 4U;
			}
			byte[] array = new byte[4];
			if (cryptoapi_BLOB.pbData != IntPtr.Zero)
			{
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, (int)cryptoapi_BLOB.cbData);
			}
			this.m_keyUsages = BitConverter.ToUInt32(array, 0);
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0005A990 File Offset: 0x00059990
		private unsafe static byte[] EncodeExtension(X509KeyUsageFlags keyUsages)
		{
			CAPIBase.CRYPT_BIT_BLOB crypt_BIT_BLOB = default(CAPIBase.CRYPT_BIT_BLOB);
			crypt_BIT_BLOB.cbData = 2U;
			crypt_BIT_BLOB.pbData = new IntPtr((void*)(&keyUsages));
			crypt_BIT_BLOB.cUnusedBits = 0U;
			byte[] result = null;
			if (!CAPI.EncodeObject("2.5.29.15", new IntPtr((void*)(&crypt_BIT_BLOB)), out result))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return result;
		}

		// Token: 0x04001B10 RID: 6928
		private uint m_keyUsages;

		// Token: 0x04001B11 RID: 6929
		private bool m_decoded;
	}
}
