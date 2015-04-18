$(document).ready(function () {
    $('.typeahead').mouseenter(GetNames);
    $('#input-23').on('rating.change', Rate);
    $('.love_author').click(LoveAuthor);
});

function LoveAuthor(userId,authorId) {
    $.ajax({
        type: "POST",
        url: "/User/FavoriteAuthor",
        data: { userId: userId,authorId:authorId },
        success: function (data) {
            $('#love-this-author').text(data);
        }
    });
}

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
    debugger;
    $.ajax({
        type: "POST",
        url: "/User/RateBook",
        data: { rate: value, bookId: $('#input-23').attr("bookid"),isSuggestion:false }
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

