namespace Bloom

module Filter =
    open System
    open System.IO.Hashing;
    open System.Numerics

            
    [<Struct>]
    type Filter = {
        hash: XxHash3;
        sum: uint64;
        n: uint32;
    }

    let create = {
        hash = new XxHash3(0L);
        sum = 0UL;
        n = 0u;
    }

    let append (data: ReadOnlySpan<byte>) (filter: Filter) =
        filter.hash.Reset()
        filter.hash.Append(data)
        let currentHash = filter.hash.GetCurrentHashAsUInt64()

        { filter with sum = filter.sum ||| currentHash }
    
    let contains (data: ReadOnlySpan<byte>) (filter: Filter) =
        filter.hash.Append(data)
        let currentHash = filter.hash.GetCurrentHashAsUInt64()

        let result = filter.sum &&& currentHash
        result = currentHash
    
    let precision filter = 
        let i = BitOperations.PopCount(filter.sum)
        (float) i / 64.0