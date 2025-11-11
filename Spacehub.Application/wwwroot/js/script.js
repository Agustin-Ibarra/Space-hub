const $bodyElement = document.querySelector("body");
const $loaderSection = document.querySelector(".load-section");

window.addEventListener("load",()=>{
  $bodyElement.removeChild($loaderSection);
});