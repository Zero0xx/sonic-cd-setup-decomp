using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003D3 RID: 979
	internal class ListenerClientCertAsyncResult : LazyAsyncResult
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x0007784C File Offset: 0x0007684C
		internal unsafe NativeOverlapped* NativeOverlapped
		{
			get
			{
				return this.m_pOverlapped;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x00077854 File Offset: 0x00076854
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* RequestBlob
		{
			get
			{
				return this.m_MemoryBlob;
			}
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0007785C File Offset: 0x0007685C
		internal ListenerClientCertAsyncResult(object asyncObject, object userState, AsyncCallback callback, uint size) : base(asyncObject, userState, callback)
		{
			this.Reset(size);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x00077870 File Offset: 0x00076870
		internal unsafe void Reset(uint size)
		{
			if (size == this.m_Size)
			{
				return;
			}
			if (this.m_Size != 0U)
			{
				Overlapped.Free(this.m_pOverlapped);
			}
			this.m_Size = size;
			if (size == 0U)
			{
				this.m_pOverlapped = null;
				this.m_MemoryBlob = null;
				this.m_BackingBuffer = null;
				return;
			}
			this.m_BackingBuffer = new byte[checked((int)size)];
			this.m_pOverlapped = new Overlapped
			{
				AsyncResult = this
			}.Pack(ListenerClientCertAsyncResult.s_IOCallback, this.m_BackingBuffer);
			this.m_MemoryBlob = (UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_BackingBuffer, 0));
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x00077904 File Offset: 0x00076904
		private unsafe static void WaitCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			ListenerClientCertAsyncResult listenerClientCertAsyncResult = (ListenerClientCertAsyncResult)overlapped.AsyncResult;
			HttpListenerRequest httpListenerRequest = (HttpListenerRequest)listenerClientCertAsyncResult.AsyncObject;
			object result = null;
			try
			{
				if (errorCode == 234U)
				{
					UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* requestBlob = listenerClientCertAsyncResult.RequestBlob;
					listenerClientCertAsyncResult.Reset(numBytes + requestBlob->CertEncodedSize);
					uint num = 0U;
					errorCode = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(httpListenerRequest.HttpListenerContext.RequestQueueHandle, httpListenerRequest.m_ConnectionId, 0U, listenerClientCertAsyncResult.m_MemoryBlob, listenerClientCertAsyncResult.m_Size, &num, listenerClientCertAsyncResult.m_pOverlapped);
					if (errorCode == 997U || errorCode == 0U)
					{
						return;
					}
				}
				if (errorCode != 0U)
				{
					listenerClientCertAsyncResult.ErrorCode = (int)errorCode;
					result = new HttpListenerException((int)errorCode);
				}
				else
				{
					UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* memoryBlob = listenerClientCertAsyncResult.m_MemoryBlob;
					if (memoryBlob != null)
					{
						if (memoryBlob->pCertEncoded != null)
						{
							try
							{
								byte[] array = new byte[memoryBlob->CertEncodedSize];
								Marshal.Copy((IntPtr)((void*)memoryBlob->pCertEncoded), array, 0, array.Length);
								result = (httpListenerRequest.ClientCertificate = new X509Certificate2(array));
							}
							catch (CryptographicException ex)
							{
								result = ex;
							}
							catch (SecurityException ex2)
							{
								result = ex2;
							}
						}
						httpListenerRequest.SetClientCertificateError((int)memoryBlob->CertFlags);
					}
				}
			}
			catch (Exception ex3)
			{
				if (NclUtilities.IsFatal(ex3))
				{
					throw;
				}
				result = ex3;
			}
			finally
			{
				if (errorCode != 997U)
				{
					httpListenerRequest.ClientCertState = ListenerClientCertState.Completed;
				}
			}
			listenerClientCertAsyncResult.InvokeCallback(result);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x00077A78 File Offset: 0x00076A78
		protected override void Cleanup()
		{
			if (this.m_pOverlapped != null)
			{
				this.m_MemoryBlob = null;
				Overlapped.Free(this.m_pOverlapped);
				this.m_pOverlapped = null;
			}
			GC.SuppressFinalize(this);
			base.Cleanup();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x00077AAC File Offset: 0x00076AAC
		~ListenerClientCertAsyncResult()
		{
			if (this.m_pOverlapped != null && !NclUtilities.HasShutdownStarted)
			{
				Overlapped.Free(this.m_pOverlapped);
				this.m_pOverlapped = null;
			}
		}

		// Token: 0x04001E82 RID: 7810
		private unsafe NativeOverlapped* m_pOverlapped;

		// Token: 0x04001E83 RID: 7811
		private byte[] m_BackingBuffer;

		// Token: 0x04001E84 RID: 7812
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* m_MemoryBlob;

		// Token: 0x04001E85 RID: 7813
		private uint m_Size;

		// Token: 0x04001E86 RID: 7814
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(ListenerClientCertAsyncResult.WaitCallback);
	}
}
