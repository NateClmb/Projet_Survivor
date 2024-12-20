using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Projet_Survivor.C_Sharp
{
    public static class XMLValidation
    {
        public static void ValidateAllXmlFiles()
        {
            var xmlFilesWithSchemas = new Dictionary<string, string>
            {
                { "../../../XML/Enemies.xml", "../../../XSD/Enemies.xsd" },
                { "../../../XML/PlayerProfile.xml", "../../../XSD/PlayerProfile.xsd" },
                { "../../../XML/Saves.xml", "../../../XSD/Saves.xsd" }
            };

            foreach (var entry in xmlFilesWithSchemas)
            {
                string xmlFile = entry.Key;
                string xsdFile = entry.Value;

                Console.WriteLine($"\nValidating XML File: {xmlFile} against XSD: {xsdFile}\n");
                ValidateXml(xmlFile, xsdFile);
            }
        }

        private static void ValidateXml(string xmlFilePath, string xsdFilePath)
        {
            bool hasErrors = false; 

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                settings.Schemas.Add(null, xsdFilePath);
                settings.ValidationType = ValidationType.Schema;

                settings.ValidationEventHandler += (sender, args) =>
                {
                    string messageType = args.Severity == XmlSeverityType.Warning ? "Warning" : "Error";
                    Console.WriteLine($"{messageType}: {args.Message}");
                    
                    if (args.Severity == XmlSeverityType.Error)
                    {
                        hasErrors = true;
                    }
                };

                using (XmlReader reader = XmlReader.Create(xmlFilePath, settings))
                {
                    while (reader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                hasErrors = true; 
            }

            if (!hasErrors)
            {
                Console.WriteLine("\nValidation succeed: XML file is valid against XSD schema.\n");
            }
            else
            {
                Console.WriteLine("\nValidation failed: XML file is not valid against XSD schema.\n");
            }
        }
    }
}
