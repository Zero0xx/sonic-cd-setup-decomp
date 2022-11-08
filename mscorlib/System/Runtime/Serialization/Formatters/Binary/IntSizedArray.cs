using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007EE RID: 2030
	[Serializable]
	internal sealed class IntSizedArray : ICloneable
	{
		// Token: 0x060047B3 RID: 18355 RVA: 0x000F5D44 File Offset: 0x000F4D44
		public IntSizedArray()
		{
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x000F5D68 File Offset: 0x000F4D68
		private IntSizedArray(IntSizedArray sizedArray)
		{
			this.objects = new int[sizedArray.objects.Length];
			sizedArray.objects.CopyTo(this.objects, 0);
			this.negObjects = new int[sizedArray.negObjects.Length];
			sizedArray.negObjects.CopyTo(this.negObjects, 0);
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x000F5DDE File Offset: 0x000F4DDE
		public object Clone()
		{
			return new IntSizedArray(this);
		}

		// Token: 0x17000C7C RID: 3196
		internal int this[int index]
		{
			get
			{
				if (index < 0)
				{
					if (-index > this.negObjects.Length - 1)
					{
						return 0;
					}
					return this.negObjects[-index];
				}
				else
				{
					if (index > this.objects.Length - 1)
					{
						return 0;
					}
					return this.objects[index];
				}
			}
			set
			{
				if (index < 0)
				{
					if (-index > this.negObjects.Length - 1)
					{
						this.IncreaseCapacity(index);
					}
					this.negObjects[-index] = value;
					return;
				}
				if (index > this.objects.Length - 1)
				{
					this.IncreaseCapacity(index);
				}
				this.objects[index] = value;
			}
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x000F5E70 File Offset: 0x000F4E70
		internal void IncreaseCapacity(int index)
		{
			try
			{
				if (index < 0)
				{
					int num = Math.Max(this.negObjects.Length * 2, -index + 1);
					int[] destinationArray = new int[num];
					Array.Copy(this.negObjects, 0, destinationArray, 0, this.negObjects.Length);
					this.negObjects = destinationArray;
				}
				else
				{
					int num2 = Math.Max(this.objects.Length * 2, index + 1);
					int[] destinationArray2 = new int[num2];
					Array.Copy(this.objects, 0, destinationArray2, 0, this.objects.Length);
					this.objects = destinationArray2;
				}
			}
			catch (Exception)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_CorruptedStream"));
			}
		}

		// Token: 0x0400248A RID: 9354
		internal int[] objects = new int[16];

		// Token: 0x0400248B RID: 9355
		internal int[] negObjects = new int[4];
	}
}
