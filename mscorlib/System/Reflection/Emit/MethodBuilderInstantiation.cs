using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000833 RID: 2099
	internal sealed class MethodBuilderInstantiation : MethodInfo
	{
		// Token: 0x06004AE0 RID: 19168 RVA: 0x001041C7 File Offset: 0x001031C7
		internal static MethodInfo MakeGenericMethod(MethodInfo method, Type[] inst)
		{
			if (!method.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException();
			}
			return new MethodBuilderInstantiation(method, inst);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x001041DE File Offset: 0x001031DE
		internal MethodBuilderInstantiation(MethodInfo method, Type[] inst)
		{
			this.m_method = method;
			this.m_inst = inst;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x001041F4 File Offset: 0x001031F4
		internal override Type[] GetParameterTypes()
		{
			return this.m_method.GetParameterTypes();
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06004AE3 RID: 19171 RVA: 0x00104201 File Offset: 0x00103201
		public override MemberTypes MemberType
		{
			get
			{
				return this.m_method.MemberType;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x0010420E File Offset: 0x0010320E
		public override string Name
		{
			get
			{
				return this.m_method.Name;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0010421B File Offset: 0x0010321B
		public override Type DeclaringType
		{
			get
			{
				return this.m_method.DeclaringType;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x00104228 File Offset: 0x00103228
		public override Type ReflectedType
		{
			get
			{
				return this.m_method.ReflectedType;
			}
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00104235 File Offset: 0x00103235
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_method.GetCustomAttributes(inherit);
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x00104243 File Offset: 0x00103243
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_method.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x00104252 File Offset: 0x00103252
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_method.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x00104261 File Offset: 0x00103261
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004AEB RID: 19179 RVA: 0x00104268 File Offset: 0x00103268
		public override Module Module
		{
			get
			{
				return this.m_method.Module;
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00104275 File Offset: 0x00103275
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x0010427D File Offset: 0x0010327D
		public override ParameterInfo[] GetParameters()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x00104284 File Offset: 0x00103284
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_method.GetMethodImplementationFlags();
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004AEF RID: 19183 RVA: 0x00104291 File Offset: 0x00103291
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x001042A2 File Offset: 0x001032A2
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_method.Attributes;
			}
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x001042AF File Offset: 0x001032AF
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x001042B6 File Offset: 0x001032B6
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_method.CallingConvention;
			}
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x001042C3 File Offset: 0x001032C3
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x001042CB File Offset: 0x001032CB
		public override MethodInfo GetGenericMethodDefinition()
		{
			return this.m_method;
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004AF5 RID: 19189 RVA: 0x001042D3 File Offset: 0x001032D3
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004AF6 RID: 19190 RVA: 0x001042D8 File Offset: 0x001032D8
		public override bool ContainsGenericParameters
		{
			get
			{
				for (int i = 0; i < this.m_inst.Length; i++)
				{
					if (this.m_inst[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return this.DeclaringType != null && this.DeclaringType.ContainsGenericParameters;
			}
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00104321 File Offset: 0x00103321
		public override MethodInfo MakeGenericMethod(params Type[] arguments)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericMethodDefinition"));
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x00104332 File Offset: 0x00103332
		public override bool IsGenericMethod
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x00104335 File Offset: 0x00103335
		internal override Type GetReturnType()
		{
			return this.m_method.GetReturnType();
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x00104342 File Offset: 0x00103342
		public override ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x00104349 File Offset: 0x00103349
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x00104350 File Offset: 0x00103350
		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002656 RID: 9814
		internal MethodInfo m_method;

		// Token: 0x04002657 RID: 9815
		private Type[] m_inst;
	}
}
