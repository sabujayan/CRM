$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    drawQRCode();

    var createModal = new abp.ModalManager(abp.appPath + 'ProjectOrder/CreateDetail');
    var updateModal = new abp.ModalManager(abp.appPath + 'ProjectOrder/UpdateDetail');

    var hfProjectOrderId = $("#hfProjectOrderId").val();
    var dataTable = $('#ProjectOrderDetailsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: false,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(indo.projectOrderDetails.projectOrderDetail.getListByProjectOrder, function () {
                return hfProjectOrderId;
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
                                        return "Are you sure to delete: " + data.record.projectTask + " ?";
                                    },
                                    action: function (data) {
                                        indo.projectOrderDetails.projectOrderDetail
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('DeleteSuccess'));
                                                dataTable.ajax.reload();
                                                drawTotal($("#hfProjectOrderId").val());
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: "Project Task",
                    data: "projectTask"
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
                    data: "quantity"
                },
                {
                    title: "Total",
                    data: "totalString"
                }
            ]
        })
    );

    var drawTotal = function (projectOrderId) {
        indo.projectOrders.projectOrder.getSummaryTotalInString(projectOrderId)
            .then(function (data) {
                $("#lblTotalBottom").text(data);
                $("#lblTotalTop").text(data);
            });
    }

    drawTotal(hfProjectOrderId);

    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        dataTable.ajax.reload();
        drawTotal(hfProjectOrderId);
    });

    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        dataTable.ajax.reload();
        drawTotal(hfProjectOrderId);
    });

    $('#NewProjectOrderDetailButton').click(function (e) {
        e.preventDefault();
        createModal.open({
            projectOrderId: hfProjectOrderId
        });
    });

    function drawQRCode() {
        var barcodeQrCode = new ej.barcodegenerator.QRCodeGenerator({
            width: '150px',
            height: '100px',
            mode: 'SVG',
            type: 'QRCode',
            displayText: { visibility: false },
            value: $("#hfProjectOrderNumber").val(),
        });
        barcodeQrCode.appendTo('#qrcode');
    };


});
