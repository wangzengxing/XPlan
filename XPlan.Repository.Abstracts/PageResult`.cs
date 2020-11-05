using System;
using System.Collections.Generic;
using System.Text;

namespace XPlan.Repository.Abstracts
{
    public class PageResult<TEntity>
    {
        public PageResult(List<TEntity> data,int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public List<TEntity> Data { get; }
        public int TotalCount { get; }
    }
}
