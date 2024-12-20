using System.Collections.Generic;
using System.Xml.Linq;

namespace Projet_Survivor.C_Sharp;

public static class EnemyLoader
{
    public static List<EnemyData> LoadEnemiesFromXML(string filePath)
    {
        List<EnemyData> enemies = new List<EnemyData>();

        // Load the xml file
        XDocument doc = XDocument.Load(filePath);

        // Match the correct values with the correct attributes 
        foreach (XElement enemyElement in doc.Descendants("Enemy"))
        {
            EnemyData enemy = new EnemyData
            {
                Name = enemyElement.Element("Name")?.Value ?? "Unknown",
                Rectangle_X = int.Parse(enemyElement.Element("Rectangle_X")?.Value ?? "0"),
                Rectangle_Y = int.Parse(enemyElement.Element("Rectangle_Y")?.Value ?? "0"),
                Size = int.Parse(enemyElement.Element("Size")?.Value ?? "0"),
                Type = enemyElement.Element("Type")?.Value ?? "Unknown",
                HP = int.Parse(enemyElement.Element("HP")?.Value ?? "0"),
                AttackDamage = int.Parse(enemyElement.Element("AttackDamage")?.Value ?? "0"),
                Speed = float.Parse(enemyElement.Element("Speed")?.Value ?? "0.0"),
                XPValue = int.Parse(enemyElement.Element("XPValue")?.Value ?? "0")
            };

            enemies.Add(enemy);
        }

        return enemies;
    }
}