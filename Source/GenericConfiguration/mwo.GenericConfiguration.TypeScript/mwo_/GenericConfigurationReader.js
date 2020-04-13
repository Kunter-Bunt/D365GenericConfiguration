"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
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
    GenericConfigurationReader.Get = function (key) {
        return new Promise(function (resolve) {
            Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(function (result) {
                if (result.entities.length > 0)
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