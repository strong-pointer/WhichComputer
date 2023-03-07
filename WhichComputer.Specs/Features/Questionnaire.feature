@questionnaire
Feature: Questionnaire
	# Feature testing questionnaire functionality.
	
	Scenario: Ensure answer returns correct follow-up questions
		Given all questions are loaded
		And I answer "Novice" for question 1
		Then I expect that the follow up questions for that answer are "2"

	Scenario: Ensure response object is properly filled in
		Given a QuestionnaireResponse object is initialized and not null
		And I add a tag "laptop" with a score of 10.0
		Then I expect that when retrieving all tags, a list with each of the currently entered tags "laptop" are returned
		And I expect that when calling for the hashed string representation, "laptop10" is returned