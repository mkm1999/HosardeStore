let Emali_Phone = document.getElementById("email-phone");
let Emali_PhoneLabel = document.getElementById("email-phoneLabel");

let Password = document.getElementById("Password");
let PasswordLabel = document.getElementById("PasswordLabel");

let remember = document.getElementById("remember");

let ReturnUrl = document.getElementById("ReturnUrl");


let Errors = "";
let IsValid = true;

let validation =
{
    Emali_PhoneIsValid: true,
    PasswordIsValid: true,
    IsPersistentIsValid: false
}

remember.addEventListener("click", () => {
    if (validation.IsPersistentIsValid == true) validation.IsPersistentIsValid = false;
    else validation.IsPersistentIsValid = true
})

Emali_Phone.addEventListener("change", EmailChanged);
function EmailChanged() {
    if (Emali_Phone.value.length >= 2) {

        Emali_PhoneLabel.style.color = "green"
        IsValid = true;
        validation.Emali_PhoneIsValid = true;
    }
    else {
        Emali_PhoneLabel.style.color = "red"
        IsValid = false;
        validation.Emali_PhoneIsValid = false;
    }
}

Password.addEventListener("change", PasswordChanged);
function PasswordChanged() {
    if (Password.value.length >= 8) {
        PasswordLabel.style.color = "green";
        IsValid = true;
        validation.PasswordIsValid = true
    }
    else {
        PasswordLabel.style.color = "red";
        IsValid = false;
        validation.PasswordIsValid = false


    }
}


function Login() {
    PasswordChanged();
    EmailChanged();
    if (IsValid)
    {
        const xhr = new XMLHttpRequest();
        // Initialize the request
        xhr.open("POST", 'Login');
        // Set content type
        xhr.setRequestHeader('Content-type', 'application/json; charset=UTF-8');
        // Send the request with data to post
        xhr.send(
            JSON.stringify({
                Email: Emali_Phone.value,
                Password: Password.value,
                IsPersistent: validation.IsPersistentIsValid,
                ReturnUrl : ReturnUrl.value
            })
        );
        xhr.onload = function (e) {
            // Check if the request was a success
            if (this.readyState === XMLHttpRequest.DONE) {
                // Get and convert the responseText into JSON
                let response = JSON.parse(xhr.responseText)
                if (response.isSuccess == false) {
                    Swal.fire({
                        icon: 'error',
                        title: 'خطا',
                        text: response.message
                    })
                }
                else {
                    //swal
                    document.location = response.data;
                }
            }
        }
    }
    else
    {
        Errors = ""
        if (validation.Emali_PhoneIsValid == false) Errors += "\nفیلد ایمیل کمتر از دو کاراکتر است";
        if (validation.PasswordIsValid == false) Errors += "\nکلمه عبور کمتر از هشت کاراکتر است";
        Swal.fire({
            icon: 'error',
            title: 'خطا',
            text: Errors
        })
    }
}