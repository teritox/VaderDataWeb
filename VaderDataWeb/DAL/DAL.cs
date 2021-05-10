using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using VaderDataWeb.Services;

namespace VaderDataWeb
{
    public class DAL
    {
        private readonly IConfiguration _configuration;
        public DAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MongoClient GetClient()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(_configuration["VaderDataDB:host"], 10255);
            settings.UseTls = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            settings.RetryWrites = false;

            MongoIdentity identity = new MongoInternalIdentity(_configuration["VaderDataDB:dbName"], _configuration["VaderDataDB:userName"]);
            MongoIdentityEvidence evidence = new PasswordEvidence(_configuration["VaderDataDB:password"]);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);

            return client;
        }

        public IMongoCollection<Models.Measurement> MeasurementCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase(_configuration["VaderDataDB:dbName"]);
            var measurementCollection = database.GetCollection<Models.Measurement>(_configuration["VaderDataDB:collectionName"]);

            return measurementCollection;
        }
    }
}
