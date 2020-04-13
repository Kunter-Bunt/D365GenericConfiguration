"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference types="xrm" />
var GenericConfigurationReader = /** @class */ (function () {
    function GenericConfigurationReader() {
    }
    GenericConfigurationReader.GetString = function (key, defaultValue) {
        return new Promise(function (resolve) {
            GenericConfigurationReader.Get(key).then(function (result) {
                if (result !== null)
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
                if (result !== null) {
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