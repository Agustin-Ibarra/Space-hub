const $body = document.querySelector("body");
const $loadMore = document.querySelector(".load-more");
const $miniSpinner = document.querySelector(".mini-spinner");
let offset = 0;

const generateItems = function (items) {
  const $list = document.querySelector(".list-item");
  offset += 12;
  items.forEach(item => {
    const $li = document.createElement("li");
    const $itemImage = document.createElement("img");
    const $divText = document.createElement("div");
    const $title = document.createElement("p");
    const $price = document.createElement("p");
    const $itemLink = document.createElement("a");
    const $icon = document.createElement("span");
    $list.appendChild($li);
    $li.setAttribute("class", "item");
    $li.appendChild($itemImage);
    $li.appendChild($divText);
    $itemImage.setAttribute("class", "item-image");
    $itemImage.setAttribute("src", item.itemImage);
    $divText.appendChild($title);
    $divText.appendChild($price);
    $divText.appendChild($itemLink);
    $divText.setAttribute("class", "item-div-text");
    $title.setAttribute("class", "item-title");
    $title.textContent = item.itemName;
    $price.setAttribute("class", "item-price");
    $price.textContent = `$ ${Number(item.itemUnitPrice).toFixed(2)}`
    $itemLink.setAttribute("class", "item-link");
    $itemLink.setAttribute("href", "/items/details");
    $itemLink.setAttribute("id", item.idItem);
    $itemLink.textContent = "Ver articulo";
    $itemLink.appendChild($icon);
    $icon.setAttribute("class","material-symbols-outlined");
    $icon.textContent = "arrow_forward";
  });
}

fetch(`/api/items/${offset}/${sessionStorage.getItem("idCategory")}`)
  .then(async (response) => {
    if (response.status === 200) {
      const items = await response.json();
      generateItems(items);
      if (items.length < 12) {
        $loadMore.classList.add("hidden");
      }
      else {
        $loadMore.classList.remove("hidden");
      }
    }
  })
  .catch((error) => {
    console.error(error);
  });


$body.addEventListener("click", (e) => {
  if (e.target.matches(".item-link")) {
    sessionStorage.setItem("idItem", e.target.id);
  }
  else if (e.target.matches(".load-more")) {
    $miniSpinner.classList.remove("hidden");
    fetch(`/api/items/${offset}/${sessionStorage.getItem("idCategory")}`)
      .then(async (response) => {
        if (response.status === 200) {
          const items = await response.json();
          generateItems(items);
          if (items.length < 12) {
            $loadMore.classList.add("hidden");
          }
          else {
            $loadMore.classList.remove("hidden");
          }
        }
      })
      .catch((error) => {
        console.error(error);
      })
      .finally(() => {
        $miniSpinner.classList.add("hidden");
      });
  }
})