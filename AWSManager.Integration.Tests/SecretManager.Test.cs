using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace AWSManager.Integration.Tests
{
    public class SecretManager
    {
        private AwsManager _sut;

        public SecretManager()
        {
            _sut = new AwsManager();
        }

        
        [Fact]
        public void CreateSecret()
        {
            var resp = _sut.Secrets.StoreSecret("testsecret:testSecret", "test3", "Simply a test secret to see if everything works");
            Assert.Equal("test",resp);
        }

        [Fact]
        public void GetSecret()
        {
            var resp = _sut.Secrets.GetSecret("test2");
            Assert.Equal(resp, "testSecret");
        }

        //[Fact]
        public void DeleteSecret()
        {
            var id = _sut.Secrets.GetSecretId("test2");
            var resp = _sut.Secrets.DeleteSecret(id);

            Assert.NotNull(resp);
        }
    }
}
