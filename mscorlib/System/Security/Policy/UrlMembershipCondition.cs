using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004BB RID: 1211
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003052 RID: 12370 RVA: 0x000A5E51 File Offset: 0x000A4E51
		internal UrlMembershipCondition()
		{
			this.m_url = null;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000A5E60 File Offset: 0x000A4E60
		public UrlMembershipCondition(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this.m_url = new URLString(url, false, true);
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x000A5EA0 File Offset: 0x000A4EA0
		// (set) Token: 0x06003054 RID: 12372 RVA: 0x000A5E84 File Offset: 0x000A4E84
		public string Url
		{
			get
			{
				if (this.m_url == null && this.m_element != null)
				{
					this.ParseURL();
				}
				return this.m_url.ToString();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_url = new URLString(value);
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000A5EC4 File Offset: 0x000A4EC4
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000A5EDC File Offset: 0x000A4EDC
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
				if (hostEnumerator.Current is Url)
				{
					if (this.m_url == null && this.m_element != null)
					{
						this.ParseURL();
					}
					if (((Url)hostEnumerator.Current).GetURLString().IsSubsetOf(this.m_url))
					{
						usedEvidence = hostEnumerator.Current;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x000A5F50 File Offset: 0x000A4F50
		public IMembershipCondition Copy()
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			return new UrlMembershipCondition
			{
				m_url = new URLString(this.m_url.ToString())
			};
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000A5F90 File Offset: 0x000A4F90
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x000A5F99 File Offset: 0x000A4F99
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x000A5FA4 File Offset: 0x000A4FA4
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.UrlMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_url != null)
			{
				securityElement.AddAttribute("Url", this.m_url.ToString());
			}
			return securityElement;
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000A6014 File Offset: 0x000A5014
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
				this.m_element = e;
				this.m_url = null;
			}
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000A6080 File Offset: 0x000A5080
		private void ParseURL()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Url");
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_UrlCannotBeNull"));
					}
					this.m_url = new URLString(text);
					this.m_element = null;
				}
			}
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000A60F0 File Offset: 0x000A50F0
		public override bool Equals(object o)
		{
			UrlMembershipCondition urlMembershipCondition = o as UrlMembershipCondition;
			if (urlMembershipCondition != null)
			{
				if (this.m_url == null && this.m_element != null)
				{
					this.ParseURL();
				}
				if (urlMembershipCondition.m_url == null && urlMembershipCondition.m_element != null)
				{
					urlMembershipCondition.ParseURL();
				}
				if (object.Equals(this.m_url, urlMembershipCondition.m_url))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000A6149 File Offset: 0x000A5149
		public override int GetHashCode()
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			if (this.m_url != null)
			{
				return this.m_url.GetHashCode();
			}
			return typeof(UrlMembershipCondition).GetHashCode();
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000A6184 File Offset: 0x000A5184
		public override string ToString()
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			if (this.m_url != null)
			{
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Url_ToStringArg"), new object[]
				{
					this.m_url.ToString()
				});
			}
			return Environment.GetResourceString("Url_ToString");
		}

		// Token: 0x04001868 RID: 6248
		private URLString m_url;

		// Token: 0x04001869 RID: 6249
		private SecurityElement m_element;
	}
}
