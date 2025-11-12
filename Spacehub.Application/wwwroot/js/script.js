const $bodyElement = document.querySelector("body");
const $loaderSection = document.querySelector(".load-section");

window.addEventListener("load",()=>{
  $bodyElement.removeChild($loaderSection);
});

$bodyElement.addEventListener("click",(e)=>{
  if(e.target.matches("#cart-link")){
    sessionStorage.setItem("redirect","/cart");
  }
})