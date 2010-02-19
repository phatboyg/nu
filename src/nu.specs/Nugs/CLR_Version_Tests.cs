namespace nu.Specs.Nugs
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CLR_Version_Tests
    {
        [Test]
        public void NAME()
        {
            Version v1 = new Version(0,0);
            Version v2 = new Version(0,1);
            Assert.That(v2, Is.GreaterThan(v1));

            Version v3 = new Version(1,0);
            Version v4 = new Version(1,1);
            Assert.That(v3, Is.LessThan(v4));


            Assert.That("1.1", Is.EqualTo(v4.ToString()));
        }
    }
}