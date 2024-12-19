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
                { "../../../XML/GameInitialization.xml", "../../../XSD/GameInitialization.xsd" },
                { "../../../XML/Player.xml", "../../../XSD/Player.xsd" },
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
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                settings.Schemas.Add(null, xsdFilePath);
                settings.ValidationType = ValidationType.Schema;

                settings.ValidationEventHandler += ValidationCallback;

                using (XmlReader reader = XmlReader.Create(xmlFilePath, settings))
                {
                    while (reader.Read()) { }
                }

                Console.WriteLine("Validation succeed : XML file is valid against XSD schema.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Validation Error : {ex.Message}");
            }
        }

        private static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            string messageType = args.Severity == XmlSeverityType.Warning ? "Warning" : "Error";
            Console.WriteLine($"{messageType}: {args.Message}");
        }
    }
}