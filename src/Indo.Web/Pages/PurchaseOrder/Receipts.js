$(function () {
    var l = abp.localization.getResource('Indo');
    var updateModal = new abp.ModalManager(abp.appPath + 'PurchaseReceipt/Update');

    var hfPurchaseOrderId = $("#hfPurchaseOrderId").val();
    var dataTable = $('#PurchaseReceiptsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.purchaseReceipts.purchaseReceipt.getListByPurchaseOrder, function () {
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
                                    visible: function (data) {
                                        return data.status == 1;
                                    },
                                    action: function (data) {
                                        updateModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: "Details",
                                    action: function (data) {
                                        window.location.href = '/PurchaseReceipt/Detail/' + data.record.id
                                    }
                                },
                                {
                                    text: "Delete",
                                    visible: function (data) {
                                        return data.status == 1;
                                    },
                                    confirmMessage: function (data) {
                                        return "Are you sure to delete: " + data.record.number + " ?";
                                    },
                                    action: function (data) {
                                        indo.purchaseReceipts.purchaseReceipt
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                },
                                {
                                    text: "Confirm Receipt",
                                    visible: function (data) {
                                        return data.status == 1;
                                    },
                                    confirmMessage: function (data) {
                                        return "Confirm receipt: " + data.record.number + " ?";
                                    },
                                    action: function (data) {
                                        indo.purchaseReceipts.purchaseReceipt
                                            .confirm(data.record.id)
                                            .then(function () {
                                                abp.notify.success("Confirm success.");
                                                dataTable.ajax.reload();
                                            });
                                    }
                                },
                                {
                                    text: "Return Receipt",
                                    visible: function (data) {
                                        return data.status == 2;
                                    },
                                    confirmMessage: function (data) {
                                        return "Return receipt: " + data.record.number + " ?";
                                    },
                                    action: function (data) {
                                        indo.purchaseReceipts.purchaseReceipt
                                            .return(data.record.id)
                                            .then(function () {
                                                abp.notify.success("Return success.");
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: "Number",
                    data: "number"
                },
                {
                    title: "Purchase Order",
                    data: "purchaseOrderNumber"
                },
                {
                    title: "Status",
                    data: "status",
                    render: function (data) {
                        return l('Enum:PurchaseReceiptStatus:' + data);
                    }
                },
                {
                    title: "Order Date", data: "receiptDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                }
            ]
        })
    );

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
    });

});
