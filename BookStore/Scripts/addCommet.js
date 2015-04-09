    $().ready(function() {
        $("#addComment").click(addComment);
    });

  function addComment(bookId, userId) {
      debugger;
      $.ajax({
          type: "POST",
          url: "/Book/AddComments",
          data: { bookId: bookId, userId: userId, content: $("#areaCommment").val() }
      });
  }