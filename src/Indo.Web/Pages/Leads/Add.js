$(function () {
    var l = abp.localization.getResource('Indo');

    $(document).ready(function () {
        let addressCount = 0;

        $('.address-information-section:first .btn-add-address').show();

        $('.btn-add-address').on('click', function () {
            const addressContainer = $('.address-information-section').first();
            const newAddressSection = addressContainer.clone();
            addressCount++;
            newAddressSection.find('.address-heading').text(`Address Information ${addressCount}`);
            newAddressSection.find('input').val('');

            newAddressSection.find('.btn-add-address').hide();

            const descriptionSection = $('.description-information-section');
            descriptionSection.before(newAddressSection);
        });
    });
});
