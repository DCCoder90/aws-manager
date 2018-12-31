using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using AWSManager.Interfaces;
using Newtonsoft.Json;

namespace AWSManager
{
    internal class SecretManager : ISecretManager
    {
        private readonly AWSCredentials _credentials;
        private readonly AmazonSecretsManagerConfig _config;
        private readonly AmazonSecretsManagerClient _client;

        public SecretManager(AWSCredentials creds)
        {
            _credentials = creds;
            _config = new AmazonSecretsManagerConfig { RegionEndpoint = RegionEndpoint.USEast2 };
            _client = new AmazonSecretsManagerClient(_credentials,_config);
        }

        public IDictionary<string, string> GetSecrets(string secretName)
        {
            var secretjson = GetSecret(secretName);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(secretjson);
        }

        public string UpdateSecret(string secretId, string secret, string description)
        {
            var request = new UpdateSecretRequest() {
                SecretId = secretId,
                SecretString = secret,
                Description = description
            };

            var response = Task.Run(async () => await _client.UpdateSecretAsync(request)).Result;
            return response?.VersionId;
        }

        public string UpdateSecret(string secretId, IDictionary<string,string> secret, string description)
        {
            var secretJson = JsonConvert.SerializeObject(secret);

            return UpdateSecret(secretId, secretJson, description);
        }

        public string GetSecret(string secretName)
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            GetSecretValueResponse response = null;
            response = Task.Run(async () => await _client.GetSecretValueAsync(request)).Result;
            return response?.SecretString;
        }

        public string GetSecretId(string secretName)
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            GetSecretValueResponse response = null;
            response = Task.Run(async () => await _client.GetSecretValueAsync(request)).Result;
            return response?.VersionId;
        }

        public string StoreSecret(IDictionary<string, string> secret, string name, string description)
        {
            var secretjson = JsonConvert.SerializeObject(secret);
            return StoreSecret(secretjson, name, description);
        }

        public string StoreSecret(string secret, string name, string description)
        {
            
            var request = new CreateSecretRequest(){Name = name, SecretString = secret, Description = description};
            var response = Task.Run(async()=> await _client.CreateSecretAsync(request)).Result;

            return response?.Name;
        }

        public DateTime? DeleteSecret(string secretId)
        {
            var request = new DeleteSecretRequest(){
                SecretId = secretId, ForceDeleteWithoutRecovery = true
            };

            var response = Task.Run(async () => await _client.DeleteSecretAsync(request)).Result;

            return response?.DeletionDate;
        }
    }
}
