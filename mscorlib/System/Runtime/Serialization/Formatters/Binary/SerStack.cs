using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007EC RID: 2028
	internal sealed class SerStack
	{
		// Token: 0x060047A2 RID: 18338 RVA: 0x000F59EA File Offset: 0x000F49EA
		internal SerStack()
		{
			this.stackId = "System";
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x000F5A10 File Offset: 0x000F4A10
		internal SerStack(string stackId)
		{
			this.stackId = stackId;
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x000F5A34 File Offset: 0x000F4A34
		internal void Push(object obj)
		{
			if (this.top == this.objects.Length - 1)
			{
				this.IncreaseCapacity();
			}
			this.objects[++this.top] = obj;
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x000F5A74 File Offset: 0x000F4A74
		internal object Pop()
		{
			if (this.top < 0)
			{
				return null;
			}
			object result = this.objects[this.top];
			this.objects[this.top--] = null;
			return result;
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x000F5AB4 File Offset: 0x000F4AB4
		internal void IncreaseCapacity()
		{
			int num = this.objects.Length * 2;
			object[] destinationArray = new object[num];
			Array.Copy(this.objects, 0, destinationArray, 0, this.objects.Length);
			this.objects = destinationArray;
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x000F5AF0 File Offset: 0x000F4AF0
		internal object Peek()
		{
			if (this.top < 0)
			{
				return null;
			}
			return this.objects[this.top];
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x000F5B0A File Offset: 0x000F4B0A
		internal object PeekPeek()
		{
			if (this.top < 1)
			{
				return null;
			}
			return this.objects[this.top - 1];
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x000F5B26 File Offset: 0x000F4B26
		internal int Count()
		{
			return this.top + 1;
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x000F5B30 File Offset: 0x000F4B30
		internal bool IsEmpty()
		{
			return this.top <= 0;
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x000F5B40 File Offset: 0x000F4B40
		[Conditional("SER_LOGGING")]
		internal void Dump()
		{
			for (int i = 0; i < this.Count(); i++)
			{
				object obj = this.objects[i];
			}
		}

		// Token: 0x04002484 RID: 9348
		internal object[] objects = new object[5];

		// Token: 0x04002485 RID: 9349
		internal string stackId;

		// Token: 0x04002486 RID: 9350
		internal int top = -1;

		// Token: 0x04002487 RID: 9351
		internal int next;
	}
}
