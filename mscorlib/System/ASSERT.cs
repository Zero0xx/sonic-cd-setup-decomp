using System;
using System.Diagnostics;

namespace System
{
	// Token: 0x02000106 RID: 262
	internal sealed class ASSERT : Exception
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x0002C2E8 File Offset: 0x0002B2E8
		private static bool AssertIsFriend(Type[] friends, StackTrace st)
		{
			Type declaringType = st.GetFrame(1).GetMethod().DeclaringType;
			Type declaringType2 = st.GetFrame(2).GetMethod().DeclaringType;
			bool flag = true;
			foreach (Type type in friends)
			{
				if (declaringType2 != type && declaringType2 != declaringType)
				{
					flag = false;
				}
			}
			if (flag)
			{
				ASSERT.Assert(false, Environment.GetResourceString("RtType.InvalidCaller"), st.ToString());
			}
			return true;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0002C35C File Offset: 0x0002B35C
		[Conditional("_DEBUG")]
		internal static void FRIEND(Type[] friends)
		{
			StackTrace st = new StackTrace();
			ASSERT.AssertIsFriend(friends, st);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0002C378 File Offset: 0x0002B378
		[Conditional("_DEBUG")]
		internal static void FRIEND(Type friend)
		{
			StackTrace st = new StackTrace();
			ASSERT.AssertIsFriend(new Type[]
			{
				friend
			}, st);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0002C3A0 File Offset: 0x0002B3A0
		[Conditional("_DEBUG")]
		internal static void FRIEND(string ns)
		{
			StackTrace stackTrace = new StackTrace();
			string @namespace = stackTrace.GetFrame(1).GetMethod().DeclaringType.Namespace;
			string namespace2 = stackTrace.GetFrame(2).GetMethod().DeclaringType.Namespace;
			ASSERT.Assert(namespace2.Equals(namespace2) || namespace2.Equals(ns), Environment.GetResourceString("RtType.InvalidCaller"), stackTrace.ToString());
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0002C409 File Offset: 0x0002B409
		[Conditional("_DEBUG")]
		internal static void PRECONDITION(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0002C411 File Offset: 0x0002B411
		[Conditional("_DEBUG")]
		internal static void PRECONDITION(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0002C41A File Offset: 0x0002B41A
		[Conditional("_DEBUG")]
		internal static void PRECONDITION(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0002C424 File Offset: 0x0002B424
		[Conditional("_DEBUG")]
		internal static void POSTCONDITION(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0002C42C File Offset: 0x0002B42C
		[Conditional("_DEBUG")]
		internal static void POSTCONDITION(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0002C435 File Offset: 0x0002B435
		[Conditional("_DEBUG")]
		internal static void POSTCONDITION(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0002C43F File Offset: 0x0002B43F
		[Conditional("_DEBUG")]
		internal static void CONSISTENCY_CHECK(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0002C447 File Offset: 0x0002B447
		[Conditional("_DEBUG")]
		internal static void CONSISTENCY_CHECK(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0002C450 File Offset: 0x0002B450
		[Conditional("_DEBUG")]
		internal static void CONSISTENCY_CHECK(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0002C45A File Offset: 0x0002B45A
		[Conditional("_DEBUG")]
		internal static void SIMPLIFYING_ASSUMPTION(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0002C462 File Offset: 0x0002B462
		[Conditional("_DEBUG")]
		internal static void SIMPLIFYING_ASSUMPTION(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0002C46B File Offset: 0x0002B46B
		[Conditional("_DEBUG")]
		internal static void SIMPLIFYING_ASSUMPTION(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0002C475 File Offset: 0x0002B475
		[Conditional("_DEBUG")]
		internal static void UNREACHABLE()
		{
			ASSERT.Assert();
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0002C47C File Offset: 0x0002B47C
		[Conditional("_DEBUG")]
		internal static void UNREACHABLE(string message)
		{
			ASSERT.Assert(message);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0002C484 File Offset: 0x0002B484
		[Conditional("_DEBUG")]
		internal static void UNREACHABLE(string message, string detailedMessage)
		{
			ASSERT.Assert(message, detailedMessage);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0002C48D File Offset: 0x0002B48D
		[Conditional("_DEBUG")]
		internal static void NOT_IMPLEMENTED()
		{
			ASSERT.Assert();
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0002C494 File Offset: 0x0002B494
		[Conditional("_DEBUG")]
		internal static void NOT_IMPLEMENTED(string message)
		{
			ASSERT.Assert(message);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0002C49C File Offset: 0x0002B49C
		[Conditional("_DEBUG")]
		internal static void NOT_IMPLEMENTED(string message, string detailedMessage)
		{
			ASSERT.Assert(message, detailedMessage);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0002C4A5 File Offset: 0x0002B4A5
		private static void Assert()
		{
			ASSERT.Assert(false, null, null);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0002C4AF File Offset: 0x0002B4AF
		private static void Assert(string message)
		{
			ASSERT.Assert(false, message, null);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0002C4B9 File Offset: 0x0002B4B9
		private static void Assert(bool condition)
		{
			ASSERT.Assert(condition, null, null);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0002C4C3 File Offset: 0x0002B4C3
		private static void Assert(bool condition, string message)
		{
			ASSERT.Assert(condition, message, null);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0002C4CD File Offset: 0x0002B4CD
		private static void Assert(string message, string detailedMessage)
		{
			ASSERT.Assert(false, message, detailedMessage);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0002C4D7 File Offset: 0x0002B4D7
		private static void Assert(bool condition, string message, string detailedMessage)
		{
		}
	}
}
