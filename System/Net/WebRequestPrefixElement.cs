using System;
using System.Globalization;
using System.Reflection;

namespace System.Net
{
	// Token: 0x0200040C RID: 1036
	internal class WebRequestPrefixElement
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060020B5 RID: 8373 RVA: 0x00080E1C File Offset: 0x0007FE1C
		// (set) Token: 0x060020B6 RID: 8374 RVA: 0x00080E90 File Offset: 0x0007FE90
		public IWebRequestCreate Creator
		{
			get
			{
				if (this.creator == null && this.creatorType != null)
				{
					lock (this)
					{
						if (this.creator == null)
						{
							this.creator = (IWebRequestCreate)Activator.CreateInstance(this.creatorType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture);
						}
					}
				}
				return this.creator;
			}
			set
			{
				this.creator = value;
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00080E9C File Offset: 0x0007FE9C
		public WebRequestPrefixElement(string P, Type creatorType)
		{
			if (!typeof(IWebRequestCreate).IsAssignableFrom(creatorType))
			{
				throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[]
				{
					creatorType.AssemblyQualifiedName,
					"IWebRequestCreate"
				}));
			}
			this.Prefix = P;
			this.creatorType = creatorType;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00080EF8 File Offset: 0x0007FEF8
		public WebRequestPrefixElement(string P, IWebRequestCreate C)
		{
			this.Prefix = P;
			this.Creator = C;
		}

		// Token: 0x040020C2 RID: 8386
		public string Prefix;

		// Token: 0x040020C3 RID: 8387
		internal IWebRequestCreate creator;

		// Token: 0x040020C4 RID: 8388
		internal Type creatorType;
	}
}
