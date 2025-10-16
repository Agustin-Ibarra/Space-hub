const $body = document.querySelector("body");
let offset = 0;

fetch(`/api/items/${offset}`)
.then(async(response)=>{
  if(response.status === 200){
    const $list = document.querySelector(".list-item");
    const items = await response.json();
    items.forEach(item => {
      const $li = document.createElement("li");
      const $itemImage = document.createElement("img");
      const $divText = document.createElement("div");
      const $title = document.createElement("p");
      const $price = document.createElement("p");
      const $itemLink = document.createElement("a");
      $list.appendChild($li);
      $li.setAttribute("class","item");
      $li.appendChild($itemImage);
      $li.appendChild($divText);
      $itemImage.setAttribute("class","item-image");
      $itemImage.setAttribute("src",item.itemImage);
      $divText.appendChild($title);
      $divText.appendChild($price);
      $divText.appendChild($itemLink);
      $divText.setAttribute("class","item-div-text");
      $title.setAttribute("class","item-title");
      $title.textContent = item.itemName;
      $price.setAttribute("class","item-price");
      $price.textContent = Number(item.itemUnitPrice).toFixed(2);
      $itemLink.setAttribute("class","item-link");
      $itemLink.setAttribute("href","/items/details");
      $itemLink.setAttribute("id",item.idItem);
      $itemLink.textContent = "Ver articulo";
    });
    console.log(items);
  }
})
.catch((error)=>{
  console.log(error);
})

$body.addEventListener("click",(e)=>{
  if(e.target.matches(".item-link")){
    sessionStorage.setItem("idItem",e.target.id);
  }
})