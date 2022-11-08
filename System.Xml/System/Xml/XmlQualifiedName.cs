using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Xml
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public class XmlQualifiedName
	{
		// Token: 0x060001EA RID: 490 RVA: 0x00008D78 File Offset: 0x00007D78
		public XmlQualifiedName() : this(string.Empty, string.Empty)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008D8A File Offset: 0x00007D8A
		public XmlQualifiedName(string name) : this(name, string.Empty)
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008D98 File Offset: 0x00007D98
		public XmlQualifiedName(string name, string ns)
		{
			this.ns = ((ns == null) ? string.Empty : ns);
			this.name = ((name == null) ? string.Empty : name);
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00008DC2 File Offset: 0x00007DC2
		public string Namespace
		{
			get
			{
				return this.ns;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00008DCA File Offset: 0x00007DCA
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008DD4 File Offset: 0x00007DD4
		public override int GetHashCode()
		{
			if (this.hash == 0)
			{
				if (XmlQualifiedName.hashCodeDelegate == null)
				{
					XmlQualifiedName.hashCodeDelegate = XmlQualifiedName.GetHashCodeDelegate();
				}
				this.hash = XmlQualifiedName.hashCodeDelegate(this.Name, this.Name.Length, 0L);
			}
			return this.hash;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00008E23 File Offset: 0x00007E23
		public bool IsEmpty
		{
			get
			{
				return this.Name.Length == 0 && this.Namespace.Length == 0;
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008E42 File Offset: 0x00007E42
		public override string ToString()
		{
			if (this.Namespace.Length != 0)
			{
				return this.Namespace + ":" + this.Name;
			}
			return this.Name;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008E70 File Offset: 0x00007E70
		public override bool Equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			XmlQualifiedName xmlQualifiedName = other as XmlQualifiedName;
			return xmlQualifiedName != null && this.Name == xmlQualifiedName.Name && this.Namespace == xmlQualifiedName.Namespace;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008EBB File Offset: 0x00007EBB
		public static bool operator ==(XmlQualifiedName a, XmlQualifiedName b)
		{
			return a == b || (a != null && b != null && a.Name == b.Name && a.Namespace == b.Namespace);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008EF1 File Offset: 0x00007EF1
		public static bool operator !=(XmlQualifiedName a, XmlQualifiedName b)
		{
			return !(a == b);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008EFD File Offset: 0x00007EFD
		public static string ToString(string name, string ns)
		{
			if (ns != null && ns.Length != 0)
			{
				return ns + ":" + name;
			}
			return name;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008F18 File Offset: 0x00007F18
		[SecuritySafeCritical]
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private static XmlQualifiedName.HashCodeOfStringDelegate GetHashCodeDelegate()
		{
			if (!XmlQualifiedName.IsRandomizedHashingDisabled())
			{
				MethodInfo method = typeof(string).GetMethod("InternalMarvin32HashString", BindingFlags.Static | BindingFlags.NonPublic);
				if (method != null)
				{
					return (XmlQualifiedName.HashCodeOfStringDelegate)Delegate.CreateDelegate(typeof(XmlQualifiedName.HashCodeOfStringDelegate), method);
				}
			}
			return new XmlQualifiedName.HashCodeOfStringDelegate(XmlQualifiedName.GetHashCodeOfString);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008F68 File Offset: 0x00007F68
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool IsRandomizedHashingDisabled()
		{
			bool result = false;
			if (!XmlQualifiedName.ReadBoolFromXmlRegistrySettings(Registry.CurrentUser, "DisableRandomizedHashingOnXmlQualifiedName", ref result))
			{
				XmlQualifiedName.ReadBoolFromXmlRegistrySettings(Registry.LocalMachine, "DisableRandomizedHashingOnXmlQualifiedName", ref result);
			}
			return result;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008FA0 File Offset: 0x00007FA0
		[SecurityCritical]
		private static bool ReadBoolFromXmlRegistrySettings(RegistryKey hive, string regValueName, ref bool value)
		{
			try
			{
				using (RegistryKey registryKey = hive.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\XML", false))
				{
					if (registryKey != null && registryKey.GetValueKind(regValueName) == RegistryValueKind.DWord)
					{
						value = ((int)registryKey.GetValue(regValueName) == 1);
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000900C File Offset: 0x0000800C
		private static int GetHashCodeOfString(string s, int length, long additionalEntropy)
		{
			return s.GetHashCode();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009014 File Offset: 0x00008014
		internal void Init(string name, string ns)
		{
			this.name = name;
			this.ns = ns;
			this.hash = 0;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000902B File Offset: 0x0000802B
		internal void SetNamespace(string ns)
		{
			this.ns = ns;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009034 File Offset: 0x00008034
		internal void Verify()
		{
			XmlConvert.VerifyNCName(this.name);
			if (this.ns.Length != 0)
			{
				XmlConvert.ToUri(this.ns);
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000905B File Offset: 0x0000805B
		internal void Atomize(XmlNameTable nameTable)
		{
			this.name = nameTable.Add(this.name);
			this.ns = nameTable.Add(this.ns);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009084 File Offset: 0x00008084
		internal static XmlQualifiedName Parse(string s, IXmlNamespaceResolver nsmgr, out string prefix)
		{
			string text;
			ValidateNames.ParseQNameThrow(s, out prefix, out text);
			string text2 = nsmgr.LookupNamespace(prefix);
			if (text2 == null)
			{
				if (prefix.Length != 0)
				{
					throw new XmlException("Xml_UnknownNs", prefix);
				}
				text2 = string.Empty;
			}
			return new XmlQualifiedName(text, text2);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000090C9 File Offset: 0x000080C9
		internal XmlQualifiedName Clone()
		{
			return (XmlQualifiedName)base.MemberwiseClone();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000090D8 File Offset: 0x000080D8
		internal static int Compare(XmlQualifiedName a, XmlQualifiedName b)
		{
			if (null == a)
			{
				if (!(null == b))
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (null == b)
				{
					return 1;
				}
				int num = string.CompareOrdinal(a.Namespace, b.Namespace);
				if (num == 0)
				{
					num = string.CompareOrdinal(a.Name, b.Name);
				}
				return num;
			}
		}

		// Token: 0x040004F4 RID: 1268
		private static XmlQualifiedName.HashCodeOfStringDelegate hashCodeDelegate = null;

		// Token: 0x040004F5 RID: 1269
		private string name;

		// Token: 0x040004F6 RID: 1270
		private string ns;

		// Token: 0x040004F7 RID: 1271
		[NonSerialized]
		private int hash;

		// Token: 0x040004F8 RID: 1272
		public static readonly XmlQualifiedName Empty = new XmlQualifiedName(string.Empty);

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x06000203 RID: 515
		private delegate int HashCodeOfStringDelegate(string s, int sLen, long additionalEntropy);
	}
}
