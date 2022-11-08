using System;
using System.Collections;

namespace System.Reflection
{
	// Token: 0x02000353 RID: 851
	internal static class Associates
	{
		// Token: 0x0600213F RID: 8511 RVA: 0x00052BB0 File Offset: 0x00051BB0
		internal static bool IncludeAccessor(MethodInfo associate, bool nonPublic)
		{
			return associate != null && (nonPublic || associate.IsPublic);
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x00052BC8 File Offset: 0x00051BC8
		internal static RuntimeMethodInfo AssignAssociates(int tkMethod, RuntimeTypeHandle declaredTypeHandle, RuntimeTypeHandle reflectedTypeHandle)
		{
			if (MetadataToken.IsNullToken(tkMethod))
			{
				return null;
			}
			bool flag = !declaredTypeHandle.Equals(reflectedTypeHandle);
			RuntimeMethodHandle methodHandle = declaredTypeHandle.GetModuleHandle().ResolveMethodHandle(tkMethod, declaredTypeHandle.GetInstantiation(), new RuntimeTypeHandle[0]);
			MethodAttributes attributes = methodHandle.GetAttributes();
			bool flag2 = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
			bool flag3 = (attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
			if (flag)
			{
				if (flag2)
				{
					return null;
				}
				if (flag3)
				{
					bool flag4 = (declaredTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic;
					if (flag4)
					{
						int slot = methodHandle.GetSlot();
						methodHandle = reflectedTypeHandle.GetMethodAt(slot);
					}
				}
			}
			MethodAttributes methodAttributes = attributes & MethodAttributes.MemberAccessMask;
			RuntimeMethodInfo runtimeMethodInfo = RuntimeType.GetMethodBase(reflectedTypeHandle, methodHandle) as RuntimeMethodInfo;
			if (runtimeMethodInfo == null)
			{
				runtimeMethodInfo = (reflectedTypeHandle.GetRuntimeType().Module.ResolveMethod(tkMethod, null, null) as RuntimeMethodInfo);
			}
			return runtimeMethodInfo;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x00052C8C File Offset: 0x00051C8C
		internal unsafe static void AssignAssociates(AssociateRecord* associates, int cAssociates, RuntimeTypeHandle declaringTypeHandle, RuntimeTypeHandle reflectedTypeHandle, out RuntimeMethodInfo addOn, out RuntimeMethodInfo removeOn, out RuntimeMethodInfo fireOn, out RuntimeMethodInfo getter, out RuntimeMethodInfo setter, out MethodInfo[] other, out bool composedOfAllPrivateMethods, out BindingFlags bindingFlags)
		{
			RuntimeMethodInfo runtimeMethodInfo;
			setter = (runtimeMethodInfo = null);
			RuntimeMethodInfo runtimeMethodInfo2;
			getter = (runtimeMethodInfo2 = runtimeMethodInfo);
			RuntimeMethodInfo runtimeMethodInfo3;
			fireOn = (runtimeMethodInfo3 = runtimeMethodInfo2);
			RuntimeMethodInfo runtimeMethodInfo4;
			removeOn = (runtimeMethodInfo4 = runtimeMethodInfo3);
			addOn = runtimeMethodInfo4;
			other = null;
			Associates.Attributes attributes = Associates.Attributes.ComposedOfAllVirtualMethods | Associates.Attributes.ComposedOfAllPrivateMethods | Associates.Attributes.ComposedOfNoPublicMembers | Associates.Attributes.ComposedOfNoStaticMembers;
			while (reflectedTypeHandle.IsGenericVariable())
			{
				reflectedTypeHandle = reflectedTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
			}
			bool isInherited = !declaringTypeHandle.Equals(reflectedTypeHandle);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < cAssociates; i++)
			{
				RuntimeMethodInfo runtimeMethodInfo5 = Associates.AssignAssociates(associates[i].MethodDefToken, declaringTypeHandle, reflectedTypeHandle);
				if (runtimeMethodInfo5 != null)
				{
					MethodAttributes attributes2 = runtimeMethodInfo5.Attributes;
					bool flag = (attributes2 & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
					bool flag2 = (attributes2 & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
					MethodAttributes methodAttributes = attributes2 & MethodAttributes.MemberAccessMask;
					bool flag3 = methodAttributes == MethodAttributes.Public;
					bool flag4 = (attributes2 & MethodAttributes.Static) != MethodAttributes.PrivateScope;
					if (flag3)
					{
						attributes &= ~Associates.Attributes.ComposedOfNoPublicMembers;
						attributes &= ~Associates.Attributes.ComposedOfAllPrivateMethods;
					}
					else if (!flag)
					{
						attributes &= ~Associates.Attributes.ComposedOfAllPrivateMethods;
					}
					if (flag4)
					{
						attributes &= ~Associates.Attributes.ComposedOfNoStaticMembers;
					}
					if (!flag2)
					{
						attributes &= ~Associates.Attributes.ComposedOfAllVirtualMethods;
					}
					if (associates[i].Semantics == MethodSemanticsAttributes.Setter)
					{
						setter = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.Getter)
					{
						getter = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.Fire)
					{
						fireOn = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.AddOn)
					{
						addOn = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.RemoveOn)
					{
						removeOn = runtimeMethodInfo5;
					}
					else
					{
						arrayList.Add(runtimeMethodInfo5);
					}
				}
			}
			bool isPublic = (attributes & Associates.Attributes.ComposedOfNoPublicMembers) == (Associates.Attributes)0;
			bool isStatic = (attributes & Associates.Attributes.ComposedOfNoStaticMembers) == (Associates.Attributes)0;
			bindingFlags = RuntimeType.FilterPreCalculate(isPublic, isInherited, isStatic);
			composedOfAllPrivateMethods = ((attributes & Associates.Attributes.ComposedOfAllPrivateMethods) != (Associates.Attributes)0);
			other = (MethodInfo[])arrayList.ToArray(typeof(MethodInfo));
		}

		// Token: 0x02000354 RID: 852
		[Flags]
		internal enum Attributes
		{
			// Token: 0x04000E0B RID: 3595
			ComposedOfAllVirtualMethods = 1,
			// Token: 0x04000E0C RID: 3596
			ComposedOfAllPrivateMethods = 2,
			// Token: 0x04000E0D RID: 3597
			ComposedOfNoPublicMembers = 4,
			// Token: 0x04000E0E RID: 3598
			ComposedOfNoStaticMembers = 8
		}
	}
}
