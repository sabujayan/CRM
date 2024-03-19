$(function () {

    var l = abp.localization.getResource('Indo');

    $("#txtEmployeeName").change(function () {
        var name = $("#txtEmployeeName").val();
        indo.employees.employee.duplicateNameCheckforAdd(name).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#txtEmployeeName").val('');
                $("#txtEmployeeName").focus();
                return false;
            }
        });
    });

    $("#txtEmployeeNumber").change(function () {
        var employeeNumber = $("#txtEmployeeNumber").val();
        indo.employees.employee.duplicateIdCheckforAdd(employeeNumber).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, employeeNumber + " Already Exists");
                $("#txtEmployeeNumber").val('');
                $("#txtEmployeeNumber").focus();
                return false;
            }
        });
    });

    var pipe = [];
    var pipe1 = [];
    var pipe2 = [];
    var itemNames = [];
    var itemNames1 = [];
    var itemNames2 = [];

    $("#ddlClientProject .custom-selects").click(function () {
        var projectcount = $("#hidprojectcount").val();
        if (projectcount == 0) {
            $("#spRecord").show();
        }
        else {
            $(this).children(".multi-input").focus();
            $(this).siblings(".custom-sel-list").toggle();
            $("#spRecord").hide();
        }
    });

    $("#ddlClientProject .custom-sel-list").hide();

    $("#ddlClientProject .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlClientProject").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidprojectnameist").val();
        $("#hidprojectnameist").val(pipe);
        var name = $(this).text();
        itemNames.push(name);
        console.log("itme length on click" + itemNames.length);
        $("#ddlClientProject .multi-input").val("");
    });

    $("#ddlClientProject").on('click', function () {
        $("#clientProject").show();
    });

    $("#ddlClientProject").on('focusin', function () {
        $("#clientProject").show();
    });

    $(function () {
        $(document).on("click", "#ddlClientProject .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlClientProject .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe.indexOf(value);
            pipe.splice(idIndex, 1);
            $("#hidprojectnameist").val();
            $("#hidprojectnameist").val(pipe);
            var name = $(this).parents("#ddlClientProject").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames.indexOf(selectedValue);
            itemNames.splice(index, 1);
            console.log("itme length on remove" + itemNames.length);
            if (itemNames.length == 0) {
                $("#ddlClientProject .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
        });
    });

    $('#ddlClientProject .multi-input').on('click', function () {
        $("#ddlClientProject .custom-sel-list li").each(function () {
            if (itemNames.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $("#ddlClientProject .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;

        $("#ddlClientProject .custom-sel-list li").each(function () {
            if (itemNames.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlClientProject .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !itemNames.includes($(this).text())) {
                $(this).show();
                if (name.includes(inputName)) {
                    matchingItemsExist = true;
                }
            } else {
                $(this).hide();
            }
        });

        if (!matchingItemsExist) {
            $("#spRecord").show();
        } else {
            $("#spRecord").hide();
        }
    });

    //
    $("#ddlClientSkill .custom-selects").click(function () {
        var skillCount = $("#hidskillcount").val();
        if (skillCount == 0) {
            $("#srpRecord").show();
        }
        else {
            $(this).children(".multi-input").focus();
            $(this).siblings(".custom-sel-list").toggle();
            $("#spRecord").hide();
        }
    });

    $("#ddlClientSkill .custom-sel-list").hide();

    $("#ddlClientSkill .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe1.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlClientSkill").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidskilllist").val();
        $("#hidskilllist").val(pipe1);
        var name = $(this).text();
        itemNames1.push(name);
        $("#ddlClientSkill .multi-input").val("");
    });

    $("#ddlClientSkill").on('click', function () {
        $("#clientSkill").show();
    });

    $("#ddlClientSkill").on('focusin', function () {
        $("#clientSkill").show();
    });

    $(function () {
        $(document).on("click", "#ddlClientSkill .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlClientSkill .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe1.indexOf(value);
            pipe1.splice(idIndex, 1);
            $("#hidskilllist").val();
            $("#hidskilllist").val(pipe1);
            var name = $(this).parents("#ddlClientSkill").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames1.indexOf(selectedValue);
            itemNames1.splice(index, 1);
            if (itemNames1.length == 0) {
                $("#ddlClientSkill .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
            $("#ddlClientSkill .multi-input").val("");
        });
    });

    $('#ddlClientSkill .multi-input').on('click', function () {
        $("#ddlClientSkill .custom-sel-list li").each(function () {
            if (itemNames1.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $("#ddlClientSkill .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;

        $("#ddlClientSkill .custom-sel-list li").each(function () {
            if (itemNames1.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlClientSkill .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !itemNames1.includes($(this).text())) {
                $(this).show();
                if (name.includes(inputName)) {
                    matchingItemsExist = true;
                }
            } else {
                $(this).hide();
            }
        });

        if (!matchingItemsExist) {
            $("#srpRecord").show();
        } else {
            $("#srpRecord").hide();
        }
    });

    //
    $("#ddlClient .custom-selects").click(function () {
        var projectcount = $("#hidclientcount").val();
        if (projectcount == 0) {
            $("#scpRecord").show();
        }
        else {
            $(this).children(".multi-input").focus();
            $(this).siblings(".custom-sel-list").toggle();
            $("#scpRecord").hide();
        }
    });


    $("#ddlClient .custom-sel-list").hide();

    $("#ddlClient .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe2.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlClient").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidclientnamelist").val();
        $("#hidclientnamelist").val(pipe2);
        var name = $(this).text();
        itemNames2.push(name);
        $("#ddlClient .multi-input").val("");
    });

    $("#ddlClient").on('click', function () {
        $("#clientNameList").show();
    });

    $("#ddlClient").on('focusin', function () {
        $("#clientNameList").show();
    });

    $(function () {
        $(document).on("click", "#ddlClient .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlClient .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe2.indexOf(value);
            pipe2.splice(idIndex, 1);
            $("#hidclientnamelist").val();
            $("#hidclientnamelist").val(pipe2);
            var name = $(this).parents("#ddlClient").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames2.indexOf(selectedValue);
            itemNames2.splice(index, 1);
            console.log("itme length on remove" + itemNames2.length);
            if (itemNames2.length == 0) {
                $("#ddlClient .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
        });
    });

    $('#ddlClient .multi-input').on('click', function () {
        $("#ddlClient .custom-sel-list li").each(function () {
            if (itemNames2.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $("#ddlClient .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;

        $("#ddlClient .custom-sel-list li").each(function () {
            if (itemNames2.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlClient .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !itemNames2.includes($(this).text())) {
                $(this).show();
                if (name.includes(inputName)) {
                    matchingItemsExist = true;
                }
            } else {
                $(this).hide();
            }
        });

        if (!matchingItemsExist) {
            $("#scpRecord").show();
        } else {
            $("#scpRecord").hide();
        }
    });

    //
    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
        }
    });

    //check validation for Add page

    $('#AddEmployeeForm').submit(function (e) {
        debugger;
        var employee_name = $('#txtEmployeeName').val();
        var emp_no = $('#txtEmployeeNumber').val();
        var street = $('#txtEmployeeStreet').val();
        var city = $('#txtEmployeeCity').val();
        var state = $('#txtEmployeeState').val();
        var zip = $('#txtEmployeeZipCode').val();
        var phone_no = $('#txtEmployeePhone').val();
        var email_id = $('#txtEmployeeEmail').val();
        var position = $('#txtEmployeePosition').val();
        $(".error").remove();

        if (employee_name.length < 1) {
            $('#txtEmployeeName').after('<span class="error">The Employee Name field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validEmployeeName = regx.test(employee_name);
            if (!validEmployeeName) {
                $('#txtEmployeeName').after('<span class="error">The Employee Name field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (emp_no.length < 1) {
            $('#txtEmployeeNumber').after('<span class="error">The Employee Number field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[0-9]*$/;
            var validEmplNo = regx.test(emp_no);
            if (!validEmplNo) {
                $('#txtEmployeeNumber').after('<span class="error">The Employee Number field should only contain number.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (street.length < 1) {
            $('#txtEmployeeStreet').after('<span class="error">The Street field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z0-9-,. ]*$/;
            var validStreet = regx.test(street);
            if (!validStreet) {
                $('#txtEmployeeStreet').after('<span class="error">The Street field should only contain character and space.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (city.length < 1) {
            $('#txtEmployeeCity').after('<span class="error">The City field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validCity = regx.test(city);
            if (!validCity) {
                $('#txtEmployeeCity').after('<span class="error">The City field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (state.length < 1) {
            $('#txtEmployeeState').after('<span class="error">The State field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validState = regx.test(state);
            if (!validState) {
                $('#txtEmployeeState').after('<span class="error">The State field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (zip.length < 1) {
            $('#txtEmployeeZipCode').after('<span class="error">The Zip field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[0-9]*$/;
            var validZip = regx.test(zip);
            if (!validZip) {
                $('#txtEmployeeZipCode').after('<span class="error">The Address field should only contain characters and numbers.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (phone_no.length < 1) {
            $('#txtEmployeePhone').after('<span class="error">The Phone No field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[0-9]{10}$/;
            var validPhone = regx.test(phone_no);
            if (!validPhone) {
                $('#txtEmployeePhone').after('<span class="error">Please provide a valid Phone No.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (email_id.length < 1) {
            $('#txtEmployeeEmail').after('<span class="error">The Email field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
            var validEmail = regx.test(email_id);
            if (!validEmail) {
                $('#txtEmployeeEmail').after('<span class="error">Please provide a valid Email Id.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (position.length < 1) {
            $('#txtEmployeePosition').after('<span class="error">The Position field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validPosition = regx.test(position);
            if (!validPosition) {
                $('#txtEmployeePosition').after('<span class="error">The Position field should only contain character.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (pipe1.length < 1) {
            $('#ddlClientSkill').after('<span class="error">The Client Skill field is require.</span>');
            e.preventDefault();
            return false;
        }
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Employee/Index";
        return true;
    });

});