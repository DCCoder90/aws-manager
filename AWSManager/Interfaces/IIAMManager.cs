using System;
using System.Collections.Generic;
using System.Text;
using Amazon.IdentityManagement.Model;

namespace AWSManager.Interfaces
{
    public interface IIAMManager
    {
        User CreateUser(string username);
    }
}
