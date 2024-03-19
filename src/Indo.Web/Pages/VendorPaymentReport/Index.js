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
        allowGrouping: true,
        groupSettings: { columns: ['period', 'cashAndBankName'] },
        allowSorting: true,
        sortSettings: { columns: [{ field: 'period', direction: 'Descending' }, { field: 'cashAndBankName', direction: 'Ascending' }] },
        allowFiltering: true,
        filterSettings: { type: 'Excel' },
        allowSelection: true,
        selectionSettings: { persistSelection: true, type: 'Single' },
        enableHover: true,
        allowExcelExport: true,
        allowPdfExport: true,
        allowTextWrap: false,
        toolbar: ['ExcelExport', 'Search'],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'cashAndBankName', headerText: 'Payment Method', textAlign: 'Left', width: 100
            },
            {
                field: 'period', headerText: 'Period', textAlign: 'Left', width: 50
            },
            {
                field: 'number', headerText: 'Payment', textAlign: 'Left', width: 150
            },
            {
                field: 'sourceDocument', headerText: 'Source Document', textAlign: 'Left', width: 150,
                template: "<a href='/${sourceDocumentPath}/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
            },
            {
                field: 'sourceDocumentModuleString', headerText: 'Module', textAlign: 'Left', width: 100
            },
            {
                field: 'vendorName', headerText: 'Vendor', textAlign: 'Left', width: 150
            },
            {
                field: 'statusString', headerText: 'Payment Status', textAlign: 'Left', width: 100,
                template: "<span class='label label-warning'>${statusString}</span>"
            },
            {
                field: 'paymentDate', headerText: 'Payment Date', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(paymentDate)}"
            },
            {
                field: 'currencyName', headerText: 'Currency', textAlign: 'Left', width: 100,
                template: '<span class="badge badge-pill badge-info ml-1">${currencyName}</span>'
            },
            {
                field: 'amount', headerText: 'Amount', textAlign: 'Left', width: 100, format: 'N2'
            },
        ],
        aggregates: [{
            columns: [{
                type: 'Sum',
                field: 'amount',
                format: 'N2',
                groupFooterTemplate: 'Total: ${Sum}'
            }]
        }],
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
            var gridHeight = ($(".pcoded-main-container").height()) - ($(".e-gridheader").outerHeight()) - ($(".e-toolbar").outerHeight()) - ($(".e-groupdroparea").outerHeight()) - ($(".e-gridpager").outerHeight());
            grid.height = gridHeight;
        },
        rowSelecting: () => {
            if (grid.getSelectedRecords().length) {
                grid.clearSelection();
            }
        },
        toolbarClick: (args) => {
            if (args.item.id === 'Grid_excelexport') {
                grid.showSpinner();
                grid.excelExport();
            }

        },
    }); 
    function loadGrid() {
        indo.vendorPayments.vendorPayment.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

});
