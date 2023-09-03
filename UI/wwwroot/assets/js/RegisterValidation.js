    let Name = document.getElementById("Name");
    let NameLabel = document.getElementById("NameLabel");

    let LastName = document.getElementById("LastName");
    let LastNameLabel = document.getElementById("LastNameLabel");

    let Email = document.getElementById("email");
    let EmailLabel = document.getElementById("emailLabel");

    let Phone = document.getElementById("phone");
    let PhoneLabel = document.getElementById("phoneLabel");

    let password = document.getElementById("password");
    let passwordLabel = document.getElementById("passwordLabel");

    let ConfirmPassword = document.getElementById("ConfirmPassword");
    let ConfirmPasswordLabel = document.getElementById("ConfirmPasswordLabel");

    let IsAgree = document.getElementById("IsAgree");

    let ReturnUrl = document.getElementById("ReturnUrl");

    let Errors = "";
    let IsValid = true;
    let validation = 
    {
        NameIsValid : true,
        LastNameIsValis : true,
        EmailIsValid : true,
        PhoneIsValid : true,
        passwordIsValid : true,
        ConfirmPasswordIsValid: true,
        IsAgreeIsValid : false
    }

    IsAgree.addEventListener("click", () => {
        if(validation.IsAgreeIsValid == true) validation.IsAgreeIsValid = false;
        else validation.IsAgreeIsValid = true
    })
    Name.addEventListener("change", NameChanged)
    function NameChanged(){
        if (Name.value.length >= 2) {

            NameLabel.style.color = "green"
            IsValid = true;
            validation.NameIsValid = true;
        }
        else {
            NameLabel.style.color = "red"
            IsValid = false;
            validation.NameIsValid = false;
        }
    }
    LastName.addEventListener("change", LastNameChanged)

    function LastNameChanged() {
        if (LastName.value.length >= 2) {

            LastNameLabel.style.color = "green"
            validation.LastNameIsValis = true;
            IsValid = true;

        }
        else {
            LastNameLabel.style.color = "red"
            IsValid = false;
            validation.LastNameIsValis = false


        }
    }
    Phone.addEventListener("change", PhoneChanged)
    function PhoneChanged() {
        var regex = new RegExp("^(\\+98|0)?9\\d{9}$");
        if (regex.test(Phone.value)) {
            PhoneLabel.style.color = "green"
            IsValid = true;
            validation.PhoneIsValid = true

        }
        else {
            PhoneLabel.style.color = "red"
            IsValid = false;
            validation.PhoneIsValid = false

        }
    }

    Email.addEventListener("change", EmailChanged);
    function EmailChanged() {
        var regex = new RegExp(/^[A-Za-z0-9_!#$%&'*+\/=?`{|}~^.-]+@[A-Za-z0-9.-]+$/, "gm")
        if (regex.test(Email.value)) {
            EmailLabel.style.color = "green"
            IsValid = true;
            validation.EmailIsValid = true
        }
        else {
            EmailLabel.style.color = "red"
            IsValid = false;
            validation.EmailIsValid = false

        }
    }
    password.addEventListener("change", PasswordChanged);
    function PasswordChanged() {
        if (password.value.length >= 8) {
            passwordLabel.style.color = "green";
            IsValid = true;
            validation.passwordIsValid = true
        }
        else {
            passwordLabel.style.color = "red";
            IsValid = false;
            validation.passwordIsValid = false


        }
    }
    ConfirmPassword.addEventListener("change", ConfirmChanged)
    function ConfirmChanged(){
        if (ConfirmPassword.value.length >= 8 && password.value == ConfirmPassword.value) {
            ConfirmPasswordLabel.style.color = "green";
            IsValid = true;
            validation.ConfirmPasswordIsValid = true

        }
        else {
            ConfirmPasswordLabel.style.color = "red";
            IsValid = false;
            validation.ConfirmPasswordIsValid = false

        }
    }
    function Register() {
        ConfirmChanged();
        PasswordChanged();
        EmailChanged();
        PhoneChanged();
        LastNameChanged();
        NameChanged();
        if(IsValid && validation.IsAgreeIsValid)
        {
            const xhr = new XMLHttpRequest();
            // Initialize the request
            xhr.open("POST", 'Register');
            // Set content type
            xhr.setRequestHeader('Content-type', 'application/json; charset=UTF-8');
            // Send the request with data to post
            xhr.send(
                JSON.stringify({
                     firstName : Name.value,
                     lastName : LastName.value,
                     email : Email.value,
                     password : password.value,
                     confirmPassword : ConfirmPassword.value,
                     phoneNumber : Phone.value,
                     returnUrl : ReturnUrl.value
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
                        if (response.data == true) {
                            setTimeout(() => {
                                document.location.href = "/"

                            },3000)
                        }
                    }
                    else {
                        location.replace(`VerifyPhoneNumber?UserName=${Email.value}&PhoneNumber=${Phone.value}&ReturnUrl=${ReturnUrl.value}`)
                    }
                }
            }
        }
        else
        {
            Errors = ""
            if(validation.NameIsValid == false)     Errors += "\nنام کمتر از دو کاراکتر است";
            if(validation.LastNameIsValis == false)     Errors += "\nنام خانوادگی کمتر از دو کاراکتر است"
            if(validation.EmailIsValid == false)     Errors += "\nایمیل وارد شده صحیح نیست"
            if(validation.PhoneIsValid == false)     Errors += "\nشماره تلفن وارد شده صحیح نیست"
            if(validation.passwordIsValid == false)     Errors += "\n رمز عبور باید حداقل 8 کاراکتر باشد"
            if(validation.ConfirmPasswordIsValid == false)     Errors += "\nرمز عبور و تکرار آن برابر نیست یا کمتر از 8 کاراکتر است" 
            if(validation.IsAgreeIsValid == false) Errors += "\nلطفا حریم خصوصی و شرایط و قوانین را تایید کنید"
            Swal.fire({
                icon: 'error',
                title: 'خطا',
                text: Errors
            })
        }
    }