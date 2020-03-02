using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LibraryCorp.Models;
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
        private List<ICosmosDocument> _modifiedDocuments;
        private CosmosClient _client;
        private Guid _partitionKey;

        public CosmosRepo(Guid partitionKey)
        {
            this._partitionKey = partitionKey;
            this._container = CosmosClientFactory.GetLibrariesContainer();
            this._modifiedDocuments = new List<ICosmosDocument>();
        }

        public void StartTransaction()
        {
            this._batch = _container.CreateTransactionalBatch(new PartitionKey(this._partitionKey.ToString()));
        }

        public void Create<T>(T document) where T: IAggregate
        {
            var cosmosDocument = new CosmosDocument<T>(this._partitionKey, document);
            _batch.CreateItem(cosmosDocument);
        }


        public async Task<Copy> GetFreeCopy(BrandId brandId)
        {
            var iterator = _container.GetItemLinqQueryable<CosmosDocument<Copy>>()
                .Where(x => x.PartitionKey == this._partitionKey)
                .Where(x => x.Data.BrandId == brandId.Value && !x.Data.IsTaken)
                .Take(1)
                .ToFeedIterator();

            var copyDocument = await iterator.GetFirst();
            _modifiedDocuments.Add(copyDocument);

            return copyDocument.Data;
        }

        public async Task<(Reservation, Copy)> GetReservationToBorrow(ReaderId readerId, ReservationId reservationId)
        {
            var reservation = await GetReservation(reservationId, readerId);
            var copy = await GetCopy(reservation.CopyId, reservationId);

            return (reservation, copy);
        }

        public async Task<Reservation> GetReservation(ReservationId reservationId, ReaderId readerId)
        {
            var iterator = _container.GetItemLinqQueryable<CosmosDocument<Reservation>>()
                .Where(x => x.PartitionKey == this._partitionKey)
                .Where(x => x.Data.ReaderId == readerId.Value && x.Data.ReservationId == reservationId.Value)
                .Take(1)
                .ToFeedIterator();

            var reservationDocument = await iterator.GetFirst();
            _modifiedDocuments.Add(reservationDocument);

            return reservationDocument.Data;
        }

        private async Task<Copy> GetCopy(CopyId copyId, ReservationId reservationId)
        {
            var iterator = _container.GetItemLinqQueryable<CosmosDocument<Copy>>()
                .Where(x => x.PartitionKey == this._partitionKey)
                .Where(x => x.Data.CopyId == copyId && x.Data.OwnerId == reservationId)
                .Take(1)
                .ToFeedIterator();

            var copyDocument = await iterator.GetFirst();
            _modifiedDocuments.Add(copyDocument);

            return copyDocument.Data;
        }


        public async Task<TransactionalBatchResponse> ExecuteAsync()
        {
            foreach (var modifiedDocument in _modifiedDocuments)
            {
                this._batch.ReplaceItem(modifiedDocument.Id, modifiedDocument);
            }

            var response =  await this._batch.ExecuteAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(response.ErrorMessage);

            return response;
        }
    }

    public static class FeedIteratorExtensions
    {
        public static async Task<T> GetFirst<T>(this FeedIterator<T> iterator)
        {
            while (iterator.HasMoreResults)
            {
                foreach (var document in await iterator.ReadNextAsync())
                {
                    return document;
                }
            }

            return default;
        }
    }
}
