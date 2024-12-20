using System;
using System.Xml;

namespace Projet_Survivor.C_Sharp
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            XMLValidation.ValidateAllXmlFiles();
            
            
            string filePath = "../../../XML/Enemies.xml";

            EnemiesReader reader = new EnemiesReader(filePath);

            reader.DisplayEnemyNames();

            reader.UpdateEnemyHP("eyeShooter", 2);

            reader.AddNewEnemy(
                name: "eyeSprinter",
                rectX: 70,
                rectY: 45,
                size: 100,
                type: "Corps à corps",
                hp: 3,
                attackDamage: 1,
                speed: 3,
                xpValue: 10
            );

            reader.SaveChanges(filePath);
            
            using var game = new Projet_Survivor.C_Sharp.World();
            game.Run();
        }
    }
}
