$(function () {

    var l = abp.localization.getResource('Indo');

    function updateSendButtonState() {
        var templateId = $("#hidEmailInformationId").val();
        var isEmployeesSelected = pipe.length > 0;

        if (templateId === "00000000-0000-0000-0000-000000000000" || pipe.length === 0) {
            $("#btnSend").prop('disabled', true);
        } else {
            $("#btnSend").prop('disabled', false);
        }
    }

    $("#btnSend").prop('disabled', true);

    //
    $("#Template_Id").change(function () {
        var selectedParentId = $(this).children("option:selected").val();
        $("#hidEmailInformationId").val(selectedParentId);
        updateSendButtonState();
    });

    //
	var pipe = [];
	var itemNames = [];

	$("#ddlEmployees .custom-selectsto").click(function () {
		var projectcount = $("#hidEmployeescount").val();
		if (projectcount == 0) {
			$("#spRecord").show();
		}
		else {
			$(this).children(".multi-input-to").focus();
			$(this).siblings(".custom-sel-list-to").toggle();
			$("#spRecord").hide();
		}
	});


	$("#ddlEmployees .custom-sel-list-to").hide();

	$(document).mouseup(function (e) {
		var container1 = $(".custom-sel-container");
		var list = $(".custom-sel-list-to");
		if (!container1.is(e.target) && container1.has(e.target).length === 0) {
			list.hide();
			$(".multi-input-to").val("");
			$("#spRecord").hide();
		}
	});

	$("#ddlEmployees").mouseleave(function () {
		$("#spRecord").hide();
	});

	$("#ddlEmployees .custom-sel-list-to li").click(function () {
		var removeicon = '<span class="removeicon">x</span>';
		pipe.push($(this).attr('value'));
		$(this).clone().append(removeicon).appendTo($(this).parents("#ddlEmployees").find(".custom-selected"));
		$(this).addClass('hideitem');
		$("#hidemployeenameist").val();
		$("#hidemployeenameist").val(pipe);
		var name = $(this).text();
		itemNames.push(name);
        $("#ddlEmployees .multi-input-to").val("");
        updateSendButtonState();
	});

	$(function () {
		$(document).on("click", "#ddlEmployees .removeicon", function (e) {
			let value = $(this).parents('li').attr('value');
			$('#ddlEmployees .custom-sel-list-to li[ value=' + value + ']').removeClass('hideitem');
			$(this).parents('li').remove();
			var idIndex = pipe.indexOf(value);
			pipe.splice(idIndex, 1);
			$("#hidemployeenameist").val();
			$("#hidemployeenameist").val(pipe);
			var name = $(this).parents("#ddlEmployees").find(".custom-selected").text().slice(0, -1);
			var selectedValue = $(this).parents('li').text().slice(0, -1);
			var index = itemNames.indexOf(selectedValue);
			itemNames.splice(index, 1);
			if (itemNames.length == 0) {
				$("#ddlEmployees .custom-sel-list-to li").each(function () {
					$(this).show();
				});
            }
            updateSendButtonState();
		});
	});

	$('#ddlEmployees .multi-input-to').on('click', function () {
		$("#ddlEmployees .custom-sel-list-to li").each(function () {
			if (itemNames.includes($(this).text())) {
				$(this).hide();
			} else {
				$(this).show();
			}
		});
	});

	$("#ddlEmployees .multi-input-to").on("input", function () {
		var inputName = $(this).val().trim().toLowerCase();
		var matchingItemsExist = false;
		$("#ddlEmployees .custom-sel-list-to li").each(function () {
			if (itemNames.includes($(this).text())) {
				$(this).hide();

			} else {
				$(this).show();
			}
		});

		$("#ddlEmployees .custom-sel-list-to li").each(function () {
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
	//
    var pipe1 = [];
    var itemNames1 = [];


    $("#ddlEmployeescc .custom-selects").click(function () {
        var projectcount = $("#hidEmployeescountcc").val();
        if (projectcount == 0) {
            $("#spRecordcc").show();
        }
        else {
            $(this).children(".multi-input").focus();
            $(this).siblings(".custom-sel-list").toggle();
            $("#spRecordcc").hide();
        }
    });


    $("#ddlEmployeescc .custom-sel-list").hide();

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
            $(".multi-input").val("");
            $("#spRecordcc").hide();
        }
    });

    $("#ddlEmployeescc").mouseleave(function () {
        $("#spRecordcc").hide();
    });

    $("#ddlEmployeescc .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe1.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlEmployeescc").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidemployeenameistcc").val();
        $("#hidemployeenameistcc").val(pipe1);
        var name = $(this).text();
        itemNames1.push(name);
        $("#ddlEmployeescc .multi-input").val("");
    });

    $(function () {
        $(document).on("click", "#ddlEmployeescc .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlEmployeescc .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe1.indexOf(value);
            pipe1.splice(idIndex, 1);
            $("#hidemployeenameistcc").val();
            $("#hidemployeenameistcc").val(pipe1);
            var name = $(this).parents("#ddlEmployeescc").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames1.indexOf(selectedValue);
            itemNames1.splice(index, 1);
            if (itemNames1.length == 0) {
                $("#ddlEmployeescc .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
        });
    });

    $('#ddlEmployeescc .multi-input').on('click', function () {
        $("#ddlEmployeescc .custom-sel-list li").each(function () {
            if (itemNames1.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $("#ddlEmployeescc .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;
        $("#ddlEmployeescc .custom-sel-list li").each(function () {
            if (itemNames1.includes($(this).text())) {
                $(this).hide();

            } else {
                $(this).show();
            }
        });

        $("#ddlEmployeescc .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !itemNames1.includes($(this).text())) {
                $(this).show();
                matchingItemsExist = true;
            } else {
                $(this).hide();
            }
        });
        if (!matchingItemsExist) {
            $("#spRecordcc").show();
        } else {
            $("#spRecordcc").hide();
        }
    });
    //
    var pipe2 = [];
    var itemNames2 = [];


    $("#ddlEmployeesbcc .custom-selects").click(function () {
        var projectcount = $("#hidEmployeescountbcc").val();
        if (projectcount == 0) {
            $("#spRecordbcc").show();
        }
        else {
            $(this).children(".multi-input").focus();
            $(this).siblings(".custom-sel-list").toggle();
            $("#spRecordbcc").hide();
        }
    });


    $("#ddlEmployeesbcc .custom-sel-list").hide();

    $(document).mouseup(function (e) {
        var container1 = $(".custom-sel-container");
        var list = $(".custom-sel-list");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            list.hide();
            $(".multi-input").val("");
            $("#spRecordbcc").hide();
        }
    });

    $("#ddlEmployeesbcc").mouseleave(function () {
        $("#spRecordbcc").hide();
    });

    $("#ddlEmployeesbcc .custom-sel-list li").click(function () {
        var removeicon = '<span class="removeicon">x</span>';
        pipe2.push($(this).attr('value'));
        $(this).clone().append(removeicon).appendTo($(this).parents("#ddlEmployeesbcc").find(".custom-selected"));
        $(this).addClass('hideitem');
        $("#hidemployeenameistbcc").val();
        $("#hidemployeenameistbcc").val(pipe2);
        var name = $(this).text();
        itemNames2.push(name);
        $("#ddlEmployeesbcc .multi-input").val("");
    });

    $(function () {
        $(document).on("click", "#ddlEmployeesbcc .removeicon", function (e) {
            let value = $(this).parents('li').attr('value');
            $('#ddlEmployeesbcc .custom-sel-list li[ value=' + value + ']').removeClass('hideitem');
            $(this).parents('li').remove();
            var idIndex = pipe2.indexOf(value);
            pipe2.splice(idIndex, 1);
            $("#hidemployeenameistbcc").val();
            $("#hidemployeenameistbcc").val(pipe2);
            var name = $(this).parents("#ddlEmployeesbcc").find(".custom-selected").text().slice(0, -1);
            var selectedValue = $(this).parents('li').text().slice(0, -1);
            var index = itemNames2.indexOf(selectedValue);
            itemNames2.splice(index, 1);
            if (itemNames2.length == 0) {
                $("#ddlEmployeesbcc .custom-sel-list li").each(function () {
                    $(this).show();
                });
            }
        });
    });

    $('#ddlEmployeesbcc .multi-input').on('click', function () {
        $("#ddlEmployeesbcc .custom-sel-list li").each(function () {
            if (itemNames2.includes($(this).text())) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    $("#ddlEmployeesbcc .multi-input").on("input", function () {
        var inputName = $(this).val().trim().toLowerCase();
        var matchingItemsExist = false;
        $("#ddlEmployeesbcc .custom-sel-list li").each(function () {
            if (itemNames2.includes($(this).text())) {
                $(this).hide();

            } else {
                $(this).show();
            }
        });

        $("#ddlEmployeesbcc .custom-sel-list li").each(function () {
            var name = $(this).text().toLowerCase();
            if (name.includes(inputName) && !itemNames2.includes($(this).text())) {
                $(this).show();
                matchingItemsExist = true;
            } else {
                $(this).hide();
            }
        });
        if (!matchingItemsExist) {
            $("#spRecordbcc").show();
        } else {
            $("#spRecordbcc").hide();
        }

    });

    //
    $("#btnSend").click(function () {
        var emailData = {
            To: $("#hidemployeenameist").val().split(","),
            Cc: $("#hidemployeenameistcc").val().split(","),
            Bcc: $("#hidemployeenameistbcc").val().split(","),
            TemplateId: $("#hidEmailInformationId").val()
        };

        $.ajax({
            url: "api/app/email-information/send-email",
            type: "POST",
            data: JSON.stringify(emailData),
            contentType: "application/json",
            beforeSend: function () {
                $("#btnSend").addClass("kt-spinner kt-spinner--v2 kt-spinner--sm kt-spinner--primary");
            },
            success: function () {
                swal({
                    title: "Success",
                    text: "Email sent successfully.",
                    icon: "success",
                    buttons: false,
                    timer: 1000
                }).then(function () {
                    window.location.href = "/EmailInformation";
                });
            },
            error: function (xhr, textStatus, errorThrown) {
                var errorMessage = xhr.responseText || "Failed to send email.";
                swal("Error", errorMessage, "error");
            },
            complete: function () {
                $("#btnSend").removeClass("kt-spinner kt-spinner--v2 kt-spinner--sm kt-spinner--primary");
            }
        });
    });

});