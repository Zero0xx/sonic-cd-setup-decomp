using System;

namespace System.Net.Sockets
{
	// Token: 0x020005D3 RID: 1491
	public struct IPPacketInformation
	{
		// Token: 0x06002EDB RID: 11995 RVA: 0x000CEAC9 File Offset: 0x000CDAC9
		internal IPPacketInformation(IPAddress address, int networkInterface)
		{
			this.address = address;
			this.networkInterface = networkInterface;
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x000CEAD9 File Offset: 0x000CDAD9
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06002EDD RID: 11997 RVA: 0x000CEAE1 File Offset: 0x000CDAE1
		public int Interface
		{
			get
			{
				return this.networkInterface;
			}
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000CEAE9 File Offset: 0x000CDAE9
		public static bool operator ==(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
		{
			return packetInformation1.Equals(packetInformation2);
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000CEAFE File Offset: 0x000CDAFE
		public static bool operator !=(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
		{
			return !packetInformation1.Equals(packetInformation2);
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000CEB18 File Offset: 0x000CDB18
		public override bool Equals(object comparand)
		{
			if (comparand == null)
			{
				return false;
			}
			if (!(comparand is IPPacketInformation))
			{
				return false;
			}
			IPPacketInformation ippacketInformation = (IPPacketInformation)comparand;
			return this.address.Equals(ippacketInformation.address) && this.networkInterface == ippacketInformation.networkInterface;
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000CEB61 File Offset: 0x000CDB61
		public override int GetHashCode()
		{
			return this.address.GetHashCode() + this.networkInterface.GetHashCode();
		}

		// Token: 0x04002C57 RID: 11351
		private IPAddress address;

		// Token: 0x04002C58 RID: 11352
		private int networkInterface;
	}
}
