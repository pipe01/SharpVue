using System;
using Yaclip;

namespace SharpVue.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = YaclipApp.New()
                .Name("SharpVue Generator")
                .Command<GenerateCommand>("gen", o => o
                    .Callback(c => c.Execute()));
        }
    }
}
