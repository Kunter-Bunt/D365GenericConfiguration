import * as sinon from "sinon";
import { XrmMockGenerator } from "xrm-mock";
import { expect } from "chai";
import { Builder } from "xml2js";
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

    it("should return default value if not persisted", () => {
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

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetBool(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
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

describe('GCR Get Number Tests', () => {
    const value = 127;
    const dflt = 2;
    const type = 122870004;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetNumber(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetNumber(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup("Nope", type);

        //Act
        const result = GenericConfigurationReader.GetNumber(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});


describe('GCR Get Semicolon List Tests', () => {
    const value = "1;2;3;4";
    const dflt = ["1", "2", "3"];
    const type = 122870006;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result.join(";")).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(null, type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});

describe('GCR Get Comma List Tests', () => {
    const value = "1,2,3,4";
    const dflt = ["1", "2", "3"];
    const type = 122870005;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result.join(",")).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(null, type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});

describe('GCR Get JSON Object Tests', () => {
    const value = { x: "a", y: 1 };
    const dflt = { a: "x", b: 3 };
    const type = 122870001;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(JSON.stringify(value), type);

        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(JSON.stringify(value), type);

        //Act
        const result = GenericConfigurationReader.GetObject(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(null, type);

        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});

describe('GCR Get XML Object Tests', () => {
    const value = { x: "a", y: 1 };
    const dflt = { a: "x", b: 3 };
    const type = 122870002;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(new Builder().buildObject(value), type);
        
        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt);

        //Assert
        return result.then((result) => {
            expect(new Builder().buildObject(result)).to.be.equal(new Builder().buildObject(value));
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(new Builder().buildObject(value), type);

        //Act
        const result = GenericConfigurationReader.GetObject(notPersistedKey, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(new Builder().buildObject(value).replace('<', ''), type);

        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});