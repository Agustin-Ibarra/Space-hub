const $body = document.querySelector("body");
const $itemImage = document.querySelector(".item-detail-img");
const $itemName = document.querySelector(".item-detail-name");
const $itemDescription = document.querySelector(".item-detail-description");
const $itemPrice = document.querySelector(".item-detail-price");
const $itemStock = document.querySelector(".item-detail-stock");
const $stock = document.querySelector(".item-detail-options-quantity");
const $imageSpinner = document.querySelector(".medium-spinner");
let quantity = 1;

fetch(`/api/items/detail/${sessionStorage.getItem("idItem")}`)
.then(async(response)=>{
  if(response.status === 200){
    const item = await response.json();
    $itemImage.setAttribute("src",item.itemImage);
    $itemImage.setAttribute("id",item.idItem)
    $itemDescription.textContent = item.itemDescription;
    $itemName.textContent = item.itemName;
    $itemPrice.textContent = `$ ${Number(item.itemUnitPrice).toFixed(2)}`;
    $itemStock.textContent = `Stock disponible: ${item.itemstock}`;
    $imageSpinner.classList.add("hidden");
  }
})
.catch((error)=>{
  console.log(error);
})

$body.addEventListener("click",(e)=>{
  if(e.target.matches(".add-btn")){
    if(quantity < 5 && quantity < Number($itemStock.textContent.replace("Stock disponible: ",""))){
      quantity ++;
      $stock.textContent = quantity;
    }
  }
  else if(e.target.matches(".less-btn")){
    if(quantity > 1){
      quantity --;
      $stock.textContent = quantity;
    }
  }
  else if(e.target.matches(".item-detail-add-cart-btn") || e.target.matches(".add-cart-icon-btn")){
    $miniSpinner = document.querySelector(".mini-spinner");
    $addCartIcon = document.querySelector(".add-cart-icon");
    $miniSpinner.classList.remove("hidden");
    $addCartIcon.classList.add("hidden");
    const itemObject = {
      idItem : $itemImage.id,
      quantity : quantity
    }
    fetch("/api/cart",{
      method:"POST",
      headers:{"Content-Type":"application/json"},
      body:JSON.stringify(itemObject)
    })
    .then(async(response)=>{
      if(response.status === 401){
        window.location.href = "/login";
      }
      else{
        if(response.status === 201){
          window.location.href = "/cart";
        }
      }
    })
    .catch((error)=>{
      console.log(error);
    })
    .finally(()=>{
      $miniSpinner.classList.add("hidden");
      $addCartIcon.classList.remove("hidden");
    });
  }
})