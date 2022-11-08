using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000340 RID: 832
	public sealed class X509ExtensionEnumerator : IEnumerator
	{
		// Token: 0x06001A36 RID: 6710 RVA: 0x0005B5AA File Offset: 0x0005A5AA
		private X509ExtensionEnumerator()
		{
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x0005B5B2 File Offset: 0x0005A5B2
		internal X509ExtensionEnumerator(X509ExtensionCollection extensions)
		{
			this.m_extensions = extensions;
			this.m_current = -1;
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0005B5C8 File Offset: 0x0005A5C8
		public X509Extension Current
		{
			get
			{
				return this.m_extensions[this.m_current];
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x0005B5DB File Offset: 0x0005A5DB
		object IEnumerator.Current
		{
			get
			{
				return this.m_extensions[this.m_current];
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005B5EE File Offset: 0x0005A5EE
		public bool MoveNext()
		{
			if (this.m_current == this.m_extensions.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0005B616 File Offset: 0x0005A616
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001B1F RID: 6943
		private X509ExtensionCollection m_extensions;

		// Token: 0x04001B20 RID: 6944
		private int m_current;
	}
}
