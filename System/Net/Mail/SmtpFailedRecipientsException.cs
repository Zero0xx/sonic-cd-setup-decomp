using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CC RID: 1740
	[Serializable]
	public class SmtpFailedRecipientsException : SmtpFailedRecipientException, ISerializable
	{
		// Token: 0x060035C6 RID: 13766 RVA: 0x000E5710 File Offset: 0x000E4710
		public SmtpFailedRecipientsException()
		{
			this.innerExceptions = new SmtpFailedRecipientException[0];
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000E5724 File Offset: 0x000E4724
		public SmtpFailedRecipientsException(string message) : base(message)
		{
			this.innerExceptions = new SmtpFailedRecipientException[0];
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000E573C File Offset: 0x000E473C
		public SmtpFailedRecipientsException(string message, Exception innerException) : base(message, innerException)
		{
			SmtpFailedRecipientException ex = innerException as SmtpFailedRecipientException;
			this.innerExceptions = ((ex == null) ? new SmtpFailedRecipientException[0] : new SmtpFailedRecipientException[]
			{
				ex
			});
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000E5775 File Offset: 0x000E4775
		protected SmtpFailedRecipientsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.innerExceptions = (SmtpFailedRecipientException[])info.GetValue("innerExceptions", typeof(SmtpFailedRecipientException[]));
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000E57A0 File Offset: 0x000E47A0
		public SmtpFailedRecipientsException(string message, SmtpFailedRecipientException[] innerExceptions) : base(message, (innerExceptions != null && innerExceptions.Length > 0) ? innerExceptions[0].FailedRecipient : null, (innerExceptions != null && innerExceptions.Length > 0) ? innerExceptions[0] : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			this.innerExceptions = ((innerExceptions == null) ? new SmtpFailedRecipientException[0] : innerExceptions);
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000E57F8 File Offset: 0x000E47F8
		internal SmtpFailedRecipientsException(ArrayList innerExceptions, bool allFailed) : base(allFailed ? SR.GetString("SmtpAllRecipientsFailed") : SR.GetString("SmtpRecipientFailed"), (innerExceptions != null && innerExceptions.Count > 0) ? ((SmtpFailedRecipientException)innerExceptions[0]).FailedRecipient : null, (innerExceptions != null && innerExceptions.Count > 0) ? ((SmtpFailedRecipientException)innerExceptions[0]) : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			this.innerExceptions = new SmtpFailedRecipientException[innerExceptions.Count];
			int num = 0;
			foreach (object obj in innerExceptions)
			{
				SmtpFailedRecipientException ex = (SmtpFailedRecipientException)obj;
				this.innerExceptions[num++] = ex;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x060035CC RID: 13772 RVA: 0x000E58D0 File Offset: 0x000E48D0
		public SmtpFailedRecipientException[] InnerExceptions
		{
			get
			{
				return this.innerExceptions;
			}
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000E58D8 File Offset: 0x000E48D8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000E58E2 File Offset: 0x000E48E2
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("innerExceptions", this.innerExceptions, typeof(SmtpFailedRecipientException[]));
		}

		// Token: 0x04003105 RID: 12549
		private SmtpFailedRecipientException[] innerExceptions;
	}
}
