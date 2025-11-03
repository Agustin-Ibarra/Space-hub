const $body = document.querySelector("body");

$body.addEventListener("click",(e)=>{
  sessionStorage.setItem("idSection",e.target.id);
})