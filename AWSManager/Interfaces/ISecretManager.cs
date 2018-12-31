using System;
using System.Collections.Generic;
using System.Text;

namespace AWSManager.Interfaces
{
    public interface ISecretManager
    {

        /// <summary>
        /// Returns the secret requested
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        string GetSecret(string secretName);

        /// <summary>
        /// Stores secret and returns name
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        string StoreSecret(string secret, string name, string description);

        /// <summary>
        /// Returns secretId of secret with secretName
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        string GetSecretId(string secretName);

        /// <summary>
        /// Schedules a secret for deletion and returns the Date to be deleted
        /// </summary>
        /// <param name="secretId"></param>
        /// <returns></returns>
        DateTime? DeleteSecret(string secretId);

        /// <summary>
        /// Stores keyvalue pair secret and returns name
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        string StoreSecret(IDictionary<string, string> secret, string name, string description);

        /// <summary>
        /// Returns the keyvalue pair secret saved
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        IDictionary<string, string> GetSecrets(string secretName);

        string UpdateSecret(string secretId, IDictionary<string, string> secret, string description);

        string UpdateSecret(string secretId, string secret, string description);
    }
}
