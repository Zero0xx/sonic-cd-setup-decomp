using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000331 RID: 817
	public class X509ChainElement
	{
		// Token: 0x060019D9 RID: 6617 RVA: 0x0005A21E File Offset: 0x0005921E
		private X509ChainElement()
		{
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0005A228 File Offset: 0x00059228
		internal unsafe X509ChainElement(IntPtr pChainElement)
		{
			CAPIBase.CERT_CHAIN_ELEMENT cert_CHAIN_ELEMENT = new CAPIBase.CERT_CHAIN_ELEMENT(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_ELEMENT)));
			uint num = (uint)Marshal.ReadInt32(pChainElement);
			if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_CHAIN_ELEMENT)))
			{
				num = (uint)Marshal.SizeOf(cert_CHAIN_ELEMENT);
			}
			X509Utils.memcpy(pChainElement, new IntPtr((void*)(&cert_CHAIN_ELEMENT)), num);
			this.m_certificate = new X509Certificate2(cert_CHAIN_ELEMENT.pCertContext);
			if (cert_CHAIN_ELEMENT.pwszExtendedErrorInfo == IntPtr.Zero)
			{
				this.m_description = string.Empty;
			}
			else
			{
				this.m_description = Marshal.PtrToStringUni(cert_CHAIN_ELEMENT.pwszExtendedErrorInfo);
			}
			if (cert_CHAIN_ELEMENT.dwErrorStatus == 0U)
			{
				this.m_chainStatus = new X509ChainStatus[0];
				return;
			}
			this.m_chainStatus = X509Chain.GetChainStatusInformation(cert_CHAIN_ELEMENT.dwErrorStatus);
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x0005A2F4 File Offset: 0x000592F4
		public X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x0005A2FC File Offset: 0x000592FC
		public X509ChainStatus[] ChainElementStatus
		{
			get
			{
				return this.m_chainStatus;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x0005A304 File Offset: 0x00059304
		public string Information
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x04001ADF RID: 6879
		private X509Certificate2 m_certificate;

		// Token: 0x04001AE0 RID: 6880
		private X509ChainStatus[] m_chainStatus;

		// Token: 0x04001AE1 RID: 6881
		private string m_description;
	}
}
