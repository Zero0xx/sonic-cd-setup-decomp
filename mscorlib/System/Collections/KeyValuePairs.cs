using System;
using System.Diagnostics;

namespace System.Collections
{
	// Token: 0x0200026E RID: 622
	[DebuggerDisplay("{value}", Name = "[{key}]", Type = "")]
	internal class KeyValuePairs
	{
		// Token: 0x06001875 RID: 6261 RVA: 0x0003F221 File Offset: 0x0003E221
		public KeyValuePairs(object key, object value)
		{
			this.value = value;
			this.key = key;
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001876 RID: 6262 RVA: 0x0003F237 File Offset: 0x0003E237
		public object Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x0003F23F File Offset: 0x0003E23F
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x040009B7 RID: 2487
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object key;

		// Token: 0x040009B8 RID: 2488
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object value;
	}
}
