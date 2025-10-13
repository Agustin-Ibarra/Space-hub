let offset = 0;

fetch(`/posts/api/${offset}`)
.then(async(response)=>{
  if(response.status === 200){
    const posts = await response.json();
    const $list = document.querySelector(".content-list");
    posts.forEach(post => {
      const $li = document.createElement("li");
      const $imageDiv = document.createElement("div");
      const $image = document.createElement("img");
      const $divText = document.createElement("div");
      const $title = document.createElement("p");
      const $category = document.createElement("p");
      const $link = document.createElement("a");
      $list.appendChild($li);
      $li.appendChild($imageDiv);
      $li.appendChild($divText);
      $li.setAttribute("class","content-list-item");
      $imageDiv.setAttribute("class","content-list-item-img-div");
      $imageDiv.appendChild($image);
      $image.setAttribute("class","content-list-item-img");
      $image.setAttribute("src",post.imagePath);
      $divText.setAttribute("class","content-list-item-text-div");
      $divText.appendChild($title);
      $divText.appendChild($category);
      $divText.appendChild($link);
      $title.setAttribute("class","content-title");
      $title.textContent = post.title;
      $category.setAttribute("class","category");
      $category.textContent = `Categoria: ${post.category}`;
      $link.setAttribute("class","article-content-link");
      $link.setAttribute("src",post.id);
      $link.textContent = "Ver notitcia completa";
    });
    console.log(posts);
  }
})
.catch((error)=>{
  console.log(error);
})