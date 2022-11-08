using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000575 RID: 1397
	[Guid("AFBF15E5-C37C-11d2-B88E-00A0C9B471B8")]
	internal interface IReflect
	{
		// Token: 0x060033E4 RID: 13284
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x060033E5 RID: 13285
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x060033E6 RID: 13286
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x060033E7 RID: 13287
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x060033E8 RID: 13288
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x060033E9 RID: 13289
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x060033EA RID: 13290
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x060033EB RID: 13291
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x060033EC RID: 13292
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x060033ED RID: 13293
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x060033EE RID: 13294
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060033EF RID: 13295
		Type UnderlyingSystemType { get; }
	}
}
