$(document).ready(function () {
    var l = abp.localization.getResource('Indo');
    // Client project Mapping for Save
    var pipe = [];
    var custompipe = [];



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
        }
    });

    $("#ddlTechnology .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlTechnology").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidtechnologynameist").val();
        $("#hidtechnologynameist").val(pipe);
    });

    $(function () {
        $(document).on("click", "#ddlTechnology .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlTechnology .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            pipe.pop(value);
            $("#hidtechnologynameist").val();
            $("#hidtechnologynameist").val(pipe);

        });
    });

    // End Client project Mapping for


    // Client project Mapping for Edit



    $("#ddlEditTechnology .custom-selects").click(function () {
        $(this).children(".multi-input").focus();
        $(this).siblings(".custom-sel-list").toggle();
    });

    $("#ddlEditTechnology .custom-sel-list").hide();

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
        }
    });


    $("#ddlEditTechnology .custom-sel-list li").each(function () {
        var customlist = $(this).attr('value');
        var techProject = $(this);
        var removeicon = '<span class="removeicon">x</span>';
        var id = $("#hidid").val();
        indo.projectes.projects.getTechnologyProjectMapping(id)
            .then(function (data) {
                console.log(data);
                for (var i = 0; i < data.length; i++) {
                    if (customlist === data[i]) {
                        techProject.clone().append(removeicon).appendTo(techProject.parents("#ddlEditTechnology").find(".custom-selected"));
                        techProject.addClass('hideitem');
                        custompipe.push(techProject.attr('value'));
                        $("#hidEdittechlist").val();
                        $("#hidEdittechlist").val(custompipe);

                    }
                }

            });
    });


    $("#ddlEditTechnology .custom-sel-list li").click(function () {

        var removeicon = '<span class="removeicon">x</span>';
        custompipe.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlEditTechnology").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidEdittechlist").val();
        $("#hidEdittechlist").val(custompipe);

    });

    $(function () {
        $(document).on("click", "#ddlEditTechnology .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlEditTechnology .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            custompipe.pop(value);
            $("#hidEdittechlist").val();
            $("#hidEdittechlist").val(custompipe);

        });
    });


    // End Client project Mapping for Edit

});