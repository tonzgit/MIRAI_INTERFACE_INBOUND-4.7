using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using iBOLT.Framework;

namespace XML_Validate
{
    public class XML_Validate : IBFlowComponent
    {
        List<xml_validate_errsRoot> Roots;
        string XSDErrorStatus;

        public override void Invoke(IBFlowContext Context)
        {
            try
            {
                //byte[] XML_path;
                
                string XSD_path = Context.FlowVars["F.XSDFilename"].ValString;//����¹ValString��ValBLOB
                string XML_path = Context.FlowVars["F.TempFilePath"].ValString;//����¹ValString��ValBLOB
                string OutputPathFileName = Context.FlowVars["C.OutputPathFileName"].ValString;


                //byte[] Blob = validate.ValidateXML(textBox1.Text, XML, out status);

                byte[] Blob = ValidateXML(XSD_path, XML_path);

                //Context.FlowVars["F.Validate_Blob"].ValBLOB = Blob;
                File.WriteAllBytes(OutputPathFileName, Blob);
                Context.FlowVars["C.ErrorStatus"].ValString = XSDErrorStatus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public byte[] ValidateXML(string XSD_path, string XML_path)//����¹ string �� byte[]
        {
            XSDErrorStatus = "S";
            Roots = new List<xml_validate_errsRoot>();
            XmlReaderSettings booksSettings = new XmlReaderSettings();

            booksSettings.Schemas.Add(null, XSD_path);
            booksSettings.ValidationType = ValidationType.Schema;
            booksSettings.ValidationEventHandler += booksSettings_ValidationEventHandler;

            //Stream sXML = new MemoryStream(XML_path);
            XmlReader books = XmlReader.Create(XML_path, booksSettings);

            while (books.Read()) {}
            books.Close();

            xml_validate_errs xml = new xml_validate_errs();
            xml.root = Roots.ToArray();
            byte[] Blob = Serialize_Blob(xml);
            return Blob;

        }
        public byte[] ValidateXML(string XSD_path, string XML_path, out string ErrorStatus)
        {
            byte[] Blob = ValidateXML(XSD_path, XML_path);
            ErrorStatus = XSDErrorStatus;
            return Blob;

        }

        private void booksSettings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Roots.Add(new xml_validate_errsRoot { err_line = e.Exception.LineNumber, err_description = e.Message });
            XSDErrorStatus = "F";

        }

        private byte[] Serialize_Blob(xml_validate_errs input)
        {
            var serializer = new XmlSerializer(input.GetType());
            byte[] data;
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, input);
                data = ms.ToArray();
            }
            return data;
        }
        public override void Definition(IBComponentDefinition Def)
        {
            try
            {
                Def.Name = "XML_Validate";
                Def.Description = "This .NET component was generated by the .NET Component.";
                Def.Icon = "DotNetComponent.gif";
                Def.Group = "";
                Def.Sync = SyncEnum.Both;
                Def.Trans = LogicEnum.No;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override void Config(IBComponentConfig Config)
        {
            try
            {
                // Place your code here
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override void Commit(IBFlowContext Context)
        {
            try
            {
                // Place your code here
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override void Rollback(IBFlowContext Context)
        {
            try
            {
                // Place your code here
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override void Emulate(IBFlowContext Context)
        {
            try
            {
                // Place your code here
                this.Invoke(Context);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}