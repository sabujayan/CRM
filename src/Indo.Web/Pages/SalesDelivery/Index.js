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
            { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom' },
            { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom' },
            { type: 'Separator' },
            { text: 'Help', tooltipText: 'Help', id: 'HelpCustom' },
            { type: 'Separator' },
            { text: 'Details', tooltipText: 'Details', prefixIcon: '', id: 'DetailsCustom' },
            { type: 'Separator' },
            { text: 'Confirm Delivery', tooltipText: 'Confirm Delivery', prefixIcon: '', id: 'ConfirmDelivery' },
            { text: 'Return Delivery', tooltipText: 'Return Delivery', prefixIcon: '', id: 'ReturnDelivery' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                template: "<a href='/SalesDelivery/Detail/${id}' target='_blank'>${number}</a>"
            },
            {
                field: 'customerName', headerText: 'Customer', textAlign: 'Left', width: 100
            },
            {
                field: 'salesOrderNumber', headerText: 'Sales Order', textAlign: 'Left', width: 150,
                template: "<a href='/SalesOrder/Detail/${salesOrderId}' target='_blank'>${salesOrderNumber}</a>"
            },
            {
                field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                template: "<span class='label label-warning'>${statusString}</span>"
            },
            {
                field: 'deliveryDate', headerText: 'Sales Delivery Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(deliveryDate)}"
            }
        ],
        beforeDataBound: () => {
            grid.showSpinner();
        },
        dataBound: () => {
            grid.element.querySelector('.e-toolbar-left').style.left = grid.element.querySelector('.e-toolbar-right').getBoundingClientRect().width + 'px';
            grid.autoFitColumns();
            grid.hideSpinner();
        },
        excelExportComplete: () => {
            grid.hideSpinner();
        },
        created: () => {
            var gridHeight = ($(".pcoded-main-container").height()) - ($(".e-gridheader").outerHeight()) - ($(".e-toolbar").outerHeight()) - ($(".e-gridpager").outerHeight());
            grid.height = gridHeight;
        },
        rowSelected: () => {
            var selectedRow = grid.getSelectedRecords()[0];
            if (selectedRow.status === 1) {
                grid.toolbarModule.enableItems(['ConfirmDelivery', 'EditCustom', 'DeleteCustom'], true);
            } else {
                grid.toolbarModule.enableItems(['ConfirmDelivery', 'EditCustom', 'DeleteCustom'], false);
            }
            if (selectedRow.status === 2) {
                grid.toolbarModule.enableItems(['ReturnDelivery'], true);
            } else {
                grid.toolbarModule.enableItems(['ReturnDelivery'], false);
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
                                indo.salesDeliveries.salesDelivery
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
                    window.location.href = '/SalesDelivery/Detail/' + selectedId
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ConfirmDelivery') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 1) {
                        abp.message.confirm('Confirm this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesDeliveries.salesDelivery
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
            if (args.item.id === 'ReturnDelivery') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Return this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesDeliveries.salesDelivery
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
        indo.salesDeliveries.salesDelivery.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.salesDeliveries.salesDelivery.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }


    $(document).on('click', '#btn-update-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.salesDeliveries.salesDelivery
            .update(formAsObject.salesDelivery.id, formAsObject.salesDelivery)
            .then(function () {
                abp.notify.success(l('UpdateSuccess'));
                window.location.href = '/SalesDelivery/Detail/' + formAsObject.salesDelivery.id;
            })
            .catch(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    /* Popup Modal */
    var helpModal = new abp.ModalManager(abp.appPath + 'SalesDelivery/Help');
    var updateModal = new abp.ModalManager(abp.appPath + 'SalesDelivery/Update');
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
    });


});
