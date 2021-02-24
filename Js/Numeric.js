function OnlyNumeric(event, ctrl) {

    if (event.which != 46 && event.which != 44 && event.which != 8 && event.which != 0 && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}
function ReplaceNumberWithCommas(yourNumber) {

    if (yourNumber.toString().indexOf('.') > 0) //found comma
    {
        var n = yourNumber.toString().split(".");
        n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return n[0] + '.' + n[1];

    }
    else
    {

        if (yourNumber.toString() == 'NaN') {

            return "";
        }
        else {
            return yourNumber.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '.' + '00';
        }
    }
   
   
}
function Format(ctrl) {
    $(ctrl).val(ReplaceNumberWithCommas($(ctrl).val()));
}
function FormatWithRound(ctrl, Digit) {
    //return $(ctrl).val(ReplaceNumberWithCommas(parseFloat($(ctrl).val())));
    return $(ctrl).val(ReplaceNumberWithCommas(parseFloat($(ctrl).val()).toFixed(Digit)));

}
function FormatWithRound2Digit(Num)
{
    return ReplaceNumberWithCommas(parseFloat(Num).toFixed(2));
}
function FormatNumber(val) {
    
    return ReplaceNumberWithCommas(val);
    //return ReplaceNumberWithCommas(parseFloat(val));
}
function UnFormat(ctrl) {
    $(ctrl).val($(ctrl).val().replace(/,/g, ''));

}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}