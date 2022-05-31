module Lastcard.Update
open Cards

type Message =
    | Ping

type LastcardGame = {
  deck: Card list
}

let initModel =
  {
    deck = Cards.newDeck |> shuffle
  }


let update message model =
    match message with
    | Ping -> model