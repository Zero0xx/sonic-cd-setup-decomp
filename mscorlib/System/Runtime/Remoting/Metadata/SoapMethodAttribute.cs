using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x0200074A RID: 1866
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class SoapMethodAttribute : SoapAttribute
	{
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x060042A0 RID: 17056 RVA: 0x000E289B File Offset: 0x000E189B
		internal bool SoapActionExplicitySet
		{
			get
			{
				return this._bSoapActionExplicitySet;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x000E28A3 File Offset: 0x000E18A3
		// (set) Token: 0x060042A2 RID: 17058 RVA: 0x000E28D9 File Offset: 0x000E18D9
		public string SoapAction
		{
			get
			{
				if (this._SoapAction == null)
				{
					this._SoapAction = this.XmlTypeNamespaceOfDeclaringType + "#" + ((MemberInfo)this.ReflectInfo).Name;
				}
				return this._SoapAction;
			}
			set
			{
				this._SoapAction = value;
				this._bSoapActionExplicitySet = true;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x060042A3 RID: 17059 RVA: 0x000E28E9 File Offset: 0x000E18E9
		// (set) Token: 0x060042A4 RID: 17060 RVA: 0x000E28EC File Offset: 0x000E18EC
		public override bool UseAttribute
		{
			get
			{
				return false;
			}
			set
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Attribute_UseAttributeNotsettable"));
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x000E28FD File Offset: 0x000E18FD
		// (set) Token: 0x060042A6 RID: 17062 RVA: 0x000E2919 File Offset: 0x000E1919
		public override string XmlNamespace
		{
			get
			{
				if (this.ProtXmlNamespace == null)
				{
					this.ProtXmlNamespace = this.XmlTypeNamespaceOfDeclaringType;
				}
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x060042A7 RID: 17063 RVA: 0x000E2922 File Offset: 0x000E1922
		// (set) Token: 0x060042A8 RID: 17064 RVA: 0x000E295A File Offset: 0x000E195A
		public string ResponseXmlElementName
		{
			get
			{
				if (this._responseXmlElementName == null && this.ReflectInfo != null)
				{
					this._responseXmlElementName = ((MemberInfo)this.ReflectInfo).Name + "Response";
				}
				return this._responseXmlElementName;
			}
			set
			{
				this._responseXmlElementName = value;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x060042A9 RID: 17065 RVA: 0x000E2963 File Offset: 0x000E1963
		// (set) Token: 0x060042AA RID: 17066 RVA: 0x000E297F File Offset: 0x000E197F
		public string ResponseXmlNamespace
		{
			get
			{
				if (this._responseXmlNamespace == null)
				{
					this._responseXmlNamespace = this.XmlNamespace;
				}
				return this._responseXmlNamespace;
			}
			set
			{
				this._responseXmlNamespace = value;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x060042AB RID: 17067 RVA: 0x000E2988 File Offset: 0x000E1988
		// (set) Token: 0x060042AC RID: 17068 RVA: 0x000E29A3 File Offset: 0x000E19A3
		public string ReturnXmlElementName
		{
			get
			{
				if (this._returnXmlElementName == null)
				{
					this._returnXmlElementName = "return";
				}
				return this._returnXmlElementName;
			}
			set
			{
				this._returnXmlElementName = value;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x000E29AC File Offset: 0x000E19AC
		private string XmlTypeNamespaceOfDeclaringType
		{
			get
			{
				if (this.ReflectInfo != null)
				{
					Type declaringType = ((MemberInfo)this.ReflectInfo).DeclaringType;
					return XmlNamespaceEncoder.GetXmlNamespaceForType(declaringType, null);
				}
				return null;
			}
		}

		// Token: 0x0400217C RID: 8572
		private string _SoapAction;

		// Token: 0x0400217D RID: 8573
		private string _responseXmlElementName;

		// Token: 0x0400217E RID: 8574
		private string _responseXmlNamespace;

		// Token: 0x0400217F RID: 8575
		private string _returnXmlElementName;

		// Token: 0x04002180 RID: 8576
		private bool _bSoapActionExplicitySet;
	}
}
