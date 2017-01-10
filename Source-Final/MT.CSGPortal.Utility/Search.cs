using System.Collections.Generic;
using System.Linq;

using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using MT.CSGPortal.Entities;

using Version = Lucene.Net.Util;

namespace MT.CSGPortal.Utility
{
    public class Search 
    {
        #region <<public methods>>

        public SearchResult<TEntity> SearchProfilesByDesignation<TEntity>(List<TEntity> entityLstObj, string searchParameter, int pageNumber) where TEntity : new()
        {
            string[] fieldValues = new string[1] { "Designation" };
            return SearchProfilesPaged(entityLstObj,searchParameter,pageNumber,ApplicationSettingsReader.PageSizeForLuceneSearch, fieldValues);
        }

        public SearchResult<TEntity> SearchProfilesPaged<TEntity>(List<TEntity> entityLstObj, string searchParameter, int pageNumber,int pageSize, string[] fieldValues) where TEntity : new()
        {
            LuceneSearch.CreateIndexDataGeneric(entityLstObj);

            SearchResult<TEntity> searchResultsObj = new SearchResult<TEntity>();

            if (!string.IsNullOrEmpty(searchParameter))
            {
                using (Directory directory = FSDirectory.Open(LuceneSearch.INDEXPATH))
                {
                    using (var indexSearcher = new IndexSearcher(directory))
                    {
                        Query queryObj;
                        using (var analyzer = new StandardAnalyzer(Version.Version.LUCENE_30))
                        {
                            var fieldParserObj = new MultiFieldQueryParser(Version.Version.LUCENE_30, fieldValues, analyzer);
                            queryObj = fieldParserObj.Parse(searchParameter + LuceneSearch.likeChar);
                            fieldParserObj.DefaultOperator = QueryParser.Operator.OR;
                            fieldParserObj.AllowLeadingWildcard = true;
                        }

                        //perform search on the index
                        TopDocs topDocs = indexSearcher.Search(queryObj, pageSize);

                        //populate the search results to searchResultsDto object
                        searchResultsObj = LuceneSearch.PopulateSearchResultDTO<TEntity>(indexSearcher, topDocs, pageNumber);
                    }
                }
            }
            else
            {
                int resultStartIndex = 1 + ((pageNumber - 1) * ApplicationSettingsReader.PageSizeForLuceneSearch);
                int resultEndIndex = pageNumber * pageSize;
                searchResultsObj.EndOfRecords = true;
                if (entityLstObj.Count < resultEndIndex)
                    resultEndIndex = entityLstObj.Count;
                else
                    searchResultsObj.EndOfRecords = false;
                searchResultsObj.TotalRecordCount = entityLstObj.Count;
                searchResultsObj.ResultData = entityLstObj.Where((x, i) => (i >= resultStartIndex && i <= resultEndIndex)).ToList<TEntity>();
            }

            return searchResultsObj;
        }

        #endregion

    }
}
