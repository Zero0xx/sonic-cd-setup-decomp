using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200035D RID: 861
	[Serializable]
	public class UriFormatException : FormatException, ISerializable
	{
		// Token: 0x06001B80 RID: 7040 RVA: 0x000674BA File Offset: 0x000664BA
		public UriFormatException()
		{
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000674C2 File Offset: 0x000664C2
		public UriFormatException(string textString) : base(textString)
		{
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000674CB File Offset: 0x000664CB
		protected UriFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000674D5 File Offset: 0x000664D5
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
