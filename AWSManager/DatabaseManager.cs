using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.RDS;
using Amazon.RDS.Model;
using Amazon.Runtime;
using Amazon.SecretsManager;
using AWSManager.Interfaces;

namespace AWSManager
{
    internal class DatabaseManager : IDatabaseManager
    {
        private readonly AWSCredentials _credentials;
        private readonly AmazonRDSConfig _config;
        private readonly AmazonRDSClient _client;
        private readonly List<string> _availabilityRegions;
        public int BackupRetentionDays = 1;
        public bool DeletionProtection = false;
        public bool EncryptStorage = true;
        private List<Tag> _tags;

        public DatabaseManager(AWSCredentials creds)
        {

            _availabilityRegions=new List<string>(){
                "US/Eastern",
                "US/Central"
            };

            _credentials = creds;
            _config = new AmazonRDSConfig(){ RegionEndpoint = RegionEndpoint.USEast2 };
            _client = new AmazonRDSClient(_credentials, _config);

            SetCreatorTag();
        }

        private void SetCreatorTag()
        {
            if(_tags==null)
                _tags = new List<Tag>();

            _tags.Clear();

            _tags.Add(new Tag()
            {
                Key = "creator",
                Value = "dccoders_awsmanager"
            });
        }

        public void SetTags(IEnumerable<KeyValuePair<string,string>> tags)
        {
            SetCreatorTag();
            foreach (KeyValuePair<string, string> tag in tags)
            {
                _tags.Add(new Tag(){Key = tag.Key, Value = tag.Value});
            }
        }

        public DBCluster CreateCluster(string databaseName, string clusterIdentifier, string masterUser, string masterPass)
        {
            var request = new CreateDBClusterRequest(){
                BackupRetentionPeriod = BackupRetentionDays,
                AvailabilityZones = _availabilityRegions,
                DeletionProtection = DeletionProtection,
                DatabaseName = databaseName,
                DBClusterIdentifier = clusterIdentifier,
                Engine = "aurora-mysql",
                MasterUsername = masterUser,
                MasterUserPassword = masterPass,
                Port = 3306,
                StorageEncrypted = EncryptStorage,
                Tags = _tags,
                EnableIAMDatabaseAuthentication = true
            };

            var response = Task.Run(async () => await _client.CreateDBClusterAsync(request)).Result;
            return response.DBCluster;
        }

        public DBCluster DeleteCluster(string clusterIdentifier)
        {
            var request = new DeleteDBClusterRequest(){
                DBClusterIdentifier = clusterIdentifier
            };
            var response = Task.Run(async () => await _client.DeleteDBClusterAsync(request)).Result;
            return response.DBCluster;
        }

        public DBInstance RestartDbInstance(string instanceIdentifier)
        {
            var request = new RebootDBInstanceRequest(instanceIdentifier);
            var response = Task.Run(async()=> await _client.RebootDBInstanceAsync(request)).Result;
            return response.DBInstance;
        }

        public DBCluster StartCluster(string clusterIdentifier)
        {
            var request = new StartDBClusterRequest(){
                DBClusterIdentifier = clusterIdentifier
            };

            var response = Task.Run(async () => await _client.StartDBClusterAsync(request)).Result;
            return response.DBCluster;
        }

        public DBCluster StopCluster(string clusterIdentifier)
        {
            var request = new StopDBClusterRequest(){
                DBClusterIdentifier = clusterIdentifier
            };

            var response = Task.Run(async () => await _client.StopDBClusterAsync(request)).Result;

            return response.DBCluster;
        }
    }
}
