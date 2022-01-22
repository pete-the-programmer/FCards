---
title: Interacting with a player - listening
concept: I/O
layout: default
---

## Listening to the player
To interact with a game we would need a simple play loop:
1. Prompt the user
1. Listen for a command
1. Execute the command
1. Go back to 1.

<!-- To play a game, we will need a standard play loop:
1. Print out state of the game
2. Print out list of commands the player can perform (optional?)
3. Print out a prompt
4. Wait for player to type a command
5. Execute the command
6. Print out result of command (maybe - depends if the state is obvious enough)
7. Go back to 1. -->

A simple play loop for the command line would look like this:
```fsharp
{% include_relative src/ch11.runloop.fs %}
```
In this example it just prints out the player's choice until the player chooses 'q'.  What this does line-by-line is:
1. Read a key from the keyboard. Note that the key is converted to lower-case so we don't have to deal with someone using capitals
1. Initialise an infinite sequence of key-reads.  We won't use them all, but we are saying we don't care how many there are as long as they don't run out.  The key-read is only _actually_ performed when a further line asks for one (i.e. lazy evaluation)
1. Keep taking keystrokes (and pass them on as a new sequence) while the key isn't a 'q'
1. Iterate (i.e. loop) though all of the keystrokes and run the function on them.  In this case the function just prints out the key.  The format string `%A` is a catch-all format where the compiler chooses the "best" format for the value.
