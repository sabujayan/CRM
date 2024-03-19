$(function () {

    var l = abp.localization.getResource('Indo');

    $("#txtProjectName").change(function () {
        var name = $("#txtProjectName").val();
        var id = $("#hidEprojectId").val();
        indo.projectes.projects.duplicateCheckforEdit(name, id).then(function (result) {
            if (result == true) {
                var errors = '';
                swal(errors, name + " Already Exists");
                $("#btnsave").prop('disabled', true);
                $("#txtProjectName").val('');
                $("#txtProjectName").focus();
                return false;
            }
        });
    });

    $('#txtStartDate').change(function () {
        var StartDate = $(this).val();
        var EndDate = $("#txtEndDate").val();
        if (StartDate !== "" && EndDate !== "") {
            var startDateObj = new Date(StartDate);
            var endDateObj = new Date(EndDate);
            if (startDateObj > endDateObj) {
                swal("Start date can't be greater than End date.");
                $(this).val('');
                $("#btnsave").prop('disabled', true);
            } else {
                $("#btnsave").prop('disabled', false);
            }
        }
    });

    $('#txtEndDate').change(function () {
        var StartDate = $("#txtStartDate").val();
        var EndDate = $(this).val();
        if (StartDate !== "" && EndDate !== "") {
            var startDateObj = new Date(StartDate);
            var endDateObj = new Date(EndDate);
            if (endDateObj < startDateObj) {
                swal("End date should be greater than Start date.");
                $(this).val('');
                $("#btnsave").prop('disabled', true);
            } else {
                $("#btnsave").prop('disabled', false);
            }
        }
    });

    // Client project Mapping for Edit

    var custompipe = [];
    var EdititemNames = [];

    $("#ddlTechnology .custom-selects").click(function () {
        $(this).children(".multi-input").focus();
        $(this).siblings(".custom-sel-list").toggle();
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

    $("#ddlTechnology .custom-sel-list li").each(function () {
        var customlist = $(this).attr('value');
        var techProject = $(this);
        var removeicon = '<span class="removeicon">x</span>';
        var id = $("#hidid").val();

        indo.projectes.projects.getTechnologyProjectMapping(id)
            .then(function (data) {
                console.log(data);
                for (var i = 0; i < data.length; i++) {
                    if (customlist === data[i]) {
                        techProject.clone().append(removeicon).appendTo(techProject.parents("#ddlTechnology").find(".custom-selected"));
                        techProject.addClass('hideitem');
                        custompipe.push(techProject.attr('value'));
                        $("#hidEdittechlist").val(custompipe);
                        EdititemNames.push(techProject.attr('value'));
                    }
                }
            });
    });

    $(function () {
        $(document).on("click", "#ddlTechnology .removeicon", function (e) {
            var client = $("#ddlClient").val();
            let value = $(this).parents('li').attr('value');
            $('#ddlTechnology .custom-sel-list li[value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = custompipe.indexOf(value);
            custompipe.splice(idIndex, 1);
            $("#hidEdittechlist").val(custompipe);
            var index = EdititemNames.indexOf(value);
            EdititemNames.splice(index, 1);

            if (EdititemNames.length == 0) {
                $("#ddlTechnology .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }

        });
    });

    $("#ddlTechnology .custom-sel-list li").click(function () {
        var client = $("#ddlClient").val();
        var removeicon = '<span class="removeicon">x</span>';
        custompipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlTechnology").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidEdittechlist").val();
        $("#hidEdittechlist").val(custompipe);
        $("#ddlTechnology .multi-input").val("");
        EdititemNames.push($(this).attr('value'));
    });

    $("#ddlTechnology").on('click', function () {
        $("#technologyList").show();
    });

    $("#ddlTechnology").on('focusin', function () {
        $("#technologyList").show();
    });

    $("#ddlTechnology .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;

        $("#ddlTechnology .custom-sel-list li").each(function () {
            if (EdititemNames.includes($(this).attr('value'))) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlTechnology .custom-sel-list li").each(function () {
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

    $('#ddlTechnology .multi-input').on('click', function () {
        $("#ddlTechnology .custom-sel-list li").each(function () {
            if (EdititemNames.includes($(this).attr('value'))) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $('#ddlClient').change(function () {
        var client = $(this).val();
        $("#hidClientsId").val();
        $("#hidClientsId").val(client);
    });

    var clientid = $("#hidClientId").val();
    $("#ddlClient").val(clientid);

    var Startdate = $("#hidStartdate").val();
    $("#txtStartDate").val(Startdate);

    $("#txtStartDate").on('change', function () {
        var startdate = $(this).val();
        if (startdate == '') {
            $("#hidSdate").val('0001-01-01');
        }
        else {
            $("#hidSdate").val();
            $("#hidSdate").val(startdate);
        }
    });
    var Enddate = $("#hidEnddate").val();
    $("#txtEndDate").val(Enddate);

    $("#txtEndDate").on('change', function () {
        var enddate = $(this).val();
        if (enddate == '') {
            $("#hidEdate").val('0001-01-01');
        }
        else {
            $("#hidEdate").val();
            $("#hidEdate").val(enddate);
        }
    });

    $('#EditProjectForm').submit(function (e) {
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
        if (custompipe.length < 1) {
            $('#ddlTechnology').after('<span class="error">The Technology List field is required.</span>');
            e.preventDefault();
            return false;
        }
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Project/Index";
        return true;
    });
});
