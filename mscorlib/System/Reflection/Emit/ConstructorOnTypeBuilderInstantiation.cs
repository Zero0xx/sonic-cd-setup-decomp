using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000852 RID: 2130
	internal sealed class ConstructorOnTypeBuilderInstantiation : ConstructorInfo
	{
		// Token: 0x06004E11 RID: 19985 RVA: 0x0010F5FB File Offset: 0x0010E5FB
		internal static ConstructorInfo GetConstructor(ConstructorInfo Constructor, TypeBuilderInstantiation type)
		{
			return new ConstructorOnTypeBuilderInstantiation(Constructor, type);
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x0010F604 File Offset: 0x0010E604
		internal ConstructorOnTypeBuilderInstantiation(ConstructorInfo constructor, TypeBuilderInstantiation type)
		{
			this.m_ctor = constructor;
			this.m_type = type;
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x0010F61A File Offset: 0x0010E61A
		internal override Type[] GetParameterTypes()
		{
			return this.m_ctor.GetParameterTypes();
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06004E14 RID: 19988 RVA: 0x0010F627 File Offset: 0x0010E627
		public override MemberTypes MemberType
		{
			get
			{
				return this.m_ctor.MemberType;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x0010F634 File Offset: 0x0010E634
		public override string Name
		{
			get
			{
				return this.m_ctor.Name;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06004E16 RID: 19990 RVA: 0x0010F641 File Offset: 0x0010E641
		public override Type DeclaringType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06004E17 RID: 19991 RVA: 0x0010F649 File Offset: 0x0010E649
		public override Type ReflectedType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x0010F651 File Offset: 0x0010E651
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_ctor.GetCustomAttributes(inherit);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0010F65F File Offset: 0x0010E65F
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_ctor.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x0010F66E File Offset: 0x0010E66E
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_ctor.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06004E1B RID: 19995 RVA: 0x0010F67D File Offset: 0x0010E67D
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06004E1C RID: 19996 RVA: 0x0010F684 File Offset: 0x0010E684
		public override Module Module
		{
			get
			{
				return this.m_ctor.Module;
			}
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x0010F691 File Offset: 0x0010E691
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x0010F699 File Offset: 0x0010E699
		public override ParameterInfo[] GetParameters()
		{
			return this.m_ctor.GetParameters();
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0010F6A6 File Offset: 0x0010E6A6
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_ctor.GetMethodImplementationFlags();
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06004E20 RID: 20000 RVA: 0x0010F6B3 File Offset: 0x0010E6B3
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.m_ctor.MethodHandle;
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06004E21 RID: 20001 RVA: 0x0010F6C0 File Offset: 0x0010E6C0
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_ctor.Attributes;
			}
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0010F6CD File Offset: 0x0010E6CD
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06004E23 RID: 20003 RVA: 0x0010F6D4 File Offset: 0x0010E6D4
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_ctor.CallingConvention;
			}
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x0010F6E1 File Offset: 0x0010E6E1
		public override Type[] GetGenericArguments()
		{
			return this.m_ctor.GetGenericArguments();
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06004E25 RID: 20005 RVA: 0x0010F6EE File Offset: 0x0010E6EE
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06004E26 RID: 20006 RVA: 0x0010F6F1 File Offset: 0x0010E6F1
		public override bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06004E27 RID: 20007 RVA: 0x0010F6F4 File Offset: 0x0010E6F4
		public override bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x0010F6F7 File Offset: 0x0010E6F7
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x04002852 RID: 10322
		internal ConstructorInfo m_ctor;

		// Token: 0x04002853 RID: 10323
		private TypeBuilderInstantiation m_type;
	}
}
