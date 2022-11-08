using System;

namespace System.Collections.Generic
{
	// Token: 0x0200028B RID: 651
	public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x170003D1 RID: 977
		TValue this[TKey key]
		{
			get;
			set;
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600198D RID: 6541
		ICollection<TKey> Keys { get; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600198E RID: 6542
		ICollection<TValue> Values { get; }

		// Token: 0x0600198F RID: 6543
		bool ContainsKey(TKey key);

		// Token: 0x06001990 RID: 6544
		void Add(TKey key, TValue value);

		// Token: 0x06001991 RID: 6545
		bool Remove(TKey key);

		// Token: 0x06001992 RID: 6546
		bool TryGetValue(TKey key, out TValue value);
	}
}
