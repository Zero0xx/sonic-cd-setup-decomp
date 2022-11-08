using System;
using System.Runtime.InteropServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000FF RID: 255
	public sealed class AdapterInformation
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x00062514 File Offset: 0x00061914
		internal AdapterInformation(int a)
		{
			this.adapter = a;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000624FC File Offset: 0x000618FC
		private AdapterInformation()
		{
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00062534 File Offset: 0x00061934
		public int Adapter
		{
			get
			{
				return this.adapter;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000658CC File Offset: 0x00064CCC
		public AdapterDetails Information
		{
			get
			{
				return Manager.GetAdapterInformation(this.adapter, 0);
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000658EC File Offset: 0x00064CEC
		public AdapterDetails GetWhqlInformation()
		{
			return Manager.GetAdapterInformation(this.adapter, 2);
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0006590C File Offset: 0x00064D0C
		public DisplayMode CurrentDisplayMode
		{
			get
			{
				return Manager.GetAdapterDisplayMode(this.adapter);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0006254C File Offset: 0x0006194C
		public DisplayModeCollection SupportedDisplayModes
		{
			get
			{
				return new DisplayModeCollection(this.adapter);
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0006256C File Offset: 0x0006196C
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(AdapterInformation) && compare.adapter == this.adapter;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000625A8 File Offset: 0x000619A8
		public override int GetHashCode()
		{
			return this.adapter.GetHashCode();
		}

		// Token: 0x0400103D RID: 4157
		private int adapter;
	}
}
