using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E7 RID: 1511
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class FixedBufferAttribute : Attribute
	{
		// Token: 0x060037E4 RID: 14308 RVA: 0x000BBBDF File Offset: 0x000BABDF
		public FixedBufferAttribute(Type elementType, int length)
		{
			this.elementType = elementType;
			this.length = length;
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060037E5 RID: 14309 RVA: 0x000BBBF5 File Offset: 0x000BABF5
		public Type ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000BBBFD File Offset: 0x000BABFD
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x04001CEB RID: 7403
		private Type elementType;

		// Token: 0x04001CEC RID: 7404
		private int length;
	}
}
