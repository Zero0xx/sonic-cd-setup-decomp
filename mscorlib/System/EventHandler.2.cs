using System;

namespace System
{
	// Token: 0x020000B8 RID: 184
	// (Invoke) Token: 0x06000AAC RID: 2732
	[Serializable]
	public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;
}
