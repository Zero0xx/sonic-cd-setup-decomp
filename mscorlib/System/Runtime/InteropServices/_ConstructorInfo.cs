using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002FA RID: 762
	[ComVisible(true)]
	[Guid("E9A19478-9646-3679-9B10-8411AE1FD57D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(ConstructorInfo))]
	public interface _ConstructorInfo
	{
		// Token: 0x06001DA1 RID: 7585
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001DA2 RID: 7586
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001DA3 RID: 7587
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001DA4 RID: 7588
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001DA5 RID: 7589
		string ToString();

		// Token: 0x06001DA6 RID: 7590
		bool Equals(object other);

		// Token: 0x06001DA7 RID: 7591
		int GetHashCode();

		// Token: 0x06001DA8 RID: 7592
		Type GetType();

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001DA9 RID: 7593
		MemberTypes MemberType { get; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001DAA RID: 7594
		string Name { get; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001DAB RID: 7595
		Type DeclaringType { get; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001DAC RID: 7596
		Type ReflectedType { get; }

		// Token: 0x06001DAD RID: 7597
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001DAE RID: 7598
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001DAF RID: 7599
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001DB0 RID: 7600
		ParameterInfo[] GetParameters();

		// Token: 0x06001DB1 RID: 7601
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001DB2 RID: 7602
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001DB3 RID: 7603
		MethodAttributes Attributes { get; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001DB4 RID: 7604
		CallingConventions CallingConvention { get; }

		// Token: 0x06001DB5 RID: 7605
		object Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001DB6 RID: 7606
		bool IsPublic { get; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001DB7 RID: 7607
		bool IsPrivate { get; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001DB8 RID: 7608
		bool IsFamily { get; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001DB9 RID: 7609
		bool IsAssembly { get; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001DBA RID: 7610
		bool IsFamilyAndAssembly { get; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001DBB RID: 7611
		bool IsFamilyOrAssembly { get; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001DBC RID: 7612
		bool IsStatic { get; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001DBD RID: 7613
		bool IsFinal { get; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001DBE RID: 7614
		bool IsVirtual { get; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001DBF RID: 7615
		bool IsHideBySig { get; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001DC0 RID: 7616
		bool IsAbstract { get; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001DC1 RID: 7617
		bool IsSpecialName { get; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001DC2 RID: 7618
		bool IsConstructor { get; }

		// Token: 0x06001DC3 RID: 7619
		object Invoke_3(object obj, object[] parameters);

		// Token: 0x06001DC4 RID: 7620
		object Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x06001DC5 RID: 7621
		object Invoke_5(object[] parameters);
	}
}
