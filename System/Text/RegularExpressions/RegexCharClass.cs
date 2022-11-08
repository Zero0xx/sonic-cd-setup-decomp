﻿using System;
using System.Collections;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000013 RID: 19
	internal sealed class RegexCharClass
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003A38 File Offset: 0x00002A38
		static RegexCharClass()
		{
			string[,] array = new string[112, 2];
			array[0, 0] = "IsAlphabeticPresentationForms";
			array[0, 1] = "ﬀﭐ";
			array[1, 0] = "IsArabic";
			array[1, 1] = "؀܀";
			array[2, 0] = "IsArabicPresentationForms-A";
			array[2, 1] = "ﭐ︀";
			array[3, 0] = "IsArabicPresentationForms-B";
			array[3, 1] = "ﹰ＀";
			array[4, 0] = "IsArmenian";
			array[4, 1] = "԰֐";
			array[5, 0] = "IsArrows";
			array[5, 1] = "←∀";
			array[6, 0] = "IsBasicLatin";
			array[6, 1] = "\0\u0080";
			array[7, 0] = "IsBengali";
			array[7, 1] = "ঀ਀";
			array[8, 0] = "IsBlockElements";
			array[8, 1] = "▀■";
			array[9, 0] = "IsBopomofo";
			array[9, 1] = "㄀㄰";
			array[10, 0] = "IsBopomofoExtended";
			array[10, 1] = "ㆠ㇀";
			array[11, 0] = "IsBoxDrawing";
			array[11, 1] = "─▀";
			array[12, 0] = "IsBraillePatterns";
			array[12, 1] = "⠀⤀";
			array[13, 0] = "IsBuhid";
			array[13, 1] = "ᝀᝠ";
			array[14, 0] = "IsCJKCompatibility";
			array[14, 1] = "㌀㐀";
			array[15, 0] = "IsCJKCompatibilityForms";
			array[15, 1] = "︰﹐";
			array[16, 0] = "IsCJKCompatibilityIdeographs";
			array[16, 1] = "豈ﬀ";
			array[17, 0] = "IsCJKRadicalsSupplement";
			array[17, 1] = "⺀⼀";
			array[18, 0] = "IsCJKSymbolsandPunctuation";
			array[18, 1] = "\u3000぀";
			array[19, 0] = "IsCJKUnifiedIdeographs";
			array[19, 1] = "一ꀀ";
			array[20, 0] = "IsCJKUnifiedIdeographsExtensionA";
			array[20, 1] = "㐀䷀";
			array[21, 0] = "IsCherokee";
			array[21, 1] = "Ꭰ᐀";
			array[22, 0] = "IsCombiningDiacriticalMarks";
			array[22, 1] = "̀Ͱ";
			array[23, 0] = "IsCombiningDiacriticalMarksforSymbols";
			array[23, 1] = "⃐℀";
			array[24, 0] = "IsCombiningHalfMarks";
			array[24, 1] = "︠︰";
			array[25, 0] = "IsCombiningMarksforSymbols";
			array[25, 1] = "⃐℀";
			array[26, 0] = "IsControlPictures";
			array[26, 1] = "␀⑀";
			array[27, 0] = "IsCurrencySymbols";
			array[27, 1] = "₠⃐";
			array[28, 0] = "IsCyrillic";
			array[28, 1] = "ЀԀ";
			array[29, 0] = "IsCyrillicSupplement";
			array[29, 1] = "Ԁ԰";
			array[30, 0] = "IsDevanagari";
			array[30, 1] = "ऀঀ";
			array[31, 0] = "IsDingbats";
			array[31, 1] = "✀⟀";
			array[32, 0] = "IsEnclosedAlphanumerics";
			array[32, 1] = "①─";
			array[33, 0] = "IsEnclosedCJKLettersandMonths";
			array[33, 1] = "㈀㌀";
			array[34, 0] = "IsEthiopic";
			array[34, 1] = "ሀᎀ";
			array[35, 0] = "IsGeneralPunctuation";
			array[35, 1] = "\u2000⁰";
			array[36, 0] = "IsGeometricShapes";
			array[36, 1] = "■☀";
			array[37, 0] = "IsGeorgian";
			array[37, 1] = "Ⴀᄀ";
			array[38, 0] = "IsGreek";
			array[38, 1] = "ͰЀ";
			array[39, 0] = "IsGreekExtended";
			array[39, 1] = "ἀ\u2000";
			array[40, 0] = "IsGreekandCoptic";
			array[40, 1] = "ͰЀ";
			array[41, 0] = "IsGujarati";
			array[41, 1] = "઀଀";
			array[42, 0] = "IsGurmukhi";
			array[42, 1] = "਀઀";
			array[43, 0] = "IsHalfwidthandFullwidthForms";
			array[43, 1] = "＀￰";
			array[44, 0] = "IsHangulCompatibilityJamo";
			array[44, 1] = "㄰㆐";
			array[45, 0] = "IsHangulJamo";
			array[45, 1] = "ᄀሀ";
			array[46, 0] = "IsHangulSyllables";
			array[46, 1] = "가ힰ";
			array[47, 0] = "IsHanunoo";
			array[47, 1] = "ᜠᝀ";
			array[48, 0] = "IsHebrew";
			array[48, 1] = "֐؀";
			array[49, 0] = "IsHighPrivateUseSurrogates";
			array[49, 1] = "\udb80\udc00";
			array[50, 0] = "IsHighSurrogates";
			array[50, 1] = "\ud800\udb80";
			array[51, 0] = "IsHiragana";
			array[51, 1] = "぀゠";
			array[52, 0] = "IsIPAExtensions";
			array[52, 1] = "ɐʰ";
			array[53, 0] = "IsIdeographicDescriptionCharacters";
			array[53, 1] = "⿰\u3000";
			array[54, 0] = "IsKanbun";
			array[54, 1] = "㆐ㆠ";
			array[55, 0] = "IsKangxiRadicals";
			array[55, 1] = "⼀⿠";
			array[56, 0] = "IsKannada";
			array[56, 1] = "ಀഀ";
			array[57, 0] = "IsKatakana";
			array[57, 1] = "゠㄀";
			array[58, 0] = "IsKatakanaPhoneticExtensions";
			array[58, 1] = "ㇰ㈀";
			array[59, 0] = "IsKhmer";
			array[59, 1] = "ក᠀";
			array[60, 0] = "IsKhmerSymbols";
			array[60, 1] = "᧠ᨀ";
			array[61, 0] = "IsLao";
			array[61, 1] = "຀ༀ";
			array[62, 0] = "IsLatin-1Supplement";
			array[62, 1] = "\u0080Ā";
			array[63, 0] = "IsLatinExtended-A";
			array[63, 1] = "Āƀ";
			array[64, 0] = "IsLatinExtended-B";
			array[64, 1] = "ƀɐ";
			array[65, 0] = "IsLatinExtendedAdditional";
			array[65, 1] = "Ḁἀ";
			array[66, 0] = "IsLetterlikeSymbols";
			array[66, 1] = "℀⅐";
			array[67, 0] = "IsLimbu";
			array[67, 1] = "ᤀᥐ";
			array[68, 0] = "IsLowSurrogates";
			array[68, 1] = "\udc00";
			array[69, 0] = "IsMalayalam";
			array[69, 1] = "ഀ඀";
			array[70, 0] = "IsMathematicalOperators";
			array[70, 1] = "∀⌀";
			array[71, 0] = "IsMiscellaneousMathematicalSymbols-A";
			array[71, 1] = "⟀⟰";
			array[72, 0] = "IsMiscellaneousMathematicalSymbols-B";
			array[72, 1] = "⦀⨀";
			array[73, 0] = "IsMiscellaneousSymbols";
			array[73, 1] = "☀✀";
			array[74, 0] = "IsMiscellaneousSymbolsandArrows";
			array[74, 1] = "⬀Ⰰ";
			array[75, 0] = "IsMiscellaneousTechnical";
			array[75, 1] = "⌀␀";
			array[76, 0] = "IsMongolian";
			array[76, 1] = "᠀ᢰ";
			array[77, 0] = "IsMyanmar";
			array[77, 1] = "ကႠ";
			array[78, 0] = "IsNumberForms";
			array[78, 1] = "⅐←";
			array[79, 0] = "IsOgham";
			array[79, 1] = "\u1680ᚠ";
			array[80, 0] = "IsOpticalCharacterRecognition";
			array[80, 1] = "⑀①";
			array[81, 0] = "IsOriya";
			array[81, 1] = "଀஀";
			array[82, 0] = "IsPhoneticExtensions";
			array[82, 1] = "ᴀᶀ";
			array[83, 0] = "IsPrivateUse";
			array[83, 1] = "豈";
			array[84, 0] = "IsPrivateUseArea";
			array[84, 1] = "豈";
			array[85, 0] = "IsRunic";
			array[85, 1] = "ᚠᜀ";
			array[86, 0] = "IsSinhala";
			array[86, 1] = "඀฀";
			array[87, 0] = "IsSmallFormVariants";
			array[87, 1] = "﹐ﹰ";
			array[88, 0] = "IsSpacingModifierLetters";
			array[88, 1] = "ʰ̀";
			array[89, 0] = "IsSpecials";
			array[89, 1] = "￰";
			array[90, 0] = "IsSuperscriptsandSubscripts";
			array[90, 1] = "⁰₠";
			array[91, 0] = "IsSupplementalArrows-A";
			array[91, 1] = "⟰⠀";
			array[92, 0] = "IsSupplementalArrows-B";
			array[92, 1] = "⤀⦀";
			array[93, 0] = "IsSupplementalMathematicalOperators";
			array[93, 1] = "⨀⬀";
			array[94, 0] = "IsSyriac";
			array[94, 1] = "܀ݐ";
			array[95, 0] = "IsTagalog";
			array[95, 1] = "ᜀᜠ";
			array[96, 0] = "IsTagbanwa";
			array[96, 1] = "ᝠក";
			array[97, 0] = "IsTaiLe";
			array[97, 1] = "ᥐᦀ";
			array[98, 0] = "IsTamil";
			array[98, 1] = "஀ఀ";
			array[99, 0] = "IsTelugu";
			array[99, 1] = "ఀಀ";
			array[100, 0] = "IsThaana";
			array[100, 1] = "ހ߀";
			array[101, 0] = "IsThai";
			array[101, 1] = "฀຀";
			array[102, 0] = "IsTibetan";
			array[102, 1] = "ༀက";
			array[103, 0] = "IsUnifiedCanadianAboriginalSyllabics";
			array[103, 1] = "᐀\u1680";
			array[104, 0] = "IsVariationSelectors";
			array[104, 1] = "︀︐";
			array[105, 0] = "IsYiRadicals";
			array[105, 1] = "꒐ꓐ";
			array[106, 0] = "IsYiSyllables";
			array[106, 1] = "ꀀ꒐";
			array[107, 0] = "IsYijingHexagramSymbols";
			array[107, 1] = "䷀一";
			array[108, 0] = "_xmlC";
			array[108, 1] = "-/0;A[_`a{·¸À×Ø÷øĲĴĿŁŉŊſƀǄǍǱǴǶǺȘɐʩʻ˂ː˒̀͆͢͠Ά΋Ό΍Ύ΢ΣϏϐϗϚϛϜϝϞϟϠϡϢϴЁЍЎѐёѝў҂҃҇ҐӅӇӉӋӍӐӬӮӶӸӺԱ՗ՙ՚աևֺֻ֑֢֣־ֿ׀ׁ׃ׅׄא׫װ׳ءػـٓ٠٪ٰڸںڿۀۏې۔ە۩۪ۮ۰ۺँऄअऺ़ॎ॑ॕक़।०॰ঁ঄অ঍এ঑ও঩প঱ল঳শ঺়ঽা৅ে৉োৎৗ৘ড়৞য়৤০৲ਂਃਅ਋ਏ਑ਓ਩ਪ਱ਲ਴ਵ਷ਸ਺਼਽ਾ੃ੇ੉ੋ੎ਖ਼੝ਫ਼੟੦ੵઁ઄અઌઍ઎એ઒ઓ઩પ઱લ઴વ઺઼૆ે૊ો૎ૠૡ૦૰ଁ଄ଅ଍ଏ଑ଓ଩ପ଱ଲ଴ଶ଺଼ୄେ୉ୋ୎ୖ୘ଡ଼୞ୟୢ୦୰ஂ஄அ஋எ஑ஒ஖ங஛ஜ஝ஞ஠ண஥ந஫மஶஷ஺ா௃ெ௉ொ௎ௗ௘௧௰ఁఄఅ఍ఎ఑ఒ఩పఴవ఺ా౅ె౉ొ౎ౕ౗ౠౢ౦౰ಂ಄ಅ಍ಎ಑ಒ಩ಪ಴ವ಺ಾ೅ೆ೉ೊ೎ೕ೗ೞ೟ೠೢ೦೰ംഄഅ഍എ഑ഒഩപഺാൄെ൉ൊൎൗ൘ൠൢ൦൰กฯะ฻เ๏๐๚ກ຃ຄ຅ງຉຊ຋ຍຎດຘນຠມ຤ລ຦ວຨສຬອຯະ຺ົ຾ເ໅ໆ໇່໎໐໚༘༚༠༪༵༶༷༸༹༺༾཈ཉཪཱ྅྆ྌྐྖྗ྘ྙྮྱྸྐྵྺႠ჆აჷᄀᄁᄂᄄᄅᄈᄉᄊᄋᄍᄎᄓᄼᄽᄾᄿᅀᅁᅌᅍᅎᅏᅐᅑᅔᅖᅙᅚᅟᅢᅣᅤᅥᅦᅧᅨᅩᅪᅭᅯᅲᅴᅵᅶᆞᆟᆨᆩᆫᆬᆮᆰᆷᆹᆺᆻᆼᇃᇫᇬᇰᇱᇹᇺḀẜẠỺἀ἖Ἐ἞ἠ὆Ὀ὎ὐ὘Ὑ὚Ὓ὜Ὕ὞Ὗ὾ᾀ᾵ᾶ᾽ι᾿ῂ῅ῆ῍ῐ῔ῖ῜ῠ῭ῲ῵ῶ´⃐⃝⃡⃢Ω℧Kℬ℮ℯↀↃ々〆〇〈〡〰〱〶ぁゕ゙゛ゝゟァ・ーヿㄅㄭ一龦가힤";
			array[109, 0] = "_xmlD";
			array[109, 1] = "0:٠٪۰ۺ०॰০ৰ੦ੰ૦૰୦୰௧௰౦౰೦೰൦൰๐๚໐໚༠༪၀၊፩፲០៪᠐᠚０：";
			array[110, 0] = "_xmlI";
			array[110, 1] = ":;A[_`a{À×Ø÷øĲĴĿŁŉŊſƀǄǍǱǴǶǺȘɐʩʻ˂Ά·Έ΋Ό΍Ύ΢ΣϏϐϗϚϛϜϝϞϟϠϡϢϴЁЍЎѐёѝў҂ҐӅӇӉӋӍӐӬӮӶӸӺԱ՗ՙ՚աևא׫װ׳ءػفًٱڸںڿۀۏې۔ەۖۥۧअऺऽाक़ॢঅ঍এ঑ও঩প঱ল঳শ঺ড়৞য়ৢৰ৲ਅ਋ਏ਑ਓ਩ਪ਱ਲ਴ਵ਷ਸ਺ਖ਼੝ਫ਼੟ੲੵઅઌઍ઎એ઒ઓ઩પ઱લ઴વ઺ઽાૠૡଅ଍ଏ଑ଓ଩ପ଱ଲ଴ଶ଺ଽାଡ଼୞ୟୢஅ஋எ஑ஒ஖ங஛ஜ஝ஞ஠ண஥ந஫மஶஷ஺అ఍ఎ఑ఒ఩పఴవ఺ౠౢಅ಍ಎ಑ಒ಩ಪ಴ವ಺ೞ೟ೠೢഅ഍എ഑ഒഩപഺൠൢกฯะัาิเๆກ຃ຄ຅ງຉຊ຋ຍຎດຘນຠມ຤ລ຦ວຨສຬອຯະັາິຽ຾ເ໅ཀ཈ཉཪႠ჆აჷᄀᄁᄂᄄᄅᄈᄉᄊᄋᄍᄎᄓᄼᄽᄾᄿᅀᅁᅌᅍᅎᅏᅐᅑᅔᅖᅙᅚᅟᅢᅣᅤᅥᅦᅧᅨᅩᅪᅭᅯᅲᅴᅵᅶᆞᆟᆨᆩᆫᆬᆮᆰᆷᆹᆺᆻᆼᇃᇫᇬᇰᇱᇹᇺḀẜẠỺἀ἖Ἐ἞ἠ὆Ὀ὎ὐ὘Ὑ὚Ὓ὜Ὕ὞Ὗ὾ᾀ᾵ᾶ᾽ι᾿ῂ῅ῆ῍ῐ῔ῖ῜ῠ῭ῲ῵ῶ´Ω℧Kℬ℮ℯↀↃ〇〈〡〪ぁゕァ・ㄅㄭ一龦가힤";
			array[111, 0] = "_xmlW";
			array[111, 1] = "$%+,0:<?A[^_`{|}~\u007f¢«¬­®·¸»¼¿ÀȡȢȴɐʮʰ˯̀͐͠ͰʹͶͺͻ΄·Έ΋Ό΍Ύ΢ΣϏϐϷЀ҇҈ӏӐӶӸӺԀԐԱ՗ՙ՚աֈֺֻ֑֢֣־ֿ׀ׁ׃ׅׄא׫װ׳ءػـٖ٠٪ٮ۔ە۝۞ۮ۰ۿܐܭܰ݋ހ޲ँऄअऺ़ॎॐॕक़।०॰ঁ঄অ঍এ঑ও঩প঱ল঳শ঺়ঽা৅ে৉োৎৗ৘ড়৞য়৤০৻ਂਃਅ਋ਏ਑ਓ਩ਪ਱ਲ਴ਵ਷ਸ਺਼਽ਾ੃ੇ੉ੋ੎ਖ਼੝ਫ਼੟੦ੵઁ઄અઌઍ઎એ઒ઓ઩પ઱લ઴વ઺઼૆ે૊ો૎ૐ૑ૠૡ૦૰ଁ଄ଅ଍ଏ଑ଓ଩ପ଱ଲ଴ଶ଺଼ୄେ୉ୋ୎ୖ୘ଡ଼୞ୟୢ୦ୱஂ஄அ஋எ஑ஒ஖ங஛ஜ஝ஞ஠ண஥ந஫மஶஷ஺ா௃ெ௉ொ௎ௗ௘௧௳ఁఄఅ఍ఎ఑ఒ఩పఴవ఺ా౅ె౉ొ౎ౕ౗ౠౢ౦౰ಂ಄ಅ಍ಎ಑ಒ಩ಪ಴ವ಺ಾ೅ೆ೉ೊ೎ೕ೗ೞ೟ೠೢ೦೰ംഄഅ഍എ഑ഒഩപഺാൄെ൉ൊൎൗ൘ൠൢ൦൰ං඄අ඗ක඲ඳ඼ල඾ව෇්෋ා෕ූ෗ෘ෠ෲ෴ก฻฿๏๐๚ກ຃ຄ຅ງຉຊ຋ຍຎດຘນຠມ຤ລ຦ວຨສຬອ຺ົ຾ເ໅ໆ໇່໎໐໚ໜໞༀ༄༓༺༾཈ཉཫཱ྅྆ྌྐ྘ྙ྽྾࿍࿏࿐ကဢဣဨဩါာဳံ်၀၊ၐၚႠ჆აჹᄀᅚᅟᆣᆨᇺሀሇለቇቈ቉ቊ቎ቐ቗ቘ቙ቚ቞በኇኈ኉ኊ኎ነኯኰ኱ኲ኶ኸ኿ዀ዁ዂ዆ወዏዐ዗ዘዯደጏጐ጑ጒ጖ጘጟጠፇፈ፛፩፽ᎠᏵᐁ᙭ᙯᙷᚁ᚛ᚠ᛫ᛮᛱᜀᜍᜎ᜕ᜠ᜵ᝀ᝔ᝠ᝭ᝮ᝱ᝲ᝴ក។ៗ៘៛៝០៪᠋᠎᠐᠚ᠠᡸᢀᢪḀẜẠỺἀ἖Ἐ἞ἠ὆Ὀ὎ὐ὘Ὑ὚Ὓ὜Ὕ὞Ὗ὾ᾀ᾵ᾶ῅ῆ῔ῖ῜῝῰ῲ῵ῶ῿⁄⁅⁒⁓⁰⁲⁴⁽ⁿ₍₠₲⃫⃐℀℻ℽ⅌⅓ↄ←〈⌫⎴⎷⏏␀␧⑀⑋①⓿─☔☖☘☙♾⚀⚊✁✅✆✊✌✨✩❌❍❎❏❓❖❗❘❟❡❨❶➕➘➰➱➿⟐⟦⟰⦃⦙⧘⧜⧼⧾⬀⺀⺚⺛⻴⼀⿖⿰⿼〄〈〒〔〠〰〱〽〾぀ぁ゗゙゠ァ・ー㄀ㄅㄭㄱ㆏㆐ㆸㇰ㈝㈠㉄㉑㉼㉿㋌㋐㋿㌀㍷㍻㏞㏠㏿㐀䶶一龦ꀀ꒍꒐꓇가힤豈郞侮恵ﬀ﬇ﬓ﬘יִ﬷טּ﬽מּ﬿נּ﭂ףּ﭅צּ﮲ﯓ﴾ﵐ﶐ﶒ﷈ﷰ﷽︀︐︠︤﹢﹣﹤﹧﹩﹪ﹰ﹵ﹶ﻽＄％＋，０：＜？Ａ［＾＿｀｛｜｝～｟ｦ﾿ￂ￈ￊ￐ￒ￘ￚ￝￠￧￨￯￼￾";
			RegexCharClass._propTable = array;
			RegexCharClass._lcTable = new RegexCharClass.LowerCaseMapping[]
			{
				new RegexCharClass.LowerCaseMapping('A', 'Z', 1, 32),
				new RegexCharClass.LowerCaseMapping('À', 'Þ', 1, 32),
				new RegexCharClass.LowerCaseMapping('Ā', 'Į', 2, 0),
				new RegexCharClass.LowerCaseMapping('İ', 'İ', 0, 105),
				new RegexCharClass.LowerCaseMapping('Ĳ', 'Ķ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ĺ', 'Ň', 3, 0),
				new RegexCharClass.LowerCaseMapping('Ŋ', 'Ŷ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ÿ', 'Ÿ', 0, 255),
				new RegexCharClass.LowerCaseMapping('Ź', 'Ž', 3, 0),
				new RegexCharClass.LowerCaseMapping('Ɓ', 'Ɓ', 0, 595),
				new RegexCharClass.LowerCaseMapping('Ƃ', 'Ƅ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ɔ', 'Ɔ', 0, 596),
				new RegexCharClass.LowerCaseMapping('Ƈ', 'Ƈ', 0, 392),
				new RegexCharClass.LowerCaseMapping('Ɖ', 'Ɗ', 1, 205),
				new RegexCharClass.LowerCaseMapping('Ƌ', 'Ƌ', 0, 396),
				new RegexCharClass.LowerCaseMapping('Ǝ', 'Ǝ', 0, 477),
				new RegexCharClass.LowerCaseMapping('Ə', 'Ə', 0, 601),
				new RegexCharClass.LowerCaseMapping('Ɛ', 'Ɛ', 0, 603),
				new RegexCharClass.LowerCaseMapping('Ƒ', 'Ƒ', 0, 402),
				new RegexCharClass.LowerCaseMapping('Ɠ', 'Ɠ', 0, 608),
				new RegexCharClass.LowerCaseMapping('Ɣ', 'Ɣ', 0, 611),
				new RegexCharClass.LowerCaseMapping('Ɩ', 'Ɩ', 0, 617),
				new RegexCharClass.LowerCaseMapping('Ɨ', 'Ɨ', 0, 616),
				new RegexCharClass.LowerCaseMapping('Ƙ', 'Ƙ', 0, 409),
				new RegexCharClass.LowerCaseMapping('Ɯ', 'Ɯ', 0, 623),
				new RegexCharClass.LowerCaseMapping('Ɲ', 'Ɲ', 0, 626),
				new RegexCharClass.LowerCaseMapping('Ɵ', 'Ɵ', 0, 629),
				new RegexCharClass.LowerCaseMapping('Ơ', 'Ƥ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ƨ', 'Ƨ', 0, 424),
				new RegexCharClass.LowerCaseMapping('Ʃ', 'Ʃ', 0, 643),
				new RegexCharClass.LowerCaseMapping('Ƭ', 'Ƭ', 0, 429),
				new RegexCharClass.LowerCaseMapping('Ʈ', 'Ʈ', 0, 648),
				new RegexCharClass.LowerCaseMapping('Ư', 'Ư', 0, 432),
				new RegexCharClass.LowerCaseMapping('Ʊ', 'Ʋ', 1, 217),
				new RegexCharClass.LowerCaseMapping('Ƴ', 'Ƶ', 3, 0),
				new RegexCharClass.LowerCaseMapping('Ʒ', 'Ʒ', 0, 658),
				new RegexCharClass.LowerCaseMapping('Ƹ', 'Ƹ', 0, 441),
				new RegexCharClass.LowerCaseMapping('Ƽ', 'Ƽ', 0, 445),
				new RegexCharClass.LowerCaseMapping('Ǆ', 'ǅ', 0, 454),
				new RegexCharClass.LowerCaseMapping('Ǉ', 'ǈ', 0, 457),
				new RegexCharClass.LowerCaseMapping('Ǌ', 'ǋ', 0, 460),
				new RegexCharClass.LowerCaseMapping('Ǎ', 'Ǜ', 3, 0),
				new RegexCharClass.LowerCaseMapping('Ǟ', 'Ǯ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ǳ', 'ǲ', 0, 499),
				new RegexCharClass.LowerCaseMapping('Ǵ', 'Ǵ', 0, 501),
				new RegexCharClass.LowerCaseMapping('Ǻ', 'Ȗ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ά', 'Ά', 0, 940),
				new RegexCharClass.LowerCaseMapping('Έ', 'Ί', 1, 37),
				new RegexCharClass.LowerCaseMapping('Ό', 'Ό', 0, 972),
				new RegexCharClass.LowerCaseMapping('Ύ', 'Ώ', 1, 63),
				new RegexCharClass.LowerCaseMapping('Α', 'Ϋ', 1, 32),
				new RegexCharClass.LowerCaseMapping('Ϣ', 'Ϯ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ё', 'Џ', 1, 80),
				new RegexCharClass.LowerCaseMapping('А', 'Я', 1, 32),
				new RegexCharClass.LowerCaseMapping('Ѡ', 'Ҁ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ґ', 'Ҿ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ӂ', 'Ӄ', 3, 0),
				new RegexCharClass.LowerCaseMapping('Ӈ', 'Ӈ', 0, 1224),
				new RegexCharClass.LowerCaseMapping('Ӌ', 'Ӌ', 0, 1228),
				new RegexCharClass.LowerCaseMapping('Ӑ', 'Ӫ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ӯ', 'Ӵ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ӹ', 'Ӹ', 0, 1273),
				new RegexCharClass.LowerCaseMapping('Ա', 'Ֆ', 1, 48),
				new RegexCharClass.LowerCaseMapping('Ⴀ', 'Ⴥ', 1, 48),
				new RegexCharClass.LowerCaseMapping('Ḁ', 'Ỹ', 2, 0),
				new RegexCharClass.LowerCaseMapping('Ἀ', 'Ἇ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ἐ', '἟', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ἠ', 'Ἧ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ἰ', 'Ἷ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ὀ', 'Ὅ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ὑ', 'Ὑ', 0, 8017),
				new RegexCharClass.LowerCaseMapping('Ὓ', 'Ὓ', 0, 8019),
				new RegexCharClass.LowerCaseMapping('Ὕ', 'Ὕ', 0, 8021),
				new RegexCharClass.LowerCaseMapping('Ὗ', 'Ὗ', 0, 8023),
				new RegexCharClass.LowerCaseMapping('Ὠ', 'Ὧ', 1, -8),
				new RegexCharClass.LowerCaseMapping('ᾈ', 'ᾏ', 1, -8),
				new RegexCharClass.LowerCaseMapping('ᾘ', 'ᾟ', 1, -8),
				new RegexCharClass.LowerCaseMapping('ᾨ', 'ᾯ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ᾰ', 'Ᾱ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ὰ', 'Ά', 1, -74),
				new RegexCharClass.LowerCaseMapping('ᾼ', 'ᾼ', 0, 8115),
				new RegexCharClass.LowerCaseMapping('Ὲ', 'Ή', 1, -86),
				new RegexCharClass.LowerCaseMapping('ῌ', 'ῌ', 0, 8131),
				new RegexCharClass.LowerCaseMapping('Ῐ', 'Ῑ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ὶ', 'Ί', 1, -100),
				new RegexCharClass.LowerCaseMapping('Ῠ', 'Ῡ', 1, -8),
				new RegexCharClass.LowerCaseMapping('Ὺ', 'Ύ', 1, -112),
				new RegexCharClass.LowerCaseMapping('Ῥ', 'Ῥ', 0, 8165),
				new RegexCharClass.LowerCaseMapping('Ὸ', 'Ό', 1, -128),
				new RegexCharClass.LowerCaseMapping('Ὼ', 'Ώ', 1, -126),
				new RegexCharClass.LowerCaseMapping('ῼ', 'ῼ', 0, 8179),
				new RegexCharClass.LowerCaseMapping('Ⅰ', 'Ⅿ', 1, 16),
				new RegexCharClass.LowerCaseMapping('Ⓐ', 'ⓐ', 1, 26),
				new RegexCharClass.LowerCaseMapping('Ａ', 'Ｚ', 1, 32)
			};
			RegexCharClass._definedCategories = new Hashtable(31);
			char[] array2 = new char[9];
			StringBuilder stringBuilder = new StringBuilder(11);
			stringBuilder.Append('\0');
			array2[0] = '\0';
			array2[1] = '\u000f';
			RegexCharClass._definedCategories["Cc"] = array2[1].ToString();
			array2[2] = '\u0010';
			RegexCharClass._definedCategories["Cf"] = array2[2].ToString();
			array2[3] = '\u001e';
			RegexCharClass._definedCategories["Cn"] = array2[3].ToString();
			array2[4] = '\u0012';
			RegexCharClass._definedCategories["Co"] = array2[4].ToString();
			array2[5] = '\u0011';
			RegexCharClass._definedCategories["Cs"] = array2[5].ToString();
			array2[6] = '\0';
			RegexCharClass._definedCategories["C"] = new string(array2, 0, 7);
			array2[1] = '\u0002';
			RegexCharClass._definedCategories["Ll"] = array2[1].ToString();
			array2[2] = '\u0004';
			RegexCharClass._definedCategories["Lm"] = array2[2].ToString();
			array2[3] = '\u0005';
			RegexCharClass._definedCategories["Lo"] = array2[3].ToString();
			array2[4] = '\u0003';
			RegexCharClass._definedCategories["Lt"] = array2[4].ToString();
			array2[5] = '\u0001';
			RegexCharClass._definedCategories["Lu"] = array2[5].ToString();
			RegexCharClass._definedCategories["L"] = new string(array2, 0, 7);
			stringBuilder.Append(new string(array2, 1, 5));
			array2[1] = '\a';
			RegexCharClass._definedCategories["Mc"] = array2[1].ToString();
			array2[2] = '\b';
			RegexCharClass._definedCategories["Me"] = array2[2].ToString();
			array2[3] = '\u0006';
			RegexCharClass._definedCategories["Mn"] = array2[3].ToString();
			array2[4] = '\0';
			RegexCharClass._definedCategories["M"] = new string(array2, 0, 5);
			array2[1] = '\t';
			RegexCharClass._definedCategories["Nd"] = array2[1].ToString();
			array2[2] = '\n';
			RegexCharClass._definedCategories["Nl"] = array2[2].ToString();
			array2[3] = '\v';
			RegexCharClass._definedCategories["No"] = array2[3].ToString();
			RegexCharClass._definedCategories["N"] = new string(array2, 0, 5);
			stringBuilder.Append(array2[1]);
			array2[1] = '\u0013';
			RegexCharClass._definedCategories["Pc"] = array2[1].ToString();
			array2[2] = '\u0014';
			RegexCharClass._definedCategories["Pd"] = array2[2].ToString();
			array2[3] = '\u0016';
			RegexCharClass._definedCategories["Pe"] = array2[3].ToString();
			array2[4] = '\u0019';
			RegexCharClass._definedCategories["Po"] = array2[4].ToString();
			array2[5] = '\u0015';
			RegexCharClass._definedCategories["Ps"] = array2[5].ToString();
			array2[6] = '\u0018';
			RegexCharClass._definedCategories["Pf"] = array2[6].ToString();
			array2[7] = '\u0017';
			RegexCharClass._definedCategories["Pi"] = array2[7].ToString();
			array2[8] = '\0';
			RegexCharClass._definedCategories["P"] = new string(array2, 0, 9);
			stringBuilder.Append(array2[1]);
			array2[1] = '\u001b';
			RegexCharClass._definedCategories["Sc"] = array2[1].ToString();
			array2[2] = '\u001c';
			RegexCharClass._definedCategories["Sk"] = array2[2].ToString();
			array2[3] = '\u001a';
			RegexCharClass._definedCategories["Sm"] = array2[3].ToString();
			array2[4] = '\u001d';
			RegexCharClass._definedCategories["So"] = array2[4].ToString();
			array2[5] = '\0';
			RegexCharClass._definedCategories["S"] = new string(array2, 0, 6);
			array2[1] = '\r';
			RegexCharClass._definedCategories["Zl"] = array2[1].ToString();
			array2[2] = '\u000e';
			RegexCharClass._definedCategories["Zp"] = array2[2].ToString();
			array2[3] = '\f';
			RegexCharClass._definedCategories["Zs"] = array2[3].ToString();
			array2[4] = '\0';
			RegexCharClass._definedCategories["Z"] = new string(array2, 0, 5);
			stringBuilder.Append('\0');
			RegexCharClass.Word = stringBuilder.ToString();
			RegexCharClass.NotWord = RegexCharClass.NegateCategory(RegexCharClass.Word);
			RegexCharClass.SpaceClass = "\0\0\u0001" + RegexCharClass.Space;
			RegexCharClass.NotSpaceClass = "\u0001\0\u0001" + RegexCharClass.Space;
			RegexCharClass.WordClass = "\0\0" + (char)RegexCharClass.Word.Length + RegexCharClass.Word;
			RegexCharClass.NotWordClass = "\u0001\0" + (char)RegexCharClass.Word.Length + RegexCharClass.Word;
			RegexCharClass.DigitClass = "\0\0\u0001" + '\t';
			RegexCharClass.NotDigitClass = "\0\0\u0001" + '￷';
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000057EB File Offset: 0x000047EB
		internal RegexCharClass()
		{
			this._rangelist = new ArrayList(6);
			this._canonical = true;
			this._categories = new StringBuilder();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005811 File Offset: 0x00004811
		private RegexCharClass(bool negate, ArrayList ranges, StringBuilder categories, RegexCharClass subtraction)
		{
			this._rangelist = ranges;
			this._categories = categories;
			this._canonical = true;
			this._negate = negate;
			this._subtractor = subtraction;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000583D File Offset: 0x0000483D
		internal bool CanMerge
		{
			get
			{
				return !this._negate && this._subtractor == null;
			}
		}

		// Token: 0x17000024 RID: 36
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00005852 File Offset: 0x00004852
		internal bool Negate
		{
			set
			{
				this._negate = value;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000585B File Offset: 0x0000485B
		internal void AddChar(char c)
		{
			this.AddRange(c, c);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005868 File Offset: 0x00004868
		internal void AddCharClass(RegexCharClass cc)
		{
			if (!cc._canonical)
			{
				this._canonical = false;
			}
			else if (this._canonical && this.RangeCount() > 0 && cc.RangeCount() > 0 && cc.GetRangeAt(0)._first <= this.GetRangeAt(this.RangeCount() - 1)._last)
			{
				this._canonical = false;
			}
			for (int i = 0; i < cc.RangeCount(); i++)
			{
				this._rangelist.Add(cc.GetRangeAt(i));
			}
			this._categories.Append(cc._categories.ToString());
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005904 File Offset: 0x00004904
		private void AddSet(string set)
		{
			if (this._canonical && this.RangeCount() > 0 && set.Length > 0 && set[0] <= this.GetRangeAt(this.RangeCount() - 1)._last)
			{
				this._canonical = false;
			}
			int i;
			for (i = 0; i < set.Length - 1; i += 2)
			{
				this._rangelist.Add(new RegexCharClass.SingleRange(set[i], set[i + 1] - '\u0001'));
			}
			if (i < set.Length)
			{
				this._rangelist.Add(new RegexCharClass.SingleRange(set[i], char.MaxValue));
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000059AB File Offset: 0x000049AB
		internal void AddSubtraction(RegexCharClass sub)
		{
			this._subtractor = sub;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000059B4 File Offset: 0x000049B4
		internal void AddRange(char first, char last)
		{
			this._rangelist.Add(new RegexCharClass.SingleRange(first, last));
			if (this._canonical && this._rangelist.Count > 0 && first <= ((RegexCharClass.SingleRange)this._rangelist[this._rangelist.Count - 1])._last)
			{
				this._canonical = false;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005A18 File Offset: 0x00004A18
		internal void AddCategoryFromName(string categoryName, bool invert, bool caseInsensitive, string pattern)
		{
			object obj = RegexCharClass._definedCategories[categoryName];
			if (obj != null)
			{
				string text = (string)obj;
				if (caseInsensitive && (categoryName.Equals("Lu") || categoryName.Equals("Lt")))
				{
					text = (string)RegexCharClass._definedCategories["Ll"];
				}
				if (invert)
				{
					text = RegexCharClass.NegateCategory(text);
				}
				this._categories.Append(text);
				return;
			}
			this.AddSet(RegexCharClass.SetFromProperty(categoryName, invert, pattern));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005A94 File Offset: 0x00004A94
		private void AddCategory(string category)
		{
			this._categories.Append(category);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005AA4 File Offset: 0x00004AA4
		internal void AddLowercase(CultureInfo culture)
		{
			this._canonical = false;
			int i = 0;
			int count = this._rangelist.Count;
			while (i < count)
			{
				RegexCharClass.SingleRange singleRange = (RegexCharClass.SingleRange)this._rangelist[i];
				if (singleRange._first == singleRange._last)
				{
					singleRange._first = (singleRange._last = char.ToLower(singleRange._first, culture));
				}
				else
				{
					this.AddLowercaseRange(singleRange._first, singleRange._last, culture);
				}
				i++;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005B20 File Offset: 0x00004B20
		private void AddLowercaseRange(char chMin, char chMax, CultureInfo culture)
		{
			int i = 0;
			int num = RegexCharClass._lcTable.Length;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (RegexCharClass._lcTable[num2]._chMax < chMin)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			if (i >= RegexCharClass._lcTable.Length)
			{
				return;
			}
			RegexCharClass.LowerCaseMapping lowerCaseMapping;
			while (i < RegexCharClass._lcTable.Length && (lowerCaseMapping = RegexCharClass._lcTable[i])._chMin <= chMax)
			{
				char c;
				if ((c = lowerCaseMapping._chMin) < chMin)
				{
					c = chMin;
				}
				char c2;
				if ((c2 = lowerCaseMapping._chMax) > chMax)
				{
					c2 = chMax;
				}
				switch (lowerCaseMapping._lcOp)
				{
				case 0:
					c = (char)lowerCaseMapping._data;
					c2 = (char)lowerCaseMapping._data;
					break;
				case 1:
					c += (char)lowerCaseMapping._data;
					c2 += (char)lowerCaseMapping._data;
					break;
				case 2:
					c |= '\u0001';
					c2 |= '\u0001';
					break;
				case 3:
					c += (c & '\u0001');
					c2 += (c2 & '\u0001');
					break;
				}
				if (c < chMin || c2 > chMax)
				{
					this.AddRange(c, c2);
				}
				i++;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005C3C File Offset: 0x00004C3C
		internal void AddWord(bool ecma, bool negate)
		{
			if (negate)
			{
				if (ecma)
				{
					this.AddSet("\00:A[_`a{İı");
					return;
				}
				this.AddCategory(RegexCharClass.NotWord);
				return;
			}
			else
			{
				if (ecma)
				{
					this.AddSet("0:A[_`a{İı");
					return;
				}
				this.AddCategory(RegexCharClass.Word);
				return;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005C76 File Offset: 0x00004C76
		internal void AddSpace(bool ecma, bool negate)
		{
			if (negate)
			{
				if (ecma)
				{
					this.AddSet("\0\t\u000e !");
					return;
				}
				this.AddCategory(RegexCharClass.NotSpace);
				return;
			}
			else
			{
				if (ecma)
				{
					this.AddSet("\t\u000e !");
					return;
				}
				this.AddCategory(RegexCharClass.Space);
				return;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005CB0 File Offset: 0x00004CB0
		internal void AddDigit(bool ecma, bool negate, string pattern)
		{
			if (!ecma)
			{
				this.AddCategoryFromName("Nd", negate, false, pattern);
				return;
			}
			if (negate)
			{
				this.AddSet("\00:");
				return;
			}
			this.AddSet("0:");
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005CE0 File Offset: 0x00004CE0
		internal static string ConvertOldStringsToClass(string set, string category)
		{
			StringBuilder stringBuilder = new StringBuilder(set.Length + category.Length + 3);
			if (set.Length >= 2 && set[0] == '\0' && set[1] == '\0')
			{
				stringBuilder.Append('\u0001');
				stringBuilder.Append((char)(set.Length - 2));
				stringBuilder.Append((char)category.Length);
				stringBuilder.Append(set.Substring(2));
			}
			else
			{
				stringBuilder.Append('\0');
				stringBuilder.Append((char)set.Length);
				stringBuilder.Append((char)category.Length);
				stringBuilder.Append(set);
			}
			stringBuilder.Append(category);
			return stringBuilder.ToString();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005D8D File Offset: 0x00004D8D
		internal static char SingletonChar(string set)
		{
			return set[3];
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005D96 File Offset: 0x00004D96
		internal static bool IsMergeable(string charClass)
		{
			return !RegexCharClass.IsNegated(charClass) && !RegexCharClass.IsSubtraction(charClass);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005DAB File Offset: 0x00004DAB
		internal static bool IsEmpty(string charClass)
		{
			return charClass[2] == '\0' && charClass[0] == '\0' && charClass[1] == '\0' && !RegexCharClass.IsSubtraction(charClass);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005DD4 File Offset: 0x00004DD4
		internal static bool IsSingleton(string set)
		{
			return set[0] == '\0' && set[2] == '\0' && set[1] == '\u0002' && !RegexCharClass.IsSubtraction(set) && (set[3] == char.MaxValue || set[3] + '\u0001' == set[4]);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005E28 File Offset: 0x00004E28
		internal static bool IsSingletonInverse(string set)
		{
			return set[0] == '\u0001' && set[2] == '\0' && set[1] == '\u0002' && !RegexCharClass.IsSubtraction(set) && (set[3] == char.MaxValue || set[3] + '\u0001' == set[4]);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005E7D File Offset: 0x00004E7D
		private static bool IsSubtraction(string charClass)
		{
			return charClass.Length > (int)('\u0003' + charClass[1] + charClass[2]);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005E98 File Offset: 0x00004E98
		internal static bool IsNegated(string set)
		{
			return set != null && set[0] == '\u0001';
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005EA9 File Offset: 0x00004EA9
		internal static bool IsECMAWordChar(char ch)
		{
			return RegexCharClass.CharInClass(ch, "\0\n\00:A[_`a{İı");
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005EB6 File Offset: 0x00004EB6
		internal static bool IsWordChar(char ch)
		{
			return RegexCharClass.CharInClass(ch, RegexCharClass.WordClass);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005EC3 File Offset: 0x00004EC3
		internal static bool CharInClass(char ch, string set)
		{
			return RegexCharClass.CharInClassRecursive(ch, set, 0);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005ED0 File Offset: 0x00004ED0
		internal static bool CharInClassRecursive(char ch, string set, int start)
		{
			int num = (int)set[start + 1];
			int num2 = (int)set[start + 2];
			int num3 = start + 3 + num + num2;
			bool flag = false;
			if (set.Length > num3)
			{
				flag = RegexCharClass.CharInClassRecursive(ch, set, num3);
			}
			bool flag2 = RegexCharClass.CharInClassInternal(ch, set, start, num, num2);
			if (set[start] == '\u0001')
			{
				flag2 = !flag2;
			}
			return flag2 && !flag;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005F34 File Offset: 0x00004F34
		private static bool CharInClassInternal(char ch, string set, int start, int mySetLength, int myCategoryLength)
		{
			int num = start + 3;
			int num2 = num + mySetLength;
			while (num != num2)
			{
				int num3 = (num + num2) / 2;
				if (ch < set[num3])
				{
					num2 = num3;
				}
				else
				{
					num = num3 + 1;
				}
			}
			return (num & 1) == (start & 1) || (myCategoryLength != 0 && RegexCharClass.CharInCategory(ch, set, start, mySetLength, myCategoryLength));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005F84 File Offset: 0x00004F84
		private static bool CharInCategory(char ch, string set, int start, int mySetLength, int myCategoryLength)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(ch);
			int i = start + 3 + mySetLength;
			int num = i + myCategoryLength;
			while (i < num)
			{
				int num2 = (int)((short)set[i]);
				if (num2 == 0)
				{
					if (RegexCharClass.CharInCategoryGroup(ch, unicodeCategory, set, ref i))
					{
						return true;
					}
				}
				else if (num2 > 0)
				{
					if (num2 == 100)
					{
						if (char.IsWhiteSpace(ch))
						{
							return true;
						}
						i++;
						continue;
					}
					else
					{
						num2--;
						if (unicodeCategory == (UnicodeCategory)num2)
						{
							return true;
						}
					}
				}
				else if (num2 == -100)
				{
					if (!char.IsWhiteSpace(ch))
					{
						return true;
					}
					i++;
					continue;
				}
				else
				{
					num2 = -1 - num2;
					if (unicodeCategory != (UnicodeCategory)num2)
					{
						return true;
					}
				}
				i++;
			}
			return false;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000600C File Offset: 0x0000500C
		private static bool CharInCategoryGroup(char ch, UnicodeCategory chcategory, string category, ref int i)
		{
			i++;
			int num = (int)((short)category[i]);
			if (num > 0)
			{
				bool flag = false;
				while (num != 0)
				{
					if (!flag)
					{
						num--;
						if (chcategory == (UnicodeCategory)num)
						{
							flag = true;
						}
					}
					i++;
					num = (int)((short)category[i]);
				}
				return flag;
			}
			bool flag2 = true;
			while (num != 0)
			{
				if (flag2)
				{
					num = -1 - num;
					if (chcategory == (UnicodeCategory)num)
					{
						flag2 = false;
					}
				}
				i++;
				num = (int)((short)category[i]);
			}
			return flag2;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006078 File Offset: 0x00005078
		private static string NegateCategory(string category)
		{
			if (category == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(category.Length);
			foreach (short num in category)
			{
				stringBuilder.Append((char)(-(char)num));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000060C0 File Offset: 0x000050C0
		internal static RegexCharClass Parse(string charClass)
		{
			return RegexCharClass.ParseRecursive(charClass, 0);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000060CC File Offset: 0x000050CC
		private static RegexCharClass ParseRecursive(string charClass, int start)
		{
			int num = (int)charClass[start + 1];
			int num2 = (int)charClass[start + 2];
			int num3 = start + 3 + num + num2;
			ArrayList arrayList = new ArrayList(num);
			int i = start + 3;
			int num4 = i + num;
			while (i < num4)
			{
				char first = charClass[i];
				i++;
				char last;
				if (i < num4)
				{
					last = charClass[i] - '\u0001';
				}
				else
				{
					last = char.MaxValue;
				}
				i++;
				arrayList.Add(new RegexCharClass.SingleRange(first, last));
			}
			RegexCharClass subtraction = null;
			if (charClass.Length > num3)
			{
				subtraction = RegexCharClass.ParseRecursive(charClass, num3);
			}
			return new RegexCharClass(charClass[start] == '\u0001', arrayList, new StringBuilder(charClass.Substring(num4, num2)), subtraction);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006186 File Offset: 0x00005186
		private int RangeCount()
		{
			return this._rangelist.Count;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00006194 File Offset: 0x00005194
		internal string ToStringClass()
		{
			if (!this._canonical)
			{
				this.Canonicalize();
			}
			int num = this._rangelist.Count * 2;
			StringBuilder stringBuilder = new StringBuilder(num + this._categories.Length + 3);
			int num2;
			if (this._negate)
			{
				num2 = 1;
			}
			else
			{
				num2 = 0;
			}
			stringBuilder.Append((char)num2);
			stringBuilder.Append((char)num);
			stringBuilder.Append((char)this._categories.Length);
			for (int i = 0; i < this._rangelist.Count; i++)
			{
				RegexCharClass.SingleRange singleRange = (RegexCharClass.SingleRange)this._rangelist[i];
				stringBuilder.Append(singleRange._first);
				if (singleRange._last != '￿')
				{
					stringBuilder.Append(singleRange._last + '\u0001');
				}
			}
			stringBuilder[1] = (char)(stringBuilder.Length - 3);
			stringBuilder.Append(this._categories);
			if (this._subtractor != null)
			{
				stringBuilder.Append(this._subtractor.ToStringClass());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006298 File Offset: 0x00005298
		private RegexCharClass.SingleRange GetRangeAt(int i)
		{
			return (RegexCharClass.SingleRange)this._rangelist[i];
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000062AC File Offset: 0x000052AC
		private void Canonicalize()
		{
			this._canonical = true;
			this._rangelist.Sort(0, this._rangelist.Count, new RegexCharClass.SingleRangeComparer());
			if (this._rangelist.Count > 1)
			{
				bool flag = false;
				int num = 1;
				int num2 = 0;
				for (;;)
				{
					IL_3B:
					char last = ((RegexCharClass.SingleRange)this._rangelist[num2])._last;
					while (num != this._rangelist.Count && last != '￿')
					{
						RegexCharClass.SingleRange singleRange;
						if ((singleRange = (RegexCharClass.SingleRange)this._rangelist[num])._first <= last + '\u0001')
						{
							if (last < singleRange._last)
							{
								last = singleRange._last;
							}
							num++;
						}
						else
						{
							IL_A0:
							((RegexCharClass.SingleRange)this._rangelist[num2])._last = last;
							num2++;
							if (!flag)
							{
								if (num2 < num)
								{
									this._rangelist[num2] = this._rangelist[num];
								}
								num++;
								goto IL_3B;
							}
							goto IL_E4;
						}
					}
					flag = true;
					goto IL_A0;
				}
				IL_E4:
				this._rangelist.RemoveRange(num2, this._rangelist.Count - num2);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000063B8 File Offset: 0x000053B8
		private static string SetFromProperty(string capname, bool invert, string pattern)
		{
			int num = 0;
			int num2 = RegexCharClass._propTable.GetLength(0);
			while (num != num2)
			{
				int num3 = (num + num2) / 2;
				int num4 = string.Compare(capname, RegexCharClass._propTable[num3, 0], StringComparison.Ordinal);
				if (num4 < 0)
				{
					num2 = num3;
				}
				else if (num4 > 0)
				{
					num = num3 + 1;
				}
				else
				{
					string text = RegexCharClass._propTable[num3, 1];
					if (!invert)
					{
						return text;
					}
					if (text[0] == '\0')
					{
						return text.Substring(1);
					}
					return '\0' + text;
				}
			}
			throw new ArgumentException(SR.GetString("MakeException", new object[]
			{
				pattern,
				SR.GetString("UnknownProperty", new object[]
				{
					capname
				})
			}));
		}

		// Token: 0x04000654 RID: 1620
		private const int FLAGS = 0;

		// Token: 0x04000655 RID: 1621
		private const int SETLENGTH = 1;

		// Token: 0x04000656 RID: 1622
		private const int CATEGORYLENGTH = 2;

		// Token: 0x04000657 RID: 1623
		private const int SETSTART = 3;

		// Token: 0x04000658 RID: 1624
		private const char Nullchar = '\0';

		// Token: 0x04000659 RID: 1625
		private const char Lastchar = '￿';

		// Token: 0x0400065A RID: 1626
		private const char GroupChar = '\0';

		// Token: 0x0400065B RID: 1627
		private const short SpaceConst = 100;

		// Token: 0x0400065C RID: 1628
		private const short NotSpaceConst = -100;

		// Token: 0x0400065D RID: 1629
		private const string ECMASpaceSet = "\t\u000e !";

		// Token: 0x0400065E RID: 1630
		private const string NotECMASpaceSet = "\0\t\u000e !";

		// Token: 0x0400065F RID: 1631
		private const string ECMAWordSet = "0:A[_`a{İı";

		// Token: 0x04000660 RID: 1632
		private const string NotECMAWordSet = "\00:A[_`a{İı";

		// Token: 0x04000661 RID: 1633
		private const string ECMADigitSet = "0:";

		// Token: 0x04000662 RID: 1634
		private const string NotECMADigitSet = "\00:";

		// Token: 0x04000663 RID: 1635
		internal const string ECMASpaceClass = "\0\u0004\0\t\u000e !";

		// Token: 0x04000664 RID: 1636
		internal const string NotECMASpaceClass = "\u0001\u0004\0\t\u000e !";

		// Token: 0x04000665 RID: 1637
		internal const string ECMAWordClass = "\0\n\00:A[_`a{İı";

		// Token: 0x04000666 RID: 1638
		internal const string NotECMAWordClass = "\u0001\n\00:A[_`a{İı";

		// Token: 0x04000667 RID: 1639
		internal const string ECMADigitClass = "\0\u0002\00:";

		// Token: 0x04000668 RID: 1640
		internal const string NotECMADigitClass = "\u0001\u0002\00:";

		// Token: 0x04000669 RID: 1641
		internal const string AnyClass = "\0\u0001\0\0";

		// Token: 0x0400066A RID: 1642
		internal const string EmptyClass = "\0\0\0";

		// Token: 0x0400066B RID: 1643
		private const int LowercaseSet = 0;

		// Token: 0x0400066C RID: 1644
		private const int LowercaseAdd = 1;

		// Token: 0x0400066D RID: 1645
		private const int LowercaseBor = 2;

		// Token: 0x0400066E RID: 1646
		private const int LowercaseBad = 3;

		// Token: 0x0400066F RID: 1647
		private ArrayList _rangelist;

		// Token: 0x04000670 RID: 1648
		private StringBuilder _categories;

		// Token: 0x04000671 RID: 1649
		private bool _canonical;

		// Token: 0x04000672 RID: 1650
		private bool _negate;

		// Token: 0x04000673 RID: 1651
		private RegexCharClass _subtractor;

		// Token: 0x04000674 RID: 1652
		private static readonly string Space = "d";

		// Token: 0x04000675 RID: 1653
		private static readonly string NotSpace = RegexCharClass.NegateCategory(RegexCharClass.Space);

		// Token: 0x04000676 RID: 1654
		private static readonly string Word;

		// Token: 0x04000677 RID: 1655
		private static readonly string NotWord;

		// Token: 0x04000678 RID: 1656
		internal static readonly string SpaceClass;

		// Token: 0x04000679 RID: 1657
		internal static readonly string NotSpaceClass;

		// Token: 0x0400067A RID: 1658
		internal static readonly string WordClass;

		// Token: 0x0400067B RID: 1659
		internal static readonly string NotWordClass;

		// Token: 0x0400067C RID: 1660
		internal static readonly string DigitClass;

		// Token: 0x0400067D RID: 1661
		internal static readonly string NotDigitClass;

		// Token: 0x0400067E RID: 1662
		private static Hashtable _definedCategories;

		// Token: 0x0400067F RID: 1663
		private static readonly string[,] _propTable;

		// Token: 0x04000680 RID: 1664
		private static readonly RegexCharClass.LowerCaseMapping[] _lcTable;

		// Token: 0x02000014 RID: 20
		private struct LowerCaseMapping
		{
			// Token: 0x060000B4 RID: 180 RVA: 0x00006475 File Offset: 0x00005475
			internal LowerCaseMapping(char chMin, char chMax, int lcOp, int data)
			{
				this._chMin = chMin;
				this._chMax = chMax;
				this._lcOp = lcOp;
				this._data = data;
			}

			// Token: 0x04000681 RID: 1665
			internal char _chMin;

			// Token: 0x04000682 RID: 1666
			internal char _chMax;

			// Token: 0x04000683 RID: 1667
			internal int _lcOp;

			// Token: 0x04000684 RID: 1668
			internal int _data;
		}

		// Token: 0x02000015 RID: 21
		private sealed class SingleRangeComparer : IComparer
		{
			// Token: 0x060000B5 RID: 181 RVA: 0x00006494 File Offset: 0x00005494
			public int Compare(object x, object y)
			{
				if (((RegexCharClass.SingleRange)x)._first < ((RegexCharClass.SingleRange)y)._first)
				{
					return -1;
				}
				if (((RegexCharClass.SingleRange)x)._first <= ((RegexCharClass.SingleRange)y)._first)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000016 RID: 22
		private sealed class SingleRange
		{
			// Token: 0x060000B7 RID: 183 RVA: 0x000064D3 File Offset: 0x000054D3
			internal SingleRange(char first, char last)
			{
				this._first = first;
				this._last = last;
			}

			// Token: 0x04000685 RID: 1669
			internal char _first;

			// Token: 0x04000686 RID: 1670
			internal char _last;
		}
	}
}
