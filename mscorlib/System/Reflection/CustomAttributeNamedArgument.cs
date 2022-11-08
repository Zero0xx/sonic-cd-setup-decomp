using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000301 RID: 769
	[ComVisible(true)]
	[Serializable]
	public struct CustomAttributeNamedArgument
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x0004B3DF File Offset: 0x0004A3DF
		public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0004B3F4 File Offset: 0x0004A3F4
		public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0004B40C File Offset: 0x0004A40C
		internal CustomAttributeNamedArgument(MemberInfo memberInfo, object value)
		{
			this.m_memberInfo = memberInfo;
			this.m_value = new CustomAttributeTypedArgument(value);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0004B421 File Offset: 0x0004A421
		internal CustomAttributeNamedArgument(MemberInfo memberInfo, CustomAttributeTypedArgument value)
		{
			this.m_memberInfo = memberInfo;
			this.m_value = value;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0004B434 File Offset: 0x0004A434
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} = {1}", new object[]
			{
				this.MemberInfo.Name,
				this.TypedValue.ToString(this.ArgumentType != typeof(object))
			});
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0004B48C File Offset: 0x0004A48C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0004B49E File Offset: 0x0004A49E
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x0004B4AE File Offset: 0x0004A4AE
		internal Type ArgumentType
		{
			get
			{
				if (!(this.m_memberInfo is FieldInfo))
				{
					return ((PropertyInfo)this.m_memberInfo).PropertyType;
				}
				return ((FieldInfo)this.m_memberInfo).FieldType;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x0004B4DE File Offset: 0x0004A4DE
		public MemberInfo MemberInfo
		{
			get
			{
				return this.m_memberInfo;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x0004B4E6 File Offset: 0x0004A4E6
		public CustomAttributeTypedArgument TypedValue
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x04000B17 RID: 2839
		private MemberInfo m_memberInfo;

		// Token: 0x04000B18 RID: 2840
		private CustomAttributeTypedArgument m_value;
	}
}
