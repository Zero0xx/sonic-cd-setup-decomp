using System;
using System.Globalization;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x02000422 RID: 1058
	[Serializable]
	public class IPEndPoint : EndPoint
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00082349 File Offset: 0x00081349
		public override AddressFamily AddressFamily
		{
			get
			{
				return this.m_Address.AddressFamily;
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00082356 File Offset: 0x00081356
		public IPEndPoint(long address, int port)
		{
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_Port = port;
			this.m_Address = new IPAddress(address);
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00082384 File Offset: 0x00081384
		public IPEndPoint(IPAddress address, int port)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_Port = port;
			this.m_Address = address;
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x000823BB File Offset: 0x000813BB
		// (set) Token: 0x06002103 RID: 8451 RVA: 0x000823C3 File Offset: 0x000813C3
		public IPAddress Address
		{
			get
			{
				return this.m_Address;
			}
			set
			{
				this.m_Address = value;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002104 RID: 8452 RVA: 0x000823CC File Offset: 0x000813CC
		// (set) Token: 0x06002105 RID: 8453 RVA: 0x000823D4 File Offset: 0x000813D4
		public int Port
		{
			get
			{
				return this.m_Port;
			}
			set
			{
				if (!ValidationHelper.ValidateTcpPort(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_Port = value;
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000823F0 File Offset: 0x000813F0
		public override string ToString()
		{
			return this.Address.ToString() + ":" + this.Port.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00082428 File Offset: 0x00081428
		public override SocketAddress Serialize()
		{
			if (this.m_Address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				SocketAddress socketAddress = new SocketAddress(this.AddressFamily, 28);
				int port = this.Port;
				socketAddress[2] = (byte)(port >> 8);
				socketAddress[3] = (byte)port;
				socketAddress[4] = 0;
				socketAddress[5] = 0;
				socketAddress[6] = 0;
				socketAddress[7] = 0;
				long scopeId = this.Address.ScopeId;
				socketAddress[24] = (byte)scopeId;
				socketAddress[25] = (byte)(scopeId >> 8);
				socketAddress[26] = (byte)(scopeId >> 16);
				socketAddress[27] = (byte)(scopeId >> 24);
				byte[] addressBytes = this.Address.GetAddressBytes();
				for (int i = 0; i < addressBytes.Length; i++)
				{
					socketAddress[8 + i] = addressBytes[i];
				}
				return socketAddress;
			}
			SocketAddress socketAddress2 = new SocketAddress(this.m_Address.AddressFamily, 16);
			socketAddress2[2] = (byte)(this.Port >> 8);
			socketAddress2[3] = (byte)this.Port;
			socketAddress2[4] = (byte)this.Address.m_Address;
			socketAddress2[5] = (byte)(this.Address.m_Address >> 8);
			socketAddress2[6] = (byte)(this.Address.m_Address >> 16);
			socketAddress2[7] = (byte)(this.Address.m_Address >> 24);
			return socketAddress2;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00082588 File Offset: 0x00081588
		public override EndPoint Create(SocketAddress socketAddress)
		{
			if (socketAddress.Family != this.AddressFamily)
			{
				throw new ArgumentException(SR.GetString("net_InvalidAddressFamily", new object[]
				{
					socketAddress.Family.ToString(),
					base.GetType().FullName,
					this.AddressFamily.ToString()
				}), "socketAddress");
			}
			if (socketAddress.Size < 8)
			{
				throw new ArgumentException(SR.GetString("net_InvalidSocketAddressSize", new object[]
				{
					socketAddress.GetType().FullName,
					base.GetType().FullName
				}), "socketAddress");
			}
			if (this.AddressFamily == AddressFamily.InterNetworkV6)
			{
				byte[] array = new byte[16];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = socketAddress[i + 8];
				}
				int port = ((int)socketAddress[2] << 8 & 65280) | (int)socketAddress[3];
				long scopeid = (long)(((int)socketAddress[27] << 24) + ((int)socketAddress[26] << 16) + ((int)socketAddress[25] << 8) + (int)socketAddress[24]);
				return new IPEndPoint(new IPAddress(array, scopeid), port);
			}
			int port2 = ((int)socketAddress[2] << 8 & 65280) | (int)socketAddress[3];
			long address = (long)((int)(socketAddress[4] & byte.MaxValue) | ((int)socketAddress[5] << 8 & 65280) | ((int)socketAddress[6] << 16 & 16711680) | (int)socketAddress[7] << 24) & (long)((ulong)-1);
			return new IPEndPoint(address, port2);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00082727 File Offset: 0x00081727
		public override bool Equals(object comparand)
		{
			return comparand is IPEndPoint && ((IPEndPoint)comparand).m_Address.Equals(this.m_Address) && ((IPEndPoint)comparand).m_Port == this.m_Port;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00082760 File Offset: 0x00081760
		public override int GetHashCode()
		{
			return this.m_Address.GetHashCode() ^ this.m_Port;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00082774 File Offset: 0x00081774
		internal IPEndPoint Snapshot()
		{
			return new IPEndPoint(this.Address.Snapshot(), this.Port);
		}

		// Token: 0x04002153 RID: 8531
		public const int MinPort = 0;

		// Token: 0x04002154 RID: 8532
		public const int MaxPort = 65535;

		// Token: 0x04002155 RID: 8533
		internal const int AnyPort = 0;

		// Token: 0x04002156 RID: 8534
		private IPAddress m_Address;

		// Token: 0x04002157 RID: 8535
		private int m_Port;

		// Token: 0x04002158 RID: 8536
		internal static IPEndPoint Any = new IPEndPoint(IPAddress.Any, 0);

		// Token: 0x04002159 RID: 8537
		internal static IPEndPoint IPv6Any = new IPEndPoint(IPAddress.IPv6Any, 0);
	}
}
