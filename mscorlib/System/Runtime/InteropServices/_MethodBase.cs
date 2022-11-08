using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002F8 RID: 760
	[ComVisible(true)]
	[Guid("6240837A-707F-3181-8E98-A36AE086766B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(MethodBase))]
	public interface _MethodBase
	{
		// Token: 0x06001D58 RID: 7512
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D59 RID: 7513
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D5A RID: 7514
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D5B RID: 7515
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D5C RID: 7516
		string ToString();

		// Token: 0x06001D5D RID: 7517
		bool Equals(object other);

		// Token: 0x06001D5E RID: 7518
		int GetHashCode();

		// Token: 0x06001D5F RID: 7519
		Type GetType();

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001D60 RID: 7520
		MemberTypes MemberType { get; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001D61 RID: 7521
		string Name { get; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001D62 RID: 7522
		Type DeclaringType { get; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001D63 RID: 7523
		Type ReflectedType { get; }

		// Token: 0x06001D64 RID: 7524
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001D65 RID: 7525
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001D66 RID: 7526
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001D67 RID: 7527
		ParameterInfo[] GetParameters();

		// Token: 0x06001D68 RID: 7528
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001D69 RID: 7529
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001D6A RID: 7530
		MethodAttributes Attributes { get; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001D6B RID: 7531
		CallingConventions CallingConvention { get; }

		// Token: 0x06001D6C RID: 7532
		object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001D6D RID: 7533
		bool IsPublic { get; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001D6E RID: 7534
		bool IsPrivate { get; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001D6F RID: 7535
		bool IsFamily { get; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001D70 RID: 7536
		bool IsAssembly { get; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001D71 RID: 7537
		bool IsFamilyAndAssembly { get; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001D72 RID: 7538
		bool IsFamilyOrAssembly { get; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001D73 RID: 7539
		bool IsStatic { get; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001D74 RID: 7540
		bool IsFinal { get; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001D75 RID: 7541
		bool IsVirtual { get; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001D76 RID: 7542
		bool IsHideBySig { get; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001D77 RID: 7543
		bool IsAbstract { get; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001D78 RID: 7544
		bool IsSpecialName { get; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001D79 RID: 7545
		bool IsConstructor { get; }

		// Token: 0x06001D7A RID: 7546
		object Invoke(object obj, object[] parameters);
	}
}
