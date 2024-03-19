window.customDateToLocaleString = function (date) {
    return luxon
        .DateTime
        .fromISO(date, {
            locale: abp.localization.currentCulture.name
        }).toLocaleString();
}
window.customFormatN2 = function (data) {
    var intl = new ej.base.Internationalization();
    var formattedValue = intl.parseNumber(data.toString(), { format: 'N2', minimumIntegerDigits: 8 });
    return formattedValue;
}

