const $body = document.querySelector("body");
const $list = document.querySelector(".content-list");
const $loadMore = document.querySelector(".load-more");
const $miniSpinner = document.querySelector(".mini-spinner");
let offset = 0;

const generateItems = function (posts) {
  offset += 12;
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
    $li.setAttribute("class", "content-list-item");
    $imageDiv.setAttribute("class", "content-list-item-img-div");
    $imageDiv.appendChild($image);
    $image.setAttribute("class", "content-list-item-img");
    $image.setAttribute("src", post.imagePath);
    $divText.setAttribute("class", "content-list-item-text-div");
    $divText.appendChild($title);
    $divText.appendChild($category);
    $divText.appendChild($link);
    $title.setAttribute("class", "content-title");
    $title.textContent = post.title;
    $category.setAttribute("class", "category");
    $category.textContent = `Categoria: ${post.category}`;
    $link.setAttribute("class", "item-link");
    $link.setAttribute("id", post.id);
    $link.setAttribute("href", "/posts/info");
    $link.textContent = "Ver notitcia completa";
  });
}

fetch(`/api/posts/${offset}`)
  .then(async (response) => {
    if (response.status === 200) {
      const posts = await response.json();
      generateItems(posts);
      if (posts.length < 12) {
        $loadMore.classList.add("hidden");
      }
      else {
        $loadMore.classList.remove("hidden");
      }
    }
  })
  .catch((error) => {
    console.log(error);
  })
  .finally(() => {
    $miniSpinner.classList.add("hidden");
  });

$body.addEventListener("click", (e) => {
  if (e.target.matches(".item-link")) {
    sessionStorage.setItem("id", e.target.id);
  }
  else if (e.target.matches(".load-more")) {
    $miniSpinner.classList.remove("hidden");
    fetch(`/api/posts/${offset}`)
      .then(async (response) => {
        if (response.status === 200) {
          const posts = await response.json();
          generateItems(posts);
          if (posts.length < 12) {
            $loadMore.classList.add("hidden");
          }
          else {
            $loadMore.classList.remove("hidden");
          }
        }
      })
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {
        $miniSpinner.classList.add("hidden");
      });
  }
})