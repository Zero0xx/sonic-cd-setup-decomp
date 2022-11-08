using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x02000018 RID: 24
	internal sealed class EmptyEnumerator : IEnumerator
	{
		// Token: 0x06000063 RID: 99 RVA: 0x000039CA File Offset: 0x000029CA
		bool IEnumerator.MoveNext()
		{
			return false;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000039CD File Offset: 0x000029CD
		void IEnumerator.Reset()
		{
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000039CF File Offset: 0x000029CF
		object IEnumerator.Current
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}
	}
}
