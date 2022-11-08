using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x020005D1 RID: 1489
	internal class TransmitFileOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002EC8 RID: 11976 RVA: 0x000CE575 File Offset: 0x000CD575
		internal TransmitFileOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback) : base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000CE580 File Offset: 0x000CD580
		internal TransmitFileOverlappedAsyncResult(Socket socket) : base(socket)
		{
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000CE58C File Offset: 0x000CD58C
		internal void SetUnmanagedStructures(byte[] preBuffer, byte[] postBuffer, FileStream fileStream, TransmitFileOptions flags, bool sync)
		{
			this.m_fileStream = fileStream;
			this.m_flags = flags;
			this.m_buffers = null;
			int num = 0;
			if (preBuffer != null && preBuffer.Length > 0)
			{
				num++;
			}
			if (postBuffer != null && postBuffer.Length > 0)
			{
				num++;
			}
			if (num != 0)
			{
				num++;
				object[] array = new object[num];
				this.m_buffers = new TransmitFileBuffers();
				array[--num] = this.m_buffers;
				if (preBuffer != null && preBuffer.Length > 0)
				{
					this.m_buffers.preBufferLength = preBuffer.Length;
					array[--num] = preBuffer;
				}
				if (postBuffer != null && postBuffer.Length > 0)
				{
					this.m_buffers.postBufferLength = postBuffer.Length;
					array[num - 1] = postBuffer;
				}
				if (sync)
				{
					base.PinUnmanagedObjects(array);
				}
				else
				{
					base.SetUnmanagedStructures(array);
				}
				if (preBuffer != null && preBuffer.Length > 0)
				{
					this.m_buffers.preBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(preBuffer, 0);
				}
				if (postBuffer != null && postBuffer.Length > 0)
				{
					this.m_buffers.postBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(postBuffer, 0);
					return;
				}
			}
			else if (!sync)
			{
				base.SetUnmanagedStructures(null);
			}
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000CE68A File Offset: 0x000CD68A
		internal void SetUnmanagedStructures(byte[] preBuffer, byte[] postBuffer, FileStream fileStream, TransmitFileOptions flags, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(preBuffer, postBuffer, fileStream, flags, false);
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000CE6A0 File Offset: 0x000CD6A0
		protected override void ForceReleaseUnmanagedStructures()
		{
			if (this.m_fileStream != null)
			{
				this.m_fileStream.Close();
				this.m_fileStream = null;
			}
			base.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000CE6C2 File Offset: 0x000CD6C2
		internal void SyncReleaseUnmanagedStructures()
		{
			this.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x000CE6CA File Offset: 0x000CD6CA
		internal TransmitFileBuffers TransmitFileBuffers
		{
			get
			{
				return this.m_buffers;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06002ECF RID: 11983 RVA: 0x000CE6D2 File Offset: 0x000CD6D2
		internal TransmitFileOptions Flags
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x04002C47 RID: 11335
		private FileStream m_fileStream;

		// Token: 0x04002C48 RID: 11336
		private TransmitFileOptions m_flags;

		// Token: 0x04002C49 RID: 11337
		private TransmitFileBuffers m_buffers;
	}
}
