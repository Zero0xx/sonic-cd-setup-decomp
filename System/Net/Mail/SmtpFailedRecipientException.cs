using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CB RID: 1739
	[Serializable]
	public class SmtpFailedRecipientException : SmtpException, ISerializable
	{
		// Token: 0x060035BC RID: 13756 RVA: 0x000E5670 File Offset: 0x000E4670
		public SmtpFailedRecipientException()
		{
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000E5678 File Offset: 0x000E4678
		public SmtpFailedRecipientException(string message) : base(message)
		{
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000E5681 File Offset: 0x000E4681
		public SmtpFailedRecipientException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000E568B File Offset: 0x000E468B
		protected SmtpFailedRecipientException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.failedRecipient = info.GetString("failedRecipient");
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000E56A6 File Offset: 0x000E46A6
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient) : base(statusCode)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000E56B6 File Offset: 0x000E46B6
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient, string serverResponse) : base(statusCode, serverResponse, true)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000E56C8 File Offset: 0x000E46C8
		public SmtpFailedRecipientException(string message, string failedRecipient, Exception innerException) : base(message, innerException)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x000E56D9 File Offset: 0x000E46D9
		public string FailedRecipient
		{
			get
			{
				return this.failedRecipient;
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000E56E1 File Offset: 0x000E46E1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000E56EB File Offset: 0x000E46EB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("failedRecipient", this.failedRecipient, typeof(string));
		}

		// Token: 0x04003103 RID: 12547
		private string failedRecipient;

		// Token: 0x04003104 RID: 12548
		internal bool fatal;
	}
}
