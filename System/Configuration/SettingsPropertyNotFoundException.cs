using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x02000714 RID: 1812
	[Serializable]
	public class SettingsPropertyNotFoundException : Exception
	{
		// Token: 0x06003774 RID: 14196 RVA: 0x000EB140 File Offset: 0x000EA140
		public SettingsPropertyNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000EB149 File Offset: 0x000EA149
		public SettingsPropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x000EB153 File Offset: 0x000EA153
		protected SettingsPropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000EB15D File Offset: 0x000EA15D
		public SettingsPropertyNotFoundException()
		{
		}
	}
}
