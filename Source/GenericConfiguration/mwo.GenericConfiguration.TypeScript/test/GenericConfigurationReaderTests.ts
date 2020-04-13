import * as sinon from "sinon";
import { XrmMockGenerator } from "xrm-mock";
import { expect } from "chai";
import GenericConfigurationReader from "../mwo_/GenericConfigurationReader"

const key = "PersistedKey";
const notPersistedKey = "Nothing";

function Setup(value: string, type: number) {
    const stub = sinon.stub(Xrm.WebApi, "retrieveMultipleRecords");
    stub.withArgs(sinon.match.string, sinon.match(key)).resolves({
        entities: [{ mwo_value: value, mwo_type: type }]
    });
    stub.withArgs(sinon.match.string, sinon.match(notPersistedKey)).resolves({
        entities: []
    });
}

beforeEach(() => {
    XrmMockGenerator.initialise();
});

describe('GCR Get String Tests', () => {
    const value = "Test";
    const dflt = "Default";

    beforeEach(() => {
        Setup(value, 122870000);
    });

    it("should retrieve the config value", () => {
        //Act
        const result = GenericConfigurationReader.GetString(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(value);
        });
    });

    it("should retrun default value", () => {
        //Act
        const result = GenericConfigurationReader.GetString(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});

