using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x020005D0 RID: 1488
	internal class OverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002EBA RID: 11962 RVA: 0x000CE231 File Offset: 0x000CD231
		internal OverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback) : base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000CE23C File Offset: 0x000CD23C
		internal IntPtr GetSocketAddressPtr()
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, 0);
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000CE24F File Offset: 0x000CD24F
		internal IntPtr GetSocketAddressSizePtr()
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, this.m_SocketAddress.GetAddressSizeOffset());
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x000CE26C File Offset: 0x000CD26C
		internal SocketAddress SocketAddress
		{
			get
			{
				return this.m_SocketAddress;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x000CE274 File Offset: 0x000CD274
		// (set) Token: 0x06002EBF RID: 11967 RVA: 0x000CE27C File Offset: 0x000CD27C
		internal SocketAddress SocketAddressOriginal
		{
			get
			{
				return this.m_SocketAddressOriginal;
			}
			set
			{
				this.m_SocketAddressOriginal = value;
			}
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000CE288 File Offset: 0x000CD288
		internal void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, bool pinSocketAddress)
		{
			this.m_SocketAddress = socketAddress;
			if (pinSocketAddress && this.m_SocketAddress != null)
			{
				object[] array = new object[2];
				array[0] = buffer;
				this.m_SocketAddress.CopyAddressSizeIntoBuffer();
				array[1] = this.m_SocketAddress.m_Buffer;
				base.SetUnmanagedStructures(array);
			}
			else
			{
				base.SetUnmanagedStructures(buffer);
			}
			this.m_SingleBuffer.Length = size;
			this.m_SingleBuffer.Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000CE2FD File Offset: 0x000CD2FD
		internal void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, bool pinSocketAddress, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffer, offset, size, socketAddress, pinSocketAddress);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000CE314 File Offset: 0x000CD314
		internal void SetUnmanagedStructures(BufferOffsetSize[] buffers)
		{
			this.m_WSABuffers = new WSABuffer[buffers.Length];
			object[] array = new object[buffers.Length];
			for (int i = 0; i < buffers.Length; i++)
			{
				array[i] = buffers[i].Buffer;
			}
			base.SetUnmanagedStructures(array);
			for (int j = 0; j < buffers.Length; j++)
			{
				this.m_WSABuffers[j].Length = buffers[j].Size;
				this.m_WSABuffers[j].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffers[j].Buffer, buffers[j].Offset);
			}
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000CE3A5 File Offset: 0x000CD3A5
		internal void SetUnmanagedStructures(BufferOffsetSize[] buffers, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffers);
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000CE3B8 File Offset: 0x000CD3B8
		internal void SetUnmanagedStructures(IList<ArraySegment<byte>> buffers)
		{
			int count = buffers.Count;
			ArraySegment<byte>[] array = new ArraySegment<byte>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = buffers[i];
				ValidationHelper.ValidateSegment(array[i]);
			}
			this.m_WSABuffers = new WSABuffer[count];
			object[] array2 = new object[count];
			for (int j = 0; j < count; j++)
			{
				array2[j] = array[j].Array;
			}
			base.SetUnmanagedStructures(array2);
			for (int k = 0; k < count; k++)
			{
				this.m_WSABuffers[k].Length = array[k].Count;
				this.m_WSABuffers[k].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(array[k].Array, array[k].Offset);
			}
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000CE49E File Offset: 0x000CD49E
		internal void SetUnmanagedStructures(IList<ArraySegment<byte>> buffers, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffers);
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000CE4AE File Offset: 0x000CD4AE
		internal override object PostCompletion(int numBytes)
		{
			if (base.ErrorCode == 0 && Logging.On)
			{
				this.LogBuffer(numBytes);
			}
			return numBytes;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000CE4CC File Offset: 0x000CD4CC
		private void LogBuffer(int size)
		{
			if (size > -1)
			{
				if (this.m_WSABuffers != null)
				{
					foreach (WSABuffer wsabuffer in this.m_WSABuffers)
					{
						Logging.Dump(Logging.Sockets, base.AsyncObject, "PostCompletion", wsabuffer.Pointer, Math.Min(wsabuffer.Length, size));
						if ((size -= wsabuffer.Length) <= 0)
						{
							return;
						}
					}
					return;
				}
				Logging.Dump(Logging.Sockets, base.AsyncObject, "PostCompletion", this.m_SingleBuffer.Pointer, Math.Min(this.m_SingleBuffer.Length, size));
			}
		}

		// Token: 0x04002C43 RID: 11331
		private SocketAddress m_SocketAddress;

		// Token: 0x04002C44 RID: 11332
		private SocketAddress m_SocketAddressOriginal;

		// Token: 0x04002C45 RID: 11333
		internal WSABuffer m_SingleBuffer;

		// Token: 0x04002C46 RID: 11334
		internal WSABuffer[] m_WSABuffers;
	}
}
