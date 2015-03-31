var countOfFields = 1; // Текущее число полей/    
var curFieldNameId = 0; // Уникальное значение для атрибута name
var maxFieldLimit = 10; // Максимальное число возможных полей
$(document).ready(function () {
    curFieldNameId = $("#Tags>input").length/2;
});

function deleteField(a) {
    if (countOfFields > 1) {
        // Получаем доступ к ДИВу, содержащему поле
        var contDiv = a.parentNode;
        // Удаляем этот ДИВ из DOM-дерева
        contDiv.parentNode.removeChild(contDiv);
        // Уменьшаем значение текущего числа полей
        countOfFields--;
    }
    // Возвращаем false, чтобы не было перехода по сслыке
    return false;
}
function addField() {
    //curFieldNameId = num;
    // Проверяем, не достигло ли число полей максимума

    if (countOfFields >= maxFieldLimit) {
        alert("Число полей достигло своего максимума = " + maxFieldLimit);
        return false;
    }

    // Создаем элемент ДИВ
    var div = document.createElement("div");
    // Добавляем HTML-контент с пом. свойства innerHTML
    div.innerHTML = "<nobr><input data-val=\"true\" data-val-number=\"The field Tag_ID must be a number.\" data-val-required=\"The Tag_ID field is required.\" name=\"Tages[" + curFieldNameId + "].Tag_ID\" type=\"hidden\" value=\"0\"><input class=\"text-box single-line\"  name=\"Tages[" + curFieldNameId + "].Tag_Name\"  type=\"text\" value=\"\"> </nobr>";
    // Добавляем новый узел в конец списка полей
    document.getElementById("Tags").appendChild(div);
    // Возвращаем false, чтобы не было перехода по сслыке
    // Увеличиваем текущее значение числа полей
    countOfFields++;
    // Увеличиваем ID
    curFieldNameId++;
    return false;
}