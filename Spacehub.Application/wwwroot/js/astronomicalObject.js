const $body = document.querySelector("body");
const $astronomicalObjectList = document.querySelector(".content-list");
let offset = 0;

const generateObjects = function () {
  fetch(`/astronomical_objects/api/${offset}`)
  .then(async (response) => {
    if (response.status === 200) {
      const astronomicalObjectList = await response.json();
      astronomicalObjectList.forEach(astronomicalObject => {
        const $li = document.createElement("li");
        const $imageDiv = document.createElement("div");
        const $image = document.createElement("img");
        const $textDiv = document.createElement("div");
        const $title = document.createElement("p");
        const $category = document.createElement("p");
        const $link = document.createElement("a");
        $astronomicalObjectList.appendChild($li);
        $li.setAttribute("class", "content-list-item");
        $imageDiv.setAttribute("class", "content-list-item-img-div");
        $image.setAttribute("src", astronomicalObject.imagePath);
        $image.setAttribute("class", "content-list-item-img");
        $textDiv.setAttribute("class", "content-list-item-text-div");
        $title.setAttribute("class", "content-title");
        $category.setAttribute("class","category");
        $link.setAttribute("class", "article-content-link");
        $link.setAttribute("href", "/astronomical_objects/info");
        $link.setAttribute("id",astronomicalObject.id);
        $li.appendChild($imageDiv);
        $li.appendChild($textDiv);
        $imageDiv.appendChild($image);
        $textDiv.appendChild($category);
        $textDiv.appendChild($title);
        $textDiv.appendChild($link);
        $image.setAttribute("src", astronomicalObject.imagePath);
        $title.textContent = astronomicalObject.title;
        $link.textContent = "Ver articulo completo";
        $category.textContent = `Categoria: ${astronomicalObject.category}`
      })
      offset += 10;
    }
    else{
      console.log(response.status);
    }
  })
  .catch ((error) => {
  console.log(error);
});
}

generateObjects();


$body.addEventListener("click", (e) => {
  // console.log(e.target);
  if(e.target.matches(".article-content-link")){
    e.preventDefault();
    sessionStorage.setItem("id",e.target.id);
    window.location.href = "/astronomical_objects/info"
  }
});