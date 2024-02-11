namespace Experiments

open System
open System.IO.Hashing

module WithRecord =
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
        let sum = filter.hash.Reset()
        filter.hash.Append(data)
        let currentHash = filter.hash.GetCurrentHashAsUInt64()

        let result = filter.sum &&& currentHash
        result = currentHash

module WithStructRecord = 
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

module StructWithoutHashReuse = 
    [<Struct>]
    type Filter = {
        sum: uint64;
        n: uint32;
    }

    let create = {
        sum = 0UL;
        n = 0u;
    }

    let append (data: ReadOnlySpan<byte>) (filter: Filter) =
        let hash = new XxHash3(0L)
        hash.Append(data)
        let currentHash = hash.GetCurrentHashAsUInt64()

        { filter with sum = filter.sum ||| currentHash }
    
    let contains (data: ReadOnlySpan<byte>) (filter: Filter) =
        let hash = new XxHash3(0L)
        hash.Append(data)
        let currentHash = hash.GetCurrentHashAsUInt64()

        let result = filter.sum &&& currentHash
        result = currentHash