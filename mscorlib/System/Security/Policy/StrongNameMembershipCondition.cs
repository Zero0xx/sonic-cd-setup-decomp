using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004B8 RID: 1208
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNameMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003024 RID: 12324 RVA: 0x000A52B9 File Offset: 0x000A42B9
		internal StrongNameMembershipCondition()
		{
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x000A52C4 File Offset: 0x000A42C4
		public StrongNameMembershipCondition(StrongNamePublicKeyBlob blob, string name, Version version)
		{
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			if (name != null && name.Equals(""))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyStrongName"));
			}
			this.m_publicKeyBlob = blob;
			this.m_name = name;
			this.m_version = version;
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000A5331 File Offset: 0x000A4331
		// (set) Token: 0x06003026 RID: 12326 RVA: 0x000A531A File Offset: 0x000A431A
		public StrongNamePublicKeyBlob PublicKey
		{
			get
			{
				if (this.m_publicKeyBlob == null && this.m_element != null)
				{
					this.ParseKeyBlob();
				}
				return this.m_publicKeyBlob;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PublicKey");
				}
				this.m_publicKeyBlob = value;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x000A53B4 File Offset: 0x000A43B4
		// (set) Token: 0x06003028 RID: 12328 RVA: 0x000A5350 File Offset: 0x000A4350
		public string Name
		{
			get
			{
				if (this.m_name == null && this.m_element != null)
				{
					this.ParseName();
				}
				return this.m_name;
			}
			set
			{
				if (value == null)
				{
					if (this.m_publicKeyBlob == null && this.m_element != null)
					{
						this.ParseKeyBlob();
					}
					if (this.m_version == null && this.m_element != null)
					{
						this.ParseVersion();
					}
					this.m_element = null;
				}
				else if (value.Length == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"));
				}
				this.m_name = value;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x000A5424 File Offset: 0x000A4424
		// (set) Token: 0x0600302A RID: 12330 RVA: 0x000A53D4 File Offset: 0x000A43D4
		public Version Version
		{
			get
			{
				if (this.m_version == null && this.m_element != null)
				{
					this.ParseVersion();
				}
				return this.m_version;
			}
			set
			{
				if (value == null)
				{
					if (this.m_name == null && this.m_element != null)
					{
						this.ParseName();
					}
					if (this.m_publicKeyBlob == null && this.m_element != null)
					{
						this.ParseKeyBlob();
					}
					this.m_element = null;
				}
				this.m_version = value;
			}
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000A5444 File Offset: 0x000A4444
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000A545C File Offset: 0x000A445C
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is StrongName)
				{
					StrongName strongName = (StrongName)hostEnumerator.Current;
					if (this.PublicKey != null && this.PublicKey.Equals(strongName.PublicKey) && (this.Name == null || (strongName.Name != null && StrongName.CompareNames(strongName.Name, this.Name))) && (this.Version == null || (strongName.Version != null && strongName.Version.CompareTo(this.Version) == 0)))
					{
						usedEvidence = strongName;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000A5505 File Offset: 0x000A4505
		public IMembershipCondition Copy()
		{
			return new StrongNameMembershipCondition(this.PublicKey, this.Name, this.Version);
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x000A551E File Offset: 0x000A451E
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000A5527 File Offset: 0x000A4527
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000A5534 File Offset: 0x000A4534
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.StrongNameMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.PublicKey != null)
			{
				securityElement.AddAttribute("PublicKeyBlob", Hex.EncodeHexString(this.PublicKey.PublicKey));
			}
			if (this.Name != null)
			{
				securityElement.AddAttribute("Name", this.Name);
			}
			if (this.Version != null)
			{
				securityElement.AddAttribute("AssemblyVersion", this.Version.ToString());
			}
			return securityElement;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000A55C8 File Offset: 0x000A45C8
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this)
			{
				this.m_name = null;
				this.m_publicKeyBlob = null;
				this.m_version = null;
				this.m_element = e;
			}
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x000A5644 File Offset: 0x000A4644
		private void ParseName()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Name");
					this.m_name = ((text == null) ? null : text);
					if (this.m_version != null && this.m_name != null && this.m_publicKeyBlob != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000A56BC File Offset: 0x000A46BC
		private void ParseKeyBlob()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("PublicKeyBlob");
					StrongNamePublicKeyBlob strongNamePublicKeyBlob = new StrongNamePublicKeyBlob();
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_BlobCannotBeNull"));
					}
					strongNamePublicKeyBlob.PublicKey = Hex.DecodeHexString(text);
					this.m_publicKeyBlob = strongNamePublicKeyBlob;
					if (this.m_version != null && this.m_name != null && this.m_publicKeyBlob != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000A5754 File Offset: 0x000A4754
		private void ParseVersion()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("AssemblyVersion");
					this.m_version = ((text == null) ? null : new Version(text));
					if (this.m_version != null && this.m_name != null && this.m_publicKeyBlob != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000A57D0 File Offset: 0x000A47D0
		public override string ToString()
		{
			string text = "";
			string text2 = "";
			if (this.Name != null)
			{
				text = " " + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("StrongName_Name"), new object[]
				{
					this.Name
				});
			}
			if (this.Version != null)
			{
				text2 = " " + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("StrongName_Version"), new object[]
				{
					this.Version
				});
			}
			return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("StrongName_ToString"), new object[]
			{
				Hex.EncodeHexString(this.PublicKey.PublicKey),
				text,
				text2
			});
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000A5898 File Offset: 0x000A4898
		public override bool Equals(object o)
		{
			StrongNameMembershipCondition strongNameMembershipCondition = o as StrongNameMembershipCondition;
			if (strongNameMembershipCondition != null)
			{
				if (this.m_publicKeyBlob == null && this.m_element != null)
				{
					this.ParseKeyBlob();
				}
				if (strongNameMembershipCondition.m_publicKeyBlob == null && strongNameMembershipCondition.m_element != null)
				{
					strongNameMembershipCondition.ParseKeyBlob();
				}
				if (object.Equals(this.m_publicKeyBlob, strongNameMembershipCondition.m_publicKeyBlob))
				{
					if (this.m_name == null && this.m_element != null)
					{
						this.ParseName();
					}
					if (strongNameMembershipCondition.m_name == null && strongNameMembershipCondition.m_element != null)
					{
						strongNameMembershipCondition.ParseName();
					}
					if (object.Equals(this.m_name, strongNameMembershipCondition.m_name))
					{
						if (this.m_version == null && this.m_element != null)
						{
							this.ParseVersion();
						}
						if (strongNameMembershipCondition.m_version == null && strongNameMembershipCondition.m_element != null)
						{
							strongNameMembershipCondition.ParseVersion();
						}
						if (object.Equals(this.m_version, strongNameMembershipCondition.m_version))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000A5984 File Offset: 0x000A4984
		public override int GetHashCode()
		{
			if (this.m_publicKeyBlob == null && this.m_element != null)
			{
				this.ParseKeyBlob();
			}
			if (this.m_publicKeyBlob != null)
			{
				return this.m_publicKeyBlob.GetHashCode();
			}
			if (this.m_name == null && this.m_element != null)
			{
				this.ParseName();
			}
			if (this.m_version == null && this.m_element != null)
			{
				this.ParseVersion();
			}
			if (this.m_name != null || this.m_version != null)
			{
				return ((this.m_name == null) ? 0 : this.m_name.GetHashCode()) + ((this.m_version == null) ? 0 : this.m_version.GetHashCode());
			}
			return typeof(StrongNameMembershipCondition).GetHashCode();
		}

		// Token: 0x04001860 RID: 6240
		private const string s_tagName = "Name";

		// Token: 0x04001861 RID: 6241
		private const string s_tagVersion = "AssemblyVersion";

		// Token: 0x04001862 RID: 6242
		private const string s_tagPublicKeyBlob = "PublicKeyBlob";

		// Token: 0x04001863 RID: 6243
		private StrongNamePublicKeyBlob m_publicKeyBlob;

		// Token: 0x04001864 RID: 6244
		private string m_name;

		// Token: 0x04001865 RID: 6245
		private Version m_version;

		// Token: 0x04001866 RID: 6246
		private SecurityElement m_element;
	}
}
