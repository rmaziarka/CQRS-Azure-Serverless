﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Build.Utilities;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;
using Task = Microsoft.Build.Utilities.Task;

namespace LibraryCorp
{
    public class CosmosRepo
    {
        private readonly Container _container;
        private TransactionalBatch _batch;
        private List<Document> _modifiedDocuments;
        private CosmosClient _client;
        private string _partitionKey;

        public CosmosRepo(string partitionKey)
        {
            this._partitionKey = partitionKey;
            this._container = CosmosClientFactory.GetLibrariesContainer();
            this._modifiedDocuments = new List<Document>();
        }

        public void StartTransaction()
        {
            this._batch = _container.CreateTransactionalBatch(new PartitionKey(this._partitionKey));
        }

        public void Create<T>(T document) where T: Aggregate
        {
            var cosmosDocument = new CosmosDocument<T>(this._partitionKey, document);
            _batch.CreateItem(cosmosDocument);
        }

        public async Task<IEnumerable<CosmosDocument<T>>> GetDocuments<T>(Expression<Func<CosmosDocument<T>, bool>> query) where T : Aggregate
        {
            var where = _container.GetItemLinqQueryable<CosmosDocument<T>>()
                .Where(x => x.PartitionKey == this._partitionKey)
                .Where(query);

            var iterator = where.ToFeedIterator();

            var list = new List<CosmosDocument<T>>();
            while (iterator.HasMoreResults)
            {
                foreach (var document in await iterator.ReadNextAsync())
                {
                    list.Add(document);
                    _modifiedDocuments.Add(document);
                }
            }

            return list;
        }

        public async Task<Copy> GetFreeCopy(string brandId)
        {
            var documents = await this.GetDocuments<Copy>(x =>
                x.Data.BrandId == brandId
                && !x.Data.IsTaken);

            return documents.FirstOrDefault()?.Data;
        }

        public async Task<TransactionalBatchResponse> ExecuteAsync()
        {
            foreach (var modifiedDocument in _modifiedDocuments)
            {
                this._batch.ReplaceItem(modifiedDocument.Id, modifiedDocument);
            }

           var response =  await this._batch.ExecuteAsync();
           return response;
        }
    }
}