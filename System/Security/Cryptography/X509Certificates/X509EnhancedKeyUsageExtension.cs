using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033C RID: 828
	public sealed class X509EnhancedKeyUsageExtension : X509Extension
	{
		// Token: 0x06001A16 RID: 6678 RVA: 0x0005AC35 File Offset: 0x00059C35
		public X509EnhancedKeyUsageExtension() : base("2.5.29.37")
		{
			this.m_enhancedKeyUsages = new OidCollection();
			this.m_decoded = true;
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0005AC54 File Offset: 0x00059C54
		public X509EnhancedKeyUsageExtension(OidCollection enhancedKeyUsages, bool critical) : base("2.5.29.37", X509EnhancedKeyUsageExtension.EncodeExtension(enhancedKeyUsages), critical)
		{
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0005AC68 File Offset: 0x00059C68
		public X509EnhancedKeyUsageExtension(AsnEncodedData encodedEnhancedKeyUsages, bool critical) : base("2.5.29.37", encodedEnhancedKeyUsages.RawData, critical)
		{
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0005AC7C File Offset: 0x00059C7C
		public OidCollection EnhancedKeyUsages
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				OidCollection oidCollection = new OidCollection();
				foreach (Oid oid in this.m_enhancedKeyUsages)
				{
					oidCollection.Add(oid);
				}
				return oidCollection;
			}
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0005ACC3 File Offset: 0x00059CC3
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0005ACD4 File Offset: 0x00059CD4
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(36L), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = (CAPIBase.CERT_ENHKEY_USAGE)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_ENHKEY_USAGE));
			this.m_enhancedKeyUsages = new OidCollection();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)cert_ENHKEY_USAGE.cUsageIdentifier))
			{
				IntPtr ptr = Marshal.ReadIntPtr(new IntPtr((long)cert_ENHKEY_USAGE.rgpszUsageIdentifier + (long)(num2 * Marshal.SizeOf(typeof(IntPtr)))));
				string oid = Marshal.PtrToStringAnsi(ptr);
				Oid oid2 = new Oid(oid, OidGroup.ExtensionOrAttribute, false);
				this.m_enhancedKeyUsages.Add(oid2);
				num2++;
			}
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0005ADA4 File Offset: 0x00059DA4
		private unsafe static byte[] EncodeExtension(OidCollection enhancedKeyUsages)
		{
			if (enhancedKeyUsages == null)
			{
				throw new ArgumentNullException("enhancedKeyUsages");
			}
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.CopyOidsToUnmanagedMemory(enhancedKeyUsages);
			byte[] result = null;
			using (safeLocalAllocHandle)
			{
				CAPIBase.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = default(CAPIBase.CERT_ENHKEY_USAGE);
				cert_ENHKEY_USAGE.cUsageIdentifier = (uint)enhancedKeyUsages.Count;
				cert_ENHKEY_USAGE.rgpszUsageIdentifier = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.EncodeObject("2.5.29.37", new IntPtr((void*)(&cert_ENHKEY_USAGE)), out result))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return result;
		}

		// Token: 0x04001B16 RID: 6934
		private OidCollection m_enhancedKeyUsages;

		// Token: 0x04001B17 RID: 6935
		private bool m_decoded;
	}
}
