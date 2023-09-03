function DeleteUser(UserId, UserName){
    swal({
        title: "آیا اطمینان دارید؟",
        text: `کاربر با نام ${UserName} حذف خواهد شد!`,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
    .then((willDelete) => {
        if (willDelete) {
            document.location = `/Admin/UserManagement/DeleteUser?UserId=${UserId}`
        }
    });
}

function AddMember() {
    let swcontent = document.createElement("div");
    swcontent.innerHTML = `<input type="text" id="FirstName" class="swal2-input m-2" placeholder="FirstName"><br/>
            <input type="text" id="LastName" class="swal2-input m-2" placeholder="LastName"><br/>
            <input type="email" id="email" class="swal2-input m-2" placeholder="Email"><br/>
            <input type="Number" id="Number" class="swal2-input m-2" placeholder="Number"><br/>
            <input type="password" id="password" class="swal2-input m-2" placeholder="Password">`;
    swal({
        title: 'AddMember',
        buttons: {
            cancel: "Cancel",
            catch: {
                text: "Submit",
                value: "true",
            },
        },
        content : swcontent,
        focusConfirm: false,
    }).then((result) => {
        if (result) {
            const FirstName = document.querySelector('#FirstName').value
            const password = document.querySelector('#password').value
            const LastName = document.querySelector('#LastName').value
            const email = document.querySelector('#email').value
            const PNumber = document.querySelector('#Number').value
            if (!FirstName || !password || !LastName || !email || !PNumber) {
                swal({
                    icon: 'error',
                    title: 'خطا!',
                    text: 'لطفا مقادیر را کنترل کنید',
                })
            }
            else document.location = `/Admin/UserManagement/AddMember?FirstName=${FirstName}&LastName=${LastName}&Email=${email}&PhoneNumber=${PNumber}&Password=${password}`;
        }
        
    })

}

function EditUser(UserId,Email,PNumber,FirstName,LastName) {
    let swcontent = document.createElement("div");
    swcontent.innerHTML = `<input type="text" value="${FirstName}" id="FirstNameEdit" class="swal2-input m-2" placeholder="FirstName"><br/>
            <input type="text" value="${LastName}" id="LastNameEdit" class="swal2-input m-2" placeholder="LastName"><br/>
            <input type="email" value="${Email}" id="emailEdit" class="swal2-input m-2" placeholder="Email"><br/>
            <input type="Number" value="${PNumber}" id="NumberEdit" class="swal2-input m-2" placeholder="Number"><br/>`;
    swal({
        title: 'EditUser',
        buttons: {
            cancel: "Cancel",
            catch: {
                text: "Submit",
                value: "true",
            },
        },
        content: swcontent,
        focusConfirm: false,
    }).then((result) => {
        if (result) {
            const FirstNameInput = document.querySelector('#FirstNameEdit').value
            const LastNameInput = document.querySelector('#LastNameEdit').value
            const emailInput = document.querySelector('#emailEdit').value
            const PNumberInput = document.querySelector('#NumberEdit').value
            if (!FirstNameInput || !LastNameInput || !emailInput || !PNumberInput) {
                swal({
                    icon: 'error',
                    title: 'خطا!',
                    text: 'لطفا مقادیر را کنترل کنید',
                })
            }
            else document.location = `/Admin/UserManagement/EditUser?FirstName=${FirstNameInput}&LastName=${LastNameInput}&Email=${emailInput}&PhoneNumber=${PNumberInput}&Id=${UserId}`;
        }

    })
}

function EditUserPassword(UserId) {
    let swcontent = document.createElement("div");
    swcontent.innerHTML = `<input type="password" id="passwordEdit" class="swal2-input m-2" placeholder="password"><br/>`;
    swal({
        title: 'EditUserPassword',
        buttons: {
            cancel: "Cancel",
            catch: {
                text: "Submit",
                value: "true",
            },
        },
        content: swcontent,
        focusConfirm: false,
    }).then((result) => {
        if (result) {
            const PasswordInput = document.querySelector('#passwordEdit').value
            if (!PasswordInput) {
                swal({
                    icon: 'error',
                    title: 'خطا!',
                    text: 'لطفا مقادیر را کنترل کنید',
                })
            }
            else document.location = `/Admin/UserManagement/EditUserPassword?newPassword=${PasswordInput}&UserId=${UserId}`;
        }

    })
}

function Search() {
    let SearchInput = document.getElementById("search").value;
    document.location = `/Admin/UserManagement/Index?SearchKey=${SearchInput}`;
}