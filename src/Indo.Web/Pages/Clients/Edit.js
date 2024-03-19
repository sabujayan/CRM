$(function () {
    var l = abp.localization.getResource('Indo');

    $("#btnEditsave").click(function () {
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Clients";
    });

    // Edit Client project Mapping for Update
    var custompipe = [];
    var EdititemNames = [];


    $("#ddlEditClientProject .custom-selects").click(function () {
        var projectcount = $("#hidEditprojectcount").val();
        if (projectcount == 0) {
            $("#spEditRecord").show();
        }
        else {
            $(this).children(".multi-input").focus();
            $(this).siblings(".custom-sel-list").toggle();
            $("#spEditRecord").hide();
        }
    });

    $("#ddlEditClientProject .custom-sel-list").hide();

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
            $(".multi-input").val("");
            $("#spRecord").hide();
        }
    });


    $("#ddlEditClientProject .custom-sel-list li").each(function () {
        var customlist = $(this).attr('value');
        var clientProject = $(this);
        var removeicon = '<span class="removeicon">x</span>';
        var id = $("#hidECid").val();
        indo.clientes.clients.getClientProjectMapping(id)
            .then(function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (customlist === data[i]) {
                        clientProject.clone().append(removeicon).appendTo(clientProject.parents("#ddlEditClientProject").find(".custom-selected"));
                        clientProject.addClass('hideitem');
                        custompipe.push(clientProject.attr('value'));
                        $("#hidEditprojectnameist").val();
                        $("#hidEditprojectnameist").val(custompipe);
                        EdititemNames.push(clientProject.attr('value'));

                    }
                }

            });
    });

    $("#ddlEditClientProject .custom-sel-list li").click(function () {

        var removeicon = '<span class="removeicon">x</span>';
        custompipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlEditClientProject").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidEditprojectnameist").val();
        $("#hidEditprojectnameist").val(custompipe);
        $("#ddlEditClientProject .multi-input").val("");
        EdititemNames.push($(this).attr('value'));
    });

    $("#ddlClientProject").on('click', function () {
        $("#projectList").show();
    });

    $("#ddlClientProject").on('focusin', function () {
        $("#projectList").show();
    });


    $(function () {
        $(document).on("click", "#ddlEditClientProject .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlEditClientProject .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = custompipe.indexOf(value);
            custompipe.splice(idIndex, 1);
            $("#hidEditprojectnameist").val();
            $("#hidEditprojectnameist").val(custompipe);
            var index = EdititemNames.indexOf(value);
            EdititemNames.splice(index, 1);
            if (EdititemNames.length == 0) {
                $("#ddlEditClientProject .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }

        });
    });

    $("#ddlEditClientProject .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase(); var matchingItemsExist = false;
        var matchingItemsExist = false;

        $("#ddlEditClientProject .custom-sel-list li").each(function () {
            if (EdititemNames.includes($(this).attr('value'))) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlEditClientProject .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !EdititemNames.includes($(this).attr('value'))) {
                $(this).show();
                matchingItemsExist = true;
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

    $('#ddlEditClientProject .multi-input').on('click', function () {
        $("#ddlEditClientProject .custom-sel-list li").each(function () {
            if (EdititemNames.includes($(this).attr('value'))) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    //check validation for Edit page

    $('#EditClientForm').submit(function (e) {
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