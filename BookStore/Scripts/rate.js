$(document).ready(function () {
    $('.typeahead').mouseenter(GetNames);
    $('#input-23').on('rating.change', Rate);
});

function GetNames() {
    $('.typeahead').unbind('mouseenter mouseleave');
    $.ajax({
        type: "POST",
        url: "/Book/GetNames",
        success: function (data) {
            debugger;
            $('.typeahead').typeahead({
                source: data,
                minLength:2,
                autoSelect: true,
                afterSelect: Selection
            });
        }
    });
}
function Rate(event, value, caption) {
    $.ajax({
        type: "POST",
        url: "/User/RateBook",
        data: { rate: value, bookId: $('#input-23').attr("bookid") }
    });
}

function Selection(item) {
    if (item.type==1) {
        window.location.replace("/Book/BookDetails?bookId="+item.id);
    }
    else if (item.type == 2) {
        window.location.replace("/Author/Index?authorId=" + item.id);
    }
}

