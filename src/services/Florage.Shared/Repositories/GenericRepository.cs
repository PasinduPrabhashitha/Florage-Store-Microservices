﻿using Florage.Shared.Contracts;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Florage.Shared.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private IMongoCollection<T>  dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        private IMongoDatabase _database;
        
        public GenericRepository(IMongoDatabase database)
        {
            _database = database;
            dbCollection = database.GetCollection<T>(typeof(T).Name);
        }

        public void SetCollectionName (string collectionName)
        {
            dbCollection = _database.GetCollection<T>(collectionName);
        }
         
        public async Task<T> CreateAsync(T entity)
        {
            await dbCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            await dbCollection.DeleteOneAsync(filterBuilder.Eq("Id", id));
        }

        public async Task<T> FilterAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await dbCollection.Find(filterBuilder.Eq("Id", id)).FirstOrDefaultAsync();
        } 

        public async Task UpdateAsync(string id, T entity)
        {
            await dbCollection.ReplaceOneAsync(filterBuilder.Eq("Id", id), entity);
        }

    }
}
