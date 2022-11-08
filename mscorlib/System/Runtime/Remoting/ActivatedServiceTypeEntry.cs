using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x0200075F RID: 1887
	[ComVisible(true)]
	public class ActivatedServiceTypeEntry : TypeEntry
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x000E5791 File Offset: 0x000E4791
		public ActivatedServiceTypeEntry(string typeName, string assemblyName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x000E57C3 File Offset: 0x000E47C3
		public ActivatedServiceTypeEntry(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.nGetSimpleName();
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004329 RID: 17193 RVA: 0x000E57FC File Offset: 0x000E47FC
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x000E582A File Offset: 0x000E482A
		// (set) Token: 0x0600432B RID: 17195 RVA: 0x000E5832 File Offset: 0x000E4832
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

		// Token: 0x0600432C RID: 17196 RVA: 0x000E583C File Offset: 0x000E483C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"type='",
				base.TypeName,
				", ",
				base.AssemblyName,
				"'"
			});
		}

		// Token: 0x040021C8 RID: 8648
		private IContextAttribute[] _contextAttributes;
	}
}
