//Load Details Part
function initial_work() {
    $("#tbl_details tbody tr").remove();

    var rows = '<tr class="input_fields_box">'  
    + '<td><input class="form-control" type="text" id="txtFileName1" name="FileName"/></td>'
    + '<td><input type="file" name="FileAttachment" id="FileAttachment1"/></td>'
    + '<td><span id="btn_temp_add1" class="glyphicon glyphicon-plus add_field_button" style="cursor:pointer"></span></td>'
    + '<td><span class="glyphicon glyphicon-trash remove_field" style="cursor:pointer"></span></td>'
    + '</tr>';

    $('#tbl_details tbody').append(rows);
}

//Add Multiple Element
jQuery_1_7_2(window).load(function () {
    var x = 1; //initilal text box count
    var max_elements = 50; //maximum input elements allowed      
  
    jQuery_1_7_2(".add_field_button").live('click', function () {

        var x = document.getElementById('hdn_count_element').value;
        if (x <= max_elements) {
            var $tr = $(this).closest('.input_fields_box');
            var $clone = $tr.clone();
            $clone.find(':text').val('');
            $clone.find('textarea').val('');
            $clone.find('.remove_field').attr("id", ''); //Remove Button ID Set Empty For New Clone Data Update.
            $clone.find('select option:first-child').attr("selected", "selected");
            $clone.find('*').each(function (index, element) {
                var ele_id = element.id;
                $('#' + ele_id).attr("id", ele_id + x);
            });
            $tr.after($clone);
            
            $clone.find("input.show_datepicker")
            .removeClass('hasDatepicker')
            .removeData('datepicker')
            .unbind()
            .datepicker({
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                yearRange: "-100:+0",
                changeYear: true,
                showButtonPanel: false,
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });

            x++;
            document.getElementById('hdn_count_element').value = x;
        }
    });
});

//Remove Element
var total_remove_element_id = "";
jQuery_1_7_2(window).load(function () {
    jQuery_1_7_2(".remove_field").live("click", function () {
        var x = document.getElementById('hdn_count_element').value;
        if (x > 1) {
            $(this).parents(".input_fields_box").remove();
            x--;
            document.getElementById('hdn_count_element').value = x;
            
            var remove_single_id = $(this).attr("id");
            total_remove_element_id += remove_single_id + "#";
            $('#hdn_remove_all_id').val(total_remove_element_id);
        }
    });
});

//Get Form Data
var DetailsTable =
{
    getData: function (table) {
        var data = [];
        table.find('tr').not(':first').each(function (rowIndex, r) {
            var cols = [];
            $(this).find('td').each(function (colIndex, c) {
                if ($(this).children(':text,textarea,select,input[type="hidden"]').length > 0)
                    cols.push($(this).children('input,textarea,select,input[type="hidden"]').val().trim());
                    //if dropdown text is needed then uncomment it and remove SELECT from above IF condition.
                else if ($(this).children('select').length > 0)
                    cols.push($(this).find('option:selected').text());
                else if ($(this).children(':checkbox').length > 0)
                    cols.push($(this).children(':checkbox').is(':checked') ? 1 : 0);
                else {
                    //cols.push($(this).text().trim());
                    cols.push($(this).find('.remove_field').attr('id'));
                }
            });

            data.push(cols);
        });
        return data;
    }
}

//Save Master Data
function save() {
    if ($('#txtNo').val() != "") {
        $.ajax({
            type: "POST",
            url: "default.aspx/SaveData",
            data: "{txtNo: '" + $('#txtNo').val() + "',txtDate: '" + $('#txtDate').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var uploadedFiles = $('#fileupload')[0].files;
                if (uploadedFiles.length > 0) {
                    var formData = new FormData();
                    for (var i = 0; i < uploadedFiles.length; i++) {
                        formData.append(uploadedFiles[i].name, uploadedFiles[i]);
                    }
                }

                //For attachment
                save_details_data();
            }
        });
    }
}

//Save Details Data
function save_details_data() {
    $('#tbl_details').find('tr').not(':first').each(function (rowIndex, r) {
        $(this).find('td').each(function (colIndex, c) {           
            if ($(this).children(':text,textarea,select,input[type="hidden"]').length > 0)
            {
                var FileName = $(this).children('input,text,textarea,input[type="hidden"]').val();
                var FileID = $(this).children('input,text,textarea,input[type="hidden"]').attr('id');
            }

            if (!!FileID)
            {
                var suffix = FileID.match(/\d+/); 

                //Upload file
                var uploadedFiles = $('#FileAttachment' + suffix)[0].files;
                if (uploadedFiles.length > 0) {
                    var formData = new FormData();
                    for (var i = 0; i < uploadedFiles.length; i++) {
                        formData.append(uploadedFiles[i].name, uploadedFiles[i]);
                    }

                    formData.append("txtFileName", FileName);

                    //Upload Part
                    $.ajax({
                        url: '../File/FileUploadHandler.ashx',
                        method: 'post',
                        contentType: false,
                        processData: false,
                        data: formData,
                        success: function () {

                        },
                        error: function (err) {
                            $('#msg').html('<div class="alert alert-warning"><a class="close" style="text-decoration: none" data-hide-closest=".alert">×</a>Request Failed</div>');
                        }
                    });
                }
            }
        });
    });
}