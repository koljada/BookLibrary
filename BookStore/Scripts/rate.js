$(document).ready(function () {
    $('#input-23').on('rating.change', function (event, value, caption) {
        debugger;
        $.ajax({
            type: "POST",
            url: "/User/RateBook",
            data: { rate: value, bookId: $('#input-23').attr("bookid") }
        });
    });
});