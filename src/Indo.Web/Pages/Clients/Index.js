$(function () {
    var l = abp.localization.getResource('Indo');

    var filterValue = "";
    var nameFilter = "";
    var emailFilter = "";
    var phoneNoFilter = "";
    var addressFilter = "";
    var clientProjectsFilter = "";
    var sortingColumn = "";
    var sortingValue = "desc";
    var SkipCount = 0;
    var MaxResultCount = parseInt($("#itemsPerPage").val()) || 5;
    var totalPages;
    var next=0;

    loadClientData();
  

    $("#itemsPerPage").on("change", function () {
        MaxResultCount = parseInt($(this).val()) || 5;
        SkipCount = 0;
        loadClientData();
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
            url: "api/app/clients/client-list",
            method: "GET",
            data: {
                Filter: filterValue,
                Sorting: sortingValue,
                SortingColumn: sortingColumn,
                SkipCount: SkipCount,
                MaxResultCount: MaxResultCount,
                nameFilter: nameFilter,
                emailFilter: emailFilter,
                phoneNoFilter: phoneNoFilter,
                addressFilter: addressFilter,
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
                    updatePagination();
                    $("#currentPage").text("Page " + (SkipCount + 1) + " of " + totalPages);
                    data.items.forEach(function (item) {
                        var row =
                            "<tr>" +
                            "<td>" +
                            item.name +
                            "</td>" +
                            "<td>" +
                            item.email +
                            "</td>" +
                            "<td>" +
                            item.phoneNumber +
                            "</td>" +
                            "<td id='tdAddress' data-row-id=" + i + ">" +
                            "<div class='td-popup'>" +
                            "<div id='divAddress_" + i + "'>" +
                            item.address +
                            "</div> " +
                            "<div class='td-sm-popup' id='divAddressDesc_" + i + "' style='display:none;'>" +
                            item.addressDesc +
                            "</div> " +
                            "</div> " +
                            "</td>" +
                            "<td id='tdProjectName' data-row-id=" + i + ">" +
                            "<div class='td-popup'>" +
                            "<div id='divProjectName_" + i + "'>" +
                            item.projectnameist +
                            "</div> " +
                            "<div class='td-sm-popup' id='divProjectDesc_" + i + "' style='display:none;'>" +
                            item.projectDesclist +
                            "</div> " +
                            "</div> " +
                            "</td>" +
                            "<td class='action-td'>" +
                            '<a href="/Clients/Edit?id=' + item.id + '" class="btn btn-sm btn-icon btn-primary" title="Edit"><i class="fas fa-edit"></i></a>' +
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


    $("#myTable").on('mouseenter', '#tdProjectName', function () {
        var id = $(this).attr("data-row-id");
        $("#divProjectDesc_" + id + "").show();
    });

    $("#myTable").on('mouseleave', '#tdProjectName', function () {
        var id = $(this).attr("data-row-id");
        $("#divProjectDesc_" + id + "").hide();
    });



    $("#kt_customers_table th").append('<i class="fa fa-sort-desc position-relative ml-1" aria-hidden="true"></i>');
    $("#kt_customers_table th.sorting_disabled").find('i').remove();

    $("#kt_customers_table th").on('click', function () {
        var columnName = $(this).data('sort');

        if (["Name", "Email", "PhoneNumber", "Address"].includes(columnName)) {
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

    $("#myTable").on('click', '#btnDelete', function () {
        var id = $(this).attr("data-user-id");
        var name = $(this).attr("data-user-name");
        abp.message.confirm("Are you sure you want to delete " + name + "?", function (isConfirmed) {
            if (isConfirmed) {
                indo.clientes.clients.delete(id)
                    .then(function () {
                        abp.message.success("Successfully Deleted");
                        window.location.href = "/Clients";
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
                    loadClientData();
                });
                $("#nextPage").addClass("enabled").removeClass("disabled");
            }
            link.appendTo(li);
            li.appendTo(paginationEl);
        }
    }



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

    $("#globalSearch").on("input", function () {
        filterValue = $(this).val();
        if ($(this).val().length > 0) {
            filterValue = $(this).val();
        }
        else {
            filterValue = "";
        }
        SkipCount = 0;
        loadClientData();
     
       
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
    $("#emailFilter").on("input", function () {
        emailFilter = $(this).val();
        if (emailFilter != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            emailFilter = "";
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


    $("#addressFilter").on("input", function () {
        addressFilter = $(this).val();
        if (addressFilter != "") {
            SkipCount = 0;
            loadClientData();
        }
        else {
            addressFilter = "";
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
        $("#emailFilter").val('');
        $("#phoneNoFilter").val('');
        $("#addressFilter").val('');
        $("#addressFilter").val('');
        $("#clientProjectsFilter").val('null');
         nameFilter = "";
         emailFilter = "";
         phoneNoFilter = "";
         addressFilter = "";
         clientProjectsFilter = "";
        SkipCount = 0;
        loadClientData();
        $("#divPaggination").show();
    });
});

