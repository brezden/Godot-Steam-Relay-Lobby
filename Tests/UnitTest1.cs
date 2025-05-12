using Xunit;

public class SanityTest
{
    [Fact]
    public void AddsTwoPlusTwo()
    {
        Assert.Equal(4, 2 + 2);
    }
}