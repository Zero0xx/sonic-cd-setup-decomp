using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CA RID: 1738
	[Serializable]
	public class SmtpException : Exception, ISerializable
	{
		// Token: 0x060035AE RID: 13742 RVA: 0x000E5330 File Offset: 0x000E4330
		private static string GetMessageForStatus(SmtpStatusCode statusCode, string serverResponse)
		{
			return SmtpException.GetMessageForStatus(statusCode) + " " + SR.GetString("MailServerResponse", new object[]
			{
				serverResponse
			});
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x000E5364 File Offset: 0x000E4364
		private static string GetMessageForStatus(SmtpStatusCode statusCode)
		{
			if (statusCode <= SmtpStatusCode.StartMailInput)
			{
				if (statusCode <= SmtpStatusCode.HelpMessage)
				{
					if (statusCode == SmtpStatusCode.SystemStatus)
					{
						return SR.GetString("SmtpSystemStatus");
					}
					if (statusCode == SmtpStatusCode.HelpMessage)
					{
						return SR.GetString("SmtpHelpMessage");
					}
				}
				else
				{
					switch (statusCode)
					{
					case SmtpStatusCode.ServiceReady:
						return SR.GetString("SmtpServiceReady");
					case SmtpStatusCode.ServiceClosingTransmissionChannel:
						return SR.GetString("SmtpServiceClosingTransmissionChannel");
					default:
						switch (statusCode)
						{
						case SmtpStatusCode.Ok:
							return SR.GetString("SmtpOK");
						case SmtpStatusCode.UserNotLocalWillForward:
							return SR.GetString("SmtpUserNotLocalWillForward");
						default:
							if (statusCode == SmtpStatusCode.StartMailInput)
							{
								return SR.GetString("SmtpStartMailInput");
							}
							break;
						}
						break;
					}
				}
			}
			else if (statusCode <= SmtpStatusCode.ClientNotPermitted)
			{
				if (statusCode == SmtpStatusCode.ServiceNotAvailable)
				{
					return SR.GetString("SmtpServiceNotAvailable");
				}
				switch (statusCode)
				{
				case SmtpStatusCode.MailboxBusy:
					return SR.GetString("SmtpMailboxBusy");
				case SmtpStatusCode.LocalErrorInProcessing:
					return SR.GetString("SmtpLocalErrorInProcessing");
				case SmtpStatusCode.InsufficientStorage:
					return SR.GetString("SmtpInsufficientStorage");
				case SmtpStatusCode.ClientNotPermitted:
					return SR.GetString("SmtpClientNotPermitted");
				}
			}
			else
			{
				switch (statusCode)
				{
				case SmtpStatusCode.CommandUnrecognized:
					break;
				case SmtpStatusCode.SyntaxError:
					return SR.GetString("SmtpSyntaxError");
				case SmtpStatusCode.CommandNotImplemented:
					return SR.GetString("SmtpCommandNotImplemented");
				case SmtpStatusCode.BadCommandSequence:
					return SR.GetString("SmtpBadCommandSequence");
				case SmtpStatusCode.CommandParameterNotImplemented:
					return SR.GetString("SmtpCommandParameterNotImplemented");
				default:
					if (statusCode == SmtpStatusCode.MustIssueStartTlsFirst)
					{
						return SR.GetString("SmtpMustIssueStartTlsFirst");
					}
					switch (statusCode)
					{
					case SmtpStatusCode.MailboxUnavailable:
						return SR.GetString("SmtpMailboxUnavailable");
					case SmtpStatusCode.UserNotLocalTryAlternatePath:
						return SR.GetString("SmtpUserNotLocalTryAlternatePath");
					case SmtpStatusCode.ExceededStorageAllocation:
						return SR.GetString("SmtpExceededStorageAllocation");
					case SmtpStatusCode.MailboxNameNotAllowed:
						return SR.GetString("SmtpMailboxNameNotAllowed");
					case SmtpStatusCode.TransactionFailed:
						return SR.GetString("SmtpTransactionFailed");
					}
					break;
				}
			}
			return SR.GetString("SmtpCommandUnrecognized");
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000E5552 File Offset: 0x000E4552
		public SmtpException(SmtpStatusCode statusCode) : base(SmtpException.GetMessageForStatus(statusCode))
		{
			this.statusCode = statusCode;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000E556E File Offset: 0x000E456E
		public SmtpException(SmtpStatusCode statusCode, string message) : base(message)
		{
			this.statusCode = statusCode;
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000E5585 File Offset: 0x000E4585
		public SmtpException() : this(SmtpStatusCode.GeneralFailure)
		{
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000E558E File Offset: 0x000E458E
		public SmtpException(string message) : base(message)
		{
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000E559E File Offset: 0x000E459E
		public SmtpException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000E55AF File Offset: 0x000E45AF
		protected SmtpException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
			this.statusCode = (SmtpStatusCode)serializationInfo.GetInt32("Status");
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000E55D1 File Offset: 0x000E45D1
		internal SmtpException(SmtpStatusCode statusCode, string serverMessage, bool serverResponse) : base(SmtpException.GetMessageForStatus(statusCode, serverMessage))
		{
			this.statusCode = statusCode;
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000E55F0 File Offset: 0x000E45F0
		internal SmtpException(string message, string serverResponse) : base(message + " " + SR.GetString("MailServerResponse", new object[]
		{
			serverResponse
		}))
		{
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000E562B File Offset: 0x000E462B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000E5635 File Offset: 0x000E4635
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("Status", (int)this.statusCode, typeof(int));
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060035BA RID: 13754 RVA: 0x000E565F File Offset: 0x000E465F
		// (set) Token: 0x060035BB RID: 13755 RVA: 0x000E5667 File Offset: 0x000E4667
		public SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				this.statusCode = value;
			}
		}

		// Token: 0x04003102 RID: 12546
		private SmtpStatusCode statusCode = SmtpStatusCode.GeneralFailure;
	}
}
