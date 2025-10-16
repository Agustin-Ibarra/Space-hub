fetch(`/api/items/detail/${sessionStorage.getItem("idItem")}`)
.then(async(response)=>{
  if(response.status === 200){
    const item = await response.json();
    console.log(item);
    const $itemImage = document.querySelector(".item-detail-img");
    const $itemName = document.querySelector(".item-detail-name");
    const $itemDescription = document.querySelector(".item-detail-description");
    const $itemPrice = document.querySelector(".item-detail-price");
    const $itemStock = document.querySelector(".item-detail-stock");
    $itemImage.setAttribute("src",item.itemImage);
    $itemDescription.textContent = item.itemDescription;
    $itemName.textContent = item.itemName;
    $itemPrice.textContent = `$ ${Number(item.itemUnitPrice).toFixed(2)}`;
    $itemStock.textContent = `Stock disponible: ${item.itemstock}`;
  }
})
.catch((error)=>{
  console.log(error);
})