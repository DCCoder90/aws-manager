using System;
using System.Collections.Generic;
using System.Text;

namespace AWSManager.Interfaces
{
    public interface IAwsManager
    {
        ISecretManager Secrets{ get; }
        IDatabaseManager Databases{ get; }
        IIAMManager IAMs{ get; }
    }
}
