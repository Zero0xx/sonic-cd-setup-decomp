using System;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000624 RID: 1572
	[Serializable]
	public class PingException : InvalidOperationException
	{
		// Token: 0x0600305E RID: 12382 RVA: 0x000D147C File Offset: 0x000D047C
		internal PingException()
		{
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000D1484 File Offset: 0x000D0484
		protected PingException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000D148E File Offset: 0x000D048E
		public PingException(string message) : base(message)
		{
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000D1497 File Offset: 0x000D0497
		public PingException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
