using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Net
{
	// Token: 0x020003CE RID: 974
	public sealed class HttpListenerContext
	{
		// Token: 0x06001EC0 RID: 7872 RVA: 0x000772C8 File Offset: 0x000762C8
		internal unsafe HttpListenerContext(HttpListener httpListener, RequestContextBase memoryBlob)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "httpListener#" + ValidationHelper.HashString(httpListener) + " requestBlob=" + ValidationHelper.HashString((IntPtr)((void*)memoryBlob.RequestBlob)));
			}
			this.m_Listener = httpListener;
			this.m_Request = new HttpListenerRequest(this, memoryBlob);
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00077330 File Offset: 0x00076330
		internal void SetIdentity(IPrincipal principal, string mutualAuthentication)
		{
			this.m_MutualAuthentication = mutualAuthentication;
			this.m_User = principal;
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x00077340 File Offset: 0x00076340
		public HttpListenerRequest Request
		{
			get
			{
				return this.m_Request;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x00077348 File Offset: 0x00076348
		public HttpListenerResponse Response
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "Response", "");
				}
				if (this.m_Response == null)
				{
					this.m_Response = new HttpListenerResponse(this);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Response", "");
				}
				return this.m_Response;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x000773A7 File Offset: 0x000763A7
		public IPrincipal User
		{
			get
			{
				if (!(this.m_User is WindowsPrincipal))
				{
					return this.m_User;
				}
				new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
				return this.m_User;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000773D2 File Offset: 0x000763D2
		internal bool PromoteCookiesToRfc2965
		{
			get
			{
				return this.m_PromoteCookiesToRfc2965;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000773DA File Offset: 0x000763DA
		internal string MutualAuthentication
		{
			get
			{
				return this.m_MutualAuthentication;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000773E2 File Offset: 0x000763E2
		internal HttpListener Listener
		{
			get
			{
				return this.m_Listener;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000773EA File Offset: 0x000763EA
		internal SafeCloseHandle RequestQueueHandle
		{
			get
			{
				return this.m_Listener.RequestQueueHandle;
			}
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000773F7 File Offset: 0x000763F7
		internal void EnsureBoundHandle()
		{
			this.m_Listener.EnsureBoundHandle();
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x00077404 File Offset: 0x00076404
		internal ulong RequestId
		{
			get
			{
				return this.Request.RequestId;
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00077414 File Offset: 0x00076414
		internal void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close()", "");
			}
			try
			{
				if (this.m_Response != null)
				{
					this.m_Response.Close();
				}
			}
			finally
			{
				try
				{
					this.m_Request.Close();
				}
				finally
				{
					IDisposable disposable = (this.m_User == null) ? null : (this.m_User.Identity as IDisposable);
					if (disposable != null && this.m_User.Identity.AuthenticationType != "NTLM" && !this.m_Listener.UnsafeConnectionNtlmAuthentication)
					{
						disposable.Dispose();
					}
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Close", "");
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000774E8 File Offset: 0x000764E8
		internal void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Abort", "");
			}
			HttpListenerContext.CancelRequest(this.RequestQueueHandle, this.m_Request.RequestId);
			try
			{
				this.m_Request.Close();
			}
			finally
			{
				IDisposable disposable = (this.m_User == null) ? null : (this.m_User.Identity as IDisposable);
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Abort", "");
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00077588 File Offset: 0x00076588
		internal UnsafeNclNativeMethods.HttpApi.HTTP_VERB GetKnownMethod()
		{
			return UnsafeNclNativeMethods.HttpApi.GetKnownVerb(this.Request.RequestBuffer, this.Request.OriginalBlobAddress);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000775A8 File Offset: 0x000765A8
		internal unsafe static void CancelRequest(SafeCloseHandle requestQueueHandle, ulong requestId)
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK http_DATA_CHUNK = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
			http_DATA_CHUNK.DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
			http_DATA_CHUNK.pBuffer = (byte*)(&http_DATA_CHUNK);
			UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(requestQueueHandle, requestId, 1U, 1, &http_DATA_CHUNK, null, SafeLocalFree.Zero, 0U, null, null);
		}

		// Token: 0x04001E75 RID: 7797
		internal const string NTLM = "NTLM";

		// Token: 0x04001E76 RID: 7798
		private HttpListener m_Listener;

		// Token: 0x04001E77 RID: 7799
		private HttpListenerRequest m_Request;

		// Token: 0x04001E78 RID: 7800
		private HttpListenerResponse m_Response;

		// Token: 0x04001E79 RID: 7801
		private IPrincipal m_User;

		// Token: 0x04001E7A RID: 7802
		private string m_MutualAuthentication;

		// Token: 0x04001E7B RID: 7803
		private bool m_PromoteCookiesToRfc2965;
	}
}
