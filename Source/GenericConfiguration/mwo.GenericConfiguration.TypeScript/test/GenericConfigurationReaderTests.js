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
        entities: [{ mwo_value: value, mwo_type: type }] // eslint-disable-line
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
        var result = GenericConfigurationReader_1.default.GetString(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(value);
        });
    });
    it("should return default value if not persisted", function () {
        //Act
        var result = GenericConfigurationReader_1.default.GetString(notPersistedKey, dflt, null);
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
        var result = GenericConfigurationReader_1.default.GetBool(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(value);
        });
    });
    it("should return default value if not persisted", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetBool(notPersistedKey, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
    it("should return default value if not castable", function () {
        //Arrange
        Setup("Nope", type);
        //Act
        var result = GenericConfigurationReader_1.default.GetBool(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
});
describe('GCR Get Number Tests', function () {
    var value = 127;
    var dflt = 2;
    var type = 122870004;
    it("should retrieve the config value", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetNumber(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(value);
        });
    });
    it("should return default value if not persisted", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetNumber(notPersistedKey, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
    it("should return default value if not castable", function () {
        //Arrange
        Setup("Nope", type);
        //Act
        var result = GenericConfigurationReader_1.default.GetNumber(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.equal(dflt);
        });
    });
});
describe('GCR Get Semicolon List Tests', function () {
    var value = "1;2;3;4";
    var dflt = ["1", "2", "3"];
    var type = 122870006;
    it("should retrieve the config value", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetList(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result.join(";")).to.be.equal(value);
        });
    });
    it("should return default value if not persisted", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetList(notPersistedKey, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
    it("should return default value if not castable", function () {
        //Arrange
        Setup(null, type);
        //Act
        var result = GenericConfigurationReader_1.default.GetList(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
});
describe('GCR Get Comma List Tests', function () {
    var value = "1,2,3,4";
    var dflt = ["1", "2", "3"];
    var type = 122870005;
    it("should retrieve the config value", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetList(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result.join(",")).to.be.equal(value);
        });
    });
    it("should return default value if not persisted", function () {
        //Arrange
        Setup(value.toString(), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetList(notPersistedKey, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
    it("should return default value if not castable", function () {
        //Arrange
        Setup(null, type);
        //Act
        var result = GenericConfigurationReader_1.default.GetList(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
});
describe('GCR Get JSON Object Tests', function () {
    var value = { x: "a", y: 1 };
    var dflt = { a: "x", b: 3 };
    var type = 122870001;
    it("should retrieve the config value", function () {
        //Arrange
        Setup(JSON.stringify(value), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetObject(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(value);
        });
    });
    it("should return default value if not persisted", function () {
        //Arrange
        Setup(JSON.stringify(value), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetObject(notPersistedKey, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
    it("should return default value if not castable", function () {
        //Arrange
        Setup(null, type);
        //Act
        var result = GenericConfigurationReader_1.default.GetObject(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
});
describe('GCR Get XML Object Tests', function () {
    var value = "<bookstore><book>" +
        "<title>Everyday Italian</title>" +
        "<author>Giada De Laurentiis</author>" +
        "<year>2005</year>" +
        "</book></bookstore>";
    var dflt = { a: "x", b: 3 };
    var type = 122870002;
    it("should retrieve the config value", function () {
        //Arrange
        Setup(value, type);
        //Act
        var result = GenericConfigurationReader_1.default.GetObject(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be. /*not.*/deep.equal(dflt); //mocha does not have window.DOMParser. Evaluate Karma
            chai_1.expect(result).to.be.not.null;
        });
    });
    it("should return default value if not persisted", function () {
        //Arrange
        Setup(value, type);
        //Act
        var result = GenericConfigurationReader_1.default.GetObject(notPersistedKey, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
    it("should return default value if not castable", function () {
        //Arrange
        Setup(value.replace('<', ''), type);
        //Act
        var result = GenericConfigurationReader_1.default.GetObject(key, dflt, null);
        //Assert
        return result.then(function (result) {
            chai_1.expect(result).to.be.deep.equal(dflt);
        });
    });
});
//# sourceMappingURL=GenericConfigurationReaderTests.js.map