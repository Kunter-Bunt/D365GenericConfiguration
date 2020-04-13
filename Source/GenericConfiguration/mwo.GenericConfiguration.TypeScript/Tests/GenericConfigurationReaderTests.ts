import assert = require('assert');
import * as sinon from "sinon";
import { XrmMockGenerator } from "xrm-mock";
import GenericConfigurationReader from "../mwo_/GenericConfigurationReader"

export function GCRGetStringTest() {
    const value = "Test";
    XrmMockGenerator.initialise();

    sinon.stub(Xrm.WebApi, "retrieveMultipleRecords").resolves({
        entites: [{ mwo_value: value}]
    });

    GenericConfigurationReader.GetString("Key", "Value").then(result => assert.ok(result === value, "Did not receive back value"))
};
