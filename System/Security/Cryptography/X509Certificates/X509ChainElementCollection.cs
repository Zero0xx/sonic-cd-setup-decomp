using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000332 RID: 818
	public sealed class X509ChainElementCollection : ICollection, IEnumerable
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x0005A30C File Offset: 0x0005930C
		internal X509ChainElementCollection()
		{
			this.m_elements = new X509ChainElement[0];
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0005A320 File Offset: 0x00059320
		internal unsafe X509ChainElementCollection(IntPtr pSimpleChain)
		{
			CAPIBase.CERT_SIMPLE_CHAIN cert_SIMPLE_CHAIN = new CAPIBase.CERT_SIMPLE_CHAIN(Marshal.SizeOf(typeof(CAPIBase.CERT_SIMPLE_CHAIN)));
			uint num = (uint)Marshal.ReadInt32(pSimpleChain);
			if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_SIMPLE_CHAIN)))
			{
				num = (uint)Marshal.SizeOf(cert_SIMPLE_CHAIN);
			}
			X509Utils.memcpy(pSimpleChain, new IntPtr((void*)(&cert_SIMPLE_CHAIN)), num);
			this.m_elements = new X509ChainElement[cert_SIMPLE_CHAIN.cElement];
			for (int i = 0; i < this.m_elements.Length; i++)
			{
				this.m_elements[i] = new X509ChainElement(Marshal.ReadIntPtr(new IntPtr((long)cert_SIMPLE_CHAIN.rgpElement + (long)(i * Marshal.SizeOf(typeof(IntPtr))))));
			}
		}

		// Token: 0x170004F2 RID: 1266
		public X509ChainElement this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_elements.Length)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
				}
				return this.m_elements[index];
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x0005A41A File Offset: 0x0005941A
		public int Count
		{
			get
			{
				return this.m_elements.Length;
			}
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0005A424 File Offset: 0x00059424
		public X509ChainElementEnumerator GetEnumerator()
		{
			return new X509ChainElementEnumerator(this);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0005A42C File Offset: 0x0005942C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new X509ChainElementEnumerator(this);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0005A434 File Offset: 0x00059434
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

		// Token: 0x060019E5 RID: 6629 RVA: 0x0005A4CE File Offset: 0x000594CE
		public void CopyTo(X509ChainElement[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x0005A4D8 File Offset: 0x000594D8
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x0005A4DB File Offset: 0x000594DB
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001AE2 RID: 6882
		private X509ChainElement[] m_elements;
	}
}
