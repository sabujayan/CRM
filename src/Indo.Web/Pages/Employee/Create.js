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

    //
    var name = $("#txtEmployeeName").val();
    var empNo = $("#txtEmployeeNumber").val();
    var street = $("#txtEmployeeStreet").val();
    var city = $("#txtEmployeeCity").val();
    var state = $("#txtEmployeeState").val();
    var zip = $("#txtEmployeeZipCode").val();
    var phoneNumber = $("#txtEmployeePhone").val();
    var email = $("#txtEmployeeEmail").val();
    var position = $("#txtEmployeePosition").val();
    var depId = $("#depId").val();
    var empGroup = $("#empGroup").val();

    //
    var pipe = [];
    var itemNames = [];

    var projectsError = $('#projectsError');
    var skillListError = $('#skillListError');
    var clientsError = $('#clientsError');

    $("#ddlClientProject .custom-selects").click(function () {
        var projectcount = $("#hidTechnologycount").val();
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


        if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            empNo == "" || /^[0-9]*$/.test(empNo) === false ||
            street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
            city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
            state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
            zip === "" || /^[0-9]*$/.test(name) === false ||
            phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
            email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
            position === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            depId === "" || empGroup === "") {
            if (pipe.length == 0) {
                $("#btnsave").prop('disabled', true);
                projectsError.show();
            }
        }

        if (name != "" || /^[a-zA-Z ]*$/.test(name) != false ||
            empNo != "" || /^[0-9]*$/.test(empNo) === false ||
            street != "" || /^[a-zA-Z0-9-,. ]*$/.test(street) != false ||
            city != "" || /^[a-zA-Z ]*$/.test(city) != false ||
            state != "" || /^[a-zA-Z ]*$/.test(state) != false ||
            zip != "" || /^[0-9]*$/.test(name) != false ||
            phoneNumber != "" || /^[0-9]{10}$/.test(phoneNumber) != false ||
            email != "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) != false ||
            position != "" || /^[a-zA-Z ]*$/.test(name) != false ||
            depId != "" || empGroup != "") {
            if (pipe.length > 0) {
                $("#btnsave").prop('disabled', false);
                projectsError.hide();
            }
            else {
                $("#btnsave").prop('disabled', true);
                projectsError.show();
            }
        }
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

            if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
                empNo == "" || /^[0-9]*$/.test(empNo) === false ||
                street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
                city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
                state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
                zip === "" || /^[0-9]*$/.test(name) === false ||
                phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
                email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
                position === "" || /^[a-zA-Z ]*$/.test(name) === false ||
                depId === "" || empGroup === "") {
                if (pipe.length == 0) {
                    $("#btnsave").prop('disabled', true);
                    projectsError.show();
                }
            }

            if (name != "" || /^[a-zA-Z ]*$/.test(name) != false ||
                empNo != "" || /^[0-9]*$/.test(empNo) === false ||
                street != "" || /^[a-zA-Z0-9-,. ]*$/.test(street) != false ||
                city != "" || /^[a-zA-Z ]*$/.test(city) != false ||
                state != "" || /^[a-zA-Z ]*$/.test(state) != false ||
                zip != "" || /^[0-9]*$/.test(name) != false ||
                phoneNumber != "" || /^[0-9]{10}$/.test(phoneNumber) != false ||
                email != "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) != false ||
                position != "" || /^[a-zA-Z ]*$/.test(name) != false ||
                depId != "" || empGroup != "") {
                if (pipe.length > 0) {
                    $("#btnsave").prop('disabled', false);
                    projectsError.hide();
                }
                else {
                    $("#btnsave").prop('disabled', true);
                    projectsError.show();
                }
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

    $("#ddlClientProject").on("focusin", function () {
        if (pipe.length === 0) {
            $("#projectsError").show();
        }
        else {
            $("#projectsError").hide();
        }
    });

    //
    $("#ddlClientSkill .custom-selects").click(function () {
        var skillCount = $("#hidprojectcount").val();
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
        pipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlClientSkill").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidprojectnameist").val();
        $("#hidprojectnameist").val(pipe);
        var name = $(this).text();
        itemNames.push(name);
        $("#ddlClientSkill .multi-input").val("");

        if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            empNo == "" || /^[0-9]*$/.test(empNo) === false ||
            street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
            city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
            state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
            zip === "" || /^[0-9]*$/.test(name) === false ||
            phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
            email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
            position === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            depId === "" || empGroup === "") {
            if (pipe.length == 0) {
                $("#btnsave").prop('disabled', true);
                skillListError.show();
            }
        }

        if (name != "" || /^[a-zA-Z ]*$/.test(name) != false ||
            empNo != "" || /^[0-9]*$/.test(empNo) === false ||
            street != "" || /^[a-zA-Z0-9-,. ]*$/.test(street) != false ||
            city != "" || /^[a-zA-Z ]*$/.test(city) != false ||
            state != "" || /^[a-zA-Z ]*$/.test(state) != false ||
            zip != "" || /^[0-9]*$/.test(name) != false ||
            phoneNumber != "" || /^[0-9]{10}$/.test(phoneNumber) != false ||
            email != "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) != false ||
            position != "" || /^[a-zA-Z ]*$/.test(name) != false ||
            depId != "" || empGroup != "") {
            if (pipe.length > 0) {
                $("#btnsave").prop('disabled', false);
                skillListError.hide();
            }
            else {
                $("#btnsave").prop('disabled', true);
                skillListError.show();
            }
        }
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
            var idIndex = pipe.indexOf(value);
            pipe.splice(idIndex, 1);
            $("#hidprojectnameist").val();
            $("#hidprojectnameist").val(pipe);
            var name = $(this).parents("#ddlClientSkill").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames.indexOf(selectedValue);
            itemNames.splice(index, 1);
            if (itemNames.length == 0) {
                $("#ddlClientSkill .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }

            if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
                empNo == "" || /^[0-9]*$/.test(empNo) === false ||
                street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
                city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
                state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
                zip === "" || /^[0-9]*$/.test(name) === false ||
                phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
                email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
                position === "" || /^[a-zA-Z ]*$/.test(name) === false ||
                depId === "" || empGroup === "") {
                if (pipe.length == 0) {
                    $("#btnsave").prop('disabled', true);
                    clientsError.show();
                }
            }

            if (name != "" || /^[a-zA-Z ]*$/.test(name) != false ||
                empNo != "" || /^[0-9]*$/.test(empNo) === false ||
                street != "" || /^[a-zA-Z0-9-,. ]*$/.test(street) != false ||
                city != "" || /^[a-zA-Z ]*$/.test(city) != false ||
                state != "" || /^[a-zA-Z ]*$/.test(state) != false ||
                zip != "" || /^[0-9]*$/.test(name) != false ||
                phoneNumber != "" || /^[0-9]{10}$/.test(phoneNumber) != false ||
                email != "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) != false ||
                position != "" || /^[a-zA-Z ]*$/.test(name) != false ||
                depId != "" || empGroup != "") {
                if (pipe.length > 0) {
                    $("#btnsave").prop('disabled', false);
                    skillListError.hide();
                }
                else {
                    $("#btnsave").prop('disabled', true);
                    skillListError.show();
                }
            }
        });
    });

    $('#ddlClientSkill .multi-input').on('click', function () {
        $("#ddlClientSkill .custom-sel-list li").each(function () {
            if (itemNames.includes($(this).text())) {
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
            if (itemNames.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlClientSkill .custom-sel-list li").each(function () {
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
            $("#srpRecord").show();
        } else {
            $("#srpRecord").hide();
        }
    });

    $("#ddlClientSkill").on("focusin", function () {
        if (pipe.length === 0) {
            $("#skillListError").show();
        }
        else {
            $("#skillListError").hide();
        }
    });

    //
    $("#ddlClient .custom-selects").click(function () {
        var projectcount = $("#hidprojectcount").val();
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
        pipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlClient").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidprojectnameist").val();
        $("#hidprojectnameist").val(pipe);
        var name = $(this).text();
        itemNames.push(name);
        $("#ddlClient .multi-input").val("");

        if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            empNo == "" || /^[0-9]*$/.test(empNo) === false ||
            street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
            city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
            state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
            zip === "" || /^[0-9]*$/.test(name) === false ||
            phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
            email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
            position === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            depId === "" || empGroup === "") {
            if (pipe.length == 0) {
                $("#btnsave").prop('disabled', true);
                clientsError.show();
            }
        }

        if (name != "" || /^[a-zA-Z ]*$/.test(name) != false ||
            empNo!= "" || /^[0-9]*$/.test(empNo) === false ||
            street != "" || /^[a-zA-Z0-9-,. ]*$/.test(street) != false ||
            city != "" || /^[a-zA-Z ]*$/.test(city) != false ||
            state != "" || /^[a-zA-Z ]*$/.test(state) != false ||
            zip != "" || /^[0-9]*$/.test(name) != false ||
            phoneNumber != "" || /^[0-9]{10}$/.test(phoneNumber) != false ||
            email != "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) != false ||
            position != "" || /^[a-zA-Z ]*$/.test(name) != false ||
            depId != "" || empGroup != "") {
            if (pipe.length > 0) {
                $("#btnsave").prop('disabled', false);
                clientsError.hide();
            }
            else {
                $("#btnsave").prop('disabled', true);
                clientsError.show();
            }
        }
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
            var idIndex = pipe.indexOf(value);
            pipe.splice(idIndex, 1);
            $("#hidprojectnameist").val();
            $("#hidprojectnameist").val(pipe);
            var name = $(this).parents("#ddlClient").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames.indexOf(selectedValue);
            itemNames.splice(index, 1);
            console.log("itme length on remove" + itemNames.length);
            if (itemNames.length == 0) {
                $("#ddlClient .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }

            if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
                empNo == "" || /^[0-9]*$/.test(empNo) === false ||
                street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
                city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
                state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
                zip === "" || /^[0-9]*$/.test(name) === false ||
                phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
                email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
                position === "" || /^[a-zA-Z ]*$/.test(name) === false ||
                depId === "" || empGroup === "") {
                if (pipe.length == 0) {
                    $("#btnsave").prop('disabled', true);
                    clientsError.show();
                }
            }

            if (name != "" || /^[a-zA-Z ]*$/.test(name) != false ||
                empNo != "" || /^[0-9]*$/.test(empNo) === false ||
                street != "" || /^[a-zA-Z0-9-,. ]*$/.test(street) != false ||
                city != "" || /^[a-zA-Z ]*$/.test(city) != false ||
                state != "" || /^[a-zA-Z ]*$/.test(state) != false ||
                zip != "" || /^[0-9]*$/.test(name) != false ||
                phoneNumber != "" || /^[0-9]{10}$/.test(phoneNumber) != false ||
                email != "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) != false ||
                position != "" || /^[a-zA-Z ]*$/.test(name) != false ||
                depId != "" || empGroup != "") {
                if (pipe.length > 0) {
                    $("#btnsave").prop('disabled', false);
                    clientsError.hide();
                }
                else {
                    $("#btnsave").prop('disabled', true);
                    clientsError.show();
                }
            }
        });
    });

    $('#ddlClient .multi-input').on('click', function () {
        $("#ddlClient .custom-sel-list li").each(function () {
            if (itemNames.includes($(this).text())) {
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
            if (itemNames.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlClient .custom-sel-list li").each(function () {
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
            $("#scpRecord").show();
        } else {
            $("#scpRecord").hide();
        }
    });

    $("#ddlClient").on("focusin", function () {
        if (pipe.length === 0) {
            $("#clientsError").show();
        }
        else {
            $("#clientsError").hide();
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
    function checkRequiredFields() {

        if (name === "" || /^[a-zA-Z ]*$/.test(name) === false ||
            empNo == "" || /^[0-9]*$/.test(empNo) === false ||
            street === "" || /^[a-zA-Z0-9-,. ]*$/.test(street) === false ||
            city === "" || /^[a-zA-Z ]*$/.test(city) === false ||
            state === "" || /^[a-zA-Z ]*$/.test(state) === false ||
            zip === "" || /^[0-9]*$/.test(name) === false ||
            phoneNumber === "" || /^[0-9]{10}$/.test(phoneNumber) === false ||
            email === "" || /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/.test(email) === false ||
            position === "" || /^[a-zA-Z ]*$/.test(name) === false /*||
            depId === "" || empGroup === ""*/ ) {
            $("#btnsave").prop('disabled', true);
        } else {
            $("#btnsave").prop('disabled', false);
        }
    }
    $('.addValidation').on('input', function () {
        checkRequiredFields();
    });

    checkRequiredFields();

    // Name Validation
    $("#txtEmployeeName").on('focus', function () {
        var nameError = $('#nameError');
        var name = $(this).val().trim();
        if (name === "") {
            nameError.show();
        } else {
            nameError.hide();
        }

        $("#txtEmployeeName").on('input', function () {
            var nameValidation = $("#nameValidation");
            var nameError = $('#nameError');
            var name = $(this).val();

            var exp = /^[a-zA-Z ]*$/;

            if (exp.test(name) === false) {
                nameValidation.show();
            } else {
                nameValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                nameError.show();
            } else {
                nameError.hide();
            }
        });

        checkRequiredFields();

    });

    // EmployeeId Validation
    $("#txtEmployeeNumber").on('focus', function () {
        var employeeNumberError = $('#employeeNumberError');
        var name = $(this).val().trim();
        if (name === "") {
            employeeNumberError.show();
        } else {
            employeeNumberError.hide();
        }

        $("#txtEmployeeNumber").on('input', function () {
            var employeeNumberValidation = $("#employeeNumberValidation");
            var employeeNumberError = $('#employeeNumberError');
            var name = $(this).val();

            var exp = /^[0-9]*$/;

            if (exp.test(name) === false) {
                employeeNumberValidation.show();
            } else {
                employeeNumberValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                employeeNumberError.show();
            } else {
                employeeNumberError.hide();
            }
        });

        checkRequiredFields();

    });

    // Street Validation
    $("#txtEmployeeStreet").on('focus', function () {
        var streetError = $('#streetError');
        var name = $(this).val().trim();
        if (name === "") {
            streetError.show();
        } else {
            streetError.hide();
        }

        $("#txtEmployeeStreet").on('input', function () {
            var streetValidation = $("#streetValidation");
            var streetError = $('#streetError');
            var name = $(this).val();

            var exp = /^[a-zA-Z0-9-,. ]*$/;

            if (exp.test(name) === false) {
                streetValidation.show();
            } else {
                streetValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                streetError.show();
            } else {
                streetError.hide();
            }
        });

        checkRequiredFields();

    });

    // City Validation
    $("#txtEmployeeCity").on('focus', function () {
        var cityError = $('#cityError');
        var name = $(this).val().trim();
        if (name === "") {
            cityError.show();
        } else {
            cityError.hide();
        }

        $("#txtEmployeeCity").on('input', function () {
            var cityValidation = $("#cityValidation");
            var cityError = $('#cityError');
            var name = $(this).val();

            var exp = /^[a-zA-Z ]*$/;

            if (exp.test(name) === false) {
                cityValidation.show();
            } else {
                cityValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                cityError.show();
            } else {
                cityError.hide();
            }
        });

        checkRequiredFields();

    });

    // State Validation
    $("#txtEmployeeState").on('focus', function () {
        var stateError = $('#stateError');
        var name = $(this).val().trim();
        if (name === "") {
            stateError.show();
        } else {
            stateError.hide();
        }

        $("#txtEmployeeState").on('input', function () {
            var stateValidation = $("#stateValidation");
            var stateError = $('#stateError');
            var name = $(this).val();

            var exp = /^[a-zA-Z ]*$/;

            if (exp.test(name) === false) {
                stateValidation.show();
            } else {
                stateValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                stateError.show();
            } else {
                stateError.hide();
            }
        });

        checkRequiredFields();

    });

    // ZipCode Validation
    $("#txtEmployeeZipCode").on('focus', function () {
        var zipCodeError = $('#zipCodeError');
        var name = $(this).val().trim();
        if (name === "") {
            zipCodeError.show();
        } else {
            zipCodeError.hide();
        }

        $("#txtEmployeeZipCode").on('input', function () {
            var zipCodeValidation = $("#zipCodeValidation");
            var zipCodeError = $('#zipCodeError');
            var name = $(this).val();

            var exp = /^[0-9]*$/;

            if (exp.test(name) === false) {
                zipCodeValidation.show();
            } else {
                zipCodeValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                zipCodeError.show();
            } else {
                zipCodeError.hide();
            }
        });

        checkRequiredFields();

    });

    // Phone Number Validation
    $("#txtEmployeePhone").on('focus', function () {
        var phoneCodeError = $('#phoneCodeError');
        var name = $(this).val().trim();
        if (name === "") {
            phoneCodeError.show();
        } else {
            phoneCodeError.hide();
        }

        $("#txtEmployeePhone").on('input', function () {
            var phoneCodeValidation = $("#phoneCodeValidation");
            var phoneCodeError = $('#phoneCodeError');
            var name = $(this).val();

            var exp = /^[0-9]{10}$/;

            if (exp.test(name) === false) {
                phoneCodeValidation.show();
            } else {
                phoneCodeValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                phoneCodeError.show();
            } else {
                phoneCodeError.hide();
            }
        });

        checkRequiredFields();

    });

    // Email Validation
    $("#txtEmployeeEmail").on('focus', function () {
        var emailCodeError = $('#emailCodeError');
        var name = $(this).val().trim();
        if (name === "") {
            emailCodeError.show();
        } else {
            emailCodeError.hide();
        }

        $("#txtEmployeeEmail").on('input', function () {
            var emailCodeValidation = $("#emailCodeValidation");
            var emailCodeError = $('#emailCodeError');
            var name = $(this).val();

            var exp = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;

            if (exp.test(name) === false) {
                emailCodeValidation.show();
            } else {
                emailCodeValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                emailCodeError.show();
            } else {
                emailCodeError.hide();
            }
        });

        checkRequiredFields();

    });

    // Position Validation
    $("#txtEmployeePosition").on('focus', function () {
        var positionError = $('#positionError');
        var name = $(this).val().trim();
        if (name === "") {
            positionError.show();
        } else {
            positionError.hide();
        }

        $("#txtEmployeePosition").on('input', function () {
            var positionValidation = $("#positionValidation");
            var positionError = $('#positionError');
            var name = $(this).val();

            var exp = /^[a-zA-Z ]*$/;

            if (exp.test(name) === false) {
                positionValidation.show();
            } else {
                positionValidation.hide();
            }

            var value = $(this).val().trim();
            if (value === "") {
                positionError.show();
            } else {
                positionError.hide();
            }
        });

        checkRequiredFields();

    });

    /*$('#createEmployee').on('click', function (e) {
        
        e.preventDefault();
        var _$form = $('form[id=AddEployeeRequest]');
        var Employeecreaterequest = _$form.serializeFormToObject();
        Employeecreaterequest.employee.skilllist = skilllist;
        Employeecreaterequest.employee.projectnamelist = projectnamelist;
        Employeecreaterequest.employee.clientnamelist = clientnameist;
        Employeecreaterequest.employee.clientnameist = clientnameist
        for (var propName in Employeecreaterequest.employee) {
            if (propName != "id" && Employeecreaterequest.employee[propName] === null
                || propName != "id" && Employeecreaterequest.employee[propName] === undefined
                || propName != "id" && Employeecreaterequest.employee[propName] === ''
            ) {
                return abp.message.error('Please Check the data of' + propName + 'and try again', 'Error');;
            }
        }

        var checkEmail = Employeecreaterequest.employee.email;
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        if (!emailReg.test(checkEmail)) {
            abp.message.error('Please enter correct Email Address');
            return;
        }
        var checkMobile = Employeecreaterequest.employee.phone;
        var filter = /^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/;
        if (!filter.test(checkMobile)) {
            abp.message.error('Please enter correct Phoune Number');
            return
        }

        indo.employees.employee.checkIfEmployeeNumberExist(Employeecreaterequest.employee.employeeNumber).done(function (result) {
            
            if (result == false) {
                indo.employees.employee.create(Employeecreaterequest.employee);

            }
            else {
                abp.message.error("Provided Employee Number Already Exist!.")
            }
        })
        
    });*/

    $("#btnsave").click(function () {
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Employee";
/*      window.location.href = "/Employee";*/
    });

    $('#addCancel').on('click', function (e) {
        window.location.href = "/Employee";
    });
});