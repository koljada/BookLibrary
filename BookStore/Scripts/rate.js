$(document).ready(function () {
    $('#logo').click(function () {
        debugger;
         $('#query').autocomplete({
        serviceUrl: 'Book/GetNames', // Страница для обработки запросов автозаполнения
        minChars: 2, // Минимальная длина запроса для срабатывания автозаполнения
        delimiter: /(,|;)\s*/, // Разделитель для нескольких запросов, символ или регулярное выражение
        maxHeight: 400, // Максимальная высота списка подсказок, в пикселях
        width: 300, // Ширина списка
        zIndex: 9999, // z-index списка
        deferRequestBy: 300, // Задержка запроса (мсек), на случай, если мы не хотим слать миллион запросов, пока пользователь печатает. Я обычно ставлю 300.
        params: { country: 'Yes' }, // Дополнительные параметры
        onSelect: function (data, value) { }, // Callback функция, срабатывающая на выбор одного из предложенных вариантов,
        lookup: ['January', 'February', 'March'] // Список вариантов для локального автозаполнения
    });

    } );

   
    $('#input-23').on('rating.change', Rate);
});
function Rate(event, value, caption) {
    $.ajax({
        type: "POST",
        url: "/User/RateBook",
        data: { rate: value, bookId: $('#input-23').attr("bookid") }
    });
}

function FindInit() {
    $.ajax({
        type: "POST",
        url: "/Book/GetNames",
        dataType: 'json',
        success: Complete
    });
}
function Complete(data) {
    debugger;
    $('#find').typeahead([{ source: data }]);
}
