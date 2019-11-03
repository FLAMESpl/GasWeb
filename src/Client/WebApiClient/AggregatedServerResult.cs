using System.Collections.Generic;
using System.Linq;

namespace GasWeb.Client.WebApiClient
{
    public class AggregatedServerResult
    {
        private AggregatedServerResult(bool successful, IReadOnlyCollection<string> errors)
        {
            Successful = successful;
            Errors = errors;
        }

        public bool Successful { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public static AggregatedServerResult From(params ServerResponse[] responses)
        {
            var successful = responses.All(x => x.Successful);
            var errors = responses.SelectMany(x => x.Errors).Distinct().ToList();
            return new AggregatedServerResult(successful, errors);
        }
    }
}
