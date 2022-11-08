using System;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006A4 RID: 1700
	internal class ActivationAttributeStack
	{
		// Token: 0x06003D6A RID: 15722 RVA: 0x000D2217 File Offset: 0x000D1217
		internal ActivationAttributeStack()
		{
			this.activationTypes = new object[4];
			this.activationAttributes = new object[4];
			this.freeIndex = 0;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x000D2240 File Offset: 0x000D1240
		internal void Push(Type typ, object[] attr)
		{
			if (this.freeIndex == this.activationTypes.Length)
			{
				object[] destinationArray = new object[this.activationTypes.Length * 2];
				object[] destinationArray2 = new object[this.activationAttributes.Length * 2];
				Array.Copy(this.activationTypes, destinationArray, this.activationTypes.Length);
				Array.Copy(this.activationAttributes, destinationArray2, this.activationAttributes.Length);
				this.activationTypes = destinationArray;
				this.activationAttributes = destinationArray2;
			}
			this.activationTypes[this.freeIndex] = typ;
			this.activationAttributes[this.freeIndex] = attr;
			this.freeIndex++;
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x000D22DD File Offset: 0x000D12DD
		internal object[] Peek(Type typ)
		{
			if (this.freeIndex == 0 || this.activationTypes[this.freeIndex - 1] != typ)
			{
				return null;
			}
			return (object[])this.activationAttributes[this.freeIndex - 1];
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x000D2310 File Offset: 0x000D1310
		internal void Pop(Type typ)
		{
			if (this.freeIndex != 0 && this.activationTypes[this.freeIndex - 1] == typ)
			{
				this.freeIndex--;
				this.activationTypes[this.freeIndex] = null;
				this.activationAttributes[this.freeIndex] = null;
			}
		}

		// Token: 0x04001F6C RID: 8044
		private object[] activationTypes;

		// Token: 0x04001F6D RID: 8045
		private object[] activationAttributes;

		// Token: 0x04001F6E RID: 8046
		private int freeIndex;
	}
}
