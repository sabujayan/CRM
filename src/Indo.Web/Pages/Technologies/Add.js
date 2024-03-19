$(function () {
    var l = abp.localization.getResource('Indo');



    $("select.TechParentId").change(function () {
        var selectedParentId = $(this).children("option:selected").val();
        $("#hidparentid").val();
        $("#hidparentid").val(selectedParentId);
    });

    $('#AddTechnologyForm').submit(function (e) {
        debugger;
        var tech_name = $('#txtTechName').val(); 
        $(".error").remove();
        if (tech_name.length < 1) {
            $('#txtTechName').after('<span class="error">Technology Name field is required.</span>');
            e.preventDefault();
            return false;
        }
        else {
            var regx = /^[a-zA-Z/#+-.* 0-9]*$/;
            var validTechName = regx.test(tech_name);
            if (!validTechName) {
                $('#txtTechName').after('<span class="error">Technology Name field should only contain [a-zA-Z/#+-.* ].</span>');
                e.preventDefault();
                return false;
            }
        }
        abp.message.success(l('Saved Successfully'));
        window.location.href = "/Technologies/Index";
        return true;
    });
});