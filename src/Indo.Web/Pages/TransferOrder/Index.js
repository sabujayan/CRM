$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    /* load grid once */
    loadGrid();

    /* Syncfusion */
    ej.base.enableRipple(window.ripple);
    var grid = new ej.grids.Grid({
        enableInfiniteScrolling: false,
        allowPaging: true,
        pageSettings: { currentPage: 1, pageSize: 100, pageSizes: ["10", "20", "100", "200", "All"] },
        allowSorting: true,
        sortSettings: { columns: [{ field: 'number', direction: 'Descending' }] },
        allowFiltering: true,
        filterSettings: { type: 'Excel' },
        allowSelection: true,
        selectionSettings: { persistSelection: true, type: 'Single' },
        enableHover: true,
        allowExcelExport: true,
        allowPdfExport: true,
        allowTextWrap: false,
        toolbar: [
            'ExcelExport', 'Search',
            { type: 'Separator' },
            { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom' },
            { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom' },
            { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom' },
            { type: 'Separator' },
            { text: 'Help', tooltipText: 'Help', id: 'HelpCustom' },
            { type: 'Separator' },
            { text: 'Details', tooltipText: 'Details', prefixIcon: '', id: 'DetailsCustom' },
            { type: 'Separator' },
            { text: 'Delivery Lookup', tooltipText: 'Delivery Order', prefixIcon: '', id: 'DeliveryLookup' },
            { text: 'Receipt Lookup', tooltipText: 'Goods Receipt', prefixIcon: '', id: 'ReceiptLookup' },
            { type: 'Separator' },
            { text: 'Confirm Transfer', tooltipText: 'Confirm Transfer', prefixIcon: '', id: 'ConfirmTransfer' },
            { text: 'Return Transfer', tooltipText: 'Return Transfer', prefixIcon: '', id: 'ReturnTransfer' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                template: "<a href='/TransferOrder/Detail/${id}' target='_blank'>${number}</a>"
            },
            {
                field: 'fromWarehouseName', headerText: 'From', textAlign: 'Left', width: 100
            },
            {
                field: 'toWarehouseName', headerText: 'To', textAlign: 'Left', width: 100
            },
            {
                field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                template: "<span class='label label-warning'>${statusString}</span>"
            },
            {
                field: 'orderDate', headerText: 'Transfer Order Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(orderDate)}"
            },
            {
                field: 'returnNumber', headerText: 'Return', textAlign: 'Left', width: 150,
                template:"<a href='/TransferOrder/Detail/${returnId}' target='_blank'>${returnNumber}</a>"
            },
            {
                field: 'originalNumber', headerText: 'Origin', textAlign: 'Left', width: 150,
                template:"<a href='/TransferOrder/Detail/${originalId}' target='_blank'>${originalNumber}</a>"
            },
        ],
        beforeDataBound: () => {
            grid.showSpinner();
        },
        dataBound: () => {
            grid.toolbarModule.element.ej2_instances[0].overflowMode = 'MultiRow';

            var toolbarrightele = grid.element.querySelector('.e-toolbar-right');
            toolbarrightele.parentElement.insertBefore(toolbarrightele, toolbarrightele.parentElement.children[0]);
            setTimeout(() => {
                if (grid.element.querySelector('.e-toolbar-left').style.marginLeft) {
                    grid.element.querySelector('.e-toolbar-left').style.marginLeft = grid.element.querySelector('.e-toolbar-right').getBoundingClientRect().width + 'px';
                }
            }, 100);

            grid.autoFitColumns();
            grid.hideSpinner();
        },
        excelExportComplete: () => {
            grid.hideSpinner();
        },
        created: () => {
            var gridHeight = ($(".pcoded-main-container").height()) - ($(".e-gridheader").outerHeight()) - ($(".e-toolbar").outerHeight()*2) - ($(".e-gridpager").outerHeight());
            grid.height = gridHeight;
        },
        rowSelected: () => {
            var selectedRow = grid.getSelectedRecords()[0];
            if (selectedRow.status !== 1) {
                grid.toolbarModule.enableItems(['DeliveryLookup', 'ReceiptLookup'], true);
            } else {
                grid.toolbarModule.enableItems(['DeliveryLookup', 'ReceiptLookup'], false);
            }
            if (selectedRow.status === 1) {
                grid.toolbarModule.enableItems(['ConfirmTransfer', 'EditCustom', 'DeleteCustom'], true);
            } else {
                grid.toolbarModule.enableItems(['ConfirmTransfer', 'EditCustom', 'DeleteCustom'], false);
            }
            if (selectedRow.status === 2) {
                grid.toolbarModule.enableItems(['ReturnTransfer'], true);
            } else {
                grid.toolbarModule.enableItems(['ReturnTransfer'], false);
            }
        },
        rowSelecting: () => {
            if (grid.getSelectedRecords().length) {
                grid.clearSelection();
            }
        },
        toolbarClick: (args) => {
            if (args.item.id === 'HelpCustom') {
                helpModal.open();
            }
            if (args.item.id === 'Grid_excelexport') {
                grid.showSpinner();
                grid.excelExport();
            }
            if (args.item.id === 'AddCustom') {
                createModal.open();
            }
            if (args.item.id === 'EditCustom') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    updateModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'DeleteCustom') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    abp.message.confirm('Delete this data: ' + selectedRow.number)
                        .then(function (confirmed) {
                            if (confirmed) {
                                indo.transferOrders.transferOrder
                                    .delete(selectedRow.id)
                                    .then(function () {
                                        abp.notify.success(l('DeleteSuccess'));
                                        reloadGrid();
                                    })
                                    .catch(function (error) {
                                        abp.notify.error("Error: " + error.message + " " + error.details);
                                    });
                            }
                        });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'DetailsCustom') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    window.location.href = '/TransferOrder/Detail/' + selectedId
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'DeliveryLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    deliveryLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ReceiptLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    receiptLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ConfirmTransfer') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 1) {
                        abp.message.confirm('Confirm this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.transferOrders.transferOrder
                                        .confirm(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Confirm success.");
                                            reloadGrid();
                                            grid.hideSpinner();
                                        })
                                        .catch(function (error) {
                                            abp.notify.error("Error: " + error.message + " " + error.details);
                                        });
                                }
                            });
                    } else {
                        abp.notify.error("Process not allowed.");
                    }
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ReturnTransfer') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Return this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.transferOrders.transferOrder
                                        .return(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Return success.");
                                            reloadGrid();
                                            grid.hideSpinner();
                                        })
                                        .catch(function (error) {
                                            abp.notify.error("Error: " + error.message + " " + error.details);
                                        });
                                }
                            });
                    } else {
                        abp.notify.error("Process not allowed.");
                    }
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
        },
    }); 
    function reloadGrid() {
        indo.transferOrders.transferOrder.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.transferOrders.transferOrder.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

    $(document).on('click', '#btn-update-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.transferOrders.transferOrder
            .update(formAsObject.transferOrder.id, formAsObject.transferOrder)
            .then(function () {
                abp.notify.success(l('UpdateSuccess'));
                window.location.href = '/TransferOrder/Detail/' + formAsObject.transferOrder.id;
            })
            .catch(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    $(document).on('click', '#btn-create-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.transferOrders.transferOrder
            .create(formAsObject.transferOrder)
            .then(function (args) {
                abp.notify.success(l('CreateSuccess'));
                window.location.href = '/TransferOrder/Detail/' + args.id;
            })
            .fail(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    /* Popup Modal */
    abp.modals.TransferOrderModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }
    var helpModal = new abp.ModalManager(abp.appPath + 'TransferOrder/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'TransferOrder/Create',
        modalClass: 'TransferOrderModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'TransferOrder/Update',
        modalClass: 'TransferOrderModal'
    });
    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        reloadGrid();
    });
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
    });


    /* Popup Delivery Lookup Modal */
    var deliveryLookupModal = new abp.ModalManager(abp.appPath + 'TransferOrder/DeliveryLookup');
    deliveryLookupModal.onOpen(function () {
        loadDeliveryLookupGrid($("#hfTransferOrderId").val());
    });
    function loadDeliveryLookupGrid(transferOrderId) {
        indo.deliveryOrders.deliveryOrder.getListByTransferOrder(transferOrderId)
            .then(function (data) {
                var deliveryLookupGrid = new ej.grids.Grid({
                    enableInfiniteScrolling: false,
                    allowPaging: false,
                    allowSorting: true,
                    allowFiltering: true,
                    filterSettings: { type: 'Excel' },
                    allowSelection: true,
                    selectionSettings: { persistSelection: true, type: 'Single' },
                    enableHover: true,
                    allowExcelExport: true,
                    allowPdfExport: true,
                    allowTextWrap: false,
                    toolbar: ['Search'],
                    columns: [
                        {
                            field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                        },
                        {
                            field: 'number', headerText: 'Delivery Order', textAlign: 'Left', width: 150,
                            template: "<a href='/DeliveryOrder/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'transferOrderNumber', headerText: 'Transfer Order', textAlign: 'Left', width: 150,
                            template: "<a href='/TransferOrder/Detail/${transferOrderId}' target='_blank'>${transferOrderNumber}</a>"
                        },
                        {
                            field: 'orderDate', headerText: 'Delivery Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(orderDate)}"
                        }
                    ],
                });
                deliveryLookupGrid.appendTo('#DeliveryLookupGrid');
                deliveryLookupGrid.dataSource = data;
            });
    }



    /* Popup Receipt Modal */
    var receiptLookupModal = new abp.ModalManager(abp.appPath + 'TransferOrder/ReceiptLookup');
    receiptLookupModal.onOpen(function () {
        loadReceiptLookupGrid($("#hfTransferOrderId").val());
    });
    function loadReceiptLookupGrid(transferOrderId) {
        indo.goodsReceipts.goodsReceipt.getListByTransferOrder(transferOrderId)
            .then(function (data) {
                var receiptLookupGrid = new ej.grids.Grid({
                    enableInfiniteScrolling: false,
                    allowPaging: false,
                    allowSorting: true,
                    allowFiltering: true,
                    filterSettings: { type: 'Excel' },
                    allowSelection: true,
                    selectionSettings: { persistSelection: true, type: 'Single' },
                    enableHover: true,
                    allowExcelExport: true,
                    allowPdfExport: true,
                    allowTextWrap: false,
                    toolbar: ['Search'],
                    columns: [
                        {
                            field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                        },
                        {
                            field: 'number', headerText: 'Goods Receipt', textAlign: 'Left', width: 150,
                            template: "<a href='/GoodsReceipt/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'deliveryOrderNumber', headerText: 'Delivery Order', textAlign: 'Left', width: 150,
                            template: "<a href='/DeliveryOrder/Detail/${deliveryOrderId}' target='_blank'>${deliveryOrderNumber}</a>"
                        },
                        {
                            field: 'orderDate', headerText: 'Receipt Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(orderDate)}"
                        }
                    ],
                });
                receiptLookupGrid.appendTo('#ReceiptLookupGrid');
                receiptLookupGrid.dataSource = data;
            });
    }


});
