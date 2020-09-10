using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TimeSheetApp.Model;

namespace TimeSheetTests
{
    [TestClass]
    public class EFDataProviderTests : DataProviderTest
    {

        public override IDataProvider DataProvider { get; set; }

        public override string ProviderName => $"{nameof(EFDataProviderTests)}";

        [TestInitialize]
        public override void Setup()
        {
            DataProvider = new EFDataProvider();
        }

        [TestCleanup]
        public override void Cleanup()
        {

        }
    }
}
