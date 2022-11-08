using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000051 RID: 81
	internal class AppDomainInitializerInfo
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00010A38 File Offset: 0x0000FA38
		internal AppDomainInitializerInfo(AppDomainInitializer init)
		{
			this.Info = null;
			if (init == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			arrayList2.Add(init);
			int num = 0;
			while (arrayList2.Count > num)
			{
				AppDomainInitializer appDomainInitializer = (AppDomainInitializer)arrayList2[num++];
				Delegate[] invocationList = appDomainInitializer.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					if (!invocationList[i].Method.IsStatic)
					{
						if (invocationList[i].Target != null)
						{
							AppDomainInitializer appDomainInitializer2 = invocationList[i].Target as AppDomainInitializer;
							if (appDomainInitializer2 == null)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_MustBeStatic"), invocationList[i].Method.ReflectedType.FullName + "::" + invocationList[i].Method.Name);
							}
							arrayList2.Add(appDomainInitializer2);
						}
					}
					else
					{
						arrayList.Add(new AppDomainInitializerInfo.ItemInfo
						{
							TargetTypeAssembly = invocationList[i].Method.ReflectedType.Module.Assembly.FullName,
							TargetTypeName = invocationList[i].Method.ReflectedType.FullName,
							MethodName = invocationList[i].Method.Name
						});
					}
				}
			}
			this.Info = (AppDomainInitializerInfo.ItemInfo[])arrayList.ToArray(typeof(AppDomainInitializerInfo.ItemInfo));
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00010BB0 File Offset: 0x0000FBB0
		internal AppDomainInitializer Unwrap()
		{
			if (this.Info == null)
			{
				return null;
			}
			AppDomainInitializer appDomainInitializer = null;
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			for (int i = 0; i < this.Info.Length; i++)
			{
				Assembly assembly = Assembly.Load(this.Info[i].TargetTypeAssembly);
				AppDomainInitializer appDomainInitializer2 = (AppDomainInitializer)Delegate.CreateDelegate(typeof(AppDomainInitializer), assembly.GetType(this.Info[i].TargetTypeName), this.Info[i].MethodName);
				if (appDomainInitializer == null)
				{
					appDomainInitializer = appDomainInitializer2;
				}
				else
				{
					appDomainInitializer = (AppDomainInitializer)Delegate.Combine(appDomainInitializer, appDomainInitializer2);
				}
			}
			return appDomainInitializer;
		}

		// Token: 0x04000190 RID: 400
		internal AppDomainInitializerInfo.ItemInfo[] Info;

		// Token: 0x02000052 RID: 82
		internal class ItemInfo
		{
			// Token: 0x04000191 RID: 401
			public string TargetTypeAssembly;

			// Token: 0x04000192 RID: 402
			public string TargetTypeName;

			// Token: 0x04000193 RID: 403
			public string MethodName;
		}
	}
}
