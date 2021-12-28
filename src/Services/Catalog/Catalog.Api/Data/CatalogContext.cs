using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        public CatalogContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetValue<string>("CatalogdbSettings:ConnectionString"));
            var database = client.GetDatabase(config.GetValue<string>("CatalogdbSettings:DatabaseName"));
            Products = database.GetCollection<Product>(config.GetValue<string>("CatalogdbSettings:CollectionName"));
            CatalogContextSeed.Seed(Products);
        }
        

    }
}
