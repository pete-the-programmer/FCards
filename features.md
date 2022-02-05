---
title: By Language Feature
description: Concepts explored in the series and where to find them
---

### Concepts

{% assign features =  site.chapters | map: 'feature' | uniq | where_exp: 'item', "item != nil" | sort_natural %}

{% for feature in features %}
#### {{ feature }}
{% for post in site.chapters %}
  {% if post.feature contains feature %}
  [{{ post.slug }}]({{ site.baseurl }}{{ post.url }})
  {% endif %}
{% endfor %}

{% endfor %}

### Keywords, Symbols, and Standard Functions

{% assign keywords =  site.chapters | map: 'keyword' | uniq | where_exp: 'item', "item != nil" | sort_natural %}

{% for keyword in keywords %}
#### {{ keyword }}
{% for post in site.chapters %}
  {% if post.keyword contains keyword %}
  [{{ post.slug }}]({{ site.baseurl }}{{ post.url }})
  {% endif %}
{% endfor %}

{% endfor %}