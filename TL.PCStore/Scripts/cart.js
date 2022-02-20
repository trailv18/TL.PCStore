var count = 0;

function plus(id) {
    let c = document.getElementById(id);  
        count = c.value;
        count++;
        c.value = count;
   }
function minus(id) {
    let c = document.getElementById(id);
    count = c.value;
    if (count > 1) {
        count--;
        c.value = count;
    }
}


function totals() {
    let total = 0;
    let products = [];
    products = JSON.parse(localStorage.getItem('products'));
    if (!products) {
        total = 0;
    } else {
        for (let i = 0; i < products.length; i++) {
            total += (products[i].qty * products[i].price);
        }
        localStorage.setItem('total', String(total));
    }
}

function deleteCart(item) {
    let products = [];
    products = JSON.parse(localStorage.getItem('products'));

    if (products.length < 0) {
        loadDataToTable();
        viewCartHeader();
    } else {

        for (let i = 0; i < products.length; i++) {
            if (products[i].id == item) {
                var ans = confirm("Xóa khỏi giỏ hàng?");
                if (ans) {
                    products.splice(i, 1);
                }
            }
        }
        localStorage.setItem("products", JSON.stringify(products));
        loadDataToTable();
        viewCartHeader();
    }
}



function updateCartDes(item) {
    let products = [];
    products = JSON.parse(localStorage.getItem('products'));
    let c = document.getElementById(item);
    count = c.value;
    if (count > 1) {
        count--;
        c.value = count;
    }
    for (let i = 0; i < products.length; i++) {
        if (products[i].id == item) {
            products[i].qty = count;
        }
    }
    localStorage.setItem("products", JSON.stringify(products));

    loadDataToTable();
    viewCartHeader();
}

function updateCartAsc(item) {
    let products = [];
    products = JSON.parse(localStorage.getItem('products'));
    let c = document.getElementById(item);

    count = c.value;
    count++;
    c.value = count;
    for (let i = 0; i < products.length; i++) {
        if (products[i].id == item) {
            products[i].qty = count;
        }
    }
    localStorage.setItem("products", JSON.stringify(products));

    loadDataToTable();
    viewCartHeader();
}

setInterval(function () {
    let products = [];
    products = JSON.parse(localStorage.getItem('products'));
    if (JSON.parse(localStorage.getItem('products'))) {
        document.getElementById("lblCartCount").innerHTML = products.length;
    } else {
        document.getElementById("lblCartCount").innerHTML = 0;
    }
});

function viewCartHeader() {

    totals();

    let cartData = document.getElementById("cartData");

    cartData.innerHTML = '';

    let products = [];
    products = JSON.parse(localStorage.getItem('products'));

    let total = localStorage.getItem('total');

    if (!products) {
        document.getElementById("total").innerHTML = 0;
        cartData.innerHTML = "<p class='text-center'>Giỏ hàng trống!</p>";
    }
    else
        if (products) {
            for (var i = 0; i < products.length; i++) {
                var html = document.createElement("div");
                var formatter = new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                });

                html.innerHTML =
                    "<div class='row'>" +
                    "<div class='col-md-3 cart-detail-img'>" +
                    "<img src='" + products[i].image.replace('~', '') + "'>" +
                    "</div>" +
                    " <div class='col-md-9 cart-detail-product'>" +
                    "<p style='font-size:10px;'>" +
                    "<a class='text-decoration-none' href = '/products/getProductDetail?url=" + products[i].url + "'> " + products[i].name +
                    "</a >" +
                    "</p > " +
                    "<span class='price'>" + formatter.format(products[i].price) + "</span> " +
                    "<span style='font-size:12px;' class='count'> Số lượng: " + products[i].qty + " </span>" +
                    "</div>" +
                    "</div>" +
                    "<hr/>";

                cartData.appendChild(html);
                total = localStorage.getItem('total');
                document.getElementById("total").innerHTML = formatter.format(total);
            }
        }
}


function loadDataToTable() {
    totals();

    let cartDataAll = document.getElementById("cartDataAll");

    cartDataAll.innerHTML = '';

    let products = [];
    products = JSON.parse(localStorage.getItem('products'));

    let total = localStorage.getItem('total');

    if (!products) {
        document.getElementById("allTotal").innerHTML = 0;
        cartDataAll.innerHTML = "<tr>Giỏ hàng trống!</tr>";
        document.getElementById("btnCheckOut").disabled = true;

    }
    else
        if (products) {
            for (var i = 0; i < products.length; i++) {
                var html = document.createElement("tr");

                var formatter = new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                });

                html.innerHTML =
                    "<td class='p-4'>" +
                    "<div class='row media align-items-center'>" +
                    "<div class='col-md-4'>" +
                    "<img width='100' height='100' src='" + products[i].image.replace('~', '') + "' class='d-block ui-bordered mr-4'>" +
                    "</div>" +
                    "<div class='col-md-8 media-body'>" +
                    "<a style='font-size: 13px;' href = '/products/getProductDetail?url=" + products[i].url + "' class='d-block text-decoration-none'>" + products[i].name + "</a>" +
                    "</div>" +
                    "</td>" +

                    "<td class='text-right font-weight-semibold align-middle p-4'>" + formatter.format(products[i].price) + "</td>" +

                    "<td class='align-middle p-4'>" +
                    "<div class='d-flex flex-row'>" +
                    "<button class='btn-qty' onclick='updateCartDes(" + products[i].id + ")' ><i class='fas fa-minus'></i></button>" +
                    "<input type='number' name='" + products[i].id + "' value='" + products[i].qty + "' id='" + products[i].id + "' class='form-control' />" +
                    "<button class='btn-qty' onclick='updateCartAsc(" + products[i].id + ")'><i class='fas fa-plus'></i></button>" +
                    "</td>" +

                    "<td class='text-right font-weight-semibold align-middle p-4'>" + formatter.format(products[i].price * products[i].qty) + "</td>" +

                    "<td class='text-center align-middle px-0'>" +
                   

                    "<button onclick='deleteCart(" + products[i].id + ")' class='shop-tooltip close float-none text-danger btn-ud'>" +
                    "<i class='fas fa-trash-alt'></i>" +
                    "</button>" +
                    "</td>";

                cartDataAll.appendChild(html);
                document.getElementById("allTotal").innerHTML = formatter.format(total);
                document.getElementById("btnCheckOut").disabled = false;
            }
        }
}