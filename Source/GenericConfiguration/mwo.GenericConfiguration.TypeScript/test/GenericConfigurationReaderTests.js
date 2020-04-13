"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var sinon = require("sinon");
var xrm_mock_1 = require("xrm-mock");
var chai_1 = require("chai");
var GenericConfigurationReader_1 = require("../mwo_/GenericConfigurationReader");
var key = "PersistedKey";
var notPersistedKey = "Nothing";
function Setup(value, type) {
    var stub = sinon.stub(Xrm.WebApi, "retrieveMultipleRecords");
    stub.withArgs(sinon.match.string, sinon.match(key)).resolves({
        entities: [{ mwo_value: value, mwo_type: type }]
    });
    stub.withArgs(sinon.match.string, sinon.match(notPersistedKey)).resolves({
        entities: []
    });
}
beforeEach(function () {
    xrm_mock_1.XrmMockGenerator.initialise();
});
describe('GCR Get String Tests', function () {
    var value = "Test";
    var dflt = "Default";
    beforeEach(function () {
        Setup(value, 122870000);
    });
    it("should retrieve the config value", function () {
        //Act
        var result = GenericConfigurationReader_1.default.GetString(key, dflt);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(value);
        });
    });
    it("should retrun default value if not persisted", function () {
        //Act
        var result = GenericConfigurationReader_1.default.GetString(notPersistedKey, dflt);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
});
describe('GCR Get Bool Tests', function () {
    var value = true;
    var dflt = false;
    var type = 122870003;
    it("should retrieve the config value", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetBool(key, dflt);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(value);
        });
    });
    it("should retrun default value if not persisted", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetBool(notPersistedKey, dflt);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
    it("should retrun default value if not castable", function () {
        //Arrange
        Setup("Nope", type);
        //Act
        var result = GenericConfigurationReader_1.default.GetBool(key, dflt);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
});
//# sourceMappingURL=GenericConfigurationReaderTests.js.map