﻿using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;
using System.Web.Script.Serialization;
using System.Xml;

namespace mwo.GenericConfiguration.Plugins.Executables
{
    class GenericConfigurationValidator : ICRMExecutable<mwo_GenericConfiguration>
    {
        public void Execute(CrmServiceContext ctx, ITracingService trace, mwo_GenericConfiguration target, mwo_GenericConfiguration preImage = null)
        {
            var subject = CombineNeededTargetAndPreImageFields(target, preImage);
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
                ThrowValidationException($"Json type configuration must be valid Json. Error: {ex.Message}");
            }
        }

        private void CheckValidXml(mwo_GenericConfiguration subject)
        {
            try
            {
                new XmlDocument().LoadXml(subject.mwo_Value);
            }
            catch (XmlException ex)
            {
                ThrowValidationException($"Xml type configuration must be valid Xml. Error: {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                ThrowValidationException($"Xml type configuration must be valid Xml. Error: {ex.Message}");
            }
        }

        private void CheckValidNumber(mwo_GenericConfiguration subject)
        {
            if (!float.TryParse(subject.mwo_Value, out float _)) 
                ThrowValidationException("Number type configurations must be parseable as float.");
        }

        private void CheckValidList(mwo_GenericConfiguration subject, string delimiter)
        {
            if (subject.mwo_Value.EndsWith(delimiter)) 
                ThrowValidationException($"List type configuration should not end with \"{delimiter}\".");
        }

        private void CheckValidBoolean(mwo_GenericConfiguration subject)
        {
            if (!bool.TryParse(subject.mwo_Value, out bool _)) 
                ThrowValidationException($"Boolean type configuraton values must either be {bool.TrueString} or {bool.FalseString}.");
        }

        private void ThrowValidationException(string msg)
        {
            string prefix = "Validation Error occured:\n";
            string helptext = "\nConsider fixing the Value or, if the Value is correct, change the Type to \"Unspecified\".";
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