function DeleteCategory(CategoryId,CategoryName) {
    swal({
        title: "آیا اطمینان دارید؟",
        text: `دسته بندی با نام ${CategoryName} حذف خواهد شد!`,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                document.location = `/Admin/Products/DeleteCategory?CategoryId=${CategoryId}`
            }
        });
}

function EditCategory(OldName,CategoryId) {
    let swcontent = document.createElement("div");
    swcontent.innerHTML = `<input type="text" id="EditCatInp" class="swal2-input m-2" value="${OldName}"><br/>`;
    swal({
        title: 'EditCategory',
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
            const NewNameInput = document.querySelector('#EditCatInp').value
            if (!NewNameInput) {
                swal({
                    icon: 'error',
                    title: 'خطا!',
                    text: 'لطفا مقادیر را کنترل کنید',
                })
            }
            else document.location = `/Admin/Products/EditCategory?CategoryId=${CategoryId}&NewName=${NewNameInput}`;
        }

    })
}

function AddNewCategory(ParentCategoryId) {
    let swcontent = document.createElement("div");
    swcontent.innerHTML = `<input type="text" id="NewCatNameInp" class="swal2-input m-2" placeholder="Name"><br/>`;
    swal({
        title: 'AddNewCategory',
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
            const NameInput = document.querySelector('#NewCatNameInp').value
            if (!NameInput) {
                swal({
                    icon: 'error',
                    title: 'خطا!',
                    text: 'لطفا مقادیر را کنترل کنید',
                })
            }
            else document.location = `/Admin/Products/AddCategory?ParentId=${ParentCategoryId}&Name=${NameInput}`;
        }

    })
}