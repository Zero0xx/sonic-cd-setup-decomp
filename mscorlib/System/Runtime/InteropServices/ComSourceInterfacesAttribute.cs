using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EB RID: 1259
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class ComSourceInterfacesAttribute : Attribute
	{
		// Token: 0x06003156 RID: 12630 RVA: 0x000A9115 File Offset: 0x000A8115
		public ComSourceInterfacesAttribute(string sourceInterfaces)
		{
			this._val = sourceInterfaces;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000A9124 File Offset: 0x000A8124
		public ComSourceInterfacesAttribute(Type sourceInterface)
		{
			this._val = sourceInterface.FullName;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x000A9138 File Offset: 0x000A8138
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2)
		{
			this._val = sourceInterface1.FullName + "\0" + sourceInterface2.FullName;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x000A915C File Offset: 0x000A815C
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3)
		{
			this._val = string.Concat(new string[]
			{
				sourceInterface1.FullName,
				"\0",
				sourceInterface2.FullName,
				"\0",
				sourceInterface3.FullName
			});
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000A91B0 File Offset: 0x000A81B0
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4)
		{
			this._val = string.Concat(new string[]
			{
				sourceInterface1.FullName,
				"\0",
				sourceInterface2.FullName,
				"\0",
				sourceInterface3.FullName,
				"\0",
				sourceInterface4.FullName
			});
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x0600315B RID: 12635 RVA: 0x000A9213 File Offset: 0x000A8213
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001907 RID: 6407
		internal string _val;
	}
}
