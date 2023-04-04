using MtpSharp;

namespace Tests;

public class Tests
{
    [Test]
    public void Test1()
    {
        string string1 = "1";
        string string2 = "a";

        int result = string1.NaturalCompare(string2);
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void TestDifferentDigitLength()
    {
        string string1 = "File 2";
        string string2 = "File 10";

        int result = string1.NaturalCompare(string2);
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void TestWithLeadingZeros()
    {
        string string1 = "File 0000000000000000000002";
        string string2 = "File 10";

        int result = string1.NaturalCompare(string2);
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void TestWithIndex()
    {
        List<string> test = new() { "File 2", "File 10", "File 1" };

        int zeroIndex = 0;
        foreach ((string item, int index) item in test.WithIndex(2))
        {
            Assert.That(item.index, Is.EqualTo(zeroIndex + 2));
            zeroIndex++;
        }
    }
}
