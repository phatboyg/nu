namespace nu.Configuration
{
    using core.Configuration;
    using core.FileSystem;
    using NDepend.Helpers.FileDirectoryPath;
    using NUnit.Framework;
    using Rhino.Mocks;

    public class ProjectConfiguration_Specs
    {
        [Test]
        public void Project_Should_Fall_Back_On_Global()
        {
            var glob = MockRepository.GenerateStub<GlobalConfiguration>();
            var fs = MockRepository.GenerateStub<FileSystem>();

            fs.Stub(f => f.ProjectConfig).Return(new FilePathRelative(@".\bob.txt"));
            fs.Stub(f => f.FileExists(@".\bob.txt")).Return(false);
            glob["name"] = "dru";

            var proj = new ProjectFileBasedConfiguration(fs, glob);

            Assert.AreEqual("dru", proj["name"]);
        }
        
    }
}