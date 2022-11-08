using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020006A9 RID: 1705
	internal class MimeMultiPart : MimeBasePart
	{
		// Token: 0x060034A7 RID: 13479 RVA: 0x000DF965 File Offset: 0x000DE965
		internal MimeMultiPart(MimeMultiPartType type)
		{
			this.MimeMultiPartType = type;
		}

		// Token: 0x17000C53 RID: 3155
		// (set) Token: 0x060034A8 RID: 13480 RVA: 0x000DF974 File Offset: 0x000DE974
		internal MimeMultiPartType MimeMultiPartType
		{
			set
			{
				if (value > MimeMultiPartType.Related || value < MimeMultiPartType.Mixed)
				{
					throw new NotSupportedException(value.ToString());
				}
				this.SetType(value);
			}
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000DF996 File Offset: 0x000DE996
		private void SetType(MimeMultiPartType type)
		{
			base.ContentType.MediaType = "multipart/" + type.ToString().ToLower(CultureInfo.InvariantCulture);
			base.ContentType.Boundary = this.GetNextBoundary();
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x000DF9D3 File Offset: 0x000DE9D3
		internal Collection<MimeBasePart> Parts
		{
			get
			{
				if (this.parts == null)
				{
					this.parts = new Collection<MimeBasePart>();
				}
				return this.parts;
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000DF9F0 File Offset: 0x000DE9F0
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			if (mimePartContext.completed)
			{
				throw e;
			}
			try
			{
				mimePartContext.outputStream.Close();
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

		// Token: 0x060034AC RID: 13484 RVA: 0x000DFA70 File Offset: 0x000DEA70
		internal void MimeWriterCloseCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.MimeWriterCloseCallbackHandler(result);
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

		// Token: 0x060034AD RID: 13485 RVA: 0x000DFAE0 File Offset: 0x000DEAE0
		private void MimeWriterCloseCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			((MimeWriter)mimePartContext.writer).EndClose(result);
			this.Complete(result, null);
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000DFB14 File Offset: 0x000DEB14
		internal void MimePartSentCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.MimePartSentCallbackHandler(result);
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

		// Token: 0x060034AF RID: 13487 RVA: 0x000DFB84 File Offset: 0x000DEB84
		private void MimePartSentCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			MimeBasePart mimeBasePart = mimePartContext.partsEnumerator.Current;
			mimeBasePart.EndSend(result);
			if (mimePartContext.partsEnumerator.MoveNext())
			{
				mimeBasePart = mimePartContext.partsEnumerator.Current;
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext.writer, this.mimePartSentCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext.writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000DFC1C File Offset: 0x000DEC1C
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
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

		// Token: 0x060034B1 RID: 13489 RVA: 0x000DFC8C File Offset: 0x000DEC8C
		private void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			mimePartContext.outputStream = mimePartContext.writer.EndGetContentStream(result);
			mimePartContext.writer = new MimeWriter(mimePartContext.outputStream, base.ContentType.Boundary);
			if (mimePartContext.partsEnumerator.MoveNext())
			{
				MimeBasePart mimeBasePart = mimePartContext.partsEnumerator.Current;
				this.mimePartSentCallback = new AsyncCallback(this.MimePartSentCallback);
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext.writer, this.mimePartSentCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext.writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000DFD50 File Offset: 0x000DED50
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, object state)
		{
			writer.WriteHeaders(base.Headers);
			MimeBasePart.MimePartAsyncResult result = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimeMultiPart.MimePartContext state2 = new MimeMultiPart.MimePartContext(writer, result, this.Parts.GetEnumerator());
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), state2);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return result;
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000DFDAC File Offset: 0x000DEDAC
		internal override void Send(BaseWriter writer)
		{
			writer.WriteHeaders(base.Headers);
			Stream contentStream = writer.GetContentStream();
			MimeWriter mimeWriter = new MimeWriter(contentStream, base.ContentType.Boundary);
			foreach (MimeBasePart mimeBasePart in this.Parts)
			{
				mimeBasePart.Send(mimeWriter);
			}
			mimeWriter.Close();
			contentStream.Close();
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000DFE2C File Offset: 0x000DEE2C
		internal string GetNextBoundary()
		{
			string result = "--boundary_" + MimeMultiPart.boundary.ToString(CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString(null, CultureInfo.InvariantCulture);
			MimeMultiPart.boundary++;
			return result;
		}

		// Token: 0x04003064 RID: 12388
		private Collection<MimeBasePart> parts;

		// Token: 0x04003065 RID: 12389
		private static int boundary;

		// Token: 0x04003066 RID: 12390
		private AsyncCallback mimePartSentCallback;

		// Token: 0x020006AA RID: 1706
		internal class MimePartContext
		{
			// Token: 0x060034B5 RID: 13493 RVA: 0x000DFE78 File Offset: 0x000DEE78
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result, IEnumerator<MimeBasePart> partsEnumerator)
			{
				this.writer = writer;
				this.result = result;
				this.partsEnumerator = partsEnumerator;
			}

			// Token: 0x04003067 RID: 12391
			internal IEnumerator<MimeBasePart> partsEnumerator;

			// Token: 0x04003068 RID: 12392
			internal Stream outputStream;

			// Token: 0x04003069 RID: 12393
			internal LazyAsyncResult result;

			// Token: 0x0400306A RID: 12394
			internal BaseWriter writer;

			// Token: 0x0400306B RID: 12395
			internal bool completed;

			// Token: 0x0400306C RID: 12396
			internal bool completedSynchronously = true;
		}
	}
}
