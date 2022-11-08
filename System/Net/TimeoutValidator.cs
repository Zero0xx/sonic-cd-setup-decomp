using System;
using System.Configuration;

namespace System.Net
{
	// Token: 0x02000667 RID: 1639
	internal sealed class TimeoutValidator : ConfigurationValidatorBase
	{
		// Token: 0x060032B4 RID: 12980 RVA: 0x000D715E File Offset: 0x000D615E
		internal TimeoutValidator(bool zeroValid)
		{
			this._zeroValid = zeroValid;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000D716D File Offset: 0x000D616D
		public override bool CanValidate(Type type)
		{
			return type == typeof(int) || type == typeof(long);
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000D718C File Offset: 0x000D618C
		public override void Validate(object value)
		{
			if (value == null)
			{
				return;
			}
			int num = (int)value;
			if (this._zeroValid && num == 0)
			{
				return;
			}
			if (num <= 0 && num != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_io_timeout_use_gt_zero"));
			}
		}

		// Token: 0x04002F61 RID: 12129
		private bool _zeroValid;
	}
}
