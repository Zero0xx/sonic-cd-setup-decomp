using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000851 RID: 2129
	internal sealed class MethodOnTypeBuilderInstantiation : MethodInfo
	{
		// Token: 0x06004DF4 RID: 19956 RVA: 0x0010F490 File Offset: 0x0010E490
		internal static MethodInfo GetMethod(MethodInfo method, TypeBuilderInstantiation type)
		{
			return new MethodOnTypeBuilderInstantiation(method, type);
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x0010F499 File Offset: 0x0010E499
		internal MethodOnTypeBuilderInstantiation(MethodInfo method, TypeBuilderInstantiation type)
		{
			this.m_method = method;
			this.m_type = type;
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0010F4AF File Offset: 0x0010E4AF
		internal override Type[] GetParameterTypes()
		{
			return this.m_method.GetParameterTypes();
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x0010F4BC File Offset: 0x0010E4BC
		public override MemberTypes MemberType
		{
			get
			{
				return this.m_method.MemberType;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x0010F4C9 File Offset: 0x0010E4C9
		public override string Name
		{
			get
			{
				return this.m_method.Name;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x0010F4D6 File Offset: 0x0010E4D6
		public override Type DeclaringType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x0010F4DE File Offset: 0x0010E4DE
		public override Type ReflectedType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x0010F4E6 File Offset: 0x0010E4E6
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_method.GetCustomAttributes(inherit);
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x0010F4F4 File Offset: 0x0010E4F4
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_method.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x0010F503 File Offset: 0x0010E503
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_method.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06004DFE RID: 19966 RVA: 0x0010F512 File Offset: 0x0010E512
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_method.MetadataTokenInternal;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06004DFF RID: 19967 RVA: 0x0010F51F File Offset: 0x0010E51F
		public override Module Module
		{
			get
			{
				return this.m_method.Module;
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x0010F52C File Offset: 0x0010E52C
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x0010F534 File Offset: 0x0010E534
		public override ParameterInfo[] GetParameters()
		{
			return this.m_method.GetParameters();
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x0010F541 File Offset: 0x0010E541
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_method.GetMethodImplementationFlags();
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06004E03 RID: 19971 RVA: 0x0010F54E File Offset: 0x0010E54E
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.m_method.MethodHandle;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x0010F55B File Offset: 0x0010E55B
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_method.Attributes;
			}
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x0010F568 File Offset: 0x0010E568
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06004E06 RID: 19974 RVA: 0x0010F56F File Offset: 0x0010E56F
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_method.CallingConvention;
			}
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x0010F57C File Offset: 0x0010E57C
		public override Type[] GetGenericArguments()
		{
			return this.m_method.GetGenericArguments();
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0010F589 File Offset: 0x0010E589
		public override MethodInfo GetGenericMethodDefinition()
		{
			return this.m_method;
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06004E09 RID: 19977 RVA: 0x0010F591 File Offset: 0x0010E591
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.m_method.IsGenericMethodDefinition;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06004E0A RID: 19978 RVA: 0x0010F59E File Offset: 0x0010E59E
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.m_method.ContainsGenericParameters;
			}
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x0010F5AB File Offset: 0x0010E5AB
		public override MethodInfo MakeGenericMethod(params Type[] typeArgs)
		{
			if (!this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericMethodDefinition"));
			}
			return MethodBuilderInstantiation.MakeGenericMethod(this, typeArgs);
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06004E0C RID: 19980 RVA: 0x0010F5CC File Offset: 0x0010E5CC
		public override bool IsGenericMethod
		{
			get
			{
				return this.m_method.IsGenericMethod;
			}
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x0010F5D9 File Offset: 0x0010E5D9
		internal override Type GetReturnType()
		{
			return this.m_method.GetReturnType();
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06004E0E RID: 19982 RVA: 0x0010F5E6 File Offset: 0x0010E5E6
		public override ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06004E0F RID: 19983 RVA: 0x0010F5ED File Offset: 0x0010E5ED
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x0010F5F4 File Offset: 0x0010E5F4
		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002850 RID: 10320
		internal MethodInfo m_method;

		// Token: 0x04002851 RID: 10321
		private TypeBuilderInstantiation m_type;
	}
}
