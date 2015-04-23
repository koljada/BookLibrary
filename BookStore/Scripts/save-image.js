$(document).ready(function () {
    //$('#submit').click(uploadFile);
    $("#AvatarFile").change(SaveAvatar);
});

function SaveAvatar(event) {
    var name = $("#Email").val();
    var img = event.target.files;
    var data = new FormData();
    data.append("file", img[0]);
    data.append("user", name);
    $.ajax({
        type: "POST",
        url: '/User/SaveAvatar',
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $("#Avatar").val(result);
        },
        error: function (xhr, status, p3) {
            alert(xhr.responseText);
        }
    });
}
function uploadFile(title, author) {
    debugger;
    var files = document.getElementById('file_upload').files;
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
                data.append("author" + x, author);
                data.append("title"+x, title);
            }
            $.ajax({
                type: "POST",
                url: '/Admin/UploadFiles',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $("#BookDetail_ContentUrl").val(result);
                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
        } else {
            alert("Браузер не поддерживает загрузку файлов HTML5!");
        }
    }
}

function saveImage(obj, Id, types) {
    var link = obj.attr("src");
    var im = $("#AuthorDetail_Image_url");
    $.ajax({
        type: "POST",
        url: '/Admin/CopyImageToHost',
        data: { imageUrl: link, Id: Id, typesearch: types },
        success: function (res) {
            debugger;
            im.val(res);
            $("#search").empty();
        }
    });
}

function SaveDesc(obj) {
    $("#AreaText").val(obj.text());
    $("#search").empty();
}
