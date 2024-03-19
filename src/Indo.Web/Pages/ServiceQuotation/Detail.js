$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    abp.modals.ServiceQuotationDetailModal = function () {

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
        viewUrl: abp.appPath + 'ServiceQuotation/CreateDetail',
        modalClass: 'ServiceQuotationDetailModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'ServiceQuotation/UpdateDetail',
        modalClass: 'ServiceQuotationDetailModal'
    });

    var hfServiceQuotationId = $("#hfServiceQuotationId").val();
    var dataTable = $('#ServiceQuotationDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.serviceQuotationDetails.serviceQuotationDetail.getListByServiceQuotation, function () {
                return hfServiceQuotationId;
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
                                        indo.serviceQuotationDetails.serviceQuotationDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfServiceQuotationId);
                                                drawDiscAmt(hfServiceQuotationId);
                                                drawBeforeTax(hfServiceQuotationId);
                                                drawTaxAmount(hfServiceQuotationId);
                                                drawTotal(hfServiceQuotationId);
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

    var drawTotal = function (serviceQuotationId) {
        indo.serviceQuotations.serviceQuotation.getSummaryTotalInString(serviceQuotationId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (serviceQuotationId) {
        indo.serviceQuotations.serviceQuotation.getSummarySubTotalInString(serviceQuotationId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (serviceQuotationId) {
        indo.serviceQuotations.serviceQuotation.getSummaryDiscAmtInString(serviceQuotationId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (serviceQuotationId) {
        indo.serviceQuotations.serviceQuotation.getSummaryBeforeTaxInString(serviceQuotationId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (serviceQuotationId) {
        indo.serviceQuotations.serviceQuotation.getSummaryTaxAmountInString(serviceQuotationId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfServiceQuotationId);
    drawDiscAmt(hfServiceQuotationId);
    drawBeforeTax(hfServiceQuotationId);
    drawTaxAmount(hfServiceQuotationId);
    drawTotal(hfServiceQuotationId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfServiceQuotationId);
        drawDiscAmt(hfServiceQuotationId);
        drawBeforeTax(hfServiceQuotationId);
        drawTaxAmount(hfServiceQuotationId);
        drawTotal(hfServiceQuotationId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfServiceQuotationId);
        drawDiscAmt(hfServiceQuotationId);
        drawBeforeTax(hfServiceQuotationId);
        drawTaxAmount(hfServiceQuotationId);
        drawTotal(hfServiceQuotationId);
    });

    $('#NewServiceQuotationDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            serviceQuotationId: hfServiceQuotationId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfServiceQuotationNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
