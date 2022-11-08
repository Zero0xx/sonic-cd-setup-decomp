using System;
using System.Reflection;

namespace System.Runtime.InteropServices.TCEAdapterGen
{
	// Token: 0x020008C5 RID: 2245
	internal class EventItfInfo
	{
		// Token: 0x060051C8 RID: 20936 RVA: 0x001262B1 File Offset: 0x001252B1
		public EventItfInfo(string strEventItfName, string strSrcItfName, string strEventProviderName, Assembly asmImport, Assembly asmSrcItf)
		{
			this.m_strEventItfName = strEventItfName;
			this.m_strSrcItfName = strSrcItfName;
			this.m_strEventProviderName = strEventProviderName;
			this.m_asmImport = asmImport;
			this.m_asmSrcItf = asmSrcItf;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001262E0 File Offset: 0x001252E0
		public Type GetEventItfType()
		{
			Type type = this.m_asmImport.GetType(this.m_strEventItfName, true, false);
			if (type != null && !type.IsVisible)
			{
				type = null;
			}
			return type;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00126310 File Offset: 0x00125310
		public Type GetSrcItfType()
		{
			Type type = this.m_asmSrcItf.GetType(this.m_strSrcItfName, true, false);
			if (type != null && !type.IsVisible)
			{
				type = null;
			}
			return type;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x0012633F File Offset: 0x0012533F
		public string GetEventProviderName()
		{
			return this.m_strEventProviderName;
		}

		// Token: 0x04002A30 RID: 10800
		private string m_strEventItfName;

		// Token: 0x04002A31 RID: 10801
		private string m_strSrcItfName;

		// Token: 0x04002A32 RID: 10802
		private string m_strEventProviderName;

		// Token: 0x04002A33 RID: 10803
		private Assembly m_asmImport;

		// Token: 0x04002A34 RID: 10804
		private Assembly m_asmSrcItf;
	}
}
