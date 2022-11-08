using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000611 RID: 1553
	[ComVisible(true)]
	public enum Shortcut
	{
		// Token: 0x0400352B RID: 13611
		None,
		// Token: 0x0400352C RID: 13612
		CtrlA = 131137,
		// Token: 0x0400352D RID: 13613
		CtrlB,
		// Token: 0x0400352E RID: 13614
		CtrlC,
		// Token: 0x0400352F RID: 13615
		CtrlD,
		// Token: 0x04003530 RID: 13616
		CtrlE,
		// Token: 0x04003531 RID: 13617
		CtrlF,
		// Token: 0x04003532 RID: 13618
		CtrlG,
		// Token: 0x04003533 RID: 13619
		CtrlH,
		// Token: 0x04003534 RID: 13620
		CtrlI,
		// Token: 0x04003535 RID: 13621
		CtrlJ,
		// Token: 0x04003536 RID: 13622
		CtrlK,
		// Token: 0x04003537 RID: 13623
		CtrlL,
		// Token: 0x04003538 RID: 13624
		CtrlM,
		// Token: 0x04003539 RID: 13625
		CtrlN,
		// Token: 0x0400353A RID: 13626
		CtrlO,
		// Token: 0x0400353B RID: 13627
		CtrlP,
		// Token: 0x0400353C RID: 13628
		CtrlQ,
		// Token: 0x0400353D RID: 13629
		CtrlR,
		// Token: 0x0400353E RID: 13630
		CtrlS,
		// Token: 0x0400353F RID: 13631
		CtrlT,
		// Token: 0x04003540 RID: 13632
		CtrlU,
		// Token: 0x04003541 RID: 13633
		CtrlV,
		// Token: 0x04003542 RID: 13634
		CtrlW,
		// Token: 0x04003543 RID: 13635
		CtrlX,
		// Token: 0x04003544 RID: 13636
		CtrlY,
		// Token: 0x04003545 RID: 13637
		CtrlZ,
		// Token: 0x04003546 RID: 13638
		CtrlShiftA = 196673,
		// Token: 0x04003547 RID: 13639
		CtrlShiftB,
		// Token: 0x04003548 RID: 13640
		CtrlShiftC,
		// Token: 0x04003549 RID: 13641
		CtrlShiftD,
		// Token: 0x0400354A RID: 13642
		CtrlShiftE,
		// Token: 0x0400354B RID: 13643
		CtrlShiftF,
		// Token: 0x0400354C RID: 13644
		CtrlShiftG,
		// Token: 0x0400354D RID: 13645
		CtrlShiftH,
		// Token: 0x0400354E RID: 13646
		CtrlShiftI,
		// Token: 0x0400354F RID: 13647
		CtrlShiftJ,
		// Token: 0x04003550 RID: 13648
		CtrlShiftK,
		// Token: 0x04003551 RID: 13649
		CtrlShiftL,
		// Token: 0x04003552 RID: 13650
		CtrlShiftM,
		// Token: 0x04003553 RID: 13651
		CtrlShiftN,
		// Token: 0x04003554 RID: 13652
		CtrlShiftO,
		// Token: 0x04003555 RID: 13653
		CtrlShiftP,
		// Token: 0x04003556 RID: 13654
		CtrlShiftQ,
		// Token: 0x04003557 RID: 13655
		CtrlShiftR,
		// Token: 0x04003558 RID: 13656
		CtrlShiftS,
		// Token: 0x04003559 RID: 13657
		CtrlShiftT,
		// Token: 0x0400355A RID: 13658
		CtrlShiftU,
		// Token: 0x0400355B RID: 13659
		CtrlShiftV,
		// Token: 0x0400355C RID: 13660
		CtrlShiftW,
		// Token: 0x0400355D RID: 13661
		CtrlShiftX,
		// Token: 0x0400355E RID: 13662
		CtrlShiftY,
		// Token: 0x0400355F RID: 13663
		CtrlShiftZ,
		// Token: 0x04003560 RID: 13664
		F1 = 112,
		// Token: 0x04003561 RID: 13665
		F2,
		// Token: 0x04003562 RID: 13666
		F3,
		// Token: 0x04003563 RID: 13667
		F4,
		// Token: 0x04003564 RID: 13668
		F5,
		// Token: 0x04003565 RID: 13669
		F6,
		// Token: 0x04003566 RID: 13670
		F7,
		// Token: 0x04003567 RID: 13671
		F8,
		// Token: 0x04003568 RID: 13672
		F9,
		// Token: 0x04003569 RID: 13673
		F10,
		// Token: 0x0400356A RID: 13674
		F11,
		// Token: 0x0400356B RID: 13675
		F12,
		// Token: 0x0400356C RID: 13676
		ShiftF1 = 65648,
		// Token: 0x0400356D RID: 13677
		ShiftF2,
		// Token: 0x0400356E RID: 13678
		ShiftF3,
		// Token: 0x0400356F RID: 13679
		ShiftF4,
		// Token: 0x04003570 RID: 13680
		ShiftF5,
		// Token: 0x04003571 RID: 13681
		ShiftF6,
		// Token: 0x04003572 RID: 13682
		ShiftF7,
		// Token: 0x04003573 RID: 13683
		ShiftF8,
		// Token: 0x04003574 RID: 13684
		ShiftF9,
		// Token: 0x04003575 RID: 13685
		ShiftF10,
		// Token: 0x04003576 RID: 13686
		ShiftF11,
		// Token: 0x04003577 RID: 13687
		ShiftF12,
		// Token: 0x04003578 RID: 13688
		CtrlF1 = 131184,
		// Token: 0x04003579 RID: 13689
		CtrlF2,
		// Token: 0x0400357A RID: 13690
		CtrlF3,
		// Token: 0x0400357B RID: 13691
		CtrlF4,
		// Token: 0x0400357C RID: 13692
		CtrlF5,
		// Token: 0x0400357D RID: 13693
		CtrlF6,
		// Token: 0x0400357E RID: 13694
		CtrlF7,
		// Token: 0x0400357F RID: 13695
		CtrlF8,
		// Token: 0x04003580 RID: 13696
		CtrlF9,
		// Token: 0x04003581 RID: 13697
		CtrlF10,
		// Token: 0x04003582 RID: 13698
		CtrlF11,
		// Token: 0x04003583 RID: 13699
		CtrlF12,
		// Token: 0x04003584 RID: 13700
		CtrlShiftF1 = 196720,
		// Token: 0x04003585 RID: 13701
		CtrlShiftF2,
		// Token: 0x04003586 RID: 13702
		CtrlShiftF3,
		// Token: 0x04003587 RID: 13703
		CtrlShiftF4,
		// Token: 0x04003588 RID: 13704
		CtrlShiftF5,
		// Token: 0x04003589 RID: 13705
		CtrlShiftF6,
		// Token: 0x0400358A RID: 13706
		CtrlShiftF7,
		// Token: 0x0400358B RID: 13707
		CtrlShiftF8,
		// Token: 0x0400358C RID: 13708
		CtrlShiftF9,
		// Token: 0x0400358D RID: 13709
		CtrlShiftF10,
		// Token: 0x0400358E RID: 13710
		CtrlShiftF11,
		// Token: 0x0400358F RID: 13711
		CtrlShiftF12,
		// Token: 0x04003590 RID: 13712
		Ins = 45,
		// Token: 0x04003591 RID: 13713
		CtrlIns = 131117,
		// Token: 0x04003592 RID: 13714
		ShiftIns = 65581,
		// Token: 0x04003593 RID: 13715
		Del = 46,
		// Token: 0x04003594 RID: 13716
		CtrlDel = 131118,
		// Token: 0x04003595 RID: 13717
		ShiftDel = 65582,
		// Token: 0x04003596 RID: 13718
		AltRightArrow = 262183,
		// Token: 0x04003597 RID: 13719
		AltLeftArrow = 262181,
		// Token: 0x04003598 RID: 13720
		AltUpArrow,
		// Token: 0x04003599 RID: 13721
		AltDownArrow = 262184,
		// Token: 0x0400359A RID: 13722
		AltBksp = 262152,
		// Token: 0x0400359B RID: 13723
		AltF1 = 262256,
		// Token: 0x0400359C RID: 13724
		AltF2,
		// Token: 0x0400359D RID: 13725
		AltF3,
		// Token: 0x0400359E RID: 13726
		AltF4,
		// Token: 0x0400359F RID: 13727
		AltF5,
		// Token: 0x040035A0 RID: 13728
		AltF6,
		// Token: 0x040035A1 RID: 13729
		AltF7,
		// Token: 0x040035A2 RID: 13730
		AltF8,
		// Token: 0x040035A3 RID: 13731
		AltF9,
		// Token: 0x040035A4 RID: 13732
		AltF10,
		// Token: 0x040035A5 RID: 13733
		AltF11,
		// Token: 0x040035A6 RID: 13734
		AltF12,
		// Token: 0x040035A7 RID: 13735
		Alt0 = 262192,
		// Token: 0x040035A8 RID: 13736
		Alt1,
		// Token: 0x040035A9 RID: 13737
		Alt2,
		// Token: 0x040035AA RID: 13738
		Alt3,
		// Token: 0x040035AB RID: 13739
		Alt4,
		// Token: 0x040035AC RID: 13740
		Alt5,
		// Token: 0x040035AD RID: 13741
		Alt6,
		// Token: 0x040035AE RID: 13742
		Alt7,
		// Token: 0x040035AF RID: 13743
		Alt8,
		// Token: 0x040035B0 RID: 13744
		Alt9,
		// Token: 0x040035B1 RID: 13745
		Ctrl0 = 131120,
		// Token: 0x040035B2 RID: 13746
		Ctrl1,
		// Token: 0x040035B3 RID: 13747
		Ctrl2,
		// Token: 0x040035B4 RID: 13748
		Ctrl3,
		// Token: 0x040035B5 RID: 13749
		Ctrl4,
		// Token: 0x040035B6 RID: 13750
		Ctrl5,
		// Token: 0x040035B7 RID: 13751
		Ctrl6,
		// Token: 0x040035B8 RID: 13752
		Ctrl7,
		// Token: 0x040035B9 RID: 13753
		Ctrl8,
		// Token: 0x040035BA RID: 13754
		Ctrl9,
		// Token: 0x040035BB RID: 13755
		CtrlShift0 = 196656,
		// Token: 0x040035BC RID: 13756
		CtrlShift1,
		// Token: 0x040035BD RID: 13757
		CtrlShift2,
		// Token: 0x040035BE RID: 13758
		CtrlShift3,
		// Token: 0x040035BF RID: 13759
		CtrlShift4,
		// Token: 0x040035C0 RID: 13760
		CtrlShift5,
		// Token: 0x040035C1 RID: 13761
		CtrlShift6,
		// Token: 0x040035C2 RID: 13762
		CtrlShift7,
		// Token: 0x040035C3 RID: 13763
		CtrlShift8,
		// Token: 0x040035C4 RID: 13764
		CtrlShift9
	}
}
