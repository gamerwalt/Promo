using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace hrvster.Web.Mvc.Common
{
    public class MockHttpSession : ISession
    {
        private readonly Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        public bool IsAvailable => throw new NotImplementedException();

        public string Id => throw new NotImplementedException();

        public IEnumerable<string> Keys => sessionStorage.Keys;

        public void Clear()
        {
            sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            sessionStorage[key] = value;
        }

        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
        {
            if (sessionStorage.Count == 0)
            {
                value = null;
                return false;
            }

            if (sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
            }

            value = null;
            return false;
        }
    }
}
