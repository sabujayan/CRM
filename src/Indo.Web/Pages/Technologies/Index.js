$(function () {
    var l = abp.localization.getResource('Indo');

    var filterValue = "";
    var nameFilter = "";
    var descriptionFilter = "";
    var sortingColumn = "";
    var sortingValue = "desc";
    var SkipCount = 0;
    var MaxResultCount = parseInt($("#itemsPerPage").val()) || 5;
    var totalPages;
    var next = 0;

    loadTechnologyData();

    $("#itemsPerPage").on("change", function () {
        MaxResultCount = parseInt($(this).val()) || 5;
        SkipCount = 0;
        loadTechnologyData();
    });

    $("#previousPage").click(function () {
        if (SkipCount > 0) {
            SkipCount == MaxResultCount;
            SkipCount--;
            loadTechnologyData();
        }
    });

    $("#nextPage").click(function () {
        if (SkipCount < totalPages) {
            SkipCount++;
            loadTechnologyData();
            next = 1;
        }
    });

    function loadTechnologyData() {
        $.ajax({
            url: "api/app/technology/technology-list",
            method: "GET",
            data: {
                Filter: filterValue,
                Sorting: sortingValue,
                SortingColumn: sortingColumn,
                SkipCount: SkipCount,
                MaxResultCount: MaxResultCount,
                nameFilter: nameFilter,
                descriptionFilter: descriptionFilter
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
                            item.description +
                            "</td>" +
                            "<td class='action-td '>" +
                            '<a href="/Technologies/Edit?id=' + item.id + '" class="btn btn-sm btn-icon btn-primary" title="Edit"><i class="fas fa-edit"></i></a>' +
                            '<button id="btnDelete"  data-user-id="' + item.id + '" data-user-name="' + item.name + '" class="btn btn-sm btn-icon btn-danger btnDelete ml-2" type="button"><i class="bi bi-trash"></i></button>' +
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
              
            },
        });
    }

    $("#kt_customers_table th").append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
    $("#kt_customers_table th.sorting_disabled").find('i').remove();

    $("#kt_customers_table th").on('click', function () {
        var columnName = $(this).data('sort');
        if (["Name","Description"].includes(columnName)) {
            if (columnName === sortingColumn) {
                sortingValue = sortingValue === "desc" ? "asc" : "desc";
            } else {
                sortingColumn = columnName;
                sortingValue = "desc";
            }

            loadTechnologyData();
            if (sortingValue === 'desc') {
                $(this).find('i').remove();
                $(this).append('<i class="fa fa-sort-asc position-relative ml-1" aria-hidden="true"></i>');
                $(this).siblings().find('i').remove();
            } else {
                $(this).find('i').remove();
                $(this).append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
                $(this).siblings().find('i').remove();
            }
        }
    });

    $("#myTable").on('click', '#btnDelete', function () {
        var id = $(this).attr("data-user-id");
        var name = $(this).attr("data-user-name");
        abp.message.confirm("Are you sure you want to delete " + name + "?", function (isConfirmed) {
            if (isConfirmed) {
                indo.technologies.technology.delete(id)
                    .then(function () {
                        abp.message.success("Successfully Deleted");
                        window.location.href = "/Technologies";
                    })
                    .catch(function (error) {
                        abp.message.error("Error: " + error.message + " " + error.details);
                    });
            }
        });
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
                    loadTechnologyData();
                });
                $("#nextPage").addClass("enabled").removeClass("disabled");
            }
            link.appendTo(li);
            li.appendTo(paginationEl);
        }
    }


   

    $("#globalSearch").on("input", function () {
        filterValue = $(this).val();
        if (filterValue != "") {
            SkipCount = 0;
            loadTechnologyData();
        }
        else {
            filterValue = "";
            SkipCount = 0;
            loadTechnologyData();
          
        }
    });



    $("#nameFilter").on("input", function () {
        nameFilter = $(this).val();
        if (nameFilter != "") {
            SkipCount = 0;
            loadTechnologyData();
        }
        else {
            nameFilter = "";
            SkipCount = 0;
            loadTechnologyData();
        }
    });
    $("#descriptionFilter").on("input", function () {
        descriptionFilter = $(this).val();
        if (descriptionFilter != "") {
            SkipCount = 0;
            loadTechnologyData();
        }
        else {
            nameFilter = "";
            SkipCount = 0;
            loadTechnologyData();
        }
    });
   
 
    $('#btnClear').click(function () {
        $("#nameFilter").val('');
        $("#descriptionFilter").val('');
        nameFilter = "";
        descriptionFilter = "";
        SkipCount = 0;
        loadTechnologyData();
   
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
            //targetDiv.fadeOut();
            $('.filter-box').fadeOut();
        }
    });

});

