namespace GasWeb.Domain.Franchises
{
    internal class SystemFranchiseCollection
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
    }
}
