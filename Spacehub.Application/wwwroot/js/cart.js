fetch("/api/cart/items")
.then(async(response)=>{
  if(response.status === 200){
    let quantity = 0;
    let subtotal = 0;
    let total = 0;
    const items = await response.json();
    console.log(items);
    const $list = document.querySelector(".cart-list")
    items.forEach(item => {
      quantity += item.quantity
      subtotal += item.unitPrice * item.quantity;
      total += item.unitPrice * item.quantity;
      const $li = document.createElement("li");
      const $DataDivContainer = document.createElement("div");
      const $imageDiv = document.createElement("div");
      const $image = document.createElement("img");
      const $divItemDescription = document.createElement("div");
      const $itemName = document.createElement("p");
      const $itemPrice = document.createElement("p");
      const $quantity = document.createElement("p");
      const $button = document.createElement("button");
      const $span = document.createElement("span");
      const $quantityResumen = document.querySelector(".quantity-items");
      const $subtotalResumen = document.querySelector(".subtotal");
      const $totalResumen = document.querySelector(".cart-pay-total");
      $list.appendChild($li);
      $li.setAttribute("class","cart-lits-item");
      $li.appendChild($DataDivContainer);
      $li.appendChild($divItemDescription);
      $li.appendChild($button);
      $DataDivContainer.setAttribute("class","cart-item-description-div");
      $DataDivContainer.appendChild($imageDiv);
      $imageDiv.setAttribute("class","cart-image-div");
      $imageDiv.appendChild($image);
      $image.setAttribute("src",item.imagePath);
      $divItemDescription.setAttribute("class","cart-description-div");
      $divItemDescription.appendChild($itemName);
      $divItemDescription.appendChild($itemPrice);
      $divItemDescription.appendChild($quantity);
      $itemName.setAttribute("class","cart-description-text cart-title");
      $itemName.textContent = item.itemName;
      $itemPrice.setAttribute("class","cart-description-text cart-price");
      $itemPrice.textContent = `Precio unitario: $${Number(item.unitPrice).toFixed(2)}`;
      $quantity.setAttribute("class","cart-description-text cart-quantity");
      $quantity.textContent = `Cantidad: ${item.quantity}`;
      $button.setAttribute("class","cart-delete-item-btn");
      $button.setAttribute("id",item.idCart);
      $button.textContent = "Eliminar articulo";
      $button.appendChild($span);
      $span.setAttribute("class","material-symbols-outlined");
      $span.textContent = "delete_forever";
      $totalResumen.textContent = `Total: $${Number(total).toFixed(2)}`;
      $subtotalResumen.textContent = `Subtotal: $${Number(subtotal).toFixed(2)}`;
      $quantityResumen.textContent = `Articulos: ${quantity}`;
    });
  }
  else{
    console.log(response.status);
  }
})
.catch((error)=>{
  console.log(error);
})