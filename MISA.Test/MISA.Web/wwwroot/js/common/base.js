


/**
 * Class quản lý sự kiện, function
 * created by ngochtb(13/11/2020)*/
class BaseJS {
    constructor() {
        this.host = "http://api.manhnv.net";
        this.apiRouter = null;
        this.setApiRouter();
        this.initEvent();
        this.loadData();

    }

    setApiRouter() {

    }


    initEvent() {
        var me = this;
        //Sự kiện click khi nhấn vào thêm mới
        $('#btnAdd').click(me.btnAddOnClick.bind(me));

        //Load lại dữ liệu khi nhấn button nạp:
        $('#btnRef').click(function () {
            me.loadData();
        })

        //Ẩn form chi tiết khi nhấn huỷ
        $('#btnCancel').click(function () {
            dialogDetail.dialog('close');
        })

        //Thực hiện lưu dữ liệu khi nhấn button lưu trên form chi tiết
        $('#btnSave').click(me.btnSaveOnClick.bind(me));

        //Sửa thông tin chi tiết khi nhấn đúp chuột chọn 1 bản ghi
        $('table tbody').on('dblclick', 'tr', function (e) {
            me.editData(e);
            console.log(e, 'e edit')
        })
    
        //Xoá bản ghi khi nhấn chuột phải
        $('table tbody').on('contextmenu', 'tr', function (e) {
            dialogConfirm.dialog('open');
            var idButtonSaveDelete = $(e.currentTarget).data('recordId');
            $('#btnConfirm').attr('idItem', idButtonSaveDelete);
        })

        //Ẩn form confirm
        $('#btnCFCancel').click(function () {
            dialogConfirm.dialog('close');
        })

        //Thực hiện xoá bản ghi
        $('#btnConfirm').click(function (e) {
            me.deleteData(e);
        })

        //Kiểm tra trường để trống
        $('.input-required').blur(validateEmpty);

        //Kiểm tra định dạng email
        $('#txtEmail').blur(validateEmail);
    }
    /**
     * Load dữ liệu
     * createdby ngochtb(13/11/2020)
     * */
    loadData() {

        try {
            var me = this;
            $('table tbody').empty();
            //Lấy thông tin cột dữ liệu
            var columns = $('table thead tr th');
            $.ajax({
                url: me.host + me.apiRouter,
                method: "GET",
                async: true,
            }).done(function (res) {
                debugger;
                $.each(res, function (index, obj) {
                    var tr = $(`<tr></tr>`);
                    $(tr).data('recordId', obj.CustomerId);
                    //Lấy thông tin dữ liệu sẽ map tương ứng với các cột
                    $.each(columns, function (index, th) {
                        var td = $(`<td></td>`);
                        var fieldName = $(th).attr('fieldName');
                        var value = obj[fieldName];
                        var formatType = $(th).attr('formatType');
                        switch (formatType) {
                            case "ddmmyyyy":
                                td.addClass("text-align-center");
                                value = formatDate(value);
                                break;
                            case "Money":
                                td.addClass("text-align-right");
                                value = formatMoney(value);
                                break;
                            default:
                                break;
                        }
                        if (fieldName == 'Gender') {
                            if (value == 1) value = 'Nam';
                            else if (value == 0) value = 'Nữ';
                            else value = 'Khác';
                        }
                        td.append(value);
                        $(tr).append(td);
                    })
                    $('table tbody').append(tr);
                })
            }).fail(function (res) {
                debugger;
            })
        } catch (e) {
            console.log(e);
        }
    }
    /**
    * Hàm xử lý khi nhấn button thêm mới
    * createdby ngochtb(20/11/2020)
    * */
    btnAddOnClick() {
        try {
            var me = this;
            me.FormMode = 'Add';
            //Hiển thị dialog thông tin chi tiết
            dialogDetail.dialog('open');
            $('input').val(null);

            //Lấy dữ liệu cho các cbbox
            var select = $('select#cbxCustomerGroup');
            select.empty();
            //Lấy dữ liệu trong nhóm khách hàng
            $('.loading').show();
            $.ajax({
                url: me.host + "/api/customergroups",
                method: "GET",

            }).done(function (res) {
                if (res) {
                    $.each(res, function (index, obj) {
                        var option = $(`<option value="${obj.CustomerGroupId}">${obj.CustomerGroupName}</option>`);
                        select.append(option);

                    })
                }
                $('.loading').hide();
            }).fail(function (res) {
                $('.loading').hide();
            })
        } catch (e) {
            console.log(e);
        }

    }

    /**
     * Hàm xử lý khi nhấn button lưu
     * createdby ngochtb(20/11/2020)
     * */

    btnSaveOnClick() {
        try {
            var me = this;
            //validate dữ liệu
            var inputValidates = $('.input-required, #txtEmail');
            $.each(inputValidates, function (index, input) {
                $(input).trigger(`blur`);
            })
            var notInput = $('input[validate=false]');
            if (notInput && notInput.length > 0) {
                alert("Dữ liệu không hợp lệ vui lòng kiểm tra lại!");
                notInput[0].focus();
                return;
            }

            //thu thập thông tin dữ liệu được nhập

            //Lấy tất cả các control nhập liệu

            var inputs = $('input[fieldName], select[fieldName]');
            var entity = {};
            $.each(inputs, function (index, input) {
                var propertyName = $(this).attr('fieldName');
                var value = $(this).val();

                //check với trường hợp input radio thì chỉ lấy value của input có checked
                if ($(this).attr('type') == 'radio') {
                    if (this.checked) {
                        entity[propertyName] = value;
                    }
                } else {
                    entity[propertyName] = value;
                }
                //check với trường hợp combobox
                if (this.tagName == "SELECT") {
                    var propertyName = $(this).attr('fieldValue');
                    entity[propertyName] = value;

                }

            })

            var method = "POST";
            if (me.FormMode == 'Edit') {
                method = "PUT";
                entity.CustomerId = me.recordId;
            }


            //gọi service tương ứng thực hiện lưu trữ dữ liệu
            $.ajax({
                url: me.host + me.apiRouter,
                method: method,
                data: JSON.stringify(entity),
                contentType: 'application/json'
            }).done(function (res) {
                //sau khi lưu thành công:
                //+đưa ra thông báo, 
                //+ẩn form chi tiết, 
                dialogDetail.dialog('close');
                //+load lại dữ liệu
                me.loadData();

            }).fail(function (res) {
            })
        }
        catch (e) {
            console.log(e);
        }
    }

    /**
     * Hàm chỉnh sử dữ liệu khi nháy đúp chuột vào td
     * createdby ngochtb(20/11/2020)
     * */
    editData(item) {
        try {
            //load form
            dialogDetail.dialog('open');
            //load dữ liệu cho các combobox:
            var me = this;
             var selects = $('select[fieldName]');
             selects.empty();
             $.each(selects, function (index, select) {
                 //Lấy dữ liệu nhóm khách hàng
                 var api = $(select).attr('api');
 
                 var fieldName = $(select).attr('fieldName');
                 var fieldValue = $(select).attr('fieldValue');
                 $('loading').show();
                 $.ajax({
                     url: me.host + api,
                     method: "GET",
                     async: false
                 }).done(function (res) {
                     if (res) {
                         $.each(res, function (index, obj) {
                             var option = $(`<option value="${obj[fieldValue]}">${obj[fieldName]}</option>`);
                             $(select).append(option);
                         })
                     }
                     $('.loading').hide();
                 }).fail(function (res) {
                     $('.loading').hide();
                 })
             })

            //Lấy khoá chính của bản ghi
            me.FormMode = 'Edit';
            var recordId = $(item.currentTarget).data('recordId');
            console.log(item.currentTarget, 'edit')
            me.recordId = recordId;
            //Gọi service lấy thông tin chi tiết qua Id
            $.ajax({
                url: me.host + me.apiRouter + `/${recordId}`,
                method: "GET",
            }).done(function (res) {
                //Binding dữ liệu lên form chi tiết
                var inputs = $('input[fieldName], select[fieldName]');
                var entity = {};
                $.each(inputs, function (index, input) {
                    var propertyName = $(this).attr('fieldName');
                    var value = res[propertyName];

                    //Load dữ liệu combobox
                    if (this.tagName == "SELECT") {
                        var propValueName = $(this).attr('fieldValue');
                        value = res[propValueName];
                    }

                    //Load dữ liệu ngày
                    if (propertyName == 'DateOfBirth') {
                        value = formatDatePicker(value);
                    }

                    //Load dữ liệu input
                    if ($(this).attr('type') == "radio") {
                        var inputValue = this.value;
                        if (value == inputValue) {
                            this.checked = true;
                        } else {
                            this.checked = false;
                        }
                    }
                    
                    else {
                        $(this).val(value);
                    }
                })
            }).fail(function (res) {
         
            })
        } catch (e) {
            console.log(e);
        }
    }

    /*
     * Hàm xoá dữ liệu khi nhấp chuột phải
     * createdby ngochtb(21/11/2020)
     * */

    deleteData(item) {
        try {
            var me = this;

            var id = $(item.currentTarget).attr('iditem');

             $.ajax({
                url: me.host + me.apiRouter + `/${id}`,
                method: 'DELETE',
             }).done(function (res) {
                 me.loadData();
            }).fail(function (res) {
             
            })
             
        } catch (e) {
            console.log(e);
        }
        dialogConfirm.dialog('close');
    }
}