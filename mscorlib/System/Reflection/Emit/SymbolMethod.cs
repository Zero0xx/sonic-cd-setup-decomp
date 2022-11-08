using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000836 RID: 2102
	internal sealed class SymbolMethod : MethodInfo
	{
		// Token: 0x06004B33 RID: 19251 RVA: 0x00104A9C File Offset: 0x00103A9C
		internal SymbolMethod(ModuleBuilder mod, MethodToken token, Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			this.m_mdMethod = token;
			this.m_tkMethod = token.Token;
			this.m_returnType = returnType;
			if (parameterTypes != null)
			{
				this.m_parameterTypes = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.m_parameterTypes, parameterTypes.Length);
			}
			else
			{
				this.m_parameterTypes = new Type[0];
			}
			this.m_module = mod;
			this.m_containingType = arrayClass;
			this.m_name = methodName;
			this.m_callingConvention = callingConvention;
			this.m_signature = SignatureHelper.GetMethodSigHelper(mod, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x00104B31 File Offset: 0x00103B31
		internal override Type[] GetParameterTypes()
		{
			return this.m_parameterTypes;
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x00104B39 File Offset: 0x00103B39
		internal MethodToken GetToken(ModuleBuilder mod)
		{
			return mod.GetArrayMethodToken(this.m_containingType, this.m_name, this.m_callingConvention, this.m_returnType, this.m_parameterTypes);
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004B36 RID: 19254 RVA: 0x00104B5F File Offset: 0x00103B5F
		public override Module Module
		{
			get
			{
				return this.m_module;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x00104B67 File Offset: 0x00103B67
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_tkMethod;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x00104B6F File Offset: 0x00103B6F
		public override Type ReflectedType
		{
			get
			{
				return this.m_containingType;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x00104B77 File Offset: 0x00103B77
		public override string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004B3A RID: 19258 RVA: 0x00104B7F File Offset: 0x00103B7F
		public override Type DeclaringType
		{
			get
			{
				return this.m_containingType;
			}
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x00104B87 File Offset: 0x00103B87
		public override ParameterInfo[] GetParameters()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x00104B98 File Offset: 0x00103B98
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x00104BA9 File Offset: 0x00103BA9
		public override MethodAttributes Attributes
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004B3E RID: 19262 RVA: 0x00104BBA File Offset: 0x00103BBA
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_callingConvention;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x00104BC2 File Offset: 0x00103BC2
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
			}
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x00104BD3 File Offset: 0x00103BD3
		internal override Type GetReturnType()
		{
			return this.m_returnType;
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x00104BDB File Offset: 0x00103BDB
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x00104BDE File Offset: 0x00103BDE
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x00104BEF File Offset: 0x00103BEF
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x00104BF2 File Offset: 0x00103BF2
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x00104C03 File Offset: 0x00103C03
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x00104C14 File Offset: 0x00103C14
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x00104C25 File Offset: 0x00103C25
		public Module GetModule()
		{
			return this.m_module;
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x00104C2D File Offset: 0x00103C2D
		public MethodToken GetToken()
		{
			return this.m_mdMethod;
		}

		// Token: 0x04002663 RID: 9827
		private ModuleBuilder m_module;

		// Token: 0x04002664 RID: 9828
		private Type m_containingType;

		// Token: 0x04002665 RID: 9829
		private string m_name;

		// Token: 0x04002666 RID: 9830
		private CallingConventions m_callingConvention;

		// Token: 0x04002667 RID: 9831
		private Type m_returnType;

		// Token: 0x04002668 RID: 9832
		private MethodToken m_mdMethod;

		// Token: 0x04002669 RID: 9833
		private int m_tkMethod;

		// Token: 0x0400266A RID: 9834
		private Type[] m_parameterTypes;

		// Token: 0x0400266B RID: 9835
		private SignatureHelper m_signature;
	}
}
