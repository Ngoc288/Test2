$(document).ready(function () {
    new CustomerJS();
    $(function () {
        dialogDetail = $("#dialog-form").dialog({
            autoOpen: false,
            width: 775,
            resizable: true,
            position: ({ my: "center", at: "center", of: window }),
            fluid: true,
            modal: true,
        
        });
        dialogConfirm = $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: "auto",
            width: 400,
            modal: true,
        });
       
    })
})

/**
 * Class quản lý các sự kiện cho trang Customer
 * created by ngochtb(13/11/2020)*/

    class CustomerJS extends BaseJS {
        constructor() {
            super();
        }
        setApiRouter() {
            this.apiRouter = "/api/customers";
        }
    }