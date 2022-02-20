function validateName() {
    let name = document.getElementById("FullName");

    if (!name.value) {
        document.getElementById("error-name").innerHTML = "Họ và tên là bắt buộc!";
    } else
        if (name.value.length < 5 || name.value.length > 255) {
            document.getElementById("error-name").innerHTML = "Họ và tên phải từ 5 - 255!";
        } else {
            document.getElementById("error-name").innerHTML = "";
        }
}

function validateEmail() {
    let email = document.getElementById("Email");
    let mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

    if (!email.value) {
        document.getElementById("error-email").innerHTML = "Email là bắt buộc!";
    } else
        if (!email.value.match(mailformat)) {
            document.getElementById("error-email").innerHTML = "Email không đúng định dạng!";
        } else {
            document.getElementById("error-email").innerHTML = "";
        }
}

function validatePassword() {
    let password = document.getElementById("Password");

    if (!password.value) {
        document.getElementById("error-password").innerHTML = "Mật khẩu là bắt buộc!";
    } else
        if (password.value.length < 6 || password.value.length > 12) {
            document.getElementById("error-password").innerHTML = "Mật khẩu phải từ 6 - 12!";
        } else {
            document.getElementById("error-password").innerHTML = "";
        }
}

function validateConfPassword() {
    let password = document.getElementById("Password");
    let confPassword = document.getElementById("confPassword");

    if (!password.value) {
        document.getElementById("error-confPassword").innerHTML = "Xác nhận mật khẩu là bắt buộc!";
    } else
        if (password.value != confPassword.value) {
            document.getElementById("error-confPassword").innerHTML = "Mật khẩu xác nhận không trùng nhau!";
        } else {
            document.getElementById("error-confPassword").innerHTML = "";
        }
}


