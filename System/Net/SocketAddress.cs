using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace System.Net
{
	// Token: 0x0200043C RID: 1084
	public class SocketAddress
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x0008658C File Offset: 0x0008558C
		public AddressFamily Family
		{
			get
			{
				return (AddressFamily)((int)this.m_Buffer[0] | (int)this.m_Buffer[1] << 8);
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x000865AE File Offset: 0x000855AE
		public int Size
		{
			get
			{
				return this.m_Size;
			}
		}

		// Token: 0x1700073C RID: 1852
		public byte this[int offset]
		{
			get
			{
				if (offset < 0 || offset >= this.Size)
				{
					throw new IndexOutOfRangeException();
				}
				return this.m_Buffer[offset];
			}
			set
			{
				if (offset < 0 || offset >= this.Size)
				{
					throw new IndexOutOfRangeException();
				}
				if (this.m_Buffer[offset] != value)
				{
					this.m_changed = true;
				}
				this.m_Buffer[offset] = value;
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00086603 File Offset: 0x00085603
		public SocketAddress(AddressFamily family) : this(family, 32)
		{
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00086610 File Offset: 0x00085610
		public SocketAddress(AddressFamily family, int size)
		{
			if (size < 2)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			this.m_Size = size;
			this.m_Buffer = new byte[(size / IntPtr.Size + 2) * IntPtr.Size];
			this.m_Buffer[0] = (byte)family;
			this.m_Buffer[1] = (byte)(family >> 8);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00086670 File Offset: 0x00085670
		internal void CopyAddressSizeIntoBuffer()
		{
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size] = (byte)this.m_Size;
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 1] = (byte)(this.m_Size >> 8);
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 2] = (byte)(this.m_Size >> 16);
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 3] = (byte)(this.m_Size >> 24);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000866FB File Offset: 0x000856FB
		internal int GetAddressSizeOffset()
		{
			return this.m_Buffer.Length - IntPtr.Size;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0008670B File Offset: 0x0008570B
		internal unsafe void SetSize(IntPtr ptr)
		{
			this.m_Size = *(int*)((void*)ptr);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0008671C File Offset: 0x0008571C
		public override bool Equals(object comparand)
		{
			SocketAddress socketAddress = comparand as SocketAddress;
			if (socketAddress == null || this.Size != socketAddress.Size)
			{
				return false;
			}
			for (int i = 0; i < this.Size; i++)
			{
				if (this[i] != socketAddress[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00086768 File Offset: 0x00085768
		public override int GetHashCode()
		{
			if (this.m_changed)
			{
				this.m_changed = false;
				this.m_hash = 0;
				int num = this.Size & -4;
				int i;
				for (i = 0; i < num; i += 4)
				{
					this.m_hash ^= ((int)this.m_Buffer[i] | (int)this.m_Buffer[i + 1] << 8 | (int)this.m_Buffer[i + 2] << 16 | (int)this.m_Buffer[i + 3] << 24);
				}
				if ((this.Size & 3) != 0)
				{
					int num2 = 0;
					int num3 = 0;
					while (i < this.Size)
					{
						num2 |= (int)this.m_Buffer[i] << num3;
						num3 += 8;
						i++;
					}
					this.m_hash ^= num2;
				}
			}
			return this.m_hash;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00086828 File Offset: 0x00085828
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 2; i < this.Size; i++)
			{
				if (i > 2)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(this[i].ToString(NumberFormatInfo.InvariantInfo));
			}
			return string.Concat(new string[]
			{
				this.Family.ToString(),
				":",
				this.Size.ToString(NumberFormatInfo.InvariantInfo),
				":{",
				stringBuilder.ToString(),
				"}"
			});
		}

		// Token: 0x040021FC RID: 8700
		internal const int IPv6AddressSize = 28;

		// Token: 0x040021FD RID: 8701
		internal const int IPv4AddressSize = 16;

		// Token: 0x040021FE RID: 8702
		private const int WriteableOffset = 2;

		// Token: 0x040021FF RID: 8703
		private const int MaxSize = 32;

		// Token: 0x04002200 RID: 8704
		internal int m_Size;

		// Token: 0x04002201 RID: 8705
		internal byte[] m_Buffer;

		// Token: 0x04002202 RID: 8706
		private bool m_changed = true;

		// Token: 0x04002203 RID: 8707
		private int m_hash;
	}
}
