using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000338 RID: 824
	public class X509Extension : AsnEncodedData
	{
		// Token: 0x060019FD RID: 6653 RVA: 0x0005A6EC File Offset: 0x000596EC
		internal X509Extension(string oid) : base(new Oid(oid, OidGroup.ExtensionOrAttribute, false))
		{
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0005A6FC File Offset: 0x000596FC
		internal X509Extension(IntPtr pExtension)
		{
			CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(pExtension, typeof(CAPIBase.CERT_EXTENSION));
			this.m_critical = cert_EXTENSION.fCritical;
			string pszObjId = cert_EXTENSION.pszObjId;
			this.m_oid = new Oid(pszObjId, OidGroup.ExtensionOrAttribute, false);
			byte[] array = new byte[cert_EXTENSION.Value.cbData];
			if (cert_EXTENSION.Value.pbData != IntPtr.Zero)
			{
				Marshal.Copy(cert_EXTENSION.Value.pbData, array, 0, array.Length);
			}
			this.m_rawData = array;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0005A790 File Offset: 0x00059790
		protected X509Extension()
		{
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0005A798 File Offset: 0x00059798
		public X509Extension(string oid, byte[] rawData, bool critical) : this(new Oid(oid, OidGroup.ExtensionOrAttribute, true), rawData, critical)
		{
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0005A7AA File Offset: 0x000597AA
		public X509Extension(AsnEncodedData encodedExtension, bool critical) : this(encodedExtension.Oid, encodedExtension.RawData, critical)
		{
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0005A7C0 File Offset: 0x000597C0
		public X509Extension(Oid oid, byte[] rawData, bool critical) : base(oid, rawData)
		{
			if (base.Oid == null || base.Oid.Value == null)
			{
				throw new ArgumentNullException("oid");
			}
			if (base.Oid.Value.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Arg_EmptyOrNullString"), "oid.Value");
			}
			this.m_critical = critical;
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0005A823 File Offset: 0x00059823
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x0005A82B File Offset: 0x0005982B
		public bool Critical
		{
			get
			{
				return this.m_critical;
			}
			set
			{
				this.m_critical = value;
			}
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0005A834 File Offset: 0x00059834
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			X509Extension x509Extension = asnEncodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(SR.GetString("Cryptography_X509_ExtensionMismatch"));
			}
			base.CopyFrom(asnEncodedData);
			this.m_critical = x509Extension.Critical;
		}

		// Token: 0x04001B04 RID: 6916
		private bool m_critical;
	}
}
