using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x02000713 RID: 1811
	[Serializable]
	public class SettingsPropertyIsReadOnlyException : Exception
	{
		// Token: 0x06003770 RID: 14192 RVA: 0x000EB11B File Offset: 0x000EA11B
		public SettingsPropertyIsReadOnlyException(string message) : base(message)
		{
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000EB124 File Offset: 0x000EA124
		public SettingsPropertyIsReadOnlyException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000EB12E File Offset: 0x000EA12E
		protected SettingsPropertyIsReadOnlyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000EB138 File Offset: 0x000EA138
		public SettingsPropertyIsReadOnlyException()
		{
		}
	}
}
