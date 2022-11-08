using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200032F RID: 815
	[ComVisible(true)]
	[Serializable]
	public sealed class Missing : ISerializable
	{
		// Token: 0x06001F11 RID: 7953 RVA: 0x0004E34B File Offset: 0x0004D34B
		private Missing()
		{
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0004E353 File Offset: 0x0004D353
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, this);
		}

		// Token: 0x04000D6C RID: 3436
		public static readonly Missing Value = new Missing();
	}
}
