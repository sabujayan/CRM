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
            { type: 'Separator' },
            { text: 'Help', tooltipText: 'Help', id: 'HelpCustom' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'suffix', headerText: 'Suffix', textAlign: 'Left', width: 100
            },
            {
                field: 'module', headerText: 'Module', textAlign: 'Left', width: 100,
                template: "${numberSequenceGetLocalizedEnumString(module)}"
            },
            {
                field: 'nextNumber', headerText: 'Next Number', textAlign: 'Left', width: 100
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
            if (args.item.id === 'HelpCustom') {
                helpModal.open();
            }
            if (args.item.id === 'EditCustom') {
                if (grid.getSelectedRecords().length) {
                    var selectedId = grid.getSelectedRecords()[0].id;
                    updateModal.open({ id: selectedId });
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }

        },
    }); 
    function reloadGrid() {
        indo.numberSequences.numberSequence.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.numberSequences.numberSequence.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }
    window.numberSequenceGetLocalizedEnumString = function (enumIdx) {
        var l = abp.localization.getResource('Indo');
        return l('Enum:NumberSequenceModules:' + enumIdx);
    }
    /* Popup Modal */
    var helpModal = new abp.ModalManager(abp.appPath + 'NumberSequence/Help');
    var updateModal = new abp.ModalManager(abp.appPath + 'NumberSequence/Update');
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
    });

});
