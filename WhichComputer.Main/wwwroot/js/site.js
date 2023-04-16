// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// initialize queuedQuestions with the first question
let queuedQuestions = [1];
let previousQuestions = [];
let numberOfFollowUpsAdded = [];
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
            numberOfFollowUpsAdded.splice(0, 0, followUps.length);
            previousQuestions.splice(0, 0, questionId);
            queuedQuestions.shift()
            console.log({previous: previousQuestions, follows: numberOfFollowUpsAdded})
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
        error: function (xhr, status, error) {
            $("#question-card").html(xhr.responseText);
        }
    });
}

function goBack() {
    let $card = $("#question-card");
    let questionID = previousQuestions.shift();
    let followUps = numberOfFollowUpsAdded.shift();
    $.ajax({
        type: "POST",
        url: "/Home/GetQuestionHTML",
        contentType: "application/x-www-form-urlencoded",
        data: {questionID: questionID},
        success: function (result, status, xhr) {
            for (let i = 0; i < followUps; i++) {
                queuedQuestions.pop();
            }
            $card.fadeOut(500, () => $card.html(result).fadeIn(500));
        },
        error: function (xhr, status, error) {
            $("#question-card").html(xhr.responseText);
        }
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
        error: function (xhr, status, error) {
          $("#question-card").html(xhr.responseText);
        }
    });
}

function postAnswers() {
    $.ajax({
        type: "POST",
        url: "/Home/UploadQuestionnaireResponse",
        contentType: "application/json",
        data: JSON.stringify(answers),
        success: function (result, status, xhr) {
            $("#question-card *").contents()
                .filter(function() {
                    return this.nodeType === 3; //Node.TEXT_NODE
                }).remove();
            $(".answer-div").css("display", "none");
            $("#prompt").text("All done!");
            Cookies.set('hash', result.value.hash);
            console.log(result);
            window.location.replace("/Home/ComputerResults?q=" + result.value.hash + "&responseId=" + result.value.responseId);
        },
        error: function (xhr, status, error) {
            $("#question-card").html(xhr.responseText);
        }
    });
}