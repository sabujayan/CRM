$(document).ready(function () {
    // Client project Mapping for Save
    var pipe = [];
    var custompipe = [];
    var itemNames = [];
  

    var l = abp.localization.getResource('Indo');

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
        }
    });

    $("#ddlClientProject").mouseleave(function () {
        $("#spRecord").hide();
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

    $(function () {
        $(document).on("click", "#ddlClientProject .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlClientProject .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe.indexOf(value);
            pipe.splice(idIndex, 1);
            //pipe.pop(value);
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
            } else {
                $(this).hide();
            }
        });
    });

    

    // End Add Client project Mapping for Save


    // Client project Mapping for Edit
    var EdititemNames = [];

    $("#ddlEditClientProject .custom-selects").click(function () {
        $(this).children(".multi-input").focus();
        $(this).siblings(".custom-sel-list").toggle();

    });

    $("#ddlEditClientProject .custom-sel-list").hide();

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
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

    $(function () {
        $(document).on("click", "#ddlEditClientProject .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlEditClientProject .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = custompipe.indexOf(value);
            custompipe.splice(idIndex, 1);
            //custompipe.pop(value);
            $("#hidEditprojectnameist").val();
            $("#hidEditprojectnameist").val(custompipe);
            var index = EdititemNames.indexOf(value);
            EdititemNames.splice(index,1);
            console.log(EdititemNames);
            if (EdititemNames.length == 0) {
                $("#ddlEditClientProject .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
           /* alert(custompipe);*/
        });
    });

    $("#ddlEditClientProject .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();

        $("#ddlEditClientProject .custom-sel-list li").each(function () {
            console.log("List Data", $(this).attr('value'));
            console.log("arraylist", EdititemNames);
            if (EdititemNames.includes($(this).attr('value'))) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("#ddlEditClientProject .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            //var name = $(this).attr('value');
            if (name.includes(inputName) && !EdititemNames.includes($(this).attr('value'))) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    $('#ddlEditClientProject .multi-input').on('click', function () {
        debugger;
        $("#ddlEditClientProject .custom-sel-list li").each(function () {
            if (EdititemNames.includes($(this).attr('value'))) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });


    

    // End Client project Mapping for Edit

});


