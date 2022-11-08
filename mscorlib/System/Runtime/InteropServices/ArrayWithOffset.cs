using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D9 RID: 1241
	[ComVisible(true)]
	[Serializable]
	public struct ArrayWithOffset
	{
		// Token: 0x06003131 RID: 12593 RVA: 0x000A8F2D File Offset: 0x000A7F2D
		public ArrayWithOffset(object array, int offset)
		{
			this.m_array = array;
			this.m_offset = offset;
			this.m_count = 0;
			this.m_count = this.CalculateCount();
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000A8F50 File Offset: 0x000A7F50
		public object GetArray()
		{
			return this.m_array;
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000A8F58 File Offset: 0x000A7F58
		public int GetOffset()
		{
			return this.m_offset;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000A8F60 File Offset: 0x000A7F60
		public override int GetHashCode()
		{
			return this.m_count + this.m_offset;
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000A8F6F File Offset: 0x000A7F6F
		public override bool Equals(object obj)
		{
			return obj is ArrayWithOffset && this.Equals((ArrayWithOffset)obj);
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000A8F87 File Offset: 0x000A7F87
		public bool Equals(ArrayWithOffset obj)
		{
			return obj.m_array == this.m_array && obj.m_offset == this.m_offset && obj.m_count == this.m_count;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000A8FB8 File Offset: 0x000A7FB8
		public static bool operator ==(ArrayWithOffset a, ArrayWithOffset b)
		{
			return a.Equals(b);
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000A8FC2 File Offset: 0x000A7FC2
		public static bool operator !=(ArrayWithOffset a, ArrayWithOffset b)
		{
			return !(a == b);
		}

		// Token: 0x06003139 RID: 12601
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CalculateCount();

		// Token: 0x040018E9 RID: 6377
		private object m_array;

		// Token: 0x040018EA RID: 6378
		private int m_offset;

		// Token: 0x040018EB RID: 6379
		private int m_count;
	}
}
