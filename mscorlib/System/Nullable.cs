using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000284 RID: 644
	[TypeDependency("System.Collections.Generic.NullableComparer`1")]
	[TypeDependency("System.Collections.Generic.NullableEqualityComparer`1")]
	[Serializable]
	public struct Nullable<T> where T : struct
	{
		// Token: 0x06001966 RID: 6502 RVA: 0x000421CC File Offset: 0x000411CC
		public Nullable(T value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x000421DC File Offset: 0x000411DC
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x000421E4 File Offset: 0x000411E4
		public T Value
		{
			get
			{
				if (this == null)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NoValue);
				}
				return this.value;
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000421FB File Offset: 0x000411FB
		public T GetValueOrDefault()
		{
			return this.value;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00042203 File Offset: 0x00041203
		public T GetValueOrDefault(T defaultValue)
		{
			if (this == null)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00042215 File Offset: 0x00041215
		public override bool Equals(object other)
		{
			if (this == null)
			{
				return other == null;
			}
			return other != null && this.value.Equals(other);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0004223B File Offset: 0x0004123B
		public override int GetHashCode()
		{
			if (this == null)
			{
				return 0;
			}
			return this.value.GetHashCode();
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00042258 File Offset: 0x00041258
		public override string ToString()
		{
			if (this == null)
			{
				return "";
			}
			return this.value.ToString();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00042279 File Offset: 0x00041279
		public static implicit operator T?(T value)
		{
			return new T?(value);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00042281 File Offset: 0x00041281
		public static explicit operator T(T? value)
		{
			return value.Value;
		}

		// Token: 0x04000A05 RID: 2565
		private bool hasValue;

		// Token: 0x04000A06 RID: 2566
		internal T value;
	}
}
