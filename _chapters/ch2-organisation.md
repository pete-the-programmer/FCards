---
title: "Chapter 2: Organisation"
layout: default
---
## Modules
You can organise your code into "modules".

E.g. a simple module in a file
```fsharp
{% include_relative src/ch2.organisation.fs %}

```

... or even multiple modules under a namespace in one file
```fsharp
{% include_relative src/ch2.organisation2.fs %}

```

## Using Modules
From another file/module you can reference the contents as a name-space, or you can also include a module/name-space using the keyword __open__
```fsharp
{% include_relative src/ch2.organisation_usage.fs %}

```
