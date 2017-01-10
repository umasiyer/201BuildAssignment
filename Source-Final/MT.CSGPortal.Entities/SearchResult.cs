using System.Collections.Generic;

namespace MT.CSGPortal.Entities
{
    public class SearchResult<TEntity>
    {
        private List<TEntity> entityObj;
        public List<TEntity> ResultData { get { return entityObj;} set {entityObj = value; }}
        public int TotalRecordCount { get; set; }
        public bool EndOfRecords { get; set; }
    }    
}
