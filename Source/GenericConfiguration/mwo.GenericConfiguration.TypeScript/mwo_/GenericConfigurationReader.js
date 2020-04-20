"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference types="xrm" />
var xml2js_1 = require("xml2js");
var GenericConfigurationReader = /** @class */ (function () {
    function GenericConfigurationReader() {
    }
    GenericConfigurationReader.GetString = function (key, defaultValue) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key).then(function (result) {
                if (result !== null && result.mwo_value !== null)
                    resolve(result.mwo_value);
                else
                    resolve(defaultValue);
            });
        });
    };
    GenericConfigurationReader.GetBool = function (key, defaultValue) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key).then(function (result) {
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
    GenericConfigurationReader.GetNumber = function (key, defaultValue) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key).then(function (result) {
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
    GenericConfigurationReader.GetList = function (key, defaultValue) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key).then(function (result) {
                var ret = defaultValue;
                if (result !== null && result.mwo_value !== null) {
                    var separator = result.mwo_type === 122870006 /*SemicolonseparatedList*/ ? ';' : ',';
                    ret = result.mwo_value.split(separator);
                }
                resolve(ret);
            });
        });
    };
    GenericConfigurationReader.GetObject = function (key, defaultValue) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key).then(function (result) {
                function handleError(error) {
                    console.warn("Failed to generate Object: " + error);
                    resolve(defaultValue);
                }
                if (result !== null && result.mwo_value !== null) {
                    try {
                        if (result.mwo_type === 122870001 /*JSON*/ || result.mwo_value.startsWith("{") || result.mwo_value.startsWith("[")) {
                            var val = JSON.parse(result.mwo_value);
                            resolve(val);
                        }
                        else if (result.mwo_type === 122870002 /*XML*/ || result.mwo_value.StartsWith("<"))
                            new xml2js_1.Parser().parseString(result.mwo_value, function (error, result) {
                                if (error)
                                    handleError(error);
                                else
                                    resolve(result);
                            });
                        else
                            resolve(defaultValue);
                    }
                    catch (error) {
                        handleError(error);
                    }
                }
                else
                    resolve(defaultValue);
            });
        });
    };
    GenericConfigurationReader.Get = function (key) {
        return new Promise(function (resolve) {
            Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(function (result) {
                if (result.entities && result.entities.length > 0)
                    resolve(result.entities[0]);
                else
                    resolve(null);
            });
        });
    };
    return GenericConfigurationReader;
}());
exports.default = GenericConfigurationReader;
//# sourceMappingURL=GenericConfigurationReader.js.map