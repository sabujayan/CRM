$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    var createModal = new abp.ModalManager(abp.appPath + 'SalesOrder/CreateDetail');
    var updateModal = new abp.ModalManager(abp.appPath + 'SalesOrder/UpdateDetail');

    var hfSalesOrderId = $("#hfSalesOrderId").val();
    var dataTable = $('#SalesOrderDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.salesOrderDetails.salesOrderDetail.getListBySalesOrder, function () {
                return hfSalesOrderId;
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
                                    visible: function (data) {
                                        return data.status == 1;
                                    },
                                    confirmMessage: function (data) {
                                        return "Are you sure to delete: " + data.record.productName + " ?";
                                    },
                                    action: function (data) {
                                        indo.salesOrderDetails.salesOrderDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfSalesOrderId);
                                                drawDiscAmt(hfSalesOrderId);
                                                drawBeforeTax(hfSalesOrderId);
                                                drawTaxAmount(hfSalesOrderId);
                                                drawTotal(hfSalesOrderId);
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: "Product",
                    data: "productName"
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
                {
                    title: "Quantity",
                    data: "quantity",
                    render: function (data, type, row) {
                        var name = '<span>' + $.fn.dataTable.render.text().display(data) + '</span>'; 
                        name +=
                            '<span class="badge badge-pill badge-info ml-1">' +
                            row.uomName +
                            '</span>';
                        return name;
                    },
                },
                {
                    title: "Uom",
                    data: "uomName"
                },
                {
                    title: "Sub Total",
                    data: "subTotalString"
                },
                {
                    title: "Disc Amt",
                    data: "discAmtString"
                },
                {
                    title: "Before Tax",
                    data: "beforeTaxString"
                },
                {
                    title: "Tax Amt",
                    data: "taxAmountString"
                },
                {
                    title: "Total",
                    data: "totalString"
                }
            ]
        })
    );

    var drawTotal = function (salesOrderId) {
        indo.salesOrders.salesOrder.getSummaryTotalInString(salesOrderId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (salesOrderId) {
        indo.salesOrders.salesOrder.getSummarySubTotalInString(salesOrderId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (salesOrderId) {
        indo.salesOrders.salesOrder.getSummaryDiscAmtInString(salesOrderId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (salesOrderId) {
        indo.salesOrders.salesOrder.getSummaryBeforeTaxInString(salesOrderId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (salesOrderId) {
        indo.salesOrders.salesOrder.getSummaryTaxAmountInString(salesOrderId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfSalesOrderId);
    drawDiscAmt(hfSalesOrderId);
    drawBeforeTax(hfSalesOrderId);
    drawTaxAmount(hfSalesOrderId);
    drawTotal(hfSalesOrderId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfSalesOrderId);
        drawDiscAmt(hfSalesOrderId);
        drawBeforeTax(hfSalesOrderId);
        drawTaxAmount(hfSalesOrderId);
        drawTotal(hfSalesOrderId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfSalesOrderId);
        drawDiscAmt(hfSalesOrderId);
        drawBeforeTax(hfSalesOrderId);
        drawTaxAmount(hfSalesOrderId);
        drawTotal(hfSalesOrderId);
    });

    $('#NewSalesOrderDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            salesOrderId: hfSalesOrderId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfSalesOrderNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
