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
        groupSettings: { columns: ['resourceName'] },
        allowSorting: true,
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
                field: 'name', headerText: 'Booking', textAlign: 'Left', width: 150
            },
            {
                field: 'description', headerText: 'Description', textAlign: 'Left', width: 150
            },
            {
                field: 'resourceName', headerText: 'Resource', textAlign: 'Left', width: 100
            },
            {
                field: 'startTime', headerText: 'Start', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(startTime)}"
            },
            {
                field: 'endTime', headerText: 'End', textAlign: 'Left', width: 100,
                template: "${customDateToLocaleString(endTime)}"
            },
            {
                field: 'location', headerText: 'Location', textAlign: 'Left', width: 100
            },
            {
                field: 'isDone', headerText: 'Done', textAlign: 'Left', width: 100
            },
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
        indo.bookings.booking.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }

});
