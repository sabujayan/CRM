$(function () {
    var l = abp.localization.getResource('Indo');

    $("#txtClientName").change(function () {
        var name = $("#txtClientName").val();
        indo.clientes.clients.duplicateCheckforAdd(name).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#txtClientName").val('');
                $("#txtClientName").focus();
                return false;
            }
        });
    });

    // Add Client project Mapping for Save
    var pipe = [];
    var itemNames = [];


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

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
            $(".multi-input").val("");
            $("#spRecord").hide();
        }
    });

    $("#ddlClientProject .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlClientProject").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidprojectnameist").val();
        $("#hidprojectnameist").val(pipe);
        var name = $(this).text();
        itemNames.push(name);
        $("#ddlClientProject .multi-input").val("");
    });

    $("#ddlClientProject").on('click', function () {
        $("#projectList").show();
    });

    $("#ddlClientProject").on('focusin', function () {
        $("#projectList").show();
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

    $('#AddClientForm').submit(function (e) {
        debugger;
        var client_name = $('#txtClientName').val();
        var email_id = $('#txtEmail').val();
        var phone_no = $('#txtPhoneNumber').val();
        var address = $('#txtAddress').val();
        var country = $('#txtCountry').val();
        var state = $('#txtState').val();
        var city = $('#txtCity').val();
        var zip = $('#txtZip').val();
        $(".error").remove();
        if (client_name.length < 1) {
            $('#txtClientName').after('<span class="error">The Name field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validClientName = regx.test(client_name);
            if (!validClientName) {
                $('#txtClientName').after('<span class="error">The Name field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (email_id.length < 1) {
            $('#txtEmail').after('<span class="error">The Email field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
            var validEmail = regx.test(email_id);
            if (!validEmail) {
                $('#txtEmail').after('<span class="error">Please provide a valid Email Id.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (phone_no.length < 1) {
            $('#txtPhoneNumber').after('<span class="error">The Phone No field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[0-9]{10}$/;
            var validPhone = regx.test(phone_no);
            if (!validPhone) {
                $('#txtPhoneNumber').after('<span class="error">Please provide a valid Phone No.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (address.length < 1) {
            $('#txtAddress').after('<span class="error">The Address field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z0-9,. ]*$/;
            var validAddress = regx.test(address);
            if (!validAddress) {
                $('#txtAddress').after('<span class="error">The Address field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (country.length < 1) {
            $('#txtCountry').after('<span class="error">The Country field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validCountry = regx.test(country);
            if (!validCountry) {
                $('#txtCountry').after('<span class="error">The Country field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (state.length < 1) {
            $('#txtState').after('<span class="error">The State field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validState = regx.test(state);
            if (!validState) {
                $('#txtState').after('<span class="error">The State field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (city.length < 1) {
            $('#txtCity').after('<span class="error">The City field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z ]*$/;
            var validCity = regx.test(city);
            if (!validCity) {
                $('#txtCity').after('<span class="error">The City field should only contain letters and spaces.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (zip.length < 1) {
            $('#txtZip').after('<span class="error">The Zip field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[0-9]*$/;
            var validZip = regx.test(zip);
            if (!validZip) {
                $('#txtZip').after('<span class="error">The Address field should only contain characters and numbers.</span>');
                e.preventDefault();
                return false;
            }
        }
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Clients/Index";
        return true;
    });
  
});



