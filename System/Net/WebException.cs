using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200049C RID: 1180
	[Serializable]
	public class WebException : InvalidOperationException, ISerializable
	{
		// Token: 0x06002401 RID: 9217 RVA: 0x0008D020 File Offset: 0x0008C020
		public WebException()
		{
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0008D030 File Offset: 0x0008C030
		public WebException(string message) : this(message, null)
		{
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0008D03A File Offset: 0x0008C03A
		public WebException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x0008D04C File Offset: 0x0008C04C
		public WebException(string message, WebExceptionStatus status) : this(message, null, status, null)
		{
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x0008D058 File Offset: 0x0008C058
		internal WebException(string message, WebExceptionStatus status, WebExceptionInternalStatus internalStatus, Exception innerException) : this(message, innerException, status, null, internalStatus)
		{
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0008D066 File Offset: 0x0008C066
		public WebException(string message, Exception innerException, WebExceptionStatus status, WebResponse response) : this(message, null, innerException, status, response)
		{
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0008D074 File Offset: 0x0008C074
		internal WebException(string message, string data, Exception innerException, WebExceptionStatus status, WebResponse response) : base(message + ((data != null) ? (": '" + data + "'") : ""), innerException)
		{
			this.m_Status = status;
			this.m_Response = response;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0008D0C0 File Offset: 0x0008C0C0
		internal WebException(string message, Exception innerException, WebExceptionStatus status, WebResponse response, WebExceptionInternalStatus internalStatus) : this(message, null, innerException, status, response, internalStatus)
		{
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x0008D0D0 File Offset: 0x0008C0D0
		internal WebException(string message, string data, Exception innerException, WebExceptionStatus status, WebResponse response, WebExceptionInternalStatus internalStatus) : base(message + ((data != null) ? (": '" + data + "'") : ""), innerException)
		{
			this.m_Status = status;
			this.m_Response = response;
			this.m_InternalStatus = internalStatus;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x0008D124 File Offset: 0x0008C124
		protected WebException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x0008D136 File Offset: 0x0008C136
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0008D140 File Offset: 0x0008C140
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x0008D14A File Offset: 0x0008C14A
		public WebExceptionStatus Status
		{
			get
			{
				return this.m_Status;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x0008D152 File Offset: 0x0008C152
		public WebResponse Response
		{
			get
			{
				return this.m_Response;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x0008D15A File Offset: 0x0008C15A
		internal WebExceptionInternalStatus InternalStatus
		{
			get
			{
				return this.m_InternalStatus;
			}
		}

		// Token: 0x04002461 RID: 9313
		private WebExceptionStatus m_Status = WebExceptionStatus.UnknownError;

		// Token: 0x04002462 RID: 9314
		private WebResponse m_Response;

		// Token: 0x04002463 RID: 9315
		[NonSerialized]
		private WebExceptionInternalStatus m_InternalStatus;
	}
}
