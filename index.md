---
description: Playing card games with F#
---

{% assign parts = site.chapters | group_by: "part" %}
{% for part in parts %}


### [ {{ part.name }} ]( {{ site.baseurl }}{{ page.url }}#{{part.name | slugify}} )

{: class="naked collapsible" id="{{part.name | slugify}}"}
{% for post in part.items %} 
- [{{ post.slug }}]({{ site.baseurl }}{{ post.url }})
{% endfor %}

{% endfor %}

