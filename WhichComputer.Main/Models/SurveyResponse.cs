using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc.Html;
//using MySqlConnector;

namespace WhichComputer.Main
{
    public class SurveyResponse
    {
        public string? Email { get; set; }

        public string? LikedResponse { get; set; }

        public string? DislikedResponse { get; set; }

        public string? Recommend { get; set; }

        public int Statisfaction { get; set; }
    }
}