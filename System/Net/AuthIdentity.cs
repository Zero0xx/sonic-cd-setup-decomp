using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004F7 RID: 1271
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct AuthIdentity
	{
		// Token: 0x060027CF RID: 10191 RVA: 0x000A4648 File Offset: 0x000A3648
		internal AuthIdentity(string userName, string password, string domain)
		{
			this.UserName = userName;
			this.UserNameLength = ((userName == null) ? 0 : userName.Length);
			this.Password = password;
			this.PasswordLength = ((password == null) ? 0 : password.Length);
			this.Domain = domain;
			this.DomainLength = ((domain == null) ? 0 : domain.Length);
			this.Flags = (ComNetOS.IsWin9x ? 1 : 2);
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000A46B1 File Offset: 0x000A36B1
		public override string ToString()
		{
			return ValidationHelper.ToString(this.Domain) + "\\" + ValidationHelper.ToString(this.UserName);
		}

		// Token: 0x040026FE RID: 9982
		internal string UserName;

		// Token: 0x040026FF RID: 9983
		internal int UserNameLength;

		// Token: 0x04002700 RID: 9984
		internal string Domain;

		// Token: 0x04002701 RID: 9985
		internal int DomainLength;

		// Token: 0x04002702 RID: 9986
		internal string Password;

		// Token: 0x04002703 RID: 9987
		internal int PasswordLength;

		// Token: 0x04002704 RID: 9988
		internal int Flags;
	}
}
