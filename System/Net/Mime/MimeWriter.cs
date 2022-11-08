using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020006AE RID: 1710
	internal class MimeWriter : BaseWriter
	{
		// Token: 0x060034CC RID: 13516 RVA: 0x000E0563 File Offset: 0x000DF563
		internal MimeWriter(Stream stream, string boundary) : this(stream, boundary, null, MimeWriter.DefaultLineLength)
		{
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000E0574 File Offset: 0x000DF574
		internal MimeWriter(Stream stream, string boundary, string preface, int lineLength)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (boundary == null)
			{
				throw new ArgumentNullException("boundary");
			}
			if (lineLength < 40)
			{
				throw new ArgumentOutOfRangeException("lineLength", lineLength, SR.GetString("MailWriterLineLengthTooSmall"));
			}
			this.stream = stream;
			this.lineLength = lineLength;
			this.onCloseHandler = new EventHandler(this.OnClose);
			this.boundaryBytes = Encoding.ASCII.GetBytes(boundary);
			this.preface = preface;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000E0610 File Offset: 0x000DF610
		internal IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			this.Close(multiAsyncResult);
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000E0634 File Offset: 0x000DF634
		internal void EndClose(IAsyncResult result)
		{
			MultiAsyncResult.End(result);
			this.stream.Close();
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000E0648 File Offset: 0x000DF648
		internal override void Close()
		{
			this.Close(null);
			this.stream.Close();
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000E065C File Offset: 0x000DF65C
		private void Close(MultiAsyncResult multiResult)
		{
			this.bufferBuilder.Append(MimeWriter.CRLF);
			this.bufferBuilder.Append(MimeWriter.DASHDASH);
			this.bufferBuilder.Append(this.boundaryBytes);
			this.bufferBuilder.Append(MimeWriter.DASHDASH);
			this.bufferBuilder.Append(MimeWriter.CRLF);
			this.Flush(multiResult);
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000E06C4 File Offset: 0x000DF6C4
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

		// Token: 0x060034D3 RID: 13523 RVA: 0x000E06FE File Offset: 0x000DF6FE
		internal override IAsyncResult BeginGetContentStream(AsyncCallback callback, object state)
		{
			return this.BeginGetContentStream(ContentTransferEncoding.SevenBit, callback, state);
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000E070C File Offset: 0x000DF70C
		internal override Stream EndGetContentStream(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (Stream)obj;
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000E0735 File Offset: 0x000DF735
		internal Stream GetContentStream(ContentTransferEncoding contentTransferEncoding)
		{
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.isInContent = true;
			return this.GetContentStream(contentTransferEncoding, null);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000E075E File Offset: 0x000DF75E
		internal override Stream GetContentStream()
		{
			return this.GetContentStream(ContentTransferEncoding.SevenBit);
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000E0768 File Offset: 0x000DF768
		private Stream GetContentStream(ContentTransferEncoding contentTransferEncoding, MultiAsyncResult multiResult)
		{
			this.CheckBoundary();
			this.bufferBuilder.Append(MimeWriter.CRLF);
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

		// Token: 0x060034D8 RID: 13528 RVA: 0x000E07E0 File Offset: 0x000DF7E0
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
			this.CheckBoundary();
			this.bufferBuilder.Append(name);
			this.bufferBuilder.Append(": ");
			this.WriteAndFold(value, name.Length + 2);
			this.bufferBuilder.Append(MimeWriter.CRLF);
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000E0864 File Offset: 0x000DF864
		internal override void WriteHeaders(NameValueCollection headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			foreach (object obj in headers)
			{
				string name = (string)obj;
				this.WriteHeader(name, headers[name]);
			}
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000E08D0 File Offset: 0x000DF8D0
		private void OnClose(object sender, EventArgs args)
		{
			if (this.contentStream != sender)
			{
				return;
			}
			this.contentStream.Flush();
			this.contentStream = null;
			this.writeBoundary = true;
			this.isInContent = false;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000E08FC File Offset: 0x000DF8FC
		private void CheckBoundary()
		{
			if (this.preface != null)
			{
				this.bufferBuilder.Append(this.preface);
				this.preface = null;
			}
			if (this.writeBoundary)
			{
				this.bufferBuilder.Append(MimeWriter.CRLF);
				this.bufferBuilder.Append(MimeWriter.DASHDASH);
				this.bufferBuilder.Append(this.boundaryBytes);
				this.bufferBuilder.Append(MimeWriter.CRLF);
				this.writeBoundary = false;
			}
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000E097C File Offset: 0x000DF97C
		private static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				MimeWriter mimeWriter = (MimeWriter)multiAsyncResult.Context;
				try
				{
					mimeWriter.stream.EndWrite(result);
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

		// Token: 0x060034DD RID: 13533 RVA: 0x000E09FC File Offset: 0x000DF9FC
		private void Flush(MultiAsyncResult multiResult)
		{
			if (this.bufferBuilder.Length > 0)
			{
				if (multiResult != null)
				{
					multiResult.Enter();
					IAsyncResult asyncResult = this.stream.BeginWrite(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length, MimeWriter.onWrite, multiResult);
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

		// Token: 0x060034DE RID: 13534 RVA: 0x000E0A94 File Offset: 0x000DFA94
		private void WriteAndFold(string value, int startLength)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (num != value.Length)
			{
				if (value[num] == ' ' || value[num] == '\t')
				{
					if (num - num3 >= this.lineLength - startLength)
					{
						startLength = 0;
						if (num2 == num3)
						{
							num2 = num;
						}
						this.bufferBuilder.Append(value, num3, num2 - num3);
						this.bufferBuilder.Append(MimeWriter.CRLF);
						num3 = num2;
					}
					num2 = num;
				}
				num++;
			}
			if (num - num3 > 0)
			{
				this.bufferBuilder.Append(value, num3, num - num3);
				return;
			}
		}

		// Token: 0x04003080 RID: 12416
		private static int DefaultLineLength = 78;

		// Token: 0x04003081 RID: 12417
		private static byte[] DASHDASH = new byte[]
		{
			45,
			45
		};

		// Token: 0x04003082 RID: 12418
		private static byte[] CRLF = new byte[]
		{
			13,
			10
		};

		// Token: 0x04003083 RID: 12419
		private byte[] boundaryBytes;

		// Token: 0x04003084 RID: 12420
		private BufferBuilder bufferBuilder = new BufferBuilder();

		// Token: 0x04003085 RID: 12421
		private Stream contentStream;

		// Token: 0x04003086 RID: 12422
		private bool isInContent;

		// Token: 0x04003087 RID: 12423
		private int lineLength;

		// Token: 0x04003088 RID: 12424
		private EventHandler onCloseHandler;

		// Token: 0x04003089 RID: 12425
		private Stream stream;

		// Token: 0x0400308A RID: 12426
		private bool writeBoundary = true;

		// Token: 0x0400308B RID: 12427
		private string preface;

		// Token: 0x0400308C RID: 12428
		private static AsyncCallback onWrite = new AsyncCallback(MimeWriter.OnWrite);
	}
}
