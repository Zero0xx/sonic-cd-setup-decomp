using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000075 RID: 117
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[Serializable]
	public sealed class AttributeUsageAttribute : Attribute
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x00015D86 File Offset: 0x00014D86
		public AttributeUsageAttribute(AttributeTargets validOn)
		{
			this.m_attributeTarget = validOn;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00015DA7 File Offset: 0x00014DA7
		internal AttributeUsageAttribute(AttributeTargets validOn, bool allowMultiple, bool inherited)
		{
			this.m_attributeTarget = validOn;
			this.m_allowMultiple = allowMultiple;
			this.m_inherited = inherited;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00015DD6 File Offset: 0x00014DD6
		public AttributeTargets ValidOn
		{
			get
			{
				return this.m_attributeTarget;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00015DDE File Offset: 0x00014DDE
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x00015DE6 File Offset: 0x00014DE6
		public bool AllowMultiple
		{
			get
			{
				return this.m_allowMultiple;
			}
			set
			{
				this.m_allowMultiple = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00015DEF File Offset: 0x00014DEF
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00015DF7 File Offset: 0x00014DF7
		public bool Inherited
		{
			get
			{
				return this.m_inherited;
			}
			set
			{
				this.m_inherited = value;
			}
		}

		// Token: 0x0400020F RID: 527
		internal AttributeTargets m_attributeTarget = AttributeTargets.All;

		// Token: 0x04000210 RID: 528
		internal bool m_allowMultiple;

		// Token: 0x04000211 RID: 529
		internal bool m_inherited = true;

		// Token: 0x04000212 RID: 530
		internal static AttributeUsageAttribute Default = new AttributeUsageAttribute(AttributeTargets.All);
	}
}
