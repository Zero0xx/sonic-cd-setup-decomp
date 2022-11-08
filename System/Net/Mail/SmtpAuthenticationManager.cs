using System;
using System.Collections;

namespace System.Net.Mail
{
	// Token: 0x020006B6 RID: 1718
	internal static class SmtpAuthenticationManager
	{
		// Token: 0x0600350D RID: 13581 RVA: 0x000E192E File Offset: 0x000E092E
		static SmtpAuthenticationManager()
		{
			if (ComNetOS.IsWin2K)
			{
				SmtpAuthenticationManager.Register(new SmtpNegotiateAuthenticationModule());
			}
			SmtpAuthenticationManager.Register(new SmtpNtlmAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpDigestAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpLoginAuthenticationModule());
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000E196C File Offset: 0x000E096C
		internal static void Register(ISmtpAuthenticationModule module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			lock (SmtpAuthenticationManager.modules)
			{
				SmtpAuthenticationManager.modules.Add(module);
			}
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000E19B8 File Offset: 0x000E09B8
		internal static ISmtpAuthenticationModule[] GetModules()
		{
			ISmtpAuthenticationModule[] result;
			lock (SmtpAuthenticationManager.modules)
			{
				ISmtpAuthenticationModule[] array = new ISmtpAuthenticationModule[SmtpAuthenticationManager.modules.Count];
				SmtpAuthenticationManager.modules.CopyTo(0, array, 0, SmtpAuthenticationManager.modules.Count);
				result = array;
			}
			return result;
		}

		// Token: 0x040030A7 RID: 12455
		private static ArrayList modules = new ArrayList();
	}
}
