using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TimeSheetApp.Model;
using TimeSheetApp.Model.Client;
using TimeSheetApp.Model.EntitiesBase;

using Process = TimeSheetApp.Model.EntitiesBase.Process;

namespace TimeSheetTests
{
    [TestClass]
    public class TSClientDataProviderTests : DataProviderTest
    {
        
        public override IDataProvider DataProvider { get; set; }
        public override string ProviderName { get => $"{nameof(TSClientDataProviderTests)}";}

        [TestInitialize]
        public override void Setup()
        {
            DataProvider = new TimeSheetClient();
        }
        
        [TestCleanup]
        public override void Cleanup()
        {

        }
    }
}
