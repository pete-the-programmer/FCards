---
title: Types
layout: default
---
## Records
A _record_ is a type of thing that has multiple parts to its value, like a name, or id, or a number.

Each of these parts has a label and a type, which itself can even be another _record_ type.

Traditionally, types of things (like a record definition) start with a capital letter, and instances of these things (e.g a variable) start with a small letter.

```fsharp
{% include_relative src/ch3.record.fs %}
```

> Note that in __F#__ parts of a type are separated with a semicolon (;) or are put on separate lines at the same level of indentation


## Discriminated Union 
A _discriminated union_ (DU for short) is a type of thing that can be __either__ of its parts, but only one at a time.

For instance a DU for the suits of a deck of cards would be...

```fsharp
{% include_relative src/ch3.du.fs %}
```

A more complex form of a DU can specify a type for each of the parts.  The types of the parts don't have to be the same.
```fsharp
{% include_relative src/ch3.du2.fs %}
```
Note that the labels for the DU _have_ to start with a capital letter.