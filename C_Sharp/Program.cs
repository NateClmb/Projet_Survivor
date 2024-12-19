using System;
using Projet_Survivor.C_Sharp;
class Program
{
    static void Main(string[] args)
    { 
        XMLValidation.ValidateAllXmlFiles();

        using var game = new World();
        game.Run();
    }
}