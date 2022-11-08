using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200054D RID: 1357
	internal static class TimerThread
	{
		// Token: 0x06002930 RID: 10544 RVA: 0x000AC598 File Offset: 0x000AB598
		static TimerThread()
		{
			TimerThread.s_ThreadEvents = new WaitHandle[]
			{
				TimerThread.s_ThreadShutdownEvent,
				TimerThread.s_ThreadReadyEvent
			};
			AppDomain.CurrentDomain.DomainUnload += TimerThread.OnDomainUnload;
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000AC614 File Offset: 0x000AB614
		internal static TimerThread.Queue CreateQueue(int durationMilliseconds)
		{
			if (durationMilliseconds == -1)
			{
				return new TimerThread.InfiniteTimerQueue();
			}
			if (durationMilliseconds < 0)
			{
				throw new ArgumentOutOfRangeException("durationMilliseconds");
			}
			TimerThread.TimerQueue timerQueue;
			lock (TimerThread.s_NewQueues)
			{
				timerQueue = new TimerThread.TimerQueue(durationMilliseconds);
				WeakReference value = new WeakReference(timerQueue);
				TimerThread.s_NewQueues.AddLast(value);
			}
			return timerQueue;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000AC67C File Offset: 0x000AB67C
		internal static TimerThread.Queue GetOrCreateQueue(int durationMilliseconds)
		{
			if (durationMilliseconds == -1)
			{
				return new TimerThread.InfiniteTimerQueue();
			}
			if (durationMilliseconds < 0)
			{
				throw new ArgumentOutOfRangeException("durationMilliseconds");
			}
			WeakReference weakReference = (WeakReference)TimerThread.s_QueuesCache[durationMilliseconds];
			TimerThread.TimerQueue timerQueue;
			if (weakReference == null || (timerQueue = (TimerThread.TimerQueue)weakReference.Target) == null)
			{
				lock (TimerThread.s_NewQueues)
				{
					weakReference = (WeakReference)TimerThread.s_QueuesCache[durationMilliseconds];
					if (weakReference == null || (timerQueue = (TimerThread.TimerQueue)weakReference.Target) == null)
					{
						timerQueue = new TimerThread.TimerQueue(durationMilliseconds);
						weakReference = new WeakReference(timerQueue);
						TimerThread.s_NewQueues.AddLast(weakReference);
						TimerThread.s_QueuesCache[durationMilliseconds] = weakReference;
						if (++TimerThread.s_CacheScanIteration % 32 == 0)
						{
							List<int> list = new List<int>();
							foreach (object obj2 in TimerThread.s_QueuesCache)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
								if (((WeakReference)dictionaryEntry.Value).Target == null)
								{
									list.Add((int)dictionaryEntry.Key);
								}
							}
							for (int i = 0; i < list.Count; i++)
							{
								TimerThread.s_QueuesCache.Remove(list[i]);
							}
						}
					}
				}
			}
			return timerQueue;
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000AC800 File Offset: 0x000AB800
		private static void Prod()
		{
			TimerThread.s_ThreadReadyEvent.Set();
			if (Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 1, 0) == 0)
			{
				new Thread(new ThreadStart(TimerThread.ThreadProc)).Start();
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000AC840 File Offset: 0x000AB840
		private static void ThreadProc()
		{
			Thread.CurrentThread.IsBackground = true;
			lock (TimerThread.s_Queues)
			{
				if (Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 1, 1) == 1)
				{
					bool flag = true;
					while (flag)
					{
						try
						{
							TimerThread.s_ThreadReadyEvent.Reset();
							for (;;)
							{
								if (TimerThread.s_NewQueues.Count > 0)
								{
									lock (TimerThread.s_NewQueues)
									{
										for (LinkedListNode<WeakReference> first = TimerThread.s_NewQueues.First; first != null; first = TimerThread.s_NewQueues.First)
										{
											TimerThread.s_NewQueues.Remove(first);
											TimerThread.s_Queues.AddLast(first);
										}
									}
								}
								int tickCount = Environment.TickCount;
								int num = 0;
								bool flag2 = false;
								LinkedListNode<WeakReference> linkedListNode = TimerThread.s_Queues.First;
								while (linkedListNode != null)
								{
									TimerThread.TimerQueue timerQueue = (TimerThread.TimerQueue)linkedListNode.Value.Target;
									if (timerQueue == null)
									{
										LinkedListNode<WeakReference> next = linkedListNode.Next;
										TimerThread.s_Queues.Remove(linkedListNode);
										linkedListNode = next;
									}
									else
									{
										int num2;
										if (timerQueue.Fire(out num2) && (!flag2 || TimerThread.IsTickBetween(tickCount, num, num2)))
										{
											num = num2;
											flag2 = true;
										}
										linkedListNode = linkedListNode.Next;
									}
								}
								int tickCount2 = Environment.TickCount;
								int millisecondsTimeout = (int)(flag2 ? (TimerThread.IsTickBetween(tickCount, num, tickCount2) ? (Math.Min((uint)(num - tickCount2), 2147483632U) + 15U) : 0U) : 30000U);
								int num3 = WaitHandle.WaitAny(TimerThread.s_ThreadEvents, millisecondsTimeout, false);
								if (num3 == 0)
								{
									break;
								}
								if (num3 == 258 && !flag2)
								{
									Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 0, 1);
									if (!TimerThread.s_ThreadReadyEvent.WaitOne(0, false) || Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 1, 0) != 0)
									{
										goto IL_194;
									}
								}
							}
							flag = false;
							continue;
							IL_194:
							flag = false;
						}
						catch (Exception ex)
						{
							if (NclUtilities.IsFatal(ex))
							{
								throw;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.Web, "TimerThread#" + Thread.CurrentThread.ManagedThreadId.ToString(NumberFormatInfo.InvariantInfo) + "::ThreadProc() - Exception:" + ex.ToString());
							}
							Thread.Sleep(1000);
						}
					}
				}
			}
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000ACA9C File Offset: 0x000ABA9C
		private static void StopTimerThread()
		{
			Interlocked.Exchange(ref TimerThread.s_ThreadState, 2);
			TimerThread.s_ThreadShutdownEvent.Set();
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000ACAB5 File Offset: 0x000ABAB5
		private static bool IsTickBetween(int start, int end, int comparand)
		{
			return start <= comparand == end <= comparand != start <= end;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000ACAD4 File Offset: 0x000ABAD4
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			try
			{
				TimerThread.StopTimerThread();
			}
			catch
			{
			}
		}

		// Token: 0x04002847 RID: 10311
		private const int c_ThreadIdleTimeoutMilliseconds = 30000;

		// Token: 0x04002848 RID: 10312
		private const int c_CacheScanPerIterations = 32;

		// Token: 0x04002849 RID: 10313
		private const int c_TickCountResolution = 15;

		// Token: 0x0400284A RID: 10314
		private static LinkedList<WeakReference> s_Queues = new LinkedList<WeakReference>();

		// Token: 0x0400284B RID: 10315
		private static LinkedList<WeakReference> s_NewQueues = new LinkedList<WeakReference>();

		// Token: 0x0400284C RID: 10316
		private static int s_ThreadState = 0;

		// Token: 0x0400284D RID: 10317
		private static AutoResetEvent s_ThreadReadyEvent = new AutoResetEvent(false);

		// Token: 0x0400284E RID: 10318
		private static ManualResetEvent s_ThreadShutdownEvent = new ManualResetEvent(false);

		// Token: 0x0400284F RID: 10319
		private static WaitHandle[] s_ThreadEvents;

		// Token: 0x04002850 RID: 10320
		private static int s_CacheScanIteration;

		// Token: 0x04002851 RID: 10321
		private static Hashtable s_QueuesCache = new Hashtable();

		// Token: 0x0200054E RID: 1358
		internal abstract class Queue
		{
			// Token: 0x06002938 RID: 10552 RVA: 0x000ACAFC File Offset: 0x000ABAFC
			internal Queue(int durationMilliseconds)
			{
				this.m_DurationMilliseconds = durationMilliseconds;
			}

			// Token: 0x17000868 RID: 2152
			// (get) Token: 0x06002939 RID: 10553 RVA: 0x000ACB0B File Offset: 0x000ABB0B
			internal int Duration
			{
				get
				{
					return this.m_DurationMilliseconds;
				}
			}

			// Token: 0x0600293A RID: 10554 RVA: 0x000ACB13 File Offset: 0x000ABB13
			internal TimerThread.Timer CreateTimer()
			{
				return this.CreateTimer(null, null);
			}

			// Token: 0x0600293B RID: 10555
			internal abstract TimerThread.Timer CreateTimer(TimerThread.Callback callback, object context);

			// Token: 0x04002852 RID: 10322
			private readonly int m_DurationMilliseconds;
		}

		// Token: 0x0200054F RID: 1359
		internal abstract class Timer : IDisposable
		{
			// Token: 0x0600293C RID: 10556 RVA: 0x000ACB1D File Offset: 0x000ABB1D
			internal Timer(int durationMilliseconds)
			{
				this.m_DurationMilliseconds = durationMilliseconds;
				this.m_StartTimeMilliseconds = Environment.TickCount;
			}

			// Token: 0x17000869 RID: 2153
			// (get) Token: 0x0600293D RID: 10557 RVA: 0x000ACB37 File Offset: 0x000ABB37
			internal int Duration
			{
				get
				{
					return this.m_DurationMilliseconds;
				}
			}

			// Token: 0x1700086A RID: 2154
			// (get) Token: 0x0600293E RID: 10558 RVA: 0x000ACB3F File Offset: 0x000ABB3F
			internal int StartTime
			{
				get
				{
					return this.m_StartTimeMilliseconds;
				}
			}

			// Token: 0x1700086B RID: 2155
			// (get) Token: 0x0600293F RID: 10559 RVA: 0x000ACB47 File Offset: 0x000ABB47
			internal int Expiration
			{
				get
				{
					return this.m_StartTimeMilliseconds + this.m_DurationMilliseconds;
				}
			}

			// Token: 0x1700086C RID: 2156
			// (get) Token: 0x06002940 RID: 10560 RVA: 0x000ACB58 File Offset: 0x000ABB58
			internal int TimeRemaining
			{
				get
				{
					if (this.HasExpired)
					{
						return 0;
					}
					if (this.Duration == -1)
					{
						return -1;
					}
					int tickCount = Environment.TickCount;
					int num = (int)(TimerThread.IsTickBetween(this.StartTime, this.Expiration, tickCount) ? Math.Min((uint)(this.Expiration - tickCount), 2147483647U) : 0U);
					if (num >= 2)
					{
						return num;
					}
					return num + 1;
				}
			}

			// Token: 0x06002941 RID: 10561
			internal abstract bool Cancel();

			// Token: 0x1700086D RID: 2157
			// (get) Token: 0x06002942 RID: 10562
			internal abstract bool HasExpired { get; }

			// Token: 0x06002943 RID: 10563 RVA: 0x000ACBB3 File Offset: 0x000ABBB3
			public void Dispose()
			{
				this.Cancel();
			}

			// Token: 0x04002853 RID: 10323
			private readonly int m_StartTimeMilliseconds;

			// Token: 0x04002854 RID: 10324
			private readonly int m_DurationMilliseconds;
		}

		// Token: 0x02000550 RID: 1360
		// (Invoke) Token: 0x06002945 RID: 10565
		internal delegate void Callback(TimerThread.Timer timer, int timeNoticed, object context);

		// Token: 0x02000551 RID: 1361
		private enum TimerThreadState
		{
			// Token: 0x04002856 RID: 10326
			Idle,
			// Token: 0x04002857 RID: 10327
			Running,
			// Token: 0x04002858 RID: 10328
			Stopped
		}

		// Token: 0x02000552 RID: 1362
		private class TimerQueue : TimerThread.Queue
		{
			// Token: 0x06002948 RID: 10568 RVA: 0x000ACBBC File Offset: 0x000ABBBC
			internal TimerQueue(int durationMilliseconds) : base(durationMilliseconds)
			{
				this.m_Timers = new TimerThread.TimerNode();
				this.m_Timers.Next = this.m_Timers;
				this.m_Timers.Prev = this.m_Timers;
			}

			// Token: 0x06002949 RID: 10569 RVA: 0x000ACBF4 File Offset: 0x000ABBF4
			internal override TimerThread.Timer CreateTimer(TimerThread.Callback callback, object context)
			{
				TimerThread.TimerNode timerNode = new TimerThread.TimerNode(callback, context, base.Duration, this.m_Timers);
				bool flag = false;
				lock (this.m_Timers)
				{
					if (this.m_Timers.Next == this.m_Timers)
					{
						if (this.m_ThisHandle == IntPtr.Zero)
						{
							this.m_ThisHandle = (IntPtr)GCHandle.Alloc(this);
						}
						flag = true;
					}
					timerNode.Next = this.m_Timers;
					timerNode.Prev = this.m_Timers.Prev;
					this.m_Timers.Prev.Next = timerNode;
					this.m_Timers.Prev = timerNode;
				}
				if (flag)
				{
					TimerThread.Prod();
				}
				return timerNode;
			}

			// Token: 0x0600294A RID: 10570 RVA: 0x000ACCB8 File Offset: 0x000ABCB8
			internal bool Fire(out int nextExpiration)
			{
				TimerThread.TimerNode next;
				do
				{
					next = this.m_Timers.Next;
					if (next == this.m_Timers)
					{
						lock (this.m_Timers)
						{
							next = this.m_Timers.Next;
							if (next == this.m_Timers)
							{
								if (this.m_ThisHandle != IntPtr.Zero)
								{
									((GCHandle)this.m_ThisHandle).Free();
									this.m_ThisHandle = IntPtr.Zero;
								}
								nextExpiration = 0;
								return false;
							}
						}
					}
				}
				while (next.Fire());
				nextExpiration = next.Expiration;
				return true;
			}

			// Token: 0x04002859 RID: 10329
			private IntPtr m_ThisHandle;

			// Token: 0x0400285A RID: 10330
			private readonly TimerThread.TimerNode m_Timers;
		}

		// Token: 0x02000553 RID: 1363
		private class InfiniteTimerQueue : TimerThread.Queue
		{
			// Token: 0x0600294B RID: 10571 RVA: 0x000ACD60 File Offset: 0x000ABD60
			internal InfiniteTimerQueue() : base(-1)
			{
			}

			// Token: 0x0600294C RID: 10572 RVA: 0x000ACD69 File Offset: 0x000ABD69
			internal override TimerThread.Timer CreateTimer(TimerThread.Callback callback, object context)
			{
				return new TimerThread.InfiniteTimer();
			}
		}

		// Token: 0x02000554 RID: 1364
		private class TimerNode : TimerThread.Timer
		{
			// Token: 0x0600294D RID: 10573 RVA: 0x000ACD70 File Offset: 0x000ABD70
			internal TimerNode(TimerThread.Callback callback, object context, int durationMilliseconds, object queueLock) : base(durationMilliseconds)
			{
				if (callback != null)
				{
					this.m_Callback = callback;
					this.m_Context = context;
				}
				this.m_TimerState = TimerThread.TimerNode.TimerState.Ready;
				this.m_QueueLock = queueLock;
			}

			// Token: 0x0600294E RID: 10574 RVA: 0x000ACD99 File Offset: 0x000ABD99
			internal TimerNode() : base(0)
			{
				this.m_TimerState = TimerThread.TimerNode.TimerState.Sentinel;
			}

			// Token: 0x1700086E RID: 2158
			// (get) Token: 0x0600294F RID: 10575 RVA: 0x000ACDA9 File Offset: 0x000ABDA9
			internal override bool HasExpired
			{
				get
				{
					return this.m_TimerState == TimerThread.TimerNode.TimerState.Fired;
				}
			}

			// Token: 0x1700086F RID: 2159
			// (get) Token: 0x06002950 RID: 10576 RVA: 0x000ACDB4 File Offset: 0x000ABDB4
			// (set) Token: 0x06002951 RID: 10577 RVA: 0x000ACDBC File Offset: 0x000ABDBC
			internal TimerThread.TimerNode Next
			{
				get
				{
					return this.next;
				}
				set
				{
					this.next = value;
				}
			}

			// Token: 0x17000870 RID: 2160
			// (get) Token: 0x06002952 RID: 10578 RVA: 0x000ACDC5 File Offset: 0x000ABDC5
			// (set) Token: 0x06002953 RID: 10579 RVA: 0x000ACDCD File Offset: 0x000ABDCD
			internal TimerThread.TimerNode Prev
			{
				get
				{
					return this.prev;
				}
				set
				{
					this.prev = value;
				}
			}

			// Token: 0x06002954 RID: 10580 RVA: 0x000ACDD8 File Offset: 0x000ABDD8
			internal override bool Cancel()
			{
				if (this.m_TimerState == TimerThread.TimerNode.TimerState.Ready)
				{
					lock (this.m_QueueLock)
					{
						if (this.m_TimerState == TimerThread.TimerNode.TimerState.Ready)
						{
							this.Next.Prev = this.Prev;
							this.Prev.Next = this.Next;
							this.Next = null;
							this.Prev = null;
							this.m_Callback = null;
							this.m_Context = null;
							this.m_TimerState = TimerThread.TimerNode.TimerState.Cancelled;
							return true;
						}
					}
					return false;
				}
				return false;
			}

			// Token: 0x06002955 RID: 10581 RVA: 0x000ACE68 File Offset: 0x000ABE68
			internal bool Fire()
			{
				if (this.m_TimerState != TimerThread.TimerNode.TimerState.Ready)
				{
					return true;
				}
				int tickCount = Environment.TickCount;
				if (TimerThread.IsTickBetween(base.StartTime, base.Expiration, tickCount))
				{
					return false;
				}
				bool flag = false;
				lock (this.m_QueueLock)
				{
					if (this.m_TimerState == TimerThread.TimerNode.TimerState.Ready)
					{
						this.m_TimerState = TimerThread.TimerNode.TimerState.Fired;
						this.Next.Prev = this.Prev;
						this.Prev.Next = this.Next;
						this.Next = null;
						this.Prev = null;
						flag = (this.m_Callback != null);
					}
				}
				if (flag)
				{
					try
					{
						TimerThread.Callback callback = this.m_Callback;
						object context = this.m_Context;
						this.m_Callback = null;
						this.m_Context = null;
						callback(this, tickCount, context);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						if (Logging.On)
						{
							Logging.PrintError(Logging.Web, "TimerThreadTimer#" + base.StartTime.ToString(NumberFormatInfo.InvariantInfo) + "::Fire() - " + SR.GetString("net_log_exception_in_callback", new object[]
							{
								ex
							}));
						}
					}
				}
				return true;
			}

			// Token: 0x0400285B RID: 10331
			private TimerThread.TimerNode.TimerState m_TimerState;

			// Token: 0x0400285C RID: 10332
			private TimerThread.Callback m_Callback;

			// Token: 0x0400285D RID: 10333
			private object m_Context;

			// Token: 0x0400285E RID: 10334
			private object m_QueueLock;

			// Token: 0x0400285F RID: 10335
			private TimerThread.TimerNode next;

			// Token: 0x04002860 RID: 10336
			private TimerThread.TimerNode prev;

			// Token: 0x02000555 RID: 1365
			private enum TimerState
			{
				// Token: 0x04002862 RID: 10338
				Ready,
				// Token: 0x04002863 RID: 10339
				Fired,
				// Token: 0x04002864 RID: 10340
				Cancelled,
				// Token: 0x04002865 RID: 10341
				Sentinel
			}
		}

		// Token: 0x02000556 RID: 1366
		private class InfiniteTimer : TimerThread.Timer
		{
			// Token: 0x06002956 RID: 10582 RVA: 0x000ACFA8 File Offset: 0x000ABFA8
			internal InfiniteTimer() : base(-1)
			{
			}

			// Token: 0x17000871 RID: 2161
			// (get) Token: 0x06002957 RID: 10583 RVA: 0x000ACFB1 File Offset: 0x000ABFB1
			internal override bool HasExpired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002958 RID: 10584 RVA: 0x000ACFB4 File Offset: 0x000ABFB4
			internal override bool Cancel()
			{
				return Interlocked.Exchange(ref this.cancelled, 1) == 0;
			}

			// Token: 0x04002866 RID: 10342
			private int cancelled;
		}
	}
}
