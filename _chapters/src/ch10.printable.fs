module Fcards.ch10

type CardNumber =
  | Two 
  | Three
  | Four
  | Five
  | Six
  | Seven
  | Eight
  | Nine
  | Ten
  | Jack
  | Queen
  | King
  | Ace
  with 
    override this.ToString() = 
      match this with 
      | Two -> "2"
      | Three -> "3"
      | Four -> "4"
      | Five -> "5"
      | Six -> "6"
      | Seven -> "7"
      | Eight -> "8"
      | Nine -> "9"
      | Ten -> "10"
      | Jack -> "J"
      | Queen -> "Q"
      | King -> "K"
      | Ace -> "A"

type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker
  with  
    override this.ToString() = 
      match this with 
      | Hearts x -> "\u2665" + x.ToString()
      | Diamonds x -> "\u2666" + x.ToString()
      | Clubs x -> "\u2663" + x.ToString()
      | Spades x -> "\u2660" + x.ToString()
      | Joker -> "Jok"

let myCard = Hearts Three