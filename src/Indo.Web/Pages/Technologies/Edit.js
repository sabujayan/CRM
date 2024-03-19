$(function () {
    var l = abp.localization.getResource('Indo');



    var selectedParentId = $("#hidparentid").val();
    var parentStatus = $("#hidparentstatus").val();
    console.log("parent status   " + parentStatus)

    if (parentStatus == "fail") {
        debugger;
        $("#Tech_ParentId").val("");

        $("select.TechParentId").change(function () {
            var selectedParentId1 = $(this).val();
            $("#hidparentid").val(selectedParentId1);
        });
    } else {
        $("#Tech_ParentId").val(selectedParentId);

        $("select.TechParentId").change(function () {
            var selectedParentId1 = $(this).val();
            $("#hidparentid").val(selectedParentId1);
        });
    }


    //
    $('#EditTechnologyForm').submit(function (e) {
        debugger;
        var tech_name = $('#txtEditName').val();
        $(".error").remove();
        if (tech_name.length < 1) {
            $('#txtEditName').after('<span class="error">Technology Name field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z/#+-.* 0-9]*$/;
            var validTechName = regx.test(tech_name);
            if (!validTechName) {
                $('#txtEditName').after('<span class="error">Technology Name field should only contain [a-zA-Z/#+-.* ].</span>');
                e.preventDefault();
                return false;
            }
        }
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Technologies/Index";
        return true;
    });
});