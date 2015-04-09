function saveImage(obj, bookId) {
    debugger;
    var link = obj.attr("src");
    //$("#Image_url").val(link);

    $.ajax({
        type: "POST",
        url: '/Admin/CopyImageToHost',
        data: { imageUrl: link, bookId: bookId },
        success: function (res) {
            $("#Image_url").val(res);
            $("#search").empty();
        }
    });
}

function SaveDesc(obj) {
    debugger;
    $("#Annotation").val(obj.text());
    $("#search").empty();
}
