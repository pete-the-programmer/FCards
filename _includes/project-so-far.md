#### Code so far

{% assign parts = 'cards,model,printing,actions,Program' | split: ',' %}

{% for part in parts %}

[{{part}}]({{ site.baseurl }}{{ page.url }}#code_{{part}})

{:class="collapsible" id="code_{{part}}"}
```fsharp
{% include src/ch{{page.chapter}}/{{part}}.fs %}
```

{% endfor %}