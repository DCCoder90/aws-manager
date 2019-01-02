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

        /// <summary>
        /// Generates cluster and fires everything up.  Returns secret identifier
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="identifier"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        string GenerateDbCluster(string clientName, string identifier, string prefix = "LW");

        /// <summary>
        /// Given a secret name removes all information and returns removal datetime
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        DateTime? RemoveCluster(string secretName);
    }
}
