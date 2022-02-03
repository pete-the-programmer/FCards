---
---
`Playing card games with F#`

{% assign parts = site.chapters | group_by: "part" %}
{% for part in parts %}
### {{ part.name }}
{% for post in part.items %}
  [{{ post.slug }}]({{ site.baseurl }}{{ post.url }})
{% endfor %}

{% endfor %}

