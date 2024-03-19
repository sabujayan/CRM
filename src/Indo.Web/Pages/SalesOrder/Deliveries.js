$(function () {
    var l = abp.localization.getResource('Indo');
    var updateModal = new abp.ModalManager(abp.appPath + 'SalesDelivery/Update');

    var hfSalesOrderId = $("#hfSalesOrderId").val();
    var dataTable = $('#SalesDeliveriesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.salesDeliveries.salesDelivery.getListBySalesOrder, function () {
                return hfSalesOrderId;
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
                                        window.location.href = '/SalesDelivery/Detail/' + data.record.id
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
                                        indo.salesDeliveries.salesDelivery
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                },
                                {
                                    text: "Confirm Delivery",
                                    visible: function (data) {
                                        return data.status == 1;
                                    },
                                    confirmMessage: function (data) {
                                        return "Confirm delivery: " + data.record.number + " ?";
                                    },
                                    action: function (data) {
                                        indo.salesDeliveries.salesDelivery
                                            .confirm(data.record.id)
                                            .then(function () {
                                                abp.notify.success("Confirm success.");
                                                dataTable.ajax.reload();
                                            });
                                    }
                                },
                                {
                                    text: "Return Delivery",
                                    visible: function (data) {
                                        return data.status == 2;
                                    },
                                    confirmMessage: function (data) {
                                        return "Return delivery: " + data.record.number + " ?";
                                    },
                                    action: function (data) {
                                        indo.salesDeliveries.salesDelivery
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
                    title: "Sales Order",
                    data: "salesOrderNumber"
                },
                {
                    title: "Status",
                    data: "status",
                    render: function (data) {
                        return l('Enum:SalesDeliveryStatus:' + data);
                    }
                },
                {
                    title: "Order Date", data: "deliveryDate",
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
