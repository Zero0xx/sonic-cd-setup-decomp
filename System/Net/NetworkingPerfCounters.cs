using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Net
{
	// Token: 0x020004F3 RID: 1267
	internal static class NetworkingPerfCounters
	{
		// Token: 0x0600279B RID: 10139 RVA: 0x000A30A4 File Offset: 0x000A20A4
		internal static void Initialize()
		{
			if (!NetworkingPerfCounters.initialized)
			{
				lock (NetworkingPerfCounters.syncObject)
				{
					if (!NetworkingPerfCounters.initialized)
					{
						if (ComNetOS.IsWin2K)
						{
							PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PermissionState.Unrestricted);
							performanceCounterPermission.Assert();
							try
							{
								string instanceName = NetworkingPerfCounters.GetInstanceName();
								NetworkingPerfCounters.ConnectionsEstablished = new PerformanceCounter();
								NetworkingPerfCounters.ConnectionsEstablished.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.ConnectionsEstablished.CounterName = "Connections Established";
								NetworkingPerfCounters.ConnectionsEstablished.InstanceName = instanceName;
								NetworkingPerfCounters.ConnectionsEstablished.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.ConnectionsEstablished.ReadOnly = false;
								NetworkingPerfCounters.ConnectionsEstablished.RawValue = 0L;
								NetworkingPerfCounters.BytesReceived = new PerformanceCounter();
								NetworkingPerfCounters.BytesReceived.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.BytesReceived.CounterName = "Bytes Received";
								NetworkingPerfCounters.BytesReceived.InstanceName = instanceName;
								NetworkingPerfCounters.BytesReceived.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.BytesReceived.ReadOnly = false;
								NetworkingPerfCounters.BytesReceived.RawValue = 0L;
								NetworkingPerfCounters.BytesSent = new PerformanceCounter();
								NetworkingPerfCounters.BytesSent.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.BytesSent.CounterName = "Bytes Sent";
								NetworkingPerfCounters.BytesSent.InstanceName = instanceName;
								NetworkingPerfCounters.BytesSent.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.BytesSent.ReadOnly = false;
								NetworkingPerfCounters.BytesSent.RawValue = 0L;
								NetworkingPerfCounters.DatagramsReceived = new PerformanceCounter();
								NetworkingPerfCounters.DatagramsReceived.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.DatagramsReceived.CounterName = "Datagrams Received";
								NetworkingPerfCounters.DatagramsReceived.InstanceName = instanceName;
								NetworkingPerfCounters.DatagramsReceived.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.DatagramsReceived.ReadOnly = false;
								NetworkingPerfCounters.DatagramsReceived.RawValue = 0L;
								NetworkingPerfCounters.DatagramsSent = new PerformanceCounter();
								NetworkingPerfCounters.DatagramsSent.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.DatagramsSent.CounterName = "Datagrams Sent";
								NetworkingPerfCounters.DatagramsSent.InstanceName = instanceName;
								NetworkingPerfCounters.DatagramsSent.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.DatagramsSent.ReadOnly = false;
								NetworkingPerfCounters.DatagramsSent.RawValue = 0L;
								NetworkingPerfCounters.globalConnectionsEstablished = new PerformanceCounter(".NET CLR Networking", "Connections Established", "_Global_", false);
								NetworkingPerfCounters.globalBytesReceived = new PerformanceCounter(".NET CLR Networking", "Bytes Received", "_Global_", false);
								NetworkingPerfCounters.globalBytesSent = new PerformanceCounter(".NET CLR Networking", "Bytes Sent", "_Global_", false);
								NetworkingPerfCounters.globalDatagramsReceived = new PerformanceCounter(".NET CLR Networking", "Datagrams Received", "_Global_", false);
								NetworkingPerfCounters.globalDatagramsSent = new PerformanceCounter(".NET CLR Networking", "Datagrams Sent", "_Global_", false);
								AppDomain.CurrentDomain.DomainUnload += NetworkingPerfCounters.ExitOrUnloadEventHandler;
								AppDomain.CurrentDomain.ProcessExit += NetworkingPerfCounters.ExitOrUnloadEventHandler;
								AppDomain.CurrentDomain.UnhandledException += NetworkingPerfCounters.ExceptionEventHandler;
							}
							catch (Win32Exception)
							{
							}
							catch (InvalidOperationException)
							{
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
						NetworkingPerfCounters.initialized = true;
					}
				}
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000A33E0 File Offset: 0x000A23E0
		private static void ExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				NetworkingPerfCounters.Cleanup();
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000A33EF File Offset: 0x000A23EF
		private static void ExitOrUnloadEventHandler(object sender, EventArgs e)
		{
			NetworkingPerfCounters.Cleanup();
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000A33F8 File Offset: 0x000A23F8
		private static void Cleanup()
		{
			PerformanceCounter performanceCounter = NetworkingPerfCounters.ConnectionsEstablished;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.BytesReceived;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.BytesSent;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.DatagramsReceived;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.DatagramsSent;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000A3450 File Offset: 0x000A2450
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private static string GetAssemblyName()
		{
			string result = null;
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				AssemblyName name = entryAssembly.GetName();
				if (name != null)
				{
					result = name.Name;
				}
			}
			return result;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000A347C File Offset: 0x000A247C
		[SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
		private static string GetInstanceName()
		{
			string text = NetworkingPerfCounters.GetAssemblyName();
			if (text == null || text.Length == 0)
			{
				text = AppDomain.CurrentDomain.FriendlyName;
			}
			StringBuilder stringBuilder = new StringBuilder(text);
			int i = 0;
			while (i < stringBuilder.Length)
			{
				char c = stringBuilder[i];
				if (c <= ')')
				{
					if (c == '#')
					{
						goto IL_76;
					}
					switch (c)
					{
					case '(':
						stringBuilder[i] = '[';
						break;
					case ')':
						stringBuilder[i] = ']';
						break;
					}
				}
				else if (c == '/' || c == '\\')
				{
					goto IL_76;
				}
				IL_7F:
				i++;
				continue;
				IL_76:
				stringBuilder[i] = '_';
				goto IL_7F;
			}
			return string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", new object[]
			{
				stringBuilder.ToString(),
				Process.GetCurrentProcess().Id
			});
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000A354D File Offset: 0x000A254D
		internal static void IncrementConnectionsEstablished()
		{
			if (NetworkingPerfCounters.ConnectionsEstablished != null)
			{
				NetworkingPerfCounters.ConnectionsEstablished.Increment();
			}
			if (NetworkingPerfCounters.globalConnectionsEstablished != null)
			{
				NetworkingPerfCounters.globalConnectionsEstablished.Increment();
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000A3573 File Offset: 0x000A2573
		internal static void AddBytesReceived(int increment)
		{
			if (NetworkingPerfCounters.BytesReceived != null)
			{
				NetworkingPerfCounters.BytesReceived.IncrementBy((long)increment);
			}
			if (NetworkingPerfCounters.globalBytesReceived != null)
			{
				NetworkingPerfCounters.globalBytesReceived.IncrementBy((long)increment);
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000A359D File Offset: 0x000A259D
		internal static void AddBytesSent(int increment)
		{
			if (NetworkingPerfCounters.BytesSent != null)
			{
				NetworkingPerfCounters.BytesSent.IncrementBy((long)increment);
			}
			if (NetworkingPerfCounters.globalBytesSent != null)
			{
				NetworkingPerfCounters.globalBytesSent.IncrementBy((long)increment);
			}
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000A35C7 File Offset: 0x000A25C7
		internal static void IncrementDatagramsReceived()
		{
			if (NetworkingPerfCounters.DatagramsReceived != null)
			{
				NetworkingPerfCounters.DatagramsReceived.Increment();
			}
			if (NetworkingPerfCounters.globalDatagramsReceived != null)
			{
				NetworkingPerfCounters.globalDatagramsReceived.Increment();
			}
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000A35ED File Offset: 0x000A25ED
		internal static void IncrementDatagramsSent()
		{
			if (NetworkingPerfCounters.DatagramsSent != null)
			{
				NetworkingPerfCounters.DatagramsSent.Increment();
			}
			if (NetworkingPerfCounters.globalDatagramsSent != null)
			{
				NetworkingPerfCounters.globalDatagramsSent.Increment();
			}
		}

		// Token: 0x040026BD RID: 9917
		private const string CategoryName = ".NET CLR Networking";

		// Token: 0x040026BE RID: 9918
		private const string ConnectionsEstablishedName = "Connections Established";

		// Token: 0x040026BF RID: 9919
		private const string BytesReceivedName = "Bytes Received";

		// Token: 0x040026C0 RID: 9920
		private const string BytesSentName = "Bytes Sent";

		// Token: 0x040026C1 RID: 9921
		private const string DatagramsReceivedName = "Datagrams Received";

		// Token: 0x040026C2 RID: 9922
		private const string DatagramsSentName = "Datagrams Sent";

		// Token: 0x040026C3 RID: 9923
		private const string GlobalInstanceName = "_Global_";

		// Token: 0x040026C4 RID: 9924
		private static PerformanceCounter ConnectionsEstablished;

		// Token: 0x040026C5 RID: 9925
		private static PerformanceCounter BytesReceived;

		// Token: 0x040026C6 RID: 9926
		private static PerformanceCounter BytesSent;

		// Token: 0x040026C7 RID: 9927
		private static PerformanceCounter DatagramsReceived;

		// Token: 0x040026C8 RID: 9928
		private static PerformanceCounter DatagramsSent;

		// Token: 0x040026C9 RID: 9929
		private static PerformanceCounter globalConnectionsEstablished;

		// Token: 0x040026CA RID: 9930
		private static PerformanceCounter globalBytesReceived;

		// Token: 0x040026CB RID: 9931
		private static PerformanceCounter globalBytesSent;

		// Token: 0x040026CC RID: 9932
		private static PerformanceCounter globalDatagramsReceived;

		// Token: 0x040026CD RID: 9933
		private static PerformanceCounter globalDatagramsSent;

		// Token: 0x040026CE RID: 9934
		private static object syncObject = new object();

		// Token: 0x040026CF RID: 9935
		private static bool initialized = false;
	}
}
