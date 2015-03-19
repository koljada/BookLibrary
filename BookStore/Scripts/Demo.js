$(document).ready(function () {
    
    $('#wiki').click(function (event) {
        $.ajax({
            type: "GET",           
            url: "http://www.en.wikipedia.org/w/api.php?action=parse&format=xml&prop=text&section=0&page=Jimi_Hendrix",
            contentType: "application/xml; charset=utf-8",
            async: false,
            dataType: "xml",
            success: function (data, textStatus) {
              
                console.log(data);
            },
            error: function (errorMessage) {
                debugger
            }
        });
    });
});