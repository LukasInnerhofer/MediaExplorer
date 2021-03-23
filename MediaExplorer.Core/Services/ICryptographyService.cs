using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    public interface ICryptographyService<Crypt> where Crypt : SymmetricAlgorithm, new()
    {
        Task SerializeAsync(string fileName, object data, byte[] key);
        Task<T> DeserializeAsync<T>(string fileName, byte[] key);
    }
}
