using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000100 RID: 256
	public sealed class AdapterListCollection : IEnumerable, IEnumerator
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x0006595C File Offset: 0x00064D5C
		internal AdapterListCollection()
		{
			this.current = -1;
			this.maxAdapter = Manager.AdapterCount;
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00065988 File Offset: 0x00064D88
		public object Current
		{
			get
			{
				return new AdapterInformation(this.current);
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000625C8 File Offset: 0x000619C8
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000625E4 File Offset: 0x000619E4
		[return: MarshalAs(UnmanagedType.U1)]
		public bool MoveNext()
		{
			if (this.maxAdapter == 0)
			{
				return false;
			}
			if (this.current == this.maxAdapter - 1)
			{
				return false;
			}
			this.current++;
			return true;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00062624 File Offset: 0x00061A24
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00062638 File Offset: 0x00061A38
		public int Count
		{
			get
			{
				return this.maxAdapter;
			}
		}

		// Token: 0x17000296 RID: 662
		public AdapterInformation this[int adapter]
		{
			get
			{
				return new AdapterInformation(adapter);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00065944 File Offset: 0x00064D44
		public AdapterInformation Default
		{
			get
			{
				return new AdapterInformation(0);
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00062650 File Offset: 0x00061A50
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(AdapterListCollection);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00062680 File Offset: 0x00061A80
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400103E RID: 4158
		private int current;

		// Token: 0x0400103F RID: 4159
		private int maxAdapter;
	}
}
