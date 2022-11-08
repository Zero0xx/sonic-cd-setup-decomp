using System;

namespace System.Windows.Forms.Layout
{
	// Token: 0x0200078D RID: 1933
	internal sealed class LayoutTransaction : IDisposable
	{
		// Token: 0x06006590 RID: 26000 RVA: 0x0017411A File Offset: 0x0017311A
		public LayoutTransaction(Control controlToLayout, IArrangedElement controlCausingLayout, string property) : this(controlToLayout, controlCausingLayout, property, true)
		{
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x00174128 File Offset: 0x00173128
		public LayoutTransaction(Control controlToLayout, IArrangedElement controlCausingLayout, string property, bool resumeLayout)
		{
			CommonProperties.xClearPreferredSizeCache(controlCausingLayout);
			this._controlToLayout = controlToLayout;
			this._resumeLayout = resumeLayout;
			if (this._controlToLayout != null)
			{
				this._controlToLayout.SuspendLayout();
				CommonProperties.xClearPreferredSizeCache(this._controlToLayout);
				if (resumeLayout)
				{
					this._controlToLayout.PerformLayout(new LayoutEventArgs(controlCausingLayout, property));
				}
			}
		}

		// Token: 0x06006592 RID: 26002 RVA: 0x00174184 File Offset: 0x00173184
		public void Dispose()
		{
			if (this._controlToLayout != null)
			{
				this._controlToLayout.ResumeLayout(this._resumeLayout);
			}
		}

		// Token: 0x06006593 RID: 26003 RVA: 0x001741A0 File Offset: 0x001731A0
		public static IDisposable CreateTransactionIf(bool condition, Control controlToLayout, IArrangedElement elementCausingLayout, string property)
		{
			if (condition)
			{
				return new LayoutTransaction(controlToLayout, elementCausingLayout, property);
			}
			CommonProperties.xClearPreferredSizeCache(elementCausingLayout);
			return default(NullLayoutTransaction);
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x001741CD File Offset: 0x001731CD
		public static void DoLayout(IArrangedElement elementToLayout, IArrangedElement elementCausingLayout, string property)
		{
			if (elementCausingLayout != null)
			{
				CommonProperties.xClearPreferredSizeCache(elementCausingLayout);
				if (elementToLayout != null)
				{
					CommonProperties.xClearPreferredSizeCache(elementToLayout);
					elementToLayout.PerformLayout(elementCausingLayout, property);
				}
			}
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x001741E9 File Offset: 0x001731E9
		public static void DoLayoutIf(bool condition, IArrangedElement elementToLayout, IArrangedElement elementCausingLayout, string property)
		{
			if (!condition)
			{
				if (elementCausingLayout != null)
				{
					CommonProperties.xClearPreferredSizeCache(elementCausingLayout);
					return;
				}
			}
			else
			{
				LayoutTransaction.DoLayout(elementToLayout, elementCausingLayout, property);
			}
		}

		// Token: 0x04003C4D RID: 15437
		private Control _controlToLayout;

		// Token: 0x04003C4E RID: 15438
		private bool _resumeLayout;
	}
}
