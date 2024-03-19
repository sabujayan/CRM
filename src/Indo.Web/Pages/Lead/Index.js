$(function () {
    /* Localization */
    var l = abp.localization.getResource('Indo');
    var stage1 = l('Enum:CustomerStage:1');
    var stage2 = l('Enum:CustomerStage:2');
    var stage3 = l('Enum:CustomerStage:3');


    /* Syncfusion */
    ej.base.enableRipple(window.ripple);

    /* Popup Modal */
    abp.modals.LeadModal = function () {

        function initModal(modalManager, args) {
            $(".custom-select").select2({
                dropdownParent: $('.modal-body')
            });
        };

        return {
            initModal: initModal
        };
    }

    var helpModal = new abp.ModalManager(abp.appPath + 'Lead/Help');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Lead/Create',
        modalClass: 'LeadModal'
    });
    var updateModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Lead/Update',
        modalClass: 'LeadModal'
    });
    createModal.onResult(function () {
        abp.notify.success(l('CreateSuccess'));
        reloadKanban();
    });
    updateModal.onResult(function () {
        abp.notify.success(l('UpdateSuccess'));
        reloadKanban();
    });

    /* Toolbar */
    loadToolbar();
    function loadToolbar() {
        var toolbarObj = new ej.navigations.Toolbar({
            items: [
                { type: 'Separator' },
                { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom', click: toolbarClick },
                { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom', click: toolbarClick },
                { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom', click: toolbarClick },
                { type: 'Separator' },
                { text: 'Help', tooltipText: 'Help', id: 'HelpCustom', click: toolbarClick },
                { type: 'Separator' },
                { text: 'Details', tooltipText: 'Details', prefixIcon: '', id: 'DetailsCustom', click: toolbarClick },
                { type: 'Separator' },
                { text: 'Convert Lead to Customer', tooltipText: 'Convert Lead to Customer', prefixIcon: '', id: 'ConvertLead', click: toolbarClick },
                { type: 'Separator' },
            ]
        });
        toolbarObj.appendTo('#toolbar');

        function toolbarClick(args) {
            if (args.item.id === 'HelpCustom') {
                helpModal.open();
            }
            if (args.item.id === 'AddCustom') {
                createModal.open();
            }
            if (args.item.id === 'EditCustom') {

                if (selectedCard !== null) {
                    var selectedId = selectedCard.id;
                    selectedCard = null;
                    updateModal.open({ id: selectedId });

                } else {
                    abp.notify.error(l('SelectRecordError'));
                }

            }
            if (args.item.id === 'DeleteCustom') {
                if (selectedCard !== null) {
                    var selectedRow = selectedCard;
                    selectedCard = null;
                    abp.message.confirm('Delete this data: ' + selectedRow.name)
                        .then(function (confirmed) {
                            if (confirmed) {
                                indo.customers.customer
                                    .delete(selectedRow.id)
                                    .then(function () {
                                        abp.notify.success(l('DeleteSuccess'));
                                        reloadKanban();
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
                if (selectedCard !== null) {
                    var selectedId = selectedCard.id;
                    selectedCard = null;
                    window.location.href = '/Lead/Detail/' + selectedId
                } else {
                    abp.notify.error(l('SelectRecordError'));
                }
            }
            if (args.item.id === 'ConvertLead') {
                if (selectedCard !== null) {
                    var selectedRow = selectedCard;
                    selectedCard = null;
                    abp.message.confirm('Convert this Lead into Customer: ' + selectedRow.name)
                        .then(function (confirmed) {
                            if (confirmed) {
                                indo.customers.customer
                                    .convertLeadToCustomer(selectedRow.id)
                                    .then(function () {
                                        abp.notify.success('Convert Lead to Customer Success.');
                                        reloadKanban();
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
        }
    }

    /* Kanban */
    var selectedCard = null;
    loadKanban();
    function loadKanban() {
        var kanbanObj = new ej.kanban.Kanban({
            dataSource: [],
            keyField: 'stageString',
            columns: [
                { headerText: stage1, keyField: stage1 },
                { headerText: stage2, keyField: stage2 },
                { headerText: stage3, keyField: stage3 }
            ],
            cardSettings: {
                headerField: 'name',
                template: '#cardTemplate',
                selectionType: 'single'
            },
            dialogOpen: (args) => {
                args.cancel = true;
                selectedCard = null;
                if (args.name === 'dialogOpen' && args.requestType === 'Edit') {
                    var selectedId = args.data.id;
                    updateModal.open({ id: selectedId });
                }
            },
            cardClick: (args) => {
                if (args.name === 'cardClick') {
                    var kanban = document.getElementById("Kanban").ej2_instances[0];
                    selectedCard = kanban.activeCardData.data;
                }
            },
            created: () => {
                var kanbanHeight = ($(".pcoded-main-container").height()) - ($(".e-toolbar").outerHeight()) - 10;
                var kanban = document.getElementById("Kanban").ej2_instances[0];
                kanban.height = kanbanHeight;
            },
            dragStop: (args) => {
                var column = args.data[0].stageString;
                if (column === l('Enum:CustomerStage:1')) {
                    var updated = args.data[0];
                    updated.stage = 1;
                    indo.customers.customer
                        .update(args.data[0].id, updated)
                        .then(function () {
                            abp.notify.success('Update Stage Success.');
                            reloadKanban();
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });
                }
                if (column === l('Enum:CustomerStage:2')) {
                    var updated = args.data[0];
                    updated.stage = 2;
                    indo.customers.customer
                        .update(args.data[0].id, updated)
                        .then(function () {
                            abp.notify.success('Update Stage Success.');
                            reloadKanban();
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });
                }
                if (column === l('Enum:CustomerStage:3')) {
                    var updated = args.data[0];
                    updated.stage = 3;
                    indo.customers.customer
                        .update(args.data[0].id, updated)
                        .then(function () {
                            abp.notify.success('Update Stage Success.');
                            reloadKanban();
                        })
                        .catch(function (error) {
                            abp.notify.error("Error: " + error.message + " " + error.details);
                        });
                }
            },
        });

        indo.customers.customer.getListLead()
            .then(function (data) {
                kanbanObj.dataSource = data;
                kanbanObj.appendTo('#Kanban');
            });
    }

    function reloadKanban() {
        indo.customers.customer.getListLead()
            .then(function (data) {
                var kanban = document.getElementById("Kanban").ej2_instances[0];
                kanban.dataSource = data;
            });
    }

});
