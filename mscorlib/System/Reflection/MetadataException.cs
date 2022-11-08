using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x02000329 RID: 809
	internal class MetadataException : Exception
	{
		// Token: 0x06001F06 RID: 7942 RVA: 0x0004DDC2 File Offset: 0x0004CDC2
		internal MetadataException(int hr)
		{
			this.m_hr = hr;
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0004DDD4 File Offset: 0x0004CDD4
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "MetadataException HResult = {0:x}.", new object[]
			{
				this.m_hr
			});
		}

		// Token: 0x04000D33 RID: 3379
		private int m_hr;
	}
}
