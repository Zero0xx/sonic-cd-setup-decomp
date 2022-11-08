using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x0200075E RID: 1886
	[ComVisible(true)]
	public class ActivatedClientTypeEntry : TypeEntry
	{
		// Token: 0x06004320 RID: 17184 RVA: 0x000E5650 File Offset: 0x000E4650
		public ActivatedClientTypeEntry(string typeName, string assemblyName, string appUrl)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (appUrl == null)
			{
				throw new ArgumentNullException("appUrl");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
			this._appUrl = appUrl;
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x000E56A4 File Offset: 0x000E46A4
		public ActivatedClientTypeEntry(Type type, string appUrl)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (appUrl == null)
			{
				throw new ArgumentNullException("appUrl");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.nGetSimpleName();
			this._appUrl = appUrl;
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004322 RID: 17186 RVA: 0x000E56FC File Offset: 0x000E46FC
		public string ApplicationUrl
		{
			get
			{
				return this._appUrl;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004323 RID: 17187 RVA: 0x000E5704 File Offset: 0x000E4704
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004324 RID: 17188 RVA: 0x000E5732 File Offset: 0x000E4732
		// (set) Token: 0x06004325 RID: 17189 RVA: 0x000E573A File Offset: 0x000E473A
		public IContextAttribute[] ContextAttributes
		{
			get
			{
				return this._contextAttributes;
			}
			set
			{
				this._contextAttributes = value;
			}
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x000E5744 File Offset: 0x000E4744
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"type='",
				base.TypeName,
				", ",
				base.AssemblyName,
				"'; appUrl=",
				this._appUrl
			});
		}

		// Token: 0x040021C6 RID: 8646
		private string _appUrl;

		// Token: 0x040021C7 RID: 8647
		private IContextAttribute[] _contextAttributes;
	}
}
