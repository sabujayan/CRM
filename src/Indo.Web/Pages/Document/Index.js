$(function () {
    /* Syncfusion */
    ej.base.enableRipple(window.ripple);
    var hostUrl = '/';
    var fileObject = new ej.filemanager.FileManager({
        ajaxSettings: {
            url: hostUrl + 'api/FileManager/FileOperations',
            getImageUrl: hostUrl + 'api/FileManager/GetImage',
            uploadUrl: hostUrl + 'api/FileManager/Upload',
            downloadUrl: hostUrl + 'api/FileManager/Download'
        },
        created: () => {
            var fileManagerHeight = ($(".pcoded-main-container").height()) - 5;
            fileObject.height = fileManagerHeight;
        },
        view: 'Details',
    });
    fileObject.appendTo('#filemanager');
});
