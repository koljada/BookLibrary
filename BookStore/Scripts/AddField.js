$(document).ready(function () {
    OwnershipsCount = $("#Tags>input").length;
    AuthorFieldsCount = $("#Authors>input").length;
    GenresFieldsCount = $("#Genres>input").length;
    $(".remove-field").click(RemoveField);
    
    $(".bookCover").click(function () {
        var link = $(this).child().attr(src);
        $("#Image_url").val(link);
    });
});
var OwnershipsCount = 0;
var AuthorFieldsCount = 0;
var GenresFieldsCount = 0;

function AddField(type) {
    //добавляем поля
    if (type == "Tag") {
        var OwnershipContainer = $("<div/>").attr("class", "ownership-container").attr("id", "OwnershipContainer" + OwnershipsCount).appendTo($("#Tags"));
        $("<input/>").attr("class", "text-box single-line").attr("type", "text").attr("id", "Ownerships[" + OwnershipsCount + "]_Name").attr("name", "Tages[" + OwnershipsCount + "].Tag_Name").attr("value", "").appendTo(OwnershipContainer);
        var RemoveButton = $("<a/>").attr("class", "remove-field glyphicon glyphicon-remove").attr("item", OwnershipsCount).attr("href", "#").appendTo(OwnershipContainer);
        RemoveButton.click(function () { RemoveField(type, RemoveButton); });
        OwnershipsCount++;
    }
    else if (type == "Author") {
        var OwnershipContainer = $("<div/>").attr("class", "ownership-container").attr("id", "OwnershipContainer" + AuthorFieldsCount).appendTo($("#Authors"));
        $("<input/>").attr("class", "text-box single-line").attr("type", "text").attr("id", "Author" + AuthorFieldsCount + "First").attr("name", "BookAuthors[" + AuthorFieldsCount + "].First_Name").attr("value", "").appendTo(OwnershipContainer);
        $("<input/>").attr("class", "text-box single-line").attr("type", "text").attr("id", "Author" + AuthorFieldsCount + "Middle").attr("name", "BookAuthors[" + AuthorFieldsCount + "].Middle_Name").attr("value", "").appendTo(OwnershipContainer);
        $("<input/>").attr("class", "text-box single-line").attr("type", "text").attr("id", "Author" + AuthorFieldsCount + "Last").attr("name", "BookAuthors[" + AuthorFieldsCount + "].Last_Name").attr("value", "").appendTo(OwnershipContainer);
        var RemoveButton = $("<a/>").attr("class", "remove-field glyphicon glyphicon-remove").attr("item", AuthorFieldsCount).attr("href", "#").appendTo(OwnershipContainer);
        RemoveButton.click(function () { RemoveField(type, RemoveButton); });
        AuthorFieldsCount = AuthorFieldsCount + 1;
    }
    else if (type == "Genre") {
        
        var OwnershipContainer = $("<div/>").attr("class", "ownership-container").attr("id", "OwnershipContainer" + GenresFieldsCount).appendTo($("#Genres"));
        $("<input/>").attr("class", "text-box single-line").attr("type", "text").attr("id", "Genre" + GenresFieldsCount).attr("name", "Genres[" + GenresFieldsCount + "].Genre_Name").attr("value", "").appendTo(OwnershipContainer);
        var RemoveButton = $("<a/>").attr("class", "remove-field glyphicon glyphicon-remove").attr("item", GenresFieldsCount).attr("href", "#").appendTo(OwnershipContainer);
        RemoveButton.click(function () { RemoveField(type, RemoveButton); });
        GenresFieldsCount++;
    }
    return false
}

function RemoveField(type, RemoveButton) {
    debugger;
    var RecalculateStartNum = parseInt(RemoveButton.attr("item"));
    var count = 0;
    //удаляем этот div
    RemoveButton.parent().remove();
    if (type == "Tag") {
        count = OwnershipsCount;
        OwnershipsCount--;
    }
    else if (type == "Genre") {
        count = GenresFieldsCount;
        GenresFieldsCount--;
    }
    else if (type == "Author") {
        count = AuthorFieldsCount;
        AuthorFieldsCount--;
    }
    for (var i = RecalculateStartNum ; i < count; i++) {
        //функция пересчета аттрибутов name и id
        RecalculateNamesAndIds(i, type);
    }
    // OwnershipsCount--;
    return false;
}

function RecalculateNamesAndIds(number, type) {
    var prevNumber = number - 1;
    //скобки "[" и "]" которые присутствуют в id DOM-объекта в jquery селекторе необходим экранировать двойным обратным слэшем \\
    if (type == "Tag") {
        $("#Ownerships\\[" + number + "\\]_Name").attr("id", "Ownerships[" + prevNumber + "]_Name").attr("name", "Tages[" + prevNumber + "].Tag_Name");
    }
    else if (type == "Genre") {
        $("#Genre" + GenresFieldsCount + "").attr("name", "Genres[" + prevNumber + "].Genre_Name");
    }
    else if (type == "Author") {
        $("#Author" + number + "First").attr("name", "BookAuthors[" + prevNumber + "].First_Name");
        $("#Author" + number + "Middle").attr("name", "BookAuthors[" + prevNumber + "].Middle_Name");
        $("#Author" + number + "Last").attr("name", "BookAuthors[" + prevNumber + "].Last_Name");
    }
    return false;
}
