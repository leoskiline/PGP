if (localStorage.getItem("role") != "Administrador") {
    debugger
    elems = document.getElementsByClassName("admin");
    for (let i = 0; i < elems.length; i++) {
        elems[i].style.display = "none";
    }
}
  