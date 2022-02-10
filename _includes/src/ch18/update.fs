module Solitaire.Update

open System
open Cards
open Solitaire.Model
open Solitaire.Actions

let private (|Number|_|) (ch:Char) =
  match Char.GetNumericValue(ch) with
  | -1.0 -> None
  | a -> a |> int |> Some

let private applyUpdate command multiPhaseGame =
  {
    multiPhaseGame with 
      game = multiPhaseGame.game|> applyCommand command
      phase = General   // all updates move the phase back to General
  }

let private nextPhase phase game = 
  {game with phase = phase}

let private updateGameGeneral game keystroke =
  match keystroke with 
  | 'd' -> game |> applyUpdate DrawCards
  | 'm' -> game |> nextPhase SelectingSourceStack
  | Number a when (a >= 1 && a <= 6) -> game |> applyUpdate (TableToStack a)
  | _ -> game

let private updateGameSourceStack game keystroke =
  match keystroke with 
  | Number stack when (stack >= 1 && stack <= 6) -> 
      game |> nextPhase (SelectingNumCards stack)
  | '\x1B' -> // [esc] key
      game |> nextPhase General
  | _ -> game

let private updateGameNumCards sourceStack game keystroke =
  let numCardsInStack = 
    game.game.stacks[sourceStack - 1] 
    |> List.filter (fun a -> a.isFaceUp ) 
    |> List.length
  match keystroke with 
  | Number card when (card >= 1 && card <= numCardsInStack) -> 
      game |> nextPhase (SelectingTargetStack (sourceStack, card))
  | 'a' ->
      game |> nextPhase (SelectingTargetStack (sourceStack, numCardsInStack))
  | '\x1B' -> // [esc] key
      game |> nextPhase SelectingSourceStack
  | _ -> game

let private updateGameTargetStack sourceStack numCards game keystroke =
  match keystroke with 
  | Number targetStack when (targetStack >= 1 && targetStack <= 6) -> 
      game |> applyUpdate (MoveCards {sourceStack=sourceStack; numCards=numCards; targetStack=targetStack})
  | '\x1B' -> // [esc] key
      game |> nextPhase (SelectingNumCards sourceStack)
  | _ -> game  

let updateGame (game: MultiPhaseGame) keystroke : MultiPhaseGame =
  match game.phase with 
  | General -> 
      updateGameGeneral game keystroke
  | SelectingSourceStack -> 
      updateGameSourceStack game keystroke
  | SelectingNumCards sourceStack -> 
      updateGameNumCards sourceStack game keystroke
  | SelectingTargetStack (sourceStack, numCards) -> 
      updateGameTargetStack sourceStack numCards game keystroke
