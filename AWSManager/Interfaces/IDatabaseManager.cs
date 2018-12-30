using System;
using System.Collections.Generic;
using System.Text;
using Amazon.RDS.Model;

namespace AWSManager.Interfaces
{
    public interface IDatabaseManager
    {
        DBCluster DeleteCluster(string clusterIdentifier);
        void SetTags(IEnumerable<KeyValuePair<string, string>> tags);
        DBCluster CreateCluster(string databaseName, string clusterIdentifier, string masterUser, string masterPass);
        DBInstance RestartDbInstance(string instanceIdentifier);
        DBCluster StopCluster(string clusterIdentifier);
        DBCluster StartCluster(string clusterIdentifier);
    }
}
