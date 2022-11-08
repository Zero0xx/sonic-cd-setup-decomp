using System;
using System.IO;
using System.Net.Mail;

namespace System.Net.Mime
{
	// Token: 0x020006AC RID: 1708
	internal class MimePart : MimeBasePart, IDisposable
	{
		// Token: 0x060034B6 RID: 13494 RVA: 0x000DFE9C File Offset: 0x000DEE9C
		internal MimePart()
		{
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x000DFEA4 File Offset: 0x000DEEA4
		public void Dispose()
		{
			if (this.stream != null)
			{
				this.stream.Close();
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x000DFEB9 File Offset: 0x000DEEB9
		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x060034B9 RID: 13497 RVA: 0x000DFEC1 File Offset: 0x000DEEC1
		// (set) Token: 0x060034BA RID: 13498 RVA: 0x000DFEC9 File Offset: 0x000DEEC9
		internal ContentDisposition ContentDisposition
		{
			get
			{
				return this.contentDisposition;
			}
			set
			{
				this.contentDisposition = value;
				if (value == null)
				{
					((HeaderCollection)base.Headers).InternalRemove(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition));
					return;
				}
				this.contentDisposition.PersistIfNeeded((HeaderCollection)base.Headers, true);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060034BB RID: 13499 RVA: 0x000DFF04 File Offset: 0x000DEF04
		// (set) Token: 0x060034BC RID: 13500 RVA: 0x000DFF74 File Offset: 0x000DEF74
		internal TransferEncoding TransferEncoding
		{
			get
			{
				if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("base64", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.Base64;
				}
				if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("quoted-printable", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.QuotedPrintable;
				}
				if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("7bit", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.SevenBit;
				}
				return TransferEncoding.Unknown;
			}
			set
			{
				if (value == TransferEncoding.Base64)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "base64";
					return;
				}
				if (value == TransferEncoding.QuotedPrintable)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "quoted-printable";
					return;
				}
				if (value == TransferEncoding.SevenBit)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "7bit";
					return;
				}
				throw new NotSupportedException(SR.GetString("MimeTransferEncodingNotSupported", new object[]
				{
					value
				}));
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000DFFF4 File Offset: 0x000DEFF4
		internal void SetContent(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.streamSet)
			{
				this.stream.Close();
				this.stream = null;
				this.streamSet = false;
			}
			this.stream = stream;
			this.streamSet = true;
			this.streamUsedOnce = false;
			this.TransferEncoding = TransferEncoding.Base64;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000E004C File Offset: 0x000DF04C
		internal void SetContent(Stream stream, string name, string mimeType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (mimeType != null && mimeType != string.Empty)
			{
				this.contentType = new ContentType(mimeType);
			}
			if (name != null && name != string.Empty)
			{
				base.ContentType.Name = name;
			}
			this.SetContent(stream);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000E00A6 File Offset: 0x000DF0A6
		internal void SetContent(Stream stream, ContentType contentType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.contentType = contentType;
			this.SetContent(stream);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000E00C4 File Offset: 0x000DF0C4
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			if (mimePartContext.completed)
			{
				throw e;
			}
			try
			{
				if (mimePartContext.outputStream != null)
				{
					mimePartContext.outputStream.Close();
				}
			}
			catch (Exception ex)
			{
				if (e == null)
				{
					e = ex;
				}
			}
			catch
			{
				if (e == null)
				{
					e = new Exception(SR.GetString("net_nonClsCompliantException"));
				}
			}
			mimePartContext.completed = true;
			mimePartContext.result.InvokeCallback(e);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000E014C File Offset: 0x000DF14C
		internal void ReadCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ReadCallbackHandler(result);
			}
			catch (Exception e)
			{
				this.Complete(result, e);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000E01BC File Offset: 0x000DF1BC
		internal void ReadCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext.bytesLeft = this.Stream.EndRead(result);
			if (mimePartContext.bytesLeft > 0)
			{
				IAsyncResult asyncResult = mimePartContext.outputStream.BeginWrite(mimePartContext.buffer, 0, mimePartContext.bytesLeft, this.writeCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.WriteCallbackHandler(asyncResult);
					return;
				}
			}
			else
			{
				this.Complete(result, null);
			}
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000E0228 File Offset: 0x000DF228
		internal void WriteCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.WriteCallbackHandler(result);
			}
			catch (Exception e)
			{
				this.Complete(result, e);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000E0298 File Offset: 0x000DF298
		internal void WriteCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext.outputStream.EndWrite(result);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext.buffer, 0, mimePartContext.buffer.Length, this.readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000E02F0 File Offset: 0x000DF2F0
		internal Stream GetEncodedStream(Stream stream)
		{
			Stream result = stream;
			if (this.TransferEncoding == TransferEncoding.Base64)
			{
				result = new Base64Stream(result);
			}
			else if (this.TransferEncoding == TransferEncoding.QuotedPrintable)
			{
				result = new QuotedPrintableStream(result, true);
			}
			else if (this.TransferEncoding == TransferEncoding.SevenBit)
			{
				result = new SevenBitStream(result);
			}
			return result;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000E0334 File Offset: 0x000DF334
		internal void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			Stream stream = mimePartContext.writer.EndGetContentStream(result);
			mimePartContext.outputStream = this.GetEncodedStream(stream);
			this.readCallback = new AsyncCallback(this.ReadCallback);
			this.writeCallback = new AsyncCallback(this.WriteCallback);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext.buffer, 0, mimePartContext.buffer.Length, this.readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000E03BC File Offset: 0x000DF3BC
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception e)
			{
				this.Complete(result, e);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000E042C File Offset: 0x000DF42C
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, object state)
		{
			writer.WriteHeaders(base.Headers);
			MimeBasePart.MimePartAsyncResult result = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimePart.MimePartContext state2 = new MimePart.MimePartContext(writer, result);
			this.ResetStream();
			this.streamUsedOnce = true;
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), state2);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return result;
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000E0488 File Offset: 0x000DF488
		internal override void Send(BaseWriter writer)
		{
			if (this.Stream != null)
			{
				byte[] buffer = new byte[17408];
				writer.WriteHeaders(base.Headers);
				Stream stream = writer.GetContentStream();
				stream = this.GetEncodedStream(stream);
				this.ResetStream();
				this.streamUsedOnce = true;
				int count;
				while ((count = this.Stream.Read(buffer, 0, 17408)) > 0)
				{
					stream.Write(buffer, 0, count);
				}
				stream.Close();
			}
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000E04F8 File Offset: 0x000DF4F8
		internal void ResetStream()
		{
			if (!this.streamUsedOnce)
			{
				return;
			}
			if (this.Stream.CanSeek)
			{
				this.Stream.Seek(0L, SeekOrigin.Begin);
				this.streamUsedOnce = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("MimePartCantResetStream"));
		}

		// Token: 0x04003073 RID: 12403
		private const int maxBufferSize = 17408;

		// Token: 0x04003074 RID: 12404
		private Stream stream;

		// Token: 0x04003075 RID: 12405
		private bool streamSet;

		// Token: 0x04003076 RID: 12406
		private bool streamUsedOnce;

		// Token: 0x04003077 RID: 12407
		private AsyncCallback readCallback;

		// Token: 0x04003078 RID: 12408
		private AsyncCallback writeCallback;

		// Token: 0x020006AD RID: 1709
		internal class MimePartContext
		{
			// Token: 0x060034CB RID: 13515 RVA: 0x000E0536 File Offset: 0x000DF536
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result)
			{
				this.writer = writer;
				this.result = result;
				this.buffer = new byte[17408];
			}

			// Token: 0x04003079 RID: 12409
			internal Stream outputStream;

			// Token: 0x0400307A RID: 12410
			internal LazyAsyncResult result;

			// Token: 0x0400307B RID: 12411
			internal int bytesLeft;

			// Token: 0x0400307C RID: 12412
			internal BaseWriter writer;

			// Token: 0x0400307D RID: 12413
			internal byte[] buffer;

			// Token: 0x0400307E RID: 12414
			internal bool completed;

			// Token: 0x0400307F RID: 12415
			internal bool completedSynchronously = true;
		}
	}
}
