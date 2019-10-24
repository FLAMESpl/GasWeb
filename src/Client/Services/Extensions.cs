using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace GasWeb.Client.Services
{
    public static class Extensions
    {
        public static Task<T> GetItemAsync<T>(this ILocalStorageService localStorage)
        {
            return localStorage.GetItemAsync<T>(typeof(T).Name);
        }

        public static Task SetItemAsync<T>(this ILocalStorageService localStorage, T value)
        {
            return localStorage.SetItemAsync(typeof(T).Name, value);
        }

        public static Task RemoveItemAsync<T>(this ILocalStorageService localStorage)
        {
            return localStorage.RemoveItemAsync(typeof(T).Name);
        }
    }
}
