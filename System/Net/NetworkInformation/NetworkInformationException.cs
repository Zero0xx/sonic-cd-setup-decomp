using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000616 RID: 1558
	[Serializable]
	public class NetworkInformationException : Win32Exception
	{
		// Token: 0x06003002 RID: 12290 RVA: 0x000CF7F0 File Offset: 0x000CE7F0
		public NetworkInformationException() : base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x000CF7FD File Offset: 0x000CE7FD
		public NetworkInformationException(int errorCode) : base(errorCode)
		{
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000CF806 File Offset: 0x000CE806
		internal NetworkInformationException(SocketError socketError) : base((int)socketError)
		{
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000CF80F File Offset: 0x000CE80F
		protected NetworkInformationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000CF819 File Offset: 0x000CE819
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
