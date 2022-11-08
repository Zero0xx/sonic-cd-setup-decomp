using System;
using System.Diagnostics;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000623 RID: 1571
	internal class IcmpPacket
	{
		// Token: 0x0600305A RID: 12378 RVA: 0x000D1337 File Offset: 0x000D0337
		internal IcmpPacket(byte[] buffer)
		{
			this.type = 8;
			this.buffer = buffer;
			ushort num = IcmpPacket.staticSequenceNumber;
			IcmpPacket.staticSequenceNumber = num + 1;
			this.sequenceNumber = num;
			this.checkSum = (ushort)this.GetCheckSum();
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x000D136E File Offset: 0x000D036E
		internal ushort Identifier
		{
			get
			{
				if (IcmpPacket.identifier == 0)
				{
					IcmpPacket.identifier = (ushort)Process.GetCurrentProcess().Id;
				}
				return IcmpPacket.identifier;
			}
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000D138C File Offset: 0x000D038C
		private uint GetCheckSum()
		{
			uint num = (uint)((ushort)this.type + this.Identifier + this.sequenceNumber);
			for (int i = 0; i < this.buffer.Length; i++)
			{
				num += (uint)((int)this.buffer[i] + ((int)this.buffer[++i] << 8));
			}
			num = (num >> 16) + (num & 65535U);
			num += num >> 16;
			return ~num;
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000D13F4 File Offset: 0x000D03F4
		internal byte[] GetBytes()
		{
			byte[] array = new byte[this.buffer.Length + 8];
			byte[] bytes = BitConverter.GetBytes(this.checkSum);
			byte[] bytes2 = BitConverter.GetBytes(this.Identifier);
			byte[] bytes3 = BitConverter.GetBytes(this.sequenceNumber);
			array[0] = this.type;
			array[1] = this.subCode;
			Array.Copy(bytes, 0, array, 2, 2);
			Array.Copy(bytes2, 0, array, 4, 2);
			Array.Copy(bytes3, 0, array, 6, 2);
			Array.Copy(this.buffer, 0, array, 8, this.buffer.Length);
			return array;
		}

		// Token: 0x04002E0E RID: 11790
		private static ushort staticSequenceNumber;

		// Token: 0x04002E0F RID: 11791
		internal byte type;

		// Token: 0x04002E10 RID: 11792
		internal byte subCode;

		// Token: 0x04002E11 RID: 11793
		internal ushort checkSum;

		// Token: 0x04002E12 RID: 11794
		internal static ushort identifier;

		// Token: 0x04002E13 RID: 11795
		internal ushort sequenceNumber;

		// Token: 0x04002E14 RID: 11796
		internal byte[] buffer;
	}
}
