using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MT.CSGPortal.DAL;
using MT.CSGPortal.Entities;
using MT.CSGPortal.Portable.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MT.CSGPortal.Utility.Tests
{
    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void SearchArchitectsTest()
        {
            var obj = new Search();
            SearchResult<MindBasicProfile> mindProfileDtoObj;
            IMindDataAccess dataAccessObj = new MindDataAccess();
            List<MindBasicProfile> mindProfileLstObj = new List<MindBasicProfile>();
            mindProfileLstObj = dataAccessObj.GetAllMinds.Select(m => new MindBasicProfile(m)).ToList<MindBasicProfile>();
            //var luceneSearchObj = new LuceneSearch();
            //perform lucene index search
            mindProfileDtoObj = obj.SearchProfilesByDesignation(mindProfileLstObj, "", 1);
            Assert.IsNotNull(mindProfileDtoObj);
        }
    }
}
