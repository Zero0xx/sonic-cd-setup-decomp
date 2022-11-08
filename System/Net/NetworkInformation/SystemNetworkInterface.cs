using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000637 RID: 1591
	internal class SystemNetworkInterface : NetworkInterface
	{
		// Token: 0x0600313B RID: 12603 RVA: 0x000D3816 File Offset: 0x000D2816
		private SystemNetworkInterface()
		{
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000D381E File Offset: 0x000D281E
		internal static NetworkInterface[] GetNetworkInterfaces()
		{
			return SystemNetworkInterface.GetNetworkInterfaces(AddressFamily.Unspecified);
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x000D3828 File Offset: 0x000D2828
		internal static int InternalLoopbackInterfaceIndex
		{
			get
			{
				int result;
				int bestInterface = (int)UnsafeNetInfoNativeMethods.GetBestInterface(16777343, out result);
				if (bestInterface != 0)
				{
					throw new NetworkInformationException(bestInterface);
				}
				return result;
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000D3850 File Offset: 0x000D2850
		internal static bool InternalGetIsNetworkAvailable()
		{
			if (ComNetOS.IsWinNt)
			{
				NetworkInterface[] networkInterfaces = SystemNetworkInterface.GetNetworkInterfaces();
				foreach (NetworkInterface networkInterface in networkInterfaces)
				{
					if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
					{
						return true;
					}
				}
				return false;
			}
			uint num = 0U;
			return UnsafeWinINetNativeMethods.InternetGetConnectedState(ref num, 0U);
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000D38BC File Offset: 0x000D28BC
		private static NetworkInterface[] GetNetworkInterfaces(AddressFamily family)
		{
			IpHelperErrors.CheckFamilyUnspecified(family);
			if (ComNetOS.IsPostWin2K)
			{
				return SystemNetworkInterface.PostWin2KGetNetworkInterfaces(family);
			}
			FixedInfo fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
			if (family != AddressFamily.Unspecified && family != AddressFamily.InterNetwork)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			SafeLocalFree safeLocalFree = null;
			uint cb = 0U;
			ArrayList arrayList = new ArrayList();
			uint adaptersInfo = UnsafeNetInfoNativeMethods.GetAdaptersInfo(SafeLocalFree.Zero, ref cb);
			while (adaptersInfo == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
					adaptersInfo = UnsafeNetInfoNativeMethods.GetAdaptersInfo(safeLocalFree, ref cb);
					if (adaptersInfo == 0U)
					{
						IpAdapterInfo ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpAdapterInfo));
						arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterInfo));
						while (ipAdapterInfo.Next != IntPtr.Zero)
						{
							ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(ipAdapterInfo.Next, typeof(IpAdapterInfo));
							arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterInfo));
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (adaptersInfo == 232U)
			{
				return new SystemNetworkInterface[0];
			}
			if (adaptersInfo != 0U)
			{
				throw new NetworkInformationException((int)adaptersInfo);
			}
			SystemNetworkInterface[] array = new SystemNetworkInterface[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = (SystemNetworkInterface)arrayList[i];
			}
			return array;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000D3A14 File Offset: 0x000D2A14
		private static SystemNetworkInterface[] GetAdaptersAddresses(AddressFamily family, FixedInfo fixedInfo)
		{
			uint cb = 0U;
			SafeLocalFree safeLocalFree = null;
			ArrayList arrayList = new ArrayList();
			uint adaptersAddresses = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(family, 0U, IntPtr.Zero, SafeLocalFree.Zero, ref cb);
			while (adaptersAddresses == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
					adaptersAddresses = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(family, 0U, IntPtr.Zero, safeLocalFree, ref cb);
					if (adaptersAddresses == 0U)
					{
						IpAdapterAddresses ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpAdapterAddresses));
						arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterAddresses));
						while (ipAdapterAddresses.next != IntPtr.Zero)
						{
							ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(ipAdapterAddresses.next, typeof(IpAdapterAddresses));
							arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterAddresses));
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
					safeLocalFree = null;
				}
			}
			if (adaptersAddresses == 232U || adaptersAddresses == 87U)
			{
				return new SystemNetworkInterface[0];
			}
			if (adaptersAddresses != 0U)
			{
				throw new NetworkInformationException((int)adaptersAddresses);
			}
			SystemNetworkInterface[] array = new SystemNetworkInterface[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = (SystemNetworkInterface)arrayList[i];
			}
			return array;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000D3B4C File Offset: 0x000D2B4C
		private static SystemNetworkInterface[] PostWin2KGetNetworkInterfaces(AddressFamily family)
		{
			FixedInfo fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
			SystemNetworkInterface[] array = null;
			try
			{
				IL_08:
				array = SystemNetworkInterface.GetAdaptersAddresses(family, fixedInfo);
			}
			catch (NetworkInformationException ex)
			{
				if ((long)ex.ErrorCode != 1L)
				{
					throw;
				}
				goto IL_08;
			}
			if (!Socket.SupportsIPv4)
			{
				return array;
			}
			uint cb = 0U;
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			if (family == AddressFamily.Unspecified || family == AddressFamily.InterNetwork)
			{
				num = UnsafeNetInfoNativeMethods.GetAdaptersInfo(SafeLocalFree.Zero, ref cb);
				int num2 = 0;
				while (num == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
						num = UnsafeNetInfoNativeMethods.GetAdaptersInfo(safeLocalFree, ref cb);
						if (num == 0U)
						{
							IntPtr intPtr = safeLocalFree.DangerousGetHandle();
							while (intPtr != IntPtr.Zero)
							{
								IpAdapterInfo ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(intPtr, typeof(IpAdapterInfo));
								int i = 0;
								while (i < array.Length)
								{
									if (array[i] != null && ipAdapterInfo.index == array[i].index)
									{
										if (!array[i].interfaceProperties.Update(fixedInfo, ipAdapterInfo))
										{
											array[i] = null;
											num2++;
											break;
										}
										break;
									}
									else
									{
										i++;
									}
								}
								intPtr = ipAdapterInfo.Next;
							}
						}
					}
					finally
					{
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (num2 != 0)
				{
					SystemNetworkInterface[] array2 = new SystemNetworkInterface[array.Length - num2];
					int num3 = 0;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] != null)
						{
							array2[num3++] = array[j];
						}
					}
					array = array2;
				}
			}
			if (num != 0U && num != 232U)
			{
				throw new NetworkInformationException((int)num);
			}
			return array;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x000D3CD8 File Offset: 0x000D2CD8
		internal SystemNetworkInterface(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.id = ipAdapterAddresses.AdapterName;
			this.name = ipAdapterAddresses.friendlyName;
			this.description = ipAdapterAddresses.description;
			this.index = ipAdapterAddresses.index;
			this.physicalAddress = ipAdapterAddresses.address;
			this.addressLength = ipAdapterAddresses.addressLength;
			this.type = ipAdapterAddresses.type;
			this.operStatus = ipAdapterAddresses.operStatus;
			this.ipv6Index = ipAdapterAddresses.ipv6Index;
			this.adapterFlags = ipAdapterAddresses.flags;
			this.interfaceProperties = new SystemIPInterfaceProperties(fixedInfo, ipAdapterAddresses);
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000D3D7C File Offset: 0x000D2D7C
		internal SystemNetworkInterface(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			this.id = ipAdapterInfo.adapterName;
			this.name = string.Empty;
			this.description = ipAdapterInfo.description;
			this.index = ipAdapterInfo.index;
			this.physicalAddress = ipAdapterInfo.address;
			this.addressLength = ipAdapterInfo.addressLength;
			if (ComNetOS.IsWin2K && !ComNetOS.IsPostWin2K)
			{
				this.name = this.ReadAdapterName(this.id);
			}
			if (this.name.Length == 0)
			{
				this.name = this.description;
			}
			SystemIPv4InterfaceStatistics systemIPv4InterfaceStatistics = new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
			this.operStatus = systemIPv4InterfaceStatistics.OperationalStatus;
			OldInterfaceType oldInterfaceType = ipAdapterInfo.type;
			if (oldInterfaceType <= OldInterfaceType.TokenRing)
			{
				if (oldInterfaceType == OldInterfaceType.Ethernet)
				{
					this.type = NetworkInterfaceType.Ethernet;
					goto IL_11B;
				}
				if (oldInterfaceType == OldInterfaceType.TokenRing)
				{
					this.type = NetworkInterfaceType.TokenRing;
					goto IL_11B;
				}
			}
			else
			{
				if (oldInterfaceType == OldInterfaceType.Fddi)
				{
					this.type = NetworkInterfaceType.Fddi;
					goto IL_11B;
				}
				switch (oldInterfaceType)
				{
				case OldInterfaceType.Ppp:
					this.type = NetworkInterfaceType.Ppp;
					goto IL_11B;
				case OldInterfaceType.Loopback:
					this.type = NetworkInterfaceType.Loopback;
					goto IL_11B;
				default:
					if (oldInterfaceType == OldInterfaceType.Slip)
					{
						this.type = NetworkInterfaceType.Slip;
						goto IL_11B;
					}
					break;
				}
			}
			this.type = NetworkInterfaceType.Unknown;
			IL_11B:
			this.interfaceProperties = new SystemIPInterfaceProperties(fixedInfo, ipAdapterInfo);
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x000D3EB1 File Offset: 0x000D2EB1
		public override string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x000D3EB9 File Offset: 0x000D2EB9
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000D3EC1 File Offset: 0x000D2EC1
		public override string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000D3ECC File Offset: 0x000D2ECC
		public override PhysicalAddress GetPhysicalAddress()
		{
			byte[] array = new byte[this.addressLength];
			Array.Copy(this.physicalAddress, array, (long)((ulong)this.addressLength));
			return new PhysicalAddress(array);
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x000D3EFF File Offset: 0x000D2EFF
		public override NetworkInterfaceType NetworkInterfaceType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000D3F07 File Offset: 0x000D2F07
		public override IPInterfaceProperties GetIPProperties()
		{
			return this.interfaceProperties;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000D3F0F File Offset: 0x000D2F0F
		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			return new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000D3F1D File Offset: 0x000D2F1D
		public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			return (networkInterfaceComponent == NetworkInterfaceComponent.IPv6 && this.ipv6Index > 0U) || (networkInterfaceComponent == NetworkInterfaceComponent.IPv4 && this.index > 0U);
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x000D3F3D File Offset: 0x000D2F3D
		public override OperationalStatus OperationalStatus
		{
			get
			{
				return this.operStatus;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x000D3F48 File Offset: 0x000D2F48
		public override long Speed
		{
			get
			{
				if (this.speed == 0L)
				{
					SystemIPv4InterfaceStatistics systemIPv4InterfaceStatistics = new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
					this.speed = systemIPv4InterfaceStatistics.Speed;
				}
				return this.speed;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600314E RID: 12622 RVA: 0x000D3F7E File Offset: 0x000D2F7E
		public override bool IsReceiveOnly
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (this.adapterFlags & AdapterFlags.ReceiveOnly) > (AdapterFlags)0;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x000D3FA2 File Offset: 0x000D2FA2
		public override bool SupportsMulticast
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (this.adapterFlags & AdapterFlags.NoMulticast) == (AdapterFlags)0;
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000D3FC8 File Offset: 0x000D2FC8
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}")]
		private string ReadAdapterName(string id)
		{
			RegistryKey registryKey = null;
			string text = string.Empty;
			try
			{
				string text2 = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + id + "\\Connection";
				registryKey = Registry.LocalMachine.OpenSubKey(text2);
				if (registryKey != null)
				{
					text = (string)registryKey.GetValue("Name");
					if (text == null)
					{
						text = string.Empty;
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return text;
		}

		// Token: 0x04002E6D RID: 11885
		private string name;

		// Token: 0x04002E6E RID: 11886
		private string id;

		// Token: 0x04002E6F RID: 11887
		private string description;

		// Token: 0x04002E70 RID: 11888
		private byte[] physicalAddress;

		// Token: 0x04002E71 RID: 11889
		private uint addressLength;

		// Token: 0x04002E72 RID: 11890
		private NetworkInterfaceType type;

		// Token: 0x04002E73 RID: 11891
		private OperationalStatus operStatus;

		// Token: 0x04002E74 RID: 11892
		private long speed;

		// Token: 0x04002E75 RID: 11893
		internal uint index;

		// Token: 0x04002E76 RID: 11894
		internal uint ipv6Index;

		// Token: 0x04002E77 RID: 11895
		private AdapterFlags adapterFlags;

		// Token: 0x04002E78 RID: 11896
		private SystemIPInterfaceProperties interfaceProperties;
	}
}
