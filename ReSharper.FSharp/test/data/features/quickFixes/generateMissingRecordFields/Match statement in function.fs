// TODO: Need to check whether this will be in sequential expression or not
type R = { A: int; B: int }

let r (someOption : option int) : R = 
    ()
    match someOption with
    | Some value -> {A = value; B = value}
    | None -> {}{caret}