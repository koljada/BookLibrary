$(document).ready(function () {
    var input = $("#input-id");
    $('#input-id').rating('update', 3);
    $('#input-id').on('rating.change', function (event, value, caption) {
       // debugger;
        $.ajax({
            type: "POST",
            url: "/User/RateBook",
            data: { rate: value, bookId: input.attr("bookID") }
        });
    });
        
   

});