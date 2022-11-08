using System;
using System.Collections;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200039F RID: 927
	public static class Dns
	{
		// Token: 0x06001CEB RID: 7403 RVA: 0x0006E484 File Offset: 0x0006D484
		private static IPHostEntry NativeToHostEntry(IntPtr nativePointer)
		{
			hostent hostent = (hostent)Marshal.PtrToStructure(nativePointer, typeof(hostent));
			IPHostEntry iphostEntry = new IPHostEntry();
			if (hostent.h_name != IntPtr.Zero)
			{
				iphostEntry.HostName = Marshal.PtrToStringAnsi(hostent.h_name);
			}
			ArrayList arrayList = new ArrayList();
			IntPtr intPtr = hostent.h_addr_list;
			nativePointer = Marshal.ReadIntPtr(intPtr);
			while (nativePointer != IntPtr.Zero)
			{
				int newAddress = Marshal.ReadInt32(nativePointer);
				arrayList.Add(new IPAddress(newAddress));
				intPtr = IntPtrHelper.Add(intPtr, IntPtr.Size);
				nativePointer = Marshal.ReadIntPtr(intPtr);
			}
			iphostEntry.AddressList = new IPAddress[arrayList.Count];
			arrayList.CopyTo(iphostEntry.AddressList, 0);
			arrayList.Clear();
			intPtr = hostent.h_aliases;
			nativePointer = Marshal.ReadIntPtr(intPtr);
			while (nativePointer != IntPtr.Zero)
			{
				string value = Marshal.PtrToStringAnsi(nativePointer);
				arrayList.Add(value);
				intPtr = IntPtrHelper.Add(intPtr, IntPtr.Size);
				nativePointer = Marshal.ReadIntPtr(intPtr);
			}
			iphostEntry.Aliases = new string[arrayList.Count];
			arrayList.CopyTo(iphostEntry.Aliases, 0);
			return iphostEntry;
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x0006E5B0 File Offset: 0x0006D5B0
		[Obsolete("GetHostByName is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IPHostEntry GetHostByName(string hostName)
		{
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			Dns.s_DnsPermission.Demand();
			return Dns.InternalGetHostByName(hostName, false);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0006E5D1 File Offset: 0x0006D5D1
		internal static IPHostEntry InternalGetHostByName(string hostName)
		{
			return Dns.InternalGetHostByName(hostName, true);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x0006E5DC File Offset: 0x0006D5DC
		internal static IPHostEntry InternalGetHostByName(string hostName, bool includeIPv6)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "GetHostByName", hostName);
			}
			if (hostName.Length > 126)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_toolong", new object[]
				{
					"hostName",
					126.ToString(NumberFormatInfo.CurrentInfo)
				}));
			}
			IPHostEntry iphostEntry;
			if (Socket.LegacySupportsIPv6 || (includeIPv6 && ComNetOS.IsPostWin2K))
			{
				iphostEntry = Dns.GetAddrInfo(hostName);
			}
			else
			{
				IntPtr intPtr = UnsafeNclNativeMethods.OSSOCK.gethostbyname(hostName);
				if (intPtr == IntPtr.Zero)
				{
					SocketException ex = new SocketException();
					IPAddress ipaddress;
					if (IPAddress.TryParse(hostName, out ipaddress))
					{
						iphostEntry = new IPHostEntry();
						iphostEntry.HostName = ipaddress.ToString();
						iphostEntry.Aliases = new string[0];
						iphostEntry.AddressList = new IPAddress[]
						{
							ipaddress
						};
						if (Logging.On)
						{
							Logging.Exit(Logging.Sockets, "DNS", "GetHostByName", iphostEntry);
						}
						return iphostEntry;
					}
					throw ex;
				}
				else
				{
					iphostEntry = Dns.NativeToHostEntry(intPtr);
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "GetHostByName", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x0006E700 File Offset: 0x0006D700
		[Obsolete("GetHostByAddress is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IPHostEntry GetHostByAddress(string address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "GetHostByAddress", address);
			}
			Dns.s_DnsPermission.Demand();
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			IPHostEntry iphostEntry = Dns.InternalGetHostByAddress(IPAddress.Parse(address), false, true);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "GetHostByAddress", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x0006E76C File Offset: 0x0006D76C
		[Obsolete("GetHostByAddress is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IPHostEntry GetHostByAddress(IPAddress address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "GetHostByAddress", "");
			}
			Dns.s_DnsPermission.Demand();
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			IPHostEntry iphostEntry = Dns.InternalGetHostByAddress(address, false, true);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "GetHostByAddress", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x0006E7D8 File Offset: 0x0006D7D8
		internal static IPHostEntry InternalGetHostByAddress(IPAddress address, bool includeIPv6, bool throwOnFailure)
		{
			SocketError socketError = SocketError.Success;
			Exception ex = null;
			if (Socket.LegacySupportsIPv6 || (includeIPv6 && ComNetOS.IsPostWin2K))
			{
				string name = Dns.TryGetNameInfo(address, out socketError);
				if (socketError == SocketError.Success)
				{
					return Dns.GetAddrInfo(name);
				}
				ex = new SocketException();
			}
			else
			{
				if (address.AddressFamily == AddressFamily.InterNetworkV6)
				{
					throw new SocketException(SocketError.ProtocolNotSupported);
				}
				int num = (int)address.m_Address;
				IntPtr intPtr = UnsafeNclNativeMethods.OSSOCK.gethostbyaddr(ref num, Marshal.SizeOf(typeof(int)), ProtocolFamily.InterNetwork);
				if (intPtr != IntPtr.Zero)
				{
					return Dns.NativeToHostEntry(intPtr);
				}
				ex = new SocketException();
			}
			if (throwOnFailure)
			{
				throw ex;
			}
			IPHostEntry iphostEntry = new IPHostEntry();
			try
			{
				iphostEntry.HostName = address.ToString();
				iphostEntry.Aliases = new string[0];
				iphostEntry.AddressList = new IPAddress[]
				{
					address
				};
			}
			catch
			{
				throw ex;
			}
			return iphostEntry;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x0006E8BC File Offset: 0x0006D8BC
		public static string GetHostName()
		{
			Dns.s_DnsPermission.Demand();
			Socket.InitializeSockets();
			StringBuilder stringBuilder = new StringBuilder(256);
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.gethostname(stringBuilder, 256);
			if (socketError != SocketError.Success)
			{
				throw new SocketException();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x0006E900 File Offset: 0x0006D900
		[Obsolete("Resolve is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IPHostEntry Resolve(string hostName)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "Resolve", hostName);
			}
			Dns.s_DnsPermission.Demand();
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			IPAddress ipaddress;
			IPHostEntry iphostEntry;
			if (Dns.TryParseAsIP(hostName, out ipaddress) && (ipaddress.AddressFamily != AddressFamily.InterNetworkV6 || Socket.LegacySupportsIPv6))
			{
				iphostEntry = Dns.InternalGetHostByAddress(ipaddress, false, false);
			}
			else
			{
				iphostEntry = Dns.InternalGetHostByName(hostName, false);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "Resolve", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x0006E98C File Offset: 0x0006D98C
		internal static IPHostEntry InternalResolveFast(string hostName, int timeout, out bool timedOut)
		{
			timedOut = false;
			if (hostName.Length > 0 && hostName.Length <= 126)
			{
				IPAddress ipaddress;
				if (Dns.TryParseAsIP(hostName, out ipaddress))
				{
					return new IPHostEntry
					{
						HostName = ipaddress.ToString(),
						Aliases = new string[0],
						AddressList = new IPAddress[]
						{
							ipaddress
						}
					};
				}
				if (Socket.OSSupportsIPv6)
				{
					try
					{
						return Dns.GetAddrInfo(hostName);
					}
					catch (Exception)
					{
						goto IL_83;
					}
				}
				IntPtr intPtr = UnsafeNclNativeMethods.OSSOCK.gethostbyname(hostName);
				if (intPtr != IntPtr.Zero)
				{
					return Dns.NativeToHostEntry(intPtr);
				}
			}
			IL_83:
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "InternalResolveFast", null);
			}
			return null;
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x0006EA4C File Offset: 0x0006DA4C
		private static void ResolveCallback(object context)
		{
			Dns.ResolveAsyncResult resolveAsyncResult = (Dns.ResolveAsyncResult)context;
			IPHostEntry result;
			try
			{
				if (resolveAsyncResult.address != null)
				{
					result = Dns.InternalGetHostByAddress(resolveAsyncResult.address, resolveAsyncResult.includeIPv6, false);
				}
				else
				{
					result = Dns.InternalGetHostByName(resolveAsyncResult.hostName, resolveAsyncResult.includeIPv6);
				}
			}
			catch (Exception ex)
			{
				if (ex is OutOfMemoryException || ex is ThreadAbortException || ex is StackOverflowException)
				{
					throw;
				}
				resolveAsyncResult.InvokeCallback(ex);
				return;
			}
			resolveAsyncResult.InvokeCallback(result);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x0006EACC File Offset: 0x0006DACC
		private static IAsyncResult HostResolutionBeginHelper(string hostName, bool useGetHostByName, bool flowContext, bool includeIPv6, bool throwOnIPAny, AsyncCallback requestCallback, object state)
		{
			Dns.s_DnsPermission.Demand();
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			IPAddress ipaddress;
			Dns.ResolveAsyncResult resolveAsyncResult;
			if (Dns.TryParseAsIP(hostName, out ipaddress))
			{
				if (throwOnIPAny && (ipaddress.Equals(IPAddress.Any) || ipaddress.Equals(IPAddress.IPv6Any)))
				{
					throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "hostNameOrAddress");
				}
				resolveAsyncResult = new Dns.ResolveAsyncResult(ipaddress, null, includeIPv6, state, requestCallback);
				if (useGetHostByName)
				{
					IPHostEntry iphostEntry = new IPHostEntry();
					iphostEntry.AddressList = new IPAddress[]
					{
						ipaddress
					};
					iphostEntry.Aliases = new string[0];
					iphostEntry.HostName = ipaddress.ToString();
					resolveAsyncResult.StartPostingAsyncOp(false);
					resolveAsyncResult.InvokeCallback(iphostEntry);
					resolveAsyncResult.FinishPostingAsyncOp();
					return resolveAsyncResult;
				}
			}
			else
			{
				resolveAsyncResult = new Dns.ResolveAsyncResult(hostName, null, includeIPv6, state, requestCallback);
			}
			if (flowContext)
			{
				resolveAsyncResult.StartPostingAsyncOp(false);
			}
			ThreadPool.UnsafeQueueUserWorkItem(Dns.resolveCallback, resolveAsyncResult);
			resolveAsyncResult.FinishPostingAsyncOp();
			return resolveAsyncResult;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x0006EBB8 File Offset: 0x0006DBB8
		private static IAsyncResult HostResolutionBeginHelper(IPAddress address, bool flowContext, bool includeIPv6, AsyncCallback requestCallback, object state)
		{
			Dns.s_DnsPermission.Demand();
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
			{
				throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "address");
			}
			Dns.ResolveAsyncResult resolveAsyncResult = new Dns.ResolveAsyncResult(address, null, includeIPv6, state, requestCallback);
			if (flowContext)
			{
				resolveAsyncResult.StartPostingAsyncOp(false);
			}
			ThreadPool.UnsafeQueueUserWorkItem(Dns.resolveCallback, resolveAsyncResult);
			resolveAsyncResult.FinishPostingAsyncOp();
			return resolveAsyncResult;
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x0006EC38 File Offset: 0x0006DC38
		private static IPHostEntry HostResolutionEndHelper(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Dns.ResolveAsyncResult resolveAsyncResult = asyncResult as Dns.ResolveAsyncResult;
			if (resolveAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (resolveAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndResolve"
				}));
			}
			resolveAsyncResult.InternalWaitForCompletion();
			resolveAsyncResult.EndCalled = true;
			Exception ex = resolveAsyncResult.Result as Exception;
			if (ex != null)
			{
				throw ex;
			}
			return (IPHostEntry)resolveAsyncResult.Result;
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x0006ECC4 File Offset: 0x0006DCC4
		[Obsolete("BeginGetHostByName is obsoleted for this type, please use BeginGetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static IAsyncResult BeginGetHostByName(string hostName, AsyncCallback requestCallback, object stateObject)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "BeginGetHostByName", hostName);
			}
			IAsyncResult asyncResult = Dns.HostResolutionBeginHelper(hostName, true, true, false, false, requestCallback, stateObject);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "BeginGetHostByName", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x0006ED18 File Offset: 0x0006DD18
		[Obsolete("EndGetHostByName is obsoleted for this type, please use EndGetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IPHostEntry EndGetHostByName(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "EndGetHostByName", asyncResult);
			}
			IPHostEntry iphostEntry = Dns.HostResolutionEndHelper(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "EndGetHostByName", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x0006ED68 File Offset: 0x0006DD68
		public static IPHostEntry GetHostEntry(string hostNameOrAddress)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "GetHostEntry", hostNameOrAddress);
			}
			Dns.s_DnsPermission.Demand();
			if (hostNameOrAddress == null)
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			IPAddress ipaddress;
			IPHostEntry iphostEntry;
			if (Dns.TryParseAsIP(hostNameOrAddress, out ipaddress))
			{
				if (ipaddress.Equals(IPAddress.Any) || ipaddress.Equals(IPAddress.IPv6Any))
				{
					throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "hostNameOrAddress");
				}
				iphostEntry = Dns.InternalGetHostByAddress(ipaddress, true, false);
			}
			else
			{
				iphostEntry = Dns.InternalGetHostByName(hostNameOrAddress, true);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "GetHostEntry", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x0006EE14 File Offset: 0x0006DE14
		public static IPHostEntry GetHostEntry(IPAddress address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "GetHostEntry", "");
			}
			Dns.s_DnsPermission.Demand();
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
			{
				throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "address");
			}
			IPHostEntry iphostEntry = Dns.InternalGetHostByAddress(address, true, false);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "GetHostEntry", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x0006EEB0 File Offset: 0x0006DEB0
		public static IPAddress[] GetHostAddresses(string hostNameOrAddress)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "GetHostAddresses", hostNameOrAddress);
			}
			Dns.s_DnsPermission.Demand();
			if (hostNameOrAddress == null)
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			IPAddress ipaddress;
			IPAddress[] array;
			if (Dns.TryParseAsIP(hostNameOrAddress, out ipaddress))
			{
				if (ipaddress.Equals(IPAddress.Any) || ipaddress.Equals(IPAddress.IPv6Any))
				{
					throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "hostNameOrAddress");
				}
				array = new IPAddress[]
				{
					ipaddress
				};
			}
			else
			{
				array = Dns.InternalGetHostByName(hostNameOrAddress, true).AddressList;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "GetHostAddresses", array);
			}
			return array;
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x0006EF64 File Offset: 0x0006DF64
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static IAsyncResult BeginGetHostEntry(string hostNameOrAddress, AsyncCallback requestCallback, object stateObject)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "BeginGetHostEntry", hostNameOrAddress);
			}
			IAsyncResult asyncResult = Dns.HostResolutionBeginHelper(hostNameOrAddress, false, true, true, true, requestCallback, stateObject);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "BeginGetHostEntry", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x0006EFB8 File Offset: 0x0006DFB8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static IAsyncResult BeginGetHostEntry(IPAddress address, AsyncCallback requestCallback, object stateObject)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "BeginGetHostEntry", address);
			}
			IAsyncResult asyncResult = Dns.HostResolutionBeginHelper(address, true, true, requestCallback, stateObject);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "BeginGetHostEntry", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x0006F00C File Offset: 0x0006E00C
		public static IPHostEntry EndGetHostEntry(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "EndGetHostEntry", asyncResult);
			}
			IPHostEntry iphostEntry = Dns.HostResolutionEndHelper(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "EndGetHostEntry", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x0006F05C File Offset: 0x0006E05C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static IAsyncResult BeginGetHostAddresses(string hostNameOrAddress, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "BeginGetHostAddresses", hostNameOrAddress);
			}
			IAsyncResult asyncResult = Dns.HostResolutionBeginHelper(hostNameOrAddress, true, true, true, true, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "BeginGetHostAddresses", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0006F0B0 File Offset: 0x0006E0B0
		public static IPAddress[] EndGetHostAddresses(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "EndGetHostAddresses", asyncResult);
			}
			IPHostEntry iphostEntry = Dns.HostResolutionEndHelper(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "EndGetHostAddresses", iphostEntry);
			}
			return iphostEntry.AddressList;
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x0006F104 File Offset: 0x0006E104
		internal static IAsyncResult UnsafeBeginGetHostAddresses(string hostName, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "UnsafeBeginGetHostAddresses", hostName);
			}
			IAsyncResult asyncResult = Dns.HostResolutionBeginHelper(hostName, true, false, true, true, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "UnsafeBeginGetHostAddresses", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x0006F158 File Offset: 0x0006E158
		[Obsolete("BeginResolve is obsoleted for this type, please use BeginGetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static IAsyncResult BeginResolve(string hostName, AsyncCallback requestCallback, object stateObject)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "BeginResolve", hostName);
			}
			IAsyncResult asyncResult = Dns.HostResolutionBeginHelper(hostName, false, true, false, false, requestCallback, stateObject);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "BeginResolve", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0006F1AC File Offset: 0x0006E1AC
		[Obsolete("EndResolve is obsoleted for this type, please use EndGetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IPHostEntry EndResolve(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "DNS", "EndResolve", asyncResult);
			}
			IPHostEntry iphostEntry = Dns.HostResolutionEndHelper(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "DNS", "EndResolve", iphostEntry);
			}
			return iphostEntry;
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x0006F1FC File Offset: 0x0006E1FC
		private unsafe static IPHostEntry GetAddrInfo(string name)
		{
			if (!ComNetOS.IsPostWin2K)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			SafeFreeAddrInfo safeFreeAddrInfo = null;
			ArrayList arrayList = new ArrayList();
			string text = null;
			AddressInfo addressInfo = default(AddressInfo);
			addressInfo.ai_flags = AddressInfoHints.AI_CANONNAME;
			addressInfo.ai_family = AddressFamily.Unspecified;
			try
			{
				int addrInfo = SafeFreeAddrInfo.GetAddrInfo(name, null, ref addressInfo, out safeFreeAddrInfo);
				if (addrInfo != 0)
				{
					throw new SocketException();
				}
				for (AddressInfo* ptr = (AddressInfo*)((void*)safeFreeAddrInfo.DangerousGetHandle()); ptr != null; ptr = ptr->ai_next)
				{
					if (text == null && ptr->ai_canonname != null)
					{
						text = new string(ptr->ai_canonname);
					}
					if ((ptr->ai_family == AddressFamily.InterNetwork && Socket.SupportsIPv4) || (ptr->ai_family == AddressFamily.InterNetworkV6 && Socket.OSSupportsIPv6))
					{
						SocketAddress socketAddress = new SocketAddress(ptr->ai_family, ptr->ai_addrlen);
						for (int i = 0; i < ptr->ai_addrlen; i++)
						{
							socketAddress.m_Buffer[i] = ptr->ai_addr[i];
						}
						if (ptr->ai_family == AddressFamily.InterNetwork)
						{
							arrayList.Add(((IPEndPoint)IPEndPoint.Any.Create(socketAddress)).Address);
						}
						else
						{
							arrayList.Add(((IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress)).Address);
						}
					}
				}
			}
			finally
			{
				if (safeFreeAddrInfo != null)
				{
					safeFreeAddrInfo.Close();
				}
			}
			IPHostEntry iphostEntry = new IPHostEntry();
			iphostEntry.HostName = ((text != null) ? text : name);
			iphostEntry.Aliases = new string[0];
			iphostEntry.AddressList = new IPAddress[arrayList.Count];
			arrayList.CopyTo(iphostEntry.AddressList);
			return iphostEntry;
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x0006F3B0 File Offset: 0x0006E3B0
		internal static string TryGetNameInfo(IPAddress addr, out SocketError errorCode)
		{
			if (!ComNetOS.IsPostWin2K)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			SocketAddress socketAddress = new IPEndPoint(addr, 0).Serialize();
			StringBuilder stringBuilder = new StringBuilder(1025);
			Socket.InitializeSockets();
			errorCode = UnsafeNclNativeMethods.OSSOCK.getnameinfo(socketAddress.m_Buffer, socketAddress.m_Size, stringBuilder, stringBuilder.Capacity, null, 0, 4);
			if (errorCode != SocketError.Success)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x0006F415 File Offset: 0x0006E415
		private static bool TryParseAsIP(string address, out IPAddress ip)
		{
			return IPAddress.TryParse(address, out ip) && ((ip.AddressFamily == AddressFamily.InterNetwork && Socket.SupportsIPv4) || (ip.AddressFamily == AddressFamily.InterNetworkV6 && Socket.OSSupportsIPv6));
		}

		// Token: 0x04001D53 RID: 7507
		private const int HostNameBufferLength = 256;

		// Token: 0x04001D54 RID: 7508
		private const int MaxHostName = 126;

		// Token: 0x04001D55 RID: 7509
		private static DnsPermission s_DnsPermission = new DnsPermission(PermissionState.Unrestricted);

		// Token: 0x04001D56 RID: 7510
		private static WaitCallback resolveCallback = new WaitCallback(Dns.ResolveCallback);

		// Token: 0x020003A4 RID: 932
		private class ResolveAsyncResult : ContextAwareResult
		{
			// Token: 0x06001D35 RID: 7477 RVA: 0x0006FC9B File Offset: 0x0006EC9B
			internal ResolveAsyncResult(string hostName, object myObject, bool includeIPv6, object myState, AsyncCallback myCallBack) : base(myObject, myState, myCallBack)
			{
				this.hostName = hostName;
				this.includeIPv6 = includeIPv6;
			}

			// Token: 0x06001D36 RID: 7478 RVA: 0x0006FCB6 File Offset: 0x0006ECB6
			internal ResolveAsyncResult(IPAddress address, object myObject, bool includeIPv6, object myState, AsyncCallback myCallBack) : base(myObject, myState, myCallBack)
			{
				this.includeIPv6 = includeIPv6;
				this.address = address;
			}

			// Token: 0x04001D6F RID: 7535
			internal readonly string hostName;

			// Token: 0x04001D70 RID: 7536
			internal bool includeIPv6;

			// Token: 0x04001D71 RID: 7537
			internal IPAddress address;
		}
	}
}
