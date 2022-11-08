using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000384 RID: 900
	internal class ValueTypeFixupInfo
	{
		// Token: 0x06002312 RID: 8978 RVA: 0x00058AD4 File Offset: 0x00057AD4
		public ValueTypeFixupInfo(long containerID, FieldInfo member, int[] parentIndex)
		{
			if (containerID == 0L && member == null)
			{
				this.m_containerID = containerID;
				this.m_parentField = member;
				this.m_parentIndex = parentIndex;
			}
			if (member == null && parentIndex == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustSupplyParent"));
			}
			if (member != null)
			{
				if (parentIndex != null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MemberAndArray"));
				}
				if (member.FieldType.IsValueType && containerID == 0L)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MustSupplyContainer"));
				}
			}
			this.m_containerID = containerID;
			this.m_parentField = member;
			this.m_parentIndex = parentIndex;
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x00058B67 File Offset: 0x00057B67
		public long ContainerID
		{
			get
			{
				return this.m_containerID;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x00058B6F File Offset: 0x00057B6F
		public FieldInfo ParentField
		{
			get
			{
				return this.m_parentField;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x00058B77 File Offset: 0x00057B77
		public int[] ParentIndex
		{
			get
			{
				return this.m_parentIndex;
			}
		}

		// Token: 0x04000EBF RID: 3775
		private long m_containerID;

		// Token: 0x04000EC0 RID: 3776
		private FieldInfo m_parentField;

		// Token: 0x04000EC1 RID: 3777
		private int[] m_parentIndex;
	}
}
