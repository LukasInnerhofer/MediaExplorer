using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    class CryptographyService<Crypt> : ICryptographyService<Crypt> where Crypt : SymmetricAlgorithm, new()
    {
        private Crypt _provider;
        private const int BufferSize = 1024;
        private int IvSize { get { return _provider.BlockSize / 8; } }

        public CryptographyService()
        {
            _provider = new Crypt();
        }

        public async Task<T> DeserializeAsync<T>(string fileName, byte[] key)
        {
            _provider.Key = key;

            using(var fs = new FileStream(fileName, FileMode.Open))
            {
                byte[] ivBuffer = new byte[IvSize];
                await fs.ReadAsync(ivBuffer, 0, IvSize);
                _provider.IV = ivBuffer;

                using(var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(fs, _provider.CreateDecryptor(), CryptoStreamMode.Read))
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
        }

        public async Task SerializeAsync(string fileName, object data, byte[] key)
        {
            _provider.Key = key;
            _provider.GenerateIV();

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                await fs.WriteAsync(_provider.IV, 0, _provider.IV.Length);

                using (var ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, data);
                    ms.Seek(0, SeekOrigin.Begin);

                    using (var cs = new CryptoStream(fs, _provider.CreateEncryptor(), CryptoStreamMode.Write))
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
        }
    }
}
