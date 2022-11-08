using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace System.IO.IsolatedStorage
{
	// Token: 0x020007AB RID: 1963
	[ComVisible(true)]
	public abstract class IsolatedStorage : MarshalByRefObject
	{
		// Token: 0x060045BF RID: 17855 RVA: 0x000ED329 File Offset: 0x000EC329
		internal static bool IsRoaming(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Roaming) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x000ED334 File Offset: 0x000EC334
		internal bool IsRoaming()
		{
			return (this.m_Scope & IsolatedStorageScope.Roaming) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x000ED344 File Offset: 0x000EC344
		internal static bool IsDomain(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x000ED34F File Offset: 0x000EC34F
		internal bool IsDomain()
		{
			return (this.m_Scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x000ED35F File Offset: 0x000EC35F
		internal static bool IsMachine(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Machine) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x000ED36B File Offset: 0x000EC36B
		internal bool IsAssembly()
		{
			return (this.m_Scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x000ED37B File Offset: 0x000EC37B
		internal static bool IsApp(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Application) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x000ED387 File Offset: 0x000EC387
		internal bool IsApp()
		{
			return (this.m_Scope & IsolatedStorageScope.Application) != IsolatedStorageScope.None;
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x000ED398 File Offset: 0x000EC398
		private string GetNameFromID(string typeID, string instanceID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(typeID);
			stringBuilder.Append(this.SeparatorInternal);
			stringBuilder.Append(instanceID);
			return stringBuilder.ToString();
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x000ED3D0 File Offset: 0x000EC3D0
		private static string GetPredefinedTypeName(object o)
		{
			if (o is Publisher)
			{
				return "Publisher";
			}
			if (o is StrongName)
			{
				return "StrongName";
			}
			if (o is Url)
			{
				return "Url";
			}
			if (o is Site)
			{
				return "Site";
			}
			if (o is Zone)
			{
				return "Zone";
			}
			return null;
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x000ED424 File Offset: 0x000EC424
		internal static string GetHash(Stream s)
		{
			string result;
			using (SHA1 sha = new SHA1CryptoServiceProvider())
			{
				byte[] buff = sha.ComputeHash(s);
				result = IsolatedStorage.ToBase32StringSuitableForDirName(buff);
			}
			return result;
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x000ED464 File Offset: 0x000EC464
		internal static string ToBase32StringSuitableForDirName(byte[] buff)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = buff.Length;
			int num2 = 0;
			do
			{
				byte b = (num2 < num) ? buff[num2++] : 0;
				byte b2 = (num2 < num) ? buff[num2++] : 0;
				byte b3 = (num2 < num) ? buff[num2++] : 0;
				byte b4 = (num2 < num) ? buff[num2++] : 0;
				byte b5 = (num2 < num) ? buff[num2++] : 0;
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b2 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b3 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b4 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b5 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(b & 224) >> 5 | (b4 & 96) >> 2]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(b2 & 224) >> 5 | (b5 & 96) >> 2]);
				b3 = (byte)(b3 >> 5);
				if ((b4 & 128) != 0)
				{
					b3 |= 8;
				}
				if ((b5 & 128) != 0)
				{
					b3 |= 16;
				}
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)b3]);
			}
			while (num2 < num);
			return stringBuilder.ToString();
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x000ED5B4 File Offset: 0x000EC5B4
		private static bool IsValidName(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsLetter(s[i]) && !char.IsDigit(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x000ED5F1 File Offset: 0x000EC5F1
		private static PermissionSet GetReflectionPermission()
		{
			if (IsolatedStorage.s_PermReflection == null)
			{
				IsolatedStorage.s_PermReflection = new PermissionSet(PermissionState.Unrestricted);
			}
			return IsolatedStorage.s_PermReflection;
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x000ED60A File Offset: 0x000EC60A
		private static SecurityPermission GetControlEvidencePermission()
		{
			if (IsolatedStorage.s_PermControlEvidence == null)
			{
				IsolatedStorage.s_PermControlEvidence = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
			}
			return IsolatedStorage.s_PermControlEvidence;
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x000ED624 File Offset: 0x000EC624
		private static PermissionSet GetExecutionPermission()
		{
			if (IsolatedStorage.s_PermExecution == null)
			{
				IsolatedStorage.s_PermExecution = new PermissionSet(PermissionState.None);
				IsolatedStorage.s_PermExecution.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			}
			return IsolatedStorage.s_PermExecution;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x000ED64E File Offset: 0x000EC64E
		private static PermissionSet GetUnrestricted()
		{
			if (IsolatedStorage.s_PermUnrestricted == null)
			{
				IsolatedStorage.s_PermUnrestricted = new PermissionSet(PermissionState.Unrestricted);
			}
			return IsolatedStorage.s_PermUnrestricted;
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x000ED667 File Offset: 0x000EC667
		protected virtual char SeparatorExternal
		{
			get
			{
				return '\\';
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x060045D1 RID: 17873 RVA: 0x000ED66B File Offset: 0x000EC66B
		protected virtual char SeparatorInternal
		{
			get
			{
				return '.';
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x060045D2 RID: 17874 RVA: 0x000ED66F File Offset: 0x000EC66F
		[CLSCompliant(false)]
		public virtual ulong MaximumSize
		{
			get
			{
				if (this.m_ValidQuota)
				{
					return this.m_Quota;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_QuotaIsUndefined"));
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x060045D3 RID: 17875 RVA: 0x000ED68F File Offset: 0x000EC68F
		[CLSCompliant(false)]
		public virtual ulong CurrentSize
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_CurrentSizeUndefined"));
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x060045D4 RID: 17876 RVA: 0x000ED6A0 File Offset: 0x000EC6A0
		public object DomainIdentity
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (this.IsDomain())
				{
					return this.m_DomainIdentity;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_DomainUndefined"));
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x060045D5 RID: 17877 RVA: 0x000ED6C0 File Offset: 0x000EC6C0
		[ComVisible(false)]
		public object ApplicationIdentity
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (this.IsApp())
				{
					return this.m_AppIdentity;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_ApplicationUndefined"));
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x060045D6 RID: 17878 RVA: 0x000ED6E0 File Offset: 0x000EC6E0
		public object AssemblyIdentity
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (this.IsAssembly())
				{
					return this.m_AssemIdentity;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_AssemblyUndefined"));
			}
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x000ED700 File Offset: 0x000EC700
		internal MemoryStream GetIdentityStream(IsolatedStorageScope scope)
		{
			IsolatedStorage.GetReflectionPermission().Assert();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			object obj;
			if (IsolatedStorage.IsApp(scope))
			{
				obj = this.m_AppIdentity;
			}
			else if (IsolatedStorage.IsDomain(scope))
			{
				obj = this.m_DomainIdentity;
			}
			else
			{
				obj = this.m_AssemIdentity;
			}
			if (obj != null)
			{
				binaryFormatter.Serialize(memoryStream, obj);
			}
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x060045D8 RID: 17880 RVA: 0x000ED760 File Offset: 0x000EC760
		public IsolatedStorageScope Scope
		{
			get
			{
				return this.m_Scope;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x060045D9 RID: 17881 RVA: 0x000ED768 File Offset: 0x000EC768
		internal string DomainName
		{
			get
			{
				if (this.IsDomain())
				{
					return this.m_DomainName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_DomainUndefined"));
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x060045DA RID: 17882 RVA: 0x000ED788 File Offset: 0x000EC788
		internal string AssemName
		{
			get
			{
				if (this.IsAssembly())
				{
					return this.m_AssemName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_AssemblyUndefined"));
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x060045DB RID: 17883 RVA: 0x000ED7A8 File Offset: 0x000EC7A8
		internal string AppName
		{
			get
			{
				if (this.IsApp())
				{
					return this.m_AppName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_ApplicationUndefined"));
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x000ED7C8 File Offset: 0x000EC7C8
		protected void InitStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			PermissionSet permissionSet = null;
			PermissionSet psDenied = null;
			Assembly assembly = IsolatedStorage.nGetCaller();
			IsolatedStorage.GetControlEvidencePermission().Assert();
			if (IsolatedStorage.IsDomain(scope))
			{
				AppDomain domain = Thread.GetDomain();
				if (!IsolatedStorage.IsRoaming(scope))
				{
					domain.nGetGrantSet(out permissionSet, out psDenied);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainGrantSet"));
					}
				}
				this._InitStore(scope, domain.Evidence, domainEvidenceType, assembly.Evidence, assemblyEvidenceType, null, null);
			}
			else
			{
				if (!IsolatedStorage.IsRoaming(scope))
				{
					assembly.nGetGrantSet(out permissionSet, out psDenied);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyGrantSet"));
					}
				}
				this._InitStore(scope, null, null, assembly.Evidence, assemblyEvidenceType, null, null);
			}
			this.SetQuota(permissionSet, psDenied);
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x000ED874 File Offset: 0x000EC874
		protected void InitStore(IsolatedStorageScope scope, Type appEvidenceType)
		{
			PermissionSet permissionSet = null;
			PermissionSet psDenied = null;
			IsolatedStorage.nGetCaller();
			IsolatedStorage.GetControlEvidencePermission().Assert();
			if (IsolatedStorage.IsApp(scope))
			{
				AppDomain domain = Thread.GetDomain();
				if (!IsolatedStorage.IsRoaming(scope))
				{
					domain.nGetGrantSet(out permissionSet, out psDenied);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainGrantSet"));
					}
				}
				ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
				if (activationContext == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationMissingIdentity"));
				}
				ApplicationSecurityInfo applicationSecurityInfo = new ApplicationSecurityInfo(activationContext);
				this._InitStore(scope, null, null, null, null, applicationSecurityInfo.ApplicationEvidence, appEvidenceType);
			}
			this.SetQuota(permissionSet, psDenied);
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x000ED90C File Offset: 0x000EC90C
		internal void InitStore(IsolatedStorageScope scope, object domain, object assem, object app)
		{
			PermissionSet permissionSet = null;
			PermissionSet psDenied = null;
			Evidence evidence = null;
			Evidence evidence2 = null;
			Evidence evidence3 = null;
			if (IsolatedStorage.IsApp(scope))
			{
				evidence3 = new Evidence();
				evidence3.AddHost(app);
			}
			else
			{
				evidence2 = new Evidence();
				evidence2.AddHost(assem);
				if (IsolatedStorage.IsDomain(scope))
				{
					evidence = new Evidence();
					evidence.AddHost(domain);
				}
			}
			this._InitStore(scope, evidence, null, evidence2, null, evidence3, null);
			if (!IsolatedStorage.IsRoaming(scope))
			{
				Assembly assembly = IsolatedStorage.nGetCaller();
				IsolatedStorage.GetControlEvidencePermission().Assert();
				assembly.nGetGrantSet(out permissionSet, out psDenied);
				if (permissionSet == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyGrantSet"));
				}
			}
			this.SetQuota(permissionSet, psDenied);
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x000ED9B0 File Offset: 0x000EC9B0
		internal void InitStore(IsolatedStorageScope scope, Evidence domainEv, Type domainEvidenceType, Evidence assemEv, Type assemEvidenceType, Evidence appEv, Type appEvidenceType)
		{
			PermissionSet permissionSet = null;
			PermissionSet psDenied = null;
			if (!IsolatedStorage.IsRoaming(scope))
			{
				if (IsolatedStorage.IsApp(scope))
				{
					permissionSet = SecurityManager.ResolvePolicy(appEv, IsolatedStorage.GetExecutionPermission(), IsolatedStorage.GetUnrestricted(), null, out psDenied);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationGrantSet"));
					}
				}
				else if (IsolatedStorage.IsDomain(scope))
				{
					permissionSet = SecurityManager.ResolvePolicy(domainEv, IsolatedStorage.GetExecutionPermission(), IsolatedStorage.GetUnrestricted(), null, out psDenied);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainGrantSet"));
					}
				}
				else
				{
					permissionSet = SecurityManager.ResolvePolicy(assemEv, IsolatedStorage.GetExecutionPermission(), IsolatedStorage.GetUnrestricted(), null, out psDenied);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyGrantSet"));
					}
				}
			}
			this._InitStore(scope, domainEv, domainEvidenceType, assemEv, assemEvidenceType, appEv, appEvidenceType);
			this.SetQuota(permissionSet, psDenied);
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x000EDA6C File Offset: 0x000ECA6C
		internal bool InitStore(IsolatedStorageScope scope, Stream domain, Stream assem, Stream app, string domainName, string assemName, string appName)
		{
			try
			{
				IsolatedStorage.GetReflectionPermission().Assert();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				if (IsolatedStorage.IsApp(scope))
				{
					this.m_AppIdentity = binaryFormatter.Deserialize(app);
					this.m_AppName = appName;
				}
				else
				{
					this.m_AssemIdentity = binaryFormatter.Deserialize(assem);
					this.m_AssemName = assemName;
					if (IsolatedStorage.IsDomain(scope))
					{
						this.m_DomainIdentity = binaryFormatter.Deserialize(domain);
						this.m_DomainName = domainName;
					}
				}
			}
			catch
			{
				return false;
			}
			this.m_Scope = scope;
			return true;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x000EDAFC File Offset: 0x000ECAFC
		private void _InitStore(IsolatedStorageScope scope, Evidence domainEv, Type domainEvidenceType, Evidence assemEv, Type assemblyEvidenceType, Evidence appEv, Type appEvidenceType)
		{
			IsolatedStorage.VerifyScope(scope);
			if (IsolatedStorage.IsApp(scope))
			{
				if (appEv == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationMissingIdentity"));
				}
			}
			else
			{
				if (assemEv == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyMissingIdentity"));
				}
				if (IsolatedStorage.IsDomain(scope) && domainEv == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainMissingIdentity"));
				}
			}
			IsolatedStorage.DemandPermission(scope);
			string typeID = null;
			string instanceID = null;
			if (IsolatedStorage.IsApp(scope))
			{
				this.m_AppIdentity = IsolatedStorage.GetAccountingInfo(appEv, appEvidenceType, IsolatedStorageScope.Application, out typeID, out instanceID);
				this.m_AppName = this.GetNameFromID(typeID, instanceID);
			}
			else
			{
				this.m_AssemIdentity = IsolatedStorage.GetAccountingInfo(assemEv, assemblyEvidenceType, IsolatedStorageScope.Assembly, out typeID, out instanceID);
				this.m_AssemName = this.GetNameFromID(typeID, instanceID);
				if (IsolatedStorage.IsDomain(scope))
				{
					this.m_DomainIdentity = IsolatedStorage.GetAccountingInfo(domainEv, domainEvidenceType, IsolatedStorageScope.Domain, out typeID, out instanceID);
					this.m_DomainName = this.GetNameFromID(typeID, instanceID);
				}
			}
			this.m_Scope = scope;
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x000EDBE4 File Offset: 0x000ECBE4
		private static object GetAccountingInfo(Evidence evidence, Type evidenceType, IsolatedStorageScope fAssmDomApp, out string typeName, out string instanceName)
		{
			object obj = null;
			object obj2 = IsolatedStorage._GetAccountingInfo(evidence, evidenceType, fAssmDomApp, out obj);
			typeName = IsolatedStorage.GetPredefinedTypeName(obj2);
			if (typeName == null)
			{
				IsolatedStorage.GetReflectionPermission().Assert();
				MemoryStream memoryStream = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj2.GetType());
				memoryStream.Position = 0L;
				typeName = IsolatedStorage.GetHash(memoryStream);
			}
			instanceName = null;
			if (obj != null)
			{
				if (obj is Stream)
				{
					instanceName = IsolatedStorage.GetHash((Stream)obj);
				}
				else if (obj is string)
				{
					if (IsolatedStorage.IsValidName((string)obj))
					{
						instanceName = (string)obj;
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream();
						BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
						binaryWriter.Write((string)obj);
						memoryStream.Position = 0L;
						instanceName = IsolatedStorage.GetHash(memoryStream);
					}
				}
			}
			else
			{
				obj = obj2;
			}
			if (instanceName == null)
			{
				IsolatedStorage.GetReflectionPermission().Assert();
				MemoryStream memoryStream = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Position = 0L;
				instanceName = IsolatedStorage.GetHash(memoryStream);
			}
			return obj2;
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x000EDCE4 File Offset: 0x000ECCE4
		private static object _GetAccountingInfo(Evidence evidence, Type evidenceType, IsolatedStorageScope fAssmDomApp, out object oNormalized)
		{
			object obj = null;
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			if (evidenceType == null)
			{
				Publisher publisher = null;
				StrongName strongName = null;
				Url url = null;
				Site site = null;
				Zone zone = null;
				while (hostEnumerator.MoveNext())
				{
					obj = hostEnumerator.Current;
					if (obj is Publisher)
					{
						publisher = (Publisher)obj;
						break;
					}
					if (obj is StrongName)
					{
						strongName = (StrongName)obj;
					}
					else if (obj is Url)
					{
						url = (Url)obj;
					}
					else if (obj is Site)
					{
						site = (Site)obj;
					}
					else if (obj is Zone)
					{
						zone = (Zone)obj;
					}
				}
				if (publisher != null)
				{
					obj = publisher;
				}
				else if (strongName != null)
				{
					obj = strongName;
				}
				else if (url != null)
				{
					obj = url;
				}
				else if (site != null)
				{
					obj = site;
				}
				else if (zone != null)
				{
					obj = zone;
				}
				else
				{
					if (fAssmDomApp == IsolatedStorageScope.Domain)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainNoEvidence"));
					}
					if (fAssmDomApp == IsolatedStorageScope.Application)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationNoEvidence"));
					}
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyNoEvidence"));
				}
			}
			else
			{
				while (hostEnumerator.MoveNext())
				{
					object obj2 = hostEnumerator.Current;
					if (obj2.GetType().Equals(evidenceType))
					{
						obj = obj2;
						break;
					}
				}
				if (obj == null)
				{
					if (fAssmDomApp == IsolatedStorageScope.Domain)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainNoEvidence"));
					}
					if (fAssmDomApp == IsolatedStorageScope.Application)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationNoEvidence"));
					}
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyNoEvidence"));
				}
			}
			if (obj is INormalizeForIsolatedStorage)
			{
				oNormalized = ((INormalizeForIsolatedStorage)obj).Normalize();
			}
			else if (obj is Publisher)
			{
				oNormalized = ((Publisher)obj).Normalize();
			}
			else if (obj is StrongName)
			{
				oNormalized = ((StrongName)obj).Normalize();
			}
			else if (obj is Url)
			{
				oNormalized = ((Url)obj).Normalize();
			}
			else if (obj is Site)
			{
				oNormalized = ((Site)obj).Normalize();
			}
			else if (obj is Zone)
			{
				oNormalized = ((Zone)obj).Normalize();
			}
			else
			{
				oNormalized = null;
			}
			return obj;
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x000EDED4 File Offset: 0x000ECED4
		private static void DemandPermission(IsolatedStorageScope scope)
		{
			IsolatedStorageFilePermission isolatedStorageFilePermission = null;
			if (scope <= (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
			{
				switch (scope)
				{
				case IsolatedStorageScope.User | IsolatedStorageScope.Assembly:
					if (IsolatedStorage.s_PermAssem == null)
					{
						IsolatedStorage.s_PermAssem = new IsolatedStorageFilePermission(IsolatedStorageContainment.AssemblyIsolationByUser, 0L, false);
					}
					isolatedStorageFilePermission = IsolatedStorage.s_PermAssem;
					break;
				case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly:
					break;
				case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly:
					if (IsolatedStorage.s_PermDomain == null)
					{
						IsolatedStorage.s_PermDomain = new IsolatedStorageFilePermission(IsolatedStorageContainment.DomainIsolationByUser, 0L, false);
					}
					isolatedStorageFilePermission = IsolatedStorage.s_PermDomain;
					break;
				default:
					switch (scope)
					{
					case IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
						if (IsolatedStorage.s_PermAssemRoaming == null)
						{
							IsolatedStorage.s_PermAssemRoaming = new IsolatedStorageFilePermission(IsolatedStorageContainment.AssemblyIsolationByRoamingUser, 0L, false);
						}
						isolatedStorageFilePermission = IsolatedStorage.s_PermAssemRoaming;
						break;
					case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
						break;
					case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
						if (IsolatedStorage.s_PermDomainRoaming == null)
						{
							IsolatedStorage.s_PermDomainRoaming = new IsolatedStorageFilePermission(IsolatedStorageContainment.DomainIsolationByRoamingUser, 0L, false);
						}
						isolatedStorageFilePermission = IsolatedStorage.s_PermDomainRoaming;
						break;
					default:
						switch (scope)
						{
						case IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine:
							if (IsolatedStorage.s_PermMachineAssem == null)
							{
								IsolatedStorage.s_PermMachineAssem = new IsolatedStorageFilePermission(IsolatedStorageContainment.AssemblyIsolationByMachine, 0L, false);
							}
							isolatedStorageFilePermission = IsolatedStorage.s_PermMachineAssem;
							break;
						case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine:
							if (IsolatedStorage.s_PermMachineDomain == null)
							{
								IsolatedStorage.s_PermMachineDomain = new IsolatedStorageFilePermission(IsolatedStorageContainment.DomainIsolationByMachine, 0L, false);
							}
							isolatedStorageFilePermission = IsolatedStorage.s_PermMachineDomain;
							break;
						}
						break;
					}
					break;
				}
			}
			else if (scope != (IsolatedStorageScope.User | IsolatedStorageScope.Application))
			{
				if (scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application))
				{
					if (scope == (IsolatedStorageScope.Machine | IsolatedStorageScope.Application))
					{
						if (IsolatedStorage.s_PermAppMachine == null)
						{
							IsolatedStorage.s_PermAppMachine = new IsolatedStorageFilePermission(IsolatedStorageContainment.ApplicationIsolationByMachine, 0L, false);
						}
						isolatedStorageFilePermission = IsolatedStorage.s_PermAppMachine;
					}
				}
				else
				{
					if (IsolatedStorage.s_PermAppUserRoaming == null)
					{
						IsolatedStorage.s_PermAppUserRoaming = new IsolatedStorageFilePermission(IsolatedStorageContainment.ApplicationIsolationByRoamingUser, 0L, false);
					}
					isolatedStorageFilePermission = IsolatedStorage.s_PermAppUserRoaming;
				}
			}
			else
			{
				if (IsolatedStorage.s_PermAppUser == null)
				{
					IsolatedStorage.s_PermAppUser = new IsolatedStorageFilePermission(IsolatedStorageContainment.ApplicationIsolationByUser, 0L, false);
				}
				isolatedStorageFilePermission = IsolatedStorage.s_PermAppUser;
			}
			isolatedStorageFilePermission.Demand();
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x000EE068 File Offset: 0x000ED068
		internal static void VerifyScope(IsolatedStorageScope scope)
		{
			if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming) || scope == (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine) || scope == (IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application) || scope == (IsolatedStorageScope.Machine | IsolatedStorageScope.Application) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application))
			{
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_Scope_Invalid"));
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x000EE0A8 File Offset: 0x000ED0A8
		internal void SetQuota(PermissionSet psAllowed, PermissionSet psDenied)
		{
			IsolatedStoragePermission permission = this.GetPermission(psAllowed);
			this.m_Quota = 0UL;
			if (permission != null)
			{
				if (permission.IsUnrestricted())
				{
					this.m_Quota = 9223372036854775807UL;
				}
				else
				{
					this.m_Quota = (ulong)permission.UserQuota;
				}
			}
			if (psDenied != null)
			{
				IsolatedStoragePermission permission2 = this.GetPermission(psDenied);
				if (permission2 != null)
				{
					if (permission2.IsUnrestricted())
					{
						this.m_Quota = 0UL;
					}
					else
					{
						ulong userQuota = (ulong)permission2.UserQuota;
						if (userQuota > this.m_Quota)
						{
							this.m_Quota = 0UL;
						}
						else
						{
							this.m_Quota -= userQuota;
						}
					}
				}
			}
			this.m_ValidQuota = true;
		}

		// Token: 0x060045E7 RID: 17895
		public abstract void Remove();

		// Token: 0x060045E8 RID: 17896
		protected abstract IsolatedStoragePermission GetPermission(PermissionSet ps);

		// Token: 0x060045E9 RID: 17897
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Assembly nGetCaller();

		// Token: 0x040022AE RID: 8878
		internal const IsolatedStorageScope c_Assembly = IsolatedStorageScope.User | IsolatedStorageScope.Assembly;

		// Token: 0x040022AF RID: 8879
		internal const IsolatedStorageScope c_Domain = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly;

		// Token: 0x040022B0 RID: 8880
		internal const IsolatedStorageScope c_AssemblyRoaming = IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming;

		// Token: 0x040022B1 RID: 8881
		internal const IsolatedStorageScope c_DomainRoaming = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming;

		// Token: 0x040022B2 RID: 8882
		internal const IsolatedStorageScope c_MachineAssembly = IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;

		// Token: 0x040022B3 RID: 8883
		internal const IsolatedStorageScope c_MachineDomain = IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;

		// Token: 0x040022B4 RID: 8884
		internal const IsolatedStorageScope c_AppUser = IsolatedStorageScope.User | IsolatedStorageScope.Application;

		// Token: 0x040022B5 RID: 8885
		internal const IsolatedStorageScope c_AppMachine = IsolatedStorageScope.Machine | IsolatedStorageScope.Application;

		// Token: 0x040022B6 RID: 8886
		internal const IsolatedStorageScope c_AppUserRoaming = IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application;

		// Token: 0x040022B7 RID: 8887
		private const string s_Publisher = "Publisher";

		// Token: 0x040022B8 RID: 8888
		private const string s_StrongName = "StrongName";

		// Token: 0x040022B9 RID: 8889
		private const string s_Site = "Site";

		// Token: 0x040022BA RID: 8890
		private const string s_Url = "Url";

		// Token: 0x040022BB RID: 8891
		private const string s_Zone = "Zone";

		// Token: 0x040022BC RID: 8892
		private static char[] s_Base32Char = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5'
		};

		// Token: 0x040022BD RID: 8893
		private ulong m_Quota;

		// Token: 0x040022BE RID: 8894
		private bool m_ValidQuota;

		// Token: 0x040022BF RID: 8895
		private object m_DomainIdentity;

		// Token: 0x040022C0 RID: 8896
		private object m_AssemIdentity;

		// Token: 0x040022C1 RID: 8897
		private object m_AppIdentity;

		// Token: 0x040022C2 RID: 8898
		private string m_DomainName;

		// Token: 0x040022C3 RID: 8899
		private string m_AssemName;

		// Token: 0x040022C4 RID: 8900
		private string m_AppName;

		// Token: 0x040022C5 RID: 8901
		private IsolatedStorageScope m_Scope;

		// Token: 0x040022C6 RID: 8902
		private static IsolatedStorageFilePermission s_PermDomain;

		// Token: 0x040022C7 RID: 8903
		private static IsolatedStorageFilePermission s_PermMachineDomain;

		// Token: 0x040022C8 RID: 8904
		private static IsolatedStorageFilePermission s_PermDomainRoaming;

		// Token: 0x040022C9 RID: 8905
		private static IsolatedStorageFilePermission s_PermAssem;

		// Token: 0x040022CA RID: 8906
		private static IsolatedStorageFilePermission s_PermMachineAssem;

		// Token: 0x040022CB RID: 8907
		private static IsolatedStorageFilePermission s_PermAssemRoaming;

		// Token: 0x040022CC RID: 8908
		private static IsolatedStorageFilePermission s_PermAppUser;

		// Token: 0x040022CD RID: 8909
		private static IsolatedStorageFilePermission s_PermAppMachine;

		// Token: 0x040022CE RID: 8910
		private static IsolatedStorageFilePermission s_PermAppUserRoaming;

		// Token: 0x040022CF RID: 8911
		private static SecurityPermission s_PermControlEvidence;

		// Token: 0x040022D0 RID: 8912
		private static PermissionSet s_PermReflection;

		// Token: 0x040022D1 RID: 8913
		private static PermissionSet s_PermUnrestricted;

		// Token: 0x040022D2 RID: 8914
		private static PermissionSet s_PermExecution;
	}
}
