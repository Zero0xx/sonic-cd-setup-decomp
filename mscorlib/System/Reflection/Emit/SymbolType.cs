using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000835 RID: 2101
	internal sealed class SymbolType : Type
	{
		// Token: 0x06004AFD RID: 19197 RVA: 0x00104358 File Offset: 0x00103358
		internal static Type FormCompoundType(char[] bFormat, Type baseType, int curIndex)
		{
			if (bFormat == null || curIndex == bFormat.Length)
			{
				return baseType;
			}
			if (bFormat[curIndex] == '&')
			{
				SymbolType symbolType = new SymbolType(TypeKind.IsByRef);
				symbolType.SetFormat(bFormat, curIndex, 1);
				curIndex++;
				if (curIndex != bFormat.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
				}
				symbolType.SetElementType(baseType);
				return symbolType;
			}
			else
			{
				if (bFormat[curIndex] == '[')
				{
					SymbolType symbolType = new SymbolType(TypeKind.IsArray);
					int num = curIndex;
					curIndex++;
					int num2 = 0;
					int num3 = -1;
					while (bFormat[curIndex] != ']')
					{
						if (bFormat[curIndex] == '*')
						{
							symbolType.m_isSzArray = false;
							curIndex++;
						}
						if ((bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9') || bFormat[curIndex] == '-')
						{
							bool flag = false;
							if (bFormat[curIndex] == '-')
							{
								flag = true;
								curIndex++;
							}
							while (bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9')
							{
								num2 *= 10;
								num2 += (int)(bFormat[curIndex] - '0');
								curIndex++;
							}
							if (flag)
							{
								num2 = -num2;
							}
							num3 = num2 - 1;
						}
						if (bFormat[curIndex] == '.')
						{
							curIndex++;
							if (bFormat[curIndex] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
							}
							curIndex++;
							if ((bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9') || bFormat[curIndex] == '-')
							{
								bool flag2 = false;
								num3 = 0;
								if (bFormat[curIndex] == '-')
								{
									flag2 = true;
									curIndex++;
								}
								while (bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9')
								{
									num3 *= 10;
									num3 += (int)(bFormat[curIndex] - '0');
									curIndex++;
								}
								if (flag2)
								{
									num3 = -num3;
								}
								if (num3 < num2)
								{
									throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
								}
							}
						}
						if (bFormat[curIndex] == ',')
						{
							curIndex++;
							symbolType.SetBounds(num2, num3);
							num2 = 0;
							num3 = -1;
						}
						else if (bFormat[curIndex] != ']')
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
						}
					}
					symbolType.SetBounds(num2, num3);
					curIndex++;
					symbolType.SetFormat(bFormat, num, curIndex - num);
					symbolType.SetElementType(baseType);
					return SymbolType.FormCompoundType(bFormat, symbolType, curIndex);
				}
				if (bFormat[curIndex] == '*')
				{
					SymbolType symbolType = new SymbolType(TypeKind.IsPointer);
					symbolType.SetFormat(bFormat, curIndex, 1);
					curIndex++;
					symbolType.SetElementType(baseType);
					return SymbolType.FormCompoundType(bFormat, symbolType, curIndex);
				}
				return null;
			}
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x00104556 File Offset: 0x00103556
		internal SymbolType(TypeKind typeKind)
		{
			this.m_typeKind = typeKind;
			this.m_iaLowerBound = new int[4];
			this.m_iaUpperBound = new int[4];
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x00104584 File Offset: 0x00103584
		internal void SetElementType(Type baseType)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			this.m_baseType = baseType;
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x0010459C File Offset: 0x0010359C
		internal void SetBounds(int lower, int upper)
		{
			if (lower != 0 || upper != -1)
			{
				this.m_isSzArray = false;
			}
			if (this.m_iaLowerBound.Length <= this.m_cRank)
			{
				int[] array = new int[this.m_cRank * 2];
				Array.Copy(this.m_iaLowerBound, array, this.m_cRank);
				this.m_iaLowerBound = array;
				Array.Copy(this.m_iaUpperBound, array, this.m_cRank);
				this.m_iaUpperBound = array;
			}
			this.m_iaLowerBound[this.m_cRank] = lower;
			this.m_iaUpperBound[this.m_cRank] = upper;
			this.m_cRank++;
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x00104634 File Offset: 0x00103634
		internal void SetFormat(char[] bFormat, int curIndex, int length)
		{
			char[] array = new char[length];
			Array.Copy(bFormat, curIndex, array, 0, length);
			this.m_bFormat = array;
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004B02 RID: 19202 RVA: 0x00104659 File Offset: 0x00103659
		internal override bool IsSzArray
		{
			get
			{
				return this.m_cRank <= 1 && this.m_isSzArray;
			}
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x0010466C File Offset: 0x0010366C
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + "*").ToCharArray(), this.m_baseType, 0);
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x00104694 File Offset: 0x00103694
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + "&").ToCharArray(), this.m_baseType, 0);
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x001046BC File Offset: 0x001036BC
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + "[]").ToCharArray(), this.m_baseType, 0);
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x001046E4 File Offset: 0x001036E4
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			string text = "";
			if (rank == 1)
			{
				text = "*";
			}
			else
			{
				for (int i = 1; i < rank; i++)
				{
					text += ",";
				}
			}
			string str = string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[]
			{
				text
			});
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + str).ToCharArray(), this.m_baseType, 0) as SymbolType;
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x0010476D File Offset: 0x0010376D
		public override int GetArrayRank()
		{
			if (!base.IsArray)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
			}
			return this.m_cRank;
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x0010478D File Offset: 0x0010378D
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x0010479E File Offset: 0x0010379E
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
			}
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x001047AF File Offset: 0x001037AF
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x001047C0 File Offset: 0x001037C0
		public override Module Module
		{
			get
			{
				Type baseType = this.m_baseType;
				while (baseType is SymbolType)
				{
					baseType = ((SymbolType)baseType).m_baseType;
				}
				return baseType.Module;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x001047F0 File Offset: 0x001037F0
		public override Assembly Assembly
		{
			get
			{
				Type baseType = this.m_baseType;
				while (baseType is SymbolType)
				{
					baseType = ((SymbolType)baseType).m_baseType;
				}
				return baseType.Assembly;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004B0D RID: 19213 RVA: 0x00104820 File Offset: 0x00103820
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004B0E RID: 19214 RVA: 0x00104834 File Offset: 0x00103834
		public override string Name
		{
			get
			{
				string str = new string(this.m_bFormat);
				Type baseType = this.m_baseType;
				while (baseType is SymbolType)
				{
					str = new string(((SymbolType)baseType).m_bFormat) + str;
					baseType = ((SymbolType)baseType).m_baseType;
				}
				return baseType.Name + str;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004B0F RID: 19215 RVA: 0x0010488D File Offset: 0x0010388D
		public override string FullName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004B10 RID: 19216 RVA: 0x00104896 File Offset: 0x00103896
		public override string AssemblyQualifiedName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);
			}
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x0010489F File Offset: 0x0010389F
		public override string ToString()
		{
			return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004B12 RID: 19218 RVA: 0x001048A8 File Offset: 0x001038A8
		public override string Namespace
		{
			get
			{
				return this.m_baseType.Namespace;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004B13 RID: 19219 RVA: 0x001048B5 File Offset: 0x001038B5
		public override Type BaseType
		{
			get
			{
				return typeof(Array);
			}
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x001048C1 File Offset: 0x001038C1
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x001048D2 File Offset: 0x001038D2
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x001048E3 File Offset: 0x001038E3
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x001048F4 File Offset: 0x001038F4
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x00104905 File Offset: 0x00103905
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x00104916 File Offset: 0x00103916
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x00104927 File Offset: 0x00103927
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x00104938 File Offset: 0x00103938
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x00104949 File Offset: 0x00103949
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x0010495A File Offset: 0x0010395A
		public override EventInfo[] GetEvents()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x0010496B File Offset: 0x0010396B
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x0010497C File Offset: 0x0010397C
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x0010498D File Offset: 0x0010398D
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x0010499E File Offset: 0x0010399E
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x001049AF File Offset: 0x001039AF
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x001049C0 File Offset: 0x001039C0
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x001049D1 File Offset: 0x001039D1
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x001049E2 File Offset: 0x001039E2
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x001049F4 File Offset: 0x001039F4
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			Type baseType = this.m_baseType;
			while (baseType is SymbolType)
			{
				baseType = ((SymbolType)baseType).m_baseType;
			}
			return baseType.Attributes;
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x00104A24 File Offset: 0x00103A24
		protected override bool IsArrayImpl()
		{
			return this.m_typeKind == TypeKind.IsArray;
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x00104A2F File Offset: 0x00103A2F
		protected override bool IsPointerImpl()
		{
			return this.m_typeKind == TypeKind.IsPointer;
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x00104A3A File Offset: 0x00103A3A
		protected override bool IsByRefImpl()
		{
			return this.m_typeKind == TypeKind.IsByRef;
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x00104A45 File Offset: 0x00103A45
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x00104A48 File Offset: 0x00103A48
		protected override bool IsValueTypeImpl()
		{
			return false;
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x00104A4B File Offset: 0x00103A4B
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x00104A4E File Offset: 0x00103A4E
		public override Type GetElementType()
		{
			return this.m_baseType;
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x00104A56 File Offset: 0x00103A56
		protected override bool HasElementTypeImpl()
		{
			return this.m_baseType != null;
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004B2F RID: 19247 RVA: 0x00104A64 File Offset: 0x00103A64
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x00104A67 File Offset: 0x00103A67
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x00104A78 File Offset: 0x00103A78
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x00104A89 File Offset: 0x00103A89
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x0400265C RID: 9820
		internal TypeKind m_typeKind;

		// Token: 0x0400265D RID: 9821
		internal Type m_baseType;

		// Token: 0x0400265E RID: 9822
		internal int m_cRank;

		// Token: 0x0400265F RID: 9823
		internal int[] m_iaLowerBound;

		// Token: 0x04002660 RID: 9824
		internal int[] m_iaUpperBound;

		// Token: 0x04002661 RID: 9825
		private char[] m_bFormat;

		// Token: 0x04002662 RID: 9826
		private bool m_isSzArray = true;
	}
}
