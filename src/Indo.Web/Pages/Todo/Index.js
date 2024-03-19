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
        sortSettings: { columns: [{ field: 'name', direction: 'Descending' }] },
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
            { text: 'Calendar View', tooltipText: 'Calendar View', prefixIcon: '', id: 'CalendarView' },
            { type: 'Separator' },
        ],
        columns: [
            { type: 'checkbox', width: 30 },
            {
                field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
            },
            {
                field: 'name', headerText: 'Name', textAlign: 'Left', width: 100,
                template:"${if(isDone)}<s>${name}</s>${else}<span>${name}</span>${/if}"
            },
            {
                field: 'description', headerText: 'Description', textAlign: 'Left', width: 150
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
                field: 'isDone', headerText: 'Done', textAlign: 'Left', width: 100
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
                    abp.message.confirm('Delete this data: ' + selectedRow.name)
                        .then(function (confirmed) {
                            if (confirmed) {
                                indo.todos.todo
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
            if (args.item.id === 'CalendarView') {
                calendarViewModal.open();
            }

        },
    }); 
    function reloadGrid() {
        indo.todos.todo.getList()
            .then(function (data) {
                grid.dataSource = data;
            });
    }
    function loadGrid() {
        indo.todos.todo.getList()
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#Grid');
            });
    }


    /* Popup Modal */
    var helpModal = new abp.ModalManager(abp.appPath + 'Todo/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Todo/Create',
        modalClass: 'TodoCreate'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Todo/Update',
        modalClass: 'TodoUpdate'
    });
    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        reloadGrid();
    });
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadGrid();
    });

    /*Calendar View*/
    var calendarViewModal = new abp.ModalManager(abp.appPath + 'Todo/CalendarView');
    calendarViewModal.onOpen(function () {
        loadCalendarView();
    });
    var scheduleObj = new ej.schedule.Schedule({
        width: '100%',
        height: 'auto',
        enableAdaptiveUI: true,
        views: ['Day', 'Week', 'Month', 'Year', 'MonthAgenda'],
        currentView: 'MonthAgenda',
        allowDragAndDrop: false,
        allowResizing: false,
        readonly: false,
        eventSettings: {
            dataSource: [],
            enableTooltip: true,
            fields: {
                id: 'id',
                subject: { name: 'name', validation: { required: true } },
                startTime: { name: 'startTime' },
                endTime: { name: 'endTime' },
                description: { name: 'description' },
                location: { name: 'location' },
                isReadonly: 'isDone'
            }
        },
        popupOpen: (args) => {
            var guid = args.data.Guid;
            if (args.name === 'popupOpen' && (args.type === 'Editor' || args.type === 'QuickInfo') && ej.base.isNullOrUndefined(guid)) {

                indo.numberSequences.numberSequence
                    .getNextNumber(17)
                    .then(function (data) {
                        if (args.type === 'QuickInfo') {
                            var input = document.getElementsByClassName('e-subject e-field e-input');
                            input[0].value = data;
                        }
                        if (args.type === 'Editor') {
                            document.getElementById('name').value = data;
                        }
                        args.data.IsAllDay = false;
                    });

            }

        },
        actionBegin: (args) => {
            //create
            if (args.name === 'actionBegin' && args.requestType === 'eventCreate') {
                var timezone = new ej.schedule.Timezone();
                var newObj = {
                    name: args.data[0].name,
                    description: args.data[0].description,
                    location: args.data[0].location,
                    startTime: timezone.removeLocalOffset(args.data[0].startTime),
                    endTime: timezone.removeLocalOffset(args.data[0].endTime),
                    isDone: args.data[0].isDone
                };
                
                indo.todos.todo
                    .create(newObj)
                    .then(function (dataRef) {
                        reloadCalendarView();
                        reloadGrid();
                        abp.notify.success(l('CreateSuccess'));
                    })
                    .catch(function (error) {
                        reloadCalendarView();
                        abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                    });
            }

            //update
            if (args.name === 'actionBegin' && args.requestType === 'eventChange') {
                var timezone = new ej.schedule.Timezone();
                var updObj = {
                    name: args.data.name,
                    description: args.data.description,
                    location: args.data.location,
                    startTime: timezone.removeLocalOffset(args.data.startTime),
                    endTime: timezone.removeLocalOffset(args.data.endTime),
                    isDone: args.data.isDone
                };

                indo.todos.todo
                    .update(args.data.id, updObj)
                    .then(function (data) {
                        reloadCalendarView();
                        reloadGrid();
                        abp.notify.success(l('UpdateSuccess'));
                    })
                    .catch(function (error) {
                        reloadCalendarView();
                        abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                    });

            }

            //delete
            if (args.name === 'actionBegin' && args.requestType === 'eventRemove') {
                indo.todos.todo
                    .delete(args.data[0].id)
                    .then(function () {
                        reloadCalendarView();
                        reloadGrid();
                        abp.notify.success(l('DeleteSuccess'));
                    })
                    .catch(function (error) {
                        reloadCalendarView();
                        abp.notify.error("Error: " + error.message + " " + error.details);
                    });

            }
        },
    });

    function loadCalendarView() {

        ej.base.L10n.load({
            'en-US': {
                'schedule': {
                    'newEvent': 'Add Todo',
                    'editEvent': 'Update Todo',
                    'deleteEvent': 'Delete Todo',
                },
            }
        });

        indo.todos.todo.getList()
            .then(function (data) {
                scheduleObj.eventSettings.dataSource = data;
                scheduleObj.appendTo('#Schedule');
            });
    }

    function reloadCalendarView() {
        indo.todos.todo.getList()
            .then(function (data) {
                scheduleObj.eventSettings.dataSource = data;
            });
    }

});
