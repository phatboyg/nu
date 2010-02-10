namespace nu.Specs.Nugs
{
    using System;
    using core.Configuration;
    using core.FileSystem;
    using core.Nugs;
    using NUnit.Framework;

    [TestFixture]
    public class Serialization_Specs
    {
        [Test]
        public void NAME()
        {
            var m = new Manifest();
            m.Name = "log4net";
            m.Version = "1.1";
            m.Files.Add(new ManifestEntry()
                {
                    Name ="log4net.dll"
                });

            Console.WriteLine(JsonUtil.ToJson(m));
        }
    }
}