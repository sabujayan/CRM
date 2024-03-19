$(function () {
    var l = abp.localization.getResource('Indo');
    var globalFilter = "";
    var nameFilter = "";
    var clientFilter = "";
    var startdateFilter = "";
    var enddateFilter = "";
    var estimateFilter = "";
    var technologyFilter = "";
    var sortingColumn = "";
    var sortingValue = "desc";
    var SkipCount = 0;
    var next = 0;
    var MaxResultCount = parseInt($("#itemsPerPage").val()) || 5;
    var totalPages;
    loadProjectData();

    $("#itemsPerPage").on("change", function () {
        MaxResultCount = parseInt($(this).val()) || 5;
        SkipCount = 0;
        loadProjectData();
    });


    $("#previousPage").click(function () {
        if (SkipCount > 0) {
            SkipCount == MaxResultCount;
            SkipCount--;
            loadProjectData();
        }
    });

    $("#nextPage").click(function () {
        if (SkipCount < totalPages) {
            SkipCount++;
            loadProjectData();
            next = 1;
        }
    });

    function loadProjectData() {
        $.ajax({
            url: "api/app/projects/project-list",
            method: "GET",
            data: {
                Filter: globalFilter,
                Sorting: sortingValue,
                sortingColumn: sortingColumn,
                SkipCount: SkipCount,
                MaxResultCount: MaxResultCount,
                nameFilter: nameFilter,
                clientFilter: clientFilter,
                startdateFilter: startdateFilter,
                enddateFilter: enddateFilter,
                technologyFilter: technologyFilter
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
                    updatePagination();
                    $("#currentPage").text("Page " + (SkipCount + 1) + " of " + totalPages);
                    data.items.forEach(function (item) {
                        var row =
                            "<tr>" +
                            "<td>" +
                            item.name +
                            "</td>" +
                            "<td>" +
                            item.clientName +
                            "</td>" +
                            "<td id='tdProjectName' data-row-id=" + i + ">" +
                            "<div class='td-popup'>" +
                            "<div id='divProjectName_" + i + "'>" +
                            item.technologynameist +
                            "</div> " +
                            "<div class='td-sm-popup' id='divProjectDesc_" + i + "' style='display:none;'>" +
                            item.technologyDesc +
                            "</div> " +
                            "</div> " +
                            "</td>" +
                            "<td>" +
                            item.sStartDate +
                            "</td>" +
                            "<td>" +
                            item.sEndDate +
                            "</td>" +
                            "<td class='action-td'>" +
                            '<a href="/Project/Edit?id=' + item.id + '" class="btn btn-sm btn-icon btn-primary" title="Edit"><i class="fas fa-edit"></i></a>' +
                            '<button id="btnDelete" data-user-id="' + item.id + '" data-user-name="' + item.name + '" class="btn btn-sm btn-icon btn-danger ml-2" type="button"><i class="bi bi-trash"></i></button>' +
                            "</td>" +
                            "</tr>";

                        $("#myTable").append(row);
                        i++;
                    });
                    $("#kt_customers_table_wrapper").show();
                    $("#divPaggination").show();
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

    $("#myTable").on('mouseenter', '#tdProjectName', function () {
        var id = $(this).attr("data-row-id");
        $("#divProjectDesc_" + id + "").show();
    });

    $("#myTable").on('mouseleave', '#tdProjectName', function () {
        var id = $(this).attr("data-row-id");
        $("#divProjectDesc_" + id + "").hide();
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
            if (i === SkipCount + 1) {
                li.addClass("active");
                $("#nextPage").addClass("disabled").removeClass("enabled");
            } else {
                link.on("click", function (event) {
                    event.preventDefault();
                    SkipCount = ($(this).text() - 1);
                    loadProjectData();
                });
                $("#nextPage").addClass("enabled").removeClass("disabled");
            }
            link.appendTo(li);
            li.appendTo(paginationEl);
        }
    }

    $("#myInput").on('input', function () {
        globalFilter = $(this).val();
        if (globalFilter != "") {
            SkipCount = 0;
            loadProjectData();
        }
        else {
            globalFilter = "";
            SkipCount = 0;
            loadProjectData();
            $("#kt_customers_table_wrapper").show();
            $("#divPaggination").show();
        }
    });

    $("#nameFilter").on('input', function () {
        nameFilter = $(this).val();
        if (nameFilter != "") {
            SkipCount = 0;
            loadProjectData();
        }
        else {
            nameFilter = "";
            SkipCount = 0;
            loadProjectData();
        }
    });
   

    $("#ddlClient").click(function () {
        var value = $(this).val();
        if (value === "null") {
            $(this).val(value);
            clientFilter = "";
        } else {
            clientFilter = value;
        }
        SkipCount = 0;
        loadProjectData();
    });


    $("#ddlTechnology").click(function () {
        var value = $(this).val();
        if (value === "null") {
            $(this).val(value);
            technologyFilter = "";
        } else {
            technologyFilter = value;
        }
        SkipCount = 0;
        loadProjectData();
    });

    $("#startdateFilter").on('change', function () {
        startdateFilter = $(this).val();
        SkipCount = 0;
        loadProjectData();
    });
    $("#enddateFilter").on('input', function () {
        enddateFilter = $(this).val();
        SkipCount = 0;
        loadProjectData();
    });

    //filter popup open
    $('.filter_btn').click(function () {
        $('.filter-box').fadeToggle();
    });
    $('.filter_close').on('click', function () {
        $('.filter-box').fadeOut();
    });

    //outside click div closed
    $(document).mouseup(function (e) {
        var targetDiv = $('.filter-box, .filter_btn');

        // if the target of the click isn't the container nor a descendant of the container
        if (!targetDiv.is(e.target) && targetDiv.has(e.target).length === 0) {
            $('.filter-box').fadeOut();
        }
    });

    $("#myTable").on('click', '#btnDelete', function () {
        var id = $(this).data('user-id');
        var name = $(this).data('user-name');
        abp.message.confirm('Delete this data: ' + name)
            .then(function (confirmed) {
                if (confirmed) {
                    indo.employees.employee
                        .delete(id)
                        .then(function () {
                            abp.notify.success(l('DeleteSuccess'));
                            window.location.href = "/Employee";
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });
                }
            });

    });

    $("#kt_customers_table th").append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
    $("#kt_customers_table th.sorting_disabled").find('i').remove();


    $("#kt_customers_table th").on('click', function () {
        var columnName = $(this).data('sort');
        if (["Name","ClientName","sStartDate","sEndDate","EstimateHours"].includes(columnName)) {
            if (columnName === sortingColumn) {
                sortingValue = sortingValue === "desc" ? "asc" : "desc";
            } else {
                sortingColumn = columnName;
                sortingValue = "desc";
            }
            loadProjectData();
            if (sortingValue === 'desc') {
                $(this).find('i').remove();
                $(this).append('<i class="fa fa-sort-asc position-relative ml-1" aria-hidden="true"></i>');
            } else {
                $(this).find('i').remove();
                $(this).append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
            }
        }
    });

    $('#btnClear').click(function () {
        $("#nameFilter").val('');
        $("#ddlClient").val('null');
        $("#startdateFilter").val('');
        $("#enddateFilter").val('');
        $("#ddlTechnology").val('null');
        nameFilter = "";
        clientFilter = "";
        technologyFilter = "";
        startdateFilter = "";
        enddateFilter = "";
        SkipCount = 0;
        loadProjectData();
        $("#divPaggination").show();
    });


   
});



