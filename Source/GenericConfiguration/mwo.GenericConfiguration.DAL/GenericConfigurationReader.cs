using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.DAL.Models.CRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace mwo.GenericConfiguration.DAL
{
    /// <summary>
    /// Class will help interacting with the CRM Entity mwo_genericconfiguration which holds configuration entries for custom logic.
    /// </summary>
    /// <remarks>
    /// Facilitates Caching to improve performance, configuration changes might be delayed.
    /// </remarks>
    public class GenericConfigurationReader
    {
        /// <summary>
        /// Change this duration if you feel that the default duration is to long/short. 
        /// You could make this configurable as well.
        /// </summary>
        public TimeSpan DefaultCacheDuration { get; } = TimeSpan.FromMinutes(10);

        private readonly ITracingService Trace;
        private IQueryable<mwo_GenericConfiguration> Query { get; set; }
        private readonly CrmServiceContext Context;
        private readonly ICache Cache;
        private readonly Func<mwo_GenericConfiguration, mwo_GenericConfiguration> Selector = _ => new mwo_GenericConfiguration { Id = _.Id, mwo_Key = _.mwo_Key, mwo_Value = _.mwo_Value, mwo_TypeEnum = _.mwo_TypeEnum };

        /// <summary>
        /// COnstructor with IOrganizationService
        /// </summary>
        /// <param name="service"></param>
        /// <param name="trace"></param>
        public GenericConfigurationReader(IOrganizationService service, ITracingService trace) : this(new CrmServiceContext(service), trace)
        {
        }

        /// <summary>
        /// Constructor with spercific Context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="trace"></param>
        public GenericConfigurationReader(CrmServiceContext context, ITracingService trace)
        {
            Context = context;
            Trace = trace;
            Query = Context.mwo_GenericConfigurationSet.Where(_ => _.StateCode == mwo_GenericConfigurationState.Active);
            Cache = new CacheManager(nameof(GenericConfigurationReader), DefaultCacheDuration);
        }

        /// <summary>
        /// Retrieves the given Key and returns its value.
        /// </summary>
        /// <param name="key">Corrensponds to the mwo_Key field for retrieval</param>
        /// <param name="defaultValue">is returned when the Key is unconfigured</param>
        /// <returns>mwo_Value, if Key was not found, returns defaultValue</returns>
        public string GetString(string key, string defaultValue)
        {
            return Get(key)?.mwo_Value ?? defaultValue;
        }

        /// <summary>
        /// Retrieves the given Key and converts it to a List.
        /// </summary>
        /// <remarks>
        /// mwo_Type determines the sepator.
        /// </remarks>
        /// <param name="key">Corrensponds to the mwo_Key field for retrieval</param>
        /// <param name="defaultValue">is returned when the Key is unconfigured</param>
        /// <returns>mwo_Value converted to a List, if Key was not found, returns defaultValue</returns>
        public IEnumerable<string> GetList(string key, IEnumerable<string> defaultValue)
        {
            var config = Get(key);
            if (config == null || string.IsNullOrEmpty(config.mwo_Value)) return defaultValue;
            var separator = config.mwo_TypeEnum == mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList ? ';' : ',';

            return config.mwo_Value.Split(separator).ToList();
        }

        /// <summary>
        /// Retrieves the given Key and converts it to a Boolean.
        /// </summary>
        /// <param name="key">Corrensponds to the mwo_Key field for retrieval</param>
        /// <param name="defaultValue">is returned when the Key is unconfigured</param>
        /// <returns>mwo_Value converted to a Boolean, if Key was not found, returns defaultValue</returns>
        public bool? GetBool(string key, bool? defaultValue)
        {
            var config = Get(key);
            var success = bool.TryParse(config?.mwo_Value, out bool value);
            return success ? value : defaultValue;
        }

        /// <summary>
        /// Retrieves the given Key and converts it to a Double.
        /// </summary>
        /// <param name="key">Corrensponds to the mwo_Key field for retrieval</param>
        /// <param name="defaultValue">is returned when the Key is unconfigured</param>
        /// <returns>mwo_Value converted to a Double, if Key was not found, returns defaultValue</returns>
        public double GetNumber(string key, double defaultValue)
        {
            var config = Get(key);
            var success = double.TryParse(config?.mwo_Value, out double value);
            return success ? value : defaultValue;
        }

        /// <summary>
        /// Retrieves the given Key and converts it to an object of the specified type.
        /// </summary>
        /// <remarks>
        /// mwo_Type determines wether JSON or XML serialization is used. If mwo_Type does not have an appropriate value, the type is guessed from the content.
        /// </remarks>
        /// <typeparam name="TOut">The Type that is expected in the Configuration, e.g. Dictionary<string,string></typeparam>
        /// <param name="key">Corrensponds to the mwo_Key field for retrieval</param>
        /// <param name="defaultValue">is returned when the Key is unconfigured</param>
        /// <returns>mwo_Value converted to the specified object, if Key was not found or deserilization is not possible, returns defaultValue</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "defaultValue will be returned")]
        public TOut GetObject<TOut>(string key, TOut defaultValue)
        {
            var config = Get(key);
            if (config == null || string.IsNullOrEmpty(config.mwo_Value)) return defaultValue;

            try
            {
                if (config.mwo_TypeEnum == mwo_GenericConfiguration_mwo_Type.JSON || config.mwo_Value.StartsWith("{") || config.mwo_Value.StartsWith("["))
                    return new JavaScriptSerializer().Deserialize<TOut>(config.mwo_Value);

                else if (config.mwo_TypeEnum == mwo_GenericConfiguration_mwo_Type.XML || config.mwo_Value.StartsWith("<"))
                    using (var reader = new StringReader(config.mwo_Value))
                        return (TOut)new XmlSerializer(typeof(TOut)).Deserialize(reader);

                Trace.Trace("Unable to guess content, returning default.");
            }
            catch (Exception ex)
            {
                Trace.Trace($"Failed to generate Object: {ex.Message}\n{ex.StackTrace}");
            }

            return defaultValue;
        }

        /// <summary>
        /// Internal retrieval method.
        /// </summary>
        /// <remarks>
        /// This method faicilates caching to improve performance.
        /// Unconfigured calues are not cached.
        /// Not all fields are returned.
        /// </remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        private mwo_GenericConfiguration Get(string key)
        {
            if (Cache.Has(key)) return Cache.Get<mwo_GenericConfiguration>(key);

            Trace.Trace("No Config in Cache, retrieving.");
            var record = Query.Where(_ => _.mwo_Key.Equals(key)).Select(Selector).FirstOrDefault();

            if (record != null) Cache.Set(key, record);
            else Trace.Trace("No Config in CRM.");

            return record;
        }
    }
}
