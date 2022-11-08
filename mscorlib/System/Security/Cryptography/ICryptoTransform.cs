using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000865 RID: 2149
	[ComVisible(true)]
	public interface ICryptoTransform : IDisposable
	{
		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06004E72 RID: 20082
		int InputBlockSize { get; }

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06004E73 RID: 20083
		int OutputBlockSize { get; }

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06004E74 RID: 20084
		bool CanTransformMultipleBlocks { get; }

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004E75 RID: 20085
		bool CanReuseTransform { get; }

		// Token: 0x06004E76 RID: 20086
		int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		// Token: 0x06004E77 RID: 20087
		byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
	}
}
