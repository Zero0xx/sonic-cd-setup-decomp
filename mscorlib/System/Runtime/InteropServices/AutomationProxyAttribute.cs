using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000500 RID: 1280
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class AutomationProxyAttribute : Attribute
	{
		// Token: 0x06003191 RID: 12689 RVA: 0x000A9855 File Offset: 0x000A8855
		public AutomationProxyAttribute(bool val)
		{
			this._val = val;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x000A9864 File Offset: 0x000A8864
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040019A2 RID: 6562
		internal bool _val;
	}
}
