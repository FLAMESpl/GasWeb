using System;
using System.Collections.Generic;

namespace GasWeb.Client.WebApiClient
{
    public class ServerResponse<T> : ServerResponse where T : class
    {
        public ServerResponse(bool successful, T result, IReadOnlyCollection<string> errors)
            : base(successful, errors)
        {
            Result = result;
        }

        public T Result { get; }

        public ServerResponse<U> To<U>() where U : class => new ServerResponse<U>(Successful, Result as U, Errors);
    }

    public class ServerResponse
    {
        public ServerResponse(bool successful, IReadOnlyCollection<string> errors)
        {
            Successful = successful;
            Errors = errors ?? Array.Empty<string>();
        }

        public bool Successful { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public static ServerResponse Success() => new ServerResponse(true, Array.Empty<string>());
        public static ServerResponse<T> Success<T>(T result) where T : class => new ServerResponse<T>(true, result, Array.Empty<string>());
        public static ServerResponse Failure(IReadOnlyCollection<string> errors) => new ServerResponse(false, errors);
        public static ServerResponse<T> Failure<T>(IReadOnlyCollection<string> errors) where T : class => new ServerResponse<T>(false, null, errors);
    }
}
