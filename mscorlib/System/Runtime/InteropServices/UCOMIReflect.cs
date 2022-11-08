using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000542 RID: 1346
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IReflect instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("AFBF15E5-C37C-11d2-B88E-00A0C9B471B8")]
	internal interface UCOMIReflect
	{
		// Token: 0x0600335C RID: 13148
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600335D RID: 13149
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x0600335E RID: 13150
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x0600335F RID: 13151
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06003360 RID: 13152
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06003361 RID: 13153
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x06003362 RID: 13154
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06003363 RID: 13155
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06003364 RID: 13156
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x06003365 RID: 13157
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06003366 RID: 13158
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003367 RID: 13159
		Type UnderlyingSystemType { get; }
	}
}
