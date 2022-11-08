using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200029F RID: 671
	internal sealed class Mscorlib_DictionaryDebugView<K, V>
	{
		// Token: 0x06001A33 RID: 6707 RVA: 0x00044454 File Offset: 0x00043454
		public Mscorlib_DictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			this.dict = dictionary;
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0004446C File Offset: 0x0004346C
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<K, V>[] Items
		{
			get
			{
				KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[this.dict.Count];
				this.dict.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04000A30 RID: 2608
		private IDictionary<K, V> dict;
	}
}
