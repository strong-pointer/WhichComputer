// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function answerChosen(questionId, answer) {
    let data = {
        questionID: questionId,
        choice: answer
    };
    console.log(data);
    $.ajax({
        type: "POST",
        url: "/Home/AnswerBtn_Click",
        contentType: "application/x-www-form-urlencoded",
        data: data,
        success: function (result, status, xhr) {
            $("#question-card").html(result);
        },
        //error: function (xhr, status, error) {
          //  $("#question-card").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
        //}
    });
}