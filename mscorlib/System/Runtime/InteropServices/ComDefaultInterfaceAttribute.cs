using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DF RID: 1247
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComDefaultInterfaceAttribute : Attribute
	{
		// Token: 0x06003142 RID: 12610 RVA: 0x000A902A File Offset: 0x000A802A
		public ComDefaultInterfaceAttribute(Type defaultInterface)
		{
			this._val = defaultInterface;
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x000A9039 File Offset: 0x000A8039
		public Type Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018F7 RID: 6391
		internal Type _val;
	}
}
