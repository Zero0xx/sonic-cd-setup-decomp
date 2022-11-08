using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FF RID: 1279
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComAliasNameAttribute : Attribute
	{
		// Token: 0x0600318F RID: 12687 RVA: 0x000A983E File Offset: 0x000A883E
		public ComAliasNameAttribute(string alias)
		{
			this._val = alias;
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x000A984D File Offset: 0x000A884D
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040019A1 RID: 6561
		internal string _val;
	}
}
