"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var GenericConfigurationReader = /** @class */ (function () {
    function GenericConfigurationReader() {
    }
    GenericConfigurationReader.GetString = function (key, defaultValue) {
        return new Promise(function (resolve) {
            Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(function (result) {
                if (result.entities.length > 0)
                    resolve(result.entities[0].mwo_value);
                else
                    resolve(defaultValue);
            });
        });
    };
    return GenericConfigurationReader;
}());
exports.default = GenericConfigurationReader;
//# sourceMappingURL=GenericConfigurationReader.js.map