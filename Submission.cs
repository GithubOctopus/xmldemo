using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://githuboctopus.github.io/xmldemo/NationalParks.xml"; // Variable name from template, do not change
        public static string xmlErrorURL = "https://githuboctopus.github.io/xmldemo/NationalParksErrors.xml"; // Variable name from template, do not change
        public static string xsdURL = "https://githuboctopus.github.io/xmldemo/NationalParks.xsd"; // Variable name from template, do not change

        public static string noErrorsMessage = "No errors are found."; // Error message copied from assignment template

        // Function provided by template, do not change
        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
            Json2Xml(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {

            string message = "";
            try {
                // Create a schema set and add the .xsd url to it
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, xsdUrl);
                
                // Create XML settings with validation type and schemas
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.ValidationType = ValidationType.Schema;
                readerSettings.Schemas = schemaSet;

                // Create an event handler for validation issues (lambda-style function)
                readerSettings.ValidationEventHandler += (sender, e) => {
                    // Add any validation messages to the returned message
                    message += $"Type: Validation Error | Content: {e.Message}\n";
                };
                
                // Create an XML reader using the defined settings and .xml url
                XmlReader reader = XmlReader.Create(xmlUrl, readerSettings);

                // Read through the entire document
                while (reader.Read());
            } catch (Exception ex) {
                // Add any XML exception thrown in the previous block to the returned message
                message += $"Type: Xml Exception | Content: {ex.Message}\n";
            }
            
            // If the message is empty, use the predetermined no errors message
            if (string.IsNullOrEmpty(message)) {
                message = noErrorsMessage;
            }
            return message;

        }

        public static string Xml2Json(string xmlUrl)
        {
            try {
                // Create an xml document
                XmlDocument document = new XmlDocument();
                // Download the contents of the url
                string xmlString = DownloadContent(xmlUrl);

                // Set up reader
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(new StringReader(xmlString), readerSettings);

                // Load the contents string into the document
                document.Load(reader);

                // Remove root element attributes
                document.DocumentElement.Attributes.RemoveAll();
                // Return the serialized json string
                return JsonConvert.SerializeXmlNode(document);
                // Note: the returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))

            } catch (Exception ex) {
                return ex.Message;
            }
        }
        
        // Tester method to verify the Xml2Json output can be de-serialized by Newtonsoft.Json
        private static void Json2Xml(string json)
        {
            try {
                XmlDocument document = JsonConvert.DeserializeXmlNode(json);
                Console.WriteLine("Json2Xml Success");
            } catch (Exception ex) {
                 Console.WriteLine("Json2Xml Failed");
            }
        }

        // Helper method from template to download content from URL
        private static string DownloadContent(string url)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string result = client.DownloadString(url);
            return result;
        }
    }

}
