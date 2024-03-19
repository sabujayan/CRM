$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();


    abp.modals.SalesQuotationDetailModal = function () {

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
        viewUrl: abp.appPath + 'SalesQuotation/CreateDetail',
        modalClass: 'SalesQuotationDetailModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'SalesQuotation/UpdateDetail',
        modalClass: 'SalesQuotationDetailModal'
    });

    var hfSalesQuotationId = $("#hfSalesQuotationId").val();
    var dataTable = $('#SalesQuotationDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.salesQuotationDetails.salesQuotationDetail.getListBySalesQuotation, function () {
                return hfSalesQuotationId;
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
                                        indo.salesQuotationDetails.salesQuotationDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfSalesQuotationId);
                                                drawDiscAmt(hfSalesQuotationId);
                                                drawBeforeTax(hfSalesQuotationId);
                                                drawTaxAmount(hfSalesQuotationId);
                                                drawTotal(hfSalesQuotationId);
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

    var drawTotal = function (salesQuotationId) {
        indo.salesQuotations.salesQuotation.getSummaryTotalInString(salesQuotationId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (salesQuotationId) {
        indo.salesQuotations.salesQuotation.getSummarySubTotalInString(salesQuotationId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (salesQuotationId) {
        indo.salesQuotations.salesQuotation.getSummaryDiscAmtInString(salesQuotationId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (salesQuotationId) {
        indo.salesQuotations.salesQuotation.getSummaryBeforeTaxInString(salesQuotationId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (salesQuotationId) {
        indo.salesQuotations.salesQuotation.getSummaryTaxAmountInString(salesQuotationId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfSalesQuotationId);
    drawDiscAmt(hfSalesQuotationId);
    drawBeforeTax(hfSalesQuotationId);
    drawTaxAmount(hfSalesQuotationId);
    drawTotal(hfSalesQuotationId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfSalesQuotationId);
        drawDiscAmt(hfSalesQuotationId);
        drawBeforeTax(hfSalesQuotationId);
        drawTaxAmount(hfSalesQuotationId);
        drawTotal(hfSalesQuotationId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfSalesQuotationId);
        drawDiscAmt(hfSalesQuotationId);
        drawBeforeTax(hfSalesQuotationId);
        drawTaxAmount(hfSalesQuotationId);
        drawTotal(hfSalesQuotationId);
    });

    $('#NewSalesQuotationDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            salesQuotationId: hfSalesQuotationId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfSalesQuotationNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
