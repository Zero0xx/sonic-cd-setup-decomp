using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F4 RID: 244
	public sealed class LightsCollection : IEnumerable, IEnumerator
	{
		// Token: 0x17000283 RID: 643
		public Light this[int index]
		{
			get
			{
				if (this.ourLights != null && this.ourLights.Length != 0)
				{
					if (this.ourLights.Length > index)
					{
						if (this.ourLights[index] == null)
						{
							this.ourLights[index] = new Light(this.pDevice, index);
						}
					}
					else
					{
						Light[] destinationArray = new Light[index + 1];
						Array.Copy(this.ourLights, 0, destinationArray, 0, this.ourLights.Length);
						this.ourLights = destinationArray;
						this.ourLights[index] = new Light(this.pDevice, index);
					}
				}
				else
				{
					this.ourLights = new Light[index + 1];
					this.ourLights[index] = new Light(this.pDevice, index);
				}
				return this.ourLights[index];
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x000729A8 File Offset: 0x00071DA8
		public object Current
		{
			get
			{
				if (this.ourLights[this.current] != null)
				{
					return this.ourLights[this.current];
				}
				return null;
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000729D8 File Offset: 0x00071DD8
		[return: MarshalAs(UnmanagedType.U1)]
		public bool MoveNext()
		{
			bool result = false;
			if (this.ourLights != null && this.ourLights.Length != 0)
			{
				this.current++;
				int num = this.current;
				if (num < this.ourLights.Length)
				{
					while (this.ourLights[num] == null)
					{
						num++;
						if (num >= this.ourLights.Length)
						{
							return result;
						}
					}
					result = true;
					this.current = num;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00072A44 File Offset: 0x00071E44
		public int Count
		{
			get
			{
				int num = 0;
				if (this.ourLights != null && this.ourLights.Length != 0)
				{
					int num2 = 0;
					if (0 < this.ourLights.Length)
					{
						do
						{
							if (this.ourLights[num2] != null)
							{
								num++;
							}
							num2++;
						}
						while (num2 < this.ourLights.Length);
					}
				}
				else
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00062110 File Offset: 0x00061510
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00062124 File Offset: 0x00061524
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00062140 File Offset: 0x00061540
		internal LightsCollection(Device dev)
		{
			this.pDevice = dev;
			this.current = -1;
		}

		// Token: 0x0400101F RID: 4127
		private int current;

		// Token: 0x04001020 RID: 4128
		private Device pDevice;

		// Token: 0x04001021 RID: 4129
		private Light[] ourLights;
	}
}
