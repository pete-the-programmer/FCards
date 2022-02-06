### Code so far

{% assign parts = include.parts | default: 'cards.fs,model.fs,printing.fs,actions.fs,Program.fs' | split: ',' %}

{% for part in parts %}

[{{part}}]({{ site.baseurl }}{{ page.url }}#code_{{part | slugify}})

{:class="collapsible" id="code_{{part | slugify}}"}
```fsharp
{% include src/ch{{page.chapter}}/{{part}} %}
```

{% endfor %}