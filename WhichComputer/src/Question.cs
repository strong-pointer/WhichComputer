﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhichComputer.App_Start
{
    public class Question
    {
        public string prompt;
        public int id;
        public string explanation;
        public int follow_up;
        public List<Answer> answers;
    }
}