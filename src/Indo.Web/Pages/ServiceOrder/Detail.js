$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    var createModal = new abp.ModalManager(abp.appPath + 'ServiceOrder/CreateDetail');
    var updateModal = new abp.ModalManager(abp.appPath + 'ServiceOrder/UpdateDetail');

    var hfServiceOrderId = $("#hfServiceOrderId").val();
    var dataTable = $('#ServiceOrderDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.serviceOrderDetails.serviceOrderDetail.getListByServiceOrder, function () {
                return hfServiceOrderId;
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
                                        return "Are you sure to delete: " + data.record.serviceName + " ?";
                                    },
                                    action: function (data) {
                                        indo.serviceOrderDetails.serviceOrderDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfServiceOrderId);
                                                drawDiscAmt(hfServiceOrderId);
                                                drawBeforeTax(hfServiceOrderId);
                                                drawTaxAmount(hfServiceOrderId);
                                                drawTotal(hfServiceOrderId);
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: "Service",
                    data: "serviceName"
                },
                {
                    title: "Price",
                    data: "priceString",
                    render: function (data, type, row) {
                        var name = '<span>' + $.fn.dataTable.render.text().display(data) + '</span>'; //prevent against possible XSS
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

    var drawTotal = function (serviceOrderId) {
        indo.serviceOrders.serviceOrder.getSummaryTotalInString(serviceOrderId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (serviceOrderId) {
        indo.serviceOrders.serviceOrder.getSummarySubTotalInString(serviceOrderId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (serviceOrderId) {
        indo.serviceOrders.serviceOrder.getSummaryDiscAmtInString(serviceOrderId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (serviceOrderId) {
        indo.serviceOrders.serviceOrder.getSummaryBeforeTaxInString(serviceOrderId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (serviceOrderId) {
        indo.serviceOrders.serviceOrder.getSummaryTaxAmountInString(serviceOrderId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfServiceOrderId);
    drawDiscAmt(hfServiceOrderId);
    drawBeforeTax(hfServiceOrderId);
    drawTaxAmount(hfServiceOrderId);
    drawTotal(hfServiceOrderId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfServiceOrderId);
        drawDiscAmt(hfServiceOrderId);
        drawBeforeTax(hfServiceOrderId);
        drawTaxAmount(hfServiceOrderId);
        drawTotal(hfServiceOrderId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfServiceOrderId);
        drawDiscAmt(hfServiceOrderId);
        drawBeforeTax(hfServiceOrderId);
        drawTaxAmount(hfServiceOrderId);
        drawTotal(hfServiceOrderId);
    });

    $('#NewServiceOrderDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            serviceOrderId: hfServiceOrderId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfServiceOrderNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
