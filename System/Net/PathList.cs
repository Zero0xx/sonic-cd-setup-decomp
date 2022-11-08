using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000394 RID: 916
	[Serializable]
	internal class PathList
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x0006D579 File Offset: 0x0006C579
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0006D588 File Offset: 0x0006C588
		public int GetCookiesCount()
		{
			int num = 0;
			foreach (object obj in this.m_list.Values)
			{
				CookieCollection cookieCollection = (CookieCollection)obj;
				num += cookieCollection.Count;
			}
			return num;
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x0006D5EC File Offset: 0x0006C5EC
		public ICollection Values
		{
			get
			{
				return this.m_list.Values;
			}
		}

		// Token: 0x17000599 RID: 1433
		public object this[string s]
		{
			get
			{
				return this.m_list[s];
			}
			set
			{
				this.m_list[s] = value;
			}
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0006D616 File Offset: 0x0006C616
		public IEnumerator GetEnumerator()
		{
			return this.m_list.GetEnumerator();
		}

		// Token: 0x04001D33 RID: 7475
		private SortedList m_list = SortedList.Synchronized(new SortedList(PathList.PathListComparer.StaticInstance));

		// Token: 0x02000395 RID: 917
		[Serializable]
		private class PathListComparer : IComparer
		{
			// Token: 0x06001CAD RID: 7341 RVA: 0x0006D624 File Offset: 0x0006C624
			int IComparer.Compare(object ol, object or)
			{
				string text = CookieParser.CheckQuoted((string)ol);
				string text2 = CookieParser.CheckQuoted((string)or);
				int length = text.Length;
				int length2 = text2.Length;
				int num = Math.Min(length, length2);
				for (int i = 0; i < num; i++)
				{
					if (text[i] != text2[i])
					{
						return (int)(text[i] - text2[i]);
					}
				}
				return length2 - length;
			}

			// Token: 0x04001D34 RID: 7476
			internal static readonly PathList.PathListComparer StaticInstance = new PathList.PathListComparer();
		}
	}
}
