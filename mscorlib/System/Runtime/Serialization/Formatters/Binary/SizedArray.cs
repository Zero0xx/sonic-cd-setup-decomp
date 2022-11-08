using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007ED RID: 2029
	[Serializable]
	internal sealed class SizedArray : ICloneable
	{
		// Token: 0x060047AC RID: 18348 RVA: 0x000F5B67 File Offset: 0x000F4B67
		internal SizedArray()
		{
			this.objects = new object[16];
			this.negObjects = new object[4];
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x000F5B88 File Offset: 0x000F4B88
		internal SizedArray(int length)
		{
			this.objects = new object[length];
			this.negObjects = new object[length];
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x000F5BA8 File Offset: 0x000F4BA8
		private SizedArray(SizedArray sizedArray)
		{
			this.objects = new object[sizedArray.objects.Length];
			sizedArray.objects.CopyTo(this.objects, 0);
			this.negObjects = new object[sizedArray.negObjects.Length];
			sizedArray.negObjects.CopyTo(this.negObjects, 0);
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x000F5C05 File Offset: 0x000F4C05
		public object Clone()
		{
			return new SizedArray(this);
		}

		// Token: 0x17000C7B RID: 3195
		internal object this[int index]
		{
			get
			{
				if (index < 0)
				{
					if (-index > this.negObjects.Length - 1)
					{
						return null;
					}
					return this.negObjects[-index];
				}
				else
				{
					if (index > this.objects.Length - 1)
					{
						return null;
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
				object obj = this.objects[index];
				this.objects[index] = value;
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x000F5C9C File Offset: 0x000F4C9C
		internal void IncreaseCapacity(int index)
		{
			try
			{
				if (index < 0)
				{
					int num = Math.Max(this.negObjects.Length * 2, -index + 1);
					object[] destinationArray = new object[num];
					Array.Copy(this.negObjects, 0, destinationArray, 0, this.negObjects.Length);
					this.negObjects = destinationArray;
				}
				else
				{
					int num2 = Math.Max(this.objects.Length * 2, index + 1);
					object[] destinationArray2 = new object[num2];
					Array.Copy(this.objects, 0, destinationArray2, 0, this.objects.Length);
					this.objects = destinationArray2;
				}
			}
			catch (Exception)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_CorruptedStream"));
			}
		}

		// Token: 0x04002488 RID: 9352
		internal object[] objects;

		// Token: 0x04002489 RID: 9353
		internal object[] negObjects;
	}
}
