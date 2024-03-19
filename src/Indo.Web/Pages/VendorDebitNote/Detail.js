﻿$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    abp.modals.VendorDebitNoteDetailModal = function () {

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
        viewUrl: abp.appPath + 'VendorDebitNote/CreateDetail',
        modalClass: 'VendorDebitNoteDetailModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'VendorDebitNote/UpdateDetail',
        modalClass: 'VendorDebitNoteDetailModal'
    });

    var hfVendorDebitNoteId = $("#hfVendorDebitNoteId").val();
    var dataTable = $('#VendorDebitNoteDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.vendorDebitNoteDetails.vendorDebitNoteDetail.getListByVendorDebitNote, function () {
                return hfVendorDebitNoteId;
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
                                        indo.vendorDebitNoteDetails.vendorDebitNoteDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfVendorDebitNoteId);
                                                drawDiscAmt(hfVendorDebitNoteId);
                                                drawBeforeTax(hfVendorDebitNoteId);
                                                drawTaxAmount(hfVendorDebitNoteId);
                                                drawTotal(hfVendorDebitNoteId);
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

    var drawTotal = function (vendorDebitNoteId) {
        indo.vendorDebitNotes.vendorDebitNote.getSummaryTotalInString(vendorDebitNoteId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (vendorDebitNoteId) {
        indo.vendorDebitNotes.vendorDebitNote.getSummarySubTotalInString(vendorDebitNoteId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (vendorDebitNoteId) {
        indo.vendorDebitNotes.vendorDebitNote.getSummaryDiscAmtInString(vendorDebitNoteId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (vendorDebitNoteId) {
        indo.vendorDebitNotes.vendorDebitNote.getSummaryBeforeTaxInString(vendorDebitNoteId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (vendorDebitNoteId) {
        indo.vendorDebitNotes.vendorDebitNote.getSummaryTaxAmountInString(vendorDebitNoteId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfVendorDebitNoteId);
    drawDiscAmt(hfVendorDebitNoteId);
    drawBeforeTax(hfVendorDebitNoteId);
    drawTaxAmount(hfVendorDebitNoteId);
    drawTotal(hfVendorDebitNoteId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfVendorDebitNoteId);
        drawDiscAmt(hfVendorDebitNoteId);
        drawBeforeTax(hfVendorDebitNoteId);
        drawTaxAmount(hfVendorDebitNoteId);
        drawTotal(hfVendorDebitNoteId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfVendorDebitNoteId);
        drawDiscAmt(hfVendorDebitNoteId);
        drawBeforeTax(hfVendorDebitNoteId);
        drawTaxAmount(hfVendorDebitNoteId);
        drawTotal(hfVendorDebitNoteId);
    });

    $('#NewVendorDebitNoteDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            vendorDebitNoteId: hfVendorDebitNoteId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfVendorDebitNoteNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
