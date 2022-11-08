using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000748 RID: 1864
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	[ComVisible(true)]
	public sealed class SoapTypeAttribute : SoapAttribute
	{
		// Token: 0x0600428E RID: 17038 RVA: 0x000E26E7 File Offset: 0x000E16E7
		internal bool IsInteropXmlElement()
		{
			return (this._explicitlySet & (SoapTypeAttribute.ExplicitlySet.XmlElementName | SoapTypeAttribute.ExplicitlySet.XmlNamespace)) != SoapTypeAttribute.ExplicitlySet.None;
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x000E26F7 File Offset: 0x000E16F7
		internal bool IsInteropXmlType()
		{
			return (this._explicitlySet & (SoapTypeAttribute.ExplicitlySet.XmlTypeName | SoapTypeAttribute.ExplicitlySet.XmlTypeNamespace)) != SoapTypeAttribute.ExplicitlySet.None;
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06004290 RID: 17040 RVA: 0x000E2708 File Offset: 0x000E1708
		// (set) Token: 0x06004291 RID: 17041 RVA: 0x000E2710 File Offset: 0x000E1710
		public SoapOption SoapOptions
		{
			get
			{
				return this._SoapOptions;
			}
			set
			{
				this._SoapOptions = value;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06004292 RID: 17042 RVA: 0x000E2719 File Offset: 0x000E1719
		// (set) Token: 0x06004293 RID: 17043 RVA: 0x000E2747 File Offset: 0x000E1747
		public string XmlElementName
		{
			get
			{
				if (this._XmlElementName == null && this.ReflectInfo != null)
				{
					this._XmlElementName = SoapTypeAttribute.GetTypeName((Type)this.ReflectInfo);
				}
				return this._XmlElementName;
			}
			set
			{
				this._XmlElementName = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlElementName;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06004294 RID: 17044 RVA: 0x000E275E File Offset: 0x000E175E
		// (set) Token: 0x06004295 RID: 17045 RVA: 0x000E2782 File Offset: 0x000E1782
		public override string XmlNamespace
		{
			get
			{
				if (this.ProtXmlNamespace == null && this.ReflectInfo != null)
				{
					this.ProtXmlNamespace = this.XmlTypeNamespace;
				}
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlNamespace;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x000E2799 File Offset: 0x000E1799
		// (set) Token: 0x06004297 RID: 17047 RVA: 0x000E27C7 File Offset: 0x000E17C7
		public string XmlTypeName
		{
			get
			{
				if (this._XmlTypeName == null && this.ReflectInfo != null)
				{
					this._XmlTypeName = SoapTypeAttribute.GetTypeName((Type)this.ReflectInfo);
				}
				return this._XmlTypeName;
			}
			set
			{
				this._XmlTypeName = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlTypeName;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06004298 RID: 17048 RVA: 0x000E27DE File Offset: 0x000E17DE
		// (set) Token: 0x06004299 RID: 17049 RVA: 0x000E280D File Offset: 0x000E180D
		public string XmlTypeNamespace
		{
			get
			{
				if (this._XmlTypeNamespace == null && this.ReflectInfo != null)
				{
					this._XmlTypeNamespace = XmlNamespaceEncoder.GetXmlNamespaceForTypeNamespace((Type)this.ReflectInfo, null);
				}
				return this._XmlTypeNamespace;
			}
			set
			{
				this._XmlTypeNamespace = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlTypeNamespace;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600429A RID: 17050 RVA: 0x000E2824 File Offset: 0x000E1824
		// (set) Token: 0x0600429B RID: 17051 RVA: 0x000E282C File Offset: 0x000E182C
		public XmlFieldOrderOption XmlFieldOrder
		{
			get
			{
				return this._XmlFieldOrder;
			}
			set
			{
				this._XmlFieldOrder = value;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600429C RID: 17052 RVA: 0x000E2835 File Offset: 0x000E1835
		// (set) Token: 0x0600429D RID: 17053 RVA: 0x000E2838 File Offset: 0x000E1838
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

		// Token: 0x0600429E RID: 17054 RVA: 0x000E284C File Offset: 0x000E184C
		private static string GetTypeName(Type t)
		{
			if (!t.IsNested)
			{
				return t.Name;
			}
			string fullName = t.FullName;
			string @namespace = t.Namespace;
			if (@namespace == null || @namespace.Length == 0)
			{
				return fullName;
			}
			return fullName.Substring(@namespace.Length + 1);
		}

		// Token: 0x04002170 RID: 8560
		private SoapTypeAttribute.ExplicitlySet _explicitlySet;

		// Token: 0x04002171 RID: 8561
		private SoapOption _SoapOptions;

		// Token: 0x04002172 RID: 8562
		private string _XmlElementName;

		// Token: 0x04002173 RID: 8563
		private string _XmlTypeName;

		// Token: 0x04002174 RID: 8564
		private string _XmlTypeNamespace;

		// Token: 0x04002175 RID: 8565
		private XmlFieldOrderOption _XmlFieldOrder;

		// Token: 0x02000749 RID: 1865
		[Flags]
		[Serializable]
		private enum ExplicitlySet
		{
			// Token: 0x04002177 RID: 8567
			None = 0,
			// Token: 0x04002178 RID: 8568
			XmlElementName = 1,
			// Token: 0x04002179 RID: 8569
			XmlNamespace = 2,
			// Token: 0x0400217A RID: 8570
			XmlTypeName = 4,
			// Token: 0x0400217B RID: 8571
			XmlTypeNamespace = 8
		}
	}
}
