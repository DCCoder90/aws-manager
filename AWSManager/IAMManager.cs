using System;
using System.Collections.Generic;
using System.Globalization;
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
        private List<Tag> _tags;


        public IAMManager(AWSCredentials creds)
        {
            _credentials = creds;
            _client = new AmazonIdentityManagementServiceClient(_credentials, RegionEndpoint.USEast2);
            SetCreatorTag();
        }

        private void SetCreatorTag()
        {
            if (_tags == null)
                _tags = new List<Tag>();

            _tags.Clear();

            _tags.AddRange(new List<Tag>(){
                new Tag(){Key = "creator", Value = "dccoders_awsmanager"},
                new Tag(){
                    Key = "datecreated",
                    Value = DateTime.Now.ToString("MM/dd/yyyyTHH:mm:ss", CultureInfo.InvariantCulture)
                }
            });
        }

        public void SetTags(IEnumerable<KeyValuePair<string, string>> tags)
        {
            SetCreatorTag();
            foreach (KeyValuePair<string, string> tag in tags)
            {
                _tags.Add(new Tag() { Key = tag.Key, Value = tag.Value });
            }
        }

        public User CreateUser(string username)
        {
            var request = new CreateUserRequest(){
                UserName = username,
                Tags = _tags
            };

            var response = Task.Run(async () => await _client.CreateUserAsync(request)).Result;
            return response.User;
        }

        public Group GetGroup(string groupName)
        {
            var request = new GetGroupRequest(groupName);
            var response = Task.Run(async () => await _client.GetGroupAsync(request)).Result;
            return response.Group;
        }

        public User GetUser(string username)
        {
            var request = new GetUserRequest(){
                UserName = username
            };

            var response = Task.Run(async () => await _client.GetUserAsync(request)).Result;

            return response.User;
        }

        public void AssignUserToGroup(string groupName, string username)
        {
            var request = new AddUserToGroupRequest(){
                GroupName = groupName,
                UserName = username
            };

            Task.Run(async () => await _client.AddUserToGroupAsync(request));
        }

        public void DeleteUser(string username)
        {
            var request = new DeleteUserRequest(username);
            Task.Run(async () => await _client.DeleteUserAsync(request));
        }

        public AccessKey CreateAccessKey(string username)
        {
            var request = new CreateAccessKeyRequest(){
                UserName = username
            };

            var response = Task.Run(async () => await _client.CreateAccessKeyAsync(request)).Result;
            return response.AccessKey;
        }
    }
}