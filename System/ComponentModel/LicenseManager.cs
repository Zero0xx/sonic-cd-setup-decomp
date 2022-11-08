using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000109 RID: 265
	[HostProtection(SecurityAction.LinkDemand, ExternalProcessMgmt = true)]
	public sealed class LicenseManager
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x0001C37A File Offset: 0x0001B37A
		private LicenseManager()
		{
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0001C384 File Offset: 0x0001B384
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0001C3D4 File Offset: 0x0001B3D4
		public static LicenseContext CurrentContext
		{
			get
			{
				if (LicenseManager.context == null)
				{
					lock (LicenseManager.internalSyncObject)
					{
						if (LicenseManager.context == null)
						{
							LicenseManager.context = new RuntimeLicenseContext();
						}
					}
				}
				return LicenseManager.context;
			}
			set
			{
				lock (LicenseManager.internalSyncObject)
				{
					if (LicenseManager.contextLockHolder != null)
					{
						throw new InvalidOperationException(SR.GetString("LicMgrContextCannotBeChanged"));
					}
					LicenseManager.context = value;
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001C424 File Offset: 0x0001B424
		public static LicenseUsageMode UsageMode
		{
			get
			{
				if (LicenseManager.context != null)
				{
					return LicenseManager.context.UsageMode;
				}
				return LicenseUsageMode.Runtime;
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001C43C File Offset: 0x0001B43C
		private static void CacheProvider(Type type, LicenseProvider provider)
		{
			if (LicenseManager.providers == null)
			{
				LicenseManager.providers = new Hashtable();
			}
			LicenseManager.providers[type] = provider;
			if (provider != null)
			{
				if (LicenseManager.providerInstances == null)
				{
					LicenseManager.providerInstances = new Hashtable();
				}
				LicenseManager.providerInstances[provider.GetType()] = provider;
			}
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001C48B File Offset: 0x0001B48B
		public static object CreateWithContext(Type type, LicenseContext creationContext)
		{
			return LicenseManager.CreateWithContext(type, creationContext, new object[0]);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001C49C File Offset: 0x0001B49C
		public static object CreateWithContext(Type type, LicenseContext creationContext, object[] args)
		{
			object result = null;
			lock (LicenseManager.internalSyncObject)
			{
				LicenseContext currentContext = LicenseManager.CurrentContext;
				try
				{
					LicenseManager.CurrentContext = creationContext;
					LicenseManager.LockContext(LicenseManager.selfLock);
					try
					{
						result = SecurityUtils.SecureCreateInstance(type, args);
					}
					catch (TargetInvocationException ex)
					{
						throw ex.InnerException;
					}
				}
				finally
				{
					LicenseManager.UnlockContext(LicenseManager.selfLock);
					LicenseManager.CurrentContext = currentContext;
				}
			}
			return result;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001C524 File Offset: 0x0001B524
		private static bool GetCachedNoLicenseProvider(Type type)
		{
			return LicenseManager.providers != null && LicenseManager.providers.ContainsKey(type);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001C53A File Offset: 0x0001B53A
		private static LicenseProvider GetCachedProvider(Type type)
		{
			if (LicenseManager.providers != null)
			{
				return (LicenseProvider)LicenseManager.providers[type];
			}
			return null;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001C555 File Offset: 0x0001B555
		private static LicenseProvider GetCachedProviderInstance(Type providerType)
		{
			if (LicenseManager.providerInstances != null)
			{
				return (LicenseProvider)LicenseManager.providerInstances[providerType];
			}
			return null;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001C570 File Offset: 0x0001B570
		private static RuntimeTypeHandle GetLicenseInteropHelperType()
		{
			return typeof(LicenseManager.LicenseInteropHelper).TypeHandle;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001C584 File Offset: 0x0001B584
		public static bool IsLicensed(Type type)
		{
			License license;
			bool result = LicenseManager.ValidateInternal(type, null, false, out license);
			if (license != null)
			{
				license.Dispose();
				license = null;
			}
			return result;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001C5A8 File Offset: 0x0001B5A8
		public static bool IsValid(Type type)
		{
			License license;
			bool result = LicenseManager.ValidateInternal(type, null, false, out license);
			if (license != null)
			{
				license.Dispose();
				license = null;
			}
			return result;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001C5CC File Offset: 0x0001B5CC
		public static bool IsValid(Type type, object instance, out License license)
		{
			return LicenseManager.ValidateInternal(type, instance, false, out license);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001C5D8 File Offset: 0x0001B5D8
		public static void LockContext(object contextUser)
		{
			lock (LicenseManager.internalSyncObject)
			{
				if (LicenseManager.contextLockHolder != null)
				{
					throw new InvalidOperationException(SR.GetString("LicMgrAlreadyLocked"));
				}
				LicenseManager.contextLockHolder = contextUser;
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001C628 File Offset: 0x0001B628
		public static void UnlockContext(object contextUser)
		{
			lock (LicenseManager.internalSyncObject)
			{
				if (LicenseManager.contextLockHolder != contextUser)
				{
					throw new ArgumentException(SR.GetString("LicMgrDifferentUser"));
				}
				LicenseManager.contextLockHolder = null;
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001C678 File Offset: 0x0001B678
		private static bool ValidateInternal(Type type, object instance, bool allowExceptions, out License license)
		{
			string text;
			return LicenseManager.ValidateInternalRecursive(LicenseManager.CurrentContext, type, instance, allowExceptions, out license, out text);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001C698 File Offset: 0x0001B698
		private static bool ValidateInternalRecursive(LicenseContext context, Type type, object instance, bool allowExceptions, out License license, out string licenseKey)
		{
			LicenseProvider licenseProvider = LicenseManager.GetCachedProvider(type);
			if (licenseProvider == null && !LicenseManager.GetCachedNoLicenseProvider(type))
			{
				LicenseProviderAttribute licenseProviderAttribute = (LicenseProviderAttribute)Attribute.GetCustomAttribute(type, typeof(LicenseProviderAttribute), false);
				if (licenseProviderAttribute != null)
				{
					Type licenseProvider2 = licenseProviderAttribute.LicenseProvider;
					licenseProvider = LicenseManager.GetCachedProviderInstance(licenseProvider2);
					if (licenseProvider == null)
					{
						licenseProvider = (LicenseProvider)SecurityUtils.SecureCreateInstance(licenseProvider2);
					}
				}
				LicenseManager.CacheProvider(type, licenseProvider);
			}
			license = null;
			bool flag = true;
			licenseKey = null;
			if (licenseProvider != null)
			{
				license = licenseProvider.GetLicense(context, type, instance, allowExceptions);
				if (license == null)
				{
					flag = false;
				}
				else
				{
					licenseKey = license.LicenseKey;
				}
			}
			if (flag && instance == null)
			{
				Type baseType = type.BaseType;
				if (baseType != typeof(object) && baseType != null)
				{
					if (license != null)
					{
						license.Dispose();
						license = null;
					}
					string text;
					flag = LicenseManager.ValidateInternalRecursive(context, baseType, null, allowExceptions, out license, out text);
					if (license != null)
					{
						license.Dispose();
						license = null;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001C778 File Offset: 0x0001B778
		public static void Validate(Type type)
		{
			License license;
			if (!LicenseManager.ValidateInternal(type, null, true, out license))
			{
				throw new LicenseException(type);
			}
			if (license != null)
			{
				license.Dispose();
				license = null;
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001C7A4 File Offset: 0x0001B7A4
		public static License Validate(Type type, object instance)
		{
			License result;
			if (!LicenseManager.ValidateInternal(type, instance, true, out result))
			{
				throw new LicenseException(type, instance);
			}
			return result;
		}

		// Token: 0x04000986 RID: 2438
		private static readonly object selfLock = new object();

		// Token: 0x04000987 RID: 2439
		private static LicenseContext context = null;

		// Token: 0x04000988 RID: 2440
		private static object contextLockHolder = null;

		// Token: 0x04000989 RID: 2441
		private static Hashtable providers;

		// Token: 0x0400098A RID: 2442
		private static Hashtable providerInstances;

		// Token: 0x0400098B RID: 2443
		private static object internalSyncObject = new object();

		// Token: 0x0200010A RID: 266
		private class LicenseInteropHelper
		{
			// Token: 0x0600085D RID: 2141 RVA: 0x0001C7E8 File Offset: 0x0001B7E8
			private static object AllocateAndValidateLicense(RuntimeTypeHandle rth, IntPtr bstrKey, int fDesignTime)
			{
				Type typeFromHandle = Type.GetTypeFromHandle(rth);
				LicenseManager.LicenseInteropHelper.CLRLicenseContext clrlicenseContext = new LicenseManager.LicenseInteropHelper.CLRLicenseContext((fDesignTime != 0) ? LicenseUsageMode.Designtime : LicenseUsageMode.Runtime, typeFromHandle);
				if (fDesignTime == 0 && bstrKey != (IntPtr)0)
				{
					clrlicenseContext.SetSavedLicenseKey(typeFromHandle, Marshal.PtrToStringBSTR(bstrKey));
				}
				object result;
				try
				{
					result = LicenseManager.CreateWithContext(typeFromHandle, clrlicenseContext);
				}
				catch (LicenseException ex)
				{
					throw new COMException(ex.Message, -2147221230);
				}
				return result;
			}

			// Token: 0x0600085E RID: 2142 RVA: 0x0001C858 File Offset: 0x0001B858
			private static int RequestLicKey(RuntimeTypeHandle rth, ref IntPtr pbstrKey)
			{
				Type typeFromHandle = Type.GetTypeFromHandle(rth);
				License license;
				string text;
				if (!LicenseManager.ValidateInternalRecursive(LicenseManager.CurrentContext, typeFromHandle, null, false, out license, out text))
				{
					return -2147483640;
				}
				if (text == null)
				{
					return -2147483640;
				}
				pbstrKey = Marshal.StringToBSTR(text);
				if (license != null)
				{
					license.Dispose();
					license = null;
				}
				return 0;
			}

			// Token: 0x0600085F RID: 2143 RVA: 0x0001C8A8 File Offset: 0x0001B8A8
			private void GetLicInfo(RuntimeTypeHandle rth, ref int pRuntimeKeyAvail, ref int pLicVerified)
			{
				pRuntimeKeyAvail = 0;
				pLicVerified = 0;
				Type typeFromHandle = Type.GetTypeFromHandle(rth);
				if (this.helperContext == null)
				{
					this.helperContext = new DesigntimeLicenseContext();
				}
				else
				{
					this.helperContext.savedLicenseKeys.Clear();
				}
				License license;
				string text;
				if (LicenseManager.ValidateInternalRecursive(this.helperContext, typeFromHandle, null, false, out license, out text))
				{
					if (this.helperContext.savedLicenseKeys.Contains(typeFromHandle.AssemblyQualifiedName))
					{
						pRuntimeKeyAvail = 1;
					}
					if (license != null)
					{
						license.Dispose();
						license = null;
						pLicVerified = 1;
					}
				}
			}

			// Token: 0x06000860 RID: 2144 RVA: 0x0001C924 File Offset: 0x0001B924
			private void GetCurrentContextInfo(ref int fDesignTime, ref IntPtr bstrKey, RuntimeTypeHandle rth)
			{
				this.savedLicenseContext = LicenseManager.CurrentContext;
				this.savedType = Type.GetTypeFromHandle(rth);
				if (this.savedLicenseContext.UsageMode == LicenseUsageMode.Designtime)
				{
					fDesignTime = 1;
					bstrKey = (IntPtr)0;
					return;
				}
				fDesignTime = 0;
				string savedLicenseKey = this.savedLicenseContext.GetSavedLicenseKey(this.savedType, null);
				bstrKey = Marshal.StringToBSTR(savedLicenseKey);
			}

			// Token: 0x06000861 RID: 2145 RVA: 0x0001C988 File Offset: 0x0001B988
			private void SaveKeyInCurrentContext(IntPtr bstrKey)
			{
				if (bstrKey != (IntPtr)0)
				{
					this.savedLicenseContext.SetSavedLicenseKey(this.savedType, Marshal.PtrToStringBSTR(bstrKey));
				}
			}

			// Token: 0x0400098C RID: 2444
			private const int S_OK = 0;

			// Token: 0x0400098D RID: 2445
			private const int E_NOTIMPL = -2147467263;

			// Token: 0x0400098E RID: 2446
			private const int CLASS_E_NOTLICENSED = -2147221230;

			// Token: 0x0400098F RID: 2447
			private const int E_FAIL = -2147483640;

			// Token: 0x04000990 RID: 2448
			private DesigntimeLicenseContext helperContext;

			// Token: 0x04000991 RID: 2449
			private LicenseContext savedLicenseContext;

			// Token: 0x04000992 RID: 2450
			private Type savedType;

			// Token: 0x0200010B RID: 267
			internal class CLRLicenseContext : LicenseContext
			{
				// Token: 0x06000863 RID: 2147 RVA: 0x0001C9B7 File Offset: 0x0001B9B7
				public CLRLicenseContext(LicenseUsageMode usageMode, Type type)
				{
					this.usageMode = usageMode;
					this.type = type;
				}

				// Token: 0x170001B5 RID: 437
				// (get) Token: 0x06000864 RID: 2148 RVA: 0x0001C9CD File Offset: 0x0001B9CD
				public override LicenseUsageMode UsageMode
				{
					get
					{
						return this.usageMode;
					}
				}

				// Token: 0x06000865 RID: 2149 RVA: 0x0001C9D5 File Offset: 0x0001B9D5
				public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
				{
					if (type != this.type)
					{
						return null;
					}
					return this.key;
				}

				// Token: 0x06000866 RID: 2150 RVA: 0x0001C9E8 File Offset: 0x0001B9E8
				public override void SetSavedLicenseKey(Type type, string key)
				{
					if (type == this.type)
					{
						this.key = key;
					}
				}

				// Token: 0x04000993 RID: 2451
				private LicenseUsageMode usageMode;

				// Token: 0x04000994 RID: 2452
				private Type type;

				// Token: 0x04000995 RID: 2453
				private string key;
			}
		}
	}
}
