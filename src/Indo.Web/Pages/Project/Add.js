$(function () {

    var l = abp.localization.getResource('Indo');

    var technologyListError = $('#technologyListError');

    $("#txtProjectName").change(function () {
        var name = $("#txtProjectName").val();
        indo.projectes.projects.duplicateCheckforAdd(name).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#txtProjectName").val('');
                $("#txtProjectName").focus();
                return false;
            }
        });
    });

    // Add Technology-project Mapping for Save

    var pipe = [];
    var itemNames = [];


    $("#ddlTechnology .custom-selects").click(function () {
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


    $("#ddlTechnology .custom-sel-list").hide();

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
            $(".multi-input").val("");
            $("#spRecord").hide();
        }
    });

    $("#ddlTechnology").mouseleave(function () {
        $("#spRecord").hide();
    });

    $("#ddlTechnology .custom-sel-list li").click(function () {
        var client = $("#ddlClient").val();
        var removeicon = '<span class="removeicon">x</span>';
        pipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlTechnology").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidtechnologynameist").val();
        $("#hidtechnologynameist").val(pipe);
        var name = $(this).text();
        itemNames.push(name);
        $("#ddlTechnology .multi-input").val("");

    });

    $(function () {
        $(document).on("click", "#ddlTechnology .removeicon", function (e) {

            var client = $("#ddlClient").val();
            let value = $(this).parents('li').attr('value');
            $('#ddlTechnology .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe.indexOf(value);
            pipe.splice(idIndex, 1);
            $("#hidtechnologynameist").val();
            $("#hidtechnologynameist").val(pipe);
            var name = $(this).parents("#ddlTechnology").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames.indexOf(selectedValue);
            itemNames.splice(index, 1);
            if (itemNames.length == 0) {
                $("#ddlTechnology .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
        });
    });

    $('#ddlTechnology .multi-input').on('click', function () {
        $("#ddlTechnology .custom-sel-list li").each(function () {
            if (itemNames.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $("#ddlTechnology .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;
        $("#ddlTechnology .custom-sel-list li").each(function () {
            if (itemNames.includes($(this).text())) {
                $(this).hide();

            } else {
                $(this).show();
            }
        });

        $("#ddlTechnology .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !itemNames.includes($(this).text())) {
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

    $('#txtStartDate').change(function () {
        var StartDate = $(this).val();
        $("#hidStartdate").val();
        var Endate = $("#txtEndDate").val();
        if (Date.parse(StartDate) > Date.parse(Endate)) {
            swal("Start date Can't be greater than End date..");
            $("#txtStartDate").val('');
            return false;
        }
        else {
            $("#hidStartdate").val(startdate);
            return true;
        }
    });

    $('#txtEndDate').change(function (e) {
        var StartDate = $("#txtStartDate").val();
        var Endate = $(this).val();
        $("#hidEndDate").val();
        if (Date.parse(StartDate) > Date.parse(Endate)) {
            swal("End date should be greater than Start date..");
            $("#txtEndDate").val('');
            return false;
        }
        else {
            $("#hidEndDate").val(enddate);
            return true;
        }
    });



    $('#ddlClient').change(function () {
        var client = $(this).val();
        $("#hidClientsId").val();
        $("#hidClientsId").val(client);
    });

    $('#AddProjectForm').submit(function (e) {
        debugger;
        var project_name = $('#txtProjectName').val();
        var client_id = $('#ddlClient').val();
        var estimate_hour = $('#txtEstimateHours').val();
        $(".error").remove();
        if (project_name.length < 1) {
            $('#txtProjectName').after('<span class="error">The Name field is required.</span>');
            e.preventDefault();
            return false;
        }
        if (client_id == "null") {
            $('#ddlClient').after('<span class="error">Plese select client.</span>');
            e.preventDefault();
            return false;
        }
        if (estimate_hour.length < 1) {
            $('#txtEstimateHours').after('<span class="error">The Estimate Hours field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[0-9_._ ]+$/;
            var validTechName = regx.test(estimate_hour);
            if (!validTechName) {
                $('#txtEstimateHours').after('<span class="error">The Estimate Hours Not in proper format.</span>');
                e.preventDefault();
                return false;
            }
        }
        if (pipe.length < 1) {
            $('#ddlTechnology').after('<span class="error">The Technology List field is required.</span>');
            e.preventDefault();
            return false;
        }
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Project/Index";
        return true;
    });
});