using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Schema
{
	// Token: 0x020001A4 RID: 420
	internal sealed class RangeContentValidator : ContentValidator
	{
		// Token: 0x060015AC RID: 5548 RVA: 0x00060140 File Offset: 0x0005F140
		internal RangeContentValidator(BitSet firstpos, BitSet[] followpos, SymbolsDictionary symbols, Positions positions, int endMarkerPos, XmlSchemaContentType contentType, bool isEmptiable, BitSet positionsWithRangeTerminals, int minmaxNodesCount) : base(contentType, false, isEmptiable)
		{
			this.firstpos = firstpos;
			this.followpos = followpos;
			this.symbols = symbols;
			this.positions = positions;
			this.positionsWithRangeTerminals = positionsWithRangeTerminals;
			this.minMaxNodesCount = minmaxNodesCount;
			this.endMarkerPos = endMarkerPos;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00060190 File Offset: 0x0005F190
		public override void InitValidation(ValidationState context)
		{
			int count = this.positions.Count;
			List<RangePositionInfo> list = context.RunningPositions;
			if (list != null)
			{
				list.Clear();
			}
			else
			{
				list = new List<RangePositionInfo>();
				context.RunningPositions = list;
			}
			RangePositionInfo item = new RangePositionInfo
			{
				curpos = this.firstpos.Clone(),
				rangeCounters = new decimal[this.minMaxNodesCount]
			};
			list.Add(item);
			context.CurrentState.NumberOfRunningPos = 1;
			context.HasMatched = item.curpos.Get(this.endMarkerPos);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00060220 File Offset: 0x0005F220
		public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
		{
			errorCode = 0;
			int num = this.symbols[name];
			bool flag = false;
			List<RangePositionInfo> runningPositions = context.RunningPositions;
			int num2 = context.CurrentState.NumberOfRunningPos;
			int i = 0;
			int num3 = -1;
			int num4 = -1;
			bool flag2 = false;
			while (i < num2)
			{
				BitSet curpos = runningPositions[i].curpos;
				for (int num5 = curpos.NextSet(-1); num5 != -1; num5 = curpos.NextSet(num5))
				{
					if (num == this.positions[num5].symbol)
					{
						num3 = num5;
						if (num4 == -1)
						{
							num4 = i;
						}
						flag2 = true;
						break;
					}
				}
				if (flag2 && this.positions[num3].particle is XmlSchemaElement)
				{
					break;
				}
				i++;
			}
			if (i == num2 && num3 != -1)
			{
				i = num4;
			}
			if (i < num2)
			{
				if (i != 0)
				{
					runningPositions.RemoveRange(0, i);
				}
				num2 -= i;
				i = 0;
				while (i < num2)
				{
					RangePositionInfo value = runningPositions[i];
					flag2 = value.curpos.Get(num3);
					if (flag2)
					{
						value.curpos = this.followpos[num3];
						runningPositions[i] = value;
						i++;
					}
					else
					{
						num2--;
						if (num2 > 0)
						{
							RangePositionInfo value2 = runningPositions[num2];
							runningPositions[num2] = runningPositions[i];
							runningPositions[i] = value2;
						}
					}
				}
			}
			else
			{
				num2 = 0;
			}
			if (num2 > 0)
			{
				if (num2 >= 10000)
				{
					context.TooComplex = true;
					num2 /= 2;
				}
				for (i = num2 - 1; i >= 0; i--)
				{
					int index = i;
					BitSet curpos2 = runningPositions[i].curpos;
					flag = (flag || curpos2.Get(this.endMarkerPos));
					while (num2 < 10000 && curpos2.Intersects(this.positionsWithRangeTerminals))
					{
						BitSet bitSet = curpos2.Clone();
						bitSet.And(this.positionsWithRangeTerminals);
						int num6 = bitSet.NextSet(-1);
						LeafRangeNode leafRangeNode = this.positions[num6].particle as LeafRangeNode;
						RangePositionInfo value = runningPositions[index];
						if (num2 + 2 >= runningPositions.Count)
						{
							runningPositions.Add(default(RangePositionInfo));
							runningPositions.Add(default(RangePositionInfo));
						}
						RangePositionInfo value3 = runningPositions[num2];
						if (value3.rangeCounters == null)
						{
							value3.rangeCounters = new decimal[this.minMaxNodesCount];
						}
						Array.Copy(value.rangeCounters, 0, value3.rangeCounters, 0, value.rangeCounters.Length);
						decimal[] rangeCounters = value3.rangeCounters;
						int pos = leafRangeNode.Pos;
						decimal d = rangeCounters[pos] = ++rangeCounters[pos];
						if (d == leafRangeNode.Max)
						{
							value3.curpos = this.followpos[num6];
							value3.rangeCounters[leafRangeNode.Pos] = 0m;
							runningPositions[num2] = value3;
							index = num2++;
						}
						else
						{
							if (d < leafRangeNode.Min)
							{
								value3.curpos = leafRangeNode.NextIteration;
								runningPositions[num2] = value3;
								num2++;
								break;
							}
							value3.curpos = leafRangeNode.NextIteration;
							runningPositions[num2] = value3;
							index = num2 + 1;
							value3 = runningPositions[index];
							if (value3.rangeCounters == null)
							{
								value3.rangeCounters = new decimal[this.minMaxNodesCount];
							}
							Array.Copy(value.rangeCounters, 0, value3.rangeCounters, 0, value.rangeCounters.Length);
							value3.curpos = this.followpos[num6];
							value3.rangeCounters[leafRangeNode.Pos] = 0m;
							runningPositions[index] = value3;
							num2 += 2;
						}
						curpos2 = runningPositions[index].curpos;
						flag = (flag || curpos2.Get(this.endMarkerPos));
					}
				}
				context.HasMatched = flag;
				context.CurrentState.NumberOfRunningPos = num2;
				return this.positions[num3].particle;
			}
			errorCode = -1;
			context.NeedValidateChildren = false;
			return null;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00060642 File Offset: 0x0005F642
		public override bool CompleteValidation(ValidationState context)
		{
			return context.HasMatched;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0006064C File Offset: 0x0005F64C
		public override ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
		{
			ArrayList arrayList = null;
			if (context.RunningPositions != null)
			{
				List<RangePositionInfo> runningPositions = context.RunningPositions;
				BitSet bitSet = new BitSet(this.positions.Count);
				for (int i = context.CurrentState.NumberOfRunningPos - 1; i >= 0; i--)
				{
					bitSet.Or(runningPositions[i].curpos);
				}
				for (int num = bitSet.NextSet(-1); num != -1; num = bitSet.NextSet(num))
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					int symbol = this.positions[num].symbol;
					if (symbol >= 0)
					{
						XmlSchemaParticle xmlSchemaParticle = this.positions[num].particle as XmlSchemaParticle;
						if (xmlSchemaParticle == null)
						{
							string text = this.symbols.NameOf(this.positions[num].symbol);
							if (text.Length != 0)
							{
								arrayList.Add(text);
							}
						}
						else
						{
							string nameString = xmlSchemaParticle.NameString;
							if (!arrayList.Contains(nameString))
							{
								arrayList.Add(nameString);
							}
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00060758 File Offset: 0x0005F758
		public override ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly)
		{
			ArrayList arrayList = new ArrayList();
			if (context.RunningPositions != null)
			{
				List<RangePositionInfo> runningPositions = context.RunningPositions;
				BitSet bitSet = new BitSet(this.positions.Count);
				for (int i = context.CurrentState.NumberOfRunningPos - 1; i >= 0; i--)
				{
					bitSet.Or(runningPositions[i].curpos);
				}
				for (int num = bitSet.NextSet(-1); num != -1; num = bitSet.NextSet(num))
				{
					int symbol = this.positions[num].symbol;
					if (symbol >= 0)
					{
						XmlSchemaParticle xmlSchemaParticle = this.positions[num].particle as XmlSchemaParticle;
						if (xmlSchemaParticle != null && !arrayList.Contains(xmlSchemaParticle))
						{
							arrayList.Add(xmlSchemaParticle);
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x04000CE3 RID: 3299
		private BitSet firstpos;

		// Token: 0x04000CE4 RID: 3300
		private BitSet[] followpos;

		// Token: 0x04000CE5 RID: 3301
		private BitSet positionsWithRangeTerminals;

		// Token: 0x04000CE6 RID: 3302
		private SymbolsDictionary symbols;

		// Token: 0x04000CE7 RID: 3303
		private Positions positions;

		// Token: 0x04000CE8 RID: 3304
		private int minMaxNodesCount;

		// Token: 0x04000CE9 RID: 3305
		private int endMarkerPos;
	}
}
