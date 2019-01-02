using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using AWSManager.Interfaces;

namespace AWSManager
{
    public class AwsManager : IAwsManager
    {
        private ISecretManager _secretManager;
        private IDatabaseManager _databaseManager;
        private IIAMManager _iamManager;

        public AwsManager(string accesskey = null, string secretkey = null)
        {
            AWSCredentials awsCredentials = null;

            if (!string.IsNullOrEmpty(accesskey) && !string.IsNullOrEmpty(secretkey))
            {
                awsCredentials = new BasicAWSCredentials(accesskey, secretkey);
            }else{
                var chain = new CredentialProfileStoreChain();
                chain.TryGetAWSCredentials(Definition.AWSProfile, out awsCredentials);
            }

            _secretManager = new SecretManager(awsCredentials);
            _databaseManager = new DatabaseManager(awsCredentials);
            _iamManager = new IAMManager(awsCredentials);
        }

        public ISecretManager Secrets{
            get{ return _secretManager; }
        }

        public IDatabaseManager Databases{
            get{ return _databaseManager; }
        }

        public IIAMManager IAMs
        {
            get { return _iamManager; }
        }

        public string GenerateDbCluster(string clientName, string identifier, string prefix = "LW")
        {
            var masterUser = Helpers.RandomPassword.Generate(5, 10);
            var masterPass = Helpers.RandomPassword.Generate(8, 15);
            var cluster = _databaseManager.CreateCluster(clientName, identifier, masterUser, masterPass);
            var user = _iamManager.CreateUser(clientName);
            var access = _iamManager.CreateAccessKey(user.UserName);

            var secret = new Dictionary<string,string>(){
                {StringDefinition.AccessKeyId,access.AccessKeyId },
                {StringDefinition.AccessKey,access.SecretAccessKey },
                {StringDefinition.Username,user.UserName },
                {StringDefinition.MasterUser, masterUser },
                {StringDefinition.MasterPass, masterPass },
                {StringDefinition.ClusterIdentifier, cluster.DBClusterIdentifier },
                {StringDefinition.Endpoint, cluster.Endpoint },
                {StringDefinition.Port, cluster.Port.ToString() }
            };

            var storedSecret =
                _secretManager.StoreSecret(secret, $"{prefix}{clientName}", $"{prefix} - {clientName} stored secrets.");
            return storedSecret;
        }

        public DateTime? RemoveCluster(string secretName)
        {
            var secrets = _secretManager.GetSecrets(secretName);

            _databaseManager.DeleteCluster(secrets[StringDefinition.ClusterIdentifier]);
            _iamManager.DeleteUser(secrets[StringDefinition.Username]);
            var secretId = _secretManager.GetSecretId(secretName);
            var removalDate = _secretManager.DeleteSecret(secretId);
            return removalDate;
        }
    }
}
