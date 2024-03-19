$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    abp.modals.VendorBillDetailModal = function () {

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
        viewUrl: abp.appPath + 'VendorBill/CreateDetail',
        modalClass: 'VendorBillDetailModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'VendorBill/UpdateDetail',
        modalClass: 'VendorBillDetailModal'
    });

    var hfVendorBillId = $("#hfVendorBillId").val();
    var dataTable = $('#VendorBillDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.vendorBillDetails.vendorBillDetail.getListByVendorBill, function () {
                return hfVendorBillId;
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
                                        indo.vendorBillDetails.vendorBillDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawSubTotal(hfVendorBillId);
                                                drawDiscAmt(hfVendorBillId);
                                                drawBeforeTax(hfVendorBillId);
                                                drawTaxAmount(hfVendorBillId);
                                                drawTotal(hfVendorBillId);
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

    var drawTotal = function (vendorBillId) {
        indo.vendorBills.vendorBill.getSummaryTotalInString(vendorBillId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    var drawSubTotal = function (vendorBillId) {
        indo.vendorBills.vendorBill.getSummarySubTotalInString(vendorBillId)
            .then(function (data) {
                $("#lblSubTotalBottom").text(data);
            });
    }

    var drawDiscAmt = function (vendorBillId) {
        indo.vendorBills.vendorBill.getSummaryDiscAmtInString(vendorBillId)
            .then(function (data) {
                $("#lblDiscAmtBottom").text(data);
            });
    }

    var drawBeforeTax = function (vendorBillId) {
        indo.vendorBills.vendorBill.getSummaryBeforeTaxInString(vendorBillId)
            .then(function (data) {
                $("#lblBeforeTaxBottom").text(data);
            });
    }

    var drawTaxAmount = function (vendorBillId) {
        indo.vendorBills.vendorBill.getSummaryTaxAmountInString(vendorBillId)
            .then(function (data) {
                $("#lblTaxAmountBottom").text(data);
            });
    }

    drawSubTotal(hfVendorBillId);
    drawDiscAmt(hfVendorBillId);
    drawBeforeTax(hfVendorBillId);
    drawTaxAmount(hfVendorBillId);
    drawTotal(hfVendorBillId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfVendorBillId);
        drawDiscAmt(hfVendorBillId);
        drawBeforeTax(hfVendorBillId);
        drawTaxAmount(hfVendorBillId);
        drawTotal(hfVendorBillId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawSubTotal(hfVendorBillId);
        drawDiscAmt(hfVendorBillId);
        drawBeforeTax(hfVendorBillId);
        drawTaxAmount(hfVendorBillId);
        drawTotal(hfVendorBillId);
    });

    $('#NewVendorBillDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            vendorBillId: hfVendorBillId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfVendorBillNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
