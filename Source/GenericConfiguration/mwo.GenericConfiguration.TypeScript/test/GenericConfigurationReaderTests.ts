import * as sinon from "sinon";
import { XrmMockGenerator } from "xrm-mock";
import { expect } from "chai";
import GenericConfigurationReader from "../mwo_/GenericConfigurationReader"

const key = "PersistedKey";
const notPersistedKey = "Nothing";

function Setup(value: string, type: number) {
    const stub = sinon.stub(Xrm.WebApi, "retrieveMultipleRecords");

    stub.withArgs(sinon.match.string, sinon.match(key)).resolves({
        entities: [{ mwo_value: value, mwo_type: type }] // eslint-disable-line
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

    it("should retrun default value if not persisted", () => {
        //Act
        const result = GenericConfigurationReader.GetString(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});

describe('GCR Get Bool Tests', () => {
    const value = true;
    const dflt = false;
    const type = 122870003;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetBool(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(value);
        });
    });

    it("should retrun default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetBool(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });

    it("should retrun default value if not castable", () => {
        //Arrange
        Setup("Nope", type);

        //Act
        const result = GenericConfigurationReader.GetBool(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});

