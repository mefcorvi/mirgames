namespace MirGames.Specs.Markdown
{
    using NUnit.Framework;

    [TestFixture]
    public sealed class MarkdownTextTransformTest
    {
        [Test]
        public void Test_Img_Link()
        {
            var transform = new MarkdownTextProcessor();
            var result = transform.GetHtml(@"[![Аннотация](http://assets.servedby-buysellads.com/p/manage/asset/id/18408)](http://ya.ru/)");
            var expected =
                "<p><a href=\"http://ya.ru/\" rel=\"nofollow\" target=\"_blank\"><img src=\"http://assets.servedby-buysellads.com/p/manage/asset/id/18408\" alt=\"Аннотация\" /></a></p>\n";

            Assert.AreEqual(expected, result);
        }
    }
}
