using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x02000760 RID: 1888
	[ComVisible(true)]
	public class WellKnownClientTypeEntry : TypeEntry
	{
		// Token: 0x0600432D RID: 17197 RVA: 0x000E5880 File Offset: 0x000E4880
		public WellKnownClientTypeEntry(string typeName, string assemblyName, string objectUrl)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (objectUrl == null)
			{
				throw new ArgumentNullException("objectUrl");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
			this._objectUrl = objectUrl;
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x000E58D4 File Offset: 0x000E48D4
		public WellKnownClientTypeEntry(Type type, string objectUrl)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (objectUrl == null)
			{
				throw new ArgumentNullException("objectUrl");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.nGetSimpleName();
			this._objectUrl = objectUrl;
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600432F RID: 17199 RVA: 0x000E592C File Offset: 0x000E492C
		public string ObjectUrl
		{
			get
			{
				return this._objectUrl;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x000E5934 File Offset: 0x000E4934
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004331 RID: 17201 RVA: 0x000E5962 File Offset: 0x000E4962
		// (set) Token: 0x06004332 RID: 17202 RVA: 0x000E596A File Offset: 0x000E496A
		public string ApplicationUrl
		{
			get
			{
				return this._appUrl;
			}
			set
			{
				this._appUrl = value;
			}
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x000E5974 File Offset: 0x000E4974
		public override string ToString()
		{
			string text = string.Concat(new string[]
			{
				"type='",
				base.TypeName,
				", ",
				base.AssemblyName,
				"'; url=",
				this._objectUrl
			});
			if (this._appUrl != null)
			{
				text = text + "; appUrl=" + this._appUrl;
			}
			return text;
		}

		// Token: 0x040021C9 RID: 8649
		private string _objectUrl;

		// Token: 0x040021CA RID: 8650
		private string _appUrl;
	}
}
