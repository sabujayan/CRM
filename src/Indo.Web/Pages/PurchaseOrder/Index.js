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
            'Search', 'ExcelExport',
            { type: 'Separator' },
            { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom' },
            { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom' },
            { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom' },
            { type: 'Separator' },
            { text: 'Help', tooltipText: 'Help', id: 'HelpCustom' },
            { type: 'Separator'},
            { text: 'Details', tooltipText: 'Details', prefixIcon: '', id: 'DetailsCustom' },
            { type: 'Separator' },
            { text: 'Confirm Order', tooltipText: 'Confirm Order', prefixIcon: '', id: 'ConfirmOrder' },
            { text: 'Cancel Order', tooltipText: 'Cancel Order', prefixIcon: '', id: 'CancelOrder' },
            { type: 'Separator' },
            { text: 'Receipt Lookup', tooltipText: 'Purchase Receipt', prefixIcon: '', id: 'ReceiptLookup' },
            { text: 'Generate Confirm Receipt', tooltipText: 'Generate Confirm Receipt', prefixIcon: '', id: 'ConfirmReceipt' }, 
            { text: 'Generate Draft Receipt', tooltipText: 'Generate Draft Receipt', prefixIcon: '', id: 'DraftReceipt' },
            { type: 'Separator' },
            { text: 'Generate Bill', tooltipText: 'Generate Bill', prefixIcon: '', id: 'GenerateBill' },
            { text: 'Bill Lookup', tooltipText: 'Vendor Bill', prefixIcon: '', id: 'BillLookup' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                template: "<a href='/PurchaseOrder/Detail/${id}' target='_blank'>${number}</a>"
            },
            {
                field: 'vendorName', headerText: 'Vendor', textAlign: 'Left', width: 100
            },
            {
                field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                template: "<span class='label label-warning'>${statusString}</span>"
            },
            {
                field: 'orderDate', headerText: 'Sales Order Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(orderDate)}"
            }
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
            var gridHeight = ($(".pcoded-main-container").height()) - ($(".e-gridheader").outerHeight()) - ($(".e-toolbar").outerHeight() * 2) - ($(".e-gridpager").outerHeight());
            grid.height = gridHeight;
        },
        rowSelected: () => {
            var selectedRow = grid.getSelectedRecords()[0];
            if (selectedRow.status !== 1) {
                grid.toolbarModule.enableItems(['ReceiptLookup', 'BillLookup'], true);
            } else {
                grid.toolbarModule.enableItems(['ReceiptLookup', 'BillLookup'], false);
            }
            if (selectedRow.status === 1) {
                grid.toolbarModule.enableItems(['ConfirmOrder', 'EditCustom', 'DeleteCustom'], true);
            } else {
                grid.toolbarModule.enableItems(['ConfirmOrder', 'EditCustom', 'DeleteCustom'], false);
            }
            if (selectedRow.status === 2) {
                grid.toolbarModule.enableItems(['CancelOrder', 'ConfirmReceipt', 'DraftReceipt', 'GenerateBill'], true);
            } else {
                grid.toolbarModule.enableItems(['CancelOrder', 'ConfirmReceipt', 'DraftReceipt', 'GenerateBill'], false);
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
                                indo.purchaseOrders.purchaseOrder
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
                    window.location.href = '/PurchaseOrder/Detail/' + selectedId
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
            if (args.item.id === 'ConfirmOrder') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 1) {
                        abp.message.confirm('Confirm this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.purchaseOrders.purchaseOrder
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
            if (args.item.id === 'CancelOrder') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Cancel this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.purchaseOrders.purchaseOrder
                                        .cancel(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Cancel success.");
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
            if (args.item.id === 'ConfirmReceipt') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Generate confirm receipt from this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.purchaseOrders.purchaseOrder
                                        .generateConfirmReceipt(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate confirm receipt success.");
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
            if (args.item.id === 'DraftReceipt') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Generate draft receipt from this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.purchaseOrders.purchaseOrder
                                        .generateDraftReceipt(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate draft receipt success.");
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
            if (args.item.id === 'GenerateBill') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Process this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.purchaseOrders.purchaseOrder
                                        .generateBill(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate bill success.");
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
            if (args.item.id === 'BillLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    billLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
        },
    });
    function reloadGrid() {
        indo.purchaseOrders.purchaseOrder.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.purchaseOrders.purchaseOrder.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

    $(document).on('click', '#btn-update-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.purchaseOrders.purchaseOrder
            .update(formAsObject.purchaseOrder.id, formAsObject.purchaseOrder)
            .then(function () {
                abp.notify.success(l('UpdateSuccess'));
                window.location.href = '/PurchaseOrder/Detail/' + formAsObject.purchaseOrder.id;
            })
            .catch(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    $(document).on('click', '#btn-create-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.purchaseOrders.purchaseOrder
            .create(formAsObject.purchaseOrder)
            .then(function (args) {
                abp.notify.success(l('CreateSuccess'));
                window.location.href = '/PurchaseOrder/Detail/' + args.id;
            })
            .fail(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    /* Popup Modal */
    abp.modals.PurchaseOrderModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }
    var helpModal = new abp.ModalManager(abp.appPath + 'PurchaseOrder/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'PurchaseOrder/Create',
        modalClass: 'PurchaseOrderModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'PurchaseOrder/Update',
        modalClass: 'PurchaseOrderModal'
    });
    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        reloadGrid();
    });
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
    });


    /* Popup Receipt Lookup Modal */
    var receiptLookupModal = new abp.ModalManager(abp.appPath + 'PurchaseOrder/ReceiptLookup');
    receiptLookupModal.onOpen(function () {
        loadReceiptLookupGrid($("#hfPurchaseOrderId").val());
    });
    function loadReceiptLookupGrid(purchaseOrderId) {
        indo.purchaseReceipts.purchaseReceipt.getListByPurchaseOrder(purchaseOrderId)
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
                            field: 'number', headerText: 'Purchase Receipt', textAlign: 'Left', width: 150,
                            template: "<a href='/PurchaseReceipt/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'purchaseOrderNumber', headerText: 'Purchase Order', textAlign: 'Left', width: 150,
                            template: "<a href='/PurchaseOrder/Detail/${purchaseOrderId}' target='_blank'>${purchaseOrderNumber}</a>"
                        },
                        {
                            field: 'receiptDate', headerText: 'Receipt Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(receiptDate)}"
                        }
                    ],
                });
                receiptLookupGrid.appendTo('#ReceiptLookupGrid');
                receiptLookupGrid.dataSource = data;
            });
    }


    /* Popup Bill Lookup Modal */
    var billLookupModal = new abp.ModalManager(abp.appPath + 'PurchaseOrder/BillLookup');
    billLookupModal.onOpen(function () {
        loadBillLookupGrid($("#hfPurchaseOrderId").val());
    });
    function loadBillLookupGrid(purchaseOrderId) {
        indo.vendorBills.vendorBill.getListWithTotalByPurchaseOrder(purchaseOrderId)
            .then(function (data) {
                var billLookupGrid = new ej.grids.Grid({
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
                            field: 'number', headerText: 'Bill', textAlign: 'Left', width: 150,
                            template: "<a href='/VendorBill/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'sourceDocument', headerText: 'Purchase Order', textAlign: 'Left', width: 150,
                            template: "<a href='/PurchaseOrder/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
                        },
                        {
                            field: 'billDate', headerText: 'Bill Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(billDate)}"
                        }
                    ],
                });
                billLookupGrid.appendTo('#BillLookupGrid');
                billLookupGrid.dataSource = data;
            });
    }



});
