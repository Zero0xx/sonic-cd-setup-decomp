using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000396 RID: 918
	[Serializable]
	public class CookieException : FormatException, ISerializable
	{
		// Token: 0x06001CB0 RID: 7344 RVA: 0x0006D6AC File Offset: 0x0006C6AC
		public CookieException()
		{
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x0006D6B4 File Offset: 0x0006C6B4
		internal CookieException(string message) : base(message)
		{
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0006D6BD File Offset: 0x0006C6BD
		internal CookieException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0006D6C7 File Offset: 0x0006C6C7
		protected CookieException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0006D6D1 File Offset: 0x0006C6D1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0006D6DB File Offset: 0x0006C6DB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
