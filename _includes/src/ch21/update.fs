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

let private nextPhase phase game = {game with phase = phase}

let private updateGameGeneral game keystroke =
  match keystroke with 
  | 'd'   -> game |> applyUpdate DrawCards
  | 'm'   -> game |> nextPhase SelectingSourceStack
  | 'a'   -> game |> nextPhase SelectingAceSource
  | Number a when (a >= 1 && a <= 6) 
          -> game |> applyUpdate (TableToStack a)
  | _     -> game

let private updateGameSourceStack game keystroke =
  match keystroke with 
  | Number stack when (stack >= 1 && stack <= 6) 
            -> game |> nextPhase (SelectingNumCards stack)
  | '\x1B'  -> game |> nextPhase General
  | _       -> game

let private updateGameNumCards sourceStack game keystroke =
  let numCardsInStack = 
    game.game.stacks[sourceStack - 1] 
    |> List.filter (fun a -> a.isFaceUp ) 
    |> List.length
  match keystroke with 
  | Number card -> game |> nextPhase (SelectingTargetStack (sourceStack, card))
  | 'a'         -> game |> nextPhase (SelectingTargetStack (sourceStack, numCardsInStack))
  | '\x1B'      -> game |> nextPhase SelectingSourceStack
  | _           -> game

let private updateGameTargetStack sourceStack numCards game keystroke =
  match keystroke with 
  | Number targetStack when (targetStack >= 1 && targetStack <= 6) -> 
          game 
          |> applyUpdate (MoveCards {sourceStack=sourceStack; numCards=numCards; targetStack=targetStack})
  | '\x1B' -> // [esc] key
          game |> nextPhase (SelectingNumCards sourceStack)
  | _ ->  game  


let private hasWon game =
  game.game.aces 
  |> List.map List.length
  |> List.sum
  |> (=) 52   // Shortcut: 
              //  It means that we use `=` as a function 
              //  with 52 as the first input
              //  and the piped value as the second input

let updateAceSourceStack game keystroke =
  let updatedGame = 
    match keystroke with 
    | Number sourceStack when (sourceStack >= 1 && sourceStack <= 6) 
              -> game |> applyUpdate (StackToAce sourceStack)
    | 't'     -> game |> applyUpdate TableToAce
    | '\x1B'  -> game |> nextPhase General
    | _       -> game  
  // check if the player has won the game after this update
  { updatedGame with phase = if hasWon updatedGame then PlayerHasWon else updatedGame.phase }  

let updatePlayerHasWon game keystroke =
  match keystroke with 
  | 'y' ->{game=newDeck |> shuffle |> deal; phase=General}
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
  | SelectingAceSource ->
      updateAceSourceStack game keystroke
  | PlayerHasWon ->
      updatePlayerHasWon game keystroke