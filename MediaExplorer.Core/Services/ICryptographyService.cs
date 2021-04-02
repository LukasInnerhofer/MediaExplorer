using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    public interface ICryptographyService
    {
        Task SerializeAsync(Stream fileStream, object data, byte[] key);
        Task<T> DeserializeAsync<T>(Stream dest, byte[] key);
        Task EncryptAsync(Stream source, Stream dest, byte[] key);
        Task DecryptAsync(Stream source, Stream dest, byte[] key);
        Task<byte[]> ComputeHashAsync(Stream source);
        byte[] ComputeHash(byte[] source);
    }
}
