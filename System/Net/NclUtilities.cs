using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003EA RID: 1002
	internal static class NclUtilities
	{
		// Token: 0x0600206E RID: 8302 RVA: 0x0007FBBC File Offset: 0x0007EBBC
		internal static bool IsThreadPoolLow()
		{
			if (ComNetOS.IsAspNetServer)
			{
				return false;
			}
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			return num < 2 || (ComNetOS.IsWinNt && num2 < 2);
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x0007FBEE File Offset: 0x0007EBEE
		internal static bool HasShutdownStarted
		{
			get
			{
				return Environment.HasShutdownStarted || AppDomain.CurrentDomain.IsFinalizingForUnload();
			}
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x0007FC04 File Offset: 0x0007EC04
		internal static bool IsCredentialFailure(SecurityStatus error)
		{
			return error == SecurityStatus.LogonDenied || error == SecurityStatus.UnknownCredentials || error == SecurityStatus.NoImpersonation || error == SecurityStatus.NoAuthenticatingAuthority || error == SecurityStatus.UntrustedRoot || error == SecurityStatus.CertExpired || error == SecurityStatus.SmartcardLogonRequired || error == SecurityStatus.BadBinding;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0007FC54 File Offset: 0x0007EC54
		internal static bool IsClientFault(SecurityStatus error)
		{
			return error == SecurityStatus.InvalidToken || error == SecurityStatus.CannotPack || error == SecurityStatus.QopNotSupported || error == SecurityStatus.NoCredentials || error == SecurityStatus.MessageAltered || error == SecurityStatus.OutOfSequence || error == SecurityStatus.IncompleteMessage || error == SecurityStatus.IncompleteCredentials || error == SecurityStatus.WrongPrincipal || error == SecurityStatus.TimeSkew || error == SecurityStatus.IllegalMessage || error == SecurityStatus.CertUnknown || error == SecurityStatus.AlgorithmMismatch || error == SecurityStatus.SecurityQosFailed || error == SecurityStatus.UnsupportedPreauth;
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x0007FCDB File Offset: 0x0007ECDB
		internal static ContextCallback ContextRelativeDemandCallback
		{
			get
			{
				if (NclUtilities.s_ContextRelativeDemandCallback == null)
				{
					NclUtilities.s_ContextRelativeDemandCallback = new ContextCallback(NclUtilities.DemandCallback);
				}
				return NclUtilities.s_ContextRelativeDemandCallback;
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0007FCFA File Offset: 0x0007ECFA
		private static void DemandCallback(object state)
		{
			((CodeAccessPermission)state).Demand();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x0007FD08 File Offset: 0x0007ED08
		internal static bool GuessWhetherHostIsLoopback(string host)
		{
			string a = host.ToLowerInvariant();
			if (a == "localhost" || a == "loopback")
			{
				return true;
			}
			IPGlobalProperties ipglobalProperties = IPGlobalProperties.InternalGetIPGlobalProperties();
			string text = ipglobalProperties.HostName.ToLowerInvariant();
			return a == text || a == text + "." + ipglobalProperties.DomainName.ToLowerInvariant();
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0007FD71 File Offset: 0x0007ED71
		internal static bool IsFatal(Exception exception)
		{
			return exception != null && (exception is OutOfMemoryException || exception is StackOverflowException || exception is ThreadAbortException);
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x0007FD94 File Offset: 0x0007ED94
		internal static IPAddress[] LocalAddresses
		{
			get
			{
				if (NclUtilities.s_AddressChange != null && NclUtilities.s_AddressChange.CheckAndReset())
				{
					return NclUtilities._LocalAddresses = NclUtilities.GetLocalAddresses();
				}
				if (NclUtilities._LocalAddresses != null)
				{
					return NclUtilities._LocalAddresses;
				}
				IPAddress[] result;
				lock (NclUtilities.LocalAddressesLock)
				{
					if (NclUtilities._LocalAddresses != null)
					{
						result = NclUtilities._LocalAddresses;
					}
					else
					{
						NclUtilities.s_AddressChange = new NetworkAddressChangePolled();
						result = (NclUtilities._LocalAddresses = NclUtilities.GetLocalAddresses());
					}
				}
				return result;
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0007FE18 File Offset: 0x0007EE18
		private static IPAddress[] GetLocalAddresses()
		{
			IPAddress[] array;
			if (ComNetOS.IsPostWin2K)
			{
				ArrayList arrayList = new ArrayList(16);
				int num = 0;
				SafeLocalFree safeLocalFree = null;
				GetAdaptersAddressesFlags flags = GetAdaptersAddressesFlags.SkipAnycast | GetAdaptersAddressesFlags.SkipMulticast | GetAdaptersAddressesFlags.SkipDnsServer | GetAdaptersAddressesFlags.SkipFriendlyName;
				uint cb = 0U;
				uint adaptersAddresses = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified, (uint)flags, IntPtr.Zero, SafeLocalFree.Zero, ref cb);
				while (adaptersAddresses == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
						adaptersAddresses = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified, (uint)flags, IntPtr.Zero, safeLocalFree, ref cb);
						if (adaptersAddresses == 0U)
						{
							IpAdapterAddresses ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpAdapterAddresses));
							for (;;)
							{
								if (ipAdapterAddresses.FirstUnicastAddress != IntPtr.Zero)
								{
									UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = SystemUnicastIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstUnicastAddress);
									num += unicastIPAddressInformationCollection.Count;
									arrayList.Add(unicastIPAddressInformationCollection);
								}
								if (ipAdapterAddresses.next == IntPtr.Zero)
								{
									break;
								}
								ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(ipAdapterAddresses.next, typeof(IpAdapterAddresses));
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
				if (adaptersAddresses != 0U && adaptersAddresses != 232U)
				{
					throw new NetworkInformationException((int)adaptersAddresses);
				}
				array = new IPAddress[num];
				uint num2 = 0U;
				using (IEnumerator enumerator = arrayList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						UnicastIPAddressInformationCollection unicastIPAddressInformationCollection2 = (UnicastIPAddressInformationCollection)obj;
						foreach (IPAddressInformation ipaddressInformation in unicastIPAddressInformationCollection2)
						{
							array[(int)((UIntPtr)(num2++))] = ipaddressInformation.Address;
						}
					}
					return array;
				}
			}
			ArrayList arrayList2 = new ArrayList(16);
			int num3 = 0;
			SafeLocalFree safeLocalFree2 = null;
			uint cb2 = 0U;
			uint adaptersInfo = UnsafeNetInfoNativeMethods.GetAdaptersInfo(SafeLocalFree.Zero, ref cb2);
			while (adaptersInfo == 111U)
			{
				try
				{
					safeLocalFree2 = SafeLocalFree.LocalAlloc((int)cb2);
					adaptersInfo = UnsafeNetInfoNativeMethods.GetAdaptersInfo(safeLocalFree2, ref cb2);
					if (adaptersInfo == 0U)
					{
						IpAdapterInfo ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(safeLocalFree2.DangerousGetHandle(), typeof(IpAdapterInfo));
						for (;;)
						{
							IPAddressCollection ipaddressCollection = ipAdapterInfo.ipAddressList.ToIPAddressCollection();
							num3 += ipaddressCollection.Count;
							arrayList2.Add(ipaddressCollection);
							if (ipAdapterInfo.Next == IntPtr.Zero)
							{
								break;
							}
							ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(ipAdapterInfo.Next, typeof(IpAdapterInfo));
						}
					}
				}
				finally
				{
					if (safeLocalFree2 != null)
					{
						safeLocalFree2.Close();
					}
				}
			}
			if (adaptersInfo != 0U && adaptersInfo != 232U)
			{
				throw new NetworkInformationException((int)adaptersInfo);
			}
			array = new IPAddress[num3];
			uint num4 = 0U;
			foreach (object obj2 in arrayList2)
			{
				IPAddressCollection ipaddressCollection2 = (IPAddressCollection)obj2;
				foreach (IPAddress ipaddress in ipaddressCollection2)
				{
					array[(int)((UIntPtr)(num4++))] = ipaddress;
				}
			}
			return array;
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x00080154 File Offset: 0x0007F154
		internal static bool IsAddressLocal(IPAddress ipAddress)
		{
			IPAddress[] localAddresses = NclUtilities.LocalAddresses;
			for (int i = 0; i < localAddresses.Length; i++)
			{
				if (ipAddress.Equals(localAddresses[i], false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00080184 File Offset: 0x0007F184
		private static object LocalAddressesLock
		{
			get
			{
				if (NclUtilities._LocalAddressesLock == null)
				{
					Interlocked.CompareExchange(ref NclUtilities._LocalAddressesLock, new object(), null);
				}
				return NclUtilities._LocalAddressesLock;
			}
		}

		// Token: 0x04001FAC RID: 8108
		private static ContextCallback s_ContextRelativeDemandCallback;

		// Token: 0x04001FAD RID: 8109
		private static IPAddress[] _LocalAddresses;

		// Token: 0x04001FAE RID: 8110
		private static object _LocalAddressesLock;

		// Token: 0x04001FAF RID: 8111
		private static NetworkAddressChangePolled s_AddressChange;
	}
}
