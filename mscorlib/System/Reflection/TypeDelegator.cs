using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000344 RID: 836
	[ComVisible(true)]
	[Serializable]
	public class TypeDelegator : Type
	{
		// Token: 0x06001FD9 RID: 8153 RVA: 0x0004FFC3 File Offset: 0x0004EFC3
		protected TypeDelegator()
		{
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0004FFCB File Offset: 0x0004EFCB
		public TypeDelegator(Type delegatingType)
		{
			if (delegatingType == null)
			{
				throw new ArgumentNullException("delegatingType");
			}
			this.typeImpl = delegatingType;
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x0004FFE8 File Offset: 0x0004EFE8
		public override Guid GUID
		{
			get
			{
				return this.typeImpl.GUID;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x0004FFF5 File Offset: 0x0004EFF5
		public override int MetadataToken
		{
			get
			{
				return this.typeImpl.MetadataToken;
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x00050004 File Offset: 0x0004F004
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.typeImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00050029 File Offset: 0x0004F029
		public override Module Module
		{
			get
			{
				return this.typeImpl.Module;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x00050036 File Offset: 0x0004F036
		public override Assembly Assembly
		{
			get
			{
				return this.typeImpl.Assembly;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x00050043 File Offset: 0x0004F043
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.typeImpl.TypeHandle;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x00050050 File Offset: 0x0004F050
		public override string Name
		{
			get
			{
				return this.typeImpl.Name;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x0005005D File Offset: 0x0004F05D
		public override string FullName
		{
			get
			{
				return this.typeImpl.FullName;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x0005006A File Offset: 0x0004F06A
		public override string Namespace
		{
			get
			{
				return this.typeImpl.Namespace;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001FE4 RID: 8164 RVA: 0x00050077 File Offset: 0x0004F077
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.typeImpl.AssemblyQualifiedName;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x00050084 File Offset: 0x0004F084
		public override Type BaseType
		{
			get
			{
				return this.typeImpl.BaseType;
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00050091 File Offset: 0x0004F091
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeImpl.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000500A5 File Offset: 0x0004F0A5
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetConstructors(bindingAttr);
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000500B3 File Offset: 0x0004F0B3
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				return this.typeImpl.GetMethod(name, bindingAttr);
			}
			return this.typeImpl.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000500DB File Offset: 0x0004F0DB
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMethods(bindingAttr);
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000500E9 File Offset: 0x0004F0E9
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetField(name, bindingAttr);
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000500F8 File Offset: 0x0004F0F8
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetFields(bindingAttr);
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x00050106 File Offset: 0x0004F106
		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this.typeImpl.GetInterface(name, ignoreCase);
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x00050115 File Offset: 0x0004F115
		public override Type[] GetInterfaces()
		{
			return this.typeImpl.GetInterfaces();
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x00050122 File Offset: 0x0004F122
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvent(name, bindingAttr);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00050131 File Offset: 0x0004F131
		public override EventInfo[] GetEvents()
		{
			return this.typeImpl.GetEvents();
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0005013E File Offset: 0x0004F13E
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (returnType == null && types == null)
			{
				return this.typeImpl.GetProperty(name, bindingAttr);
			}
			return this.typeImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0005016A File Offset: 0x0004F16A
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetProperties(bindingAttr);
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x00050178 File Offset: 0x0004F178
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvents(bindingAttr);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00050186 File Offset: 0x0004F186
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00050194 File Offset: 0x0004F194
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000501A3 File Offset: 0x0004F1A3
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000501B3 File Offset: 0x0004F1B3
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMembers(bindingAttr);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000501C1 File Offset: 0x0004F1C1
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.typeImpl.Attributes;
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000501CE File Offset: 0x0004F1CE
		protected override bool IsArrayImpl()
		{
			return this.typeImpl.IsArray;
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000501DB File Offset: 0x0004F1DB
		protected override bool IsPrimitiveImpl()
		{
			return this.typeImpl.IsPrimitive;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000501E8 File Offset: 0x0004F1E8
		protected override bool IsByRefImpl()
		{
			return this.typeImpl.IsByRef;
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000501F5 File Offset: 0x0004F1F5
		protected override bool IsPointerImpl()
		{
			return this.typeImpl.IsPointer;
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x00050202 File Offset: 0x0004F202
		protected override bool IsValueTypeImpl()
		{
			return this.typeImpl.IsValueType;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0005020F File Offset: 0x0004F20F
		protected override bool IsCOMObjectImpl()
		{
			return this.typeImpl.IsCOMObject;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x0005021C File Offset: 0x0004F21C
		public override Type GetElementType()
		{
			return this.typeImpl.GetElementType();
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00050229 File Offset: 0x0004F229
		protected override bool HasElementTypeImpl()
		{
			return this.typeImpl.HasElementType;
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x00050236 File Offset: 0x0004F236
		public override Type UnderlyingSystemType
		{
			get
			{
				return this.typeImpl.UnderlyingSystemType;
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x00050243 File Offset: 0x0004F243
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(inherit);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00050251 File Offset: 0x0004F251
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x00050260 File Offset: 0x0004F260
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.typeImpl.IsDefined(attributeType, inherit);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0005026F File Offset: 0x0004F26F
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			return this.typeImpl.GetInterfaceMap(interfaceType);
		}

		// Token: 0x04000DE2 RID: 3554
		protected Type typeImpl;
	}
}
