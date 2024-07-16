using SynonymSearchApp_ApplicationCore.IServices;
using SynonymSearchApp_ApplicationCore.Services;
using SynonymSearchApp_Domain.Models;

namespace SynonymSearchApp_Tests
{
    public class SynonymSearchAppTests
    {
        private readonly SynonymSearchService _synonymSearchService;
        public SynonymSearchAppTests()
        {
            _synonymSearchService = new SynonymSearchService();
        }

        [Fact]
        public void TestSynonymSearch_ShouldReturnExpectedResult1()
        {
            _synonymSearchService.AddSynonym(new Synonym { Key = "b", Value = "a" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "c", Value = "b" });

            var result = _synonymSearchService.GetSynonymList("a");
            Assert.Equal(["b", "c"], result);
        }

        [Fact]
        public void TestSynonymSearch_ShouldReturnExpectedResult2()
        {
            _synonymSearchService.AddSynonym(new Synonym { Key = "wash", Value = "clean" });

            var result = _synonymSearchService.GetSynonymList("wash");
            Assert.Equal(["clean"], result);
        }

        [Fact]
        public void TestSynonymSearch_ShouldReturnExpectedResult3()
        {
            _synonymSearchService.AddSynonym(new Synonym { Key = "wash", Value = "clean" });

            var result = _synonymSearchService.GetSynonymList("clean");
            Assert.Equal(["wash"], result);
        }

        [Fact]
        public void TestSynonymSearch_ShouldReturnExpectedResult4()
        {
            _synonymSearchService.AddSynonym(new Synonym { Key = "cura", Value = "curica" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "cura", Value = "djevojcica" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "djevojcica", Value = "dijete" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "seka", Value = "djevojcica" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "žena", Value = "cura" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "žena", Value = "osoba" });
            _synonymSearchService.AddSynonym(new Synonym { Key = "teta", Value = "žena" });
            var result = _synonymSearchService.GetSynonymList("cura");
            Assert.Equal(["curica", "djevojcica", "dijete", "seka", "žena", "osoba", "teta" ], result);
        }
    }
}