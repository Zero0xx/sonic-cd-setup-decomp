using System;
using System.IO;

namespace System.Xml
{
	// Token: 0x02000032 RID: 50
	internal class XmlCachedStream : MemoryStream
	{
		// Token: 0x06000179 RID: 377 RVA: 0x000075AC File Offset: 0x000065AC
		internal XmlCachedStream(Uri uri, Stream stream)
		{
			this.uri = uri;
			try
			{
				byte[] buffer = new byte[4096];
				int count;
				while ((count = stream.Read(buffer, 0, 4096)) > 0)
				{
					this.Write(buffer, 0, count);
				}
				base.Position = 0L;
			}
			finally
			{
				stream.Close();
			}
		}

		// Token: 0x040004B9 RID: 1209
		private const int MoveBufferSize = 4096;

		// Token: 0x040004BA RID: 1210
		private Uri uri;
	}
}
