$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');
    /* load grid once */
    /*loadGrid();*/
    var filterValue = "";
    var nameFilter = "";
    var cityFilter = "";
    var phoneNoFilter = "";
    var positionFilter = "";
    var clientProjectsFilter = "";
    var sortingColumn = "";
    var sortingValue = "desc";
    var SkipCount = 0;
    var MaxResultCount = parseInt($("#itemsPerPage").val()) || 5;
    var totalPages;
    var next = 0;
    
    loadClientData();
    $("#itemsPerPage").on("change", function () {
        MaxResultCount = parseInt($(this).val()) || 5;
        SkipCount = 0;
        loadClientData();
    });
    $("#globalSearch").on("input", function () {
        filterValue = $(this).val();
        if (filterValue != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            filterValue = "";
            SkipCount = 0;
            loadClientData();
            $("#kt_customers_table_wrapper").show();
            $("#divPaggination").show();
        }
    });

    $("#previousPage").click(function () {
        if (SkipCount > 0) {
            SkipCount == MaxResultCount;
            SkipCount--;
            loadClientData();
        }
    });

    $("#nextPage").click(function () {
        if (SkipCount < totalPages) {
            SkipCount++;
            loadClientData();
            next = 1;
        }
    });

    function loadClientData() {
        $.ajax({
            url: "api/app/employee/employee-list",
            method: "GET",
            data: {
                Filter: filterValue,
                Sorting: sortingValue,
                SortingColumn: sortingColumn,
                SkipCount: SkipCount,
                MaxResultCount: MaxResultCount,
                nameFilter: nameFilter,
                phoneNoFilter: phoneNoFilter,
                cityFilter: cityFilter,
                positionFilter: positionFilter,
                clientProjectsFilter: clientProjectsFilter
            },
            success: function (data, textStatus, xhr) {
                var i = 0;
                if (SkipCount === 0) {
                    $("#previousPage").addClass("disabled").removeClass("enabled");
                }
                else {
                    $("#previousPage").addClass("enabled").removeClass("disabled");
                }
                if (data.items.length === 0 && next === 1) {
                    next = 0;
                    return false;
                }
                $("#myTable").empty();
                totalCount = data.totalCount;
                if (totalCount > 0) {
                    totalPages = Math.ceil(totalCount / MaxResultCount);
                    console.log(totalPages);
                    updatePagination();
                    $("#currentPage").text("Page " + (SkipCount + 1) + " of " + totalPages);
                    data.items.forEach(function (item) {
                        var row =
                            "<tr>" +
                            "<td>" +
                            item.name +
                            "</td>" +
                            "<td>" +
                            item.phone +
                            "</td>" +
                            "<td>" +
                            item.city +
                            "</td>" +
                            "<td>" +
                             item.position +
                            "</td>" +
                            "<td>" +
                            item.clientNameList +
                            "</td>" +
                            "<td>" +
                            item.projectNameList +
                            "</td>" +
                            "<td class='action-td'>" +
                            '<a href="/employee/Update?id=' + item.id + '" class="btn btn-sm btn-icon btn-primary" title="Edit"><i class="fas fa-edit"></i></a>' +
                            '<button id="btnDelete"  data-user-id="' + item.id + '" data-user-name="' + item.name + '" class="btn btn-sm btn-icon btn-danger btnDelete ml-2" type="button"><i class="bi bi-trash"></i></button>' +
                            "</td>"
                            "</tr>";

                        $("#myTable").append(row);
                        i++;
                    });
                }
                else {
                    $("#myTable").html("<tr><td colspan='6' class='pt-10 text-center'>No Record Found</td></tr>");
                    $("#divPaggination").hide();
                }
            },
            error: function (xhr, status, error) {
                console.error(error);
            },
        });
    }

    $("#myTable").on('mouseenter', '#tdAddress', function () {
        var id = $(this).attr("data-row-id");
        $("#divAddressDesc_" + id + "").show();
    });

    $("#myTable").on('mouseleave', '#tdAddress', function () {
        var id = $(this).attr("data-row-id");
        $("#divAddressDesc_" + id + "").hide();
    });

    $("#kt_customers_table th").append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
    $("#kt_customers_table th.sorting_disabled").find('i').remove();

    $("#kt_customers_table th").on('click', function () {
        var columnName = $(this).data('sort');

        if (["Name", "Phone", "City", "Position", "DepartmentName"].includes(columnName)) {
            if (columnName === sortingColumn) {
                sortingValue = sortingValue === "desc" ? "asc" : "desc";

            } else {
                sortingColumn = columnName;
                sortingValue = "desc";
            }
            loadClientData();

            if (sortingValue === 'desc') {
                $(this).find('i').remove();
                $(this).append('<i class="fa fa-sort-asc position-relative ml-1" aria-hidden="true"></i>');
            } else {
                $(this).find('i').remove();
                $(this).append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
            }
        }
    });

    $(document).on("change", '#ddlEditProject', function () {
        var selected = $("#ddlEditProject option:selected");    /*Current Selected Value*/
        var message = "";
        var arrSelected = [];      /*Array to store multiple values in stack*/
        selected.each(function () {
            arrSelected.push($(this).val());    /*Stack the Value*/
            message += $(this).val() + ",";
        });
        $("#hidEditprojectlist").val(message)
        return true;
    });

    $('.px-7.py-5').hide();

    $('.btn-close').on('click', function () {
        $('.px-7.py-5').hide();
    });

    $('.btn.btn-light-primary.me-3').click(function () {
        $('.px-7.py-5').show();
    });

    $("#globalSearch").on("input", function () {
        filterValue = $(this).val();
        if (filterValue != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            filterValue = "";
            SkipCount = 0;
            loadClientData();
            $("#kt_customers_table_wrapper").show();
            $("#divPaggination").show();
        }
    });



    $("#nameFilter").on("input", function () {
        nameFilter = $(this).val();
        if (nameFilter != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            nameFilter = "";
            SkipCount = 0;
            loadClientData();
        }
    });
    $("#cityFilter").on("input", function () {
        cityFilter = $(this).val();
        if (cityFilter != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            cityFilter = "";
            SkipCount = 0;
            loadClientData();
        }
    });
    $("#phoneNoFilter").on("input", function () {
        phoneNoFilter = $(this).val();
        if (phoneNoFilter != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            phoneNoFilter = "";
            SkipCount = 0;
            loadClientData();
        }
    });


    $("#positionFilter").on("input", function () {
        positionFilter = $(this).val();
        if (positionFilter != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            positionFilter = "";
            SkipCount = 0;
            loadClientData();
        }
    });


    $("#clientProjectsFilter").click(function () {
        var value = $(this).val();
        if (value === "null") {
            $(this).val(value);
            clientProjectsFilter = "";
        } else {
            clientProjectsFilter = value;
        }
        SkipCount = 0;
        loadClientData();
    });

    $('#btnClear').click(function () {
        $("#nameFilter").val('');
        $("#cityFilter").val('');
        $("#phoneNoFilter").val('');
        $("#positionFilter").val('');
        $("#clientProjectsFilter").val('null');
        nameFilter = "";
        positionFilter = "";
        phoneNoFilter = "";
        cityFilter = "";
        clientProjectsFilter = "";
        SkipCount = 0;
        loadClientData();
        $("#divPaggination").show();
    });
    function updatePagination() {
        var paginationEl = $("#pagination");
        paginationEl.empty();
        for (var i = 1; i <= totalPages; i++) {
            var li = $("<li>").addClass("page-item");
            var link = $("<a>")
                .addClass("page-link")
                .text(i)
                .attr("href", "#");
            if (i === (SkipCount + 1)) {
                li.addClass("active");
                $("#nextPage").addClass("disabled").removeClass("enabled");
            } else {
                link.on("click", function (event) {
                    event.preventDefault();
                    SkipCount = ($(this).text() - 1);
                    loadClientData();
                });
                $("#nextPage").addClass("enabled").removeClass("disabled");
            }
            link.appendTo(li);
            li.appendTo(paginationEl);
        }
    }

    $("#myTable").on('click', '#btnDelete', function () {
        var id = $(this).attr("data-user-id");
        var name = $(this).attr("data-user-name");
        abp.message.confirm("Are you sure you want to delete " + name + "?", function (isConfirmed) {
            if (isConfirmed) {
                indo.employees.employee.delete(id)
                    .then(function () {
                        abp.message.success("Successfully Deleted");
                        window.location.href = "/Employee";
                    })
                    .catch(function (error) {
                        abp.message.error("Error: " + error.message + " " + error.details);
                    });
            }
        });
    });
});
