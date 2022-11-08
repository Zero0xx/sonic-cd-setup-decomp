using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E3 RID: 1251
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibImportClassAttribute : Attribute
	{
		// Token: 0x06003149 RID: 12617 RVA: 0x000A907E File Offset: 0x000A807E
		public TypeLibImportClassAttribute(Type importClass)
		{
			this._importClassName = importClass.ToString();
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600314A RID: 12618 RVA: 0x000A9092 File Offset: 0x000A8092
		public string Value
		{
			get
			{
				return this._importClassName;
			}
		}

		// Token: 0x040018FE RID: 6398
		internal string _importClassName;
	}
}
