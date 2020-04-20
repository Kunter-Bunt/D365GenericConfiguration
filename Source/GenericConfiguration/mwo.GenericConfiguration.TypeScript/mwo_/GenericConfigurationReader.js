"use strict";
/// <reference types="xrm" />
var GenericConfigurationReader = /** @class */ (function () {
    function GenericConfigurationReader() {
    }
    GenericConfigurationReader.GetString = function (key, defaultValue, storage) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key, storage).then(function (result) {
                if (result !== null && result.mwo_value !== null)
                    resolve(result.mwo_value);
                else
                    resolve(defaultValue);
            });
        });
    };
    GenericConfigurationReader.GetBool = function (key, defaultValue, storage) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key, storage).then(function (result) {
                var ret = defaultValue;
                if (result !== null && result.mwo_value !== null) {
                    var val = result.mwo_value.toLowerCase();
                    if (val === "true")
                        ret = true;
                    else if (val === "false")
                        ret = false;
                }
                resolve(ret);
            });
        });
    };
    GenericConfigurationReader.GetNumber = function (key, defaultValue, storage) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key, storage).then(function (result) {
                var ret = defaultValue;
                if (result !== null && result.mwo_value !== null) {
                    var val = Number(result.mwo_value);
                    if (!Number.isNaN(val))
                        ret = val;
                }
                resolve(ret);
            });
        });
    };
    GenericConfigurationReader.GetList = function (key, defaultValue, storage) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key, storage).then(function (result) {
                var ret = defaultValue;
                if (result !== null && result.mwo_value !== null) {
                    var separator = result.mwo_type === 122870006 /*SemicolonseparatedList*/ ? ';' : ',';
                    ret = result.mwo_value.split(separator);
                }
                resolve(ret);
            });
        });
    };
    GenericConfigurationReader.GetObject = function (key, defaultValue, storage) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key, storage).then(function (result) {
                if (result !== null && result.mwo_value !== null) {
                    try {
                        if (result.mwo_type === 122870001 /*JSON*/ || result.mwo_value.startsWith("{") || result.mwo_value.startsWith("["))
                            resolve(JSON.parse(result.mwo_value));
                        else if (result.mwo_type === 122870002 /*XML*/ || result.mwo_value.StartsWith("<"))
                            resolve(new window.DOMParser().parseFromString(result.mwo_value, "text/xml").documentElement);
                        else
                            resolve(defaultValue);
                    }
                    catch (error) {
                        console.warn("Failed to generate Object: " + error);
                        resolve(defaultValue);
                    }
                }
                else
                    resolve(defaultValue);
            });
        });
    };
    GenericConfigurationReader.Get = function (key, storage) {
        var _this = this;
        return new Promise(function (resolve) {
            var cache = _this.GetFromCache(key, storage);
            if (cache)
                resolve(cache);
            else
                Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "?$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(function (result) {
                    if (result.entities && result.entities.length > 0) {
                        _this.SaveToCache(key, result.entities[0], storage);
                        resolve(result.entities[0]);
                    }
                    else
                        resolve(null);
                });
        });
    };
    GenericConfigurationReader.GetFromCache = function (key, storage) {
        if (storage && storage.getItem("mwo_genericconfiguration_" + key))
            return JSON.parse(storage.getItem("mwo_genericconfiguration_" + key));
        else
            return null;
    };
    GenericConfigurationReader.SaveToCache = function (key, value, storage) {
        if (storage)
            storage.setItem("mwo_genericconfiguration_" + key, JSON.stringify(value));
    };
    return GenericConfigurationReader;
}());
//# sourceMappingURL=GenericConfigurationReader.js.map