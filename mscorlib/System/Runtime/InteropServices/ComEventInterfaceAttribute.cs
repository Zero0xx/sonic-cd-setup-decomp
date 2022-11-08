using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000503 RID: 1283
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	public sealed class ComEventInterfaceAttribute : Attribute
	{
		// Token: 0x06003198 RID: 12696 RVA: 0x000A98A9 File Offset: 0x000A88A9
		public ComEventInterfaceAttribute(Type SourceInterface, Type EventProvider)
		{
			this._SourceInterface = SourceInterface;
			this._EventProvider = EventProvider;
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x000A98BF File Offset: 0x000A88BF
		public Type SourceInterface
		{
			get
			{
				return this._SourceInterface;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x000A98C7 File Offset: 0x000A88C7
		public Type EventProvider
		{
			get
			{
				return this._EventProvider;
			}
		}

		// Token: 0x040019A6 RID: 6566
		internal Type _SourceInterface;

		// Token: 0x040019A7 RID: 6567
		internal Type _EventProvider;
	}
}
