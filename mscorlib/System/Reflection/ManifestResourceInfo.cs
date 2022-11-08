using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000312 RID: 786
	[ComVisible(true)]
	public class ManifestResourceInfo
	{
		// Token: 0x06001EA5 RID: 7845 RVA: 0x0004D5D2 File Offset: 0x0004C5D2
		internal ManifestResourceInfo(Assembly containingAssembly, string containingFileName, ResourceLocation resourceLocation)
		{
			this._containingAssembly = containingAssembly;
			this._containingFileName = containingFileName;
			this._resourceLocation = resourceLocation;
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x0004D5EF File Offset: 0x0004C5EF
		public virtual Assembly ReferencedAssembly
		{
			get
			{
				return this._containingAssembly;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x0004D5F7 File Offset: 0x0004C5F7
		public virtual string FileName
		{
			get
			{
				return this._containingFileName;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x0004D5FF File Offset: 0x0004C5FF
		public virtual ResourceLocation ResourceLocation
		{
			get
			{
				return this._resourceLocation;
			}
		}

		// Token: 0x04000B6F RID: 2927
		private Assembly _containingAssembly;

		// Token: 0x04000B70 RID: 2928
		private string _containingFileName;

		// Token: 0x04000B71 RID: 2929
		private ResourceLocation _resourceLocation;
	}
}
