using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000356 RID: 854
	[Serializable]
	internal sealed class RuntimeEventInfo : EventInfo, ISerializable
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x0005347C File Offset: 0x0005247C
		internal RuntimeEventInfo()
		{
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00053484 File Offset: 0x00052484
		internal unsafe RuntimeEventInfo(int tkEvent, RuntimeType declaredType, RuntimeType.RuntimeTypeCache reflectedTypeCache, out bool isPrivate)
		{
			MetadataImport metadataImport = declaredType.Module.MetadataImport;
			this.m_token = tkEvent;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_declaringType = declaredType;
			RuntimeTypeHandle typeHandleInternal = declaredType.GetTypeHandleInternal();
			RuntimeTypeHandle runtimeTypeHandle = reflectedTypeCache.RuntimeTypeHandle;
			metadataImport.GetEventProps(tkEvent, out this.m_utf8name, out this.m_flags);
			int associatesCount = metadataImport.GetAssociatesCount(tkEvent);
			AssociateRecord* ptr = stackalloc AssociateRecord[sizeof(AssociateRecord) * associatesCount];
			metadataImport.GetAssociates(tkEvent, ptr, associatesCount);
			RuntimeMethodInfo runtimeMethodInfo;
			Associates.AssignAssociates(ptr, associatesCount, typeHandleInternal, runtimeTypeHandle, out this.m_addMethod, out this.m_removeMethod, out this.m_raiseMethod, out runtimeMethodInfo, out runtimeMethodInfo, out this.m_otherMethod, out isPrivate, out this.m_bindingFlags);
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0005352C File Offset: 0x0005252C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeEventInfo runtimeEventInfo = o as RuntimeEventInfo;
			return runtimeEventInfo != null && runtimeEventInfo.m_token == this.m_token && this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().Equals(runtimeEventInfo.m_declaringType.GetTypeHandleInternal().GetModuleHandle());
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x00053583 File Offset: 0x00052583
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0005358C File Offset: 0x0005258C
		public override string ToString()
		{
			if (this.m_addMethod == null || this.m_addMethod.GetParametersNoCopy().Length == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoPublicAddMethod"));
			}
			return this.m_addMethod.GetParametersNoCopy()[0].ParameterType.SigToString() + " " + this.Name;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000535E7 File Offset: 0x000525E7
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x00053600 File Offset: 0x00052600
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00053648 File Offset: 0x00052648
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x0005368E File Offset: 0x0005268E
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Event;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00053694 File Offset: 0x00052694
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = new Utf8String(this.m_utf8name).ToString();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x000536CE File Offset: 0x000526CE
		public override Type DeclaringType
		{
			get
			{
				return this.m_declaringType;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000536D6 File Offset: 0x000526D6
		public override Type ReflectedType
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeType;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600216F RID: 8559 RVA: 0x000536E3 File Offset: 0x000526E3
		public override int MetadataToken
		{
			get
			{
				return this.m_token;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000536EB File Offset: 0x000526EB
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000536F8 File Offset: 0x000526F8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedType, null, MemberTypes.Event);
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x0005371C File Offset: 0x0005271C
		public override MethodInfo[] GetOtherMethods(bool nonPublic)
		{
			ArrayList arrayList = new ArrayList();
			if (this.m_otherMethod == null)
			{
				return new MethodInfo[0];
			}
			for (int i = 0; i < this.m_otherMethod.Length; i++)
			{
				if (Associates.IncludeAccessor(this.m_otherMethod[i], nonPublic))
				{
					arrayList.Add(this.m_otherMethod[i]);
				}
			}
			return arrayList.ToArray(typeof(MethodInfo)) as MethodInfo[];
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x00053785 File Offset: 0x00052785
		public override MethodInfo GetAddMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_addMethod, nonPublic))
			{
				return null;
			}
			return this.m_addMethod;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x0005379D File Offset: 0x0005279D
		public override MethodInfo GetRemoveMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_removeMethod, nonPublic))
			{
				return null;
			}
			return this.m_removeMethod;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000537B5 File Offset: 0x000527B5
		public override MethodInfo GetRaiseMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_raiseMethod, nonPublic))
			{
				return null;
			}
			return this.m_raiseMethod;
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000537CD File Offset: 0x000527CD
		public override EventAttributes Attributes
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x04000E1A RID: 3610
		private int m_token;

		// Token: 0x04000E1B RID: 3611
		private EventAttributes m_flags;

		// Token: 0x04000E1C RID: 3612
		private string m_name;

		// Token: 0x04000E1D RID: 3613
		private unsafe void* m_utf8name;

		// Token: 0x04000E1E RID: 3614
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000E1F RID: 3615
		private RuntimeMethodInfo m_addMethod;

		// Token: 0x04000E20 RID: 3616
		private RuntimeMethodInfo m_removeMethod;

		// Token: 0x04000E21 RID: 3617
		private RuntimeMethodInfo m_raiseMethod;

		// Token: 0x04000E22 RID: 3618
		private MethodInfo[] m_otherMethod;

		// Token: 0x04000E23 RID: 3619
		private RuntimeType m_declaringType;

		// Token: 0x04000E24 RID: 3620
		private BindingFlags m_bindingFlags;
	}
}
