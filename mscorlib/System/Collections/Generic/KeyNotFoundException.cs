using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	// Token: 0x020002A2 RID: 674
	[ComVisible(true)]
	[Serializable]
	public class KeyNotFoundException : SystemException, ISerializable
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x000444E4 File Offset: 0x000434E4
		public KeyNotFoundException() : base(Environment.GetResourceString("Arg_KeyNotFound"))
		{
			base.SetErrorCode(-2146232969);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00044501 File Offset: 0x00043501
		public KeyNotFoundException(string message) : base(message)
		{
			base.SetErrorCode(-2146232969);
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00044515 File Offset: 0x00043515
		public KeyNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146232969);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0004452A File Offset: 0x0004352A
		protected KeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
