using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms
{
	// Token: 0x02000647 RID: 1607
	[ListBindable(false)]
	[DesignerSerializer("System.Windows.Forms.Design.TableLayoutControlCollectionCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class TableLayoutControlCollection : Control.ControlCollection
	{
		// Token: 0x060054B2 RID: 21682 RVA: 0x00134C9C File Offset: 0x00133C9C
		public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
		{
			this._container = container;
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x00134CAC File Offset: 0x00133CAC
		public TableLayoutPanel Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x00134CB4 File Offset: 0x00133CB4
		public virtual void Add(Control control, int column, int row)
		{
			base.Add(control);
			this._container.SetColumn(control, column);
			this._container.SetRow(control, row);
		}

		// Token: 0x040036DC RID: 14044
		private TableLayoutPanel _container;
	}
}
