using System;
using System.Collections.Generic;
using System.Text;
using Amazon.IdentityManagement.Model;

namespace AWSManager.Interfaces
{
    public interface IIAMManager
    {
        User CreateUser(string username);
        void AssignUserToGroup(string groupName, string username);
        User GetUser(string username);
        Group GetGroup(string groupName);
        void DeleteUser(string username);
        void SetTags(IEnumerable<KeyValuePair<string, string>> tags);
        AccessKey CreateAccessKey(string username);
    }
}
