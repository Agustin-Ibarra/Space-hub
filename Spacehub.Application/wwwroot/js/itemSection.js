const $body = document.querySelector("body");

$body.addEventListener("click",(e)=>{
  sessionStorage.setItem("idCategory",e.target.id);
})