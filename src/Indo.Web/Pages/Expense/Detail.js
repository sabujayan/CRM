$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    var createModal = new abp.ModalManager(abp.appPath + 'Expense/CreateDetail');
    var updateModal = new abp.ModalManager(abp.appPath + 'Expense/UpdateDetail');

    var hfExpenseId = $("#hfExpenseId").val();
    var dataTable = $('#ExpenseDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.expenseDetails.expenseDetail.getListByExpense, function () {
                return hfExpenseId;
            }),
            columnDefs: [
                {
                    title: "Actions",
                    rowAction: {
                        items:
                            [
                                {
                                    text: "Edit",
                                    action: function (data) {
                                        updateModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: "Delete",
                                    confirmMessage: function (data) {
                                        return "Are you sure to delete: " + data.record.summaryNote + " ?";
                                    },
                                    action: function (data) {
                                        indo.expenseDetails.expenseDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawTotal($("#hfExpenseId").val());
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: "Summary Note",
                    data: "summaryNote"
                },
                {
                    title: "Price",
                    data: "priceString",
                    render: function (data, type, row) {
                        var name = '<span>' + $.fn.dataTable.render.text().display(data) + '</span>';
                        name +=
                            '<span class="badge badge-pill badge-info ml-1">' +
                            row.currencyName +
                            '</span>';
                        return name;
                    },
                },
            ]
        })
    );

    var drawTotal = function (expenseId) {
        indo.expenses.expense.getSummaryTotalInString(expenseId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    drawTotal(hfExpenseId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawTotal(hfExpenseId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawTotal(hfExpenseId);
    });

    $('#NewExpenseDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            expenseId: hfExpenseId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfExpenseNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };


});
