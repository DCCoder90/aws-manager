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

        public AwsManager()
        {
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials(Definition.AWSProfile, out awsCredentials))
            {
                _secretManager = new SecretManager(awsCredentials);
            }
        }

        public ISecretManager Secrets{
            get{ return _secretManager; }
        }
    }
}
