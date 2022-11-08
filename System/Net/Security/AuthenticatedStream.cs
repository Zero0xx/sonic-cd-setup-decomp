using System;
using System.IO;

namespace System.Net.Security
{
	// Token: 0x02000586 RID: 1414
	public abstract class AuthenticatedStream : Stream
	{
		// Token: 0x06002B7C RID: 11132 RVA: 0x000BC814 File Offset: 0x000BB814
		protected AuthenticatedStream(Stream innerStream, bool leaveInnerStreamOpen)
		{
			if (innerStream == null || innerStream == Stream.Null)
			{
				throw new ArgumentNullException("innerStream");
			}
			if (!innerStream.CanRead || !innerStream.CanWrite)
			{
				throw new ArgumentException(SR.GetString("net_io_must_be_rw_stream"), "innerStream");
			}
			this._InnerStream = innerStream;
			this._LeaveStreamOpen = leaveInnerStreamOpen;
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x000BC870 File Offset: 0x000BB870
		public bool LeaveInnerStreamOpen
		{
			get
			{
				return this._LeaveStreamOpen;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000BC878 File Offset: 0x000BB878
		protected Stream InnerStream
		{
			get
			{
				return this._InnerStream;
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000BC880 File Offset: 0x000BB880
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._LeaveStreamOpen)
					{
						this._InnerStream.Flush();
					}
					else
					{
						this._InnerStream.Close();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06002B80 RID: 11136
		public abstract bool IsAuthenticated { get; }

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06002B81 RID: 11137
		public abstract bool IsMutuallyAuthenticated { get; }

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06002B82 RID: 11138
		public abstract bool IsEncrypted { get; }

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06002B83 RID: 11139
		public abstract bool IsSigned { get; }

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06002B84 RID: 11140
		public abstract bool IsServer { get; }

		// Token: 0x040029BD RID: 10685
		private Stream _InnerStream;

		// Token: 0x040029BE RID: 10686
		private bool _LeaveStreamOpen;
	}
}
