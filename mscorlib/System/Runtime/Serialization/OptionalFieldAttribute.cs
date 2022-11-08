using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036F RID: 879
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class OptionalFieldAttribute : Attribute
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x000572AE File Offset: 0x000562AE
		// (set) Token: 0x0600228D RID: 8845 RVA: 0x000572B6 File Offset: 0x000562B6
		public int VersionAdded
		{
			get
			{
				return this.versionAdded;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Serialization_OptionalFieldVersionValue"));
				}
				this.versionAdded = value;
			}
		}

		// Token: 0x04000E8F RID: 3727
		private int versionAdded = 1;
	}
}
