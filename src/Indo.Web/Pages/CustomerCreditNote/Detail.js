$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    abp.modals.CustomerCreditNoteDetailModal = function () {

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
        viewUrl: abp.appPath + 'CustomerCreditNote/CreateDetail',
        modalClass: 'CustomerCreditNoteDetailModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'CustomerCreditNote/UpdateDetail',
        modalClass: 'CustomerCreditNoteDetailModal'
    });

    var hfCustomerCreditNoteId = $("#hfCustomerCreditNoteId").val();
    var dataTable = $('#CustomerCreditNoteDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.customerCreditNoteDetails.customerCreditNoteDetail.getListByCustomerCreditNote, function () {
                return hfCustomerCreditNoteId;
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
                                        indo.customerCreditNoteDetails.customerCreditNoteDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfCustomerCreditNoteId);
                                                drawDiscAmt(hfCustomerCreditNoteId);
                                                drawBeforeTax(hfCustomerCreditNoteId);
                                                drawTaxAmount(hfCustomerCreditNoteId);
                                                drawTotal(hfCustomerCreditNoteId);
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

    var drawTotal = function (customerCreditNoteId) {
        indo.customerCreditNotes.customerCreditNote.getSummaryTotalInString(customerCreditNoteId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (customerCreditNoteId) {
        indo.customerCreditNotes.customerCreditNote.getSummarySubTotalInString(customerCreditNoteId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (customerCreditNoteId) {
        indo.customerCreditNotes.customerCreditNote.getSummaryDiscAmtInString(customerCreditNoteId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (customerCreditNoteId) {
        indo.customerCreditNotes.customerCreditNote.getSummaryBeforeTaxInString(customerCreditNoteId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (customerCreditNoteId) {
        indo.customerCreditNotes.customerCreditNote.getSummaryTaxAmountInString(customerCreditNoteId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfCustomerCreditNoteId);
    drawDiscAmt(hfCustomerCreditNoteId);
    drawBeforeTax(hfCustomerCreditNoteId);
    drawTaxAmount(hfCustomerCreditNoteId);
    drawTotal(hfCustomerCreditNoteId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfCustomerCreditNoteId);
        drawDiscAmt(hfCustomerCreditNoteId);
        drawBeforeTax(hfCustomerCreditNoteId);
        drawTaxAmount(hfCustomerCreditNoteId);
        drawTotal(hfCustomerCreditNoteId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfCustomerCreditNoteId);
        drawDiscAmt(hfCustomerCreditNoteId);
        drawBeforeTax(hfCustomerCreditNoteId);
        drawTaxAmount(hfCustomerCreditNoteId);
        drawTotal(hfCustomerCreditNoteId);
    });

    $('#NewCustomerCreditNoteDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            customerCreditNoteId: hfCustomerCreditNoteId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfCustomerCreditNoteNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
