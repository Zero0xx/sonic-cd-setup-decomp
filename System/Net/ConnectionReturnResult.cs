using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004C6 RID: 1222
	internal class ConnectionReturnResult
	{
		// Token: 0x060025AC RID: 9644 RVA: 0x00096106 File Offset: 0x00095106
		internal ConnectionReturnResult()
		{
			this.m_Context = new List<ConnectionReturnResult.RequestContext>(5);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x0009611A File Offset: 0x0009511A
		internal ConnectionReturnResult(int capacity)
		{
			this.m_Context = new List<ConnectionReturnResult.RequestContext>(capacity);
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x0009612E File Offset: 0x0009512E
		internal bool IsNotEmpty
		{
			get
			{
				return this.m_Context.Count != 0;
			}
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00096141 File Offset: 0x00095141
		internal static void Add(ref ConnectionReturnResult returnResult, HttpWebRequest request, CoreResponseData coreResponseData)
		{
			if (coreResponseData == null)
			{
				throw new InternalException();
			}
			if (returnResult == null)
			{
				returnResult = new ConnectionReturnResult();
			}
			returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(request, coreResponseData));
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x0009616A File Offset: 0x0009516A
		internal static void AddExceptionRange(ref ConnectionReturnResult returnResult, HttpWebRequest[] requests, Exception exception)
		{
			ConnectionReturnResult.AddExceptionRange(ref returnResult, requests, exception, exception);
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00096178 File Offset: 0x00095178
		internal static void AddExceptionRange(ref ConnectionReturnResult returnResult, HttpWebRequest[] requests, Exception exception, Exception firstRequestException)
		{
			if (exception == null)
			{
				throw new InternalException();
			}
			if (returnResult == null)
			{
				returnResult = new ConnectionReturnResult(requests.Length);
			}
			for (int i = 0; i < requests.Length; i++)
			{
				if (i == 0)
				{
					returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(requests[i], firstRequestException));
				}
				else
				{
					returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(requests[i], exception));
				}
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000961DC File Offset: 0x000951DC
		internal static void SetResponses(ConnectionReturnResult returnResult)
		{
			if (returnResult == null)
			{
				return;
			}
			for (int i = 0; i < returnResult.m_Context.Count; i++)
			{
				try
				{
					HttpWebRequest request = returnResult.m_Context[i].Request;
					request.SetAndOrProcessResponse(returnResult.m_Context[i].CoreResponse);
				}
				catch (Exception)
				{
					returnResult.m_Context.RemoveRange(0, i + 1);
					if (returnResult.m_Context.Count > 0)
					{
						ThreadPool.UnsafeQueueUserWorkItem(ConnectionReturnResult.s_InvokeConnectionCallback, returnResult);
					}
					throw;
				}
			}
			returnResult.m_Context.Clear();
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00096278 File Offset: 0x00095278
		private static void InvokeConnectionCallback(object objectReturnResult)
		{
			ConnectionReturnResult responses = (ConnectionReturnResult)objectReturnResult;
			ConnectionReturnResult.SetResponses(responses);
		}

		// Token: 0x04002570 RID: 9584
		private static readonly WaitCallback s_InvokeConnectionCallback = new WaitCallback(ConnectionReturnResult.InvokeConnectionCallback);

		// Token: 0x04002571 RID: 9585
		private List<ConnectionReturnResult.RequestContext> m_Context;

		// Token: 0x020004C7 RID: 1223
		private struct RequestContext
		{
			// Token: 0x060025B5 RID: 9653 RVA: 0x000962A5 File Offset: 0x000952A5
			internal RequestContext(HttpWebRequest request, object coreResponse)
			{
				this.Request = request;
				this.CoreResponse = coreResponse;
			}

			// Token: 0x04002572 RID: 9586
			internal HttpWebRequest Request;

			// Token: 0x04002573 RID: 9587
			internal object CoreResponse;
		}
	}
}
