using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000609 RID: 1545
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class TypeForwardedToAttribute : Attribute
	{
		// Token: 0x06003803 RID: 14339 RVA: 0x000BBD45 File Offset: 0x000BAD45
		public TypeForwardedToAttribute(Type destination)
		{
			this._destination = destination;
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x000BBD54 File Offset: 0x000BAD54
		public Type Destination
		{
			get
			{
				return this._destination;
			}
		}

		// Token: 0x04001D07 RID: 7431
		private Type _destination;
	}
}
