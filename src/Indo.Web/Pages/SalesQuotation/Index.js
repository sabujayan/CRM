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
            { text: 'Confirm Quotation', tooltipText: 'Confirm Quotation', prefixIcon: '', id: 'ConfirmQuotation' },
            { text: 'Cancel Quotation', tooltipText: 'Cancel Quotation', prefixIcon: '', id: 'CancelQuotation' },
            { type: 'Separator' },
            { text: 'Convert To Order', tooltipText: 'Convert To Order', prefixIcon: '', id: 'ConvertToOrder' },
            { type: 'Separator' },
            { text: 'Order Lookup', tooltipText: 'Sales Order', prefixIcon: '', id: 'OrderLookup' },
            { type: 'Separator' },
            { text: 'Kanban', tooltipText: 'Kanban', prefixIcon: '', id: 'KanbanView' },
            { type: 'Separator' }
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                template: "<a href='/SalesQuotation/Detail/${id}' target='_blank'>${number}</a>"
            },
            {
                field: 'customerName', headerText: 'Customer', textAlign: 'Left', width: 100
            },
            {
                field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                template: "<span class='label label-warning'>${statusString}</span>"
            },
            {
                field: 'pipelineString', headerText: 'Pipeline', textAlign: 'Left', width: 100
            },
            {
                field: 'quotationDate', headerText: 'Quotation Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(quotationDate)}"
            },
            {
                field: 'quotationValidUntilDate', headerText: 'Valid Until', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(quotationValidUntilDate)}"
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
                grid.toolbarModule.enableItems(['OrderLookup'], true);
            } else {
                grid.toolbarModule.enableItems(['OrderLookup'], false);
            }
            if (selectedRow.status === 1) {
                grid.toolbarModule.enableItems(['ConfirmQuotation', 'EditCustom', 'DeleteCustom'], true);
            } else {
                grid.toolbarModule.enableItems(['ConfirmQuotation', 'EditCustom', 'DeleteCustom'], false);
            }
            if (selectedRow.status === 2) {
                grid.toolbarModule.enableItems(['CancelQuotation', 'ConvertToOrder'], true);
            } else {
                grid.toolbarModule.enableItems(['CancelQuotation', 'ConvertToOrder'], false);
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
                                indo.salesQuotations.salesQuotation
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
                    window.location.href = '/SalesQuotation/Detail/' + selectedId
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ConfirmQuotation') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 1) {
                        abp.message.confirm('Confirm this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesQuotations.salesQuotation
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
            if (args.item.id === 'CancelQuotation') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Cancel this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesQuotations.salesQuotation
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
            if (args.item.id === 'ConvertToOrder') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Convert this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.salesQuotations.salesQuotation
                                        .convertToOrder(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Convert to order success.");
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
            if (args.item.id === 'OrderLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    orderLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'KanbanView') {
                window.location.href = '/SalesQuotation?ViewMode=Kanban';
            }
        },
    }); 
    function reloadGrid() {
        indo.salesQuotations.salesQuotation.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.salesQuotations.salesQuotation.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

    $(document).on('click', '#btn-update-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.salesQuotations.salesQuotation
            .update(formAsObject.salesQuotation.id, formAsObject.salesQuotation)
            .then(function () {
                abp.notify.success(l('UpdateSuccess'));
                window.location.href = '/SalesQuotation/Detail/' + formAsObject.salesQuotation.id;
            })
            .catch(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    $(document).on('click', '#btn-create-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.salesQuotations.salesQuotation
            .create(formAsObject.salesQuotation)
            .then(function (args) {
                abp.notify.success(l('CreateSuccess'));
                window.location.href = '/SalesQuotation/Detail/' + args.id;
            })
            .fail(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });


    /* Popup Modal */
    abp.modals.SalesQuotationModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }
    var helpModal = new abp.ModalManager(abp.appPath + 'SalesQuotation/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'SalesQuotation/Create',
        modalClass: 'SalesQuotationModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'SalesQuotation/Update',
        modalClass: 'SalesQuotationModal'
    });
    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        reloadGrid();
        reloadKanban();
    });
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
        reloadKanban();
    });

    /* Popup Lookup Modal */
    var orderLookupModal = new abp.ModalManager(abp.appPath + 'SalesQuotation/OrderLookup');
    orderLookupModal.onOpen(function () {
        loadOrderLookupGrid($("#hfSalesQuotationId").val());
    });
    function loadOrderLookupGrid(salesQuotationId) {
        indo.salesOrders.salesOrder.getListWithTotalByQuotation(salesQuotationId)
            .then(function (data) {
                var orderLookupGrid = new ej.grids.Grid({
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
                            field: 'number', headerText: 'Sales Order', textAlign: 'Left', width: 150,
                            template: "<a href='/SalesOrder/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'sourceDocument', headerText: 'Sales Quotation', textAlign: 'Left', width: 150,
                            template: "<a href='/SalesQuotation/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
                        },
                        {
                            field: 'orderDate', headerText: 'Order Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(orderDate)}"
                        }
                    ],
                });
                orderLookupGrid.appendTo('#OrderLookupGrid');
                orderLookupGrid.dataSource = data;
            });
    }


    /* Toolbar */
    loadToolbar();
    function loadToolbar() {
        var toolbarObj = new ej.navigations.Toolbar({
            items: [
                { type: 'Separator' },
                { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom', click: toolbarClick },
                { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom', click: toolbarClick },
                { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom', click: toolbarClick },
                { type: 'Separator' },
                { text: 'Help', tooltipText: 'Help', id: 'HelpCustom', click: toolbarClick },
                { type: 'Separator' },
                { text: 'Details', tooltipText: 'Details', prefixIcon: '', id: 'DetailsCustom', click: toolbarClick },
                { type: 'Separator' },
                { text: 'List', tooltipText: 'List', prefixIcon: '', id: 'ListView', click: toolbarClick },
                { type: 'Separator' },
            ]
        });
        toolbarObj.appendTo('#toolbar');

        function toolbarClick(args) {
            if (args.item.id === 'HelpCustom') {
                helpModal.open();
            }
            if (args.item.id === 'AddCustom') {
                createModal.open();
            }
            if (args.item.id === 'EditCustom') {

                if (selectedCard !== null) {
                    var selectedId = selectedCard.id;
                    selectedCard = null;
                    updateModal.open({ id: selectedId });

                } else {
                    abp.notify.error(l('SelectRecordError'));
                }

            }
            if (args.item.id === 'DeleteCustom') {
                if (selectedCard !== null) {
                    var selectedRow = selectedCard;
                    selectedCard = null;
                    abp.message.confirm('Delete this data: ' + selectedRow.number)
                        .then(function (confirmed) {
                            if (confirmed) {
                                indo.salesQuotations.salesQuotation
                                    .delete(selectedRow.id)
                                    .then(function () {
                                        abp.notify.success(l('DeleteSuccess'));
                                        reloadKanban();
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
                if (selectedCard !== null) {
                    var selectedId = selectedCard.id;
                    selectedCard = null;
                    window.location.href = '/SalesQuotation/Detail/' + selectedId
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ListView') {
                window.location.href = '/SalesQuotation?ViewMode=List';
            }
        }
    }

    /* Kanban */
    var pipeline1 = l('Enum:SalesQuotationPipeline:1');
    var pipeline2 = l('Enum:SalesQuotationPipeline:2');
    var pipeline3 = l('Enum:SalesQuotationPipeline:3');
    var pipeline4 = l('Enum:SalesQuotationPipeline:4');
    var pipeline5 = l('Enum:SalesQuotationPipeline:5');
    var pipeline6 = l('Enum:SalesQuotationPipeline:6');
    var selectedCard = null;
    loadKanban();
    function loadKanban() {
        var kanbanObj = new ej.kanban.Kanban({
            dataSource: [],
            keyField: 'pipelineString',
            columns: [
                { headerText: pipeline1, keyField: pipeline1 },
                { headerText: pipeline2, keyField: pipeline2 },
                { headerText: pipeline3, keyField: pipeline3 },
                { headerText: pipeline4, keyField: pipeline4 },
                { headerText: pipeline5, keyField: pipeline5 },
                { headerText: pipeline6, keyField: pipeline6 }
            ],
            cardSettings: {
                headerField: 'number',
                template: '#cardTemplate',
                selectionType: 'single'
            },
            dialogOpen: (args) => {
                args.cancel = true;
                selectedCard = null;
                if (args.name === 'dialogOpen' && args.requestType === 'Edit') {
                    var selectedId = args.data.id;
                    updateModal.open({ id: selectedId });
                }
            },
            cardClick: (args) => {
                if (args.name === 'cardClick') {
                    var kanban = document.getElementById("Kanban").ej2_instances[0];
                    selectedCard = kanban.activeCardData.data;
                }
            },
            created: () => {
                var kanbanHeight = ($(".pcoded-main-container").height()) - ($(".e-toolbar").outerHeight()) - 10;
                var kanban = document.getElementById("Kanban").ej2_instances[0];
                kanban.height = kanbanHeight;
            },
            dragStop: (args) => {
                var pipeline = 0;
                var column = args.data[0].pipelineString;
                if (column === l('Enum:SalesQuotationPipeline:1')) {
                    pipeline = 1;
                }
                if (column === l('Enum:SalesQuotationPipeline:2')) {
                    pipeline = 2;
                }
                if (column === l('Enum:SalesQuotationPipeline:3')) {
                    pipeline = 3;
                }
                if (column === l('Enum:SalesQuotationPipeline:4')) {
                    pipeline = 4;
                }
                if (column === l('Enum:SalesQuotationPipeline:5')) {
                    pipeline = 5;
                }
                if (column === l('Enum:SalesQuotationPipeline:6')) {
                    pipeline = 6;
                }

                if (pipeline !== 0) {
                    var updated = args.data[0];
                    updated.pipeline = pipeline;
                    indo.salesQuotations.salesQuotation
                        .update(args.data[0].id, updated)
                        .then(function () {
                            abp.notify.success('Update Pipeline Success.');
                            reloadKanban();
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }
            },
        });

        indo.salesQuotations.salesQuotation.getList()
            .then(function (data) {
                kanbanObj.dataSource = data;
                kanbanObj.appendTo('#Kanban');
            });
    }

    function reloadKanban() {
        indo.salesQuotations.salesQuotation.getList()
            .then(function (data) {
                var kanban = document.getElementById("Kanban").ej2_instances[0];
                kanban.dataSource = data;
            });
    }

});
