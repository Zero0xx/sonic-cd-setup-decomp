using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AD RID: 173
	[Serializable]
	internal sealed class Empty : ISerializable
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x0001F915 File Offset: 0x0001E915
		private Empty()
		{
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0001F91D File Offset: 0x0001E91D
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0001F924 File Offset: 0x0001E924
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 1, null, null);
		}

		// Token: 0x040003C2 RID: 962
		public static readonly Empty Value = new Empty();
	}
}
