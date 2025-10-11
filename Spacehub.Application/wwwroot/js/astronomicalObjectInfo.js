fetch(`/astronomical_objects/detail/api/${sessionStorage.getItem("id")}`)
.then(async(response)=>{
  if(response.status === 200){
    const astronomicalObject = await response.json();
    const textContent = astronomicalObject.textContent.replace(/\r?\n/g, '<br/><br/>');
    console.log(astronomicalObject.textContent.replace("/\r?\n/g","<br/>"));
    const $title = document.querySelector(".object-info-title");
    const $category = document.querySelector(".category");
    const $image = document.querySelector(".image-astronomical-object-info");
    const $textContent = document.querySelector(".object-info-text");
    $title.textContent = astronomicalObject.title;
    $category.textContent = `Categoria: ${astronomicalObject.category}`;
    $image.setAttribute("src",astronomicalObject.imagePath);
    $textContent.innerHTML = textContent;
  }
})
.catch((error)=>{
  console.log(error);
});