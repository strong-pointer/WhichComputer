@questionnaire
Feature: Questionnaire
	# Feature testing questionnaire functionality.
	
	Scenario: Ensure answer returns correct follow-up questions
		Given all questions are loaded
		And I answer "Novice" for question 1
		Then I expect that the follow up questions for that answer are "2"