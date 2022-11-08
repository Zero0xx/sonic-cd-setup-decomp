using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F8 RID: 1784
	public class IgnoreSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06003701 RID: 14081 RVA: 0x000EA001 File Offset: 0x000E9001
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			return null;
		}
	}
}
