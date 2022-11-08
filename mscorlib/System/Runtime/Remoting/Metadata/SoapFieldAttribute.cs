using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x0200074B RID: 1867
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class SoapFieldAttribute : SoapAttribute
	{
		// Token: 0x060042AF RID: 17071 RVA: 0x000E29E3 File Offset: 0x000E19E3
		public bool IsInteropXmlElement()
		{
			return (this._explicitlySet & SoapFieldAttribute.ExplicitlySet.XmlElementName) != SoapFieldAttribute.ExplicitlySet.None;
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x000E29F3 File Offset: 0x000E19F3
		// (set) Token: 0x060042B1 RID: 17073 RVA: 0x000E2A21 File Offset: 0x000E1A21
		public string XmlElementName
		{
			get
			{
				if (this._xmlElementName == null && this.ReflectInfo != null)
				{
					this._xmlElementName = ((FieldInfo)this.ReflectInfo).Name;
				}
				return this._xmlElementName;
			}
			set
			{
				this._xmlElementName = value;
				this._explicitlySet |= SoapFieldAttribute.ExplicitlySet.XmlElementName;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x000E2A38 File Offset: 0x000E1A38
		// (set) Token: 0x060042B3 RID: 17075 RVA: 0x000E2A40 File Offset: 0x000E1A40
		public int Order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		// Token: 0x04002181 RID: 8577
		private SoapFieldAttribute.ExplicitlySet _explicitlySet;

		// Token: 0x04002182 RID: 8578
		private string _xmlElementName;

		// Token: 0x04002183 RID: 8579
		private int _order;

		// Token: 0x0200074C RID: 1868
		[Flags]
		[Serializable]
		private enum ExplicitlySet
		{
			// Token: 0x04002185 RID: 8581
			None = 0,
			// Token: 0x04002186 RID: 8582
			XmlElementName = 1
		}
	}
}
