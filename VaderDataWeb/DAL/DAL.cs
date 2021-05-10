using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace VaderDataWeb
{
    public class DAL
    {
        private string host = "vaderdatadb.mongo.cosmos.azure.com";
        private string userName = "vaderdatadb";
        private string password = "FABdUS97XHZqf4XlvVXKqhzTWJeLiTrOD5V65RxEk48W1CzQnDdPI0CgDKkEIrtXkE4DggCMu3N2SP2uh7gSvA==";

        private string dbName = "VaderDataDB";
        private string collectionName = "VaderDataCollection";


        private MongoClient GetClient()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseTls = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            settings.RetryWrites = false;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);

            return client;
        }

        public IMongoCollection<Models.Measurement> MeasurementCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase(dbName);
            var measurementCollection = database.GetCollection<Models.Measurement>(collectionName);

            return measurementCollection;
        }
    }
}
