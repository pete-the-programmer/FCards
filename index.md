---
description: Playing card games with F#
---

Welcome and follow me through actual code from zero __F#__ knowledge to a working app in the cloud.

I sometimes struggle with lessons and docs that are structured primarily around the language using a vague real-world domain.
So instead we'll start with a real challenge (making a card game) and learn what the language provides to make our dreams a reality.

{:class="jumbo"}
This is all about my journey of writing a card game or two using __F#__

---

{% assign parts = site.chapters | group_by: "part" %}
{% for part in parts %}
### [ {{ part.name }} ]( {{ site.baseurl }}{{ page.url }}#{{part.name | slugify}} )

{: class="naked collapsible" id="{{part.name | slugify}}"}
{% for post in part.items %} 
- [{{ post.slug }}]({{ site.baseurl }}{{ post.url }})
{% endfor %}

{% endfor %}

### [ Multi-player (still writing)]( {{ site.baseurl }}{{ page.url }}#multi-player )

{: class="naked collapsible" id="multi-player"}
- Calling home
- Identifying yourself
- Remembering your game
- Making moves
- Teaming up