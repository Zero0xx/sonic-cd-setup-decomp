using System;

namespace System.ComponentModel
{
	// Token: 0x020000F2 RID: 242
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ImmutableObjectAttribute : Attribute
	{
		// Token: 0x060007F5 RID: 2037 RVA: 0x0001BEF2 File Offset: 0x0001AEF2
		public ImmutableObjectAttribute(bool immutable)
		{
			this.immutable = immutable;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001BF08 File Offset: 0x0001AF08
		public bool Immutable
		{
			get
			{
				return this.immutable;
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001BF10 File Offset: 0x0001AF10
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ImmutableObjectAttribute immutableObjectAttribute = obj as ImmutableObjectAttribute;
			return immutableObjectAttribute != null && immutableObjectAttribute.Immutable == this.immutable;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001BF3D File Offset: 0x0001AF3D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001BF45 File Offset: 0x0001AF45
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ImmutableObjectAttribute.Default);
		}

		// Token: 0x0400097C RID: 2428
		public static readonly ImmutableObjectAttribute Yes = new ImmutableObjectAttribute(true);

		// Token: 0x0400097D RID: 2429
		public static readonly ImmutableObjectAttribute No = new ImmutableObjectAttribute(false);

		// Token: 0x0400097E RID: 2430
		public static readonly ImmutableObjectAttribute Default = ImmutableObjectAttribute.No;

		// Token: 0x0400097F RID: 2431
		private bool immutable = true;
	}
}
