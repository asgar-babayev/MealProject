let hamburger = document.getElementById("hamburger");


// hamburger.addEventListener("click", function () {
//     this.classList.toggle("d-none");

// })


function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

let btn = document.querySelectorAll(".btncar");
let data = document.querySelectorAll(".cat-img");




getMeal();
btn.forEach(x => {
    x.addEventListener("click", function () {
        let id = x.getAttribute("data-id");
        for (var i = 0; i < data.length; i++) {
            let dtid = data[i].getAttribute("data-target");
            if (id == dtid) {
                data[i].classList.add("active");
            }
            else {
                data[i].classList.remove("active");
            }
        }
    })
})





function getMeal() {
    btn.forEach(x => {
        let id = x.getAttribute("data-id");
        for (var i = 0; i < data.length; i++) {
            let dtid = data[i].getAttribute("data-target");
            if (id == dtid) {
                data[i].classList.add("active");
            }
            else {
                data[i].classList.remove("active");
            }
        }
    })
}