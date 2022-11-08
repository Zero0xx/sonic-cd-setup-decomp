using System;
using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
	// Token: 0x020002AF RID: 687
	internal static class Assert
	{
		// Token: 0x06001B05 RID: 6917 RVA: 0x00046CAE File Offset: 0x00045CAE
		static Assert()
		{
			Assert.AddFilter(new DefaultFilter());
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00046CBC File Offset: 0x00045CBC
		public static void AddFilter(AssertFilter filter)
		{
			if (Assert.iFilterArraySize <= Assert.iNumOfFilters)
			{
				AssertFilter[] array = new AssertFilter[Assert.iFilterArraySize + 2];
				if (Assert.iNumOfFilters > 0)
				{
					Array.Copy(Assert.ListOfFilters, array, Assert.iNumOfFilters);
				}
				Assert.iFilterArraySize += 2;
				Assert.ListOfFilters = array;
			}
			Assert.ListOfFilters[Assert.iNumOfFilters++] = filter;
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x00046D20 File Offset: 0x00045D20
		public static void Check(bool condition, string conditionString, string message)
		{
			if (!condition)
			{
				Assert.Fail(conditionString, message);
			}
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x00046D2C File Offset: 0x00045D2C
		public static void Fail(string conditionString, string message)
		{
			StackTrace location = new StackTrace();
			int i = Assert.iNumOfFilters;
			while (i > 0)
			{
				AssertFilters assertFilters = Assert.ListOfFilters[--i].AssertFailure(conditionString, message, location);
				if (assertFilters == AssertFilters.FailDebug)
				{
					if (Debugger.IsAttached)
					{
						Debugger.Break();
						return;
					}
					if (!Debugger.Launch())
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DebuggerLaunchFailed"));
					}
					break;
				}
				else if (assertFilters == AssertFilters.FailTerminate)
				{
					Environment.Exit(-1);
				}
				else if (assertFilters == AssertFilters.FailIgnore)
				{
					return;
				}
			}
		}

		// Token: 0x06001B09 RID: 6921
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ShowDefaultAssertDialog(string conditionString, string message);

		// Token: 0x04000A49 RID: 2633
		private static AssertFilter[] ListOfFilters;

		// Token: 0x04000A4A RID: 2634
		private static int iNumOfFilters;

		// Token: 0x04000A4B RID: 2635
		private static int iFilterArraySize;
	}
}
