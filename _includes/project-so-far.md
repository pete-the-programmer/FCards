### Code so far {{page.chapter}}

{% assign parts = include.parts | default: 'cards.fs,model.fs,printing.fs,actions.fs,Program.fs' | split: ',' %}

{% for part in parts %}
{% assign ext = part | split: '.' %}
[{{part}}]({{ site.baseurl }}{{ page.url }}#code_{{part | slugify}})

{:class="collapsible" id="code_{{part | slugify}}"}
```{{ ext[1] | replace: 'fs', 'fsharp' }}
{% include src/ch{{page.chapter}}/{{part}} %}
```

{% endfor %}