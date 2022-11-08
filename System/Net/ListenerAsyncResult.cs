using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004E3 RID: 1251
	internal class ListenerAsyncResult : LazyAsyncResult
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x000A0A67 File Offset: 0x0009FA67
		internal static IOCompletionCallback IOCallback
		{
			get
			{
				return ListenerAsyncResult.s_IOCallback;
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000A0A6E File Offset: 0x0009FA6E
		internal ListenerAsyncResult(object asyncObject, object userState, AsyncCallback callback) : base(asyncObject, userState, callback)
		{
			this.m_RequestContext = new AsyncRequestContext(this);
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000A0A88 File Offset: 0x0009FA88
		private unsafe static void WaitCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)overlapped.AsyncResult;
			object obj = null;
			try
			{
				if (errorCode != 0U && errorCode != 234U)
				{
					listenerAsyncResult.ErrorCode = (int)errorCode;
					obj = new HttpListenerException((int)errorCode);
				}
				else
				{
					HttpListener httpListener = listenerAsyncResult.AsyncObject as HttpListener;
					if (errorCode == 0U)
					{
						bool flag = false;
						try
						{
							obj = httpListener.HandleAuthentication(listenerAsyncResult.m_RequestContext, out flag);
							goto IL_99;
						}
						finally
						{
							if (flag)
							{
								listenerAsyncResult.m_RequestContext = ((obj == null) ? new AsyncRequestContext(listenerAsyncResult) : null);
							}
							else
							{
								listenerAsyncResult.m_RequestContext.Reset(0UL, 0U);
							}
						}
					}
					listenerAsyncResult.m_RequestContext.Reset(listenerAsyncResult.m_RequestContext.RequestBlob->RequestId, numBytes);
					IL_99:
					if (obj == null)
					{
						uint num = listenerAsyncResult.QueueBeginGetContext();
						if (num != 0U && num != 997U)
						{
							obj = new HttpListenerException((int)num);
						}
					}
					if (obj == null)
					{
						return;
					}
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				obj = ex;
			}
			listenerAsyncResult.InvokeCallback(obj);
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000A0B8C File Offset: 0x0009FB8C
		internal unsafe uint QueueBeginGetContext()
		{
			uint num;
			for (;;)
			{
				(base.AsyncObject as HttpListener).EnsureBoundHandle();
				uint size = 0U;
				num = UnsafeNclNativeMethods.HttpApi.HttpReceiveHttpRequest((base.AsyncObject as HttpListener).RequestQueueHandle, this.m_RequestContext.RequestBlob->RequestId, 1U, this.m_RequestContext.RequestBlob, this.m_RequestContext.Size, &size, this.m_RequestContext.NativeOverlapped);
				if (num == 87U && this.m_RequestContext.RequestBlob->RequestId != 0UL)
				{
					this.m_RequestContext.RequestBlob->RequestId = 0UL;
				}
				else
				{
					if (num != 234U)
					{
						break;
					}
					this.m_RequestContext.Reset(this.m_RequestContext.RequestBlob->RequestId, size);
				}
			}
			return num;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000A0C52 File Offset: 0x0009FC52
		protected override void Cleanup()
		{
			if (this.m_RequestContext != null)
			{
				this.m_RequestContext.ReleasePins();
				this.m_RequestContext.Close();
			}
			base.Cleanup();
		}

		// Token: 0x0400268C RID: 9868
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(ListenerAsyncResult.WaitCallback);

		// Token: 0x0400268D RID: 9869
		private AsyncRequestContext m_RequestContext;
	}
}
