using System;
using System.Xml;
using System.IO;

namespace Projet_Survivor.C_Sharp
{
    public class EnemiesReader
    {
        private XmlDocument xmlDoc;
        private string filePath;

        public EnemiesReader(string filePath)
        {
            try
            {
                this.filePath = filePath;
                xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                Console.WriteLine("XML file loaded successfully.\n");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{filePath}' was not found.");
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Error: Failed to parse XML file '{filePath}'. Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        public void DisplayEnemyNames()
        {
            try
            {
                Console.WriteLine("Enemies list:");
                XmlNodeList enemyNames = xmlDoc.SelectNodes("//Enemy/Name");

                if (enemyNames == null || enemyNames.Count == 0)
                {
                    Console.WriteLine("No enemies found in the file.");
                    return;
                }

                foreach (XmlNode node in enemyNames)
                {
                    Console.WriteLine($"- {node.InnerText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while displaying enemy names: {ex.Message}");
            }
        }

        public void UpdateEnemyHP(string enemyName, int newHP)
        {
            try
            {
                Console.WriteLine($"\nEnemy '{enemyName}' HP modification to {newHP}...");

                XmlNode enemyHPNode = xmlDoc.SelectSingleNode($"//Enemy[Name='{enemyName}']/HP");

                if (enemyHPNode != null)
                {
                    enemyHPNode.InnerText = newHP.ToString();
                    Console.WriteLine("Modification done successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: No enemy found with the name '{enemyName}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating enemy HP: {ex.Message}");
            }
        }

        public void AddNewEnemy(string name, int rectX, int rectY, int size, string type, int hp, int attackDamage, int speed, int xpValue)
        {
            try
            {
                Console.WriteLine($"\nAdding new enemy '{name}' to the list of enemies...");

                XmlNode existingEnemy = xmlDoc.SelectSingleNode($"//Enemy[Name='{name}']");
                if (existingEnemy != null)
                {
                    Console.WriteLine($"Error: An enemy with the name '{name}' already exists. Cannot add a duplicate.");
                    return;
                }

                XmlNode enemiesNode = xmlDoc.SelectSingleNode("//Enemies");

                if (enemiesNode == null)
                {
                    Console.WriteLine("Error: No root <Enemies> node found in the file.");
                    return;
                }

                XmlElement newEnemy = xmlDoc.CreateElement("Enemy");

                XmlElement nameElement = xmlDoc.CreateElement("Name");
                nameElement.InnerText = $"{name}";
                newEnemy.AppendChild(nameElement);

                XmlElement rectXElement = xmlDoc.CreateElement("Rectangle_X");
                rectXElement.InnerText = rectX.ToString();
                newEnemy.AppendChild(rectXElement);

                XmlElement rectYElement = xmlDoc.CreateElement("Rectangle_Y");
                rectYElement.InnerText = rectY.ToString();
                newEnemy.AppendChild(rectYElement);

                XmlElement sizeElement = xmlDoc.CreateElement("Size");
                sizeElement.InnerText = size.ToString();
                newEnemy.AppendChild(sizeElement);

                XmlElement typeElement = xmlDoc.CreateElement("Type");
                typeElement.InnerText = type;
                newEnemy.AppendChild(typeElement);

                XmlElement hpElement = xmlDoc.CreateElement("HP");
                hpElement.InnerText = hp.ToString();
                newEnemy.AppendChild(hpElement);

                XmlElement attackElement = xmlDoc.CreateElement("AttackDamage");
                attackElement.InnerText = attackDamage.ToString();
                newEnemy.AppendChild(attackElement);

                XmlElement speedElement = xmlDoc.CreateElement("Speed");
                speedElement.InnerText = speed.ToString();
                newEnemy.AppendChild(speedElement);

                XmlElement xpElement = xmlDoc.CreateElement("XPValue");
                xpElement.InnerText = xpValue.ToString();
                newEnemy.AppendChild(xpElement);

                enemiesNode.AppendChild(newEnemy);
                Console.WriteLine("New enemy added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding a new enemy: {ex.Message}");
            }
        }


        public void SaveChanges(string filePath)
        {
            try
            {
                Console.WriteLine($"\nSaving changes to '{filePath}'...");
                xmlDoc.Save(filePath);
                Console.WriteLine("Save done successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Error: Unauthorized access to save the file '{filePath}'. Check file permissions.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Error: The directory for the file '{filePath}' was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving the file: {ex.Message}");
            }
        }
    }
}
