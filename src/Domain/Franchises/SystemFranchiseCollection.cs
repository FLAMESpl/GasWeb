using System.Collections;
using System.Collections.Generic;

namespace GasWeb.Domain.Franchises
{
    internal class SystemFranchiseCollection : IEnumerable<long>
    {
        public SystemFranchiseCollection(long lotos, long orlen, long bp, long auchan)
        {
            Lotos = lotos;
            Orlen = orlen;
            Bp = bp;
            Auchan = auchan;
        }

        public long Lotos { get; }
        public long Orlen { get; }
        public long Bp { get; }
        public long Auchan { get; }

        public IEnumerator<long> GetEnumerator()
        {
            yield return Lotos;
            yield return Orlen;
            yield return Bp;
            yield return Auchan;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
