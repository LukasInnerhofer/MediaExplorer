using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    class CryptographyService : ICryptographyService
    {
        private SymmetricAlgorithm _cryptoProvider;
        private HashAlgorithm _hashProvider;
        private const int BufferSize = 1024;
        private int IvSize { get { return _cryptoProvider.BlockSize / 8; } }
        private int HashSize { get { return _hashProvider.HashSize / 8; } }

        public CryptographyService(SymmetricAlgorithm cryptoProvider, HashAlgorithm hashProvider)
        {
            _cryptoProvider = cryptoProvider;
            _hashProvider = hashProvider;
        }

        ~CryptographyService()
        {
            _cryptoProvider.Dispose();
            _hashProvider.Dispose();
        }

        public async Task SerializeAsync(Stream stream, object data, byte[] key)
        {
            _cryptoProvider.Key = key;
            _cryptoProvider.GenerateIV();

            await stream.WriteAsync(_cryptoProvider.IV, 0, _cryptoProvider.IV.Length);
            await stream.WriteAsync(_hashProvider.ComputeHash(key), 0, HashSize);

            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, data);
                ms.Seek(0, SeekOrigin.Begin);

                using (var cs = new CryptoStream(stream, _cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    var buffer = new byte[BufferSize];
                    int bytesRead = 0;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await cs.WriteAsync(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public async Task<T> DeserializeAsync<T>(Stream source, byte[] key)
        {
            _cryptoProvider.Key = key;

            var ivBuffer = new byte[IvSize];
            await source.ReadAsync(ivBuffer, 0, IvSize);
            _cryptoProvider.IV = ivBuffer;

            var keyHash = new byte[HashSize];
            await source.ReadAsync(keyHash, 0, HashSize);
            if (!Enumerable.SequenceEqual(_hashProvider.ComputeHash(key), keyHash))
            {
                throw new InvalidKeyException();
            }

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(source, _cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    var buffer = new byte[BufferSize];
                    int bytesRead = 0;
                    while ((bytesRead = await cs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, bytesRead);
                    }
                    ms.Seek(0, SeekOrigin.Begin);

                    return (T)new BinaryFormatter().Deserialize(ms);
                }
            }
        }

        public async Task EncryptAsync(Stream source, Stream dest, byte[] key)
        {
            _cryptoProvider.Key = key;
            _cryptoProvider.GenerateIV();

            await dest.WriteAsync(_cryptoProvider.IV, 0, _cryptoProvider.IV.Length);
            await dest.WriteAsync(_hashProvider.ComputeHash(key), 0, HashSize);

            using (var cs = new CryptoStream(dest, _cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write))
            {
                var buffer = new byte[BufferSize];
                int bytesRead = 0;
                while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await cs.WriteAsync(buffer, 0, bytesRead);
                }
            }
        }

        public async Task DecryptAsync(Stream source, Stream dest, byte[] key)
        {
            source.Seek(0, SeekOrigin.End);
            int size = (int)source.Position;
            source.Seek(0, SeekOrigin.Begin);

            _cryptoProvider.Key = key;

            var ivBuffer = new byte[IvSize];
            await source.ReadAsync(ivBuffer, 0, IvSize);
            _cryptoProvider.IV = ivBuffer;

            var keyHash = new byte[HashSize];
            await source.ReadAsync(keyHash, 0, HashSize);
            if (!Enumerable.SequenceEqual(_hashProvider.ComputeHash(key), keyHash))
            {
                throw new InvalidKeyException();
            }

            using (var cs = new CryptoStream(source, _cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read))
            {
                var buffer = new byte[size];
                await cs.ReadAsync(buffer, 0, size);
                await dest.WriteAsync(buffer, 0, size);
            }
        }

        public async Task<byte[]> ComputeHashAsync(Stream source)
        {
            source.Seek(0, SeekOrigin.End);
            var buffer = new byte[source.Position];
            source.Seek(0, SeekOrigin.Begin);
            await source.ReadAsync(buffer, 0, buffer.Length);
            return _hashProvider.ComputeHash(buffer);
        }

        public byte[] ComputeHash(byte[] source)
        {
            return _hashProvider.ComputeHash(source);
        }
    }
}
