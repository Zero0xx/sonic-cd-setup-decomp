using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002FC RID: 764
	[Guid("F59ED4E4-E68F-3218-BD77-061AA82824BF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(PropertyInfo))]
	[ComVisible(true)]
	public interface _PropertyInfo
	{
		// Token: 0x06001DE9 RID: 7657
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001DEA RID: 7658
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001DEB RID: 7659
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001DEC RID: 7660
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001DED RID: 7661
		string ToString();

		// Token: 0x06001DEE RID: 7662
		bool Equals(object other);

		// Token: 0x06001DEF RID: 7663
		int GetHashCode();

		// Token: 0x06001DF0 RID: 7664
		Type GetType();

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001DF1 RID: 7665
		MemberTypes MemberType { get; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001DF2 RID: 7666
		string Name { get; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001DF3 RID: 7667
		Type DeclaringType { get; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001DF4 RID: 7668
		Type ReflectedType { get; }

		// Token: 0x06001DF5 RID: 7669
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001DF6 RID: 7670
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001DF7 RID: 7671
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001DF8 RID: 7672
		Type PropertyType { get; }

		// Token: 0x06001DF9 RID: 7673
		object GetValue(object obj, object[] index);

		// Token: 0x06001DFA RID: 7674
		object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x06001DFB RID: 7675
		void SetValue(object obj, object value, object[] index);

		// Token: 0x06001DFC RID: 7676
		void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x06001DFD RID: 7677
		MethodInfo[] GetAccessors(bool nonPublic);

		// Token: 0x06001DFE RID: 7678
		MethodInfo GetGetMethod(bool nonPublic);

		// Token: 0x06001DFF RID: 7679
		MethodInfo GetSetMethod(bool nonPublic);

		// Token: 0x06001E00 RID: 7680
		ParameterInfo[] GetIndexParameters();

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001E01 RID: 7681
		PropertyAttributes Attributes { get; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001E02 RID: 7682
		bool CanRead { get; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001E03 RID: 7683
		bool CanWrite { get; }

		// Token: 0x06001E04 RID: 7684
		MethodInfo[] GetAccessors();

		// Token: 0x06001E05 RID: 7685
		MethodInfo GetGetMethod();

		// Token: 0x06001E06 RID: 7686
		MethodInfo GetSetMethod();

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001E07 RID: 7687
		bool IsSpecialName { get; }
	}
}
