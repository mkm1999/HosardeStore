let Inp1 = document.getElementById("Inp1");
let Inp2 = document.getElementById("Inp2");
let Inp3 = document.getElementById("Inp3");
let Inp4 = document.getElementById("Inp4");
let Inp5 = document.getElementById("Inp5");
let Inp6 = document.getElementById("Inp6");




let PhoneNumberInput = document.getElementById("PhoneNumber");
let UserIdInput = document.getElementById("UserId");
let ReturnUrlInput = document.getElementById("ReturnUrl");


function Verify() {
    let VerifyCodeInput = Inp1.value + Inp2.value + Inp3.value + Inp4.value + Inp5.value + Inp6.value;
    const xhr = new XMLHttpRequest();
    // Initialize the request
    xhr.open("POST", 'VerifyPhoneNumber');
    // Set content type
    xhr.setRequestHeader('Content-type', 'application/json; charset=UTF-8');
    // Send the request with data to post
    xhr.send(
        JSON.stringify({
            verifyCode: VerifyCodeInput,
            phoneNumber: PhoneNumberInput.value,
            userName: UserIdInput.value,
            returnUrl: ReturnUrlInput.value
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
                //swal shoma vared shodeid
                location.replace(`${response.data}`)
            }
        }
    }
}