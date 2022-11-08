using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x020005D2 RID: 1490
	internal class ReceiveMessageOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002ED0 RID: 11984 RVA: 0x000CE6DA File Offset: 0x000CD6DA
		internal ReceiveMessageOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback) : base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000CE6E5 File Offset: 0x000CD6E5
		internal IntPtr GetSocketAddressSizePtr()
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, this.m_SocketAddress.GetAddressSizeOffset());
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000CE702 File Offset: 0x000CD702
		internal SocketAddress SocketAddress
		{
			get
			{
				return this.m_SocketAddress;
			}
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000CE70C File Offset: 0x000CD70C
		internal unsafe void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, SocketFlags socketFlags)
		{
			bool flag = ((Socket)base.AsyncObject).AddressFamily == AddressFamily.InterNetwork;
			bool flag2 = ((Socket)base.AsyncObject).AddressFamily == AddressFamily.InterNetworkV6;
			this.m_MessageBuffer = new byte[ReceiveMessageOverlappedAsyncResult.s_WSAMsgSize];
			this.m_WSABufferArray = new byte[ReceiveMessageOverlappedAsyncResult.s_WSABufferSize];
			if (flag)
			{
				this.m_ControlBuffer = new byte[ReceiveMessageOverlappedAsyncResult.s_ControlDataSize];
			}
			else if (flag2)
			{
				this.m_ControlBuffer = new byte[ReceiveMessageOverlappedAsyncResult.s_ControlDataIPv6Size];
			}
			object[] array = new object[(this.m_ControlBuffer != null) ? 5 : 4];
			array[0] = buffer;
			array[1] = this.m_MessageBuffer;
			array[2] = this.m_WSABufferArray;
			this.m_SocketAddress = socketAddress;
			this.m_SocketAddress.CopyAddressSizeIntoBuffer();
			array[3] = this.m_SocketAddress.m_Buffer;
			if (this.m_ControlBuffer != null)
			{
				array[4] = this.m_ControlBuffer;
			}
			base.SetUnmanagedStructures(array);
			this.m_WSABuffer = (WSABuffer*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSABufferArray, 0));
			this.m_WSABuffer->Length = size;
			this.m_WSABuffer->Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
			this.m_Message = (UnsafeNclNativeMethods.OSSOCK.WSAMsg*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_MessageBuffer, 0));
			this.m_Message->socketAddress = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, 0);
			this.m_Message->addressLength = (uint)this.m_SocketAddress.Size;
			this.m_Message->buffers = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSABufferArray, 0);
			this.m_Message->count = 1U;
			if (this.m_ControlBuffer != null)
			{
				this.m_Message->controlBuffer.Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_ControlBuffer, 0);
				this.m_Message->controlBuffer.Length = this.m_ControlBuffer.Length;
			}
			this.m_Message->flags = socketFlags;
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000CE8D5 File Offset: 0x000CD8D5
		internal void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, SocketFlags socketFlags, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffer, offset, size, socketAddress, socketFlags);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000CE8EC File Offset: 0x000CD8EC
		private unsafe void InitIPPacketInformation()
		{
			IPAddress ipaddress = null;
			if (this.m_ControlBuffer.Length == ReceiveMessageOverlappedAsyncResult.s_ControlDataSize)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlData controlData = (UnsafeNclNativeMethods.OSSOCK.ControlData)Marshal.PtrToStructure(this.m_Message->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));
				if (controlData.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress((long)((ulong)controlData.address));
				}
				this.m_IPPacketInformation = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.None, (int)controlData.index);
				return;
			}
			if (this.m_ControlBuffer.Length == ReceiveMessageOverlappedAsyncResult.s_ControlDataIPv6Size)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6 controlDataIPv = (UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6)Marshal.PtrToStructure(this.m_Message->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));
				if (controlDataIPv.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress(controlDataIPv.address);
				}
				this.m_IPPacketInformation = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.IPv6None, (int)controlDataIPv.index);
				return;
			}
			this.m_IPPacketInformation = default(IPPacketInformation);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000CE9EC File Offset: 0x000CD9EC
		internal void SyncReleaseUnmanagedStructures()
		{
			this.InitIPPacketInformation();
			this.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000CE9FA File Offset: 0x000CD9FA
		protected unsafe override void ForceReleaseUnmanagedStructures()
		{
			this.m_flags = this.m_Message->flags;
			base.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000CEA13 File Offset: 0x000CDA13
		internal override object PostCompletion(int numBytes)
		{
			this.InitIPPacketInformation();
			if (base.ErrorCode == 0 && Logging.On)
			{
				this.LogBuffer(numBytes);
			}
			return numBytes;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000CEA37 File Offset: 0x000CDA37
		private unsafe void LogBuffer(int size)
		{
			Logging.Dump(Logging.Sockets, base.AsyncObject, "PostCompletion", this.m_WSABuffer->Pointer, Math.Min(this.m_WSABuffer->Length, size));
		}

		// Token: 0x04002C4A RID: 11338
		private unsafe UnsafeNclNativeMethods.OSSOCK.WSAMsg* m_Message;

		// Token: 0x04002C4B RID: 11339
		internal SocketAddress SocketAddressOriginal;

		// Token: 0x04002C4C RID: 11340
		internal SocketAddress m_SocketAddress;

		// Token: 0x04002C4D RID: 11341
		private unsafe WSABuffer* m_WSABuffer;

		// Token: 0x04002C4E RID: 11342
		private byte[] m_WSABufferArray;

		// Token: 0x04002C4F RID: 11343
		private byte[] m_ControlBuffer;

		// Token: 0x04002C50 RID: 11344
		internal byte[] m_MessageBuffer;

		// Token: 0x04002C51 RID: 11345
		internal SocketFlags m_flags;

		// Token: 0x04002C52 RID: 11346
		private static readonly int s_ControlDataSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));

		// Token: 0x04002C53 RID: 11347
		private static readonly int s_ControlDataIPv6Size = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));

		// Token: 0x04002C54 RID: 11348
		private static readonly int s_WSABufferSize = Marshal.SizeOf(typeof(WSABuffer));

		// Token: 0x04002C55 RID: 11349
		private static readonly int s_WSAMsgSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.WSAMsg));

		// Token: 0x04002C56 RID: 11350
		internal IPPacketInformation m_IPPacketInformation;
	}
}
