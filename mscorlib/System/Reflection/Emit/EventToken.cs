using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000824 RID: 2084
	[ComVisible(true)]
	[Serializable]
	public struct EventToken
	{
		// Token: 0x06004A28 RID: 18984 RVA: 0x00101D78 File Offset: 0x00100D78
		internal EventToken(int str)
		{
			this.m_event = str;
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004A29 RID: 18985 RVA: 0x00101D81 File Offset: 0x00100D81
		public int Token
		{
			get
			{
				return this.m_event;
			}
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00101D89 File Offset: 0x00100D89
		public override int GetHashCode()
		{
			return this.m_event;
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x00101D91 File Offset: 0x00100D91
		public override bool Equals(object obj)
		{
			return obj is EventToken && this.Equals((EventToken)obj);
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x00101DA9 File Offset: 0x00100DA9
		public bool Equals(EventToken obj)
		{
			return obj.m_event == this.m_event;
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x00101DBA File Offset: 0x00100DBA
		public static bool operator ==(EventToken a, EventToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x00101DC4 File Offset: 0x00100DC4
		public static bool operator !=(EventToken a, EventToken b)
		{
			return !(a == b);
		}

		// Token: 0x040025E4 RID: 9700
		public static readonly EventToken Empty = default(EventToken);

		// Token: 0x040025E5 RID: 9701
		internal int m_event;
	}
}
