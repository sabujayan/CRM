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
            { text: 'Confirm Order', tooltipText: 'Confirm Order', prefixIcon: '', id: 'ConfirmOrder' },
            { text: 'Cancel Order', tooltipText: 'Cancel Order', prefixIcon: '', id: 'CancelOrder' },
            { type: 'Separator' },
            { text: 'Delivery Lookup', tooltipText: 'Sales Delivery', prefixIcon: '', id: 'DeliveryLookup' },
            { text: 'Generate Confirm Delivery', tooltipText: 'Generate Confirm Delivery', prefixIcon: '', id: 'ConfirmDelivery' }, 
            { text: 'Generate Draft Delivery', tooltipText: 'Generate Draft Delivery', prefixIcon: '', id: 'DraftDelivery' },
            { type: 'Separator' },
            { text: 'Generate Invoice', tooltipText: 'Generate Invoice', prefixIcon: '', id: 'GenerateInvoice' },
            { text: 'Invoice Lookup', tooltipText: 'Customer Invoice', prefixIcon: '', id: 'InvoiceLookup' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                template: "<a href='/SalesOrder/Detail/${id}' target='_blank'>${number}</a>"
            },
            {
                field: 'customerName', headerText: 'Customer', textAlign: 'Left', width: 100
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
            var gridHeight = ($(".pcoded-main-container").height()) - ($(".e-gridheader").outerHeight()) - ($(".e-toolbar").outerHeight()*2) - ($(".e-gridpager").outerHeight());
            grid.height = gridHeight;
        },
        rowSelected: () => {
            var selectedRow = grid.getSelectedRecords()[0];
            if (selectedRow.status !== 1) {
                grid.toolbarModule.enableItems(['DeliveryLookup', 'InvoiceLookup'], true);
            } else {
                grid.toolbarModule.enableItems(['DeliveryLookup', 'InvoiceLookup'], false);
            }
            if (selectedRow.status === 1) {
                grid.toolbarModule.enableItems(['ConfirmOrder', 'EditCustom', 'DeleteCustom'], true);
            } else {
                grid.toolbarModule.enableItems(['ConfirmOrder', 'EditCustom', 'DeleteCustom'], false);
            }
            if (selectedRow.status === 2) {
                grid.toolbarModule.enableItems(['CancelOrder', 'ConfirmDelivery', 'DraftDelivery', 'GenerateInvoice'], true);
            } else {
                grid.toolbarModule.enableItems(['CancelOrder', 'ConfirmDelivery', 'DraftDelivery', 'GenerateInvoice'], false);
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
                                indo.salesOrders.salesOrder
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
                    window.location.href = '/SalesOrder/Detail/' + selectedId
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
            if (args.item.id === 'ConfirmOrder') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 1) {
                        abp.message.confirm('Confirm this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesOrders.salesOrder
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
                                    indo.salesOrders.salesOrder
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
            if (args.item.id === 'ConfirmDelivery') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Generate confirm delivery from this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesOrders.salesOrder
                                        .generateConfirmDelivery(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate confirm delivery success.");
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
            if (args.item.id === 'DraftDelivery') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Generate draft delivery from this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesOrders.salesOrder
                                        .generateDraftDelivery(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate draft delivery success.");
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
            if (args.item.id === 'GenerateInvoice') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Process this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesOrders.salesOrder
                                        .generateInvoice(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate invoice success.");
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
            if (args.item.id === 'InvoiceLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    invoiceLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
        },
    }); 
    function reloadGrid() {
        indo.salesOrders.salesOrder.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.salesOrders.salesOrder.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

    $(document).on('click', '#btn-update-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.salesOrders.salesOrder
            .update(formAsObject.salesOrder.id, formAsObject.salesOrder)
            .then(function () {
                abp.notify.success(l('UpdateSuccess'));
                window.location.href = '/SalesOrder/Detail/' + formAsObject.salesOrder.id;
            })
            .catch(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    $(document).on('click', '#btn-create-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.salesOrders.salesOrder
            .create(formAsObject.salesOrder)
            .then(function (args) {
                abp.notify.success(l('CreateSuccess'));
                window.location.href = '/SalesOrder/Detail/' + args.id;
            })
            .fail(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });


    /* Popup Modal */
    abp.modals.SalesOrderModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }
    var helpModal = new abp.ModalManager(abp.appPath + 'SalesOrder/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'SalesOrder/Create',
        modalClass: 'SalesOrderModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'SalesOrder/Update',
        modalClass: 'SalesOrderModal'
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
    var deliveryLookupModal = new abp.ModalManager(abp.appPath + 'SalesOrder/DeliveryLookup');
    deliveryLookupModal.onOpen(function () {
        loadDeliveryLookupGrid($("#hfSalesOrderId").val());
    });
    function loadDeliveryLookupGrid(salesOrderId) {
        indo.salesDeliveries.salesDelivery.getListBySalesOrder(salesOrderId)
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
                            field: 'number', headerText: 'Sales Delivery', textAlign: 'Left', width: 150,
                            template: "<a href='/SalesDelivery/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'salesOrderNumber', headerText: 'Sales Order', textAlign: 'Left', width: 150,
                            template: "<a href='/SalesOrder/Detail/${salesOrderId}' target='_blank'>${salesOrderNumber}</a>"
                        },
                        {
                            field: 'deliveryDate', headerText: 'Delivery Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(deliveryDate)}"
                        }
                    ],
                });
                deliveryLookupGrid.appendTo('#DeliveryLookupGrid');
                deliveryLookupGrid.dataSource = data;
            });
    }

    /* Popup Invoice Lookup Modal */
    var invoiceLookupModal = new abp.ModalManager(abp.appPath + 'SalesOrder/InvoiceLookup');
    invoiceLookupModal.onOpen(function () {
        loadInvoiceLookupGrid($("#hfSalesOrderId").val());
    });
    function loadInvoiceLookupGrid(salesOrderId) {
        indo.customerInvoices.customerInvoice.getListWithTotalBySalesOrder(salesOrderId)
            .then(function (data) {
                var invoiceLookupGrid = new ej.grids.Grid({
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
                            field: 'number', headerText: 'Invoice', textAlign: 'Left', width: 150,
                            template: "<a href='/CustomerInvoice/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'sourceDocument', headerText: 'Sales Order', textAlign: 'Left', width: 150,
                            template: "<a href='/SalesOrder/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
                        },
                        {
                            field: 'invoiceDate', headerText: 'Invoice Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(invoiceDate)}"
                        }
                    ],
                });
                invoiceLookupGrid.appendTo('#InvoiceLookupGrid');
                invoiceLookupGrid.dataSource = data;
            });
    }


});
