using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x02000761 RID: 1889
	[ComVisible(true)]
	public class WellKnownServiceTypeEntry : TypeEntry
	{
		// Token: 0x06004334 RID: 17204 RVA: 0x000E59E0 File Offset: 0x000E49E0
		public WellKnownServiceTypeEntry(string typeName, string assemblyName, string objectUri, WellKnownObjectMode mode)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (objectUri == null)
			{
				throw new ArgumentNullException("objectUri");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
			this._objectUri = objectUri;
			this._mode = mode;
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x000E5A3C File Offset: 0x000E4A3C
		public WellKnownServiceTypeEntry(Type type, string objectUri, WellKnownObjectMode mode)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (objectUri == null)
			{
				throw new ArgumentNullException("objectUri");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.FullName;
			this._objectUri = objectUri;
			this._mode = mode;
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06004336 RID: 17206 RVA: 0x000E5A9B File Offset: 0x000E4A9B
		public string ObjectUri
		{
			get
			{
				return this._objectUri;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004337 RID: 17207 RVA: 0x000E5AA3 File Offset: 0x000E4AA3
		public WellKnownObjectMode Mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06004338 RID: 17208 RVA: 0x000E5AAC File Offset: 0x000E4AAC
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06004339 RID: 17209 RVA: 0x000E5ADA File Offset: 0x000E4ADA
		// (set) Token: 0x0600433A RID: 17210 RVA: 0x000E5AE2 File Offset: 0x000E4AE2
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

		// Token: 0x0600433B RID: 17211 RVA: 0x000E5AEC File Offset: 0x000E4AEC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"type='",
				base.TypeName,
				", ",
				base.AssemblyName,
				"'; objectUri=",
				this._objectUri,
				"; mode=",
				this._mode.ToString()
			});
		}

		// Token: 0x040021CB RID: 8651
		private string _objectUri;

		// Token: 0x040021CC RID: 8652
		private WellKnownObjectMode _mode;

		// Token: 0x040021CD RID: 8653
		private IContextAttribute[] _contextAttributes;
	}
}
