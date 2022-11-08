using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
	// Token: 0x0200043D RID: 1085
	[Serializable]
	public class SocketException : Win32Exception
	{
		// Token: 0x0600220C RID: 8716 RVA: 0x000868CF File Offset: 0x000858CF
		public SocketException() : base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000868DC File Offset: 0x000858DC
		internal SocketException(EndPoint endPoint) : base(Marshal.GetLastWin32Error())
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000868F0 File Offset: 0x000858F0
		public SocketException(int errorCode) : base(errorCode)
		{
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000868F9 File Offset: 0x000858F9
		internal SocketException(int errorCode, EndPoint endPoint) : base(errorCode)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00086909 File Offset: 0x00085909
		internal SocketException(SocketError socketError) : base((int)socketError)
		{
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00086912 File Offset: 0x00085912
		protected SocketException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x0008691C File Offset: 0x0008591C
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x00086924 File Offset: 0x00085924
		public override string Message
		{
			get
			{
				if (this.m_EndPoint == null)
				{
					return base.Message;
				}
				return base.Message + " " + this.m_EndPoint.ToString();
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x00086950 File Offset: 0x00085950
		public SocketError SocketErrorCode
		{
			get
			{
				return (SocketError)base.NativeErrorCode;
			}
		}

		// Token: 0x04002204 RID: 8708
		[NonSerialized]
		private EndPoint m_EndPoint;
	}
}
