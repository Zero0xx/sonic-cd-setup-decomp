using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000768 RID: 1896
	internal class AgileAsyncWorkerItem
	{
		// Token: 0x06004360 RID: 17248 RVA: 0x000E639D File Offset: 0x000E539D
		public AgileAsyncWorkerItem(IMethodCallMessage message, AsyncResult ar, object target)
		{
			this._message = new MethodCall(message);
			this._ar = ar;
			this._target = target;
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x000E63BF File Offset: 0x000E53BF
		public static void ThreadPoolCallBack(object o)
		{
			((AgileAsyncWorkerItem)o).DoAsyncCall();
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x000E63CC File Offset: 0x000E53CC
		public void DoAsyncCall()
		{
			new StackBuilderSink(this._target).AsyncProcessMessage(this._message, this._ar);
		}

		// Token: 0x040021DD RID: 8669
		private IMethodCallMessage _message;

		// Token: 0x040021DE RID: 8670
		private AsyncResult _ar;

		// Token: 0x040021DF RID: 8671
		private object _target;
	}
}
