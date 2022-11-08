using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200032D RID: 813
	public sealed class X509Certificate2Enumerator : IEnumerator
	{
		// Token: 0x060019BF RID: 6591 RVA: 0x00059708 File Offset: 0x00058708
		private X509Certificate2Enumerator()
		{
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00059710 File Offset: 0x00058710
		internal X509Certificate2Enumerator(X509Certificate2Collection mappings)
		{
			this.baseEnumerator = ((IEnumerable)mappings).GetEnumerator();
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x00059724 File Offset: 0x00058724
		public X509Certificate2 Current
		{
			get
			{
				return (X509Certificate2)this.baseEnumerator.Current;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x00059736 File Offset: 0x00058736
		object IEnumerator.Current
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x00059743 File Offset: 0x00058743
		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00059750 File Offset: 0x00058750
		bool IEnumerator.MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0005975D File Offset: 0x0005875D
		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0005976A File Offset: 0x0005876A
		void IEnumerator.Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x04001ABD RID: 6845
		private IEnumerator baseEnumerator;
	}
}
