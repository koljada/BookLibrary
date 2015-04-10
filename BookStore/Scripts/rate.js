$(document).ready(function () {
    $('#find').click(FindInit);

    $('#input-23').on('rating.change', Rate);
});
function Rate(event, value,caption) {
    $.ajax({
        type: "POST",
        url: "/User/RateBook",
        data: { rate: value, bookId: $('#input-23').attr("bookid") }
    });
}

function FindInit() {
    $.ajax({
        type: "POST",
        url: "/Book/GetNames",
        dataType: 'json',
        success: Complete
    });
}
function Complete(data) {
    debugger;
    $('#find').typeahead([{source: data}]);
}