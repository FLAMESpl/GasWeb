using System.Collections;
using System.Collections.Generic;

namespace GasWeb.Domain.Franchises
{
    internal class SystemFranchiseCollection : IEnumerable<long>
    {
        public SystemFranchiseCollection(long lotos, long orlen, long bp)
        {
            Lotos = lotos;
            Orlen = orlen;
            Bp = bp;
        }

        public long Lotos { get; }
        public long Orlen { get; }
        public long Bp { get; }

        public IEnumerator<long> GetEnumerator()
        {
            yield return Lotos;
            yield return Orlen;
            yield return Bp;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
