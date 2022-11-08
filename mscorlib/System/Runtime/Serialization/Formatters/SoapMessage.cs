using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007BB RID: 1979
	[ComVisible(true)]
	[Serializable]
	public class SoapMessage : ISoapMessage
	{
		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x0600467C RID: 18044 RVA: 0x000F07E6 File Offset: 0x000EF7E6
		// (set) Token: 0x0600467D RID: 18045 RVA: 0x000F07EE File Offset: 0x000EF7EE
		public string[] ParamNames
		{
			get
			{
				return this.paramNames;
			}
			set
			{
				this.paramNames = value;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x000F07F7 File Offset: 0x000EF7F7
		// (set) Token: 0x0600467F RID: 18047 RVA: 0x000F07FF File Offset: 0x000EF7FF
		public object[] ParamValues
		{
			get
			{
				return this.paramValues;
			}
			set
			{
				this.paramValues = value;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004680 RID: 18048 RVA: 0x000F0808 File Offset: 0x000EF808
		// (set) Token: 0x06004681 RID: 18049 RVA: 0x000F0810 File Offset: 0x000EF810
		public Type[] ParamTypes
		{
			get
			{
				return this.paramTypes;
			}
			set
			{
				this.paramTypes = value;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06004682 RID: 18050 RVA: 0x000F0819 File Offset: 0x000EF819
		// (set) Token: 0x06004683 RID: 18051 RVA: 0x000F0821 File Offset: 0x000EF821
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
			set
			{
				this.methodName = value;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06004684 RID: 18052 RVA: 0x000F082A File Offset: 0x000EF82A
		// (set) Token: 0x06004685 RID: 18053 RVA: 0x000F0832 File Offset: 0x000EF832
		public string XmlNameSpace
		{
			get
			{
				return this.xmlNameSpace;
			}
			set
			{
				this.xmlNameSpace = value;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004686 RID: 18054 RVA: 0x000F083B File Offset: 0x000EF83B
		// (set) Token: 0x06004687 RID: 18055 RVA: 0x000F0843 File Offset: 0x000EF843
		public Header[] Headers
		{
			get
			{
				return this.headers;
			}
			set
			{
				this.headers = value;
			}
		}

		// Token: 0x0400230E RID: 8974
		internal string[] paramNames;

		// Token: 0x0400230F RID: 8975
		internal object[] paramValues;

		// Token: 0x04002310 RID: 8976
		internal Type[] paramTypes;

		// Token: 0x04002311 RID: 8977
		internal string methodName;

		// Token: 0x04002312 RID: 8978
		internal string xmlNameSpace;

		// Token: 0x04002313 RID: 8979
		internal Header[] headers;
	}
}
