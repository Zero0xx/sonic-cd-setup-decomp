using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000EA RID: 234
	public sealed class TextureStateManagerCollection
	{
		// Token: 0x17000249 RID: 585
		[IndexerName("TextureState")]
		public TextureStateManager this[int index]
		{
			get
			{
				Debug.Assert(this.textureList != null, "There is no valid array created.");
				return this.textureList[index];
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0007166C File Offset: 0x00070A6C
		internal TextureStateManagerCollection(Device dev)
		{
			this.textureList = new TextureStateManager[8];
			int num = 0;
			do
			{
				this.textureList[num] = new TextureStateManager(dev, num);
				num++;
			}
			while (num < 8);
		}

		// Token: 0x04000FEF RID: 4079
		private TextureStateManager[] textureList;
	}
}
