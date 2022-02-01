module Solitaire.Model

open System
open Cards

type StackCard = {
  card: Card
  isFaceUp: bool
} with 
    override this.ToString() =
      if this.isFaceUp then
        this.card.ToString()
      else 
        "###"

type Game = {
  deck: Card list
  table: Card list
  stacks: StackCard list list
}