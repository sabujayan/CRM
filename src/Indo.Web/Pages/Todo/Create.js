abp.modals.TodoCreate = function () {

    function initializeDateControl() {
        var startTimeObj = new ej.calendars.DateTimePicker();
        startTimeObj.appendTo('#Todo_StartTime');

        var endTimeObj = new ej.calendars.DateTimePicker();
        endTimeObj.appendTo('#Todo_EndTime');
    }

    function initModal(modalManager, args) {
        initializeDateControl();
    };

    return {
        initModal: initModal
    };
};