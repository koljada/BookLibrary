var OwnershipsCount = 0;

$(document).ready(function () {
    OwnershipsCount = $("#Tags>input").length / 2;
    var droplist = $("#Genre_Genre_Name");
    droplist.change(function () {        
        AddField( $("#Genre_Genre_Name option:selected").text() );}         
        );   
    $(".remove-field").click(RemoveField);
});

function AddField(tag) {
    //добавляем поля
    debugger
    var OwnershipContainer = $("<div/>").attr("class", "ownership-container").attr("id", "OwnershipContainer" + OwnershipsCount).appendTo($("#Tags"));
    $("<input/>").attr("type", "hidden").attr("data-val", "true").attr("name", "Tage" + OwnershipsCount + ".Tag_Name").attr("id", "Tages[" + OwnershipsCount + "]_Tages_ID").attr("value", "0").appendTo(OwnershipContainer);
    $("<input/>").attr("class", "text-box single-line").attr("type", "text").attr("id", "Ownerships[" + OwnershipsCount + "]_Name").attr("name", "Tages[" + OwnershipsCount + "].Tag_Name").attr("value", tag).appendTo(OwnershipContainer);
    var RemoveButton = $("<a/>").attr("class", "remove-field glyphicon glyphicon-remove").attr("item", OwnershipsCount).attr("href", "#").appendTo(OwnershipContainer);
    //на нажатие этого элемента добавляем обработчик - функцию удаления
    RemoveButton.click(RemoveField);
    OwnershipsCount++;
    return false
}

function RemoveField() {
    debugger;
    var RecalculateStartNum = parseInt($(this).attr("item"));
    //удаляем этот div
    $(this).parent().remove();
    for (var i = RecalculateStartNum ; i < OwnershipsCount; i++) {
        //функция пересчета аттрибутов name и id
        RecalculateNamesAndIds(i);
    }
    OwnershipsCount--;
    return false
}

function RecalculateNamesAndIds(number) {
    var prevNumber = number - 1;
    //скобки "[" и "]" которые присутствуют в id DOM-объекта в jquery селекторе необходим экранировать двойным обратным слэшем \\
    $("#Ownerships\\[" + number + "\\]_Name").attr("id", "Ownerships[" + prevNumber + "]_Name").attr("name", "Tages[" + prevNumber + "].Tag_Name");
    return false
}
