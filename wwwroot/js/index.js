var FakeQuantity = document.getElementById("FakeQuantity");
var Quantity = document.getElementById("Quantity");
var Price = document.getElementById("Price");
var FakePrice = document.getElementById("FakePrice");
var soluong = 1;
var tong = 0;
var QuantityCart = document.getElementById("QuantityCart");
var IndexQuantity = document.getElementById("IndexQuantity");
startLoad();


function startLoad() {
    if (IndexQuantity != null) {
        localStorage.setItem("quantity", IndexQuantity.value);
        loadCart(localStorage.getItem("quantity"));
    } else {
        loadCart(localStorage.getItem("quantity"));
    }
}


function loadCart(x) {
    QuantityCart.innerHTML = x;
}
function Plus(x) {
    var gia = Number(FakePrice.innerHTML);
    soluong += x;
    if (soluong == 0) {
        alert("Quantity must > 0");
        soluong = 1;
        return 0;
    }
    tong = gia * soluong;
    FakeQuantity.textContent = soluong;
    Quantity.value = soluong;
    Price.value = tong;
}
