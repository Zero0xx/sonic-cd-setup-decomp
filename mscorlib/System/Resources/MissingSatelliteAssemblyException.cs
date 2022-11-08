using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x02000433 RID: 1075
	[ComVisible(true)]
	[Serializable]
	public class MissingSatelliteAssemblyException : SystemException
	{
		// Token: 0x06002BD4 RID: 11220 RVA: 0x00092A44 File Offset: 0x00091A44
		public MissingSatelliteAssemblyException() : base(Environment.GetResourceString("MissingSatelliteAssembly_Default"))
		{
			base.SetErrorCode(-2146233034);
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x00092A61 File Offset: 0x00091A61
		public MissingSatelliteAssemblyException(string message) : base(message)
		{
			base.SetErrorCode(-2146233034);
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x00092A75 File Offset: 0x00091A75
		public MissingSatelliteAssemblyException(string message, string cultureName) : base(message)
		{
			base.SetErrorCode(-2146233034);
			this._cultureName = cultureName;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x00092A90 File Offset: 0x00091A90
		public MissingSatelliteAssemblyException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233034);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x00092AA5 File Offset: 0x00091AA5
		protected MissingSatelliteAssemblyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x00092AAF File Offset: 0x00091AAF
		public string CultureName
		{
			get
			{
				return this._cultureName;
			}
		}

		// Token: 0x0400155A RID: 5466
		private string _cultureName;
	}
}
