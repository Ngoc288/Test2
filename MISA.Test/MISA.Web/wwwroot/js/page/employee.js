$(document).ready(function () {
    new EmployeeJS();
    dialogDetail = $("#dialog").dialog({
        autoOpen: false,
        fluid: true,
        //height: 400,
        //width: '700px',
        minWidth: 700,
        resizable: true,
        position: ({ my: "center", at: "center", of: window }),
        modal: true,
    });

    $("#btnAdd").button().on("click", function () {
        dialog.dialog("open");
    });

})



/**
 * Class quản lý các sự kiện cho trang Employee
 * created by ngochtb(13/11/2020)*/

class EmployeeJS extends BaseJS{
    constructor() {
        super();
    }

    setDataURL() {
        this.getDataURL = "http://api.manhnv.net/api/employees";
    }

}