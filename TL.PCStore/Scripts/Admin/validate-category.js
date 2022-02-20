function validateName() {
    let name = document.getElementById("Name");

    if (!name.value) {
        document.getElementById("error-name").innerHTML = "Tên loại sản phẩm là bắt buộc!";
    }
    else
    if (name.value.length < 2 || name.value.length > 255) {
        document.getElementById("error-name").innerHTML = "Tên loại sản phẩm phải từ 2 - 255!";
    } else {
        document.getElementById("error-name").innerHTML = "";
    }
}

function validateUrl() {
    let url = document.getElementById("Url");

    if (!url.value) {
        document.getElementById("error-url").innerHTML = "Url loại sản phẩm là bắt buộc!";
    }
    else {
        document.getElementById("error-url").innerHTML = "";
    }
}
