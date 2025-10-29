const $body = document.querySelector("body");
const $spinner = document.querySelector(".mini-spinner");

$body.addEventListener("click",(e)=>{
  if(e.target.matches(".send-form")){
    e.preventDefault();
    const $error = document.querySelector(".error-form");
    $error.textContent = "";
    $spinner.classList.remove("hidden");
    const $username = document.getElementById("username");
    const $password = document.getElementById("password");
    fetch("/api/login",{
      method:"POST",
      headers:{"Content-Type":"application/json"},
      body:JSON.stringify({
        username:$username.value,
        password:$password.value
      })
    })
    .then(async(response)=>{
      if(response.status >= 400){
        const errorMessage = await response.json();
        $error.textContent = errorMessage.error
      }
      else if(response.status === 200){
        if(sessionStorage.getItem("redirect")){
          window.location.href = sessionStorage.getItem("redirect");
        }
        else{
          window.location.href = "/";
        }
      }
    })
    .catch((error)=>{
      console.log(error)
    })
    .finally(()=>{
      $spinner.classList.add("hidden");
    })
  }
})