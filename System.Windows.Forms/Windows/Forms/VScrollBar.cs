using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000722 RID: 1826
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionVScrollBar")]
	public class VScrollBar : ScrollBar
	{
		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x060060C9 RID: 24777 RVA: 0x0016273C File Offset: 0x0016173C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 1;
				return createParams;
			}
		}

		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x060060CA RID: 24778 RVA: 0x0016275F File Offset: 0x0016175F
		protected override Size DefaultSize
		{
			get
			{
				return new Size(SystemInformation.VerticalScrollBarWidth, 80);
			}
		}

		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x060060CB RID: 24779 RVA: 0x0016276D File Offset: 0x0016176D
		// (set) Token: 0x060060CC RID: 24780 RVA: 0x00162770 File Offset: 0x00161770
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return RightToLeft.No;
			}
			set
			{
			}
		}

		// Token: 0x140003B6 RID: 950
		// (add) Token: 0x060060CD RID: 24781 RVA: 0x00162772 File Offset: 0x00161772
		// (remove) Token: 0x060060CE RID: 24782 RVA: 0x0016277B File Offset: 0x0016177B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}
	}
}
