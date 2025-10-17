const $body = document.querySelector("body");

$body.addEventListener("click",(e)=>{
  console.log(e.target);
  if(e.target.matches(".send-form")){
    e.preventDefault();
    const $username = document.getElementById("username");
    const $password = document.getElementById("password");
    console.log($username.value,$password.value);
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
        const $error = document.querySelector(".error-form");
        const errorMessage = await response.json();
        $error.textContent = errorMessage.error
      }
      else if(response.status === 200){
        console.log("authorized");
      }
    })
    .catch((error)=>{
      console.log(error)
    })
  }
})