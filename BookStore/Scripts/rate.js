$(document).ready(function () {
    $('.typeahead').mouseenter(GetNames);
    $('#input-23').on('rating.change', Rate);
    // $('.love_author').click(LoveAuthor);
    $("#wish").click(WishBook);
    $('.statistic').mouseenter(BarGraph);
});

function BarGraph() {
    $('.statistic').unbind('mouseenter mouseleave');
    var rates;
    var bookId = $("#thisBookId").val();
    $.ajax({
            type: "GET",
            url: "/Book/BarGraph",
            data: {bookId:bookId},
            success: function (res) {
                debugger;
                //rates = res;
                var ctx = $("#myChart").get(0).getContext("2d");
                // This will get the first returned node in the jQuery collection.
                var data = {
                    labels: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                    datasets: [
                        {
                            label: "My First dataset",
                            //fillColor: "rgba(220,220,220,0.5)",
                            fillColor: "#669ae1",
                            strokeColor: "rgba(220,220,220,0.8)",
                            highlightFill: "#669ac0",
                            highlightStroke: "rgba(220,220,220,1)",
                            data: res
                        }
                    ]
                };
                
                var options = {
                    //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                    scaleBeginAtZero: true,
                    //Boolean - Whether grid lines are shown across the chart
                    scaleShowGridLines: true,
                    //String - Colour of the grid lines
                    scaleGridLineColor: "rgba(0,0,0,.05)",
                    //Number - Width of the grid lines
                    scaleGridLineWidth: 1,
                    //Boolean - Whether to show horizontal lines (except X axis)
                    scaleShowHorizontalLines: true,
                    //Boolean - Whether to show vertical lines (except Y axis)
                    scaleShowVerticalLines: true,
                    //Boolean - If there is a stroke on each bar
                    barShowStroke: true,
                    //Number - Pixel width of the bar stroke
                    barStrokeWidth: 1,
                    //Number - Spacing between each of the X value sets
                    barValueSpacing: 1,
                    //Number - Spacing between data sets within X values
                    barDatasetSpacing: 2,
                    //String - A legend template
                    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"
                };
                var myBarChart = new Chart(ctx).Bar(data, options);
        }
    });
    
}

function WishBook(bookId, userId) {
    $.ajax({
        type: "GET",
        url: "/User/WishBook",
        data: { userId: userId, bookId: bookId },
        success: function (data) {
            $('#wishedUsers').text(data);
        }
    });
}
function LoveAuthor(userId, authorId) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/User/FavoriteAuthor",
        data: { userId: userId, authorId: authorId },
        success: function (data) {
            $('#love-this-author').text(data);
        }
    });
};

function GetNames() {
    $('.typeahead').unbind('mouseenter mouseleave');
    $.ajax({
        type: "POST",
        url: "/Book/GetNames",
        success: function (data) {
            $('.typeahead').typeahead({
                source: data,
                minLength: 2,
                autoSelect: true,
                afterSelect: Selection
            });
        }
    });
}
function Rate(event, value, caption) {
    $.ajax({
        type: "POST",
        url: "/User/RateBook",
        data: { rate: value, bookId: $('#input-23').attr("bookid"), isSuggestion: false }
    });
}

function Selection(item) {
    if (item.type == 1) {
        window.location.replace("/Book/BookDetails?bookId=" + item.id);
    }
    else if (item.type == 2) {
        window.location.replace("/Author/Index?authorId=" + item.id);
    }
}

