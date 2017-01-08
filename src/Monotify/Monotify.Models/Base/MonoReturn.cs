using System.Collections;
using System.Collections.Generic;

namespace Monotify.Models.Base
{
    public class MonoReturn<T> : MonoReturn
    {
        public new T Data { get; set; }
    }

    public class MonoReturn
    {
        public MonoStatusCode Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
        public MonoErrorCollection Errors { get; set; }
        public object Data { get; set; }
    }

    public class MonoError
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public MonoStatusCode Code { get; set; }
        public string InternalMessage { get; set; }
    }

    public enum MonoStatusCode
    {
        Unknown = 0,
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502
    }

    public class MonoErrorCollection : ICollection<MonoError>
    {
        private readonly List<MonoError> _data;

        public MonoErrorCollection()
        {
            _data = new List<MonoError>();
        }

        public IEnumerator<MonoError> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public void Add(string message)
        {
            Add(string.Empty, message);
        }

        public void Add(string key, string message, string internalErrorMessage = default(string), MonoStatusCode code = 0)
        {
            Add(new MonoError
            {
                Code = code,
                InternalMessage = internalErrorMessage,
                Key = key,
                Message = message
            });
        }

        public void Add(MonoError item)
        {
            _data.Add(item);
        }

        public void AddRange(IEnumerable<MonoError> items)
        {
            _data.AddRange(items);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(MonoError item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(MonoError[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public bool Remove(MonoError item)
        {
            return _data.Remove(item);
        }

        public int Count => _data.Count;
        public bool IsReadOnly => false;
    }
}