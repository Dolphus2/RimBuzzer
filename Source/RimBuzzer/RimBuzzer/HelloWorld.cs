using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Dolphus.RimBuzzer
{
    [StaticConstructorOnStartup]
    public static class HelloWorld
    {
        static HelloWorld() //our constructor
        {
            Log.Message("Hello World!"); //Outputs "Hello World!" to the dev console.
        }
    }
}