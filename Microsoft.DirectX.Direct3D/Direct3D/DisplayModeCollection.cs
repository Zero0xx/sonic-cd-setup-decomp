using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000FE RID: 254
	public sealed class DisplayModeCollection : IEnumerable, IEnumerator
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x000659A8 File Offset: 0x00064DA8
		internal DisplayModeCollection(int a)
		{
			this.currList = default(FormatList);
			this.current = -1;
			this.adapter = a;
			this.format = 0;
			this.currentIndex = 0;
			this.formatList = new ArrayList();
			Format[] values = Enum.GetValues(typeof(Format));
			this.max = 0;
			int num = 0;
			if (0 < values.Length)
			{
				do
				{
					int adapterModeCount = Manager.GetAdapterModeCount(this.adapter, values[num]);
					if (adapterModeCount > 0)
					{
						FormatList formatList = default(FormatList);
						formatList.minModeNum = this.max;
						formatList.maxModeNum = this.max + adapterModeCount;
						formatList.format = values[num];
						this.formatList.Add(formatList);
						this.max += adapterModeCount;
					}
					num++;
				}
				while (num < values.Length);
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00065A9C File Offset: 0x00064E9C
		internal DisplayModeCollection(int a, int f)
		{
			this.currList = default(FormatList);
			this.current = -1;
			this.adapter = a;
			this.format = f;
			this.max = Manager.GetAdapterModeCount(this.adapter, (Format)this.format);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000623D0 File Offset: 0x000617D0
		private DisplayModeCollection()
		{
			this.currList = default(FormatList);
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00065AEC File Offset: 0x00064EEC
		public object Current
		{
			get
			{
				default(DisplayMode) = new DisplayMode();
				DisplayMode displayMode;
				if (this.format == 0 && this.formatList != null && this.formatList.Count != 0)
				{
					FormatList formatList = (FormatList)(this.formatList[this.currentIndex] as FormatList);
					if (formatList.maxModeNum <= this.current)
					{
						this.currentIndex++;
						formatList = (FormatList)(this.formatList[this.currentIndex] as FormatList);
					}
					displayMode = Manager.EnumAdapterModes(this.adapter, formatList.format, this.current - formatList.minModeNum);
				}
				else
				{
					displayMode = Manager.EnumAdapterModes(this.adapter, (Format)this.format, this.current);
				}
				return displayMode;
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000623F4 File Offset: 0x000617F4
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00062410 File Offset: 0x00061810
		[return: MarshalAs(UnmanagedType.U1)]
		public bool MoveNext()
		{
			if (this.max == 0)
			{
				return false;
			}
			if (this.current == this.max - 1)
			{
				return false;
			}
			this.current++;
			return true;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00062450 File Offset: 0x00061850
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00062464 File Offset: 0x00061864
		public int Count
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x1700028E RID: 654
		public DisplayModeCollection this[Format f]
		{
			get
			{
				return new DisplayModeCollection(this.adapter, (int)f);
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0006249C File Offset: 0x0006189C
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(DisplayModeCollection) && this.adapter == compare.adapter;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000624D8 File Offset: 0x000618D8
		public override int GetHashCode()
		{
			return this.adapter.GetHashCode() ^ this.format;
		}

		// Token: 0x04001036 RID: 4150
		private int adapter;

		// Token: 0x04001037 RID: 4151
		private int current;

		// Token: 0x04001038 RID: 4152
		private int max;

		// Token: 0x04001039 RID: 4153
		private int format;

		// Token: 0x0400103A RID: 4154
		private int currentIndex;

		// Token: 0x0400103B RID: 4155
		private FormatList currList;

		// Token: 0x0400103C RID: 4156
		private ArrayList formatList;
	}
}
