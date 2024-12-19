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

            reader.UpdateEnemyHP("eyeShooter", 5);

            reader.AddNewEnemy(
                name: "daBoss",
                rectX: 150,
                rectY: 200,
                size: 120,
                type: "Distance",
                hp: 8,
                attackDamage: 3,
                speed: 2,
                xpValue: 25
            );

            reader.SaveChanges(filePath);
            
            using var game = new Projet_Survivor.C_Sharp.World();
            game.Run();
        }
    }
}
