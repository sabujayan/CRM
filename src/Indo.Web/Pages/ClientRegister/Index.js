
$(document).ready(function () {
    var l = abp.localization.getResource('Indo');
    var emailExpression = $("#hidemailexpression").val();
    console.log("email expression:  " + emailExpression);
    $("#txtClientEmail").change(function () {
        var name = $("#txtClientEmail").val();
        indo.projectes.projects.duplicateCheckforClientRegister(name).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#txtClientEmail").val('');
                $("#txtClientEmail").focus();
                return false;
            }
        });
    });

    $("#txtClientNumber").change(function () {
        var name = $("#txtClientNumber").val();
        indo.projectes.projects.duplicateCheckforClientRegisterPhoneNumber(name).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#txtClientNumber").val('');
                $("#txtClientNumber").focus();
                return false;
            }
        });
    });

    $("#txtClientName").change(function () {
        var name = $("#txtClientName").val();
        indo.projectes.projects.duplicateCheckforClient(name).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#txtClientName").val('');
                $("#txtClientName").focus();
                return false;
            }
        });
    });

    $("#txtClientName").on('keypress', function (event) {
        if (event.charCode != 0) {
            var regex = new RegExp("^[a-zA-Z_ ]*$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    });

    $("#txtClientName").on('focus', function () {
        var name = $(this).val().trim();
        if (name === '') {
            $("#nameError").css("display", "block");
            $("#nameValidation").css("display", "none");
        }
    });
    $("#txtClientName").on('input', function () {
        $("#nameError").css("display", "none");
        $("#nameValidation").css("display", "none");

    });
    $("#txtClientEmail").on('focus', function () {
        debugger;
        var email = $(this).val().trim();
        if (email === '') {
            $("#emailError").css("display", "block");
            $("#emailValidation").css("display", "none");
        }
    });
    $("#txtClientEmail").on('input', function () {
        debugger;
        var emailValidation = $("#emailValidation");
        var emailError = $('#emailError');
        var email = $(this).val().trim();
        var exp = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
        if (exp.test(email) === false) {
            if (email === '') {
                emailError.show();
                emailValidation.hide();
            } else {
                emailValidation.show();
                emailError.hide();
            }
        } else {
            emailValidation.hide();
            emailError.hide();
        }
    });
    $("#txtClientEmail").keypress(function (e) {
        if (e.which === 32)
            return false;
    });

    $("#txtClientPhoneNumber").on('focus', function () {
        var phone = $(this).val().trim();
        if (phone === '') {
            $("#phoneNumberError").css("display", "block");
            $("#phoneNumberValidation").css("display", "none");
        }
    });

    $("#txtClientPhoneNumber").on('input', function () {
        var phoneNumberValidation = $("#phoneNumberValidation");
        var phoneNumberError = $('#phoneNumberError');
        var phoneNumber = $(this).val();
        var exp = /^(0|91)?[6-9][0-9]{9}$/;
        if (exp.test(phoneNumber) === false) {
            if (phoneNumber === '') {
                phoneNumberError.show();
                phoneNumberValidation.hide();
            } else {
                phoneNumberValidation.show();
                phoneNumberError.hide();
            }
        } else {
            phoneNumberValidation.hide();
            phoneNumberError.hide();
        }
    });
    $("#txtClientPhoneNumber").keypress(function (e) {
        if (e.which === 32)
            return false;
    });
    $("#txtClientPhoneNumber").keypress(function (e) {
        if (event.charCode != 0) {
            var regex = new RegExp("^[0-9]*$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    });

  
    $("#txtClientAddress").on('focus', function () {
        var address = $(this).val().trim();
        if (address === '') {
            $("#AddressError").css("display", "block");
            $("#AddressValidation").css("display", "none");
        }
    });
    $("#txtClientAddress").on('input', function () {
        $("#AddressError").css("display", "none");
        $("#AddressValidation").css("display", "none");

    });

    $("#txtCountry").on('focus', function () {
        var country = $(this).val().trim();
        if (country === '') {
            $("#CountryError").css("display", "block");
            $("#CountryValidation").css("display", "none");
        }
    });
    $("#txtCountry").on('input', function () {
        $("#CountryError").css("display", "none");
        $("#CountryValidation").css("display", "none");

    });
    $("#txtCountry").on('keypress', function (event) {
        if (event.charCode != 0) {
            var regex = new RegExp("^[a-zA-Z_ ]*$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    });

    $("#txtState").on('focus', function () {
        var state = $(this).val().trim();
        if (state === '') {
            $("#StateError").css("display", "block");
            $("#StateValidation").css("display", "none");
        }
    });

    $("#txtState").on('input', function () {
        $("#StateError").css("display", "none");
        $("#StateValidation").css("display", "none");

    });

    $("#txtState").on('keypress', function (event) {
        if (event.charCode != 0) {
            var regex = new RegExp("^[a-zA-Z_ ]*$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    });

    $("#txtCity").on('focus', function () {
        var city = $(this).val().trim();
        if (city === '') {
            $("#CityError").css("display", "block");
            $("#CityValidation").css("display", "none");
        }
    });

    $("#txtCity").on('input', function () {
        $("#CityError").css("display", "none");
        $("#CityValidation").css("display", "none");
    });

    $("#txtCity").on('keypress', function (event) {
        if (event.charCode != 0) {
            var regex = new RegExp("^[a-zA-Z_ ]*$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    });

    $("#txtZip").on('focus', function () {
        var zip = $(this).val().trim();
        if (zip === '') {
            $("#ZipError").css("display", "block");
            $("#ZipValidation").css("display", "none");
        }
    });

    $("#txtZip").on('input', function () {
        $("#ZipError").css("display", "none");
        $("#ZipValidation").css("display", "none");
    });

    $("#txtZip").on('keypress', function (event) {
        if (event.charCode != 0) {
            var regex = new RegExp("^[0-9]*$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    });
    $("#txtZip").keypress(function (e) {
        if (e.which === 32)
            return false;
    });

    $("#txtPassword").on('focus', function () {
        var password = $(this).val().trim();
        if (password === '') {
            $("#PasswordError").css("display", "block");
            $("#PasswordValidation").css("display", "none");
        }
    });

    $("#txtPassword").on('input', function () {
        $("#PasswordError").css("display", "none");
        $("#PasswordValidation").css("display", "none");
    });

    function checkRequiredFields() {
        var name = $("#txtClientName").val();
        var email = $("#txtClientEmail").val();
        var phone = $("#txtClientPhoneNumber").val();
        var address = $("#txtClientAddress").val();
        var country = $("#txtCountry").val();
        var state = $("#txtState").val();
        var city = $("#txtCity").val();
        var zip = $("#txtZip").val();
        var password = $("#txtPassword").val();
        var confirmpass = $("#txtConfirmPassword").val();
      
        if (name === "" || email === "" || phone === "" || address === "" || country === "" || state === "" || city === "" || zip === "" || password === "" || confirmpass==="") {
            $("#btnsave").prop('disabled', true);
        } else {
            $("#btnsave").prop('disabled', false);
        }
    }
    $('.addValidation').on('input', function () {
        checkRequiredFields();
    });

    checkRequiredFields();

    $("#txtConfirmPassword").on('focus', function () {
        var confirmpassword = $(this).val().trim();
        if (confirmpassword === '') {
            $("#cpasswordError").css("display", "block");
            $("#cpasswordValidation").css("display", "none");
        }
    });

    $("#txtConfirmPassword").on('input', function () {
      
        $("#cpasswordError").css("display", "none");
        var password = $("#txtPassword").val();
        var confirmpass = $("#txtConfirmPassword").val();
        if (confirmpass == "") {
            $("#cpasswordError").css("display", "block");
            $("#cpasswordValidation").css("display", "none");

        }
        else {
            if (password != confirmpass) {
                $("#cpasswordError").css("display", "none");
                $("#cpasswordValidation").css("display", "block");
                $("#btnsave").prop('disabled', true);
            } else {
                $("#btnsave").prop('disabled', false);
                $("#cpasswordError").css("display", "none");
                $("#cpasswordValidation").css("display", "none");
            }
        }
    });



});
