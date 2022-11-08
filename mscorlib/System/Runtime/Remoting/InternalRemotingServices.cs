using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Cache;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x0200076A RID: 1898
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class InternalRemotingServices
	{
		// Token: 0x060043C6 RID: 17350 RVA: 0x000E7D4D File Offset: 0x000E6D4D
		[Conditional("_LOGGING")]
		public static void DebugOutChnl(string s)
		{
			Message.OutToUnmanagedDebugger("CHNL:" + s + "\n");
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x000E7D64 File Offset: 0x000E6D64
		[Conditional("_LOGGING")]
		public static void RemotingTrace(params object[] messages)
		{
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x000E7D66 File Offset: 0x000E6D66
		[Conditional("_DEBUG")]
		public static void RemotingAssert(bool condition, string message)
		{
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x000E7D68 File Offset: 0x000E6D68
		[CLSCompliant(false)]
		public static void SetServerIdentity(MethodCall m, object srvID)
		{
			((IInternalMessage)m).ServerIdentityObject = (ServerIdentity)srvID;
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x000E7D84 File Offset: 0x000E6D84
		internal static RemotingMethodCachedData GetReflectionCachedData(MethodBase mi)
		{
			RemotingMethodCachedData remotingMethodCachedData = (RemotingMethodCachedData)mi.Cache[CacheObjType.RemotingData];
			if (remotingMethodCachedData == null)
			{
				remotingMethodCachedData = new RemotingMethodCachedData(mi);
				mi.Cache[CacheObjType.RemotingData] = remotingMethodCachedData;
			}
			return remotingMethodCachedData;
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x000E7DC0 File Offset: 0x000E6DC0
		internal static RemotingTypeCachedData GetReflectionCachedData(Type mi)
		{
			RemotingTypeCachedData remotingTypeCachedData = (RemotingTypeCachedData)mi.Cache[CacheObjType.RemotingData];
			if (remotingTypeCachedData == null)
			{
				remotingTypeCachedData = new RemotingTypeCachedData(mi);
				mi.Cache[CacheObjType.RemotingData] = remotingTypeCachedData;
			}
			return remotingTypeCachedData;
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x000E7DFC File Offset: 0x000E6DFC
		internal static RemotingCachedData GetReflectionCachedData(MemberInfo mi)
		{
			RemotingCachedData remotingCachedData = (RemotingCachedData)mi.Cache[CacheObjType.RemotingData];
			if (remotingCachedData == null)
			{
				if (mi is MethodBase)
				{
					remotingCachedData = new RemotingMethodCachedData(mi);
				}
				else if (mi is Type)
				{
					remotingCachedData = new RemotingTypeCachedData(mi);
				}
				else
				{
					remotingCachedData = new RemotingCachedData(mi);
				}
				mi.Cache[CacheObjType.RemotingData] = remotingCachedData;
			}
			return remotingCachedData;
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x000E7E58 File Offset: 0x000E6E58
		internal static RemotingCachedData GetReflectionCachedData(ParameterInfo reflectionObject)
		{
			RemotingCachedData remotingCachedData = (RemotingCachedData)reflectionObject.Cache[CacheObjType.RemotingData];
			if (remotingCachedData == null)
			{
				remotingCachedData = new RemotingCachedData(reflectionObject);
				reflectionObject.Cache[CacheObjType.RemotingData] = remotingCachedData;
			}
			return remotingCachedData;
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x000E7E94 File Offset: 0x000E6E94
		public static SoapAttribute GetCachedSoapAttribute(object reflectionObject)
		{
			MemberInfo memberInfo = reflectionObject as MemberInfo;
			if (memberInfo != null)
			{
				return InternalRemotingServices.GetReflectionCachedData(memberInfo).GetSoapAttribute();
			}
			return InternalRemotingServices.GetReflectionCachedData((ParameterInfo)reflectionObject).GetSoapAttribute();
		}
	}
}
