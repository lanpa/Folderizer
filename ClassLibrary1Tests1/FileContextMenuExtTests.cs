using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSShellExtContextMenuHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSShellExtContextMenuHandler.Tests
{
    [TestClass()]
    public class FileContextMenuExtTests
    {
        [TestMethod()]
        public void commomFileNameTest()
        {
            FileContextMenuExt fc = new FileContextMenuExt();

            List<string> inputFileNames = new List<string>();
            inputFileNames.Add("bananamango-280.jpg");
            inputFileNames.Add("bananamango-280_large.jpg");
            Assert.AreEqual("bananamango-280", fc.commomFileName(inputFileNames));

            inputFileNames = new List<string>();
            inputFileNames.Add("midd123.txt");
            inputFileNames.Add("midd123 - copy.txt");
            Assert.AreEqual("midd123", fc.commomFileName(inputFileNames));

            inputFileNames = new List<string>();
            inputFileNames.Add("bananamango-280 .jpg");
            inputFileNames.Add("bananamango-280 large.jpg");
            string tmp = fc.commomFileName(inputFileNames);
            Assert.AreEqual("bananamango-280", fc.commomFileName(inputFileNames));

            inputFileNames = new List<string>();
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -01[夢の中で逢った、ような……]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -02[それはとっても嬉しいなって]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -03[もう何も怖くない]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -04[奇跡も、魔法も、あるんだよ]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -05[後悔なんて、あるわけない]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -06[こんなの絶対おかしいよ]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -07[本当の気持ちと向き合えますか?]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -08[あたしって、ほんとバカ]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -09[そんなの、あたしが許さない]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -10[もう誰にも頼らない]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -11[最後に残った道しるべ]");
            inputFileNames.Add("魔法少女小圓 (YYDM-11FANS) -12[わたしの、最高の友達]");
            inputFileNames.Add("魔法少女小圓 (華盟&澄空) 字幕檔[BDRIP][X264_AAC][1280X720].rar");

            Assert.AreEqual("魔法少女小圓 (YYDM-11FANS) -", fc.commomFileName(inputFileNames));

            //special characters
            inputFileNames = new List<string>();
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -01[夢の中で逢った、ような……]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -02[それはとっても嬉しいなって]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -03[もう何も怖くない]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -04[奇跡も、魔法も、あるんだよ]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -05[後悔なんて、あるわけない]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -06[こんなの絶対おかしいよ]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -07[本当の気持ちと向き合えますか?]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -08[あたしって、ほんとバカ]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -09[そんなの、あたしが許さない]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -10[もう誰にも頼らない]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -11[最後に残った道しるべ]");
            inputFileNames.Add("まどか☆マギカ (YYDM-11FANS) -12[わたしの、最高の友達]");
            inputFileNames.Add("まどか☆マギカ (華盟&澄空) 字幕檔[BDRIP][X264_AAC][1280X720].rar");

            Assert.AreEqual("まどか☆マギカ (YYDM-11FANS) -", fc.commomFileName(inputFileNames));


            //Assert.Fail();
        }
    }
}