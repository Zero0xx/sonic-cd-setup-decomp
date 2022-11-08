using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082D RID: 2093
	[ComVisible(true)]
	[Serializable]
	public struct Label
	{
		// Token: 0x06004A78 RID: 19064 RVA: 0x00102BE0 File Offset: 0x00101BE0
		internal Label(int label)
		{
			this.m_label = label;
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x00102BE9 File Offset: 0x00101BE9
		internal int GetLabelValue()
		{
			return this.m_label;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x00102BF1 File Offset: 0x00101BF1
		public override int GetHashCode()
		{
			return this.m_label;
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x00102BF9 File Offset: 0x00101BF9
		public override bool Equals(object obj)
		{
			return obj is Label && this.Equals((Label)obj);
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x00102C11 File Offset: 0x00101C11
		public bool Equals(Label obj)
		{
			return obj.m_label == this.m_label;
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x00102C22 File Offset: 0x00101C22
		public static bool operator ==(Label a, Label b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x00102C2C File Offset: 0x00101C2C
		public static bool operator !=(Label a, Label b)
		{
			return !(a == b);
		}

		// Token: 0x0400261F RID: 9759
		internal int m_label;
	}
}
