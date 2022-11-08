using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000429 RID: 1065
	[Serializable]
	public class ProtocolViolationException : InvalidOperationException, ISerializable
	{
		// Token: 0x06002155 RID: 8533 RVA: 0x00084011 File Offset: 0x00083011
		public ProtocolViolationException()
		{
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00084019 File Offset: 0x00083019
		public ProtocolViolationException(string message) : base(message)
		{
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00084022 File Offset: 0x00083022
		protected ProtocolViolationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0008402C File Offset: 0x0008302C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x00084036 File Offset: 0x00083036
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
