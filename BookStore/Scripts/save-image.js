$(document).ready(function () {
    
    //$('#submit').click(uploadFile);
});

function uploadFile(title,author) {
    var files = document.getElementById('file_upload').files;
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
                data.append("author" + x, author);
                data.append("title"+x, title);
            }
            debugger;
            $.ajax({
                type: "POST",
                url: '/Admin/UploadFiles',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $("#ContentUrl").val(result);
                    //alert(result);
                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
            debugger;
        } else {
            alert("Браузер не поддерживает загрузку файлов HTML5!");
        }
    }
}

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
