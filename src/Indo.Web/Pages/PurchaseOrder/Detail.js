$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    var createModal = new abp.ModalManager(abp.appPath + 'PurchaseOrder/CreateDetail');
    var updateModal = new abp.ModalManager(abp.appPath + 'PurchaseOrder/UpdateDetail');

    var hfPurchaseOrderId = $("#hfPurchaseOrderId").val();
    var dataTable = $('#PurchaseOrderDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.purchaseOrderDetails.purchaseOrderDetail.getListByPurchaseOrder, function () {
                return hfPurchaseOrderId;
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
                                        indo.purchaseOrderDetails.purchaseOrderDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfPurchaseOrderId);
                                                drawDiscAmt(hfPurchaseOrderId);
                                                drawBeforeTax(hfPurchaseOrderId);
                                                drawTaxAmount(hfPurchaseOrderId);
                                                drawTotal(hfPurchaseOrderId);
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

    var drawTotal = function (purchaseOrderId) {
        indo.purchaseOrders.purchaseOrder.getSummaryTotalInString(purchaseOrderId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (purchaseOrderId) {
        indo.purchaseOrders.purchaseOrder.getSummarySubTotalInString(purchaseOrderId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (purchaseOrderId) {
        indo.purchaseOrders.purchaseOrder.getSummaryDiscAmtInString(purchaseOrderId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (purchaseOrderId) {
        indo.purchaseOrders.purchaseOrder.getSummaryBeforeTaxInString(purchaseOrderId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (purchaseOrderId) {
        indo.purchaseOrders.purchaseOrder.getSummaryTaxAmountInString(purchaseOrderId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfPurchaseOrderId);
    drawDiscAmt(hfPurchaseOrderId);
    drawBeforeTax(hfPurchaseOrderId);
    drawTaxAmount(hfPurchaseOrderId);
    drawTotal(hfPurchaseOrderId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfPurchaseOrderId);
        drawDiscAmt(hfPurchaseOrderId);
        drawBeforeTax(hfPurchaseOrderId);
        drawTaxAmount(hfPurchaseOrderId);
        drawTotal(hfPurchaseOrderId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfPurchaseOrderId);
        drawDiscAmt(hfPurchaseOrderId);
        drawBeforeTax(hfPurchaseOrderId);
        drawTaxAmount(hfPurchaseOrderId);
        drawTotal(hfPurchaseOrderId);
    });

    $('#NewPurchaseOrderDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            purchaseOrderId: hfPurchaseOrderId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfPurchaseOrderNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
