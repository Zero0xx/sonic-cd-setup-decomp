using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000826 RID: 2086
	[ComVisible(true)]
	[Serializable]
	public struct FieldToken
	{
		// Token: 0x06004A4A RID: 19018 RVA: 0x00102195 File Offset: 0x00101195
		internal FieldToken(int field, Type fieldClass)
		{
			this.m_fieldTok = field;
			this.m_class = fieldClass;
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x001021A5 File Offset: 0x001011A5
		public int Token
		{
			get
			{
				return this.m_fieldTok;
			}
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x001021AD File Offset: 0x001011AD
		public override int GetHashCode()
		{
			return this.m_fieldTok;
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x001021B5 File Offset: 0x001011B5
		public override bool Equals(object obj)
		{
			return obj is FieldToken && this.Equals((FieldToken)obj);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x001021CD File Offset: 0x001011CD
		public bool Equals(FieldToken obj)
		{
			return obj.m_fieldTok == this.m_fieldTok && obj.m_class == this.m_class;
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x001021EF File Offset: 0x001011EF
		public static bool operator ==(FieldToken a, FieldToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x001021F9 File Offset: 0x001011F9
		public static bool operator !=(FieldToken a, FieldToken b)
		{
			return !(a == b);
		}

		// Token: 0x040025ED RID: 9709
		public static readonly FieldToken Empty = default(FieldToken);

		// Token: 0x040025EE RID: 9710
		internal int m_fieldTok;

		// Token: 0x040025EF RID: 9711
		internal object m_class;
	}
}
