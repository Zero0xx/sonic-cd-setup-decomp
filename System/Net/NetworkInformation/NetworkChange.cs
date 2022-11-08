using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Threading;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000611 RID: 1553
	public sealed class NetworkChange
	{
		// Token: 0x06002FE8 RID: 12264 RVA: 0x000CF1B6 File Offset: 0x000CE1B6
		private NetworkChange()
		{
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06002FE9 RID: 12265 RVA: 0x000CF1BE File Offset: 0x000CE1BE
		// (remove) Token: 0x06002FEA RID: 12266 RVA: 0x000CF1DD File Offset: 0x000CE1DD
		public static event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
		{
			add
			{
				if (!ComNetOS.IsWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
				}
				NetworkChange.AvailabilityChangeListener.Start(value);
			}
			remove
			{
				NetworkChange.AvailabilityChangeListener.Stop(value);
			}
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06002FEB RID: 12267 RVA: 0x000CF1E5 File Offset: 0x000CE1E5
		// (remove) Token: 0x06002FEC RID: 12268 RVA: 0x000CF204 File Offset: 0x000CE204
		public static event NetworkAddressChangedEventHandler NetworkAddressChanged
		{
			add
			{
				if (!ComNetOS.IsWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
				}
				NetworkChange.AddressChangeListener.Start(value);
			}
			remove
			{
				NetworkChange.AddressChangeListener.Stop(value);
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06002FED RID: 12269 RVA: 0x000CF20C File Offset: 0x000CE20C
		internal static bool CanListenForNetworkChanges
		{
			get
			{
				return ComNetOS.IsWin2K;
			}
		}

		// Token: 0x02000612 RID: 1554
		internal static class AvailabilityChangeListener
		{
			// Token: 0x06002FEE RID: 12270 RVA: 0x000CF218 File Offset: 0x000CE218
			private static void RunHandlerCallback(object state)
			{
				((NetworkAvailabilityChangedEventHandler)state)(null, new NetworkAvailabilityEventArgs(NetworkChange.AvailabilityChangeListener.isAvailable));
			}

			// Token: 0x06002FEF RID: 12271 RVA: 0x000CF230 File Offset: 0x000CE230
			private static void ChangedAddress(object sender, EventArgs eventArgs)
			{
				lock (NetworkChange.AvailabilityChangeListener.syncObject)
				{
					bool flag = SystemNetworkInterface.InternalGetIsNetworkAvailable();
					if (flag != NetworkChange.AvailabilityChangeListener.isAvailable)
					{
						NetworkChange.AvailabilityChangeListener.isAvailable = flag;
						DictionaryEntry[] array = new DictionaryEntry[NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Count];
						NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.CopyTo(array, 0);
						for (int i = 0; i < array.Length; i++)
						{
							NetworkAvailabilityChangedEventHandler networkAvailabilityChangedEventHandler = (NetworkAvailabilityChangedEventHandler)array[i].Key;
							ExecutionContext executionContext = (ExecutionContext)array[i].Value;
							if (executionContext == null)
							{
								networkAvailabilityChangedEventHandler(null, new NetworkAvailabilityEventArgs(NetworkChange.AvailabilityChangeListener.isAvailable));
							}
							else
							{
								ExecutionContext.Run(executionContext.CreateCopy(), NetworkChange.AvailabilityChangeListener.s_RunHandlerCallback, networkAvailabilityChangedEventHandler);
							}
						}
					}
				}
			}

			// Token: 0x06002FF0 RID: 12272 RVA: 0x000CF2F0 File Offset: 0x000CE2F0
			internal static void Start(NetworkAvailabilityChangedEventHandler caller)
			{
				lock (NetworkChange.AvailabilityChangeListener.syncObject)
				{
					if (NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray == null)
					{
						NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray = new ListDictionary();
						NetworkChange.AvailabilityChangeListener.addressChange = new NetworkAddressChangedEventHandler(NetworkChange.AvailabilityChangeListener.ChangedAddress);
					}
					if (NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Count == 0)
					{
						NetworkChange.AvailabilityChangeListener.isAvailable = NetworkInterface.GetIsNetworkAvailable();
						NetworkChange.AddressChangeListener.UnsafeStart(NetworkChange.AvailabilityChangeListener.addressChange);
					}
					if (caller != null && !NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Contains(caller))
					{
						NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Add(caller, ExecutionContext.Capture());
					}
				}
			}

			// Token: 0x06002FF1 RID: 12273 RVA: 0x000CF384 File Offset: 0x000CE384
			internal static void Stop(NetworkAvailabilityChangedEventHandler caller)
			{
				lock (NetworkChange.AvailabilityChangeListener.syncObject)
				{
					NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Remove(caller);
					if (NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Count == 0)
					{
						NetworkChange.AddressChangeListener.Stop(NetworkChange.AvailabilityChangeListener.addressChange);
					}
				}
			}

			// Token: 0x04002DBF RID: 11711
			private static object syncObject = new object();

			// Token: 0x04002DC0 RID: 11712
			private static ListDictionary s_availabilityCallerArray = null;

			// Token: 0x04002DC1 RID: 11713
			private static NetworkAddressChangedEventHandler addressChange = null;

			// Token: 0x04002DC2 RID: 11714
			private static bool isAvailable = false;

			// Token: 0x04002DC3 RID: 11715
			private static ContextCallback s_RunHandlerCallback = new ContextCallback(NetworkChange.AvailabilityChangeListener.RunHandlerCallback);
		}

		// Token: 0x02000613 RID: 1555
		internal static class AddressChangeListener
		{
			// Token: 0x06002FF3 RID: 12275 RVA: 0x000CF408 File Offset: 0x000CE408
			private static void AddressChangedCallback(object stateObject, bool signaled)
			{
				lock (NetworkChange.AddressChangeListener.s_callerArray)
				{
					NetworkChange.AddressChangeListener.s_isPending = false;
					if (NetworkChange.AddressChangeListener.s_isListening)
					{
						NetworkChange.AddressChangeListener.s_isListening = false;
						DictionaryEntry[] array = new DictionaryEntry[NetworkChange.AddressChangeListener.s_callerArray.Count];
						NetworkChange.AddressChangeListener.s_callerArray.CopyTo(array, 0);
						NetworkChange.AddressChangeListener.StartHelper(null, false, (StartIPOptions)stateObject);
						for (int i = 0; i < array.Length; i++)
						{
							NetworkAddressChangedEventHandler networkAddressChangedEventHandler = (NetworkAddressChangedEventHandler)array[i].Key;
							ExecutionContext executionContext = (ExecutionContext)array[i].Value;
							if (executionContext == null)
							{
								networkAddressChangedEventHandler(null, EventArgs.Empty);
							}
							else
							{
								ExecutionContext.Run(executionContext.CreateCopy(), NetworkChange.AddressChangeListener.s_runHandlerCallback, networkAddressChangedEventHandler);
							}
						}
					}
				}
			}

			// Token: 0x06002FF4 RID: 12276 RVA: 0x000CF4D4 File Offset: 0x000CE4D4
			private static void RunHandlerCallback(object state)
			{
				((NetworkAddressChangedEventHandler)state)(null, EventArgs.Empty);
			}

			// Token: 0x06002FF5 RID: 12277 RVA: 0x000CF4E7 File Offset: 0x000CE4E7
			internal static void Start(NetworkAddressChangedEventHandler caller)
			{
				NetworkChange.AddressChangeListener.StartHelper(caller, true, StartIPOptions.Both);
			}

			// Token: 0x06002FF6 RID: 12278 RVA: 0x000CF4F1 File Offset: 0x000CE4F1
			internal static void UnsafeStart(NetworkAddressChangedEventHandler caller)
			{
				NetworkChange.AddressChangeListener.StartHelper(caller, false, StartIPOptions.Both);
			}

			// Token: 0x06002FF7 RID: 12279 RVA: 0x000CF4FC File Offset: 0x000CE4FC
			private static void StartHelper(NetworkAddressChangedEventHandler caller, bool captureContext, StartIPOptions startIPOptions)
			{
				lock (NetworkChange.AddressChangeListener.s_callerArray)
				{
					if (NetworkChange.AddressChangeListener.s_ipv4Socket == null)
					{
						Socket.InitializeSockets();
						if (Socket.SupportsIPv4)
						{
							int num = -1;
							NetworkChange.AddressChangeListener.s_ipv4Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP, true, false);
							UnsafeNclNativeMethods.OSSOCK.ioctlsocket(NetworkChange.AddressChangeListener.s_ipv4Socket, -2147195266, ref num);
							NetworkChange.AddressChangeListener.s_ipv4WaitHandle = NetworkChange.AddressChangeListener.s_ipv4Socket.GetEventHandle();
						}
						if (Socket.OSSupportsIPv6)
						{
							int num = -1;
							NetworkChange.AddressChangeListener.s_ipv6Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.IP, true, false);
							UnsafeNclNativeMethods.OSSOCK.ioctlsocket(NetworkChange.AddressChangeListener.s_ipv6Socket, -2147195266, ref num);
							NetworkChange.AddressChangeListener.s_ipv6WaitHandle = NetworkChange.AddressChangeListener.s_ipv6Socket.GetEventHandle();
						}
					}
					if (caller != null && !NetworkChange.AddressChangeListener.s_callerArray.Contains(caller))
					{
						NetworkChange.AddressChangeListener.s_callerArray.Add(caller, captureContext ? ExecutionContext.Capture() : null);
					}
					if (!NetworkChange.AddressChangeListener.s_isListening && NetworkChange.AddressChangeListener.s_callerArray.Count != 0)
					{
						if (!NetworkChange.AddressChangeListener.s_isPending)
						{
							if (Socket.SupportsIPv4 && (startIPOptions & StartIPOptions.StartIPv4) != StartIPOptions.None)
							{
								NetworkChange.AddressChangeListener.s_registeredWait = ThreadPool.UnsafeRegisterWaitForSingleObject(NetworkChange.AddressChangeListener.s_ipv4WaitHandle, new WaitOrTimerCallback(NetworkChange.AddressChangeListener.AddressChangedCallback), StartIPOptions.StartIPv4, -1, true);
								int num2;
								SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(NetworkChange.AddressChangeListener.s_ipv4Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num2, SafeNativeOverlapped.Zero, IntPtr.Zero);
								if (socketError != SocketError.Success)
								{
									NetworkInformationException ex = new NetworkInformationException();
									if ((long)ex.ErrorCode != 10035L)
									{
										throw ex;
									}
								}
								socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(NetworkChange.AddressChangeListener.s_ipv4Socket, NetworkChange.AddressChangeListener.s_ipv4Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
								if (socketError != SocketError.Success)
								{
									throw new NetworkInformationException();
								}
							}
							if (Socket.OSSupportsIPv6 && (startIPOptions & StartIPOptions.StartIPv6) != StartIPOptions.None)
							{
								NetworkChange.AddressChangeListener.s_registeredWait = ThreadPool.UnsafeRegisterWaitForSingleObject(NetworkChange.AddressChangeListener.s_ipv6WaitHandle, new WaitOrTimerCallback(NetworkChange.AddressChangeListener.AddressChangedCallback), StartIPOptions.StartIPv6, -1, true);
								int num2;
								SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(NetworkChange.AddressChangeListener.s_ipv6Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num2, SafeNativeOverlapped.Zero, IntPtr.Zero);
								if (socketError != SocketError.Success)
								{
									NetworkInformationException ex2 = new NetworkInformationException();
									if ((long)ex2.ErrorCode != 10035L)
									{
										throw ex2;
									}
								}
								socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(NetworkChange.AddressChangeListener.s_ipv6Socket, NetworkChange.AddressChangeListener.s_ipv6Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
								if (socketError != SocketError.Success)
								{
									throw new NetworkInformationException();
								}
							}
						}
						NetworkChange.AddressChangeListener.s_isListening = true;
						NetworkChange.AddressChangeListener.s_isPending = true;
					}
				}
			}

			// Token: 0x06002FF8 RID: 12280 RVA: 0x000CF74C File Offset: 0x000CE74C
			internal static void Stop(object caller)
			{
				lock (NetworkChange.AddressChangeListener.s_callerArray)
				{
					NetworkChange.AddressChangeListener.s_callerArray.Remove(caller);
					if (NetworkChange.AddressChangeListener.s_callerArray.Count == 0 && NetworkChange.AddressChangeListener.s_isListening)
					{
						NetworkChange.AddressChangeListener.s_isListening = false;
					}
				}
			}

			// Token: 0x04002DC4 RID: 11716
			private static ListDictionary s_callerArray = new ListDictionary();

			// Token: 0x04002DC5 RID: 11717
			private static ContextCallback s_runHandlerCallback = new ContextCallback(NetworkChange.AddressChangeListener.RunHandlerCallback);

			// Token: 0x04002DC6 RID: 11718
			private static RegisteredWaitHandle s_registeredWait;

			// Token: 0x04002DC7 RID: 11719
			private static bool s_isListening = false;

			// Token: 0x04002DC8 RID: 11720
			private static bool s_isPending = false;

			// Token: 0x04002DC9 RID: 11721
			private static SafeCloseSocketAndEvent s_ipv4Socket = null;

			// Token: 0x04002DCA RID: 11722
			private static SafeCloseSocketAndEvent s_ipv6Socket = null;

			// Token: 0x04002DCB RID: 11723
			private static WaitHandle s_ipv4WaitHandle = null;

			// Token: 0x04002DCC RID: 11724
			private static WaitHandle s_ipv6WaitHandle = null;
		}
	}
}
