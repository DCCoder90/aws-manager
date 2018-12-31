using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using AWSManager.Interfaces;

namespace AWSManager
{
    internal class IAMManager : IIAMManager
    {
        private readonly AWSCredentials _credentials;
        private readonly AmazonIdentityManagementServiceClient _client;

        public IAMManager(AWSCredentials creds)
        {
            _credentials = creds;
            _client = new AmazonIdentityManagementServiceClient(_credentials, RegionEndpoint.USEast2);
        }

        public User CreateUser(string username)
        {
            var request = new CreateUserRequest(){
                UserName = username
            };

            var response = Task.Run(async () => await _client.CreateUserAsync(request)).Result;
            return response.User;
        }
    }
}
