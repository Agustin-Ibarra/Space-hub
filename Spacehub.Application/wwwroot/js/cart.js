const $body = document.querySelector("body");
const $cartList = document.querySelector(".cart-list");
const $emptyCart = document.querySelector(".cart-empty-text");
const $quantityResumen = document.querySelector(".quantity-items");
const $subtotalResumen = document.querySelector(".subtotal");
const $totalResumen = document.querySelector(".cart-pay-total");
const cartList = [];
let itemsList;

const stripe = Stripe("pk_test_51OmMbwCPAgcnoLpT0U8CaOBQVRL2G1vXk8GKB6xZxbUTUF2uHMAZjRv37F7JBUSgF9WNJQQ4TfBOTcizQcwEswLN00icpSllq2");

fetch("/api/cart/items")
  .then(async (response) => {
    if (response.status === 200) {
      let quantity = 0;
      let subtotal = 0;
      let total = 0;
      const items = await response.json();
      itemsList = items;
      items.forEach(item => {
        const cart = {
          idCart: item.idCart,
          idItem: item.idItem,
          quantity: item.quantity
        }
        cartList.push(cart);
        quantity += item.quantity
        subtotal += item.unitPrice * item.quantity;
        total += item.unitPrice * item.quantity;
        $emptyCart.classList.add("hidden");
        const $li = document.createElement("li");
        const $DataDivContainer = document.createElement("div");
        const $imageDiv = document.createElement("div");
        const $image = document.createElement("img");
        const $divItemDescription = document.createElement("div");
        const $itemName = document.createElement("p");
        const $itemPrice = document.createElement("p");
        const $quantity = document.createElement("p");
        const $deleteBtn = document.createElement("button");
        const $span = document.createElement("span");
        const $spinner = document.createElement("div");
        $cartList.appendChild($li);
        $li.setAttribute("class", "cart-lits-item");
        $li.appendChild($DataDivContainer);
        $li.appendChild($divItemDescription);
        $li.appendChild($deleteBtn);
        $DataDivContainer.setAttribute("class", "cart-item-description-div");
        $DataDivContainer.appendChild($imageDiv);
        $imageDiv.setAttribute("class", "cart-image-div");
        $imageDiv.appendChild($image);
        $image.setAttribute("src", item.imagePath);
        $image.setAttribute("id", item.idItem);
        $divItemDescription.setAttribute("class", "cart-description-div");
        $divItemDescription.appendChild($itemName);
        $divItemDescription.appendChild($itemPrice);
        $divItemDescription.appendChild($quantity);
        $itemName.setAttribute("class", "cart-description-text cart-title");
        $itemName.textContent = item.itemName;
        $itemPrice.setAttribute("class", "cart-description-text cart-price");
        $itemPrice.textContent = `Precio unitario: $${Number(item.unitPrice).toFixed(2)}`;
        $quantity.setAttribute("class", "cart-description-text cart-quantity");
        $quantity.textContent = `Cantidad: ${item.quantity}`;
        $deleteBtn.setAttribute("class", "cart-delete-item-btn");
        $deleteBtn.setAttribute("id", item.idCart);
        $deleteBtn.textContent = "Eliminar";
        $deleteBtn.appendChild($span);
        $deleteBtn.appendChild($spinner);
        $spinner.setAttribute("class", "mini-spinner hidden");
        $span.setAttribute("class", "material-symbols-outlined cart-delete-icon");
        $span.textContent = "delete_forever";
        $totalResumen.textContent = `Total: $${Number(total).toFixed(2)}`;
        $subtotalResumen.textContent = `Subtotal: $${Number(subtotal).toFixed(2)}`;
        $quantityResumen.textContent = `Articulos: ${quantity}`;
      });
    }
    else if (response.status === 404) {
      $emptyCart.classList.remove("hidden");
    }
  })
  .catch((error) => {
    console.error(error);
  })

$body.addEventListener("click", (e) => {
  if (e.target.matches(".cart-delete-item-btn")) {
    let cartToDelete;
    e.target.childNodes[1].classList.add("hidden");
    e.target.childNodes[2].classList.remove("hidden");
    cartList.forEach(cart => {
      if (Number(e.target.id) === cart.idCart) {
        cartToDelete = cart;
        index = cartList.indexOf(cartToDelete)
      }
    });
    fetch("/api/cart/items", {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(cartToDelete)
    })
      .then((response) => {
        if (response.status === 204) {
          window.location.reload();
        }
      })
      .catch((error) => {
        console.error(error);
      })
      .finally(() => {
        e.target.childNodes[1].classList.remove("hidden");
        e.target.childNodes[2].classList.add("hidden");
      });
  }
  else if (e.target.matches(".cart-pay-btn")) {
    e.target.childNodes[1].classList.add("hidden");
    e.target.childNodes[3].classList.remove("hidden");
    fetch("/api/payment", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ itemsList: cartList })
    })
      .then(async (response) => {
        const session = await response.json();
        console.log(session);
        await stripe.redirectToCheckout({ sessionId: session.idSession });
      })
      .catch((error) => {
        console.error("Error al iniciar el pago:", error);
      })
      .finally(() => {
        e.target.childNodes[1].classList.remove("hidden");
        e.target.childNodes[3].classList.add("hidden");
      });
  }
});