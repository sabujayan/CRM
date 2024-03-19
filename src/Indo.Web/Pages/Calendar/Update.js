abp.modals.CalendarUpdate = function () {

    function initializeDateControl() {
        var startTimeObj = new ej.calendars.DateTimePicker();
        startTimeObj.appendTo('#Calendar_StartTime');

        var endTimeObj = new ej.calendars.DateTimePicker();
        endTimeObj.appendTo('#Calendar_EndTime');
    }

    function initModal(modalManager, args) {
        initializeDateControl();
    };

    return {
        initModal: initModal
    };
};