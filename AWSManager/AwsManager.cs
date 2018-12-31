using System;
using System.Collections.Generic;
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

        public AwsManager()
        {
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials(Definition.AWSProfile, out awsCredentials))
            {
                _secretManager = new SecretManager(awsCredentials);
                _databaseManager = new DatabaseManager(awsCredentials);
                _iamManager = new IAMManager(awsCredentials);
            }
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
    }
}
