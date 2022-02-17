{% assign chapterLinked = site.chapters | where: "chapter", include.chapter | first %}
[chapter {{chapterLinked.chapter}}]({{site.baseurl}}{{chapterLinked.url}})