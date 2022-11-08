using System;

namespace System.Net.Sockets
{
	// Token: 0x020005BE RID: 1470
	public class SendPacketsElement
	{
		// Token: 0x06002DDA RID: 11738 RVA: 0x000C9F85 File Offset: 0x000C8F85
		private SendPacketsElement()
		{
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000C9F8D File Offset: 0x000C8F8D
		public SendPacketsElement(string filepath) : this(filepath, null, 0, 0, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.File)
		{
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000C9F9A File Offset: 0x000C8F9A
		public SendPacketsElement(string filepath, int offset, int count) : this(filepath, null, offset, count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.File)
		{
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000C9FA7 File Offset: 0x000C8FA7
		public SendPacketsElement(string filepath, int offset, int count, bool endOfPacket) : this(filepath, null, offset, count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.File | UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.EndOfPacket)
		{
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000C9FB4 File Offset: 0x000C8FB4
		public SendPacketsElement(byte[] buffer) : this(null, buffer, 0, buffer.Length, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.Memory)
		{
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000C9FC3 File Offset: 0x000C8FC3
		public SendPacketsElement(byte[] buffer, int offset, int count) : this(null, buffer, offset, count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.Memory)
		{
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000C9FD0 File Offset: 0x000C8FD0
		public SendPacketsElement(byte[] buffer, int offset, int count, bool endOfPacket) : this(null, buffer, offset, count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.Memory | UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.EndOfPacket)
		{
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000C9FDD File Offset: 0x000C8FDD
		private SendPacketsElement(string filepath, byte[] buffer, int offset, int count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags flags)
		{
			this.m_FilePath = filepath;
			this.m_Buffer = buffer;
			this.m_Offset = offset;
			this.m_Count = count;
			this.m_Flags = flags;
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x000CA00A File Offset: 0x000C900A
		public string FilePath
		{
			get
			{
				return this.m_FilePath;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x000CA012 File Offset: 0x000C9012
		public byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002DE4 RID: 11748 RVA: 0x000CA01A File Offset: 0x000C901A
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000CA022 File Offset: 0x000C9022
		public int Offset
		{
			get
			{
				return this.m_Offset;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x000CA02A File Offset: 0x000C902A
		public bool EndOfPacket
		{
			get
			{
				return (this.m_Flags & UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.EndOfPacket) != UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.None;
			}
		}

		// Token: 0x04002B52 RID: 11090
		internal string m_FilePath;

		// Token: 0x04002B53 RID: 11091
		internal byte[] m_Buffer;

		// Token: 0x04002B54 RID: 11092
		internal int m_Offset;

		// Token: 0x04002B55 RID: 11093
		internal int m_Count;

		// Token: 0x04002B56 RID: 11094
		internal UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags m_Flags;
	}
}
