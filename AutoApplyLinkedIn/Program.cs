using AutoApplyLinkedIn;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var keywords = new List<string>
        {
            "React",
            "React Native",
            "Internship",
            "Backend",
            "API",
            "AI",
            "Full Stack",
            "Frontend",
            "Mvc",
            "Wpf",
            "Typescript",
            "Js",
            ".Net",
            "Rest API",
        };

        var bot = new LinkedInJobApplyBot(keywords);
        bot.Start();
    }
}
