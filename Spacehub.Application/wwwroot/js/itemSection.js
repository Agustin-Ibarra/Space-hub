const $body = document.querySelector("body");

$body.addEventListener("click",(e)=>{
  e.preventDefault();
  if(e.target.matches(".article-content-link" || e.target.matches(".article-content-link-icon"))){
    sessionStorage.setItem("idCategory",e.target.id);
    window.location.href = "/items";
  }
});