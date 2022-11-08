using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004B3 RID: 1203
	internal class AutoWebProxyScriptWrapper
	{
		// Token: 0x06002533 RID: 9523 RVA: 0x000943F1 File Offset: 0x000933F1
		static AutoWebProxyScriptWrapper()
		{
			AppDomain.CurrentDomain.DomainUnload += AutoWebProxyScriptWrapper.OnDomainUnload;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x00094420 File Offset: 0x00093420
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation)]
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		internal AutoWebProxyScriptWrapper()
		{
			Exception ex = null;
			if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError == null && AutoWebProxyScriptWrapper.s_ProxyScriptHelperType == null)
			{
				lock (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLock)
				{
					if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError == null && AutoWebProxyScriptWrapper.s_ProxyScriptHelperType == null)
					{
						try
						{
							AutoWebProxyScriptWrapper.s_ProxyScriptHelperType = Type.GetType("System.Net.VsaWebProxyScript, Microsoft.JScript, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
						}
						catch (Exception ex2)
						{
							ex = ex2;
						}
						if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperType == null)
						{
							AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError = ((ex == null) ? new InternalException() : ex);
						}
					}
				}
			}
			if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError != null)
			{
				throw new TypeLoadException(SR.GetString("net_cannot_load_proxy_helper"), (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError is InternalException) ? null : AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError);
			}
			this.CreateAppDomain();
			ex = null;
			try
			{
				ObjectHandle objectHandle = Activator.CreateInstance(this.scriptDomain, AutoWebProxyScriptWrapper.s_ProxyScriptHelperType.Assembly.FullName, AutoWebProxyScriptWrapper.s_ProxyScriptHelperType.FullName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.CreateInstance, null, null, null, null, null);
				if (objectHandle != null)
				{
					this.site = (IWebProxyScript)objectHandle.Unwrap();
				}
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			if (this.site == null)
			{
				lock (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLock)
				{
					if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError == null)
					{
						AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError = ((ex == null) ? new InternalException() : ex);
					}
				}
				throw new TypeLoadException(SR.GetString("net_cannot_load_proxy_helper"), (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError is InternalException) ? null : AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError);
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000945A4 File Offset: 0x000935A4
		[SecurityPermission(SecurityAction.Assert, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlAppDomain))]
		private void CreateAppDomain()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				try
				{
				}
				finally
				{
					Monitor.Enter(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					flag = true;
				}
				if (AutoWebProxyScriptWrapper.s_CleanedUp)
				{
					throw new InvalidOperationException(SR.GetString("net_cant_perform_during_shutdown"));
				}
				if (AutoWebProxyScriptWrapper.s_AppDomainInfo == null)
				{
					AutoWebProxyScriptWrapper.s_AppDomainInfo = new AppDomainSetup();
					AutoWebProxyScriptWrapper.s_AppDomainInfo.DisallowBindingRedirects = true;
					AutoWebProxyScriptWrapper.s_AppDomainInfo.DisallowCodeDownload = true;
					AutoWebProxyScriptWrapper.s_AppDomainInfo.ApplicationBase = Environment.SystemDirectory;
				}
				AppDomain appDomain = AutoWebProxyScriptWrapper.s_ExcessAppDomain;
				if (appDomain != null)
				{
					TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), appDomain);
					throw new InvalidOperationException(SR.GetString("net_cant_create_environment"));
				}
				this.appDomainIndex = AutoWebProxyScriptWrapper.s_NextAppDomainIndex++;
				try
				{
				}
				finally
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
					PolicyLevel policyLevel = PolicyLevel.CreateAppDomainLevel();
					policyLevel.RootCodeGroup = new UnionCodeGroup(new AllMembershipCondition(), new PolicyStatement(permissionSet, PolicyStatementAttribute.Exclusive));
					Evidence evidence = new Evidence();
					evidence.AddHost(new Zone(SecurityZone.Internet));
					AutoWebProxyScriptWrapper.s_ExcessAppDomain = AppDomain.CreateDomain("WebProxyScript", evidence, AutoWebProxyScriptWrapper.s_AppDomainInfo);
					AutoWebProxyScriptWrapper.s_ExcessAppDomain.SetAppDomainPolicy(policyLevel);
					try
					{
						AutoWebProxyScriptWrapper.s_AppDomains.Add(this.appDomainIndex, AutoWebProxyScriptWrapper.s_ExcessAppDomain);
						this.scriptDomain = AutoWebProxyScriptWrapper.s_ExcessAppDomain;
					}
					finally
					{
						if (object.ReferenceEquals(this.scriptDomain, AutoWebProxyScriptWrapper.s_ExcessAppDomain))
						{
							AutoWebProxyScriptWrapper.s_ExcessAppDomain = null;
						}
						else
						{
							try
							{
								AutoWebProxyScriptWrapper.s_AppDomains.Remove(this.appDomainIndex);
							}
							finally
							{
								TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), AutoWebProxyScriptWrapper.s_ExcessAppDomain);
							}
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
				}
			}
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000947D4 File Offset: 0x000937D4
		internal void Close()
		{
			this.site.Close();
			TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), this.appDomainIndex);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x0009480C File Offset: 0x0009380C
		~AutoWebProxyScriptWrapper()
		{
			if (!NclUtilities.HasShutdownStarted && this.scriptDomain != null)
			{
				TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), this.appDomainIndex);
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00094864 File Offset: 0x00093864
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void CloseAppDomainCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			try
			{
				AppDomain appDomain = context as AppDomain;
				if (appDomain == null)
				{
					AutoWebProxyScriptWrapper.CloseAppDomain((int)context);
				}
				else if (object.ReferenceEquals(appDomain, AutoWebProxyScriptWrapper.s_ExcessAppDomain))
				{
					try
					{
						AppDomain.Unload(appDomain);
					}
					catch (AppDomainUnloadedException)
					{
					}
					AutoWebProxyScriptWrapper.s_ExcessAppDomain = null;
				}
			}
			catch (Exception exception)
			{
				if (NclUtilities.IsFatal(exception))
				{
					throw;
				}
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000948D4 File Offset: 0x000938D4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void CloseAppDomain(int index)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			AppDomain domain;
			try
			{
				try
				{
				}
				finally
				{
					Monitor.Enter(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					flag = true;
				}
				if (AutoWebProxyScriptWrapper.s_CleanedUp)
				{
					return;
				}
				domain = (AppDomain)AutoWebProxyScriptWrapper.s_AppDomains[index];
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					flag = false;
				}
			}
			try
			{
				AppDomain.Unload(domain);
			}
			catch (AppDomainUnloadedException)
			{
			}
			finally
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					try
					{
					}
					finally
					{
						Monitor.Enter(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
						flag = true;
					}
					AutoWebProxyScriptWrapper.s_AppDomains.Remove(index);
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					}
				}
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000949CC File Offset: 0x000939CC
		[ReliabilityContract(Consistency.MayCorruptProcess, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			lock (AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot)
			{
				if (!AutoWebProxyScriptWrapper.s_CleanedUp)
				{
					AutoWebProxyScriptWrapper.s_CleanedUp = true;
					foreach (object obj in AutoWebProxyScriptWrapper.s_AppDomains.Values)
					{
						AppDomain domain = (AppDomain)obj;
						try
						{
							AppDomain.Unload(domain);
						}
						catch
						{
						}
					}
					AutoWebProxyScriptWrapper.s_AppDomains.Clear();
					AppDomain appDomain = AutoWebProxyScriptWrapper.s_ExcessAppDomain;
					if (appDomain != null)
					{
						try
						{
							AppDomain.Unload(appDomain);
						}
						catch
						{
						}
						AutoWebProxyScriptWrapper.s_ExcessAppDomain = null;
					}
				}
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600253B RID: 9531 RVA: 0x00094AA0 File Offset: 0x00093AA0
		internal string ScriptBody
		{
			get
			{
				return this.scriptText;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x0600253C RID: 9532 RVA: 0x00094AA8 File Offset: 0x00093AA8
		// (set) Token: 0x0600253D RID: 9533 RVA: 0x00094AB0 File Offset: 0x00093AB0
		internal byte[] Buffer
		{
			get
			{
				return this.scriptBytes;
			}
			set
			{
				this.scriptBytes = value;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600253E RID: 9534 RVA: 0x00094AB9 File Offset: 0x00093AB9
		// (set) Token: 0x0600253F RID: 9535 RVA: 0x00094AC1 File Offset: 0x00093AC1
		internal DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
			set
			{
				this.lastModified = value;
			}
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00094ACA File Offset: 0x00093ACA
		internal string FindProxyForURL(string url, string host)
		{
			return this.site.Run(url, host);
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x00094AD9 File Offset: 0x00093AD9
		internal bool Compile(Uri engineScriptLocation, string scriptBody, byte[] buffer)
		{
			if (this.site.Load(engineScriptLocation, scriptBody, typeof(WebProxyScriptHelper)))
			{
				this.scriptText = scriptBody;
				this.scriptBytes = buffer;
				return true;
			}
			return false;
		}

		// Token: 0x04002500 RID: 9472
		private const string c_appDomainName = "WebProxyScript";

		// Token: 0x04002501 RID: 9473
		private int appDomainIndex;

		// Token: 0x04002502 RID: 9474
		private AppDomain scriptDomain;

		// Token: 0x04002503 RID: 9475
		private IWebProxyScript site;

		// Token: 0x04002504 RID: 9476
		private static AppDomain s_ExcessAppDomain;

		// Token: 0x04002505 RID: 9477
		private static Hashtable s_AppDomains = new Hashtable();

		// Token: 0x04002506 RID: 9478
		private static bool s_CleanedUp;

		// Token: 0x04002507 RID: 9479
		private static int s_NextAppDomainIndex;

		// Token: 0x04002508 RID: 9480
		private static AppDomainSetup s_AppDomainInfo;

		// Token: 0x04002509 RID: 9481
		private static Type s_ProxyScriptHelperType;

		// Token: 0x0400250A RID: 9482
		private static Exception s_ProxyScriptHelperLoadError;

		// Token: 0x0400250B RID: 9483
		private static object s_ProxyScriptHelperLock = new object();

		// Token: 0x0400250C RID: 9484
		private string scriptText;

		// Token: 0x0400250D RID: 9485
		private byte[] scriptBytes;

		// Token: 0x0400250E RID: 9486
		private DateTime lastModified;
	}
}
