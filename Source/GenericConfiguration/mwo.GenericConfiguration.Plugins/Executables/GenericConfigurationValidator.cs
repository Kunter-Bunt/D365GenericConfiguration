using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;
using System.Web.Script.Serialization;
using System.Xml;

namespace mwo.GenericConfiguration.Plugins.Executables
{
    /// <summary>
    /// Class for Validating Generic Configuration Records.
    /// </summary>
    public class GenericConfigurationValidator : ICRMExecutable<mwo_GenericConfiguration>
    {

        /// <summary>
        /// Execute will run Validation based on type of the configuration. 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="trace"></param>
        /// <param name="target"></param>
        /// <param name="preImage"></param>
        public void Execute(CrmServiceContext ctx, ITracingService trace, mwo_GenericConfiguration target, mwo_GenericConfiguration preImage = null)
        {
            if (target == null) throw new InvalidPluginExecutionException(nameof(target) + Errors.NullError);
            var subject = CombineNeededTargetAndPreImageFields(target, preImage);
            trace?.Trace($"{mwo_GenericConfiguration.Fields.mwo_Value}: {subject.mwo_Value}");
            trace?.Trace($"{mwo_GenericConfiguration.Fields.mwo_Type}: {subject.mwo_TypeEnum?.ToString()}");

            switch (subject.mwo_TypeEnum)
            {
                case mwo_GenericConfiguration_mwo_Type.Boolean:
                    CheckValidBoolean(subject);
                    break;
                case mwo_GenericConfiguration_mwo_Type.CommaseparatedList:
                    CheckValidList(subject, ",");
                    break;
                case mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList:
                    CheckValidList(subject, ";");
                    break;
                case mwo_GenericConfiguration_mwo_Type.Number:
                    CheckValidNumber(subject);
                    break;
                case mwo_GenericConfiguration_mwo_Type.JSON:
                    CheckValidJson(subject);
                    break;
                case mwo_GenericConfiguration_mwo_Type.XML:
                    CheckValidXml(subject);
                    break;
            }
        }

        private void CheckValidJson(mwo_GenericConfiguration subject)
        {
            try
            {
                new JavaScriptSerializer().DeserializeObject(subject.mwo_Value);
            }
            catch (ArgumentException ex)
            {
                ThrowValidationException($"Json type configuration must be valid Json. \nError: {ex.Message}");
            }
        }

        private void CheckValidXml(mwo_GenericConfiguration subject)
        {
            try
            {
                XmlDocument doc = new XmlDocument() { XmlResolver = null };
                System.IO.StringReader sreader = new System.IO.StringReader(subject.mwo_Value);
                using (XmlReader reader = XmlReader.Create(sreader, new XmlReaderSettings() { XmlResolver = null }))
                    doc.Load(reader);
            }
            catch (XmlException ex)
            {
                ThrowValidationException($"Xml type configuration must be valid Xml. \nError: {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                ThrowValidationException($"Xml type configuration must be valid Xml. \nError: {ex.Message}");
            }
        }

        private void CheckValidNumber(mwo_GenericConfiguration subject)
        {
            if (!float.TryParse(subject.mwo_Value, out float _)) 
                ThrowValidationException("Number type configurations must be parseable as float.");
        }

        private void CheckValidList(mwo_GenericConfiguration subject, string delimiter)
        {
            if (subject.mwo_Value?.EndsWith(delimiter, StringComparison.Ordinal) == true) 
                ThrowValidationException($"List type configuration should not end with \"{delimiter}\".");
        }

        private void CheckValidBoolean(mwo_GenericConfiguration subject)
        {
            if (!bool.TryParse(subject.mwo_Value, out bool _)) 
                ThrowValidationException($"Boolean type configuraton values must either be {bool.TrueString} or {bool.FalseString}.");
        }

        private static void ThrowValidationException(string msg)
        {
            string prefix = "Validation Error occured:\n";
            string helptext = "\nConsider fixing the Value or, if the Value is correct, change the Type to \"Unspecified\" to skip validation.";
            throw new InvalidPluginExecutionException(prefix + msg + helptext);
        }

        private mwo_GenericConfiguration CombineNeededTargetAndPreImageFields(mwo_GenericConfiguration target, mwo_GenericConfiguration preImage)
        {
            if (preImage == null) return target;
            return new mwo_GenericConfiguration
            {
                mwo_Value = target.Attributes.Contains(mwo_GenericConfiguration.Fields.mwo_Value) ? target.mwo_Value : preImage.mwo_Value,
                mwo_TypeEnum = target.Attributes.Contains(mwo_GenericConfiguration.Fields.mwo_Type) ? target.mwo_TypeEnum : preImage.mwo_TypeEnum,
            };
        }
    }
}