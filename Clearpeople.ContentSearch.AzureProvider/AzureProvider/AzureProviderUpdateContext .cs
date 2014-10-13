using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class AzureProviderUpdateContext : IProviderUpdateContext 
    {
        public void AddDocument(object itemToAdd, params Sitecore.ContentSearch.Linq.Common.IExecutionContext[] executionContexts)
        {
            throw new NotImplementedException();
        }

        public void AddDocument(object itemToAdd, Sitecore.ContentSearch.Linq.Common.IExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }

        public ICommitPolicyExecutor CommitPolicyExecutor
        {
            get { throw new NotImplementedException(); }
        }

        public void Delete(IIndexableId id)
        {
            throw new NotImplementedException();
        }

        public void Delete(IIndexableUniqueId id)
        {
            throw new NotImplementedException();
        }

        private readonly AzureSearchIndex _index;
        public ISearchIndex Index
        {
            get { return _index; }
        }

        public bool IsParallel
        {
            get { throw new NotImplementedException(); }
        }

        public ParallelOptions ParallelOptions
        {
            get { throw new NotImplementedException(); }
        }

        public void UpdateDocument(object itemToUpdate, object criteriaForUpdate, params Sitecore.ContentSearch.Linq.Common.IExecutionContext[] executionContexts)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocument(object itemToUpdate, object criteriaForUpdate, Sitecore.ContentSearch.Linq.Common.IExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Optimize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
