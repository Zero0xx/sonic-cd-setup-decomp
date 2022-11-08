using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004AD RID: 1197
	internal class AutoWebProxyScriptEngine
	{
		// Token: 0x060024E4 RID: 9444 RVA: 0x000922F4 File Offset: 0x000912F4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal AutoWebProxyScriptEngine(WebProxy proxy, bool useRegistry)
		{
			this.webProxy = proxy;
			this.m_UseRegistry = useRegistry;
			this.m_AutoDetector = AutoWebProxyScriptEngine.AutoDetector.CurrentAutoDetector;
			this.m_NetworkChangeStatus = this.m_AutoDetector.NetworkChangeStatus;
			SafeRegistryHandle.RegOpenCurrentUser(131097U, out this.hkcu);
			if (this.m_UseRegistry)
			{
				this.ListenForRegistry();
				this.m_Identity = WindowsIdentity.GetCurrent();
			}
			this.webProxyFinder = new HybridWebProxyFinder(this);
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x00092368 File Offset: 0x00091368
		private void EnterLock(ref int syncStatus)
		{
			if (syncStatus == 0)
			{
				lock (this)
				{
					if (syncStatus != 4)
					{
						syncStatus = 1;
						while (this.m_LockHeld)
						{
							Monitor.Wait(this);
							if (syncStatus == 4)
							{
								Monitor.Pulse(this);
								return;
							}
						}
						syncStatus = 2;
						this.m_LockHeld = true;
					}
				}
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000923CC File Offset: 0x000913CC
		private void ExitLock(ref int syncStatus)
		{
			if (syncStatus != 0 && syncStatus != 4)
			{
				lock (this)
				{
					this.m_LockHeld = false;
					if (syncStatus == 3)
					{
						this.webProxyFinder.Reset();
						syncStatus = 4;
					}
					else
					{
						syncStatus = 0;
					}
					Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x00092428 File Offset: 0x00091428
		internal void Abort(ref int syncStatus)
		{
			lock (this)
			{
				switch (syncStatus)
				{
				case 0:
					syncStatus = 4;
					break;
				case 1:
					syncStatus = 4;
					Monitor.PulseAll(this);
					break;
				case 2:
					syncStatus = 3;
					this.webProxyFinder.Abort();
					break;
				}
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x0009248C File Offset: 0x0009148C
		// (set) Token: 0x060024E9 RID: 9449 RVA: 0x00092494 File Offset: 0x00091494
		internal bool AutomaticallyDetectSettings
		{
			get
			{
				return this.automaticallyDetectSettings;
			}
			set
			{
				if (this.automaticallyDetectSettings != value)
				{
					this.automaticallyDetectSettings = value;
					this.webProxyFinder.Reset();
				}
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000924B1 File Offset: 0x000914B1
		// (set) Token: 0x060024EB RID: 9451 RVA: 0x000924B9 File Offset: 0x000914B9
		internal Uri AutomaticConfigurationScript
		{
			get
			{
				return this.automaticConfigurationScript;
			}
			set
			{
				if (!object.Equals(this.automaticConfigurationScript, value))
				{
					this.automaticConfigurationScript = value;
					this.webProxyFinder.Reset();
				}
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000924DB File Offset: 0x000914DB
		internal ICredentials Credentials
		{
			get
			{
				return this.webProxy.Credentials;
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000924E8 File Offset: 0x000914E8
		internal bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			int num = 0;
			return this.GetProxies(destination, out proxyList, ref num);
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00092504 File Offset: 0x00091504
		internal bool GetProxies(Uri destination, out IList<string> proxyList, ref int syncStatus)
		{
			proxyList = null;
			this.CheckForChanges(ref syncStatus);
			if (!this.webProxyFinder.IsValid)
			{
				return false;
			}
			bool result;
			try
			{
				this.EnterLock(ref syncStatus);
				if (syncStatus != 2)
				{
					result = false;
				}
				else
				{
					result = this.webProxyFinder.GetProxies(destination, out proxyList);
				}
			}
			finally
			{
				this.ExitLock(ref syncStatus);
			}
			return result;
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x00092564 File Offset: 0x00091564
		internal WebProxyData GetWebProxyData()
		{
			WebProxyDataBuilder webProxyDataBuilder;
			if (ComNetOS.IsWin7)
			{
				webProxyDataBuilder = new WinHttpWebProxyBuilder();
			}
			else
			{
				webProxyDataBuilder = new RegBlobWebProxyDataBuilder(this.m_AutoDetector.Connectoid, this.hkcu);
			}
			return webProxyDataBuilder.Build();
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000925A0 File Offset: 0x000915A0
		internal void Close()
		{
			if (this.m_AutoDetector != null)
			{
				int num = 0;
				try
				{
					this.EnterLock(ref num);
					if (this.m_AutoDetector != null)
					{
						this.registrySuppress = true;
						if (this.registryChangeEventPolicy != null)
						{
							this.registryChangeEventPolicy.Close();
							this.registryChangeEventPolicy = null;
						}
						if (this.registryChangeEventLM != null)
						{
							this.registryChangeEventLM.Close();
							this.registryChangeEventLM = null;
						}
						if (this.registryChangeEvent != null)
						{
							this.registryChangeEvent.Close();
							this.registryChangeEvent = null;
						}
						if (this.regKeyPolicy != null && !this.regKeyPolicy.IsInvalid)
						{
							this.regKeyPolicy.Close();
						}
						if (this.regKeyLM != null && !this.regKeyLM.IsInvalid)
						{
							this.regKeyLM.Close();
						}
						if (this.regKey != null && !this.regKey.IsInvalid)
						{
							this.regKey.Close();
						}
						if (this.hkcu != null)
						{
							this.hkcu.RegCloseKey();
							this.hkcu = null;
						}
						if (this.m_Identity != null)
						{
							this.m_Identity.Dispose();
							this.m_Identity = null;
						}
						this.webProxyFinder.Dispose();
						this.m_AutoDetector = null;
					}
				}
				finally
				{
					this.ExitLock(ref num);
				}
			}
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000926F0 File Offset: 0x000916F0
		internal void ListenForRegistry()
		{
			if (!this.registrySuppress)
			{
				if (this.registryChangeEvent == null)
				{
					this.ListenForRegistryHelper(ref this.regKey, ref this.registryChangeEvent, IntPtr.Zero, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
				}
				if (this.registryChangeEventLM == null)
				{
					this.ListenForRegistryHelper(ref this.regKeyLM, ref this.registryChangeEventLM, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
				}
				if (this.registryChangeEventPolicy == null)
				{
					this.ListenForRegistryHelper(ref this.regKeyPolicy, ref this.registryChangeEventPolicy, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
				}
				if (this.registryChangeEvent == null && this.registryChangeEventLM == null && this.registryChangeEventPolicy == null)
				{
					this.registrySuppress = true;
				}
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x00092794 File Offset: 0x00091794
		private void ListenForRegistryHelper(ref SafeRegistryHandle key, ref AutoResetEvent changeEvent, IntPtr baseKey, string subKey)
		{
			uint num = 0U;
			if (key == null || key.IsInvalid)
			{
				if (baseKey == IntPtr.Zero)
				{
					if (this.hkcu != null)
					{
						num = this.hkcu.RegOpenKeyEx(subKey, 0U, 131097U, out key);
					}
					else
					{
						num = 1168U;
					}
				}
				else
				{
					num = SafeRegistryHandle.RegOpenKeyEx(baseKey, subKey, 0U, 131097U, out key);
				}
				if (num == 0U)
				{
					changeEvent = new AutoResetEvent(false);
				}
			}
			if (num == 0U)
			{
				num = key.RegNotifyChangeKeyValue(true, 4U, changeEvent.SafeWaitHandle, true);
			}
			if (num != 0U)
			{
				if (key != null && !key.IsInvalid)
				{
					try
					{
						num = key.RegCloseKey();
					}
					catch (Exception exception)
					{
						if (NclUtilities.IsFatal(exception))
						{
							throw;
						}
					}
				}
				key = null;
				if (changeEvent != null)
				{
					changeEvent.Close();
					changeEvent = null;
				}
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x0009285C File Offset: 0x0009185C
		private void RegistryChanged()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_system_setting_update"));
			}
			WebProxyData webProxyData;
			using (this.m_Identity.Impersonate())
			{
				webProxyData = this.GetWebProxyData();
			}
			this.webProxy.Update(webProxyData);
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000928C0 File Offset: 0x000918C0
		private void ConnectoidChanged()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_update_due_to_ip_config_change"));
			}
			this.m_AutoDetector = AutoWebProxyScriptEngine.AutoDetector.CurrentAutoDetector;
			if (this.m_UseRegistry)
			{
				WebProxyData webProxyData;
				using (this.m_Identity.Impersonate())
				{
					webProxyData = this.GetWebProxyData();
				}
				this.webProxy.Update(webProxyData);
			}
			if (this.automaticallyDetectSettings)
			{
				this.webProxyFinder.Reset();
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0009294C File Offset: 0x0009194C
		internal void CheckForChanges()
		{
			int num = 0;
			this.CheckForChanges(ref num);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00092964 File Offset: 0x00091964
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private void CheckForChanges(ref int syncStatus)
		{
			try
			{
				bool flag = AutoWebProxyScriptEngine.AutoDetector.CheckForNetworkChanges(ref this.m_NetworkChangeStatus);
				bool flag2 = false;
				if (!flag)
				{
					if (!this.needConnectoidUpdate)
					{
						goto IL_53;
					}
				}
				try
				{
					this.EnterLock(ref syncStatus);
					if (flag || this.needConnectoidUpdate)
					{
						this.needConnectoidUpdate = (syncStatus != 2);
						if (!this.needConnectoidUpdate)
						{
							this.ConnectoidChanged();
							flag2 = true;
						}
					}
				}
				finally
				{
					this.ExitLock(ref syncStatus);
				}
				IL_53:
				if (this.m_UseRegistry)
				{
					bool flag3 = false;
					AutoResetEvent autoResetEvent = this.registryChangeEvent;
					if (!this.registryChangeDeferred)
					{
						if (!(flag3 = (autoResetEvent != null && autoResetEvent.WaitOne(0, false))))
						{
							goto IL_100;
						}
					}
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag3 || this.registryChangeDeferred)
						{
							this.registryChangeDeferred = (syncStatus != 2);
							if (!this.registryChangeDeferred && this.registryChangeEvent != null)
							{
								try
								{
									using (this.m_Identity.Impersonate())
									{
										this.ListenForRegistryHelper(ref this.regKey, ref this.registryChangeEvent, IntPtr.Zero, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
									}
								}
								catch
								{
									throw;
								}
								this.needRegistryUpdate = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
					IL_100:
					flag3 = false;
					autoResetEvent = this.registryChangeEventLM;
					if (!this.registryChangeLMDeferred)
					{
						if (!(flag3 = (autoResetEvent != null && autoResetEvent.WaitOne(0, false))))
						{
							goto IL_1A0;
						}
					}
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag3 || this.registryChangeLMDeferred)
						{
							this.registryChangeLMDeferred = (syncStatus != 2);
							if (!this.registryChangeLMDeferred && this.registryChangeEventLM != null)
							{
								try
								{
									using (this.m_Identity.Impersonate())
									{
										this.ListenForRegistryHelper(ref this.regKeyLM, ref this.registryChangeEventLM, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
									}
								}
								catch
								{
									throw;
								}
								this.needRegistryUpdate = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
					IL_1A0:
					flag3 = false;
					autoResetEvent = this.registryChangeEventPolicy;
					if (!this.registryChangePolicyDeferred)
					{
						if (!(flag3 = (autoResetEvent != null && autoResetEvent.WaitOne(0, false))))
						{
							goto IL_240;
						}
					}
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag3 || this.registryChangePolicyDeferred)
						{
							this.registryChangePolicyDeferred = (syncStatus != 2);
							if (!this.registryChangePolicyDeferred && this.registryChangeEventPolicy != null)
							{
								try
								{
									using (this.m_Identity.Impersonate())
									{
										this.ListenForRegistryHelper(ref this.regKeyPolicy, ref this.registryChangeEventPolicy, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
									}
								}
								catch
								{
									throw;
								}
								this.needRegistryUpdate = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
					IL_240:
					if (this.needRegistryUpdate)
					{
						try
						{
							this.EnterLock(ref syncStatus);
							if (this.needRegistryUpdate && syncStatus == 2)
							{
								this.needRegistryUpdate = false;
								if (!flag2)
								{
									this.RegistryChanged();
								}
							}
						}
						finally
						{
							this.ExitLock(ref syncStatus);
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x040024DB RID: 9435
		private bool automaticallyDetectSettings;

		// Token: 0x040024DC RID: 9436
		private Uri automaticConfigurationScript;

		// Token: 0x040024DD RID: 9437
		private WebProxy webProxy;

		// Token: 0x040024DE RID: 9438
		private IWebProxyFinder webProxyFinder;

		// Token: 0x040024DF RID: 9439
		private bool m_LockHeld;

		// Token: 0x040024E0 RID: 9440
		private bool m_UseRegistry;

		// Token: 0x040024E1 RID: 9441
		private int m_NetworkChangeStatus;

		// Token: 0x040024E2 RID: 9442
		private AutoWebProxyScriptEngine.AutoDetector m_AutoDetector;

		// Token: 0x040024E3 RID: 9443
		private SafeRegistryHandle hkcu;

		// Token: 0x040024E4 RID: 9444
		private WindowsIdentity m_Identity;

		// Token: 0x040024E5 RID: 9445
		private SafeRegistryHandle regKey;

		// Token: 0x040024E6 RID: 9446
		private SafeRegistryHandle regKeyLM;

		// Token: 0x040024E7 RID: 9447
		private SafeRegistryHandle regKeyPolicy;

		// Token: 0x040024E8 RID: 9448
		private AutoResetEvent registryChangeEvent;

		// Token: 0x040024E9 RID: 9449
		private AutoResetEvent registryChangeEventLM;

		// Token: 0x040024EA RID: 9450
		private AutoResetEvent registryChangeEventPolicy;

		// Token: 0x040024EB RID: 9451
		private bool registryChangeDeferred;

		// Token: 0x040024EC RID: 9452
		private bool registryChangeLMDeferred;

		// Token: 0x040024ED RID: 9453
		private bool registryChangePolicyDeferred;

		// Token: 0x040024EE RID: 9454
		private bool needRegistryUpdate;

		// Token: 0x040024EF RID: 9455
		private bool needConnectoidUpdate;

		// Token: 0x040024F0 RID: 9456
		private bool registrySuppress;

		// Token: 0x020004AE RID: 1198
		private static class SyncStatus
		{
			// Token: 0x040024F1 RID: 9457
			internal const int Unlocked = 0;

			// Token: 0x040024F2 RID: 9458
			internal const int Locking = 1;

			// Token: 0x040024F3 RID: 9459
			internal const int LockOwner = 2;

			// Token: 0x040024F4 RID: 9460
			internal const int AbortedLocked = 3;

			// Token: 0x040024F5 RID: 9461
			internal const int Aborted = 4;
		}

		// Token: 0x020004AF RID: 1199
		private class AutoDetector
		{
			// Token: 0x060024F8 RID: 9464 RVA: 0x00092D1C File Offset: 0x00091D1C
			private static void Initialize()
			{
				if (!AutoWebProxyScriptEngine.AutoDetector.s_Initialized)
				{
					lock (AutoWebProxyScriptEngine.AutoDetector.s_LockObject)
					{
						if (!AutoWebProxyScriptEngine.AutoDetector.s_Initialized)
						{
							AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector = new AutoWebProxyScriptEngine.AutoDetector(UnsafeNclNativeMethods.RasHelper.GetCurrentConnectoid(), 1);
							if (NetworkChange.CanListenForNetworkChanges)
							{
								AutoWebProxyScriptEngine.AutoDetector.s_AddressChange = new NetworkAddressChangePolled();
							}
							if (UnsafeNclNativeMethods.RasHelper.RasSupported)
							{
								AutoWebProxyScriptEngine.AutoDetector.s_RasHelper = new UnsafeNclNativeMethods.RasHelper();
							}
							AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion = 1;
							AutoWebProxyScriptEngine.AutoDetector.s_Initialized = true;
						}
					}
				}
			}

			// Token: 0x060024F9 RID: 9465 RVA: 0x00092DA4 File Offset: 0x00091DA4
			internal static bool CheckForNetworkChanges(ref int changeStatus)
			{
				AutoWebProxyScriptEngine.AutoDetector.Initialize();
				AutoWebProxyScriptEngine.AutoDetector.CheckForChanges();
				int num = changeStatus;
				changeStatus = AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion;
				return num != changeStatus;
			}

			// Token: 0x060024FA RID: 9466 RVA: 0x00092DD0 File Offset: 0x00091DD0
			private static void CheckForChanges()
			{
				bool flag = false;
				if (AutoWebProxyScriptEngine.AutoDetector.s_RasHelper != null && AutoWebProxyScriptEngine.AutoDetector.s_RasHelper.HasChanged)
				{
					AutoWebProxyScriptEngine.AutoDetector.s_RasHelper.Reset();
					flag = true;
				}
				if (AutoWebProxyScriptEngine.AutoDetector.s_AddressChange != null && AutoWebProxyScriptEngine.AutoDetector.s_AddressChange.CheckAndReset())
				{
					flag = true;
				}
				if (flag)
				{
					Interlocked.Increment(ref AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion);
					AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector = new AutoWebProxyScriptEngine.AutoDetector(UnsafeNclNativeMethods.RasHelper.GetCurrentConnectoid(), AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion);
				}
			}

			// Token: 0x170007A5 RID: 1957
			// (get) Token: 0x060024FB RID: 9467 RVA: 0x00092E37 File Offset: 0x00091E37
			internal static AutoWebProxyScriptEngine.AutoDetector CurrentAutoDetector
			{
				get
				{
					AutoWebProxyScriptEngine.AutoDetector.Initialize();
					return AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector;
				}
			}

			// Token: 0x060024FC RID: 9468 RVA: 0x00092E45 File Offset: 0x00091E45
			private AutoDetector(string connectoid, int currentVersion)
			{
				this.m_Connectoid = connectoid;
				this.m_CurrentVersion = currentVersion;
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x060024FD RID: 9469 RVA: 0x00092E5B File Offset: 0x00091E5B
			internal string Connectoid
			{
				get
				{
					return this.m_Connectoid;
				}
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x060024FE RID: 9470 RVA: 0x00092E63 File Offset: 0x00091E63
			internal int NetworkChangeStatus
			{
				get
				{
					return this.m_CurrentVersion;
				}
			}

			// Token: 0x040024F6 RID: 9462
			private static NetworkAddressChangePolled s_AddressChange;

			// Token: 0x040024F7 RID: 9463
			private static UnsafeNclNativeMethods.RasHelper s_RasHelper;

			// Token: 0x040024F8 RID: 9464
			private static int s_CurrentVersion;

			// Token: 0x040024F9 RID: 9465
			private static volatile AutoWebProxyScriptEngine.AutoDetector s_CurrentAutoDetector;

			// Token: 0x040024FA RID: 9466
			private static volatile bool s_Initialized;

			// Token: 0x040024FB RID: 9467
			private static object s_LockObject = new object();

			// Token: 0x040024FC RID: 9468
			private readonly string m_Connectoid;

			// Token: 0x040024FD RID: 9469
			private readonly int m_CurrentVersion;
		}
	}
}
