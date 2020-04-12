"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var assert = require("assert");
var sinon = require("sinon");
var xrm_mock_1 = require("xrm-mock");
var GenericConfigurationReader_1 = require("../mwo_/GenericConfigurationReader");
function GCRGetStringTest() {
    var value = "Test";
    xrm_mock_1.XrmMockGenerator.initialise();
    sinon.stub(Xrm.WebApi, "retrieveMultipleRecords").resolves({
        entites: [{ mwo_value: value }]
    });
    GenericConfigurationReader_1.default.GetString("Key", "Value").then(function (result) { return assert.ok(result === value, "Did not receive back value"); });
}
exports.GCRGetStringTest = GCRGetStringTest;
;
//# sourceMappingURL=GenericConfigurationReaderTests.js.map