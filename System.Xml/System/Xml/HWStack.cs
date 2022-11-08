using System;

namespace System.Xml
{
	// Token: 0x02000019 RID: 25
	internal class HWStack : ICloneable
	{
		// Token: 0x06000067 RID: 103 RVA: 0x000039E8 File Offset: 0x000029E8
		internal HWStack(int GrowthRate) : this(GrowthRate, int.MaxValue)
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000039F6 File Offset: 0x000029F6
		internal HWStack(int GrowthRate, int limit)
		{
			this.growthRate = GrowthRate;
			this.used = 0;
			this.stack = new object[GrowthRate];
			this.size = GrowthRate;
			this.limit = limit;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003A28 File Offset: 0x00002A28
		internal object Push()
		{
			if (this.used == this.size)
			{
				if (this.limit <= this.used)
				{
					throw new XmlException("Xml_StackOverflow", string.Empty);
				}
				object[] destinationArray = new object[this.size + this.growthRate];
				if (this.used > 0)
				{
					Array.Copy(this.stack, 0, destinationArray, 0, this.used);
				}
				this.stack = destinationArray;
				this.size += this.growthRate;
			}
			return this.stack[this.used++];
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003AC4 File Offset: 0x00002AC4
		internal object Pop()
		{
			if (0 < this.used)
			{
				this.used--;
				return this.stack[this.used];
			}
			return null;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003AF9 File Offset: 0x00002AF9
		internal object Peek()
		{
			if (this.used <= 0)
			{
				return null;
			}
			return this.stack[this.used - 1];
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003B15 File Offset: 0x00002B15
		internal void AddToTop(object o)
		{
			if (this.used > 0)
			{
				this.stack[this.used - 1] = o;
			}
		}

		// Token: 0x17000017 RID: 23
		internal object this[int index]
		{
			get
			{
				if (index >= 0 && index < this.used)
				{
					return this.stack[index];
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				if (index >= 0 && index < this.used)
				{
					this.stack[index] = value;
					return;
				}
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003B78 File Offset: 0x00002B78
		internal int Length
		{
			get
			{
				return this.used;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003B80 File Offset: 0x00002B80
		private HWStack(object[] stack, int growthRate, int used, int size)
		{
			this.stack = stack;
			this.growthRate = growthRate;
			this.used = used;
			this.size = size;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003BA5 File Offset: 0x00002BA5
		public object Clone()
		{
			return new HWStack((object[])this.stack.Clone(), this.growthRate, this.used, this.size);
		}

		// Token: 0x04000479 RID: 1145
		private object[] stack;

		// Token: 0x0400047A RID: 1146
		private int growthRate;

		// Token: 0x0400047B RID: 1147
		private int used;

		// Token: 0x0400047C RID: 1148
		private int size;

		// Token: 0x0400047D RID: 1149
		private int limit;
	}
}
