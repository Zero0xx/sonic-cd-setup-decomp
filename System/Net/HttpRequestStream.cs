using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004E4 RID: 1252
	internal class HttpRequestStream : Stream
	{
		// Token: 0x060026EA RID: 9962 RVA: 0x000A0C8B File Offset: 0x0009FC8B
		internal HttpRequestStream(HttpListenerContext httpContext)
		{
			this.m_HttpContext = httpContext;
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x000A0C9A File Offset: 0x0009FC9A
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x000A0C9D File Offset: 0x0009FC9D
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x000A0CA0 File Offset: 0x0009FCA0
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000A0CA3 File Offset: 0x0009FCA3
		public override void Flush()
		{
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x000A0CA5 File Offset: 0x0009FCA5
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000A0CB6 File Offset: 0x0009FCB6
		// (set) Token: 0x060026F1 RID: 9969 RVA: 0x000A0CC7 File Offset: 0x0009FCC7
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000A0CD8 File Offset: 0x0009FCD8
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000A0CE9 File Offset: 0x0009FCE9
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000A0CFC File Offset: 0x0009FCFC
		public unsafe override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Read", "");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Read", "dataRead:0");
				}
				return 0;
			}
			uint num = 0U;
			if (this.m_DataChunkIndex != -1)
			{
				num = UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
			}
			if (this.m_DataChunkIndex == -1 && (ulong)num < (ulong)((long)size))
			{
				uint num2 = 0U;
				offset += (int)num;
				size -= (int)num;
				if (size > 131072)
				{
					size = 131072;
				}
				uint num3;
				fixed (byte* ptr = buffer)
				{
					num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, 1U, (void*)((byte*)ptr + offset), (uint)size, &num2, null);
					num += num2;
				}
				if (num3 != 0U && num3 != 38U)
				{
					Exception ex = new HttpListenerException((int)num3);
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "Read", ex);
					}
					throw ex;
				}
				this.UpdateAfterRead(num3, num);
			}
			if (Logging.On)
			{
				Logging.Dump(Logging.HttpListener, this, "Read", buffer, offset, (int)num);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Read", "dataRead:" + num);
			}
			return (int)num;
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000A0EB8 File Offset: 0x0009FEB8
		private void UpdateAfterRead(uint statusCode, uint dataRead)
		{
			if (statusCode == 38U || dataRead == 0U)
			{
				this.Close();
			}
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000A0EC8 File Offset: 0x0009FEC8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public unsafe override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "BeginRead", "");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "BeginRead", "");
				}
				HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback);
				httpRequestStreamAsyncResult.InvokeCallback(0U);
				return httpRequestStreamAsyncResult;
			}
			HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult2 = null;
			uint num = 0U;
			if (this.m_DataChunkIndex != -1)
			{
				num = UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
				if (this.m_DataChunkIndex != -1 && (ulong)num == (ulong)((long)size))
				{
					httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, buffer, offset, (uint)size, 0U);
					httpRequestStreamAsyncResult2.InvokeCallback(num);
				}
			}
			if (this.m_DataChunkIndex == -1 && (ulong)num < (ulong)((long)size))
			{
				uint num2 = 0U;
				offset += (int)num;
				size -= (int)num;
				if (size > 131072)
				{
					size = 131072;
				}
				httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, buffer, offset, (uint)size, num);
				try
				{
					if (buffer != null)
					{
						int num3 = buffer.Length;
					}
					this.m_HttpContext.EnsureBoundHandle();
					num2 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, 1U, httpRequestStreamAsyncResult2.m_pPinnedBuffer, (uint)size, null, httpRequestStreamAsyncResult2.m_pOverlapped);
				}
				catch (Exception e)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "BeginRead", e);
					}
					httpRequestStreamAsyncResult2.InternalCleanup();
					throw;
				}
				if (num2 != 0U && num2 != 997U)
				{
					if (num2 == 38U)
					{
						httpRequestStreamAsyncResult2.m_pOverlapped->InternalLow = IntPtr.Zero;
					}
					httpRequestStreamAsyncResult2.InternalCleanup();
					if (num2 != 38U)
					{
						Exception ex = new HttpListenerException((int)num2);
						if (Logging.On)
						{
							Logging.Exception(Logging.HttpListener, this, "BeginRead", ex);
						}
						httpRequestStreamAsyncResult2.InternalCleanup();
						throw ex;
					}
					httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, num);
					httpRequestStreamAsyncResult2.InvokeCallback(0U);
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "BeginRead", "");
			}
			return httpRequestStreamAsyncResult2;
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000A111C File Offset: 0x000A011C
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndRead", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = asyncResult as HttpRequestStream.HttpRequestStreamAsyncResult;
			if (httpRequestStreamAsyncResult == null || httpRequestStreamAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (httpRequestStreamAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndRead"
				}));
			}
			httpRequestStreamAsyncResult.EndCalled = true;
			object obj = httpRequestStreamAsyncResult.InternalWaitForCompletion();
			Exception ex = obj as Exception;
			if (ex != null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndRead", ex);
				}
				throw ex;
			}
			uint num = (uint)obj;
			this.UpdateAfterRead((uint)httpRequestStreamAsyncResult.ErrorCode, num);
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "EndRead", "");
			}
			return (int)(num + httpRequestStreamAsyncResult.m_dataAlreadyRead);
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000A1210 File Offset: 0x000A0210
		public override void Write(byte[] buffer, int offset, int size)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000A1221 File Offset: 0x000A0221
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000A1232 File Offset: 0x000A0232
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000A1244 File Offset: 0x000A0244
		protected override void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Dispose", "");
			}
			try
			{
				this.m_Closed = true;
			}
			finally
			{
				base.Dispose(disposing);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Dispose", "");
			}
		}

		// Token: 0x0400268E RID: 9870
		private const int MaxReadSize = 131072;

		// Token: 0x0400268F RID: 9871
		private HttpListenerContext m_HttpContext;

		// Token: 0x04002690 RID: 9872
		private uint m_DataChunkOffset;

		// Token: 0x04002691 RID: 9873
		private int m_DataChunkIndex;

		// Token: 0x04002692 RID: 9874
		private bool m_Closed;

		// Token: 0x020004E5 RID: 1253
		private class HttpRequestStreamAsyncResult : LazyAsyncResult
		{
			// Token: 0x060026FC RID: 9980 RVA: 0x000A12AC File Offset: 0x000A02AC
			internal HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback) : base(asyncObject, userState, callback)
			{
			}

			// Token: 0x060026FD RID: 9981 RVA: 0x000A12B7 File Offset: 0x000A02B7
			internal HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, uint dataAlreadyRead) : base(asyncObject, userState, callback)
			{
				this.m_dataAlreadyRead = dataAlreadyRead;
			}

			// Token: 0x060026FE RID: 9982 RVA: 0x000A12CC File Offset: 0x000A02CC
			internal unsafe HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, byte[] buffer, int offset, uint size, uint dataAlreadyRead) : base(asyncObject, userState, callback)
			{
				this.m_dataAlreadyRead = dataAlreadyRead;
				this.m_pOverlapped = new Overlapped
				{
					AsyncResult = this
				}.Pack(HttpRequestStream.HttpRequestStreamAsyncResult.s_IOCallback, buffer);
				this.m_pPinnedBuffer = (void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
			}

			// Token: 0x060026FF RID: 9983 RVA: 0x000A1320 File Offset: 0x000A0320
			private unsafe static void Callback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
				HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = overlapped.AsyncResult as HttpRequestStream.HttpRequestStreamAsyncResult;
				object result = null;
				try
				{
					if (errorCode != 0U && errorCode != 38U)
					{
						httpRequestStreamAsyncResult.ErrorCode = (int)errorCode;
						result = new HttpListenerException((int)errorCode);
					}
					else
					{
						result = numBytes;
						if (Logging.On)
						{
							Logging.Dump(Logging.HttpListener, httpRequestStreamAsyncResult, "Callback", (IntPtr)httpRequestStreamAsyncResult.m_pPinnedBuffer, (int)numBytes);
						}
					}
				}
				catch (Exception ex)
				{
					result = ex;
				}
				httpRequestStreamAsyncResult.InvokeCallback(result);
			}

			// Token: 0x06002700 RID: 9984 RVA: 0x000A13A4 File Offset: 0x000A03A4
			protected override void Cleanup()
			{
				base.Cleanup();
				if (this.m_pOverlapped != null)
				{
					Overlapped.Free(this.m_pOverlapped);
				}
			}

			// Token: 0x04002693 RID: 9875
			internal unsafe NativeOverlapped* m_pOverlapped;

			// Token: 0x04002694 RID: 9876
			internal unsafe void* m_pPinnedBuffer;

			// Token: 0x04002695 RID: 9877
			internal uint m_dataAlreadyRead;

			// Token: 0x04002696 RID: 9878
			private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpRequestStream.HttpRequestStreamAsyncResult.Callback);
		}
	}
}
