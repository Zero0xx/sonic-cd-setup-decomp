using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200035B RID: 859
	[ComVisible(true)]
	public interface IFormatter
	{
		// Token: 0x060021A8 RID: 8616
		object Deserialize(Stream serializationStream);

		// Token: 0x060021A9 RID: 8617
		void Serialize(Stream serializationStream, object graph);

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060021AA RID: 8618
		// (set) Token: 0x060021AB RID: 8619
		ISurrogateSelector SurrogateSelector { get; set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060021AC RID: 8620
		// (set) Token: 0x060021AD RID: 8621
		SerializationBinder Binder { get; set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060021AE RID: 8622
		// (set) Token: 0x060021AF RID: 8623
		StreamingContext Context { get; set; }
	}
}
