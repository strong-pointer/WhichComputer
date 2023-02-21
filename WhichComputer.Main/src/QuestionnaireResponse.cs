﻿using System.Collections.Generic;

namespace WhichComputer
{
    public class QuestionnaireResponse
    {
        // For double[] value: [0] == Total Score;  [1] == Total Count
        private Dictionary<string, double[]> _TagToTotalScoreAndCount;
        private string _HashedResponse;

        public QuestionnaireResponse()
        {
            // Initialize the private vars
            _TagToTotalScoreAndCount = new Dictionary<string, double[]>();
            _HashedResponse = string.Empty;
        }

        public void AddTagScore(string tag, double score)
        {
            /* This tag has been used previously &&
                at least 1 score has been added to this given tag */
            if (_TagToTotalScoreAndCount.ContainsKey(tag))
            {
                // Increments the counter for the specified tag
                _TagToTotalScoreAndCount[tag][1] += 1;

                // Add the new score to the total score for the tag
                _TagToTotalScoreAndCount[tag][0] += score;
            }

            /* This tag has not been used yet */
            else
            {
                // Explicitly make the array value an array with size 2
                double[] tempArr = new double[2];
                _TagToTotalScoreAndCount[tag] = tempArr;

                // Initialize the initial counter to 1
                _TagToTotalScoreAndCount[tag][1] = 1;

                /* Due to the score being the first one for the tag
                    we can simply assign the Total Score to score */
                _TagToTotalScoreAndCount[tag][0] = score;
            }
        }

        // Returns a list of each of the tags currently associated with the Dictionary
        public List<string> GetAllTags()
        {
            return new List<string>(_TagToTotalScoreAndCount.Keys);
        }

        // If the given tag is valid, the average score is calculated and returned as a double
        public double GetTagAverage(string tag)
        {
            if (_TagToTotalScoreAndCount.ContainsKey(tag))
            {
                return _TagToTotalScoreAndCount[tag][0] / _TagToTotalScoreAndCount[tag][1];
            }
            else
            {
                Console.WriteLine("Error! GetTagAverage() in QuestionnaireResponse.cs:\ninput tag is invalid, has no score in map");
                return -1;
            }
        }

        public string GetHashedResponse()
        {
            // Reset string in case of this function being called >1 time
            _HashedResponse = string.Empty;

            // Calculate the hashed string by iterating through the dictionary in sorted order by its key
            foreach (var kvp in _TagToTotalScoreAndCount.OrderBy(x => x.Key))
            {
                // The average scores are given some leeway in that they are always rounded up to the nearest integer
                _HashedResponse += kvp.Key + Math.Ceiling(kvp.Value[0] / kvp.Value[1]).ToString();
            }

            // Checks if the hash function worked
            if (string.IsNullOrEmpty(_HashedResponse))
            {
                Console.WriteLine("Error! Hash String is empty upon calling GetHashedResponse in Questionnaire.cs");
            }

            // If there are exact matches (to the next int on average tag scores), then "hashes" will collide as intended
            return _HashedResponse;
        }
    }
}
