using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x020002F1 RID: 753
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_AssemblyName))]
	[ComVisible(true)]
	[Serializable]
	public sealed class AssemblyName : _AssemblyName, ICloneable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06001D25 RID: 7461 RVA: 0x00049EAE File Offset: 0x00048EAE
		public AssemblyName()
		{
			this._HashAlgorithm = AssemblyHashAlgorithm.None;
			this._VersionCompatibility = AssemblyVersionCompatibility.SameMachine;
			this._Flags = AssemblyNameFlags.None;
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x00049ECB File Offset: 0x00048ECB
		// (set) Token: 0x06001D27 RID: 7463 RVA: 0x00049ED3 File Offset: 0x00048ED3
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001D28 RID: 7464 RVA: 0x00049EDC File Offset: 0x00048EDC
		// (set) Token: 0x06001D29 RID: 7465 RVA: 0x00049EE4 File Offset: 0x00048EE4
		public Version Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x00049EED File Offset: 0x00048EED
		// (set) Token: 0x06001D2B RID: 7467 RVA: 0x00049EF5 File Offset: 0x00048EF5
		public CultureInfo CultureInfo
		{
			get
			{
				return this._CultureInfo;
			}
			set
			{
				this._CultureInfo = value;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x00049EFE File Offset: 0x00048EFE
		// (set) Token: 0x06001D2D RID: 7469 RVA: 0x00049F06 File Offset: 0x00048F06
		public string CodeBase
		{
			get
			{
				return this._CodeBase;
			}
			set
			{
				this._CodeBase = value;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x00049F0F File Offset: 0x00048F0F
		public string EscapedCodeBase
		{
			get
			{
				if (this._CodeBase == null)
				{
					return null;
				}
				return AssemblyName.EscapeCodeBase(this._CodeBase);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x00049F28 File Offset: 0x00048F28
		// (set) Token: 0x06001D30 RID: 7472 RVA: 0x00049F48 File Offset: 0x00048F48
		public ProcessorArchitecture ProcessorArchitecture
		{
			get
			{
				int num = (int)((this._Flags & (AssemblyNameFlags)112) >> 4);
				if (num > 4)
				{
					num = 0;
				}
				return (ProcessorArchitecture)num;
			}
			set
			{
				int num = (int)(value & (ProcessorArchitecture)7);
				if (num <= 4)
				{
					this._Flags = (AssemblyNameFlags)((long)this._Flags & (long)((ulong)-241));
					this._Flags |= (AssemblyNameFlags)(num << 4);
				}
			}
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x00049F84 File Offset: 0x00048F84
		public object Clone()
		{
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Init(this._Name, this._PublicKey, this._PublicKeyToken, this._Version, this._CultureInfo, this._HashAlgorithm, this._VersionCompatibility, this._CodeBase, this._Flags, this._StrongNameKeyPair);
			assemblyName._HashForControl = this._HashForControl;
			assemblyName._HashAlgorithmForControl = this._HashAlgorithmForControl;
			return assemblyName;
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x00049FF4 File Offset: 0x00048FF4
		public static AssemblyName GetAssemblyName(string assemblyFile)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			string fullPathInternal = Path.GetFullPathInternal(assemblyFile);
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fullPathInternal).Demand();
			return AssemblyName.nGetFileInformation(fullPathInternal);
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x0004A028 File Offset: 0x00049028
		internal void SetHashControl(byte[] hash, AssemblyHashAlgorithm hashAlgorithm)
		{
			this._HashForControl = hash;
			this._HashAlgorithmForControl = hashAlgorithm;
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x0004A038 File Offset: 0x00049038
		public byte[] GetPublicKey()
		{
			return this._PublicKey;
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x0004A040 File Offset: 0x00049040
		public void SetPublicKey(byte[] publicKey)
		{
			this._PublicKey = publicKey;
			if (publicKey == null)
			{
				this._Flags ^= AssemblyNameFlags.PublicKey;
				return;
			}
			this._Flags |= AssemblyNameFlags.PublicKey;
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x0004A069 File Offset: 0x00049069
		public byte[] GetPublicKeyToken()
		{
			if (this._PublicKeyToken == null)
			{
				this._PublicKeyToken = this.nGetPublicKeyToken();
			}
			return this._PublicKeyToken;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0004A085 File Offset: 0x00049085
		public void SetPublicKeyToken(byte[] publicKeyToken)
		{
			this._PublicKeyToken = publicKeyToken;
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x0004A08E File Offset: 0x0004908E
		// (set) Token: 0x06001D39 RID: 7481 RVA: 0x0004A09C File Offset: 0x0004909C
		public AssemblyNameFlags Flags
		{
			get
			{
				return this._Flags & (AssemblyNameFlags)(-241);
			}
			set
			{
				this._Flags &= (AssemblyNameFlags)240;
				this._Flags |= (value & (AssemblyNameFlags)(-241));
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0004A0C4 File Offset: 0x000490C4
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0004A0CC File Offset: 0x000490CC
		public AssemblyHashAlgorithm HashAlgorithm
		{
			get
			{
				return this._HashAlgorithm;
			}
			set
			{
				this._HashAlgorithm = value;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x0004A0D5 File Offset: 0x000490D5
		// (set) Token: 0x06001D3D RID: 7485 RVA: 0x0004A0DD File Offset: 0x000490DD
		public AssemblyVersionCompatibility VersionCompatibility
		{
			get
			{
				return this._VersionCompatibility;
			}
			set
			{
				this._VersionCompatibility = value;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0004A0E6 File Offset: 0x000490E6
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x0004A0EE File Offset: 0x000490EE
		public StrongNameKeyPair KeyPair
		{
			get
			{
				return this._StrongNameKeyPair;
			}
			set
			{
				this._StrongNameKeyPair = value;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x0004A0F7 File Offset: 0x000490F7
		public string FullName
		{
			get
			{
				return this.nToString();
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0004A100 File Offset: 0x00049100
		public override string ToString()
		{
			string fullName = this.FullName;
			if (fullName == null)
			{
				return base.ToString();
			}
			return fullName;
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x0004A120 File Offset: 0x00049120
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_Name", this._Name);
			info.AddValue("_PublicKey", this._PublicKey, typeof(byte[]));
			info.AddValue("_PublicKeyToken", this._PublicKeyToken, typeof(byte[]));
			info.AddValue("_CultureInfo", (this._CultureInfo == null) ? -1 : this._CultureInfo.LCID);
			info.AddValue("_CodeBase", this._CodeBase);
			info.AddValue("_Version", this._Version);
			info.AddValue("_HashAlgorithm", this._HashAlgorithm, typeof(AssemblyHashAlgorithm));
			info.AddValue("_HashAlgorithmForControl", this._HashAlgorithmForControl, typeof(AssemblyHashAlgorithm));
			info.AddValue("_StrongNameKeyPair", this._StrongNameKeyPair, typeof(StrongNameKeyPair));
			info.AddValue("_VersionCompatibility", this._VersionCompatibility, typeof(AssemblyVersionCompatibility));
			info.AddValue("_Flags", this._Flags, typeof(AssemblyNameFlags));
			info.AddValue("_HashForControl", this._HashForControl, typeof(byte[]));
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x0004A27C File Offset: 0x0004927C
		public void OnDeserialization(object sender)
		{
			if (this.m_siInfo == null)
			{
				return;
			}
			this._Name = this.m_siInfo.GetString("_Name");
			this._PublicKey = (byte[])this.m_siInfo.GetValue("_PublicKey", typeof(byte[]));
			this._PublicKeyToken = (byte[])this.m_siInfo.GetValue("_PublicKeyToken", typeof(byte[]));
			int @int = this.m_siInfo.GetInt32("_CultureInfo");
			if (@int != -1)
			{
				this._CultureInfo = new CultureInfo(@int);
			}
			this._CodeBase = this.m_siInfo.GetString("_CodeBase");
			this._Version = (Version)this.m_siInfo.GetValue("_Version", typeof(Version));
			this._HashAlgorithm = (AssemblyHashAlgorithm)this.m_siInfo.GetValue("_HashAlgorithm", typeof(AssemblyHashAlgorithm));
			this._StrongNameKeyPair = (StrongNameKeyPair)this.m_siInfo.GetValue("_StrongNameKeyPair", typeof(StrongNameKeyPair));
			this._VersionCompatibility = (AssemblyVersionCompatibility)this.m_siInfo.GetValue("_VersionCompatibility", typeof(AssemblyVersionCompatibility));
			this._Flags = (AssemblyNameFlags)this.m_siInfo.GetValue("_Flags", typeof(AssemblyNameFlags));
			try
			{
				this._HashAlgorithmForControl = (AssemblyHashAlgorithm)this.m_siInfo.GetValue("_HashAlgorithmForControl", typeof(AssemblyHashAlgorithm));
				this._HashForControl = (byte[])this.m_siInfo.GetValue("_HashForControl", typeof(byte[]));
			}
			catch (SerializationException)
			{
				this._HashAlgorithmForControl = AssemblyHashAlgorithm.None;
				this._HashForControl = null;
			}
			this.m_siInfo = null;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0004A458 File Offset: 0x00049458
		public AssemblyName(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyName.Length == 0 || assemblyName[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Format_StringZeroLength"));
			}
			this._Name = assemblyName;
			this.nInit();
		}

		// Token: 0x06001D45 RID: 7493
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool ReferenceMatchesDefinition(AssemblyName reference, AssemblyName definition);

		// Token: 0x06001D46 RID: 7494
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int nInit(out Assembly assembly, bool forIntrospection, bool raiseResolveEvent);

		// Token: 0x06001D47 RID: 7495 RVA: 0x0004A4A8 File Offset: 0x000494A8
		internal void nInit()
		{
			Assembly assembly = null;
			this.nInit(out assembly, false, false);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0004A4C2 File Offset: 0x000494C2
		internal AssemblyName(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0004A4D4 File Offset: 0x000494D4
		internal void Init(string name, byte[] publicKey, byte[] publicKeyToken, Version version, CultureInfo cultureInfo, AssemblyHashAlgorithm hashAlgorithm, AssemblyVersionCompatibility versionCompatibility, string codeBase, AssemblyNameFlags flags, StrongNameKeyPair keyPair)
		{
			this._Name = name;
			if (publicKey != null)
			{
				this._PublicKey = new byte[publicKey.Length];
				Array.Copy(publicKey, this._PublicKey, publicKey.Length);
			}
			if (publicKeyToken != null)
			{
				this._PublicKeyToken = new byte[publicKeyToken.Length];
				Array.Copy(publicKeyToken, this._PublicKeyToken, publicKeyToken.Length);
			}
			if (version != null)
			{
				this._Version = (Version)version.Clone();
			}
			this._CultureInfo = cultureInfo;
			this._HashAlgorithm = hashAlgorithm;
			this._VersionCompatibility = versionCompatibility;
			this._CodeBase = codeBase;
			this._Flags = flags;
			this._StrongNameKeyPair = keyPair;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0004A574 File Offset: 0x00049574
		void _AssemblyName.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0004A57B File Offset: 0x0004957B
		void _AssemblyName.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0004A582 File Offset: 0x00049582
		void _AssemblyName.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x0004A589 File Offset: 0x00049589
		void _AssemblyName.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001D4E RID: 7502
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AssemblyName nGetFileInformation(string s);

		// Token: 0x06001D4F RID: 7503
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string nToString();

		// Token: 0x06001D50 RID: 7504
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern byte[] nGetPublicKeyToken();

		// Token: 0x06001D51 RID: 7505
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string EscapeCodeBase(string codeBase);

		// Token: 0x04000ADC RID: 2780
		private string _Name;

		// Token: 0x04000ADD RID: 2781
		private byte[] _PublicKey;

		// Token: 0x04000ADE RID: 2782
		private byte[] _PublicKeyToken;

		// Token: 0x04000ADF RID: 2783
		private CultureInfo _CultureInfo;

		// Token: 0x04000AE0 RID: 2784
		private string _CodeBase;

		// Token: 0x04000AE1 RID: 2785
		private Version _Version;

		// Token: 0x04000AE2 RID: 2786
		private StrongNameKeyPair _StrongNameKeyPair;

		// Token: 0x04000AE3 RID: 2787
		private SerializationInfo m_siInfo;

		// Token: 0x04000AE4 RID: 2788
		private byte[] _HashForControl;

		// Token: 0x04000AE5 RID: 2789
		private AssemblyHashAlgorithm _HashAlgorithm;

		// Token: 0x04000AE6 RID: 2790
		private AssemblyHashAlgorithm _HashAlgorithmForControl;

		// Token: 0x04000AE7 RID: 2791
		private AssemblyVersionCompatibility _VersionCompatibility;

		// Token: 0x04000AE8 RID: 2792
		private AssemblyNameFlags _Flags;
	}
}
