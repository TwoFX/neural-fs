module Util

open System.Collections.Generic

let memoize f =
    let dict = new Dictionary<'a * 'b,_>()
    fun a b ->
        match dict.TryGetValue((a, b)) with
        | (true, v) -> v
        | _ ->
            let temp = f a b
            dict.Add((a, b), temp)
            temp

let fix a b = a