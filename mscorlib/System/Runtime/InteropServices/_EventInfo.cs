using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002FD RID: 765
	[CLSCompliant(false)]
	[Guid("9DE59C64-D889-35A1-B897-587D74469E5B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(EventInfo))]
	[ComVisible(true)]
	public interface _EventInfo
	{
		// Token: 0x06001E08 RID: 7688
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001E09 RID: 7689
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001E0A RID: 7690
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001E0B RID: 7691
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001E0C RID: 7692
		string ToString();

		// Token: 0x06001E0D RID: 7693
		bool Equals(object other);

		// Token: 0x06001E0E RID: 7694
		int GetHashCode();

		// Token: 0x06001E0F RID: 7695
		Type GetType();

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001E10 RID: 7696
		MemberTypes MemberType { get; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001E11 RID: 7697
		string Name { get; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001E12 RID: 7698
		Type DeclaringType { get; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001E13 RID: 7699
		Type ReflectedType { get; }

		// Token: 0x06001E14 RID: 7700
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001E15 RID: 7701
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001E16 RID: 7702
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001E17 RID: 7703
		MethodInfo GetAddMethod(bool nonPublic);

		// Token: 0x06001E18 RID: 7704
		MethodInfo GetRemoveMethod(bool nonPublic);

		// Token: 0x06001E19 RID: 7705
		MethodInfo GetRaiseMethod(bool nonPublic);

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001E1A RID: 7706
		EventAttributes Attributes { get; }

		// Token: 0x06001E1B RID: 7707
		MethodInfo GetAddMethod();

		// Token: 0x06001E1C RID: 7708
		MethodInfo GetRemoveMethod();

		// Token: 0x06001E1D RID: 7709
		MethodInfo GetRaiseMethod();

		// Token: 0x06001E1E RID: 7710
		void AddEventHandler(object target, Delegate handler);

		// Token: 0x06001E1F RID: 7711
		void RemoveEventHandler(object target, Delegate handler);

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001E20 RID: 7712
		Type EventHandlerType { get; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001E21 RID: 7713
		bool IsSpecialName { get; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001E22 RID: 7714
		bool IsMulticast { get; }
	}
}
