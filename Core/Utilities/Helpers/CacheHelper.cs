using System;

namespace Core.Utilities.Helpers
{
    public static class CacheHelper
    {
        public static object MapToSerializable(object iDataResult)
        {
            var resultType = iDataResult.GetType();
            var data = resultType.GetProperty("Data").GetValue(iDataResult);
            var message = resultType.GetProperty("Message").GetValue(iDataResult);
            var success = (bool)resultType.GetProperty("Success").GetValue(iDataResult);

            var serializableType = typeof(SerializableDataResult<>).MakeGenericType(data.GetType());
            var serializable = Activator.CreateInstance(serializableType);

            serializableType.GetProperty("Data").SetValue(serializable, data);
            serializableType.GetProperty("Message").SetValue(serializable, message);
            serializableType.GetProperty("Success").SetValue(serializable, success);

            return serializable;
        }
    }

    public class SerializableDataResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
