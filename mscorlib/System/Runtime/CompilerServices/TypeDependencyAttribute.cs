using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F4 RID: 1524
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	internal sealed class TypeDependencyAttribute : Attribute
	{
		// Token: 0x060037FA RID: 14330 RVA: 0x000BBCF0 File Offset: 0x000BACF0
		public TypeDependencyAttribute(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this.typeName = typeName;
		}

		// Token: 0x04001D06 RID: 7430
		private string typeName;
	}
}
