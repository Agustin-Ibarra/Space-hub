const $idPurchase = document.getElementById("id-purchase");
const $purchaseDate = document.getElementById("purchase-date");
const $purchaseProvider = document.getElementById("purchase-provider");
const $purchaseCustomer = document.getElementById("purchase-customer");
const $purchasesubtotal = document.getElementById("purchase-subtotal");
const $purchaseItemSection = document.querySelector(".purchase-item-data-section");
const $purchaseTotal = document.querySelector(".purchase-total");

const getPurchaseDetails = function (idPurchase){
  fetch(`/api/purchase/detail/${idPurchase}`)
  .then(async(response)=>{
    const purchaseDeatails = await response.json();
    $purchaseCustomer.textContent = `Cliente: ${purchaseDeatails.userData}`;
    purchaseDeatails.purchases.forEach(purchase => {
      const $dataDiv = document.createElement("div");
      const $item = document.createElement("p");
      const $quantity = document.createElement("p");
      const $unitPrice = document.createElement("p");
      const $subtotal = document.createElement("p");
      $purchaseItemSection.appendChild($dataDiv);
      $dataDiv.setAttribute("class","purchase-items-data-div");
      $dataDiv.appendChild($item);
      $dataDiv.appendChild($quantity);
      $dataDiv.appendChild($unitPrice);
      $dataDiv.appendChild($subtotal);
      $item.setAttribute("class","purchase-items-data-text");
      $item.textContent = `Articulo: ${purchase.itemName}`;
      $quantity.setAttribute("class","purchase-items-data-text");
      $quantity.textContent = `Cantidad: ${purchase.quantity}`;
      $unitPrice.setAttribute("class","purchase-items-data-text");
      $unitPrice.textContent = `Precio unitario: $${Number(purchase.unitPrice).toFixed(2)}`;
      $subtotal.setAttribute("class","purchase-items-data-text");
      $subtotal.textContent = `Subtotal: $${Number(purchase.subtotal).toFixed(2)}`;
    });
  })
  .catch((error)=>{
    console.error(error);
  })
}

const generatePurchase = function (itemsList){
  fetch("/api/purchase",{
    method:"POST",
    headers:{"Content-Type":"application/json"},
    body:JSON.stringify({itemsList:itemsList})
  })
  .then(async(response)=>{
    if(response.status === 201){
      const purchase = await response.json();
      const dateFormat = new Date(purchase.datePurchase);
      $idPurchase.textContent = `Nº ${purchase.idPurchase}`;
      $purchaseDate.textContent = `Fecha: ${dateFormat.toLocaleString()}`;
      $purchaseProvider.textContent = "Space Hub";
      $purchaseTotal.textContent = `Total a pagar: $${Number(purchase.totalPurchase).toFixed(2)}`;
      getPurchaseDetails(purchase.idPurchase);  
    }
  })
  .catch((error)=>{
    console.error(error);
  })
}

fetch("/api/cart")
.then(async(response)=>{
  const itemsCartList = await response.json();
  generatePurchase(itemsCartList);
})
.catch((error)=>{
  console.error(error);
})