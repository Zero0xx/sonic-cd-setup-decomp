using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000626 RID: 1574
	public class PingReply
	{
		// Token: 0x06003069 RID: 12393 RVA: 0x000D154B File Offset: 0x000D054B
		internal PingReply()
		{
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000D1553 File Offset: 0x000D0553
		internal PingReply(IPStatus ipStatus)
		{
			this.ipStatus = ipStatus;
			this.buffer = new byte[0];
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000D1570 File Offset: 0x000D0570
		internal PingReply(byte[] data, int dataLength, IPAddress address, int time)
		{
			this.address = address;
			this.rtt = (long)time;
			this.ipStatus = this.GetIPStatus((IcmpV4Type)data[20], (IcmpV4Code)data[21]);
			if (this.ipStatus == IPStatus.Success)
			{
				this.buffer = new byte[dataLength - 28];
				Array.Copy(data, 28, this.buffer, 0, dataLength - 28);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000D15E0 File Offset: 0x000D05E0
		internal PingReply(IcmpEchoReply reply)
		{
			this.address = new IPAddress((long)((ulong)reply.address));
			this.ipStatus = (IPStatus)reply.status;
			if (this.ipStatus == IPStatus.Success)
			{
				this.rtt = (long)((ulong)reply.roundTripTime);
				this.buffer = new byte[(int)reply.dataSize];
				Marshal.Copy(reply.data, this.buffer, 0, (int)reply.dataSize);
				this.options = new PingOptions(reply.options);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000D1674 File Offset: 0x000D0674
		internal PingReply(Icmp6EchoReply reply, IntPtr dataPtr, int sendSize)
		{
			this.address = new IPAddress(reply.Address.Address, (long)((ulong)reply.Address.ScopeID));
			this.ipStatus = (IPStatus)reply.Status;
			if (this.ipStatus == IPStatus.Success)
			{
				this.rtt = (long)((ulong)reply.RoundTripTime);
				this.buffer = new byte[sendSize];
				Marshal.Copy(IntPtrHelper.Add(dataPtr, 36), this.buffer, 0, sendSize);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000D16FC File Offset: 0x000D06FC
		private IPStatus GetIPStatus(IcmpV4Type type, IcmpV4Code code)
		{
			switch (type)
			{
			case IcmpV4Type.ICMP4_ECHO_REPLY:
				return IPStatus.Success;
			case (IcmpV4Type)1:
			case (IcmpV4Type)2:
				break;
			case IcmpV4Type.ICMP4_DST_UNREACH:
				switch (code)
				{
				case IcmpV4Code.ICMP4_UNREACH_NET:
					return IPStatus.DestinationNetworkUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_HOST:
					return IPStatus.DestinationHostUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_PROTOCOL:
					return IPStatus.DestinationProtocolUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_PORT:
					return IPStatus.DestinationPortUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_FRAG_NEEDED:
					return IPStatus.PacketTooBig;
				default:
					return IPStatus.DestinationUnreachable;
				}
				break;
			case IcmpV4Type.ICMP4_SOURCE_QUENCH:
				return IPStatus.SourceQuench;
			default:
				switch (type)
				{
				case IcmpV4Type.ICMP4_TIME_EXCEEDED:
					return IPStatus.TtlExpired;
				case IcmpV4Type.ICMP4_PARAM_PROB:
					return IPStatus.ParameterProblem;
				}
				break;
			}
			return IPStatus.Unknown;
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x0600306F RID: 12399 RVA: 0x000D178F File Offset: 0x000D078F
		public IPStatus Status
		{
			get
			{
				return this.ipStatus;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003070 RID: 12400 RVA: 0x000D1797 File Offset: 0x000D0797
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x000D179F File Offset: 0x000D079F
		public long RoundtripTime
		{
			get
			{
				return this.rtt;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003072 RID: 12402 RVA: 0x000D17A7 File Offset: 0x000D07A7
		public PingOptions Options
		{
			get
			{
				if (!ComNetOS.IsWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
				}
				return this.options;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x000D17C6 File Offset: 0x000D07C6
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x04002E18 RID: 11800
		private IPAddress address;

		// Token: 0x04002E19 RID: 11801
		private PingOptions options;

		// Token: 0x04002E1A RID: 11802
		private IPStatus ipStatus;

		// Token: 0x04002E1B RID: 11803
		private long rtt;

		// Token: 0x04002E1C RID: 11804
		private byte[] buffer;
	}
}
