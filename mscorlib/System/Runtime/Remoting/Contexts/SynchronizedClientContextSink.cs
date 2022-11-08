using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200079D RID: 1949
	internal class SynchronizedClientContextSink : InternalSink, IMessageSink
	{
		// Token: 0x0600457A RID: 17786 RVA: 0x000EC5E2 File Offset: 0x000EB5E2
		internal SynchronizedClientContextSink(SynchronizationAttribute prop, IMessageSink nextSink)
		{
			this._property = prop;
			this._nextSink = nextSink;
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x000EC5F8 File Offset: 0x000EB5F8
		~SynchronizedClientContextSink()
		{
			this._property.Dispose();
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x000EC62C File Offset: 0x000EB62C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message;
			if (this._property.IsReEntrant)
			{
				this._property.HandleThreadExit();
				message = this._nextSink.SyncProcessMessage(reqMsg);
				this._property.HandleThreadReEntry();
			}
			else
			{
				LogicalCallContext logicalCallContext = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
				string text = logicalCallContext.RemotingData.LogicalCallID;
				bool flag = false;
				if (text == null)
				{
					text = Identity.GetNewLogicalCallID();
					logicalCallContext.RemotingData.LogicalCallID = text;
					flag = true;
				}
				bool flag2 = false;
				if (this._property.SyncCallOutLCID == null)
				{
					this._property.SyncCallOutLCID = text;
					flag2 = true;
				}
				message = this._nextSink.SyncProcessMessage(reqMsg);
				if (flag2)
				{
					this._property.SyncCallOutLCID = null;
					if (flag)
					{
						LogicalCallContext logicalCallContext2 = (LogicalCallContext)message.Properties[Message.CallContextKey];
						logicalCallContext2.RemotingData.LogicalCallID = null;
					}
				}
			}
			return message;
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x000EC710 File Offset: 0x000EB710
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			if (!this._property.IsReEntrant)
			{
				LogicalCallContext logicalCallContext = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
				string newLogicalCallID = Identity.GetNewLogicalCallID();
				logicalCallContext.RemotingData.LogicalCallID = newLogicalCallID;
				this._property.AsyncCallOutLCIDList.Add(newLogicalCallID);
			}
			SynchronizedClientContextSink.AsyncReplySink replySink2 = new SynchronizedClientContextSink.AsyncReplySink(replySink, this._property);
			return this._nextSink.AsyncProcessMessage(reqMsg, replySink2);
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x0600457E RID: 17790 RVA: 0x000EC782 File Offset: 0x000EB782
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x0400228C RID: 8844
		internal IMessageSink _nextSink;

		// Token: 0x0400228D RID: 8845
		internal SynchronizationAttribute _property;

		// Token: 0x0200079E RID: 1950
		internal class AsyncReplySink : IMessageSink
		{
			// Token: 0x0600457F RID: 17791 RVA: 0x000EC78A File Offset: 0x000EB78A
			internal AsyncReplySink(IMessageSink nextSink, SynchronizationAttribute prop)
			{
				this._nextSink = nextSink;
				this._property = prop;
			}

			// Token: 0x06004580 RID: 17792 RVA: 0x000EC7A0 File Offset: 0x000EB7A0
			public virtual IMessage SyncProcessMessage(IMessage reqMsg)
			{
				WorkItem workItem = new WorkItem(reqMsg, this._nextSink, null);
				this._property.HandleWorkRequest(workItem);
				if (!this._property.IsReEntrant)
				{
					this._property.AsyncCallOutLCIDList.Remove(((LogicalCallContext)reqMsg.Properties[Message.CallContextKey]).RemotingData.LogicalCallID);
				}
				return workItem.ReplyMessage;
			}

			// Token: 0x06004581 RID: 17793 RVA: 0x000EC809 File Offset: 0x000EB809
			public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000C37 RID: 3127
			// (get) Token: 0x06004582 RID: 17794 RVA: 0x000EC810 File Offset: 0x000EB810
			public IMessageSink NextSink
			{
				get
				{
					return this._nextSink;
				}
			}

			// Token: 0x0400228E RID: 8846
			internal IMessageSink _nextSink;

			// Token: 0x0400228F RID: 8847
			internal SynchronizationAttribute _property;
		}
	}
}
