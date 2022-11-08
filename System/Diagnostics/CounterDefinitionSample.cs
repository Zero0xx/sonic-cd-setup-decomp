using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200076A RID: 1898
	internal class CounterDefinitionSample
	{
		// Token: 0x06003A70 RID: 14960 RVA: 0x000F8C7C File Offset: 0x000F7C7C
		internal CounterDefinitionSample(NativeMethods.PERF_COUNTER_DEFINITION perfCounter, CategorySample categorySample, int instanceNumber)
		{
			this.NameIndex = perfCounter.CounterNameTitleIndex;
			this.CounterType = perfCounter.CounterType;
			this.offset = perfCounter.CounterOffset;
			this.size = perfCounter.CounterSize;
			if (instanceNumber == -1)
			{
				this.instanceValues = new long[1];
			}
			else
			{
				this.instanceValues = new long[instanceNumber];
			}
			this.categorySample = categorySample;
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000F8CE4 File Offset: 0x000F7CE4
		private long ReadValue(IntPtr pointer)
		{
			if (this.size == 4)
			{
				return (long)((ulong)Marshal.ReadInt32((IntPtr)((long)pointer + (long)this.offset)));
			}
			if (this.size == 8)
			{
				return Marshal.ReadInt64((IntPtr)((long)pointer + (long)this.offset));
			}
			return -1L;
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000F8D38 File Offset: 0x000F7D38
		internal CounterSample GetInstanceValue(string instanceName)
		{
			if (!this.categorySample.InstanceNameTable.ContainsKey(instanceName))
			{
				if (instanceName.Length > 127)
				{
					instanceName = instanceName.Substring(0, 127);
				}
				if (!this.categorySample.InstanceNameTable.ContainsKey(instanceName))
				{
					throw new InvalidOperationException(SR.GetString("CantReadInstance", new object[]
					{
						instanceName
					}));
				}
			}
			int num = (int)this.categorySample.InstanceNameTable[instanceName];
			long rawValue = this.instanceValues[num];
			long baseValue = 0L;
			if (this.BaseCounterDefinitionSample != null)
			{
				CategorySample categorySample = this.BaseCounterDefinitionSample.categorySample;
				int num2 = (int)categorySample.InstanceNameTable[instanceName];
				baseValue = this.BaseCounterDefinitionSample.instanceValues[num2];
			}
			return new CounterSample(rawValue, baseValue, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x000F8E3C File Offset: 0x000F7E3C
		internal InstanceDataCollection ReadInstanceData(string counterName)
		{
			InstanceDataCollection instanceDataCollection = new InstanceDataCollection(counterName);
			string[] array = new string[this.categorySample.InstanceNameTable.Count];
			this.categorySample.InstanceNameTable.Keys.CopyTo(array, 0);
			int[] array2 = new int[this.categorySample.InstanceNameTable.Count];
			this.categorySample.InstanceNameTable.Values.CopyTo(array2, 0);
			for (int i = 0; i < array.Length; i++)
			{
				long baseValue = 0L;
				if (this.BaseCounterDefinitionSample != null)
				{
					CategorySample categorySample = this.BaseCounterDefinitionSample.categorySample;
					int num = (int)categorySample.InstanceNameTable[array[i]];
					baseValue = this.BaseCounterDefinitionSample.instanceValues[num];
				}
				CounterSample sample = new CounterSample(this.instanceValues[array2[i]], baseValue, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
				instanceDataCollection.Add(array[i], new InstanceData(array[i], sample));
			}
			return instanceDataCollection;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000F8F64 File Offset: 0x000F7F64
		internal CounterSample GetSingleValue()
		{
			long rawValue = this.instanceValues[0];
			long baseValue = 0L;
			if (this.BaseCounterDefinitionSample != null)
			{
				baseValue = this.BaseCounterDefinitionSample.instanceValues[0];
			}
			return new CounterSample(rawValue, baseValue, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x000F8FD8 File Offset: 0x000F7FD8
		internal void SetInstanceValue(int index, IntPtr dataRef)
		{
			long num = this.ReadValue(dataRef);
			this.instanceValues[index] = num;
		}

		// Token: 0x04003336 RID: 13110
		internal readonly int NameIndex;

		// Token: 0x04003337 RID: 13111
		internal readonly int CounterType;

		// Token: 0x04003338 RID: 13112
		internal CounterDefinitionSample BaseCounterDefinitionSample;

		// Token: 0x04003339 RID: 13113
		private readonly int size;

		// Token: 0x0400333A RID: 13114
		private readonly int offset;

		// Token: 0x0400333B RID: 13115
		private long[] instanceValues;

		// Token: 0x0400333C RID: 13116
		private CategorySample categorySample;
	}
}
