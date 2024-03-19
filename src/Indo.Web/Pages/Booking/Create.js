abp.modals.BookingCreate = function () {

    function initializeDateControl() {
        var startTimeObj = new ej.calendars.DateTimePicker();
        startTimeObj.appendTo('#Booking_StartTime');

        var endTimeObj = new ej.calendars.DateTimePicker();
        endTimeObj.appendTo('#Booking_EndTime');

        $(".custom-select").select2({
            dropdownParent: $('.modal-body')
        });
    }

    function initModal(modalManager, args) {
        initializeDateControl();
    };

    return {
        initModal: initModal
    };
};