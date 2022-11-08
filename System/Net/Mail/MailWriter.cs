using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x0200069F RID: 1695
	internal class MailWriter : BaseWriter
	{
		// Token: 0x06003468 RID: 13416 RVA: 0x000DE770 File Offset: 0x000DD770
		internal MailWriter(Stream stream) : this(stream, MailWriter.DefaultLineLength)
		{
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x000DE780 File Offset: 0x000DD780
		internal MailWriter(Stream stream, int lineLength)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (lineLength < 0)
			{
				throw new ArgumentOutOfRangeException("lineLength");
			}
			this.stream = stream;
			this.lineLength = lineLength;
			this.onCloseHandler = new EventHandler(this.OnClose);
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000DE7DB File Offset: 0x000DD7DB
		internal override void Close()
		{
			this.stream.Write(MailWriter.CRLF, 0, 2);
			this.stream.Close();
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000DE7FC File Offset: 0x000DD7FC
		internal IAsyncResult BeginGetContentStream(ContentTransferEncoding contentTransferEncoding, AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			Stream result = this.GetContentStream(contentTransferEncoding, multiAsyncResult);
			if (!(multiAsyncResult.Result is Exception))
			{
				multiAsyncResult.Result = result;
			}
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000DE836 File Offset: 0x000DD836
		internal override IAsyncResult BeginGetContentStream(AsyncCallback callback, object state)
		{
			return this.BeginGetContentStream(ContentTransferEncoding.SevenBit, callback, state);
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000DE844 File Offset: 0x000DD844
		internal override Stream EndGetContentStream(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (Stream)obj;
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000DE86D File Offset: 0x000DD86D
		internal Stream GetContentStream(ContentTransferEncoding contentTransferEncoding)
		{
			return this.GetContentStream(contentTransferEncoding, null);
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000DE877 File Offset: 0x000DD877
		internal override Stream GetContentStream()
		{
			return this.GetContentStream(ContentTransferEncoding.SevenBit);
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000DE880 File Offset: 0x000DD880
		private Stream GetContentStream(ContentTransferEncoding contentTransferEncoding, MultiAsyncResult multiResult)
		{
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.isInContent = true;
			this.bufferBuilder.Append(MailWriter.CRLF);
			this.Flush(multiResult);
			Stream stream = this.stream;
			if (contentTransferEncoding == ContentTransferEncoding.SevenBit)
			{
				stream = new SevenBitStream(stream);
			}
			else if (contentTransferEncoding == ContentTransferEncoding.QuotedPrintable)
			{
				stream = new QuotedPrintableStream(stream, this.lineLength);
			}
			else if (contentTransferEncoding == ContentTransferEncoding.Base64)
			{
				stream = new Base64Stream(stream, this.lineLength);
			}
			ClosableStream result = new ClosableStream(stream, this.onCloseHandler);
			this.contentStream = result;
			return result;
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000DE910 File Offset: 0x000DD910
		internal override void WriteHeader(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.bufferBuilder.Append(name);
			this.bufferBuilder.Append(": ");
			this.WriteAndFold(value);
			this.bufferBuilder.Append(MailWriter.CRLF);
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000DE984 File Offset: 0x000DD984
		internal override void WriteHeaders(NameValueCollection headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			foreach (object obj in headers)
			{
				string name = (string)obj;
				string[] values = headers.GetValues(name);
				foreach (string value in values)
				{
					this.WriteHeader(name, value);
				}
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000DEA28 File Offset: 0x000DDA28
		private void OnClose(object sender, EventArgs args)
		{
			this.contentStream.Flush();
			this.contentStream = null;
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000DEA3C File Offset: 0x000DDA3C
		private static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				MailWriter mailWriter = (MailWriter)multiAsyncResult.Context;
				try
				{
					mailWriter.stream.EndWrite(result);
					multiAsyncResult.Leave();
				}
				catch (Exception result2)
				{
					multiAsyncResult.Leave(result2);
				}
				catch
				{
					multiAsyncResult.Leave(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x000DEABC File Offset: 0x000DDABC
		private void Flush(MultiAsyncResult multiResult)
		{
			if (this.bufferBuilder.Length > 0)
			{
				if (multiResult != null)
				{
					multiResult.Enter();
					IAsyncResult asyncResult = this.stream.BeginWrite(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length, MailWriter.onWrite, multiResult);
					if (asyncResult.CompletedSynchronously)
					{
						this.stream.EndWrite(asyncResult);
						multiResult.Leave();
					}
				}
				else
				{
					this.stream.Write(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length);
				}
				this.bufferBuilder.Reset();
			}
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000DEB54 File Offset: 0x000DDB54
		private void WriteAndFold(string value)
		{
			if (value.Length < MailWriter.DefaultLineLength)
			{
				this.bufferBuilder.Append(value);
				return;
			}
			int num = 0;
			int length = value.Length;
			while (length - num > MailWriter.DefaultLineLength)
			{
				int num2 = value.LastIndexOf(' ', num + MailWriter.DefaultLineLength - 1, MailWriter.DefaultLineLength - 1);
				if (num2 > -1)
				{
					this.bufferBuilder.Append(value, num, num2 - num);
					this.bufferBuilder.Append(MailWriter.CRLF);
					num = num2;
				}
				else
				{
					this.bufferBuilder.Append(value, num, MailWriter.DefaultLineLength);
					num += MailWriter.DefaultLineLength;
				}
			}
			if (num < length)
			{
				this.bufferBuilder.Append(value, num, length - num);
			}
		}

		// Token: 0x04003039 RID: 12345
		private static byte[] CRLF = new byte[]
		{
			13,
			10
		};

		// Token: 0x0400303A RID: 12346
		private static int DefaultLineLength = 78;

		// Token: 0x0400303B RID: 12347
		private Stream contentStream;

		// Token: 0x0400303C RID: 12348
		private bool isInContent;

		// Token: 0x0400303D RID: 12349
		private int lineLength;

		// Token: 0x0400303E RID: 12350
		private EventHandler onCloseHandler;

		// Token: 0x0400303F RID: 12351
		private Stream stream;

		// Token: 0x04003040 RID: 12352
		private BufferBuilder bufferBuilder = new BufferBuilder();

		// Token: 0x04003041 RID: 12353
		private static AsyncCallback onWrite = new AsyncCallback(MailWriter.OnWrite);
	}
}
