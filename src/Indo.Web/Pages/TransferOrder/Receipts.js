$(function () {
    var l = abp.localization.getResource('Indo');
    var updateModal = new abp.ModalManager(abp.appPath + 'GoodsReceipt/Update');

    var hfTransferOrderId = $("#hfTransferOrderId").val();
    var dataTable = $('#GoodsReceiptsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.goodsReceipts.goodsReceipt.getListByTransferOrder, function () {
                return hfTransferOrderId;
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
                                        window.location.href = '/GoodsReceipt/Detail/' + data.record.id
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
                                        indo.goodsReceipts.goodsReceipt
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
                    title: "Number",
                    data: "number"
                },
                {
                    title: "Delivery",
                    data: "deliveryOrderNumber"
                },
                {
                    title: "From",
                    data: "fromWarehouseName"
                },
                {
                    title: "To",
                    data: "toWarehouseName"
                },
                {
                    title: "Status",
                    data: "status",
                    render: function (data) {
                        return l('Enum:GoodsReceiptStatus:' + data);
                    }
                },
                {
                    title: "Order Date", data: "orderDate",
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
