using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000567 RID: 1383
	[ComVisible(true)]
	[Serializable]
	public sealed class UnknownWrapper
	{
		// Token: 0x060033B7 RID: 13239 RVA: 0x000ADCE4 File Offset: 0x000ACCE4
		public UnknownWrapper(object obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000ADCF3 File Offset: 0x000ACCF3
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04001B0E RID: 6926
		private object m_WrappedObject;
	}
}
