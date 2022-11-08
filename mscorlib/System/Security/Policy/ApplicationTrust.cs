using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x0200049A RID: 1178
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationTrust : ISecurityEncodable
	{
		// Token: 0x06002E9C RID: 11932 RVA: 0x0009D5F1 File Offset: 0x0009C5F1
		public ApplicationTrust(ApplicationIdentity applicationIdentity) : this()
		{
			this.ApplicationIdentity = applicationIdentity;
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0009D600 File Offset: 0x0009C600
		public ApplicationTrust() : this(new PermissionSet(PermissionState.None))
		{
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x0009D60E File Offset: 0x0009C60E
		internal ApplicationTrust(PermissionSet defaultGrantSet) : this(defaultGrantSet, null)
		{
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x0009D618 File Offset: 0x0009C618
		internal ApplicationTrust(PermissionSet defaultGrantSet, StrongName[] fullTrustAssemblies)
		{
			this.DefaultGrantSet = new PolicyStatement(defaultGrantSet);
			this.FullTrustAssemblies = fullTrustAssemblies;
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x0009D633 File Offset: 0x0009C633
		// (set) Token: 0x06002EA1 RID: 11937 RVA: 0x0009D63B File Offset: 0x0009C63B
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return this.m_appId;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(Environment.GetResourceString("Argument_InvalidAppId"));
				}
				this.m_appId = value;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002EA2 RID: 11938 RVA: 0x0009D657 File Offset: 0x0009C657
		// (set) Token: 0x06002EA3 RID: 11939 RVA: 0x0009D673 File Offset: 0x0009C673
		public PolicyStatement DefaultGrantSet
		{
			get
			{
				if (this.m_psDefaultGrant == null)
				{
					return new PolicyStatement(new PermissionSet(PermissionState.None));
				}
				return this.m_psDefaultGrant;
			}
			set
			{
				if (value == null)
				{
					this.m_psDefaultGrant = null;
					this.m_grantSetSpecialFlags = 0;
					return;
				}
				this.m_psDefaultGrant = value;
				this.m_grantSetSpecialFlags = SecurityManager.GetSpecialFlags(this.m_psDefaultGrant.PermissionSet, null);
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002EA4 RID: 11940 RVA: 0x0009D6A5 File Offset: 0x0009C6A5
		// (set) Token: 0x06002EA5 RID: 11941 RVA: 0x0009D6AD File Offset: 0x0009C6AD
		internal StrongName[] FullTrustAssemblies
		{
			get
			{
				return this.m_fullTrustAssemblies;
			}
			set
			{
				this.m_fullTrustAssemblies = value;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002EA6 RID: 11942 RVA: 0x0009D6B6 File Offset: 0x0009C6B6
		// (set) Token: 0x06002EA7 RID: 11943 RVA: 0x0009D6BE File Offset: 0x0009C6BE
		public bool IsApplicationTrustedToRun
		{
			get
			{
				return this.m_appTrustedToRun;
			}
			set
			{
				this.m_appTrustedToRun = value;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x0009D6C7 File Offset: 0x0009C6C7
		// (set) Token: 0x06002EA9 RID: 11945 RVA: 0x0009D6CF File Offset: 0x0009C6CF
		public bool Persist
		{
			get
			{
				return this.m_persist;
			}
			set
			{
				this.m_persist = value;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x0009D6D8 File Offset: 0x0009C6D8
		// (set) Token: 0x06002EAB RID: 11947 RVA: 0x0009D700 File Offset: 0x0009C700
		public object ExtraInfo
		{
			get
			{
				if (this.m_elExtraInfo != null)
				{
					this.m_extraInfo = ApplicationTrust.ObjectFromXml(this.m_elExtraInfo);
					this.m_elExtraInfo = null;
				}
				return this.m_extraInfo;
			}
			set
			{
				this.m_elExtraInfo = null;
				this.m_extraInfo = value;
			}
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x0009D710 File Offset: 0x0009C710
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("ApplicationTrust");
			securityElement.AddAttribute("version", "1");
			if (this.m_appId != null)
			{
				securityElement.AddAttribute("FullName", SecurityElement.Escape(this.m_appId.FullName));
			}
			if (this.m_appTrustedToRun)
			{
				securityElement.AddAttribute("TrustedToRun", "true");
			}
			if (this.m_persist)
			{
				securityElement.AddAttribute("Persist", "true");
			}
			if (this.m_psDefaultGrant != null)
			{
				SecurityElement securityElement2 = new SecurityElement("DefaultGrant");
				securityElement2.AddChild(this.m_psDefaultGrant.ToXml());
				securityElement.AddChild(securityElement2);
			}
			if (this.m_fullTrustAssemblies != null)
			{
				SecurityElement securityElement3 = new SecurityElement("FullTrustAssemblies");
				for (int i = 0; i < this.m_fullTrustAssemblies.Length; i++)
				{
					if (this.m_fullTrustAssemblies[i] != null)
					{
						securityElement3.AddChild(this.m_fullTrustAssemblies[i].ToXml());
					}
				}
				securityElement.AddChild(securityElement3);
			}
			if (this.ExtraInfo != null)
			{
				securityElement.AddChild(ApplicationTrust.ObjectToXml("ExtraInfo", this.ExtraInfo));
			}
			return securityElement;
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x0009D820 File Offset: 0x0009C820
		public void FromXml(SecurityElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (string.Compare(element.Tag, "ApplicationTrust", StringComparison.Ordinal) != 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXML"));
			}
			this.m_psDefaultGrant = null;
			this.m_grantSetSpecialFlags = 0;
			this.m_fullTrustAssemblies = null;
			this.m_appTrustedToRun = false;
			string text = element.Attribute("TrustedToRun");
			if (text != null && string.Compare(text, "true", StringComparison.Ordinal) == 0)
			{
				this.m_appTrustedToRun = true;
			}
			string text2 = element.Attribute("Persist");
			if (text2 != null && string.Compare(text2, "true", StringComparison.Ordinal) == 0)
			{
				this.m_persist = true;
			}
			string text3 = element.Attribute("FullName");
			if (text3 != null && text3.Length > 0)
			{
				this.m_appId = new ApplicationIdentity(text3);
			}
			SecurityElement securityElement = element.SearchForChildByTag("DefaultGrant");
			if (securityElement != null)
			{
				SecurityElement securityElement2 = securityElement.SearchForChildByTag("PolicyStatement");
				if (securityElement2 != null)
				{
					PolicyStatement policyStatement = new PolicyStatement(null);
					policyStatement.FromXml(securityElement2);
					this.m_psDefaultGrant = policyStatement;
					this.m_grantSetSpecialFlags = SecurityManager.GetSpecialFlags(policyStatement.PermissionSet, null);
				}
			}
			SecurityElement securityElement3 = element.SearchForChildByTag("FullTrustAssemblies");
			if (securityElement3 != null && securityElement3.InternalChildren != null)
			{
				this.m_fullTrustAssemblies = new StrongName[securityElement3.Children.Count];
				IEnumerator enumerator = securityElement3.Children.GetEnumerator();
				int num = 0;
				while (enumerator.MoveNext())
				{
					this.m_fullTrustAssemblies[num] = new StrongName();
					this.m_fullTrustAssemblies[num].FromXml(enumerator.Current as SecurityElement);
					num++;
				}
			}
			this.m_elExtraInfo = element.SearchForChildByTag("ExtraInfo");
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x0009D9C0 File Offset: 0x0009C9C0
		private static SecurityElement ObjectToXml(string tag, object obj)
		{
			ISecurityEncodable securityEncodable = obj as ISecurityEncodable;
			SecurityElement securityElement;
			if (securityEncodable != null)
			{
				securityElement = securityEncodable.ToXml();
				if (!securityElement.Tag.Equals(tag))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXML"));
				}
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			byte[] sArray = memoryStream.ToArray();
			securityElement = new SecurityElement(tag);
			securityElement.AddAttribute("Data", Hex.EncodeHexString(sArray));
			return securityElement;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x0009DA34 File Offset: 0x0009CA34
		private static object ObjectFromXml(SecurityElement elObject)
		{
			if (elObject.Attribute("class") != null)
			{
				ISecurityEncodable securityEncodable = XMLUtil.CreateCodeGroup(elObject) as ISecurityEncodable;
				if (securityEncodable != null)
				{
					securityEncodable.FromXml(elObject);
					return securityEncodable;
				}
			}
			string hexString = elObject.Attribute("Data");
			MemoryStream serializationStream = new MemoryStream(Hex.DecodeHexString(hexString));
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return binaryFormatter.Deserialize(serializationStream);
		}

		// Token: 0x040017D9 RID: 6105
		private ApplicationIdentity m_appId;

		// Token: 0x040017DA RID: 6106
		private bool m_appTrustedToRun;

		// Token: 0x040017DB RID: 6107
		private bool m_persist;

		// Token: 0x040017DC RID: 6108
		private object m_extraInfo;

		// Token: 0x040017DD RID: 6109
		private SecurityElement m_elExtraInfo;

		// Token: 0x040017DE RID: 6110
		private PolicyStatement m_psDefaultGrant;

		// Token: 0x040017DF RID: 6111
		private StrongName[] m_fullTrustAssemblies;

		// Token: 0x040017E0 RID: 6112
		[NonSerialized]
		private int m_grantSetSpecialFlags;
	}
}
