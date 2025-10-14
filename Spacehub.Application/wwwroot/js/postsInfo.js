const $body = document.querySelector("body");

fetch(`/api/posts/info/${sessionStorage.getItem("id")}`)
.then(async(response)=>{
  if(response.status === 200){
    const post = await response.json();
    const text = post.textContent.replace(/\r?\n/g, '<br/><br/>');
    const $title = document.querySelector(".object-info-title");
    const $category = document.querySelector(".category");
    const $image = document.querySelector(".post-image-div");
    const $subtitle = document.querySelector(".sub-title-post");
    const $textContent = document.querySelector(".object-info-text");
    const $createdAt = document.querySelector(".created-at");
    const date = new Date(post.createdAt);
    $title.textContent = post.title;
    $category.textContent +=  post.category;
    $createdAt.textContent = `Publicado el: ${date.toLocaleDateString()}`;
    $image.setAttribute("src",post.imagePath);
    $subtitle.textContent = post.textDescription;
    $textContent.innerHTML = text;
  }
})
.catch((error)=>{
  console.log(error);
})

fetch("/api/posts/info/suggestion")
.then(async(response)=>{
  if(response.status === 200){
    const suggestions = await response.json();
    const $list = document.querySelector(".object-sub-list");
    suggestions.forEach(suggestion => {
      const $item = document.createElement("li");
      const $divImage = document.createElement("div");
      const $image = document.createElement("img");
      const $title = document.createElement("p");
      const $link = document.createElement("a");
      $list.appendChild($item);
      $item.setAttribute("class","object-sub-list-item");
      $item.appendChild($divImage);
      $item.appendChild($title);
      $item.appendChild($link);
      $divImage.setAttribute("class","object-sub-list-img-div");
      $divImage.appendChild($image);
      $image.setAttribute("src",suggestion.imagePath);
      $title.setAttribute("class","object-sub-list-title");
      $title.textContent = suggestion.title
      $link.setAttribute("class","object-sub-list-link");
      $link.setAttribute("href","/posts/info")
      $link.setAttribute("id",suggestion.id);
      $link.textContent = "Ver mas";
    });
  }
})
.catch((error)=>{
  console.log(error);
})

$body.addEventListener("click",(e)=>{
  if(e.target.matches(".object-sub-list-link")){
    sessionStorage.setItem("id",e.target.id);
  }
})