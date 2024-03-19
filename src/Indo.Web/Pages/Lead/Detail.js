$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');

    /*============================================================= Contact */
    loadContactGrid();
    function loadContactGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogContact' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'name', headerText: 'Name', textAlign: 'Left', width: 100,
                },
                {
                    field: 'phone', headerText: 'Phone', textAlign: 'Left', width: 100,
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '600';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.name : '',
                        placeholder: 'Name*',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('name'));

                    var name = args.form.elements.namedItem('name');
                    name.focus();

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.street : '',
                        placeholder: 'Street',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('street'));

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.city : '',
                        placeholder: 'City',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('city'));

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.state : '',
                        placeholder: 'State',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('state'));

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.zipCode : '',
                        placeholder: 'ZipCode',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('zipCode'));

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.phone : '',
                        placeholder: 'Phone',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('phone'));

                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.email : '',
                        placeholder: 'Email',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('email'));
                    

                    args.form.ej2_instances[0].addRules('name', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {

                    var updated = {
                        customerId: args.data.customerId,
                        name: args.data.name,
                        street: args.data.street,
                        city: args.data.city,
                        state: args.data.state,
                        zipCode: args.data.zipCode,
                        phone: args.data.phone,
                        email: args.data.email
                    }

                    indo.contacts.contact
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadContactGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.contacts.contact
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadContactGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        name: args.data.name,
                        street: args.data.street,
                        city: args.data.city,
                        state: args.data.state,
                        zipCode: args.data.zipCode,
                        phone: args.data.phone,
                        email: args.data.email
                    }

                    indo.contacts.contact
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadContactGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }

        });

        indo.contacts.contact.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridContact');
            });
    }
    function reloadContactGrid() {
        indo.contacts.contact.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridContact").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Contact */

    /*============================================================= Project Order */
    loadProjectOrderGrid();
    function loadProjectOrderGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogProject' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Project Order', textAlign: 'Left', width: 150,
                    template: "<a href='/ProjectOrder/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
                if (selectedRow.status === 1) {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], true);
                } else {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], false);
                }
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    if (args.requestType === 'beginEdit') {

                        new ej.inputs.TextBox({
                            value: args.rowData.number,
                            placeholder: 'Number*',
                            floatLabelType: 'Always'
                        }, args.form.elements.namedItem('number'));

                    } else {

                        indo.numberSequences.numberSequence
                            .getNextNumber(2)
                            .then((data) => {
                                new ej.inputs.TextBox({
                                    value: data,
                                    placeholder: 'Number*',
                                    floatLabelType: 'Always'
                                }, args.form.elements.namedItem('number'));
                            });

                    }

                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Order Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.orderDate : new Date()
                    }, args.form.elements.namedItem('orderDate'));


                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    indo.projectOrders.projectOrder.getSalesExecutiveLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.salesExecutiveId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Sales Executive*'
                            }, args.form.elements.namedItem('salesExecutiveId'));
                        });

                    indo.projectOrders.projectOrder.getRatingLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.rating : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Rating*'
                            }, args.form.elements.namedItem('rating'));
                        });


                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.description : '',
                        placeholder: 'Description',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('description'));

                    var description = args.form.elements.namedItem('description');
                    description.focus();

                    args.form.ej2_instances[0].addRules('number', { required: true });
                    args.form.ej2_instances[0].addRules('customerId', { required: true });
                    args.form.ej2_instances[0].addRules('orderDate', { required: true });
                    args.form.ej2_instances[0].addRules('salesExecutiveId', { required: true });
                    args.form.ej2_instances[0].addRules('rating', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {
                    
                    var updated = {
                        customerId: args.data.customerId,
                        number: args.data.number,
                        description: args.data.description,
                        orderDate: args.data.orderDate,
                        salesExecutiveId: args.data.salesExecutiveId,
                        rating: args.data.rating
                    }

                    indo.projectOrders.projectOrder
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadProjectGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.projectOrders.projectOrder
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadProjectGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        number: args.data.number,
                        description: args.data.description,
                        orderDate: args.data.orderDate,
                        salesExecutiveId: args.data.salesExecutiveId,
                        rating: args.data.rating
                    }

                    indo.projectOrders.projectOrder
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadProjectGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });
        indo.projectOrders.projectOrder.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridProjectOrder');
            });
    }
    function reloadProjectGrid() {
        indo.projectOrders.projectOrder.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridProjectOrder").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Project Order */


    /*============================================================= Service Quotation */
    loadServiceQuotationGrid();
    function loadServiceQuotationGrid() {
        var pipeline;
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogServiceQuotation' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Service Quotation', textAlign: 'Left', width: 150,
                    template: "<a href='/ServiceQuotation/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
                if (selectedRow.status === 1) {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], true);
                } else {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], false);
                }
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    if (args.requestType === 'beginEdit') {

                        new ej.inputs.TextBox({
                            value: args.rowData.number,
                            placeholder: 'Number*',
                            floatLabelType: 'Always'
                        }, args.form.elements.namedItem('number'));

                        pipeline = args.rowData.pipeline;

                    } else {

                        indo.numberSequences.numberSequence
                            .getNextNumber(23)
                            .then((data) => {
                                new ej.inputs.TextBox({
                                    value: data,
                                    placeholder: 'Number*',
                                    floatLabelType: 'Always'
                                }, args.form.elements.namedItem('number'));
                            });

                    }

                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Quotation Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.quotationDate : new Date()
                    }, args.form.elements.namedItem('quotationDate'));

                    var validUntilDate = new Date();
                    validUntilDate.setDate(validUntilDate.getDate() + 14);
                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Quotation Valid Until Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.quotationValidUntilDate : validUntilDate
                    }, args.form.elements.namedItem('quotationValidUntilDate'));

                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    indo.serviceQuotations.serviceQuotation.getSalesExecutiveLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.salesExecutiveId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Sales Executive*'
                            }, args.form.elements.namedItem('salesExecutiveId'));
                        });


                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.description : '',
                        placeholder: 'Description',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('description'));

                    var description = args.form.elements.namedItem('description');
                    description.focus();

                    args.form.ej2_instances[0].addRules('number', { required: true });
                    args.form.ej2_instances[0].addRules('customerId', { required: true });
                    args.form.ej2_instances[0].addRules('quotationDate', { required: true });
                    args.form.ej2_instances[0].addRules('quotationValidUntilDate', { required: true });
                    args.form.ej2_instances[0].addRules('salesExecutiveId', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {

                    var updated = {
                        customerId: args.data.customerId,
                        number: args.data.number,
                        description: args.data.description,
                        quotationDate: args.data.quotationDate,
                        quotationValidUntilDate: args.data.quotationValidUntilDate,
                        salesExecutiveId: args.data.salesExecutiveId,
                        pipeline: pipeline
                    }

                    indo.serviceQuotations.serviceQuotation
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadServiceQuotationGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.serviceQuotations.serviceQuotation
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadServiceQuotationGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        number: args.data.number,
                        description: args.data.description,
                        quotationDate: args.data.quotationDate,
                        quotationValidUntilDate: args.data.quotationValidUntilDate,
                        salesExecutiveId: args.data.salesExecutiveId
                    }

                    indo.serviceQuotations.serviceQuotation
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadServiceQuotationGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });
        indo.serviceQuotations.serviceQuotation.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridServiceQuotation');
            });
    }
    function reloadServiceQuotationGrid() {
        indo.serviceQuotations.serviceQuotation.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridServiceQuotation").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Service Quotation */



    /*============================================================= Sales Quotation */
    loadSalesQuotationGrid();
    function loadSalesQuotationGrid() {
        var pipeline;
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogSalesQuotation' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Sales Quotation', textAlign: 'Left', width: 150,
                    template: "<a href='/SalesQuotation/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
                if (selectedRow.status === 1) {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], true);
                } else {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], false);
                }
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    if (args.requestType === 'beginEdit') {

                        new ej.inputs.TextBox({
                            value: args.rowData.number,
                            placeholder: 'Number*',
                            floatLabelType: 'Always'
                        }, args.form.elements.namedItem('number'));

                        pipeline = args.rowData.pipeline;

                    } else {

                        indo.numberSequences.numberSequence
                            .getNextNumber(24)
                            .then((data) => {
                                new ej.inputs.TextBox({
                                    value: data,
                                    placeholder: 'Number*',
                                    floatLabelType: 'Always'
                                }, args.form.elements.namedItem('number'));
                            });

                    }

                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Quotation Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.quotationDate : new Date()
                    }, args.form.elements.namedItem('quotationDate'));

                    var validUntilDate = new Date();
                    validUntilDate.setDate(validUntilDate.getDate() + 14);
                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Quotation Valid Until Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.quotationValidUntilDate : validUntilDate
                    }, args.form.elements.namedItem('quotationValidUntilDate'));

                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    indo.salesQuotations.salesQuotation.getSalesExecutiveLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.salesExecutiveId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Sales Executive*'
                            }, args.form.elements.namedItem('salesExecutiveId'));
                        });


                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.description : '',
                        placeholder: 'Description',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('description'));

                    var description = args.form.elements.namedItem('description');
                    description.focus();

                    args.form.ej2_instances[0].addRules('number', { required: true });
                    args.form.ej2_instances[0].addRules('customerId', { required: true });
                    args.form.ej2_instances[0].addRules('quotationDate', { required: true });
                    args.form.ej2_instances[0].addRules('quotationValidUntilDate', { required: true });
                    args.form.ej2_instances[0].addRules('salesExecutiveId', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {

                    var updated = {
                        customerId: args.data.customerId,
                        number: args.data.number,
                        description: args.data.description,
                        quotationDate: args.data.quotationDate,
                        quotationValidUntilDate: args.data.quotationValidUntilDate,
                        salesExecutiveId: args.data.salesExecutiveId,
                        pipeline: pipeline
                    }

                    indo.salesQuotations.salesQuotation
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadSalesQuotationGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.salesQuotations.salesQuotation
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadSalesQuotationGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        number: args.data.number,
                        description: args.data.description,
                        quotationDate: args.data.quotationDate,
                        quotationValidUntilDate: args.data.quotationValidUntilDate,
                        salesExecutiveId: args.data.salesExecutiveId
                    }

                    indo.salesQuotations.salesQuotation
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadSalesQuotationGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });
        indo.salesQuotations.salesQuotation.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridSalesQuotation');
            });
    }
    function reloadSalesQuotationGrid() {
        indo.salesQuotations.salesQuotation.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridSalesQuotation").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Sales Quotation */



    /*============================================================= Service Order */
    loadServiceOrderGrid();
    function loadServiceOrderGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogService' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Service Order', textAlign: 'Left', width: 150,
                    template: "<a href='/ServiceOrder/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
                if (selectedRow.status === 1) {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], true);
                } else {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], false);
                }
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    if (args.requestType === 'beginEdit') {

                        new ej.inputs.TextBox({
                            value: args.rowData.number,
                            placeholder: 'Number*',
                            floatLabelType: 'Always'
                        }, args.form.elements.namedItem('number'));

                    } else {

                        indo.numberSequences.numberSequence
                            .getNextNumber(1)
                            .then((data) => {
                                new ej.inputs.TextBox({
                                    value: data,
                                    placeholder: 'Number*',
                                    floatLabelType: 'Always'
                                }, args.form.elements.namedItem('number'));
                            });

                    }

                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Order Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.orderDate : new Date()
                    }, args.form.elements.namedItem('orderDate'));

                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    indo.serviceOrders.serviceOrder.getSalesExecutiveLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.salesExecutiveId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Sales Executive*'
                            }, args.form.elements.namedItem('salesExecutiveId'));
                        });


                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.description : '',
                        placeholder: 'Description',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('description'));

                    var description = args.form.elements.namedItem('description');
                    description.focus();

                    args.form.ej2_instances[0].addRules('number', { required: true });
                    args.form.ej2_instances[0].addRules('customerId', { required: true });
                    args.form.ej2_instances[0].addRules('orderDate', { required: true });
                    args.form.ej2_instances[0].addRules('salesExecutiveId', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {
                    
                    var updated = {
                        customerId: args.data.customerId,
                        number: args.data.number,
                        description: args.data.description,
                        orderDate: args.data.orderDate,
                        salesExecutiveId: args.data.salesExecutiveId
                    }

                    indo.serviceOrders.serviceOrder
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadServiceGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.serviceOrders.serviceOrder
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadServiceGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        number: args.data.number,
                        description: args.data.description,
                        orderDate: args.data.orderDate,
                        salesExecutiveId: args.data.salesExecutiveId
                    }

                    indo.serviceOrders.serviceOrder
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadServiceGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });
        indo.serviceOrders.serviceOrder.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridServiceOrder');
            });
    }
    function reloadServiceGrid() {
        indo.serviceOrders.serviceOrder.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridServiceOrder").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Service Order */


    /*============================================================= Sales Order */
    loadSalesOrderGrid();
    function loadSalesOrderGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogSales' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Sales Order', textAlign: 'Left', width: 150,
                    template: "<a href='/SalesOrder/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
                if (selectedRow.status === 1) {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], true);
                } else {
                    grid.toolbarModule.enableItems([grid.element.id + '_delete', grid.element.id + '_edit'], false);
                }
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    if (args.requestType === 'beginEdit') {

                        new ej.inputs.TextBox({
                            value: args.rowData.number,
                            placeholder: 'Number*',
                            floatLabelType: 'Always'
                        }, args.form.elements.namedItem('number'));

                    } else {

                        indo.numberSequences.numberSequence
                            .getNextNumber(5)
                            .then((data) => {
                                new ej.inputs.TextBox({
                                    value: data,
                                    placeholder: 'Number*',
                                    floatLabelType: 'Always'
                                }, args.form.elements.namedItem('number'));
                            });

                    }

                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Order Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.orderDate : new Date()
                    }, args.form.elements.namedItem('orderDate'));

                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    indo.salesOrders.salesOrder.getSalesExecutiveLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.salesExecutiveId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Sales Executive*'
                            }, args.form.elements.namedItem('salesExecutiveId'));
                        });


                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.description : '',
                        placeholder: 'Description',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('description'));

                    var description = args.form.elements.namedItem('description');
                    description.focus();

                    args.form.ej2_instances[0].addRules('number', { required: true });
                    args.form.ej2_instances[0].addRules('customerId', { required: true });
                    args.form.ej2_instances[0].addRules('orderDate', { required: true });
                    args.form.ej2_instances[0].addRules('salesExecutiveId', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {
                    
                    var updated = {
                        customerId: args.data.customerId,
                        number: args.data.number,
                        description: args.data.description,
                        orderDate: args.data.orderDate,
                        salesExecutiveId: args.data.salesExecutiveId
                    }

                    indo.salesOrders.salesOrder
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadSalesGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.salesOrders.salesOrder
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadSalesGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        number: args.data.number,
                        description: args.data.description,
                        orderDate: args.data.orderDate,
                        salesExecutiveId: args.data.salesExecutiveId
                    }

                    indo.salesOrders.salesOrder
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadSalesGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });
        indo.salesOrders.salesOrder.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridSalesOrder');
            });
    }
    function reloadSalesGrid() {
        indo.salesOrders.salesOrder.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridSalesOrder").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Sales Order */


    /*============================================================= Invoice */
    loadInvoiceGrid();
    function loadInvoiceGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Dialog', showDeleteConfirmDialog: true },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Invoice', textAlign: 'Left', width: 150,
                    template: "<a href='/CustomerInvoice/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'sourceDocument', headerText: 'Source Document', textAlign: 'Left', width: 150,
                    template: "<a href='/${sourceDocumentPath}/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
        });
        indo.customerInvoices.customerInvoice.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridCustomerInvoice');
            });
    }
    function reloadSalesGrid() {
        indo.customerInvoices.customerInvoice.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridCustomerInvoice").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Invoice */


    /*============================================================= Credit Note */
    loadCreditNoteGrid();
    function loadCreditNoteGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Dialog', showDeleteConfirmDialog: true },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Credit Note', textAlign: 'Left', width: 150,
                    template: "<a href='/CustomerCreditNote/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'customerInvoiceNumber', headerText: 'Invoice', textAlign: 'Left', width: 150,
                    template: "<a href='/CustomerInvoice/Detail/${customerInvoiceId}' target='_blank'>${customerInvoiceNumber}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
        });
        indo.customerCreditNotes.customerCreditNote.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridCustomerCreditNote');
            });
    }
    function reloadSalesGrid() {
        indo.customerCreditNotes.customerCreditNote.getListWithTotalByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridCustomerCreditNote").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Credit Note */

    /*============================================================= Payment */
    loadPaymentGrid();
    function loadPaymentGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Dialog', showDeleteConfirmDialog: true },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Number', textAlign: 'Left', width: 150,
                },
                {
                    field: 'amount', headerText: 'Amount', textAlign: 'Left', width: 100, format: 'N2'
                },
                {
                    field: 'sourceDocument', headerText: 'Source Document', textAlign: 'Left', width: 150,
                    template: "<a href='/${sourceDocumentPath}/Detail/${sourceDocumentId}' target='_blank'>${sourceDocument}</a>"
                },
                {
                    field: 'statusString', headerText: 'Status', textAlign: 'Left', width: 100,
                    template: "<span class='label label-warning'>${statusString}</span>"
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelected: () => {
                var selectedRow = grid.getSelectedRecords()[0];
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
        });
        indo.customerPayments.customerPayment.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridCustomerPayment');
            });
    }
    function reloadSalesGrid() {
        indo.customerPayments.customerPayment.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridCustomerPayment").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Payment */

    /*============================================================= Expense */
    loadExpenseGrid();
    function loadExpenseGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true, template: '#DialogExpense' },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'number', headerText: 'Expense', textAlign: 'Left', width: 150,
                    template: "<a href='/Expense/Detail/${id}' target='_blank'>${number}</a>"
                },
                {
                    field: 'employeeName', headerText: 'Employee', textAlign: 'Left', width: 150,
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {

                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';

                    if (args.requestType === 'beginEdit') {

                        new ej.inputs.TextBox({
                            value: args.rowData.number,
                            placeholder: 'Number*',
                            floatLabelType: 'Always'
                        }, args.form.elements.namedItem('number'));

                    } else {

                        indo.numberSequences.numberSequence
                            .getNextNumber(14)
                            .then((data) => {
                                new ej.inputs.TextBox({
                                    value: data,
                                    placeholder: 'Number*',
                                    floatLabelType: 'Always'
                                }, args.form.elements.namedItem('number'));
                            });

                    }

                    new ej.calendars.DatePicker({
                        floatLabelType: 'Always',
                        placeholder: 'Expense Date*',
                        value: args.requestType === 'beginEdit' ? args.rowData.expenseDate : new Date()
                    }, args.form.elements.namedItem('expenseDate'));

                    indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                        .then(function (data) {
                            new ej.dropdowns.DropDownList({
                                value: $('#hfCustomerId').val(),
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Lead*'
                            }, args.form.elements.namedItem('customerId'));
                        });

                    indo.expenses.expense.getExpenseTypeLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.expenseTypeId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Expense Type*'
                            }, args.form.elements.namedItem('expenseTypeId'));
                        });

                    indo.expenses.expense.getEmployeeLookup()
                        .then((data) => {
                            new ej.dropdowns.DropDownList({
                                value: args.requestType === 'beginEdit' ? args.rowData.employeeId : data.items[0].id,
                                popupHeight: '200px',
                                floatLabelType: 'Always',
                                dataSource: data.items,
                                fields: { text: 'name', value: 'id' },
                                placeholder: 'Employee*'
                            }, args.form.elements.namedItem('employeeId'));
                        });


                    new ej.inputs.TextBox({
                        value: args.requestType === 'beginEdit' ? args.rowData.description : '',
                        placeholder: 'Description',
                        floatLabelType: 'Always'
                    }, args.form.elements.namedItem('description'));

                    var description = args.form.elements.namedItem('description');
                    description.focus();

                    args.form.ej2_instances[0].addRules('number', { required: true });
                    args.form.ej2_instances[0].addRules('customerId', { required: true });
                    args.form.ej2_instances[0].addRules('expenseDate', { required: true });
                    args.form.ej2_instances[0].addRules('expenseTypeId', { required: true });
                    args.form.ej2_instances[0].addRules('employeeId', { required: true });

                }


                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {

                    var updated = {
                        customerId: args.data.customerId,
                        number: args.data.number,
                        description: args.data.description,
                        expenseDate: args.data.expenseDate,
                        expenseTypeId: args.data.expenseTypeId,
                        employeeId: args.data.employeeId
                    }

                    indo.expenses.expense
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadExpenseGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.expenses.expense
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadExpenseGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        number: args.data.number,
                        description: args.data.description,
                        expenseDate: args.data.expenseDate,
                        expenseTypeId: args.data.expenseTypeId,
                        employeeId: args.data.employeeId
                    }

                    indo.expenses.expense
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadExpenseGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });

        indo.expenses.expense.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridExpense');
            });
    }
    function reloadExpenseGrid() {
        indo.expenses.expense.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridExpense").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Expense */


    /*============================================================= Important Date */
    loadImportantDate();
    function loadImportantDate() {
        var scheduleObj = new ej.schedule.Schedule({
            width: '100%',
            height: 'auto',
            enableAdaptiveUI: false,
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
                    location: { name: 'location' }
                }
            },
            popupOpen: (args) => {
                var guid = args.data.Guid;
                if (args.name === 'popupOpen' && (args.type === 'Editor' || args.type === 'QuickInfo') && ej.base.isNullOrUndefined(guid)) {

                    indo.numberSequences.numberSequence
                        .getNextNumber(21)
                        .then(function (data) {
                            if (args.type === 'QuickInfo') {
                                var input = document.getElementsByClassName('e-subject e-field e-input');
                                input[0].value = data;
                            }
                            if (args.type === 'Editor') {
                                var input = document.getElementsByClassName('e-subject e-field');
                                input[1].value = data;
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
                        customerId: $('#hfCustomerId').val()
                    };

                    indo.importantDates.importantDate
                        .create(newObj)
                        .then(function (dataRef) {
                            reloadCalendarView();
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
                        customerId: $('#hfCustomerId').val()
                    };

                    indo.importantDates.importantDate
                        .update(args.data.id, updObj)
                        .then(function (data) {
                            reloadCalendarView();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            reloadCalendarView();
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.name === 'actionBegin' && args.requestType === 'eventRemove') {
                    indo.importantDates.importantDate
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadCalendarView();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            reloadCalendarView();
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }
            },
        });



        indo.importantDates.importantDate.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                scheduleObj.eventSettings.dataSource = data;
                scheduleObj.appendTo('#Date');
            });


        function reloadCalendarView() {
            indo.importantDates.importantDate.getListByCustomer($('#hfCustomerId').val())
                .then(function (data) {
                    scheduleObj.eventSettings.dataSource = data;
                });
        }
    }
    /*============================================================= Important Date */


    /*============================================================= Task */
    loadTask();
    function loadTask() {
        var shceduleObj = new ej.schedule.Schedule({
            width: '100%',
            height: 'auto',
            enableAdaptiveUI: false,
            views: ['TimelineDay', 'TimelineWeek', 'TimelineMonth', 'MonthAgenda'],
            currentView: 'MonthAgenda',
            allowDragAndDrop: false,
            allowResizing: false,
            readonly: false,
            group: {
                enableCompactView: false,
                byGroupID: false,
                resources: ['Customers', 'Activities']
            },
            resources: [
                {
                    field: 'customerId',
                    title: 'Lead',
                    name: 'Customers',
                    allowMultiple: false,
                    dataSource: [],
                    textField: 'name',
                    idField: 'id'
                },
                {
                    field: 'activityId',
                    title: 'Activities',
                    name: 'Activities',
                    allowMultiple: false,
                    dataSource: [],
                    textField: 'name',
                    idField: 'id'
                }
            ],
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
                        .getNextNumber(22)
                        .then(function (data) {
                            if (args.type === 'QuickInfo') {
                                var input = document.getElementsByClassName('e-subject e-field e-input');
                                input[0].value = data;
                            }
                            if (args.type === 'Editor') {
                                var input = document.getElementsByClassName('e-subject e-field');
                                input[1].value = data;
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
                        isDone: args.data[0].isDone,
                        customerId: args.data[0].customerId,
                        activityId: args.data[0].activityId
                    };

                    indo.tasks.task
                        .create(newObj)
                        .then(function (dataRef) {
                            reloadCalendarTaskView();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            reloadCalendarTaskView();
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
                        isDone: args.data.isDone,
                        customerId: args.data.customerId,
                        activityId: args.data.activityId
                    };

                    indo.tasks.task
                        .update(args.data.id, updObj)
                        .then(function (data) {
                            reloadCalendarTaskView();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            reloadCalendarTaskView();
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.name === 'actionBegin' && args.requestType === 'eventRemove') {
                    indo.tasks.task
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadCalendarTaskView();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            reloadCalendarTaskView();
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }
            },
        });


        indo.tasks.task.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {

                shceduleObj.eventSettings.dataSource = data;
                shceduleObj.appendTo('#Task');

                indo.customers.customer.getListByCustomer($('#hfCustomerId').val())
                    .then(function (data) {
                        shceduleObj.resources[0].dataSource = data;
                    });

                indo.activities.activity.getList()
                    .then(function (data) {
                        shceduleObj.resources[1].dataSource = data;
                    });
            });


        function reloadCalendarTaskView() {
            indo.tasks.task.getListByCustomer($('#hfCustomerId').val())
                .then(function (data) {
                    shceduleObj.eventSettings.dataSource = data;
                });
        }
    }
    /*============================================================= Task */


    /*============================================================= Note */
    loadNoteGrid();
    function loadNoteGrid() {
        var grid = new ej.grids.Grid({
            enableInfiniteScrolling: false,
            allowPaging: true,
            pageSettings: { currentPage: 1, pageSize: 10 },
            allowGrouping: false,
            groupSettings: { columns: ['customerName'] },
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' },
            allowSelection: true,
            selectionSettings: { persistSelection: true, type: 'Single' },
            enableHover: true,
            allowExcelExport: false,
            allowPdfExport: false,
            allowTextWrap: false,
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Dialog', showDeleteConfirmDialog: true  },
            toolbar: ['Add', 'Edit', 'Delete', 'Search'],
            columns: [
                { type: 'checkbox', width: 30 },
                {
                    field: 'id', isPrimaryKey: true, headerText: 'ID', visible: false
                },
                {
                    field: 'description', headerText: 'Description', textAlign: 'Left', width: 150,
                    validationRules: { required: true }
                },
            ],
            dataBound: () => {
                grid.autoFitColumns();
            },
            rowSelecting: () => {
                if (grid.getSelectedRecords().length) {
                    grid.clearSelection();
                }
            },
            toolbarClick: (args) => {
                if (args.item.text === 'Delete') {
                    grid.editModule.alertDObj.target = document.body;
                    grid.editModule.dialogObj.target = document.body;
                }
                if (args.item.text === 'Edit') {
                    grid.editModule.alertDObj.target = document.body;
                }
            },
            actionComplete: (args) => {

                if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {
                    var dialog = args.dialog;
                    dialog.height = '500';
                    dialog.width = '500';
                    dialog.header = args.requestType === 'beginEdit' ? 'Update Record' : 'New Record';
                }
                
                //update
                if (args.action === 'edit' && args.type === 'actionComplete' && args.requestType === 'save') {

                    var updated = {
                        customerId: args.data.customerId,
                        description: args.data.description
                    }

                    indo.notes.note
                        .update(args.data.id, updated)
                        .then(function (data) {
                            reloadNoteGrid();
                            abp.notify.success(l('UpdateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });

                }

                //delete
                if (args.type === 'actionComplete' && args.requestType === 'delete') {

                    indo.notes.note
                        .delete(args.data[0].id)
                        .then(function () {
                            reloadNoteGrid();
                            abp.notify.success(l('DeleteSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });

                }

            },
            actionBegin: (args) => {

                //create
                if (args.action === 'add' && args.type === 'actionBegin' && args.requestType === 'save') {

                    var inserted = {
                        customerId: $('#hfCustomerId').val(),
                        description: args.data.description
                    }

                    indo.notes.note
                        .create(inserted)
                        .then(function (dataRef) {
                            reloadNoteGrid();
                            abp.notify.success(l('CreateSuccess'));
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.code + " " + error.message + " " + error.details);
                        });
                }


            }
        });

        indo.notes.note.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                grid.dataSource = data;
                grid.appendTo('#GridNote');
            });
    }
    function reloadNoteGrid() {
        indo.notes.note.getListByCustomer($('#hfCustomerId').val())
            .then(function (data) {
                var grid = document.getElementById("GridNote").ej2_instances[0];
                grid.dataSource = data;
            });
    }
    /*============================================================= Note */



    /*============================================================= File Manager */
    var fileManagerFirstOpen = true;
    loadFileManager();
    function loadFileManager() {
        var hostUrl = '/';
        var fileObject = new ej.filemanager.FileManager({
            ajaxSettings: {
                url: hostUrl + 'api/FileManagerCustomer/FileOperations',
                getImageUrl: hostUrl + 'api/FileManagerCustomer/GetImage',
                uploadUrl: hostUrl + 'api/FileManagerCustomer/Upload',
                downloadUrl: hostUrl + 'api/FileManagerCustomer/Download?folder=' + $('#hfRootFolder').val()
            },
            beforeSend: (args) => {
                args.ajaxSettings.beforeSend = function (args) {
                    args.httpRequest.setRequestHeader('folder', $('#hfRootFolder').val());
                }
            },
            beforeImageLoad: (args) => {
                args.imageUrl = args.imageUrl + '&folder=' + $('#hfRootFolder').val();
            },
            created: () => {
                var fileManagerHeight = ($(".pcoded-main-container").height()) - 5;
                fileObject.height = fileManagerHeight;
            },
            view: 'Details',
        });
        fileObject.appendTo('#filemanager');
    }
    /*=========================================================== File Manager*/


    /* Tab Click */
    $('#v-pills-tab a').on('click', function (e) {
        e.preventDefault();

        //Important Date
        if (e.target.id === 'v-pills-date-tab') {

            ej.base.L10n.load({
                'en-US': {
                    'schedule': {
                        'newEvent': 'Add Important Date',
                        'editEvent': 'Update Important Date',
                        'deleteEvent': 'Delete Important Date',
                    },
                }
            });

            var scheduledObj = document.getElementById("Date").ej2_instances[0];
            scheduledObj.refresh();
        }

        //Task
        if (e.target.id === 'v-pills-task-tab') {

            ej.base.L10n.load({
                'en-US': {
                    'schedule': {
                        'newEvent': 'Add Task',
                        'editEvent': 'Update Task',
                        'deleteEvent': 'Delete Task',
                    },
                }
            });

            var scheduledObj = document.getElementById("Task").ej2_instances[0];
            scheduledObj.refresh();
        }

        //File Manager
        if (e.target.id === 'v-pills-file-tab' && fileManagerFirstOpen) {
            fileManagerFirstOpen = false;
            setTimeout(() => {
                var fileObject = document.getElementById("filemanager").ej2_instances[0];
                fileObject.refresh();
            }, 500);
        }
    })


});
