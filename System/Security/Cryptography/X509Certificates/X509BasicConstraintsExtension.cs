using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033B RID: 827
	public sealed class X509BasicConstraintsExtension : X509Extension
	{
		// Token: 0x06001A0D RID: 6669 RVA: 0x0005A9E8 File Offset: 0x000599E8
		public X509BasicConstraintsExtension() : base("2.5.29.19")
		{
			this.m_decoded = true;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0005A9FC File Offset: 0x000599FC
		public X509BasicConstraintsExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint, bool critical) : base("2.5.29.19", X509BasicConstraintsExtension.EncodeExtension(certificateAuthority, hasPathLengthConstraint, pathLengthConstraint), critical)
		{
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0005AA13 File Offset: 0x00059A13
		public X509BasicConstraintsExtension(AsnEncodedData encodedBasicConstraints, bool critical) : base("2.5.29.19", encodedBasicConstraints.RawData, critical)
		{
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0005AA27 File Offset: 0x00059A27
		public bool CertificateAuthority
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_isCA;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x0005AA3D File Offset: 0x00059A3D
		public bool HasPathLengthConstraint
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_hasPathLenConstraint;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0005AA53 File Offset: 0x00059A53
		public int PathLengthConstraint
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_pathLenConstraint;
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0005AA69 File Offset: 0x00059A69
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0005AA7C File Offset: 0x00059A7C
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (base.Oid.Value == "2.5.29.10")
			{
				if (!CAPI.DecodeObject(new IntPtr(13L), this.m_rawData, out safeLocalAllocHandle, out num))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				CAPIBase.CERT_BASIC_CONSTRAINTS_INFO cert_BASIC_CONSTRAINTS_INFO = (CAPIBase.CERT_BASIC_CONSTRAINTS_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_BASIC_CONSTRAINTS_INFO));
				byte[] array = new byte[1];
				Marshal.Copy(cert_BASIC_CONSTRAINTS_INFO.SubjectType.pbData, array, 0, 1);
				this.m_isCA = ((array[0] & 128) != 0);
				this.m_hasPathLenConstraint = cert_BASIC_CONSTRAINTS_INFO.fPathLenConstraint;
				this.m_pathLenConstraint = (int)cert_BASIC_CONSTRAINTS_INFO.dwPathLenConstraint;
			}
			else
			{
				if (!CAPI.DecodeObject(new IntPtr(15L), this.m_rawData, out safeLocalAllocHandle, out num))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO cert_BASIC_CONSTRAINTS2_INFO = (CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO));
				this.m_isCA = (cert_BASIC_CONSTRAINTS2_INFO.fCA != 0);
				this.m_hasPathLenConstraint = (cert_BASIC_CONSTRAINTS2_INFO.fPathLenConstraint != 0);
				this.m_pathLenConstraint = (int)cert_BASIC_CONSTRAINTS2_INFO.dwPathLenConstraint;
			}
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005ABBC File Offset: 0x00059BBC
		private unsafe static byte[] EncodeExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint)
		{
			CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO cert_BASIC_CONSTRAINTS2_INFO = default(CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO);
			cert_BASIC_CONSTRAINTS2_INFO.fCA = (certificateAuthority ? 1 : 0);
			cert_BASIC_CONSTRAINTS2_INFO.fPathLenConstraint = (hasPathLengthConstraint ? 1 : 0);
			if (hasPathLengthConstraint)
			{
				if (pathLengthConstraint < 0)
				{
					throw new ArgumentOutOfRangeException("pathLengthConstraint", SR.GetString("Arg_OutOfRange_NeedNonNegNum"));
				}
				cert_BASIC_CONSTRAINTS2_INFO.dwPathLenConstraint = (uint)pathLengthConstraint;
			}
			byte[] result = null;
			if (!CAPI.EncodeObject("2.5.29.19", new IntPtr((void*)(&cert_BASIC_CONSTRAINTS2_INFO)), out result))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return result;
		}

		// Token: 0x04001B12 RID: 6930
		private bool m_isCA;

		// Token: 0x04001B13 RID: 6931
		private bool m_hasPathLenConstraint;

		// Token: 0x04001B14 RID: 6932
		private int m_pathLenConstraint;

		// Token: 0x04001B15 RID: 6933
		private bool m_decoded;
	}
}
