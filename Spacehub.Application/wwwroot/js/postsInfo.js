fetch(`/api/posts/info/${sessionStorage.getItem("id")}`)
.then(async(response)=>{
  if(response.status === 200){
    const post = await response.json();
    console.log(post);
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