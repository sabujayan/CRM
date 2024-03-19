$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    var createModal = new abp.ModalManager(abp.appPath + 'PurchaseReceipt/CreateDetail');
    var updateModal = new abp.ModalManager(abp.appPath + 'PurchaseReceipt/UpdateDetail');

    var hfPurchaseReceiptId = $("#hfPurchaseReceiptId").val();
    var dataTable = $('#PurchaseReceiptDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.purchaseReceiptDetails.purchaseReceiptDetail.getListByPurchaseReceipt, function () {
                return hfPurchaseReceiptId;
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
                                        indo.purchaseReceiptDetails.purchaseReceiptDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
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
                }
            ]
        })
    );


    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
    });

    $('#NewPurchaseReceiptDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            purchaseReceiptId: hfPurchaseReceiptId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfPurchaseReceiptNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };

});
