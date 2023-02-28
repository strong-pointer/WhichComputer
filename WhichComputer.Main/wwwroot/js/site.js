// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// initialize queuedQuestions with the first question
let queuedQuestions = [1];
let answers = {}
function answerChosen(questionId, answer) {
    let data = {
        questionID: questionId,
        choice: answer
    };
    $.ajax({
        type: "POST",
        url: "/Home/GetFollowUpToQuestion",
        contentType: "application/x-www-form-urlencoded",
        data: data,
        success: function (result, status, xhr) {
            let followUps = result['followUps'];
            queuedQuestions.shift();
            if (!followUps.includes(-1)) {
                // This is not the last question in the sequence.
                queuedQuestions = queuedQuestions.concat(followUps);
            }
            answers[questionId] = answer;
            if (queuedQuestions.length === 0) {
                console.log(answers);
                postAnswers();
                return;
            } else {
                setQuestionHTML(queuedQuestions[0]);
            }
            console.log(queuedQuestions)
            
        },
        //error: function (xhr, status, error) {
          //  $("#question-card").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
        //}
    });
}

function setQuestionHTML(questionId) {
    let $card = $("#question-card")
    $.ajax({
        type: "POST",
        url: "/Home/GetQuestionHTML",
        contentType: "application/x-www-form-urlencoded",
        data: {questionID: questionId},
        success: function (result, status, xhr) {
            $card.fadeOut(500, () => $card.html(result).fadeIn(500));
        },
        //error: function (xhr, status, error) {
        //  $("#question-card").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
        //}
    });
}

function postAnswers() {
    $.ajax({
        type: "POST",
        url: "/Home/UploadQuestionnaireResponse",
        contentType: "application/json",
        data: JSON.stringify(answers),
        success: function (result, status, xhr) {
            $("#question-card > div").fadeOut(500).html("<h5 class=\"pb-4 mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white text-center\">All done!</h5>").fadeIn(500);
        },
        //error: function (xhr, status, error) {
        //  $("#question-card").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
        //}
    });
}