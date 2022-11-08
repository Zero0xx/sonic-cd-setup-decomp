using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033F RID: 831
	public sealed class X509ExtensionCollection : ICollection, IEnumerable
	{
		// Token: 0x06001A2A RID: 6698 RVA: 0x0005B2EC File Offset: 0x0005A2EC
		public X509ExtensionCollection()
		{
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0005B300 File Offset: 0x0005A300
		internal unsafe X509ExtensionCollection(SafeCertContextHandle safeCertContextHandle)
		{
			using (SafeCertContextHandle safeCertContextHandle2 = CAPI.CertDuplicateCertificateContext(safeCertContextHandle))
			{
				CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle2.DangerousGetHandle());
				CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
				uint cExtension = cert_INFO.cExtension;
				IntPtr rgExtension = cert_INFO.rgExtension;
				for (uint num = 0U; num < cExtension; num += 1U)
				{
					X509Extension x509Extension = new X509Extension(new IntPtr((long)rgExtension + (long)((ulong)num * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CERT_EXTENSION))))));
					X509Extension x509Extension2 = CryptoConfig.CreateFromName(x509Extension.Oid.Value) as X509Extension;
					if (x509Extension2 != null)
					{
						x509Extension2.CopyFrom(x509Extension);
						x509Extension = x509Extension2;
					}
					this.Add(x509Extension);
				}
			}
		}

		// Token: 0x17000507 RID: 1287
		public X509Extension this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
				}
				return (X509Extension)this.m_list[index];
			}
		}

		// Token: 0x17000508 RID: 1288
		public X509Extension this[string oid]
		{
			get
			{
				string text = X509Utils.FindOidInfo(2U, oid, OidGroup.ExtensionOrAttribute);
				if (text == null)
				{
					text = oid;
				}
				foreach (object obj in this.m_list)
				{
					X509Extension x509Extension = (X509Extension)obj;
					if (string.Compare(x509Extension.Oid.Value, text, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return x509Extension;
					}
				}
				return null;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x0005B4C4 File Offset: 0x0005A4C4
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0005B4D1 File Offset: 0x0005A4D1
		public int Add(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			return this.m_list.Add(extension);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0005B4ED File Offset: 0x0005A4ED
		public X509ExtensionEnumerator GetEnumerator()
		{
			return new X509ExtensionEnumerator(this);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0005B4F5 File Offset: 0x0005A4F5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new X509ExtensionEnumerator(this);
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0005B500 File Offset: 0x0005A500
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0005B59A File Offset: 0x0005A59A
		public void CopyTo(X509Extension[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0005B5A4 File Offset: 0x0005A5A4
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x0005B5A7 File Offset: 0x0005A5A7
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001B1E RID: 6942
		private ArrayList m_list = new ArrayList();
	}
}
