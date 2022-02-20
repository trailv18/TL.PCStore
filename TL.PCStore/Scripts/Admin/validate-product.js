////let validName = false;
////let validPrice = false;
////let validCategory = false;
////let validBrand = false;
////let validUrl = false;
////let validStock = false;
////let validFile = false;

////function enableButtonSubmit() {
////    let isDisabledButonEdit = validName && validUrl && validPrice && validCategory && validBrand && validStock && validFile;
////    if (!isDisabledButonEdit) {
////        document.getElementById("btnSubmit").disabled = true;
////    } else {
////        document.getElementById("btnSubmit").disabled = false;
////    }
////}

function validateName() {
    let name = document.getElementById("Name");

    if (!name.value) {
        document.getElementById("error-name").innerHTML = "Tên sản phẩm là bắt buộc!";
    } else
        if (name.value.length < 5 || name.value.length > 255) {
            document.getElementById("error-name").innerHTML = "Tên sản phẩm phải từ 5 - 255!";

        } else {
            document.getElementById("error-name").innerHTML = "";

        }
}

function validateUrl() {
    let url = document.getElementById("Url");

    if (!url.value) {
        document.getElementById("error-url").innerHTML = "Url sản phẩm là bắt buộc!";
    }
    else {
        document.getElementById("error-url").innerHTML = "";
    }
}

function validatePrice() {
    let price = document.getElementById("Price");
    if (!price.value) {
        document.getElementById("error-price").innerHTML = "Giá sản phẩm là bắt buộc!";
    } else if (price.value < 10000 || price.value > 100000000) {
        document.getElementById("error-price").innerHTML = "Giá sản phẩm phải từ 10.000 - 100.000.000!";
    }
    else {
        document.getElementById("error-price").innerHTML = "";
    }
}

function validateCategory() {
    let category = document.getElementById("CategoryId");
    if (!category.value) {
        document.getElementById("error-category").innerHTML = "Loại sản phẩm là bắt buộc!";
    }
    else {
        console.log(category.value);
        document.getElementById("error-category").innerHTML = "";
    }
}

function validateBrand() {
    let brand = document.getElementById("BrandId");
    if (!brand.value) {
        document.getElementById("error-brand").innerHTML = "Thương hiệu sản phẩm là bắt buộc!";
    }
    else {
        document.getElementById("error-brand").innerHTML = "";
    }
}

function validateStock() {
    let stock = document.getElementById("Stock");
    if (!stock.value) {
        document.getElementById("error-stock").innerHTML = "Số lượng sản phẩm là bắt buộc!";
    } else
        if (stock.value < 1 || stock.value > 1000) {
            document.getElementById("error-stock").innerHTML = "Số lượng sản phẩm phải từ 1 - 1000!";
        }
        else {
            document.getElementById("error-stock").innerHTML = "";
        }
}

function validateFile() {
    let file = document.getElementById("file").files[0];
    const validImageTypes = ['image/gif', 'image/jpeg', 'image/jpg', 'image/png'];
    let infoArea = document.getElementById('upload-label');

    if (file.length == 0) {
        document.getElementById("error-file").innerHTML = "Vui lòng chọn ảnh!";
    } else
        if (!validImageTypes.includes(file['type'])) {
            document.getElementById("error-file").innerHTML = "File không hợp lệ. Vui lòng chọn file *gif/*jpg/*png!";
            infoArea.textContent = file.name;
        }
        else {
            let reader = new FileReader();
            reader.onload = function () {
                let output = document.getElementById('img');
                output.src = reader.result;
                infoArea.textContent = file.name;
            };
            reader.readAsDataURL(event.target.files[0]);

            document.getElementById("error-file").innerHTML = "";
        }
}


function validateDescription() {
    let description = document.getElementById("summernoteDes");
    if (description.summernote('isEmpty')) {
        document.getElementById("error-description").innerHTML = "Chi tiết sản phẩm là bắt buộc!";
    }
}