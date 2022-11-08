using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004E7 RID: 1255
	internal class HttpResponseStreamAsyncResult : LazyAsyncResult
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x000A1DF0 File Offset: 0x000A0DF0
		internal ushort dataChunkCount
		{
			get
			{
				return (ushort)this.m_DataChunks.Length;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x000A1DFB File Offset: 0x000A0DFB
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* pDataChunks
		{
			get
			{
				return (UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_DataChunks, 0));
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000A1E0E File Offset: 0x000A0E0E
		internal HttpResponseStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback) : base(asyncObject, userState, callback)
		{
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000A1E1C File Offset: 0x000A0E1C
		internal unsafe HttpResponseStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, byte[] buffer, int offset, int size, bool chunked, bool sentHeaders) : base(asyncObject, userState, callback)
		{
			this.m_SentHeaders = sentHeaders;
			Overlapped overlapped = new Overlapped();
			overlapped.AsyncResult = this;
			this.m_DataChunks = new UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[chunked ? 3 : 1];
			object[] array = new object[1 + this.m_DataChunks.Length];
			array[this.m_DataChunks.Length] = this.m_DataChunks;
			int num = 0;
			byte[] array2 = null;
			if (chunked)
			{
				array2 = ConnectStream.GetChunkHeader(size, out num);
				this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[0].BufferLength = (uint)(array2.Length - num);
				array[0] = array2;
				this.m_DataChunks[1] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[1].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[1].BufferLength = (uint)size;
				array[1] = buffer;
				this.m_DataChunks[2] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[2].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[2].BufferLength = (uint)NclConstants.CRLF.Length;
				array[2] = NclConstants.CRLF;
			}
			else
			{
				this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[0].BufferLength = (uint)size;
				array[0] = buffer;
			}
			this.m_pOverlapped = overlapped.Pack(HttpResponseStreamAsyncResult.s_IOCallback, array);
			if (chunked)
			{
				this.m_DataChunks[0].pBuffer = (byte*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(array2, num));
				this.m_DataChunks[1].pBuffer = (byte*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset));
				this.m_DataChunks[2].pBuffer = (byte*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(NclConstants.CRLF, 0));
				return;
			}
			this.m_DataChunks[0].pBuffer = (byte*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset));
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000A2020 File Offset: 0x000A1020
		private unsafe static void Callback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = overlapped.AsyncResult as HttpResponseStreamAsyncResult;
			object result = null;
			try
			{
				if (errorCode != 0U && errorCode != 38U)
				{
					httpResponseStreamAsyncResult.ErrorCode = (int)errorCode;
					result = new HttpListenerException((int)errorCode);
				}
				else
				{
					result = ((httpResponseStreamAsyncResult.m_DataChunks.Length == 1) ? httpResponseStreamAsyncResult.m_DataChunks[0].BufferLength : 0U);
					if (Logging.On)
					{
						for (int i = 0; i < httpResponseStreamAsyncResult.m_DataChunks.Length; i++)
						{
							Logging.Dump(Logging.HttpListener, httpResponseStreamAsyncResult, "Callback", (IntPtr)((void*)httpResponseStreamAsyncResult.m_DataChunks[0].pBuffer), (int)httpResponseStreamAsyncResult.m_DataChunks[0].BufferLength);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = ex;
			}
			catch
			{
				result = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			httpResponseStreamAsyncResult.InvokeCallback(result);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000A2110 File Offset: 0x000A1110
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this.m_pOverlapped != null)
			{
				Overlapped.Free(this.m_pOverlapped);
			}
		}

		// Token: 0x0400269A RID: 9882
		internal unsafe NativeOverlapped* m_pOverlapped;

		// Token: 0x0400269B RID: 9883
		private UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[] m_DataChunks;

		// Token: 0x0400269C RID: 9884
		internal bool m_SentHeaders;

		// Token: 0x0400269D RID: 9885
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpResponseStreamAsyncResult.Callback);
	}
}
