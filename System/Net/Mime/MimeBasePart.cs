using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020006A7 RID: 1703
	internal class MimeBasePart
	{
		// Token: 0x06003495 RID: 13461 RVA: 0x000DF46B File Offset: 0x000DE46B
		internal MimeBasePart()
		{
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000DF473 File Offset: 0x000DE473
		internal static bool ShouldUseBase64Encoding(Encoding encoding)
		{
			return encoding == Encoding.Unicode || encoding == Encoding.UTF8 || encoding == Encoding.UTF32 || encoding == Encoding.BigEndianUnicode;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000DF498 File Offset: 0x000DE498
		internal static string EncodeHeaderValue(string value, Encoding encoding, bool base64Encoding)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (encoding == null && MimeBasePart.IsAscii(value, false))
			{
				return value;
			}
			if (encoding == null)
			{
				encoding = Encoding.GetEncoding("utf-8");
			}
			string value2 = encoding.BodyName;
			if (encoding == Encoding.BigEndianUnicode)
			{
				value2 = "utf-16be";
			}
			stringBuilder.Append("=?");
			stringBuilder.Append(value2);
			stringBuilder.Append("?");
			stringBuilder.Append(base64Encoding ? "B" : "Q");
			stringBuilder.Append("?");
			byte[] bytes = encoding.GetBytes(value);
			if (base64Encoding)
			{
				Base64Stream base64Stream = new Base64Stream(-1);
				base64Stream.EncodeBytes(bytes, 0, bytes.Length, true);
				stringBuilder.Append(Encoding.ASCII.GetString(base64Stream.WriteState.Buffer, 0, base64Stream.WriteState.Length));
			}
			else
			{
				QuotedPrintableStream quotedPrintableStream = new QuotedPrintableStream(-1);
				quotedPrintableStream.EncodeBytes(bytes, 0, bytes.Length);
				stringBuilder.Append(Encoding.ASCII.GetString(quotedPrintableStream.WriteState.Buffer, 0, quotedPrintableStream.WriteState.Length));
			}
			stringBuilder.Append("?=");
			return stringBuilder.ToString();
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000DF5BC File Offset: 0x000DE5BC
		internal static string DecodeHeaderValue(string value)
		{
			if (value == null || value.Length == 0)
			{
				return string.Empty;
			}
			string[] array = value.Split(new char[]
			{
				'?'
			});
			if (array.Length != 5 || array[0] != "=" || array[4] != "=")
			{
				return value;
			}
			string name = array[1];
			bool flag = array[2] == "B";
			byte[] bytes = Encoding.ASCII.GetBytes(array[3]);
			int count;
			if (flag)
			{
				Base64Stream base64Stream = new Base64Stream();
				count = base64Stream.DecodeBytes(bytes, 0, bytes.Length);
			}
			else
			{
				QuotedPrintableStream quotedPrintableStream = new QuotedPrintableStream();
				count = quotedPrintableStream.DecodeBytes(bytes, 0, bytes.Length);
			}
			Encoding encoding = Encoding.GetEncoding(name);
			return encoding.GetString(bytes, 0, count);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000DF680 File Offset: 0x000DE680
		internal static Encoding DecodeEncoding(string value)
		{
			if (value == null || value.Length == 0)
			{
				return null;
			}
			string[] array = value.Split(new char[]
			{
				'?'
			});
			if (array.Length != 5 || array[0] != "=" || array[4] != "=")
			{
				return null;
			}
			string name = array[1];
			return Encoding.GetEncoding(name);
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000DF6E0 File Offset: 0x000DE6E0
		internal static bool IsAscii(string value, bool permitCROrLF)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				bool result;
				if (c > '\u007f')
				{
					result = false;
				}
				else
				{
					if (permitCROrLF || (c != '\r' && c != '\n'))
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000DF734 File Offset: 0x000DE734
		internal static bool IsAnsi(string value, bool permitCROrLF)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				bool result;
				if (c > 'ÿ')
				{
					result = false;
				}
				else
				{
					if (permitCROrLF || (c != '\r' && c != '\n'))
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600349C RID: 13468 RVA: 0x000DF78A File Offset: 0x000DE78A
		// (set) Token: 0x0600349D RID: 13469 RVA: 0x000DF79D File Offset: 0x000DE79D
		internal string ContentID
		{
			get
			{
				return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentID));
					return;
				}
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)] = value;
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600349E RID: 13470 RVA: 0x000DF7CB File Offset: 0x000DE7CB
		// (set) Token: 0x0600349F RID: 13471 RVA: 0x000DF7DE File Offset: 0x000DE7DE
		internal string ContentLocation
		{
			get
			{
				return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentLocation));
					return;
				}
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)] = value;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x060034A0 RID: 13472 RVA: 0x000DF80C File Offset: 0x000DE80C
		internal NameValueCollection Headers
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new HeaderCollection();
				}
				if (this.contentType == null)
				{
					this.contentType = new ContentType();
				}
				this.contentType.PersistIfNeeded(this.headers, false);
				if (this.contentDisposition != null)
				{
					this.contentDisposition.PersistIfNeeded(this.headers, false);
				}
				return this.headers;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x060034A1 RID: 13473 RVA: 0x000DF871 File Offset: 0x000DE871
		// (set) Token: 0x060034A2 RID: 13474 RVA: 0x000DF88C File Offset: 0x000DE88C
		internal ContentType ContentType
		{
			get
			{
				if (this.contentType == null)
				{
					this.contentType = new ContentType();
				}
				return this.contentType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.contentType = value;
				this.contentType.PersistIfNeeded((HeaderCollection)this.Headers, true);
			}
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000DF8BA File Offset: 0x000DE8BA
		internal virtual void Send(BaseWriter writer)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000DF8C1 File Offset: 0x000DE8C1
		internal virtual IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000DF8C8 File Offset: 0x000DE8C8
		internal void EndSend(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as MimeBasePart.MimePartAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndSend"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			if (lazyAsyncResult.Result is Exception)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
		}

		// Token: 0x04003060 RID: 12384
		internal const string defaultCharSet = "utf-8";

		// Token: 0x04003061 RID: 12385
		protected ContentType contentType;

		// Token: 0x04003062 RID: 12386
		protected ContentDisposition contentDisposition;

		// Token: 0x04003063 RID: 12387
		private HeaderCollection headers;

		// Token: 0x020006A8 RID: 1704
		internal class MimePartAsyncResult : LazyAsyncResult
		{
			// Token: 0x060034A6 RID: 13478 RVA: 0x000DF95A File Offset: 0x000DE95A
			internal MimePartAsyncResult(MimeBasePart part, object state, AsyncCallback callback) : base(part, state, callback)
			{
			}
		}
	}
}
