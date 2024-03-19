$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    abp.modals.CustomerInvoiceDetailModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'CustomerInvoice/CreateDetail',
        modalClass: 'CustomerInvoiceDetailModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'CustomerInvoice/UpdateDetail',
        modalClass: 'CustomerInvoiceDetailModal'
    });

    var hfCustomerInvoiceId = $("#hfCustomerInvoiceId").val();
    var dataTable = $('#CustomerInvoiceDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.customerInvoiceDetails.customerInvoiceDetail.getListByCustomerInvoice, function () {
                return hfCustomerInvoiceId;
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
                                        indo.customerInvoiceDetails.customerInvoiceDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfCustomerInvoiceId);
                                                drawDiscAmt(hfCustomerInvoiceId);
                                                drawBeforeTax(hfCustomerInvoiceId);
                                                drawTaxAmount(hfCustomerInvoiceId);
                                                drawTotal(hfCustomerInvoiceId);
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: "Item",
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

    var drawTotal = function (customerInvoiceId) {
        indo.customerInvoices.customerInvoice.getSummaryTotalInString(customerInvoiceId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (customerInvoiceId) {
        indo.customerInvoices.customerInvoice.getSummarySubTotalInString(customerInvoiceId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (customerInvoiceId) {
        indo.customerInvoices.customerInvoice.getSummaryDiscAmtInString(customerInvoiceId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (customerInvoiceId) {
        indo.customerInvoices.customerInvoice.getSummaryBeforeTaxInString(customerInvoiceId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (customerInvoiceId) {
        indo.customerInvoices.customerInvoice.getSummaryTaxAmountInString(customerInvoiceId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfCustomerInvoiceId);
    drawDiscAmt(hfCustomerInvoiceId);
    drawBeforeTax(hfCustomerInvoiceId);
    drawTaxAmount(hfCustomerInvoiceId);
    drawTotal(hfCustomerInvoiceId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfCustomerInvoiceId);
        drawDiscAmt(hfCustomerInvoiceId);
        drawBeforeTax(hfCustomerInvoiceId);
        drawTaxAmount(hfCustomerInvoiceId);
        drawTotal(hfCustomerInvoiceId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfCustomerInvoiceId);
        drawDiscAmt(hfCustomerInvoiceId);
        drawBeforeTax(hfCustomerInvoiceId);
        drawTaxAmount(hfCustomerInvoiceId);
        drawTotal(hfCustomerInvoiceId);
    });

    $('#NewCustomerInvoiceDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            customerInvoiceId: hfCustomerInvoiceId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfCustomerInvoiceNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
