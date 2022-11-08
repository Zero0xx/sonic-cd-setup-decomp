using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x020005BF RID: 1471
	public class SocketAsyncEventArgs : EventArgs, IDisposable
	{
		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002DE7 RID: 11751 RVA: 0x000CA03A File Offset: 0x000C903A
		// (remove) Token: 0x06002DE8 RID: 11752 RVA: 0x000CA053 File Offset: 0x000C9053
		private event EventHandler<SocketAsyncEventArgs> m_Completed;

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000CA06C File Offset: 0x000C906C
		public SocketAsyncEventArgs()
		{
			if (!ComNetOS.IsPostWin2K)
			{
				throw new NotSupportedException(SR.GetString("WinXPRequired"));
			}
			this.m_ExecutionCallback = new ContextCallback(this.ExecutionCallback);
			this.m_SendPacketsSendSize = -1;
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06002DEA RID: 11754 RVA: 0x000CA0A4 File Offset: 0x000C90A4
		// (set) Token: 0x06002DEB RID: 11755 RVA: 0x000CA0AC File Offset: 0x000C90AC
		public Socket AcceptSocket
		{
			get
			{
				return this.m_AcceptSocket;
			}
			set
			{
				this.m_AcceptSocket = value;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06002DEC RID: 11756 RVA: 0x000CA0B5 File Offset: 0x000C90B5
		public byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06002DED RID: 11757 RVA: 0x000CA0BD File Offset: 0x000C90BD
		public int Offset
		{
			get
			{
				return this.m_Offset;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06002DEE RID: 11758 RVA: 0x000CA0C5 File Offset: 0x000C90C5
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06002DEF RID: 11759 RVA: 0x000CA0CD File Offset: 0x000C90CD
		// (set) Token: 0x06002DF0 RID: 11760 RVA: 0x000CA0D8 File Offset: 0x000C90D8
		public IList<ArraySegment<byte>> BufferList
		{
			get
			{
				return this.m_BufferList;
			}
			set
			{
				this.StartConfiguring();
				try
				{
					if (value != null && this.m_Buffer != null)
					{
						throw new ArgumentException(SR.GetString("net_ambiguousbuffers", new object[]
						{
							"Buffer"
						}));
					}
					this.m_BufferList = value;
					this.m_BufferListChanged = true;
					this.CheckPinMultipleBuffers();
				}
				finally
				{
					this.Complete();
				}
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x000CA144 File Offset: 0x000C9144
		public int BytesTransferred
		{
			get
			{
				return this.m_BytesTransferred;
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06002DF2 RID: 11762 RVA: 0x000CA14C File Offset: 0x000C914C
		// (remove) Token: 0x06002DF3 RID: 11763 RVA: 0x000CA16C File Offset: 0x000C916C
		public event EventHandler<SocketAsyncEventArgs> Completed
		{
			add
			{
				this.m_Completed = (EventHandler<SocketAsyncEventArgs>)Delegate.Combine(this.m_Completed, value);
				this.m_CompletedChanged = true;
			}
			remove
			{
				this.m_Completed = (EventHandler<SocketAsyncEventArgs>)Delegate.Remove(this.m_Completed, value);
				this.m_CompletedChanged = true;
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000CA18C File Offset: 0x000C918C
		protected virtual void OnCompleted(SocketAsyncEventArgs e)
		{
			EventHandler<SocketAsyncEventArgs> completed = this.m_Completed;
			if (completed != null)
			{
				completed(e.m_CurrentSocket, e);
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06002DF5 RID: 11765 RVA: 0x000CA1B0 File Offset: 0x000C91B0
		// (set) Token: 0x06002DF6 RID: 11766 RVA: 0x000CA1B8 File Offset: 0x000C91B8
		public bool DisconnectReuseSocket
		{
			get
			{
				return this.m_DisconnectReuseSocket;
			}
			set
			{
				this.m_DisconnectReuseSocket = value;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06002DF7 RID: 11767 RVA: 0x000CA1C1 File Offset: 0x000C91C1
		public SocketAsyncOperation LastOperation
		{
			get
			{
				return this.m_CompletedOperation;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06002DF8 RID: 11768 RVA: 0x000CA1C9 File Offset: 0x000C91C9
		public IPPacketInformation ReceiveMessageFromPacketInfo
		{
			get
			{
				return this.m_ReceiveMessageFromPacketInfo;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06002DF9 RID: 11769 RVA: 0x000CA1D1 File Offset: 0x000C91D1
		// (set) Token: 0x06002DFA RID: 11770 RVA: 0x000CA1D9 File Offset: 0x000C91D9
		public EndPoint RemoteEndPoint
		{
			get
			{
				return this.m_RemoteEndPoint;
			}
			set
			{
				this.m_RemoteEndPoint = value;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06002DFB RID: 11771 RVA: 0x000CA1E2 File Offset: 0x000C91E2
		// (set) Token: 0x06002DFC RID: 11772 RVA: 0x000CA1EC File Offset: 0x000C91EC
		public SendPacketsElement[] SendPacketsElements
		{
			get
			{
				return this.m_SendPacketsElements;
			}
			set
			{
				this.StartConfiguring();
				try
				{
					this.m_SendPacketsElements = value;
					this.m_SendPacketsElementsInternal = null;
				}
				finally
				{
					this.Complete();
				}
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x000CA228 File Offset: 0x000C9228
		// (set) Token: 0x06002DFE RID: 11774 RVA: 0x000CA230 File Offset: 0x000C9230
		public TransmitFileOptions SendPacketsFlags
		{
			get
			{
				return this.m_SendPacketsFlags;
			}
			set
			{
				this.m_SendPacketsFlags = value;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06002DFF RID: 11775 RVA: 0x000CA239 File Offset: 0x000C9239
		// (set) Token: 0x06002E00 RID: 11776 RVA: 0x000CA241 File Offset: 0x000C9241
		public int SendPacketsSendSize
		{
			get
			{
				return this.m_SendPacketsSendSize;
			}
			set
			{
				this.m_SendPacketsSendSize = value;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000CA24A File Offset: 0x000C924A
		// (set) Token: 0x06002E02 RID: 11778 RVA: 0x000CA252 File Offset: 0x000C9252
		public SocketError SocketError
		{
			get
			{
				return this.m_SocketError;
			}
			set
			{
				this.m_SocketError = value;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x000CA25B File Offset: 0x000C925B
		// (set) Token: 0x06002E04 RID: 11780 RVA: 0x000CA263 File Offset: 0x000C9263
		public SocketFlags SocketFlags
		{
			get
			{
				return this.m_SocketFlags;
			}
			set
			{
				this.m_SocketFlags = value;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06002E05 RID: 11781 RVA: 0x000CA26C File Offset: 0x000C926C
		// (set) Token: 0x06002E06 RID: 11782 RVA: 0x000CA274 File Offset: 0x000C9274
		public object UserToken
		{
			get
			{
				return this.m_UserToken;
			}
			set
			{
				this.m_UserToken = value;
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000CA27D File Offset: 0x000C927D
		public void SetBuffer(byte[] buffer, int offset, int count)
		{
			this.SetBufferInternal(buffer, offset, count);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000CA288 File Offset: 0x000C9288
		public void SetBuffer(int offset, int count)
		{
			this.SetBufferInternal(this.m_Buffer, offset, count);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000CA298 File Offset: 0x000C9298
		private void SetBufferInternal(byte[] buffer, int offset, int count)
		{
			this.StartConfiguring();
			try
			{
				if (buffer == null)
				{
					this.m_Buffer = null;
					this.m_Offset = 0;
					this.m_Count = 0;
				}
				else
				{
					if (this.m_BufferList != null)
					{
						throw new ArgumentException(SR.GetString("net_ambiguousbuffers", new object[]
						{
							"BufferList"
						}));
					}
					if (offset < 0 || offset > buffer.Length)
					{
						throw new ArgumentOutOfRangeException("offset");
					}
					if (count < 0 || count > buffer.Length - offset)
					{
						throw new ArgumentOutOfRangeException("count");
					}
					this.m_Buffer = buffer;
					this.m_Offset = offset;
					this.m_Count = count;
				}
				this.CheckPinSingleBuffer(true);
			}
			finally
			{
				this.Complete();
			}
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000CA350 File Offset: 0x000C9350
		internal void SetResults(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.m_SocketError = socketError;
			this.m_BytesTransferred = bytesTransferred;
			this.m_SocketFlags = flags;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000CA367 File Offset: 0x000C9367
		private void ExecutionCallback(object ignored)
		{
			this.OnCompleted(this);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000CA370 File Offset: 0x000C9370
		internal void Complete()
		{
			this.m_Operating = 0;
			if (this.m_DisposeCalled)
			{
				this.Dispose();
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000CA387 File Offset: 0x000C9387
		public void Dispose()
		{
			this.m_DisposeCalled = true;
			if (Interlocked.CompareExchange(ref this.m_Operating, 2, 0) != 0)
			{
				return;
			}
			this.FreeOverlapped(false);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000CA3B0 File Offset: 0x000C93B0
		~SocketAsyncEventArgs()
		{
			this.FreeOverlapped(true);
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000CA3E0 File Offset: 0x000C93E0
		private void StartConfiguring()
		{
			int num = Interlocked.CompareExchange(ref this.m_Operating, -1, 0);
			if (num == 1 || num == -1)
			{
				throw new InvalidOperationException(SR.GetString("net_socketopinprogress"));
			}
			if (num == 2)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000CA428 File Offset: 0x000C9428
		internal void StartOperationCommon(Socket socket)
		{
			if (Interlocked.CompareExchange(ref this.m_Operating, 1, 0) == 0)
			{
				if (ExecutionContext.IsFlowSuppressed())
				{
					this.m_Context = null;
					this.m_ContextCopy = null;
				}
				else
				{
					if (this.m_CompletedChanged || socket != this.m_CurrentSocket)
					{
						this.m_CompletedChanged = false;
						this.m_Context = null;
						this.m_ContextCopy = null;
					}
					if (this.m_Context == null)
					{
						this.m_Context = ExecutionContext.Capture();
					}
					if (this.m_Context != null)
					{
						this.m_ContextCopy = this.m_Context.CreateCopy();
					}
				}
				this.m_CurrentSocket = socket;
				return;
			}
			if (this.m_DisposeCalled)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			throw new InvalidOperationException(SR.GetString("net_socketopinprogress"));
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000CA4E0 File Offset: 0x000C94E0
		internal void StartOperationAccept()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Accept;
			this.m_AcceptAddressBufferCount = 2 * (this.m_CurrentSocket.m_RightEndPoint.Serialize().Size + 16);
			if (this.m_Buffer != null)
			{
				if (this.m_Count < this.m_AcceptAddressBufferCount)
				{
					throw new ArgumentException(SR.GetString("net_buffercounttoosmall", new object[]
					{
						"Count"
					}));
				}
			}
			else
			{
				if (this.m_AcceptBuffer == null || this.m_AcceptBuffer.Length < this.m_AcceptAddressBufferCount)
				{
					this.m_AcceptBuffer = new byte[this.m_AcceptAddressBufferCount];
				}
				this.CheckPinSingleBuffer(false);
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000CA57A File Offset: 0x000C957A
		internal void StartOperationConnect()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Connect;
			this.PinSocketAddressBuffer();
			this.CheckPinNoBuffer();
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000CA58F File Offset: 0x000C958F
		internal void StartOperationDisconnect()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Disconnect;
			this.CheckPinNoBuffer();
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000CA59E File Offset: 0x000C959E
		internal void StartOperationReceive()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Receive;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000CA5A7 File Offset: 0x000C95A7
		internal void StartOperationReceiveFrom()
		{
			this.m_CompletedOperation = SocketAsyncOperation.ReceiveFrom;
			this.PinSocketAddressBuffer();
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000CA5B8 File Offset: 0x000C95B8
		internal unsafe void StartOperationReceiveMessageFrom()
		{
			this.m_CompletedOperation = SocketAsyncOperation.ReceiveFrom;
			this.PinSocketAddressBuffer();
			if (this.m_WSAMessageBuffer == null)
			{
				this.m_WSAMessageBuffer = new byte[SocketAsyncEventArgs.s_WSAMsgSize];
				this.m_WSAMessageBufferGCHandle = GCHandle.Alloc(this.m_WSAMessageBuffer, GCHandleType.Pinned);
				this.m_PtrWSAMessageBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSAMessageBuffer, 0);
			}
			bool flag = this.m_CurrentSocket.AddressFamily == AddressFamily.InterNetwork;
			bool flag2 = this.m_CurrentSocket.AddressFamily == AddressFamily.InterNetworkV6;
			if (flag && (this.m_ControlBuffer == null || this.m_ControlBuffer.Length != SocketAsyncEventArgs.s_ControlDataSize))
			{
				if (this.m_ControlBufferGCHandle.IsAllocated)
				{
					this.m_ControlBufferGCHandle.Free();
				}
				this.m_ControlBuffer = new byte[SocketAsyncEventArgs.s_ControlDataSize];
			}
			else if (flag2 && (this.m_ControlBuffer == null || this.m_ControlBuffer.Length != SocketAsyncEventArgs.s_ControlDataIPv6Size))
			{
				if (this.m_ControlBufferGCHandle.IsAllocated)
				{
					this.m_ControlBufferGCHandle.Free();
				}
				this.m_ControlBuffer = new byte[SocketAsyncEventArgs.s_ControlDataIPv6Size];
			}
			if (!this.m_ControlBufferGCHandle.IsAllocated)
			{
				this.m_ControlBufferGCHandle = GCHandle.Alloc(this.m_ControlBuffer, GCHandleType.Pinned);
				this.m_PtrControlBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_ControlBuffer, 0);
			}
			if (this.m_Buffer != null)
			{
				if (this.m_WSARecvMsgWSABufferArray == null)
				{
					this.m_WSARecvMsgWSABufferArray = new WSABuffer[1];
				}
				this.m_WSARecvMsgWSABufferArray[0].Pointer = this.m_PtrSingleBuffer;
				this.m_WSARecvMsgWSABufferArray[0].Length = this.m_Count;
				this.m_WSARecvMsgWSABufferArrayGCHandle = GCHandle.Alloc(this.m_WSARecvMsgWSABufferArray, GCHandleType.Pinned);
				this.m_PtrWSARecvMsgWSABufferArray = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSARecvMsgWSABufferArray, 0);
			}
			else
			{
				this.m_WSARecvMsgWSABufferArrayGCHandle = GCHandle.Alloc(this.m_WSABufferArray, GCHandleType.Pinned);
				this.m_PtrWSARecvMsgWSABufferArray = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSABufferArray, 0);
			}
			UnsafeNclNativeMethods.OSSOCK.WSAMsg* ptr = (UnsafeNclNativeMethods.OSSOCK.WSAMsg*)((void*)this.m_PtrWSAMessageBuffer);
			ptr->socketAddress = this.m_PtrSocketAddressBuffer;
			ptr->addressLength = (uint)this.m_SocketAddress.Size;
			ptr->buffers = this.m_PtrWSARecvMsgWSABufferArray;
			if (this.m_Buffer != null)
			{
				ptr->count = 1U;
			}
			else
			{
				ptr->count = (uint)this.m_WSABufferArray.Length;
			}
			if (this.m_ControlBuffer != null)
			{
				ptr->controlBuffer.Pointer = this.m_PtrControlBuffer;
				ptr->controlBuffer.Length = this.m_ControlBuffer.Length;
			}
			ptr->flags = this.m_SocketFlags;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000CA804 File Offset: 0x000C9804
		internal void StartOperationSend()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Send;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000CA810 File Offset: 0x000C9810
		internal void StartOperationSendPackets()
		{
			this.m_CompletedOperation = SocketAsyncOperation.SendPackets;
			if (this.m_SendPacketsElements != null)
			{
				this.m_SendPacketsElementsInternal = (SendPacketsElement[])this.m_SendPacketsElements.Clone();
			}
			this.m_SendPacketsElementsFileCount = 0;
			this.m_SendPacketsElementsBufferCount = 0;
			foreach (SendPacketsElement sendPacketsElement in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement != null)
				{
					if (sendPacketsElement.m_FilePath != null && sendPacketsElement.m_FilePath.Length > 0)
					{
						this.m_SendPacketsElementsFileCount++;
					}
					if (sendPacketsElement.m_Buffer != null)
					{
						this.m_SendPacketsElementsBufferCount++;
					}
				}
			}
			if (this.m_SendPacketsElementsFileCount > 0)
			{
				this.m_SendPacketsFileStreams = new FileStream[this.m_SendPacketsElementsFileCount];
				this.m_SendPacketsFileHandles = new SafeHandle[this.m_SendPacketsElementsFileCount];
				int num = 0;
				foreach (SendPacketsElement sendPacketsElement2 in this.m_SendPacketsElementsInternal)
				{
					if (sendPacketsElement2 != null && sendPacketsElement2.m_FilePath != null && sendPacketsElement2.m_FilePath.Length > 0)
					{
						Exception ex = null;
						try
						{
							this.m_SendPacketsFileStreams[num] = new FileStream(sendPacketsElement2.m_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
						}
						catch (Exception ex2)
						{
							ex = ex2;
						}
						if (ex != null)
						{
							for (int k = 0; k < this.m_SendPacketsElementsFileCount; k++)
							{
								this.m_SendPacketsFileHandles[k] = null;
								if (this.m_SendPacketsFileStreams[k] != null)
								{
									this.m_SendPacketsFileStreams[k].Close();
									this.m_SendPacketsFileStreams[k] = null;
								}
							}
							throw ex;
						}
						ExceptionHelper.UnmanagedPermission.Assert();
						try
						{
							this.m_SendPacketsFileHandles[num] = this.m_SendPacketsFileStreams[num].SafeFileHandle;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						num++;
					}
				}
			}
			this.CheckPinSendPackets();
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000CA9DC File Offset: 0x000C99DC
		internal void StartOperationSendTo()
		{
			this.m_CompletedOperation = SocketAsyncOperation.SendTo;
			this.PinSocketAddressBuffer();
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000CA9EC File Offset: 0x000C99EC
		private void CheckPinNoBuffer()
		{
			if (this.m_PinState == SocketAsyncEventArgs.PinState.None)
			{
				this.SetupOverlappedSingle(true);
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000CAA00 File Offset: 0x000C9A00
		private void CheckPinSingleBuffer(bool pinUsersBuffer)
		{
			if (pinUsersBuffer)
			{
				if (this.m_Buffer == null)
				{
					if (this.m_PinState == SocketAsyncEventArgs.PinState.SingleBuffer)
					{
						this.FreeOverlapped(false);
						return;
					}
				}
				else
				{
					if (this.m_PinState != SocketAsyncEventArgs.PinState.SingleBuffer || this.m_PinnedSingleBuffer != this.m_Buffer)
					{
						this.FreeOverlapped(false);
						this.SetupOverlappedSingle(true);
						return;
					}
					if (this.m_Offset != this.m_PinnedSingleBufferOffset)
					{
						this.m_PinnedSingleBufferOffset = this.m_Offset;
						this.m_PtrSingleBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, this.m_Offset);
						this.m_WSABuffer.Pointer = this.m_PtrSingleBuffer;
					}
					if (this.m_Count != this.m_PinnedSingleBufferCount)
					{
						this.m_PinnedSingleBufferCount = this.m_Count;
						this.m_WSABuffer.Length = this.m_Count;
						return;
					}
				}
			}
			else if (this.m_PinState != SocketAsyncEventArgs.PinState.SingleAcceptBuffer || this.m_PinnedSingleBuffer != this.m_AcceptBuffer)
			{
				this.FreeOverlapped(false);
				this.SetupOverlappedSingle(false);
			}
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000CAAE8 File Offset: 0x000C9AE8
		private void CheckPinMultipleBuffers()
		{
			if (this.m_BufferList == null)
			{
				if (this.m_PinState == SocketAsyncEventArgs.PinState.MultipleBuffer)
				{
					this.FreeOverlapped(false);
					return;
				}
			}
			else if (this.m_PinState != SocketAsyncEventArgs.PinState.MultipleBuffer || this.m_BufferListChanged)
			{
				this.m_BufferListChanged = false;
				this.FreeOverlapped(false);
				try
				{
					this.SetupOverlappedMultiple();
				}
				catch (Exception)
				{
					this.FreeOverlapped(false);
					throw;
				}
			}
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000CAB50 File Offset: 0x000C9B50
		private void CheckPinSendPackets()
		{
			if (this.m_PinState != SocketAsyncEventArgs.PinState.None)
			{
				this.FreeOverlapped(false);
			}
			this.SetupOverlappedSendPackets();
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000CAB68 File Offset: 0x000C9B68
		private void PinSocketAddressBuffer()
		{
			if (this.m_PinnedSocketAddress == this.m_SocketAddress)
			{
				return;
			}
			if (this.m_SocketAddressGCHandle.IsAllocated)
			{
				this.m_SocketAddressGCHandle.Free();
			}
			this.m_SocketAddressGCHandle = GCHandle.Alloc(this.m_SocketAddress.m_Buffer, GCHandleType.Pinned);
			this.m_SocketAddress.CopyAddressSizeIntoBuffer();
			this.m_PtrSocketAddressBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, 0);
			this.m_PtrSocketAddressBufferSize = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, this.m_SocketAddress.GetAddressSizeOffset());
			this.m_PinnedSocketAddress = this.m_SocketAddress;
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000CAC04 File Offset: 0x000C9C04
		private void FreeOverlapped(bool checkForShutdown)
		{
			if (!checkForShutdown || !NclUtilities.HasShutdownStarted)
			{
				if (this.m_PtrNativeOverlapped != null && !this.m_PtrNativeOverlapped.IsInvalid)
				{
					this.m_PtrNativeOverlapped.Dispose();
					this.m_PtrNativeOverlapped = null;
					this.m_Overlapped = null;
					this.m_PinState = SocketAsyncEventArgs.PinState.None;
					this.m_PinnedAcceptBuffer = null;
					this.m_PinnedSingleBuffer = null;
					this.m_PinnedSingleBufferOffset = 0;
					this.m_PinnedSingleBufferCount = 0;
				}
				if (this.m_SocketAddressGCHandle.IsAllocated)
				{
					this.m_SocketAddressGCHandle.Free();
				}
				if (this.m_WSAMessageBufferGCHandle.IsAllocated)
				{
					this.m_WSAMessageBufferGCHandle.Free();
				}
				if (this.m_WSARecvMsgWSABufferArrayGCHandle.IsAllocated)
				{
					this.m_WSARecvMsgWSABufferArrayGCHandle.Free();
				}
				if (this.m_ControlBufferGCHandle.IsAllocated)
				{
					this.m_ControlBufferGCHandle.Free();
				}
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000CACD0 File Offset: 0x000C9CD0
		private void SetupOverlappedSingle(bool pinSingleBuffer)
		{
			this.m_Overlapped = new Overlapped();
			if (!pinSingleBuffer)
			{
				this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_AcceptBuffer));
				this.m_PinnedAcceptBuffer = this.m_AcceptBuffer;
				this.m_PtrAcceptBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_AcceptBuffer, 0);
				this.m_PtrSingleBuffer = IntPtr.Zero;
				this.m_PinState = SocketAsyncEventArgs.PinState.SingleAcceptBuffer;
				return;
			}
			if (this.m_Buffer != null)
			{
				this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_Buffer));
				this.m_PinnedSingleBuffer = this.m_Buffer;
				this.m_PinnedSingleBufferOffset = this.m_Offset;
				this.m_PinnedSingleBufferCount = this.m_Count;
				this.m_PtrSingleBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, this.m_Offset);
				this.m_PtrAcceptBuffer = IntPtr.Zero;
				this.m_WSABuffer.Pointer = this.m_PtrSingleBuffer;
				this.m_WSABuffer.Length = this.m_Count;
				this.m_PinState = SocketAsyncEventArgs.PinState.SingleBuffer;
				return;
			}
			this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), null));
			this.m_PinnedSingleBuffer = null;
			this.m_PinnedSingleBufferOffset = 0;
			this.m_PinnedSingleBufferCount = 0;
			this.m_PtrSingleBuffer = IntPtr.Zero;
			this.m_PtrAcceptBuffer = IntPtr.Zero;
			this.m_WSABuffer.Pointer = this.m_PtrSingleBuffer;
			this.m_WSABuffer.Length = this.m_Count;
			this.m_PinState = SocketAsyncEventArgs.PinState.NoBuffer;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000CAE64 File Offset: 0x000C9E64
		private void SetupOverlappedMultiple()
		{
			ArraySegment<byte>[] array = new ArraySegment<byte>[this.m_BufferList.Count];
			this.m_BufferList.CopyTo(array, 0);
			this.m_Overlapped = new Overlapped();
			if (this.m_ObjectsToPin == null || this.m_ObjectsToPin.Length != array.Length)
			{
				this.m_ObjectsToPin = new object[array.Length];
			}
			for (int i = 0; i < array.Length; i++)
			{
				this.m_ObjectsToPin[i] = array[i].Array;
			}
			if (this.m_WSABufferArray == null || this.m_WSABufferArray.Length != array.Length)
			{
				this.m_WSABufferArray = new WSABuffer[array.Length];
			}
			this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_ObjectsToPin));
			for (int j = 0; j < array.Length; j++)
			{
				ArraySegment<byte> segment = array[j];
				ValidationHelper.ValidateSegment(segment);
				this.m_WSABufferArray[j].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(segment.Array, segment.Offset);
				this.m_WSABufferArray[j].Length = segment.Count;
			}
			this.m_PinState = SocketAsyncEventArgs.PinState.MultipleBuffer;
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000CAF90 File Offset: 0x000C9F90
		private void SetupOverlappedSendPackets()
		{
			this.m_Overlapped = new Overlapped();
			this.m_SendPacketsDescriptor = new UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElement[this.m_SendPacketsElementsFileCount + this.m_SendPacketsElementsBufferCount];
			if (this.m_ObjectsToPin == null || this.m_ObjectsToPin.Length != this.m_SendPacketsElementsBufferCount + 1)
			{
				this.m_ObjectsToPin = new object[this.m_SendPacketsElementsBufferCount + 1];
			}
			this.m_ObjectsToPin[0] = this.m_SendPacketsDescriptor;
			int num = 1;
			foreach (SendPacketsElement sendPacketsElement in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement.m_Buffer != null && sendPacketsElement.m_Count > 0)
				{
					this.m_ObjectsToPin[num] = sendPacketsElement.m_Buffer;
					num++;
				}
			}
			this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_ObjectsToPin));
			this.m_PtrSendPacketsDescriptor = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SendPacketsDescriptor, 0);
			int num2 = 0;
			int num3 = 0;
			foreach (SendPacketsElement sendPacketsElement2 in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement2 != null)
				{
					if (sendPacketsElement2.m_Buffer != null && sendPacketsElement2.m_Count > 0)
					{
						this.m_SendPacketsDescriptor[num2].buffer = Marshal.UnsafeAddrOfPinnedArrayElement(sendPacketsElement2.m_Buffer, sendPacketsElement2.m_Offset);
						this.m_SendPacketsDescriptor[num2].length = (uint)sendPacketsElement2.m_Count;
						this.m_SendPacketsDescriptor[num2].flags = sendPacketsElement2.m_Flags;
						num2++;
					}
					else if (sendPacketsElement2.m_FilePath != null && sendPacketsElement2.m_FilePath.Length != 0)
					{
						this.m_SendPacketsDescriptor[num2].fileHandle = this.m_SendPacketsFileHandles[num3].DangerousGetHandle();
						this.m_SendPacketsDescriptor[num2].fileOffset = (long)sendPacketsElement2.m_Offset;
						this.m_SendPacketsDescriptor[num2].length = (uint)sendPacketsElement2.m_Count;
						this.m_SendPacketsDescriptor[num2].flags = sendPacketsElement2.m_Flags;
						num3++;
						num2++;
					}
				}
			}
			this.m_PinState = SocketAsyncEventArgs.PinState.SendPackets;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000CB1AC File Offset: 0x000CA1AC
		internal void LogBuffer(int size)
		{
			switch (this.m_PinState)
			{
			case SocketAsyncEventArgs.PinState.SingleAcceptBuffer:
				Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation + "Async)", this.m_AcceptBuffer, 0, size);
				return;
			case SocketAsyncEventArgs.PinState.SingleBuffer:
				Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation + "Async)", this.m_Buffer, this.m_Offset, size);
				return;
			case SocketAsyncEventArgs.PinState.MultipleBuffer:
				foreach (WSABuffer wsabuffer in this.m_WSABufferArray)
				{
					Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation + "Async)", wsabuffer.Pointer, Math.Min(wsabuffer.Length, size));
					if ((size -= wsabuffer.Length) <= 0)
					{
						return;
					}
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000CB2B0 File Offset: 0x000CA2B0
		internal void LogSendPacketsBuffers(int size)
		{
			foreach (SendPacketsElement sendPacketsElement in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement != null)
				{
					if (sendPacketsElement.m_Buffer != null && sendPacketsElement.m_Count > 0)
					{
						Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation + "Async)Buffer", sendPacketsElement.m_Buffer, sendPacketsElement.m_Offset, Math.Min(sendPacketsElement.m_Count, size));
					}
					else if (sendPacketsElement.m_FilePath != null && sendPacketsElement.m_FilePath.Length != 0)
					{
						Logging.PrintInfo(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation + "Async)", "Not logging data from file: " + sendPacketsElement.m_FilePath);
					}
				}
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000CB386 File Offset: 0x000CA386
		internal void UpdatePerfCounters(int size, bool sendOp)
		{
			if (sendOp)
			{
				NetworkingPerfCounters.AddBytesSent(size);
				if (this.m_CurrentSocket.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
					return;
				}
			}
			else
			{
				NetworkingPerfCounters.AddBytesReceived(size);
				if (this.m_CurrentSocket.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsReceived();
				}
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000CB3BE File Offset: 0x000CA3BE
		internal void FinishOperationSyncFailure(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(socketError, bytesTransferred, flags);
			this.m_CurrentSocket.UpdateStatusAfterSocketError(socketError);
			this.Complete();
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000CB3DB File Offset: 0x000CA3DB
		internal void FinishOperationAsyncFailure(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(socketError, bytesTransferred, flags);
			this.m_CurrentSocket.UpdateStatusAfterSocketError(socketError);
			this.Complete();
			if (this.m_Context == null)
			{
				this.OnCompleted(this);
				return;
			}
			ExecutionContext.Run(this.m_ContextCopy, this.m_ExecutionCallback, null);
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000CB41C File Offset: 0x000CA41C
		internal unsafe void FinishOperationSuccess(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(socketError, bytesTransferred, flags);
			SocketAddress socketAddress2;
			switch (this.m_CompletedOperation)
			{
			case SocketAsyncOperation.Accept:
			{
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogBuffer(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, false);
					}
				}
				SocketAddress socketAddress = this.m_CurrentSocket.m_RightEndPoint.Serialize();
				IntPtr intPtr;
				int num;
				IntPtr source;
				UnsafeNclNativeMethods.OSSOCK.GetAcceptExSockaddrs((this.m_PtrSingleBuffer != IntPtr.Zero) ? this.m_PtrSingleBuffer : this.m_PtrAcceptBuffer, (this.m_Count != 0) ? (this.m_Count - this.m_AcceptAddressBufferCount) : 0, this.m_AcceptAddressBufferCount / 2, this.m_AcceptAddressBufferCount / 2, out intPtr, out num, out source, out socketAddress.m_Size);
				Marshal.Copy(source, socketAddress.m_Buffer, 0, socketAddress.m_Size);
				try
				{
					IntPtr intPtr2 = this.m_CurrentSocket.SafeHandle.DangerousGetHandle();
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_AcceptSocket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateAcceptContext, ref intPtr2, Marshal.SizeOf(intPtr2));
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.OperationAborted;
				}
				if (socketError == SocketError.Success)
				{
					this.m_AcceptSocket = this.m_CurrentSocket.UpdateAcceptSocket(this.m_AcceptSocket, this.m_CurrentSocket.m_RightEndPoint.Create(socketAddress), false);
					goto IL_4E7;
				}
				this.SetResults(socketError, bytesTransferred, SocketFlags.None);
				this.m_AcceptSocket = null;
				goto IL_4E7;
			}
			case SocketAsyncOperation.Connect:
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogBuffer(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, true);
					}
				}
				try
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_CurrentSocket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateConnectContext, null, 0);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.OperationAborted;
				}
				if (socketError == SocketError.Success)
				{
					this.m_CurrentSocket.SetToConnected();
					goto IL_4E7;
				}
				goto IL_4E7;
			case SocketAsyncOperation.Disconnect:
				this.m_CurrentSocket.SetToDisconnected();
				this.m_CurrentSocket.m_RemoteEndPoint = null;
				goto IL_4E7;
			case SocketAsyncOperation.Receive:
				if (bytesTransferred <= 0)
				{
					goto IL_4E7;
				}
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, false);
					goto IL_4E7;
				}
				goto IL_4E7;
			case SocketAsyncOperation.ReceiveFrom:
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogBuffer(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, false);
					}
				}
				this.m_SocketAddress.SetSize(this.m_PtrSocketAddressBufferSize);
				socketAddress2 = this.m_RemoteEndPoint.Serialize();
				if (socketAddress2.Equals(this.m_SocketAddress))
				{
					goto IL_4E7;
				}
				try
				{
					this.m_RemoteEndPoint = this.m_RemoteEndPoint.Create(this.m_SocketAddress);
					goto IL_4E7;
				}
				catch
				{
					goto IL_4E7;
				}
				break;
			case SocketAsyncOperation.ReceiveMessageFrom:
				break;
			case SocketAsyncOperation.Send:
				if (bytesTransferred <= 0)
				{
					goto IL_4E7;
				}
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, true);
					goto IL_4E7;
				}
				goto IL_4E7;
			case SocketAsyncOperation.SendPackets:
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogSendPacketsBuffers(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, true);
					}
				}
				if (this.m_SendPacketsFileStreams != null)
				{
					for (int i = 0; i < this.m_SendPacketsElementsFileCount; i++)
					{
						this.m_SendPacketsFileHandles[i] = null;
						if (this.m_SendPacketsFileStreams[i] != null)
						{
							this.m_SendPacketsFileStreams[i].Close();
							this.m_SendPacketsFileStreams[i] = null;
						}
					}
				}
				this.m_SendPacketsFileStreams = null;
				this.m_SendPacketsFileHandles = null;
				goto IL_4E7;
			case SocketAsyncOperation.SendTo:
				if (bytesTransferred <= 0)
				{
					goto IL_4E7;
				}
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, true);
					goto IL_4E7;
				}
				goto IL_4E7;
			default:
				goto IL_4E7;
			}
			if (bytesTransferred > 0)
			{
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, false);
				}
			}
			this.m_SocketAddress.SetSize(this.m_PtrSocketAddressBufferSize);
			socketAddress2 = this.m_RemoteEndPoint.Serialize();
			if (!socketAddress2.Equals(this.m_SocketAddress))
			{
				try
				{
					this.m_RemoteEndPoint = this.m_RemoteEndPoint.Create(this.m_SocketAddress);
				}
				catch
				{
				}
			}
			IPAddress ipaddress = null;
			UnsafeNclNativeMethods.OSSOCK.WSAMsg* ptr = (UnsafeNclNativeMethods.OSSOCK.WSAMsg*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSAMessageBuffer, 0));
			if (this.m_ControlBuffer.Length == SocketAsyncEventArgs.s_ControlDataSize)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlData controlData = (UnsafeNclNativeMethods.OSSOCK.ControlData)Marshal.PtrToStructure(ptr->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));
				if (controlData.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress((long)((ulong)controlData.address));
				}
				this.m_ReceiveMessageFromPacketInfo = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.None, (int)controlData.index);
			}
			else if (this.m_ControlBuffer.Length == SocketAsyncEventArgs.s_ControlDataIPv6Size)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6 controlDataIPv = (UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6)Marshal.PtrToStructure(ptr->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));
				if (controlDataIPv.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress(controlDataIPv.address);
				}
				this.m_ReceiveMessageFromPacketInfo = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.IPv6None, (int)controlDataIPv.index);
			}
			else
			{
				this.m_ReceiveMessageFromPacketInfo = default(IPPacketInformation);
			}
			IL_4E7:
			if (socketError != SocketError.Success)
			{
				this.SetResults(socketError, bytesTransferred, flags);
				this.m_CurrentSocket.UpdateStatusAfterSocketError(socketError);
			}
			this.Complete();
			if (this.m_ContextCopy == null)
			{
				this.OnCompleted(this);
				return;
			}
			ExecutionContext.Run(this.m_ContextCopy, this.m_ExecutionCallback, null);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000CB984 File Offset: 0x000CA984
		private unsafe void CompletionPortCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			SocketFlags flags = SocketFlags.None;
			SocketError socketError = (SocketError)errorCode;
			if (socketError == SocketError.Success)
			{
				this.FinishOperationSuccess(socketError, (int)numBytes, flags);
				return;
			}
			if (socketError != SocketError.OperationAborted)
			{
				if (this.m_CurrentSocket.CleanedUp)
				{
					socketError = SocketError.OperationAborted;
				}
				else
				{
					try
					{
						UnsafeNclNativeMethods.OSSOCK.WSAGetOverlappedResult(this.m_CurrentSocket.SafeHandle, this.m_PtrNativeOverlapped, out numBytes, false, out flags);
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
					catch
					{
						socketError = SocketError.OperationAborted;
					}
				}
			}
			this.FinishOperationAsyncFailure(socketError, (int)numBytes, flags);
		}

		// Token: 0x04002B57 RID: 11095
		private const int Configuring = -1;

		// Token: 0x04002B58 RID: 11096
		private const int Free = 0;

		// Token: 0x04002B59 RID: 11097
		private const int InProgress = 1;

		// Token: 0x04002B5A RID: 11098
		private const int Disposed = 2;

		// Token: 0x04002B5B RID: 11099
		internal static readonly int s_ControlDataSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));

		// Token: 0x04002B5C RID: 11100
		internal static readonly int s_ControlDataIPv6Size = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));

		// Token: 0x04002B5D RID: 11101
		internal static readonly int s_WSAMsgSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.WSAMsg));

		// Token: 0x04002B5E RID: 11102
		internal Socket m_AcceptSocket;

		// Token: 0x04002B5F RID: 11103
		internal byte[] m_Buffer;

		// Token: 0x04002B60 RID: 11104
		internal WSABuffer m_WSABuffer;

		// Token: 0x04002B61 RID: 11105
		internal IntPtr m_PtrSingleBuffer;

		// Token: 0x04002B62 RID: 11106
		internal int m_Count;

		// Token: 0x04002B63 RID: 11107
		internal int m_Offset;

		// Token: 0x04002B64 RID: 11108
		internal IList<ArraySegment<byte>> m_BufferList;

		// Token: 0x04002B65 RID: 11109
		private bool m_BufferListChanged;

		// Token: 0x04002B66 RID: 11110
		internal WSABuffer[] m_WSABufferArray;

		// Token: 0x04002B67 RID: 11111
		private int m_BytesTransferred;

		// Token: 0x04002B69 RID: 11113
		private bool m_CompletedChanged;

		// Token: 0x04002B6A RID: 11114
		private bool m_DisconnectReuseSocket;

		// Token: 0x04002B6B RID: 11115
		private SocketAsyncOperation m_CompletedOperation;

		// Token: 0x04002B6C RID: 11116
		private IPPacketInformation m_ReceiveMessageFromPacketInfo;

		// Token: 0x04002B6D RID: 11117
		private EndPoint m_RemoteEndPoint;

		// Token: 0x04002B6E RID: 11118
		internal TransmitFileOptions m_SendPacketsFlags;

		// Token: 0x04002B6F RID: 11119
		internal int m_SendPacketsSendSize;

		// Token: 0x04002B70 RID: 11120
		internal SendPacketsElement[] m_SendPacketsElements;

		// Token: 0x04002B71 RID: 11121
		private SendPacketsElement[] m_SendPacketsElementsInternal;

		// Token: 0x04002B72 RID: 11122
		internal int m_SendPacketsElementsFileCount;

		// Token: 0x04002B73 RID: 11123
		internal int m_SendPacketsElementsBufferCount;

		// Token: 0x04002B74 RID: 11124
		private SocketError m_SocketError;

		// Token: 0x04002B75 RID: 11125
		internal SocketFlags m_SocketFlags;

		// Token: 0x04002B76 RID: 11126
		private object m_UserToken;

		// Token: 0x04002B77 RID: 11127
		internal byte[] m_AcceptBuffer;

		// Token: 0x04002B78 RID: 11128
		internal int m_AcceptAddressBufferCount;

		// Token: 0x04002B79 RID: 11129
		internal IntPtr m_PtrAcceptBuffer;

		// Token: 0x04002B7A RID: 11130
		internal SocketAddress m_SocketAddress;

		// Token: 0x04002B7B RID: 11131
		private GCHandle m_SocketAddressGCHandle;

		// Token: 0x04002B7C RID: 11132
		private SocketAddress m_PinnedSocketAddress;

		// Token: 0x04002B7D RID: 11133
		internal IntPtr m_PtrSocketAddressBuffer;

		// Token: 0x04002B7E RID: 11134
		internal IntPtr m_PtrSocketAddressBufferSize;

		// Token: 0x04002B7F RID: 11135
		private byte[] m_WSAMessageBuffer;

		// Token: 0x04002B80 RID: 11136
		private GCHandle m_WSAMessageBufferGCHandle;

		// Token: 0x04002B81 RID: 11137
		internal IntPtr m_PtrWSAMessageBuffer;

		// Token: 0x04002B82 RID: 11138
		private byte[] m_ControlBuffer;

		// Token: 0x04002B83 RID: 11139
		private GCHandle m_ControlBufferGCHandle;

		// Token: 0x04002B84 RID: 11140
		internal IntPtr m_PtrControlBuffer;

		// Token: 0x04002B85 RID: 11141
		private WSABuffer[] m_WSARecvMsgWSABufferArray;

		// Token: 0x04002B86 RID: 11142
		private GCHandle m_WSARecvMsgWSABufferArrayGCHandle;

		// Token: 0x04002B87 RID: 11143
		private IntPtr m_PtrWSARecvMsgWSABufferArray;

		// Token: 0x04002B88 RID: 11144
		internal FileStream[] m_SendPacketsFileStreams;

		// Token: 0x04002B89 RID: 11145
		internal SafeHandle[] m_SendPacketsFileHandles;

		// Token: 0x04002B8A RID: 11146
		private UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElement[] m_SendPacketsDescriptor;

		// Token: 0x04002B8B RID: 11147
		internal IntPtr m_PtrSendPacketsDescriptor;

		// Token: 0x04002B8C RID: 11148
		private ExecutionContext m_Context;

		// Token: 0x04002B8D RID: 11149
		private ExecutionContext m_ContextCopy;

		// Token: 0x04002B8E RID: 11150
		private ContextCallback m_ExecutionCallback;

		// Token: 0x04002B8F RID: 11151
		private Socket m_CurrentSocket;

		// Token: 0x04002B90 RID: 11152
		private bool m_DisposeCalled;

		// Token: 0x04002B91 RID: 11153
		private int m_Operating;

		// Token: 0x04002B92 RID: 11154
		internal SafeNativeOverlapped m_PtrNativeOverlapped;

		// Token: 0x04002B93 RID: 11155
		private Overlapped m_Overlapped;

		// Token: 0x04002B94 RID: 11156
		private object[] m_ObjectsToPin;

		// Token: 0x04002B95 RID: 11157
		private SocketAsyncEventArgs.PinState m_PinState;

		// Token: 0x04002B96 RID: 11158
		private byte[] m_PinnedAcceptBuffer;

		// Token: 0x04002B97 RID: 11159
		private byte[] m_PinnedSingleBuffer;

		// Token: 0x04002B98 RID: 11160
		private int m_PinnedSingleBufferOffset;

		// Token: 0x04002B99 RID: 11161
		private int m_PinnedSingleBufferCount;

		// Token: 0x04002B9A RID: 11162
		private static bool s_LoggingEnabled = Logging.On;

		// Token: 0x020005C0 RID: 1472
		private enum PinState
		{
			// Token: 0x04002B9C RID: 11164
			None,
			// Token: 0x04002B9D RID: 11165
			NoBuffer,
			// Token: 0x04002B9E RID: 11166
			SingleAcceptBuffer,
			// Token: 0x04002B9F RID: 11167
			SingleBuffer,
			// Token: 0x04002BA0 RID: 11168
			MultipleBuffer,
			// Token: 0x04002BA1 RID: 11169
			SendPackets
		}
	}
}
