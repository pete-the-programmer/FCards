---
---

<ol class="articles-list">
  {% for post in site.chapters %}
    <li>
      <a href="{{ site.baseurl }}{{ post.url }}">{{ post.slug }}</a>
    </li>
  {% endfor %}
</ol>