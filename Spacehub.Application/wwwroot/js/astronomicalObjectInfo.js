fetch(`/api/astronomical_objects/info/${sessionStorage.getItem("id")}`)
.then(async(response)=>{
  if(response.status === 200){
    const astronomicalObject = await response.json();
    const textContent = astronomicalObject.textContent.replace(/\r?\n/g, '<br/><br/>');
    const $title = document.querySelector(".object-info-title");
    const $category = document.querySelector(".category");
    const $image = document.querySelector(".image-astronomical-object-info");
    const $textContent = document.querySelector(".object-info-text");
    $title.textContent = astronomicalObject.title;
    $category.textContent = `Categoria: ${astronomicalObject.category}`;
    $image.setAttribute("src",astronomicalObject.imagePath);
    $textContent.innerHTML = textContent;
  }
})
.catch((error)=>{
  console.log(error);
});

fetch(`/api/astronomical_objects/suggestion/${sessionStorage.getItem("id")}`)
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
      $link.setAttribute("href","/astronomical_objects/info")
      $link.setAttribute("id",suggestion.id);
      $link.textContent = "Ver mas";
    });
  }
})
.catch((error)=>{
  console.log(error);
})