using System.Collections.Generic;
using System.Xml.Linq;

public static class EnemyLoader
{
    public static List<EnemyData> LoadEnemiesFromXML(string filePath)
    {
        List<EnemyData> enemies = new List<EnemyData>();

        // Charger le fichier XML
        XDocument doc = XDocument.Load(filePath);

        // Parcourir chaque élément <Enemy> dans le XML
        foreach (XElement enemyElement in doc.Descendants("Enemy"))
        {
            EnemyData enemy = new EnemyData
            {
                Name = enemyElement.Element("Name")?.Value ?? "Unknown",
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