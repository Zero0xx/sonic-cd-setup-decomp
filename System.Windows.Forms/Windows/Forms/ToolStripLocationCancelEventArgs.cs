using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006EC RID: 1772
	internal class ToolStripLocationCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06005DD5 RID: 24021 RVA: 0x001541AE File Offset: 0x001531AE
		public ToolStripLocationCancelEventArgs(Point newLocation, bool value) : base(value)
		{
			this.newLocation = newLocation;
		}

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06005DD6 RID: 24022 RVA: 0x001541BE File Offset: 0x001531BE
		public Point NewLocation
		{
			get
			{
				return this.newLocation;
			}
		}

		// Token: 0x04003963 RID: 14691
		private Point newLocation;
	}
}
