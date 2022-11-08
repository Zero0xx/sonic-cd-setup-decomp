using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000376 RID: 886
	internal class PrefixLookup
	{
		// Token: 0x06001BCC RID: 7116 RVA: 0x0006924C File Offset: 0x0006824C
		internal void Add(string prefix, object value)
		{
			lock (this.m_Store)
			{
				this.m_Store[prefix] = value;
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0006928C File Offset: 0x0006828C
		internal object Lookup(string lookupKey)
		{
			if (lookupKey == null)
			{
				return null;
			}
			object result = null;
			int num = 0;
			lock (this.m_Store)
			{
				foreach (object obj in this.m_Store)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = (string)dictionaryEntry.Key;
					if (lookupKey.StartsWith(text))
					{
						int length = text.Length;
						if (length > num)
						{
							num = length;
							result = dictionaryEntry.Value;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001C7F RID: 7295
		private Hashtable m_Store = new Hashtable();
	}
}
