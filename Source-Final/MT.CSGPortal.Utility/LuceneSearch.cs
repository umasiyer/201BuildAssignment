using System;
using System.Collections.Generic;
//namespace related to Lucene search    
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using MT.CSGPortal.Entities;

using Version = Lucene.Net.Util;

namespace MT.CSGPortal.Utility
{
   public class LuceneSearch 
    {
       public static string INDEXPATH = @"\SearchIndex.txt";
       public static readonly char likeChar = '*';

       public SearchResult<TEntity> SearchProfileData<TEntity>(List<TEntity> entityLstObj, string searchParameter, int pageNumber) where TEntity : new()
       {
           CreateIndexDataGeneric(entityLstObj);

           SearchResult<TEntity> searchResultsObj = new SearchResult<TEntity>();

           using (Directory directory = FSDirectory.Open(INDEXPATH))
           {
               using (var indexSearcher = new IndexSearcher(directory))
               {
                   //Prepare the query containing all the fields that has to be searched
                   Query queryObj = PrepareQueryParser(entityLstObj[0], searchParameter);

                   //perform search on the index
                   TopDocs topDocs = indexSearcher.Search(queryObj, ApplicationSettingsReader.MaxPageResultsLucene);

                   //populate the search results to searchResultsDto object
                   searchResultsObj = PopulateSearchResultDTO<TEntity>(indexSearcher, topDocs, pageNumber);
               }
           }
           return searchResultsObj;
       }

       public static void CreateIndexDataGeneric<TEntity>(List<TEntity> entityObj)
       {
           using (var directory = FSDirectory.Open(INDEXPATH))
           {
               using (var indexWriter = new IndexWriter(directory, new StandardAnalyzer(Version.Version.LUCENE_30), true, IndexWriter.MaxFieldLength.UNLIMITED))
               {
                   for (int i = 0; i < entityObj.Count; i++)
                   {
                       Document documentObj = new Document();
                       var propertiesInfo = entityObj[i].GetType().GetProperties();

                       foreach (var colVal in propertiesInfo)
                       {
                           string fieldName = ((System.Reflection.MemberInfo)colVal).Name;
                           documentObj.Add(new Field(fieldName, colVal.GetValue(entityObj[i]).ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                       }
                       indexWriter.AddDocument(documentObj);
                   }
                   indexWriter.Optimize();
                   indexWriter.Commit();
               }
           }
       }

       private Query PrepareQueryParser<TEntity>(TEntity entityObj, string searchParameter)
       {
           var propertiesInfo = entityObj.GetType().GetProperties();
           string[] fieldValues = new string[propertiesInfo.Length];
           for (int i = 0; i < propertiesInfo.Length; i++)
           {
               fieldValues[i] = ((System.Reflection.MemberInfo)propertiesInfo[i]).Name;
           }
           Query queryObj;
           using (var analyzer = new StandardAnalyzer(Version.Version.LUCENE_30))
           {
               var fieldParserObj = new MultiFieldQueryParser(Version.Version.LUCENE_30, fieldValues, analyzer);

               queryObj = fieldParserObj.Parse(searchParameter + likeChar);
               fieldParserObj.DefaultOperator = QueryParser.Operator.OR;
               fieldParserObj.AllowLeadingWildcard = true;
           }
           return queryObj;
       }

       public static SearchResult<TEntity> PopulateSearchResultDTO<TEntity>(Searchable searcherObj,TopDocs topDocsObj,int pageNumber) where TEntity : new()
       {
           SearchResult<TEntity> searchResultDtoObj = new SearchResult<TEntity>();
           searchResultDtoObj.ResultData = new List<TEntity>();
           TEntity entityObj;

           int startPageNumber = 1 + ((pageNumber - 1) * ApplicationSettingsReader.PageSizeForLuceneSearch);
           int endPageNumber = pageNumber * ApplicationSettingsReader.PageSizeForLuceneSearch;

           bool endOfRecords = true;
           if (topDocsObj.TotalHits < endPageNumber)
           { endPageNumber = topDocsObj.TotalHits; }
           else
               endOfRecords = false;           

           for (int pageIndex = startPageNumber - 1; pageIndex < endPageNumber; pageIndex++)
           {
               entityObj = new TEntity();

               var propertiesInfo = entityObj.GetType().GetProperties();
               string fieldName = string.Empty;
                             
               ScoreDoc scoreDoc = topDocsObj.ScoreDocs[pageIndex];
               Document doc = searcherObj.Doc(scoreDoc.Doc);

               for (int i = 0; i < propertiesInfo.Length; i++)
               {
                   fieldName = ((System.Reflection.MemberInfo)propertiesInfo[i]).Name;
                   var targetObj = propertiesInfo[i];
                                      
                   Type targetType;
                   if(targetObj.PropertyType.IsGenericType && targetObj.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                   {  targetType = Nullable.GetUnderlyingType(targetObj.PropertyType);}
                   else
                   {  targetType = targetObj.PropertyType;}
                    
                   var targetVal = Convert.ChangeType(doc.Get(fieldName),targetType);

                   entityObj.GetType().GetProperties()[i].SetValue(entityObj, targetVal);                  
                   
               }                              
               searchResultDtoObj.ResultData.Add(entityObj);
           }                   
          
           searchResultDtoObj.TotalRecordCount = topDocsObj.TotalHits;
           searchResultDtoObj.EndOfRecords = endOfRecords;

           return searchResultDtoObj;
       }

    }
}
