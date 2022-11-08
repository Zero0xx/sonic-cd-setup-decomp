using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002F9 RID: 761
	[Guid("FFCC1B5D-ECB8-38DD-9B01-3DC8ABC2AA5F")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(MethodInfo))]
	public interface _MethodInfo
	{
		// Token: 0x06001D7B RID: 7547
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D7C RID: 7548
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D7D RID: 7549
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D7E RID: 7550
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D7F RID: 7551
		string ToString();

		// Token: 0x06001D80 RID: 7552
		bool Equals(object other);

		// Token: 0x06001D81 RID: 7553
		int GetHashCode();

		// Token: 0x06001D82 RID: 7554
		Type GetType();

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001D83 RID: 7555
		MemberTypes MemberType { get; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001D84 RID: 7556
		string Name { get; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001D85 RID: 7557
		Type DeclaringType { get; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001D86 RID: 7558
		Type ReflectedType { get; }

		// Token: 0x06001D87 RID: 7559
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001D88 RID: 7560
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001D89 RID: 7561
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001D8A RID: 7562
		ParameterInfo[] GetParameters();

		// Token: 0x06001D8B RID: 7563
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001D8C RID: 7564
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001D8D RID: 7565
		MethodAttributes Attributes { get; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001D8E RID: 7566
		CallingConventions CallingConvention { get; }

		// Token: 0x06001D8F RID: 7567
		object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001D90 RID: 7568
		bool IsPublic { get; }

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001D91 RID: 7569
		bool IsPrivate { get; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001D92 RID: 7570
		bool IsFamily { get; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001D93 RID: 7571
		bool IsAssembly { get; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001D94 RID: 7572
		bool IsFamilyAndAssembly { get; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001D95 RID: 7573
		bool IsFamilyOrAssembly { get; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001D96 RID: 7574
		bool IsStatic { get; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001D97 RID: 7575
		bool IsFinal { get; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001D98 RID: 7576
		bool IsVirtual { get; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001D99 RID: 7577
		bool IsHideBySig { get; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001D9A RID: 7578
		bool IsAbstract { get; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001D9B RID: 7579
		bool IsSpecialName { get; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001D9C RID: 7580
		bool IsConstructor { get; }

		// Token: 0x06001D9D RID: 7581
		object Invoke(object obj, object[] parameters);

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001D9E RID: 7582
		Type ReturnType { get; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001D9F RID: 7583
		ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		// Token: 0x06001DA0 RID: 7584
		MethodInfo GetBaseDefinition();
	}
}
