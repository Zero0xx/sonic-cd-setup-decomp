using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x020005CB RID: 1483
	internal class BaseOverlappedAsyncResult : ContextAwareResult
	{
		// Token: 0x06002E93 RID: 11923 RVA: 0x000CD7C9 File Offset: 0x000CC7C9
		internal BaseOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback) : base(socket, asyncState, asyncCallback)
		{
			this.m_UseOverlappedIO = (Socket.UseOverlappedIO || socket.UseOnlyOverlappedIO);
			if (this.m_UseOverlappedIO)
			{
				this.m_CleanupCount = 1;
				return;
			}
			this.m_CleanupCount = 2;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000CD801 File Offset: 0x000CC801
		internal BaseOverlappedAsyncResult(Socket socket) : base(socket, null, null)
		{
			this.m_CleanupCount = 1;
			this.m_DisableOverlapped = true;
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000CD81A File Offset: 0x000CC81A
		internal virtual object PostCompletion(int numBytes)
		{
			return numBytes;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000CD824 File Offset: 0x000CC824
		internal void SetUnmanagedStructures(object objectsToPin)
		{
			if (!this.m_DisableOverlapped)
			{
				object[] array = null;
				bool alreadyTriedCast = false;
				bool flag = false;
				if (this.m_Cache != null)
				{
					if (objectsToPin == null && this.m_Cache.PinnedObjects == null)
					{
						flag = true;
					}
					else if (this.m_Cache.PinnedObjects != null)
					{
						if (this.m_Cache.PinnedObjectsArray == null)
						{
							if (objectsToPin == this.m_Cache.PinnedObjects)
							{
								flag = true;
							}
						}
						else if (objectsToPin != null)
						{
							alreadyTriedCast = true;
							array = (objectsToPin as object[]);
							if (array != null && array.Length == 0)
							{
								array = null;
							}
							if (array != null && array.Length == this.m_Cache.PinnedObjectsArray.Length)
							{
								flag = true;
								for (int i = 0; i < array.Length; i++)
								{
									if (array[i] != this.m_Cache.PinnedObjectsArray[i])
									{
										flag = false;
										break;
									}
								}
							}
						}
					}
				}
				if (!flag && this.m_Cache != null)
				{
					this.m_Cache.Free();
					this.m_Cache = null;
				}
				Socket socket = (Socket)base.AsyncObject;
				if (this.m_UseOverlappedIO)
				{
					this.m_UnmanagedBlob = SafeOverlappedFree.Alloc(socket.SafeHandle);
					this.PinUnmanagedObjects(objectsToPin);
					this.m_OverlappedEvent = new AutoResetEvent(false);
					Marshal.WriteIntPtr(this.m_UnmanagedBlob.DangerousGetHandle(), Win32.OverlappedhEventOffset, this.m_OverlappedEvent.SafeWaitHandle.DangerousGetHandle());
					return;
				}
				socket.BindToCompletionPort();
				if (this.m_Cache == null)
				{
					if (array != null)
					{
						this.m_Cache = new OverlappedCache(new Overlapped(), array, BaseOverlappedAsyncResult.s_IOCallback);
					}
					else
					{
						this.m_Cache = new OverlappedCache(new Overlapped(), objectsToPin, BaseOverlappedAsyncResult.s_IOCallback, alreadyTriedCast);
					}
				}
				this.m_Cache.Overlapped.AsyncResult = this;
			}
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000CD9AD File Offset: 0x000CC9AD
		protected void SetupCache(ref OverlappedCache overlappedCache)
		{
			if (!this.m_UseOverlappedIO && !this.m_DisableOverlapped)
			{
				this.m_Cache = ((overlappedCache == null) ? null : Interlocked.Exchange<OverlappedCache>(ref overlappedCache, null));
				this.m_CleanupCount++;
			}
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000CD9E4 File Offset: 0x000CC9E4
		protected void PinUnmanagedObjects(object objectsToPin)
		{
			if (this.m_Cache != null)
			{
				this.m_Cache.Free();
				this.m_Cache = null;
			}
			if (objectsToPin != null)
			{
				if (objectsToPin.GetType() == typeof(object[]))
				{
					object[] array = (object[])objectsToPin;
					this.m_GCHandles = new GCHandle[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							this.m_GCHandles[i] = GCHandle.Alloc(array[i], GCHandleType.Pinned);
						}
					}
					return;
				}
				this.m_GCHandles = new GCHandle[1];
				this.m_GCHandles[0] = GCHandle.Alloc(objectsToPin, GCHandleType.Pinned);
			}
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000CDA88 File Offset: 0x000CCA88
		internal void ExtractCache(ref OverlappedCache overlappedCache)
		{
			if (!this.m_UseOverlappedIO && !this.m_DisableOverlapped)
			{
				OverlappedCache overlappedCache2 = (this.m_Cache == null) ? null : Interlocked.Exchange<OverlappedCache>(ref this.m_Cache, null);
				if (overlappedCache2 != null)
				{
					if (overlappedCache == null)
					{
						overlappedCache = overlappedCache2;
					}
					else
					{
						OverlappedCache overlappedCache3 = Interlocked.Exchange<OverlappedCache>(ref overlappedCache, overlappedCache2);
						if (overlappedCache3 != null)
						{
							overlappedCache3.Free();
						}
					}
				}
				this.ReleaseUnmanagedStructures();
			}
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000CDAE0 File Offset: 0x000CCAE0
		private unsafe static void CompletionPortCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			BaseOverlappedAsyncResult baseOverlappedAsyncResult = (BaseOverlappedAsyncResult)overlapped.AsyncResult;
			overlapped.AsyncResult = null;
			SocketError socketError = (SocketError)errorCode;
			if (socketError != SocketError.Success && socketError != SocketError.OperationAborted)
			{
				Socket socket = baseOverlappedAsyncResult.AsyncObject as Socket;
				if (socket == null)
				{
					socketError = SocketError.NotSocket;
				}
				else if (socket.CleanedUp)
				{
					socketError = SocketError.OperationAborted;
				}
				else
				{
					try
					{
						SocketFlags socketFlags;
						if (!UnsafeNclNativeMethods.OSSOCK.WSAGetOverlappedResult(socket.SafeHandle, baseOverlappedAsyncResult.m_Cache.NativeOverlapped, out numBytes, false, out socketFlags))
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
						}
					}
					catch (ObjectDisposedException)
					{
						socketError = SocketError.OperationAborted;
					}
				}
			}
			baseOverlappedAsyncResult.ErrorCode = (int)socketError;
			object result = baseOverlappedAsyncResult.PostCompletion((int)numBytes);
			baseOverlappedAsyncResult.ReleaseUnmanagedStructures();
			baseOverlappedAsyncResult.InvokeCallback(result);
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000CDBA0 File Offset: 0x000CCBA0
		private void OverlappedCallback(object stateObject, bool Signaled)
		{
			BaseOverlappedAsyncResult baseOverlappedAsyncResult = (BaseOverlappedAsyncResult)stateObject;
			uint num = (uint)Marshal.ReadInt32(IntPtrHelper.Add(baseOverlappedAsyncResult.m_UnmanagedBlob.DangerousGetHandle(), 0));
			uint numBytes = (uint)((num != 0U) ? -1 : Marshal.ReadInt32(IntPtrHelper.Add(baseOverlappedAsyncResult.m_UnmanagedBlob.DangerousGetHandle(), Win32.OverlappedInternalHighOffset)));
			baseOverlappedAsyncResult.ErrorCode = (int)num;
			object result = baseOverlappedAsyncResult.PostCompletion((int)numBytes);
			baseOverlappedAsyncResult.ReleaseUnmanagedStructures();
			baseOverlappedAsyncResult.InvokeCallback(result);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000CDC08 File Offset: 0x000CCC08
		internal SocketError CheckAsyncCallOverlappedResult(SocketError errorCode)
		{
			if (this.m_UseOverlappedIO)
			{
				if (errorCode == SocketError.Success || errorCode == SocketError.IOPending)
				{
					ThreadPool.UnsafeRegisterWaitForSingleObject(this.m_OverlappedEvent, new WaitOrTimerCallback(this.OverlappedCallback), this, -1, true);
					return SocketError.Success;
				}
				base.ErrorCode = (int)errorCode;
				base.Result = -1;
				this.ReleaseUnmanagedStructures();
			}
			else
			{
				this.ReleaseUnmanagedStructures();
				if (errorCode == SocketError.Success || errorCode == SocketError.IOPending)
				{
					return SocketError.Success;
				}
				base.ErrorCode = (int)errorCode;
				base.Result = -1;
				if (this.m_Cache != null)
				{
					this.m_Cache.Overlapped.AsyncResult = null;
				}
				this.ReleaseUnmanagedStructures();
			}
			return errorCode;
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06002E9D RID: 11933 RVA: 0x000CDCAC File Offset: 0x000CCCAC
		internal SafeHandle OverlappedHandle
		{
			get
			{
				if (this.m_UseOverlappedIO)
				{
					if (this.m_UnmanagedBlob != null && !this.m_UnmanagedBlob.IsInvalid)
					{
						return this.m_UnmanagedBlob;
					}
					return SafeOverlappedFree.Zero;
				}
				else
				{
					if (this.m_Cache != null)
					{
						return this.m_Cache.NativeOverlapped;
					}
					return SafeNativeOverlapped.Zero;
				}
			}
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000CDCFC File Offset: 0x000CCCFC
		private void ReleaseUnmanagedStructures()
		{
			if (Interlocked.Decrement(ref this.m_CleanupCount) == 0)
			{
				this.ForceReleaseUnmanagedStructures();
			}
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000CDD11 File Offset: 0x000CCD11
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this.m_CleanupCount > 0 && Interlocked.Exchange(ref this.m_CleanupCount, 0) > 0)
			{
				this.ForceReleaseUnmanagedStructures();
			}
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000CDD38 File Offset: 0x000CCD38
		protected virtual void ForceReleaseUnmanagedStructures()
		{
			this.ReleaseGCHandles();
			GC.SuppressFinalize(this);
			if (this.m_UnmanagedBlob != null && !this.m_UnmanagedBlob.IsInvalid)
			{
				this.m_UnmanagedBlob.Close(true);
				this.m_UnmanagedBlob = null;
			}
			OverlappedCache.InterlockedFree(ref this.m_Cache);
			if (this.m_OverlappedEvent != null)
			{
				this.m_OverlappedEvent.Close();
				this.m_OverlappedEvent = null;
			}
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000CDDA0 File Offset: 0x000CCDA0
		~BaseOverlappedAsyncResult()
		{
			this.ReleaseGCHandles();
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000CDDCC File Offset: 0x000CCDCC
		private void ReleaseGCHandles()
		{
			GCHandle[] gchandles = this.m_GCHandles;
			if (gchandles != null)
			{
				for (int i = 0; i < gchandles.Length; i++)
				{
					if (gchandles[i].IsAllocated)
					{
						gchandles[i].Free();
					}
				}
			}
		}

		// Token: 0x04002C31 RID: 11313
		private SafeOverlappedFree m_UnmanagedBlob;

		// Token: 0x04002C32 RID: 11314
		private AutoResetEvent m_OverlappedEvent;

		// Token: 0x04002C33 RID: 11315
		private int m_CleanupCount;

		// Token: 0x04002C34 RID: 11316
		private bool m_DisableOverlapped;

		// Token: 0x04002C35 RID: 11317
		private bool m_UseOverlappedIO;

		// Token: 0x04002C36 RID: 11318
		private GCHandle[] m_GCHandles;

		// Token: 0x04002C37 RID: 11319
		private OverlappedCache m_Cache;

		// Token: 0x04002C38 RID: 11320
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(BaseOverlappedAsyncResult.CompletionPortCallback);
	}
}
