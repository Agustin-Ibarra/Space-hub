const $body = document.querySelector("body");
const $astronomicalObjectList = document.querySelector(".content-list");
let offset = 0;

const generateObjects = function () {
  fetch(`/astronomical_objects/api/${offset}`)
  .then(async (response) => {
    if (response.status === 200) {
      const astronomicalObjectList = await response.json();
      console.log(astronomicalObjectList);
      astronomicalObjectList.forEach(object => {
        const $li = document.createElement("li");
        const $imageDiv = document.createElement("div");
        const $image = document.createElement("img");
        const $textDiv = document.createElement("div");
        const $title = document.createElement("p");
        const $link = document.createElement("a");
        $astronomicalObjectList.appendChild($li);
        $li.setAttribute("class", "content-list-item");
        $imageDiv.setAttribute("class", "content-list-item-img-div");
        $image.setAttribute("src", object.imagePath);
        $image.setAttribute("class", "content-list-item-img");
        $textDiv.setAttribute("class", "content-list-item-text-div");
        $title.setAttribute("class", "content-title");
        $link.setAttribute("class", "article-content-link");
        $link.setAttribute("href", "/astronomical_objects/info");
        $li.appendChild($imageDiv);
        $li.appendChild($textDiv);
        $imageDiv.appendChild($image);
        $textDiv.appendChild($title);
        $textDiv.appendChild($link);
        $image.setAttribute("src", object.imagePath);
        $title.textContent = object.title;
        $link.textContent = "Ver articulo completo";
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
  console.log(e.target.matches);
});