function validateName() {
    let name = document.getElementById("Name");

    if (!name.value) {
        document.getElementById("error-name").innerHTML = "Tên quyền là bắt buộc!";
    } else
        if (name.value.length < 2 || name.value.length > 255) {
            document.getElementById("error-name").innerHTML = "Tên quyền phải từ 2 - 255!";
        } else {
            document.getElementById("error-name").innerHTML = "";
        }
}
