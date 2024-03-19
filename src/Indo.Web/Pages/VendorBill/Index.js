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
            { text: 'Confirm Bill', tooltipText: 'Confirm Bill', prefixIcon: '', id: 'ConfirmBill' },
            { type: 'Separator' },
            { text: 'Debit Note', tooltipText: 'Debit Note', prefixIcon: '', id: 'DebitNote' },
            { text: 'Debit Note Lookup', tooltipText: 'Debit Note Lookup', prefixIcon: '', id: 'DebitNoteLookup' },
            { type: 'Separator' },
            { text: 'Generate Payment', tooltipText: 'Generate Payment', prefixIcon: '', id: 'GeneratePayment' },
            { text: 'Payment Lookup', tooltipText: 'Payment Lookup', prefixIcon: '', id: 'PaymentLookup' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                template: "<a href='/VendorBill/Detail/${id}' target='_blank'>${number}</a>"
            },
            {
                field: 'sourceDocument', headerText: 'Source Document', textAlign: 'Left', width: 150,
                template: "<a href='/${sourceDocumentPath}/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
            },
            {
                field: 'sourceDocumentModuleString', headerText: 'Module', textAlign: 'Left', width: 100
            },
            {
                field: 'vendorName', headerText: 'Vendor', textAlign: 'Left', width: 100
            },
            {
                field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                template: "<span class='label label-warning'>${statusString}</span>"
            },
            {
                field: 'billDate', headerText: 'Bill Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(billDate)}"
            },
            {
                field: 'billDueDate', headerText: 'Due Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(billDueDate)}"
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

            grid.toolbarModule.enableItems(['AddCustom'], false);
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
                grid.toolbarModule.enableItems(['DebitNoteLookup', 'PaymentLookup'], true);
            } else {
                grid.toolbarModule.enableItems(['DebitNoteLookup', 'PaymentLookup'], false);
            }
            if (selectedRow.status === 1) {
                grid.toolbarModule.enableItems(['ConfirmBill', 'EditCustom', 'DeleteCustom'], true);
            } else {
                grid.toolbarModule.enableItems(['ConfirmBill', 'EditCustom', 'DeleteCustom'], false);
            }
            if (selectedRow.status === 2) {
                grid.toolbarModule.enableItems(['DebitNote', 'GeneratePayment'], true);
            } else {
                grid.toolbarModule.enableItems(['DebitNote', 'GeneratePayment'], false);
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
                                indo.vendorBills.vendorBill
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
                    window.location.href = '/VendorBill/Detail/' + selectedId
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ConfirmBill') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 1) {
                        abp.message.confirm('Confirm this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.vendorBills.vendorBill
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
            if (args.item.id === 'DebitNote') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Debit note this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.vendorBills.vendorBill
                                        .generateDebitNote(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Debit note success.");
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
            if (args.item.id === 'DebitNoteLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    debitNoteLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'GeneratePayment') {
                if (grid.getSelectedRecords().length) {
                    var selectedRow = grid.getSelectedRecords()[0];
                    if (selectedRow.status === 2) {
                        abp.message.confirm('Generate payment from this data: ' + selectedRow.number)
                            .then(function (confirmed) {
                                if (confirmed) {
                                    grid.showSpinner();
                                    indo.vendorBills.vendorBill
                                        .generatePayment(selectedRow.id)
                                        .then(function () {
                                            abp.notify.success("Generate payment success.");
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
            if (args.item.id === 'PaymentLookup') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    paymentLookupModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
        },
    }); 
    function reloadGrid() {
        indo.vendorBills.vendorBill.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.vendorBills.vendorBill.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

    $(document).on('click', '#btn-update-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.vendorBills.vendorBill
            .update(formAsObject.vendorBill.id, formAsObject.vendorBill)
            .then(function () {
                abp.notify.success(l('UpdateSuccess'));
                window.location.href = '/VendorBill/Detail/' + formAsObject.vendorBill.id;
            })
            .catch(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });

    $(document).on('click', '#btn-create-go-to-detail', function () {
        var $form = $('#form');
        var formAsObject = $form.serializeFormToObject();
        indo.vendorBills.vendorBill
            .create(formAsObject.vendorBill)
            .then(function (args) {
                abp.notify.success(l('CreateSuccess'));
                window.location.href = '/VendorBill/Detail/' + args.id;
            })
            .fail(function (error) {
                abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
            });
    });


    /* Popup Modal */
    abp.modals.VendorBillModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }
    var helpModal = new abp.ModalManager(abp.appPath + 'VendorBill/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'VendorBill/Create',
        modalClass: 'VendorBillModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'VendorBill/Update',
        modalClass: 'VendorBillModal'
    });
    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        reloadGrid();
    });
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
    });


    /* Popup Debit Note Lookup Modal */
    var debitNoteLookupModal = new abp.ModalManager(abp.appPath + 'VendorBill/DebitNoteLookup');
    debitNoteLookupModal.onOpen(function () {
        loadDebitNoteLookupGrid($("#hfVendorBillId").val());
    });
    function loadDebitNoteLookupGrid(vendorBillId) {
        indo.vendorDebitNotes.vendorDebitNote.getListWithTotalByBill(vendorBillId)
            .then(function (data) {
                var debitNoteLookupGrid = new ej.grids.Grid({
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
                            field: 'number', headerText: 'Debit Note', textAlign: 'Left', width: 150,
                            template: "<a href='/VendorDebitNote/Detail/${id}' target='_blank'>${number}</a>"
                        },
                        {
                            field: 'vendorBillNumber', headerText: 'Bill', textAlign: 'Left', width: 150,
                            template: "<a href='/VendorBill/Detail/${vendorBillId}' target='_blank'>${vendorBillNumber}</a>"
                        },
                        {
                            field: 'debitNoteDate', headerText: 'Debit Note Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(debitNoteDate)}"
                        }
                    ],
                });
                debitNoteLookupGrid.appendTo('#DebitNoteLookupGrid');
                debitNoteLookupGrid.dataSource = data;
            });
    }

    /* Popup Payment Lookup Modal */
    var paymentLookupModal = new abp.ModalManager(abp.appPath + 'VendorBill/PaymentLookup');
    paymentLookupModal.onOpen(function () {
        loadPaymentLookupGrid($("#hfVendorBillId").val());
    });
    function loadPaymentLookupGrid(vendorBillId) {
        indo.vendorPayments.vendorPayment.getListByBill(vendorBillId)
            .then(function (data) {
                var paymentLookupGrid = new ej.grids.Grid({
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
                            field: 'number', headerText: 'Payment', textAlign: 'Left', width: 150
                        },
                        {
                            field: 'sourceDocument', headerText: 'Bill', textAlign: 'Left', width: 150,
                            template: "<a href='/${sourceDocumentPath}/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
                        },
                        {
                            field: 'paymentDate', headerText: 'Payment Date', textAlign: 'Left', width: 100,
                            template: "${customDateToLocaleString(paymentDate)}"
                        }
                    ],
                });
                paymentLookupGrid.appendTo('#PaymentLookupGrid');
                paymentLookupGrid.dataSource = data;
            });
    }


});
