using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000769 RID: 1897
	internal class CategorySample
	{
		// Token: 0x06003A6C RID: 14956 RVA: 0x000F850C File Offset: 0x000F750C
		internal unsafe CategorySample(byte[] data, CategoryEntry entry, PerformanceCounterLib library)
		{
			this.entry = entry;
			this.library = library;
			int nameIndex = entry.NameIndex;
			NativeMethods.PERF_DATA_BLOCK perf_DATA_BLOCK = new NativeMethods.PERF_DATA_BLOCK();
			fixed (byte* ptr = data)
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				Marshal.PtrToStructure(intPtr, perf_DATA_BLOCK);
				this.SystemFrequency = perf_DATA_BLOCK.PerfFreq;
				this.TimeStamp = perf_DATA_BLOCK.PerfTime;
				this.TimeStamp100nSec = perf_DATA_BLOCK.PerfTime100nSec;
				intPtr = (IntPtr)((long)intPtr + (long)perf_DATA_BLOCK.HeaderLength);
				int numObjectTypes = perf_DATA_BLOCK.NumObjectTypes;
				if (numObjectTypes == 0)
				{
					this.CounterTable = new Hashtable();
					this.InstanceNameTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				else
				{
					NativeMethods.PERF_OBJECT_TYPE perf_OBJECT_TYPE = null;
					bool flag = false;
					for (int i = 0; i < numObjectTypes; i++)
					{
						perf_OBJECT_TYPE = new NativeMethods.PERF_OBJECT_TYPE();
						Marshal.PtrToStructure(intPtr, perf_OBJECT_TYPE);
						if (perf_OBJECT_TYPE.ObjectNameTitleIndex == nameIndex)
						{
							flag = true;
							break;
						}
						intPtr = (IntPtr)((long)intPtr + (long)perf_OBJECT_TYPE.TotalByteLength);
					}
					if (!flag)
					{
						throw new InvalidOperationException(SR.GetString("CantReadCategoryIndex", new object[]
						{
							nameIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.CounterFrequency = perf_OBJECT_TYPE.PerfFreq;
					this.CounterTimeStamp = perf_OBJECT_TYPE.PerfTime;
					int numCounters = perf_OBJECT_TYPE.NumCounters;
					int numInstances = perf_OBJECT_TYPE.NumInstances;
					if (numInstances == -1)
					{
						this.IsMultiInstance = false;
					}
					else
					{
						this.IsMultiInstance = true;
					}
					intPtr = (IntPtr)((long)intPtr + (long)perf_OBJECT_TYPE.HeaderLength);
					CounterDefinitionSample[] array = new CounterDefinitionSample[numCounters];
					this.CounterTable = new Hashtable(numCounters);
					for (int j = 0; j < array.Length; j++)
					{
						NativeMethods.PERF_COUNTER_DEFINITION perf_COUNTER_DEFINITION = new NativeMethods.PERF_COUNTER_DEFINITION();
						Marshal.PtrToStructure(intPtr, perf_COUNTER_DEFINITION);
						array[j] = new CounterDefinitionSample(perf_COUNTER_DEFINITION, this, numInstances);
						intPtr = (IntPtr)((long)intPtr + (long)perf_COUNTER_DEFINITION.ByteLength);
						int counterType = array[j].CounterType;
						if (!PerformanceCounterLib.IsBaseCounter(counterType))
						{
							if (counterType != 1073742336)
							{
								this.CounterTable[array[j].NameIndex] = array[j];
							}
						}
						else if (j > 0)
						{
							array[j - 1].BaseCounterDefinitionSample = array[j];
						}
					}
					if (!this.IsMultiInstance)
					{
						this.InstanceNameTable = new Hashtable(1, StringComparer.OrdinalIgnoreCase);
						this.InstanceNameTable["systemdiagnosticsperfcounterlibsingleinstance"] = 0;
						for (int k = 0; k < array.Length; k++)
						{
							array[k].SetInstanceValue(0, intPtr);
						}
					}
					else
					{
						string[] array2 = null;
						this.InstanceNameTable = new Hashtable(numInstances, StringComparer.OrdinalIgnoreCase);
						for (int l = 0; l < numInstances; l++)
						{
							NativeMethods.PERF_INSTANCE_DEFINITION perf_INSTANCE_DEFINITION = new NativeMethods.PERF_INSTANCE_DEFINITION();
							Marshal.PtrToStructure(intPtr, perf_INSTANCE_DEFINITION);
							if (perf_INSTANCE_DEFINITION.ParentObjectTitleIndex > 0 && array2 == null)
							{
								array2 = this.GetInstanceNamesFromIndex(perf_INSTANCE_DEFINITION.ParentObjectTitleIndex);
							}
							string text;
							if (array2 != null && perf_INSTANCE_DEFINITION.ParentObjectInstance >= 0 && perf_INSTANCE_DEFINITION.ParentObjectInstance < array2.Length - 1)
							{
								text = array2[perf_INSTANCE_DEFINITION.ParentObjectInstance] + "/" + Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)perf_INSTANCE_DEFINITION.NameOffset));
							}
							else
							{
								text = Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)perf_INSTANCE_DEFINITION.NameOffset));
							}
							string key = text;
							int num = 1;
							while (this.InstanceNameTable.ContainsKey(key))
							{
								key = text + "#" + num.ToString(CultureInfo.InvariantCulture);
								num++;
							}
							this.InstanceNameTable[key] = l;
							intPtr = (IntPtr)((long)intPtr + (long)perf_INSTANCE_DEFINITION.ByteLength);
							for (int m = 0; m < array.Length; m++)
							{
								array[m].SetInstanceValue(l, intPtr);
							}
							intPtr = (IntPtr)((long)intPtr + (long)Marshal.ReadInt32(intPtr));
						}
					}
					ptr = null;
				}
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x000F88FC File Offset: 0x000F78FC
		internal unsafe string[] GetInstanceNamesFromIndex(int categoryIndex)
		{
			byte[] performanceData = this.library.GetPerformanceData(categoryIndex.ToString(CultureInfo.InvariantCulture));
			fixed (byte* ptr = performanceData)
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				NativeMethods.PERF_DATA_BLOCK perf_DATA_BLOCK = new NativeMethods.PERF_DATA_BLOCK();
				Marshal.PtrToStructure(intPtr, perf_DATA_BLOCK);
				intPtr = (IntPtr)((long)intPtr + (long)perf_DATA_BLOCK.HeaderLength);
				int numObjectTypes = perf_DATA_BLOCK.NumObjectTypes;
				NativeMethods.PERF_OBJECT_TYPE perf_OBJECT_TYPE = null;
				bool flag = false;
				for (int i = 0; i < numObjectTypes; i++)
				{
					perf_OBJECT_TYPE = new NativeMethods.PERF_OBJECT_TYPE();
					Marshal.PtrToStructure(intPtr, perf_OBJECT_TYPE);
					if (perf_OBJECT_TYPE.ObjectNameTitleIndex == categoryIndex)
					{
						flag = true;
						break;
					}
					intPtr = (IntPtr)((long)intPtr + (long)perf_OBJECT_TYPE.TotalByteLength);
				}
				string[] result;
				if (!flag)
				{
					result = new string[0];
				}
				else
				{
					int numCounters = perf_OBJECT_TYPE.NumCounters;
					int numInstances = perf_OBJECT_TYPE.NumInstances;
					intPtr = (IntPtr)((long)intPtr + (long)perf_OBJECT_TYPE.HeaderLength);
					if (numInstances == -1)
					{
						result = new string[0];
					}
					else
					{
						CounterDefinitionSample[] array = new CounterDefinitionSample[numCounters];
						for (int j = 0; j < array.Length; j++)
						{
							NativeMethods.PERF_COUNTER_DEFINITION perf_COUNTER_DEFINITION = new NativeMethods.PERF_COUNTER_DEFINITION();
							Marshal.PtrToStructure(intPtr, perf_COUNTER_DEFINITION);
							intPtr = (IntPtr)((long)intPtr + (long)perf_COUNTER_DEFINITION.ByteLength);
						}
						string[] array2 = new string[numInstances];
						for (int k = 0; k < numInstances; k++)
						{
							NativeMethods.PERF_INSTANCE_DEFINITION perf_INSTANCE_DEFINITION = new NativeMethods.PERF_INSTANCE_DEFINITION();
							Marshal.PtrToStructure(intPtr, perf_INSTANCE_DEFINITION);
							array2[k] = Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)perf_INSTANCE_DEFINITION.NameOffset));
							intPtr = (IntPtr)((long)intPtr + (long)perf_INSTANCE_DEFINITION.ByteLength);
							intPtr = (IntPtr)((long)intPtr + (long)Marshal.ReadInt32(intPtr));
						}
						result = array2;
					}
				}
				return result;
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x000F8ABC File Offset: 0x000F7ABC
		internal CounterDefinitionSample GetCounterDefinitionSample(string counter)
		{
			int i = 0;
			while (i < this.entry.CounterIndexes.Length)
			{
				int num = this.entry.CounterIndexes[i];
				string text = (string)this.library.NameTable[num];
				if (text != null && string.Compare(text, counter, StringComparison.OrdinalIgnoreCase) == 0)
				{
					CounterDefinitionSample counterDefinitionSample = (CounterDefinitionSample)this.CounterTable[num];
					if (counterDefinitionSample == null)
					{
						foreach (object obj in this.CounterTable.Values)
						{
							CounterDefinitionSample counterDefinitionSample2 = (CounterDefinitionSample)obj;
							if (counterDefinitionSample2.BaseCounterDefinitionSample != null && counterDefinitionSample2.BaseCounterDefinitionSample.NameIndex == num)
							{
								return counterDefinitionSample2.BaseCounterDefinitionSample;
							}
						}
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
					return counterDefinitionSample;
				}
				else
				{
					i++;
				}
			}
			throw new InvalidOperationException(SR.GetString("CantReadCounter", new object[]
			{
				counter
			}));
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x000F8BE8 File Offset: 0x000F7BE8
		internal InstanceDataCollectionCollection ReadCategory()
		{
			InstanceDataCollectionCollection instanceDataCollectionCollection = new InstanceDataCollectionCollection();
			for (int i = 0; i < this.entry.CounterIndexes.Length; i++)
			{
				int num = this.entry.CounterIndexes[i];
				string text = (string)this.library.NameTable[num];
				if (text != null && text != string.Empty)
				{
					CounterDefinitionSample counterDefinitionSample = (CounterDefinitionSample)this.CounterTable[num];
					if (counterDefinitionSample != null)
					{
						instanceDataCollectionCollection.Add(text, counterDefinitionSample.ReadInstanceData(text));
					}
				}
			}
			return instanceDataCollectionCollection;
		}

		// Token: 0x0400332C RID: 13100
		internal readonly long SystemFrequency;

		// Token: 0x0400332D RID: 13101
		internal readonly long TimeStamp;

		// Token: 0x0400332E RID: 13102
		internal readonly long TimeStamp100nSec;

		// Token: 0x0400332F RID: 13103
		internal readonly long CounterFrequency;

		// Token: 0x04003330 RID: 13104
		internal readonly long CounterTimeStamp;

		// Token: 0x04003331 RID: 13105
		internal Hashtable CounterTable;

		// Token: 0x04003332 RID: 13106
		internal Hashtable InstanceNameTable;

		// Token: 0x04003333 RID: 13107
		internal bool IsMultiInstance;

		// Token: 0x04003334 RID: 13108
		private CategoryEntry entry;

		// Token: 0x04003335 RID: 13109
		private PerformanceCounterLib library;
	}
}
