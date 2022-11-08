using System;
using System.IO;

namespace System.Net
{
	// Token: 0x02000597 RID: 1431
	internal class FixedSizeReader
	{
		// Token: 0x06002C18 RID: 11288 RVA: 0x000BDC85 File Offset: 0x000BCC85
		public FixedSizeReader(Stream transport)
		{
			this._Transport = transport;
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000BDC94 File Offset: 0x000BCC94
		public int ReadPacket(byte[] buffer, int offset, int count)
		{
			int num = count;
			for (;;)
			{
				int num2 = this._Transport.Read(buffer, offset, num);
				if (num2 == 0)
				{
					break;
				}
				num -= num2;
				offset += num2;
				if (num == 0)
				{
					return count;
				}
			}
			if (num != count)
			{
				throw new IOException(SR.GetString("net_io_eof"));
			}
			return 0;
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000BDCD8 File Offset: 0x000BCCD8
		public void AsyncReadPacket(AsyncProtocolRequest request)
		{
			this._Request = request;
			this._TotalRead = 0;
			this.StartReading();
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000BDCF0 File Offset: 0x000BCCF0
		private void StartReading()
		{
			for (;;)
			{
				IAsyncResult asyncResult = this._Transport.BeginRead(this._Request.Buffer, this._Request.Offset + this._TotalRead, this._Request.Count - this._TotalRead, FixedSizeReader._ReadCallback, this);
				if (!asyncResult.CompletedSynchronously)
				{
					break;
				}
				int bytes = this._Transport.EndRead(asyncResult);
				if (this.CheckCompletionBeforeNextRead(bytes))
				{
					return;
				}
			}
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000BDD60 File Offset: 0x000BCD60
		private bool CheckCompletionBeforeNextRead(int bytes)
		{
			if (bytes == 0)
			{
				if (this._TotalRead == 0)
				{
					this._Request.CompleteRequest(0);
					return true;
				}
				throw new IOException(SR.GetString("net_io_eof"));
			}
			else
			{
				if ((this._TotalRead += bytes) == this._Request.Count)
				{
					this._Request.CompleteRequest(this._Request.Count);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000BDDD0 File Offset: 0x000BCDD0
		private static void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			FixedSizeReader fixedSizeReader = (FixedSizeReader)transportResult.AsyncState;
			AsyncProtocolRequest request = fixedSizeReader._Request;
			try
			{
				int bytes = fixedSizeReader._Transport.EndRead(transportResult);
				if (!fixedSizeReader.CheckCompletionBeforeNextRead(bytes))
				{
					fixedSizeReader.StartReading();
				}
			}
			catch (Exception e)
			{
				if (request.IsUserCompleted)
				{
					throw;
				}
				request.CompleteWithError(e);
			}
		}

		// Token: 0x040029F7 RID: 10743
		private static readonly AsyncCallback _ReadCallback = new AsyncCallback(FixedSizeReader.ReadCallback);

		// Token: 0x040029F8 RID: 10744
		private readonly Stream _Transport;

		// Token: 0x040029F9 RID: 10745
		private AsyncProtocolRequest _Request;

		// Token: 0x040029FA RID: 10746
		private int _TotalRead;
	}
}
